using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chart
{
    public class DTK_ModbusRTU : Nordson, IDispenserController
    {
        string portName1;
        int baudRate1;
        string portName2;
        int baudRate2;
        Parity parity;
        int dataBits;
        StopBits stopBits;

        private int Time_Num1 = 0;
        private int Time_Num2 = 0;

        bool reading = false;
        /// <summary>
        /// 温度1
        /// </summary>
        private int F1;
        /// <summary>
        /// 温度2
        /// </summary>
        private int F2;
        /// <summary>
        /// 通讯正常
        /// </summary>
        public bool Isok = false;
        /// <summary>
        /// com口
        /// </summary>
        private SerialPort _ComPort1 = null, _ComPort2 = null;

        private string value;
        public bool b_Close = false;

        public East_Win_ModbusRTUunit norDson232unit1;

        /// <summary>
        /// 温度超时锁
        /// </summary>
        private static ManualResetEvent _TimeOut1 = new ManualResetEvent(false);

        /// <summary>
        /// 压力超时锁
        /// </summary>
        private static ManualResetEvent _TimeOut2 = new ManualResetEvent(false);
        /// <summary>
        /// 添加串口
        /// </summary>
        /// <param name="portName"></param>
        /// 
        public double controller_Presure
        {
            get
            {
                return Presure;
            }
            set
            {
                Presure = value;
                norDson232unit1.Presure = value.ToString("f3");
            }
        }

        public double controller_T1
        {
            get
            {
                return Convert.ToDouble(T1);
            }
            set
            {
                T1 = value.ToString("f3");
                norDson232unit1.T1 = value.ToString("f3");
            }
        }

        public double controller_T2
        {
            get
            {
                return Convert.ToDouble(T2); ;
            }
            set
            {
                T2 = value.ToString("f3");
                norDson232unit1.T2 = value.ToString("f3");
            }
        }

        public double controller_ABrate
        {
            get
            {
                return 0;
            }
            set
            {

            }
        }

        public double controller_APress
        {
            get
            {
                return 0;
            }
            set
            {

            }
        }

        public double controller_BPress
        {
            get
            {
                return 0;
            }
            set
            {

            }
        }
        public DTK_ModbusRTU(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            this.portName1 = portName;
            this.baudRate1 = baudRate;
            this.parity = parity;
            this.dataBits = dataBits;
            this.stopBits = stopBits;

            norDson232unit1 = new East_Win_ModbusRTUunit(this);
            _ComPort1 = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
            connect(_ComPort1);
           
            _ComPort1.DataReceived += new SerialDataReceivedEventHandler(_ComPort_DataReceived);
            _ComPort1.ErrorReceived += _ComPort_ErrorReceived;

            _ThreadRun = true;
            Task.Factory.StartNew(() => { BackThread(); });
        }

        public DTK_ModbusRTU(string portName1, int baudRate1, string portName2, int baudRate2, Parity parity, int dataBits, StopBits stopBits)
        {
            this.portName1 = portName1;
            this.baudRate1 = baudRate1;
            this.portName2 = portName2;
            this.baudRate2 = baudRate2;
            this.parity = parity;
            this.dataBits = dataBits;
            this.stopBits = stopBits;

            norDson232unit1 = new East_Win_ModbusRTUunit(this);
            _ComPort1 = new SerialPort(portName1, baudRate1, parity, dataBits, stopBits);
            _ComPort2 = new SerialPort(portName2, baudRate2, parity, dataBits, stopBits);
            connect(_ComPort1);
            connect(_ComPort2);
            _ComPort1.DataReceived += new SerialDataReceivedEventHandler(_ComPort_DataReceived);
            _ComPort1.ErrorReceived += _ComPort_ErrorReceived;
            _ComPort2.DataReceived += new SerialDataReceivedEventHandler(_ComPort2_DataReceived);
            _ComPort2.ErrorReceived += _ComPort2_ErrorReceived;
            _ThreadRun = true;
            Task.Factory.StartNew(() => { BackThread(); });
        }

        public bool connect(SerialPort _ComPort)
        {
            try
            {
                if (_ComPort == null)
                {
                    _ComPort = new SerialPort(portName1, baudRate1, parity, dataBits, stopBits);
                }
                if (!_ComPort.IsOpen)
                {
                    _ComPort.Open();
                    _ComPort.DiscardInBuffer();
                }
                Thread.Sleep(200);
                if (_ComPort.IsOpen)
                {
                    Isok = true;
                    norDson232unit1.IsNorDsonState = true;
                    b_Connection = true;
                }
                else
                {
                    Isok = false;
                    norDson232unit1.IsNorDsonState = false;
                    b_Connection = false;
                }
            }
            catch (Exception e)
            {
                Isok = false;
                norDson232unit1.IsNorDsonState = false;
                b_Connection = false;
                CloseConnect(_ComPort);
                return false;
            }
            return true;
        }
        public void CloseConnect(SerialPort _ComPort)
        {
            if (_ComPort != null)
            {
                if (_ComPort.IsOpen)
                {
                    _ComPort.Close();
                    _ComPort.Dispose();
                    _ComPort = null;
                    Thread.Sleep(500);

                    Isok = false;
                    norDson232unit1.IsNorDsonState = false;
                    b_Connection = false;
                    //_TimeOut1.Set();
                }
            }
        }
        public void Recont()
        {
            CloseConnect(_ComPort1);
            CloseConnect(_ComPort2);
            connect(_ComPort1);
            connect(_ComPort2);
        }

        public override UserControl GetControl()
        {
            return norDson232unit1;
        }
        private void _ComPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {

        }
        private void _ComPort2_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {

        }
        byte mReceiveByte;
        byte[] bData = new byte[10];
        int x = 0;
        private void _ComPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {

                //将byte数据转换成字符串窗口显示
                int count = _ComPort1.BytesToRead;
                //创建对应内存空间
                byte[] RecieveBuf = new byte[count];
                //读取并往内存变量写入
                _ComPort1.Read(RecieveBuf, 0, count);


                strFormat(RecieveBuf);

                Time_Num1 = 0;


                b_Connection = true;
                Isok = true;
                norDson232unit1.IsNorDsonState = true;
            }
            catch
            {
                
            }
            reading = false;
           
        }
        private void _ComPort2_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {

                //将byte数据转换成字符串窗口显示
                int count = _ComPort2.BytesToRead;
                //创建对应内存空间
                byte[] RecieveBuf = new byte[count];
                //读取并往内存变量写入
                _ComPort2.Read(RecieveBuf, 0, count);

               

                Time_Num1 = 0;


                b_Connection = true;
                Isok = true;
                norDson232unit1.IsNorDsonState = true;
            }
            catch
            {

            }
            reading = false;

        }
        private void strFormat(byte[] bb)
        {
            int i;
            string mTempStr1 = "", mTempStr2 = "", mPresure = ""; 
            if (true)//RTU接受方式
            {
              
                //从站3---功能码03---------读取当前温度值
                  if (bb[0] == 3 && bb[1] == 03 && bb[2] == 4)//胶管
                {
                    mTempStr1 = "";
                    mTempStr2 = "";

                   

                    mTempStr1= bb[3].ToString("X2");

                    mTempStr2 = bb[4].ToString("X2");

                    _ComPort1.DiscardInBuffer();//清输入缓冲区
                    Thread.Sleep(20);

                    if (mTempStr1 == "")
                    {
                        mTempStr1 = "0";
                    }

                    if (mTempStr2 == "")
                    {
                        mTempStr2 = "0";
                    }


                    double d1 = string16ToInt(mTempStr1)*256 + string16ToInt(mTempStr2);

                    T2 = d1.ToString("0.00");
                    norDson232unit1.T2 = d1.ToString("0.00");
                    
                }
                //从站4---功能码03---------读取当前温度值
             else  if (bb[0] == 4 && bb[1] == 03 && bb[2] == 4)//喷嘴
                {
                    mTempStr1 = "";
                    mTempStr2 = "";



                    mTempStr1 = bb[3].ToString("X2");

                    mTempStr2 = bb[4].ToString("X2");

                    _ComPort1.DiscardInBuffer();//清输入缓冲区
                    Thread.Sleep(20);

                    if (mTempStr1 == "")
                    {
                        mTempStr1 = "0";
                    }

                    if (mTempStr2 == "")
                    {
                        mTempStr2 = "0";
                    }


                    double d1 = string16ToInt(mTempStr1) * 256 + string16ToInt(mTempStr2);

                    T1 = d1.ToString("0.00"); ;
                    norDson232unit1.T1 = d1.ToString("0.00");
                   



                }


             
            }
        }
        private bool _ThreadRun = false;

  
    
        /// <summary>
        /// 是否使用胶阀通讯
        /// </summary>
        bool b_Use1 = false;
        public bool b_NOUse
        {
            get
            {
                return b_Use1;
            }

            set
            {
                b_Use1 = value;
                norDson232unit1.setState(value);
            }
        }

        public bool controller_b_Close
        {
            set
            {
                b_Close = value;
            }
        }

        public UserControl controller_userControl
        {
            get
            {
                return norDson232unit1;
            }
        }

        public bool controller_Isok
        {
            get
            {
                return Isok;
            }
        }

        private void BackThread()
        {
            #region 台达温控表 从站1

            string SendMessage1 = "03 03 10 00 00 02 C1 29";
      
            byte[] SendCommand1 = strToHexByte(SendMessage1);

            #endregion
            #region 台达温控表 从站2

            string SendMessage2 = "04 03 10 00 00 02 C0 9E";
           
            byte[] SendCommand2 = strToHexByte(SendMessage2);

            #endregion
            #region 气压表 从站3

            string SendMessage3 = "04 03 10 00 00 02 C0 9E";

            byte[] SendCommand3 = strToHexByte(SendMessage3);

            #endregion
            while (_ThreadRun)
            {
                if (b_Close)
                {
                    break;
                }               
                Thread.Sleep(1000);// 线程循环间隔  
                while  (reading ==true )
                {
                    Thread.Sleep(50);
                }
                try
                {
                  
                    try
                    {
                       

                        try
                        {
                            if(Time_Num1>3)
                            {
                                b_Connection = false;
                                Isok = false;
                                norDson232unit1.IsNorDsonState = false;
                            }


                            _ComPort1.DiscardInBuffer();//串口数据清除
                            _ComPort1.DiscardOutBuffer();


                            //获取当前温度1                        
                            _ComPort1.Write(SendCommand1, 0, SendCommand1.Length);

                            Thread.Sleep(300);// 线程循环间隔  
                            _ComPort1.DiscardInBuffer();//串口数据清除
                            _ComPort1.DiscardOutBuffer();
                            //获取当前温度2                     
                            _ComPort1.Write(SendCommand2, 0, SendCommand2.Length);

                            Thread.Sleep(300);// 线程循环间隔 


                            _ComPort2.DiscardInBuffer();//串口数据清除
                            _ComPort2.DiscardOutBuffer();
                            //获取当前温度2                     
                            _ComPort2.Write(SendCommand3, 0, SendCommand3.Length);

                            Thread.Sleep(300);// 线程循环间隔 

                            Time_Num1++;



                        }
                        catch
                        { }

                    
                    }
                    catch
                    { }

                }
                catch (Exception ex)
                {

                }
            }
        }

        private byte[] strToHexByte(string hexString)  //将字符转强制换成16进制数
        {
            //去除空格
            hexString = hexString.Replace(" ", "");
            //如果不是两个的再后面加""
            if ((hexString.Length % 2) != 0)
            {
                hexString += " ";
            }
            //  48 49
            byte[] returnBytes = new byte[hexString.Length / 2];
            // 转成16进制
            for (int i = 0; i < returnBytes.Length; i++)
            {
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2).Replace(" ", ""), 16);
            }

            return returnBytes;
        }


        private double string16ToInt(string S)
        {
            string str1 = string.Empty;
            string[] str = S.Trim().Split(' '); //S变成字符串数组
            foreach (var item in str)
            {
                str1 += item;
            }
            int temparture = Convert.ToInt32(str1, 16);//将接受的16进制字符串转成整数
            double Now_temparture = Math.Round(Convert.ToDouble(temparture) / 10.0, 1);  //double类型的，保留小数点后一位
            return Now_temparture;
        }
        #region CRC校验
        /// <summary>
        /// CRC校验码
        /// </summary>
        private byte ucCRCHi = 0xFF;
        /// <summary>
        /// CRC校验码
        /// </summary>
        private byte ucCRCLo = 0xFF;
        private readonly byte[] aucCRCHi = {
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
             0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
             0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40
         };
        private readonly byte[] aucCRCLo = {
             0x00, 0xC0, 0xC1, 0x01, 0xC3, 0x03, 0x02, 0xC2, 0xC6, 0x06, 0x07, 0xC7,
             0x05, 0xC5, 0xC4, 0x04, 0xCC, 0x0C, 0x0D, 0xCD, 0x0F, 0xCF, 0xCE, 0x0E,
             0x0A, 0xCA, 0xCB, 0x0B, 0xC9, 0x09, 0x08, 0xC8, 0xD8, 0x18, 0x19, 0xD9,
             0x1B, 0xDB, 0xDA, 0x1A, 0x1E, 0xDE, 0xDF, 0x1F, 0xDD, 0x1D, 0x1C, 0xDC,
             0x14, 0xD4, 0xD5, 0x15, 0xD7, 0x17, 0x16, 0xD6, 0xD2, 0x12, 0x13, 0xD3,
             0x11, 0xD1, 0xD0, 0x10, 0xF0, 0x30, 0x31, 0xF1, 0x33, 0xF3, 0xF2, 0x32,
             0x36, 0xF6, 0xF7, 0x37, 0xF5, 0x35, 0x34, 0xF4, 0x3C, 0xFC, 0xFD, 0x3D,
             0xFF, 0x3F, 0x3E, 0xFE, 0xFA, 0x3A, 0x3B, 0xFB, 0x39, 0xF9, 0xF8, 0x38,
             0x28, 0xE8, 0xE9, 0x29, 0xEB, 0x2B, 0x2A, 0xEA, 0xEE, 0x2E, 0x2F, 0xEF,
             0x2D, 0xED, 0xEC, 0x2C, 0xE4, 0x24, 0x25, 0xE5, 0x27, 0xE7, 0xE6, 0x26,
             0x22, 0xE2, 0xE3, 0x23, 0xE1, 0x21, 0x20, 0xE0, 0xA0, 0x60, 0x61, 0xA1,
             0x63, 0xA3, 0xA2, 0x62, 0x66, 0xA6, 0xA7, 0x67, 0xA5, 0x65, 0x64, 0xA4,
             0x6C, 0xAC, 0xAD, 0x6D, 0xAF, 0x6F, 0x6E, 0xAE, 0xAA, 0x6A, 0x6B, 0xAB,
             0x69, 0xA9, 0xA8, 0x68, 0x78, 0xB8, 0xB9, 0x79, 0xBB, 0x7B, 0x7A, 0xBA,
             0xBE, 0x7E, 0x7F, 0xBF, 0x7D, 0xBD, 0xBC, 0x7C, 0xB4, 0x74, 0x75, 0xB5,
             0x77, 0xB7, 0xB6, 0x76, 0x72, 0xB2, 0xB3, 0x73, 0xB1, 0x71, 0x70, 0xB0,
             0x50, 0x90, 0x91, 0x51, 0x93, 0x53, 0x52, 0x92, 0x96, 0x56, 0x57, 0x97,
             0x55, 0x95, 0x94, 0x54, 0x9C, 0x5C, 0x5D, 0x9D, 0x5F, 0x9F, 0x9E, 0x5E,
             0x5A, 0x9A, 0x9B, 0x5B, 0x99, 0x59, 0x58, 0x98, 0x88, 0x48, 0x49, 0x89,
             0x4B, 0x8B, 0x8A, 0x4A, 0x4E, 0x8E, 0x8F, 0x4F, 0x8D, 0x4D, 0x4C, 0x8C,
             0x44, 0x84, 0x85, 0x45, 0x87, 0x47, 0x46, 0x86, 0x82, 0x42, 0x43, 0x83,
             0x41, 0x81, 0x80, 0x40
         };
        private byte [] trueCrc16(byte[] pucFrame, int usLen)
        {
            int i = 0;
            byte[] bt = new byte[2];
            ucCRCHi = 0xFF;
            ucCRCLo = 0xFF;
            UInt16 iIndex = 0x0000;

            while (usLen-- > 0)
            {
                iIndex = (UInt16)(ucCRCLo ^ pucFrame[i++]);
                ucCRCLo = (byte)(ucCRCHi ^ aucCRCHi[iIndex]);
                ucCRCHi = aucCRCLo[iIndex];
            }
            bt[0] = ucCRCLo;
            bt[1] = ucCRCHi;
            return bt;
        }
        #endregion
    }
}
