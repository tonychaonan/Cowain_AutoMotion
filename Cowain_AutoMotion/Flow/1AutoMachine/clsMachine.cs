using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.InteropServices;
using MotionBase;
using Cowain_Form.FormView;
using ToolTotal_1;
using Cowain_AutoMotion.Flow;
using System.Threading.Tasks;
using Chart;
using System.Windows.Forms;
using Cowain;
using Cowain_AutoMotion.FormView;
using static Cowain_Machine.Flow.MErrorDefine;
using System.Threading;
using Cowain_AutoMotion.Flow.Hive;
using System.Net.NetworkInformation;
using Cowain_AutoMotion;
using Cowain_AutoMachine.Flow.IO_Cylinder;
using OpenOffice_Connect;
using System.Reflection;
using Cowain_AutoMotion.Flow._2Work;

namespace Cowain_Machine.Flow
{
    public class clsMachine : Base
    {
        #region Machine建構式&解構式

        public clsMachine(Type homeEnum1, Type stepEnum1, string instanceName1) : base(homeEnum1, stepEnum1, instanceName1)
        {
            //******* 2021.01.16新增, 讀取SystemParm   ********
            String strPath = System.IO.Directory.GetCurrentDirectory();
            String strNowPath = strPath.Replace("\\bin\\x64\\Debug", "");
            //  String strSysIniPath = strNowPath + "\\SystemParameter.ini";
            //-----------
            //MSystemParameter.m_SysParm.Initial();
            //MSystemParameter.m_SysParm.LoadParameter(strSysIniPath);
            //*************************************************
            String strBasePath = strNowPath + "\\DataSet.xml";
            m_strLogBaseDirectory = strNowPath;
            GetWorkAndMachinePath(ref strBasePath);
            //-----------------------------
            SetMachineDataPath(m_strMachinePath);
            //-----------------------------
            m_SgTower = new clsMSignalTower(typeof(Base.HomeStep_Base), typeof(clsMSignalTower.enStep), "三色灯", this, 0, "SignalTower", "三色燈", 1000);
            AddBase(ref m_SgTower.m_NowAddress);
            //-----------------------------
            //在这里加载workProcess
            WorkProcessLoad.instance.workProcessInitial(this);
            //--------------------------------------
            StartInitial();//初始化轴卡
            //------------------------轴卡加载完成后，再往下执行
            int iRetCode = 0;
            for (int i = 0; i < 1000; i++)
            {
                Thread.Sleep(10);
                GetInitialStatus(ref iRetCode);
                if (iRetCode > 1000)
                {
                    break;
                }
            }
            //------------------------
            string path11 = System.IO.Directory.GetCurrentDirectory().Replace("\\Cowain_AutoMotion\\bin\\x64\\Debug", "");
            SetIOs();
            m_SgTower.SetIOSignal(ref m_LightTowerR, ref m_LightTowerY, ref m_LightTowerG, ref m_Buzzer);
            //-----------------------------
            m_InitStep = enInitStep.StartLoading;
            m_isShutDown = false;
            thLight = new Thread(refreshLightAndCheckController);
            thLight.Priority = ThreadPriority.Lowest;
            thLight.IsBackground = true;
            thLight.Start();

        }
        ~clsMachine()
        {
            m_isShutDown = true;
        }
        #endregion

        //***************************
        public frm_Main pfrmMain;
        Thread thLight;
        /// <summary>
        /// 用于主页用户刷新
        /// </summary>
        public bool NeedRef = false;
        /// <summary>
        /// 显示提示的事件
        /// </summary>
        public static event Action<string> showHinttEvent;

