using DevExpress.XtraCharts.Native;
using Get_SHA1_Help;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cowain_AutoMotion.Flow.Hive
{
    class HIVEDataDefine
    {

        /// <summary>
        /// HIVE选配功能启禁用集合
        /// </summary>
        public static HIVE_CheckClass Hive_machineState = new HIVE_CheckClass();

        /// <summary>
        /// HIVE选配功能数据集合
        /// </summary>
        public static HIVE_SHA1_DataClass HIVE_sha1_Data = new HIVE_SHA1_DataClass();

        private static string temp_Main_SW_SHA1_Name = "0";
        private static string temp_Vision_SW_SHA1_Name = "0";
        private static string temp_Vision_Project_SHA1_Name = "0";
        private static string CD_SHA1_Name = "0";
        /// <summary>
        /// 储存Limit相关信息
        /// </summary>
        public static List<Limit> limitdata = new List<Limit>();
        public static string Get_SHA1(string filepath)
        {
            string StrSHA1_NAME = "";
            if (File.Exists(filepath))
            {
                try
                {
                    string SHA1NAME = Get_SHA1_Help_Class.GetFileSHA1Name(filepath);
                    string[] tempsha1name = SHA1NAME.Replace("哈希:", "$").Split('$');
                    string[] tempsha1name002 = tempsha1name[1].Replace("\r\n", "$").Split('$');
                    StrSHA1_NAME = tempsha1name002[1];
                }
                catch
                {
                    StrSHA1_NAME = "";

                }
            }
            return StrSHA1_NAME;
        }
        public static void Init()
        {
            //if (HIVEDataDefine.Hive_machineState.Start_Get_Sha1)
            {
                temp_Main_SW_SHA1_Name = Get_SHA1(HIVE_sha1_Data.Main_SW_Path);
                temp_Vision_SW_SHA1_Name = Get_SHA1(HIVE_sha1_Data.Vision_SW_Path);
                temp_Vision_Project_SHA1_Name = Get_SHA1(HIVE_sha1_Data.Vision_Project_Path);
                CD_SHA1_Name = Get_SHA1(HIVE_sha1_Data.CD_Path);

                if (temp_Main_SW_SHA1_Name != HIVE_sha1_Data.Main_SW_SHA1_Name)
                {
                    Save_SHA1_Change("主软件SHA1签名变更为:  " + temp_Main_SW_SHA1_Name);
                }


                if (temp_Vision_SW_SHA1_Name != HIVE_sha1_Data.Vision_SW_SHA1_Name)
                {
                    Save_SHA1_Change("视觉软件SHA1签名变更为:  " + temp_Vision_SW_SHA1_Name);
                }



                if (temp_Vision_Project_SHA1_Name != HIVE_sha1_Data.Vision_Project_SHA1_Name)
                {
                    Save_SHA1_Change("视觉项目工程文件的SHA1签名:  " + temp_Vision_Project_SHA1_Name);
                }

                if (CD_SHA1_Name != HIVE_sha1_Data.CD_SHA1_Name)
                {
                    Save_SHA1_Change("视觉项目工程文件的SHA1签名:  " + CD_SHA1_Name);
                }

                HIVE_sha1_Data.Main_SW_SHA1_Name = temp_Main_SW_SHA1_Name;
                HIVE_sha1_Data.Vision_SW_SHA1_Name = temp_Vision_SW_SHA1_Name;
                HIVE_sha1_Data.Vision_Project_SHA1_Name = temp_Vision_Project_SHA1_Name;
                HIVE_sha1_Data.CD_SHA1_Name = CD_SHA1_Name;
            }
        }
        /// <summary>
        /// HIVE选配功能启禁用集合
        /// </summary>
        public class HIVE_CheckClass : JsonHelper
        {
            /// <summary>
            /// 开启获取 SHA1 循环
            /// </summary>
            public bool Start_Get_Sha1 = false;
            #region 自动上传Hive Idle状态
            /// <summary>
            /// 是否启用自动上传Idle状态
            /// </summary>
            public bool[] USEAutoSendidle = new bool[] { false, false };
            /// <summary>
            /// 自动上传Idle状态特定时间
            /// </summary>
            public string[] HIVEAutoSendidleTime = new string[] { "0:0", "0:0" };
            /// <summary>
            /// 自动上传Idle状态持续时间
            /// </summary>
            public string[] HIVEAutoSendidleKeepTime = new string[] { "8", "8" };
            #endregion

            #region 自动上传日常点检
            /// <summary>
            /// 是否启用自动上传Idle状态
            /// </summary>
            public bool[] USEAutoDailyCheckup = new bool[] { false, false };
            /// <summary>
            /// 自动上传Idle状态特定时间
            /// </summary>
            public string[] HIVEAutoDailyCheckupTime = new string[] { "0:0", "0:0" };
            #endregion

            #region 手动发送报警状态
            /// <summary>
            /// 是否手动发送报警状态,false为未发送，true为已发送
            /// </summary>
            public bool IsSendErrorDown = false;
            /// <summary>
            /// 手动发送报警状态的ErrorDownSelectedIndex号
            /// </summary>
            public int ErrorDownSelectedIndex = 0;
            /// <summary>
            /// 手动发送报警状态的ErrorMessageSelectedIndex号
            /// </summary>
            public int ErrorMessageSelectedIndex = 0;
            public string ErrorDownstarttime = "";
            public string ErrorDownstoptime = "";
            #endregion
        }
        public class HIVE_SHA1_DataClass : JsonHelper
        {
            [Category("1 哈希值相关参数"), DisplayName("主软件可执行文件路径"), Description("主软件可执行文件路径")]
            public string Main_SW_Path { get; set; } = Application.StartupPath + "\\Cowain_AutoMotion.exe";

            [Category("1 哈希值相关参数"), DisplayName("视觉软件可执行文件路径"), Description("视觉软件可执行文件路径")]
            public string Vision_SW_Path { get; set; } = "0";

            [Category("1 哈希值相关参数"), DisplayName("视觉项目工程文件路径"), Description("视觉项目工程文件路径")]
            public string Vision_Project_Path { get; set; } = "0";

            [Category("1 哈希值相关参数"), DisplayName("主软件可执行文件的SHA1签名"), Description("主软件可执行文件的SHA1签名")]
            public string Main_SW_SHA1_Name { get; set; } = "0";

            [Category("1 哈希值相关参数"), DisplayName("视觉软件可执行文件的SHA1签名"), Description("视觉软件可执行文件的SHA1签名")]
            public string Vision_SW_SHA1_Name { get; set; } = "0";

            [Category("1 哈希值相关参数"), DisplayName("视觉项目工程文件的SHA1签名"), Description("视觉项目工程文件的SHA1签名")]
            public string Vision_Project_SHA1_Name { get; set; } = "0";

            [Category("1 哈希值相关参数"), DisplayName("获取SHA1的循环时间"), Description("获取SHA1的循环时间")]
            public string Get_Sha1_Loop_Time { get; set; } = "1000";

            [Category("1 哈希值相关参数"), DisplayName("视觉项目工程文件的路径"), Description("视觉项目工程文件的路径")]
            public string CD_Path { get; set; } = "0";

            [Category("1 哈希值相关参数"), DisplayName("视觉项目工程文件的SHA1签名"), Description("视觉项目工程文件的SHA1签名")]
            public string CD_SHA1_Name { get; set; } = "0";
        }
        public static void ReadParams()
        {

            Hive_machineState.ReaderParams(Program.StrBaseDic, "HIVE_CheckClass", ref Hive_machineState);
            if (Hive_machineState == null)
            {
                Hive_machineState = new HIVE_CheckClass();
                Hive_machineState.ReadBufferDate(Program.StrBaseDic, "HIVE_CheckClass", ref Hive_machineState);
            }
            Hive_machineState.SetSaveFile(Program.StrBaseDic, "HIVE_CheckClass", Hive_machineState);


            HIVE_sha1_Data.ReaderParams(Program.StrBaseDic, "HIVE_SHA1_DataClass", ref HIVE_sha1_Data);
            if (HIVE_sha1_Data == null)
            {
                HIVE_sha1_Data = new HIVE_SHA1_DataClass();
                HIVE_sha1_Data.ReadBufferDate(Program.StrBaseDic, "HIVE_SHA1_DataClass", ref HIVE_sha1_Data);
            }
            HIVE_sha1_Data.SetSaveFile(Program.StrBaseDic, "HIVE_SHA1_DataClass", HIVE_sha1_Data);
            Init();
        }


        public static void Save_SHA1_Change(string result)
        {
            try
            {

                string fileName;
                fileName = string.Format("{0}.txt", DateTime.Now.ToString("yyyy_MM_dd"));
                string outputPath = @"D:\DATA\SHA1签名变更记录";
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
                    sw.WriteLine(DateTime.Now.ToString("HH:mm:ss") + result + "\r\n");
                    sw.Close();
                    fs.Close();

                }
                else
                {
                    fs = new System.IO.FileStream(fullFileName, System.IO.FileMode.Append, System.IO.FileAccess.Write, FileShare.Read);
                    //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
                    sw = new StreamWriter(fs, System.Text.Encoding.Default);
                    sw.WriteLine(DateTime.Now.ToString("HH:mm:ss") + result + "\r\n");
                    sw.Close();
                    fs.Close();
                }
            }
            catch
            {


            }
        }

    }
    public class HiveMachineData
    {
        public string unit_sn = "";
        //public string fixture_id = "";
        public Dictionary<string, string> serials = new Dictionary<string, string>();
        public bool pass = false;
        public string input_time = DateTime.Now.ToString();
        public string output_time = DateTime.Now.ToString();
        public Dictionary<string, object> data = new Dictionary<string, object>();

        public Dictionary<string, limitdata> limit = new Dictionary<string, limitdata>();
        public blob[] blobs = new blob[1];
    }
    public class limitdata
    {
        public double upper_limit;
        public double lower_limit;
    }
    public class blob
    {
        public string file_name;
    }

    public class Limit
    {
        public string Name;
        public bool Enable;
        public double Standard;
        public double lower_limit;
        public double upper_limit;

    }
}
