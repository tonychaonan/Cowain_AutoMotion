using Cowain_AutoMotion;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Cowain_AutoMotion
{
    public class SerialPortClass
    {
        public SerialPort serialPort;
        private System.ComponentModel.IContainer components = null;
        /* Command ********************************************************** */

        //  IniFile myIniFile;
        /* RX =============================================================== */
        byte[] RX_Buffer = new byte[512];                       // RX 緩衝區
                                                                /* ================================================================== */
        public bool m_bDataReceive = false;
        bool m_bisIDLE = true;
        public string strRecData = "";
        /* ****************************************************************** */
        public SerialPortClass(string portName, int iBaudRate)
        {
            components = new System.ComponentModel.Container();
            serialPort = new System.IO.Ports.SerialPort(this.components);
            serialPort.PortName = portName;
            serialPort.BaudRate = iBaudRate;
            serialPort.ReadBufferSize = 128;
            serialPort.WriteBufferSize = 64;
            serialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort_DataReceived);
        }
        public SerialPortClass(ComPortData readerPara)
        {
            components = new System.ComponentModel.Container();
            serialPort = new System.IO.Ports.SerialPort(this.components);
            serialPort.DataBits = readerPara.iDataBit;
            serialPort.StopBits = readerPara.StopBits;
            serialPort.Parity = readerPara.Parity;
            serialPort.PortName = readerPara.strPortName;
            serialPort.BaudRate = readerPara.iBaudRate;
            serialPort.ReadBufferSize = 128;
            serialPort.WriteBufferSize = 64;
            serialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort_DataReceived);
        }
        public SerialPortClass(int nStation, string portName, int iBaudRate, int dataBits, System.IO.Ports.StopBits stopbits, System.IO.Ports.Parity parity)
        {

            components = new System.ComponentModel.Container();
            serialPort = new System.IO.Ports.SerialPort(this.components);
            serialPort.DataBits = dataBits;
            serialPort.StopBits = stopbits;
            serialPort.Parity = parity;
            serialPort.PortName = portName;
            serialPort.BaudRate = iBaudRate;
            serialPort.ReadBufferSize = 128;
            serialPort.WriteBufferSize = 64;
            serialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort_DataReceived);
        }

        ~SerialPortClass()
        {
        }

        public bool COMPort_Connect()
        {
            try
            {
                serialPort.Open();                              // 開啟新的序列埠連線.
                serialPort.DiscardInBuffer();                   // 捨棄序列驅動程式接收緩衝區的資料.
                serialPort.DiscardOutBuffer();                  // 捨棄序列驅動程式傳輸緩衝區的資料.
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DisConnect()
        {
            try
            {
                serialPort.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool REConnect(String portname)
        {
            try
            {
                serialPort.PortName = portname;

                serialPort.Open();                              // 開啟新的序列埠連線.
                serialPort.DiscardInBuffer();                   // 捨棄序列驅動程式接收緩衝區的資料.
                serialPort.DiscardOutBuffer();                  // 捨棄序列驅動程式傳輸緩衝區的資料.
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            System.Threading.Thread.Sleep(10);
            int bytes = serialPort.BytesToRead;

            RX_Buffer = new byte[512];
            if (bytes > 0)
            {
                serialPort.Read(RX_Buffer, 0, bytes);
                //Packet_Check(bytes);                       // 封包資料判斷
            }

            string RecData = "";
            for (int i = 0; i < RX_Buffer.Length; i++)
            {
                if ((RX_Buffer[i] > 31 && RX_Buffer[i] < 127))
                    RecData = RecData + Convert.ToChar(RX_Buffer[i]);
            }

            m_bDataReceive = true;
            strRecData = RecData;
            m_bisIDLE = true;
        }

        public bool GetisIDLE() { return m_bisIDLE; }
        public string GetReceivedDatat() { return strRecData; }
        public bool GetReceivedDatat(ref string strReceiveData)
        {
            strReceiveData = strRecData;
            return m_bDataReceive;
        }

        public void COMPort_Disconnect()                   // 串列埠離線
        {
            serialPort.Close();                             // 關閉連接埠連線.
        }

        public void SetWriteData(string strData)
        {
            try
            {
                m_bisIDLE = false;
                m_bDataReceive = false;
                strRecData = "";
                //m_bDataReceive = false;
                //for (int i = 0; i < RX_Buffer.Length; i++)
                //    RX_Buffer[i] = 0;
                //-------------
                serialPort.Write(strData);
            }
            catch
            {
                return;
            }
        }
        public void SetWriteDataBYTE()
        {
            try
            {
                m_bisIDLE = false;
                m_bDataReceive = false;
                strRecData = "";
                //m_bDataReceive = false;
                //for (int i = 0; i < RX_Buffer.Length; i++)
                //    RX_Buffer[i] = 0;
                //-------------
                byte[] value = new byte[] { 0x0A };
                serialPort.Write(value, 0, value.Length);
            }
            catch
            {
                return;
            }
        }

        public void M_SetWriteDataBYTE(string strData)
        {
            try
            {
                m_bisIDLE = false;
                m_bDataReceive = false;
                strRecData = "";
                strData = strData.Replace(" ", "");
                if ((strData.Length % 2) != 0)
                {
                    strData += " ";
                }
                //  48 49
                byte[] returnBytes = new byte[strData.Length / 2];
                // 16转成10进制
                for (int i = 0; i < returnBytes.Length; i++)
                {
                    returnBytes[i] = Convert.ToByte(strData.Substring(i * 2, 2).Replace(" ", ""), 16);
                }
                serialPort.Write(returnBytes, 0, returnBytes.Length);

            }
            catch (Exception ex)
            {
                return;
            }
        }
    }
    public class ComPortData
    {
        public string controlName { get; set; } = "扫码枪";
        public bool b_Use { get; set; } = false;
        public string strPortName { get; set; } = "COM99";
        public int iBaudRate { get; set; } = 9600;
        public int iDataBit { get; set; } = 8;
        public StopBits StopBits { get; set; } = StopBits.One;
        public Parity Parity { get; set; } = Parity.None;
        public ComPortData()
        {

        }
        public ComPortData(string controlName1,bool b_Use1, string strPortName1, int iBaudRate1, int iDataBit1 = 8, StopBits StopBits1 = StopBits.One, Parity Parity1 = Parity.None)
        {
            controlName = controlName1;
            b_Use = b_Use1;
            strPortName = strPortName1;
            iBaudRate = iBaudRate1;
            iDataBit = iDataBit1;
            StopBits = StopBits1;
            Parity = Parity1;
        }
    }
}
