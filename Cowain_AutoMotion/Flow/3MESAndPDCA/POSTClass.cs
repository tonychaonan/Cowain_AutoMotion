using Cowain_AutoMotion;
using Cowain_AutoMotion.Flow;
using Cowain_AutoMotion.Flow._2Work;
using Cowain_AutoMotion.Flow._3MESAndPDCA;
using Cowain_Form.FormView;
using Cowain_Machine;
using Cowain_Machine.Flow;
using DevExpress.XtraPrinting;
using Newtonsoft.Json;
using Sunny.UI.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ToolTotal_1;
using static ModuleTech.ReaderIPInfo_Ex;

namespace Post
{
    public enum CMDStep
    {
        用户登录,
        检查UOP,
        UC获取SN,
        检查SN类型,
        上传数据,
        提交过站,
        上传PDCA,
        waitTime,
        CheckPDCAReciveMSG,
        更新MESHashKey,
        检查HashKey,
        更新点检信息,
        Bobcat检查UOP,
        Bobcat过站,
    }
    public class MesData
    {
        public int station = 0;
        /// <summary>
        /// 当前步骤
        /// </summary>
        public CMDStep cmdStep;
        /// <summary>
        /// 是否上传成功
        /// </summary>
        public string Result;
        /// <summary>
        /// NG时返回的错误信息
        /// </summary>
        public string errMSG = "";
        /// <summary>
        /// 发送的全部信息
        /// </summary>
        public string sendMSG = "";
        /// <summary>
        /// 返回的全部信息
        /// </summary>
        public string returnStr = "";
        /// <summary>
        /// 要替换的值
        /// </summary>
        public Dictionary<string, string> setData = new Dictionary<string, string>();
        /// <summary>
        /// 返回的值
        /// </summary>
        public Dictionary<string, string> dataReturn = new Dictionary<string, string>();
        public MesData(int station1, CMDStep cmdStep1, string Result1, Dictionary<string, string> data1)
        {
            station = station1;
            cmdStep = cmdStep1;
            Result = Result1;
            setData = data1;
        }
    }
    public class POSTClass
    {
        private static object locker1 = new object();
        private static List<MesData> myListResult = new List<MesData>();
        private CMDStep currentCMDStep;
        /// <summary>
        /// 存储MES的指令
        /// </summary>
        private static Queue<MesData> myQueueCMD = new Queue<MesData>();
        /// <summary>
        /// 存储PDCA的指令
        /// </summary>
        private static Queue<MesData> myQueueCMDForPDCA = new Queue<MesData>();
        /// <summary>
        /// 线程退出
        /// </summary>
        public bool b_Close = false;
        private static POSTClass instace;
        public string Pdcaresult = "";
        HttpWebRequest Web = null;
        HttpWebResponse Res = null;
        /// <summary>
        ///延时
        /// </summary>
        public System.Timers.Timer POSTm_tmDelay = new System.Timers.Timer();
        Thread threadForMES;
        Thread threadForPDCA;
        //------------------------- 

        //mini参数-----------------------
        public static NetClientPort socket;

        //--------------------------------
        private POSTClass()
        {
            threadForMES = new Thread(CircleForMES);
            threadForMES.IsBackground = true;
            threadForMES.Start();
            threadForPDCA = new Thread(CircleForPDCA);
            threadForPDCA.IsBackground = true;
            threadForPDCA.Start();
            socket = new NetClientPort();
            string ip = MESDataDefine.MESLXData.StrMiniIP;
            string port = MESDataDefine.MESLXData.StrMiniPort;
            Task.Run(() =>
            {
                socket.Open(ip, Convert.ToInt32(port));
                socket.receiveDoneSocketEvent += SocketEvent;
            });
            POSTm_tmDelay = new System.Timers.Timer(500);
            POSTm_tmDelay.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent_DelayTimeOut);
        }
        public void SocketEvent(string msgStr)
        {
            Pdcaresult = msgStr.Trim();
            SaveDatePDCA("_PDCA反馈信息:" + msgStr + "\r\n");
        }
        public static POSTClass CreateInstance()
        {
            lock (locker1)
            {
                if (instace == null)
                {
                    instace = new POSTClass();
                }
                return instace;
            }
        }

