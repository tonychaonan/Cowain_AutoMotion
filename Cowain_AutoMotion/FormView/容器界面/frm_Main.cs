using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using MotionBase;
using ToolTotal;
using Cowain_AutoMotion.FormView;
using Chart;
using System.Threading;
using System.Reflection;
using OmronFinsUI;
using System.Diagnostics;
using Cowain_AutoMotion.Flow;
using Cowain;
using ReadAndWriteConfig;
using Cowain_AutoMotion;
using System.Runtime.InteropServices;
using Cowain_AutoMotion.Flow.Hive;
using Cowain_Machine.Flow;
using System.Net.NetworkInformation;
using System.IO;
using System.Text.RegularExpressions;
using Cowain_Machine;
using static Chart.ChartTime;
using Cowain_AutoMotion.Flow._2Work;
using Cowain_AutoMotion.FormView._4弹窗;
using Cowain_AutoMotion.Flow.Common;

namespace Cowain_Form.FormView
{
    public partial class frm_Main : DevExpress.XtraEditors.XtraForm
    {
        //public frm_Main()
        //{
        //    InitializeComponent();
        //}

        /// <summary>
        /// 更新提示
        /// </summary>
        public static event Action<string> UpdataSignificant;

        #region 自定义变量
        private int childFormNumber = 0;
        //↓↓↓↓↓↓↓↓↓↓   參數、變數  ↓↓↓↓↓↓↓↓↓↓↓
        // public static clsMachine pMachine;
        Sys_Define.enPasswordType m_enLoginType = Sys_Define.enPasswordType.UnLogin;
        bool m_bShowViewOK = false;
        private Form[] pFormView = new Form[(int)(enformList.enMax)];
        frm_ErrorDlg pErrDlg = null;
        bool m_bNomalMode = true;  //程式測試時 , m_bNomalMode= false

        bool m_bRecTimer = false;
        TimeSpan tsNowTimer, tsRecTimer;
        /// <summary>
        /// 三国语言只转换一遍
        /// </summary>
        bool b_GetLanguage = false;
        //---------------------------
        int iTestCount = 0;
        //新增内容
        public static FormError formError;
        public static FormData formData;
        Cowain_AutoMotion.Flow.Common.Monitor Monitor = new Cowain_AutoMotion.Flow.Common.Monitor();
        public static ListCT ListCT1;
        public static ListCT ListCT2;
        //public static FormData formData;
        frm_DownTime fm_downTime;
        Thread PlanDownTimeAutoStop;
        ChartTime.MachineStatus oldstatus = ChartTime.MachineStatus.running;
        //public ChartTime.MachineStatus MSignalTowerstatus = ChartTime.MachineStatus.running;
        public static List<Control> control = new List<Control>(); //所有控件
        private Error pError;
        public enum enformList
        {
            enHomeForm = 0,
            enAlarmForm,
            enControlForm,
            enDataForm,
            enVisionForm,
            enSettingForm,
            enMax,
        };
        public static Dictionary<enformList, Form> FormList = new Dictionary<enformList, Form>();
        public static Dictionary<enformList, Form> ShowList = new Dictionary<enformList, Form>();
        public static Dictionary<enformList, ToolStripButton> FormButtonList = new Dictionary<enformList, ToolStripButton>();
        /// <summary>
        /// 当前物料状态
        /// </summary>
        private MaterialType m_CurMaterial = MaterialType.Normal;
        bool mini = false;
        bool mes = false;

        System.Timers.Timer m_tmDelay;
        private void OnTimedEvent_DelayTimeOut(object source, System.Timers.ElapsedEventArgs e) { m_tmDelay.Enabled = false; }
        /// <summary>
        /// 获取SHA1签名 线程
        /// </summary>
        Thread Get_SHA1_Thread;
        /// <summary>
        /// 界面刷新线程
        /// </summary>
        Thread ReFlash_Thread;

        Thread thread;

        //**************** Message處理 ***********************
        public const int WM_ShowErrorDlg = 0x8000;
        public const int WM_ShowEMGDlg = 0x8001;
        public const int WM_AirInsufficient = 0x8002;
        public const int WM_HomeCompleted = 0x8003;
        public const int WM_DoorisNotClosed = 0x8004;
        //public const int WM_ShowTriggerImage = 0x8005;

        public const int WM_Gantry1Glue = 0x8005;//胶水使用情况
        public const int WM_Gantry2Glue = 0x8006;//胶水使用情况
        public const int WM_Gantry1GlueTime = 0x8007;//胶水有效期情况
        public const int WM_Gantry2GlueTime = 0x8008;//胶水有效期情况

        public const int WM_HIVEReveiceFail = 0x8009;//HIVE返回失败

        public const int WM_NgRunnerAlram = 0x8010;//NG流道出料超时报警

        public const int WM_AutoStart = 0x0000;
        public const int WM_MalStop = 0x1001;//手动按下停止

        public const int WM_ChangeToHome = 0x9000;
        public const int WM_ChangeToAlarm = 0x9001;
        public const int WM_ChangeToControl = 0x9002;
        public const int WM_ChangeToData = 0x9003;
        public const int WM_ChangeToAlarming = 0x9004;//将界面切换到报警界面，并切换按钮图片


        /// <summary>
        /// 用于保存胶量管控参数自动写入 
        /// </summary>
        int Count = 0;
        ShowInfo showInfo = new ShowInfo();

        private enum HIVEState
        {
            运行中 = 0,
            待料中,
            工程模式中,
            计划停机中,
            故障停机中
        }

        MessageQueue messageQueue;
        List<RichTextBox> rtb = new List<RichTextBox>();
        private bool m_bPause = false;
        /// <summary>
        /// 上一次点击的按钮，用于切换按钮图标
        /// </summary>
        private enformList m_LastSelect = enformList.enHomeForm;


        /// <summary>
        /// 矫正速度OR 复检重量
        /// </summary>
        bool RecheckWeight = false;//false矫正速度；  true复检重量

        /// <summary>
        /// 龙门状态；用作于自动称胶矫正时，互斥前后龙门不能同时自动称胶矫正; true龙门未使用矫正
        /// </summary>
        bool GantryStatus = true;

        /// <summary>
        /// 胶重
        /// </summary>
        double resultGlue = 0;

        Stopwatch sw = new Stopwatch();
        /// <summary>
        /// 开始、暂停、停止按钮状态
        /// </summary>
        public enum enRunstate
        {
            enRunning = 0,
            enPause,
            enStop,
            enMax,
        };
        /// <summary>
        /// 记录是否在报错
        /// </summary>
        public static bool m_OnError = false;

        public enum enPlanMode
        {
            日常点检 = 0,
            更换AB胶水,
            更换HM胶水,
            更换针头,
            更换胶阀,
            压力测试,
            镭射标定,
            设备耗材更换,
            MaterialReplacement,
            周点检,
            其它,
            original
        }

        private enPlanMode m_CurStopMode = enPlanMode.original;
        /// <summary>
        /// 计划停机模式
        /// </summary>
        public enPlanMode CurStopMode
        {
            get
            {
                return m_CurStopMode;
            }

            set
            {
                m_CurStopMode = value;
            }
        }
        /// <summary>
        /// 累计多少次后才PING一次
        /// </summary>
        private int PingCount = 0;
        #endregion
      
        #region 自定义方法
        public frm_Main()
        {
            Mainflow.ShowSignificant += showHealthForm1;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            Form.CheckForIllegalCrossThreadCalls = false;  //開放UI之Thread可被跨執行緒存取UI            
                                                           //---------------------------------------
            MachineDataDefine.FormVisibled = new bool[(int)(enformList.enMax)];
            MachineDataDefine.pMachine = new clsMachine(typeof(clsMachine.enHomeStep), typeof(clsMachine.enStep), "Machine流程");
            MachineDataDefine.pMachine.pfrmMain = this;
            MachineDataDefine.miSuMiControl = new MiSuMiControl();
            String strPath = System.IO.Directory.GetCurrentDirectory();
            String strNowPath = strPath.Replace("\\Cowain_AutoMotion\\bin\\x64\\Debug", "");
            Task.Run(() =>
            {
                //frm_Conveyor = new frm_Conveyor();
                //读取三国语言
                ConfigHelper ConfigHelper = new ConfigHelper(strNowPath + "\\Cowain_AutoMotion\\configuration.xml");
                GetLanguage GetLanguage = new GetLanguage();
            });
            //---------------------
            frm_LoadingDlg LoadingDlg = new frm_LoadingDlg(ref MachineDataDefine.pMachine);
            LoadingDlg.ShowDialog();
            //---------------------
            //this.WindowState = FormWindowState.Maximized;
            InitializeComponent();
            SetTitleCenter();
            messageQueue = new MessageQueue(rtb);
            LogAuto.MessageEvent += Util_MessageEvent;
            formData = new FormData(MachineDataDefine.MachineCfgS.IntFileSaveDays);
            //if (MachineDataDefine.machineState.IsAbleTestRun || MachineDataDefine.machineState.IsAbleNullRun)
            //    frm_Main.formData.ChartTime1.Engineering();
            ListCT1 = new ListCT("L Conveyor");
            ListCT2 = new ListCT("R Conveyor");
            // Task.Run(new Action(DoHIVE));

            Get_SHA1_Thread = new Thread(Get_Sha1_Loop);
            Get_SHA1_Thread.Priority = ThreadPriority.Lowest;
            Get_SHA1_Thread.IsBackground = true;
            Get_SHA1_Thread.Start();

            //PlanDownTimeAutoStop = new Thread(new ThreadStart(PlanCycle));
            //PlanDownTimeAutoStop.Priority = ThreadPriority.Lowest;
            //PlanDownTimeAutoStop.IsBackground = true;
            //PlanDownTimeAutoStop.Start();
            //获取Mac地址
            //MachineDataDefine.hive_mac = Commons.GetMacAdd("HIVE");
            //if (MachineDataDefine.hive_mac == "00:00:00:00:00:00" || string.IsNullOrWhiteSpace(MachineDataDefine.hive_mac))
            //{
            //    LogAuto.Notify("获取Mac地址！" + "00:00:00:00:00:00", (int)MachineStation.主监控, MotionLogLevel.Info);
            //    // MachineDataDefine.NGmac = true;
            //}
            //else
            //{
            //    LogAuto.Notify("获取Mac地址！" + MachineDataDefine.hive_mac, (int)MachineStation.主监控, MotionLogLevel.Info);
            //}
            ReFlash_Thread = new Thread(DoReflash);
            ReFlash_Thread.Priority = ThreadPriority.Lowest;
            ReFlash_Thread.IsBackground = true;
            ReFlash_Thread.Start();

            //高于正常，低于正常，最高，最低，正常(自动称胶)
            //thread = new Thread(IOAuxiliarySigntimer);
            //thread.IsBackground = true;
            //thread.Priority = ThreadPriority.Lowest;
            //thread.Start();

            m_tmDelay = new System.Timers.Timer(1000);
            m_tmDelay.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent_DelayTimeOut);
            //WorkProcessLoad.instance.workProcess_AxisTakeIn.SNShowEvent += ShowSN;
            
