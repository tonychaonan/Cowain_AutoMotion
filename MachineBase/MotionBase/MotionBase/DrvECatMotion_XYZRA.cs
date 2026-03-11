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
using Inovance.InoMotionCotrollerShop.InoServiceContract.EtherCATConfigApi;


namespace MotionBase
{
    public class DrvECatMotion_XYZRA : Base
    {
        public DrvECatMotion_XYZRA(Type homeEnum1, Type stepEnum1, string instanceName1, Base parent, int nStation,  ushort uTableNo ,String strEName, String strCName, int ErrCodeBase)
            : base(homeEnum1, stepEnum1, instanceName1,parent, nStation, strEName, strCName, ErrCodeBase)
        {
            for (int i = 0; i < m_pMotor.Length; i++)
                m_pMotor[i] = null;
            //------
            m_pSetIO = null;
            m_pSetAssistIO = null;
            //------
            int iTableNo= (int)uTableNo;
            int iSetTableNo = iTableNo - 1;
            //------
            m_uTableNO = (ushort) iSetTableNo;
        }
        //--------
        #region 參數&變數
        //************************
        public DrvMotor[] m_pMotor = new DrvMotor[5];  // X,Y,Z,R,A
        public DrvIO m_pSetIO;
        public DrvIO m_pSetAssistIO;

        public Sys_Define.enAixsType AxisType;
        public Sys_Define.tyMotionStatus m_MotionStatus = new Sys_Define.tyMotionStatus();
        //--------------------
        Sys_Define.tyAXIS_XYZR m_nowPosXYZR = new Sys_Define.tyAXIS_XYZR();
        Sys_Define.tyAXIS_XYZRA m_nowPosXYZRA = new Sys_Define.tyAXIS_XYZRA();
        //-----    
        Sys_Define.tyAXIS_XYZR m_nowPlusePosXYZR = new Sys_Define.tyAXIS_XYZR();
        Sys_Define.tyAXIS_XYZRA m_nowPlusePosXYZRA = new Sys_Define.tyAXIS_XYZRA();
        //--------------------
        Sys_Define.tyAXIS_XYZR m_BasePointXYZR = new Sys_Define.tyAXIS_XYZR();
        Sys_Define.tyAXIS_XYZRA m_BasePointXYZRA = new Sys_Define.tyAXIS_XYZRA();
        //-----
        Sys_Define.tyAXIS_XYZR m_NowPointXYZR = new Sys_Define.tyAXIS_XYZR();
        Sys_Define.tyAXIS_XYZRA m_NowPointXYZRA = new Sys_Define.tyAXIS_XYZRA();

        Sys_Define.tyAXIS_XYZRA m_NowRecordBasePointXYZRA = new Sys_Define.tyAXIS_XYZRA();
        Sys_Define.tyAXIS_XYZRA m_NowRecordPointXYZRA = new Sys_Define.tyAXIS_XYZRA();
        //--------------------
        public int m_nErrorCode = (int) (ErrorDefine.enErrorCode.MotionBuffer动做中电机驱动器异常);
        ushort m_uTableNO = 0;
        ushort m_uFiFo = 0;
        int m_iAxisCount = 0;
        //-------------------------
        Dictionary<int, Sys_Define.tyMotionBufferData_XYZRA> m_MotionBufferData = new Dictionary<int, Sys_Define.tyMotionBufferData_XYZRA>();
        #endregion
        //-------------------------
        public enum enStep
        {
            MotionBufferMove_Start = 0,
            MotionBuffer_SetBufferData,
            MotionBufferMoving,
            MotionBufferMovingCompleted,
            MotionBufferMove_CheckisAlarm,
            MotionBufferMoveCompleted,
            //------------
            MotionBufferMove_Alarm,
        }
        enStep m_Step;
        //------------------------------------
        public bool SetMotor(ref DrvMotor pXMotor, ref DrvMotor pYMotor, ref DrvMotor pZMotor)
        {
            AxisType = Sys_Define.enAixsType.en_XYZ;
            m_iAxisCount = 3;
            m_pMotor[0] = pXMotor;
            m_pMotor[1] = pYMotor;
            m_pMotor[2] = pZMotor;
            m_pMotor[3] = null;
            m_pMotor[4] = null;

            return true;
        }

        public bool SetMotor(ref DrvMotor pXMotor,ref DrvMotor pYMotor ,ref DrvMotor pZMotor,ref DrvMotor pRMotor)
        {
            AxisType = Sys_Define.enAixsType.en_XYZR;
            m_iAxisCount = 4;
            m_pMotor[0] = pXMotor;
            m_pMotor[1] = pYMotor;
            m_pMotor[2] = pZMotor;
            m_pMotor[3] = pRMotor;
            m_pMotor[4] = null;

            return true;
        }

        public bool SetMotor(ref DrvMotor pXMotor, ref DrvMotor pYMotor, ref DrvMotor pZMotor, ref DrvMotor pRMotor,ref DrvMotor pAMotor)
        {
            AxisType = Sys_Define.enAixsType.en_XYZRA;
            m_iAxisCount = 5;
            m_pMotor[0] = pXMotor;
            m_pMotor[1] = pYMotor;
            m_pMotor[2] = pZMotor;
            m_pMotor[3] = pRMotor;
            m_pMotor[4] = pAMotor;
            return true;
        }


        //----------------------------------------
        public override void Stop()
        {
            ushort rt1 = 0,rt2=0, rt3 = 0;
            ushort StopMode = 2;  //Mode:2  , 緊急停止 , 清除FIFO
            bool bBusy = true;
            //-------------------
            if(m_Status != 狀態.待命)
            {
                UInt32 m_retRtn1 = 999, m_retRtn2 = 999, m_retRtn3 = 999;
                UInt32 m_retRtn4 = 999;
                short pStatus = 999;

                m_retRtn1 = ImcApi.IMC_CrdStop(Sys_Define.m_cardHandle, (short)m_uTableNO,0);
                if (m_retRtn1 != ImcApi.EXE_SUCCESS)
                    Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI :XYZRA Stop  ,IMC_CrdStop  Fail, NG_Code: 0x" + m_retRtn1.ToString("x8") + ", CrdNo:" + m_uTableNO.ToString());
                //----------------
                if (Sys_Define.m_bCommandLogEnable)
                    Sys_Define.RecordMessageLog("Stop_ApiRecord", "dllAPI :XYZRA Stop  ,IMC_CrdStop , NG_Code: 0x" + m_retRtn1.ToString("x8") + ", CrdNo:" + m_uTableNO.ToString());


                DateTime dt_Start = DateTime.Now;

                while (bBusy)
                {
                    m_retRtn4 = ImcApi.IMC_CrdGetStatus(Sys_Define.m_cardHandle, (short)m_uTableNO, ref pStatus);
                    if (m_retRtn4 != ImcApi.EXE_SUCCESS)
                        Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI :XYZRA Stop  ,IMC_CrdGetStatus  Fail, NG_Code: 0x" + m_retRtn4.ToString("x8") + ", CrdNo:" + m_uTableNO.ToString() + ", Status:" + pStatus.ToString());
                    //-------------------
                    if (Sys_Define.m_bCommandLogEnable)
                        Sys_Define.RecordMessageLog("Stop_ApiRecord", "dllAPI :XYZRA Stop  ,IMC_CrdGetStatus  , NG_Code: 0x" + m_retRtn4.ToString("x8") + ", CrdNo:" + m_uTableNO.ToString() + ", Status:" + pStatus.ToString());

                    bool bStop = ((pStatus & 0x0500) == 0x0500) ? true : false;  //Status= 0x500，Crd->Emg Stop

                    if (pStatus == 0 || bStop)  //pStatus == 0
                    {
                        bBusy = false;
                        string strCrdStopTime= (DateTime.Now - dt_Start).TotalMilliseconds.ToString("0.0");
                        if (Sys_Define.m_bCommandLogEnable)
                            Sys_Define.RecordMessageLog("Stop_ApiRecord", "dllAPI :XYZRA Stop  , Realse Time(ms): " + strCrdStopTime);
                    }

                }


                ////*********************************
                m_retRtn2 = ImcApi.IMC_CrdClrData(Sys_Define.m_cardHandle, (short)m_uTableNO);
                if (m_retRtn2 != ImcApi.EXE_SUCCESS)
                    Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI :XYZRA Stop  ,IMC_CrdClrData  Fail, NG_Code: 0x" + m_retRtn2.ToString("x8") + ", CrdNo:" + m_uTableNO.ToString());
                //------------   
                if (Sys_Define.m_bCommandLogEnable)
                    Sys_Define.RecordMessageLog("Stop_ApiRecord", "dllAPI :XYZRA Stop  ,IMC_CrdClrData , NG_Code: 0x" + m_retRtn2.ToString("x8") + ", CrdNo:" + m_uTableNO.ToString());
                //*********************************
                m_retRtn3 = ImcApi.IMC_CrdDeleteMtSys(Sys_Define.m_cardHandle, (short)m_uTableNO);
                if (Sys_Define.m_bCommandLogEnable)
                    Sys_Define.RecordMessageLog("Stop_ApiRecord", "dllAPI :XYZRA Stop  ,IMC_CrdDeleteMtSys , NG_Code: 0x" + m_retRtn3.ToString("x8") + ", CrdNo:" + m_uTableNO.ToString());
                if (m_retRtn3 != ImcApi.EXE_SUCCESS)
                    Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : XYZRA  ,IMC_CrdDeleteMtSys  Fail, NG_Code: 0x" + m_retRtn3.ToString("x8") + ", CrdNo:" + m_uTableNO.ToString());
                //*********************************
            }
            //-------------------
            m_Status = 狀態.待命;
            base.Stop();
        }

        public bool GetMotionStatus(ref Sys_Define.tyMotionStatus MotionStatus)
        {

            UInt32 m_retRtn1 = 999, m_retRtn2 = 999, m_retRtn3 = 999;
            short pCrdDone = 0,pStatus=0;
            m_retRtn1 = ImcApi.IMC_CrdGetArrivalSts(Sys_Define.m_cardHandle, (short)m_uTableNO,ref pCrdDone);
            if (m_retRtn1 != ImcApi.EXE_SUCCESS)
                Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : GetMotionStatus  ,IMC_CrdGetArrivalSts  Fail, NG_Code: 0x" + m_retRtn1.ToString("x8"));

            m_retRtn2 = ImcApi.IMC_CrdGetStatus(Sys_Define.m_cardHandle, (short)m_uTableNO, ref pStatus);
            if (m_retRtn2 != ImcApi.EXE_SUCCESS)
                Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : GetMotionStatus  ,IMC_CrdGetStatus  Fail, NG_Code: 0x" + m_retRtn2.ToString("x8"));


            int nRetSpace = 0;
            m_retRtn3 = ImcApi.IMC_CrdGetSpace(Sys_Define.m_cardHandle, (short)m_uTableNO, ref nRetSpace);
            if (m_retRtn3 != ImcApi.EXE_SUCCESS)
                Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : GetMotionStatus  ,IMC_CrdGetSpace  Fail, NG_Code: 0x" + m_retRtn3.ToString("x8"));


            bool bCheckCrdSpace = false;
            if (Sys_Define.m_bCheckCrdSpcae)
            {
                if ((m_retRtn3 == ImcApi.EXE_SUCCESS) && (nRetSpace == 15000))
                    bCheckCrdSpace = true;
            }
            else
            {
                bCheckCrdSpace = true;
            }
            //记录轴的状态
            if (pStatus > 1)
            {
                Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : IMC_CrdGetStatus,IMC_CrdGetStatus  Fail, NG_Code:" + pStatus);
                MotionStatus.MotionStatus = Sys_Define.enMotionBufferStatus.en_Fault;
                for (int i = 0; i < m_iAxisCount; i++)
                {
                    if (m_pMotor[i].isAlarm())
                    {
                        Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : IMC_CrdGetStatus,IMC_CrdGetStatus  Fail," + (i + 1).ToString() + "轴报警");
                    }
                    if (m_pMotor[i].isPEL())
                    {
                        Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : IMC_CrdGetStatus,IMC_CrdGetStatus  Fail," + (i + 1).ToString() + "轴正极限报警");
                    }
                    if (m_pMotor[i].isMEL())
                    {
                        Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : IMC_CrdGetStatus,IMC_CrdGetStatus  Fail," + (i + 1).ToString() + "轴负极限报警");
                    }
                }
                return true;
            }
            //----------------
            if (m_retRtn1 == ImcApi.EXE_SUCCESS && m_retRtn2 == ImcApi.EXE_SUCCESS)
            {
                if (pCrdDone == 0)  //&& pStatus == 1
                    MotionStatus.MotionStatus = Sys_Define.enMotionBufferStatus.en_Action;
                if (pCrdDone == 1 &&  bCheckCrdSpace)  // && pStatus == 0
                    MotionStatus.MotionStatus = Sys_Define.enMotionBufferStatus.en_Idle;
                if ( pStatus > 1)
                    MotionStatus.MotionStatus = Sys_Define.enMotionBufferStatus.en_Fault;
                return true;
            }
            else {
                return false;
            }
        }

