using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
namespace Chart
{
    /// <summary>
    ///  客户端与服务器之间的会话类 
    /// </summary>
  public  class Session:ICloneable
    {
        public enum ExitType
        {
            NormalExit,
            ExceptionExit
        }
        private string _id;

        private byte[] _datagram;

        private Socket _cliSock;

        private ExitType _exitType;
        /// <summary> 
        /// 返回会话的ID 
        /// </summary> 
        public string ID
        {
            get
            {
                return _id;
            }
        }

        /// <summary> 
        /// 存取会话的报文 
        /// </summary> 
        public byte[] Datagram
        {
            get
            {
                return _datagram;
            }
            set
            {
                _datagram = value;
            }
        }

        /// <summary> 
        /// 获得与客户端会话关联的Socket对象 
        /// </summary> 
        public Socket ClientSocket
        {
            get
            {
                return _cliSock;
            }
        }

        /// <summary> 
        /// 存取客户端的退出方式 
        /// </summary> 
        public ExitType TypeOfExit
        {
            get
            {
                return _exitType;
            }

            set
            {
                _exitType = value;
            }
        }

        /// <summary> 
        /// 构造函数 
        /// </summary> 
        /// <param name="cliSock">会话使用的Socket连接</param> 
        public Session(Socket cliSock)
        {
            _cliSock = cliSock;
            _id = cliSock.RemoteEndPoint.ToString();
        }

        public void Close()
        {
            _cliSock.Shutdown(SocketShutdown.Both);
            _cliSock.Close();
        }

        public object Clone()
        {
            Session newSession = new Session(_cliSock);
            newSession.Datagram = _datagram;
            newSession.TypeOfExit = _exitType;
            return newSession;
        }
    }
}
