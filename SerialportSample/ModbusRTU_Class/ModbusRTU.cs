using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO.Ports;
using System.Timers;
using System.ComponentModel;
using System.Threading;


namespace SerialportSample
{
    public class ModbusRTU
    {

        #region//////////////////修改过待删除的量///////////////////////
        //public const byte ReadCoils        = 0x01;
        //public const byte ReadDistrbuteBits= 0x02;
        //public const byte ReadStorageRegs  = 0x03;
        //public const byte ReadInputRegs    = 0x04;
        //public const byte WriteSingleCoil  = 0x05;
        //public const byte WriteSingleReg   = 0x06;
        //public const byte WriteCoils       = 0x0F;
        //public const byte WriteRegs        = 0x10;


        //public const byte ERR_OK= 0x00;
        //public const byte ERR_Station = 0x01;
        //public const byte ERR_FunctionCode = 0x02;
        //public const byte ERR_Address = 0x03;
        //public const byte ERR_NumOrData = 0x04;//与NumOrData相对应
        //public const byte ERR_CRCCode = 0x05;
        //public const byte ERR_BreakFrame = 0x06;
        //public const byte ERR_Response = 0xFF;

        #endregion

        public static ModbusDataRepository MasterDataRepos = new ModbusDataRepository(65536);
        private TransmitingStatus TxRxStatus= TransmitingStatus.Idle;

        public static byte[] TransmitRxBuffer = new byte[256];
        private static byte[] TransmitTxBuffer;
               
        public static UInt16 TransmitRxLength = 0;
        private static UInt16 TransmitPointerRX = 0;
        public UInt16 RetryCNT = 0;
        
        private Boolean IsACKTimeout = false;
        private Boolean IsBroadcastTimeout = false;
        private Boolean IsRxDone = false;

        public byte StationID = 1;
        public byte FunctionCode = 0;   
        //public UInt16 qErrorCode = 0;//暂未使用
        //public UInt16 wCRCCode = 0;//暂未使用
        public UInt16 TxAddress = 0;


        private static UInt16 NumOrData = 0;//存放读取（写入）的寄存器数量或者写入单个线圈（寄存器）的值
        private static UInt16 DataByteNumber = 0;
        public UInt16 NumOfTransmitData = 0;
        private static Word2Byte TempWord;
        public ByteBits[] ByteBitsExchange=new ByteBits[2];

        public static ArrayList LowSpeedADU = new ArrayList();
        private static UInt16 LowSpeedADUIndex = 0;
        public static ArrayList HighSpeedADU = new ArrayList();

        private static System.Timers.Timer RxDataTimer =new System.Timers.Timer();
        private static System.Timers.Timer ACKTimer = new System.Timers.Timer();
        private static System.Timers.Timer BroadcastTimer = new System.Timers.Timer();
        private static System.Timers.Timer PeriodicTxTimer = new System.Timers.Timer();

        #region////////////////////委托与事件的声明///////////////////////
        public delegate void ModbusSendFrameDelegate(byte[] buffer,int offset,int count);//声明委托类型（就像声明结构体类型一样，委托也是一种类型）
        public static ModbusSendFrameDelegate ModbusSendFrame;//定义委托变量（就像申明了结构体以后，用所申明的结构体定义变量一样）  最后给这个委托变量赋值，就可以使用了，不赋值就是空的，直接使用会报错

        public delegate void ModbusTransmitSuccessDelegate();
        public static event ModbusTransmitSuccessDelegate ModbusTransmitSuccessEvent;//声明数据传输成功事件

        public delegate void ModbusReceiveExceptionDelegate();
        public static event ModbusReceiveExceptionDelegate ModbusReceiveExceptionEvent;//声明收到异常帧事件


        #endregion

        #region/////////////////////ModbusRTU类构造器//////////////////////
        public ModbusRTU()
        {
            
            ACKTimer.Interval= ACKTimerInterval;
            BroadcastTimer.Interval= BroadcastTimerInterval;
            RxDataTimer.Interval = RxTimerInterval;
            PeriodicTxTimer.Interval = 100;//待修改
            PeriodicTxTimer.Enabled = true;

            RxDataTimer.Elapsed += RxDataTimer_Elapsed;//为接收数据定时器创建定时函数
            BroadcastTimer.Elapsed += BroadcastTimer_Elapsed;//为广播定时器创建定时函数
            ACKTimer.Elapsed += ACKTimer_Elapsed;//为应答超时定时器创建定时函数
            PeriodicTxTimer.Elapsed += PeriodicTxTimer_Elapsed;//为周期发送定时器创建定时函数

            ModbusTransmitSuccessEvent += ModbusRTU_ModbusTransmitSuccessEvent;         
            ModbusReceiveExceptionEvent += ModbusRTU_ModbusReceiveExceptionEvent;
        }

        private void ModbusRTU_ModbusReceiveExceptionEvent()
        {
            Console.Write("接收到异常帧\n");
            //作为主设备 收到异常帧 可以触发重发机制
        }

        private void ModbusRTU_ModbusTransmitSuccessEvent()
        {
            //目前只是Master的功能   Slave的需要添加
            TxRxStatus = TransmitingStatus.Idle;
        }

        #endregion

        #region///////////////////////ModbusRTU类属性//////////////////////////
        private static Boolean _IsMaster = true;
        private static int _ControlIndex =-1;
        public static double _ACKTimerInterval = 200;
        public static double _BroadcastTimerInterval = 200;
        public static double _RxTimerInterval = 10;
        public static UInt16 _DepthOfFIFO = 500;

        [Category("ModbusProperty"), Description("主/从选择")]
        public static Boolean IsMaster
        {
            get
            {
                return _IsMaster;
            }
            set
            {
                _IsMaster = value;
            }

        }

        [Category("ModbusProperty"), Description("调用Modbus功能的控件数量")]
        public static int TotalControlNumber //外部控件的索引
        {
            get
            {
                return  _ControlIndex + 1;
            }       
        }

        [Category("ModbusProperty"), Description("调用Modbus功能的控件数量")]
        public static double ACKTimerInterval
        {
            get
            {
                return _ACKTimerInterval;
            }
            set
            {
                _ACKTimerInterval = value;
            }

        }
        [Category("ModbusProperty"), Description("调用Modbus功能的控件数量")]
        public static double BroadcastTimerInterval
        {
            get
            {
                return _BroadcastTimerInterval;
            }
            set
            {
                _BroadcastTimerInterval = value;
            }

        }
        [Category("ModbusProperty"), Description("调用Modbus功能的控件数量")]
        public static double RxTimerInterval
        {
            get
            {
                return _RxTimerInterval;
            }
            set
            {
                _RxTimerInterval = value;
            }

        }
        [Category("ModbusProperty"), Description("FIFO深度")]
        public static UInt16 DepthOfFIFO
        {
            get
            {
                return _DepthOfFIFO;
            }
            set
            {
                _DepthOfFIFO = value;
                if (value < 1) _DepthOfFIFO = 1;
                if (value > 1000) _DepthOfFIFO = 1000;          
            }
        }