        private bool ClearBuffer()
        {
            ushort rt1 = 0,rt2=0;
            ushort StopMode = 2;  //Mode:2  , 緊急停止 , 清除FIFO

            UInt32 m_retRtn0=999,m_retRtn1 = 999, m_retRtn2=999, m_retRtn3 = 999, m_retRtn4 = 999;
            bool bBusy = true;
            short pStatus = 999;

            m_retRtn0 = ImcApi.IMC_CrdStop(Sys_Define.m_cardHandle, (short)m_uTableNO,0);
            if (m_retRtn0 != ImcApi.EXE_SUCCESS)
                Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : ClearBuffer  ,IMC_CrdStop  Fail, NG_Code: 0x" + m_retRtn0.ToString("x8") + ", CrdNo:" + m_uTableNO.ToString());

            DateTime dt_Start = DateTime.Now;
            while (bBusy)
            {
                m_retRtn1 = ImcApi.IMC_CrdGetStatus(Sys_Define.m_cardHandle, (short)m_uTableNO, ref pStatus);
                if (m_retRtn1 != ImcApi.EXE_SUCCESS)
                    Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI :ClearBuffer  ,IMC_CrdGetStatus  Fail, NG_Code: 0x" + m_retRtn1.ToString("x8") + ", CrdNo:" + m_uTableNO.ToString() + ", Status:" + pStatus.ToString());
                //-------------------
                bool bStop = ((pStatus & 0x0500) == 0x0500) ? true : false;  //Status= 0x500，Crd->Emg Stop

                if (pStatus == 0 || bStop)
                {
                    bBusy = false;
                    string strCrdStopTime = (DateTime.Now - dt_Start).TotalMilliseconds.ToString("0.0");
                    Sys_Define.RecordMessageLog("Crd_ReleaseTime", "dllAPI :ClearBuffer  , Realse Time(ms): " + strCrdStopTime + ", CrdNo:" + m_uTableNO.ToString());
                }
            }
            //----------------------

            m_retRtn2 = ImcApi.IMC_CrdClrData(Sys_Define.m_cardHandle, (short)m_uTableNO);
            if (m_retRtn2 != ImcApi.EXE_SUCCESS)
                Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : ClearBuffer  ,IMC_CrdClrData  Fail, NG_Code: 0x" + m_retRtn2.ToString("x8") + ", CrdNo:" + m_uTableNO.ToString());
  
            m_retRtn3 = ImcApi.IMC_CrdDeleteMtSys(Sys_Define.m_cardHandle, (short)m_uTableNO);
            if (m_retRtn3 != ImcApi.EXE_SUCCESS)
                Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : ClearBuffer  ,IMC_CrdDeleteMtSys  Fail, NG_Code: 0x" + m_retRtn4.ToString("x8") + ", CrdNo:" + m_uTableNO.ToString());
     
            //-----------------------------
            if (m_retRtn0 == ImcApi.EXE_SUCCESS && m_retRtn1 == ImcApi.EXE_SUCCESS && m_retRtn2 == ImcApi.EXE_SUCCESS &&
                m_retRtn3 == ImcApi.EXE_SUCCESS ) 
                return true;
            else
                return false;
                
        }

        private bool SetMotionStart()
        {
            ushort rt = 0;

            int iMaxVel = 0, iArcRefRadius = 0;

            //#region  取得Set_Crd_Parameter資料
            UInt32 m_retRtn1 = 999;

            m_retRtn1 = ImcApi.IMC_CrdStart(Sys_Define.m_cardHandle, (short)m_uTableNO);
            if (m_retRtn1 != ImcApi.EXE_SUCCESS)
                Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionStart  ,IMC_CrdStart  Fail, NG_Code: 0x" + m_retRtn1.ToString("x8"));


            if (m_retRtn1 == ImcApi.EXE_SUCCESS)
                return true;
            else
                return false;

        }

        public bool SetNewBufferData(ref Sys_Define.tyMotionBufferData_XYZRA[] BufferData)
        {            
            m_MotionBufferData.Clear();
            for (int i = 0; i < BufferData.Length; i++)
                m_MotionBufferData.Add(i, BufferData[i]);

            return true;
        }

        private bool SetMotionBufferData(ref Sys_Define.tyAXIS_XYZRA basePoint,double dbDx=0,double dbDy=0)
        {

            ushort rt = 0;
            int iAxisNum = m_iAxisCount; //m_pMotor.Length;
            //-----------
            string strArcMode = "";
            ushort[] ArcNodeID = new ushort[2];
            int[] ArcTargetPos = new int[2];
            int[] ArcCenterPos = new int[2];
            //-------
            ushort[] SphereArcNodeID = new ushort[3];
            int[] SphereArcTargetPos = new int[3];
            int[] SphereArcMidPos = new int[3];
            //------------
            ushort uFollowAxesNum =1; 
            ushort[] SphereArcFollowNodeID = new ushort[4];
            ushort[] SphereArcTwoFollowNodeID = new ushort[5];
            int[] OneFollowAxesTargetPosition = new int[1];
            int[] TwoFollowAxesTargetPosition = new int[2];
            //------------
            ushort[] NodeID = new ushort[iAxisNum];
            ushort[] SlotID = new ushort[iAxisNum];
            int[] OrgPos = new int[iAxisNum];
            int[] TargetPos = new int[iAxisNum];
            int[] CenterPos = new int[iAxisNum];
            //-----------
            Sys_Define.tyAXIS_XY tyCenterGap = new Sys_Define.tyAXIS_XY();
            Sys_Define.tyAXIS_XYZ tyRecordSphereMidPosition = new Sys_Define.tyAXIS_XYZ();
            DrvMotor.tyMotor_Parameter MotorParameter = new DrvMotor.tyMotor_Parameter();

            int iMaxVel = 0;
            int iCommandID = 0;
            int iCount = 0;
            tyRecordSphereMidPosition.X = tyRecordSphereMidPosition.Y = tyRecordSphereMidPosition.Z = 0;

            #region 獲取EtherCat Scan Time
            short iCycleTime = 0;
            short iHwType = 0;
            double dbScanTime = 0;

            UInt32 m_retRtn0 = ImcApi.IMC_GetHwSysPara(Sys_Define.m_cardHandle, ref iCycleTime, ref iHwType);
            if (m_retRtn0 != ImcApi.EXE_SUCCESS)
                Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_GetHwSysPara  Fail, NG_Code: 0x" + m_retRtn0.ToString("x8"));

            if (iCycleTime == 2000)
                dbScanTime = 1.0 / 500.0;
            else if (iCycleTime == 1000)
                dbScanTime = 1.0 / 1000.0;
            else if (iCycleTime == 500)
                dbScanTime = 1.0 / 2000.0;
            else if (iCycleTime == 250)
                dbScanTime = 1.0 / 4000.0;
            else if (iCycleTime == 125)
                dbScanTime = 1.0 / 8000.0;
            #endregion


            #region  取得Set_Crd_Parameter資料

            short[] shNodeID = new short[3];  //XYZ三軸
            for (int i = 0; i < 3; i++)
            {
                m_pMotor[i].GetParameter(ref MotorParameter);
                if (i == 0)
                    iMaxVel = (int)MotorParameter.HiSpeed;  //以第一軸的速度為主
                //--------------
                shNodeID[i] =   (short) MotorParameter.uAxisID;
            }

            #endregion



            //*********************************************
            DrvMotor.tyMotor_Parameter[] pMotorParameter = new DrvMotor.tyMotor_Parameter[5];
            double[] dbUnitRev = new double[5];
            double[] dbPluseRev = new double[5];

            for (int i = 0; i < pMotorParameter.Length; i++)
            {
                if (m_pMotor[i]!=null)
                {
                    pMotorParameter[i] = new DrvMotor.tyMotor_Parameter();
                    m_pMotor[i].GetParameter(ref pMotorParameter[i]);
                    dbUnitRev[i] = pMotorParameter[i].dbUnitRev;
                    dbPluseRev[i] = pMotorParameter[i].dbPluseRev;
                }        
            }



            //*********************************************
            #region Set_Crd_parameters (建立標準Offset偏差座標)

            short state = 99;
            //m_retRtn0 = ImcApi.IMC_CrdGetStatus(Sys_Define.m_cardHandle, (short)m_uTableNO, ref state);//判断坐标系状态
            //if (m_retRtn0 != ImcApi.EXE_SUCCESS)
            {
                m_retRtn0 = ImcApi.IMC_CrdSetMtSys(Sys_Define.m_cardHandle, (short)m_uTableNO, shNodeID, 5000, 5000000.0);
                if (m_retRtn0 != ImcApi.EXE_SUCCESS)
                    Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdSetMtSys  Fail, NG_Code: 0x" + m_retRtn0.ToString("x8"));

                m_retRtn0 = ImcApi.IMC_CrdSetFolVelMode(Sys_Define.m_cardHandle, (short)m_uTableNO, 1);
                if (m_retRtn0 != ImcApi.EXE_SUCCESS)
                    Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdSetFolVelMode  Fail, NG_Code: 0x" + m_retRtn0.ToString("x8"));

                m_retRtn0 = ImcApi.IMC_CrdSetSmoothParam(Sys_Define.m_cardHandle, (short)m_uTableNO, 10, 5);
                if (m_retRtn0 != ImcApi.EXE_SUCCESS)
                    Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdSetSmoothParam  Fail, NG_Code: 0x" + m_retRtn0.ToString("x8"));
            }

            ImcApi.TCrdAdvParam tCrdAdvParam = new ImcApi.TCrdAdvParam();
            tCrdAdvParam.userVelMode = 0;
            tCrdAdvParam.transMode = 0;
            tCrdAdvParam.noDataProtect = 1;
            tCrdAdvParam.noCoplaneCircOptm = 0;
            tCrdAdvParam.turnCoef = 1;
            tCrdAdvParam.circAccChangeEn = 0;
            tCrdAdvParam.tol = 5000;
            //m_retRtn0 = ImcApi.IMC_CrdGetAdvParam(Sys_Define.m_cardHandle, (short)m_uTableNO, ref tCrdAdvParam);
            //if (m_retRtn0 != ImcApi.EXE_SUCCESS)
            //    Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdGetAdvParam  Fail, NG_Code: 0x" + m_retRtn0.ToString("x8"));

            //tCrdAdvParam.tol = 5000;

            m_retRtn0 = ImcApi.IMC_CrdSetAdvParam(Sys_Define.m_cardHandle, (short)m_uTableNO, ref tCrdAdvParam);
            if (m_retRtn0 != ImcApi.EXE_SUCCESS)
                Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdSetAdvParam  Fail, NG_Code: 0x" + m_retRtn0.ToString("x8"));

            #endregion

            UInt32 m_retRtn =9999;
            //--------------------------------
            bool bNowIOStatus = m_MotionBufferData[0].bStartDelay_ActionIOStatus;
            DrvIO.tyIO_Parameter pIOParameter = new DrvIO.tyIO_Parameter();
            DrvIO.tyIO_Parameter pAssistIOParameter = new DrvIO.tyIO_Parameter();
            //-------------------------------
            double dbMaxSpeed = Convert.ToDouble(pMotorParameter[0].HiSpeed);
            //------
            int iBaseXPluse = Convert.ToInt32((basePoint.X / dbUnitRev[0]) * dbPluseRev[0]);
            int iBaseYPluse = Convert.ToInt32((basePoint.Y / dbUnitRev[1]) * dbPluseRev[1]);
            int iBaseZPluse = Convert.ToInt32((basePoint.Z / dbUnitRev[2]) * dbPluseRev[2]);
            int iBaseRPluse = -99999;
            int iBaseAPluse = -99999;
            if (m_pMotor[3] != null)
                iBaseRPluse= Convert.ToInt32((basePoint.R / dbUnitRev[3]) * dbPluseRev[3]);
            if(m_pMotor[4]!=null)
                iBaseAPluse = Convert.ToInt32((basePoint.A / dbUnitRev[4]) * dbPluseRev[4]);
            //-------------------------------         
            if (m_pSetIO != null)
                m_pSetIO.GetParameter(ref pIOParameter);
            if(m_pSetAssistIO!=null)
                m_pSetAssistIO.GetParameter(ref pAssistIOParameter);



            //Sys_Define.m_bRecordBufferCommand = true;
            //Sys_Define.m_bRecordBufferIOCommand = true;

            if (Sys_Define.m_bCommandLogEnable)
                Sys_Define.RecordMessageLog("BufferCommandRecord", "//--------------------------------------------------------------");


            for (int i = 0; i < m_MotionBufferData.Count; i++)
            {
                #region  設置StartDelay &  StartDelay I/O狀態
                if (m_MotionBufferData[i].dbStart_Delay > 0 && m_pSetIO != null)
                {
                    ushort uStartDelayIOStatus = (m_MotionBufferData[i].bStartDelay_ActionIOStatus == true) ? (ushort)1 : (ushort)0;
                    int iDoNo = (pIOParameter.uNodeID * 8) + pIOParameter.uPinID;
                    short shDoNo = (short)iDoNo;
                    short shStartDelayIOStatus = (short)uStartDelayIOStatus;
                    m_retRtn = ImcApi.IMC_CrdSetDOEx(Sys_Define.m_cardHandle, (short)m_uTableNO, shDoNo, 0, (short)shStartDelayIOStatus, iCommandID);
                    if (m_retRtn != ImcApi.EXE_SUCCESS)
                    {
                        Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdSetDOEx  Fail, NG_Code: 0x" + m_retRtn.ToString("x8") + ", CrdNo:" + m_uTableNO.ToString() + ", doNO:" + shDoNo.ToString() +
                                                                          ", doType: 0, doLevel:" + shStartDelayIOStatus.ToString());
                        //---------
                        uint uCardStatus = 0;
                        m_retRtn = ImcApi.IMC_GetCardSts(Sys_Define.m_cardHandle, ref uCardStatus);
                        Sys_Define.RecordMessageLog("API_ErrorRecord", "    dllAPI : SetMotionBufferData  ,IMC_GetCardSts , Rt_Code: 0x" + m_retRtn.ToString("x8") + " , pStatus: " + uCardStatus.ToString());
                        //---------
                        ImcApi.TRsouresNum trRsoures = new ImcApi.TRsouresNum();
                        m_retRtn = ImcApi.IMC_GetCardResource(Sys_Define.m_cardHandle, ref trRsoures);
                        Sys_Define.RecordMessageLog("API_ErrorRecord", "    dllAPI : SetMotionBufferData  ,IMC_GetCardResource , Rt_Code: 0x" + m_retRtn.ToString("x8") + " , AxNum: " + trRsoures.axNum.ToString()
                                                                             + " , DiNum: " + trRsoures.diNum.ToString() + " , DoNum: " + trRsoures.doNum.ToString());
                    }

                    if (Sys_Define.m_bCommandLogEnable)
                        Sys_Define.RecordMessageLog("BufferCommandRecord", "Rt_Code: 0x"+ m_retRtn.ToString("x8") + ", IMC_CrdSetDOEx, CrdNo:" + m_uTableNO.ToString()+", doNO:"+ shDoNo.ToString() +
                                                                           ", doType: 0, doLevel:" + shStartDelayIOStatus.ToString());

                    iCommandID++;
                    //------------------
                    if (m_pSetAssistIO != null)
                    {
                        iDoNo = (pAssistIOParameter.uNodeID * 8) + pAssistIOParameter.uPinID;
                        shDoNo = (short)iDoNo;
                        shStartDelayIOStatus = (short)uStartDelayIOStatus;
                        m_retRtn = ImcApi.IMC_CrdSetDOEx(Sys_Define.m_cardHandle, (short)m_uTableNO, shDoNo, 0, (short)shStartDelayIOStatus, iCommandID);
                        if (m_retRtn != ImcApi.EXE_SUCCESS)
                        {
                            Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdSetDOEx  Fail, NG_Code: 0x" + m_retRtn.ToString("x8") + ", CrdNo:" + m_uTableNO.ToString() + ", doNO:" + shDoNo.ToString() +
                                                  ", doType: 0, doLevel:" + shStartDelayIOStatus.ToString());
                            //---------
                            uint uCardStatus = 0;
                            m_retRtn = ImcApi.IMC_GetCardSts(Sys_Define.m_cardHandle, ref uCardStatus);
                            Sys_Define.RecordMessageLog("API_ErrorRecord", "    dllAPI : SetMotionBufferData  ,IMC_GetCardSts , Rt_Code: 0x" + m_retRtn.ToString("x8") + " , pStatus: " + uCardStatus.ToString());
                            //---------
                            ImcApi.TRsouresNum trRsoures = new ImcApi.TRsouresNum();
                            m_retRtn = ImcApi.IMC_GetCardResource(Sys_Define.m_cardHandle, ref trRsoures);
                            Sys_Define.RecordMessageLog("API_ErrorRecord", "    dllAPI : SetMotionBufferData  ,IMC_GetCardResource , Rt_Code: 0x" + m_retRtn.ToString("x8") + " , AxNum: " + trRsoures.axNum.ToString()
                                                                                 + " , DiNum: " + trRsoures.diNum.ToString() + " , DoNum: " + trRsoures.doNum.ToString());
                        }

                        if (Sys_Define.m_bCommandLogEnable)
                            Sys_Define.RecordMessageLog("BufferCommandRecord", "Rt_Code: 0x" + m_retRtn.ToString("x8") + ",IMC_CrdSetDOEx, CrdNo:" + m_uTableNO.ToString() + ", doNO:" + shDoNo.ToString() +
                                                                                            ", doType: 0, doLevel:" + shStartDelayIOStatus.ToString());

                        iCommandID++; 
                    }
                    //----------
                    double dbWaitPeriod = (m_MotionBufferData[i].dbStart_Delay) * (1.0 / dbScanTime);  //dbScanTime= 0.001-> 1ms , 0.0005->500us
                    int iWaitPeriod = (int)dbWaitPeriod;
                    m_retRtn=ImcApi.IMC_CrdWaitTime(Sys_Define.m_cardHandle, (short)m_uTableNO, iWaitPeriod, iCommandID);
                    if (m_retRtn != ImcApi.EXE_SUCCESS)
                        Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdWaitTime  Fail, NG_Code: 0x" + m_retRtn.ToString("x8"));

                    if (Sys_Define.m_bCommandLogEnable)
                        Sys_Define.RecordMessageLog("BufferCommandRecord", "Rt_Code: 0x" + m_retRtn.ToString("x8") + ",IMC_CrdWaitTime, CrdNo:" + m_uTableNO.ToString() + ", WaitPeriod:" + iWaitPeriod.ToString());

                    iCommandID++;
                }
                #endregion
                //********************************
                #region Set I/O On/Off狀態