        [DllImport("User32.dll")]
        public static extern int PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        //***************************
        public enum enHomeStep
        {
            StartHome = 0,
            判断安全位,
            前龙门回原,
            后龙门回原,
            把夹爪松开,
            等待夹爪松开,
            Work_HomeCompleted,
            HomeCompleted,
        }
        enHomeStep m_HomeStep;
        public enum enStep
        {
            StartAuto = 0,
            调整位膨胀气缸初始状态回,
            调整位膨胀顶升气缸初始状态下,
            调整位定位销固定顶升气缸初始状态下1,
            调整位压板限位气缸初始状态缩回,
            调整位压板限位气缸缩回到位,
            ToSafePos,
            AllStAuto,
            Autoing,
            //--------------
            //--------------
            CheckOnlyNextLens,
            PickAutoing,
            AutoStop,
            HoldTrayReset,
            Pausing,

        }
        enStep m_Step;
        public enum enInitStep
        {
            StartLoading,
            載入Machine_Data,
            載入Work_Data,
            系統資料Init,
            系統Init完成,
            enMax,
        }
        enInitStep m_InitStep;
        //**********************
        public DrvIO m_EmgIO, m_AirOk, m_DoorSR1, m_DoorSR2, m_DoorSR3, m_DoorSR4;   //Input I/O
        public DrvIO m_Start, m_Stop, m_Reset;
        public DrvIO m_inSafety, m_OutSafety;
        //******************************
        public DrvIO m_LightTowerR, m_LightTowerG, m_LightTowerY, m_Buzzer;   //Output I/O
                                                                              //   public DrvIO m_LightStart, m_LightStop, m_LightReset;
        /// <summary>
        /// 外部触发循环停止
        /// </summary>
        public DrvIO Isstop;
        //******************************
        public clsMSignalTower m_SgTower = null;
        //public clsCCDSystem m_VisionSystem;

        public bool m_bAutoing = false, m_bPausing = false, b_iSafeDoorShow = true;
        bool m_bisEmg = false, m_bAirInsufficient = false;
        public bool m_bCycleStop = false;
        //----------------------
        private double m_dbSpeed;
        public double m_dbAutoingSpeed = 50;
        /// <summary>
        /// 门检查功能
        /// </summary>
        public bool m_bCheckDoorSR = false;
        /// <summary>
        /// 安全光栅是否触发
        /// </summary>
        public bool anquan = false;
        /// <summary>
        /// 安全光栅触发及时
        /// </summary>
        public DateTime anquanTime = DateTime.Now;
        //  public bool m_checkTestMode = false;
        /// <summary>
        /// 检查软件版本变量
        /// </summary>
        public bool b_Version = true;
        //----------------------

        //**********************
        public Stack<Error> myErrorStack = new Stack<Error>();

        bool m_isShutDown, m_isErrorHappen;
        String m_strWorkDirectory, m_strDataSetPath;
        String m_strLogBaseDirectory;

        狀態[] m_NowStatus = new 狀態[5];

        //************************
        public void ReMoveEmgStatus() { m_bisEmg = false; }
        public void ReMoveAirInsufficient() { m_bAirInsufficient = false; }
        public bool GetisAutoing() { return m_bAutoing; }

        public bool GetisPausing() { return m_bPausing; }

        public String GetWorkDataDirectory() { return m_strWorkDirectory; }
        public String GetWorkDataPath() { return m_strWorkPath; }
        public String GetMachineDataPath() { return m_strMachinePath; }

