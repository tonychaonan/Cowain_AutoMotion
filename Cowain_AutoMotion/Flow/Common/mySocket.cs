using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cowain_AutoMotion
{
    public enum SocketEnum
    {
        Client,
        Server
    }
    public interface ISocket
    {
        string strConnectionOK { get; set; }
        string StrBack { get; set; }
        void Start();
        void StopConnetct();
        bool SendMsg(string msg);
        SocketEnum socketType { get; }

    }
    public class SocketClient: ISocket
    {
        private Socket mysocket;
        public string strConnectionOK { get; set; } = "";
      //  public HandleAcceptMSG handleMSG;//当修改参数时重新给handleMSG赋值,防止函数执行不到最后一行
        public string StrBack { get; set; } = "";
        public SocketEnum socketType
        {
            get
            {
                return SocketEnum.Client;
            }
        }
        public Thread myThread;
        private string myIP;
        private int myPort;
        public SocketClient( string IP, int Port)
        {
         //   handleMSG = myHandleMSG;
            myIP = IP;
            myPort = Port;
        }
        public void Start()
        {
            if (myThread == null)
            {
                myThread = new Thread(mySocketClienSet);
                myThread.IsBackground = true;
                myThread.Start();
            }
            else
            {
                myThread.Abort();
                Thread.Sleep(10);
                myThread = new Thread(mySocketClienSet);
                myThread.IsBackground = true;
                myThread.Start();
            }
        }
        public void mySocketClienSet(object str)
        {
            mysocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //连接服务器       
            try
            {
                mysocket.Connect(IPAddress.Parse(myIP), myPort);
                strConnectionOK = "OK";
            }
            catch
            {
                try
                {
                    mysocket.Connect(IPAddress.Parse(myIP), myPort);
                    strConnectionOK = "OK";
                }
                catch
                {
                    strConnectionOK = "NG";
                }
            }
            byte[] data = new byte[1024 * 1024];
            while (true)
            {
                Thread.Sleep(1);
                //   getMSG = "";
                int len = 0;
                try
                {
                    len = mysocket.Receive(data, 0, data.Length, SocketFlags.None);
                }
                catch
                {
                    //服务器异常退出
                    StopConnetct();
                   // handleMSG.ErrMSG("服务器异常退出");
                    strConnectionOK = "NG";
                    break;
                }
                if (len <= 0)
                {
                    //程序正常退出
                    StopConnetct();
                   // handleMSG.ErrMSG("服务器正常退出");
                    strConnectionOK = "NG";
                    break;
                }
                else
                {
                    StrBack = Encoding.Default.GetString(data, 0, len);
                  //  handleMSG.HandleMSGAndSendToServer(StrBack);
                }
            }
        }
        public void StopConnetct()
        {
            try
            {
                if (mysocket.Connected)
                {
                    mysocket.Shutdown(SocketShutdown.Both);
                    mysocket.Close(100);
                }
            }
            catch
            {

            }
            strConnectionOK = "NG";
        }
        public bool SendMsg(string sendMSg)
        {
            bool result = false;
            try
            {
                byte[] data1 = Encoding.Default.GetBytes(sendMSg);
                mysocket.Send(data1, 0, data1.Length, SocketFlags.None);
                result = true;
            }
            catch
            {
                strConnectionOK = "NG";

            }
            return result;
        }
    }
    #region  //旧版服务器禁掉
    //public class SocketServer:ISocket
    //{
    //    private Socket ClientProxSocket = null;
    //    public string strConnectionOK { get; set; } = "";
    //    public bool b_sendMSG;
    //    private Socket mysocket;
    //    public Thread myThread;
    //    public string StrBack { get; set; } = "";
    //    private string IP;
    //    private int Port;
    //    public SocketEnum socketType
    //    {
    //        get
    //        {
    //            return SocketEnum.Server;
    //        }
    //    }
    //    public SocketServer(string ip, int port)
    //    {
    //        IP = ip;
    //        Port = port;
    //    }
    //    public void Start()
    //    {
    //        myThread = new Thread(ReceiveDateFromClient);
    //        myThread.IsBackground = true;
    //        myThread.Start();
    //    }
    //    private void ReceiveDateFromClient()
    //    {
    //        mysocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    //        mysocket.Bind(new IPEndPoint(IPAddress.Parse(IP), Port));
    //        mysocket.Listen(10);
    //        while (true)
    //        {
    //            Thread.Sleep(1);
    //            while (true)
    //            {
    //                Thread.Sleep(1);
    //                ClientProxSocket = mysocket.Accept();
    //                if (ClientProxSocket != null)//判断有没有客户端连接
    //                {
    //                   // handleMSG.ErrMSG("客户端连接上了");
    //                    strConnectionOK = "OK";
    //                    break;
    //                }
    //                else
    //                {
    //                  //  handleMSG.ErrMSG("客户端断开连接");
    //                    strConnectionOK = "NG";
    //                }
    //            }
    //            byte[] data = new byte[1024 * 1024];
    //            while (true)
    //            {
    //                Thread.Sleep(1);
    //                StrBack = "";
    //                int len = 0;
    //                try
    //                {
    //                    len = ClientProxSocket.Receive(data, 0, data.Length, SocketFlags.None);
    //                }
    //                catch
    //                {
    //                 //   handleMSG.ErrMSG("客户端非正常退出");
    //                    StopConnetct();
    //                    strConnectionOK = "NG";
    //                    break;
    //                }
    //                if (len <= 0)
    //                {
    //                    //程序正常退出
    //                  //  handleMSG.ErrMSG("客户端正常退出");
    //                    StopConnetct();
    //                    strConnectionOK = "NG";
    //                    break;
    //                }
    //                else
    //                {
    //                    StrBack = Encoding.Default.GetString(data, 0, len);
    //                }
    //             //   handleMSG.HandleMSGAndSendToServer(StrBack);
    //            }
    //        }

    //    }
    //    public void StopConnetct()
    //    {
    //        try
    //        {
    //            if (ClientProxSocket.Connected)
    //            {
    //                ClientProxSocket.Shutdown(SocketShutdown.Both);
    //                ClientProxSocket.Close(100);
    //            }
    //        }
    //        catch
    //        {

    //        }
    //        strConnectionOK = "NG";
    //    }
    //    /// <summary>
    //    /// send MSG to Client
    //    /// </summary>
    //    /// <param name="MSG"></param>
    //    /// <returns></returns>
    //    public bool SendMsg(string MSG)
    //    {
    //        bool result = false;
    //        try
    //        {
    //            if (ClientProxSocket.Connected)
    //            {
    //                byte[] data = Encoding.Default.GetBytes(MSG);
    //                ClientProxSocket.Send(data, 0, data.Length, SocketFlags.None);
    //                strConnectionOK = "OK";
    //                result = true;
    //            }
    //            else
    //            {
    //                StopConnetct();
    //                strConnectionOK = "NG";
    //            }
    //        }
    //        catch
    //        {
    //            StopConnetct();
    //            strConnectionOK = "NG";
    //        }
    //        return result;
    //    }
    //}
    #endregion

    /// <summary>
    /// 服务器的类
    /// </summary>
    public class SocketSever
    {
        public SocketSever(string ip, string port)
        {
            IPAddress.TryParse(ip, out _ip);
            int.TryParse(port, out _port);
            Start();
        }
        private IPAddress _ip;
        private int _port;
        private TcpListener _listener;
        private TcpClient _client;
        private NetworkStream stream;
        private int _ReadTimeOut = 3000;
        private string _receivedMessage = "";
        /// <summary>
        /// 接收到消息时发生
        /// </summary>
        public event Action<string> ReceiveMessageEvent;

        private void Start()
        {
            _listener = new TcpListener(IPAddress.Any, _port);
            _listener.Start();
            Connect();
            Receive();
        }
        /// <summary>
        /// 读取超时时间
        /// </summary>
        public int ReadTimeOut
        {
            get { return _ReadTimeOut; }
            set { _ReadTimeOut = value; }
        }
        public string ReceiveMessage
        {
            get { return _receivedMessage; }
        }
        /// <summary>
        /// 向客户端发送消息
        /// </summary>
        /// <param name="msg"></param>
        public void Send(string msg)
        {
            try
            {
                _receivedMessage = "";
                byte[] buffer = Encoding.Default.GetBytes(msg);
                stream.WriteTimeout = 5000;
                stream.Write(buffer, 0, buffer.Length);
            }
            catch (Exception)
            {

            }

        }
        /// <summary>
        /// 从客户端接收的消息
        /// </summary>
        private void Receive()
        {
            Task.Run(() =>
            {
                try
                {
                    while (true)
                    {
                        Thread.Sleep(10);
                        if (_client != null&& stream != null)
                        {
                            if (stream.DataAvailable)
                            {
                                byte[] buffer = new byte[1024 * 1024];
                                stream.Read(buffer, 0, buffer.Length);
                                _receivedMessage = Encoding.Default.GetString(buffer).Trim('\0');
                                ReceiveMessageEvent?.Invoke(_receivedMessage);
                            }
                        }

                    }
                }
                catch (Exception)
                {


                }

            }
            );
        }

        private void Connect()
        {
            Task.Run(async () =>
            {
                while (_listener != null)
                {
                    Thread.Sleep(100);
                    _client = await _listener.AcceptTcpClientAsync();
                    string ip = _client.Client.RemoteEndPoint.ToString().Split(':')[0];
                    //if(ip!=_ip.ToString())
                    //{
                    //    _client.Close();
                    //    continue;
                    //}
                    stream = _client.GetStream();
                    stream.ReadTimeout = _ReadTimeOut;
                }
            });
        }
        public void Close()
        {
            _listener.Stop();
        }
    }



    public abstract class HandleAcceptMSG
    {
        /// <summary>
        /// Handle the getMSG 
        /// </summary>
        /// <param name="getMSG">getMSG from the server</param>
        /// <returns>Send to server MSG</returns>
        public abstract string HandleMSGAndSendToServer(string getMSG);
        /// <summary>
        /// Handle the ErrStr 
        /// </summary>
        /// <param name="ErrStr">ErrStr</param>
        public abstract void ErrMSG(string ErrStr);
    }
}
