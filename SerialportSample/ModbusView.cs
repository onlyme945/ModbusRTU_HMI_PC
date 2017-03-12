using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace SerialportSample
{
    [ToolboxBitmap(typeof(TextBox))]
    public  class ModbusView:System.Windows.Forms.TextBox
    {
        [DllImport("user32.dll")]
        public static extern bool HideCaret(IntPtr hWnd);//隐藏textbox的光标
        [DllImport("user32.dll")]
        public static extern bool ShowCaret(IntPtr hWnd);//显示textbox的光标

        private TextBox textBox1;
        private ModbusRTU modbus1=new ModbusRTU();
        private System.Timers.Timer PeriodicRequestTimer =new System.Timers.Timer();

        #region"/////////////////////自定义的属性///////////////////"
        private int _MyModbusIndex;
        private byte _StationID;
        private UInt16 _WriteAddress;
        private UInt16 _ReadAddress;
        private WriteFunctionCodeEnum _WriteFunctionCode = WriteFunctionCodeEnum.WriteCoils;
        private ReadFunctionCodeEnum _ReadFunctionCode = ReadFunctionCodeEnum.ReadCoils;

        [Category("ModbusRTU"), Description("查询周期")]
        public double RequestPeriod
        {
            get
            {
                return PeriodicRequestTimer.Interval;
            }
            set
            {
                PeriodicRequestTimer.Interval = value;
            }
        }
        [Category("ModbusRTU"), Description("周期查询使能")]
        public bool EnablePeriodRequest
        {
            get
            {
                return PeriodicRequestTimer.Enabled;
            }
            set
            {
                PeriodicRequestTimer.Enabled = value;
            }
        }


        [Category("ModbusRTU"), Description("从站号（ID）")]
        public byte StationID
        {
            set
            {
                _StationID = value;
            }

            get
            {
                return _StationID;
            }

        }

        [Category("ModbusRTU"), Description("写入地址（WriteAddress）")]
        public UInt16 WriteAddress
        {
            set
            {
                _WriteAddress = value;

            }
            get
            {
                return _WriteAddress;
            }

        }

        [Category("ModbusRTU"), Description("读取地址（ReadAddress）")]
        public UInt16 ReadAddress
        {
            set
            {
                _ReadAddress = value;

            }
            get
            {
                return _ReadAddress;
            }

        }

        [Category("ModbusRTU"), Description("读取指令（功能码）")]
        public ReadFunctionCodeEnum ReadFuncCode
        {
            get
            {
                return _ReadFunctionCode;
            }
            set
            {
                _ReadFunctionCode = value;
            }

        }

        [Category("ModbusRTU"), Description("写指令（功能码）")]
        public WriteFunctionCodeEnum WriyeFuncCode
        {
            get
            {
                return _WriteFunctionCode;
            }
            set
            {
                _WriteFunctionCode = value;

            }

        }

        [Category("ModbusRTU"), Description("本控件的Modbus索引值")]
        public int MyModbusIndex
        {
            get
            {
                return _MyModbusIndex;
            }
        }


        #endregion

        public ModbusView()
        {
            PeriodicRequestTimer.Elapsed += PeriodicRequestTimer_Elapsed;
            _MyModbusIndex=ModbusRTU.GetMyModbusIndex();//初始化控件时获得自己的Modbus索引值  重中之重

        }

        private void PeriodicRequestTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            
            modbus1.ControlName=this.Name;
            modbus1.RequestADU_ReadCoils(StationID,ReadAddress,1);
            Console.Write("1s---");
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
          
            base.OnMouseDown(e);
            ShowCaret(this.Handle);
            Console.Write("OnMouseDown!");

        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            if (e.KeyChar == (char)Keys.Enter)
                HideCaret(this.Handle);

            Console.Write("OnKeyPress!");
        }
      
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 21);
            this.textBox1.TabIndex = 0;
            this.ResumeLayout(false);

        }

        #region"////////////////////////枚举量或结构体////////////////////////"
        public enum ReadFunctionCodeEnum
        {
            [Description("读线圈")]
            ReadCoils=0x01,
            [Description("读离散线圈")]
            ReadDistrbuteBits =0x02,
            [Description("读保持寄存器")]
            ReadStorageRegs = 0x03,
            [Description("读输入寄存器")]
            ReadInputRegs = 0x04

         }

        public enum WriteFunctionCodeEnum
        {
            [Description("写单个线圈")]
            WriteSingleCoil = 0x05,
            [Description("写单个寄存器")]
            WriteSingleReg = 0x06,
            [Description("写线圈")]
            WriteCoils = 0x0F,
            [Description("写寄存器")]
            WriteRegs = 0x10

        }
        #endregion
       
    }
}