                if (m_pSetIO != null)
                {

                    ushort uSetValue = (m_MotionBufferData[i].bActionIOStatus == true) ? (ushort)1 : (ushort)0;
                    int iDoNo = (pIOParameter.uNodeID * 8) + pIOParameter.uPinID;
                    short shDoNo = (short)iDoNo;
                    short shSetValue = (short)uSetValue;
                    m_retRtn=ImcApi.IMC_CrdSetDOEx(Sys_Define.m_cardHandle, (short)m_uTableNO, shDoNo, 0, shSetValue, iCommandID);
                    if (m_retRtn != ImcApi.EXE_SUCCESS)
                    {
                        Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdSetDOEx  Fail, NG_Code: 0x" + m_retRtn.ToString("x8") + ", CrdNo:" + m_uTableNO.ToString() + ", doNO:" + shDoNo.ToString() +
                                                                        ", doType: 0, doLevel:" + shSetValue.ToString());
                        //---------
                        uint uCardStatus = 0;
                        m_retRtn = ImcApi.IMC_GetCardSts(Sys_Define.m_cardHandle, ref uCardStatus);
                        Sys_Define.RecordMessageLog("API_ErrorRecord", "    dllAPI : SetMotionBufferData  ,IMC_GetCardSts , Rt_Code: 0x" + m_retRtn.ToString("x8") + " , pStatus: " + uCardStatus.ToString());
                        //---------
                        ImcApi.TRsouresNum trRsoures = new ImcApi.TRsouresNum();
                        m_retRtn = ImcApi.IMC_GetCardResource(Sys_Define.m_cardHandle, ref trRsoures);
                        Sys_Define.RecordMessageLog("API_ErrorRecord", "    dllAPI : SetMotionBufferData  ,IMC_GetCardResource , Rt_Code: 0x" + m_retRtn.ToString("x8") + " , AxNum: " + trRsoures.axNum.ToString()
                                                                             + " , DiNum: " + trRsoures.diNum.ToString() + " , DoNum: " + trRsoures.doNum.ToString());

                    }

                    if (Sys_Define.m_bCommandLogEnable)
                        Sys_Define.RecordMessageLog("BufferCommandRecord", "Rt_Code: 0x" + m_retRtn.ToString("x8") + ",IMC_CrdSetDOEx, CrdNo:" + m_uTableNO.ToString() + ", doNO:" + shDoNo.ToString() +
                                                                                        ", doType: 0, doLevel:" + shSetValue.ToString());

                    iCommandID++;
                    //------------------
                    if (m_pSetAssistIO != null)
                    {
                        iDoNo = (pAssistIOParameter.uNodeID * 8) + pAssistIOParameter.uPinID;
                        shDoNo = (short)iDoNo;
                        shSetValue = (short)uSetValue;
                        m_retRtn= ImcApi.IMC_CrdSetDOEx(Sys_Define.m_cardHandle, (short)m_uTableNO, shDoNo, 0, shSetValue, iCommandID);
                        if (m_retRtn != ImcApi.EXE_SUCCESS)
                        {
                            Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdSetDOEx  Fail, NG_Code: 0x" + m_retRtn.ToString("x8") + ", CrdNo:" + m_uTableNO.ToString() + ", doNO:" + shDoNo.ToString() +
                                                                            ", doType: 0, doLevel:" + shSetValue.ToString());
                            //---------
                            uint uCardStatus = 0;
                            m_retRtn = ImcApi.IMC_GetCardSts(Sys_Define.m_cardHandle, ref uCardStatus);
                            Sys_Define.RecordMessageLog("API_ErrorRecord", "    dllAPI : SetMotionBufferData  ,IMC_GetCardSts , Rt_Code: 0x" + m_retRtn.ToString("x8") + " , pStatus: " + uCardStatus.ToString());
                            //---------
                            ImcApi.TRsouresNum trRsoures = new ImcApi.TRsouresNum();
                            m_retRtn = ImcApi.IMC_GetCardResource(Sys_Define.m_cardHandle, ref trRsoures);
                            Sys_Define.RecordMessageLog("API_ErrorRecord", "    dllAPI : SetMotionBufferData  ,IMC_GetCardResource , Rt_Code: 0x" + m_retRtn.ToString("x8") + " , AxNum: " + trRsoures.axNum.ToString()
                                                                                 + " , DiNum: " + trRsoures.diNum.ToString() + " , DoNum: " + trRsoures.doNum.ToString());
                        }

                        if (Sys_Define.m_bCommandLogEnable)
                            Sys_Define.RecordMessageLog("BufferCommandRecord", "Rt_Code: 0x" + m_retRtn.ToString("x8") + ",IMC_CrdSetDOEx, CrdNo:" + m_uTableNO.ToString() + ", doNO:" + shDoNo.ToString() +
                                                                                            ", doType: 0, doLevel:" + shSetValue.ToString());

