using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Threading;
using MotionBase;
using ToolTotal_1;
using System.Drawing;
using Cowain_AutoMotion.Flow;
using Cowain_AutoMotion;
using AutoBackup;
using static Cowain_AutoMotion.SQLSugarHelper;
using System.ComponentModel;
using Cowain_Machine.Flow;
using Cowain_AutoMotion.Flow._2Work;
using Cowain_AutoMotion.Flow.Hive;
using Cowain_AutoMotion.Flow.Common;

namespace Cowain_Machine
{
    /// <summary>
    /// 设备运行参数类：包含功能启用禁用 设备信息参数 流程控制变量等信息
    /// </summary>
    public class MachineDataDefine
    {
        /// <summary>
        /// 拍照NG原因
        /// </summary>
        public static string PhotoNGMSG = "";
        public static string PhotoMSG = "";
        public static string loginstr = "";//存储mes收发信息
        public static string str1 = "";
        public static string pdcastr = "";
        public static string str = "";
        public static bool 是否感应 = false;
        public static bool NGmac = false;
        /// <summary>
        /// 连三不连五 给点胶机发不做料信号
        /// </summary>
        public static bool StopNG= false;
        /// <summary>
        /// Hive网卡Mac地址
        /// </summary>
        public static string hive_mac = "00:00:00:00:00:00";
        /// <summary>
        /// 界面显示数值
        /// </summary>
        public static string[] ShowPram = new string[19];
        /// <summary>
        ///界面是否可以显示数值
        /// </summary>
        public static bool ShowVaule = false;
        /// <summary>
        /// 自动备份功能集合 2022.02.21新增
        /// </summary>
        public static AutoBackupClass AutoBackup = new AutoBackupClass();
        /// <summary>
        /// 自动备份工具 2022.02.21新增
        /// </summary>
        public static BackupHelper backup;
        /// <summary>
        /// 功能启用禁用合集
        /// </summary>
        public static MachineState machineState = new MachineState();
        /// <summary>
        /// 设备信息参数合集
        /// </summary>
        public static MachineConfigClass MachineCfgS = new MachineConfigClass();

        public static SettingData settingData = new SettingData();
        /// <summary>
        /// 窗口是否加载完成
        /// </summary>
        public static bool IsMainFormLoading = false;
        /// <summary>
        /// 设备流程
        /// </summary>
        public static MachineLightEumn MachineLightEumn = MachineLightEumn.程序待回原;
        /// <summary>
        /// 是否是自动运行状态
        /// </summary>
        public static bool IsAutoing = false;
        public static bool IsHomeComplete = false;
        public static bool IsCycleStop = false;
        public static bool IsFormOpen = false;
        /// <summary>
        /// 停止运行
        /// </summary>
        public static bool Button_Stop = false;
        /// <summary>
        /// 是否有报警
        /// </summary>
        public static bool IsAlarmShow = false;
        public static Dictionary<string, Image> ImagesDic = new Dictionary<string, Image>();
        public static clsMachine pMachine;
        private static bool b_UseLAD1 = false;
        public static MiSuMiControl miSuMiControl;
        public static HardWareConfigClass HardwareCfg = new HardWareConfigClass();

        /// <summary>
        /// 螺丝批NG
        /// </summary>
        public static bool screwNG = false;

        /// <summary>
        /// 机械手宕机信号
        /// </summary>
        public static bool RobotDownError = false;
        /// <summary>
        /// 机械手宕机弹一次弹窗
        /// </summary>
        public static bool RobotDownForm = true;
        public static bool Authorizeis = false;

        /// <summary>
        /// 流程控制变量集合
        /// </summary>
        public static MachineControl MachineControlS = new MachineControl();
        /// <summary>
        /// 当前lad模式结束下料
        /// </summary>
        public static bool downLad = false;
        /// <summary>
        /// 组装超时
        /// </summary>
        public static bool timeout = false;
        /// <summary>
        /// 电批标定
        /// </summary>
        public static bool electriccalib = false;

