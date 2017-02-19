using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Text.RegularExpressions;

namespace SerialportSample
{
    public partial class SerialportSampleForm : Form
    {
        private SerialPort comm = new SerialPort();
        private StringBuilder builder = new StringBuilder();//避免在事件处理方法中反复的创建，定义到外面。
        private long received_count = 0;//接收计数
        private long send_count = 0;//发送计数
        private bool Listening = false;//是否没有执行完invoke相关操作
        private bool Closing = false;//是否正在关闭串口，执行Application.DoEvents，并阻止再次invoke
        private List<byte> buffer = new List<byte>(4096);//默认分配1页内存，并始终限制不允许超过
        private byte[] binary_data_1 = new byte[9];//AA 44 05 01 02 03 04 05 EA

        public SerialportSampleForm()
        {
            InitializeComponent();
        }

        //窗体初始化
        private void Form1_Load(object sender, EventArgs e)
        {
            //初始化下拉串口名称列表框
            string[] ports = SerialPort.GetPortNames();
            Array.Sort(ports);
            comboPortName.Items.AddRange(ports);
            comboPortName.SelectedIndex = comboPortName.Items.Count > 0 ? 0 : -1;
            comboBaudrate.SelectedIndex = comboBaudrate.Items.IndexOf("19200");

            //初始化SerialPort对象
            comm.NewLine = "\r\n";
            comm.RtsEnable = true;//根据实际情况吧。

            //添加事件注册
            comm.DataReceived += comm_DataReceived;
        }

        void comm_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (Closing) return;//如果正在关闭，忽略操作，直接返回，尽快的完成串口监听线程的一次循环
            try
            {
                Listening = true;//设置标记，说明我已经开始处理数据，一会儿要使用系统UI的。
                int n = comm.BytesToRead;//先记录下来，避免某种原因，人为的原因，操作几次之间时间长，缓存不一致
                byte[] buf = new byte[n];//声明一个临时数组存储当前来的串口数据
                received_count += n;//增加接收计数
                comm.Read(buf, 0, n);//读取缓冲数据

                /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //<协议解析>
                bool data_1_catched = false;//缓存记录数据是否捕获到
                //1.缓存数据
                buffer.AddRange(buf);
                //2.完整性判断
                while (buffer.Count >= 4)//至少要包含头（2字节）+长度（1字节）+校验（1字节）
                {
                    //请不要担心使用>=，因为>=已经和>,<,=一样，是独立操作符，并不是解析成>和=2个符号
                    //2.1 查找数据头
                    if (buffer[0] == 0xAA && buffer[1] == 0x44)
                    {
                        //2.2 探测缓存数据是否有一条数据的字节，如果不够，就不用费劲的做其他验证了
                        //前面已经限定了剩余长度>=4，那我们这里一定能访问到buffer[2]这个长度
                        int len = buffer[2];//数据长度
                        //数据完整判断第一步，长度是否足够
                        //len是数据段长度,4个字节是while行注释的3部分长度
                        if (buffer.Count < len + 4) break;//数据不够的时候什么都不做
                        //这里确保数据长度足够，数据头标志找到，我们开始计算校验
                        //2.3 校验数据，确认数据正确
                        //异或校验，逐个字节异或得到校验码
                        byte checksum = 0;
                        for (int i = 0; i < len + 3; i++)//len+3表示校验之前的位置
                        {
                            checksum ^= buffer[i];
                        }
                        if (checksum != buffer[len + 3]) //如果数据校验失败，丢弃这一包数据
                        {
                            buffer.RemoveRange(0, len + 4);//从缓存中删除错误数据
                            continue;//继续下一次循环
                        }
                        //至此，已经被找到了一条完整数据。我们将数据直接分析，或是缓存起来一起分析
                        //我们这里采用的办法是缓存一次，好处就是如果你某种原因，数据堆积在缓存buffer中
                        //已经很多了，那你需要循环的找到最后一组，只分析最新数据，过往数据你已经处理不及时
                        //了，就不要浪费更多时间了，这也是考虑到系统负载能够降低。
                        buffer.CopyTo(0, binary_data_1, 0, len + 4);//复制一条完整数据到具体的数据缓存
                        data_1_catched = true;
                        buffer.RemoveRange(0, len + 4);//正确分析一条数据，从缓存中移除数据。
                    }
                    else
                    {
                        //这里是很重要的，如果数据开始不是头，则删除数据
                        buffer.RemoveAt(0);
                    }
                }
                //分析数据
                if (data_1_catched)
                {
                    //我们的数据都是定好格式的，所以当我们找到分析出的数据1，就知道固定位置一定是这些数据，我们只要显示就可以了
                    string data = binary_data_1[3].ToString("X2") + " " + binary_data_1[4].ToString("X2") + " " +
                        binary_data_1[5].ToString("X2") + " " + binary_data_1[6].ToString("X2") + " " +
                        binary_data_1[7].ToString("X2");
                    //更新界面
                    this.Invoke((EventHandler)(delegate { txData.Text = data; }));
                }
                //如果需要别的协议，只要扩展这个data_n_catched就可以了。往往我们协议多的情况下，还会包含数据编号，给来的数据进行
                //编号，协议优化后就是： 头+编号+长度+数据+校验
                //</协议解析>
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////

                builder.Clear();//清除字符串构造器的内容
                //因为要访问ui资源，所以需要使用invoke方式同步ui。
                this.Invoke((EventHandler)(delegate
                {
                    //判断是否是显示为16禁止
                    if (checkBoxHexView.Checked)
                    {
                        //依次的拼接出16进制字符串
                        foreach (byte b in buf)
                        {
                            builder.Append(b.ToString("X2") + " ");
                        }
                    }
                    else
                    {
                        //直接按ASCII规则转换成字符串
                        builder.Append(Encoding.ASCII.GetString(buf));
                    }
                    //追加的形式添加到文本框末端，并滚动到最后。
                    this.txGet.AppendText(builder.ToString());
                    //修改接收计数
                    labelGetCount.Text = "Get:" + received_count.ToString();
                }));
            }
            finally
            {
                Listening = false;//我用完了，ui可以关闭串口了。
            }
        }