                        iCommandID++;
                    }
                }

                #endregion

                //********************************
                #region 設置MotionBuffer移動內容

                if (i == 0)
                {
                    m_NowPointXYZRA = basePoint;
                    m_NowRecordPointXYZRA = basePoint;
                }

                TargetPos[0] = iBaseXPluse + Convert.ToInt32((m_MotionBufferData[i].tyTargetPosition.X / dbUnitRev[0]) * dbPluseRev[0]);
                TargetPos[1] = iBaseYPluse + Convert.ToInt32((m_MotionBufferData[i].tyTargetPosition.Y / dbUnitRev[1]) * dbPluseRev[1]);
                TargetPos[2] = iBaseZPluse + Convert.ToInt32((m_MotionBufferData[i].tyTargetPosition.Z / dbUnitRev[2]) * dbPluseRev[2]);

                //計算R軸&A軸的移動PPS
                #region 計算R軸&A軸的移動PPS
                if (AxisType == Sys_Define.enAixsType.en_XYZR || AxisType == Sys_Define.enAixsType.en_XYZRA)
                {
                    double dbTargetR = basePoint.R + m_MotionBufferData[i].tyTargetPosition.R;
                    double dbNowR = m_NowRecordPointXYZRA.R;
                    double dbDR = dbTargetR - dbNowR;

                    TargetPos[3] =  Convert.ToInt32((dbDR / dbUnitRev[3]) * dbPluseRev[3]);
                    m_NowRecordPointXYZRA.R= basePoint.R + m_MotionBufferData[i].tyTargetPosition.R;
                }


                if (AxisType == Sys_Define.enAixsType.en_XYZRA)
                {
                    double dbTargetA = basePoint.A + m_MotionBufferData[i].tyTargetPosition.A;
                    double dbNowA = m_NowRecordPointXYZRA.A;
                    double dbDA = dbTargetA - dbNowA;

                    TargetPos[4] = Convert.ToInt32((dbDA / dbUnitRev[4]) * dbPluseRev[4]);
                    m_NowRecordPointXYZRA.A = basePoint.A + m_MotionBufferData[i].tyTargetPosition.A;
                }
                #endregion


                int iTargeVel = Convert.ToInt32((m_MotionBufferData[i].SpeedRate / 100) * dbMaxSpeed);
                int iEndVel = 0; //預設為0;
                int iAccVel = Convert.ToInt32((m_MotionBufferData[i].AccSpeedRate / 100) * dbMaxSpeed);
                //--------
                if (m_MotionBufferData[i].MotionType == Sys_Define.enMotionType.en_Line)
                {
                    double dbRGap = 0, dbAGap = 0;

                    double dbXGap = Math.Abs((basePoint.X + m_MotionBufferData[i].tyTargetPosition.X) - m_NowPointXYZRA.X);
                    double dbYGap = Math.Abs((basePoint.Y + m_MotionBufferData[i].tyTargetPosition.Y) - m_NowPointXYZRA.Y);
                    double dbZGap = Math.Abs((basePoint.Z + m_MotionBufferData[i].tyTargetPosition.Z) - m_NowPointXYZRA.Z);
                    if (AxisType == Sys_Define.enAixsType.en_XYZR || AxisType == Sys_Define.enAixsType.en_XYZRA)
                         dbRGap = Math.Abs((basePoint.R + m_MotionBufferData[i].tyTargetPosition.R) - m_NowPointXYZRA.R);
                    if (AxisType == Sys_Define.enAixsType.en_XYZRA)
                         dbAGap = Math.Abs((basePoint.A + m_MotionBufferData[i].tyTargetPosition.A) - m_NowPointXYZRA.A);
                    //--------------------   
                    double dbZAddPPS = 0;
                    if (i != 0)
                    {
                        if (m_MotionBufferData[i - 1].MotionType == Sys_Define.enMotionType.en_Line)
                        {
                            #region 判斷,若反轉則Z加入 5PPS ; 來達避免停頓

                            if (m_MotionBufferData[i - 1].tyTargetPosition.X >= 0)
                            {
                                if (m_MotionBufferData[i].tyTargetPosition.X < m_MotionBufferData[i - 1].tyTargetPosition.X && dbZAddPPS==0)
                                    dbZAddPPS = 5.0;
                            }
                            if (m_MotionBufferData[i - 1].tyTargetPosition.X < 0)
                            {
                                if (m_MotionBufferData[i].tyTargetPosition.X > m_MotionBufferData[i - 1].tyTargetPosition.X && dbZAddPPS == 0)
                                    dbZAddPPS = 5.0;
                            }
                            //--------------------
                            if (m_MotionBufferData[i - 1].tyTargetPosition.Y >= 0)
                            {
                                if (m_MotionBufferData[i].tyTargetPosition.Y < m_MotionBufferData[i - 1].tyTargetPosition.Y && dbZAddPPS == 0)
                                    dbZAddPPS = 5.0;
                            }

                            if (m_MotionBufferData[i - 1].tyTargetPosition.Y < 0)
                            {
                                if (m_MotionBufferData[i].tyTargetPosition.Y > m_MotionBufferData[i - 1].tyTargetPosition.Y && dbZAddPPS == 0)
                                    dbZAddPPS = 5.0;
                            }
                            //--------------------
                            if (m_MotionBufferData[i - 1].tyTargetPosition.Z >= 0)
                            {
                                if (m_MotionBufferData[i].tyTargetPosition.Z < m_MotionBufferData[i - 1].tyTargetPosition.Z && dbZAddPPS == 0)
                                    dbZAddPPS = 5.0;
                            }

                            if (m_MotionBufferData[i - 1].tyTargetPosition.Z < 0)
                            {
                                if (m_MotionBufferData[i].tyTargetPosition.Z > m_MotionBufferData[i - 1].tyTargetPosition.Z && dbZAddPPS == 0)
                                    dbZAddPPS = 5.0;
                            }
                            #endregion
                        }
                    }

                    //--------------------       
                    double[] dbXYZTargetPos = new double[3];
                    dbXYZTargetPos[0] = Convert.ToDouble( TargetPos[0]);
                    dbXYZTargetPos[1] = Convert.ToDouble(TargetPos[1]);
                    dbXYZTargetPos[2] = Convert.ToDouble(TargetPos[2]) + dbZAddPPS;

                    //m_retRtn = ImcApi.IMC_CrdSetTrajVel(Sys_Define.m_cardHandle, (short)m_uTableNO, iTargeVel);
                    //m_retRtn = ImcApi.IMC_CrdSetTrajAcc(Sys_Define.m_cardHandle, (short)m_uTableNO, iAccVel);
                    //-------------
                    if (dbXGap > 0 || dbYGap > 0 || dbZGap > 0)
                    {
                        if (dbRGap > 0 || dbAGap > 0)
                        {
                            #region R&A Gap >0

                            //double dbMaxVel = (double)iTargeVel;
                            //double dbNewMaxVel = (double)iTargeVel;
                            ////----------
                            //double dbRAMaxGap = (dbRGap > dbAGap) ? dbRGap : dbAGap;
                            //if (dbRAMaxGap >= 10)
                            //    dbNewMaxVel = dbMaxVel / (dbRAMaxGap / 10.0);
                            ////----------
                            //m_retRtn = ImcApi.IMC_CrdSetTrajVel(Sys_Define.m_cardHandle, (short)m_uTableNO, dbNewMaxVel);
                            //m_retRtn = ImcApi.IMC_CrdSetTrajAcc(Sys_Define.m_cardHandle, (short)m_uTableNO, iAccVel);
                            //-----------------------

                            //double dbRMaxVel = (dbPluseRev[3] * 1000) / 60.0 ;// p/r * r/min /60s/min
                            //double dbAMaxVel = (dbPluseRev[4] * 1000) / 60.0;
                            double dbRMaxVel = (double)pMotorParameter[3].HiSpeed;
                            double dbAMaxVel = (double)pMotorParameter[4].HiSpeed;

                            //-----------
                            double dbRGapPPS = 0, dbAGapPPS = 0;
                            double dbXGapPPS= ((dbXGap / dbUnitRev[0]) * dbPluseRev[0]);
                            double dbYGapPPS = ((dbYGap / dbUnitRev[1]) * dbPluseRev[1]);
                            double dbZGapPPS = ((dbZGap / dbUnitRev[2]) * dbPluseRev[2]);
                            if (dbRGap > 0)
                                dbRGapPPS = ((dbRGap / dbUnitRev[3]) * dbPluseRev[3]);
                            if (dbAGap > 0)
                                dbAGapPPS = ((dbAGap / dbUnitRev[4]) * dbPluseRev[4]);

                            double dbXYPosition = Math.Sqrt(Math.Pow(dbXGapPPS, 2.0) + Math.Pow(dbYGapPPS, 2.0) + Math.Pow(dbZGapPPS, 2.0));
                            double V1 = CalcSyncVel(dbXYPosition, iTargeVel, iAccVel, dbAGapPPS, dbAMaxVel);
                            double V2 = CalcSyncVel(dbXYPosition, iTargeVel, iAccVel, dbRGapPPS, dbRMaxVel);

                            double dNewVel = Math.Min(V1, V2);
                            // int iNewVel = CalSpeed(dbXGapPPS, dbYGapPPS, dbZGapPPS, dbRGapPPS, dbAGapPPS, iTargeVel,iAccVel, dbRAMaxVel,(int)dbPluseRev[3],(int)dbPluseRev[4]);

                            m_retRtn = ImcApi.IMC_CrdSetTrajVel(Sys_Define.m_cardHandle, (short)m_uTableNO, dNewVel);
                            if (m_retRtn != ImcApi.EXE_SUCCESS)
                                Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdSetTrajVel  Fail, NG_Code: 0x" + m_retRtn.ToString("x8"));
                            if (Sys_Define.m_bCommandLogEnable)
                                Sys_Define.RecordMessageLog("BufferCommandRecord", "Rt_Code: 0x" + m_retRtn.ToString("x8") + ",IMC_CrdSetTrajVel, CrdNo:" + m_uTableNO.ToString() + ", tgtVel:" + dNewVel.ToString());

                            m_retRtn = ImcApi.IMC_CrdSetTrajAcc(Sys_Define.m_cardHandle, (short)m_uTableNO, (double)iAccVel);
                            if (m_retRtn != ImcApi.EXE_SUCCESS)
                                Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdSetTrajAcc  Fail, NG_Code: 0x" + m_retRtn.ToString("x8"));
                            if (Sys_Define.m_bCommandLogEnable)
                                Sys_Define.RecordMessageLog("BufferCommandRecord", "Rt_Code: 0x" + m_retRtn.ToString("x8") + ",IMC_CrdSetTrajAcc, CrdNo:" + m_uTableNO.ToString() + ", tgtAcc:" + iAccVel.ToString());

                            #endregion
                        }
                        else
                        {
                            #region R Gap=0  or  A Gap =0  

                            m_retRtn = ImcApi.IMC_CrdSetTrajVel(Sys_Define.m_cardHandle, (short)m_uTableNO, iTargeVel);
                            if (m_retRtn != ImcApi.EXE_SUCCESS)
                                Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdSetTrajVel  Fail, NG_Code: 0x" + m_retRtn.ToString("x8"));
                            if (Sys_Define.m_bCommandLogEnable)
                                Sys_Define.RecordMessageLog("BufferCommandRecord", "Rt_Code: 0x" + m_retRtn.ToString("x8") + ",IMC_CrdSetTrajVel, CrdNo:" + m_uTableNO.ToString() + ", tgtVel:" + iTargeVel.ToString());


                            m_retRtn = ImcApi.IMC_CrdSetTrajAcc(Sys_Define.m_cardHandle, (short)m_uTableNO, iAccVel);
                            if (m_retRtn != ImcApi.EXE_SUCCESS)
                                Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdSetTrajAcc  Fail, NG_Code: 0x" + m_retRtn.ToString("x8"));
                            if (Sys_Define.m_bCommandLogEnable)
                                Sys_Define.RecordMessageLog("BufferCommandRecord", "Rt_Code: 0x" + m_retRtn.ToString("x8") + ",IMC_CrdSetTrajAcc, CrdNo:" + m_uTableNO.ToString() + ", tgtAcc:" + iAccVel.ToString());

                            #endregion

                        }
                        //-------------


                        if (AxisType == Sys_Define.enAixsType.en_XYZR || AxisType == Sys_Define.enAixsType.en_XYZRA)
                        {
                            short axisNo = (short)pMotorParameter[3].uAxisID;
                            if (dbRGap > 0)
                            {
                                m_retRtn = ImcApi.IMC_CrdSyncMove(Sys_Define.m_cardHandle, (short)m_uTableNO, axisNo, TargetPos[3], iCommandID);
                                if (m_retRtn != ImcApi.EXE_SUCCESS)
                                    Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdSyncMove  Fail, NG_Code: 0x" + m_retRtn.ToString("x8"));
                                if (Sys_Define.m_bCommandLogEnable)
                                    Sys_Define.RecordMessageLog("BufferCommandRecord", "Rt_Code: 0x" + m_retRtn.ToString("x8") + ",IMC_CrdSyncMove, CrdNo:" + m_uTableNO.ToString() + ", axNo:" + axisNo.ToString()
                                                                                        + ", SyncPos: " + TargetPos[3].ToString() );


                                iCommandID++;
                            }
                        }
                        if (AxisType == Sys_Define.enAixsType.en_XYZRA)
                        {
                            short axisNo = (short)pMotorParameter[4].uAxisID;
                            if (dbAGap > 0)
                            {
                                m_retRtn = ImcApi.IMC_CrdSyncMove(Sys_Define.m_cardHandle, (short)m_uTableNO, axisNo, TargetPos[4], iCommandID);
                                if (m_retRtn != ImcApi.EXE_SUCCESS)
                                    Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdSyncMove  Fail, NG_Code: 0x" + m_retRtn.ToString("x8"));

                                if (Sys_Define.m_bCommandLogEnable)
                                    Sys_Define.RecordMessageLog("BufferCommandRecord", "Rt_Code: 0x" + m_retRtn.ToString("x8") + ",IMC_CrdSyncMove, CrdNo:" + m_uTableNO.ToString() + ", axNo:" + axisNo.ToString()
                                                                                        + ", SyncPos: " + TargetPos[4].ToString());


                                iCommandID++;
                            }
                        }
                        //----------------------
                        m_retRtn = ImcApi.IMC_CrdLineXYZEx(Sys_Define.m_cardHandle, (short)m_uTableNO, dbXYZTargetPos, iCommandID);
                        if (m_retRtn != ImcApi.EXE_SUCCESS)
                            Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdLineXYZEx  Fail, NG_Code: 0x" + m_retRtn.ToString("x8"));

                        if (Sys_Define.m_bCommandLogEnable)
                            Sys_Define.RecordMessageLog("BufferCommandRecord", "Rt_Code: 0x" + m_retRtn.ToString("x8") + ",IMC_CrdLineXYZEx, CrdNo:" + m_uTableNO.ToString() + ", XPos:" + dbXYZTargetPos[0].ToString()
                                                                                + ", YPos:" + dbXYZTargetPos[1].ToString() + ", ZPos:" + dbXYZTargetPos[2].ToString());


                        iCommandID++;
                    }
                    else
                    {
                        if (AxisType == Sys_Define.enAixsType.en_XYZR)
                        {
                            short[] axisNo = new short[2];
                            double[] dbTargetR = new double[2];
                            axisNo[0]=(short)pMotorParameter[3].uAxisID;
                            axisNo[1] = 30;
                            dbTargetR[0] = (double)TargetPos[3];
                            dbTargetR[1] = 0;
                            if (dbRGap > 0)
                            {
                                m_retRtn = ImcApi.IMC_CrdMultiSyncMove(Sys_Define.m_cardHandle, (short)m_uTableNO,2, axisNo, dbTargetR, iTargeVel, iAccVel,  1, iCommandID);
                                if (m_retRtn != ImcApi.EXE_SUCCESS)
                                    Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdMultiSyncMove  Fail, NG_Code: 0x" + m_retRtn.ToString("x8"));


                                if (Sys_Define.m_bCommandLogEnable)
                                    Sys_Define.RecordMessageLog("BufferCommandRecord", "Rt_Code: 0x" + m_retRtn.ToString("x8") + ",IMC_CrdMultiSyncMove, CrdNo:" + m_uTableNO.ToString() + ", RPos:" + dbTargetR[0].ToString()
                                                                                        + ", APos:" + dbTargetR[1].ToString() + ", tgtVel:" + iTargeVel.ToString() + ", tgtAcc:" + iAccVel.ToString());

                                iCommandID++;
                            }
                        }
                        //--------------
                        if (AxisType == Sys_Define.enAixsType.en_XYZRA)
                        {
                            short[] axisNoArray = new short[2];
                            double[] dbMutiSyncMoveTarget = new double[2];
                            axisNoArray[0]= (short)pMotorParameter[3].uAxisID;
                            axisNoArray[1] = (short)pMotorParameter[4].uAxisID;
                            dbMutiSyncMoveTarget[0]= (double)TargetPos[3];
                            dbMutiSyncMoveTarget[1] = (double)TargetPos[4];
                            //-----
                            if (dbRGap > 0 || dbAGap > 0)
                            {
                                m_retRtn = ImcApi.IMC_CrdMultiSyncMove(Sys_Define.m_cardHandle, (short)m_uTableNO, 2,axisNoArray,  dbMutiSyncMoveTarget, iTargeVel, iAccVel, 1, iCommandID);
                                if (m_retRtn != ImcApi.EXE_SUCCESS)
                                    Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdMultiSyncMove  Fail, NG_Code: 0x" + m_retRtn.ToString("x8"));

                                if (Sys_Define.m_bCommandLogEnable)
                                    Sys_Define.RecordMessageLog("BufferCommandRecord", "Rt_Code: 0x" + m_retRtn.ToString("x8") + ",IMC_CrdMultiSyncMove, CrdNo:" + m_uTableNO.ToString() + ", RPos:" + dbMutiSyncMoveTarget[0].ToString()
                                                                                        + ", APos:" + dbMutiSyncMoveTarget[1].ToString() + ", tgtVel:" + iTargeVel.ToString() + ", tgtAcc:" + iAccVel.ToString());

                                iCommandID++;
                            }
                        }
                    }
                }
                else {
                    #region  Mode: 3點畫弧模式

                    Sys_Define.tyAXIS_XY tyMidPosition, tyTargetPosition, tyNowPoint, tyCenterPosition;
                    Sys_Define.tyAXIS_XYZ tySphereMidPosition, tySphereTargetPosition;
                    Sys_Define.tyAXIS_XYZR tySphereFollowMidPosition, tySphereFollowTargetPosition;
                    Sys_Define.tyAXIS_XYZRA  tySphereTwoFollowTargetPosition;
                    //--------------------------
                    ushort uCircleDir = 0;
                    bool bis2DArc = false;

                    if (m_MotionBufferData[i].CircleMode == Sys_Define.enMotionCircleMode.en_XY)
                    {
                        #region  Mode: XY畫弧點位換算
                        strArcMode = "XY";
                        //------
                        tyNowPoint.X = m_NowPointXYZRA.X;
                        tyNowPoint.Y = m_NowPointXYZRA.Y;
                        tyMidPosition.X = basePoint.X + m_MotionBufferData[i].tyMidPosition.X;
                        tyMidPosition.Y = basePoint.Y + m_MotionBufferData[i].tyMidPosition.Y;
                        tyTargetPosition.X = basePoint.X + m_MotionBufferData[i].tyTargetPosition.X;
                        tyTargetPosition.Y = basePoint.Y + m_MotionBufferData[i].tyTargetPosition.Y;
                        //------
                        double[] dbArcCenterPos = new double[2];
                        double[] dbArcTargetPos = new double[2];
                        tyCenterPosition = CalCenter(ref tyNowPoint, ref tyMidPosition, ref tyTargetPosition);
                        uCircleDir = CalCircleDir(ref tyNowPoint, ref tyMidPosition, ref tyTargetPosition);
                        tyCenterGap.X = tyCenterPosition.X - tyNowPoint.X;
                        tyCenterGap.Y = tyCenterPosition.Y - tyNowPoint.Y;

                        short shCircleDir = 0;
                        if (uCircleDir == 0)
                            shCircleDir = -1;
                        else if (uCircleDir == 1)
                            shCircleDir = 1;               
                        //------------
                        dbArcCenterPos[0]= ((tyCenterGap.X / dbUnitRev[0]) * dbPluseRev[0]);
                        dbArcCenterPos[1] = ((tyCenterGap.Y / dbUnitRev[1]) * dbPluseRev[1]);
                        //------------
                        dbArcTargetPos[0]= ((tyTargetPosition.X / dbUnitRev[0]) * dbPluseRev[0]);
                        dbArcTargetPos[1] = ((tyTargetPosition.Y / dbUnitRev[1]) * dbPluseRev[1]);
                        //----------------------
                        if(shCircleDir==1 || shCircleDir==-1)
                        {
                            m_retRtn=ImcApi.IMC_CrdSetTrajVel(Sys_Define.m_cardHandle, (short)m_uTableNO, iTargeVel);
                            if (m_retRtn != ImcApi.EXE_SUCCESS)
                                Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdSetTrajVel  Fail, NG_Code: 0x" + m_retRtn.ToString("x8"));

                            if (Sys_Define.m_bCommandLogEnable)
                                Sys_Define.RecordMessageLog("BufferCommandRecord", "Rt_Code: 0x" + m_retRtn.ToString("x8") + ",IMC_CrdSetTrajVel, CrdNo:" + m_uTableNO.ToString() + ", tgtVel:" + iTargeVel.ToString());


                            m_retRtn = ImcApi.IMC_CrdSetTrajAcc(Sys_Define.m_cardHandle, (short)m_uTableNO, iAccVel);
                            if (m_retRtn != ImcApi.EXE_SUCCESS)
                                Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdSetTrajAcc  Fail, NG_Code: 0x" + m_retRtn.ToString("x8"));

                            if (Sys_Define.m_bCommandLogEnable)
                                Sys_Define.RecordMessageLog("BufferCommandRecord", "Rt_Code: 0x" + m_retRtn.ToString("x8") + ",IMC_CrdSetTrajAcc, CrdNo:" + m_uTableNO.ToString() + ", tgtAcc:" + iAccVel.ToString());

                            //----------------------
                            m_retRtn = ImcApi.IMC_CrdArcCenterXYPlane(Sys_Define.m_cardHandle, (short)m_uTableNO, dbArcCenterPos, dbArcTargetPos, shCircleDir, 0, 0, iCommandID);
                            if (m_retRtn != ImcApi.EXE_SUCCESS)
                                Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdArcCenterXYPlane  Fail, NG_Code: 0x" + m_retRtn.ToString("x8"));

                            if (Sys_Define.m_bCommandLogEnable)
                                Sys_Define.RecordMessageLog("BufferCommandRecord", "Rt_Code: 0x" + m_retRtn.ToString("x8") + ",IMC_CrdArcCenterXYPlane, CrdNo:" + m_uTableNO.ToString() + ", Center[0]:" + dbArcCenterPos[0].ToString()+ ", Center[1]:" + dbArcCenterPos[1].ToString()
                                                                                    + ", End[0]:" + dbArcTargetPos[0].ToString() + ", End[1]:" + dbArcTargetPos[1].ToString() + ", dir:" + shCircleDir.ToString());

                            iCommandID++;
                        }
   
                        #endregion
                    }
                    else if (m_MotionBufferData[i].CircleMode == Sys_Define.enMotionCircleMode.en_XZ)
                    {
                        #region  Mode: XZ畫弧點位換算

                        strArcMode = "XZ";
                        //------
                        tyNowPoint.X = m_NowPointXYZRA.X;
                        tyNowPoint.Y = m_NowPointXYZRA.Z;
                        tyMidPosition.X = basePoint.X + m_MotionBufferData[i].tyMidPosition.X;
                        tyMidPosition.Y = basePoint.Z + m_MotionBufferData[i].tyMidPosition.Z;
                        tyTargetPosition.X = basePoint.X + m_MotionBufferData[i].tyTargetPosition.X;
                        tyTargetPosition.Y = basePoint.Z + m_MotionBufferData[i].tyTargetPosition.Z;
                        //------
                        double[] dbArcCenterPos = new double[2];
                        double[] dbArcTargetPos = new double[2];
                        tyCenterPosition = CalCenter(ref tyNowPoint, ref tyMidPosition, ref tyTargetPosition);
                        uCircleDir = CalCircleDir(ref tyNowPoint, ref tyMidPosition, ref tyTargetPosition);
                        tyCenterGap.X = tyCenterPosition.X - tyNowPoint.X;
                        tyCenterGap.Y = tyCenterPosition.Y - tyNowPoint.Y;

                        short shCircleDir = 0;
                        if (uCircleDir == 0)
                            shCircleDir = 1;
                        else if (uCircleDir == 1)
                            shCircleDir = -1;
                        //----------
                        dbArcCenterPos[0] = ((tyCenterGap.Y / dbUnitRev[2]) * dbPluseRev[2]);  
                        dbArcCenterPos[1] = ((tyCenterGap.X / dbUnitRev[0]) * dbPluseRev[0]);
                        dbArcTargetPos[0] = ((tyTargetPosition.Y / dbUnitRev[2]) * dbPluseRev[2]);
                        dbArcTargetPos[1] = ((tyTargetPosition.X / dbUnitRev[0]) * dbPluseRev[0]); 
                        //----------
                        if (shCircleDir == 1 || shCircleDir == -1)
                        {
                            m_retRtn= ImcApi.IMC_CrdSetTrajVel(Sys_Define.m_cardHandle, (short)m_uTableNO, iTargeVel);
                            if (m_retRtn != ImcApi.EXE_SUCCESS)
                                Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdSetTrajVel  Fail, NG_Code: 0x" + m_retRtn.ToString("x8"));

                            if (Sys_Define.m_bCommandLogEnable)
                                Sys_Define.RecordMessageLog("BufferCommandRecord", "Rt_Code: 0x" + m_retRtn.ToString("x8") + ",IMC_CrdSetTrajVel, CrdNo:" + m_uTableNO.ToString() + ", tgtVel:" + iTargeVel.ToString());

                            m_retRtn = ImcApi.IMC_CrdSetTrajAcc(Sys_Define.m_cardHandle, (short)m_uTableNO, iAccVel);
                            if (m_retRtn != ImcApi.EXE_SUCCESS)
                                Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdSetTrajAcc  Fail, NG_Code: 0x" + m_retRtn.ToString("x8"));

                            //----------------------
                            m_retRtn=ImcApi.IMC_CrdArcCenterZXPlane(Sys_Define.m_cardHandle, (short)m_uTableNO, dbArcCenterPos, dbArcTargetPos, (short)shCircleDir, 0, 0, iCommandID);
                            if (m_retRtn != ImcApi.EXE_SUCCESS)
                                Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdArcCenterZXPlane  Fail, NG_Code: 0x" + m_retRtn.ToString("x8"));

                            iCommandID++;
                        }
                        #endregion
                    }
                    else if (m_MotionBufferData[i].CircleMode == Sys_Define.enMotionCircleMode.en_YZ)
                    {
                        #region  Mode: YZ畫弧點位換算

                        strArcMode = "YZ";
                        //-------
                        tyNowPoint.X = m_NowPointXYZRA.Y;
                        tyNowPoint.Y = m_NowPointXYZRA.Z;
                        tyMidPosition.X = basePoint.Y + m_MotionBufferData[i].tyMidPosition.Y;
                        tyMidPosition.Y = basePoint.Z + m_MotionBufferData[i].tyMidPosition.Z;
                        tyTargetPosition.X = basePoint.Y + m_MotionBufferData[i].tyTargetPosition.Y;
                        tyTargetPosition.Y = basePoint.Z + m_MotionBufferData[i].tyTargetPosition.Z;
                        //------
                        double[] dbArcCenterPos = new double[2];
                        double[] dbArcTargetPos = new double[2];
                        tyCenterPosition = CalCenter(ref tyNowPoint, ref tyMidPosition, ref tyTargetPosition);
                        uCircleDir = CalCircleDir(ref tyNowPoint, ref tyMidPosition, ref tyTargetPosition);
                        tyCenterGap.X = tyCenterPosition.X - tyNowPoint.X;
                        tyCenterGap.Y = tyCenterPosition.Y - tyNowPoint.Y;
                        //------
                        short shCircleDir = 0;
                        if (uCircleDir == 0)
                            shCircleDir = -1;
                        else if (uCircleDir == 1)
                            shCircleDir = 1;
                        //--------------
                        dbArcCenterPos[0] = ((tyCenterGap.X / dbUnitRev[1]) * dbPluseRev[1]);
                        dbArcCenterPos[1] = ((tyCenterGap.Y / dbUnitRev[2]) * dbPluseRev[2]);
                        dbArcTargetPos[0] = ((tyTargetPosition.X / dbUnitRev[1]) * dbPluseRev[1]);
                        dbArcTargetPos[1] = ((tyTargetPosition.Y / dbUnitRev[2]) * dbPluseRev[2]);
                        //--------------
                        if (shCircleDir == 1 || shCircleDir == -1)
                        {
                            m_retRtn = ImcApi.IMC_CrdSetTrajVel(Sys_Define.m_cardHandle, (short)m_uTableNO, iTargeVel);
                            if (m_retRtn != ImcApi.EXE_SUCCESS)
                                Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdSetTrajVel  Fail, NG_Code: 0x" + m_retRtn.ToString("x8"));

                            if (Sys_Define.m_bCommandLogEnable)
                                Sys_Define.RecordMessageLog("BufferCommandRecord", "Rt_Code: 0x" + m_retRtn.ToString("x8") + ",IMC_CrdSetTrajVel, CrdNo:" + m_uTableNO.ToString() + ", tgtVel:" + iTargeVel.ToString());


                            m_retRtn = ImcApi.IMC_CrdSetTrajAcc(Sys_Define.m_cardHandle, (short)m_uTableNO, iAccVel);
                            if (m_retRtn != ImcApi.EXE_SUCCESS)
                                Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdSetTrajAcc  Fail, NG_Code: 0x" + m_retRtn.ToString("x8"));

                            //----------------------
                            m_retRtn=ImcApi.IMC_CrdArcCenterYZPlane(Sys_Define.m_cardHandle, (short)m_uTableNO, dbArcCenterPos, dbArcTargetPos, (short)uCircleDir, 0, 0, iCommandID);
                            if (m_retRtn != ImcApi.EXE_SUCCESS)
                                Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdArcCenterYZPlane  Fail, NG_Code: 0x" + m_retRtn.ToString("x8"));

                            iCommandID++;
                        }
                        #endregion
                    }
                    else if (m_MotionBufferData[i].CircleMode == Sys_Define.enMotionCircleMode.en_XYZ)
                    {
                        #region  Mode: XYZ畫弧點位換算

                        strArcMode = "XYZ";
                        double[] dbSphereArcMidPos = new double[3];
                        double[] dbSphereArcTargetPos = new double[3];
                        //-------
                        tySphereMidPosition.X = basePoint.X + m_MotionBufferData[i].tyMidPosition.X;
                        tySphereMidPosition.Y = basePoint.Y + m_MotionBufferData[i].tyMidPosition.Y;
                        tySphereMidPosition.Z = basePoint.Z + m_MotionBufferData[i].tyMidPosition.Z;
                        //-------
                        double dbSphereMidZPos = tySphereMidPosition.Z;
                        //-------
                        if (i != 0)
                        {
                            if (m_MotionBufferData[i - 1].MotionType != Sys_Define.enMotionType.en_Line)
                            {
                                #region 判斷,若反轉則Z加入 5PPS ; 來達避免停頓
                                if (m_MotionBufferData[i - 1].CircleMode == Sys_Define.enMotionCircleMode.en_XYZ)
                                {
                                    if (tySphereMidPosition.X == tyRecordSphereMidPosition.X &&
                                        tySphereMidPosition.Y == tyRecordSphereMidPosition.Y &&
                                        tySphereMidPosition.Z == tyRecordSphereMidPosition.Z )
                                    {
                                        dbSphereMidZPos = dbSphereMidZPos + 0.005;
                                    }
                                }
                                #endregion
                            }
                        }
                        //-------
                        tyRecordSphereMidPosition.X = tySphereMidPosition.X;
                        tyRecordSphereMidPosition.Y = tySphereMidPosition.Y;
                        tyRecordSphereMidPosition.Z = tySphereMidPosition.Z;
                        //-------
                        tySphereTargetPosition.X = basePoint.X + m_MotionBufferData[i].tyTargetPosition.X;
                        tySphereTargetPosition.Y = basePoint.Y + m_MotionBufferData[i].tyTargetPosition.Y;
                        tySphereTargetPosition.Z = basePoint.Z + m_MotionBufferData[i].tyTargetPosition.Z;
                        //-------
                        dbSphereArcMidPos[0] = ((tySphereMidPosition.X / dbUnitRev[0]) * dbPluseRev[0]);
                        dbSphereArcMidPos[1] = ((tySphereMidPosition.Y / dbUnitRev[1]) * dbPluseRev[1]);
                        dbSphereArcMidPos[2] = ((dbSphereMidZPos / dbUnitRev[2]) * dbPluseRev[2]);
                        //---------------
                        dbSphereArcTargetPos[0] = ((tySphereTargetPosition.X / dbUnitRev[0]) * dbPluseRev[0]);
                        dbSphereArcTargetPos[1] = ((tySphereTargetPosition.Y / dbUnitRev[1]) * dbPluseRev[1]);
                        dbSphereArcTargetPos[2] = ((tySphereTargetPosition.Z / dbUnitRev[2]) * dbPluseRev[2]);

                        //----------
                        m_retRtn = ImcApi.IMC_CrdSetTrajVel(Sys_Define.m_cardHandle, (short)m_uTableNO, iTargeVel);
                        if (m_retRtn != ImcApi.EXE_SUCCESS)
                            Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdSetTrajVel  Fail, NG_Code: 0x" + m_retRtn.ToString("x8"));

                        if (Sys_Define.m_bCommandLogEnable)
                            Sys_Define.RecordMessageLog("BufferCommandRecord", "Rt_Code: 0x" + m_retRtn.ToString("x8") + ",IMC_CrdSetTrajVel, CrdNo:" + m_uTableNO.ToString() + ", tgtVel:" + iTargeVel.ToString());





                        m_retRtn = ImcApi.IMC_CrdSetTrajAcc(Sys_Define.m_cardHandle, (short)m_uTableNO, iAccVel);
                        if (m_retRtn != ImcApi.EXE_SUCCESS)
                            Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdSetTrajAcc  Fail, NG_Code: 0x" + m_retRtn.ToString("x8"));

                        //----------------------
                        m_retRtn = ImcApi.IMC_CrdArcThreePoint(Sys_Define.m_cardHandle, (short)m_uTableNO, dbSphereArcMidPos, dbSphereArcTargetPos,  iCommandID);
                        if (m_retRtn != ImcApi.EXE_SUCCESS)
                            Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdArcThreePoint  Fail, NG_Code: 0x" + m_retRtn.ToString("x8"));

                        iCommandID++;

                        #endregion
                    }
                    else if (m_MotionBufferData[i].CircleMode == Sys_Define.enMotionCircleMode.en_XYZR)
                    {

                        #region  Mode: XYZR畫弧點位換算

                        strArcMode = "XYZR";
                        double[] dbSphereArcMidPos = new double[3];
                        double[] dbSphereArcTargetPos = new double[3];
                        //-------
                        tySphereFollowMidPosition.X = basePoint.X + m_MotionBufferData[i].tyMidPosition.X;
                        tySphereFollowMidPosition.Y = basePoint.Y + m_MotionBufferData[i].tyMidPosition.Y;
                        tySphereFollowMidPosition.Z = basePoint.Z + m_MotionBufferData[i].tyMidPosition.Z;
                        //-------
                        m_retRtn=ImcApi.IMC_CrdSetTrajVel(Sys_Define.m_cardHandle, (short)m_uTableNO, iTargeVel);
                        if (m_retRtn != ImcApi.EXE_SUCCESS)
                            Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdSetTrajVel  Fail, NG_Code: 0x" + m_retRtn.ToString("x8"));

                        if (Sys_Define.m_bCommandLogEnable)
                            Sys_Define.RecordMessageLog("BufferCommandRecord", "Rt_Code: 0x" + m_retRtn.ToString("x8") + ",IMC_CrdSetTrajVel, CrdNo:" + m_uTableNO.ToString() + ", tgtVel:" + iTargeVel.ToString());



                        m_retRtn = ImcApi.IMC_CrdSetTrajAcc(Sys_Define.m_cardHandle, (short)m_uTableNO, iAccVel);
                        if (m_retRtn != ImcApi.EXE_SUCCESS)
                            Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdSetTrajAcc  Fail, NG_Code: 0x" + m_retRtn.ToString("x8"));
                        //----------------------
                        if (AxisType == Sys_Define.enAixsType.en_XYZR || AxisType == Sys_Define.enAixsType.en_XYZRA)
                        {
                            short axisNo = (short)pMotorParameter[3].uAxisID;
                            m_retRtn=ImcApi.IMC_CrdSyncMove(Sys_Define.m_cardHandle, (short)m_uTableNO, axisNo, TargetPos[3], iCommandID);
                            if (m_retRtn != ImcApi.EXE_SUCCESS)
                                Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdSyncMove  Fail, NG_Code: 0x" + m_retRtn.ToString("x8"));

                            iCommandID++;
                        }

                        //-------
                        double dbSphereMidZPos = tySphereFollowMidPosition.Z;
                        //-------
                        if (i != 0)
                        {
                            if (m_MotionBufferData[i - 1].MotionType != Sys_Define.enMotionType.en_Line)
                            {
                                #region 判斷,若反轉則Z加入 5PPS ; 來達避免停頓
                                if (m_MotionBufferData[i - 1].CircleMode == Sys_Define.enMotionCircleMode.en_XYZR)
                                {
                                    if (tySphereFollowMidPosition.X == tyRecordSphereMidPosition.X &&
                                        tySphereFollowMidPosition.Y == tyRecordSphereMidPosition.Y &&
                                        tySphereFollowMidPosition.Z == tyRecordSphereMidPosition.Z)
                                    {
                                        dbSphereMidZPos = dbSphereMidZPos + 0.005;
                                    }
                                }
                                #endregion
                            }
                        }
                        //-------
                        tyRecordSphereMidPosition.X = tySphereFollowMidPosition.X;
                        tyRecordSphereMidPosition.Y = tySphereFollowMidPosition.Y;
                        tyRecordSphereMidPosition.Z = tySphereFollowMidPosition.Z;
                        //-------
                        tySphereFollowTargetPosition.X = basePoint.X + m_MotionBufferData[i].tyTargetPosition.X;
                        tySphereFollowTargetPosition.Y = basePoint.Y + m_MotionBufferData[i].tyTargetPosition.Y;
                        tySphereFollowTargetPosition.Z = basePoint.Z + m_MotionBufferData[i].tyTargetPosition.Z;
                        tySphereFollowTargetPosition.R = basePoint.R + m_MotionBufferData[i].tyTargetPosition.R;
                        //-------
                        dbSphereArcMidPos[0] = Convert.ToInt32((tySphereFollowMidPosition.X / dbUnitRev[0]) * dbPluseRev[0]);
                        dbSphereArcMidPos[1] = Convert.ToInt32((tySphereFollowMidPosition.Y / dbUnitRev[1]) * dbPluseRev[1]);
                        dbSphereArcMidPos[2] = Convert.ToInt32((dbSphereMidZPos / dbUnitRev[2]) * dbPluseRev[2]);
                        //---------------
                        dbSphereArcTargetPos[0] = Convert.ToInt32((tySphereFollowTargetPosition.X / dbUnitRev[0]) * dbPluseRev[0]);
                        dbSphereArcTargetPos[1] = Convert.ToInt32((tySphereFollowTargetPosition.Y / dbUnitRev[1]) * dbPluseRev[1]);
                        dbSphereArcTargetPos[2] = Convert.ToInt32((tySphereFollowTargetPosition.Z / dbUnitRev[2]) * dbPluseRev[2]);
                        //---------------
                        m_retRtn= ImcApi.IMC_CrdArcThreePoint(Sys_Define.m_cardHandle, (short)m_uTableNO, dbSphereArcMidPos, dbSphereArcTargetPos, iCommandID);
                        if (m_retRtn != ImcApi.EXE_SUCCESS)
                            Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdArcThreePoint  Fail, NG_Code: 0x" + m_retRtn.ToString("x8"));

                        iCommandID++;

                        #endregion
                    }
                    else if (m_MotionBufferData[i].CircleMode == Sys_Define.enMotionCircleMode.en_XYZRA)
                    {
                        #region  Mode: XYZRA畫弧點位換算

                        strArcMode = "XYZRA";
                        double[] dbSphereArcMidPos = new double[3];
                        double[] dbSphereArcTargetPos = new double[3];
                        //-------
                        m_retRtn= ImcApi.IMC_CrdSetTrajVel(Sys_Define.m_cardHandle, (short)m_uTableNO, iTargeVel);
                        if (m_retRtn != ImcApi.EXE_SUCCESS)
                            Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdSetTrajVel  Fail, NG_Code: 0x" + m_retRtn.ToString("x8"));

                        if (Sys_Define.m_bCommandLogEnable)
                            Sys_Define.RecordMessageLog("BufferCommandRecord", "Rt_Code: 0x" + m_retRtn.ToString("x8") + ",IMC_CrdSetTrajVel, CrdNo:" + m_uTableNO.ToString() + ", tgtVel:" + iTargeVel.ToString());



                        m_retRtn = ImcApi.IMC_CrdSetTrajAcc(Sys_Define.m_cardHandle, (short)m_uTableNO, iAccVel);
                        if (m_retRtn != ImcApi.EXE_SUCCESS)
                            Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdSetTrajAcc  Fail, NG_Code: 0x" + m_retRtn.ToString("x8"));

                        //----------------------
                        if (AxisType == Sys_Define.enAixsType.en_XYZR || AxisType == Sys_Define.enAixsType.en_XYZRA)
                        {
                            short axisNo = (short)pMotorParameter[3].uAxisID;
                            m_retRtn=ImcApi.IMC_CrdSyncMove(Sys_Define.m_cardHandle, (short)m_uTableNO, axisNo, TargetPos[3], iCommandID);
                            if (m_retRtn != ImcApi.EXE_SUCCESS)
                                Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdSyncMove  Fail, NG_Code: 0x" + m_retRtn.ToString("x8"));

                            iCommandID++;
                        }
                        if (AxisType == Sys_Define.enAixsType.en_XYZRA)
                        {
                            short axisNo = (short)pMotorParameter[4].uAxisID;
                            m_retRtn=ImcApi.IMC_CrdSyncMove(Sys_Define.m_cardHandle, (short)m_uTableNO, axisNo, TargetPos[4], iCommandID);
                            if (m_retRtn != ImcApi.EXE_SUCCESS)
                                Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdSyncMove  Fail, NG_Code: 0x" + m_retRtn.ToString("x8"));

                            iCommandID++;
                        }
                        //---------------
                        tySphereFollowMidPosition.X = basePoint.X + m_MotionBufferData[i].tyMidPosition.X;
                        tySphereFollowMidPosition.Y = basePoint.Y + m_MotionBufferData[i].tyMidPosition.Y;
                        tySphereFollowMidPosition.Z = basePoint.Z + m_MotionBufferData[i].tyMidPosition.Z;
                        //-------
                        tySphereTwoFollowTargetPosition.X = basePoint.X + m_MotionBufferData[i].tyTargetPosition.X;
                        tySphereTwoFollowTargetPosition.Y = basePoint.Y + m_MotionBufferData[i].tyTargetPosition.Y;
                        tySphereTwoFollowTargetPosition.Z = basePoint.Z + m_MotionBufferData[i].tyTargetPosition.Z;
                        //-------
                        dbSphereArcMidPos[0] = ((tySphereFollowMidPosition.X / dbUnitRev[0]) * dbPluseRev[0]);
                        dbSphereArcMidPos[1] = ((tySphereFollowMidPosition.Y / dbUnitRev[1]) * dbPluseRev[1]);
                        dbSphereArcMidPos[2] =((tySphereFollowMidPosition.Z / dbUnitRev[2]) * dbPluseRev[2]);
                        //---------------
                        dbSphereArcTargetPos[0] = ((tySphereTwoFollowTargetPosition.X / dbUnitRev[0]) * dbPluseRev[0]);
                        dbSphereArcTargetPos[1] = ((tySphereTwoFollowTargetPosition.Y / dbUnitRev[1]) * dbPluseRev[1]);
                        dbSphereArcTargetPos[2] = ((tySphereTwoFollowTargetPosition.Z / dbUnitRev[2]) * dbPluseRev[2]);
                        //---------------
                        m_retRtn= ImcApi.IMC_CrdArcThreePoint(Sys_Define.m_cardHandle, (short)m_uTableNO, dbSphereArcMidPos, dbSphereArcTargetPos, iCommandID);
                        if (m_retRtn != ImcApi.EXE_SUCCESS)
                            Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdArcThreePoint  Fail, NG_Code: 0x" + m_retRtn.ToString("x8"));

                        iCommandID++;

                        #endregion
                    }

                    #endregion
                }

                //------------
                m_NowPointXYZRA.X = basePoint.X + m_MotionBufferData[i].tyTargetPosition.X;
                m_NowPointXYZRA.Y = basePoint.Y + m_MotionBufferData[i].tyTargetPosition.Y;
                m_NowPointXYZRA.Z = basePoint.Z + m_MotionBufferData[i].tyTargetPosition.Z;
                m_NowPointXYZRA.R = basePoint.R + m_MotionBufferData[i].tyTargetPosition.R;
                m_NowPointXYZRA.A = basePoint.A + m_MotionBufferData[i].tyTargetPosition.A;
                #endregion
                //********************************
                #region End設置Delay &  EndDelay I/O狀態
                if (m_MotionBufferData[i].dbEnd_Delay > 0 && m_pSetIO != null)
                {

                    ushort uEndDelayIOStatus = (m_MotionBufferData[i].bEndDelay_ActionIOStatus == true) ? (ushort)1 : (ushort)0;
                    int iDoNo = (pIOParameter.uNodeID * 8) + pIOParameter.uPinID;
                    short shDoNo = (short)iDoNo;
                    short shEndDelayIOStatus = (short)uEndDelayIOStatus;
                    m_retRtn=ImcApi.IMC_CrdSetDOEx(Sys_Define.m_cardHandle, (short)m_uTableNO, shDoNo, 0, (short)shEndDelayIOStatus, iCommandID);
                    if (m_retRtn != ImcApi.EXE_SUCCESS)
                    {
                        Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdSetDOEx  Fail, NG_Code: 0x" + m_retRtn.ToString("x8") + ", CrdNo:" + m_uTableNO.ToString() + ", doNO:" + shDoNo.ToString() +
                                                 ", doType: 0, doLevel:" + shEndDelayIOStatus.ToString());
                        //---------
                        uint uCardStatus = 0;
                        m_retRtn = ImcApi.IMC_GetCardSts(Sys_Define.m_cardHandle, ref uCardStatus);
                        Sys_Define.RecordMessageLog("API_ErrorRecord", "    dllAPI : SetMotionBufferData  ,IMC_GetCardSts , Rt_Code: 0x" + m_retRtn.ToString("x8") + " , pStatus: " + uCardStatus.ToString());
                        //---------
                        ImcApi.TRsouresNum trRsoures = new ImcApi.TRsouresNum();
                        m_retRtn = ImcApi.IMC_GetCardResource(Sys_Define.m_cardHandle, ref trRsoures);
                        Sys_Define.RecordMessageLog("API_ErrorRecord", "    dllAPI : SetMotionBufferData  ,IMC_GetCardResource , Rt_Code: 0x" + m_retRtn.ToString("x8") + " , AxNum: " + trRsoures.axNum.ToString()
                                                                             + " , DiNum: " + trRsoures.diNum.ToString() + " , DoNum: " + trRsoures.doNum.ToString());
                    }

                    if (Sys_Define.m_bCommandLogEnable)
                        Sys_Define.RecordMessageLog("BufferCommandRecord", "Rt_Code: 0x" + m_retRtn.ToString("x8") + ", IMC_CrdSetDOEx, CrdNo:" + m_uTableNO.ToString() + ", doNO:" + shDoNo.ToString() +
                                                                           ", doType: 0, doLevel:" + shEndDelayIOStatus.ToString());


                    iCommandID++;
                    //------------------
                    if (m_pSetAssistIO != null)
                    {
                        iDoNo = (pAssistIOParameter.uNodeID * 8) + pAssistIOParameter.uPinID;
                        shDoNo = (short)iDoNo;
                        shEndDelayIOStatus = (short)uEndDelayIOStatus;
                        m_retRtn=ImcApi.IMC_CrdSetDOEx(Sys_Define.m_cardHandle, (short)m_uTableNO, shDoNo, 0, (short)shEndDelayIOStatus, iCommandID);
                        if (m_retRtn != ImcApi.EXE_SUCCESS)
                        {
                            Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdSetDOEx  Fail, NG_Code: 0x" + m_retRtn.ToString("x8") + ", CrdNo:" + m_uTableNO.ToString() + ", doNO:" + shDoNo.ToString() +
                                                                           ", doType: 0, doLevel:" + shEndDelayIOStatus.ToString());
                            //---------
                            uint uCardStatus = 0;
                            m_retRtn = ImcApi.IMC_GetCardSts(Sys_Define.m_cardHandle, ref uCardStatus);
                            Sys_Define.RecordMessageLog("API_ErrorRecord", "    dllAPI : SetMotionBufferData  ,IMC_GetCardSts , Rt_Code: 0x" + m_retRtn.ToString("x8") + " , pStatus: " + uCardStatus.ToString());
                            //---------
                            ImcApi.TRsouresNum trRsoures = new ImcApi.TRsouresNum();
                            m_retRtn = ImcApi.IMC_GetCardResource(Sys_Define.m_cardHandle, ref trRsoures);
                            Sys_Define.RecordMessageLog("API_ErrorRecord", "    dllAPI : SetMotionBufferData  ,IMC_GetCardResource , Rt_Code: 0x" + m_retRtn.ToString("x8") + " , AxNum: " + trRsoures.axNum.ToString()
                                                                                 + " , DiNum: " + trRsoures.diNum.ToString() + " , DoNum: " + trRsoures.doNum.ToString());

                        }

                        if (Sys_Define.m_bCommandLogEnable)
                            Sys_Define.RecordMessageLog("BufferCommandRecord", "Rt_Code: 0x" + m_retRtn.ToString("x8") + ", IMC_CrdSetDOEx, CrdNo:" + m_uTableNO.ToString() + ", doNO:" + shDoNo.ToString() +
                                                                               ", doType: 0, doLevel:" + shEndDelayIOStatus.ToString());

                        iCommandID++;
                    }
                    //----------
                    double dbWaitPeriod = (m_MotionBufferData[i].dbEnd_Delay) * (1.0 / dbScanTime);  //dbScanTime= 0.001-> 1ms , 0.0005->500us
                    int iWaitPeriod = (int)dbWaitPeriod;
                    m_retRtn=ImcApi.IMC_CrdWaitTime(Sys_Define.m_cardHandle, (short)m_uTableNO, iWaitPeriod, iCommandID);
                    if (m_retRtn != ImcApi.EXE_SUCCESS)
                        Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdWaitTime  Fail, NG_Code: 0x" + m_retRtn.ToString("x8"));

                    if (Sys_Define.m_bCommandLogEnable)
                        Sys_Define.RecordMessageLog("BufferCommandRecord", "Rt_Code: 0x" + m_retRtn.ToString("x8") + ",IMC_CrdWaitTime, CrdNo:" + m_uTableNO.ToString() + ", WaitPeriod:" + iWaitPeriod.ToString());


                    iCommandID++;
                }
                #endregion
            }
            //************************************
            short isFinish = 0;
            short IsSeg = 0;
            UInt32 m_retRtn1 = 999;
            while (isFinish == 0)
            {
                m_retRtn1 = ImcApi.IMC_CrdEndDataEx(Sys_Define.m_cardHandle, (short)m_uTableNO, ref isFinish, ref IsSeg);
                if (m_retRtn1 != ImcApi.EXE_SUCCESS)
                    Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : SetMotionBufferData  ,IMC_CrdEndDataEx  Fail, NG_Code: 0x" + m_retRtn1.ToString("x8"));

                if (Sys_Define.m_bCommandLogEnable)
                    Sys_Define.RecordMessageLog("BufferCommandRecord", "Rt_Code: 0x" + m_retRtn1.ToString("x8") + ",IMC_CrdEndDataEx , CrdNo:" + m_uTableNO.ToString() + ", isFinish:" + isFinish.ToString());


                if (m_retRtn1 != ImcApi.EXE_SUCCESS)
                    return false;
                Thread.Sleep(5);
            }

            //Sys_Define.m_bRecordBufferCommand = false;
            //Sys_Define.m_bRecordBufferIOCommand = false;
            
            return true;
        }

        public override void StepCycle(ref double dbTime)
        {
            string strD = null, strE = null;
            int iErrorCode=0;
            m_Step = (enStep)m_nStep;
            switch (m_Step)
            {
                ////******************************************
                #region MotinBuffer
                case enStep.MotionBufferMove_Start:
                    if (isSafe(ref this.m_NowAddress, ref  strD, ref  strE, ref iErrorCode)) //詢問上層是否安全
                    {
                        for (int i = 0; i < m_iAxisCount; i++)
                        {                           
                            if (m_pMotor[i].isAlarm())
                                m_pMotor[i].AlarmReset();
                            if (!m_pMotor[i].isSevOn())
                                m_pMotor[i].SetSevON(true);
                            if (!m_pMotor[i].isMotionDone())
                                m_pMotor[i].MotorStop();
                        }
                        //--------------------------------------------------------------
                        //ClearBuffer();
                        //--------------------------------------------------------------
                        m_nStep = (int)enStep.MotionBuffer_SetBufferData;
                    }else{
                        Error pError = new Error(ref this.m_NowAddress, strD, strE, iErrorCode);
                        pError.AddErrSloution("再試一次/Retry", (int)enStep.MotionBufferMove_Start);
                        pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    }
                    break;
                case enStep.MotionBuffer_SetBufferData:
                    {
                        int iCount = 0;
                        for (int i = 0; i < m_iAxisCount; i++)
                        {
                            if (m_pMotor[i].isMotionDone())
                                iCount++;
                        }
                        //----------------------
                        if (iCount == m_iAxisCount)
                        {
                           bool bisSetOk= SetMotionBufferData(ref m_BasePointXYZRA);
                            //--------------
                            if (bisSetOk)
                                m_nStep = (int)enStep.MotionBufferMoving;
                            else
                            {
                                ClearBuffer();
                                Error pError = new Error(ref this.m_NowAddress, "SetMotionBufferData异常", "", (int)ErrorDefine.enErrorCode.SetMotionBufferData异常);
                                pError.AddErrSloution("重新SetMotionBufferData", (int)enStep.MotionBuffer_SetBufferData);
                                pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                            }
                        }
                    }
                    break;
                case enStep.MotionBufferMoving:
                    {
                        SetMotionStart();
                        m_nStep = (int)enStep.MotionBufferMovingCompleted;
                    }
                    break;
                case enStep.MotionBufferMovingCompleted:
                    {

                        UInt32 m_retRtn1 = 999, m_retRtn2 = 999;
                        m_retRtn1 = ImcApi.IMC_CrdGetUserID(Sys_Define.m_cardHandle, (short)m_uTableNO, ref Sys_Define.m_nUserID);
                        m_retRtn2 = ImcApi.IMC_CrdGetSpace(Sys_Define.m_cardHandle, (short)m_uTableNO, ref Sys_Define.m_nRetSpace);
                        //-------------------------
                        GetMotionStatus(ref m_MotionStatus);
                        if (m_MotionStatus.MotionStatus == Sys_Define.enMotionBufferStatus.en_Idle  )
                            m_nStep = (int)enStep.MotionBufferMove_CheckisAlarm;
                        else {
                            if (m_MotionStatus.MotionStatus == Sys_Define.enMotionBufferStatus.en_Fault )
                            {
                                m_nStep = (int)enStep.MotionBufferMove_Alarm;
                            }
                        }
                    }
                    break;
                case enStep.MotionBufferMove_CheckisAlarm:
                    {
                        int iAlarmCount = 0, iMelorPel = 0;
                        //-----------------------
                        for (int i = 0; i < m_iAxisCount; i++)
                        {
                            if (m_pMotor[i].isAlarm())
                            {
                                iAlarmCount++;
                                Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : ," + (i + 1).ToString() + "轴报警");
                            }
                            if (m_pMotor[i].isMELorPEL())
                            {
                                iMelorPel++;
                                if (m_pMotor[i].isPEL())
                                {
                                    Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : ," + (i + 1).ToString() + "轴正极限报警");
                                }
                                if (m_pMotor[i].isMEL())
                                {
                                    Sys_Define.RecordMessageLog("API_ErrorRecord", "dllAPI : ," + (i + 1).ToString() + "轴负极限报警");
                                }
                            }
                        }
                        //----------------------
                        if (iAlarmCount == 0 && iMelorPel == 0 )
                            m_nStep = (int)enStep.MotionBufferMoveCompleted;
                        else
                            m_nStep = (int)enStep.MotionBufferMove_Alarm;
                    }
                    break;
                   case enStep.MotionBufferMoveCompleted:
                    {
                        bool bClearOK=ClearBuffer();
                        if (bClearOK)
                        {
                            m_pSetIO.SetIO(false);
                            m_Status = 狀態.待命;
                        }
                        else {
                            Error pError = new Error(ref this.m_NowAddress, "ClearBuffer异常", "", (int)ErrorDefine.enErrorCode.CleraBuffer异常);
                            pError.AddErrSloution("重新ClearBuffer", (int)enStep.MotionBufferMoveCompleted);
                            pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                        }
                    }
                    break;

                #endregion
                //-------------------------
                case enStep.MotionBufferMove_Alarm:
                    {
                        Error pError = new Error(ref this.m_NowAddress, "MotionBuffer动做中电机驱动器异常", "", (int)ErrorDefine.enErrorCode.MotionBuffer动做中电机驱动器异常);
                        pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    }
                    break;



                    ////******************************************
            }
            base.StepCycle(ref dbTime);
        }

        //-------------------------------------------------------------------------

        public bool StartMotion(ref Sys_Define.tyAXIS_XYZRA basePoint)
        {
            m_BasePointXYZRA = basePoint;
            //--------------
            int doStep = (int)enStep.MotionBufferMove_Start;
            bool bRet = DoStep(doStep);
            return bRet;
        }


        public double CalcSyncVel(double targetPos,double targetVel,double targetAcc,double syncPos,double syncVelMax)
        {
            double newTargetVel = 0.0;
            double targetVacc;
            double posKin;
            double syncLenAcc;
            double syncVel;

            if(syncPos <= 0.0 || targetPos <= 0.0 || targetVel <= 0.0 || targetAcc < 0.0)
            {
                return targetVel;
            }
            if( syncVelMax <= 0.0)
            {
                return newTargetVel;
            }
            posKin = targetPos / syncPos;
            targetVacc = Math.Sqrt(targetAcc * targetPos);

            if(targetVacc >= targetVel)
            {
                syncVel = targetVel / posKin;
            }else
            {
                syncVel = targetVacc / posKin;
            }

            if (syncVel > syncVelMax)
            {
                newTargetVel = syncVelMax * posKin;
            }
            else
            {
                newTargetVel = targetVel;
            }

            return newTargetVel;
        }

      
        public int  CalSpeed(double dbDX,double dbDY,double dbDZ,  double dbDR,double dbDA,int iActionVel,int iAccVel,int iRAMaxSpeed, int RPulseRev, int APulseRev)
        {
            double dbXYPosition = Math.Sqrt(Math.Pow(dbDX, 2.0) + Math.Pow(dbDY, 2.0) + Math.Pow(dbDZ, 2.0));
            int V1=  (int)CalcSyncVel(dbXYPosition, iActionVel, iAccVel, dbDR, iRAMaxSpeed);
            int V2=  (int)CalcSyncVel(dbXYPosition, iActionVel, iAccVel, dbDA, iRAMaxSpeed);

            return Math.Min(V1, V2);
            // double dbXYPosition = Math.Sqrt ( Math.Pow(dbDX, 2.0) + Math.Pow(dbDY, 2.0) + Math.Pow(dbDZ, 2.0));
            // //double dbRAPosition = Math.Sqrt(Math.Pow(dbDR, 2.0) + Math.Pow(dbDA, 2.0) );
            // //----------



            // double T = dbXYPosition / (double)iActionVel;
            // //double t_RASpeed = dbRAPosition / T;
            // double dbRAMaxSpeed = (double)iRAMaxSpeed;
            // //----------
            //// if (t_RASpeed >= dbRAMaxSpeed)
            // {
            //     if (dbDR > dbDA)
            //     {
            //         double r_Time = (dbDR / RPulseRev / iRAMaxSpeed) * 60;
            //         int RealXYZSpeed = (int)(dbXYPosition / r_Time);
            //         return RealXYZSpeed;
            //     }
            //     else
            //     {
            //         double r_Time = (dbDA / APulseRev / iRAMaxSpeed) * 60;
            //         int RealXYZSpeed = (int)(dbXYPosition / r_Time);
            //         return RealXYZSpeed;
            //     }
            // }
            // //else {
            // //    return iActionVel;
            // //}
        }
        //--------------------
        private Sys_Define.tyAXIS_XY CalCenter(ref Sys_Define.tyAXIS_XY Point1, ref Sys_Define.tyAXIS_XY Point2, ref Sys_Define.tyAXIS_XY Point3)
        {
            Sys_Define.tyAXIS_XY CenterPoint = new Sys_Define.tyAXIS_XY();
            //---------------------------
            double x1, y1, x2, y2, x3, y3;
            double a, b, c, g, e, f;
            double dbCX,dbCY, dbR;
            x1 = Point1.X;
            y1 = Point1.Y;
            x2 = Point2.X;
            y2 = Point2.Y;
            x3 = Point3.X;
            y3 = Point3.Y;
            //---------------------------
            e = 2 * (x2 - x1);
            f = 2 * (y2 - y1);
            g = x2 * x2 - x1 * x1 + y2 * y2 - y1 * y1;
            a = 2 * (x3 - x2);
            b = 2 * (y3 - y2);
            c = x3 * x3 - x2 * x2 + y3 * y3 - y2 * y2;
            //------------
            CenterPoint.X = dbCX = (g * b - c * f) / (e * b - a * f);
            CenterPoint.Y = dbCY = (a * g - c * e) / (a * f - b * e);
            dbR = Math.Sqrt((dbCX - x1) * (dbCX - x1) + (dbCY - y1) * (dbCY - y1));
            return CenterPoint;
        }
  
        private ushort CalCircleDir(ref Sys_Define.tyAXIS_XY Point1, ref Sys_Define.tyAXIS_XY Point2, ref Sys_Define.tyAXIS_XY Point3)
        {
            ushort uCircleDir = 0;
            double ABX = Point2.X - Point1.X;//向量AB的横坐标
            double ABY = Point2.Y - Point1.Y;//向量AB的纵坐标
            double BCX = Point3.X - Point2.X;
            double BCY = Point3.Y - Point2.Y;

            //向量AB和BC的叉乘
            double product = ABX * BCY - BCX * ABY;

            if (product > 0)
            {
                uCircleDir = 1; // "逆时针"
            }
            else if (product == 0)
            {
                uCircleDir = 2; // "三点共线"
            }
            else
            {
                uCircleDir = 0; // "顺时针"
            }

            return uCircleDir;
        }

        private double CalAngle(Sys_Define.tyAXIS_XY center, Sys_Define.tyAXIS_XY P1, Sys_Define.tyAXIS_XY P2)
        {
            double ma_X = P1.X - center.X;
            double ma_Y = P1.Y - center.Y;
            double mb_x = P2.X - center.X;
            double mb_y = P2.Y - center.Y;
            double v1 = (ma_X * mb_x) + (ma_Y * mb_y);
            double ma_val = Math.Sqrt(ma_X * ma_X + ma_Y * ma_Y);
            double mb_val = Math.Sqrt(mb_x * mb_x + mb_y * mb_y);
            double cosM = v1 / (ma_val * mb_val);
            double retAngle = Math.Acos(cosM) * 180 / Math.PI;

            return retAngle;
        }



    }
}
