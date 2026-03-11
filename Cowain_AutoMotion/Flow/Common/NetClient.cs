using Cowain_AutoMotion.Flow;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ToolTotal_1
{
    public class NetClientPort : ToolTotal.NetClient
    {
        public new delegate void SocketDelegate(string msgStr);
        //IP地址和端口号（用于服务器端）
        private IPEndPoint ServerInfo;
        //客户端运行的Socket
        public new Socket ClientSocket;
        //接收缓冲区大小
        private Byte[] MsgReceiveBuffer;
        //发送缓冲区大小
        private Byte[] MsgSendBuffer;
        public new bool connectOk;
        public new string RemoteStrIP { get; set; }
        public new Int32 RemotePort { get; set; }
        public new string SN = "";
        public new event SocketDelegate receiveDoneSocketEvent;
        public new event SocketDelegate waitConnectEvent;
        public new event SocketDelegate sendDone;
        public NetClientPort()
        {


            //客户端所使用的套接字对象
            MsgReceiveBuffer = new Byte[65535];
            MsgSendBuffer = new Byte[65535];

        }
        public new void Open(string strIP, Int32 port)
        {
            this.RemoteStrIP = strIP;
            this.RemotePort = port;
            if (!connectOk)
            {
                ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //实例化一个IP地址和端口号（用于服务器端）
                ServerInfo = new IPEndPoint(IPAddress.Parse(this.RemoteStrIP), this.RemotePort);
                try
                {
                    //通过“套接字”“根据IP地址和端口号”连接服务器
                    ClientSocket.Connect(ServerInfo);
                    if (waitConnectEvent != null)
                    {
                        waitConnectEvent("连接服务器 " + ServerInfo.ToString() + " 成功");
                        SaveNetClientLog("IP:" + RemoteStrIP + "Port:" + RemotePort + "   服务器连接成功！");
                    }
                    //从服务器端异步接收返回的消息（通过“ReceiveCallBack”方法异步接收消息）
                    ClientSocket.BeginReceive(MsgReceiveBuffer, 0, MsgReceiveBuffer.Length, 0, new AsyncCallback(ReceiveCallBack), null);
                    connectOk = true;
                    TCPStatic = true;

                }
                catch (Exception ex)
                {
                    if (waitConnectEvent != null)
                    {
                        waitConnectEvent("服务器" + ServerInfo.ToString() + "无响应");
                        SaveNetClientLog("IP:" + RemoteStrIP + "Port:" + RemotePort + ServerInfo.ToString() + "服务器 无响应");
                    }
                }
            }
        }

        public new void ReceiveCallBack(IAsyncResult AR)
        {
            //string str = "";
            try
            {

                //结束接收（返回接收数据的大小）
                int REnd = ClientSocket.EndReceive(AR);
                //MessageBox.Show(REnd.ToString());
                if (REnd == 0 && connectOk)
                {
                    connectOk = false;
                    TCPStatic = false;
                    //  ClientSocket.Shutdown(SocketShutdown.Both);
                    Thread.Sleep(10);
                    // ClientSocket.Disconnect(false);
                    ClientSocket.Close();
                    //  ClientSocket = null;
                    if (waitConnectEvent != null)
                    {
                        SaveNetClientLog("【接收到信息时，服务器断开了连接】" + StrBack);
                        return;
                    }
                }
                //显示所接收的消息
                //this.RecieveMsg.AppendText(Encoding.Unicode.GetString(MsgReceiveBuffer, 0, REnd));
                //触发事件

                if (receiveDoneSocketEvent != null)
                    receiveDoneSocketEvent(Encoding.Default.GetString(MsgReceiveBuffer, 0, REnd));
                               
                StrBack = Encoding.Default.GetString(MsgReceiveBuffer, 0, REnd);

                //if (StrBack.Contains("No response from SFC") || StrBack.Contains("err"))
                //{
                //    TCPStatic = false;
                //    connectOk = false;
                //}

                //if (StrBack.Contains("err"))
                //    StrBack = "MES反馈Error";
                //StrBack = "";
                //再次开始异步接收
                ClientSocket.BeginReceive(MsgReceiveBuffer, 0, MsgReceiveBuffer.Length, 0, new AsyncCallback(ReceiveCallBack), null);
                //TCPStatic = true;


                SaveNetClientLog("【接收到信息，  字符串内容】"+ StrBack);
               

            }
            catch
            {
                TCPStatic = false;
                connectOk = false;                
                StrBack = "err";
                SaveNetClientLog("【接收到信息，字符串内容】" + StrBack);
            }

        }
        public new string StrBack = "";
        public new bool TCPStatic = false;
        public new void StopConnect()
        {
            if (connectOk && ClientSocket != null)
            {
                connectOk = false;
                TCPStatic = false;
                //ClientSocket.Send(Encoding.Unicode.GetBytes(this.txtNickName.Text + "离开了房间！\n"));
                //ClientSocket.Shutdown(SocketShutdown.Both);
                Thread.Sleep(10);
                // ClientSocket.Disconnect(false);
                ClientSocket.Close();
                //    ClientSocket = null;
                if (waitConnectEvent != null)
                    waitConnectEvent(ServerInfo.ToString() + " 断开了连接！\n");
            }


        }

        public new void SendMsg(string sendMsg)
        {
            StrBack = "";
            if (sendMsg.Length == 0)
            {
                if (waitConnectEvent != null)
                {
                    SaveNetClientLog("【发送信息，字符串内容为空】" + sendMsg);
                    return;
                }
            }
            //MsgSendBuffer = Encoding.Default.GetBytes(sendMsg);
            //如果已连接则发送消息
            if (ClientSocket.Connected && connectOk)
            {
                //发送消息（同时异步调用“ReceiveCallBack”方法接收数据）
                ClientSocket.Send(Encoding.Default.GetBytes(sendMsg), 0, sendMsg.Length, 0);
                SaveNetClientLog("【发送信息，    字符串内容】" + sendMsg);

                if (sendDone != null)
                {
                    sendDone("Ok");
                }


            }
            else
            {
                TCPStatic = false;
                StrBack = "err";
                //WriteErrorLog("当前与服务器断开连接,无法发送信息！");
                if (waitConnectEvent != null)
                    SaveNetClientLog("【当前与服务器断开连接,无法发送信息！】" + sendMsg);/* waitConnectEvent("");*/
            }
        }

        public new void SendMsg2(string sendMsg)//字符串以16进制发送
        {
            if (sendMsg.Length == 0)
            {
                if (waitConnectEvent != null)
                    waitConnectEvent("发送内容不能为空");
                return;
            }
            //MsgSendBuffer = Encoding.Default.GetBytes(sendMsg);
            //如果已连接则发送消息
            if (ClientSocket.Connected && connectOk)
            {
                //发送消息（同时异步调用“ReceiveCallBack”方法接收数据）
                //    ClientSocket.Send(Encoding.Default.GetBytes(sendMsg), 0, sendMsg.Length, 0);

                byte[] array = HexStringToByteArray(sendMsg);
                //  ClientSocket[i].Send(Encoding.Default.GetBytes(array), 0, array.Length, 0);
                ClientSocket.Send(array, 0, array.Length, 0);


                //      WriteErrorLog("发送的字符串内容---" + sendMsg);
                if (sendDone != null)
                {
                    sendDone("Ok");
                }
                //TCPStatic = true;
            }
            else
            {
                TCPStatic = false;
                //WriteErrorLog("当前与服务器断开连接,无法发送信息！");
                if (waitConnectEvent != null)
                    waitConnectEvent("当前与服务器断开连接,无法发送信息！");
            }
        }

        public new static byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }

        private static void WriteErrorLog(string msg)
        {
            string fileName = System.AppDomain.CurrentDomain.BaseDirectory + "Log\\MES " + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            FileStream fs = new FileStream(fileName, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("[" + DateTime.Now.ToString() + "] 日志内容：" + msg);
            sw.Close();
            fs.Close();
        }

        object locker = new object();
        public void SaveNetClientLog(string mes, bool iswrite = true)
        {
            try
            {
                lock (locker)
                {
                    string fileName;
                    fileName = string.Format("{0}.txt", DateTime.Now.ToString("yyyy_MM_dd"));
                    string outputPath = @"D:\DATA\TCP通信Log";
                    if (!Directory.Exists(outputPath))
                    {
                        Directory.CreateDirectory(outputPath);
                    }
                    string fullFileName = Path.Combine(outputPath, fileName);
                    System.IO.FileStream fs;
                    //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
                    StreamWriter sw;
                    if (!File.Exists(fullFileName))
                    {
                        fs = new System.IO.FileStream(fullFileName, System.IO.FileMode.Append, System.IO.FileAccess.Write, FileShare.Read);
                        sw = new StreamWriter(fs, System.Text.Encoding.Default);
                        if (iswrite)
                            sw.WriteLine(DateTime.Now.ToString("HH:mm:ss:fff") + "    " + mes);
                        else
                            sw.WriteLine(DateTime.Now.ToString("HH:mm:ss:fff") + "    " + mes);
                        sw.Close();
                        fs.Close();

                    }
                    else
                    {
                        fs = new System.IO.FileStream(fullFileName, System.IO.FileMode.Append, System.IO.FileAccess.Write, FileShare.Read);
                        //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
                        sw = new StreamWriter(fs, System.Text.Encoding.Default);
                        if (iswrite)
                            sw.WriteLine(DateTime.Now.ToString("HH:mm:ss:fff") + "    " + mes);
                        else
                            sw.WriteLine(DateTime.Now.ToString("HH:mm:ss:fff") + "    " + mes);
                        sw.Close();
                        fs.Close();
                    }
                }
            }
            catch
            {

            }
        }
    }
}
