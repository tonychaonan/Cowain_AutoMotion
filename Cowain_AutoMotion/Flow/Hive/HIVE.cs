using Cowain_AutoMotion;
using Cowain_AutoMotion.Flow;
using Cowain_AutoMotion.Flow._2Work;
using Cowain_AutoMotion.Flow.Hive;
using Cowain_Form.FormView;
using Cowain_Machine.Flow;
using DevExpress.XtraCharts.Native;
using lixun_upload_HIVE;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ToolTotal;

namespace Cowain_Machine.Flow
{
    public class HIVE
    {
        private static object obj = new object();
        public HIVE()
        {

        }
        private static HIVE instance = null;
        public static HIVE HIVEInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (obj)
                    {
                        if (instance == null)
                        {
                            instance = new HIVE();
                        }
                    }
                }
                return instance;
            }
        }
        /// <summary>
        /// 记录进料时间  用于HIVE
        /// </summary>
        public string[] HIVEStarttime = new string[2];//记录进料时间  用于HIVE
        /// <summary>
        /// 记录出料料时间  用于HIVE
        /// </summary>
        public string[] HIVEStoptime = new string[2];//记录出料料时间  用于HIVE

        /// <summary>
        /// 记录进料时间  用于计算HIVE CT
        /// </summary>
        public DateTime[] hivestarttime = new DateTime[2];

        /// <summary>
        /// 记录出料料时间  用于计算HIVE CT
        /// </summary>
        public DateTime[] hivestoptime = new DateTime[2];
        /// <summary>
        /// 记录上次出料时间
        /// </summary>
        public DateTime[] lastStoptime = new DateTime[2];

        /// <summary>
        /// 记录出料料时间  用于计算HIVE CT
        /// </summary>
        public double[] CT = { 15, 16 };
        /// <summary>
        /// 是否正在发送Chronos
        /// </summary>
        public bool HiveSendChronos_IsRunning = false;
        /// <summary>
        /// 用于计算HIVE 产品间隔CT
        /// </summary>
        public double CT_VAL = 10;
        /// <summary>
        /// 胶路是否读取成功
        /// </summary>
        public bool getGlueSpeed_IsOK = false;
        /// <summary>
        /// 允许读角度的行数
        /// </summary>
        public bool CanReadGluePathCount = false;
        /// <summary>
        /// 用于hive---条码枪扫的SN或者是MES获取到的SN，在下次流道进料后会被覆盖
        /// </summary>
        public string[] StrSNS = new string[2] { "", "" };

        /// <summary>
        /// 用于hive---条码枪扫的UC，在下次流道进料后会被覆盖
        /// </summary>
        public string[] StrUCS = new string[2] { "", "" };

        /// <summary>
        /// 记录上一片料流出时间  用于计算HIVE 产品间隔CT
        /// </summary>
        public DateTime last_one_time = new DateTime();
        /// <summary>
        /// 记录HIVE反馈状态
        /// </summary>
        private bool hive_reveice_status = true;
        public bool HIVE_Reveice_Status
        {
            get { return hive_reveice_status; }
            set
            {
                hive_reveice_status = value;
                if (!hive_reveice_status)
                {
                    SaveErrorHIVE("HIVE Receive Data Fail!");
                }
            }
        }
        public bool HIVE_Error = false;
        /// <summary>
        /// 不上传HIVE的ErrorMessage集合
        /// </summary>
        public List<string> NotUploadErrorMessageList = new List<string>()
        {
            "",
            "NULL",
            //"Drop Material",
            //"Left Carry in Error" , 
            //"Right Carry in Error" , 
            //"Left Carry out Error" , 
            //"Right Carry out Error" , 
            //"CCD1 Data Exception" , 
            //"CCD2 Data Exception" , 
            //"Glue1 Check Error" , 
            //"Glue2 Check Error" ,
            //"Left BarCode Reader1 is Error",
            //"LeftBarCode Reader2 is Error",
            //"Right BarCode Reader1 is Error",
            //"Right BarCode Reader2 is Error",
            //"FileExis NOT Found",
        };
        HttpWebRequest Web = null;
        HttpWebResponse Res = null;

        HttpWebRequest Web1 = null;
        HttpWebResponse Res1 = null;
        //IniFile myIniFile;
        // int DisType = 0;
        public string SendStr_mes = "";
        //string path;
        private object locker = new object();
        private object locker1 = new object();
        /// <summary>
        /// 上一次HIVE发送状态
        /// </summary>
        public static int PreviousState = 1;
        /// <summary>
        /// 上传图片到HIVE
        /// </summary>
        /// <param name="sn">产品sn码</param>
        /// <param name="input_time"></param>
        /// <param name="output_time"></param>
        /// <param name="output_ct"></param>
        /// <param name="filePaths">图片文件路径list<></param>
        /// <returns></returns>
        //public string HiveSendMACHINEDATA(string sn, string uc, int no, string hiveValue, string hiveLimit, Dictionary<string, string> dataDir,bool isremote)
        //{
        //    HiveLogType hiveType = HiveLogType.MachineData;
        //    int state = (int)frm_Main.formData?.ChartTime1.RunStatus+1;
        //    dataDir.Add("HIVE状态", state.ToString());
        //    if (MachineDataDefine.b_UseLAD)
        //    {
        //        dataDir.Add("LAD类型", MachineDataDefine.LADModel.ToString());
        //        dataDir.Add("操作员序列号", MachineDataDefine.LADOPID.ToString());
        //        // dataDir.Add("测试时间", DateTime.Now.ToString("yyyyMMddHHmmss"));
        //        dataDir.Add("测试时间", MachineDataDefine.测试时间);
        //        hiveType = HiveLogType.MachineData_LAD;
        //    }
        //    if (string.IsNullOrWhiteSpace(sn))
        //    {
        //        sn = "TEST" + DateTime.Now.ToString("HHmmss");
        //    }
        //    sn = sn.PadLeft(10, '0');
        //    dataDir["产品SN"] = sn;
        //    double outct = 0;
        //    if(dataDir.Keys.Contains("产品CT")!=true)
        //    {
        //        dataDir.Add("产品CT", "0");
        //    }
        //    //if (lastStoptime[no] != null && lastStoptime[no] != DateTime.Parse("0001-1-1"))
        //    //{
        //    //    outct = (hivestoptime[no] - lastStoptime[no]).TotalSeconds;
        //    //    dataDir["产品CT"] = outct.ToString();
        //    //}

        //    if (MachineDataDefine.FirstProduct)
        //    {
        //        outct = 0;
        //        dataDir["产品CT"] = outct.ToString();
        //        if( MachineDataDefine.remoteFirstProduct)
        //        {
        //            MachineDataDefine.remoteFirstProduct = false;
        //            MachineDataDefine.FirstProduct = false;
        //        }

        //    }
        //    if (outct >= 3600)
        //    {
        //        outct = 3600;
        //        dataDir["产品CT"] = outct.ToString();
        //    }
        //    string value = formatData(hiveType, dataDir,isremote);
        //    string fullFileName = dataDir["压缩包全路径"];
        //    var filePaths = new List<string>() { fullFileName };
        //    if (!File.Exists(fullFileName))
        //    {
        //        fullFileName = "";
        //        filePaths = new List<string>() { };
        //    }

        //       // SaveDateHIVE("发送数据MACHINEDATA：" + value, HiveLogType.MachineData);

        //    //if (MachineDataDefine.machineState.b_UseRemoteQualification)
        //    if(isremote)
        //    {
        //        Remote.RemoteInstance.SaveDateRemote(value, Remote.RemoteLogType.MachineData);
        //        return "";//远程模式下，只保存数据到本地
        //    }
        //    else
        //    {
        //        SaveDateHIVE("发送数据MACHINEDATA：" + value, HiveLogType.MachineData);
        //    }
        //   // SaveDateHIVE("发送数据MACHINEDATA：" + value, HiveLogType.MachineData);
        //    var uploadTool = new UploadToHive("http://10.0.0.2:5008/v5/capture/machinedata");
        //    lixun_upload_HIVE.Models.HiveResponse uploadRes = null;
        //    string res = string.Empty;
        //    try
        //    {
        //        uploadRes = uploadTool.MultipartFormDataContentTest(value, filePaths);
        //        res = JsonConvert.SerializeObject(uploadRes);
        //    }
        //    catch (Exception err)
        //    {
        //        SaveDateHIVE(err.Message, HiveLogType.Other);
        //    }

        //    SaveDateHIVE("接收数据MACHINEDATA：" + res, HiveLogType.MachineData);
        //    return res;
        //}

        /// <summary>
        /// 发送做料数据
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        public string HiveSendMACHINEDATA(ProductPoint product)//TAG
        {
            try
            {
                int hiveState = (int)frm_Main.formData?.ChartTime1.RunStatus + 1;
                HiveMachineData _hiveMachineData = new HiveMachineData();
                _hiveMachineData.unit_sn = product.SN;
                _hiveMachineData.pass = product.pass;
                _hiveMachineData.input_time = product.startTime.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");
                _hiveMachineData.output_time = product.endTime.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("fixture_id", product.UC);
                data.Add("head_id", 1);
                data.Add("sequence", 0);
                data.Add("hive_state", hiveState);
                double outputCt=(product.endTime - product.lastEndTime).TotalSeconds;
                if (outputCt>3600)
                {
                    outputCt = 3600;
                }
                data.Add("output_ct", outputCt.ToString("f2"));
                data.Add("unit_ct", double.Parse((product.endTime - product.startTime).TotalSeconds.ToString("0.00")));
                data.Add("sw_version", MESDataDefine.MESLXData.SW_Version);
                data.Add("limits_version", "CWN_1.0.0.0_230817_POR");
                data.Add("recheck_result_OK", product.pass ? 1 : 0);
                data.Add("first_vision_tossing_NG", product.pass ? 0 : 1);
                foreach (data item in product.datas)
                {
                    data.Add(item.test, item.value);
                }
                _hiveMachineData.data = data;
                Dictionary<string, limitdata> limit = new Dictionary<string, limitdata>();
                foreach (Limit item in HIVEDataDefine.limitdata)
                {
                    limitdata limitdata = new limitdata();
                    limitdata.upper_limit = item.upper_limit;
                    limitdata.lower_limit = item.lower_limit;
                    limit.Add(item.Name, limitdata);
                }
                _hiveMachineData.limit = limit;
                Dictionary<string, string> serials = new Dictionary<string, string>();
                serials.Add("station_id", MESDataDefine.MESLXData.terminalName);
                serials.Add("mac_addr", MachineDataDefine.hive_mac);
                serials.Add("machine_type", "aoi");
                string failure_code = "NA";
                for (int i = 0; i < product.datas.Count; i++)
                {
                    if (Convert.ToDouble(product.datas[i].value) > HIVEDataDefine.limitdata[i].upper_limit || Convert.ToDouble(product.datas[i].value) < HIVEDataDefine.limitdata[i].lower_limit)
                    {
                        if (failure_code != "NA")
                        {
                            failure_code = "Gross Fail";
                        }
                        else
                        {
                            failure_code = product.datas[i].test.ToString();
                        }
                    }
                }
                serials.Add("failure_code", failure_code);
                _hiveMachineData.serials = serials;
                string filePath = product.fullFileName;
                var filePaths = new List<string>() { filePath };
                if (!File.Exists(filePath))
                {
                    filePath = "";
                    filePaths = new List<string>() { };
                }
                _hiveMachineData.blobs[0] = new blob { file_name = Path.GetFileName(filePath) };


                string value = JsonConvert.SerializeObject(_hiveMachineData);
                var up = new UploadToHive("http://10.0.0.2:5008/v5/capture/machinedata");
                SaveDateHIVE("发送数据MACHINEDATA：" + value, HiveLogType.MachineData);
                if (MachineDataDefine.HardwareCfg.Remote == 1)
                {
                    Remote.RemoteInstance.SaveDateRemote(value, Remote.RemoteLogType.MachineData);
                }
                var res = up.MultipartFormDataContentTest(value, filePaths);
                string StrRes = JsonConvert.SerializeObject(res);
                if (res.ErrorCode == null && res.ErrorText == null)
                {
                    SaveDateHIVE("接受HIVE数据反馈：" + StrRes, HiveLogType.MachineData);
                }
                else
                {
                    SaveDateHIVE("接受HIVE数据反馈失败：" + res.ErrorText, HiveLogType.MachineData);
                }
                return StrRes;

            }
            catch (Exception ex)
            {
                SaveDateHIVE(ex.Message, HiveLogType.MachineError);
                return ex.Message.ToString();
            }
        }

        /// <summary>
        ///  HIVE系统发送errorcode
        /// </summary>
        /// <param name="errorcode">报警码</param>
        /// <param name="messgae">报警信息</param>
        /// <param name="occurrence_time">报警开始时间</param>
        /// <param name="resolved_time">报警结束时间</param>
        /// <param name="isauto">true表示自动运行时调用此函数</param>
        /// <returns></returns>
        public string HiveSendERRORDATA(string errorcode, string errtype, string messgae, string occurrence_time, string resolved_time, long Start_time, long Stop_time, bool isremote)
        {
            try
            {
                lock (locker1)
                {
                    Thread.Sleep(100);
                    SendStr_mes = "";
                    Dictionary<string, string> dataDir = new Dictionary<string, string>();
                    dataDir.Add("错误类型", errtype);
                    dataDir.Add("错误代码", errorcode);
                    dataDir.Add("错误开始时间", occurrence_time);
                    dataDir.Add("解决时间", resolved_time);
                    dataDir.Add("错误内容", messgae);
                    dataDir.Add("类型", "Error");
                    string value = formatData(HiveLogType.MachineError, dataDir, isremote);
                    // if (MachineDataDefine.machineState.b_UseRemoteQualification)
                    if (isremote)
                    {
                        Remote.RemoteInstance.SaveDateRemote(value, Remote.RemoteLogType.MachineError);
                        return "";//远程模式下，只保存数据到本地
                    }
                    string url = "http://10.0.0.2:5008/v5/capture/errordata";
                    Web = (HttpWebRequest)WebRequest.Create(url);
                    Web.Method = "POST";
                    Web.ContentType = "multipart/form-data";
                    Web.Timeout = 200;
                    string dat = value;
                    SaveDateHIVE("发送数据ERRORDATA：" + dat, HiveLogType.MachineError);
                    SendStr_mes = dat;
                    string str = "";
                    for (int i = 0; i < 3; i++)
                    {
                        try
                        {
                            byte[] data = Encoding.UTF8.GetBytes(dat);
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
                        catch (Exception ex)
                        {
                            if (i == 0 || i == 2)
                            {
                                SaveDateHIVE("接受HIVE数据反馈失败：" + ex.ToString(), HiveLogType.Other);
                            }
                            continue;
                        }
                        if (str != "")
                        {
                            SaveDateHIVE("接收数据ERRORDATA：" + str, HiveLogType.MachineError);
                            HIVE_Reveice_Status = true;
                            return str;
                        }
                    }
                    HIVE_Reveice_Status = false;
                    return "";
                }
            }
            catch (Exception ex)
            {
                SaveDateHIVE(ex.Message, HiveLogType.Other);
                HIVE_Reveice_Status = false;
                return "";
            }
        }

        public void HiveSend(string errorcode, string errtype, string messgae, string starttime, string stoptime, long Starterror_time, long Stoperror_time)
        {
            if (MachineDataDefine.machineState.b_UseHive && !MachineDataDefine.machineState.b_UseTestRun)
            {
                HIVE.HIVEInstance.HiveSendERRORDATA(frm_Main.formError.ErrorUnit1.ErrorCode, frm_Main.formError.ErrorUnit1.ErrorType, frm_Main.formError.ErrorUnit1.ErrorMessage, starttime, stoptime, Starterror_time, Stoperror_time, false);
            }
            if (MachineDataDefine.machineState.b_UseRemoteQualification)
            {
                HIVE.HIVEInstance.HiveSendERRORDATA(frm_Main.formError.ErrorUnit1.ErrorCode, frm_Main.formError.ErrorUnit1.ErrorType, frm_Main.formError.ErrorUnit1.ErrorMessage, starttime, stoptime, Starterror_time, Stoperror_time, true);
            }
        }
        //wsd
        /// <summary>
        /// PlanDownTime的情况下HIVE发送errorcode
        /// </summary>
        /// <param name="errorcode">报警码</param>
        /// <param name="messgae">报警信息</param>
        /// <param name="occurrence_time">报警开始时间</param>
        /// <param name="resolved_time">报警结束时间</param>
        /// <param name="isauto">true表示自动运行时调用此函数</param>
        /// <returns></returns>
        public string HiveSendERRORDATA(string errorcode, string errtype, string messgae, string occurrence_time, string resolved_time, bool ISREMOTE)
        {
            try
            {
                lock (locker1)
                {
                    SendStr_mes = "";
                    Dictionary<string, string> dataDir = new Dictionary<string, string>();
                    dataDir.Add("错误类型", errtype);
                    dataDir.Add("错误代码", errorcode);
                    dataDir.Add("错误开始时间", occurrence_time);
                    dataDir.Add("解决时间", resolved_time);
                    dataDir.Add("错误内容", messgae);
                    dataDir.Add("类型", "Planned");
                    string value = formatData(HiveLogType.MachineError, dataDir, ISREMOTE);
                    if (MachineDataDefine.machineState.b_UseRemoteQualification)
                    {
                        Remote.RemoteInstance.SaveDateRemote(value, Remote.RemoteLogType.MachineError);
                        return "";//远程模式下，只保存数据到本地
                    }
                    string url = "http://10.0.0.2:5008/v5/capture/errordata";
                    Web = (HttpWebRequest)WebRequest.Create(url);
                    Web.Method = "POST";
                    Web.ContentType = "multipart/form-data";
                    Web.Timeout = 200;
                    string dat = value;
                    SaveDateHIVE("发送数据ERRORDATA：" + dat, HiveLogType.MachineError);

                    byte[] data = Encoding.UTF8.GetBytes(dat);
                    Web.ContentLength = data.Length;

                    SendStr_mes = dat;
                    string str = "";
                    for (int i = 0; i < 3; i++)
                    {
                        try
                        {
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
                        catch (Exception ex)
                        {
                            if (i == 0 || i == 2)
                            {
                                SaveDateHIVE("接受HIVE数据反馈失败：" + ex.ToString(), HiveLogType.Other);
                            }
                            continue;
                        }
                        if (str != "")
                        {
                            SaveDateHIVE("接收数据ERRORDATA：" + str, HiveLogType.MachineError);
                            HIVE_Reveice_Status = true;
                            return str;
                        }
                    }
                    HIVE_Reveice_Status = false;
                    return "";
                }
            }
            catch (Exception ex)
            {
                SaveDateHIVE(ex.Message, HiveLogType.Other);
                HIVE_Reveice_Status = false;
                return "";
            }
        }

        /// <summary>
        /// HIVE系统发送设备状态
        /// </summary>
        /// <param name="sataus">1:running 2：idle 3：engineering 4：planned downtime 5：error_down</param>
        /// <param name="errorcode">sataus为5时传值</param>
        /// <param name="messgae">sataus为5时传值</param>
        /// <returns></returns>
        public string HiveSendMACHINESTATE(int sataus, string errorcode, string messgae, string errtype, bool isremote, string badge = "")
        {
            try
            {
                lock (locker1)
                {
                    SendStr_mes = "";
                    string value = "";
                    if (sataus == PreviousState)
                    {
                        return "";
                    }
                    HiveLogType hiveLogType = HiveLogType.MachineState1;
                    if (sataus == 2)
                    {
                        hiveLogType = HiveLogType.MachineState2;
                    }
                    else if (sataus == 3)
                    {
                        hiveLogType = HiveLogType.MachineState3;
                    }
                    else if (sataus == 4)
                    {
                        hiveLogType = HiveLogType.MachineState4;
                    }
                    else if (sataus == 5)
                    {
                        hiveLogType = HiveLogType.MachineState5;
                    }
                    if (hiveLogType == HiveLogType.MachineState1 || hiveLogType == HiveLogType.MachineState2 || hiveLogType == HiveLogType.MachineState3)
                    {
                        Dictionary<string, string> dataDir = new Dictionary<string, string>();
                        dataDir.Add("当前状态", sataus.ToString());
                        dataDir.Add("状态改变时间", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800"));
                        dataDir.Add("以前状态", PreviousState.ToString());
                        dataDir.Add("软件版本", MESDataDefine.MESLXData.SW_Version);
                        dataDir.Add("MS哈希值", HIVEDataDefine.HIVE_sha1_Data.Main_SW_SHA1_Name.ToString());
                        dataDir.Add("VS哈希值", HIVEDataDefine.HIVE_sha1_Data.Vision_SW_SHA1_Name.ToString());
                        // if (MachineDataDefine.machineState.b_UseRemoteQualification)
                        if (isremote)
                        {
                            dataDir.Add("卡号", "1234567890");
                        }
                        else
                        {
                            dataDir.Add("卡号", badge);
                        }
                        value = formatData(hiveLogType, dataDir, isremote);
                    }
                    else if (hiveLogType == HiveLogType.MachineState4 || hiveLogType == HiveLogType.MachineState5)
                    {
                        Dictionary<string, string> dataDir = new Dictionary<string, string>();
                        dataDir.Add("当前状态", sataus.ToString());
                        dataDir.Add("状态改变时间", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800"));
                        dataDir.Add("以前状态", PreviousState.ToString());
                        dataDir.Add("软件版本", MESDataDefine.MESLXData.SW_Version);
                        dataDir.Add("MS哈希值", HIVEDataDefine.HIVE_sha1_Data.Main_SW_SHA1_Name.ToString());
                        dataDir.Add("VS哈希值", HIVEDataDefine.HIVE_sha1_Data.Vision_SW_SHA1_Name.ToString());
                        dataDir.Add("错误信息", errtype);
                        dataDir.Add("报错明细", messgae);
                        //if (MachineDataDefine.machineState.b_UseRemoteQualification)
                        if (isremote)
                        {
                            dataDir.Add("卡号", "1234567890");
                        }
                        else
                        {
                            dataDir.Add("卡号", badge);
                        }
                        value = formatData(hiveLogType, dataDir, isremote);
                    }
                    //不考虑发送成功与否
                    PreviousState = sataus;
                    // if (MachineDataDefine.machineState.b_UseRemoteQualification)
                    if (isremote)
                    {
                        Remote.RemoteInstance.SaveDateRemote(value, Remote.RemoteLogType.MachineState);
                        return "";//远程模式下，只保存数据到本地
                    }
                    string url = "http://10.0.0.2:5008/v5/capture/machinestate";//"
                    Web = (HttpWebRequest)WebRequest.Create(url);
                    Web.Method = "POST";
                    Web.ContentType = "multipart/form-data";
                    Web.Timeout = 200;
                    string dat = value;
                    SaveDateHIVE("发送数据MACHINESTATE：" + dat, HiveLogType.MachineState);

                    SendStr_mes = dat;
                    byte[] data = Encoding.UTF8.GetBytes(dat);
                    Web.ContentLength = data.Length;

                    string str = "";
                    for (int i = 0; i < 3; i++)
                    {
                        try
                        {
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
                        catch (Exception ex)
                        {
                            if (i == 0 || i == 2)
                            {
                                SaveDateHIVE("接受HIVE数据反馈失败：" + ex.ToString(), HiveLogType.Other);
                            }
                            continue;
                        }
                        if (str != "")
                        {
                            SaveDateHIVE("接收数据MACHINESTATE：" + str, HiveLogType.MachineState);
                            HIVE_Reveice_Status = true;
                            return str;
                        }
                    }
                    HIVE_Reveice_Status = false;
                    return "";
                }
            }
            catch (Exception ex)
            {
                SaveDateHIVE(ex.Message, HiveLogType.Other);
                HIVE_Reveice_Status = false;
                return "";
            }

            //  return "";
        }

        public string HiveSendMACHINESTATE_Error(int sataus, string errorcode, string messgae, string errtype, bool isremote, string badge)
        {
            try
            {
                lock (locker1)
                {
                    SendStr_mes = "";
                    string value = "";
                    if (sataus != 5)
                    {
                        return "";
                    }
                    //HiveLogType hiveLogType = HiveLogType.MachineState1;
                    //if (sataus == 2)
                    //{
                    //    hiveLogType = HiveLogType.MachineState2;
                    //}
                    //else if (sataus == 3)
                    //{
                    //    hiveLogType = HiveLogType.MachineState3;
                    //}
                    //else if (sataus == 4)
                    //{
                    //    hiveLogType = HiveLogType.MachineState4;
                    //}
                    //else if (sataus == 5)
                    //{
                    HiveLogType hiveLogType = HiveLogType.MachineState5;
                    //}
                    if (hiveLogType == HiveLogType.MachineState1 || hiveLogType == HiveLogType.MachineState2 || hiveLogType == HiveLogType.MachineState3)
                    {
                        Dictionary<string, string> dataDir = new Dictionary<string, string>();
                        dataDir.Add("当前状态", sataus.ToString());
                        dataDir.Add("状态改变时间", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800"));
                        dataDir.Add("以前状态", PreviousState.ToString());
                        dataDir.Add("软件版本", MESDataDefine.MESLXData.SW_Version);
                        dataDir.Add("MS哈希值", HIVEDataDefine.HIVE_sha1_Data.Main_SW_SHA1_Name.ToString());
                        dataDir.Add("VS哈希值", HIVEDataDefine.HIVE_sha1_Data.Vision_SW_SHA1_Name.ToString());
                        // if (MachineDataDefine.machineState.b_UseRemoteQualification)
                        if (isremote)
                        {
                            dataDir.Add("卡号", "1234567890");
                        }
                        else
                        {
                            dataDir.Add("卡号", badge);
                        }
                        value = formatData(hiveLogType, dataDir, isremote);
                    }
                    else if (hiveLogType == HiveLogType.MachineState4 || hiveLogType == HiveLogType.MachineState5)
                    {
                        Dictionary<string, string> dataDir = new Dictionary<string, string>();
                        dataDir.Add("当前状态", sataus.ToString());
                        dataDir.Add("状态改变时间", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800"));
                        dataDir.Add("以前状态", PreviousState.ToString());
                        dataDir.Add("软件版本", MESDataDefine.MESLXData.SW_Version);
                        dataDir.Add("MS哈希值", HIVEDataDefine.HIVE_sha1_Data.Main_SW_SHA1_Name.ToString());
                        dataDir.Add("VS哈希值", HIVEDataDefine.HIVE_sha1_Data.Vision_SW_SHA1_Name.ToString());
                        dataDir.Add("错误信息", errtype);
                        dataDir.Add("报错明细", messgae);
                        //if (MachineDataDefine.machineState.b_UseRemoteQualification)
                        if (isremote)
                        {
                            dataDir.Add("卡号", "1234567890");
                        }
                        else
                        {
                            dataDir.Add("卡号", badge);
                        }
                        value = formatData(hiveLogType, dataDir, isremote);
                    }
                    //不考虑发送成功与否
                    PreviousState = sataus;
                    // if (MachineDataDefine.machineState.b_UseRemoteQualification)
                    if (isremote)
                    {
                        Remote.RemoteInstance.SaveDateRemote(value, Remote.RemoteLogType.MachineState);
                        return "";//远程模式下，只保存数据到本地
                    }
                    string url = "http://10.0.0.2:5008/v5/capture/machinestate";//"
                    Web = (HttpWebRequest)WebRequest.Create(url);
                    Web.Method = "POST";
                    Web.ContentType = "multipart/form-data";
                    Web.Timeout = 200;
                    string dat = value;
                    SaveDateHIVE("发送数据MACHINESTATE：" + dat, HiveLogType.MachineState);

                    SendStr_mes = dat;
                    byte[] data = Encoding.UTF8.GetBytes(dat);
                    Web.ContentLength = data.Length;

                    string str = "";
                    for (int i = 0; i < 3; i++)
                    {
                        try
                        {
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
                        catch (Exception ex)
                        {
                            if (i == 0 || i == 2)
                            {
                                SaveDateHIVE("接受HIVE数据反馈失败：" + ex.ToString(), HiveLogType.Other);
                            }
                            continue;
                        }
                        if (str != "")
                        {
                            SaveDateHIVE("接收数据MACHINESTATE：" + str, HiveLogType.MachineState);
                            HIVE_Reveice_Status = true;
                            return str;
                        }
                    }
                    HIVE_Reveice_Status = false;
                    return "";
                }
            }
            catch (Exception ex)
            {
                SaveDateHIVE(ex.Message, HiveLogType.Other);
                HIVE_Reveice_Status = false;
                return "";
            }
        }
        public enum HiveLogType
        {
            MachineState,
            MachineError,
            MachineData,
            MachineData_LAD,
            MachineState1,
            MachineState2,
            MachineState3,
            MachineState4,
            MachineState5,
            Other
        }
        public void SaveDateHIVE(string result, HiveLogType type)
        {
            try
            {
                lock (locker)
                {
                    string fileName = "";
                    if (type == HiveLogType.MachineState)
                    {
                        fileName = string.Format("{0}.txt", DateTime.Now.ToString("yyyyMMdd"));
                    }
                    else
                    {
                        fileName = string.Format("hour_{0}.txt", DateTime.Now.ToString("HH"));
                    }


                    string outputPath = @"D:\DATA\HIVE Log\";
                    if (type == HiveLogType.MachineState)
                    {
                        outputPath += "Machine State";
                    }
                    else if (type == HiveLogType.MachineError)
                    {
                        outputPath += "Machine Error";
                    }
                    else if (type == HiveLogType.MachineData)
                    {
                        outputPath += "Machine Data";
                    }
                    else
                    {
                        outputPath = @"D:\DATA\H other log";
                    }
                    if (type == HiveLogType.MachineState)
                    {

                    }
                    else
                    {
                        outputPath += @"\" + DateTime.Now.ToString("yyyyMMdd");
                    }


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
                        //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
                        sw = new StreamWriter(fs, System.Text.Encoding.Default);
                        sw.WriteLine(DateTime.Now.ToString("HH:mm:ss") + result + "");
                        sw.Close();
                        fs.Close();
                    }
                    else
                    {
                        fs = new System.IO.FileStream(fullFileName, System.IO.FileMode.Append, System.IO.FileAccess.Write, FileShare.Read);
                        //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
                        sw = new StreamWriter(fs, System.Text.Encoding.Default);
                        sw.WriteLine(DateTime.Now.ToString("HH:mm:ss") + result + "");
                        sw.Close();
                        fs.Close();
                    }
                }
            }
            catch { }
        }
        public void SaveErrorHIVE(string result)
        {
            try
            {
                lock (locker)
                {
                    string fileName;
                    fileName = string.Format("{0}.txt", DateTime.Now.ToString("yyyy_MM_dd"));
                    string outputPath = @"D:\DATA\HIVE记录\HIVE异常记录";
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
                        //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
                        sw = new StreamWriter(fs, System.Text.Encoding.Default);
                        sw.WriteLine(DateTime.Now.ToString("HH:mm:ss") + result + "");
                        sw.Close();
                        fs.Close();
                    }
                    else
                    {
                        fs = new System.IO.FileStream(fullFileName, System.IO.FileMode.Append, System.IO.FileAccess.Write, FileShare.Read);
                        //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
                        sw = new StreamWriter(fs, System.Text.Encoding.Default);
                        sw.WriteLine(DateTime.Now.ToString("HH:mm:ss") + result + "");
                        sw.Close();
                        fs.Close();
                    }
                }
            }
            catch
            {
            }
        }

        //   MESLXData mESLXData;
        private string Get_Load_SHA1()
        {
            string temp_Product_Parameter001 = "";
            int Count = 0;
            //#region  左流道            
            //if (pGantryParm1.enMatchMode == MSystemDateDefine.enMatchingMode.Normalmode || pGantryParm1.enMatchMode == MSystemDateDefine.enMatchingMode.JustFrontStation)
            //{
            //    for (int i = 0; i < HIVEDataDefine.HIVE_sha1_Data.PathCount[0]; i++)
            //    {
            //        Count++;
            //        if (i<=0)
            //        {
            //            temp_Product_Parameter001 += "\"CD_SHA1\":" + "\"" + HIVEDataDefine.HIVE_sha1_Data.GluePath_SHA1_Name[0][i] + "\",";
            //        }
            //        else if (i <= 4)
            //        {
            //            temp_Product_Parameter001 += "\"CD" + Count + "_SHA1\":" + "\"" + HIVEDataDefine.HIVE_sha1_Data.GluePath_SHA1_Name[0][i] + "\",";
            //        }
            //        else
            //            break;
            //    }
            //}
            //if (pGantryParm1.enMatchMode == MSystemDateDefine.enMatchingMode.Normalmode || pGantryParm1.enMatchMode == MSystemDateDefine.enMatchingMode.JustBackStation)
            //{
            //    for (int i = 0; i < HIVEDataDefine.HIVE_sha1_Data.PathCount[2]; i++)
            //    {
            //        Count++;
            //        if (i <= 0)
            //        {
            //            temp_Product_Parameter001 += "\"CD_SHA1\":" + "\"" + HIVEDataDefine.HIVE_sha1_Data.GluePath_SHA1_Name[2][i] + "\",";
            //        }
            //        else if (i <= 4)
            //        {
            //            temp_Product_Parameter001 += "\"CD" + Count + "_SHA1\":" + "\"" + HIVEDataDefine.HIVE_sha1_Data.GluePath_SHA1_Name[2][i] + "\",";
            //        }
            //        else
            //            break;
            //    }
            //}
            //#endregion
            return temp_Product_Parameter001;
        }
        /// <summary>
        /// 替换字符串
        /// </summary>
        /// <param name="hiveLogType"></param>
        /// <param name="datas"></param>
        /// <returns></returns>
        public string formatData(HiveLogType hiveLogType, Dictionary<string, string> datas, bool isRemote)
        {
            if (hiveLogType == HiveLogType.Other)
            {
                return "";
            }
            string data = "";
            //   if (MachineDataDefine.machineState.b_UseRemoteQualification)
            if (isRemote)
            {
                data = MESDataDefine.myHIVEForRemoteQualificationMsg[0][hiveLogType.ToString()];
            }
            else
            {
                data = MESDataDefine.myHIVEMsg[0][hiveLogType.ToString()];
            }
            foreach (var item in datas)
            {
                while (data.Contains(item.Key))
                {
                    data = data.Replace(item.Key, item.Value);
                }
            }
            return data;
        }
    }
}
