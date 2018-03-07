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
        private UInt16 _WriteAddress=0;      
        private UInt16 _ReadAddress=0;
        private double _MaxValue = 100;
        private double _MinValue = 1;
        private byte _ReadDataLengthInWord = 1;//至少为1个字，不能为0，不能超过125个字
        private byte _WriteDataLengthInWord = 1;
        private WriteFunctionCodeEnum _WriteFunctionCode = WriteFunctionCodeEnum.WriteCoils;
        private ReadFunctionCodeEnum _ReadFunctionCode = ReadFunctionCodeEnum.ReadCoils;
        private ReadDataTypeEnum _ReadDataType = ReadDataTypeEnum.UINT16;
        private WriteDataTypeEnum _WriteDataType = WriteDataTypeEnum.UINT16;

        private string StringInText;
        private static Word2Byte TempWord=new Word2Byte();
        private static DoubleWord2Byte TempDWord=new DoubleWord2Byte();
        private static FourWord2Byte TempFWord=new FourWord2Byte();

        [Category("ModbusRTU"), Description("读取数据的长度（以字为单位）")]
        public byte ReadDataLengthInWord
        {
            get
            {
                return _ReadDataLengthInWord;
            }

            set
            {
                //一帧数据中包含的字数据长度不能超过125，这一点需要特别注意，加以框定    待处理

                ModbusRTU.VoteToConfirmTransmitRegs('-', ModbusRTU.MasterDataRepos.RStorageRegFlagVoter, ModbusRTU.MasterDataRepos.RStorageRegFlag, _ReadAddress, _ReadDataLengthInWord);//数据长度改变前，将上次地址与数据长度共同决定的表决器值减1并判断票选结果

                switch (_ReadDataType)     //此段代码保证了只有在数据类型为ManualSet的情况下_ReadDataLengthInWord才可以被手动修改，否则程序根据数据类型自动设定数据长度
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
                        if (value < 1) value = 1;     //手动设置时数据长度不能低于1word
                        if (value > 100) value = 100; //不能大于100word
                        _ReadDataLengthInWord = value;
                        break;
                    default:
                        break;
                }

                ModbusRTU.VoteToConfirmTransmitRegs('+', ModbusRTU.MasterDataRepos.RStorageRegFlagVoter, ModbusRTU.MasterDataRepos.RStorageRegFlag, _ReadAddress, _ReadDataLengthInWord);//数据长度改变后，将本次地址与数据长度共同决定的表决器值加1并判断票选结果

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
                if (value > 65535) value = 65535;  //对地址范围加以框定，否则在运行中输入超出范围的数据会弹出异常并中断执行
                if(value<0) value = 0;             //地址范围本来就是0-65535，进行此框定也是合理的
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
                ModbusRTU.VoteToConfirmTransmitRegs('-', ModbusRTU.MasterDataRepos.RStorageRegFlagVoter,ModbusRTU.MasterDataRepos.RStorageRegFlag, _ReadAddress, _ReadDataLengthInWord);//上次地址所对应的表决器值减1并判断票选结果

                if (value > 65535) value = 65535;  //对地址范围加以框定，否则在运行中输入超出范围的数据会弹出异常并中断执行
                if (value < 0) value = 0;             //地址范围本来就是0-65535，进行此框定也是合理的
                _ReadAddress = value;//新设置的地址存入地址属性

                ModbusRTU.VoteToConfirmTransmitRegs('+', ModbusRTU.MasterDataRepos.RStorageRegFlagVoter, ModbusRTU.MasterDataRepos.RStorageRegFlag, _ReadAddress, _ReadDataLengthInWord);//本次地址所对应的表决器值加1并判断票选结果

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
        [Category("ModbusRTU"), Description("输入数据上限值（MaxValue）")]
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

        [Category("ModbusRTU"), Description("输入数据下限值（MinValue）")]
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
          
            ModbusRTU.VoteToConfirmTransmitRegs('+',ModbusRTU.MasterDataRepos.RStorageRegFlagVoter, ModbusRTU.MasterDataRepos.RStorageRegFlag, _ReadAddress,_ReadDataLengthInWord);//控件初始化时，票决器根据地址值自动加1，并判断票选结果
     
        }

        private void PeriodicRequestTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //周期性刷新数据代码   待完成
                this.Invoke((EventHandler)(delegate  //解决线程间调用显示的问题   可能存在线程间等待的问题，需要确认并优化
                {
                    this.Text = StringInText;
                }));
                      
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {

            // 应立刻关闭text的刷新操作      待完成

            base.OnMouseDown(e);
            ShowCaret(this.Handle);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

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
