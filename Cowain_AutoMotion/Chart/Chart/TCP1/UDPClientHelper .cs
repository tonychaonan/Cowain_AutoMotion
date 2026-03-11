using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chart
{
  public  class UDPClientHelper
    {
        private Session _session;

        private bool _isConnected = false;

        /// <summary> 
        /// 接收数据缓冲区大小64K 
        /// </summary> 
        public const int DefaultBufferSize = 4 * 1024 * 1024;
        /// <summary> 
        /// 接收数据缓冲区 
        /// </summary> 
        private byte[] _recvDataBuffer = new byte[DefaultBufferSize];
        /// <summary>
        /// 客户端收到的数据
        /// </summary>
        private string _receivedData;

        /// <summary> 
        /// 编码解码器 
        /// </summary> 
        private Coder _coder;

        /// <summary> 
        /// 默认构造函数,使用默认的编码格式 
        /// </summary> 
        public UDPClientHelper()
        {
            _coder = new Coder(Coder.EncodingMothord.Default);
        }

        /// <summary>
        /// 客户端收到的数据
        /// </summary>
        public string ReceivedData
        {
            get
            {
                return _receivedData;
            }

        }

        public Coder Corder
        {
            get
            {
                return _coder;
            }

        }
        /// <summary> 
        /// 已经连接服务器事件 
        /// </summary> 
        public event NetEvent ConnectedServerEvent;

        /// <summary> 
        /// 连接断开事件 
        /// </summary> 
        public event NetEvent DisConnectedServerEvent;

        /// <summary> 
        /// 接收到数据报文事件 
        /// </summary> 
        public event NetEvent ReceivedDatagramEvent;

        public event Action FailConnectedServerEvent;
        /// <summary> 
        /// 发送数据报文完成事件 
        /// </summary> 
        public event NetEvent DataSendedEvent;
        //连接服务器
        public void Connect(string ip,int port)
        {
            if (_isConnected)
            {
               //已经连接，断开
            }
            //创建通信socket
            Socket newsock = new Socket(AddressFamily.InterNetwork,SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint iep = new IPEndPoint(IPAddress.Parse(ip), port);

            //连接
            newsock.BeginConnect(iep,new AsyncCallback(Connected), newsock);

        }

        private void Connected(IAsyncResult iar)
        {
            try
            {
                Socket socket = (Socket)iar.AsyncState;
                socket.EndConnect(iar);
                _session = new Session(socket);
                _isConnected = true;
                //触发连接建立事件 
                if (ConnectedServerEvent != null)
                {
                    ConnectedServerEvent(this, new NetEventArgs(_session, ""));
                }
                socket.BeginReceive(_recvDataBuffer, 0, DefaultBufferSize, SocketFlags.None, new AsyncCallback(RecvData), socket);

            }
            catch (SocketException ex)
            {
              //  Util.WriteLog(this.GetType(), ex);
                if (FailConnectedServerEvent != null)
                    {
                        FailConnectedServerEvent();
                    }

               

            }
            

        }
        private void RecvData(IAsyncResult iar)
        {
            Socket socket = (Socket)iar.AsyncState;
       

            try
            {
                int r = socket.EndReceive(iar);
                //正常退出
                if (r == 0)
                {
                    _session.TypeOfExit = Session.ExitType.NormalExit;

                    if (DisConnectedServerEvent != null)
                    {
                        DisConnectedServerEvent(this, new NetEventArgs(_session, ""));
                    }
                    return;
                }
              //  _receivedData = _coder.GetEncodingString(_recvDataBuffer, 0, r);
                StringBuilder strBuider = new StringBuilder();
                for (int index = 0; index < r; index++)
                {
                    strBuider.Append(((int)_recvDataBuffer[index]).ToString("X2"));
                }
                _receivedData = strBuider.ToString();

                //通过事件发布收到的报文 
                if (ReceivedDatagramEvent != null)
                {
                    //将类赋值给接口
                    ICloneable copySession = _session;
                    //
                    Session clientSession = (Session)copySession.Clone();
                    clientSession.Datagram = new byte[r];
                    clientSession.Datagram = _recvDataBuffer;
                    ReceivedDatagramEvent(this, new NetEventArgs(clientSession, ""));
                }//end of if(ReceivedDatagram != null) 

                //继续接收数据 
                _session.ClientSocket.BeginReceive(_recvDataBuffer, 0, DefaultBufferSize, SocketFlags.None,
                    new AsyncCallback(RecvData), _session.ClientSocket);
            }
            catch (SocketException ex)
            {
                //客户端退出 
                if (10054 == ex.ErrorCode)
                {
                    //服务器强制的关闭连接，强制退出 
                    _session.TypeOfExit = Session.ExitType.ExceptionExit;

                    if (DisConnectedServerEvent != null)
                    {
                        DisConnectedServerEvent(this, new NetEventArgs(_session,""));
                    }
                }
                else
                {
                    ////产生发送错误事件 
                    //this.ErrorSocket((SocketError)ex.ErrorCode);
                }
            }
            //对已经释放的对象的操作
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


        public virtual void SendText(string datagram)
        {
            if (datagram.Length == 0)
            {
                return;
            }

            if (!_isConnected)
            {
                throw (new ApplicationException("没有连接服务器，不能发送数据"));
            }
            try
            {
                //获得报文的编码字节 
                SocketError ErrorCode = new SocketError();
                byte[] data = _coder.GetTextBytes(datagram);
                _session.ClientSocket.BeginSend(data,0, data.Length, SocketFlags.None, out ErrorCode, new AsyncCallback(SendDataEnd), _session.ClientSocket);
                if (ErrorCode != SocketError.Success)
                {
                    //产生发送错误事件
                   // this.ErrorSocket(ErrorCode);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual void SendTextHEX(string datagram)
        {
          
            if (datagram.Length == 0)
            { 
                return;
            }

            if (!_isConnected)
            {
                
            }
            try
            {
                //获得报文的编码字节 
                SocketError ErrorCode = new SocketError();
                byte[] data = HexStrTobyte(datagram);
                _session.ClientSocket.BeginSend(data, 0, data.Length, SocketFlags.None, out ErrorCode, new AsyncCallback(SendDataEnd), _session.ClientSocket);
                if (ErrorCode != SocketError.Success)
                {
                    //产生发送错误事件
                    // this.ErrorSocket(ErrorCode);
                }
            }
            catch (SocketException ex)
            {
               
            }
        }

        private byte[] HexStrTobyte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2).Trim(), 16);
            return returnBytes;
        }
        private void SendDataEnd(IAsyncResult iar)
        {
            try
            {
                Socket socket = (Socket)iar.AsyncState;
                int sent = socket.EndSend(iar);
                if (DataSendedEvent != null)
                {
                    DataSendedEvent(this, new NetEventArgs(_session, ""));
                }
            }
            catch (SocketException ex)
            {

            }
        
        }

        public virtual void Close()
        {
            if (!_isConnected)
            {
                return;
            }
            _session.Close();
            _session = null;
            _isConnected = false;
        }
    }
}
