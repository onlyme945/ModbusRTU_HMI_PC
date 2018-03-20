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
        private SystemInfo WorkSpcaeSystemInfoForm = new SystemInfo();
        private FaultInfo WorkSpcaeFaultInfoForm = new FaultInfo();
        private Waveform WorkSpcaeWaveformForm = new Waveform();
        private WorkMode WorkSpaceWorkModeForm = new WorkMode();

        public SerialportSampleForm()
        {
            InitializeComponent();
            WorkSpcaeSystemInfoForm.MdiParent = this;
            WorkSpcaeFaultInfoForm.MdiParent = this;
            WorkSpcaeWaveformForm.MdiParent = this;
            WorkSpaceWorkModeForm.MdiParent = this;

            ModbusRTU.IsMaster = true;
            ModbusRTU.AssembleRequestADU(1, ModbusRTU.LoadUnmannedBuses((byte)ModbusRTU.ModbusFuncCode.ReadStorageRegs, 8));
            ModbusRTU.AssembleRequestADU(1, ModbusRTU.LoadUnmannedBuses((byte)ModbusRTU.ModbusFuncCode.ReadInputRegs, 8));
            ModbusRTU.AssembleRequestADU(1, ModbusRTU.LoadUnmannedBuses((byte)ModbusRTU.ModbusFuncCode.ReadCoils, 8));

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void 通讯设置ToolStripMenuItem_Click(object sender, EventArgs e)//新建并打开通讯设置窗口
        {
            CommunicationSettingForm ComSettingForm = new CommunicationSettingForm();
            ComSettingForm.ShowDialog();
        }


        private void 工作空间ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if(treeView1.SelectedNode.Name=="SystemStatus")
            {
                WorkSpcaeSystemInfoForm.Show();
                WorkSpcaeSystemInfoForm.Activate();
            }
            if (treeView1.SelectedNode.Name == "Waveforms")
            {
                WorkSpcaeWaveformForm.Show();
                WorkSpcaeWaveformForm.Activate();
            }
            if (treeView1.SelectedNode.Name == "FaultMsg")
            {
                WorkSpcaeFaultInfoForm.Show();
                WorkSpcaeFaultInfoForm.Activate();
            }
            if (treeView1.SelectedNode.Name == "WorkMode")
            {
                WorkSpaceWorkModeForm.Show();
                WorkSpaceWorkModeForm.Activate();
            }
        }
    }
}