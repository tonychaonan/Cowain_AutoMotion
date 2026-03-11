using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chart
{
    /// <summary>
    /// 服务器类
    /// </summary>
  public  class TCPServerHelper
    {

       // private  Dictionary<string, Socket> dicsocket = new Dictionary<string, Socket>();
        /// <summary> 
        /// 默认的服务器最大连接客户端端数据 
        /// </summary> 
        public const int DefaultMaxClient = 100;

        /// <summary> 
        /// 接收数据缓冲区大小64K 
        /// </summary> 
        public const int DefaultBufferSize = 4 * 1024 * 1024;

        /// <summary> 
        /// 最大数据报文大小 
        /// </summary> 
        public const int MaxDatagramSize = 4 * 1024 * 1024;

        ///// <summary> 
        ///// 报文解析器 
        ///// </summary> 
        //private DatagramResolver _resolver;

        /// <summary> 
        /// 通讯格式编码解码器 
        /// </summary> 
        private Coder _coder;

        /// <summary>
        /// 服务器程序监听的IP地址
        /// </summary>
        public IPAddress _serverIP;
        /// <summary> 
        /// 服务器程序使用的端口 
        /// </summary> 
        private ushort _port;

        /// <summary> 
        /// 服务器程序允许的最大客户端连接数 
        /// </summary> 
        private ushort _maxClient;

        /// <summary> 
        /// 服务器的运行状态 
        /// </summary> 
        private bool _isRun;

        /// <summary> 
        /// 接收数据缓冲区 
        /// </summary> 
        private byte[] _recvDataBuffer;

        /// <summary> 
        /// 服务器使用的异步Socket类, 
        /// </summary> 
        private Socket _svrSock;

        /// <summary> 
        /// 保存所有客户端会话的哈希表 
        /// </summary> 
        private Hashtable _sessionTable;

        /// <summary> 
        /// 当前的连接的客户端数 
        /// </summary> 
        private ushort _clientCount;

        /// <summary>
        /// 服务器收到的数据
        /// </summary>
        private string _receivedData;
        /// <summary>
        /// 客户端连接
        /// </summary>
        public event NetEvent ClientConnEvent;
        /// <summary>
        /// 客户端关闭
        /// </summary>
        public event NetEvent ClientCloseEvent;

        /// <summary> 
        /// 服务器已经满事件 
        /// </summary> 
        public event NetEvent ServerFullEvent;

        /// <summary> 
        /// 服务器接收到数据事件 
        /// </summary> 
        public event NetEvent RecvDataEvent;
        public string GetIp()
        {
            return _serverIP.ToString();
        }
        public TCPServerHelper(IPAddress serverIP, ushort port, ushort maxClient)
        {
            _serverIP = serverIP;
            _port = port;
            _maxClient = maxClient;
            _coder = new Coder(Coder.EncodingMothord.Default);
            _isRun = false;
        }


        // <summary> 
        /// 构造函数(默认使用Default编码方式和DefaultMaxClient(100)个客户端的容量) 
        /// </summary> 
        /// <param name="port">服务器端监听的端口号</param> 
        public TCPServerHelper(IPAddress serverIP, ushort port)
            : this(serverIP, port, DefaultMaxClient)
        {
        }

        public Hashtable SessionTable
        {
            get
            {
                return _sessionTable;
            }
        }
        /// <summary> 
        /// 服务器运行状态 
        /// </summary> 
        public bool IsRun
        {
            get
            {
                return _isRun;
            }
        }

        /// <summary>
        /// 服务器收到的数据
        /// </summary>
        public string ReceivedData
        {
            get
            {
                return _receivedData;
            }
        }
        /// <summary>
        /// 服务器地址
        /// </summary>
        public IPAddress ServerIP
        {
            get
            {
                return _serverIP;
            }
        }
        /// <summary>
        /// 服务器端口
        /// </summary>
        public ushort Port
        {
            get
            {
                return _port;
            }
        }

      
        /// <summary> 
        /// 启动服务器程序,开始监听客户端请求 
        /// </summary> 
        public void Start()
        {
            try
            {
                
                _sessionTable = new Hashtable(53);
                _recvDataBuffer = new byte[DefaultBufferSize];
                //创建socket
                _svrSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //绑定端口
                IPEndPoint iep = new IPEndPoint(_serverIP, _port);
                _svrSock.Bind(iep);

                //开始监听
                _svrSock.Listen(5);

                //开始异步接收客户端数据
                _svrSock.BeginAccept(new AsyncCallback(AcceptConn), _svrSock);
                _isRun = true;
            }
            catch 
            {
                _isRun = false;


            }
            
        }
        public  void Stop()
        {
            if (!_isRun)
            {
                throw (new ApplicationException("SocketServer已经停止"));
            }

            //这个条件语句，一定要在关闭所有客户端以前调用 
            //否则在EndConn会出现错误 
            _isRun = false;

            //关闭数据连接,负责客户端会认为是强制关闭连接 
            if (_svrSock.Connected)
            {
                _svrSock.Shutdown(SocketShutdown.Both);
            }

            foreach (Session client in _sessionTable.Values)
            {
                client.Close();
            }

            _sessionTable.Clear();

            //清理资源 
            _svrSock.Close();

            _sessionTable = null;

        }

        
        private void AcceptConn(IAsyncResult iar)
        { 
            //如果服务器停止了服务,就不能再接收新的客户端 
            if (!_isRun)
            {
                return;
            }
            Socket oldserver = (Socket)iar.AsyncState;
            //获取通信socket
            Socket client = oldserver.EndAccept(iar);

            if (_clientCount == _maxClient)
            {
                if (ServerFullEvent != null)
                {
                    ServerFullEvent(this, new NetEventArgs(new Session(client), GetIp()));
                }
            }
            else
            {   

                //将客户端添加的表格中  也可以通过dictionry来表示
                Session newSession = new Session(client);

                
                _sessionTable.Add(newSession.ID, newSession);
                //客户端引用计数+1 
                _clientCount++;
                              
                client.BeginReceive(_recvDataBuffer,0, _recvDataBuffer.Length,SocketFlags.None,new AsyncCallback(ReceiveData),client);


                //新的客户段连接,发出通知 
                if (ClientConnEvent != null)
                {
                    ClientConnEvent(this, new NetEventArgs(newSession, GetIp()));
                }
            }
            _svrSock.BeginAccept(new AsyncCallback(AcceptConn), _svrSock);
        }


        private void ReceiveData(IAsyncResult iar)
        {
            Socket client = (Socket)iar.AsyncState;
            try
            {
                int r = client.EndReceive(iar);
                if (r == 0)
                {  
                    //正常的关闭 
                    closeClient(client, Session.ExitType.NormalExit);
                    return;
                }
                _receivedData = _coder.GetEncodingString(_recvDataBuffer, 0, r);

                //发布收到数据的事件 
                if (RecvDataEvent != null)
                {
                    string id =client.RemoteEndPoint.ToString();

                    Session sendDataSession = (Session)_sessionTable[id];
 
                   // Debug.Assert(sendDataSession != null);
                    ICloneable copySession = (ICloneable)sendDataSession;
                    Session clientSession = (Session)copySession.Clone();
                    clientSession.Datagram = _recvDataBuffer;
                    RecvDataEvent(this, new NetEventArgs(clientSession, GetIp()));
                }
                //继续接收来自来客户端的数据 
                client.BeginReceive(_recvDataBuffer, 0, _recvDataBuffer.Length, SocketFlags.None,
                    new AsyncCallback(ReceiveData), client);

            }
            catch (SocketException ex)
            {

                //客户端退出 
                if (10054 == ex.ErrorCode)
                {
                    //客户端强制关闭 
                    closeClient(client, Session.ExitType.ExceptionExit);
                }
            }
            catch (ObjectDisposedException ex)
            {
                //这里的实现不够优雅 
                //当调用CloseSession()时,会结束数据接收,但是数据接收 
                //处理中会调用int recv = client.EndReceive(iar); 
                //就访问了CloseSession()已经处置的对象 
                //我想这样的实现方法也是无伤大雅的. 
                if (ex != null)
                {
                    ex = null;
                    //DoNothing; 
                }
            }
        }

       
        public void closeClient(Socket client, Session.ExitType exitType)
        {
            //  Debug.Assert(client != null);

            //查找该客户端是否存在,如果不存在,抛出异常 
            string id = client.RemoteEndPoint.ToString();

            Session closeClient= (Session)_sessionTable[id];
         

    

            if (closeClient != null)
            {
                if (closeClient != null)
                {
                    closeClient.TypeOfExit = exitType;
                    closeClient.Datagram = null;

                    _sessionTable.Remove(closeClient.ID);

                    _clientCount--;

                    //客户端强制关闭链接 
                    if (ClientCloseEvent != null)
                    {
                        ClientCloseEvent(this, new NetEventArgs(closeClient,GetIp()));
                    }

                    closeClient.Close();
                }
            }
            else
            {
               // throw (new ApplicationException("需要关闭的Socket对象不存在"));
            }
        }

        public void SendText(string ip, string datagram)
        {
            try
            {
                if (ip == null)
                    return;
                Session sendDataSession = (Session)_sessionTable[ip];
                //获得数据编码 
                byte[] data = _coder.GetTextBytes(datagram);

                sendDataSession.ClientSocket.BeginSend(data, 0, data.Length, SocketFlags.None,
                    new AsyncCallback(SendDataEnd), sendDataSession.ClientSocket);

            //    Common.Util.Notify(datagram);

            }
            catch 
            {

              
            }
           
        }
        /// <summary> 
        /// 发送数据完成处理函数 
        /// </summary> 
        /// <param name="iar">目标客户端Socket</param> 
        protected virtual void SendDataEnd(IAsyncResult iar)
        {
            Socket client = (Socket)iar.AsyncState;

            int sent = client.EndSend(iar);
        }

    }
}
