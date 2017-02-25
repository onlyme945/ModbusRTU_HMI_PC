using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace SerialportSample
{
    public class ModbusRTU
    {
        public const byte ReadCoils        = 0x01;
        public const byte ReadDistrbuteBits= 0x02;
        public const byte ReadStorageRegs  = 0x03;
        public const byte ReadInputRegs    = 0x04;
        public const byte WriteSingleCoil  = 0x05;
        public const byte WriteSingleReg   = 0x06;
        public const byte WriteCoils       = 0x0F;
        public const byte WriteRegs        = 0x10;

        public const byte Transmitting     = 0x00;
        public const byte Receiving        = 0x01;
        public const byte IdleWait         = 0x02;


        public static byte[] TxRxBuffer = new byte[256];
        public byte[] Datas = new byte[250];
        public static UInt16 TxLength = 0;
        public static UInt16 RxLength = 0;
        public UInt16 Pointer = 0;
        public UInt16 RetryCNT = 0;
        public UInt16 ACKTimeoutCNT = 0;
        public UInt16 BroadcastTimeoutCNT = 0;
        public UInt16 RxTimeoutCNT = 0;
        public Boolean IsACKTimeout = false;
        public Boolean IsBroadcastTimeout = false;
        public Boolean IsRxDone = false;
        public byte StationID = 0;
        public byte FunctionCode = 0;
        public UInt16 ErrorCode = 0;
        public UInt16 CRCCode = 0;
        public UInt16 Address = 0;
        public UInt16 TargetNumber = 0;
        public UInt16 DataByteNumber = 0;
        public UInt16 DataWordNumber = 0;
        public UInt16 Status = 0;
        public Word2Byte TempWord;
        public Boolean IsMaster = false;

        public delegate void ModbusSendFrameDelegate(byte[] buffer,int offset,int count);//声明委托类型（就像声明结构体类型一样，委托也是一种类型）
        public ModbusSendFrameDelegate ModbusSendFrame;//定义委托变量（就像申明了结构体以后，用所申明的结构体定义变量一样）  最后给这个委托变量赋值，就可以使用了，不赋值就是空的，直接使用会报错

        public Boolean RequestADU_ReadCoils(byte ID,UInt16 Addr,UInt16 Num)
        {
            StationID = ID;
            Address = Addr;
            FunctionCode = ReadCoils;


            TxRxBuffer[0] = ID;
            TxRxBuffer[1] = ReadCoils;
            TempWord.Word = Addr;
            TxRxBuffer[2] = TempWord.HByte;
            TxRxBuffer[3] = TempWord.LByte;
            TempWord.Word = Num;
            TxRxBuffer[4] = TempWord.HByte;
            TxRxBuffer[5] = TempWord.LByte;
            TempWord.Word = CRCCaculation.CRC16(TxRxBuffer, 6);
            TxRxBuffer[6] = TempWord.HByte;
            TxRxBuffer[7] = TempWord.LByte;
            TxLength = 8;

            ModbusSendFrame(TxRxBuffer,0,TxLength);//调用发送函数（委托）

            return true;
        }

        public Boolean RequestADU_ReadDistrbuteBits(byte ID, UInt16 Addr, UInt16 Num)
        {
            StationID = ID;
            Address = Addr;
            FunctionCode = ReadDistrbuteBits;

            TxRxBuffer[0] = ID;
            TxRxBuffer[1] = ReadDistrbuteBits;
            TempWord.Word = Addr;
            TxRxBuffer[2] = TempWord.HByte;
            TxRxBuffer[3] = TempWord.LByte;
            TempWord.Word = Num;
            TxRxBuffer[4] = TempWord.HByte;
            TxRxBuffer[5] = TempWord.LByte;
            TempWord.Word = CRCCaculation.CRC16(TxRxBuffer, 6);
            TxRxBuffer[6] = TempWord.HByte;
            TxRxBuffer[7] = TempWord.LByte;
            TxLength = 8;

            ModbusSendFrame(TxRxBuffer, 0, TxLength);//调用发送函数（委托）
                                                    
            return true;
        }
        public Boolean RequestADU_ReadStorageRegs(byte ID, UInt16 Addr, UInt16 Num)
        {
            StationID = ID;
            Address = Addr;
            FunctionCode = ReadStorageRegs;

            TxRxBuffer[0] = ID;
            TxRxBuffer[1] = ReadStorageRegs;
            TempWord.Word = Addr;
            TxRxBuffer[2] = TempWord.HByte;
            TxRxBuffer[3] = TempWord.LByte;
            TempWord.Word = Num;
            TxRxBuffer[4] = TempWord.HByte;
            TxRxBuffer[5] = TempWord.LByte;
            TempWord.Word = CRCCaculation.CRC16(TxRxBuffer, 6);
            TxRxBuffer[6] = TempWord.HByte;
            TxRxBuffer[7] = TempWord.LByte;
            TxLength = 8;

            ModbusSendFrame(TxRxBuffer, 0, TxLength);//调用发送函数（委托）

            return true;
        }
        public Boolean RequestADU_ReadInputRegs(byte ID, UInt16 Addr, UInt16 Num)
        {
            StationID = ID;
            Address = Addr;
            FunctionCode = ReadInputRegs;

            TxRxBuffer[0] = ID;
            TxRxBuffer[1] = ReadInputRegs;
            TempWord.Word = Addr;
            TxRxBuffer[2] = TempWord.HByte;
            TxRxBuffer[3] = TempWord.LByte;
            TempWord.Word = Num;
            TxRxBuffer[4] = TempWord.HByte;
            TxRxBuffer[5] = TempWord.LByte;
            TempWord.Word = CRCCaculation.CRC16(TxRxBuffer, 6);
            TxRxBuffer[6] = TempWord.HByte;
            TxRxBuffer[7] = TempWord.LByte;
            TxLength = 8;

            ModbusSendFrame(TxRxBuffer, 0, TxLength);//调用发送函数（委托）

            return true;
        }

        public Boolean RequestADU_WriteSingleCoil(byte ID, UInt16 Addr, UInt16 Cmd)
        {
            StationID = ID;
            Address = Addr;
            FunctionCode = WriteSingleCoil;

            TxRxBuffer[0] = ID;
            TxRxBuffer[1] = WriteSingleCoil;
            TempWord.Word = Addr;
            TxRxBuffer[2] = TempWord.HByte;
            TxRxBuffer[3] = TempWord.LByte;
            TempWord.Word = Cmd;
            TxRxBuffer[4] = TempWord.HByte;
            TxRxBuffer[5] = TempWord.LByte;
            TempWord.Word = CRCCaculation.CRC16(TxRxBuffer, 6);
            TxRxBuffer[6] = TempWord.HByte;
            TxRxBuffer[7] = TempWord.LByte;
            TxLength = 8;

            ModbusSendFrame(TxRxBuffer, 0, TxLength);//调用发送函数（委托）

            return true;
        }

        public Boolean RequestADU_WriteSingleReg(byte ID, UInt16 Addr, UInt16 SingleData)
        {
            StationID = ID;
            Address = Addr;
            FunctionCode = WriteSingleReg;

            TxRxBuffer[0] = ID;
            TxRxBuffer[1] = WriteSingleReg;
            TempWord.Word = Addr;
            TxRxBuffer[2] = TempWord.HByte;
            TxRxBuffer[3] = TempWord.LByte;
            TempWord.Word = SingleData;
            TxRxBuffer[4] = TempWord.HByte;
            TxRxBuffer[5] = TempWord.LByte;
            TempWord.Word = CRCCaculation.CRC16(TxRxBuffer, 6);
            TxRxBuffer[6] = TempWord.HByte;
            TxRxBuffer[7] = TempWord.LByte;
            TxLength = 8;

            ModbusSendFrame(TxRxBuffer, 0, TxLength);//调用发送函数（委托）

            return true;
        }

        public Boolean RequestADU_WriteCoils(byte ID, UInt16 Addr, UInt16 Num)//要好好考虑
        {
            StationID = ID;
            Address = Addr;
            FunctionCode = WriteCoils;

            TxRxBuffer[0] = ID;
            TxRxBuffer[1] = WriteCoils;
            TempWord.Word = Addr;
            TxRxBuffer[2] = TempWord.HByte;
            TxRxBuffer[3] = TempWord.LByte;
            TempWord.Word = Num;
            TxRxBuffer[4] = TempWord.HByte;
            TxRxBuffer[5] = TempWord.LByte;
            TempWord.Word = CRCCaculation.CRC16(TxRxBuffer, 6);
            TxRxBuffer[6] = TempWord.HByte;
            TxRxBuffer[7] = TempWord.LByte;
            TxLength = 8;

            ModbusSendFrame(TxRxBuffer, 0, TxLength);//调用发送函数（委托）

            return true;
        }

        public Boolean RequestADU_WriteRegs(byte ID, UInt16 Addr, UInt16 Num,UInt16[] DataBuffer)
        {
            StationID = ID;
            Address = Addr;
            FunctionCode = WriteRegs;

            TxRxBuffer[0] = ID;
            TxRxBuffer[1] = WriteRegs;
            TempWord.Word = Addr;
            TxRxBuffer[2] = TempWord.HByte;
            TxRxBuffer[3] = TempWord.LByte;
            TempWord.Word = Num;
            TxRxBuffer[4] = TempWord.HByte;
            TxRxBuffer[5] = TempWord.LByte;
            TxRxBuffer[6] = (byte)(2*Num);
            Pointer = 7;
            for (int i = 0; i < Num; i++)
            {
                TempWord.Word = DataBuffer[i];
                TxRxBuffer[Pointer++] = TempWord.HByte;
                TxRxBuffer[Pointer++] = TempWord.LByte;
            }

            TempWord.Word = CRCCaculation.CRC16(TxRxBuffer, Pointer);
            TxRxBuffer[Pointer++] = TempWord.HByte;
            TxRxBuffer[Pointer++] = TempWord.LByte;
            TxLength = Pointer;

            ModbusSendFrame(TxRxBuffer, 0, TxLength);//调用发送函数（委托）

            return true;
        }


        public Boolean SendFrame(byte[] data,UInt16 offset,UInt16 length) //最好搞成委托，好调用外部的发送函数
        {

            return true;

        }


        public Boolean Assemble_RequestADU(UInt16 BeginAddress,byte[] DataToSend)
        {
            switch (FunctionCode)
            {
                case ReadCoils:
                case ReadDistrbuteBits:
                case ReadStorageRegs:
                case ReadInputRegs:      
                TxRxBuffer[0] = StationID;
                TxRxBuffer[1] = FunctionCode;
                TempWord.Word = BeginAddress;
                TxRxBuffer[2] = TempWord.HByte;
                TxRxBuffer[3] = TempWord.LByte;
                TempWord.Word = TargetNumber;
                TxRxBuffer[4] = TempWord.HByte;
                TxRxBuffer[5] = TempWord.LByte;
                TempWord.Word = CRCCaculation.CRC16(TxRxBuffer,6);
                TxRxBuffer[6] = TempWord.HByte;
                TxRxBuffer[7] = TempWord.LByte;
                TxLength = 8;
                    break;

                case WriteSingleCoil:
                    break;

                case WriteSingleReg:
                    break;

                case WriteCoils:
                    break;

                case WriteRegs:
                    break;

                default:
                    break;

            }
            

            return true;
        }

        public Boolean Assemble_ResponseADU()
        {

            return true;
        }
        public Boolean Assemble_ExceptionADU()
        {

            return true;
        }

        public Boolean Disassemble_ReceivedADU(System.Timers.Timer  ACKTimer)
        {
            if (IsMaster == true)
            {
                if (StationID != TxRxBuffer[0]) //站号错误不进行任何处理，立即返回（ACKTimeout定时器继续定时）
                    return false;

                ACKTimer.Enabled = false;//通过上面站号检测的说明是期望子节点发送来的响应帧，因而停止ACKTimeout倒计时。
                TempWord.HByte = TxRxBuffer[RxLength - 1];
                TempWord.LByte = TxRxBuffer[RxLength - 2];
                TempWord.Word= CRCCaculation.CRC16(TxRxBuffer,(UInt16)(RxLength-2));


            }
            else
            {


            }
            return true;
        }


    }


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
        public UInt16 Word;
    }
}