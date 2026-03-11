using HslCommunication.Profinet.Omron;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HslCommunication;
using System.Collections;
using System.Threading;
using System.Windows.Forms;

namespace OmronFinsUI
{
    //PLC地址：
    //D4900：保压机剩余保压位数量（int），根据数量，判断是否放料
    //D4810-D4890:点胶机放料置位地址（int）,当放一个料时，置1，延时500ms，置0
    public class OmronFins
    {
        public OmronFinsUdp OmronFinsNet;

        //public ModbusTcpNet OmronFinsNet = new ModbusTcpNet();
        // EnumAlarm eA = new EnumAlarm();
        public delegate void AlarmDelegate(string strMsg);
        public event AlarmDelegate ReceiveEvent;
        public bool bConnect
        {
            get;
            private set;
        }
        public Hashtable ht = new Hashtable();

        public bool[] baInput = new bool[80];
        public bool[] baOutput = new bool[60];
        public bool[] baAlarm = new bool[50];
        public bool[] baCylinder = new bool[130];
        public bool[] baButton = new bool[50];
        public bool[] baConveyorState = new bool[10];
        public int[] iaTimeData = new int[20];
        public int[] iaStateData = new int[50];
        public bool[] bAlarmEvent = new bool[60];
        public bool b_Close = false;

        public OmronFins()
        {
            InitializeHashtable();
        }
        public void InitializeHashtable()
        {
            //Thread tConnectBool = new Thread(Connect);
            //  tConnectBool.Start();
            //Thread tReadData = new Thread(Read2);
            //tReadData.Start();
            //OmronFinsNet.SA1 = 0x20; // PC网络号，PC的IP地址的最后一个数
            // OmronFinsNet.DA1 = 0x10; // PLC网络号，PLC的IP地址的最后一个数
            // OmronFinsNet.DA2 = 0x00; // PLC单元号，通常为0
            //OmronFinsNet.ConnectTimeOut = 10;
            bConnect = false;
        }
        public bool Connect(string ip, string port)
        {
            try
            {
                int Port = Convert.ToInt16(port);
                OmronFinsNet = new OmronFinsUdp(ip, Port);
               // string connect = OmronFinsNet.ConnectionId.ToString();
                //if (connect.IsSuccess)
                //{
                //    bConnect = true;
                //}
                //else
                //{
                //    bConnect = false;
                //}
                bConnect = OmronFinsNet.ReadInt16("D1000").IsSuccess;
                bConnect = true;
            }
            catch (Exception ex)
            {
                bConnect = false;
                //  MessageBox.Show(ex.Message);
            }
            return bConnect;
        }
        public void DisConnect()
        {
            try
            {
                // OmronFinsNet.d;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            // return bConnect;
        }
        public bool Readbool(string address)
        {
            try
            {
                bool result = OmronFinsNet.ReadBool(address).Content;
                return result;
            }
            catch { return false; }
        }
        public int Readint(string address)
        {
            try
            {
                int result = OmronFinsNet.ReadInt16(address).Content;
                return result;
            }
            catch { return 0; }
        }
        public bool Write(string address, bool value)
        {
            OmronFinsNet.Write(address, value);
            bool feedback = false;
            int i = 0;
            while (i < 10)
            {
                OperateResult<bool> result = OmronFinsNet.ReadBool(address);
                if (result.Content.ToString() == value.ToString())
                {
                    feedback = true;
                    break;
                }
                i++;
                Thread.Sleep(100);
            }
            return feedback;
        }
        public bool Write(string address, Int32 value)
        {
            OmronFinsNet.Write(address, value);
            bool feedback = false;
            int i = 0;
            while (i < 10)
            {
                OperateResult<Int16> result = OmronFinsNet.ReadInt16(address);
                if (result.Content.ToString() == value.ToString())
                {
                    feedback = true;
                    break;
                }
                i++;
                Thread.Sleep(100);
            }
            return feedback;
        }

    }

}