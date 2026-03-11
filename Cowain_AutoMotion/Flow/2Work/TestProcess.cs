using Cowain_AutoMotion.Flow;
using Cowain_Machine;
using MotionBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Cowain_Machine.Flow.MErrorDefine;

namespace Cowain_AutoMotion
{
    public class TestProcess : Base
    {
        private TestProcess_WorkStep currentWorkStep;
        int index1 = 0;
        int index2 = 0;
        int runtype = 0;
        public TestProcess(Type homeEnum1, Type stepEnum1, string instanceName1, Base parent) : base(homeEnum1, stepEnum1, instanceName1, parent, false)
        {

        }
        public enum TestProcess_WorkStep
        {
            Start = 0,
            执行步骤1,
            执行步骤2,
            执行步骤3,
            执行步骤31,
            执行步骤4,
            执行步骤5,
            执行步骤6,
            执行步骤7,
            执行步骤8,
            执行步骤81,
            执行步骤9,
            Completed
        }
        public override bool DoHomeStep(int iHomeStep)
        {
            m_bHomeCompleted = true;
            m_Status = 狀態.待命;
            return true;
        }
        public override void StepCycle(ref double dbTime)
        {
            currentWorkStep = (TestProcess_WorkStep)m_nStep;
            switch (currentWorkStep)
            {
                case TestProcess_WorkStep.Start:
                    if(timerDelay.Enabled==false)
                    {
                        timerDelay.Interval = 1000;
                        timerDelay.Start();
                        if (runtype == 0)
                        {
                            m_nStep = (int)TestProcess_WorkStep.执行步骤1;
                            runtype = 1;
                        }
                        else
                        {
                            m_nStep = (int)TestProcess_WorkStep.执行步骤6;
                            runtype = 0;
                        }
                    }
                    break;
                case TestProcess_WorkStep.执行步骤1:
                    if (timerDelay.Enabled == false)
                    {
                        timerDelay.Interval = 1000;
                        timerDelay.Start();
                        m_nStep = (int)TestProcess_WorkStep.执行步骤2;
                    }
                    break;
                case TestProcess_WorkStep.执行步骤2:
                    if (timerDelay.Enabled == false)
                    {
                        timerDelay.Interval = 1000;
                        timerDelay.Start();
                        if (index1 == 0)
                        {
                            m_nStep = (int)TestProcess_WorkStep.执行步骤3;
                            index1 = 1;
                        }
                        else
                        {
                            m_nStep = (int)TestProcess_WorkStep.执行步骤31;
                            index1 = 0;
                        }
                    }
                    break;
                case TestProcess_WorkStep.执行步骤3:
                    if (timerDelay.Enabled == false)
                    {
                        timerDelay.Interval = 1000;
                        timerDelay.Start();
                        m_nStep = (int)TestProcess_WorkStep.执行步骤4;
                        break;
                    }
                    break;
                case TestProcess_WorkStep.执行步骤31:
                    if (timerDelay.Enabled == false)
                    {
                        timerDelay.Interval = 1000;
                        timerDelay.Start();
                        m_nStep = (int)TestProcess_WorkStep.执行步骤4;
                        break;
                    }
                    break;
                case TestProcess_WorkStep.执行步骤4:
                    if (timerDelay.Enabled == false)
                    {
                        timerDelay.Interval = 1000;
                        timerDelay.Start();
                        m_nStep = (int)TestProcess_WorkStep.执行步骤5;
                        break;
                    }
                    break;
                case TestProcess_WorkStep.执行步骤5:
                    if (timerDelay.Enabled == false)
                    {
                        timerDelay.Interval = 1000;
                        timerDelay.Start();
                        m_nStep = (int)TestProcess_WorkStep.Completed;
                        break;
                    }
                    break;
                case TestProcess_WorkStep.执行步骤6:
                    if (timerDelay.Enabled == false)
                    {
                        timerDelay.Interval = 1000;
                        timerDelay.Start();
                        m_nStep = (int)TestProcess_WorkStep.执行步骤7;
                        break;
                    }
                    break;
                case TestProcess_WorkStep.执行步骤7:
                    if (timerDelay.Enabled == false)
                    {
                        timerDelay.Interval = 1000;
                        timerDelay.Start();
                        if (index2 == 0)
                        {
                            m_nStep = (int)TestProcess_WorkStep.执行步骤8;
                            index2 = 1;
                        }
                        else
                        {
                            m_nStep = (int)TestProcess_WorkStep.执行步骤81;
                            index2 = 0;
                        }
                    }
                    break;
                case TestProcess_WorkStep.执行步骤8:
                    if (timerDelay.Enabled == false)
                    {
                        timerDelay.Interval = 1000;
                        timerDelay.Start();
                        m_nStep = (int)TestProcess_WorkStep.执行步骤9;
                        break;
                    }
                    break;
                case TestProcess_WorkStep.执行步骤81:
                    if (timerDelay.Enabled == false)
                    {
                        timerDelay.Interval = 1000;
                        timerDelay.Start();
                        m_nStep = (int)TestProcess_WorkStep.执行步骤9;
                        break;
                    }
                    break;
                case TestProcess_WorkStep.执行步骤9:
                    if (timerDelay.Enabled == false)
                    {
                        timerDelay.Interval = 1000;
                        timerDelay.Start();
                        m_nStep = (int)TestProcess_WorkStep.Completed;
                        break;
                    }
                    break;
                case TestProcess_WorkStep.Completed:
                    if (timerDelay.Enabled == false)
                    {
                        timerDelay.Interval = 1000;
                        timerDelay.Start();
                        m_nStep = (int)TestProcess_WorkStep.Start;
                        break;
                    }
                    break;
            }
        }
    }
}
