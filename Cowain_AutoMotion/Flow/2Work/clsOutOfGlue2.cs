using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows;
using System.IO;
using MotionBase;
using ToolTotal;
using System.Windows.Forms;
using Cowain_Form.FormView;
using Cowain_AutoDispenser.Flow;

namespace Cowain_Machine.Flow
{
    public class clsOutOfGlue2 : Base
    {
        //-------------------------
        #region clsOutOfGlue 建構式&解構式
        public clsOutOfGlue(Base parent, int nStation, int nSubID, clsDispenserSt m_DispenserSt1, clsCleanParts pClearParts1, String strEName, String strCName, int ErrCodeBase)
           : base(parent, nStation, strEName, strCName, ErrCodeBase)
        {
            String strStation = nStation.ToString();
            m_iSubID = nSubID;
            Station = nStation;
            m_DispenserSt = m_DispenserSt1;
            pClearParts = pClearParts1;
            m_tmDelay = new System.Timers.Timer(1000);
            m_tmDelay.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent_DelayTimeOut);


            //---------------------
            if (nSubID == 0)
                pGantryParm = MSystemParameter.m_SysParm.Gantry1Parm;
            else
                pGantryParm = MSystemParameter.m_SysParm.Gantry2Parm;
        }
        ~clsOutOfGlue()
        {
        }
        #endregion
        int Station = 0;
        int m_iSubID = 0;
        MSystemParameter.GantryParm pGantryParm;
        #region 參數&變數
        public clsDispenserSt m_DispenserSt;//当前点胶模组
        clsCleanParts pClearParts;
        bool b_MoveOutOfGlue = true;//是否需要回到安全位
        long CleanAfterTime = DateTime.Now.Ticks;//擦胶后计时
        long CleanAfterTime1 = DateTime.Now.Ticks;
        public DrvIO pDisAir = null, pDisSpeed = null;
        public DrvIO pDisCompleted = null;
        /// <summary>
        /// 允许自动排胶计时 ，条件：机台开始待料时，生产此片产品后  
        /// </summary>
        public bool m_AllowTimes = false;//是否 
        System.Timers.Timer m_tmDelay;
        MDataDefine define = new MDataDefine();
        private void OnTimedEvent_DelayTimeOut(object source, System.Timers.ElapsedEventArgs e) { m_tmDelay.Enabled = false; }
        //-----------------------------------------------------     
        double m_dbMoveSpeedRatio = 30;
        public Sys_Define.tyAXIS_XYZRA m_tCCDyPosition = new Sys_Define.tyAXIS_XYZRA();
        //-------------------------
        public enum DispenserStatuType
        {
            空闲,
            点胶前擦胶,
            点胶后擦胶,
            自动排胶,
        }
        public MSystemParameter.enDispenserClearType pClearType;
        public DispenserStatuType m_DispenserStatuType = 0;
        public enum enStep
        {
            clsOutOfGlue_Start,
            MoveSafePose_Z,
            JudgeType,
            MoveOutOfGluePos_X,
            MoveOutOfGluePos_Z,
            startOutOfGlue,
            stopOutOfGlue,
            upZToSafePose_Z,
            MoveCleanGluePos_X,
            MoveCleanGluePos_Z,



            MoveRelX,
            waitTime1,



            startCleanGlue,


            upZToSafePoseAgain_Z,

            MoveOutOfGlue_X,//判断是否需要运动到排胶位
            MoveCCD_X,
            OutOfGlueCompleted,
        }
        enStep m_Step;
        public enum enHomeStep
        {
            StartHome = 0,
            HomeCompleted,
        }
        enHomeStep m_HomeStep;

        #endregion

        //-------------------------------------------

