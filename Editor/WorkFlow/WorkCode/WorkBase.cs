using Cowain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cowain
{
    public class WorkBase
    {
        public enum step
        {
            start,
            run,
            idel,
            error,
        }
        public step step1_Work = step.idel;
        public step step1_Home = step.idel;
        public int m_nStep = 0;
        private Status status = Status.待命;
        public Status m_Status
        {
            get
            {
                return status;
            }
            set
            {
                if (status == Status.待命 && value == Status.动作中)
                {
                    startEvent?.Invoke();
                }
                else if (status == Status.动作中 && value == Status.待命)
                {
                    status = value;
                    finishEvent?.Invoke();
                }
                else if(status == Status.错误)
                {
                    errorEvent?.Invoke();
                }
                status = value;
            }
        }
        private Thread thread;
        public WorkBase Address;
        public CTimer Timer1 = new CTimer();
        public CTimer Timer2 = new CTimer();
        public Action startEvent;
        public Action finishEvent;
        public Action errorEvent;
        public enum Status
        {
            待命,
            动作中,
            错误,
        }
        public bool isIdle()
        {
            if (m_Status == Status.待命)
            {
                return true;
            }
            return false;
        }
        public WorkBase()
        {
            thread = new Thread(work);
            thread.IsBackground = true;
            thread.Start();
            Address = this;
        }
        private void work()
        {
            while (true)
            {
                Thread.Sleep(1);
                if (m_Status == Status.待命 || m_Status == Status.错误)
                {
                    continue;
                }
                StepCycle();
                HomeCycle();
            }
        }
        public virtual void Stop()
        {
            m_Status = Status.待命;
        }
        public virtual void StepCycle()
        {
           //  m_Status = Status.待命;
        }
        public virtual void HomeCycle()
        {
           // m_Status = Status.待命;
        }
        public virtual void StepAction()
        {
            m_Status = Status.动作中;
            step1_Work = step.start;
        }
        public virtual void HomeAction()
        {
            m_Status = Status.动作中;
            step1_Home = step.start;
        }
    }
}
