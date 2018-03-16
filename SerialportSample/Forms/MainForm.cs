using System;
using System.Configuration;
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
        private BitInByte testbitinbyte = new BitInByte("Bit",65536);
        private ModbusRTU ModbusMaster = new ModbusRTU();
        private SerialPort comm = new SerialPort();

        private bool Listening = false;//是否没有执行完invoke相关操作
        private bool Closing = false;//是否正在关闭串口，执行Application.DoEvents，并阻止再次invoke


        public SerialportSampleForm()
        {
            InitializeComponent();
        }
        
        //窗体初始化
        private void Form1_Load(object sender, EventArgs e)
        {
            string PortNameStored = ConfigurationManager.AppSettings["ComPort"];
            string LastBaudrate = ConfigurationManager.AppSettings["Baudrate"];
            //初始化下拉串口名称列表框
            string[] ports = SerialPort.GetPortNames();
            Array.Sort(ports);
            comboPortName.Items.AddRange(ports);


            if (comboPortName.Items.Contains(PortNameStored) == true)
                comboPortName.SelectedIndex = comboPortName.Items.IndexOf(PortNameStored);
            else
                comboPortName.SelectedIndex = comboPortName.Items.Count > 0 ? 0 : -1;
            comboBaudrate.SelectedIndex = comboBaudrate.Items.IndexOf(LastBaudrate);

            
            //初始化SerialPort对象
            comm.NewLine = "\r\n";
            comm.RtsEnable = true;//根据实际情况吧。

            //添加事件注册
            ModbusRTU.IsMaster = true;
            comm.DataReceived += ModbusMaster.ModbusReceiveData_SerialPort;
            ModbusRTU.ModbusSendFrame += comm.Write;//为Modbus实例中的发送委托关联正确的发送函数

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
                
                ModbusRTU.AssembleRequestADU(1, ModbusRTU.LoadUnmannedBuses((byte)ModbusRTU.ModbusFuncCode.ReadStorageRegs, 8));
                ModbusRTU.AssembleRequestADU(1, ModbusRTU.LoadUnmannedBuses((byte)ModbusRTU.ModbusFuncCode.ReadInputRegs, 8));
                ModbusRTU.AssembleRequestADU(1, ModbusRTU.LoadUnmannedBuses((byte)ModbusRTU.ModbusFuncCode.ReadCoils, 8));
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


        private void button2_Click(object sender, EventArgs e)
        {
            ArrayList temparraylist = new ArrayList();
            UInt16[] temparray1 = new UInt16[2];
            UInt16[] temparray2 = new UInt16[2];
            UInt16 tempcount = 0;
            temparraylist = ModbusRTU.LoadUnmannedBuses((byte)ModbusRTU.ModbusFuncCode.ReadStorageRegs,8);

            richTextBox1.Text = "";          
            foreach (UInt16[] tempa in temparraylist)
            {             
                richTextBox1.Text = richTextBox1.Text+ tempa[0].ToString() + " " + tempa[1].ToString() + " " + tempa[2].ToString() + "\n";
            }
            temparraylist = ModbusRTU.LoadUnmannedBuses((byte)ModbusRTU.ModbusFuncCode.ReadInputRegs, 8);
            foreach (UInt16[] tempa in temparraylist)
            {
                richTextBox1.Text = richTextBox1.Text + tempa[0].ToString() + " " + tempa[1].ToString() + " " + tempa[2].ToString() + "\n";
            }

        }

        private void SerialportSampleForm_FormClosing(object sender, FormClosingEventArgs e)//程序关闭时保存设置信息，下次启动时无需再次设置
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);//获取Configuration对象
            config.AppSettings.Settings["ComPort"].Value = comboPortName.Text;//
            config.AppSettings.Settings["Baudrate"].Value = comboBaudrate.Text;//
            config.Save(ConfigurationSaveMode.Modified);//保存
            ConfigurationManager.RefreshSection("appSettings");//刷新

        }

        private void 通讯设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommunicationSettingForm ComSettingForm = new CommunicationSettingForm();
            ComSettingForm.ShowDialog();
        }
    }
}