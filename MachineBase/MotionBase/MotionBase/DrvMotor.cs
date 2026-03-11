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
using Common.Excel;
using System.Windows.Forms;
using System.Timers;

namespace MotionBase
{
    public class DrvMotor : Base
    {
        public DrvMotor(Type homeEnum1, Type stepEnum1, string instanceName1, Base parent, int nStation, String strID, String strEName, String strCName, int ErrCodeBase)
            : base(homeEnum1, stepEnum1, instanceName1, parent, nStation, strEName, strCName, ErrCodeBase)
        {
            m_strID = strID;
            //----------------
            MotorList.Add(strCName, (DrvMotor)m_NowAddress);
            m_nMotoCount++;
            //----------------
            mTimer = new System.Timers.Timer(1000);
            mTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);
            mTimer1 = new System.Timers.Timer(1000);
            mTimer1.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent1);
        }


        //--------
        #region 參數&變數
        //************************
        protected string m_strID, m_strUnit;
        protected ushort m_AxisID, m_CardID;
        protected double m_dbUnitRev, m_dbPluseRev, m_dbHomeOffset;
        protected ushort m_HomeMode, m_AxisDir;
        //----------------
        protected ushort m_PluseModule, m_ModuleMelLogic, m_ModulePelLogic;
        protected ushort m_Module_InPluseMode, m_Module_OutPluseMode;
        //----------------
        protected uint m_HiSpeed, m_LoSpeed, m_HomeLoSpeed, m_HomeHiSpeed;
        protected uint m_DelayTime, m_HomeTime, m_ActionTime;
        protected uint m_ErrorCode;
        //----------------
        protected double m_dbHiAccTime, m_dbHiDesTime;
        protected double m_dbLoAccTime, m_dbLoDesTime;
        protected double m_dbHomeAccTime, m_dbHomeDesTime;

        System.Timers.Timer mTimer;
        System.Timers.Timer mTimer1;
        protected double m_dbMaxPos, m_dbMinPos;
        protected double m_dbPos1, m_dbPos2;
        protected double m_dbNowPos, m_dbMoveingSpeed, m_dbTargetPos, m_dbDist;
        protected double m_dbNowCommand, m_dbNowPDOCommand;
        //----------
        protected bool m_bisHiSpeed = true, m_bStopSevOffThenOn = false;
        protected bool m_bStopReSetCommand = false;
        protected bool m_bCheckMotionDone = true;
        protected int m_nCheckCount;
        //----------
        protected int m_nNowPositionPPS, m_nNowCommandPPS, m_nNowPDOCommandPPS;

        //----------
        /// <summary>
        /// 在轴没有到位，但是已经停止运动时，增加自动第二次运动，加一个补丁
        /// </summary>
        private bool b_Retry = false;
        enum enMotorErrorCode
        {
            enMLError = 0,
            enPLError,
            enDriverAlarm,
            en未Servo_On,
            en未Motion_Done,
            en回原点逾时,
            enOverSoftLimit,
            enAbs位置移动逾时,
            enRev位置移动逾时,
            enRepeat移动逾时,
            enMax,
        }

        int[] m_nMotorErrorCode = new int[(int)enMotorErrorCode.enMax];

        //----------
        public struct tyMotor_Parameter
        {
            public int i_Station;
            public string strID;
            public string strEName;
            public string strCName;
            public string strUnit;
            //-------------------
            public ushort uAxisID;
            public ushort uCardID;
            public double dbUnitRev;
            public double dbPluseRev;
            public double dbHomeOffset;
            public ushort uHomeMode;
            public ushort uAxisDir;
            //----------------
            public ushort uPluseModule;
            public ushort uModule_InPluseMode;
            public ushort uModule_OutPluseMode;
            public ushort uModuleMelLogic;
            public ushort uModulePelLogic;
            //---------------
            public uint HiSpeed;
            public uint LoSpeed;
            public uint HomeLoSpeed;
            public uint HomeHiSpeed;
            public uint DelayTime;
            public uint HomeTime;
            public uint ActionTime;
            //----------------
            public double dbHiAccTime;
            public double dbHiDesTime;
            public double dbLoAccTime;
            public double dbLoDesTime;
            public double dbHomeAccTime;
            public double dbHomeDesTime;
            //----------------
            public double dbMaxPos;
            public double dbMinPos;
            public double dbPos1;
            public double dbPos2;
            //--------------------



        }

        //****************************
        #endregion
        //-------------------------
        public enum enHomeStep
        {
            StartHome = 0,
            DoHoming,
            HomeOffset,
            HomeCompleted,

        }
        enHomeStep m_HomeStep;
        public enum enStep
        {
            StartAbsMove = 0,
            AbsMoving,
            AbsMoveToLimitPos,
            AbsMoveisMotionDone,
            AbsMoveCompleted,
            //------------
            StartRevMove,
            RevMoving,
            RevMoveToLimitPos,
            RevMoveCompleted,
            //------------
            StartRepeat,
            RepeatMoving,
            MoveToPos1,
            WaittingTimer,
            MoveToPos2,
            //------------
            CheckError,
            ActionError,
        }
        enStep m_Step;

        //------------------------------------
        public double GetUnitRev() { return m_dbUnitRev; }

        public double GetPluseRev() { return m_dbPluseRev; }
        public bool GetParameter(ref tyMotor_Parameter tyParameter)
        {
            tyParameter.i_Station = m_nStation;
            tyParameter.strID = m_strID;
            tyParameter.strCName = m_strCName;
            tyParameter.strEName = m_strEName;
            tyParameter.strUnit = m_strUnit;
            //-------------------
            tyParameter.uAxisID = m_AxisID;
            tyParameter.uCardID = m_CardID;
            tyParameter.dbUnitRev = m_dbUnitRev;
            tyParameter.dbPluseRev = m_dbPluseRev;
            tyParameter.dbHomeOffset = m_dbHomeOffset;
            tyParameter.uHomeMode = m_HomeMode;
            tyParameter.uAxisDir = m_AxisDir;
            //-------------------
            tyParameter.uPluseModule = m_PluseModule;
            tyParameter.uModule_InPluseMode = m_Module_InPluseMode;
            tyParameter.uModule_OutPluseMode = m_Module_OutPluseMode;
            tyParameter.uModuleMelLogic = m_ModuleMelLogic;
            tyParameter.uModulePelLogic = m_ModulePelLogic;
            //-------------------
            tyParameter.HiSpeed = m_HiSpeed;
            tyParameter.LoSpeed = m_LoSpeed;
            tyParameter.HomeLoSpeed = m_HomeLoSpeed;
            tyParameter.HomeHiSpeed = m_HomeHiSpeed;
            tyParameter.DelayTime = m_DelayTime;
            tyParameter.HomeTime = m_HomeTime;
            tyParameter.ActionTime = m_ActionTime;
            //----------------
            tyParameter.dbHiAccTime = m_dbHiAccTime;
            tyParameter.dbHiDesTime = m_dbHiDesTime;
            tyParameter.dbLoAccTime = m_dbLoAccTime;
            tyParameter.dbLoDesTime = m_dbLoDesTime;
            tyParameter.dbHomeAccTime = m_dbHomeAccTime;
            tyParameter.dbHomeDesTime = m_dbHomeDesTime;
            //----------------
            tyParameter.dbMaxPos = m_dbMaxPos;
            tyParameter.dbMinPos = m_dbMinPos;
            tyParameter.dbPos1 = m_dbPos1;
            tyParameter.dbPos2 = m_dbPos2;

            return true;
        }
        private void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e) { mTimer.Enabled = false; }
        private void OnTimedEvent1(object source, System.Timers.ElapsedEventArgs e) { mTimer1.Enabled = false; }
        //------------------------------------
        public override bool Init()
        {
            return base.Init();
        }
        public override void Stop()
        {
            MotorStop();
            m_Status = 狀態.待命;
            base.Stop();
        }
        public override bool LoadMachineData(string strMachinePath)
        {
            if (m_SystemDataBaseType == enSystemDataBaseType.enTXT)       //判斷MachineData檔案格式
                LoadTxtMachineData(strMachinePath);
            else
                LoadMDBMachineData(strMachinePath);

            //--------------------
            // LoadErrorCodeData(strMachinePath);
            LoadTxtErrorCodeData(strMachinePath);
            //--------------------
            return base.LoadMachineData(strMachinePath);
        }

        public bool LoadXmlMachineData(string strMachinePath)
        {
            bool bRet = true;

            string strXmlFilePath = strMachinePath.Replace(".mdb", "");
            strXmlFilePath = strXmlFilePath + ".xml";

            string strMotorNode = "MachineData/Motors/" + m_strID;
            GetXmlData(strXmlFilePath, strMotorNode, "CardID", ref m_CardID);
            GetXmlData(strXmlFilePath, strMotorNode, "AxisID", ref m_AxisID);
            GetXmlData(strXmlFilePath, strMotorNode, "UnitRev", ref m_dbUnitRev);
            GetXmlData(strXmlFilePath, strMotorNode, "PulseRev", ref m_dbPluseRev);
            GetXmlData(strXmlFilePath, strMotorNode, "HomeMode", ref m_HomeMode);
            //-----------

            //-----------
            GetXmlData(strXmlFilePath, strMotorNode, "AxisDir", ref m_AxisDir);
            GetXmlData(strXmlFilePath, strMotorNode, "HiSpeed", ref m_HiSpeed);
            GetXmlData(strXmlFilePath, strMotorNode, "LoSpeed", ref m_LoSpeed);
            GetXmlData(strXmlFilePath, strMotorNode, "HomeHiSpeed", ref m_HomeHiSpeed);
            GetXmlData(strXmlFilePath, strMotorNode, "HomeLoSpeed", ref m_HomeLoSpeed);
            //------------
            GetXmlData(strXmlFilePath, strMotorNode, "HiAccTime", ref m_dbHiAccTime);
            GetXmlData(strXmlFilePath, strMotorNode, "HiDesTime", ref m_dbHiDesTime);
            GetXmlData(strXmlFilePath, strMotorNode, "LoAccTime", ref m_dbLoAccTime);
            GetXmlData(strXmlFilePath, strMotorNode, "LoDesTime", ref m_dbLoDesTime);
            GetXmlData(strXmlFilePath, strMotorNode, "HomeAccTime", ref m_dbHomeAccTime);
            //------------
            GetXmlData(strXmlFilePath, strMotorNode, "HomeDesTime", ref m_dbHomeDesTime);
            GetXmlData(strXmlFilePath, strMotorNode, "HomeOffset", ref m_dbHomeOffset);
            GetXmlData(strXmlFilePath, strMotorNode, "PMaxPos", ref m_dbMaxPos);
            GetXmlData(strXmlFilePath, strMotorNode, "MMinPos", ref m_dbMinPos);
            GetXmlData(strXmlFilePath, strMotorNode, "P1", ref m_dbPos1);
            //------------
            GetXmlData(strXmlFilePath, strMotorNode, "P2", ref m_dbPos2);
            GetXmlData(strXmlFilePath, strMotorNode, "Delay", ref m_DelayTime);
            GetXmlData(strXmlFilePath, strMotorNode, "HomeTime", ref m_HomeTime);
            GetXmlData(strXmlFilePath, strMotorNode, "ActionTime", ref m_ActionTime);
            GetXmlData(strXmlFilePath, strMotorNode, "Unit", ref m_strUnit);

            ////-------------------------------------------
            //mTimer.Interval = m_DelayTime;
            mTimer.Interval = 1;

            return bRet;
        }
        private bool LoadTxtMachineData(string strMachinePath)
        {
            string striniFilePath = strMachinePath.Replace(".mdb", "");
            striniFilePath = striniFilePath + ".txt";
            //---------------------------

            bool bRet = true;
            //String iniPath = strMachinePath;

            String iniPath = striniFilePath;
            String strData = "", strMotorID = "";
            String[] strPramData = new string[27];
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
                strMotorID = "";
                strData = line.Replace(" ", "");
                strData = strData.Replace("\t", "");
                //-------------------
                int iLength = strData.Length;
                int iIndex = strData.IndexOf(",");
                if (iIndex > 0)
                    strMotorID = strData.Remove(iIndex);

                if (strMotorID == m_strID)
                {
                    for (int i = 0; i < 27; i++)
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
            m_CardID = Convert.ToUInt16(strPramData[2]);
            m_AxisID = Convert.ToUInt16(strPramData[3]);
            m_dbUnitRev = Convert.ToDouble(strPramData[4]);
            m_dbPluseRev = Convert.ToDouble(strPramData[5]);
            m_HomeMode = Convert.ToUInt16(strPramData[6]);
            //m_AxisDir = Convert.ToUInt16(strPramData[7]);
            m_HiSpeed = Convert.ToUInt32(strPramData[8]);
            m_LoSpeed = Convert.ToUInt32(strPramData[9]);
            m_HomeHiSpeed = Convert.ToUInt32(strPramData[10]);
            m_HomeLoSpeed = Convert.ToUInt32(strPramData[11]);
            m_dbHiAccTime = Convert.ToDouble(strPramData[12]);
            m_dbHiDesTime = Convert.ToDouble(strPramData[13]);
            m_dbLoAccTime = Convert.ToDouble(strPramData[14]);
            m_dbLoDesTime = Convert.ToDouble(strPramData[15]);
            m_dbHomeAccTime = Convert.ToDouble(strPramData[16]);
            m_dbHomeDesTime = Convert.ToDouble(strPramData[17]);
            m_dbHomeOffset = Convert.ToDouble(strPramData[18]);
            m_dbMaxPos = Convert.ToDouble(strPramData[19]);
            m_dbMinPos = Convert.ToDouble(strPramData[20]);
            m_dbPos1 = Convert.ToDouble(strPramData[21]);
            m_dbPos2 = Convert.ToDouble(strPramData[22]);
            m_DelayTime = Convert.ToUInt32(strPramData[23]);
            m_HomeTime = Convert.ToUInt32(strPramData[24]);
            m_ActionTime = Convert.ToUInt32(strPramData[25]);
            m_strUnit = strPramData[26];
            ////-------------------------------------------
            mTimer.Interval = m_DelayTime;

            return bRet;
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
                sSQL = "Select * From  Motors Where ID=" + "'" + m_strID + "'";
                ret = oDB_load.Find(sSQL, ref oRs);
                if (ret)
                {
                    retRead = oRs.Read();
                    m_CardID = Convert.ToUInt16(oRs["CardID"]);
                    m_AxisID = Convert.ToUInt16(oRs["AxisID"]);
                    m_dbUnitRev = Convert.ToDouble(oRs["UnitRev"]);
                    m_dbPluseRev = Convert.ToDouble(oRs["PulseRev"]);
                    m_HomeMode = Convert.ToUInt16(oRs["HomeMode"]);
                    m_AxisDir = Convert.ToUInt16(oRs["AxisDir"]);
                    //----------
                    m_PluseModule = Convert.ToUInt16(oRs["isPlsModule"]);
                    m_Module_InPluseMode = Convert.ToUInt16(oRs["InPlsMode"]);
                    m_Module_OutPluseMode = Convert.ToUInt16(oRs["OutPlsMode"]);
                    m_ModuleMelLogic = Convert.ToUInt16(oRs["ModuleMelLogic"]);
                    m_ModulePelLogic = Convert.ToUInt16(oRs["ModulePelLogic"]);
                    //----------
                    m_HiSpeed = Convert.ToUInt32(oRs["HiSpeed"]);
                    m_LoSpeed = Convert.ToUInt32(oRs["LoSpeed"]);
                    m_HomeHiSpeed = Convert.ToUInt32(oRs["HomeHiSpeed"]);
                    m_HomeLoSpeed = Convert.ToUInt32(oRs["HomeLoSpeed"]);
                    //----------
                    m_dbHiAccTime = Convert.ToDouble(oRs["HiAccTime"]);
                    m_dbHiDesTime = Convert.ToDouble(oRs["HiDesTime"]);
                    m_dbLoAccTime = Convert.ToDouble(oRs["LoAccTime"]);
                    m_dbLoDesTime = Convert.ToDouble(oRs["LoDesTime"]);
                    m_dbHomeAccTime = Convert.ToDouble(oRs["HomeAccTime"]);
                    m_dbHomeDesTime = Convert.ToDouble(oRs["HomeDesTime"]);
                    m_dbHomeOffset = Convert.ToDouble(oRs["HomeOffset"]);
                    m_dbMaxPos = Convert.ToDouble(oRs["PMaxPos"]);
                    m_dbMinPos = Convert.ToDouble(oRs["MMinPos"]);
                    m_dbPos1 = Convert.ToDouble(oRs["P1"]);
                    //----------
                    m_dbPos2 = Convert.ToDouble(oRs["P2"]);
                    m_DelayTime = Convert.ToUInt32(oRs["Delay"]);
                    m_HomeTime = Convert.ToUInt32(oRs["HomeTime"]);
                    m_ActionTime = Convert.ToUInt32(oRs["ActionTime"]);
                    m_strUnit = oRs["Unit"].ToString();
                    ////-------------------------------------------
                    mTimer.Interval = m_DelayTime;

                }
                else
                {
                    sSQL = ("Insert into Motors (ID,CName,EName,CardID,isPlsModule,ModulePelLogic,ModuleMelLogic,AxisID,UnitRev,PulseRev,HomeMode,AxisDir,HiSpeed,LoSpeed,HomeHiSpeed,HomeLoSpeed,HiAccTime,HiDesTime,LoAccTime,LoDesTime,HomeAccTime,HomeDesTime,HomeOffset,PMaxPos,MMinPos,P1,P2,Delay,HomeTime,ActionTime,Unit,InPlsMode,OutPlsMode) Values('" +
                                                 m_strID + "','" + m_strCName + "','" + m_strEName + "',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,mm,0,0)");
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
            return bRet;
        }




        public bool SaveMDBMachineData(string strMachinePath, ref tyMotor_Parameter tyParameter)
        {
            bool bRet = true;
            //--------------
            SaveToDataBase(strMachinePath, "Motors", "ID", m_strID, "AxisID", tyParameter.uAxisID.ToString());
            SaveToDataBase(strMachinePath, "Motors", "ID", m_strID, "UnitRev", tyParameter.dbUnitRev.ToString());
            SaveToDataBase(strMachinePath, "Motors", "ID", m_strID, "PulseRev", tyParameter.dbPluseRev.ToString());
            SaveToDataBase(strMachinePath, "Motors", "ID", m_strID, "HomeMode", tyParameter.uHomeMode.ToString());
            SaveToDataBase(strMachinePath, "Motors", "ID", m_strID, "AxisDir", tyParameter.uAxisDir.ToString());
            //--------------
            SaveToDataBase(strMachinePath, "Motors", "ID", m_strID, "isPlsModule", tyParameter.uPluseModule.ToString());
            SaveToDataBase(strMachinePath, "Motors", "ID", m_strID, "InPlsMode", tyParameter.uModule_InPluseMode.ToString());
            SaveToDataBase(strMachinePath, "Motors", "ID", m_strID, "OutPlsMode", tyParameter.uModule_OutPluseMode.ToString());
            SaveToDataBase(strMachinePath, "Motors", "ID", m_strID, "ModuleMelLogic", tyParameter.uModuleMelLogic.ToString());
            SaveToDataBase(strMachinePath, "Motors", "ID", m_strID, "ModulePelLogic", tyParameter.uModulePelLogic.ToString());
            //--------------
            SaveToDataBase(strMachinePath, "Motors", "ID", m_strID, "HiSpeed", tyParameter.HiSpeed.ToString());
            SaveToDataBase(strMachinePath, "Motors", "ID", m_strID, "HiAccTime", tyParameter.dbHiAccTime.ToString());
            SaveToDataBase(strMachinePath, "Motors", "ID", m_strID, "HiDesTime", tyParameter.dbHiDesTime.ToString());
            //-------------
            SaveToDataBase(strMachinePath, "Motors", "ID", m_strID, "HomeHiSpeed", tyParameter.HomeHiSpeed.ToString());
            SaveToDataBase(strMachinePath, "Motors", "ID", m_strID, "HomeLoSpeed", tyParameter.HomeLoSpeed.ToString());
            SaveToDataBase(strMachinePath, "Motors", "ID", m_strID, "HomeAccTime", tyParameter.dbHomeAccTime.ToString());
            SaveToDataBase(strMachinePath, "Motors", "ID", m_strID, "HomeLoSpeed", tyParameter.dbHomeDesTime.ToString());
            SaveToDataBase(strMachinePath, "Motors", "ID", m_strID, "HomeOffset", tyParameter.dbHomeOffset.ToString());
            //--------------
            SaveToDataBase(strMachinePath, "Motors", "ID", m_strID, "PMaxPos", tyParameter.dbMaxPos.ToString());
            SaveToDataBase(strMachinePath, "Motors", "ID", m_strID, "MMinPos", tyParameter.dbMinPos.ToString());
            SaveToDataBase(strMachinePath, "Motors", "ID", m_strID, "HomeTime", tyParameter.HomeTime.ToString());
            SaveToDataBase(strMachinePath, "Motors", "ID", m_strID, "ActionTime", tyParameter.ActionTime.ToString());
            SaveToDataBase(strMachinePath, "Motors", "ID", m_strID, "Unit", tyParameter.strUnit);
            //--------------


            return true;
        }

        public void SetNewParameter(ref tyMotor_Parameter tyParameter)
        {
            //-------------------
            m_AxisID = tyParameter.uAxisID;
            m_dbUnitRev = tyParameter.dbUnitRev;
            m_dbPluseRev = tyParameter.dbPluseRev;
            m_AxisDir = tyParameter.uAxisDir;
            m_PluseModule = tyParameter.uPluseModule;
            //-----------------
            m_Module_InPluseMode = tyParameter.uModule_InPluseMode;
            m_Module_OutPluseMode = tyParameter.uModule_OutPluseMode;
            m_ModuleMelLogic = tyParameter.uModuleMelLogic;
            m_ModulePelLogic = tyParameter.uModulePelLogic;
            //-----------------
            m_HiSpeed = tyParameter.HiSpeed;
            m_dbHiAccTime = tyParameter.dbHiAccTime;
            m_dbHiDesTime = tyParameter.dbHiDesTime;
            //-----------------
            m_HomeMode = tyParameter.uHomeMode;
            m_HomeHiSpeed = tyParameter.HomeHiSpeed;
            m_HomeLoSpeed = tyParameter.HomeLoSpeed;
            m_dbHomeAccTime = tyParameter.dbHomeAccTime;
            m_dbHomeDesTime = tyParameter.dbHomeDesTime;
            m_dbHomeOffset = tyParameter.dbHomeOffset;
            //-----------------
            m_dbMaxPos = tyParameter.dbMaxPos;
            m_dbMinPos = tyParameter.dbMinPos;
            m_HomeTime = tyParameter.HomeTime;
            m_ActionTime = tyParameter.ActionTime;
            m_strUnit = tyParameter.strUnit;
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

                    for (int i = 0; i < m_nMotorErrorCode.Length; i++)
                    {
                        string strData = "ErrorCode" + (i + 1).ToString();
                        m_nMotorErrorCode[i] = Convert.ToUInt16(oRs[strData]);
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
                for (int j = 0; j < m_nMotorErrorCode.Length; j++)
                {
                    m_nMotorErrorCode[j] = Convert.ToInt16(datas[j]);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("在读取ERRORCODE时出错," + e.Message);
                bRet = false;
            }
            return bRet;
        }

        public override void HomeCycle(ref double dbTime)
        {
            m_HomeStep = (enHomeStep)m_nHomeStep;
            switch (m_HomeStep)
            {
                case enHomeStep.StartHome:
                    {
                        if (isAlarm())
                            AlarmReset();
                        //-----------------
                        if (!isSevOn())
                            SetSevON(true);
                        //------------------
                        if (!isMotionDone())
                            MotorStop();

                        mTimer.Interval = 300;
                        mTimer.Start();
                        m_nHomeStep = (int)enHomeStep.DoHoming;
                    }
                    break;
                case enHomeStep.DoHoming:
                    if (mTimer.Enabled == false)
                    {
                        bool bAlarm = isAlarm();
                        bool bSevOn = isSevOn();
                        bool bMotionDone = isMotionDone();
                        if (!bAlarm && bSevOn && bMotionDone) //無異常, Servo->on  , MotionDone->OK
                        {
                            MotorDoHome();
                            mTimer.Interval = m_HomeTime;
                            mTimer.Start();
                            m_nHomeStep = (int)enHomeStep.HomeOffset;
                        }
                        else
                        {
                            Error pError = null;
                            if (bAlarm && pError == null)
                            {
                                string code = getErrorCode();
                                pError = new Error(ref this.m_NowAddress, "电机驱动器异常 伺服报警码：" + code, "", m_nMotorErrorCode[(int)enMotorErrorCode.enDriverAlarm]);
                            }
                            //-----------
                            if (!bSevOn && pError == null)
                                //pError = new Error(ref this.m_NowAddress, "电机未Servo_On", "", (int)ErrorDefine.enErrorCode.电机未Servo_On);
                                pError = new Error(ref this.m_NowAddress, "电机未Servo_On", "", m_nMotorErrorCode[(int)enMotorErrorCode.en未Servo_On]);
                            //-----------
                            if (!bMotionDone && pError == null)
                                //pError = new Error(ref this.m_NowAddress, "电机未Motion_Done", "", (int)ErrorDefine.enErrorCode.电机未Motion_Done);
                                pError = new Error(ref this.m_NowAddress, "电机未Motion_Done", "", m_nMotorErrorCode[(int)enMotorErrorCode.en未Motion_Done]);
                            //----------
                            pError.AddErrSloution("再试一次/Retry", (int)enHomeStep.StartHome);
                            pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                        }
                    }
                    break;
                case enHomeStep.HomeOffset:
                    if (isHomeCompletedForAPI()) //isMotionDone() &&
                    {
                        bool bisAlarm = isAlarm();
                        if (bisAlarm)
                        {
                            Error pError = null;
                            string code = getErrorCode();
                            pError = new Error(ref this.m_NowAddress, "电机驱动器异常 伺服报警码：" + code, "", m_nMotorErrorCode[(int)enMotorErrorCode.enDriverAlarm]);
                            pError.AddErrSloution("再试一次/Retry", (int)enHomeStep.StartHome);
                            pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                        }
                        else
                        {
                            if (m_dbHomeOffset != 0)
                                MotorRMove(m_dbHomeOffset, 30);
                            //----------------------
                            m_nHomeStep = (int)enHomeStep.HomeCompleted;
                        }
                    }
                    else
                    {
                        if (mTimer.Enabled == false)
                        {
                            MotorStop();
                            //Error pError = new Error(ref this.m_NowAddress, "电机回原点逾时", "", (int)ErrorDefine.enErrorCode.电机回Home逾时);
                            string code = getErrorCode();
                            Error pError = new Error(ref this.m_NowAddress, "电机回原点逾时 伺服报警码：" + code, "", m_nMotorErrorCode[(int)enMotorErrorCode.en回原点逾时]);
                            pError.AddErrSloution("再试一次/Retry", (int)enHomeStep.StartHome);
                            pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                        }
                    }
                    break;
                case enHomeStep.HomeCompleted:
                    if (isMotionDone())
                    {
                        m_bHomeCompleted = true;
                        SetPosPosition(0.0);
                        m_Status = 狀態.待命;
                    }
                    break;
            }
            base.HomeCycle(ref dbTime);
        }
        public override void StepCycle(ref double dbTime)
        {
            try
            {
                string strD = null, strE = null;
            int iErrorCode = 0;
            m_Step = (enStep)m_nStep;
            switch (m_Step)
            {
                //******************************************
                #region 絕對移動(AbsMove Step)
                case enStep.StartAbsMove:
                    if (isSafe(ref this.m_NowAddress, ref strD, ref strE, ref iErrorCode)) //詢問上層是否安全
                    {
                        mTimer1.Enabled = false;
                        mTimer1.Interval = 5000;
                        mTimer1.Start();
                        if (isAlarm())
                            AlarmReset();
                        if (!isSevOn())
                            SetSevON(true);
                        if (!isMotionDone())
                            MotorStop();
                        //------------------
                        bool bError = (isAlarm() || isPEL() || isMEL() || !isSevOn());
                        if (bError)
                            m_nStep = (int)enStep.ActionError;
                        else
                        {
                            m_nCheckCount = 0;
                            m_nStep = (int)enStep.AbsMoving;
                        }
                    }
                    else
                    {
                        Error pError = new Error(ref this.m_NowAddress, strD, strE, iErrorCode);
                        pError.AddErrSloution("再试一次/Retry", (int)enStep.StartAbsMove);
                        pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    }
                    break;
                case enStep.AbsMoving:
                    if (isMotionDone() || !m_bCheckMotionDone)
                    {
                        if (m_dbTargetPos < m_dbMaxPos && m_dbTargetPos > m_dbMinPos)
                        {
                            MotorAMove(m_dbTargetPos, m_dbMoveingSpeed);
                            Thread.Sleep(10);
                            mTimer.Interval = m_ActionTime;
                            mTimer.Start();
                            m_nStep = (int)enStep.AbsMoveisMotionDone;
                        }
                        else
                        {
                            //Error pError = new Error(ref this.m_NowAddress, "位置超出軟體極限", "", (int)ErrorDefine.enErrorCode.位置超出软体极限);
                            string strErrMsg = "(" + m_strID + ")" + "位置超出軟體極限";
                            Error pError = new Error(ref this.m_NowAddress, strErrMsg, "", m_nMotorErrorCode[(int)enMotorErrorCode.enOverSoftLimit]);
                            pError.AddErrSloution("移至软体极限位置/Move to Soft Limit", (int)enStep.AbsMoveToLimitPos);
                            pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                        }
                    }
                    else
                    {
                        m_nCheckCount++;
                        if (m_nCheckCount >= 5)
                        {
                            MotorStop();
                            //Error pError = new Error(ref this.m_NowAddress, "电机未Motion_Done", "", (int)ErrorDefine.enErrorCode.电机未Motion_Done);
                            string strErrMsg = "(" + m_strID + ")" + "电机未Motion_Done";
                            Error pError = new Error(ref this.m_NowAddress, strErrMsg, "", m_nMotorErrorCode[(int)enMotorErrorCode.en未Motion_Done]);
                            pError.AddErrSloution("再试一次/Retry", (int)enStep.StartAbsMove);
                            pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                        }
                    }
                    break;
                case enStep.AbsMoveToLimitPos:
                    {
                        if (m_dbTargetPos > m_dbMaxPos)
                            m_dbTargetPos = m_dbMaxPos;
                        if (m_dbTargetPos < m_dbMinPos)
                            m_dbTargetPos = m_dbMinPos;
                        //---------------------------
                        MotorAMove(m_dbTargetPos, m_dbMoveingSpeed);
                        Thread.Sleep(10);
                        mTimer.Interval = m_ActionTime;
                        mTimer.Start();
                        m_nStep = (int)enStep.AbsMoveisMotionDone;
                    }
                    break;
                case enStep.AbsMoveisMotionDone:
                    if (isMotionDone())
                    {
                        bool bError = (isAlarm() || isMELorPEL());
                        if (bError)
                            m_nStep = (int)enStep.ActionError;
                        else
                            m_nStep = (int)enStep.AbsMoveCompleted;
                    }
                    else
                    {
                        Error pError = null;
                        if (mTimer.Enabled == false)
                        {
                            MotorStop();
                            //pError = new Error(ref this.m_NowAddress, "电机Abs位置移动逾时", "", (int)ErrorDefine.enErrorCode.电机Abs位置移动逾时);
                            string strErrMsg = "(" + m_strID + ")" + "电机Abs位置移动逾时";
                            pError = new Error(ref this.m_NowAddress, strErrMsg, "", m_nMotorErrorCode[(int)enMotorErrorCode.enAbs位置移动逾时]);
                            pError.AddErrSloution("再试一次/Retry", (int)enStep.StartAbsMove);
                            pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                        }
                        //--------------------------------
                        bool bError = (isAlarm() || isPEL() || isMEL() || !isSevOn());
                        if (bError && pError == null)
                            m_nStep = (int)enStep.ActionError;
                    }
                    break;
                case enStep.AbsMoveCompleted:
                    if (isMotionDone())
                    {
                        double dbNowPos = GetPosition();
                        if (dbNowPos < m_dbTargetPos + 0.2 && dbNowPos > m_dbTargetPos - 0.2)
                        {
                            //string str11 = "TargetPos: " + m_dbTargetPos.ToString("0.000") + ",NowPos: " + dbNowPos.ToString("0.000");
                            //Sys_Define.RecordMessageLog("API_ErrorRecord", "位置记录,轴：" + m_AxisID.ToString() + str11);
                            m_Status = 狀態.待命;
                        }
                        else if (b_Retry && mTimer1.Enabled == false)
                        {
                            b_Retry = false;
                            m_nStep = (int)enStep.StartAbsMove;
                            string str11 = ",TargetPos: " + m_dbTargetPos.ToString("0.000") + ",NowPos: " + dbNowPos.ToString("0.000");
                            Sys_Define.RecordMessageLog("API_ErrorRecord", "位置异常,轴：" + m_AxisID.ToString() + str11 + ",再次运动");
                            break;
                        }
                        else
                        {
                            Error pError = null;
                            if (mTimer.Enabled == false)
                            {
                                MotorStop();
                                //pError = new Error(ref this.m_NowAddress, "电机Abs位置移动逾时", "", (int)ErrorDefine.enErrorCode.电机Abs位置移动逾时);
                                string strErrMsg = "(" + m_strID + ")" + "电机Abs位置移动逾时" +
                                                   ",TargetPos: " + m_dbTargetPos.ToString("0.000") +
                                                   ",NowPos: " + dbNowPos.ToString("0.000");
                                pError = new Error(ref this.m_NowAddress, strErrMsg, "", m_nMotorErrorCode[(int)enMotorErrorCode.enAbs位置移动逾时]);
                                pError.AddErrSloution("再试一次/Retry", (int)enStep.StartAbsMove);
                                pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                            }
                        }
                    }
                    break;
                #endregion
                //******************************************
                #region 相對移動(RevMove Step)
                case enStep.StartRevMove:
                    if (isSafe(ref this.m_NowAddress, ref strD, ref strE, ref iErrorCode)) //詢問上層是否安全
                    {
                        if (isAlarm())
                            AlarmReset();
                        if (!isSevOn())
                            SetSevON(true);
                        if (!isMotionDone())
                            MotorStop();
                        //-----------
                        bool bError = (isAlarm() || isPEL() || isMEL() || !isSevOn());
                        if (bError)
                            m_nStep = (int)enStep.ActionError;
                        else
                        {
                            m_nCheckCount = 0;
                            m_nStep = (int)enStep.RevMoving;
                        }
                    }
                    else
                    {
                        MotorStop();
                        Error pError = new Error(ref this.m_NowAddress, strD, strE, iErrorCode);
                        pError.AddErrSloution("再试一次/Retry", (int)enStep.StartRevMove);
                        pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    }
                    break;
                case enStep.RevMoving:
                    if (isMotionDone() || !m_bCheckMotionDone)
                    {
                        double dbNowPos = GetPosition();
                        double dbTagPos = dbNowPos + m_dbDist;
                        if (dbTagPos < m_dbMaxPos && dbTagPos > m_dbMinPos)
                        {
                            MotorRMove(m_dbDist, m_dbMoveingSpeed);
                            Thread.Sleep(10);
                            mTimer.Interval = m_ActionTime;
                            mTimer.Start();
                            m_nStep = (int)enStep.RevMoveCompleted;
                        }
                        else
                        {
                            //Error pError = new Error(ref this.m_NowAddress, "位置超出软体极限", "", (int)ErrorDefine.enErrorCode.位置超出软体极限);
                            string strErrMsg = "(" + m_strID + ")" + "位置超出软体极限";
                            Error pError = new Error(ref this.m_NowAddress, strErrMsg, "", m_nMotorErrorCode[(int)enMotorErrorCode.enOverSoftLimit]);
                            pError.AddErrSloution("移至软体极限位置/Move to Soft Limit", (int)enStep.RevMoveToLimitPos);
                            pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                        }
                    }
                    else
                    {
                        m_nCheckCount++;
                        if (m_nCheckCount >= 3)
                        {
                            MotorStop();
                            //Error pError = new Error(ref this.m_NowAddress, "电机未Motion_Done", "", (int)ErrorDefine.enErrorCode.电机未Motion_Done);
                            string strErrMsg = "(" + m_strID + ")" + "电机未Motion_Done";
                            Error pError = new Error(ref this.m_NowAddress, strErrMsg, "", m_nMotorErrorCode[(int)enMotorErrorCode.en未Motion_Done]);
                            pError.AddErrSloution("再试一次/Retry", (int)enStep.StartRevMove);
                            pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                        }
                    }
                    break;
                case enStep.RevMoveToLimitPos:
                    {
                        double dbTagMovePos = 0;
                        double dbNowPos = GetPosition();
                        double dbTagPos = dbNowPos + m_dbDist;
                        if (dbTagPos > m_dbMaxPos)
                            dbTagMovePos = m_dbMaxPos;
                        if (dbTagPos < m_dbMinPos)
                            dbTagMovePos = m_dbMinPos;
                        //------------------
                        MotorAMove(dbTagMovePos, m_dbMoveingSpeed);
                        Thread.Sleep(10);
                        mTimer.Interval = m_ActionTime;
                        mTimer.Start();
                        m_nStep = (int)enStep.RevMoveCompleted;
                    }
                    break;
                case enStep.RevMoveCompleted:
                    if (isMotionDone())
                    {
                        m_Status = 狀態.待命;
                    }
                    else
                    {
                        Error pError = null;
                        if (mTimer.Enabled == false)
                        {
                            MotorStop();
                            //pError = new Error(ref this.m_NowAddress, "电机Rev位置移动逾时", "", (int)ErrorDefine.enErrorCode.电机Rev位置移动逾时);
                            string strErrMsg = "(" + m_strID + ")" + "电机Rev位置移动逾时";
                            pError = new Error(ref this.m_NowAddress, strErrMsg, "", m_nMotorErrorCode[(int)enMotorErrorCode.enRev位置移动逾时]);
                            pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                        }
                        //--------------------------------
                        bool bError = (isAlarm() || isPEL() || isMEL() || !isSevOn());
                        if (bError && pError == null)
                            m_nStep = (int)enStep.ActionError;
                    }
                    break;
                #endregion
                //******************************************
                #region 反覆絕對位置移動(Repeat Step)
                case enStep.StartRepeat:
                    if (isSafe(ref this.m_NowAddress, ref strD, ref strE, ref iErrorCode)) //詢問上層是否安全
                    {
                        if (mTimer.Enabled == false)
                        {
                            if (isAlarm())
                                AlarmReset();
                            if (!isSevOn())
                                SetSevON(true);
                            if (!isMotionDone())
                                MotorStop();
                            //-----------
                            bool bError = (isAlarm() || isPEL() || isMEL() || !isSevOn());
                            if (bError)
                                m_nStep = (int)enStep.ActionError;
                            else
                            {
                                m_nCheckCount = 0;
                                m_nStep = (int)enStep.RepeatMoving;
                            }
                        }
                    }
                    else
                    {
                        MotorStop();
                        Error pError = new Error(ref this.m_NowAddress, strD, strE, iErrorCode);
                        pError.AddErrSloution("再试一次/Retry", (int)enStep.StartRepeat);
                        pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    }
                    break;
                case enStep.RepeatMoving:
                    if (isMotionDone() || !m_bCheckMotionDone)
                    {
                        if (m_dbPos1 > m_dbMaxPos)
                            m_dbPos1 = m_dbMaxPos;
                        if (m_dbPos1 < m_dbMinPos)
                            m_dbPos1 = m_dbMinPos;
                        //------------------------
                        MotorAMove(m_dbPos1, m_dbMoveingSpeed);
                        Thread.Sleep(10);
                        mTimer.Interval = m_ActionTime;
                        mTimer.Start();
                        m_nStep = (int)enStep.MoveToPos1;
                    }
                    else
                    {
                        m_nCheckCount++;
                        if (m_nCheckCount >= 3)
                        {
                            MotorStop();
                            //Error pError = new Error(ref this.m_NowAddress, "电机未Motion_Done", "", (int)ErrorDefine.enErrorCode.电机未Motion_Done);
                            string strErrMsg = "(" + m_strID + ")" + "电机未Motion_Done";
                            Error pError = new Error(ref this.m_NowAddress, strErrMsg, "", m_nMotorErrorCode[(int)enMotorErrorCode.en未Motion_Done]);
                            pError.AddErrSloution("再试一次/Retry", (int)enStep.StartRepeat);
                            pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                        }
                    }
                    break;
                case enStep.MoveToPos1:
                    if (isMotionDone()) //&& mTimer.Enabled == false)
                    {
                        mTimer.Stop();
                        mTimer.Interval = m_DelayTime;
                        mTimer.Start();
                        m_nStep = (int)enStep.WaittingTimer;
                    }
                    else
                    {
                        Error pError = null;
                        if (mTimer.Enabled == false)
                        {
                            MotorStop();
                            //pError = new Error(ref this.m_NowAddress, "电机Repeat移动逾时", "", (int)ErrorDefine.enErrorCode.电机Repeat移动逾时);
                            string strErrMsg = "(" + m_strID + ")" + "电机Repeat移动逾时";
                            pError = new Error(ref this.m_NowAddress, strErrMsg, "", m_nMotorErrorCode[(int)enMotorErrorCode.enRepeat移动逾时]);
                            pError.AddErrSloution("再试一次/Retry", (int)enStep.StartRepeat);
                            pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                        }
                        //--------------------------------
                        bool bError = (isAlarm() || isPEL() || isMEL() || !isSevOn());
                        if (bError && pError == null)
                            m_nStep = (int)enStep.ActionError;

                    }
                    break;
                case enStep.WaittingTimer:
                    if (isMotionDone() && mTimer.Enabled == false)
                    {
                        if (m_dbPos2 > m_dbMaxPos)
                            m_dbPos2 = m_dbMaxPos;
                        if (m_dbPos2 < m_dbMinPos)
                            m_dbPos2 = m_dbMinPos;
                        //--------------------------
                        MotorAMove(m_dbPos2, m_dbMoveingSpeed);
                        mTimer.Interval = m_ActionTime;
                        mTimer.Start();
                        m_nStep = (int)enStep.MoveToPos2;
                    }
                    break;
                case enStep.MoveToPos2:
                    if (isMotionDone()) //&& mTimer.Enabled == false)
                    {
                        mTimer.Stop();
                        mTimer.Interval = m_DelayTime;
                        mTimer.Start();
                        m_nStep = (int)enStep.StartRepeat;
                    }
                    else
                    {
                        Error pError = null;
                        if (mTimer.Enabled == false)
                        {
                            MotorStop();
                            //pError = new Error(ref this.m_NowAddress, "电机Repeat移动逾时", "", (int)ErrorDefine.enErrorCode.电机Repeat移动逾时);
                            string strErrMsg = "(" + m_strID + ")" + "电机Repeat移动逾时";
                            pError = new Error(ref this.m_NowAddress, strErrMsg, "", m_nMotorErrorCode[(int)enMotorErrorCode.enRepeat移动逾时]);
                            pError.AddErrSloution("再试一次/Retry", (int)enStep.StartRepeat);
                            pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                        }
                        //--------------------------------
                        bool bError = (isAlarm() || isPEL() || isMEL() || !isSevOn());
                        if (bError && pError == null)
                            m_nStep = (int)enStep.ActionError;
                    }
                    break;
                #endregion 
                //******************************************
                #region 動作時異常
                case enStep.ActionError:
                    {
                        MotorStop();
                        Error pError = null;
                        if (isAlarm() && pError == null)
                        {
                            string code = getErrorCode();
                            string strErrMsg = "(" + m_strID + ")" + "电机驱动器异常 伺服报警码：" + code;
                            pError = new Error(ref this.m_NowAddress, strErrMsg, "", m_nMotorErrorCode[(int)enMotorErrorCode.enDriverAlarm]);
                        }
                        //-----------
                        if (!isSevOn() && pError == null)
                        {

                            string strErrMsg = "(" + m_strID + ")" + "电机未Servo_On";
                            pError = new Error(ref this.m_NowAddress, strErrMsg, "", m_nMotorErrorCode[(int)enMotorErrorCode.en未Servo_On]);
                        }
                        //-----------
                        if (isPEL() && pError == null)
                        {

                            string strErrMsg = "(" + m_strID + ")" + "电机正极限_On";
                            pError = new Error(ref this.m_NowAddress, strErrMsg, "", m_nMotorErrorCode[(int)enMotorErrorCode.enPLError]);
                        }
                        //-----------
                        if (isMEL() && pError == null)
                        {

                            string strErrMsg = "(" + m_strID + ")" + "电机负极限_On";
                            pError = new Error(ref this.m_NowAddress, strErrMsg, "", m_nMotorErrorCode[(int)enMotorErrorCode.enMLError]);
                        }
                        //----------
                        SetSevON(false);
                        pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    }
                    break;
                    #endregion
                    //******************************************

            }
            }
            catch (Exception e11)
            {
                string err = e11.Message + "," + e11.StackTrace;
                Sys_Define.RecordMessageLog("API_ErrorRecord", "线程抛异常2," + err);
            }
            base.StepCycle(ref dbTime);
        }
        //-------------------------------------
        public bool DoHome()
        {
            m_bHomeCompleted = false;
            m_Mode = enMode.自動;
            //m_Status = 狀態.待命;
            m_Status = 狀態.準備動作中;
            int doStep = (int)enHomeStep.StartHome;
            if (DoHomeStep(doStep))
                return true;
            else
                return false;
        }
        public bool Repeat(double dbSpeed = 100)
        {
            m_Mode = enMode.自動;
            m_Status = 狀態.準備動作中;
            mTimer.Enabled = false;
            m_dbMoveingSpeed = dbSpeed;
            int doStep = (int)enStep.StartRepeat;
            if (DoStep(doStep))
                return true;
            else
                return false;
        }
        public bool Repeat(double dbAPos, double dbBPos, uint uDelaytime, double dbSpeed = 100)
        {
            m_Mode = enMode.自動;
            m_Status = 狀態.準備動作中;
            mTimer.Enabled = false;
            m_dbMoveingSpeed = dbSpeed;
            m_dbPos1 = dbAPos;
            m_dbPos2 = dbBPos;
            m_DelayTime = uDelaytime;
            int doStep = (int)enStep.StartRepeat;
            if (DoStep(doStep))
                return true;
            else
                return false;
        }
        public bool AbsMove(double dbPos, double dbSpeed = 100)
        {
            b_Retry = true;
            m_Mode = enMode.自動;
            m_Status = 狀態.準備動作中;
            mTimer.Enabled = false;
            m_dbTargetPos = dbPos;
            m_dbMoveingSpeed = dbSpeed;
            int doStep = (int)enStep.StartAbsMove;
            if (DoStep(doStep))
                return true;
            else
                return false;
        }
        public bool RevMove(double dbDist, double dbSpeed = 100)
        {
            m_Mode = enMode.自動;
            m_Status = 狀態.準備動作中;
            mTimer.Enabled = false;
            m_dbDist = dbDist;
            m_dbMoveingSpeed = dbSpeed;
            int doStep = (int)enStep.StartRevMove;
            if (DoStep(doStep))
                return true;
            else
                return false;
        }
        public void SetStopSevOffThenOn(bool bValue) { m_bStopSevOffThenOn = bValue; }  // m_bStopSevOffThenOn=true, 馬達若Stop需Servo off 再Servo On

        virtual public void SetStopReSetCommand(bool bValue)　//m_bStopReSetCommand=true啟動對標機制, Stop後會GetPosition然後再SetCommand , 避免暴衝問題
        {
            m_bStopReSetCommand = bValue;
        }
        //-------------------------------------
        virtual public bool SetSevON(bool bOn) { return true; }
        virtual public void SetCheckMotionDone(bool bCheck) { m_bCheckMotionDone = bCheck; }
        //------------------------------------

        virtual public bool isHomeCompleted() { return false; }
        virtual public bool isHomeCompletedForAPI() { return false; }
        virtual public bool MotorDoHome() { return true; }
        virtual public bool MotorStop() { return true; }
        virtual public bool MotorEmgStop() { return true; }
        virtual protected bool MotorAMove(double dbPos, double dbSpeed = 100)
        {
            m_dbTargetPos = dbPos;
            m_dbMoveingSpeed = dbSpeed;
            return true;
        }  //未保護的AMove
        virtual protected bool MotorRMove(double dbDist, double dbSpeed = 100)
        {
            m_dbDist = dbDist;
            m_dbMoveingSpeed = dbSpeed;
            return true;
        }  //未保護的RMove
        /// <summary>
        /// 单轴不检测运动
        /// </summary>
        /// <param name="dbDist"></param>
        /// <param name="dbSpeed"></param>
        /// <returns></returns>
        public bool MotorMove(double dbDist, double dbSpeed = 100)
        {
            return MotorRMove(m_dbDist, m_dbMoveingSpeed); ;
        }
        //------------------------------------
        public void SetHiSpeed(bool bisHiSpeed) { m_bisHiSpeed = bisHiSpeed; }
        public bool GetisHiSpeed() { return m_bisHiSpeed; }
        //------------------------------------
        virtual public bool Set齒輪比(uint n分子, uint n分母) { return true; }
        virtual public bool SetPosPosition(double dbPos)
        {
            m_dbNowPos = dbPos;
            return true;
        }
        virtual public bool SetPPSPosition(int nPPS)
        {
            m_nNowPositionPPS = nPPS;
            return true;
        }
        virtual public bool SetPPSCommand(int nPPS)
        {
            m_nNowCommandPPS = nPPS;
            return true;
        }
        virtual public bool SetPDOCommand(int nPPS)
        {
            m_nNowCommandPPS = nPPS;
            return true;
        }
        virtual public bool PositionCommandAlignment()
        {
            m_nNowPositionPPS = m_nNowCommandPPS;
            return true;
        }
        virtual public bool AlarmReset() { return true; }
        virtual public double GetPosition() { return m_dbNowPos; }
        virtual public int GetNowPositionPPS() { return m_nNowPositionPPS; }
        virtual public int GetNowCommandPPS() { return m_nNowCommandPPS; }
        virtual public double GetCommand() { return m_dbNowCommand; }
        virtual public double GetPDOCommand() { return m_dbNowPDOCommand; }
        virtual public uint GetErrorCode() { return m_ErrorCode; }
        //------------------------------------
        virtual public bool SetCompareInitial(double dbPos, int iPluseMode, int TriggerTime, int Channel, int QepSource) { return true; }
        virtual public bool SetCompareChannelSource(int Channel, int QepSource) { return true; }
        virtual public bool SetCompareChannelDirection(int Channel, int DirInverse) { return true; }
        virtual public bool SetCompareEnable(int Channel, int Enable) { return true; }
        virtual public bool SetComparePosition(int iSetPos, int Channel, int Dir, int interval, int Trigger_Cnt) { return true; }

        virtual public bool SetCompareTable(ref int iTable, int iTableSize) { return true; }
        virtual public bool ChangeRunningSpeed(int iNewSpeed) { return true; }
        virtual public bool SetMaxTorque(int iTorRate) { return true; }
        virtual public bool SetComparePolarity(int iInverse) { return true; }
        //-------------------------------------
        virtual public bool isMotionDone() { return true; }
        virtual public bool isAlarm() { return false; }
        virtual public bool isSevOn() { return false; }
        virtual public bool isPEL() { return false; }
        virtual public bool isMEL() { return false; }
        virtual public bool isMELorPEL() { return false; }
        virtual public bool isHome() { return false; }


        virtual public string getErrorCode() { return ""; }
    }
}
