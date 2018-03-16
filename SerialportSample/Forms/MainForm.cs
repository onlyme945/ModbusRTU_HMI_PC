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
        private ModbusRTU ModbusMaster = new ModbusRTU();

        public SerialportSampleForm()
        {
            InitializeComponent();
            ModbusRTU.IsMaster = true;
            ModbusRTU.AssembleRequestADU(1, ModbusRTU.LoadUnmannedBuses((byte)ModbusRTU.ModbusFuncCode.ReadStorageRegs, 8));
            ModbusRTU.AssembleRequestADU(1, ModbusRTU.LoadUnmannedBuses((byte)ModbusRTU.ModbusFuncCode.ReadInputRegs, 8));
            ModbusRTU.AssembleRequestADU(1, ModbusRTU.LoadUnmannedBuses((byte)ModbusRTU.ModbusFuncCode.ReadCoils, 8));

        }

        private void Form1_Load(object sender, EventArgs e)
        {

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
            

        }

        private void 通讯设置ToolStripMenuItem_Click(object sender, EventArgs e)//新建并打开通讯设置窗口
        {
            CommunicationSettingForm ComSettingForm = new CommunicationSettingForm();
            ComSettingForm.ShowDialog();
        }
    }
}