using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MotionBase;
using System.Threading;

namespace Cowain_AutoMotion
{
    public class clsMSignalTower111
    {
        public clsMSignalTower111()
        {
            m_tmLightOpen = new System.Timers.Timer(1000);
            m_tmLightClose = new System.Timers.Timer(1000);
            //------------
            m_OpenTime = 3000;    //On -> 3000ms
            m_CloseTime = 1000;   //Off-> 1000ms      
            //------------
            m_tmLightOpen.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent_LightOpen);
            m_tmLightClose.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent_LightClose);
            thread = new Thread(work);
            thread.IsBackground = true;
            thread.Start();
        }
        #region 參數&變數
        public DrvIO pLightR
        {
            get
            {
                return HardWareControl.getOutputIO(EnumParam_OutputIO.三色灯_红灯);
            }
        }
        public DrvIO pLightY
        {
            get
            {
                return HardWareControl.getOutputIO(EnumParam_OutputIO.三色灯_黄灯);
            }
        }
        public DrvIO pLightG
        {
            get
            {
                return HardWareControl.getOutputIO(EnumParam_OutputIO.三色灯_绿灯);
            }
        }
        public DrvIO pBuzzer
        {
            get
            {
                return HardWareControl.getOutputIO(EnumParam_OutputIO.三色灯_蜂鸣器);
            }
        }
        System.Timers.Timer m_tmLightOpen, m_tmLightClose;
        bool m_bBuzzerOff = false;
        uint m_OpenTime, m_CloseTime;
        public bool Alarm = false;
        public static bool NoUse_m_bBuzzer = false;
        Thread thread;
        int m_nStep = 0;
        //--------------------------------
        private void OnTimedEvent_LightOpen(object source, System.Timers.ElapsedEventArgs e) { m_tmLightOpen.Enabled = false; }
        private void OnTimedEvent_LightClose(object source, System.Timers.ElapsedEventArgs e) { m_tmLightClose.Enabled = false; }
        //--------------------
        public enum enTowerMode
        {
            enIdle = 0,
            enDoHome,
            enAlarm,
            enAutoRuning,
            enEmg,
            enStop,
            enLoading,
        }
        enTowerMode m_enTowerMode;
        public enum enStep
        {
            StartTowerLight,
            WaitOpenTimeOut,
            WaitCloseTimeOut,
            TowerLightCompleted,
            //--------------
            StartIOLight,
            WaitOpenIOTimeOut,
            WaitCloseIOTimeOut,
            TowerLightIOCompleted,
        }
        enStep m_Step;
        #endregion
        private void work()
        {
            while (true)
            {
                Thread.Sleep(100);
                StepCycle();
                if (Base.baseStatus == Base.Status.停止)
                {
                    pBuzzer.SetIO(false);
                }
            }
        }
        public void StepCycle()
        {
            m_Step = (enStep)m_nStep;
            switch (m_Step)
            {
                case enStep.StartTowerLight:
                    if (m_tmLightOpen.Enabled == false && m_tmLightClose.Enabled == false)
                    {
                        if (m_enTowerMode == enTowerMode.enAutoRuning)
                        {
                            SetLightStatus(false, false, true, false);//绿灯亮-运行
                            m_nStep = (int)enStep.TowerLightCompleted;
                        }
                        else if (m_enTowerMode == enTowerMode.enIdle)
                        {
                            SetLightStatus(false, true, false, false);//黄灯闪-运行待料
                            m_tmLightOpen.Interval = m_OpenTime;
                            m_tmLightOpen.Start();
                            m_nStep = (int)enStep.WaitOpenTimeOut;
                            //m_nStep = (int)enStep.TowerLightCompleted;
                        }
                        else if (m_enTowerMode == enTowerMode.enDoHome)
                        {
                            SetLightStatus(false, true, false, false);//黄灯闪-回原中
                            m_tmLightOpen.Interval = m_OpenTime;
                            m_tmLightOpen.Start();
                            m_nStep = (int)enStep.WaitOpenTimeOut;
                        }
                        else if (m_enTowerMode == enTowerMode.enAlarm)
                        {
                            //SetLightStatus(true, false, false, true);
                            //bool _m_bBuzzer = true;
                            //if (clsMSignalTower.NoUse_m_bBuzzer)-----------------------------------
                            //{
                            //    m_bBuzzerOff = true;
                            //}
                            SetLightStatus(true, false, false, true);//红灯+蜂鸣+3/1-报警
                            m_tmLightOpen.Interval = m_OpenTime;
                            m_tmLightOpen.Start();
                            Alarm = true;
                            m_nStep = (int)enStep.WaitOpenTimeOut;
                        }
                        else if (m_enTowerMode == enTowerMode.enEmg)
                        {
                            SetLightStatus(true, false, false, true);//红灯+蜂鸣+3/1-急停
                            Alarm = true;
                            m_nStep = (int)enStep.WaitOpenTimeOut;
                        }
                        else if (m_enTowerMode == enTowerMode.enStop)
                        {
                            SetLightStatus(false, true, false, false);//黄灯亮-停止
                            m_nStep = (int)enStep.TowerLightCompleted;
                        }
                        else if (m_enTowerMode == enTowerMode.enLoading)
                        {
                            SetLightStatus(false, true, false, false);//黄灯亮-停止
                            m_nStep = (int)enStep.TowerLightCompleted;
                        }
                    }
                    break;
                case enStep.WaitOpenTimeOut:
                    if (m_tmLightOpen.Enabled == false)
                    {
                        if (m_enTowerMode == enTowerMode.enDoHome)
                        {
                            SetLightStatus(false, false, false, false);//归原
                            m_tmLightClose.Interval = m_CloseTime;
                            m_tmLightClose.Start();
                            m_nStep = (int)enStep.WaitCloseTimeOut;
                        }
                        else if (m_enTowerMode == enTowerMode.enAlarm)
                        {
                            SetLightStatus(false, false, false, false);//红灯闪-报警
                            //SetLightStatus(true, false, false, false);//红灯亮-报警
                            m_tmLightClose.Interval = m_CloseTime;
                            m_tmLightClose.Start();
                            m_nStep = (int)enStep.WaitCloseTimeOut;
                        }
                        else if (m_enTowerMode == enTowerMode.enIdle)
                        {
                            SetLightStatus(false, false, false, false);//黄灯闪-运行待料
                            //SetLightStatus(false, true, false, false);//黄灯亮-运行待料
                            m_tmLightClose.Interval = m_CloseTime;
                            m_tmLightClose.Start();
                            m_nStep = (int)enStep.WaitCloseTimeOut;
                            //m_nStep = (int)enStep.TowerLightCompleted;
                        }
                    }
                    break;
                case enStep.WaitCloseTimeOut:
                    if (m_tmLightClose.Enabled == false)
                    {
                        m_nStep = (int)enStep.StartTowerLight;
                    }
                    break;
            }
        }
        //-------------------------------------------
        /// <summary>
        /// 三色灯
        /// </summary>
        /// <param name="bLightR">红</param>
        /// <param name="bLightY">黄</param>
        /// <param name="bLightG">绿</param>
        /// <param name="bBuzzer">蜂鸣</param>
        public void SetLightStatus(bool bLightR, bool bLightY, bool bLightG, bool bBuzzer)
        {
            if (pLightR != null)
                pLightR.SetIO(bLightR);

            if (pLightY != null)
                pLightY.SetIO(bLightY);
            if (pLightG != null)
                pLightG.SetIO(bLightG);
            if (pBuzzer != null && !m_bBuzzerOff)
                pBuzzer.SetIO(bBuzzer);
        }
        public void TowerLight(enTowerMode enMode)
        {
            SetTowerMode(enMode);
            m_Step = enStep.StartTowerLight;
        }
        public void LightReset()
        {
            m_Step = enStep.StartTowerLight;
        }
        private void SetTowerMode(enTowerMode enMode)
        {
            m_bBuzzerOff = false;
            m_enTowerMode = enMode;
            if (enMode == enTowerMode.enDoHome)
            {
                m_OpenTime = 1000;    //On -> 1000ms
                m_CloseTime = 1000;   //Off-> 1000ms              
            }
            else if (enMode == enTowerMode.enAlarm)
            {
                m_OpenTime = 3000;    //On -> 300ms
                m_CloseTime = 1000;   //Off-> 300ms
            }
            //else if (enMode == enTowerMode.enLoading)
            //{
            //    m_OpenTime = 5000;    //On -> 300ms
            //    m_CloseTime = 5000;   //Off-> 300ms
            //}
        }
        public void SetBuzzerOff()
        {
            m_bBuzzerOff = true;
            pBuzzer.SetIO(false);
        }
        public void SetBuzzerOn()
        {
            m_bBuzzerOff = false;
            pBuzzer.SetIO(true);
        }
    }
}