using System;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;

namespace SerialportSample
{
    public partial class CommunicationSettingForm : Form
    {

        private bool Listening = false;//是否没有执行完invoke相关操作
        private bool Closing = false;//是否正在关闭串口，执行Application.DoEvents，并阻止再次invoke
        public CommunicationSettingForm()
        {
            InitializeComponent();
        }

        private void CommunicationSettingForm_Load(object sender, EventArgs e)
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

            buttonOpenClose.Text = ModbusRTU.SerialCommPort.IsOpen ? "Close" : "Open";

           
        }

        private void CommunicationSettingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);//获取Configuration对象
                config.AppSettings.Settings["ComPort"].Value = comboPortName.Text;//
                config.AppSettings.Settings["Baudrate"].Value = comboBaudrate.Text;//
                config.Save(ConfigurationSaveMode.Modified);//保存
                ConfigurationManager.RefreshSection("appSettings");//刷新

            }
            
        }

        private void buttonOpenClose_Click(object sender, EventArgs e)
        {
            if (ModbusRTU.SerialCommPort.IsOpen)
            {
                Closing = true;
                while (Listening) Application.DoEvents();
                //打开时点击，则关闭串口
                ModbusRTU.SerialCommPort.Close();
            }
            else
            {
              
                //关闭时点击，则设置好端口，波特率后打开
                ModbusRTU.SerialCommPort.PortName = comboPortName.Text;
                ModbusRTU.SerialCommPort.BaudRate = int.Parse(comboBaudrate.Text);
                try
                {
                    ModbusRTU.SerialCommPort.Open();
                }
                catch (Exception ex)
                {
                    //捕获到异常信息，创建一个新的comm对象，之前的不能用了。
                    ModbusRTU.SerialCommPort = new SerialPort();
                    //现实异常信息给客户。
                    MessageBox.Show(ex.Message);
                }
            }
            //设置按钮的状态
            buttonOpenClose.Text = ModbusRTU.SerialCommPort.IsOpen ? "Close" : "Open";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
