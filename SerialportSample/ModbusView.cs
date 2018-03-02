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

        private TextBox ModbusTextBox;
        //private ModbusRTU modbus1=new ModbusRTU();
        private System.Timers.Timer PeriodicRequestTimer =new System.Timers.Timer();

        #region"/////////////////////自定义的属性///////////////////"
        private int _MyModbusIndex=-1;
        private byte _StationID=1;
        private UInt16 _WriteAddress=1;
        private UInt16 _ReadAddress=1;
        private double _MaxValue = 0;
        private double _MinValue = 100;
        private byte _ReadDataLengthInWord = 1;
        private byte _WriteDataLengthInWord = 1;
        private WriteFunctionCodeEnum _WriteFunctionCode = WriteFunctionCodeEnum.WriteCoils;
        private ReadFunctionCodeEnum _ReadFunctionCode = ReadFunctionCodeEnum.ReadCoils;
        private ReadDataTypeEnum _ReadDataType = ReadDataTypeEnum.UINT16;
        private WriteDataTypeEnum _WriteDataType = WriteDataTypeEnum.UINT16;



        [Category("ModbusRTU"), Description("读取数据的长度（以字为单位）")]
        public byte ReadDataLengthInWord
        {
            get
            {
                return _ReadDataLengthInWord;
            }

            set
            {
               
                _ReadDataLengthInWord = value;
                

            }

        }

        [Category("ModbusRTU"), Description("输出数据的长度（以字为单位）")]
        public byte WriteDataLengthInWord
        {
            get
            {
                return _WriteDataLengthInWord;
            }

            set
            {
                _WriteDataLengthInWord = value;
            }

        }


        [Category("ModbusRTU"), Description("读取的数据类型")]
        public ReadDataTypeEnum ReadDataType
        {
            get
            {
                return _ReadDataType;
            }
            set
            {
                _ReadDataType = value;
                switch (_ReadDataType)
                {
                    case ReadDataTypeEnum.UINT16:
                    case ReadDataTypeEnum.INT16:
                        ReadDataLengthInWord = 1;
                        break;
                    case ReadDataTypeEnum.FLOAT32:
                        ReadDataLengthInWord = 2;
                        break;
                    case ReadDataTypeEnum.FLOAT64:
                        ReadDataLengthInWord = 4;
                        break;
                    case ReadDataTypeEnum.ManualSet:  //直接取用  ReadDataLengthInWord 中设置的值，无需修改     
                    default:
                        break;
                }
            }

        }

        [Category("ModbusRTU"), Description("发送的数据类型")]
        public WriteDataTypeEnum WriteDataType
        {
            get
            {
                return _WriteDataType;
            }
            set
            {
                _WriteDataType = value;
            }

        }

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
        public WriteFunctionCodeEnum WriteFuncCode
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
        [Category("ModbusRTU"), Description("输入上限值（MaxValue）")]
        public double MaxValue
        {
            get
            {
                return _MaxValue;
            }

            set
            {               
                _MaxValue = value;
                if (value < _MinValue) _MaxValue = _MinValue;
            }
        }

        [Category("ModbusRTU"), Description("输入下限值（MinValue）")]
        public double MinValue
        {
            get
            {
                return _MinValue;
            }

            set
            {
                _MinValue = value;
                if (value > _MaxValue) _MinValue = _MaxValue;
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
            switch (ReadDataType)
            {
                case ReadDataTypeEnum.UINT16:
                case ReadDataTypeEnum.INT16:
                    _ReadDataLengthInWord = 1;
                    break;
                case ReadDataTypeEnum.FLOAT32:
                    _ReadDataLengthInWord = 2;
                    break;
                case ReadDataTypeEnum.FLOAT64:
                    _ReadDataLengthInWord = 4;
                    break;
                case ReadDataTypeEnum.ManualSet:  //直接取用  ReadDataLengthInWord 中设置的值，无需修改                
                default:
                break;
            }


            ModbusRTU.AssembleRequestADU(_MyModbusIndex,false,_StationID,(byte)ReadFuncCode,ReadAddress, _ReadDataLengthInWord, null);
            
            if (ModbusRTU.GetDataStorageFlag(_MyModbusIndex) == true)
                this.Invoke((EventHandler)(delegate      //解决线程间调用显示的问题   可能存在线程间等待的问题，需要确认并优化
                {
                    this.Text = "22222";
                }));
            
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            ShowCaret(this.Handle);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            //byte[] DataToTx;

            //if (e.KeyChar == (char)Keys.Enter)
            //{
            //    HideCaret(this.Handle);
            //    ModbusRTU.AssembleRequestADU(_MyModbusIndex, true, 1, (byte)WriteFuncCode, WriteAddress, 1, null);

            //}

        }
      
        private void InitializeComponent()
        {
            this.ModbusTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ModbusTextBox
            // 
            this.ModbusTextBox.Location = new System.Drawing.Point(0, 0);
            this.ModbusTextBox.Name = "ModbusTextBox";
            this.ModbusTextBox.Size = new System.Drawing.Size(100, 21);
            this.ModbusTextBox.TabIndex = 0;
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
        public enum ReadDataTypeEnum
        {
            [Description("UINT16")]
            UINT16=0x01,
            [Description("INT16")]
            INT16 = 0x02,
            [Description("FLOAT32")]
            FLOAT32=0x03,
            [Description("FLOAT64")]
            FLOAT64 = 0x04,
            [Description("手动设置")]
            ManualSet=0x05


        }
        public enum WriteDataTypeEnum
        {
            [Description("UINT16")]
            UINT16 = 0x01,
            [Description("INT16")]
            INT16 = 0x02,
            [Description("FLOAT32")]
            FLOAT32 = 0x03,
            [Description("FLOAT64")]
            FLOAT64 = 0x04,
            [Description("手动设置")]
            ManualSet = 0x05

        }
        #endregion

    }
}