        public static string  msg = MachineDataDefine.msg = $" User:{MachineDataDefine.Login_Name} Function:{MachineDataDefine.Login_Function} UID:{MachineDataDefine.Login_CardID} Authorize:{MachineDataDefine.Authorize_Name} Function:{MachineDataDefine.Authorize_Function} UID:{MachineDataDefine.Authorize_CardID} ";
        #region 登录信息
        /// <summary>
        /// 登录者名称
        /// </summary>
        public static string Login_Name = "null";
        /// <summary>
        /// 登录者职能
        /// </summary>
        public static string Login_Function = "null";
        /// <summary>
        /// 登录者卡号 
        /// </summary>
        public static string Login_CardID = "null";
        /// <summary>
        /// 授权者名称
        /// </summary>
        public static string Authorize_Name = "null";
        /// <summary>
        /// 授权者职能
        /// </summary>
        public static string Authorize_Function = "null";
        /// <summary>
        /// 授权者卡号
        /// </summary>
        public static string Authorize_CardID = "null";
        #endregion
        public MachineDataDefine()
        {
            try
            {
                String strPath = System.IO.Directory.GetCurrentDirectory();
                String strNowPath = strPath.Replace("\\Cowain_AutoMotion\\bin\\x64\\Debug", "");
                String pictPath = strNowPath + "\\DataBaseData\\Picture";
                string[] picts = Directory.GetFiles(pictPath);
                ImagesDic.Clear();
                for (int i = 0; i < picts.Length; i++)
                {
                    using (FileStream rs = new FileStream(picts[i], FileMode.Open, FileAccess.Read))
                    {
                        Image start = Image.FromStream(rs);
                        string[] name1 = picts[i].Split('\\');
                        string[] names = name1[name1.Length - 1].Split('.');
                        if (ImagesDic.Keys.Contains(names[0]) != true)
                        {
                            ImagesDic.Add(names[0], start);
                        }
                    }
                }
                StationImage = ImagesDic[MachineDataDefine.settingData.Station];
            }
            catch
            {
            }
        }
        public static bool b_UseLAD
        {
            get
            {
                return b_UseLAD1;
            }
            set
            {
                if (b_UseLAD1 == false && value == true)
                {
                    MachineDataDefine.测试时间 = DateTime.Now.ToString("yyyyMMddHHmmss");
                }
                b_UseLAD1 = value;
            }
        }
        public static int LADModel = -1;
        public static int LADOPID = 1;
        public static string ZipFilePath;
        /// <summary>
        /// 加载参数
        /// </summary>
        public static void ReadParams()
        {
            JsonHelper.LoadConfigParams("Limit", out HIVEDataDefine.limitdata);
            #region 自动备份功能读取参数 2022.02.21新增
            AutoBackup.ReaderParams(Program.StrBaseDic, "AutoBackupClass", ref AutoBackup);
            if (AutoBackup == null)
            {
                AutoBackup = new AutoBackupClass();
                AutoBackup.ReadBufferDate(Program.StrBaseDic, "AutoBackupClass", ref AutoBackup);
            }
            AutoBackup.SetSaveFile(Program.StrBaseDic, "AutoBackupClass", AutoBackup);
            #endregion
            machineState.ReaderParams(Program.StrBaseDic, "MachineState", ref machineState);
            if (machineState == null)
            {
                machineState = new MachineState();
                machineState.ReadBufferDate(Program.StrBaseDic, "MachineState", ref machineState);
            }
            machineState.SetSaveFile(Program.StrBaseDic, "MachineState", machineState);
            
            HardwareCfg.ReaderParams(Program.StrBaseDic, "HardWareConfigClass", ref HardwareCfg);
            if (HardwareCfg == null)
            {
                HardwareCfg = new HardWareConfigClass();
                HardwareCfg.ReadBufferDate(Program.StrBaseDic, "HardWareConfigClass", ref HardwareCfg);
            }
            HardwareCfg.SetSaveFile(Program.StrBaseDic, "HardWareConfigClass", HardwareCfg);

            settingData.ReaderParams(Program.AccessControl, "SettingData", ref settingData);
            if (settingData == null)
            {
                settingData = new SettingData();
                settingData.ReadBufferDate(Program.AccessControl, "SettingData", ref settingData);
            }
            settingData.SetSaveFile(Program.AccessControl, "SettingData", settingData);

            if (MachineDataDefine.HardwareCfg.Remote == 1)
            {
                MachineDataDefine.machineState.b_UseRemoteQualification = true;
            }
            else
            {
                MachineDataDefine.machineState.b_UseRemoteQualification = false;
            }
            //不保存的配置
            //machineState.b_UseRemoteQualification = false;
            //------------

            MachineCfgS.ReaderParams(Program.StrBaseDic, "MachineConfigClass", ref MachineCfgS);
            if (MachineCfgS == null)
            {
                MachineCfgS = new MachineConfigClass();
                MachineCfgS.ReadBufferDate(Program.StrBaseDic, "MachineConfigClass", ref MachineCfgS);
            }
            MachineCfgS.SpecClasss.Clear();
            if (MachineCfgS.SpecClasss.Count == 0)
            {
                SpecClass specClassOutX = new SpecClass("OutX", true, -999, 999, 0);
                MachineCfgS.SpecClasss.Add(specClassOutX);
                SpecClass specClassOutY = new SpecClass("OutY", true, -999, 999, 0);
                MachineCfgS.SpecClasss.Add(specClassOutY);
                SpecClass specClassOutR = new SpecClass("OutR", true, -999, 999, 0);
                MachineCfgS.SpecClasss.Add(specClassOutR);

                SpecClass specClassOffsetX = new SpecClass("OffsetX", true, -999, 999, 0);
                MachineCfgS.SpecClasss.Add(specClassOffsetX);
                SpecClass specClassOffsetY = new SpecClass("OffsetY", true, -999, 999, 0);
                MachineCfgS.SpecClasss.Add(specClassOffsetY);
                SpecClass specClassOffsetR = new SpecClass("OffsetR", true, -999, 999, 0);
                MachineCfgS.SpecClasss.Add(specClassOffsetR);

                SpecClass specClassP1 = new SpecClass("P1", true, -999, 999, 0);
                MachineCfgS.SpecClasss.Add(specClassP1);
                SpecClass specClassP2 = new SpecClass("P2", true, -999, 999, 0);
                MachineCfgS.SpecClasss.Add(specClassP2);
                SpecClass specClassP3 = new SpecClass("P3", true, -999, 999, 0);
                MachineCfgS.SpecClasss.Add(specClassP3);
                SpecClass specClassP4 = new SpecClass("P4", true, -999, 999, 0);
                MachineCfgS.SpecClasss.Add(specClassP4);
                SpecClass specClassP5 = new SpecClass("P5", true, -999, 999, 0);
                MachineCfgS.SpecClasss.Add(specClassP5);
                SpecClass specClassP6 = new SpecClass("P6", true, -999, 999, 0);
                MachineCfgS.SpecClasss.Add(specClassP6);
                SpecClass specClassP7 = new SpecClass("P7", true, -999, 999, 0);
                MachineCfgS.SpecClasss.Add(specClassP7);
                SpecClass specClassP8 = new SpecClass("P8", true, -999, 999, 0);
                MachineCfgS.SpecClasss.Add(specClassP8);

                SpecClass specClassShiftP1P5 = new SpecClass("ShiftP1P5", true, -999, 999, 0);
                MachineCfgS.SpecClasss.Add(specClassShiftP1P5);
                SpecClass specClassShiftP2P4 = new SpecClass("ShiftP2P6", true, -999, 999, 0);
                MachineCfgS.SpecClasss.Add(specClassShiftP2P4);
                SpecClass specClassShiftP3P7 = new SpecClass("ShiftP3P7", true, -999, 999, 0);
                MachineCfgS.SpecClasss.Add(specClassShiftP3P7);
                SpecClass specClassShiftP4P8 = new SpecClass("ShiftP4P8", true, -999, 999, 0);
                MachineCfgS.SpecClasss.Add(specClassShiftP4P8);

                SpecClass specClassBestVaule = new SpecClass("最优解1", true, -999, 999, 0);
                MachineCfgS.SpecClasss.Add(specClassBestVaule);
            }
           // MachineDataDefine.MachineCfgS.RelMovePoints.Clear();
            if (MachineCfgS.RelMovePoints.Count == 0)
            {
                for (int i = 0; i < 14; i++)
                {
                    MachineCfgS.RelMovePoints.Add(new Cowain_Machine.RelMovePoint(0, 0, 0));
                }
            }
            if (MachineCfgS.QiuMovePoints.Count == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    MachineCfgS.QiuMovePoints.Add(new Cowain_Machine.RelMovePoint(0, 0, 0));
                }
            }
            MachineCfgS.SetSaveFile(Program.StrBaseDic, "MachineConfigClass", MachineCfgS);

        }