            if (!MachineDataDefine.miSuMiControl.Connect("COM3"))
            {
                MessageBox.Show("连接失败，请检查串口/接线");
                return;
            }
          //  MessageBox.Show("夹爪连接成功");
        }

        private frm_SignificantAdvice frm_Significant;

        public void showHealthForm1(string NGmsg)
        {
            LogAuto.SaveErrorInfo(NGmsg);
            if (frm_Significant == null || frm_Significant.IsDisposed)
            {
                frm_Significant = new frm_SignificantAdvice(ref MachineDataDefine.pMachine, NGmsg);
                //frm_SignificantAdvice.Show();
            }
            if (frm_Significant.Visible)
            {
                UpdataSignificant(NGmsg);
                return;
            }
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    frm_Significant.Show();
                }));
            }
            else
            {
                frm_Significant.Show();
            }
        }
        private void CurrentDomain_UnhandledException(object sender, EventArgs e)
        {
            RunDataDefine.RunDataS.WriteParams(RunDataDefine.RunDataS);
            UnhandledExceptionEventArgs eventArgs = (UnhandledExceptionEventArgs)e;
            LogAuto.SaveSoftWareFlashLog(eventArgs.ExceptionObject.ToString(), MotionLogLevel.Fatal);
            MachineDataDefine.pMachine.Stop();

            MessageBox.Show("程序数据异常，将关闭软件" + "记录在D:\\DATA\\异常记录\\");


            Process[] allProcesses = Process.GetProcesses();
            foreach (Process item in allProcesses)
            {
                if (item.ProcessName.Contains("Cowain_AutoMotion"))
                {
                    item.Kill();
                }
            }
        }
        private void Application_ThreadException(object sender, EventArgs e)
        {
            RunDataDefine.RunDataS.WriteParams(RunDataDefine.RunDataS);
            ThreadExceptionEventArgs eventArgs = (ThreadExceptionEventArgs)e;
            LogAuto.SaveSoftWareFlashLog(eventArgs.Exception.ToString(), MotionLogLevel.Fatal);
            MachineDataDefine.pMachine.Stop();

            MessageBox.Show("程序数据异常，将关闭软件" + "记录在D:\\DATA\\异常记录\\");

            Process[] allProcesses = Process.GetProcesses();
            foreach (Process item in allProcesses)
            {
                if (item.ProcessName.Contains("Cowain_AutoMotion"))
                {
                    item.Kill();
                }
            }
        }
        private void DoHIVE()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(200);
                    if (frm_Main.formData?.ChartTime1.RunStatus != oldstatus)
                    {
                        if (frm_Main.formData.ChartTime1.RunStatus != ChartTime.MachineStatus.error_down && frm_Main.formData.ChartTime1.RunStatus != ChartTime.MachineStatus.planned_downtime)
                        {
                            if (MachineDataDefine.machineState.b_UseHive && (/*MachineDataDefine.machineState.UpLoadError == 1 ||*/ frm_Main.formData.ChartTime1.RunStatus == ChartTime.MachineStatus.idle || oldstatus == ChartTime.MachineStatus.idle) && !HIVEDataDefine.Hive_machineState.IsSendErrorDown)
                            {
                                if (HIVE.HIVEInstance.HIVE_Reveice_Status)
                                {
                                    HIVE.HIVEInstance.HiveSendMACHINESTATE((int)frm_Main.formData.ChartTime1.RunStatus + 1, "", "", "", false);
                                }
                            }
                            if (MachineDataDefine.machineState.b_UseRemoteQualification && (/*MachineDataDefine.machineState.UpLoadError == 1 ||*/ frm_Main.formData.ChartTime1.RunStatus == ChartTime.MachineStatus.idle || oldstatus == ChartTime.MachineStatus.idle) && !HIVEDataDefine.Hive_machineState.IsSendErrorDown)
                            {
                                if (HIVE.HIVEInstance.HIVE_Reveice_Status)
                                {
                                    HIVE.HIVEInstance.HiveSendMACHINESTATE((int)frm_Main.formData.ChartTime1.RunStatus + 1, "", "", "", true);
                                }
                            }
                        }

                        oldstatus = frm_Main.formData.ChartTime1.RunStatus;
                    }
                }
                catch
                {
                }
            }
        }

        private void Get_Sha1_Loop()
        {
            while (true)
            {

                try
                {
                    if (HIVEDataDefine.Hive_machineState.Start_Get_Sha1)
                    {

                        HIVEDataDefine.Init();
                    }

                    Random x = new Random();
                    int b = 0;
                    if (Convert.ToInt32(HIVEDataDefine.HIVE_sha1_Data.Get_Sha1_Loop_Time) > 5)
                        b = x.Next(-Convert.ToInt32(HIVEDataDefine.HIVE_sha1_Data.Get_Sha1_Loop_Time) / 2, Convert.ToInt32(HIVEDataDefine.HIVE_sha1_Data.Get_Sha1_Loop_Time) / 2);

                    Thread.Sleep(Convert.ToInt32(HIVEDataDefine.HIVE_sha1_Data.Get_Sha1_Loop_Time) * 1000 + b * 1000);

                }
                catch { }
                Thread.Sleep(1000);
            }
        }

        private void DoReflash()
        {
            while (true)
            {
                try
                {
                    Reflash();
                    //if (MachineDataDefine.machineState.b_UseHive)
                    //{
                    //    bool connect = Commons.PingIP("10.0.0.2", 3000);
                    //    if (!connect && (MachineDataDefine.machineState.b_UseRemoteQualification != true))//远程模式下不检查连接状态
                    //    {
                    //        //连接不到HIVE将状态切换到DT
                    //        frm_Main.formData.ChartTime1.StartError();
                    //        HIVE.HIVEInstance.HiveSendMACHINESTATE(5, "2076", "HIVE Communicate Error", "HIVE Error", false);
                    //        MachineDataDefine.m_CardID = "";
                    //    }
                    //}

                    if (MachineDataDefine.machineState.b_UsePDCA)
                    {
                        PingCount++;
                        if (PingCount >= 25)
                        {
                            Commons.PingIP(MESDataDefine.MESLXData.StrMiniIP, 3000);
                            PingCount = 0;
                        }
                    }
                    
                    if ((string.IsNullOrWhiteSpace(MachineDataDefine.hive_mac) || MachineDataDefine.hive_mac == "00:00:00:00:00:00") && MachineDataDefine.NGmac)
                    {
                        MachineDataDefine.hive_mac = "00:00:00:00:00:00";
                        MachineDataDefine.NGmac = false;
                        showHealthForm("获取网卡MAC地址失败");
                    }




                    #endregion
                    Thread.Sleep(400);
                }
                catch (Exception err)
                {

                }
            }
        }

        private void Reflash()
        {
            if (this == null)
            {
                return;
            }

            if (this.InvokeRequired)
            {
                try
                {
                    this.Invoke(new Action(Reflash));
                    return;
                }
                catch (Exception e)
                {
                    return;
                }
            }

            if (MachineDataDefine.pMachine == null)
                return;

            if ((int)MachineDataDefine.pMachine.m_LoginUser > 1)
            {
                //自动登出
                long a = MouseKeyBoardOperate.GetLastInputTime();//秒
                if (a > MachineDataDefine.MachineCfgS.LogoutTime * 60)
                {
                    MachineDataDefine.pMachine.m_LoginUser = MotionBase.Sys_Define.enPasswordType.UnLogin;
                    MachineDataDefine.m_LoginUser = Sys_Define.enPasswordType.UnLogin;
                    MachineDataDefine.m_LoginUserName = "";
                    MachineDataDefine.m_LoginCardID = "";
                    ChangForm(enformList.enHomeForm);
                    ShowUserButton(MachineDataDefine.pMachine.m_LoginUser);
                }
                //只刷新一次,避免重复刷新
                if (MachineDataDefine.pMachine.NeedRef)
                {
                    ShowUserButton(MachineDataDefine.pMachine.m_LoginUser);
                    MachineDataDefine.pMachine.NeedRef = false;
                }
            }
            if (!MachineDataDefine.IsAutoing)
            {
                m_bPause = false;
            }

            //--------------------------更新時間 && 程式版本------------------------
            DateTime dtNow = DateTime.Now;
            string strDate = dtNow.Year.ToString() + "/" + string.Format("{0:D2}", dtNow.Month) + "/" + string.Format("{0:D2}", dtNow.Day);
            string strTime = string.Format("{0:D2}", dtNow.Hour) + " : " + string.Format("{0:D2}", dtNow.Minute) + " : " +
                             string.Format("{0:D2}", dtNow.Second);
            //label_Date.Text = "Date: " + strDate;
            //label_Time.Text = "Time: " + strTime;
            //-----------------------------------------------------------------------
            MachineDataDefine.IsHomeComplete = MachineDataDefine.pMachine.GetHomeCompleted();

            //toolStripStatusLabel.Text = "设备状态：";
            toolStripStatusLabel1.Text = MachineDataDefine.pMachine.m_Status.ToString();
            //toolStripStatusLabel2.Text = "  ||  HIVE状态：";
            toolStripStatusLabel3.Text = ((HIVEState)frm_Main.formData.ChartTime1.RunStatus).ToString() + " " + ((int)frm_Main.formData.ChartTime1.RunStatus + 1).ToString();

            #region HIVE状态刷新
            if (MachineDataDefine.machineState.b_UseHive)
            {
                switch ((HIVEState)formData.ChartTime1.RunStatus)
                {
                    case HIVEState.运行中:
                        //toolStripStatusLabel3.BackColor = Color.Green;
                        if (labelMode.Text != "Running")
                        {
                            labelMode.Text = "Running";
                            labelMode.BackColor = Color.FromArgb(211, 235, 115);
                            toolStrip.BackColor = Color.FromArgb(235, 235, 237);
                        }
                        break;
                    case HIVEState.待料中:
                        //toolStripStatusLabel3.BackColor = Color.LightBlue;
                        if (labelMode.Text != "Idle")
                        {
                            labelMode.Text = "Idle";
                            labelMode.BackColor = Color.FromArgb(235, 255, 254);
                            toolStrip.BackColor = Color.FromArgb(235, 235, 237);
                        }
                        break;
                    case HIVEState.工程模式中:
                        //toolStripStatusLabel3.BackColor = Color.Purple;
                        if (labelMode.Text != "Engineering")
                        {
                            labelMode.Text = "Engineering";
                            labelMode.BackColor = Color.FromArgb(204, 171, 216);
                            toolStrip.BackColor = Color.FromArgb(204, 171, 216);
                        }
                        break;
                    case HIVEState.计划停机中:
                        //toolStripStatusLabel3.BackColor = Color.Pink;
                        if (labelMode.Text != "Planned DT")
                        {
                            labelMode.Text = "Planned DT";
                            labelMode.BackColor = Color.FromArgb(255, 215, 212);
                            toolStrip.BackColor = Color.FromArgb(255, 215, 212);
                        }
                        break;
                    case HIVEState.故障停机中:
                        //toolStripStatusLabel3.BackColor = Color.Red;
                        if (m_SelectWorkMode == frm_ModeSelect.enWorkMode.Manually_Downtime)
                        {
                            if (labelMode.Text != "Manually DT")
                            {
                                labelMode.Text = "Manually DT";
                                labelMode.BackColor = Color.FromArgb(235, 115, 115);
                            }

                        }
                        else
                        {
                            if (labelMode.Text != "Down Time")
                            {
                                labelMode.Text = "Down Time";
                                labelMode.BackColor = Color.FromArgb(235, 115, 115);
                            }
                        }
                        toolStrip.BackColor = Color.FromArgb(235, 115, 115);
                        break;
                    default:
                        if (labelMode.Text != "HIVE")
                        {
                            labelMode.Text = "HIVE";
                            labelMode.BackColor = Color.Silver;
                            toolStrip.BackColor = Color.FromArgb(235, 235, 237);
                        }
                        break;
                }
            }
            else
            {
                if (labelMode.Text != "HIVE")
                {
                    labelMode.Text = "HIVE";
                    labelMode.BackColor = Color.Silver;
                }
            }
            #endregion
            #region 物料状态刷新
            if (MachineDataDefine.settingData.ProdType == "E_SKU")
            {
                MachineDataDefine.HardwareCfg.MaterialTypeEnum = MaterialType.E_SKU;
            }
            else if (MachineDataDefine.settingData.ProdType == "M_SKU")
            {
                MachineDataDefine.HardwareCfg.MaterialTypeEnum = MaterialType.M_SKU;
            }
            else if (MachineDataDefine.settingData.ProdType == "Normal")
            {
                MachineDataDefine.HardwareCfg.MaterialTypeEnum = MaterialType.Normal;
            }
            else
            {
                MachineDataDefine.HardwareCfg.MaterialTypeEnum = MaterialType.Common;
            }

            if (MachineDataDefine.HardwareCfg.MaterialTypeEnum != m_CurMaterial)
            {
                switch (MachineDataDefine.HardwareCfg.MaterialTypeEnum)
                {
                    case MaterialType.Normal:
                        labType.Visible = false;
                        pictureEdit2.Visible = true;
                        pictureEdit2.Image = Cowain_AutoMotion.Properties.Resources.Normal;
                        m_CurMaterial = MaterialType.Normal;
                        break;
                    case MaterialType.E_SKU:
                        labType.Visible = false;
                        pictureEdit2.Visible = true;
                        pictureEdit2.Image = Cowain_AutoMotion.Properties.Resources.E_SKU;
                        m_CurMaterial = MaterialType.E_SKU;
                        break;
                    case MaterialType.M_SKU:
                        m_CurMaterial = MaterialType.M_SKU;
                        break;
                    case MaterialType.Common:
                        try
                        {

                            labType.Visible = false;
                            pictureEdit2.Visible = true;
                            string filename = Application.StartupPath.Replace(@"bin\x64\Debug", @"Resources\" + MachineDataDefine.settingData.Station + ".png");
                            Bitmap bitmap = new Bitmap(filename);
                            pictureEdit2.Image = bitmap;
                        }
                        catch (Exception)
                        {
                            labType.Text = MachineDataDefine.settingData.Station;
                            labType.BackColor = Color.FromArgb(235, 235, 237);
                            labType.ForeColor = Color.FromArgb(204, 204, 204);
                            pictureEdit2.Visible = false;
                            labType.Visible = true;
                        }

                        //  pictureEdit2.Image = Cowain_AutoMotion.Properties.Resources.Common;
                        m_CurMaterial = MaterialType.Common;
                        break;
                }
            }
            #endregion
            CheckMachineError();

            if (/*MachineDataDefine.MachineControlS.IsAutoing &&*/ frm_Main.formData?.ChartTime1.RunStatus != frm_Main.formData?.ChartTime1.MSignalTowerstatus)
            {
                MachineDataDefine.pMachine.m_SgTower.Stop();
                switch (frm_Main.formData.ChartTime1.RunStatus)
                {
                    case ChartTime.MachineStatus.running:
                        MachineDataDefine.pMachine.m_SgTower.TowerLight(clsMSignalTower.enTowerMode.enAutoRuning);
                        break;
                    case ChartTime.MachineStatus.idle:
                        MachineDataDefine.pMachine.m_SgTower.TowerLight(clsMSignalTower.enTowerMode.enIdle);
                        break;
                    case ChartTime.MachineStatus.planned_downtime:
                        MachineDataDefine.pMachine.m_SgTower.TowerLight(clsMSignalTower.enTowerMode.enIdle);
                        break;
                    case ChartTime.MachineStatus.error_down:
                        MachineDataDefine.pMachine.m_SgTower.TowerLight(clsMSignalTower.enTowerMode.enAlarm);
                        break;
                    default:
                        break;
                }
                frm_Main.formData.ChartTime1.MSignalTowerstatus = frm_Main.formData.ChartTime1.RunStatus;
            }
        }

        Stopwatch sw1 = new Stopwatch();
        Stopwatch sw2 = new Stopwatch();
        bool FZiostate1 = false;
        bool FZiostate2 = false;
        double IOoff1 = 0;
        double IOoff2 = 0;
        public bool GetShowViewOK() { return m_bShowViewOK; }

        private void ShowNewForm(object sender, EventArgs e)
        {
            #region 判断系统是否已启动

            //主窗口加载完成信号
            MachineDataDefine.IsMainFormLoading = true;
            #endregion
            //************************
            #region 版本判定
            String strFilePath = System.IO.Directory.GetCurrentDirectory();
            strFilePath = strFilePath + "\\Cowain_AutoMotion.exe";
            //-------------------------------
            System.IO.FileInfo fi = new System.IO.FileInfo(strFilePath);
            //---------
            String strTime = fi.LastWriteTime.ToShortDateString();

            //label_Ver.Text = "Releas Ver: " + MESDataDefine.MESDatas.StrVersion;
            //MDataDefine.Log_Version = "V" + Version;
            #endregion
            //---------------------
            HomeForm hfm = new HomeForm(ref MachineDataDefine.pMachine);
            hfm.pfrmMain = this;
            hfm.SensorEvent += ShowSensor;
            pFormView[(int)enformList.enHomeForm] = hfm;

            pFormView[(int)enformList.enAlarmForm] = new AlarmForm();
            pFormView[(int)enformList.enControlForm] = new ControlForm(ref MachineDataDefine.pMachine);
            pFormView[(int)enformList.enDataForm] = new DataReviewForm();
            //pFormView[(int)enformList.enVisionForm] = new VisionForm();
            VisionForm vfm = new VisionForm();
            vfm.VisionEvent += ShowVistionDetail;
            pFormView[(int)enformList.enVisionForm] = vfm;

            //pFormView[(int)enformList.enRecipeForm] = new frm_Recipe(ref pMachine);
            //pFormView[(int)enformList.enManulForm] = new frm_Manul(ref pMachine);
            //pFormView[(int)enformList.enVisionForm] = new frm_Vision(ref pMachine);
            //pFormView[(int)enformList.enTeachForm] = new frm_Teach(ref pMachine);
            //******************************************************************
            //新增内容
            formError = new FormError(MachineDataDefine.MachineCfgS.IntFileSaveDays);
            formError.ErrorUnit1.StationID = MachineDataDefine.settingData.Station;
            //pFormView[(int)enformList.enErrorForm] = formError;

            pFormView[(int)enformList.enSettingForm] = new SettingForm(MachineDataDefine.pMachine);

            //pFormView[(int)enformList.enDataForm2] = formData;
            //formData.ListCT1 = ListCT1;
            //formData.ListCT2 = ListCT2;
            //pFormView[(int)enformList.enConveyorForm] = frm_Conveyor;
            //******************************************************************
            ToolStripButton[] pToolstrip = { btnHome, btnAlarm, btnControl, btnData,
                                          btnVision,btnSetting};  //  toolStrip_VisionBt,toolStrip_TeachBt,toolStrip_Teach2Bt
            for (int i = 0; i < pFormView.Length; i++)
            {
                FormList.Add((enformList)i, pFormView[i]);
                FormList[(enformList)i].MdiParent = this;
                FormList[(enformList)i].WindowState = FormWindowState.Maximized;
                FormButtonList.Add((enformList)i, pToolstrip[i]);
            }
            //******************************************************************
            if (m_bNomalMode)
            {
                //toolStrip_AutoBt.Enabled = false;
                //toolStrip_RecipeBt.Enabled = false;
                //toolStrip_ManulBt.Enabled = false;
                //toolStrip_TeachBt.Enabled = false;
                //toolStrip_VisionBt.Enabled = false;
                ShowUserButton(Sys_Define.enPasswordType.UnLogin);
            }
            ChangForm(enformList.enHomeForm);
            m_bShowViewOK = true;
            //if (string.IsNullOrWhiteSpace(MachineDataDefine.hive_mac) || MachineDataDefine.hive_mac == "00:00:00:00:00:00")
            //{
            //    MachineDataDefine.hive_mac = "00:00:00:00:00:00";
            //    pError = new Error(ref MachineDataDefine.pMachine.m_NowAddress, "获取网卡MAC地址失败", "", (int)MErrorDefine.MErrorCode.HIVE通讯失败);
            //    pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
            //}
        }

        private void ChangForm(enformList enFormName)
        {
            if (MachineDataDefine.IsFormChanging)
                return;
            MachineDataDefine.IsFormChanging = true;
            //DelayMS(50);
            int Widths = 0, Heights = 0, Widths2 = 0, Heights2 = 0; ;
            int iMax = Convert.ToInt16(enformList.enMax);
            for (int i = 0; i < iMax; i++)
            {
                FormList[(enformList)i].Hide();
                FormButtonList[(enformList)i].Checked = false;
            }
            if (ShowList.Count > 0)
            {
                ShowList[0].Close();
            }
            //图标切换
            switch (m_LastSelect)
            {
                case enformList.enHomeForm:
                    btnHome.Image = Cowain_AutoMotion.Properties.Resources.Home_De_energized;
                    break;
                case enformList.enAlarmForm:
                    btnAlarm.Image = Cowain_AutoMotion.Properties.Resources.Alarm_De_energized;
                    break;
                case enformList.enControlForm:
                    btnControl.Image = Cowain_AutoMotion.Properties.Resources.Config_De_energized;
                    break;
                case enformList.enDataForm:
                    btnData.Image = Cowain_AutoMotion.Properties.Resources.Data_De_energized;
                    break;
                case enformList.enVisionForm:
                    btnVision.Image = Cowain_AutoMotion.Properties.Resources.Vision_De_energized;
                    break;
                case enformList.enSettingForm:
                    btnSetting.Image = Cowain_AutoMotion.Properties.Resources.Setting_De_energized;
                    break;
                default:
                    break;
            }

            switch (enFormName)
            {
                case enformList.enHomeForm:
                    btnHome.Image = Cowain_AutoMotion.Properties.Resources.Home_Energized;
                    m_LastSelect = enformList.enHomeForm;
                    break;
                case enformList.enAlarmForm:
                    if (m_OnError)
                    {
                        btnAlarm.Image = Cowain_AutoMotion.Properties.Resources.Alarm_Alerted;
                    }
                    else
                    {
                        btnAlarm.Image = Cowain_AutoMotion.Properties.Resources.Alarm_Energized;
                    }

                    m_LastSelect = enformList.enAlarmForm;
                    break;
                case enformList.enControlForm:
                    btnControl.Image = Cowain_AutoMotion.Properties.Resources.Config_Energized;
                    m_LastSelect = enformList.enControlForm;
                    break;
                case enformList.enDataForm:
                    btnData.Image = Cowain_AutoMotion.Properties.Resources.Data_Energized;
                    m_LastSelect = enformList.enDataForm;
                    break;
                case enformList.enVisionForm:
                    btnVision.Image = Cowain_AutoMotion.Properties.Resources.Vision_Energized;
                    m_LastSelect = enformList.enVisionForm;
                    break;
                case enformList.enSettingForm:
                    btnSetting.Image = Cowain_AutoMotion.Properties.Resources.Setting_Energized;
                    m_LastSelect = enformList.enSettingForm;
                    break;
                default:
                    break;
            }

            //------------------------------------
            FormList[enFormName].Dock = DockStyle.Fill;
            FormList[enFormName].Show();
            FormButtonList[enFormName].Checked = true;
            //------------------
            if (MachineDataDefine.LanguageStyle != 1 && b_GetLanguage == false)
            {
                b_GetLanguage = true;
                control.Clear();
                getControl(FormList[enFormName].Controls);
                for (int i = 0; i < control.Count; i++)
                {
                    if (control[i] is Label)
                    {
                        Widths = control[i].Width;
                        Heights = control[i].Height;
                        ((Label)control[i]).AutoEllipsis = true;
                    }
                    if (control[i] is CheckBox)
                    {
                        Widths2 = control[i].Width;
                        Heights2 = control[i].Height;
                        ((CheckBox)control[i]).AutoEllipsis = true;
                    }
                    // }
                    //将所有控件txt变成可转语言
                    control[i].Text = JudgeLanguage.JudgeLag(control[i].Text);
                    if (control[i] is Label)
                    {
                        control[i].Width = Widths;
                        control[i].Height = Heights;
                        ((Label)control[i]).AutoSize = false;
                        ((Label)control[i]).AutoEllipsis = true;
                    }
                    if (control[i] is CheckBox)
                    {
                        control[i].Width = Widths2;
                        control[i].Height = Heights2;
                        ((CheckBox)control[i]).AutoSize = false;
                        ((CheckBox)control[i]).AutoEllipsis = true;
                    }
                    else if (control[i] is Button)
                    {
                        ((Button)control[i]).AutoEllipsis = true;
                    }

                }
                // button3.Text = JudgeLanguage.JudgeLag(button3.Text);

            }
            m_bRecTimer = false;  //切換畫面後 時間重新計時
            //DelayMS(60);
            MachineDataDefine.IsFormChanging = false;
        }

        /// <summary>
        /// 显示图片明细
        /// </summary>
        /// <param name="picInfo"></param>
        private void ShowVistionDetail(VisionForm.PictureInfo picInfo)
        {
            int iMax = Convert.ToInt16(enformList.enMax);
            for (int i = 0; i < iMax; i++)
            {
                FormList[(enformList)i].Hide();
                FormButtonList[(enformList)i].Checked = false;
            }
            if (ShowList.Count > 0)
            {
                ShowList[0].Close();
            }

            ShowList.Clear();
            VisionDetail vfm = new VisionDetail();
            vfm.MdiParent = this;
            vfm.WindowState = FormWindowState.Maximized;
            vfm.m_PictureInfo = picInfo;
            ShowList.Add(0, vfm);
            vfm.Show();
        }

        /// <summary>
        /// 显示Sensor窗体
        /// </summary>
        private void ShowSensor()
        {
            int iMax = Convert.ToInt16(enformList.enMax);
            for (int i = 0; i < iMax; i++)
            {
                FormList[(enformList)i].Hide();
                FormButtonList[(enformList)i].Checked = false;
            }
            if (ShowList.Count > 0)
            {
                ShowList[0].Close();
            }

            ShowList.Clear();
            frm_Sensor sensor = new frm_Sensor(ref MachineDataDefine.pMachine);
            sensor.MdiParent = this;
            sensor.WindowState = FormWindowState.Maximized;
            ShowList.Add(0, sensor);
            sensor.Show();
        }

        public void DelayMS(int milliSecond)
        {
            int start = Environment.TickCount;
            while (Math.Abs(Environment.TickCount - start) < milliSecond)
            {
                Application.DoEvents();
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 49626 || m.Msg == 70)
            {

            }
            else
            {

            }
            if (!MachineDataDefine.IsAlarmShow)
            {
                switch (m.Msg)
                {
                    case WM_ShowErrorDlg:
                        {
                            if (MachineDataDefine.pMachine.myErrorStack.Count != 0)
                            {
                                Error pErr = MachineDataDefine.pMachine.myErrorStack.Pop();
                                frm_ErrorDlg pErrDlg = new frm_ErrorDlg(ref pErr, ref MachineDataDefine.pMachine);
                                m_OnError = true;
                                ChangForm(enformList.enAlarmForm);
                                pErrDlg.ShowDialog();
                                MachineDataDefine.IsAlarmShow = true;
                            }
                        }
                        break;
                    case WM_ShowEMGDlg:
                        {
                            if (pErrDlg != null)
                            {
                                pErrDlg.Close();
                                MachineDataDefine.pMachine.myErrorStack.Clear();
                                pErrDlg = null;
                            }
                            //-----------------------------------------
                            ChangForm(enformList.enHomeForm);
                            //toolStrip_LoginBt.Enabled = true;
                            //toolStrip_HomeBt.Enabled = true;
                            //toolStrip_AutoBt.Enabled = false;
                            //toolStrip_RecipeBt.Enabled = false;
                            //toolStrip_ManulBt.Enabled = false;
                            //toolStrip_TeachBt.Enabled = false;
                            //-----------------------------------------
                            dia_EMG pEmgDlg = new dia_EMG(ref MachineDataDefine.pMachine, dia_EMG.dia_ShowStatus.enShowEMG);
                            pEmgDlg.ShowDialog();
                            //MachineDataDefine.MachineControlS.IsAlarmShow = true;
                        }
                        break;
                    case WM_AirInsufficient:
                        {
                            if (pErrDlg != null)
                            {
                                pErrDlg.Close();
                                MachineDataDefine.pMachine.myErrorStack.Clear();
                                pErrDlg = null;
                            }
                            //-----------------------------------------
                            dia_EMG pEmgDlg = new dia_EMG(ref MachineDataDefine.pMachine, dia_EMG.dia_ShowStatus.enShowAirInsufficient);
                            pEmgDlg.ShowDialog();

                        }
                        break;
                    case WM_HomeCompleted:
                        {
                            //ChangForm(enformList.enAutoForm);
                        }
                        break;
                    case WM_DoorisNotClosed:
                        {


                            if (!MachineDataDefine.IsFormOpen)
                            {
                                //MachineDataDefine.MachineControlS.IsFormOpen = true;
                                dia_EMG pEmgDlg = new dia_EMG(ref MachineDataDefine.pMachine, dia_EMG.dia_ShowStatus.enShowDoorisNotClosed);
                                MachineDataDefine.pMachine.m_SgTower.SetLightStatus(true, false, false, true);//停止-红灯亮
                                MachineDataDefine.IsFormOpen = true;
                                pEmgDlg.ShowDialog();

                            }
                            //MachineDataDefine.MachineControlS.IsAlarmShow = true;
                        }
                        break;
                    case WM_NgRunnerAlram:
                        {
                            dia_EMG pEmgDlg = new dia_EMG(ref MachineDataDefine.pMachine, dia_EMG.dia_ShowStatus.enNgRunnerAlram);
                            pEmgDlg.ShowDialog();
                        }
                        break;
                    case WM_Gantry1Glue:
                        {
                            dia_EMG pEmgDlg = new dia_EMG(ref MachineDataDefine.pMachine, dia_EMG.dia_ShowStatus.enChangeGantry1Glue);
                            pEmgDlg.ShowDialog();
                        }
                        break;
                    case WM_Gantry2Glue:
                        {
                            dia_EMG pEmgDlg = new dia_EMG(ref MachineDataDefine.pMachine, dia_EMG.dia_ShowStatus.enChangeGantry2Glue);
                            pEmgDlg.ShowDialog();
                        }
                        break;
                    case WM_Gantry1GlueTime:
                        {
                            //if (MachineDataDefine.MachineControlS.IsAutoing)
                            //{
                            //    MachineDataDefine.MachineControlS.IsCycleStop = true;
                            //}
                            dia_EMG pEmgDlg = new dia_EMG(ref MachineDataDefine.pMachine, dia_EMG.dia_ShowStatus.enChangeGantry1GlueTime);
                            pEmgDlg.ShowDialog();
                        }
                        break;
                    case WM_Gantry2GlueTime:
                        {
                            //if (MachineDataDefine.MachineControlS.IsAutoing)
                            //{
                            //    MachineDataDefine.MachineControlS.IsCycleStop = true;
                            //}
                            dia_EMG pEmgDlg = new dia_EMG(ref MachineDataDefine.pMachine, dia_EMG.dia_ShowStatus.enChangeGantry2GlueTime);
                            pEmgDlg.ShowDialog();
                        }
                        break;
                    case WM_HIVEReveiceFail:
                        {
                            if (MachineDataDefine.IsAutoing)
                            {
                                MachineDataDefine.IsCycleStop = true;
                                //while (MachineDataDefine.MachineControlS.IsAutoing)
                                //{
                                //    Thread.Sleep(100);
                                //}
                            }
                            dia_EMG pEmgDlg = new dia_EMG(ref MachineDataDefine.pMachine, dia_EMG.dia_ShowStatus.enHIVEReveiceFail);
                            pEmgDlg.ShowDialog();
                        }
                        break;
                    case WM_AutoStart:
                        {
                            //if(!MachineDataDefine.machineState.b_UseLMI&& !MachineDataDefine.machineState.b_UseLYG)
                            //{
                            //    MsgBoxHelper.DxMsgShowErr("请先选择视觉厂商");
                            //    return;
                            //}
                            //LAD模式相关判定
                            if (MachineDataDefine.machineState.b_UseTestRun && MachineDataDefine.b_UseLAD)
                            {
                                if (MachineDataDefine.LADModel == 1 || MachineDataDefine.LADModel == 3)
                                {
                                    MsgBoxHelper.DxMsgShowErr("CPK或SCS模式不能在调机模式下运行");
                                    return;
                                }
                                else if (MachineDataDefine.LADModel == 2 || !MachineDataDefine.machineState.b_UseMes)
                                {
                                    //MsgBoxHelper.DxMsgShowErr("GRR模式必须开启mes用于获取SN");
                                    //return;
                                }
                            }
                            if (MachineDataDefine.machineState.b_UseHive)
                            {
                                //Hive状态判定
                                if ((HIVEState)formData.ChartTime1.RunStatus == HIVEState.运行中 || ((HIVEState)formData.ChartTime1.RunStatus == HIVEState.待料中 && (HIVEState)formData.ChartTime1.LastRunStatus == HIVEState.运行中))
                                {
                                    if (!MachineDataDefine.machineState.b_UseMes)
                                    {
                                        MsgBoxHelper.DxMsgShowErr("Running状态下必须开启mes，如当前产品无需上传mes，请将Hive状态切换到Engineering状态");
                                        return;
                                    }

                                }

                                if ((HIVEState)formData.ChartTime1.RunStatus != HIVEState.运行中 && !((HIVEState)formData.ChartTime1.RunStatus == HIVEState.待料中 && (HIVEState)formData.ChartTime1.LastRunStatus == HIVEState.运行中))
                                {
                                    string errMsg = string.Empty;
                                    if ((HIVEState)formData.ChartTime1.RunStatus == HIVEState.待料中)
                                    {
                                        errMsg = "当前状态是[" + formData.ChartTime1.LastRunStatus.ToString() + "],请确认是否是调机料？";
                                    }
                                    else
                                    {
                                        errMsg = "当前状态是[" + formData.ChartTime1.RunStatus.ToString() + "],请确认是否是调机料？";
                                    }
                                    DialogResult result = MsgBoxHelper.DxMsgShowQues(errMsg);
                                    if (result != DialogResult.Yes)
                                    {
                                        return;
                                    }
                                }
                            }
                            btnPause.Enabled = true;
                            btnStop.Enabled = true;
                            dia_ShowModel showstate;
                            if (MachineDataDefine.machineState.b_UseTestRun != true)
                            {
                                showstate = new dia_ShowModel(MachineDataDefine.pMachine, true);
                            }
                            else
                            {
                                showstate = new dia_ShowModel(MachineDataDefine.pMachine, false);
                            }
                            showstate.ShowDialog();
                            ChangeRunPic(enRunstate.enRunning);
                            ChangForm(enformList.enHomeForm);
                        }
                        break;
                    case WM_MalStop:
                        ChangeRunPic(enRunstate.enStop);
                        break;
                    case WM_ChangeToHome:
                        ChangForm(enformList.enHomeForm);
                        break;
                    case WM_ChangeToAlarm:
                        ChangForm(enformList.enAlarmForm);
                        break;
                    case WM_ChangeToControl:
                        ChangForm(enformList.enControlForm);
                        break;
                    case WM_ChangeToData:
                        ChangForm(enformList.enDataForm);
                        break;
                    case WM_ChangeToAlarming:
                        m_OnError = true;
                        ChangForm(enformList.enAlarmForm);
                        break;
                }
            }
            base.WndProc(ref m);
        }

        private void ShowUserButton(Sys_Define.enPasswordType enLoginType)
        {

            //toolStrip_AutoBt.Visible = true;
            //toolStripSeparator_Auto.Visible = true;

            //toolStrip_LoginBt.Text = "Login";
            ////------------------
            //toolStrip_RecipeBt.Visible = false;
            //toolStripSeparator_Recipe.Visible = false;
            //toolStrip_ManulBt.Visible = false;
            //toolStripSeparator_Manul.Visible = false;
            //toolStrip_TeachBt.Visible = false;
            //toolStripSeparator_Teach.Visible = false;
            //toolStrip_VisionBt.Visible = false;
            //toolStripSeparator_Vision.Visible = false;
            //toolStrip_Teach2Bt.Visible = false;
            //toolStripSeparator_Teach2.Visible = false;
            //-------------------------------------------------
            if (enLoginType == Sys_Define.enPasswordType.Operator)
            {
                //toolStrip_AutoBt.Visible = true;
                //toolStripSeparator_Auto.Visible = true;
                //toolStrip_LoginBt.Image = Cowain_AutoMotion.Properties.Resources.user1;
                //toolStrip_LoginBt.Text = "User";
                btnHome.Enabled = true;
                btnHome.Checked = false;
                btnAlarm.Enabled = true;
                btnAlarm.Checked = false;
                btnControl.Enabled = true;
                btnControl.Checked = false;
                btnData.Enabled = true;
                btnData.Checked = false;
                btnVision.Enabled = true;
                btnVision.Checked = false;
                btnSetting.Enabled = false;
                btnSetting.Checked = false;

                m_enLoginType = Sys_Define.enPasswordType.Operator;
                //labUser.Text = "Level 1";
                //labUser.BackColor = Color.FromArgb(128, 255, 128);

                labelMode.Enabled = true;

                #region 图标显示
                picUser.Image = Cowain_AutoMotion.Properties.Resources.Level1;
                #endregion
            }
            else if (enLoginType == Sys_Define.enPasswordType.Eng)
            {
                //toolStrip_RecipeBt.Enabled = true;
                //toolStrip_RecipeBt.Visible = true;
                //toolStrip_AutoBt.Visible = true;
                //toolStripSeparator_Auto.Visible = true;
                //toolStrip_RecipeBt.Visible = true;
                //toolStripSeparator_Recipe.Visible = true;
                //toolStrip_ManulBt.Visible = true;
                //toolStripSeparator_Manul.Visible = true;
                //toolStrip_VisionBt.Visible = false;
                //toolStripSeparator_Vision.Visible = false;
                //toolStrip_LoginBt.Image = Cowain_AutoMotion.Properties.Resources.user2;
                //toolStrip_LoginBt.Text = "ICTEng";
                btnHome.Enabled = true;
                btnHome.Checked = false;
                btnAlarm.Enabled = true;
                btnAlarm.Checked = false;
                btnControl.Enabled = true;
                btnControl.Checked = false;
                btnData.Enabled = true;
                btnData.Checked = false;
                btnVision.Enabled = true;
                btnVision.Checked = false;
                btnSetting.Enabled = true;
                btnSetting.Checked = false;
                m_enLoginType = Sys_Define.enPasswordType.Eng;
                //labUser.Text = "Level 2";
                //labUser.BackColor = Color.FromArgb(192, 192, 255); 

                labelMode.Enabled = true;

                #region 图标显示
                picUser.Image = Cowain_AutoMotion.Properties.Resources.Level2;
                #endregion
            }
            else if (enLoginType == Sys_Define.enPasswordType.MacEng)  //enLoginType == Sys_Define.enPasswordType.ItEng ||
            {
                //toolStrip_AutoBt.Visible = true;
                //toolStripSeparator_Auto.Visible = true;
                //toolStrip_RecipeBt.Visible = true;
                //toolStripSeparator_Recipe.Visible = true;
                //toolStrip_ManulBt.Visible = true;
                //toolStripSeparator_Manul.Visible = true;
                //toolStrip_TeachBt.Visible = true;
                //toolStripSeparator_Teach.Visible = true;
                //toolStrip_VisionBt.Visible = false;
                //toolStripSeparator_Vision.Visible = false;
                //toolStrip_LoginBt.Image = Cowain_AutoMotion.Properties.Resources.user5;
                //toolStrip_LoginBt.Text = "SSE";
                btnHome.Enabled = true;
                btnHome.Checked = false;
                btnAlarm.Enabled = true;
                btnAlarm.Checked = false;
                btnControl.Enabled = true;
                btnControl.Checked = false;
                btnData.Enabled = true;
                btnData.Checked = false;
                btnVision.Enabled = true;
                btnVision.Checked = false;
                btnSetting.Enabled = true;
                btnSetting.Checked = false;
                m_enLoginType = Sys_Define.enPasswordType.Maker;
                //labUser.Text = "Level 2";
                //labUser.BackColor = Color.FromArgb(192, 192, 255);

                labelMode.Enabled = true;

                #region 图标显示
                picUser.Image = Cowain_AutoMotion.Properties.Resources.Level2;
                #endregion
            }
            else if (enLoginType == Sys_Define.enPasswordType.Maker)
            {
                //toolStrip_AutoBt.Visible = true;
                //toolStripSeparator_Auto.Visible = true;
                //toolStrip_RecipeBt.Visible = true;
                //toolStripSeparator_Recipe.Visible = true;
                //toolStrip_ManulBt.Visible = true;
                //toolStripSeparator_Manul.Visible = true;
                //toolStrip_TeachBt.Visible = true;
                //toolStripSeparator_Teach.Visible = true;
                //toolStrip_VisionBt.Visible = false;
                //toolStripSeparator_Vision.Visible = false;
                //toolStrip_LoginBt.Image = Cowain_AutoMotion.Properties.Resources.user5;
                //toolStrip_LoginBt.Text = "SW";
                btnHome.Enabled = true;
                btnHome.Checked = false;
                btnAlarm.Enabled = true;
                btnAlarm.Checked = false;
                btnControl.Enabled = true;
                btnControl.Checked = false;
                btnData.Enabled = true;
                btnData.Checked = false;
                btnVision.Enabled = true;
                btnVision.Checked = false;
                btnSetting.Enabled = true;
                btnSetting.Checked = false;
                m_enLoginType = Sys_Define.enPasswordType.Maker;
                //labUser.Text = "Level 3";
                //labUser.BackColor = Color.Red;

                labelMode.Enabled = true;

                #region 图标显示
                picUser.Image = Cowain_AutoMotion.Properties.Resources.Level3;
                #endregion
            }
            else
            {
                btnHome.Enabled = true;
                btnHome.Checked = false;
                btnAlarm.Enabled = true;
                btnAlarm.Checked = false;
                btnControl.Enabled = false;
                btnControl.Checked = false;
                btnData.Enabled = true;
                btnData.Checked = false;
                btnVision.Enabled = true;
                btnVision.Checked = false;
                btnSetting.Enabled = false;
                btnSetting.Checked = false;

                labelMode.Enabled = false;
                m_enLoginType = Sys_Define.enPasswordType.UnLogin;
                //labUser.Text = "";
                //labUser.BackColor = Color.White;

                #region 图标显示
                picUser.Image = Cowain_AutoMotion.Properties.Resources.No_login;
                #endregion
            }
            SettingForm fm = pFormView[(int)enformList.enSettingForm] as SettingForm;
            if (fm != null)
            {
                fm.SetUserButton(enLoginType);
            }

            //----------------------
            //ChangForm(enformList.enHomeForm);
        }

        private void CheckMachineError()
        {
            if (MachineDataDefine.pMachine == null)
                return;

            if (MachineDataDefine.pMachine.myErrorStack.Count != 0)
            {
                frm_Auto pAuto = null;
                Error pErr;
                if (pErrDlg == null)
                {
                    MachineDataDefine.pMachine.m_SgTower.TowerLight(clsMSignalTower.enTowerMode.enAlarm);
                    pErr = MachineDataDefine.pMachine.myErrorStack.Pop();
                    //--------------------
                    try
                    {
                        pErrDlg = new frm_ErrorDlg(ref pErr, ref MachineDataDefine.pMachine);
                        m_OnError = true;
                        ChangForm(enformList.enAlarmForm);
                        pErrDlg.ShowDialog();
                    }
                    catch
                    {
                        LogAuto.Notify("报警框弹出异常", (int)MachineStation.主监控, MotionLogLevel.Info);
                    }

                    //--------------------
                    pErrDlg = null;


                    if (pAuto != null)
                    {

                        pAuto = null;
                    }

                }
            }
        }
        private void getControl(Control.ControlCollection etc)
        {
            foreach (Control ct in etc)
            {
                try
                {
                    control.Add(ct);
                }
                catch
                { }

                if (ct.HasChildren)
                {
                    getControl(ct.Controls);
                }
            }
        }

        /// <summary>
        /// 调用LOG信息入队方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">log的信息，索引，等级</param>
        private void Util_MessageEvent(object sender, MessageEventArgs e)
        {
            messageQueue.ShowMessage(e);
        }

        private void SetTitleCenter()
        {
            string titleMsg = MESDataDefine.MESLXData.SW_Version;
            //Graphics g = this.CreateGraphics();
            //Double startingPoint = (this.Width / 2) - (g.MeasureString(titleMsg, this.Font).Width / 2);
            //Double widthOfASpace = g.MeasureString(" ", this.Font).Width;
            //String tmp = " ";
            //Double tmpWidth = 0;

            //while ((tmpWidth + widthOfASpace) < startingPoint)
            //{
            //    tmp += " ";
            //    tmpWidth += widthOfASpace;
            //}
            this.Text = titleMsg;
        }
        /// <summary>
        /// 切换运行、暂停、停止按钮的图片
        /// </summary>
        private void ChangeRunPic(enRunstate enRun)
        {
            switch (enRun)
            {
                case enRunstate.enRunning:
                    btnStart.Image = Cowain_AutoMotion.Properties.Resources.Running_Energized;
                    btnPause.Image = Cowain_AutoMotion.Properties.Resources.Paused_De_energized;
                    btnStop.Image = Cowain_AutoMotion.Properties.Resources.Stopped_De_energized;
                    break;
                case enRunstate.enPause:
                    btnStart.Image = Cowain_AutoMotion.Properties.Resources.Running_De_energized;
                    btnPause.Image = Cowain_AutoMotion.Properties.Resources.Energized_Paused;
                    btnStop.Image = Cowain_AutoMotion.Properties.Resources.Stopped_De_energized;
                    break;
                case enRunstate.enStop:
                    btnStart.Image = Cowain_AutoMotion.Properties.Resources.Running_De_energized;
                    btnPause.Image = Cowain_AutoMotion.Properties.Resources.Paused_De_energized;
                    btnStop.Image = Cowain_AutoMotion.Properties.Resources.Stopped_Energized;
                    break;
                default:
                    btnStart.Image = Cowain_AutoMotion.Properties.Resources.Running_De_energized;
                    btnPause.Image = Cowain_AutoMotion.Properties.Resources.Paused_De_energized;
                    btnStop.Image = Cowain_AutoMotion.Properties.Resources.Stopped_Energized;
                    break;
            }
        }

        /// <summary>
        /// 显示最近一片料的做料情况（上传MES成功与否）
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="ok"></param>
        private void ShowSN(string sn, bool ok)
        {
            HomeForm fm = (HomeForm)FormList[enformList.enHomeForm];
            fm.ShowSN(sn, ok);
            DataReviewForm dataForm = (DataReviewForm)FormList[enformList.enDataForm];
            dataForm.ShowSN(sn, ok);
        }

        private void btnAlarm_Click(object sender, EventArgs e)
        {
            ChangForm(enformList.enAlarmForm);
        }

        private void btnControl_Click(object sender, EventArgs e)
        {
            ChangForm(enformList.enControlForm);
        }

        private void btnData_Click(object sender, EventArgs e)
        {
            ChangForm(enformList.enDataForm);
        }

        private void btnVision_Click(object sender, EventArgs e)
        {
            ChangForm(enformList.enVisionForm);
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            ChangForm(enformList.enSettingForm);
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            ChangForm(enformList.enHomeForm);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            //回原判断
            if (!MachineDataDefine.pMachine.GetHomeCompleted())
            {
                if (MachineDataDefine.MachineLightEumn != MachineLightEumn.程序回原中)
                {
                    string strShowMessage = "归原";
                    formError.ErrorUnit1.AddActionMessage(strShowMessage);
                    LogAuto.Notify("Main窗体自动复位", (int)MachineStation.归零, MotionLogLevel.Hint);
                    MachineDataDefine.pMachine.DoHome();
                }
                else
                {
                    MsgBoxHelper.DxMsgShowInfo("程序正在回原中，请等待回原结束后再启动！");
                    return;
                }
            }

            if (!MachineDataDefine.IsAutoing && MachineDataDefine.pMachine.GetHomeCompleted())
            {
                if (ChartTime.lastRunStatus == MachineStatus.running)
                {
                    //if (!MachineDataDefine.machineState.b_UseMes)
                    //{
                    //    MsgBoxHelper.DxMsgShowWarn("Running 状态下必须启用mes,请检查是否是生产模式！");
                    //    return;
                    //}
                }
                if (MachineDataDefine.machineState.b_UseHive)
                {
                    //if (new Ping().Send("10.0.0.2", 3000).Status.ToString().Trim() != "Success")
                    //{
                    //    HIVE.HIVEInstance.HIVE_Reveice_Status = false;
                    //    return;
                    //}
                }
                if (MachineDataDefine.b_UseLAD != true && MachineDataDefine.machineState.b_UseHive)
                {
                    frm_Main.formData?.ChartTime1.StartRun();//2023.08.07新增
                }
                if (HIVEDataDefine.Hive_machineState.IsSendErrorDown && MachineDataDefine.machineState.b_UseHive)
                {
                    //MessageBox.Show("发送报警状态时不允许启动！", "提示", MessageBoxButtons.OK,
                    //   MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                    MsgBoxHelper.DxMsgShowWarn("发送报警状态时不允许启动！");
                    return;
                }
                if (MachineDataDefine.IsFormOpen)
                {

                    //MessageBox.Show("请检查设备安全门是否已全部关闭！", "提示", MessageBoxButtons.OK,
                    //   MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                    MsgBoxHelper.DxMsgShowWarn("请检查设备安全门是否已全部关闭！");
                    LogAuto.Notify("设备安全门存在未关闭状况！", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                    return;
                }
                LogAuto.Notify("窗体启动button按下", (int)MachineStation.主监控, MotionLogLevel.Info);

                if (MachineDataDefine.b_UseLAD != true)
                {
                    ChangeRunPic(enRunstate.enRunning);
                }
                MachineDataDefine.pMachine.StateChoose();

                btnStart.Checked = true;
                btnPause.Checked = false;
                btnStop.Checked = false;

                btnPause.Enabled = true;
                btnStop.Enabled = true;

            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            //if (m_bPause == false)
            //{
            //    pMachine.EnablePause();
            //    m_bPause = true;
            //}
            //else
            //{
            //    pMachine.DisablePause();
            //    m_bPause = false;
            //}
            //循环停止
            //MachineDataDefine.MachineControlS.IsCycleStop = true;
            //pMachine.NeedRef = true;

            //btnStart.Checked = false;
            //btnPause.Checked = true;
            //btnStop.Checked = false;

            MachineDataDefine.pMachine.StopAuto(false);
            btnStart.Checked = false;
            btnPause.Checked = true;
            btnStop.Checked = false;

            ChangeRunPic(enRunstate.enPause);
            //在启用远程模式时，需要发送idel状态
            if (MachineDataDefine.machineState.b_UseRemoteQualification)
            {
                HIVE.HIVEInstance.HiveSendMACHINESTATE(2, "", "Manually press the stop button", "Manually Downtime", true, MachineDataDefine.m_CardID);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (frm_Main.formData.ChartTime1.RunStatus != ChartTime.MachineStatus.error_down)
            {
                //刷卡二次确认
                frm_PlanDoubleConfirm doubleConfm = new frm_PlanDoubleConfirm(ref MachineDataDefine.pMachine);
                doubleConfm.NeedSN = false;
                doubleConfm.PlanType = "";
                if (doubleConfm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
            }
            frm_Main.formError.ErrorUnit1.StartErrorMessage("90099");

            LogAuto.Notify("停止button按下", (int)MachineStation.主监控, MotionLogLevel.Alarm);

            //if (pMachine.GetisAutoing())
            //{
            frm_Main.formData.ChartTime1.ForceDown();
            //}

            if (!HIVE.HIVEInstance.NotUploadErrorMessageList.Exists(t => t == frm_Main.formError.ErrorUnit1.ErrorMessage))
            {
                if (MachineDataDefine.machineState.b_UseHive /*&& !MachineDataDefine.machineState.IsAbleTestRun*/
                    && !HIVEDataDefine.Hive_machineState.IsSendErrorDown)
                {
                    HIVE.HIVEInstance.HiveSendMACHINESTATE(5, "O09OOOO-99-99", "Manually press the stop button", "Manually Downtime", false, MachineDataDefine.m_CardID);
                    MachineDataDefine.m_CardID = "";
                }
                if (MachineDataDefine.machineState.b_UseRemoteQualification /*&& !MachineDataDefine.machineState.IsAbleTestRun*/
                    && !HIVEDataDefine.Hive_machineState.IsSendErrorDown)
                {
                    HIVE.HIVEInstance.HiveSendMACHINESTATE(5, "O09OOOO-99-99", "Manually press the stop button", "Manually Downtime", true, MachineDataDefine.m_CardID);
                    MachineDataDefine.m_CardID = "";
                }
            }
            MachineDataDefine.pMachine.StopAuto();
            btnStart.Checked = false;
            btnPause.Checked = false;
            btnStop.Checked = true;
            ChangeRunPic(enRunstate.enStop);
            frm_Main.formError.ErrorUnit1.EndErrorMessage("90099");
        }
        private frm_HealthWarm healthFrm;
        private void showHealthForm(string NGmsg)
        {
            if (healthFrm == null || healthFrm.IsDisposed)
            {
                healthFrm = new frm_HealthWarm(ref MachineDataDefine.pMachine, NGmsg);
            }
            if (healthFrm.Visible)
            {
                return;
            }
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    healthFrm.Show();
                }));
            }
            else
            {
                healthFrm.Show();
            }
            if (!MachineDataDefine.machineState.b_Usehummer)//蜂鸣器未禁用
            {
                MachineDataDefine.pMachine.m_Buzzer.SetIO(true);
            }
        }
        private void frm_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dlgResult;
            //if (frm_Main.formData?.ChartTime1.RunStatus == Chart.ChartTime.MachineStatus.error_down)
            //{
            //    dlgResult = MsgBoxHelper.DxMsgShowQues(JudgeLanguage.JudgeLag(" 当前为DownTime状态请切换状态后退出软件"), JudgeLanguage.JudgeLag("将关闭软件"));
            //    e.Cancel = true;
            //    return;
            //}
            //else
            //{
            dlgResult = MsgBoxHelper.DxMsgShowQues(JudgeLanguage.JudgeLag(" 请确认是否退出软件"), JudgeLanguage.JudgeLag("将关闭软件"));
            // }

            if (dlgResult != DialogResult.Yes)
            {
                e.Cancel = true;
            }
            else
            {
                try
                {
                    if (MachineDataDefine.backup != null)
                    {
                        MachineDataDefine.backup.Stop();        // 停止自动备份线程 2022.02.21新增
                    }
                    //RunDataDefine.RunDataS.WriteParams(DispenserDataDefine.DispenserDataS);
                    RunDataDefine.RunDataS.WriteParams(RunDataDefine.RunDataS);
                    formData.ChartTime1.b_Close = true;
                    //Form form1 = pFormView[(int)enformList.enConveyorForm];
                    //((frm_Conveyor)form1).OF.b_Close = true;
                    //MachineDataDefine.pMachine.m_LightStart.SetIO(false);
                    MachineDataDefine.pMachine.m_LightTowerG.SetIO(false);
                    MachineDataDefine.pMachine.m_LightTowerR.SetIO(false);
                    MachineDataDefine.pMachine.m_LightTowerY.SetIO(false);
                    MachineDataDefine.pMachine.m_Buzzer.SetIO(false);
                }
                catch (Exception err)
                { }

                Process[] allProcesses = Process.GetProcesses();
                foreach (Process item in allProcesses)
                {
                    if (item.ProcessName.Contains("Cowain_AutoMotion"))
                    {
                        item.Kill();
                    }
                }
            }
        }

        private frm_ModeSelect.enWorkMode m_SelectWorkMode = frm_ModeSelect.enWorkMode.original;
        private int m_GantryType = -1;

        private void labelMode_Click(object sender, EventArgs e)
        {
            if ((int)MachineDataDefine.pMachine.m_LoginUser < 1)
            {
                //权限不够直接退出
                return;
            }
            frm_ModeSelect fm = new frm_ModeSelect(ref MachineDataDefine.pMachine);
            if (fm.ShowDialog() == DialogResult.OK)
            {
                if (fm.CurMode == frm_ModeSelect.enWorkMode.Planned_Downtime)
                {
                    m_GantryType = fm.GantryType;
                }
                #region 退出计划停机输入相关信息,弃用
                //frm_PlanDoubleConfirm doubleConfm = new frm_PlanDoubleConfirm();
                //doubleConfm.GantryType = m_GantryType;
                //if (m_SelectWorkMode == frm_ModeSelect.enWorkMode.Planned_Downtime &&
                //    fm.CurMode != frm_ModeSelect.enWorkMode.Planned_Downtime)
                //{
                //    switch (m_CurStopMode)
                //    {
                //        case enPlanMode.日常点检:
                //        case enPlanMode.更换针头:
                //        case enPlanMode.更换胶阀:
                //        case enPlanMode.设备耗材更换:
                //        case enPlanMode.镭射标定:
                //        case enPlanMode.MaterialReplacement:
                //        case enPlanMode.压力测试:
                //        case enPlanMode.周点检:
                //        case enPlanMode.其它:
                //            //new frm_PlanDoubleConfirm(m_CurMode.ToString(),false);
                //            doubleConfm.NeedSN = false;
                //            doubleConfm.PlanType = m_CurStopMode.ToString();
                //            break;
                //        case enPlanMode.更换AB胶水:
                //        case enPlanMode.更换HM胶水:
                //            //new frm_PlanDoubleConfirm(m_CurMode.ToString(),true);
                //            doubleConfm.NeedSN = true;
                //            doubleConfm.PlanType = m_CurStopMode.ToString();
                //            break;
                //        default:
                //            MsgBoxHelper.DxMsgShowErr("计划停机类型异常！");
                //            break;
                //    }

                //    if (doubleConfm.ShowDialog() != DialogResult.OK)
                //    {
                //        return;
                //    }
                //    else
                //    {
                //        string loginfo = "退出计划停机，切换到["+ fm.CurMode.ToString() + "]状态;Operator:"+ doubleConfm.txt_Op.Text.Trim() + ";Old SN:"+ doubleConfm.txt_Old.Text.Trim() + ";New SN:"+ doubleConfm.txt_New.Text.Trim() + ";";
                //        LogAuto.Notify(loginfo, (int)MachineStation.主监控, MotionLogLevel.Info);
                //    }
                //}
                #endregion
                bool send = false;
                if (fm.CurMode == frm_ModeSelect.enWorkMode.Running)
                {
                    //if ((HIVEState)formData.ChartTime1.RunStatus == HIVEState.运行中)
                    //{
                    //    labelMode.Text = "Running";
                    //    labelMode.BackColor = Color.FromArgb(128, 255, 128);
                    //}
                    //else if((HIVEState)formData.ChartTime1.RunStatus == HIVEState.待料中)
                    //{
                    //    labelMode.Text = "Idle";
                    //    labelMode.BackColor = Color.FromArgb(192, 255, 255);
                    //}
                    //else if ((HIVEState)formData.ChartTime1.RunStatus == HIVEState.故障停机中)
                    //{
                    //    labelMode.Text = "Down Time";
                    //    labelMode.BackColor = Color.FromArgb(255, 128, 128);
                    //}
                    //labelMode.Text = "Running";
                    //labelMode.BackColor = Color.FromArgb(211, 235, 115);

                    //if (m_SelectWorkMode != frm_ModeSelect.enWorkMode.Running)
                    //{
                    send = frm_Main.formData.ChartTime1.MalRun();
                    if (MachineDataDefine.machineState.b_UseHive && !HIVEDataDefine.Hive_machineState.IsSendErrorDown && send)
                    {
                        HIVE.HIVEInstance.HiveSendMACHINESTATE(1, "", "", "", false, MachineDataDefine.m_CardID);
                    }
                    if (MachineDataDefine.machineState.b_UseRemoteQualification && !HIVEDataDefine.Hive_machineState.IsSendErrorDown && send)
                    {
                        HIVE.HIVEInstance.HiveSendMACHINESTATE(1, "", "", "", true, MachineDataDefine.m_CardID);
                    }
                    MachineDataDefine.m_CardID = "";
                    m_SelectWorkMode = frm_ModeSelect.enWorkMode.Running;
                    //}
                }
                else if (fm.CurMode == frm_ModeSelect.enWorkMode.Idle)
                {
                    //labelMode.Text = "Idle";
                    //labelMode.BackColor = Color.FromArgb(235, 255, 254);
                    //if (m_SelectWorkMode != frm_ModeSelect.enWorkMode.Idle)
                    //{
                    send = frm_Main.formData.ChartTime1.MalIdle();

                    if (MachineDataDefine.machineState.b_UseHive && !HIVEDataDefine.Hive_machineState.IsSendErrorDown && send)
                    {
                        HIVE.HIVEInstance.HiveSendMACHINESTATE(2, "", "", "", false, MachineDataDefine.m_CardID);
                    }
                    if (MachineDataDefine.machineState.b_UseRemoteQualification && !HIVEDataDefine.Hive_machineState.IsSendErrorDown && send)
                    {
                        HIVE.HIVEInstance.HiveSendMACHINESTATE(2, "", "", "", true, MachineDataDefine.m_CardID);
                    }
                    MachineDataDefine.m_CardID = "";
                    //}
                    m_SelectWorkMode = frm_ModeSelect.enWorkMode.Idle;
                }
                else if (fm.CurMode == frm_ModeSelect.enWorkMode.Engineering)
                {
                    //labelMode.Text = "Engineering";
                    //labelMode.BackColor = Color.FromArgb(204, 171, 216);
                    //if (m_SelectWorkMode != frm_ModeSelect.enWorkMode.Engineering)
                    //{
                    send = frm_Main.formData.ChartTime1.MalEng();

                    if (MachineDataDefine.machineState.b_UseHive && !HIVEDataDefine.Hive_machineState.IsSendErrorDown && send)
                    {
                        HIVE.HIVEInstance.HiveSendMACHINESTATE(3, "", "", "", false, MachineDataDefine.m_CardID);
                    }
                    if (MachineDataDefine.machineState.b_UseRemoteQualification && !HIVEDataDefine.Hive_machineState.IsSendErrorDown && send)
                    {
                        HIVE.HIVEInstance.HiveSendMACHINESTATE(3, "", "", "", true, MachineDataDefine.m_CardID);
                    }
                    MachineDataDefine.m_CardID = "";

                    m_SelectWorkMode = frm_ModeSelect.enWorkMode.Engineering;
                    //}

                }
                else if (fm.CurMode == frm_ModeSelect.enWorkMode.Planned_Downtime)
                {
                    //labelMode.Text = "Planned DT";
                    //labelMode.BackColor = Color.FromArgb(255, 215, 212);
                    //if (m_SelectWorkMode != frm_ModeSelect.enWorkMode.Planned_Downtime&&
                    //    m_CurStopMode != (enPlanMode)fm.CurStopMode)
                    //{
                    if (fm.CurStopMode == frm_ModeSelect.enPlanMode.日常点检)
                    {
                        fm.SendHive("PD-01", "Check", "Daily Maintenance", MachineDataDefine.m_CardID);
                    }
                    else if (fm.CurStopMode == frm_ModeSelect.enPlanMode.更换AB胶水)
                    {
                        fm.SendHive("PD-02", "AB Glue", "Glue Change", MachineDataDefine.m_CardID);
                    }
                    else if (fm.CurStopMode == frm_ModeSelect.enPlanMode.更换HM胶水)
                    {
                        fm.SendHive("PD-02", "HM Gule", "Glue Change", MachineDataDefine.m_CardID);
                    }
                    else if (fm.CurStopMode == frm_ModeSelect.enPlanMode.更换针头)
                    {
                        fm.SendHive("PD-03", "", "Needle Change", MachineDataDefine.m_CardID);
                    }
                    else if (fm.CurStopMode == frm_ModeSelect.enPlanMode.更换胶阀)
                    {
                        fm.SendHive("PD-04", "", "Valve Change", MachineDataDefine.m_CardID);
                    }
                    else if (fm.CurStopMode == frm_ModeSelect.enPlanMode.压力测试)
                    {
                        fm.SendHive("PD-05", "", "Stress Test", MachineDataDefine.m_CardID);
                    }
                    else if (fm.CurStopMode == frm_ModeSelect.enPlanMode.镭射标定)
                    {
                        fm.SendHive("PD-06", "", "Laser Calibration", MachineDataDefine.m_CardID);
                    }
                    else if (fm.CurStopMode == frm_ModeSelect.enPlanMode.设备耗材更换)
                    {
                        fm.SendHive("PD-07", "", "Consumable Part Replacement", MachineDataDefine.m_CardID);
                    }
                    else if (fm.CurStopMode == frm_ModeSelect.enPlanMode.MaterialReplacement)
                    {
                        fm.SendHive("PD-08", "", "Material Replacement", MachineDataDefine.m_CardID);
                    }
                    else if (fm.CurStopMode == frm_ModeSelect.enPlanMode.周点检)
                    {
                        fm.SendHive("PD-09", "", "Weekly Maintenance", MachineDataDefine.m_CardID);
                    }
                    else if (fm.CurStopMode == frm_ModeSelect.enPlanMode.胶水称重)
                    {
                        fm.SendHive("PD-10", "", "Glue Weighing", MachineDataDefine.m_CardID);
                    }
                    else if (fm.CurStopMode == frm_ModeSelect.enPlanMode.LAD)
                    {
                        fm.SendHive("PD-11", "", "LAD", MachineDataDefine.m_CardID);
                    }
                    else if (fm.CurStopMode == frm_ModeSelect.enPlanMode.其它)
                    {
                        fm.SendHive("PD-101", "", "Others", MachineDataDefine.m_CardID);
                    }
                    MachineDataDefine.m_CardID = "";
                    m_CurStopMode = (enPlanMode)fm.CurStopMode;
                    m_SelectWorkMode = frm_ModeSelect.enWorkMode.Planned_Downtime;
                    //}
                    //m_CurStopMode = (enPlanMode)fm.CurStopMode;
                    //m_SelectWorkMode = frm_ModeSelect.enWorkMode.Planned_Downtime;
                }
                else if (fm.CurMode == frm_ModeSelect.enWorkMode.Manually_Downtime)
                {
                    //labelMode.Text = "Manually DT";
                    //labelMode.BackColor = Color.FromArgb(235, 115, 115);
                    //if (m_SelectWorkMode != frm_ModeSelect.enWorkMode.Manually_Downtime)
                    //{
                    Task.Run(() =>
                    {
                        {
                            frm_Main.formData.Chartcapacity1.AddDT();
                            //frm_Main.formError.ErrorUnit1.StartErrorMessage(m_Error.m_ErrorCode.ToString("0000"));
                            frm_Main.formError.ErrorUnit1.StartErrorMessage("90099");

                            //if (!MachineDataDefine.machineState.IsAbleTestRun)
                            //{
                            frm_Main.formData.ChartTime1.MalDown();
                            //}

                            //if (!HIVE.HIVEInstance.NotUploadErrorMessageList.Exists(t => t == frm_Main.formError.ErrorUnit1.ErrorMessage))
                            //{
                            if (MachineDataDefine.machineState.b_UseHive /*&& !MachineDataDefine.machineState.IsAbleTestRun*/
                            && !HIVEDataDefine.Hive_machineState.IsSendErrorDown)
                            {
                                HIVE.HIVEInstance.HiveSendMACHINESTATE(5, "O09OOOO-99-99", "Manually press the stop button", "Manually Downtime", false, MachineDataDefine.m_CardID);
                                MachineDataDefine.m_CardID = "";
                            }
                            if (MachineDataDefine.machineState.b_UseRemoteQualification /*&& !MachineDataDefine.machineState.IsAbleTestRun*/
                                                           && !HIVEDataDefine.Hive_machineState.IsSendErrorDown)
                            {
                                HIVE.HIVEInstance.HiveSendMACHINESTATE(5, "O09OOOO-99-99", "Manually press the stop button", "Manually Downtime", true, MachineDataDefine.m_CardID);
                                MachineDataDefine.m_CardID = "";
                            }
                            frm_Main.formError.ErrorUnit1.EndErrorMessage("90099");
                            //}
                        }
                    });

                    m_SelectWorkMode = frm_ModeSelect.enWorkMode.Manually_Downtime;
                    //}
                }
            }

        }

        private void pictureEdit1_Click(object sender, EventArgs e)
        {
            //pMachine.CloseCard();
        }

        private void picUser_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void picUser_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DialogResult result = MsgBoxHelper.DxMsgShowQues("确定要登出吗？");
                if (result == DialogResult.Yes)
                {
                    //注销
                    MachineDataDefine.pMachine.m_LoginUser = MotionBase.Sys_Define.enPasswordType.UnLogin;
                    MachineDataDefine.m_LoginUser = MachineDataDefine.pMachine.m_LoginUser;
                    MachineDataDefine.m_LoginUserName = "";
                    MachineDataDefine.m_LoginCardID = "";
                    MachineDataDefine.Login_Name = "null";
                    MachineDataDefine.Login_Function = "null";
                    MachineDataDefine.Login_CardID = "null";
                    MachineDataDefine.Authorize_Name = "null";
                    MachineDataDefine.Authorize_Function = "null";
                    MachineDataDefine.Authorize_CardID = "null";
                    LogAuto.Notify("退出登录" + ":" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + $" User:{MachineDataDefine.Login_Name} Function:{MachineDataDefine.Login_Function} UID:{MachineDataDefine.Login_CardID} Authorize:{MachineDataDefine.Authorize_Name} Function:{MachineDataDefine.Authorize_Function} UID:{MachineDataDefine.Authorize_CardID} ", (int)MachineStation.主监控, MotionLogLevel.Info);
                    string ss = $" User:{MachineDataDefine.Login_Name} Function:{MachineDataDefine.Login_Function} UID:{MachineDataDefine.Login_CardID} Authorize:{MachineDataDefine.Authorize_Name} Function:{MachineDataDefine.Authorize_Function} UID:{MachineDataDefine.Authorize_CardID} ";
                    MachineDataDefine.msg = ss;
                    ChangForm(enformList.enHomeForm);
                    ShowUserButton(MachineDataDefine.pMachine.m_LoginUser);
                }
                else
                {
                    LogAuto.Notify("取消退出登录" + ":" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + $" User:{MachineDataDefine.Login_Name} Function:{MachineDataDefine.Login_Function} UID:{MachineDataDefine.Login_CardID} Authorize:{MachineDataDefine.Authorize_Name} Function:{MachineDataDefine.Authorize_Function} UID:{MachineDataDefine.Authorize_CardID} ", (int)MachineStation.主监控, MotionLogLevel.Info);

                    string ss = $" User:{MachineDataDefine.Login_Name} Function:{MachineDataDefine.Login_Function} UID:{MachineDataDefine.Login_CardID} Authorize:{MachineDataDefine.Authorize_Name} Function:{MachineDataDefine.Authorize_Function} UID:{MachineDataDefine.Authorize_CardID} ";
                    MachineDataDefine.msg = ss;
                }
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!MachineDataDefine.machineState.b_UseMesLogin)
            {
                dia_Login_New m_LoginDlg = new dia_Login_New(ref MachineDataDefine.pMachine);
                //dia_Login_Remote m_LoginDlg = new dia_Login_Remote(ref pMachine);
                //------------
                m_LoginDlg.ShowDialog();
            }
            else
            {
                //dia_Login_New m_LoginDlg = new dia_Login_New(ref pMachine);
                dia_Login_Remote m_LoginDlg = new dia_Login_Remote(ref MachineDataDefine.pMachine);
                //------------
                m_LoginDlg.ShowDialog();
            }
            ////dia_Login m_LoginDlg = new dia_Login(ref pMachine);
            //dia_Login_New m_LoginDlg = new dia_Login_New(ref pMachine);
            ////dia_Login_Remote m_LoginDlg = new dia_Login_Remote(ref pMachine);
            ////------------
            //m_LoginDlg.ShowDialog();
            #region 图标显示
            if (MachineDataDefine.pMachine.m_LoginUser != Sys_Define.enPasswordType.UnLogin && MachineDataDefine.Authorizeis)
            {
                MachineDataDefine.Authorizeis = false;
                btnControl.Image = Cowain_AutoMotion.Properties.Resources.Config_De_energized;
                btnData.Image = Cowain_AutoMotion.Properties.Resources.Data_De_energized;
                btnVision.Image = Cowain_AutoMotion.Properties.Resources.Vision_De_energized;
                btnSetting.Image = Cowain_AutoMotion.Properties.Resources.Setting_De_energized;
                LogAuto.Notify("登录成功" + ":" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + $" User:{MachineDataDefine.Login_Name} Function:{MachineDataDefine.Login_Function} UID:{MachineDataDefine.Login_CardID} Authorize:{MachineDataDefine.Authorize_Name} Function:{MachineDataDefine.Authorize_Function} UID:{MachineDataDefine.Authorize_CardID} ", (int)MachineStation.主监控, MotionLogLevel.Info);
                // MachineDataDefine.msg = "登录成功" + ":" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + $" User:{MachineDataDefine.Login_Name} Function:{MachineDataDefine.Login_Function} UID:{MachineDataDefine.Login_CardID} Authorize:{MachineDataDefine.Authorize_Name} Function:{MachineDataDefine.Authorize_Function} UID:{MachineDataDefine.Authorize_CardID} ";
                MachineDataDefine.msg = $" User:{MachineDataDefine.Login_Name} Function:{MachineDataDefine.Login_Function} UID:{MachineDataDefine.Login_CardID} Authorize:{MachineDataDefine.Authorize_Name} Function:{MachineDataDefine.Authorize_Function} UID:{MachineDataDefine.Authorize_CardID} ";
            }
            else
            {
                btnControl.Image = Cowain_AutoMotion.Properties.Resources.Config_Disabled;
                btnData.Image = Cowain_AutoMotion.Properties.Resources.Data_Disabled;
                btnVision.Image = Cowain_AutoMotion.Properties.Resources.Vision_Disabled;
                btnSetting.Image = Cowain_AutoMotion.Properties.Resources.Setting_Disabled;
                LogAuto.Notify("登录失败" + ":" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + $" User:{MachineDataDefine.Login_Name} Function:{MachineDataDefine.Login_Function} UID:{MachineDataDefine.Login_CardID} Authorize:{MachineDataDefine.Authorize_Name} Function:{MachineDataDefine.Authorize_Function} UID:{MachineDataDefine.Authorize_CardID} ", (int)MachineStation.主监控, MotionLogLevel.Info);
                //   MachineDataDefine.msg = "登录失败" + ":" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + $" User:{MachineDataDefine.Login_Name} Function:{MachineDataDefine.Login_Function} UID:{MachineDataDefine.Login_CardID} Authorize:{MachineDataDefine.Authorize_Name} Function:{MachineDataDefine.Authorize_Function} UID:{MachineDataDefine.Authorize_CardID} ";
                MachineDataDefine.msg = $" User:{MachineDataDefine.Login_Name} Function:{MachineDataDefine.Login_Function} UID:{MachineDataDefine.Login_CardID} Authorize:{MachineDataDefine.Authorize_Name} Function:{MachineDataDefine.Authorize_Function} UID:{MachineDataDefine.Authorize_CardID} ";
            }
            #endregion
            ShowUserButton(MachineDataDefine.pMachine.m_LoginUser);
            ChangForm(enformList.enHomeForm);
            //m_LoginDlg.Close();
            //-------------------------
        }


    }

    public class MouseKeyBoardOperate
    {
        /// <summary>
        /// 创建结构体用于返回捕获时间
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        struct LASTINPUTINFO
        {
            /// <summary>
            /// 设置结构体块容量
            /// </summary>
            [MarshalAs(UnmanagedType.U4)]
            public int cbSize;

            /// <summary>
            /// 抓获的时间
            /// </summary>
            [MarshalAs(UnmanagedType.U4)]
            public uint dwTime;
        }

        [DllImport("user32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);
        /// <summary>
        /// 获取键盘和鼠标没有操作的时间
        /// </summary>
        /// <returns>用户上次使用系统到现在的时间间隔，单位为秒</returns>
        public static long GetLastInputTime()
        {
            LASTINPUTINFO vLastInputInfo = new LASTINPUTINFO();
            vLastInputInfo.cbSize = Marshal.SizeOf(vLastInputInfo);
            if (!GetLastInputInfo(ref vLastInputInfo))
            {
                return 0;
            }
            else
            {
                uint count = (uint)(Environment.TickCount - vLastInputInfo.dwTime);
                long icount = count / 1000;
                return icount;
            }
        }
    }
}