        private void buttonOpenClose_Click(object sender, EventArgs e)
        {
            //根据当前串口对象，来判断操作
            if (comm.IsOpen)
            {
                Closing = true;
                while (Listening) Application.DoEvents();
                //打开时点击，则关闭串口
                comm.Close();
            }
            else
            {
                //关闭时点击，则设置好端口，波特率后打开
                comm.PortName = comboPortName.Text;
                comm.BaudRate = int.Parse(comboBaudrate.Text);
                try
                {
                    comm.Open();
                }
                catch(Exception ex)
                {
                    //捕获到异常信息，创建一个新的comm对象，之前的不能用了。
                    comm = new SerialPort();
                    //现实异常信息给客户。
                    MessageBox.Show(ex.Message);
                }
            }
            //设置按钮的状态
            buttonOpenClose.Text = comm.IsOpen ? "Close" : "Open";
            buttonSend.Enabled = comm.IsOpen;
        }

        //动态的修改获取文本框是否支持自动换行。
        private void checkBoxNewlineGet_CheckedChanged(object sender, EventArgs e)
        {
            txGet.WordWrap = checkBoxNewlineGet.Checked;
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            //定义一个变量，记录发送了几个字节
            int n = 0;
            //16进制发送
            if (checkBoxHexSend.Checked)
            {
                //我们不管规则了。如果写错了一些，我们允许的，只用正则得到有效的十六进制数
                MatchCollection mc = Regex.Matches(txSend.Text, @"(?i)[\da-f]{2}");
                List<byte> buf = new List<byte>();//填充到这个临时列表中
                //依次添加到列表中
                foreach (Match m in mc)
                {
                    buf.Add(byte.Parse(m.Value));
                }
                //转换列表为数组后发送
                comm.Write(buf.ToArray(), 0, buf.Count);
                //记录发送的字节数
                n = buf.Count;
            }
            else//ascii编码直接发送
            {
                //包含换行符
                if (checkBoxNewlineSend.Checked)
                {
                    comm.WriteLine(txSend.Text);
                    n = txSend.Text.Length + 2;
                }
                else//不包含换行符
                {
                    comm.Write(txSend.Text);
                    n = txSend.Text.Length;
                }
            }
            send_count += n;//累加发送字节数
            labelSendCount.Text = "Send:" + send_count.ToString();//更新界面
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            //复位接受和发送的字节数计数器并更新界面。
            send_count = received_count = 0;
            labelGetCount.Text = "Get:0";
            labelSendCount.Text = "Send:0";
        }
    }
}