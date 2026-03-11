using Cowain_AutoMotion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolTotal;
using ToolTotal_1;

namespace Cowain_AutoMotion
{
    public class ConnectionControl
    {
        public static RS232 getRS232Control(EnumParam_ConnectionName connectionName)
        {
            foreach (var item in Connections.Instance.RS232ControlList)
            {
                if (item.Key == connectionName.ToString())
                {
                    //if(item.Value.strRecData==null)
                    //{
                    //    return item.Value;
                    //}
                    return item.Value;
                }
            }
            return null;
        }
        public static ISocket getSocketControl(EnumParam_ConnectionName connectionName)
        {
            foreach (var item in Connections.Instance.SocketControlList)
            {
                if (item.Key == connectionName.ToString())
                {
                    return item.Value;
                }
            }
            return null;
        }
    }
    public class Connections
    {
        private static Connections currentInstance;
        public ConnectionControlParam connectionControlParam;
        public Dictionary<string, RS232> RS232ControlList = null;
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
            RS232ControlList = new Dictionary<string, RS232>();
            SocketControlList = new Dictionary<string, ISocket>();
            connectionControlParam = new ConnectionControlParam();
            connectionControlParam.ReaderParams(Program.StrBaseDic, "ConnectionControlParam", ref connectionControlParam);
            if (connectionControlParam == null)
            {
                connectionControlParam = new ConnectionControlParam();
                connectionControlParam.ReadBufferDate(Program.StrBaseDic, "ConnectionControlParam", ref connectionControlParam);
            }
            connectionControlParam.SetSaveFile(Program.StrBaseDic, "ConnectionControlParam", connectionControlParam);
            //实例化所有的串口通讯
            foreach (var item in connectionControlParam.RS232Params)
            {
                if (RS232ControlList.Keys.Contains(item.controlName) != true)
                {
                    if (item.b_Use)
                    {
                        Cowain_AutoMotion.RS232 Scanner = new Cowain_AutoMotion.RS232(item);
                        Scanner.COMPort_Connect();
                        RS232ControlList.Add(item.controlName, Scanner);
                    }
                }
            }
            //实例化所有的网口通讯
            foreach (var item in connectionControlParam.SocketParams)
            {
                if (SocketControlList.Keys.Contains(item.controlName) != true)
                {
                    if (item.b_Use)
                    {
                        if (item.b_IsClient)
                        {
                            ISocket socket = new  SocketClient(item.IP, item.port);
                            socket.Start();
                            SocketControlList.Add(item.controlName, socket);
                        }
                        //else
                        //{
                        //    ISocket socket = new SocketServer(item.IP, item.port);
                        //    socket.Start();
                        //    SocketControlList.Add(item.controlName, socket);
                        //}
                    }
                }
            }
        }
        public RS232 getRS232Control(EnumParam_ConnectionName connectionName)
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
    public class ConnectionControlParam : JsonHelper
    {
        [Category("1 串口通讯参数集合"), DisplayName("串口通讯所有集合"), Description("串口通讯所有集合")]
        public List<ComPortData> RS232Params { get; set; } = new List<ComPortData>();
        [Category("2 网口通讯参数集合"), DisplayName("网口通讯参数集合"), Description("网口通讯参数集合")]
        public List<SocketParam> SocketParams { get; set; } = new List<SocketParam>();
    }
    public class SocketParam
    {
        [Category("1 硬件启用"), DisplayName("true为启用，false为禁用"), Description("true为启用，false为禁用")]
        public bool b_Use { get; set; } = true;
        [Category("1 硬件的名称"), DisplayName("网口的名称,必须唯一"), Description("网口的名称,必须唯一")]
        public string controlName { get; set; } = "CCD1";
        [Category("2 IP地址"), DisplayName("IP地址"), Description("IP地址")]
        public string IP { get; set; } = "127.0.0.1";
        [Category("3 端口号"), DisplayName("端口号"), Description("端口号")]
        public int port { get; set; } = 9000;
        [Category("4 类型"), DisplayName("true是客户端，false是服务器"), Description("true是客户端，false是服务器")]
        public bool b_IsClient { get; set; } = true;
    }
}