        #endregion

        #region//////////////////////ModbusRTU类方法//////////////////////////

        public static bool VoteToConfirmTransmitRegs(char IncreaseOrDecrease, byte FuncCode , UInt16 FirstAddress , byte DataLength) //确定某一地址的数据传送与否的表决器
        {
            byte tempCount=0;
            UInt16 tempAddress = 0;
            UInt16[] VoterReg;
            BitInByte DataStorageFlagReg;

            switch (FuncCode)//根据功能码选择对应的票选器 以及标志位寄存器
            {
                case (byte)ModbusFuncCode.ReadCoils:
                    VoterReg = MasterDataRepos.RCoilFlagVoter;
                    DataStorageFlagReg = MasterDataRepos.RCoilFlag;
                    break;
                case (byte)ModbusFuncCode.ReadDistributeBits:
                    VoterReg = MasterDataRepos.RDistributeBitFlagVoter;
                    DataStorageFlagReg = MasterDataRepos.RDistributeBitFlag;
                    break;
                case (byte)ModbusFuncCode.ReadStorageRegs:
                    VoterReg = MasterDataRepos.RStorageRegFlagVoter;
                    DataStorageFlagReg = MasterDataRepos.RStorageRegFlag;
                    break;
                case (byte)ModbusFuncCode.ReadInputRegs:
                    VoterReg = MasterDataRepos.RInputRegFlagVoter;
                    DataStorageFlagReg = MasterDataRepos.RInputRegFlag;
                    break;

                case (byte)ModbusFuncCode.WriteSingleCoil://方案待定
                    VoterReg = MasterDataRepos.WCoilFlagVoter;
                    DataStorageFlagReg = MasterDataRepos.WCoilFlag;
                    break;
                case (byte)ModbusFuncCode.WriteCoils:
                    VoterReg = MasterDataRepos.WCoilFlagVoter;
                    DataStorageFlagReg = MasterDataRepos.WCoilFlag;
                    break;
                case (byte)ModbusFuncCode.WriteSingleReg://方案待定
                    VoterReg = MasterDataRepos.WStorageRegFlagVoter;
                    DataStorageFlagReg = MasterDataRepos.WStorageRegFlag;
                    break;
                case (byte)ModbusFuncCode.WriteRegs:
                    VoterReg = MasterDataRepos.WStorageRegFlagVoter;
                    DataStorageFlagReg = MasterDataRepos.WStorageRegFlag;
                    break;
                default:
                    return false;
            }

            if (IncreaseOrDecrease == '+')
            {
                for (tempCount = 0; tempCount < DataLength; tempCount++)
                {                   
                    tempAddress =(UInt16) (FirstAddress + tempCount);
                    VoterReg[tempAddress] += 1;//票选器投票加1
                    DataStorageFlagReg[tempAddress] = true;//票选器投票增加，对应地址位显然是需要传输数据的
                }
            }
                
            else if (IncreaseOrDecrease == '-')
            {
                for ( tempCount = 0; tempCount < DataLength; tempCount++)
                {
                    tempAddress = (UInt16)(FirstAddress + tempCount);
                    VoterReg[tempAddress] -= 1;//票选器投票减1
                    if(VoterReg[tempAddress]==0)//如果票选器投票为0，说明当前的地址位未被使用，不需要进行数据传输
                        DataStorageFlagReg[tempAddress] = false;//因而需要把与地址对应的数据传输请求标志位清0

                }
                    
            }

            return true;
        }

        public static bool SuspendRead(byte FuncCode,UInt16 FirstAddress,byte DataLength)//暂停读取数据
        {
            BitInByte bitInByte;
            switch (FuncCode)//根据功能码选择对应的票选器 以及标志位寄存器
            {
                case (byte)ModbusFuncCode.ReadCoils:
                    bitInByte=MasterDataRepos.RCoilFlag;
                    break;
                case (byte)ModbusFuncCode.ReadDistributeBits:
                    bitInByte = MasterDataRepos.RDistributeBitFlag;
                    break;
                case (byte)ModbusFuncCode.ReadStorageRegs:
                    bitInByte = MasterDataRepos.RStorageRegFlag;
                    break;
                case (byte)ModbusFuncCode.ReadInputRegs:
                    bitInByte = MasterDataRepos.RInputRegFlag;
                    break;
    
                default:
                    return  false;
            }
            for (UInt16 tempCount = 0; tempCount < DataLength; tempCount++)
                bitInByte[(uint)FirstAddress+ tempCount] = false;

            return true;
        }

        public static bool ResumeRead(byte FuncCode, UInt16 FirstAddress, byte DataLength)//恢复读取数据
        {
            BitInByte bitInByte;
            switch (FuncCode)//根据功能码选择对应的票选器 以及标志位寄存器
            {
                case (byte)ModbusFuncCode.ReadCoils:
                    bitInByte = MasterDataRepos.RCoilFlag;
                    break;
                case (byte)ModbusFuncCode.ReadDistributeBits:
                    bitInByte = MasterDataRepos.RDistributeBitFlag;
                    break;
                case (byte)ModbusFuncCode.ReadStorageRegs:
                    bitInByte = MasterDataRepos.RStorageRegFlag;
                    break;
                case (byte)ModbusFuncCode.ReadInputRegs:
                    bitInByte = MasterDataRepos.RInputRegFlag;
                    break;

                default:
                    return false;
            }
            for (UInt16 tempCount = 0; tempCount < DataLength; tempCount++)
                bitInByte[(uint)FirstAddress + tempCount] = true;

            return true;
        }
        #endregion

        #region//////////////////////定时器Tick事件////////////////////////

        private void PeriodicTxTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //if (TxRxStatus == TransmitingStatus.Receiving)
            //    RxDataTimer.Enabled = true;
                Console.Write(RxDataTimer.Enabled.ToString()) ;
            MasterSendFrame();
        }

        private void ACKTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            TxRxStatus = TransmitingStatus.Idle;
            ACKTimer.Enabled = false;
            Console.Write("NO ACK!");
           