        /// <summary>
        /// 统计产品进入的时间与上一片下料的时间，差值大于8，则重新开始CT统计
        /// </summary>
        public static DateTime[] firstInProductTimes = new DateTime[] { DateTime.Now, DateTime.Now };
        /// <summary>
        /// 相机复检结果
        /// </summary>
        public static string StrRecheckResult = "OK";

        /// <summary>
        /// 0未进行人工复判，1人工复判OK,2人工复判NG
        /// </summary>
        public static string StrManulStatus = "0";

        /// <summary>
        /// 从下料开始统计单产品CT
        /// </summary>
        public static string StrProductCT = "0";
        /// <summary>
        /// 点击启动按钮时置为true，重新开始统计CT
        /// </summary>
        public static bool[] IsFirstInProduct = new bool[] { true, true };
        /// <summary>
        /// 记录程序主流程运行步骤
        /// </summary>
        public static int[] StrDisStAutoStep = new int[2] { -1, -1 };
        /// <summary>
        /// 记录擦胶流程运行步骤
        /// </summary>
        public static int[] StrOutGlueStep = new int[2] { -1, -1 };

        /// <summary>
        /// 三色灯相关变量1
        /// </summary>
        public static bool IsReset = false;
        /// <summary>
        /// 三色灯相关变量1
        /// </summary>
        public static bool IsUnLoading = false;
        /// <summary>
        /// 当前工站的图片
        /// </summary>
        public static Image StationImage;
        public static bool isLeft = false;
        public static bool isRight = false;
        /// <summary>
        /// 设备使用者等级
        /// </summary>
        public static Sys_Define.enPasswordType m_LoginUser;
        /// <summary>
        /// 设备使用者名称
        /// </summary>
        public static string m_LoginUserName = string.Empty;
        /// <summary>
        /// 登录用户卡号
        /// </summary>
        public static string m_LoginCardID = string.Empty;
        /// <summary>
        /// 用户卡号
        /// </summary>
        public static string m_CardID = string.Empty;
        /// <summary>
        /// 旧SN
        /// </summary>
        public static string m_OldSN = string.Empty;
        /// <summary>
        /// 新SN
        /// </summary>
        public static string m_NewSN = string.Empty;
        public static bool m_bUiTimerEnable = false;

