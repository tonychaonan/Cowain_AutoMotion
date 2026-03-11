using Post;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cowain_AutoMotion
{
    public class MESDataDefine
    {
        public static MESLXData MESLXData = new MESLXData();
        /// <summary>
        /// 存储MES上传相关信息
        /// </summary>
        public static Dictionary<string, string>[] myMesMsg = new Dictionary<string, string>[2];
        /// <summary>
        /// 存储HIVE上传相关信息
        /// </summary>
        public static Dictionary<string, string>[] myHIVEMsg = new Dictionary<string, string>[2];
        /// <summary>
        /// 存储HIVE远程模式上传相关信息
        /// </summary>
        public static Dictionary<string, string>[] myHIVEForRemoteQualificationMsg = new Dictionary<string, string>[2];
        /// <summary>
        /// 载具SN
        /// </summary>
        public static string holdSN = "";
        /// <summary>
        /// 产品的SN
        /// </summary>
        public static string SN = "";
        /// <summary>
        /// 产品开始坐料的时间
        /// </summary>
        public static string startDateTime = "";
        /// <summary>
        /// 单产品的CT
        /// </summary>
        public static string CT = "";
        public static void readMESAndHIVEDataType(int station)
        {
            myMesMsg[station] = new Dictionary<string, string>();
            myHIVEMsg[station] = new Dictionary<string, string>();
            myHIVEForRemoteQualificationMsg[station] = new Dictionary<string, string>();
            String strPath = System.IO.Directory.GetCurrentDirectory();
            String strNowPath = strPath.Replace("\\Cowain_AutoMotion\\bin\\x64\\Debug", "");
            // string name = "MES";
            //if (station == 1)
            //{
            //    name = "后龙门";
            //}
            try
            {
                //加载MES
                String strBasePath = strNowPath + "\\DataBaseData\\MESType\\";
                string[] dataFiles = Directory.GetFiles(strBasePath + "MES");
                for (int i = 0; i < dataFiles.Length; i++)
                {
                    string dataType = File.ReadAllText(dataFiles[i]);
                    string[] name1 = dataFiles[i].Split('\\');
                    string[] names = name1[name1.Length - 1].Split('.');
                    if (myMesMsg[station].Keys.Contains(names[0]) != true)
                    {
                        myMesMsg[station].Add(names[0], dataType);
                    }
                }
                //加载HIVE
                string[] dataFiles11 = Directory.GetFiles(strBasePath + "HIVE");
                for (int i = 0; i < dataFiles11.Length; i++)
                {
                    string dataType = File.ReadAllText(dataFiles11[i]);
                    string[] name1 = dataFiles11[i].Split('\\');
                    string[] names = name1[name1.Length - 1].Split('.');
                    if (myHIVEMsg[station].Keys.Contains(names[0]) != true)
                    {
                        myHIVEMsg[station].Add(names[0], dataType);
                    }
                }
                //加载远程模式下的HIVE文件
                string[] dataFiles22 = Directory.GetFiles(strBasePath + "HIVE\\Remote Qualification");
                for (int i = 0; i < dataFiles22.Length; i++)
                {
                    string dataType = File.ReadAllText(dataFiles22[i]);
                    string[] name1 = dataFiles22[i].Split('\\');
                    string[] names = name1[name1.Length - 1].Split('.');
                    if (myHIVEForRemoteQualificationMsg[station].Keys.Contains(names[0]) != true)
                    {
                        myHIVEForRemoteQualificationMsg[station].Add(names[0], dataType);
                    }
                }
            }
            catch
            {

            }
            //实例化上传的类
            POSTClass pOSTClass = POSTClass.CreateInstance();
        }
        public static void ReadParams()
        {
            MESLXData.ReaderParams(Program.StrBaseDic, "MESParamData", ref MESLXData);
            if(MESLXData==null)
            {
                MESLXData=new MESLXData();
                MESLXData.ReadBufferDate(Program.StrBaseDic, "MESParamData", ref MESLXData);
            }
        }
        public static void initial()
        {
            readMESAndHIVEDataType(0);
            ReadParams();
        }
    }
    /// <summary>
    /// 立讯Mes参数
    /// </summary>
    public class MESLXData : JsonHelper
    {
        [Category("1 MES相关参数"), DisplayName("MESIP 地址"),  Description("MESIP 地址,用于ping，测试通断")]
        public string StrMesIP { get; set; } = "10.32.16.97";

        [Category("1 MES相关参数"), DisplayName("PDCA IP 地址"), Description("PDCA 地址,用于ping，测试通断")]
        public string StrPDCAIP { get; set; } = "169.254.1.10";

        [Category("1 MES相关参数"), DisplayName("HIVE IP 地址"), Description("HIVE 地址,用于ping，测试通断")]
        public string StrHIVEIP { get; set; } = "10.0.0.2";


        [Category("1 MES相关参数"), DisplayName("checkSN的URL"), Description("checkSN的URL")]
        public string AssyCheckURL { get; set; } = "http://10.32.16.97/api/Assy/AssyCheck";

        [Category("1 MES相关参数"), DisplayName("过站的URL"), Description("过站的URL")]
        public string AssyGoURL { get; set; } = "http://10.32.16.97/api/Assy/AssyGo";

        [Category("1 MES相关参数"), DisplayName("UC获取SN的URL"), Description("UC获取SN的URL")]
        public string GetCmdResURL { get; set; } = "http://10.32.16.97/api/Assy/GetCmdResult";

        [Category("1 MES相关参数"), DisplayName("Bobcat的URL"), Description("Bobcat的URL")]
        public string BobcatURL { get; set; } = "http://10.32.16.137/bobcat";
        [Category("1 MES相关参数"), DisplayName("上传测试数据的URL"), Description("上传测试数据的URL")]
        public string ColTestDataURL { get; set; } = "http://10.32.16.97/api/Assy/CollectTestData";


        [Category("1 MES相关参数"), DisplayName("数据搜集指令（CollectTestData）指令"), Description("数据搜集指令（CollectTestData）指令")]
        public string AssyGoCode { get; set; } = "REC";


        [Category("1 MES相关参数"), DisplayName("设备号（设备自己的名称）"), Description("设备号（设备自己的名称）")]
        public string StrStationID { get; set; } = "KSH26NMPLH365M0100005";


        [Category("1 MES相关参数"), DisplayName("站号（设备在流水线上的名称）"), Description("站号（设备在流水线上的名称）")]
        public string terminalName { get; set; } = "ITKS_E03-4FT-01A_1_STATION130";



        [Category("1 MES相关参数"), DisplayName("获取SN的dat"), Description("获取SN的dat")]
        public string StrGetSNDat { get; set; } = "CMD=ATT&P=RFID_GET_SN";


        [Category("1 MES相关参数"), DisplayName("Check SN的dat"), Description("Check SN的dat")]
        public string StrCheckSNDat { get; set; } = "CMD=UOP";


        [Category("1 MES相关参数"), DisplayName("上传SN的dat"), Description("上传SN的dat")]
        public string StrLinkSNDat { get; set; } = "CMD=ADD";


        [Category("1 MES相关参数"), DisplayName("Line"), Description("Line")]
        public string StrLine { get; set; } = "D1-3F-H26-OFF-L02";

        [Category("1 MES相关参数"), DisplayName("项目号"), Description("项目号")]
        public string ProjectCode { get; set; } = "B788";


        [Category("1 MES相关参数"), DisplayName("empNo"), Description("empNo")]
        public string empNo { get; set; } = "AUTOH38";

        [Category("2 上传图片相关"), DisplayName("压缩包要存储的本地全路径"), Description("压缩包要存储的本地全路径")]
        public string StrLocalPictureZipPath { get; set; } = @"F:\\Image-Zip";

        [Category("2 上传图片相关"), DisplayName("压缩包要存储的本地相对路径"), Description("压缩包要存储的本地相对路径")]
        public string StrLocalPictureZipPathName { get; set; } = "Image-Zip";

        [Category("2 上传图片相关"),DisplayName("StrRemotepath"), Description("StrRemotepath")]
        public string StrRemotepath { get; set; } = "abg_pic";


        [Category("2 上传图片相关"), DisplayName("StrImgUrl"), Description("StrImgUrl")]
        public string StrImgUrl { get; set; } = "http://10.103.6.30/UploadFileData/abgService?wsdl";


        [Category("2 上传图片相关"), DisplayName("StrStationName"), Description("StrStationName")]
        public string StrStationName { get; set; } = "AE_38";


        [Category("2 上传图片相关"), DisplayName("相机暂存图片的文件夹"), Description("相机暂存图片的文件夹")]
        public string StrCCDPicturePath { get; set; } = @"F:\Image";


        [Category("2 上传图片相关"), DisplayName("StrCopyRePath"), Description("StrCopyRePath")]
        public string StrCopyRePath { get; set; } = "Y:";


        [Category("2 上传图片相关"), DisplayName("StrPDCAImagePath"), Description("StrPDCAImagePath")]
        public string StrPDCAImagePath { get; set; } = @"smb://169.254.1.11/";


        [Category("2 上传图片相关"), DisplayName("StrImagetimeDelayS"), Description("StrImagetimeDelayS")]
        public string StrImagetimeDelayS { get; set; } = "";


        [Category("3 MacMini相关参数"), DisplayName("MacMini 地址"), Description("MacMini 地址")]
        public string StrMiniIP { get; set; } = "169.254.1.10";

        [Category("3 MacMini相关参数"), DisplayName("MacMini 端口号"), Description("MacMini 端口号")]
        public string StrMiniPort { get; set; } = "1111";


        [Category("3 MacMini相关参数"), DisplayName("电脑的账号"), Description("电脑的账号")]
        public string StrUser { get; set; } = "user";


        [Category("3 MacMini相关参数"), DisplayName("电脑的密码"), Description("电脑的密码")]
        public string StrPassWord { get; set; } = "123";

        [Category("4 Hive相关参数"), DisplayName("machinedataURL"), Description("machinedataURL")]
        public string machineDataURL { get; set; } = "http://10.0.0.2:5008/v5/capture/machinedata";

        [Category("4 Hive相关参数"), DisplayName("errorDataURL"), Description("errorDataURL")]
        public string errorDataURL { get; set; } = "http://10.0.0.2:5008/v5/capture/errordata";

        [Category("4 Hive相关参数"), DisplayName("软件版本"), Description("软件版本")]
        public string SW_Version { get; set; } = "COW_4.1.0.0_230706_POR";

        [Category("4 Hive相关参数"), DisplayName("limits_version"), Description("limits_version")]
        public  string limits_version { get; set; } = "CWN_1.1.0.0_230306_POR";

    }
}