        private void OnTimedEvent_DelayTimeOut(object source, System.Timers.ElapsedEventArgs e)
        {
            POSTm_tmDelay.Enabled = false;
        }
        public static void AddCMD(int station, CMDStep cmdStep, Dictionary<string, string> data)
        {
            lock (locker1)
            {
                MesData mesData = new MesData(station, cmdStep, "", data);
                if (mesData.cmdStep == CMDStep.上传PDCA)
                {
                    myQueueCMDForPDCA.Enqueue(mesData);
                }
                else
                {
                    myQueueCMD.Enqueue(mesData);

                }
                bool b_Exist = false;
                for (int i = 0; i < myListResult.Count; i++)
                {
                    if (myListResult[i].station == station && myListResult[i].cmdStep == cmdStep)
                    {
                        myListResult[i].Result = "";
                        myListResult[i].errMSG = "";
                        myListResult[i].returnStr = "";
                        myListResult[i].sendMSG = "";
                        myListResult[i].dataReturn.Clear();
                        b_Exist = true;
                        break;
                    }
                }
                if (b_Exist == false)
                {
                    myListResult.Add(mesData);
                }
            }
        }
        /// <summary>
        /// 如果拿到结果则返回true,
        /// </summary>
        public static MesData getResult(int station, CMDStep cmdStep)
        {
            lock (locker1)
            {
                for (int i = 0; i < myListResult.Count; i++)
                {
                    if (myListResult[i].station == station && myListResult[i].cmdStep == cmdStep)
                    {
                        return myListResult[i];
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 拿到发送的数据
        /// </summary>
        public static string getSendMSG(int station, CMDStep cmdStep)
        {
            string sendMSG = "";
            lock (locker1)
            {
                for (int i = 0; i < myListResult.Count; i++)
                {
                    if (myListResult[i].station == station && myListResult[i].cmdStep == cmdStep)
                    {
                        sendMSG = myListResult[i].sendMSG;
                        break;
                    }
                }
            }
            return sendMSG;
        }
        /// <summary>
        /// 拿到接收的数据
        /// </summary>
        public static string getReturnMSG(int station, CMDStep cmdStep)
        {
            string sendMSG = "";
            lock (locker1)
            {
                for (int i = 0; i < myListResult.Count; i++)
                {
                    if (myListResult[i].station == station && myListResult[i].cmdStep == cmdStep)
                    {
                        sendMSG = myListResult[i].returnStr;
                        break;
                    }
                }
            }
            return sendMSG;
        }
        /// <summary>
        /// 拿到Err的数据
        /// </summary>
        public static string getErrMSG(int station, CMDStep cmdStep)
        {
            string sendMSG = "";
            lock (locker1)
            {
                for (int i = 0; i < myListResult.Count; i++)
                {
                    if (myListResult[i].station == station && myListResult[i].cmdStep == cmdStep)
                    {
                        sendMSG = myListResult[i].errMSG;
                        break;
                    }
                }
            }
            return sendMSG;
        }
        public void clear()
        {
            lock (locker1)
            {
                myListResult.Clear();
                myQueueCMD.Clear();
            }
        }
        private void AddResult(MesData mesData, bool result)
        {
            lock (locker1)
            {
                for (int j = 0; j < myListResult.Count; j++)
                {
                    if (myListResult[j].station == mesData.station && myListResult[j].cmdStep == mesData.cmdStep)
                    {
                        if (result)
                        {
                            myListResult[j].Result = "OK";
                        }
                        else
                        {
                            myListResult[j].Result = "NG";
                        }
                        myListResult[j].sendMSG = mesData.sendMSG;
                        myListResult[j].returnStr = mesData.returnStr;
                        myListResult[j].errMSG = mesData.errMSG;
                        myListResult[j].dataReturn = mesData.dataReturn;
                    }
                }
            }
        }
        public void CircleForMES()
        {
            while (true)
            {
                if (b_Close)
                {
                    break;
                }
                Thread.Sleep(1);
                MesData mesData = null;
                if (myQueueCMD.Count > 0)
                {
                    lock (locker1)
                    {
                        mesData = myQueueCMD.Dequeue();
                    }
                }
                else
                {
                    //return;
                    continue;

                }
                //如果屏蔽MES，则直接返回true
                //if (MDataDefine.m_bMESMode)
                //{
                //    AddResult(mesData, true);
                //    continue;
                //}
                //如果本地文件不存在，则直接返回true

                if (MESDataDefine.myMesMsg[mesData.station].ContainsKey(mesData.cmdStep.ToString()) != true)
                {
                    AddResult(mesData, true);
                    continue;
                }
                //if (mesData.cmdStep != CMDStep.用户登录)
                //{
                //    if (MESDataDefine.CMD == "")
                //    {
                //        continue;
                //    }
                //}
                CMDStep currentCMDStep = mesData.cmdStep;
                string mesStr = "";
                switch (currentCMDStep)
                {
                    default:
                        mesStr = formatMESData(mesData);//对要发送的字符串进行截取，以适应一个文件传多次的情况
                        string[] mesStrs = mesStr.Split(new string[] { "{\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                        string returnStr = "";
                        for (int i = 0; i < 1; i++)
                        {
                            if (mesStrs[i].Length > 1)
                            {
                                mesStrs[i] = "{\r\n" + mesStrs[i];
                                string str1 = mesData.cmdStep.ToString() + "\r\n" + "send\r\n" + mesStrs[i];
                                MachineDataDefine.loginstr = str1;
                                SaveDateMes(mesData.cmdStep.ToString() + "\r\n" + "send\r\n" + mesStrs[i]);
                                mesData.sendMSG = mesStrs[i];
                                try
                                {
                                    string getStr = push(mesStrs[i], ref mesData);
                                    returnStr += getStr;
                                    mesData.returnStr = returnStr;
                                    bool result = JudgeResult(getStr, ref mesData);
                                    SaveDateMes(mesData.cmdStep.ToString() + "\r\n" + "get\r\n" + getStr + "\r\n");
                                    if (result != true)
                                    {
                                        AddResult(mesData, result);
                                        break;
                                    }
                                    else if (i == mesStrs.Length - 1)
                                    {
                                        AddResult(mesData, result);
                                    }
                                }
                                catch
                                {

                                }
                            }
                        }
                        break;
                }
            }
        }
        public void CircleForPDCA()
        {
            while (true)
            {
                if (b_Close)
                {
                    break;
                }
                Thread.Sleep(1);
                MesData mesData = null;
                if (myQueueCMDForPDCA.Count > 0)
                {
                    lock (locker1)
                    {
                        mesData = myQueueCMDForPDCA.Dequeue();
                    }
                }
                else
                {
                    continue;
                }
                //如果本地文件不存在，则直接返回true
                if (MESDataDefine.myMesMsg[mesData.station].ContainsKey(mesData.cmdStep.ToString()) != true)
                {
                    AddResult(mesData, true);
                    continue;
                }
                string mesStr = "";
                string getStr = "";
                bool b_Circle = true;
                //if (mesData.cmdStep == CMDStep.上传PDCA)
                //{
                //    currentCMDStep = mesData.cmdStep;
                //}
                currentCMDStep = CMDStep.上传PDCA;
                while (b_Circle)
                {
                    switch (currentCMDStep)
                    {
                        case CMDStep.上传PDCA:
                            mesStr = formatMESData(mesData);
                            string s = mesData.cmdStep.ToString() + "\r\n" + "send\r\n" + mesStr;
                            MachineDataDefine.pdcastr = s;
                            SaveDatePDCA(mesData.cmdStep.ToString() + "\r\n" + "send\r\n" + mesStr);
                            mesData.sendMSG = mesStr;
                            if (socket.connectOk != true)
                            {
                                string ip = MESDataDefine.MESLXData.StrMiniIP;
                                string port = MESDataDefine.MESLXData.StrMiniPort;
                                socket.Open(ip, Convert.ToInt32(port));
                            }
                            socket.SendMsg(mesStr + "\r\n");
                            currentCMDStep = CMDStep.waitTime;
                            break;
                        case CMDStep.waitTime:
                            string strGet = socket.StrBack;
                            if (strGet.Length > 0)
                            {
                                getStr += strGet;
                                socket.StrBack = "";
                            }
                            if (getStr.Contains("@{}@") || getStr.Contains("err") || getStr.Contains("bad"))
                            {
                                currentCMDStep = CMDStep.CheckPDCAReciveMSG;
                            }

                            break;
                        case CMDStep.CheckPDCAReciveMSG://判断PDCA返回的值是否符合要求
                            mesData.returnStr = getStr;
                            SaveDatePDCA(mesData.cmdStep.ToString() + "\r\n" + "get\r\n" + getStr);
                            bool result = JudgeResult(mesData.returnStr, ref mesData);
                            AddResult(mesData, result);
                            b_Circle = false;
                            break;
                    }
                }
            }
        }
        private string getValue(MesData mesData, string mesStr)
        {
            string currentStr = mesStr;
            currentStr = currentStr.Replace('{', ' ');
            currentStr = currentStr.Replace('}', ' ');
            currentStr = currentStr.Replace('\\', ' ');
            currentStr = currentStr.Replace('"', ' ');
            currentStr = currentStr.Replace(',', ' ');
            currentStr = currentStr.Replace(':', ' ');
            currentStr = currentStr.Replace('\r', ' ');
            currentStr = currentStr.Replace('\n', ' ');
            currentStr = currentStr.Replace('@', ' ');
            currentStr = currentStr.Replace('$', ' ');
            currentStr = currentStr.Replace('=', ' ');
            string[] currentStrs = currentStr.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            List<string> myList = new List<string>();
            for (int i = 0; i < currentStrs.Length; i++)
            {
                if (currentStrs[i].Trim() != "")
                {
                    myList.Add(currentStrs[i]);
                }
            }
            for (int i = 0; i < myList.Count; i++)
            {
                foreach (var item in mesData.setData)
                {
                    if (myList[i] == item.Key)
                    {
                        mesStr = mesStr.Replace(myList[i], item.Value);
                    }
                }
            }
            return mesStr;
        }
        private string formatMESData(MesData mesData11)
        {
            string currentStr = MESDataDefine.myMesMsg[mesData11.station][mesData11.cmdStep.ToString()];
            string mesData = getValue(mesData11, currentStr);
            return mesData;
        }
        private string push(string mesStr, ref MesData mesData)
        {

            string str = "";
            try
            {
                if (mesData.cmdStep == CMDStep.用户登录 || mesData.cmdStep == CMDStep.UC获取SN || mesData.cmdStep == CMDStep.检查SN类型)
                {
                    Web = (HttpWebRequest)WebRequest.Create(MESDataDefine.MESLXData.GetCmdResURL);
                }
                else if (mesData.cmdStep == CMDStep.检查UOP)
                {
                    Web = (HttpWebRequest)WebRequest.Create(MESDataDefine.MESLXData.AssyCheckURL);
                }
                else if (mesData.cmdStep == CMDStep.上传数据)
                {
                    Web = (HttpWebRequest)WebRequest.Create(MESDataDefine.MESLXData.ColTestDataURL);
                }
                else if (mesData.cmdStep == CMDStep.提交过站)
                {
                    Web = (HttpWebRequest)WebRequest.Create(MESDataDefine.MESLXData.AssyGoURL);
                }
                byte[] data = Encoding.UTF8.GetBytes(mesStr);
                Web.Method = "POST";
                Web.ContentType = "application/json"; //"application/x-www-form-urlencoded";// Web.ContentType = "application/json";

                Web.ContentLength = data.Length;
                using (var stream = Web.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                Res = (HttpWebResponse)Web.GetResponse();

                using (StreamReader sr = new StreamReader(Res.GetResponseStream(), Encoding.UTF8))
                {
                    str = sr.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                lock (locker1)
                {
                    for (int i = 0; i < myListResult.Count; i++)
                    {
                        if (myListResult[i].station == mesData.station && myListResult[i].cmdStep == mesData.cmdStep)
                        {
                            myListResult[i].Result = "NG";
                            myListResult[i].errMSG = e.ToString();
                        }
                    }
                }
            }
            return str;
        }
        private bool JudgeResult(string currentStr1, ref MesData mesData)
        {
            bool b_Result = false;
            string currentStr = currentStr1;
            currentStr = currentStr.Replace('{', '^');
            currentStr = currentStr.Replace('}', '^');
            currentStr = currentStr.Replace('\\', '^');
            currentStr = currentStr.Replace('"', '^');
            currentStr = currentStr.Replace(',', '^');
            currentStr = currentStr.Replace(':', '^');
            currentStr = currentStr.Replace('\r', '^');
            currentStr = currentStr.Replace('@', '^');
            currentStr = currentStr.Replace('\n', '^');
            currentStr = currentStr.Replace(';', '^');
            currentStr = currentStr.Replace('=', '^');
            string[] currentStrs = currentStr.Split('^');
            List<string> myList = new List<string>();
            for (int i = 0; i < currentStrs.Length; i++)
            {
                if (currentStrs[i].Trim() != "")
                {
                    myList.Add(currentStrs[i]);
                }
            }
            for (int i = 0; i < myList.Count; i++)
            {
                switch (mesData.cmdStep)
                {
                    case CMDStep.用户登录:
                        if (myList[i] == "Result")
                        {
                            if (myList[i + 1] == "true")
                            {
                                mesData.Result = "OK";
                                b_Result = true;
                                Dictionary<string, string> dataReturn = new Dictionary<string, string>();
                                dataReturn.Add("Name", myList[i + 4]);
                                dataReturn.Add("Access_Level", myList[i + 6]);
                                dataReturn.Add("Function", myList[i + 8]);
                                mesData.dataReturn = dataReturn;
                            }
                            else
                            {
                                mesData.Result = "NG";
                                string err = currentStr;
                                mesData.errMSG = err;
                                b_Result = false;
                            }
                        }
                        break;
                    case CMDStep.UC获取SN:
                        if (myList[i] == "Result")
                        {
                            if (myList.Count > i + 4)
                            {
                                if (myList[i + 1] == "true" && myList[i + 4] != "")
                                {
                                    mesData.Result = "OK";
                                    //  MachineDataDefine.str = myList[3];
                                    b_Result = true;
                                    Dictionary<string, string> dataReturn = new Dictionary<string, string>();
                                    dataReturn.Add("SN", myList[i + 4]);
                                    mesData.dataReturn = dataReturn;
                                }
                                else
                                {

                                    mesData.Result = "NG";
                                    MachineDataDefine.str = myList[3];
                                    string err = currentStr;
                                    mesData.errMSG = err;
                                    b_Result = false;
                                }
                            }
                            else
                            {
                                mesData.Result = "NG";
                                MachineDataDefine.str = myList[3];
                                string err = currentStr;
                                mesData.errMSG = err;
                                b_Result = false;
                            }
                            //for (int j = 0; j < myList.Count; j++)
                            //{
                            //MachineDataDefine.str1= myList[0]+","+ myList[1]+","+myList[2]+":"+ myList[3];
                            //}

                        }
                        break;

                    case CMDStep.检查SN类型:
                        if (myList[i] == "Result")
                        {
                            if (myList.Count > i + 4)
                            {
                                if (myList[i + 1] == "true" && myList[i + 4] != "" && ((myList[i + 4].Contains("M") && MachineDataDefine.HardwareCfg.MaterialTypeEnum == MaterialType.M_SKU) || (myList[i + 4].Contains("E") && MachineDataDefine.HardwareCfg.MaterialTypeEnum == MaterialType.E_SKU) || (MachineDataDefine.HardwareCfg.MaterialTypeEnum == MaterialType.Normal)))
                                {
                                    mesData.Result = "OK";
                                    b_Result = true;

                                }
                                else
                                {

                                    mesData.Result = "NG";
                                    MachineDataDefine.str = myList[3];
                                    string err = currentStr;
                                    mesData.errMSG = err;
                                    b_Result = false;
                                }
                            }
                            else
                            {
                                mesData.Result = "NG";
                                MachineDataDefine.str = myList[3];
                                string err = currentStr;
                                mesData.errMSG = err;
                                b_Result = false;
                            }
                            //for (int j = 0; j < myList.Count; j++)
                            //{
                            //MachineDataDefine.str1 = myList[0] + "," + myList[1] + "," + myList[2] + ":" + myList[3];
                            //}

                        }
                        break;


                    case CMDStep.检查UOP:
                        if (myList[i] == "Result")
                        {
                            if (myList[i + 1] == "true")
                            {
                                mesData.Result = "OK";
                                b_Result = true;
                            }
                            else
                            {
                                mesData.Result = "NG";
                                MachineDataDefine.str = myList[3];
                                string err = currentStr;
                                mesData.errMSG = err;
                                b_Result = false;
                            }
                            //MachineDataDefine.str1 = myList[0] + "," + myList[1] + "," + myList[2] + ":" + myList[3];
                        }
                        break;
                    case CMDStep.上传数据:
                        if (myList[i] == "Result")
                        {
                            if (myList[i + 1] == "true")
                            {
                                mesData.Result = "OK";
                                b_Result = true;
                            }
                            else
                            {
                                mesData.Result = "NG";
                                MachineDataDefine.str = myList[3];
                                string err = currentStr;
                                mesData.errMSG = err;
                                b_Result = false;
                            }
                            //MachineDataDefine.str1 = myList[0] + "," + myList[1] + "," + myList[2] + ":" + myList[3];
                        }
                        break;
                    case CMDStep.提交过站:
                        if (myList[i] == "Result")
                        {
                            if (myList[i + 1] == "true")
                            {
                                mesData.Result = "OK";
                                b_Result = true;
                            }
                            else
                            {
                                MachineDataDefine.str = myList[3];
                                mesData.Result = "NG";
                                string err = currentStr;
                                mesData.errMSG = err;
                                b_Result = false;
                            }
                            //MachineDataDefine.str1 = myList[0] + "," + myList[1] + "," + myList[2] + ":" + myList[3];
                        }
                        break;
                    //case CMDStep.上传软件版本:
                    //    if (myList[i] == "Result")
                    //    {
                    //        if (myList[i + 1] == "OK")
                    //        {
                    //            b_Result = true;
                    //        }
                    //        else
                    //        {
                    //            int n = currentStr.IndexOf("Result");
                    //            string err = currentStr.Substring(n, currentStr.Length - n - 1);
                    //            mesData.errMSG = err;
                    //            b_Result = false;
                    //        }
                    //    }
                    //    break;
                    case CMDStep.上传PDCA:
                        if (currentStr.Length > 10)
                        {
                            if (currentStr.Contains("bad"))
                            {
                                mesData.Result = "NG";
                                //  MachineDataDefine.str = myList[3];
                                MachineDataDefine.str = myList[0] + "," + myList[1];
                                int index1 = currentStr.IndexOf("bad");
                                mesData.errMSG = currentStr.Substring(index1, currentStr.Length - index1 - 1);
                            }
                            else if (currentStr.Contains("err"))
                            {
                                mesData.Result = "NG";
                                // MachineDataDefine.str = myList[3];
                                MachineDataDefine.str = "err"; //myList[0] + "," + myList[1];
                                int index1 = currentStr.IndexOf("err");
                                mesData.errMSG = currentStr.Substring(index1, currentStr.Length - index1 - 1);
                            }
                            else
                            {
                                mesData.Result = "OK";
                                b_Result = true;
                            }
                        }
                        else
                        {
                            mesData.errMSG = "获取数据超时";
                        }
                        if (myList != null)
                        {
                            MachineDataDefine.str1 = "";//myList[0] + "," + myList[1]; //+ "," + myList[2] + ":" + myList[3];
                        }
                        else
                        {
                            MachineDataDefine.str = "获取数据超时";
                        }
                        break;
                }
                MachineDataDefine.str1 = currentStr1;
            }

            return b_Result;
        }
        public static void SaveDateMes(string mes)
        {
            try
            {
                string fileName;
                fileName = string.Format("{0}.txt", DateTime.Now.ToString("yyyy_MM_dd"));
                string outputPath = @"D:\MES Log";
                if (!Directory.Exists(outputPath))
                {
                    Directory.CreateDirectory(outputPath);
                }
                string fullFileName = Path.Combine(outputPath, fileName);
                System.IO.FileStream fs;
                StreamWriter sw;
                if (!File.Exists(fullFileName))
                {
                    fs = new System.IO.FileStream(fullFileName, System.IO.FileMode.Append, System.IO.FileAccess.Write, FileShare.Read);
                    sw = new StreamWriter(fs, System.Text.Encoding.Default);
                    sw.WriteLine(DateTime.Now.ToString("HH:mm:ss:fff") + "__" + mes);
                    sw.Close();
                    fs.Close();

                }
                else
                {
                    fs = new System.IO.FileStream(fullFileName, System.IO.FileMode.Append, System.IO.FileAccess.Write, FileShare.Read);
                    sw = new StreamWriter(fs, System.Text.Encoding.Default);
                    sw.WriteLine(DateTime.Now.ToString("HH:mm:ss:fff") + "__" + mes);
                    sw.Close();
                    fs.Close();
                }
            }
            catch
            {


            }
        }

        public void SaveDatePDCA(string mes)
        {
            try
            {
                string fileName;
                fileName = string.Format("{0}.txt", DateTime.Now.ToString("yyyy_MM_dd"));
                string outputPath = @"D:\PDCA Log";
                if (!Directory.Exists(outputPath))
                {
                    Directory.CreateDirectory(outputPath);
                }
                string fullFileName = Path.Combine(outputPath, fileName);
                System.IO.FileStream fs;
                StreamWriter sw;
                if (!File.Exists(fullFileName))
                {
                    fs = new System.IO.FileStream(fullFileName, System.IO.FileMode.Append, System.IO.FileAccess.Write, FileShare.Read);
                    sw = new StreamWriter(fs, System.Text.Encoding.Default);
                    sw.WriteLine(DateTime.Now.ToString("HH:mm:ss:fff") + "__" + mes);
                    sw.Close();
                    fs.Close();

                }
                else
                {
                    fs = new System.IO.FileStream(fullFileName, System.IO.FileMode.Append, System.IO.FileAccess.Write, FileShare.Read);
                    sw = new StreamWriter(fs, System.Text.Encoding.Default);
                    sw.WriteLine(DateTime.Now.ToString("HH:mm:ss:fff") + "__" + mes);
                    sw.Close();
                    fs.Close();
                }
            }
            catch
            {


            }
        }

        public static MesResult GetMesResult(MesPostData mesPostData, CMDStep cmdStep)
        {
            MesResult mesResult = new MesResult();
            try
            {
                string url = "";
                if (cmdStep == CMDStep.用户登录)
                {
                    mesPostData.pCmd = "GET_OP_ACCESS";
                    url = MESDataDefine.MESLXData.GetCmdResURL;
                }
                else if (cmdStep == CMDStep.检查UOP)
                {
                    url = MESDataDefine.MESLXData.AssyCheckURL;
                }
                else if (cmdStep == CMDStep.上传数据)
                {
                    url = MESDataDefine.MESLXData.ColTestDataURL;
                }
                else if (cmdStep == CMDStep.提交过站)
                {
                    url = MESDataDefine.MESLXData.AssyGoURL;
                }
                else if (cmdStep == CMDStep.更新MESHashKey)
                {
                    mesPostData.pCmd = "SAVE_AUDIO_INFO";
                    url = MESDataDefine.MESLXData.GetCmdResURL;
                }
                else if (cmdStep == CMDStep.检查HashKey)
                {
                    mesPostData.pCmd = "CHECK_AUDIO_INFO";
                    url = MESDataDefine.MESLXData.GetCmdResURL;
                }
                else if (cmdStep == CMDStep.UC获取SN)
                {
                    mesPostData.pCmd = "RFID_GET_SN";
                    url = MESDataDefine.MESLXData.GetCmdResURL;
                }
                else if (cmdStep == CMDStep.检查SN类型)
                {
                    mesPostData.pCmd = "GET_SN_PART_TYPE";
                    url = MESDataDefine.MESLXData.GetCmdResURL;
                }
                else if (cmdStep == CMDStep.更新点检信息)
                {
                    mesPostData.pCmd = "EQUIPMENT_MT";
                    url = MESDataDefine.MESLXData.GetCmdResURL;
                }
                HttpWebRequest Web = (HttpWebRequest)WebRequest.Create(url);
                string str = JsonConvert.SerializeObject(mesPostData);
                SaveDateMes(cmdStep.ToString() + "\r\n" + "send\r\n" + str);
                byte[] data = Encoding.UTF8.GetBytes(str);
                Web.Method = "POST";
                Web.ContentType = "application/json";
                Web.ContentLength = data.Length;
                using (var stream = Web.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                HttpWebResponse Res = (HttpWebResponse)Web.GetResponse();

                using (StreamReader sr = new StreamReader(Res.GetResponseStream(), Encoding.UTF8))
                {
                    string result = sr.ReadToEnd();
                    mesResult = JsonConvert.DeserializeAnonymousType<MesResult>(result, mesResult);
                }
            }
            catch (Exception e)
            {
                mesResult.Result = false;
                mesResult.RetMsg = e.Message;
            }
            string getStr = JsonConvert.SerializeObject(mesResult);
            SaveDateMes(cmdStep.ToString() + "\r\n" + "get\r\n" + getStr + "\r\n");
            return mesResult;
        }
        public static string GetBobcatResult(string str, CMDStep cmdStep)
        {
            string result = "";
            try
            {
                HttpWebRequest Web = (HttpWebRequest)WebRequest.Create(MESDataDefine.MESLXData.BobcatURL);
                SaveDateMes(cmdStep.ToString() + "\r\n" + "send\r\n" + str);
                byte[] data = Encoding.UTF8.GetBytes(str);
                Web.Method = "POST";
                Web.ContentType = "application/x-www-form-urlencoded";
                Web.ContentLength = data.Length;
                using (var stream = Web.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                HttpWebResponse Res = (HttpWebResponse)Web.GetResponse();

                using (StreamReader sr = new StreamReader(Res.GetResponseStream(), Encoding.UTF8))
                {
                    result = sr.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            SaveDateMes(cmdStep.ToString() + "\r\n" + "get\r\n" + result + "\r\n");
            return result;
        }
        /// <summary>
        /// 根据UC获取SN
        /// </summary>
        /// <param name="uc"></param>
        /// <returns></returns>
        public static MesResult PostMesGetSN(ProductPoint product)
        {
            MesPostData mesPostData = new MesPostData();
            mesPostData.empNo = MESDataDefine.MESLXData.empNo;
            mesPostData.terminalName = MESDataDefine.MESLXData.terminalName;
            mesPostData.serial_Number = product.UC;
            return GetMesResult(mesPostData, CMDStep.UC获取SN);
        }
        /// <summary>
        /// 获取产品类型
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="RL"></param>
        /// <returns></returns>
        public static MesResult MES_CheckSnType(ProductPoint product)  // mes Check SN 类型（M or E）
        {
            MesPostData mesPostData = new MesPostData();
            mesPostData.empNo = MESDataDefine.MESLXData.empNo;
            mesPostData.terminalName = MESDataDefine.MESLXData.terminalName;
            mesPostData.serial_Number = product.SN;
            return GetMesResult(mesPostData, CMDStep.检查SN类型);
        }
        /// <summary>
        /// MesCheckSN
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        public static MesResult PostCheckSN(ProductPoint product)
        {
            MesPostData mesPostData = new MesPostData();
            mesPostData.empNo = MESDataDefine.MESLXData.empNo;
            mesPostData.terminalName = MESDataDefine.MESLXData.terminalName;
            mesPostData.serial_Number = product.SN;
            return GetMesResult(mesPostData, CMDStep.检查UOP);
        }
        /// <summary>
        /// MesCheckSN
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        public static string BobcatCheckSN(ProductPoint product)
        {
            string str = "sn=" + product.SN + "&c=QUERY_RECORD&tsid=" + MESDataDefine.MESLXData.terminalName + "&p=unit_process_check";
            return GetBobcatResult(str, CMDStep.Bobcat检查UOP);
        }
        /// <summary>
        /// 上传测试数据
        /// </summary>
        /// <returns></returns>
        public static MesResult PostTestData(ProductPoint product)
        {
            MesPostData mesPostData = new MesPostData();
            string pass = product.pass ? "PASS" : "FAIL";
            string testData = "test_result=" + pass +
                              "$unit_sn=" + product.SN +
                              "$uut_start=" + product.startTime.ToString("yyyy-MM-dd HH:mm:ss") +
                              "$uut_stop=" + product.endTime.ToString("yyyy-MM-dd HH:mm:ss") +
                              "$limits_version=" +
                              "$software_name=" +
                              "$software_version=" + MESDataDefine.MESLXData.SW_Version +
                              "$station_id=" + MESDataDefine.MESLXData.terminalName +
                              "$fixture_id=" + product.UC;
            string result = "";
            foreach (data item in product.datas)
            {
                result = result +
                            "lower_limit=" + item.lower_limit.ToString("0.00") +
                            "$parametric_key=" +
                            "$priority=" +
                            "$result=" +
                            "$sub_sub_test=" +
                            "$sub_test=" +
                            "$test=" + item.test +
                            "$units=" + "" +
                            "$upper_limit=" + item.upper_limit.ToString("0.00") +
                            "$value=" + item.value.ToString("0.00") +
                            "$Message=,";
            }


            mesPostData.results = result;
            mesPostData.testData = testData;
            mesPostData.empNo = MESDataDefine.MESLXData.empNo;
            mesPostData.terminalName = MESDataDefine.MESLXData.terminalName;
            mesPostData.serial_Number = product.SN;
            mesPostData.toolingNo = product.UC;
            mesPostData.collectType = "REC";

            return GetMesResult(mesPostData, CMDStep.上传数据);
        }
        /// <summary>
        /// MES过站
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        public static MesResult PostPassStation(ProductPoint product)
        {
            MesPostData mesPostData = new MesPostData();
            mesPostData.empNo = MESDataDefine.MESLXData.empNo;
            mesPostData.terminalName = MESDataDefine.MESLXData.terminalName;
            mesPostData.serial_Number = product.SN;
            mesPostData.toolingNo = product.UC;
            return GetMesResult(mesPostData, CMDStep.提交过站);
        }
        /// <summary>
        /// Bobcat过站
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        public static string BobcatADD_RECORD(ProductPoint product)
        {

            string str = "result=" + (product.pass ? "PASS" : "FAIL") +
                "&c=ADD_RECORD" +
                "&product=" + MESDataDefine.MESLXData.ProjectCode +
                "&sn=" + product.SN +
               // "&test_station_name =" + product.SN +
                " &station_id=" + MESDataDefine.MESLXData.terminalName +
                "&mac_address =" +
                "&sw_version =" + MESDataDefine.MESLXData.SW_Version +
                "&audit_mode=0" +
                "&start_time=" + product.startTime.ToString("yyyy-MM-dd HH:mm:ss") +
                "&stop_time=" + product.endTime.ToString("yyyy-MM-dd HH:mm:ss");
            if (!product.pass)
            {
                str += "&list_of_failing_tests="+
                    "failure_message =";
            }
            return GetBobcatResult(str, CMDStep.Bobcat过站);
        }
    }
}
