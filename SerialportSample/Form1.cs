using System;
using System.Collections;
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
        /*在master_child1中能看到*/
        /*子1进1*/
        /*子1进2*/
        /*子1进3*/
        /*子1进4*/
        /*子1进5*/

        /*子1进7*/

        private BitInByte testbitinbyte = new BitInByte("Bit",65536);
        private ModbusRTU ModbusMaster = new ModbusRTU();
        private SerialPort comm = new SerialPort();
        private StringBuilder builder = new StringBuilder();//避免在事件处理方法中反复的创建，定义到外面。
        private long received_count = 0;//接收计数
        private long send_count = 0;//发送计数
        private bool Listening = false;//是否没有执行完invoke相关操作
        private bool Closing = false;//是否正在关闭串口，执行Application.DoEvents，并阻止再次invoke
        private List<byte> buffer = new List<byte>(4096);//默认分配1页内存，并始终限制不允许超过
        private byte[] binary_data_1 = new byte[9];//AA 44 05 01 02 03 04 05 EA

        private byte[] testbytedata = { 8, 9, 10 };
        private UInt16[] testworddata = { 0x11,0x22,0x33,0x44};


        private ArrayList testarrayList = new ArrayList();

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
            comboBaudrate.SelectedIndex = comboBaudrate.Items.IndexOf("9600");

            //初始化SerialPort对象
            comm.NewLine = "\r\n";
            comm.RtsEnable = true;//根据实际情况吧。

            //添加事件注册
            //comm.DataReceived += comm_DataReceived;
            //comm.DataReceived += Modbus_DataReceived;

            ModbusRTU.IsMaster = true;
            comm.DataReceived += ModbusMaster.ModbusReceiveData_SerialPort;
            ModbusRTU.ModbusSendFrame += comm.Write;//为Modbus实例中的发送委托关联正确的发送函数
            ModbusRTU.SetDataStorage();
            //ModbusMaster.RxDataTimer = Timer_RxDone;

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
                
                ModbusRTU.AssembleRequestADU(false, 1, (byte)ModbusRTU.ModbusFuncCode.ReadStorageRegs, ModbusRTU.LoadUnmannedBuses(ModbusRTU.MasterDataRepos.RStorageRegFlag, 'r'), null);

                //关闭时点击，则设置好端口，波特率后打开
                comm.PortName = comboPortName.Text;
                comm.BaudRate = int.Parse(comboBaudrate.Text);
                try
                {
                    comm.Open();
                }
                catch (Exception ex)
                {
                    //捕获到异常信息，创建一个新的comm对象，之前的不能用了。
                    comm = new SerialPort();
                    //现实异常信息给客户。
                    MessageBox.Show(ex.Message);
                }
            }
            //设置按钮的状态
            buttonOpenClose.Text = comm.IsOpen ? "Close" : "Open";
            //buttonSend.Enabled = comm.IsOpen;
        }



        private void buttonReset_Click(object sender, EventArgs e)
        {
            //复位接受和发送的字节数计数器并更新界面。
            send_count = received_count = 0;
            
           
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            //modbusView1.ReadAddress = Convert.ToUInt16(textBox5.Text);
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            //modbusView1.ReadDataLengthInWord = Convert.ToByte(textBox6.Text);
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //textBox1.Text = ModbusRTU.MasterDataRepos.RStorageRegFlag[0].ToString();

            //textBox2.Text = ModbusRTU.MasterDataRepos.RStorageRegFlag[1].ToString();
            //textBox3.Text = ModbusRTU.MasterDataRepos.RStorageRegFlag[65534].ToString();
            //textBox4.Text = ModbusRTU.MasterDataRepos.RStorageRegFlag[65535].ToString();
            ArrayList temparraylist = new ArrayList();
            UInt16[] temparray1 = new UInt16[2];
            UInt16[] temparray2 = new UInt16[2];
            UInt16 tempcount = 0;
            temparraylist = ModbusRTU.LoadUnmannedBuses(ModbusRTU.MasterDataRepos.RStorageRegFlag,'r');
            textBox9.Text = temparraylist.Count.ToString();
            textBox10.Text = ModbusRTU.MasterDataRepos.RStorageRegFlag[0].ToString();
            textBox11.Text = ModbusRTU.MasterDataRepos.RStorageRegFlag[1].ToString();

            richTextBox1.Text = "";          
            foreach (UInt16[] tempa in temparraylist)
            {             
                richTextBox1.Text = richTextBox1.Text+ tempa[0].ToString() + " " + tempa[1].ToString() + "\n";
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           //modbusView1.ReadAddress= Convert.ToUInt16(textBox1.Text);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            modbusView1.ReadAddress = Convert.ToUInt16(textBox2.Text);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            //modbusView3.ReadAddress = Convert.ToUInt16(textBox3.Text);
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            //modbusView4.ReadAddress = Convert.ToUInt16(textBox4.Text);
        }

        private void textBox5_TextChanged_1(object sender, EventArgs e)
        {
            //modbusView1.ReadDataLengthInWord = Convert.ToByte(textBox5.Text);
        }

        private void textBox6_TextChanged_1(object sender, EventArgs e)
        {
            modbusView1.ReadDataLengthInWord = Convert.ToByte(textBox6.Text);
        }

        private void textBox7_TextChanged_1(object sender, EventArgs e)
        {
            //modbusView3.ReadDataLengthInWord = Convert.ToByte(textBox7.Text);
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            //modbusView4.ReadDataLengthInWord = Convert.ToByte(textBox8.Text);
        }
    }
}