using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SerialportSample
{
    public partial class LabelModbus : System.Windows.Forms.Label
    {
        public LabelModbus()
        {
            InitializeComponent();
            PeriodicRefreshTimer.Elapsed += PeriodicRefreshTimer_Elapsed;
            ModbusRTU.VoteToConfirmTransmitRegs('+', (byte)_ReadFunctionCode, _ReadAddress, _ReadDataLengthInWord);//控件初始化时，票决器根据地址值自动加1，并判断票选结果
        }

        //public LabelModbus(IContainer container)
        //{
        //    container.Add(this);

        //    InitializeComponent();
        //}//弃用的带参构造函数

        private System.Timers.Timer PeriodicRefreshTimer = new System.Timers.Timer();

        #region"/////////////////////自定义的属性///////////////////"
        private byte _StationID = 1;
        private UInt16 _ReadAddress = 0;
        private byte _ReadDataLengthInWord = 1;//至少为1个字，不能为0，不能超过125个字
        private ReadFunctionCodeEnum _ReadFunctionCode = ReadFunctionCodeEnum.ReadStorageRegs;
        private ReadDataTypeEnum _ReadDataType = ReadDataTypeEnum.UINT16;

        private string StringInText;
        private static Word2Byte TempWord = new Word2Byte();
        private static DoubleWord2Byte TempDWord = new DoubleWord2Byte();
        private static FourWord2Byte TempFWord = new FourWord2Byte();

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

                ModbusRTU.VoteToConfirmTransmitRegs('-', (byte)_ReadFunctionCode, _ReadAddress, _ReadDataLengthInWord);//数据长度改变前，将上次地址与数据长度共同决定的表决器值减1并判断票选结果

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
                        if (value > 150) value = 150; //不能大于150word
                        _ReadDataLengthInWord = value;
                        break;
                    default:
                        break;
                }

                ModbusRTU.VoteToConfirmTransmitRegs('+', (byte)_ReadFunctionCode, _ReadAddress, _ReadDataLengthInWord);//数据长度改变后，将本次地址与数据长度共同决定的表决器值加1并判断票选结果

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
     

        [Category("ModbusRTU"), Description("查询周期")]
        public double RequestPeriod
        {
            get
            {
                return PeriodicRefreshTimer.Interval;
            }
            set
            {
                PeriodicRefreshTimer.Interval = value;
            }
        }
        [Category("ModbusRTU"), Description("周期查询使能")]
        public bool EnablePeriodRequest
        {
            get
            {
                return PeriodicRefreshTimer.Enabled;
            }
            set
            {
                PeriodicRefreshTimer.Enabled = value;
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


        [Category("ModbusRTU"), Description("读取地址（ReadAddress）")]
        public UInt16 ReadAddress
        {
            set
            {
                ModbusRTU.VoteToConfirmTransmitRegs('-', (byte)_ReadFunctionCode, _ReadAddress, _ReadDataLengthInWord);//上次地址所对应的表决器值减1并判断票选结果

                if (value > 65535) value = 65535;  //对地址范围加以框定，否则在运行中输入超出范围的数据会弹出异常并中断执行
                if (value < 0) value = 0;             //地址范围本来就是0-65535，进行此框定也是合理的
                _ReadAddress = value;//新设置的地址存入地址属性

                ModbusRTU.VoteToConfirmTransmitRegs('+', (byte)_ReadFunctionCode, _ReadAddress, _ReadDataLengthInWord);//本次地址所对应的表决器值加1并判断票选结果

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
                ModbusRTU.VoteToConfirmTransmitRegs('-', (byte)_ReadFunctionCode, _ReadAddress, _ReadDataLengthInWord);//本次地址所对应的表决器值加1并判断票选结果
                _ReadFunctionCode = value;
                ModbusRTU.VoteToConfirmTransmitRegs('+', (byte)_ReadFunctionCode, _ReadAddress, _ReadDataLengthInWord);//本次地址所对应的表决器值加1并判断票选结果

            }

        }

        #endregion



        private void PeriodicRefreshTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //周期性刷新数据代码   待完成
            UInt16[] tempRegs;

            switch (_ReadFunctionCode)
            {
                case ReadFunctionCodeEnum.ReadInputRegs:
                    tempRegs = ModbusRTU.MasterDataRepos.InputRegs;
                    break;

                case ReadFunctionCodeEnum.ReadStorageRegs:
                    tempRegs = ModbusRTU.MasterDataRepos.StorageRegs;
                    break;


                default:
                    this.Text = "读功能码错误";
                    return;

            }


            switch (_ReadDataType)
            {
                case ReadDataTypeEnum.UINT16://显示正确
                    StringInText = tempRegs[_ReadAddress].ToString();
                    break;
                case ReadDataTypeEnum.INT16://显示正确

                    TempWord.Word = tempRegs[_ReadAddress];
                    StringInText = TempWord.SignedWord.ToString();
                    break;
                case ReadDataTypeEnum.FLOAT32://显示正确
                    TempDWord.LWord.Word = tempRegs[_ReadAddress];
                    TempDWord.HWord.Word = tempRegs[_ReadAddress + 1];
                    StringInText = TempDWord.Float32.ToString();
                    break;
                case ReadDataTypeEnum.FLOAT64: //显示不正确 20180309 HS
                    TempFWord.LLWord.Word = tempRegs[_ReadAddress + 1];
                    TempFWord.LWord.Word = tempRegs[_ReadAddress + 0];
                    TempFWord.HWord.Word = tempRegs[_ReadAddress + 3];
                    TempFWord.HHWord.Word = tempRegs[_ReadAddress + 2];
                    StringInText = TempFWord.Float64.ToString();
                    break;
                case ReadDataTypeEnum.ManualSet:  //直接取用  ReadDataLengthInWord 中设置的值，无需修改   
                    StringInText = "暂未提供";
                    break;
                default:
                    break;
            }

            this.Invoke((EventHandler)(delegate  //解决线程间调用显示的问题   可能存在线程间等待的问题，需要确认并优化
            {
                this.Text = StringInText;
            }));

        }

        #region"////////////////////////枚举量或结构体////////////////////////"
        public enum ReadFunctionCodeEnum
        {
            //[Description("读线圈")]
            //ReadCoils = 0x01,
            //[Description("读离散线圈")]
            //ReadDistrbuteBits = 0x02,
            [Description("读保持寄存器")]
            ReadStorageRegs = 0x03,
            [Description("读输入寄存器")]
            ReadInputRegs = 0x04

        }


        public enum ReadDataTypeEnum
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