        //******************************
        private bool GetWorkAndMachinePath(ref string path)
        {
            m_strDataSetPath = path;
            m_strMachinePath = m_strWorkPath = m_strWorkDirectory = "";
            //-----------------
            m_strMachinePath = Application.StartupPath.Replace(@"Cowain_AutoMotion\bin\x64\Debug", @"DataBaseData\Machine.mdb");
            m_strWorkDirectory = Application.StartupPath.Replace(@"Cowain_AutoMotion\bin\x64\Debug", @"DataBaseData\WorkData\");
            m_strWorkPath = Application.StartupPath.Replace(@"Cowain_AutoMotion\bin\x64\Debug", @"DataBaseData\WorkData\MYTEST.xml");
            //GetXmlData(path, "DataSet/PathData", "MachinePath", ref m_strMachinePath);
            //GetXmlData(path, "DataSet/PathData", "WorkDirectory", ref m_strWorkDirectory);
            //GetXmlData(path, "DataSet/PathData", "WorkPath", ref m_strWorkPath);
            return true;
        }
        public bool SaveWorkPath(string strPath)
        {
            //bool bRet=SaveToDataBase(m_strDataSetPath, "Path", "ID_NAME", "WorkPath", "Path", strPath);
            bool bRet = SaveXmlData(m_strDataSetPath, "DataSet/PathData", "WorkPath", strPath);
            return bRet;
        }

        public override void ErrorHappen(ref Error pError)  //異常發生
        {
            myErrorStack.Push(pError);
            //PostMessage(pfrmMain.Handle, frm_Main.WM_ShowErrorDlg, 0, 0);
        }
        //*****************************
        public override void Stop()
        {
            Base.stop();
            //初始化变量
            //AxisTakeOut.onec = true;
            //AxisTakeOut.screw = false;
            //AxisTakeOut.recheck = false;
            //AxisTakeOut.screwValue = "999";
            //AxisTakeOut.i = 1;
            //AxisTakeOut.finish = false;
            //AxisTakeIn.holeSN = "";
            //AxisTakeIn.ProductSN = "";
            //AxisTakeIn.electricReady = false;
            //AxisTakeIn.electricStart = false;
            //气缸停止
            // HardWareControl.stopGetValve();??
            MachineDataDefine.RobotDownError = false;
            MachineDataDefine.machineState.Isdia_ShowModelShow = false;
            //MachineDataDefine.FirstProduct = true;
            MachineDataDefine.remoteFirstProduct = false;
            //RunnerOut.first = true;
            bool NextStop = false;
            MachineDataDefine.NGmac = false;
            MachineDataDefine.MachineControlS.continueNG_three.Clear();
            MachineDataDefine.MachineControlS.continueNG_five.Clear();
            if (!m_bisEmg && !MachineDataDefine.machineState.b_UseTestRun)
            {
                MachineDataDefine.IsCycleStop = true;
            }
            if (!NextStop)
            {
                m_bCycleStop = false;
                myErrorStack.Clear();
                //-----------------
                m_bAutoing = false;
                m_bPausing = false;
                MachineDataDefine.IsReset = false;
                MachineDataDefine.IsCycleStop = false;
                MachineDataDefine.IsAutoing = false;
                //-----------------
                m_SgTower.Stop();
                m_SgTower.SetLightStatus(false, true, false, false);//停止-黄灯亮
                m_SgTower.TowerLight(clsMSignalTower.enTowerMode.enStop);
                //----------

                base.Stop();
            }
        }
        public Error pError = null;

        public void IsDoorStop()
        {
            m_bCycleStop = false;
            myErrorStack.Clear();
            //-----------------
            m_bAutoing = false;
            m_bPausing = false;
            MachineDataDefine.IsReset = false;
            MachineDataDefine.IsCycleStop = false;
            MachineDataDefine.IsAutoing = false;
            //-----------------
            // m_SgTower.Stop();
            //m_SgTower.SetLightStatus(true, false, false, true);//停止-红灯亮
            //m_SgTower.TowerLight(clsMSignalTower.enTowerMode.enStop);
            //----------
            base.Stop();
        }
        public override bool LoadMachineData(string strMachinePath)
        {
            return base.LoadMachineData(strMachinePath);
        }
        public override bool LoadWorkData(string strWorkPath)
        {
            m_strWorkPath = strWorkPath;

            int iCheckDoor = 0, iTestMode = 0, m_iCheckTest = 0;
            //-----------------
            GetXmlData(strWorkPath, "WorkData/Machine_Parm", "RunSpeed", ref m_dbAutoingSpeed);
            ////-------
            //GetXmlData(strWorkPath, "WorkData/Machine_Parm", "TestMode", ref iTestMode);
            //m_bTestMode = (iTestMode == 1) ? true : false;
            // MachineDataDefine.m_bTestMode = (iTestMode == 1) ? true : false;
            //---------
            GetXmlData(strWorkPath, "WorkData/Machine_Parm", "CheckDoor", ref iCheckDoor);
            // m_bCheckDoorSR = (iCheckDoor == 1) ? true : false;
            //---------
            //默认是正常工作模式，不再加载测试模式
            //  GetXmlData(strWorkPath, "WorkData/Machine_Parm", "CheckTestMode", ref m_iCheckTest);
            //  m_checkTestMode = (m_iCheckTest == 1) ? true : false;
            return base.LoadWorkData(strWorkPath);
        }
        public bool SaveWorkData(string strWorkPath)
        {
            //-----------------
            SaveXmlData(strWorkPath, "WorkData/Machine_Parm", "RunSpeed", m_dbAutoingSpeed.ToString());
            ////-------
            //   int iTestMode = (m_bTestMode) ? 1 : 0;
            int iTestMode = (m_bTestMode) ? 0 : 0;
            SaveXmlData(strWorkPath, "WorkData/Machine_Parm", "TestMode", iTestMode.ToString());
            //---------
            int iCheckDoor = (m_bCheckDoorSR) ? 1 : 0;
            SaveXmlData(strWorkPath, "WorkData/Machine_Parm", "CheckDoor", iCheckDoor.ToString());
            return true;
        }
        public override void Cycle(ref double dbTime)
        {
            if (MachineDataDefine.IsMainFormLoading != true)
            {
                return;
            }
            if (m_Card0.CardisOpen() != true)
            {
                return;
            }
            if (m_EmgIO == null)
            {
                m_EmgIO = HardWareControl.getInputIO(EnumParam_InputIO.急停按钮);
                //m_Start= HardWareControl.getInputIO(EnumParam_InputIO.开始按钮);
                //m_Stop = HardWareControl.getInputIO(EnumParam_InputIO.停止按钮);
                m_Reset = HardWareControl.getInputIO(EnumParam_InputIO.复位按钮);
            }
            #region 急停
            if (!m_bisEmg)
            {
                if (!m_EmgIO.GetValue() && pfrmMain.GetShowViewOK())
                {
                    Thread.Sleep(100);
                    if (!m_EmgIO.GetValue() && pfrmMain.GetShowViewOK())
                    {
                        m_bisEmg = true;
                        m_bHomeCompleted = false;  //設備回Home完成  = false;             
                                                   //----------------------
                        Stop();
                        PostMessage(pfrmMain.Handle, frm_Main.WM_ShowEMGDlg, 0, 0);
                        StopAuto();
                        LogAuto.Notify("急停被按下", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                    }
                }
            }
            #endregion

            #region Door檢知
            if (pfrmMain.GetShowViewOK())
            {
                if (CheckDoors.openDoor())
                {
                    IsDoorStop();
                    PostMessage(pfrmMain.Handle, frm_Main.WM_DoorisNotClosed, 0, 0);
                    LogAuto.Notify("安全门报警,设备暂停", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                }
            }
            #endregion
            if (MachineDataDefine.machineState.b_UseGratingCheck && Mainflow.isWorking)
            {
                if (anquan)
                {
                    if (HardWareControl.getInputIO(EnumParam_InputIO.安全光栅).GetValue())
                    {
                        showHinttEvent("");
                        anquan = false;
                    }
                    else
                    {
                        if ((DateTime.Now - anquanTime).TotalSeconds > 0.5)
                        {
                            LogAuto.Notify("长时间触碰安全光栅，设备暂停", (int)MachineStation.主监控, MotionLogLevel.Info);
                            showHinttEvent("长时间触碰安全光栅，设备暂停");
                            StopAuto();
                            anquan = false;
                            Mainflow.isWorking = false;
                        }
                    }
                }
                else
                {
                    if (!HardWareControl.getInputIO(EnumParam_InputIO.安全光栅).GetValue())
                    {
                        LogAuto.Notify("安全光栅触发！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        showHinttEvent("请勿触碰安全光栅");
                        anquan = true;
                        anquanTime = DateTime.Now;
                    }
                }

            }
            if (MachineDataDefine.machineState.b_UseHive && !HIVE.HIVEInstance.HIVE_Reveice_Status && !HIVE.HIVEInstance.HIVE_Error)
            {
                HIVE.HIVEInstance.HIVE_Error = true;
                PostMessage(pfrmMain.Handle, frm_Main.WM_HIVEReveiceFail, 0, 0);
                HIVE.HIVEInstance.SaveDateHIVE("HIVE回复失败,设备停止!", HIVE.HiveLogType.Other);
            }
            #region 启动按钮
            if (m_Start?.GetValue() == true && !MachineDataDefine.IsAutoing && GetHomeCompleted() && !MachineDataDefine.machineState.Isdia_ShowModelShow)//考虑在自动排胶中的情况
            {
                LogAuto.Notify("启动按钮按下", (int)MachineStation.主监控, MotionLogLevel.Info);
                if (MachineDataDefine.IsFormOpen)
                {
                    MessageBox.Show(JudgeLanguage.JudgeLag("请检查设备安全门是否已全部关闭！"), JudgeLanguage.JudgeLag("提示"), MessageBoxButtons.OK,
                       MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                    LogAuto.Notify("设备安全门存在未关闭状况！", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                    return;
                }
                if (MachineDataDefine.machineState.b_UseHive)
                {
                    if (new Ping().Send(MESDataDefine.MESLXData.StrHIVEIP, 3000).Status.ToString().Trim() != "Success")
                    {
                        HIVE.HIVEInstance.HIVE_Reveice_Status = false;
                        return;
                    }
                }
                if (HIVEDataDefine.Hive_machineState.IsSendErrorDown && MachineDataDefine.machineState.b_UseHive)
                {
                    MessageBox.Show(JudgeLanguage.JudgeLag("发送报警状态时不允许启动！"), JudgeLanguage.JudgeLag("提示"), MessageBoxButtons.OK,
                       MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                    return;
                }
                //Task.Run(() =>
                //{
                StateChoose(); // 
                               //});
                               ////Thread.Sleep(1000);
                               //StartAuto();
            }
            #endregion
            #region 停止按钮
            if (m_Stop?.GetValue() == true)
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
                LogAuto.Notify("停止按钮按下", (int)MachineStation.主监控, MotionLogLevel.Hint);
                MachineDataDefine.Button_Stop = true;
                #region 将状态切换到DT                
                //if (GetisAutoing())
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
                frm_Main.formError.ErrorUnit1.EndErrorMessage("90099");
                #endregion
                StopAuto();
            }
            #endregion
            #region 复位按钮
            if (m_Reset?.GetValue() == true && !MachineDataDefine.IsAutoing)
            {
               
                LogAuto.Notify("复位按钮按下", (int)MachineStation.归零, MotionLogLevel.Hint);
                string strShowMessage = "归原";
                frm_Main.formError.ErrorUnit1.AddActionMessage(strShowMessage);
                DoHome();
            }
            #endregion
            if (m_bHomeCompleted)
            {
                HardWareControl.getOutputIO(EnumParam_OutputIO.复位按钮灯).SetIO(false);
            }
            else
            {
                HardWareControl.getOutputIO(EnumParam_OutputIO.复位按钮灯).SetIO(true);
            }
            base.Cycle(ref dbTime);
        }
        public override void HomeCycle(ref double dbTime)
        {
            m_HomeStep = (enHomeStep)m_nHomeStep;
            try
            {
                switch (m_HomeStep)
                {
                    case enHomeStep.StartHome:
                        {
                            m_bHomeCompleted = false;
                            m_nHomeStep = (int)enHomeStep.Work_HomeCompleted;
                            m_SgTower.TowerLight(clsMSignalTower.enTowerMode.enDoHome);
                            WorkProcessLoad.instance.workProcess_Mainflow.DoHomeStep(0);
                        }
                        break;         
                            //&& WorkProcessLoad.instance.workProcess_RunnerIn.GetHomeCompleted()
                            //&& WorkProcessLoad.instance.workProcess_AxisTakeIn.GetHomeCompleted()
                            //&& WorkProcessLoad.instance.workProcess_AxisTakeOut.GetHomeCompleted()
                    case enHomeStep.Work_HomeCompleted:
                        if (WorkProcessLoad.instance.workProcess_Mainflow.GetHomeCompleted()
                        )
                        {
                            m_nHomeStep = (int)enHomeStep.HomeCompleted;
                        }
                        break;
                    case enHomeStep.HomeCompleted:
                        {
                            HardWareControl.getOutputIO(EnumParam_OutputIO.复位按钮灯).SetIO(false);
                            m_bHomeCompleted = true;
                            LogAuto.Notify("三色灯为待命状态---黄灯亮", (int)MachineStation.归零, MotionLogLevel.Info);
                            m_SgTower.Stop();
                            m_SgTower.SetLightStatus(false, true, false, false);//归原完成-黄灯亮
                            PostMessage(pfrmMain.Handle, frm_Main.WM_HomeCompleted, 0, 0);
                            m_Status = 狀態.待命;
                            MachineDataDefine.MachineLightEumn = MachineLightEumn.程序回原成功待启动;
                            LogAuto.Notify("归零---", (int)MachineStation.归零, MotionLogLevel.Hint);
                        }
                        break;
                }
            }
            catch
            {
                m_bHomeCompleted = false;
                LogAuto.Notify("归原失败:" + m_HomeStep, (int)MachineStation.归零, MotionLogLevel.Alarm);
            }
            base.HomeCycle(ref dbTime);
        }
        public override void StepCycle(ref double dbTime)
        {
            m_Step = (enStep)m_nStep;
            switch (m_Step)
            {
                //---------------------------
                #region   Auto
                case enStep.StartAuto:
                    {
                        BaseDataDefine.machineSpeed = MachineDataDefine.machineState.machineSpeed;
                        MachineDataDefine.machineState.UpLoadError = 0;
                        Mainflow.isFirstProduct = true;
                        //新增内容
                        //HardWareControl.movePoint(EnumParam_Point.后龙门XY取螺丝待命位);
                        //HardWareControl.getValve(EnumParam_Valve.进料阻挡气缸).Open();
                        //HardWareControl.getValve(EnumParam_Valve.进料顶升气缸).Open();
                        //HardWareControl.getValve(EnumParam_Valve.作料位阻挡气缸).Open();
                        //HardWareControl.getValve(EnumParam_Valve.作料位顶升气缸).Close();
                        //HardWareControl.getValve(EnumParam_Valve.出料位阻挡气缸).Open();
                        //HardWareControl.getValve(EnumParam_Valve.出料位顶升气缸).Close();
                        //HardWareControl.getValve(EnumParam_Valve.左侧Driver进料滑台气缸).Close();
                        //HardWareControl.getValve(EnumParam_Valve.标定吸真空电磁阀ON).Close();
                        //HardWareControl.getValve(EnumParam_Valve.取螺丝吸真空电磁阀ON).Close();
                        HardWareControl.movePoint(EnumParam_Point.拍照位1);
                        m_nStep = (int)enStep.AllStAuto;
                        //}
                    }
                    break;
             
                case enStep.AllStAuto:
                    //设备开始工作
                    //if (m_DispenserAuto.isIDLE())
                    //{
                    //    if (m_RunnerAuto != null)
                    //        m_RunnerAuto.RunnerStartAuto();
                    //    //-------------
                    //    m_DispenserAuto.DispenserStationAuto(m_dbAutoingSpeed);
                    //    m_nStep = (int)enStep.Autoing;
                    //}
                    //else
                    //{
                    //    m_DispenserAuto.Stop();
                    //}
                    //if (WorkProcessLoad.instance.workProcess_DriverEnter.isIDLE())
                    //{
                    //    WorkProcessLoad.instance.workProcess_DriverEnter.DoStep(0);
                    //}
                    //if (WorkProcessLoad.instance.workProcess_RunnerIn.isIDLE())
                    //{
                    //    WorkProcessLoad.instance.workProcess_RunnerIn.DoStep(0);
                    //}
                    //if (WorkProcessLoad.instance.workProcess_AxisTakeIn.isIDLE())
                    //{
                    //    WorkProcessLoad.instance.workProcess_AxisTakeIn.DoStep(0);
                    //}
                    //if (WorkProcessLoad.instance.workProcess_AxisTakeOut.isIDLE())
                    //{
                    //    WorkProcessLoad.instance.workProcess_AxisTakeOut.DoStep(0);
                    //}
                    //if (WorkProcessLoad.instance.testProcess.isIDLE())
                    //{
                    //    WorkProcessLoad.instance.testProcess.DoStep(0);
                    //}
                    if (WorkProcessLoad.instance.workProcess_Mainflow.isIDLE())
                    {
                        WorkProcessLoad.instance.workProcess_Mainflow.DoStep(0);
                    }
                    m_nStep = (int)enStep.Autoing;
                    break;
                case enStep.Autoing:               
                    if (MachineDataDefine.IsAutoing == false)
                    {
                        //循环停止
                        //frm_Main.formData.ChartTime1.StartRun(); 2022-04-27
                        //設備停止
                        m_nStep = (int)enStep.AutoStop;
                    }
                    break;
                //#endregion
                case enStep.AutoStop:
                    //新增内容
                    //frm_Main.formData.ChartTime1.StartWait();
                    // MachineDataDefine.MachineControlS.IsAutoing = false;
                    MachineDataDefine.IsCycleStop = false;
                    //MDataDefine.m_bPastingHeadReady = true;
                    //  MachineDataDefine.MachineControlS.m_StationStop = false;
                    Stop();
                    m_bAutoing = false;
                    m_Status = 狀態.待命;
                    break;
                #endregion
                //--------------------------------
                case enStep.Pausing:
                    {
                        //m_pickAutoRun[0].m_Status = 狀態.暫停;
                        //m_pickAutoRun[1].m_Status = 狀態.暫停;
                        //m_pickAutoRun[0].m_pPickSt.m_Status = 狀態.暫停;
                        //m_pickAutoRun[1].m_pPickSt.m_Status = 狀態.暫停;
                        //m_pDisUvAuto.m_Status = 狀態.暫停;
                    }
                    break;
            }
            base.StepCycle(ref dbTime);
        }
        bool light = false;
        int indexLight = 0;
        int indexLight1 = 0;
        int indexLight2 = 0;
        //线程  刷新面板三个按钮和胶水报警等内容
        private void refreshLightAndCheckController()
        {
            while (true)
            {
                if (MachineDataDefine.MachineLightEumn == MachineLightEumn.程序回原成功待启动)
                {//回原结束，待启动
                    //m_LightReset.SetIO(false);
                    //m_LightStop.SetIO(false);
                    //indexLight++;
                    //if (indexLight > 6)
                    //{
                    //    indexLight = 0;
                    //    light = !light;
                    //    if (light)
                    //        m_LightStart.SetIO(true);
                    //    else
                    //        m_LightStart.SetIO(false);
                    //}
                }
                if (MachineDataDefine.MachineLightEumn == MachineLightEumn.程序待回原)
                {//程序开始时
                    //m_LightStart.SetIO(false);
                    //m_LightStop.SetIO(false);
                    //indexLight1++;
                    //if (indexLight1 > 6)
                    //{
                    //    indexLight1 = 0;
                    //    light = !light;
                    //    if (light)
                    //        m_LightReset.SetIO(true);
                    //    else
                    //        m_LightReset.SetIO(false);
                    //}
                }

                if (MachineDataDefine.MachineLightEumn == MachineLightEumn.程序回原中)
                {//程序开始时
                    //m_LightStart.SetIO(false);
                    //m_LightStop.SetIO(false);
                    //m_LightReset.SetIO(true);
                }

                if (MachineDataDefine.MachineLightEumn == MachineLightEumn.程序启动中)
                {//程序开始时
                    //m_LightStart.SetIO(true);
                    //m_LightStop.SetIO(false);
                    //m_LightReset.SetIO(false);
                }
                if (MachineDataDefine.MachineLightEumn == MachineLightEumn.程序停止中)
                {//程序开始时
                    //m_LightStart.SetIO(false);
                    //m_LightStop.SetIO(true);
                    //m_LightReset.SetIO(false);
                }

                //700ms刷一次
                if (GetHomeCompleted())
                {
                    indexLight2++;
                    if (indexLight2 > 7)
                    {
                        indexLight2 = 0;
                    }
                }
                Thread.Sleep(100);
            }
        }
        //******************************
        private void SetIOs()
        {
            #region Input I/O
            m_EmgIO = HardWareControl.getInputIO(EnumParam_InputIO.急停按钮);
            //m_Start = HardWareControl.getInputIO(EnumParam_InputIO.启动按钮);
            //m_Stop = HardWareControl.getInputIO(EnumParam_InputIO.停止按钮);
            m_Reset = HardWareControl.getInputIO(EnumParam_InputIO.复位按钮);
            //  m_inSafety = StaticParam.InputDictionary1["安全光栅"];
            m_LightTowerR = HardWareControl.getOutputIO(EnumParam_OutputIO.三色灯_红灯);
            m_LightTowerY = HardWareControl.getOutputIO(EnumParam_OutputIO.三色灯_黄灯);
            m_LightTowerG = HardWareControl.getOutputIO(EnumParam_OutputIO.三色灯_绿灯);
            m_Buzzer = HardWareControl.getOutputIO(EnumParam_OutputIO.三色灯_蜂鸣器);
            //--------------------
            //m_LightStop = HardWareControl.getOutputIO(EnumParam_OutputIO.停止灯);
            //m_LightReset = HardWareControl.getOutputIO(EnumParam_OutputIO.复位灯);
            //m_LightStart = HardWareControl.getOutputIO(EnumParam_OutputIO.启动灯);
            #endregion
            //*******************************************************
        }
        public void StateChoose()
        {
            if (!MachineDataDefine.machineState.Isdia_ShowModelShow)
            {
                if (MachineDataDefine.b_UseLAD)
                {
                    if (MachineDataDefine.LADModel == 2)
                    {
                        MachineDataDefine.machineState.b_UseMes = false;
                        MachineDataDefine.machineState.b_UsePDCA = false;
                        MachineDataDefine.machineState.b_UseHive = true;
                    }
                    else
                    {
                        MachineDataDefine.machineState.b_UseMes = true;
                        MachineDataDefine.machineState.b_UsePDCA = true;
                        MachineDataDefine.machineState.b_UseHive = true;
                    }
                }
                MachineDataDefine.machineState.Isdia_ShowModelShow = true;
                //m_SgTower.TowerLight(clsMSignalTower.enTowerMode.enAutoRuning);
                PostMessage(pfrmMain.Handle, frm_Main.WM_AutoStart, 0, 0);
            }
        }
        public void StartAuto(double dbSpeed = 100)
        {
            HIVE.HIVEInstance.CanReadGluePathCount = false;
            //当读胶路速度时不获取行数
            HIVE.HIVEInstance.CanReadGluePathCount = true;
            HIVE.HIVEInstance.last_one_time = DateTime.Now;
            //如果是正常作料，判断CCD与PDCA是否连上，
            MachineDataDefine.MachineLightEumn = MachineLightEumn.程序启动中;
            LogAuto.Notify("自动运行----开启", (int)MachineStation.主监控, MotionLogLevel.Hint);
            //Stop();
            frm_Main.formData.ChartTime1.MSignalTowerstatus = ChartTime.MachineStatus.idle;
            //LogAuto.Notify("自动流程开启", (int)MachineStation.主);
            MachineDataDefine.IsCycleStop = false;

            //MESDataDefine.StrCarryBarCodeS[1] = "";
            //------------------
            MachineDataDefine.IsAutoing = true;
            MachineDataDefine.IsAlarmShow = false;
            m_bAutoing = true;
            MachineDataDefine.IsReset = false;
            m_Mode = enMode.自動;
            m_dbSpeed = dbSpeed;
            int doStep = (int)enStep.StartAuto;
            bool bRet = DoStep(doStep);
            if (bRet)
            {
                frm_Main.m_OnError = false;
            }
            MachineDataDefine.MachineLightEumn = MachineLightEumn.程序启动中;
            b_iSafeDoorShow = false;
            //return bRet;
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void StopAuto(bool stop = true)
        {
            if (stop)
            {
                PostMessage(pfrmMain.Handle, frm_Main.WM_MalStop, 0, 0);
            }

            MachineDataDefine.MachineLightEumn = MachineLightEumn.程序停止中;
            //m_LightStart.SetIO(false);
            //m_LightStop.SetIO(true);
            //m_LightReset.SetIO(false);


            MachineDataDefine.IsAutoing = false;
            Stop();
            LogAuto.Notify("设备停止", (int)MachineStation.主监控, MotionLogLevel.Hint);
            NeedRef = true;
        }

        /// <summary>
        /// 归零
        /// </summary>
        /// <returns></returns>
        public bool DoHome()
        {
            DialogResult result = MessageBox.Show(JudgeLanguage.JudgeLag("设备即将归零，检查是否清空载具"), JudgeLanguage.JudgeLag("提示"), MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            if (result != DialogResult.OK)
            {
                return false;
            }
            MachineDataDefine.IsReset = false;
            MachineDataDefine.MachineLightEumn = MachineLightEumn.程序回原中;
            m_bHomeCompleted = false;
            //Stop();
            //WorkProcessLoad.instance.workProcess_Mainflow.Stop();
            int doStep = (int)enHomeStep.StartHome;
            bool bRet = DoHomeStep(doStep);
            if (bRet)
            {
                frm_Main.m_OnError = false;
            }
            return bRet;
        }
        public void EnablePause()
        {
            m_bPausing = true;
        }
        public void DisablePause()
        {
            m_bPausing = false;
        }
        /// <summary>
        /// 将界面切换到Alarm界面
        /// </summary>
        private void ChangeToAlarm()
        {
            PostMessage(pfrmMain.Handle, frm_Main.WM_ChangeToAlarming, 0, 0);
        }

    }
}
