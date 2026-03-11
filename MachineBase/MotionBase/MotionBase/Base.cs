using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using System.Xml;
using MotionBase;
using Inovance.InoMotionCotrollerShop.InoServiceContract.EtherCATConfigApi;
using OpenOffice_Connect;
using System.IO;
using System.Reflection;
using Cowain_AutoMachine.Flow.IO_Cylinder;
using System.Windows.Forms;

namespace MotionBase
{
    public class Base
    {
        #region Base建構式&解構式
        public Base(Type homeEnum1, Type stepEnum1,string instanceName1)
        {
            //----------------
            mTimer = new System.Timers.Timer(1000);
            mTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);
            timerDelay = new System.Timers.Timer(1000);
            timerDelay.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent1);
            //----------------
            m_NowAddress = this;
            m_Parent = null;
            m_nID = m_SolutionID = 0;
            m_nErrCodeBase = 0;
            m_nRetCode = (int)ErrorDefine.enErrorCode.程式未Initial;
            m_bisEStop = m_bisInterLock = false;
            m_Status = 狀態.初始化;
            homeEnum = homeEnum1;
            stepEnum = stepEnum1;
            instanceName = instanceName1;
            ShowStepManager.instance.addShowStep(new ShowStep(homeEnum, stepEnum, instanceName));
        }
        public Base(Type homeEnum1, Type stepEnum1, string instanceName1,Base parent, bool bAddBaseAddress = false)
        {
            //----------------
            mTimer = new System.Timers.Timer(1000);
            mTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);

            timerDelay = new System.Timers.Timer(1000);
            timerDelay.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent1);
            //----------------
            m_NowAddress = this;
            m_Parent = parent;
            m_nID = m_SolutionID = 0;
            m_nErrCodeBase = 0;
            m_bisEStop = m_bisInterLock = false;
            //-----------------
            if (bAddBaseAddress)
                AddBase(ref m_NowAddress);
            //-----------------
            m_Status = 狀態.初始化;
            homeEnum = homeEnum1;
            stepEnum = stepEnum1;
            instanceName = instanceName1;
            ShowStepManager.instance.addShowStep(new ShowStep(homeEnum, stepEnum, instanceName));
        }
        public Base(Type homeEnum1, Type stepEnum1, string instanceName1, Base parent, int ErrCodeBase = 0)
        {
            //----------------
            mTimer = new System.Timers.Timer(1000);
            mTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);
            //----------------
            m_NowAddress = this;
            m_Parent = parent;
            m_nID = m_SolutionID = 0;
            m_nErrCodeBase = ErrCodeBase;
            m_bisEStop = m_bisInterLock = false;
            m_Status = 狀態.初始化;
            homeEnum = homeEnum1;
            stepEnum = stepEnum1;
            instanceName = instanceName1;
            ShowStepManager.instance.addShowStep(new ShowStep(homeEnum, stepEnum, instanceName));
        }
        public Base(Type homeEnum1, Type stepEnum1, string instanceName1, Base parent, int nStation, String TableName, String strCName, bool bAddBaseAddress = false)
        {
            //----------------
            mTimer = new System.Timers.Timer(1000);
            mTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);
            //----------------
            m_NowAddress = this;
            m_Parent = parent;
            m_nID = m_SolutionID = 0;
            m_nStation = nStation;
            m_nErrCodeBase = 0;
            m_TableName = TableName;
            m_strCName = strCName;
            m_bisEStop = m_bisInterLock = false;
            //-----------------
            if (bAddBaseAddress)
                AddBase(ref m_NowAddress);
            //-----------------
            m_Status = 狀態.初始化;
            homeEnum = homeEnum1;
            stepEnum = stepEnum1;
            instanceName = instanceName1;
            ShowStepManager.instance.addShowStep(new ShowStep(homeEnum, stepEnum, instanceName));
        }

        public Base(Type homeEnum1, Type stepEnum1, string instanceName1, Base parent, int nStation, int ErrCodeBase = 0)
        {
            //----------------
            mTimer = new System.Timers.Timer(1000);
            mTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);
            //----------------
            m_NowAddress = this;
            m_Parent = parent;
            m_nID = m_SolutionID = 0;
            m_nStation = nStation;
            m_nErrCodeBase = ErrCodeBase;
            m_TableName = "";
            m_strCName = "";
            m_bisEStop = m_bisInterLock = false;
            m_Status = 狀態.初始化;
            homeEnum = homeEnum1;
            stepEnum = stepEnum1;
            instanceName = instanceName1;
            ShowStepManager.instance.addShowStep(new ShowStep(homeEnum, stepEnum, instanceName));
        }
        public Base(Type homeEnum1, Type stepEnum1, string instanceName1, Base parent, int nStation, String TableName, String strCName, int ErrCodeBase = 0)
        {
            //----------------
            mTimer = new System.Timers.Timer(1000);
            mTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);
            //----------------
            m_NowAddress = this;
            m_Parent = parent;
            m_nID = m_SolutionID = 0;
            m_nStation = nStation;
            m_nErrCodeBase = ErrCodeBase;
            m_TableName = TableName;
            m_strCName = strCName;
            m_bisEStop = m_bisInterLock = false;
            m_Status = 狀態.初始化;
            homeEnum = homeEnum1;
            stepEnum = stepEnum1;
            instanceName = instanceName1;
            ShowStepManager.instance.addShowStep(new ShowStep(homeEnum, stepEnum, instanceName));
        }
        ~Base()
        {
            ReleaseChilds();
        }
        #endregion

        //↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        private static Thread CycleThread = null;
        protected Sys_DataBase oDB = null;
        protected static AddMDBMachineData oDB_load = null;

        public Dictionary<int, Base> ChildList = new Dictionary<int, Base>();
        private double m_dbScanTime;  // Cycle Time時間
        public Base m_NowAddress;

       public DrvECatCard m_Card0;
        int iStep = 0;  //DoThread內使用之Step ID

        private bool m_isStop = false;
        private bool m_isRelease = false, m_bisInitCompleted = false;
        protected Base m_Parent;
        protected String m_TableName, m_strCName, m_strEName;
        protected int m_nID, m_SolutionID;
        protected int m_nStation, m_nCheckCount = 0;
        protected int m_nErrCodeBase, m_nRetCode;
        public string instanceName = "";
        private int m_nStep1=-1, m_nHomeStep1=-1;
        protected int m_nStep
        {
            get
            {
                return m_nStep1;
            }
            set
            {
                if (m_nStep1 != value)
                {
                    ShowStepManager.instance.addStepMSG(instanceName, StepType.作业, value);
                }
                m_nStep1 = value;
            }
        }
        protected int m_nHomeStep
        {
            get
            {
                return m_nHomeStep1;
            }
            set
            {
                if (m_nHomeStep1 != value)
                {
                    ShowStepManager.instance.addStepMSG(instanceName, StepType.回原, value);
                }
                m_nHomeStep1 = value;
            }
        }
        protected bool m_bisEStop, m_bisInterLock;
        protected String m_strWorkPath = "", m_strMachinePath = "", m_strMDBMachinePath = "";

        System.Timers.Timer mTimer;
        public System.Timers.Timer timerDelay;
        private void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e) { mTimer.Enabled = false; }
        private void OnTimedEvent1(object source, System.Timers.ElapsedEventArgs e) { timerDelay.Enabled = false; }
        //------------------------
        private static Stack<Error> ErrorStack = new Stack<Error>();
        private static Stack<String> ErrorMessageStack = new Stack<String>();
        //------------------------
        protected static int m_nMotoCount = 0, m_nValveCount = 0, m_nTimerCount = 0, m_nIOCount = 0;
        public static Dictionary<string, DrvMotor> MotorList = new Dictionary<string, DrvMotor>();
        protected static Dictionary<string, DrvValve> ValveList = new Dictionary<string, DrvValve>();
        protected static Dictionary<int, Base> TimerList = new Dictionary<int, Base>();
        protected static Dictionary<int, DrvIO> IOList = new Dictionary<int, DrvIO>();
        protected static Dictionary<string, DrvIO> InputsIOList = new Dictionary<string, DrvIO>();
        protected static Dictionary<string, DrvIO> OutputsIOList = new Dictionary<string, DrvIO>();
        private static object obj = new object();
        //----------------------
        private static Status baseStatus1 = Status.正常;

        Type homeEnum;
        Type stepEnum;


        public static Status baseStatus
        {
            get { return baseStatus1; }
        }
        public static void stop()
        {
            baseStatus1 = Status.停止;
        }
        public static void pause()
        {
            baseStatus1 = Status.暂停;
        }
        public static void start()
        {
            baseStatus1 = Status.正常;
        }
        //---------------------
        //↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑
        public enum Status
        {
            正常,
            暂停,
            停止,
        }
        public enum 狀態  //狀態
        {
            初始化,
            待命,
            準備動作中,
            動作中,
            回HOME中,
            錯誤發生,
            ALARM,
            急停,
            暫停,
        }
        public enum enMode
        {
            自動,
            手動,
            維護,
            未知,
        }
        public enum enSystemDataBaseType
        {
            enMDB,
            enTXT,
        }
        public enum HomeStep_Base
        {
            Start = 0,
            Completed
        }
        public enum Step_Base
        {
            Start = 0,
            Completed
        }
        public enSystemDataBaseType m_SystemDataBaseType = enSystemDataBaseType.enMDB;

        public 狀態 m_Status;
        public enMode m_Mode;
        public bool m_bHomeCompleted = false;
        public bool m_bTestMode = false;
        public Sys_Define.enPasswordType m_LoginUser;
        public string StrCardName = "汇川";
        public string StrCardVersion = "None";
        private Int16[] pVersion = new Int16[7];


        public bool AddBase(ref Base BaseAddress)
        {
            try
            {
                ChildList.Add(m_nID, BaseAddress);
                m_nID++;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        void ReleaseChilds()
        {
            m_isRelease = true;
            ChildList.Clear();
            //MotorList.Clear();
            m_nMotoCount = m_nID = 0;
        }
        //*****************************************  
        public int GetStationNo() { return m_nStation; }
        internal string GetCName() { return m_strCName; }
        internal string GetTableName() { return m_TableName; }
        internal string GetEName() { return m_strEName; }
        //-----------------------------------------
        public bool GetHomeCompleted() { return m_bHomeCompleted; }
        public 狀態 GetStatus() { return m_Status; }
        public bool isIDLE()
        {
            bool ret = true;
            int iCount = ChildList.Count;
            if (m_Status == 狀態.待命 && !m_bisEStop && !m_bisInterLock)
            {
                for (int i = 0; i < iCount; i++)
                {
                    bool bCheck = ChildList[i].isIDLE();
                    if (!bCheck)
                    {
                        ret = false;
                        break;
                    }
                }
            }
            else
            {
                ret = false;
            }
            return ret;
        }
        public void SetNextStep(int iStep) { m_nStep = iStep; }
        public void SetNextHomeStep(int iHomeStep) { m_nHomeStep = iHomeStep; }
        //******************************************
        private bool AllMotorAddToChild()
        {
            if (MotorList.Count != 0)
            {
                for (int i = 0; i < MotorList.Count; i++)
                {
                    MotorList[MotorList.Keys.ToList()[i]].m_SystemDataBaseType = m_SystemDataBaseType;
                    // Base pBase = MotorList[MotorList.Keys.ToList()[i]].m_Parent;
                    //  pBase.AddBase(ref MotorList[MotorList.Keys.ToList()[i]].m_NowAddress);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool AllValveAddToChild()
        {
            if (ValveList.Count != 0)
            {
                for (int i = 0; i < ValveList.Count; i++)
                {
                    ValveList[ValveList.Keys.ToList()[i]].m_SystemDataBaseType = m_SystemDataBaseType;
                    //  Base pBase = ValveList[ValveList.Keys.ToList()[i]].m_Parent;
                    //   pBase.AddBase(ref ValveList[ValveList.Keys.ToList()[i]].m_NowAddress);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool AllTimerAddToChild()
        {
            if (TimerList.Count != 0)
            {
                for (int i = 0; i < TimerList.Count; i++)
                {
                    TimerList[i].m_SystemDataBaseType = m_SystemDataBaseType;
                    //  Base pBase = TimerList[i].m_Parent;
                    //   pBase.AddBase(ref TimerList[i].m_NowAddress);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SearchValveUseIO()
        {
            DrvValve pValve;
            DrvIO pIO;
            if (ValveList.Count != 0)
            {
                foreach (var item in ValveList)
                {
                    pValve = (DrvValve)(item.Value).m_NowAddress;
                    foreach (var item11 in InputsIOList)
                    {
                        pIO = (DrvIO)(item11.Value).m_NowAddress;
                        string strID = pIO.GetID();
                        if (strID == pValve.m_strOpenSR)
                        {
                            pValve.m_OpenSR = pIO;
                        }
                        else if (strID == pValve.m_strCloseSR)
                        {
                            pValve.m_CloseSR = pIO;
                        }
                    }
                    foreach (var item22 in OutputsIOList)
                    {
                        pIO = (DrvIO)(item22.Value).m_NowAddress;
                        string strID = pIO.GetID();
                        //----------------------
                        if (strID == pValve.m_strOpenIO)
                        {
                            pValve.m_OpenIO = pIO;
                        }
                        else if (strID == pValve.m_strCloseIO)
                        {
                            pValve.m_CloseIO = pIO;
                        }
                    }
                }
            }
        }
        //---------------------------------
        public static Dictionary<string, DrvMotor> GetMotorList() { return MotorList; }
        public static Dictionary<string, DrvValve> GetValveList() { return ValveList; }
        public static Dictionary<int, Base> GetTimerList() { return TimerList; }
        public static Dictionary<int, DrvIO> GetIOList() { return IOList; }
        public static Dictionary<string, DrvIO> GetInputsIOList() { return InputsIOList; }
        public static Dictionary<string, DrvIO> GetOutputsIOList() { return OutputsIOList; }


        private void openMDB(string strMachinePath)
        {
            oDB_load = new AddMDBMachineData(strMachinePath);
        }
        private void closeMDB(string strMachinePath)
        {
            //oRs = null;
            oDB_load.DisConnect();
        }

        //*******************************************
        virtual public void Cycle(ref double dbTime)
        {
            try
            {
                if (Base.baseStatus == Status.暂停)
                {
                    return;
                }
                if (Base.baseStatus == Status.停止)
                {
                    Stop();
                    m_Status = 狀態.待命;
                    Base.start();
                }
                //int iCount = ChildList.Count;
                if (m_Status == 狀態.動作中) //&& m_Mode != enMode.未知
                    StepCycle(ref dbTime);
                else if (m_Status == 狀態.回HOME中) //&& m_Mode != enMode.未知
                    HomeCycle(ref dbTime);

                //---------------------------
                if (m_isStop)
                    return;

                //for (int i = 0; i < iCount; i++)
                //    ChildList[i].Cycle(ref dbTime);
            }
            catch (Exception e11)
            {
                string err = e11.Message + "," + e11.StackTrace;
                Sys_Define.RecordMessageLog("API_ErrorRecord", "线程抛异常1," + err);
            }
        }

        virtual public bool DoStep(int iStep)
        {
            if ((m_Status == 狀態.待命 || m_Status == 狀態.準備動作中) && !m_bisEStop && !m_bisInterLock)  //&& m_Mode!= enMode.未知
            {
                m_Status = 狀態.動作中;
                m_nStep = iStep;
                return true;
            }
            else
            {
                return false;
            }
        }
        virtual public void StepCycle(ref double dbTime)
        {
        }
        virtual public bool DoHomeStep(int iHomeStep)
        {
            if ((m_Status == 狀態.待命 || m_Status == 狀態.準備動作中) && !m_bisEStop && !m_bisInterLock)
            {
                m_Status = 狀態.回HOME中;
                m_nHomeStep = iHomeStep;
                return true;
            }
            else
            {
                return false;
            }
        }
        virtual public void HomeCycle(ref double dbTime)
        {
        }
        //*****************************************
        virtual public void ErrorHappen(ref Error pError)
        {
            if (m_Parent == null)
                ErrorStack.Push(pError);
            else

                m_Parent.ErrorHappen(ref pError);


        }
        virtual public void ErrorMessage(string strMessage)
        {
            ErrorMessageStack.Push(strMessage);
        }
        //*****************************************
        virtual public bool Init()
        {
            bool bRet = true;
            m_Status = 狀態.待命;
            //---------------------------
            for (int i = 0; i < ChildList.Count; i++)
            {
                bRet = ChildList[i].Init();
                if (!bRet)
                {
                    //---------------拋出Init之錯誤訊息
                    return false;
                }
            }
            return bRet;
        }

        virtual public void Stop()
        {
            m_isStop = true;

            //for (int i = 0; i < 3; i++)
            {
                Thread.Sleep(1);
                for (int j = 0; j < ChildList.Count; j++)
                {
                    ChildList[j].Stop();
                }
            }
            m_Status = 狀態.待命;
            m_isStop = false;
        }
        public void SetMachineDataPath(String strMachinePath) { m_strMachinePath = strMachinePath; }
        virtual public bool LoadMachineData(String strMachinePath)
        {
            m_strMachinePath = strMachinePath;
            int i = 0;
            bool bRet;
            for (i = 0; i < ChildList.Count; i++)
            {
                //   Thread.Sleep(50);
                bRet = ChildList[i].LoadMachineData(strMachinePath);
                if (!bRet)
                    return false;
            }
            return true;
        }
        virtual public bool LoadIOMachineData(String strMachinePath)
        {
            m_strMachinePath = strMachinePath;

            AddMDBMachineData MDB = new AddMDBMachineData(strMachinePath);
            //----------加载IO
            var inputArray1 = (from port in clsIO_Ports.IOList where port.Key.Contains("X") select port.Value).Cast<DrvIO>().
                                 OrderBy(x => (x as DrvECatIO).GetType().GetField("m_strID", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(x) as IComparable).ToArray();
            var outputArray = (from port in clsIO_Ports.IOList where port.Key.Contains("Y") select port.Value).Cast<DrvIO>().
                                OrderBy(x => (x as DrvECatIO).GetType().GetField("m_strID", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(x) as IComparable).ToArray();


            for (int j = 0; j < inputArray1.Length; j++)
            {
                if (!InputsIOList.Keys.Contains(inputArray1[j].GetCName()))
                {
                    InputsIOList.Add(inputArray1[j].GetCName(), inputArray1[j]);
                }
            }
            for (int j = 0; j < outputArray.Length; j++)
            {
                if (!OutputsIOList.Keys.Contains(outputArray[j].GetCName()))
                {
                    OutputsIOList.Add(outputArray[j].GetCName(), outputArray[j]);
                }
            }
            int i = 0;
            bool bRet;
            for (i = 0; i < IOList.Count; i++)
            {
                // Thread.Sleep(50);
                IOList[IOList.Keys.ToList()[i]].m_SystemDataBaseType = m_SystemDataBaseType;
                bRet = IOList[IOList.Keys.ToList()[i]].LoadMachineData(strMachinePath);
                if (!bRet)
                    return false;
            }
            return true;
        }
        virtual public bool LoadMotorMachineData(String strMachinePath)
        {
            m_strMachinePath = strMachinePath;
            int i = 0;
            bool bRet;
            for (i = 0; i < MotorList.Count; i++)
            {
                // Thread.Sleep(50);
                MotorList[MotorList.Keys.ToList()[i]].m_SystemDataBaseType = m_SystemDataBaseType;
                bRet = MotorList[MotorList.Keys.ToList()[i]].LoadMachineData(strMachinePath);
                if (!bRet)
                    return false;
            }
            return true;
        }
        virtual public bool LoadWorkData(String strWorkPath)
        {
            m_strWorkPath = strWorkPath;
            bool bRet;
            for (int i = 0; i < ChildList.Count; i++)
            {
                bRet = ChildList[i].LoadWorkData(strWorkPath);
                if (!bRet)
                    return false;
            }
            return true;
        }
        //****************************************
        #region DataBaseType: MDB , GetData & SaveData  
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="strPath">路径</param>
        /// <param name="strTable">表格名称</param>
        /// <param name="strIDName">列表头</param>
        /// <param name="strItemName">指定行</param>
        /// <param name="strDataType">指定列</param>
        /// <param name="retValue">查到的值</param>
        /// <returns></returns>
        private bool GetDataBaseData(String strPath, String strTable, String strIDName, String strItemName, String strDataType, ref object retValue)
        {
            lock (obj)
            {
                bool ret = false;
                System.Data.Common.DbDataReader oRs = null;
                oDB = new Sys_DataBase(strPath);
                try
                {
                    string sSQL = "", strType = "";

                    if (IntPtr.Size == 4)
                        sSQL = "Select * From " + strTable + " Where " + strIDName + "=" + strItemName;
                    if (IntPtr.Size == 8)
                        sSQL = "Select * From " + strTable + " Where " + strIDName + "=" + "'" + strItemName + "' ";
                    //-------------
                    ret = oDB.Fun_RsSQL(sSQL, ref oRs);

                    if (ret == false) return false;
                    if (oRs.Read() == false) return false;

                    retValue = oRs[strDataType];
                    //**************
                    //oDB.Fun_CloseRS(ref oRs);
                    //oRs = null;
                    oDB.Fun_CloseDB();
                    oDB = null;
                    return true;
                }
                catch (Exception ex)
                {
                    oDB_load.Fun_CloseRS(ref oRs);
                    //oRs = null;
                    //oDB.Fun_CloseDB();
                    //oDB = null;
                    return false;
                }
                //return ret;
            }
        }
        public bool GetDataBaseData(String strPath, String strTable, String strIDName, String strItemName, String strDataType, ref bool retValue)
        {
            try
            {
                object retobj = new object();
                bool ret = GetDataBaseData(strPath, strTable, strIDName, strItemName, strDataType, ref retobj);
                retValue = Convert.ToBoolean(retobj);
                retobj = null;
                return ret;
            }
            catch
            {
                return false;
            }
        }
        public bool GetDataBaseData(String strPath, String strTable, String strIDName, String strItemName, String strDataType, ref int retValue)
        {
            try
            {
                object retobj = new object();
                bool ret = GetDataBaseData(strPath, strTable, strIDName, strItemName, strDataType, ref retobj);
                retValue = Convert.ToInt32(retobj);
                return ret;
            }
            catch
            {
                return false;
            }
        }
        public bool GetDataBaseData(String strPath, String strTable, String strIDName, String strItemName, String strDataType, ref double retValue)
        {
            try
            {
                object retobj = new object();
                bool ret = GetDataBaseData(strPath, strTable, strIDName, strItemName, strDataType, ref retobj);
                retValue = Convert.ToDouble(retobj);
                retobj = null;
                return ret;
            }
            catch
            {
                return false;
            }
        }
        public bool GetDataBaseData(String strPath, String strTable, String strIDName, String strItemName, String strDataType, ref string retValue)
        {
            try
            {
                object retobj = new object();
                bool ret = GetDataBaseData(strPath, strTable, strIDName, strItemName, strDataType, ref retobj);
                retValue = retobj.ToString();
                retobj = null;
                return ret;
            }
            catch
            {
                return false;
            }
        }
        public bool GetDataBaseData(String strPath, String strTable, String strIDName, String strItemName, String strDataType, ref ushort retValue)
        {
            try
            {
                object retobj = new object();
                bool ret = GetDataBaseData(strPath, strTable, strIDName, strItemName, strDataType, ref retobj);
                retValue = Convert.ToUInt16(retobj);
                retobj = null;
                return ret;
            }
            catch
            {
                return false;
            }
        }
        public bool GetDataBaseData(String strPath, String strTable, String strIDName, String strItemName, String strDataType, ref uint retValue)
        {
            try
            {
                object retobj = new object();
                bool ret = GetDataBaseData(strPath, strTable, strIDName, strItemName, strDataType, ref retobj);
                retValue = Convert.ToUInt32(retobj);
                retobj = null;
                return ret;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="strPath">路径</param>
        /// <param name="strTable">表格名</param>
        /// <param name="strIDName">查询列表头</param>
        /// <param name="strItemName">查询列中特定行</param>
        /// <param name="strDataType">行中某一项</param>
        /// <param name="strSetValue">修改值</param>
        /// <returns></returns>
        public bool SaveToDataBase(String strPath, String strTable, String strIDName, String strItemName, String strDataType, string strSetValue)
        {
            oDB = new Sys_DataBase(strPath);
            try
            {
                string sSQL;
                bool ret;

                sSQL = "Update " + strTable + " Set " + strDataType + " ='" + strSetValue + "' Where " + strIDName + "= '" + strItemName + "'";

                //sSQL = sSQL + "Path= '" + strPath.ToString() + "' ,";
                //sSQL = sSQL + " Where ID_NAME= 'WorkPath' ";
                ret = oDB.Fun_ExecSQL(sSQL);
                oDB.Fun_CloseDB();
                oDB = null;
                return ret;
            }
            catch (Exception ex)
            {
                oDB.Fun_CloseDB();
                oDB = null;
                return false;
            }

        }

        #endregion
        //------------------
        #region DataBaseType: XML , GetData & SaveData  
        /// <summary>
        /// 获取XLM数据
        /// </summary>
        /// <param name="strFilePath">路径</param>
        /// <param name="strXmlNodeName">根名</param>
        /// <param name="strXmlNodeItem">节点</param>
        /// <param name="retValue">获取的值</param>
        /// <returns></returns>
        private bool GetXmlData(String strFilePath, String strXmlNodeName, String strXmlNodeItem, ref object retValue)
        {
            bool ret = false;
            string strGetValue = "";
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(strFilePath);  //Load Xml檔案
                //------------------------------------
                int iCount = 0;
                XmlNode root = xmlDoc.SelectSingleNode(strXmlNodeName);
                XmlNodeList NodeLists = xmlDoc.SelectNodes(strXmlNodeName);
                //-------------
                int iCountMax = root.ChildNodes.Count;
                if (iCountMax != 0)  //iCount>0, 使用 SelectSigleNode取得資料 ; iCount=0 , SelectNodes取得資料 
                {
                    foreach (XmlElement elm in root.ChildNodes)
                    {
                        if (elm.Name == strXmlNodeItem)
                            retValue = elm.InnerText;
                    }
                }
                else
                {
                    foreach (XmlNode OneNode in NodeLists)
                    {
                        retValue = OneNode.Attributes[strXmlNodeItem].Value;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            return ret;
        }

        public bool GetXmlData(String strFilePath, String strXmlNodeName, String strXmlNodeItem, ref bool retValue)
        {
            try
            {
                object retobj = new object();
                bool ret = GetXmlData(strFilePath, strXmlNodeName, strXmlNodeItem, ref retobj);
                int iRetValue = Convert.ToInt16(retobj);
                retValue = Convert.ToBoolean(iRetValue);
                retobj = null;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool GetXmlData(String strFilePath, String strXmlNodeName, String strXmlNodeItem, ref int retValue)
        {
            try
            {
                object retobj = new object();
                bool ret = GetXmlData(strFilePath, strXmlNodeName, strXmlNodeItem, ref retobj);
                retValue = Convert.ToInt32(retobj);
                retobj = null;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool GetXmlData(String strFilePath, String strXmlNodeName, String strXmlNodeItem, ref double retValue)
        {
            try
            {
                object retobj = new object();
                bool ret = GetXmlData(strFilePath, strXmlNodeName, strXmlNodeItem, ref retobj);
                retValue = Convert.ToDouble(retobj);
                retobj = null;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool GetXmlData(String strFilePath, String strXmlNodeName, String strXmlNodeItem, ref string retValue)
        {
            try
            {
                object retobj = new object();
                bool ret = GetXmlData(strFilePath, strXmlNodeName, strXmlNodeItem, ref retobj);
                retValue = Convert.ToString(retobj);
                retobj = null;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool GetXmlData(String strFilePath, String strXmlNodeName, String strXmlNodeItem, ref ushort retValue)
        {
            try
            {
                object retobj = new object();
                bool ret = GetXmlData(strFilePath, strXmlNodeName, strXmlNodeItem, ref retobj);
                retValue = Convert.ToUInt16(retobj);
                retobj = null;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool GetXmlData(String strFilePath, String strXmlNodeName, String strXmlNodeItem, ref uint retValue)
        {
            try
            {
                object retobj = new object();
                bool ret = GetXmlData(strFilePath, strXmlNodeName, strXmlNodeItem, ref retobj);
                retValue = Convert.ToUInt32(retobj);
                retobj = null;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SaveXmlData(String strFilePath, String strXmlNodeName, String strXmlNodeItem, string strSetValue)
        {
            bool ret = false;
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(strFilePath);  //Load Xml檔案
                //------------------------------------
                int iCount = 0;
                XmlNode root = xmlDoc.SelectSingleNode(strXmlNodeName);
                XmlNodeList NodeLists = xmlDoc.SelectNodes(strXmlNodeName);
                //-------------
                int iCountMax = root.ChildNodes.Count;
                if (iCountMax != 0)  //iCount>0, 使用 SelectSigleNode取得資料 ; iCount=0 , SelectNodes取得資料 
                {
                    foreach (XmlElement elm in root.ChildNodes)
                    {
                        if (elm.Name == strXmlNodeItem)
                            elm.InnerText = strSetValue;
                    }
                }
                else
                {
                    foreach (XmlNode OneNode in NodeLists)
                    {
                        OneNode.Attributes[strXmlNodeItem].Value = strSetValue;
                    }
                }
                xmlDoc.Save(strFilePath);

                return true;
            }
            catch (Exception ex)
            {
                //oDB_load.Fun_CloseDB();
                //oDB_load = null;
                return false;
            }

        }

        #endregion
        //------------------

        //*****************************************
        virtual public bool isSafe(ref Base pBase, ref string strCDiscript, ref string strEDiscript, ref int ErrorCode)
        {
            if (m_Parent != null)
            {
                return m_Parent.isSafe(ref pBase, ref strCDiscript, ref strEDiscript, ref ErrorCode);       //向上層詢問是否安全
            }
            return true;
        }
        //*****************************************
        public double GetScanTime()
        {
            return m_dbScanTime;
        }
        public bool GetInitCompleted() { return m_bisInitCompleted; }
        public void SetInitCompleted(bool bCompleted)
        {
            m_bisInitCompleted = bCompleted;
        }
        public bool StartInitial()
        {
            if (CycleThread == null)
            {
                CycleThread = new Thread(DoThread);
                CycleThread.Priority = ThreadPriority.Highest;
                CycleThread.IsBackground = true;
                CycleThread.Start();
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool GetInitialStatus(ref int iRetCode)
        {
            iRetCode = m_nRetCode;
            if (m_nRetCode == (int)ErrorDefine.enErrorCode.Initial成功)
                return true;
            else
                return false;
        }
        private void DoThread()
        {
            DateTime dtOld = DateTime.Now;
            DateTime dtNow;
            iStep = 0;
            int iCheckVerifyCount = 0, iCheckPasswrodCount = 0;
            m_nRetCode = (int)ErrorDefine.enErrorCode.Initial中;
            while (!m_isRelease)
            {
                switch (iStep)
                {
                    #region Init
                    case 0: // Init
                        {
                            m_nRetCode = (int)ErrorDefine.enErrorCode.CardOPen;
                            m_Card0 = new DrvECatCard(typeof(Base.HomeStep_Base),typeof(Base.Step_Base),"板卡资源",this, "EtherCat Card0", "Ether軸卡0", 0, true, 0);
                            if (m_strMachinePath != "" && File.Exists(m_strMachinePath))
                            {
                                AllMotorAddToChild();  //Motor加入Child
                                AllValveAddToChild();  //Valve加入Child
                                AllTimerAddToChild();  //Timer加入Child
                                AddBase(ref BaseDataDefine.clsPointsMoveManage.m_NowAddress);//把点位管理加到base
                                //加载硬件配置
                                clsMotors clsMotor = new clsMotors(typeof(HomeStep_Base),typeof(StepType),"轴集合",this, 0, m_strMachinePath, "", 2000);
                                AddBase(ref clsMotor.m_NowAddress);
                                clsCylinders clsCylinder = new clsCylinders(typeof(HomeStep_Base), typeof(StepType), "气缸集合", this, 0, m_strMachinePath, "", 2000);
                                AddBase(ref clsCylinder.m_NowAddress);
                                clsIO_Ports clsPort = new clsIO_Ports(typeof(HomeStep_Base), typeof(StepType),"IO集合",this, 0, m_strMachinePath, "", 2000);
                                AddBase(ref clsPort.m_NowAddress);
                                //-------------------
                                m_nRetCode = (int)ErrorDefine.enErrorCode.LoadingData;
                                openMDB(m_strMachinePath);
                                LoadIOMachineData(m_strMachinePath);
                                LoadMotorMachineData(m_strMachinePath);
                                LoadMachineData(m_strMachinePath);
                                closeMDB(m_strMachinePath);
                                LoadWorkData(m_strWorkPath);
                                //-------------------
                                m_nRetCode = (int)ErrorDefine.enErrorCode.CheckVerifykey;
                                SearchValveUseIO();    //取得Valve IO Address
                                cardLoded();//执行板卡加载之后的事件

                                int iCount = ChildList.Count;
                                for (int i = 0; i < iCount; i++)
                                {
                                    StartStepCycle(ChildList[i]);
                                    getChild(ChildList[i]);
                                }
                                Init();
                                //Stop();
                                SetInitCompleted(true);
                                m_Card0.CardInit();
                                if (m_Card0.CardisOpen())
                                {
                                    m_nRetCode = (int)ErrorDefine.enErrorCode.CheckPassword;
                                    mTimer.Interval = Sys_Define.m_iRecordCardLoadRatioTimer;
                                    mTimer.Start();
                                    iStep = 5;
                                    ImcApi.IMC_GetVersion(Sys_Define.m_cardHandle, pVersion);
                                    try
                                    {
                                        StrCardVersion = pVersion[5].ToString() + "/" + pVersion[4].ToString() +
                                   "/" + pVersion[3].ToString() + "  V" + pVersion[2].ToString() + "." + pVersion[1].ToString() + "." + pVersion[0].ToString();
                                    }
                                    catch
                                    {


                                    }


                                }
                                else
                                {
                                    m_nRetCode = (int)ErrorDefine.enErrorCode.Initial失敗;
                                    CycleThread.Abort(); //軸卡開卡失敗 , 則結束Thread 
                                }
                            }
                            else
                            {
                                this.Cycle(ref m_dbScanTime);
                                Thread.Sleep(10);
                                if (m_bisInitCompleted == true)
                                    iStep = 5;  //Security_Check
                            }
                        }
                        break;
                    #endregion
                    //******************
                    #region Cycle 
                    case 5: //StartCycleRun
                        {
                            m_nRetCode = (int)ErrorDefine.enErrorCode.Initial成功;
                            dtNow = DateTime.Now;
                            TimeSpan total = dtNow.Subtract(dtOld);
                            m_dbScanTime = Convert.ToDouble(total.Milliseconds);
                            dtOld = dtNow;
                            this.Cycle(ref m_dbScanTime);
                            Thread.Sleep(1);
                            UInt32 m_retRtn;
                            //------------------------------
                            double dbLoadRatio = 0;
                            try
                            {
                                if (mTimer.Enabled == false && Sys_Define.m_bRecordCardLoadRatio)
                                {
                                    m_retRtn = ImcApi.IMC_GetCalcLoadRatio(Sys_Define.m_cardHandle, ref dbLoadRatio);
                                    string strLoadRatio = "Card_LoadRatio=" + dbLoadRatio.ToString();
                                    Sys_Define.RecordMessageLog("CardLoadRaito", strLoadRatio);
                                    //------------
                                    mTimer.Start();
                                }
                            }
                            catch
                            {
                                Sys_Define.RecordMessageLog("CardLoadRaito", "API DataFail!");
                            }
                        }
                        break;
                    #endregion
                    //--------
                    #region Reboot Card
                    case 100: //Start Reboot Card
                        {
                            UInt32 m_retRtn = ImcApi.IMC_RebootCard(Sys_Define.m_cardHandle);
                            if (m_retRtn != ImcApi.EXE_SUCCESS)
                                Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : IMC_RebootCard  Fail, NG_Code: 0x" + m_retRtn.ToString("x8"));
                            else
                            {
                                mTimer.Stop();
                                mTimer.Interval = 100;
                                mTimer.Start();
                                iStep = 105;
                            }
                        }
                        break;
                    case 105: //Get Reboot Status
                        if (mTimer.Enabled == false)
                        {
                            uint uStatus = 0;
                            UInt32 m_retRtn = ImcApi.IMC_GetRebootCardFinish(Sys_Define.m_cardHandle, ref uStatus);
                            if (m_retRtn != ImcApi.EXE_SUCCESS)
                                Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : IMC_GetRebootCardFinish  Fail, NG_Code: 0x" + m_retRtn.ToString("x8"));
                            else
                            {
                                if (uStatus == 0) //uStatus=0 , 代表Reboot完成
                                {
                                    mTimer.Interval = 500;
                                    mTimer.Start();
                                    iStep = 110;
                                }
                                else
                                {
                                    mTimer.Interval = 50;
                                    mTimer.Start();
                                }
                            }
                        }
                        break;
                    case 110: //Get Reboot Status
                        if (mTimer.Enabled == false && m_Card0.CardisOpen())
                        {
                            bool bCardClose = m_Card0.CloseCard();
                            if (bCardClose)
                            {
                                mTimer.Interval = 100;
                                mTimer.Start();
                                iStep = 115;
                            }
                        }
                        break;
                    case 115:
                        if (mTimer.Enabled == false)
                        {
                            CycleThread.Abort(); //軸卡Reboot結束 , 則結束Thread 
                        }
                        break;
                        #endregion
                }
            }
        }

        public void ReBootCard()
        {
            if (m_Parent == null && iStep < 100)
                iStep = 100;
        }
        double StepScanTime = 0;
        public bool StartStepCycle(Base parent)
        {
            Thread StepCycleThread = new Thread(new ParameterizedThreadStart(DoStepThread));
            StepCycleThread.Priority = ThreadPriority.Normal;
            StepCycleThread.IsBackground = true;
            //
            StepCycleThread.Start(parent);
            return true;
        }
        private void getChild(Base child)
        {
            int iCount = child.ChildList.Count;
            for (int i = 0; i < iCount; i++)
            {
                StartStepCycle(child.ChildList[i]);
                if (child.ChildList[i].ChildList.Count > 0)
                {
                    getChild(child.ChildList[i]);
                }
            }
        }
        public void DoStepThread(Object ChildBase)
        {
            //  try
            //  {
            //Queue<string> que = new Queue<string>();
            //bool m_One = false;
            double m_dbStepScanTime = 0;
            DateTime dtOld = DateTime.Now;
            DateTime dtNow;
            while (true)
            {
                dtNow = DateTime.Now;
                TimeSpan total = dtNow.Subtract(dtOld);
                Base Child = (Base)ChildBase;

                m_dbStepScanTime = Convert.ToDouble(total.Milliseconds);

                StepScanTime = m_dbStepScanTime;


                dtOld = dtNow;
                Thread.Sleep(1);

                Child.Cycle(ref m_dbStepScanTime);
            }
            //}
            //catch(Exception e)
            //{
            //    MessageBox.Show("程序发生异常，请重新启动"+ e.Message);
            //}
        }
        /// <summary>
        /// 板卡加载之后
        /// </summary>
        public virtual void cardLoded()
        {

        }
        /// <summary>
        /// 延时
        /// </summary>
        /// <param name="time"></param>
        public void timeDelayStart(int time)
        {
            timerDelay.Enabled = false;
            timerDelay.Interval = time;
            timerDelay.Start();
        }
    }
}