        public override void Stop()
        {
            //把点胶阀关闭，气关闭，夹子打开，状态置为空闲
            try
            {
                if (!m_DispenserSt.m_bisNordson)
                {
                    m_DispenserSt.m_Dispenser.pDispenerAirIO.SetIO(false);
                }
                m_DispenserSt.m_Dispenser.pDispenerIO.SetIO(false);
                if (m_DispenserSt.pDisCompleted != null)
                {
                    m_DispenserSt.pDisCompleted.SetIO(true);
                }
                if (pClearParts.m_Clip_On != null)
                {
                    pClearParts.m_Clip_On.SetIO(false);
                }
                m_Status = 狀態.待命;
                m_DispenserStatuType = DispenserStatuType.空闲;
                MDataDefine.outGlueCount[m_iSubID] = 0;//点击停止后把自动排胶的时间重置
            }
            catch
            {

            }
            base.Stop();
        }
        public override void HomeCycle(ref double dbTime)
        {
            m_HomeStep = (enHomeStep)m_nHomeStep;
            switch (m_HomeStep)
            {
                case enHomeStep.StartHome:
                    m_nHomeStep = (int)enHomeStep.HomeCompleted;
                    break;
                case enHomeStep.HomeCompleted:
                    m_bHomeCompleted = true;
                    m_Status = 狀態.待命;
                    break;
            }
            base.HomeCycle(ref dbTime);
        }
        Log log = new Log();
        int changepos = 0;//擦胶位置
        public override void StepCycle(ref double dbTime)
        {
            if (MachineDataDefine.StrOutGlueStep[m_iSubID] != m_nStep)
            {
                LogAuto.Notify("流道" + m_iSubID.ToString() + "----进入擦胶流程:" + ((enStep)m_nStep).ToString(), (int)MachineStation.前排胶 + m_iSubID);
                MachineDataDefine.StrOutGlueStep[m_iSubID] = m_nStep;
            }
            m_Step = (enStep)m_nStep;
            switch (m_Step)
            {
                case enStep.clsOutOfGlue_Start:
                    if (m_DispenserSt?.m_Dispenser?.pMotZ?.isIDLE() == true)
                    {
                        LogAuto.Notify("流道" + m_iSubID.ToString() + "进入擦胶模式：" + ((DispenserStatuType)m_DispenserStatuType).ToString(), (int)MachineStation.前排胶 + m_iSubID);
                        m_nStep = (int)enStep.MoveSafePose_Z;
                        MDataDefine.machineStatus[m_iSubID] = MachineStatus.自动排胶中;
                        if (pClearType == MSystemParameter.enDispenserClearType.NonUse)
                        {//未启用擦胶模组
                            m_nStep = (int)enStep.OutOfGlueCompleted;
                            MDataDefine.SaveOutOfGlueLog("流道" + m_iSubID.ToString() + "---未使用擦胶模组，跳出", m_iSubID);
                        }
                    }
                    break;
                case enStep.MoveSafePose_Z:
                    {
                        double dbSafePos = m_DispenserSt.m_tyPosition[(int)clsDispenserSt.enPosition.SafePos].Z;
                        m_DispenserSt.m_Dispenser.pMotZ.AbsMove(dbSafePos, m_dbMoveSpeedRatio);
                        LogAuto.Notify("流道" + m_iSubID.ToString() + "---Z轴移动至SafePos：" + dbSafePos, m_iSubID);
                        m_nStep = (int)enStep.JudgeType;
                    }
                    break;
                case enStep.JudgeType:
                    if (m_DispenserSt.m_Dispenser.pMotZ.isIDLE())
                    {
                        LogAuto.Notify("流道" + m_iSubID.ToString() + "---Z轴位置已到：" + m_DispenserSt.m_Dispenser.pMotZ.GetPosition(), m_iSubID);
                        if (m_DispenserStatuType == DispenserStatuType.点胶前擦胶)
                        {
                            long nowTime = DateTime.Now.Ticks;
                            long m_AfterCleanTime = (nowTime - CleanAfterTime1) / 10000 / 1000;
                            if (m_AfterCleanTime > MDataDefine.outGlueDelay && !MDataDefine.b_UseOutGlue && !MDataDefine.b_NullRun)
                            {
                                CleanAfterTime1 = DateTime.Now.Ticks;
                                m_nStep = (int)enStep.MoveOutOfGluePos_X;
                                LogAuto.Notify("流道" + m_iSubID.ToString() + "---470点胶前开始100秒强制排胶", m_iSubID);
                            }
                            else
                                m_nStep = (int)enStep.MoveCleanGluePos_X;
                        }
                        else if (m_DispenserStatuType == DispenserStatuType.点胶后擦胶)
                            m_nStep = (int)enStep.MoveCleanGluePos_X;
                        else if (m_DispenserStatuType == DispenserStatuType.自动排胶)
                            m_nStep = (int)enStep.MoveOutOfGluePos_X;
                    }
                    break;
                case enStep.MoveOutOfGluePos_X:
                    if ((m_DispenserSt.m_Dispenser.pMotX.isIDLE() && m_DispenserSt.m_Dispenser.pMotZ.isIDLE()) != true)
                    {
                        break;
                    }
                    double dbZposIsSafe = m_DispenserSt.m_Dispenser.pMotZ.GetPosition();
                    if (dbZposIsSafe >= (m_DispenserSt.m_tyPosition[(int)clsDispenserSt.enPosition.SafePos].Z) - 3)
                    {
                        double PaiJiaoPos = m_DispenserSt.m_tyPosition[(int)clsDispenserSt.enPosition.PaiJiaoPos].X;
                        LogAuto.Notify("流道" + m_iSubID.ToString() + "---Z轴位置:" + dbZposIsSafe + "---X轴准备运动至排胶位：" + PaiJiaoPos, m_iSubID);
                        m_DispenserSt.m_Dispenser.pMotX.AbsMove(PaiJiaoPos, m_dbMoveSpeedRatio);
                        m_nStep = (int)enStep.MoveOutOfGluePos_Z;
                    }
                    else
                    {
                        MessageBox.Show("存在撞机风险！，请确认！！！自动模式将停止-1");
                        LogAuto.Notify("流道" + m_iSubID.ToString() + "---去排胶位前，Z轴位置：" + dbZposIsSafe + "---不安全：", m_iSubID);
                        frm_Auto.m_frm_Auto.Stop();
                        return;
                    }
                    break;
                case enStep.MoveOutOfGluePos_Z:
                    if (m_DispenserSt.m_Dispenser.pMotX.isIDLE() && m_DispenserSt.m_Dispenser.pMotZ.isIDLE())
                    {
                        double PaiJiaoPos = m_DispenserSt.m_tyPosition[(int)clsDispenserSt.enPosition.PaiJiaoPos].Z;
                        LogAuto.Notify("流道" + m_iSubID.ToString() + "---X已至排胶位：" + m_DispenserSt.m_Dispenser.pMotX.GetPosition() + "---准备运动Z:" + PaiJiaoPos, m_iSubID);
                        m_DispenserSt.m_Dispenser.pMotZ.AbsMove(PaiJiaoPos, m_dbMoveSpeedRatio);
                        m_nStep = (int)enStep.startOutOfGlue;
                    }
                    break;
                case enStep.startOutOfGlue:
                    if (m_DispenserSt.m_Dispenser.pMotZ.isIDLE())
                    {
                        LogAuto.Notify("流道" + m_iSubID.ToString() + "---Z轴已到排胶位：" + m_DispenserSt.m_Dispenser.pMotZ.GetPosition(), m_iSubID);
                        if (m_DispenserSt.m_bisNordson)
                        {
                            m_DispenserSt.m_Dispenser.pDispenerAirIO.SetIO(true);
                        }
                        m_DispenserSt.m_Dispenser.pDispenerIO.SetIO(true);
                        MDataDefine.PurgeTime[m_iSubID] = DateTime.Now.ToString("yyyyMMddHHmmss");
                        if (m_DispenserSt.pDisCompleted != null)
                        {
                            m_DispenserSt.pDisCompleted.SetIO(false);
                        }
                        m_tmDelay.Interval = MDataDefine.DisTime[m_iSubID] * 1000;
                        m_tmDelay.Start();
                        m_nStep = (int)enStep.stopOutOfGlue;
                    }
                    break;
                case enStep.stopOutOfGlue:
                    if (m_tmDelay.Enabled == false)
                    {
                        if (m_DispenserSt.m_bisNordson)
                        {
                            m_DispenserSt.m_Dispenser.pDispenerAirIO.SetIO(false);
                        }
                        m_DispenserSt.m_Dispenser.pDispenerIO.SetIO(false);

                        //------------------------------------------------------------------------------------------
                        //胶量计量2-自动排胶计量
                        double GlueWatch_Last = MDataDefine.GlueWatch_Now[m_iSubID];
                        MDataDefine.GlueWatch_Now[m_iSubID] = MDataDefine.GlueWatch_Now[m_iSubID] + MDataDefine.GlueOutClear[m_iSubID];//出胶量为GlueOutClear mg
                        MDataDefine.SaveGlueControl("\n上次累计出胶量：" + GlueWatch_Last + "\t此次出胶量：" + MDataDefine.GlueOutClear[m_iSubID] + "\t排胶后累计出胶量：" + MDataDefine.GlueWatch_Now[m_iSubID], m_iSubID);
                        define.GlueWatchWrite(MDataDefine.GlueWatch_Now[m_iSubID], m_iSubID);//写入累计胶量       


                        if (m_DispenserSt.pDisCompleted != null)
                        {
                            m_DispenserSt.pDisCompleted.SetIO(true);
                        }
                        m_DispenserSt.m_Dispenser.pDispenerAirCutIO.SetIO(true);//--------断胶吹气
                        m_nStep = (int)enStep.upZToSafePose_Z;
                    }
                    break;
                case enStep.upZToSafePose_Z:
                    if (m_DispenserSt.m_Dispenser.pMotZ.isIDLE())
                    {
                        double dbSafePos1 = m_DispenserSt.m_tyPosition[(int)clsDispenserSt.enPosition.SafePos].Z;
                        LogAuto.Notify("流道" + m_iSubID.ToString() + "---排胶结束，Z轴运动到SafePos：" + dbSafePos1, m_iSubID);
                        m_DispenserSt.m_Dispenser.pMotZ.AbsMove(dbSafePos1, m_dbMoveSpeedRatio);
                        m_nStep = (int)enStep.MoveCleanGluePos_X;
                    }
                    break;
                case enStep.MoveCleanGluePos_X:
                    if ((m_DispenserSt.m_Dispenser.pMotX.isIDLE() && m_DispenserSt.m_Dispenser.pMotZ.isIDLE()) != true)
                    {
                        m_DispenserSt.m_Dispenser.pDispenerAirCutIO.SetIO(false);//--------断胶吹气
                        break;
                    }
                    double dbZposIsSafe1 = m_DispenserSt.m_Dispenser.pMotZ.GetPosition();
                    if (dbZposIsSafe1 >= (m_DispenserSt.m_tyPosition[(int)clsDispenserSt.enPosition.SafePos].Z) - 3)
                    {
                        double ClearPos = m_DispenserSt.m_tyPosition[(int)clsDispenserSt.enPosition.ClearPos1].X;

                        if (pGantryParm.ChangeClearPos)//交互擦胶
                        {
                            if (changepos == 0)
                                ClearPos = m_DispenserSt.m_tyPosition[(int)clsDispenserSt.enPosition.ClearPos1].X;
                            else
                                ClearPos = m_DispenserSt.m_tyPosition[(int)clsDispenserSt.enPosition.ClearPos2].X;
                        }
                        LogAuto.Notify("流道" + m_iSubID.ToString() + "---Z轴位置:" + dbZposIsSafe1 + "---X轴准备运动至ClearPos：" + ClearPos, m_iSubID);
                        m_DispenserSt.m_Dispenser.pMotX.AbsMove(ClearPos, m_dbMoveSpeedRatio);
                        m_nStep = (int)enStep.MoveCleanGluePos_Z;
                    }
                    else
                    {
                        MessageBox.Show("存在撞机风险！，请确认！！！自动模式将停止-2");
                        LogAuto.Notify("流道" + m_iSubID.ToString() + "---排胶后运动到安全位，Z轴位置：" + dbZposIsSafe1 + "---不安全：", m_iSubID);
                        frm_Auto.m_frm_Auto.Stop();
                        return;
                    }
                    break;
                case enStep.MoveCleanGluePos_Z:
                    if (m_DispenserSt.m_Dispenser.pMotX.isIDLE() && m_DispenserSt.m_Dispenser.pMotZ.isIDLE())
                    {
                        if ((MDataDefine.stationName.Contains("470") || m_DispenserStatuType != DispenserStatuType.点胶后擦胶)|| 
                            (m_DispenserStatuType == DispenserStatuType.点胶后擦胶 && MSystemParameter.m_SysParm.Gantry1Parm.enMatchMode == MSystemParameter.m_SysParm.Gantry2Parm.enMatchMode))
                        {
                            double dbSafePos1 = m_DispenserSt.m_tyPosition[(int)clsDispenserSt.enPosition.ClearPos1].Z;
                            if (pGantryParm.ChangeClearPos)//交互擦胶
                            {
                                if (changepos == 0)
                                {
                                    dbSafePos1 = m_DispenserSt.m_tyPosition[(int)clsDispenserSt.enPosition.ClearPos1].Z;
                                    changepos = 1;
                                }
                                else
                                {
                                    dbSafePos1 = m_DispenserSt.m_tyPosition[(int)clsDispenserSt.enPosition.ClearPos2].Z;
                                    changepos = 0;
                                }
                            }
                            LogAuto.Notify("流道" + m_iSubID.ToString() + "---X轴已到擦胶位:" + m_DispenserSt.m_Dispenser.pMotX.GetPosition(), m_iSubID);
                            LogAuto.Notify("流道" + m_iSubID.ToString() + "---Z轴准备运动到擦胶Z:" + dbSafePos1, m_iSubID);
                            m_DispenserSt.m_Dispenser.pMotZ.AbsMove(dbSafePos1, m_dbMoveSpeedRatio);
                            if (pClearType == MSystemParameter.enDispenserClearType.Type1)
                            {
                                LogAuto.Notify("流道" + m_iSubID.ToString() + "---无夹子模式", m_iSubID);
                                m_nStep = (int)enStep.MoveRelX;
                            }
                            else
                            {
                                LogAuto.Notify("流道" + m_iSubID.ToString() + "---有夹子模式", m_iSubID);
                                m_nStep = (int)enStep.startCleanGlue;
                            }
                        }
                        else
                        {
                            LogAuto.Notify("流道" + m_iSubID.ToString() + "---非470设备点胶后不进行擦胶，直接结束", m_iSubID);
                            m_nStep = (int)enStep.OutOfGlueCompleted;
                        }
                    }
                    break;
                case enStep.MoveRelX:
                    if (m_DispenserSt.m_Dispenser.pMotZ.isIDLE())
                    {
                        LogAuto.Notify("流道" + m_iSubID.ToString() + "无夹子---Z到擦胶Z：" + m_DispenserSt.m_Dispenser.pMotZ.GetPosition(), m_iSubID);
                        LogAuto.Notify("流道" + m_iSubID.ToString() + "---X轴移动：3", m_iSubID);
                        m_DispenserSt.m_Dispenser.pMotX.RevMove(3, m_dbMoveSpeedRatio);
                        m_nStep = (int)enStep.waitTime1;
                    }

                    break;
                case enStep.waitTime1:
                    if (m_DispenserSt.m_Dispenser.pMotX.isIDLE())
                    {
                        m_nStep = (int)enStep.startCleanGlue;
                    }
                    break;
                case enStep.startCleanGlue:
                    if (m_DispenserSt.m_Dispenser.pMotZ.isIDLE())
                    {
                        LogAuto.Notify("流道" + m_iSubID.ToString() + "有夹子---Z到擦胶Z：" + m_DispenserSt.m_Dispenser.pMotZ.GetPosition(), m_iSubID);
                        pClearParts.CleanPartsAction();
                        if (pClearType == MSystemParameter.enDispenserClearType.Type2)
                        {
                            m_tmDelay.Interval = 500;
                            m_tmDelay.Start();
                        }
                        m_nStep = (int)enStep.upZToSafePoseAgain_Z;
                    }
                    break;
                case enStep.upZToSafePoseAgain_Z:
                    if (pClearType == MSystemParameter.enDispenserClearType.Type1 || m_tmDelay.Enabled == false)
                    {
                        if (m_DispenserSt.m_Dispenser.pMotZ.isIDLE())
                        {
                            double dbSafePos1 = m_DispenserSt.m_tyPosition[(int)clsDispenserSt.enPosition.SafePos].Z;
                            LogAuto.Notify("流道" + m_iSubID.ToString() + "---Z轴准备运动到SafePos：" + dbSafePos1, m_iSubID);
                            m_DispenserSt.m_Dispenser.pMotZ.AbsMove(dbSafePos1, m_dbMoveSpeedRatio);
                            m_nStep = (int)enStep.MoveOutOfGlue_X;
                            MDataDefine.outGlueCount[m_iSubID] = 0;//出胶后把自动排胶的时间重置     
                            if (m_DispenserStatuType != DispenserStatuType.点胶前擦胶)
                                m_AllowTimes = true;
                            CleanAfterTime = DateTime.Now.Ticks;
                        }
                    }
                    break;
                case enStep.MoveOutOfGlue_X:
                    if (b_MoveOutOfGlue)
                    {
                        if ((m_DispenserSt.m_Dispenser.pMotX.isIDLE() && m_DispenserSt.m_Dispenser.pMotZ.isIDLE()) != true)
                        {
                            break;
                        }
                        MDataDefine.machineStatus[m_iSubID] = MachineStatus.待料中;  //排胶各轴真实完成后，再置换设备状态

                        double dbZposIsSafe2 = m_DispenserSt.m_Dispenser.pMotZ.GetPosition();
                        if (dbZposIsSafe2 >= (m_DispenserSt.m_tyPosition[(int)clsDispenserSt.enPosition.SafePos].Z) - 3)
                        {
                            LogAuto.Notify("流道" + m_iSubID.ToString() + "---Z轴已到SafePos：" + dbZposIsSafe2, m_iSubID);
                            m_nStep = (int)enStep.OutOfGlueCompleted;
                        }
                        else
                        {
                            MessageBox.Show("存在撞机风险！，请确认！！！自动模式将停止-3");
                            LogAuto.Notify("流道" + m_iSubID.ToString() + "---Z轴未到SafePos,不安全：" + dbZposIsSafe2, m_iSubID);
                            frm_Auto.m_frm_Auto.Stop();
                            return;
                        }
                    }
                    else
                    {
                        //310设备排胶后不能立马去拍照位，其他设备可以
                        if (MDataDefine.stationName.Contains("310"))
                            m_nStep = (int)enStep.OutOfGlueCompleted;
                        else
                            m_nStep = (int)enStep.MoveCCD_X;
                    }
                    break;
                case enStep.MoveCCD_X:

                    if ((m_DispenserSt.m_Dispenser.pMotX.isIDLE() && m_DispenserSt.m_Dispenser.pMotZ.isIDLE()) != true)
                    {
                        break;
                    }
                    double dbZposIsSafe3 = m_DispenserSt.m_Dispenser.pMotZ.GetPosition();
                    if (dbZposIsSafe3 >= (m_DispenserSt.m_tyPosition[(int)clsDispenserSt.enPosition.SafePos].Z) - 3)
                    {
                        double ccdPos = m_tCCDyPosition.X;
                        m_DispenserSt.m_Dispenser.pMotX.AbsMove(ccdPos, m_dbMoveSpeedRatio);
                        m_nStep = (int)enStep.OutOfGlueCompleted;
                    }
                    else
                    {
                        MessageBox.Show("存在撞机风险！，请确认！！！自动模式将停止-3");
                        LogAuto.Notify("流道" + m_iSubID.ToString() + "---Z轴未到dbSafePos1,不安全：" + dbZposIsSafe3, m_iSubID);
                        frm_Auto.m_frm_Auto.Stop();
                        return;
                    }
                    break;
                case enStep.OutOfGlueCompleted:
                    if (m_DispenserSt.m_Dispenser.pMotX.isIDLE())
                    {
                        if (m_DispenserStatuType == DispenserStatuType.点胶后擦胶 && !MDataDefine.stationName.Contains("470"))
                        {
                            MDataDefine.machineStatus[m_iSubID] = MachineStatus.待料中;  //排胶各轴真实完成后，再置换设备状态
                            m_AllowTimes = true;
                            CleanAfterTime = DateTime.Now.Ticks;
                        }
                        LogAuto.Notify("流道" + m_iSubID.ToString() + "---擦胶完成", m_iSubID);
                        m_DispenserStatuType = DispenserStatuType.空闲;
                        m_Status = 狀態.待命;
                    }

                    break;
            }
            base.StepCycle(ref dbTime);
        }
        public override bool LoadMachineData(string strMachinePath)
        {
            String strPath = System.IO.Directory.GetCurrentDirectory();
            String strNowPath = strPath.Replace("\\bin\\x64\\Debug", "");
            String strDataSetPath = strNowPath + "\\DataSet.xml";
            //******************************

            if (m_iSubID == 0)
            {
                pClearType = MSystemParameter.m_SysParm.MachineParmeter.Dispenser1ClearType;
            }
            else
            {
                pClearType = MSystemParameter.m_SysParm.MachineParmeter.Dispenser2ClearType;
            }
            return base.LoadMachineData(strMachinePath);
        }

