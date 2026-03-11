using System;
using System.Linq;
using System.Data;
using System.Text;
using System.IO.Ports;
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using MotionBase;
using OpenOffice_Connect;
using Common.Excel;
using System.Windows.Forms;

namespace MotionBase
{
    public class DrvValve : Base
    {
        #region 建構&解構式
        public DrvValve(Type homeEnum1, Type stepEnum1, string instanceName1,Base parent, int nStation, string strID, string strEName, string strCName, int ErrCodeBase = 0)
            : base(homeEnum1,stepEnum1,instanceName1,parent, nStation, strEName, strCName, ErrCodeBase)
        {
            m_strID = strID;
            m_strEName = strEName;
            //------------
            ValveList.Add(strCName, (DrvValve)m_NowAddress);
            m_nValveCount++;
            m_ActionMode = enActionMode.mode_Normal;
            //------------------------------------------
            m_tmOpenWait = new System.Timers.Timer(1000);
            m_tmOpenTimeOut = new System.Timers.Timer(1000);
            m_tmCloseWait = new System.Timers.Timer(1000);
            m_tmCloseTimeOut = new System.Timers.Timer(1000);
            m_tmRepeatWait = new System.Timers.Timer(1000);
            //------------
            m_tmOpenWait.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent_OpenWait);
            m_tmOpenTimeOut.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent_OpenTimeOut);
            m_tmCloseWait.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent_CloseWait);
            m_tmCloseTimeOut.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent_CloseTimeOut);
            m_tmRepeatWait.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent_RepeatTimeOut);
        }

        ~DrvValve()
        {
        }
        #endregion
        //-------------------------
        #region 參數&變數
        protected string m_strID;
        public string m_strOpenIO, m_strCloseIO, m_strOpenSR, m_strCloseSR;
        public enum enActionMode
        {
            mode_Normal,
            Mode_Test,      //不進行Sensor確認 , 即認定OK
            Mode_TestPassCheckTime, //不進行Sensor確認, Pass檢查時間, 即認定OK
            Mode_OkThenOff, //如果成功就自動關(OffValve)
            Mode_NgThenOff, //如果不成功就自動關(OffValve)
            Mode_NgThenPassError,  ////如果不成功 , 則pass Error訊息
        }
        enActionMode m_ActionMode;
        public enum enStep
        {
            StartOpenValve = 0,
            WaittingOpen,
            WaittingOpenWaitTime,
            OpenCompleted,
            WaitOpenRepeatWaitTime,
            //-----------
            StartCloseValve,
            WaittingClose,
            WaittingCloseWaitTime,
            CloseCompletetd,
            WaitCloseRepeatWaitTime,
        }
        enStep m_Step;


        enum enValveErrorCode
        {
            en电磁阀Open逾时 = 0,
            en电磁阀Close逾时,
            enMax,
        }

        int[] m_nValveErrorCode = new int[(int)enValveErrorCode.enMax];

        public struct tyValve_Parameter
        {
            public int i_Station;
            public string strID;
            public string strEName;
            public string strCName;
            //---------------------
            public string strOpenIO;
            public string strCloseIO;
            public string strOpenSR;
            public string strCloseSR;

            //-------------------
            public uint OpenWaitTime;
            public uint OpenTimeOut;
            public uint CloseWaitTime;
            public uint CloseTimeOut;
            //--------------------
        }
        //-------------------------
        public DrvIO m_OpenIO = null, m_CloseIO = null;
        public DrvIO m_OpenSR = null, m_CloseSR = null;
        System.Timers.Timer m_tmOpenWait, m_tmOpenTimeOut;
        System.Timers.Timer m_tmCloseWait, m_tmCloseTimeOut;
        System.Timers.Timer m_tmRepeatWait;
        public uint m_OpenWaitTime, m_OpenTimeOut, m_CloseWaitTime, m_CloseTimeOut;
        bool m_bOpen = false, m_bClose = false, m_bRepeat = false;
        bool m_bTestRunMode = false;
        #endregion
        //-------------------------
        private void OnTimedEvent_OpenWait(object source, System.Timers.ElapsedEventArgs e) { m_tmOpenWait.Enabled = false; }
        private void OnTimedEvent_OpenTimeOut(object source, System.Timers.ElapsedEventArgs e) { m_tmOpenTimeOut.Enabled = false; }
        private void OnTimedEvent_CloseWait(object source, System.Timers.ElapsedEventArgs e) { m_tmCloseWait.Enabled = false; }
        private void OnTimedEvent_CloseTimeOut(object source, System.Timers.ElapsedEventArgs e) { m_tmCloseTimeOut.Enabled = false; }
        private void OnTimedEvent_RepeatTimeOut(object source, System.Timers.ElapsedEventArgs e) { m_tmRepeatWait.Enabled = false; }

        //-------------------------
        public bool GetParameter(ref tyValve_Parameter tyParameter)
        {
            tyParameter.i_Station = m_nStation;
            tyParameter.strID = m_strID;
            tyParameter.strCName = m_strCName;
            //tyParameter.strEName = m_strEName;
            //------------------
            tyParameter.strOpenIO = m_strOpenIO;
            tyParameter.strCloseIO = m_strCloseIO;
            tyParameter.strOpenSR = m_strOpenSR;
            tyParameter.strCloseSR = m_strCloseSR;
            //-------------------
            tyParameter.OpenWaitTime = m_OpenWaitTime;
            tyParameter.OpenTimeOut = m_OpenTimeOut;
            tyParameter.CloseWaitTime = m_CloseWaitTime;
            tyParameter.CloseTimeOut = m_CloseTimeOut;
            //----------------
            return true;
        }
        public bool GetisOpen() { return m_bOpen; }
        public bool GetisClose() { return m_bClose; }
        public bool OffAction()
        {
            if (m_OpenIO != null)
                m_OpenIO.SetIO(false);
            if (m_CloseIO != null)
                m_CloseIO.SetIO(false);
            //------------
            m_bOpen = m_bClose = false;
            return true;
        }

        public void SetTestRunMode(bool bMode) { m_bTestRunMode = bMode; }
        //-------------------------
        public override bool LoadMachineData(string strMachinePath)
        {
            if (m_SystemDataBaseType == enSystemDataBaseType.enTXT)       //判斷MachineData檔案格式
                LoadTxtMachineData(strMachinePath);
            else
                LoadMDBMachineData(strMachinePath);
            //------------------------------------

            //LoadErrorCodeData(strMachinePath);
            LoadTxtErrorCodeData(strMachinePath);
            //------------------------------------
            return base.LoadMachineData(strMachinePath);
        }

        private bool LoadMDBMachineData(string strMachinePath)
        {
            bool bRet = true;
            //oDB = new Sys_DataBase(strMachinePath);
            System.Data.Common.DbDataReader oRs = null;
            bool RetValue = false;
            try
            {
                string sSQL;
                bool ret = false, retRead = false;
                sSQL = "Select * From  " + m_strEName + " Where ID=" + "'" + m_strID + "'";
                ret = oDB_load.Find(sSQL, ref oRs);
                if (ret)
                {
                    retRead = oRs.Read();
                    //----------------
                    m_strCName = (oRs["CName"]).ToString();
                    m_strEName = (oRs["EName"]).ToString();
                    //----------------------------------
                    m_strOpenIO = (oRs["OpenIO_ID"]).ToString();
                    m_strCloseIO = (oRs["CloseIO_ID"]).ToString();
                    m_strOpenSR = (oRs["OpenSR_ID"]).ToString();
                    m_strCloseSR = (oRs["CloseSR_ID"]).ToString();
                    //----------------------------------
                    m_OpenWaitTime = Convert.ToUInt16(oRs["OpenWaitTime"]);
                    m_OpenTimeOut = Convert.ToUInt16(oRs["OpenTimeOut"]);
                    m_CloseWaitTime = Convert.ToUInt16(oRs["CloseWaitTime"]);
                    m_CloseTimeOut = Convert.ToUInt16(oRs["CloseTimeOut"]);

                }
                else
                {
                    sSQL = ("Insert into Valves (ID,CName,EName,OpenIO_ID,OpenSR_ID,CloseIO_ID,CloseSR_ID,OpenWaitTime,OpenTimeOut,CloseWaitTime,CloseTimeOut) Values('" +
                                                 m_strID + "','" + m_strCName + "','" + m_strEName + "',,,,,1000,1000,1000,1000)");
                    ret = oDB_load.Save(sSQL);
                }
                oDB_load.Fun_CloseRS(ref oRs);
                //oRs = null;
                //oDB.Fun_CloseDB();
                //oDB = null;
                //RetValue = (ret && retRead) ? true : false;
            }
            catch (Exception ex)
            {
                oDB_load.Fun_CloseRS(ref oRs);
                //oRs = null;
                //oDB.Fun_CloseDB();
                //oDB = null;
                return false;
            }
            return base.LoadMachineData(strMachinePath);

        }
        private bool LoadTxtMachineData(string strMachinePath)
        {
            string striniFilePath = strMachinePath.Replace(".mdb", "");
            striniFilePath = striniFilePath + ".txt";
            //---------------------------

            bool bRet = true;
            //String iniPath = strMachinePath;

            String iniPath = striniFilePath;
            String strData = "", strValveID = "";
            String[] strPramData = new string[10];
            StreamReader sr;
            try
            {
                sr = new StreamReader(iniPath, Encoding.Default);
            }
            catch (Exception)
            {
                return false;
            }

            String line;
            while ((line = sr.ReadLine()) != null)
            {
                strValveID = "";
                strData = line.Replace(" ", "");
                strData = strData.Replace("\t", "");
                //-------------------
                int iLength = strData.Length;
                int iIndex = strData.IndexOf(",");
                if (iIndex > 0)
                    strValveID = strData.Remove(iIndex);

                if (strValveID == m_strID)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        strData = strData.Remove(0, iIndex + 1);
                        iIndex = strData.IndexOf(",");
                        if (iIndex >= 0)
                            strPramData[i] = strData.Remove(iIndex);
                        else
                            strPramData[i] = strData;
                    }
                }
            }

            m_strCName = strPramData[0];
            m_strEName = strPramData[1];
            m_strOpenIO = Convert.ToString(strPramData[2]);
            m_strOpenSR = Convert.ToString(strPramData[3]);
            m_strCloseIO = Convert.ToString(strPramData[4]);
            m_strCloseSR = Convert.ToString(strPramData[5]);
            //----------------------------------
            m_OpenWaitTime = Convert.ToUInt16(strPramData[6]);
            m_OpenTimeOut = Convert.ToUInt16(strPramData[7]);
            m_CloseWaitTime = Convert.ToUInt16(strPramData[8]);
            m_CloseTimeOut = Convert.ToUInt16(strPramData[9]);

            return bRet;
        }

        private bool LoadErrorCodeData(string strMachinePath)
        {

            bool bRet = true;
            oDB = new Sys_DataBase(strMachinePath);
            System.Data.Common.DbDataReader oRs = null;
            bool RetValue = false;
            try
            {
                string sSQL;
                bool ret = false, retRead = false;
                sSQL = "Select * From  ErrorCode Where ID=" + "'" + m_strID + "'";
                ret = oDB.Fun_RsSQL(sSQL, ref oRs);
                if (ret)
                {
                    retRead = oRs.Read();

                    for (int i = 0; i < m_nValveErrorCode.Length; i++)
                    {
                        string strData = "ErrorCode" + (i + 1).ToString();
                        m_nValveErrorCode[i] = Convert.ToUInt16(oRs[strData]);
                    }
                }
                oDB.Fun_CloseRS(ref oRs);
                oRs = null;
                oDB.Fun_CloseDB();
                oDB = null;
                //RetValue = (ret && retRead) ? true : false;
            }
            catch (Exception ex)
            {
                oDB.Fun_CloseRS(ref oRs);
                oRs = null;
                oDB.Fun_CloseDB();
                oDB = null;
                return false;
            }
            return bRet;
        }

        private bool LoadTxtErrorCodeData(string strMachinePath)
        {
            bool bRet = true;
            try
            {
                string striniFilePath = strMachinePath.Replace(@"DataBaseData\Machine.mdb", "");
                striniFilePath = striniFilePath + @"Cowain_AutoMotion\bin\x64\Debug\Config\ERROR.xls";
                List<string[]> lists = ExcelHelper_.ReadFromExcel(striniFilePath, "ERROR");
                List<string> datas = new List<string>();
                for (int i = 0; i < lists.Count; i++)
                {
                    if (lists[i][7] == m_strID)
                    {
                        datas.Add(lists[i][0]);
                    }
                }
                for (int j = 0; j < m_nValveErrorCode.Length; j++)
                {
                    m_nValveErrorCode[j] = Convert.ToInt16(datas[j]);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("在读取ERRORCODE时出错," + e.Message);
                bRet = false;
            }
            return bRet;
        }
        public override void StepCycle(ref double dbTime)
        {
            string strD = null, strE = null;
            int iErrorCode = 0;
            m_Step = (enStep)m_nStep;
            switch (m_Step)
            {
                //***********************************
                #region Open流程
                case enStep.StartOpenValve:
                    if (isSafe(ref this.m_NowAddress, ref strD, ref strE, ref iErrorCode)) //詢問上層是否安全
                    {
                        if (m_OpenIO != null)
                            m_OpenIO.SetIO(true);
                        if (m_CloseIO != null)
                            m_CloseIO.SetIO(false);
                        //------------------
                        m_tmOpenTimeOut.Interval = m_OpenTimeOut;
                        m_tmOpenTimeOut.Start();
                        //------------------
                        m_nStep = (int)enStep.WaittingOpen;
                    }
                    else
                    {
                        Error pError = new Error(ref this.m_NowAddress, strD, strE, iErrorCode);
                        pError.AddErrSloution("再試一次/Retry", (int)enStep.StartOpenValve);
                        pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    }
                    break;
                case enStep.WaittingOpen:
                    if (m_OpenSR != null)
                    {
                        Thread.Sleep(1000);
                        m_OpenSR.SetIO(true);
                        m_CloseSR.SetIO(false);
                        if (m_OpenSR.GetValue() || m_bTestRunMode || m_ActionMode == enActionMode.Mode_TestPassCheckTime)
                        {
                            m_tmOpenWait.Interval = m_OpenWaitTime;
                            m_tmOpenWait.Start();
                            m_tmOpenTimeOut.Enabled = false;
                            //-------------------
                            m_nStep = (int)enStep.WaittingOpenWaitTime;
                        }
                        else
                        {
                            if (m_tmOpenTimeOut.Enabled == false)
                            {
                                if (m_ActionMode == enActionMode.Mode_Test)  // 若為Mode_Test ,則跳過(PassWait) TimeOut確認時間
                                {
                                    m_tmOpenWait.Interval = m_OpenWaitTime;
                                    m_tmOpenWait.Start();
                                    //-------------------
                                    m_nStep = (int)enStep.WaittingOpenWaitTime;
                                }
                                else
                                {
                                    if (m_ActionMode == enActionMode.Mode_NgThenPassError) //若為Mode_NgThenPassError , Pass Error視窗顯示
                                    {
                                        m_nStep = (int)enStep.OpenCompleted;
                                    }
                                    else
                                    {
                                        if (m_ActionMode == enActionMode.Mode_NgThenOff)  //若為測試模式 , Pass TimeOut
                                        {
                                            if (m_OpenIO != null)
                                                m_OpenIO.SetIO(false);
                                        }
                                        //Error pError = new Error(ref this.m_NowAddress, "电磁阀Open逾时", "", (int)ErrorDefine.enErrorCode.电磁阀Open逾时);
                                        string strErrMsg = "(" + m_strID +"["+ m_strCName + "]"+ ")" + "电磁阀Open逾时";
                                        Error pError = new Error(ref this.m_NowAddress, strErrMsg, "", m_nValveErrorCode[(int)enValveErrorCode.en电磁阀Open逾时]);
                                        pError.AddErrSloution("再试一次/Retry", (int)enStep.StartOpenValve);
                                        pError.AddErrSloution("确认OK /Pass", (int)enStep.WaittingOpenWaitTime);
                                        pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        m_tmOpenWait.Interval = m_OpenWaitTime;
                        m_tmOpenWait.Start();
                        m_nStep = (int)enStep.WaittingOpenWaitTime;
                    }
                    break;
                case enStep.WaittingOpenWaitTime:
                    if (m_tmOpenWait.Enabled == false)
                    {
                        if (m_ActionMode == enActionMode.Mode_OkThenOff)  //若為OkThenOff , 則Off ,Open I/O
                        {
                            if (m_OpenIO != null)
                                m_OpenIO.SetIO(false);
                        }
                        m_nStep = (int)enStep.OpenCompleted;
                    }
                    break;
                case enStep.OpenCompleted:
                    {
                        //if (m_ActionMode == enActionMode.Mode_NgThenPassError && m_OpenSR != null && !m_OpenSR.GetValue() )
                        //    m_bOpen = false;
                        //else
                        m_bOpen = true;
                        //-------------------
                        m_bClose = false;
                        if (m_bRepeat)
                        {
                            m_tmRepeatWait.Start();
                            m_nStep = (int)enStep.WaitOpenRepeatWaitTime;
                        }
                        else
                        {
                            m_Status = 狀態.待命;
                        }
                    }
                    break;
                case enStep.WaitOpenRepeatWaitTime:
                    if (m_tmRepeatWait.Enabled == false)
                    {
                        m_nStep = (int)enStep.StartCloseValve;
                    }
                    break;
                #endregion 
                //***********************************
                #region Close流程
                case enStep.StartCloseValve:
                    if (isSafe(ref this.m_NowAddress, ref strD, ref strE, ref iErrorCode)) //詢問上層是否安全
                    {
                        if (m_OpenIO != null)
                            m_OpenIO.SetIO(false);
                        if (m_CloseIO != null)
                            m_CloseIO.SetIO(true);
                        //------------------
                        m_tmCloseTimeOut.Interval = m_CloseTimeOut;
                        m_tmCloseTimeOut.Start();
                        //------------------
                        m_nStep = (int)enStep.WaittingClose;
                    }
                    else
                    {
                        Error pError = new Error(ref this.m_NowAddress, strD, strE, iErrorCode);
                        pError.AddErrSloution("再试一次/Retry", (int)enStep.StartCloseValve);
                        pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    }
                    break;
                case enStep.WaittingClose:
                    if (m_CloseSR != null)
                    {
                        Thread.Sleep(1000);
                        m_OpenSR.SetIO(false);
                        m_CloseSR.SetIO(true);
                        if (m_CloseSR.GetValue() || m_bTestRunMode || m_ActionMode == enActionMode.Mode_TestPassCheckTime)
                        {
                            m_tmCloseWait.Interval = m_CloseWaitTime;
                            m_tmCloseWait.Start();
                            m_tmCloseTimeOut.Enabled = false;
                            //-------------------
                            m_nStep = (int)enStep.WaittingCloseWaitTime;
                        }
                        else
                        {
                            if (m_tmCloseTimeOut.Enabled == false)
                            {
                                if (m_ActionMode == enActionMode.Mode_Test)  //若為測試模式 , Pass 電磁閥Close逾時
                                {
                                    m_tmCloseWait.Interval = m_CloseWaitTime;
                                    m_tmCloseWait.Start();
                                    //-------------------
                                    m_nStep = (int)enStep.WaittingCloseWaitTime;
                                }
                                else
                                {
                                    if (m_ActionMode == enActionMode.Mode_NgThenPassError) //若為Mode_NgThenPassError , Pass Error視窗顯示
                                    {
                                        m_nStep = (int)enStep.CloseCompletetd;
                                    }
                                    else
                                    {
                                        if (m_ActionMode == enActionMode.Mode_NgThenOff)  //若為測試模式 , Pass TimeOut
                                        {
                                            if (m_CloseIO != null)
                                                m_CloseIO.SetIO(false);
                                        }
                                        //Error pError = new Error(ref this.m_NowAddress, "电磁阀Close逾时", "", (int)ErrorDefine.enErrorCode.电磁阀Close逾时);
                                        string strErrMsg = "(" + m_strID + "[" + m_strCName + "]" + ")" + "电磁阀Close逾时";
                                        Error pError = new Error(ref this.m_NowAddress, strErrMsg, "", m_nValveErrorCode[(int)enValveErrorCode.en电磁阀Close逾时]);
                                        pError.AddErrSloution("再试一次/Retry", (int)enStep.StartCloseValve);
                                        pError.AddErrSloution("确认OK /Pass", (int)enStep.WaittingCloseWaitTime);
                                        pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                                    }

                                }
                            }
                        }
                    }
                    else
                    {
                        m_tmCloseWait.Interval = m_CloseWaitTime;
                        m_tmCloseWait.Start();
                        m_nStep = (int)enStep.WaittingCloseWaitTime;
                    }
                    break;
                case enStep.WaittingCloseWaitTime:
                    if (m_tmCloseWait.Enabled == false)
                    {
                        if (m_ActionMode == enActionMode.Mode_OkThenOff)  //若為OkThenOff , 則Off ,Open I/O
                        {
                            if (m_CloseIO != null)
                                m_CloseIO.SetIO(false);
                        }
                        m_nStep = (int)enStep.CloseCompletetd;
                    }
                    break;
                case enStep.CloseCompletetd:
                    {
                        m_bOpen = false;
                        //if (m_ActionMode == enActionMode.Mode_NgThenPassError)
                        //    m_bClose = false;
                        //else
                        m_bClose = true;
                        //--------------------------
                        if (m_bRepeat)
                        {
                            m_tmRepeatWait.Start();
                            m_nStep = (int)enStep.WaitCloseRepeatWaitTime;
                        }
                        else
                        {
                            m_Status = 狀態.待命;
                        }
                    }
                    break;
                case enStep.WaitCloseRepeatWaitTime:
                    if (m_tmRepeatWait.Enabled == false)
                    {
                        m_nStep = (int)enStep.StartOpenValve;
                    }
                    break;
                    #endregion
                    //***********************************
            }
            base.StepCycle(ref dbTime);
        }
        public override void Stop()
        {
            //-----------
            m_bRepeat = false;
            base.Stop();
        }
        public bool Open(enActionMode ActMode = enActionMode.mode_Normal)
        {
            m_bRepeat = m_bOpen = m_bClose = false;
            m_Mode = enMode.自動;
            m_ActionMode = ActMode;
            int doStep = (int)enStep.StartOpenValve;
            bool bRet = DoStep(doStep);
            return bRet;
        }
        public bool Close(enActionMode ActMode = enActionMode.mode_Normal)
        {
            m_bRepeat = m_bOpen = m_bClose = false;
            m_Mode = enMode.自動;
            m_ActionMode = ActMode;
            int doStep = (int)enStep.StartCloseValve;
            bool bRet = DoStep(doStep);
            return bRet;
        }
        public bool Repeat(enActionMode ActMode = enActionMode.mode_Normal, uint uiRepeatWaitTime = 1000)
        {
            m_tmRepeatWait.Interval = uiRepeatWaitTime;
            m_bOpen = m_bClose = false;
            m_bRepeat = true;
            m_ActionMode = ActMode;
            m_Mode = enMode.自動;
            //--------------------
            int doStep = (int)enStep.StartOpenValve;
            bool bRet = DoStep(doStep);
            return bRet;
        }
    }
}
