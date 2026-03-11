using Cowain_AutoMotion;
using mySocket;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cowain_AutoMotion
{
    public class Connections
    {
        private static Connections currentInstance;
        public Dictionary<string, SerialPortClass> RS232ControlList = null;
        public Dictionary<string, ISocket> SocketControlList = null;
        public static Connections Instance
        {
            get
            {
                if (currentInstance == null)
                {
                    currentInstance = new Cowain_AutoMotion.Connections();
                }
                return currentInstance;
            }
        }
        public void initial()
        {
            RS232ControlList = new Dictionary<string, SerialPortClass>();
            SocketControlList = new Dictionary<string, ISocket>();
            List<SocketParam> socketParams = SQLSugarHelper.DBContext<SocketParam>.GetInstance().GetList();
            List<SerialPortParam> serialPortParam = SQLSugarHelper.DBContext<SerialPortParam>.GetInstance().GetList();
            //实例化所有的串口通讯
            foreach (var item in serialPortParam)
            {
                if (RS232ControlList.Keys.Contains(item.CName) != true)
                {
                    if (item.Used)
                    {
                        ComPortData comPortData = new ComPortData(item.CName, item.Used, item.COMPort, item.BaudRate, item.DataBit, (StopBits)item.StopBit, (Parity)item.ParityBit);
                        Cowain_AutoMotion.SerialPortClass Scanner = new Cowain_AutoMotion.SerialPortClass(comPortData);
                        Scanner.COMPort_Connect();
                        RS232ControlList.Add(item.CName, Scanner);
                    }
                }
            }
            //实例化所有的网口通讯
            foreach (var item in socketParams)
            {
                if (SocketControlList.Keys.Contains(item.CName) != true)
                {
                    if (item.Used)
                    {
                        if (item.isServer != true)
                        {
                            ISocket socket = new SocketClient(item.IP, item.Port.ToString());
                            socket.Start();
                            SocketControlList.Add(item.CName, socket);
                        }
                        else
                        {
                            ISocket socket = new SocketServer(item.IP, item.Port.ToString());
                            socket.Start();
                            SocketControlList.Add(item.CName, socket);
                        }
                    }
                }
            }
        }
        public SerialPortClass getRS232Control(EnumParam_ConnectionName connectionName)
        {
            foreach (var item in RS232ControlList)
            {
                if (item.Key == connectionName.ToString())
                {
                    return item.Value;
                }
            }
            return null;
        }
        public ISocket getSocketControl(EnumParam_ConnectionName connectionName)
        {
            foreach (var item in SocketControlList)
            {
                if (item.Key == connectionName.ToString())
                {
                    return item.Value;
                }
            }
            return null;
        }
    }
}