        public override bool isSafe(ref Base pBase, ref string strCDiscript, ref string strEDiscript, ref int ErrorCode)
        {
            return base.isSafe(ref pBase, ref strCDiscript, ref strEDiscript, ref ErrorCode);
        }
        //-------------------------------------------
        public bool DoHome()
        {
            int doStep = (int)enHomeStep.StartHome;
            bool bRet = DoHomeStep(doStep);
            return bRet;
        }

        public bool DispenserAction(double dbSpeed, DispenserStatuType m_DispenserStatuType1, Sys_Define.tyAXIS_XYZRA m_tCCDyPosition1, bool b_MoveOutOfGlue1 = true)
        {
            m_AllowTimes = false;
            m_tCCDyPosition = m_tCCDyPosition1;
            b_MoveOutOfGlue = b_MoveOutOfGlue1;
            m_dbMoveSpeedRatio = dbSpeed;
            m_DispenserStatuType = m_DispenserStatuType1;
            if (m_DispenserStatuType == DispenserStatuType.自动排胶)
            {
                m_dbMoveSpeedRatio = 30;
            }
            //-----------
            int doStep = (int)enStep.clsOutOfGlue_Start;
            bool bRet = DoStep(doStep);
            return bRet;
        }
        public bool DispenserAction(double dbSpeed, DispenserStatuType m_DispenserStatuType1, bool b_MoveOutOfGlue1 = true)
        {

            m_AllowTimes = false;
            m_tCCDyPosition = m_DispenserSt.m_tyPosition[(int)clsDispenserSt.enPosition.CCDPos1];
            b_MoveOutOfGlue = b_MoveOutOfGlue1;
            m_dbMoveSpeedRatio = dbSpeed;
            m_DispenserStatuType = m_DispenserStatuType1;
            if (m_DispenserStatuType == DispenserStatuType.自动排胶)
            {
                m_dbMoveSpeedRatio = 30;
            }
            //-----------
            int doStep = (int)enStep.clsOutOfGlue_Start;
            bool bRet = DoStep(doStep);
            return bRet;
        }
    }
}