            IsACKTimeout = true;
        }

        private void BroadcastTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            TxRxStatus = TransmitingStatus.Idle;
            BroadcastTimer.Enabled = false;
            Console.Write("Broadcasting done!");
            IsBroadcastTimeout = true;
        }

        private void RxDataTimer_Elapsed(object sender, EventArgs e)
        {
            RxDataTimer.Enabled = false;
            ModbusReceiveData_SerialPort_Done(); //数据流的间隙时间超过帧间间隔时间表示一帧接收完成
            IsRxDone = true;
        }
        private void ModbusReceiveData_SerialPort_Done()
        {
            TxRxStatus = TransmitingStatus.Idle;//通过RxDataTimer定时器的超时行为判断一帧接收完成后，立即将总线状态置为Idle  运行新一轮的发送接收
                                                //但是要非常注意  万一前面接收到帧还没有解析完，新的发送帧已发送完成并且开始接收新的应答帧  会导致目前唯一的放置接收数据的数组被覆盖，导致前一帧的解析出错
                                                //！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！
            Disassemble_ReceivedADU();//解析收到的帧
            TransmitPointerRX = 0;
        }

        #endregion
   
        #region//////////////////////////ModbusRTU类收/发事件//////////////////////////
        private Boolean MasterSendFrame()
        {           
            Console.Write(TxRxStatus+"\n");
            if (TxRxStatus == TransmitingStatus.Idle)
            {
                if ((HighSpeedADU.Count != 0) || (LowSpeedADU.Count != 0))//高速、低速ADU数组非空则开始发送
                {
                    //Console.Write(HighSpeedADU.Count+" "+ LowSpeedADU.Count+"\n");
                   
                    if (HighSpeedADU.Count != 0)
                    {
                        TransmitTxBuffer = (byte[]) HighSpeedADU[0];//从FIFO中取用待发送的帧                         
                         HighSpeedADU.RemoveAt(0);//控制指令等高速指令，发送完 且 成功 之后即丢弃   这里需要改进，增加发送出错，多次尝试机制————2018.03.08 HS
                    }
                    else//LowSpeedADU.Count != 0
                    {
                        if (LowSpeedADUIndex == LowSpeedADU.Count)
                            LowSpeedADUIndex = 0;
                         TransmitTxBuffer = (byte[])LowSpeedADU[LowSpeedADUIndex++];

                    }

                    try //此处不能用if语句简化
                    {
                        ModbusSendFrame(TransmitTxBuffer, 0, TransmitTxBuffer.Length);//调用发送函数（委托）
                    }
                    catch
                    {
                        Console.Write("Error:ModbusSendFrame未关联 或 Serialport未打开！\n");
                        return false;
                    }

                    TxRxStatus = TransmitingStatus.Sending;//Modbus收发标志位置 “发送中”

                    if (TransmitTxBuffer[0] == 0)//此处用到StationID，说明全局的StationID变量是必要的，不可弃用
                    {
                        IsBroadcastTimeout = false;//如果是广播帧，先清广播帧超时标志位（其实没必要，只是为了以防不可预知的改变）
                        BroadcastTimer.Enabled = true;//打开广播帧超时定时器
                    }
                    else
                    {
                        IsACKTimeout = false;
                        ACKTimer.Enabled = true;
                    }

                }

            }//TxRxStatus == TransmitingStatus.Idle
            return true;

        }

        public void ModbusReceiveData_SerialPort(object sender, SerialDataReceivedEventArgs e)//供外部SerialPort调用的数据接收事件
        {
            RxDataTimer.Enabled = false;//定时器停止计数并将计数值归零
            RxDataTimer.Enabled = true;//定时器重新开始计数
            TxRxStatus = TransmitingStatus.Receiving;//Modbus状态为标记为“接收中”
            UInt16 tempCount = (UInt16)((SerialPort)sender).BytesToRead;//先记录下来，避免某种原因，人为的原因，操作几次之间时间长，缓存不一致
            ((SerialPort)sender).Read(TransmitRxBuffer, TransmitPointerRX, tempCount);//读取缓冲数据
            TransmitPointerRX += tempCount;//增加接收计数
            TransmitRxLength = TransmitPointerRX;
            
        }


        #endregion

        #region/////////////////////////ModbusRTU(主)读写帧组装方法/////////////////////////
        public static void AssembleRequestADU(byte StationID,ArrayList DataTransmitBuses)
        {
            byte[] TxFrame; //组装发送请求帧的时候用局部的数组，免得破坏当前时刻的发送帧（全局数组TransmitTxFrame中的数据）
            byte FuncCode;
            byte tempCount = 0;
            UInt16 Addr;
            UInt16 NumCmdData;
            UInt16 DataByteNumber = 0;
            UInt16 Pointer = 0;
            UInt16 TransmitTxLength = 0;
            BitInByte tempBit2Byte=new BitInByte("Byte",1);

            if (DataTransmitBuses.Count > 0)
            {
                foreach (UInt16[] TansmitBus in DataTransmitBuses)
                {
                    Addr = TansmitBus[0];
                    NumCmdData = TansmitBus[1];
                    FuncCode=(byte)TansmitBus[2];

                    Console.Write("数据长度："+ NumCmdData.ToString());

                    switch (FuncCode)
                    {
                        case (byte)ModbusFuncCode.ReadCoils:
                        case (byte)ModbusFuncCode.ReadDistributeBits:
                        case (byte)ModbusFuncCode.ReadStorageRegs:
                        case (byte)ModbusFuncCode.ReadInputRegs:
                            TransmitTxLength = 8;
                            break;
                        case (byte)ModbusFuncCode.WriteSingleCoil:
                            if (MasterDataRepos.Coils[Addr] == true)
                                NumCmdData = 0xFF00;
                            else
                                NumCmdData = 0;
                            TransmitTxLength = 8;
                            break;
                        case (byte)ModbusFuncCode.WriteSingleReg:
                            NumCmdData = MasterDataRepos.StorageRegs[Addr];
                            TransmitTxLength = 8;
                            break;

                        case (byte)ModbusFuncCode.WriteCoils:
                            DataByteNumber = (UInt16)(NumCmdData / 8 + (NumCmdData % 8 == 0 ? 0 : 1));
                            TransmitTxLength = (UInt16)(9 + DataByteNumber);
                            break;

                        case (byte)ModbusFuncCode.WriteRegs:
                            DataByteNumber = (UInt16)(2 * NumCmdData);
                            TransmitTxLength = (UInt16)(9 + DataByteNumber);
                            break;

                        default:
                            break;
                    }
                    TxFrame = new byte[TransmitTxLength];
                    TxFrame[0] = StationID;
                    TxFrame[1] = FuncCode;
                    TempWord.Word = Addr;
                    TxFrame[2] = TempWord.HByte;
                    TxFrame[3] = TempWord.LByte;
                    TempWord.Word = NumCmdData;
                    TxFrame[4] = TempWord.HByte;
                    TxFrame[5] = TempWord.LByte;
                    Pointer = 6;
                    if (DataByteNumber != 0)
                    {
                        TxFrame[Pointer++] = (byte)DataByteNumber;//填入字节数 
                        tempBit2Byte.Bytes[0] = 0;

                        for (UInt16 i = 0; i < NumCmdData; i++)
                        {

                                if (FuncCode == (byte)ModbusFuncCode.WriteRegs)
                                {
                                    TempWord.Word = MasterDataRepos.StorageRegs[Addr + i];
                                    TxFrame[Pointer++] = TempWord.HByte;//bytes
                                    TxFrame[Pointer++] = TempWord.LByte;//bytes

                                }
                                else
                                {
                                    tempBit2Byte[tempCount++] = MasterDataRepos.Coils[(UInt32)Addr + i];
                                    if ((tempCount == 8) || (i== (NumCmdData-1)))
                                    {
                                        TxFrame[Pointer++] = tempBit2Byte.Bytes[0];
                                        tempBit2Byte.Bytes[0]=0;
                                        tempCount = 0;
                                    }
                                                               
                                }
                        }
                        
                            
                    }
                    TempWord.Word = CRCCaculation.CRC16(TxFrame, Pointer);
                    TxFrame[Pointer++] = TempWord.HByte;//crc code
                    TxFrame[Pointer] = TempWord.LByte;


                    switch (FuncCode)
                    {
                        case (byte)ModbusFuncCode.ReadCoils:
                        case (byte)ModbusFuncCode.ReadDistributeBits:
                        case (byte)ModbusFuncCode.ReadStorageRegs:
                        case (byte)ModbusFuncCode.ReadInputRegs:
                            LowSpeedADU.Add(TxFrame.Clone());
                            break;
                        case (byte)ModbusFuncCode.WriteSingleCoil:
                        case (byte)ModbusFuncCode.WriteSingleReg:
                        case (byte)ModbusFuncCode.WriteCoils:
                        case (byte)ModbusFuncCode.WriteRegs:
                            HighSpeedADU.Add(TxFrame.Clone());
                            break;
                        default:
                            break;
                    }
                //Pointer = 0;//为接收逻辑的使用复位   好像不需要用到啊，先注释掉 ————  2018.03.08  HS

                }


            }

           

        }

        #endregion

        #region//////////////////////////ModbusRTU(从)响应帧组装方法/////////////////////////
        public Boolean Assemble_ResponseADU()//Slaver To DO...
        {

            return true;
        }
        public Boolean Assemble_ExceptionADU()
        {

            return true;
        }

        #endregion
    
        #region///////////////////////ModbusRTU(主/从)Rx帧解析方法//////////////////////////
       public byte Disassemble_ReceivedADU()
        {
            UInt16 tempAddress;
            UInt16 firstAddress;
            BitInByte bitInByte = new BitInByte("Byte",1);
            byte tempIndex=0;
            byte funcCode = 0;
            if (IsMaster == true)
            {               
                if (StationID != TransmitRxBuffer[0]) //站号错误不进行任何处理，立即返回（ACKTimeout定时器继续定时）
                    return (byte)ErrorStatus.ERR_Station;

                ACKTimer.Enabled = false;//通过上面站号检测的说明是期望子节点发送来的响应帧，因而停止ACKTimeout倒计时。

                if (TransmitRxLength < 5) return (byte)ErrorStatus.ERR_BreakFrame;

                TempWord.HByte = TransmitRxBuffer[TransmitRxLength - 2];
                TempWord.LByte = TransmitRxBuffer[TransmitRxLength - 1];
                if (TempWord.Word != CRCCaculation.CRC16(TransmitRxBuffer, (UInt16)(TransmitRxLength - 2)))
                    return (byte)ErrorStatus.ERR_CRCCode;//检测CRC是否错误

                funcCode = TransmitRxBuffer[1];//记录功能码
                TempWord.HByte= TransmitRxBuffer[2];
                TempWord.LByte = TransmitRxBuffer[3];
                TxAddress = TempWord.Word;

                Console.Write(funcCode.ToString()+'\n');

                switch (funcCode)
                {
                    case (byte)ModbusFuncCode.WriteSingleCoil:                        
                    case (byte)ModbusFuncCode.WriteCoils:                 
                    case (byte)ModbusFuncCode.WriteRegs:                       
                    case (byte)ModbusFuncCode.WriteSingleReg:
                        TempWord.HByte = TransmitRxBuffer[2];//检查地址是否正确
                        TempWord.LByte = TransmitRxBuffer[3];
                        if (TempWord.Word != TxAddress) //这里的address没有被赋过值，估计会有问题    20180312——HS
                            return (byte)ErrorStatus.ERR_Address;

                        TempWord.HByte = TransmitTxBuffer[4];
                        TempWord.LByte = TransmitTxBuffer[5];
                        NumOrData = TempWord.Word;//获取写指令发送帧中 写出数据的个数 或者 写单个寄存器的指令
                        TempWord.HByte = TransmitRxBuffer[4];//检查写入单元数量或写入的单个数据是否正确
                        TempWord.LByte = TransmitRxBuffer[5];
                        if (TempWord.Word != NumOrData)
                            return (byte)ErrorStatus.ERR_NumOrData;
                        break;

                    case (byte)ModbusFuncCode.ReadCoils:
                        TempWord.HByte = TransmitTxBuffer[2];//从发送帧中获取读取的字串的首地址
                        TempWord.LByte = TransmitTxBuffer[3];
                        firstAddress = TempWord.Word;
                        DataByteNumber = TransmitRxBuffer[2];//读到数据的字节数
                        tempAddress = firstAddress;
                        for (UInt16 i = 0; i < DataByteNumber; i++)
                        {
                            bitInByte.Bytes[0] = TransmitRxBuffer[3+i];  //读取接收到的数据到位拆分字节                          
                            for(byte tempCount=0;tempCount<8;tempCount++)//判断每一位所对应的读标志寄存器是否置位
                            if (MasterDataRepos.RCoilFlag[(UInt32)tempAddress + tempCount] == true)//如果读标志寄存器置位  说明需要读取位值  否则不操作，即不读值
                                MasterDataRepos.Coils[(UInt32)tempAddress + tempCount] = bitInByte[tempCount];

                            tempAddress += 8;//一个字节的数据读完后，位地址值加8
                        }
                        break;

                    case (byte)ModbusFuncCode.ReadDistributeBits:
                        TempWord.HByte = TransmitTxBuffer[2];//从发送帧中获取读取的字串的首地址
                        TempWord.LByte = TransmitTxBuffer[3];
                        firstAddress = TempWord.Word;
                        DataByteNumber = TransmitRxBuffer[2];//读到数据的字节数
                        tempAddress = firstAddress;
                        for (UInt16 i = 0; i < DataByteNumber; i++)
                        {
                            bitInByte.Bytes[0] = TransmitRxBuffer[3 + i];  //读取接收到的数据到位拆分字节                          
                            for (byte tempCount = 0; tempCount < 8; tempCount++)//判断每一位所对应的读标志寄存器是否置位
                                if (MasterDataRepos.RCoilFlag[(UInt32)tempAddress + tempCount] == true)//如果读标志寄存器置位  说明需要读取位值  否则不操作，即不读值
                                    MasterDataRepos.Coils[(UInt32)tempAddress + tempCount] = bitInByte[tempCount];

                            tempAddress += 8;//一个字节的数据读完后，位地址值加8
                        }
                        break;
                    case (byte)ModbusFuncCode.ReadStorageRegs:
                        NumOfTransmitData = (UInt16)(TransmitRxBuffer[2] >> 1);//从字节数中获取字长
                        TempWord.HByte = TransmitTxBuffer[2];//从发送帧中获取读取的字串的首地址
                        TempWord.LByte = TransmitTxBuffer[3];
                        firstAddress = TempWord.Word;
                        for (UInt16 i = 0; i < NumOfTransmitData; i++)
                        {
                            tempAddress = (UInt16)(firstAddress + i);
                            if (MasterDataRepos.RStorageRegFlag[tempAddress] == true)
                            {
                                TempWord.HByte = TransmitRxBuffer[tempIndex + 3];
                                TempWord.LByte = TransmitRxBuffer[tempIndex + 4];
                                MasterDataRepos.StorageRegs[tempAddress] = TempWord.Word;
                            }
                            tempIndex += 2;//存完一个字后索引值加2
                        }
                        break;
                    case (byte)ModbusFuncCode.ReadInputRegs:
                        NumOfTransmitData = (UInt16)(TransmitRxBuffer[2] >> 1);//从字节数中获取字长
                        TempWord.HByte = TransmitTxBuffer[2];//从发送帧中获取读取的字串的首地址
                        TempWord.LByte = TransmitTxBuffer[3];
                        firstAddress = TempWord.Word;
                        for (UInt16 i = 0; i < NumOfTransmitData; i++)
                        {
                            tempAddress = (UInt16)(firstAddress + i);
                            if (MasterDataRepos.RStorageRegFlag[tempAddress] == true)
                            {
                                TempWord.HByte = TransmitRxBuffer[tempIndex + 3];
                                TempWord.LByte = TransmitRxBuffer[tempIndex + 4];
                                MasterDataRepos.StorageRegs[tempAddress] = TempWord.Word;
                            }
                            tempIndex += 2;//存完一个字后索引值加2
                        }

                        break;


                    case ((byte)ModbusFuncCode.ReadCoils + 0x80):
                    case ((byte)ModbusFuncCode.ReadDistributeBits + 0x80):
                    case ((byte)ModbusFuncCode.ReadStorageRegs + 0x80):
                    case ((byte)ModbusFuncCode.ReadInputRegs + 0x80):
                    case ((byte)ModbusFuncCode.WriteSingleCoil + 0x80):
                    case ((byte)ModbusFuncCode.WriteSingleReg + 0x80):
                    case ((byte)ModbusFuncCode.WriteCoils + 0x80):
                    case ((byte)ModbusFuncCode.WriteRegs + 0x80):

                        ModbusReceiveExceptionEvent();//（触发事件）接收到异常帧
                        return (byte)ErrorStatus.ERR_Response;                      

                    default:
                        return (byte)ErrorStatus.ERR_FunctionCode;//检测FunctionCode是否错误
                       
                }
                Console.Write(funcCode.ToString()+'\n');

                switch (funcCode)
                {
                    case (byte)ModbusFuncCode.WriteSingleCoil:
                        MasterDataRepos.WCoilFlag[TxAddress] = false; //写指令结束后把对应的标志位清掉
                        ResumeRead((byte)ModbusFuncCode.ReadCoils, TxAddress, 1);
                        break;
                    case (byte)ModbusFuncCode.WriteCoils:
                        TempWord.HByte = TransmitTxBuffer[4];
                        TempWord.LByte = TransmitTxBuffer[5];
                        NumOfTransmitData = TempWord.Word;
                        for (UInt16 tempCount = 0; tempCount < NumOfTransmitData; tempCount++)
                            MasterDataRepos.WCoilFlag[(uint)TxAddress + tempCount] = false; //
                        ResumeRead((byte)ModbusFuncCode.ReadCoils, TxAddress, (byte)NumOfTransmitData);
                        break;
                    case (byte)ModbusFuncCode.WriteRegs:
                        TempWord.HByte = TransmitTxBuffer[4];
                        TempWord.LByte = TransmitTxBuffer[5];
                        NumOfTransmitData = TempWord.Word;
                        for (UInt16 tempCount = 0; tempCount < NumOfTransmitData; tempCount++)
                            MasterDataRepos.WStorageRegFlag[(uint)TxAddress + tempCount] = false;
                        ResumeRead((byte)ModbusFuncCode.ReadStorageRegs, TxAddress, (byte)NumOfTransmitData);
                        break;
                    case (byte)ModbusFuncCode.WriteSingleReg:
                        MasterDataRepos.WStorageRegFlag[TxAddress] = false; //
                        ResumeRead((byte)ModbusFuncCode.ReadStorageRegs, TxAddress, 1);
                        break;

                    default:
                        break;

                }

            }
            else
            {
                //Slaver To Do...

            }

            ModbusTransmitSuccessEvent();//（触发事件）如果以上均正确，说明Modbus收发正确无误，触发传输成功事件
            //TxRxStatus = TransmitingStatus.Idle;//总线状态置为空闲，允许发送其他帧（会不会放在本函数最后好，不然要是Modbus发送了其他的帧，并接收了返回的帧，上一次此处的处理还未完成怎么办？）

            return (byte)ErrorStatus.ERR_OK;
        }

        #endregion

        #region/////////////////////ModbusRTU类中的枚举量////////////////////////
        public enum TransmitingStatus
        {
            Sending = 0x00,
            Receiving = 0x01,
            Idle = 0x02,
            //Disassembling=0x03
        }

        public enum ErrorStatus
        {
            ERR_OK = 0x00,
            ERR_Station = 0x01,
            ERR_FunctionCode = 0x02,
            ERR_Address = 0x03,
            ERR_NumOrData = 0x04,//与NumOrData相对应
            ERR_CRCCode = 0x05,
            ERR_BreakFrame = 0x06,
            ERR_Response = 0xFF
        }

        public enum ModbusFuncCode
        {
            ReadCoils = 0x01,
            ReadDistributeBits = 0x02,
            ReadStorageRegs = 0x03,
            ReadInputRegs = 0x04,
            WriteSingleCoil = 0x05,
            WriteSingleReg = 0x06,
            WriteCoils = 0x0F,
            WriteRegs = 0x10
        }

        #endregion

        public struct ModbusDataRepository
        {
            public BitInByte Coils;   
            public BitInByte RCoilFlag;//读Coil标志位
            public UInt16[]  RCoilFlagVoter;//读Coil标志位票选器
            public BitInByte CoilRxDoneFlag;//Coil数据接收成功标志位
            public DoubleWord2Byte RCoilAddrRange;
            public BitInByte WCoilFlag;//写Coil标志位                          *****待用最下面创建的位数组代替，以节约内存用量*****
            public UInt16[]  WCoilFlagVoter;//读Coil标志位票选器          
            public BitInByte CoilTxDoneFlag;//Coil数据发送成功标志位
            public DoubleWord2Byte WCoilAddrRange;


            public BitInByte DistributeBits;  
            public BitInByte RDistributeBitFlag;//读DistributeBit标志位
            public UInt16[]  RDistributeBitFlagVoter;//读DistributeBit标志位票选器
            public BitInByte DistributeBitRxDoneFlag;//DistributeBit数据接收成功标志位
            public DoubleWord2Byte RDistributeBitAddrRange;



            public UInt16[]  StorageRegs;
            public BitInByte WStorageRegFlag;//写StorageReg标志位                          *****待用最下面创建的位数组代替，以节约内存用量*****
            public BitInByte RStorageRegFlag;//读StorageReg标志位
            public UInt16[]  RStorageRegFlagVoter;//读StorageReg标志位票选器
            public UInt16[]  WStorageRegFlagVoter;//读StorageReg标志位票选器
            public BitInByte StorageRegRxDoneFlag;//StorageReg数据接收成功标志位
            public BitInByte StorageRegTxDoneFlag;//StorageReg数据发送成功标志位
            public DoubleWord2Byte RStorageRegAddrRange;
            public DoubleWord2Byte WStorageRegAddrRange;


            public UInt16[]  InputRegs;
            public BitInByte RInputRegFlag;//读InputReg标志位
            public UInt16[]  RInputRegFlagVoter;//读InputReg标志位票选器
            public BitInByte InputRegRxDoneFlag;//InputReg数据接收成功标志位
            public DoubleWord2Byte RInputRegAddrRange;


            public ModbusDataRepository(UInt32 NumOfRegs)
            {
                if (NumOfRegs > 65536) NumOfRegs = 65536; //限制InputReg的数量
                if (NumOfRegs < 1) NumOfRegs = 1;
                Coils = new BitInByte("Bit", NumOfRegs);
                WCoilFlag = new BitInByte("Bit", NumOfRegs);
                RCoilFlag = new BitInByte("Bit", NumOfRegs);
                RCoilFlagVoter = new UInt16[NumOfRegs];
                WCoilFlagVoter = new UInt16[NumOfRegs];
                CoilRxDoneFlag = new BitInByte("Bit", NumOfRegs);
                CoilTxDoneFlag = new BitInByte("Bit", NumOfRegs);
                RCoilAddrRange = new DoubleWord2Byte();
                WCoilAddrRange = new DoubleWord2Byte();

                DistributeBits = new BitInByte("Bit", NumOfRegs);
                RDistributeBitFlag = new BitInByte("Bit", NumOfRegs);
                RDistributeBitFlagVoter = new UInt16[NumOfRegs];
                DistributeBitRxDoneFlag = new BitInByte("Bit", NumOfRegs);
                RDistributeBitAddrRange = new DoubleWord2Byte();

                InputRegs = new UInt16[NumOfRegs];
                RInputRegFlag = new BitInByte("Bit", NumOfRegs);
                RInputRegFlagVoter = new UInt16[NumOfRegs];
                InputRegRxDoneFlag = new BitInByte("Bit", NumOfRegs);
                RInputRegAddrRange = new DoubleWord2Byte();

                StorageRegs = new UInt16[NumOfRegs];
                WStorageRegFlag = new BitInByte("Bit", NumOfRegs);
                RStorageRegFlag = new BitInByte("Bit", NumOfRegs);
                RStorageRegFlagVoter = new UInt16[NumOfRegs];
                WStorageRegFlagVoter = new UInt16[NumOfRegs];
                StorageRegRxDoneFlag = new BitInByte("Bit", NumOfRegs);
                StorageRegTxDoneFlag = new BitInByte("Bit", NumOfRegs);
                RStorageRegAddrRange = new DoubleWord2Byte();
                WStorageRegAddrRange = new DoubleWord2Byte();
            }
          
        }  //Modbus 数据存储库产生器


        public static ArrayList LoadUnmannedBuses(byte FuncCode,UInt16 IntervalBetweenDatas)//无人巴士 功能      可以把允许的无效字（位）间隔作为形参传进来*****待添加
        {
            ArrayList TransmitBuses = new ArrayList();
            UInt32 tempAddress = 0;
            byte FrameDataLimitInWord = 150;
            UInt16 tempFirstAddress = 0;
            UInt16 tempLastAddress = 0;          
            byte tempCount = 0;
            UInt16[] TransmitBus = new UInt16[3];//第0个数存地址；第1个数存待传输的数据长度
            bool IsFirstSeatFree = true;
            BitInByte DataStorageFlagReg;

                switch (FuncCode)//根据功能码选择对应的票选器 以及标志位寄存器
                {
                    case (byte)ModbusFuncCode.ReadCoils:
                    if (IntervalBetweenDatas > 1184) IntervalBetweenDatas = 1184;
                        DataStorageFlagReg = MasterDataRepos.RCoilFlag;
                        break;
                    case (byte)ModbusFuncCode.ReadDistributeBits:
                    if (IntervalBetweenDatas > 1184) IntervalBetweenDatas = 1184;
                    DataStorageFlagReg = MasterDataRepos.RDistributeBitFlag;
                        break;
                    case (byte)ModbusFuncCode.ReadStorageRegs:
                    if (IntervalBetweenDatas > 148) IntervalBetweenDatas = 148;
                    DataStorageFlagReg = MasterDataRepos.RStorageRegFlag;
                        break;
                    case (byte)ModbusFuncCode.ReadInputRegs:
                    if (IntervalBetweenDatas > 148) IntervalBetweenDatas = 148;
                    DataStorageFlagReg = MasterDataRepos.RInputRegFlag;
                        break;

                    case (byte)ModbusFuncCode.WriteSingleCoil://方案待定
                        IntervalBetweenDatas = 0;
                        DataStorageFlagReg = MasterDataRepos.WCoilFlag;
                        break;
                    case (byte)ModbusFuncCode.WriteCoils:
                    if (IntervalBetweenDatas > 1184) IntervalBetweenDatas = 1184;
                    DataStorageFlagReg = MasterDataRepos.WCoilFlag;
                        break;
                    case (byte)ModbusFuncCode.WriteSingleReg://方案待定
                        IntervalBetweenDatas = 0;
                        DataStorageFlagReg = MasterDataRepos.WStorageRegFlag;
                        break;
                    case (byte)ModbusFuncCode.WriteRegs:
                    if (IntervalBetweenDatas > 148) IntervalBetweenDatas = 148;
                    DataStorageFlagReg = MasterDataRepos.WStorageRegFlag;
                        break;
                    default:
                        return TransmitBuses;
                }

           
                for (tempAddress = 0; tempAddress < 65536; tempAddress++)
                {                   
                        if (IsFirstSeatFree == true)//Bus第一个位置是空的
                        {
                            if (DataStorageFlagReg[tempAddress] == true)
                            {
                                TransmitBus[0] = (UInt16)tempAddress;//记录地址
                                tempFirstAddress = TransmitBus[0];//用于计算传输数据长度
                                tempLastAddress = TransmitBus[0];//用于计算传输数据长度
                                if (tempAddress == 65535) //如果是最后一个地址，则装载bus后退出循环
                                {
                                    TransmitBus[1] = 1;//数据长度只为1
                                    TransmitBus[2] = FuncCode;
                                    TransmitBuses.Add(TransmitBus.Clone());//见下文注释  ArrayList 像是动态的指针数组
                                    break;
                                }
                                IsFirstSeatFree = false;
                            }

                        }
                        else
                        {

                            if (DataStorageFlagReg[tempAddress] == true)
                            {
                                if (tempAddress - tempFirstAddress + 1 < FrameDataLimitInWord)
                                {
                                    if (tempAddress < 65535)
                                    {
                                        if (tempCount < IntervalBetweenDatas+1)//有效数据间无效数据的间隔+1
                                        {
                                            tempCount = 0;
                                            tempLastAddress = (UInt16)tempAddress;
                                        }
                                    }
                                    else//tempAddress==65535  扫描到最后一个地址
                                    {
                                        tempCount = 0;
                                        tempLastAddress = (UInt16)tempAddress;
                                        TransmitBus[1] = (UInt16)(tempLastAddress - tempFirstAddress + 1);
                                        TransmitBus[2] = FuncCode;
                                        TransmitBuses.Add(TransmitBus.Clone());// 必须用这种新声明数组的方法，用后面这种方法TransmitBuses.Add(TransmitBus)，最终会让ArrayList中的所有数组元素都一样

                                        IsFirstSeatFree = true;
                                        break;
                                    }
                                }
                                else  //tempAddress - tempFirstAddress + 1==150    传输数据的字长如果到了150则自动停止继续装车
                                {
                                    tempCount = 0;
                                    tempLastAddress = (UInt16)tempAddress;
                                    TransmitBus[1] = (UInt16)(tempLastAddress - tempFirstAddress + 1);
                                    TransmitBus[2] = FuncCode;
                                    TransmitBuses.Add(TransmitBus.Clone());
                                    IsFirstSeatFree = true;
                                    if (tempAddress < 65535)
                                        continue;
                                    else
                                        break;
                                }

                            }
                            else //DataStorageRegFlag[tempAddress] == false
                            {

                                if (tempAddress - tempFirstAddress + 1 < FrameDataLimitInWord)
                                {
                                    if (tempAddress < 65535)
                                    {
                                        if (tempCount == IntervalBetweenDatas)//有效数据间的间隔，可被调整，来控制bus的大小
                                        {
                                            tempCount = 0;
                                            TransmitBus[1] = (UInt16)(tempLastAddress - tempFirstAddress + 1);
                                            TransmitBus[2] = FuncCode;
                                            TransmitBuses.Add(TransmitBus.Clone());//
                                            IsFirstSeatFree = true;
                                        }
                                        else
                                            tempCount++;
                                    }
                                    else//tempAddress == 65535
                                    {
                                        tempCount = 0;
                                        TransmitBus[1] = (UInt16)(tempLastAddress - tempFirstAddress + 1);
                                        TransmitBus[2] = FuncCode;
                                        TransmitBuses.Add(TransmitBus.Clone());//
                                        IsFirstSeatFree = true;
                                        break;
                                    }
                                }
                                else//tempAddress - tempFirstAddress + 1 == 150
                                {
                                    tempCount = 0;
                                    TransmitBus[1] = (UInt16)(tempLastAddress - tempFirstAddress + 1);
                                    TransmitBus[2] = FuncCode;
                                    TransmitBuses.Add(TransmitBus.Clone());//
                                    IsFirstSeatFree = true;
                                    if (tempAddress < 65535)
                                        continue;
                                    else
                                        break;
                                }
                            }

                        }
                  
                }//End for (tempAddress = 0; tempAddress < 65536; tempAddress++)             
            return TransmitBuses;
        }

    }//End of Class ModbusRTU
    //ModbusRTU类到此结束

    #region/////////////////////////ModbusRTU类之外 自定义结构体及附加功能/////////////////////
    public class CRCCaculation
    {

        /* CRC 高位字节值表 */
        public static readonly  byte[] auchCRCHi = {
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
            0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
            0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1,
            0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1,
            0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
            0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40,
            0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1,
            0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
            0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40,
            0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
            0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
            0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
            0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
            0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
            0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40,
            0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1,
            0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
            0x80, 0x41, 0x00, 0xC1, 0x81, 0x40 };
        /* CRC低位字节值表*/
        public static readonly byte[] auchCRCLo = {
            0x00, 0xC0, 0xC1, 0x01, 0xC3, 0x03, 0x02, 0xC2, 0xC6, 0x06,
            0x07, 0xC7, 0x05, 0xC5, 0xC4, 0x04, 0xCC, 0x0C, 0x0D, 0xCD,
            0x0F, 0xCF, 0xCE, 0x0E, 0x0A, 0xCA, 0xCB, 0x0B, 0xC9, 0x09,
            0x08, 0xC8, 0xD8, 0x18, 0x19, 0xD9, 0x1B, 0xDB, 0xDA, 0x1A,
            0x1E, 0xDE, 0xDF, 0x1F, 0xDD, 0x1D, 0x1C, 0xDC, 0x14, 0xD4,
            0xD5, 0x15, 0xD7, 0x17, 0x16, 0xD6, 0xD2, 0x12, 0x13, 0xD3,
            0x11, 0xD1, 0xD0, 0x10, 0xF0, 0x30, 0x31, 0xF1, 0x33, 0xF3,
            0xF2, 0x32, 0x36, 0xF6, 0xF7, 0x37, 0xF5, 0x35, 0x34, 0xF4,
            0x3C, 0xFC, 0xFD, 0x3D, 0xFF, 0x3F, 0x3E, 0xFE, 0xFA, 0x3A,
            0x3B, 0xFB, 0x39, 0xF9, 0xF8, 0x38, 0x28, 0xE8, 0xE9, 0x29,
            0xEB, 0x2B, 0x2A, 0xEA, 0xEE, 0x2E, 0x2F, 0xEF, 0x2D, 0xED,
            0xEC, 0x2C, 0xE4, 0x24, 0x25, 0xE5, 0x27, 0xE7, 0xE6, 0x26,
            0x22, 0xE2, 0xE3, 0x23, 0xE1, 0x21, 0x20, 0xE0, 0xA0, 0x60,
            0x61, 0xA1, 0x63, 0xA3, 0xA2, 0x62, 0x66, 0xA6, 0xA7, 0x67,
            0xA5, 0x65, 0x64, 0xA4, 0x6C, 0xAC, 0xAD, 0x6D, 0xAF, 0x6F,
            0x6E, 0xAE, 0xAA, 0x6A, 0x6B, 0xAB, 0x69, 0xA9, 0xA8, 0x68,
            0x78, 0xB8, 0xB9, 0x79, 0xBB, 0x7B, 0x7A, 0xBA, 0xBE, 0x7E,
            0x7F, 0xBF, 0x7D, 0xBD, 0xBC, 0x7C, 0xB4, 0x74, 0x75, 0xB5,
            0x77, 0xB7, 0xB6, 0x76, 0x72, 0xB2, 0xB3, 0x73, 0xB1, 0x71,
            0x70, 0xB0, 0x50, 0x90, 0x91, 0x51, 0x93, 0x53, 0x52, 0x92,
            0x96, 0x56, 0x57, 0x97, 0x55, 0x95, 0x94, 0x54, 0x9C, 0x5C,
            0x5D, 0x9D, 0x5F, 0x9F, 0x9E, 0x5E, 0x5A, 0x9A, 0x9B, 0x5B,
            0x99, 0x59, 0x58, 0x98, 0x88, 0x48, 0x49, 0x89, 0x4B, 0x8B,
            0x8A, 0x4A, 0x4E, 0x8E, 0x8F, 0x4F, 0x8D, 0x4D, 0x4C, 0x8C,
            0x44, 0x84, 0x85, 0x45, 0x87, 0x47, 0x46, 0x86, 0x82, 0x42,
            0x43, 0x83, 0x41, 0x81, 0x80, 0x40 };

        //int *puchMsg ; 							/* 要进行CRC校验的消息 */ 
        //unsigned int usDataLen ; 							/* 消息中字节数 */ 

       public static UInt16 CRC16(byte[] puchMsgg, UInt16 usDataLen)
        {
            int i;
            byte uchCRCHi = 0xFF;              /* 高CRC字节初始化 */
            byte uchCRCLo = 0xFF;              /* 低CRC 字节初始化 */
            int uIndex;                          /* CRC循环中的索引 */
            for (i = 0; i < usDataLen; i++)
            {                       /* 传输消息缓冲区 */
                uIndex = uchCRCHi ^ puchMsgg[i];      /* 计算CRC */
                uchCRCHi = (byte)(uchCRCLo ^ auchCRCHi[uIndex]);
                uchCRCLo = auchCRCLo[uIndex];
            }
            return (ushort)(uchCRCHi << 8 | uchCRCLo);
        }

    }

    [StructLayout(LayoutKind.Explicit)]
    public struct Word2Byte
    {
        [FieldOffset(0)]
        public byte LByte;

        [FieldOffset(1)]
        public byte HByte;

        [FieldOffset(0)]
        public UInt16 Word;  //无符号

        [FieldOffset(0)]
        public Int16 SignedWord;

    }

    [StructLayout(LayoutKind.Explicit)]
    public struct DoubleWord2Byte
    {
        [FieldOffset(0)]
        public byte Byte0;

        [FieldOffset(1)]
        public byte Byte1;

        [FieldOffset(2)]
        public byte Byte2;

        [FieldOffset(3)]
        public byte Byte3;

        [FieldOffset(0)]
        public Word2Byte LWord;

        [FieldOffset(2)]
        public Word2Byte HWord;

        [FieldOffset(0)]
        public float Float32;

        [FieldOffset(0)]
        public UInt32 UInt32Form;

        [FieldOffset(0)]
        public Int32 Int32Form;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct FourWord2Byte
    {
        [FieldOffset(0)]
        public byte Byte0;

        [FieldOffset(1)]
        public byte Byte1;

        [FieldOffset(2)]
        public byte Byte2;

        [FieldOffset(3)]
        public byte Byte3;

        [FieldOffset(4)]
        public byte Byte4;

        [FieldOffset(5)]
        public byte Byte5;

        [FieldOffset(6)]
        public byte Byte6;

        [FieldOffset(7)]
        public byte Byte7;

        [FieldOffset(0)]
        public Word2Byte LLWord;

        [FieldOffset(2)]
        public Word2Byte LWord;

        [FieldOffset(4)]
        public Word2Byte HWord;

        [FieldOffset(5)]
        public Word2Byte HHWord;

        [FieldOffset(0)]
        public double Float64;
    }


    public struct ByteBits //使用索引器取用byte中的位值
    {
        public byte ALL;

        public bool this [byte index]
        {
            get
            {
                return (ALL & (1 << index)) != 0;
            }

            set
            {
                if (value)
                    ALL |=(byte)(1 << index);
                else
                    ALL &=(byte) (~(1 << index));
            }

        }

    }

    public struct BitInByte //使用索引器取用byte中的位值
    {
        private UInt16 IndexOfByte;
        private byte IndexOfBit;
        public byte[] Bytes;        //连续的字数组


        public BitInByte(string InBitOrByte, UInt32 NumOfData)    //用字节数初始化结构体
        {
            if (NumOfData < 1) NumOfData = 1;
            if (InBitOrByte == "Bit")
                if ((NumOfData % 8) != 0)
                    NumOfData = (UInt32)(NumOfData / 8 + 1);
                else
                    NumOfData = (UInt32)(NumOfData / 8);
                          
            Bytes = new byte[NumOfData];
            IndexOfByte = 0;
            IndexOfBit = 0;
        }


        public bool this[UInt32 FullIndex]             //把连续字数组看成一长串的位数组，位数组中元素的索引下标 （此下标一定不能超过字数组长度*8）
        {
            get
            {
                IndexOfByte = (UInt16)(FullIndex / 8);  //根据位的索引下标，确定该位所在的字节
                IndexOfBit = (byte)(FullIndex % 8);   //根据位的索引下标，确定该位在该字节中所处的位置

                return (Bytes[IndexOfByte] & (1 << IndexOfBit)) != 0;   //取出位数组中索引值所对应的位的值
            }

            set
            {
                IndexOfByte = (UInt16)(FullIndex / 8);  //根据位的索引下标，确定该位所在的字节
                IndexOfBit = (byte)(FullIndex % 8);   //根据位的索引下标，确定该位在该字节中所处的位置

                if (value)
                    Bytes[IndexOfByte] |= (byte)(1 << IndexOfBit);   //设置位数组中索引值所对应的位的值
                else
                    Bytes[IndexOfByte] &= (byte)(~(1 << IndexOfBit));
            }

        }

    }

    #endregion

}