using System;
using System.Linq;
using System.Data;
using System.Text;
using System.IO.Ports;
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading;
using System.IO;

namespace MotionBase
{
    public class DrvFlashIO:Base
    {
        public DrvFlashIO(Type homeEnum1, Type stepEnum1, string instanceName1, Base parent, int nStation, String strEName, String strCName)
            : base(homeEnum1, stepEnum1, instanceName1,parent, nStation, strEName, strCName, 0)
        {
            m_OutIO = null;
            m_OnTimer = m_OffTimer = null;
            //------------
        }

        public enum enStep
        {
            StartIOFlash_On = 0,
            IOFlash_Off,
        }enStep m_Step;

        public DrvIO m_OutIO;
        public ScanTimer m_OnTimer;
        public ScanTimer m_OffTimer;

        public override void StepCycle(ref double dbTime)
        {
            m_Step = (enStep)m_nStep;
            switch (m_Step)
            {
                case enStep.StartIOFlash_On:
                    if (m_OffTimer.isTimeOut())
                    {
                        m_OutIO.SetIO(true);
                        m_OnTimer.Start();
                        m_nStep = (int)enStep.IOFlash_Off;
                    }
                    break;
                case enStep.IOFlash_Off:
                    if (m_OnTimer.isTimeOut())
                    {
                        m_OutIO.SetIO(false);
                        m_OffTimer.Start();
                        m_nStep = (int)enStep.StartIOFlash_On;
                    }
                    break;
            }
            base.StepCycle(ref dbTime);
        }
        public override void Stop()
        {
            m_OutIO.SetIO(false);
            m_OnTimer.Stop();
            m_OffTimer.Stop();
            base.Stop();
        }

        public bool StartIOFlash()
        {
            m_Mode = enMode.自動;
            int doStep = (int)enStep.StartIOFlash_On;
            if (DoStep(doStep))
                return true;
            else
                return false;
        }
    }
}
