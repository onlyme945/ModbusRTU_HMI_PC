using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SerialportSample
{
    public partial class BitStatus : UserControl
    {
        public BitStatus()
        {
            InitializeComponent();
            this.BorderStyle = BorderStyle.FixedSingle;
            PeriodicRefreshTimer.Elapsed += PeriodicRefreshTimer_Elapsed;
            ModbusRTU.VoteToConfirmTransmitRegs('+', (byte)_ReadFunctionCode, _ReadAddress, _ReadDataLengthInBit);//控件初始化时，票决器根据地址值自动加1，并判断票选结果
        }

        

    

    private System.Timers.Timer PeriodicRefreshTimer = new System.Timers.Timer();

    #region"/////////////////////自定义的属性///////////////////"
    private byte _StationID = 1;
    private UInt16 _ReadAddress = 0;
    private byte _ReadDataLengthInBit = 1;//至少为1个字，不能为0，不能超过125个字
    private ReadFunctionCodeEnum _ReadFunctionCode = ReadFunctionCodeEnum.ReadCoils;

    //private string StringInText;
    //private static Word2Byte TempWord = new Word2Byte();
    //private static DoubleWord2Byte TempDWord = new DoubleWord2Byte();
    //private static FourWord2Byte TempFWord = new FourWord2Byte();

    [Category("ModbusRTU"), Description("读取数据的长度（以字为单位）")]
    public byte ReadDataLengthInBit
    {
        get
        {
            return _ReadDataLengthInBit;
        }

        set
        {
            //一帧数据中包含的字数据长度不能超过125，这一点需要特别注意，加以框定    待处理

            ModbusRTU.VoteToConfirmTransmitRegs('-', (byte)_ReadFunctionCode, _ReadAddress, _ReadDataLengthInBit);//数据长度改变前，将上次地址与数据长度共同决定的表决器值减1并判断票选结果


            if (value < 1) value = 1;     //手动设置时数据长度不能低于1word
            if (value > 150) value = 150; //不能大于150word
            _ReadDataLengthInBit = value;


            ModbusRTU.VoteToConfirmTransmitRegs('+', (byte)_ReadFunctionCode, _ReadAddress, _ReadDataLengthInBit);//数据长度改变后，将本次地址与数据长度共同决定的表决器值加1并判断票选结果

        }

    }



    [Category("ModbusRTU"), Description("查询周期")]
    public double RefreshPeriod
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
            ModbusRTU.VoteToConfirmTransmitRegs('-', (byte)_ReadFunctionCode, _ReadAddress, _ReadDataLengthInBit);//上次地址所对应的表决器值减1并判断票选结果

            if (value > 65535) value = 65535;  //对地址范围加以框定，否则在运行中输入超出范围的数据会弹出异常并中断执行
            if (value < 0) value = 0;             //地址范围本来就是0-65535，进行此框定也是合理的
            _ReadAddress = value;//新设置的地址存入地址属性

            ModbusRTU.VoteToConfirmTransmitRegs('+', (byte)_ReadFunctionCode, _ReadAddress, _ReadDataLengthInBit);//本次地址所对应的表决器值加1并判断票选结果

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
            ModbusRTU.VoteToConfirmTransmitRegs('-', (byte)_ReadFunctionCode, _ReadAddress, _ReadDataLengthInBit);//本次地址所对应的表决器值加1并判断票选结果
            _ReadFunctionCode = value;
            ModbusRTU.VoteToConfirmTransmitRegs('+', (byte)_ReadFunctionCode, _ReadAddress, _ReadDataLengthInBit);//本次地址所对应的表决器值加1并判断票选结果

        }

    }

    #endregion



    private void PeriodicRefreshTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        //周期性刷新数据代码   待完成
        BitInByte tempRegs;

        switch (_ReadFunctionCode)
        {
            case ReadFunctionCodeEnum.ReadCoils:
                tempRegs = ModbusRTU.MasterDataRepos.Coils;
                break;

            case ReadFunctionCodeEnum.ReadDistrbuteBits:
                tempRegs = ModbusRTU.MasterDataRepos.DistributeBits;
                break;


            default:
                this.Text = "读功能码错误";
                return;

        }

        if (tempRegs[_ReadAddress] == true)
            this.BackColor = System.Drawing.Color.Orange;

        else
            this.BackColor = Color.White;

    }

    #region"////////////////////////枚举量或结构体////////////////////////"
    public enum ReadFunctionCodeEnum
    {
        [Description("读线圈")]
        ReadCoils = 0x01,
        [Description("读离散线圈")]
        ReadDistrbuteBits = 0x02
    }
}
}
#endregion