        //-------------------------------------
        /// <summary>
        /// 语言类型
        /// </summary>
        public static int LanguageStyle = Convert.ToInt16(GetLanguage.LanguageNumber);
        /// <summary>
        /// 窗体是否已经切换过
        /// </summary>
        public static bool[] FormVisibled;
        private static object locker = new object();
        private static bool isFormChanging = false;
        /// <summary>
        /// 记录第一次作料
        /// </summary>
        public static bool FirstProduct = true;

        /// <summary>
        /// 远程记录第一次作料
        /// </summary>
        public static bool remoteFirstProduct = false;

        public static string 测试时间 = DateTime.Now.ToString("yyyyMMddHHmmss");

        /// <summary>
        /// SettingFOrm界面控件
        /// </summary>
        public static List<Control> controlSettingForm = new List<Control>();
        /// <summary>
        /// 是否正在切换界面
        /// </summary>
        public static bool IsFormChanging
        {
            get
            {
                lock (locker)
                {
                    return isFormChanging;
                }
            }
            set
            {
                lock (locker)
                {
                    isFormChanging = value;
                }
            }
        }
    }
    /// <summary>
    /// 设备信息集合
    /// </summary>
    public class MachineConfigClass : JsonHelper
    {
       
        [Category("1 设备相关参数"), DisplayName("图片保存天数"), Description("图片保存天数")]
        public int IntFileSaveDays { get; set; } = 10;
        [Category("1 设备相关参数"), DisplayName("自动登出时间"), Description("自动登出时间")]
        public int LogoutTime { get; set; } = 5;
        [Category("1 设备相关参数"), DisplayName("工厂名称"), Description("工厂名称")]
        public MachineFactory MachineFactoryEumn { get; set; } = MachineFactory.ICTKS;
        
        [Category("1 设备相关参数"), DisplayName("设备数量"), Description("设备数量")]
        public int QPL { get; set; } = 1;
        
        [Category("4 设备Spec参数"), DisplayName("每个点的spec"), Description("每个点的spec")]
        public List<SpecClass> SpecClasss { get; set; } = new List<SpecClass>();
        [Category("5 设备轴标定参数"), DisplayName("相对运动点位"), Description("相对运动点位")]
        public List<RelMovePoint> RelMovePoints { get; set; } = new List<RelMovePoint>();

        [Category("6 设备球标定参数"), DisplayName("相对运动点位"), Description("相对运动点位")]
        public List<RelMovePoint> QiuMovePoints { get; set; } = new List<RelMovePoint>();

        [Category("7 视觉测试指令"), DisplayName("测试指令集合"), Description("测试指令集合")]
        public List<CCDCMD> CMDLists { get; set; } = new List<CCDCMD>();

        [Category("膨胀气缸动作次数"), DisplayName("膨胀气缸动作次数"), Description("膨胀气缸动作次数")]
        public int CylinderCount { get; set; } = 0;
        [Category("当前膨胀气缸动作次数"), DisplayName("当前膨胀气缸动作次数"), Description("当前膨胀气缸动作次数")]
        public int CurrentCylinderCount { get; set; } = 0;
        [Category("UC长度"), DisplayName("UC长度"), Description("UC长度")]
        public int UCLength { get; set; } = 18;
        [Category("SN长度"), DisplayName("SN长度"), Description("SN长度")]
        public int SNLength { get; set; } = 18;
    }
    /// <summary>
    /// 设备启禁用集合
    /// </summary>
    public class MachineState : JsonHelper
    {
        /// <summary>
        /// 使用CCD
        /// </summary>
        public bool b_UseCCD = false;
        /// <summary>
        /// 使用MES
        /// </summary>
        public bool b_UseMes = false;
        /// <summary>
        /// 使用扫码枪
        /// </summary>
        public bool b_UseScaner = false;
        /// <summary>
        ///启用调机模式
        /// </summary>
        public bool b_UseTestRun = false;
        /// <summary>
        ///弹窗显示
        /// </summary>
        public bool Isdia_ShowModelShow = false;
        /// <summary>
        /// 报警次数，用于卡控报警上传至HIVE
        /// </summary>
        public int UpLoadError = 0;
        /// <summary>
        /// 报警代号
        /// </summary>
        public string ErrorCode = "";
        /// <summary>
        /// 使用Hive
        /// </summary>
        public bool b_UseHive = false;
        /// <summary>
        ///使用安全光栅检查功能
        /// </summary>
        public bool b_UseGratingCheck = false;
        /// <summary>
        ///  使用PDCA上传
        /// </summary>
        public bool b_UsePDCA = false;
        /// <summary>
        /// 是否上传照片  禁用：true  启用：false
        /// </summary>
        public bool b_UseSendPic = false;

        /// <summary>
        /// 报警不停机  启用：true  禁用： false
        /// </summary>
        public bool ErrorNOStop = false;
        /// <summary>
        ///使用门开关检查功能
        /// </summary>
        public bool b_UseDoorCheck = false;
        /// <summary>
        /// 是否使用MES获取登录
        /// </summary>
        public bool b_UseMesLogin = false;
        /// <summary>
        /// 设备的运行速度
        /// </summary>
        public int machineSpeed = 30;
        /// <summary>
        /// 是否启用远程模式
        /// </summary>
        public bool b_UseRemoteQualification = false;
      
        /// <summary>
        /// 启用蜂鸣器
        /// </summary>
        public bool b_Usehummer = false;
        /// <summary>
        /// 使用扫码枪
        /// </summary>
        public bool b_isNGOK = false;

        /// <summary>
        /// 启用连三不连五
        /// </summary>
        public bool UseUpLoadMesErr = false;
        /// <summary>
        /// 连续失败次数
        /// </summary>
        public int ContinuousNum = 3;
        /// <summary>
        /// 一定时间内不连续失败次数
        /// </summary>
        public int NGNumInTime = 5;
        /// <summary>
        /// 时间范围（分钟）
        /// </summary>
        public int NGNumTime = 60;

        /// <summary>
        /// 是否启用UC获取SN
        /// </summary>
        public bool CheckUCgetSN = false;
    }
    /// <summary>
    /// 获取CCD点位
    /// </summary>
    public class CCDPoint
    {
        public double X;
        public double Y;
        public double R;
        public CCDPoint(double x1, double y1, double r1)
        {
            X = x1;
            Y = y1;
            R = r1;
        }
    }
    public enum MachineStation
    {
        主监控,
        归零
    }


    public enum MachineFactory
    {
        Goertek,
        ICTKS
    }
    public enum MachineLightEumn
    {
        程序待回原,
        程序回原中,
        程序回原成功待启动,
        程序启动中,
        程序停止中
    }
    public enum MachineStatusEumn
    {
        待料中,
        手动点胶中,
        自动做料中,
        自动排胶中,
    }
    public enum RunnerStatusEumn
    {
        等待进料,
        有料等待作料,
        有料作料完成,
    }
    public enum CalibrationStatus
    {
        标定开始,
        标定中,
        标定结束,
    }
    public enum MesStatus
    {
        UpLoadMesOK,
        UpLoadMesNG,
        CheckSN,
        Init
    }

    public enum AxisType
    {
        汇川,
        台达,
        original
    }
    /// <summary>
    /// 物料类型
    /// </summary>
    public enum MaterialType
    {
        Normal,
        E_SKU,
        M_SKU,
        Common
    }

    /// <summary>
    /// Setting配置信息
    /// </summary>
    public class SettingData : JsonHelper
    {
        /// <summary>
        /// 是否启用MES管控
        /// </summary>
        public bool AccessControlWithMES;
        /// <summary>
        ///  项目号
        /// </summary>
        public string ProjectCode;
        /// <summary>
        /// ⾃动注销登录⽤户的时间设置(分钟)
        /// </summary>
        public int AutomaticallySignOut;
        /// <summary>
        /// 项⽬所在地
        /// </summary>
        public string Site;
        /// <summary>
        ///产品类型
        /// </summary>
        public string ProdType;
        /// <summary>
        ///线体编号
        /// </summary>
        public string Line;
        /// <summary>
        /// 制程站ID
        /// </summary>
        public string Station;
        /// <summary>
        /// 机器QPL编号
        /// </summary>
        public string Machine;
        /// <summary>
        /// 语⾔
        /// </summary>
        public string Language;
        /// <summary>
        /// 选择集合
        /// </summary>
        public GeneralConfig GeneralConfig = new GeneralConfig();

    }
    public class GeneralConfig
    {
        /// <summary>
        /// 项⽬所在地
        /// </summary>
        public string[] Site;
        /// <summary>
        ///产品类型
        /// </summary>
        public string[] ProdType;
        /// <summary>
        ///线体编号
        /// </summary>
        public string[] Line;
        /// <summary>
        /// 制程站ID
        /// </summary>
        public string[] Station;
        /// <summary>
        /// 机器QPL编号
        /// </summary>
        public string[] Machine;
        /// <summary>
        /// 语⾔
        /// </summary>
        public string[] Language;

    }

    /// <summary>
    /// 自动备份功能集合 2022.02.21新增
    /// </summary>
    public class AutoBackupClass : JsonHelper
    {
        /// <summary>
        /// 启用自动备份
        /// </summary>
        public bool IsBackup = true;
        /// <summary>
        /// 源文件路径
        /// </summary>
        public string RootPath = "D:/";
        /// <summary>
        /// 源文件夹名
        /// </summary>
        public string DirName = "Cowain_AutoMotion";
        /// <summary>
        /// 备份文件保存路径
        /// </summary>
        public string BackupPath = "F:/备份/";
        /// <summary>
        /// 备份间隔(H)
        /// </summary>
        public int BackupTime = 24;
        /// <summary>
        /// 保留时间(H)
        /// </summary>
        public int RemoveTime = 168;
    }
    public class RelMovePoint
    {
        public double X { get; set; } = 0;
        public double Y { get; set; } = 0;
        public double R { get; set; } = 0;
        public RelMovePoint(double X1, double Y1, double R1)
        {
            X = X1;
            Y = Y1;
            R = R1;
        }
    }
    public class SpecClass
    {
        public string name { get; set; } = "";
        public bool b_Use { get; set; } = true;
        public double LSpec { get; set; } = -999.000;
        public double USpec { get; set; } = 999.000;
        public double SValue { get; set; } = 0;
        public SpecClass(string name1, bool b_Use1, double LSpec1, double USpec1, double SValue1)
        {
            name = name1;
            b_Use = b_Use1;
            LSpec = LSpec1;
            USpec = USpec1;
            SValue = SValue1;
        }
    }
    /// <summary>
    /// 流程控制变量集合
    /// </summary>
    public class MachineControl
    {
        /// <summary>
        /// 设备UI按钮停止
        /// </summary>
        public bool Button_Stop = false;

        /// <summary>
        /// 设备IO按钮停止
        /// </summary>
        public bool Button_Stop_IO = false;
        /// <summary>
        /// 设备是否自动
        /// </summary>
        public bool IsAutoing = false;

        /// <summary>
        /// 设备是否已经回原点
        /// </summary>
        public bool IsHomeComplete = false;

        /// <summary>
        /// 设备循环停止
        /// </summary>
        public bool IsCycleStop = false;
        /// <summary>
        /// 主窗口加载完成信号
        /// </summary>
        public bool IsMainFormLoading = false;
        /// <summary>
        /// 控制面板灯的状态
        /// </summary>
        public MachineLightEumn MachineLightEumn = MachineLightEumn.程序待回原;
        /// <summary>
        /// 报警时的单例变量
        /// </summary>
        public bool IsAlarmShow = false;
        /// <summary>
        /// 判断窗口是否被打开
        /// </summary>
        public bool IsFormOpen = false;

        #region   连三不连五

        /// <summary>
        /// 连续3片NG
        /// </summary>
        public List<DateTime> continueNG_three = new List<DateTime>();
        /// <summary>
        /// 不连续的5片NG
        /// </summary>
        public List<DateTime> continueNG_five = new List<DateTime>();
        /// <summary>
        /// gross结果
        /// </summary>
        public bool gross_COF_result = true;
        #endregion
    }
    public class CCDCMD
    {
        [Category("1 指令名称"), DisplayName("指令名称"), Description("指令名称")]
        public string name { get; set; } = "";

        [Category("2 指令字符串"), DisplayName("指令字符串"), Description("指令字符串")]
        public string CMD { get; set; } = "";
    }
    public class HardWareConfigClass : JsonHelper
    {
        /// <summary>
        /// 开启远程登录模式
        /// </summary>
        public int Remote = 0;

        public AxisType AxisTypeEnum = AxisType.original;

        public MaterialType MaterialTypeEnum = MaterialType.Normal;
    }
}
