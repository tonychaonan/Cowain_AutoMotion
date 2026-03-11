using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chart
{
    public delegate void NetEvent(object sender, NetEventArgs e);
  public  class NetEventArgs:EventArgs
    {
        /// <summary>
        ///客户端ip信息
        /// </summary>
        public Session Client
        {
            get
            {
                return _client;
            }

        }
        /// <summary>
        /// 服务器ip
        /// </summary>
        public string ServeIp
        {
            get
            {
                return _ip;
            }
        }
        private string _ip;
        private Session _client;
        public NetEventArgs(Session client,string ServeIp)
        {
            if (null == client)
            {
                throw (new ArgumentNullException());
            }
            _ip = ServeIp;
            _client = client;
        }
    }
}
