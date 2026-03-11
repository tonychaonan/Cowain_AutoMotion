using Cowain_AutoMotion.Flow._2Work;
using MotionBase;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Cowain_AutoMotion
{
    public class WorkProcessLoad
    {
        private static WorkProcessLoad workProcessLoad;
        public static WorkProcessLoad instance
        {
            get
            {
                if(workProcessLoad==null)
                {
                    workProcessLoad = new WorkProcessLoad();
                }
                return workProcessLoad;
            }
        }
        public Dictionary<string, Base> processList = new Dictionary<string, Base>();
        //--------------------------------------------------
        public Mainflow workProcess_Mainflow;
        //public DriverEnter workProcess_DriverEnter;
        //public RunnerIn workProcess_RunnerIn;
        //public AxisTakeIn workProcess_AxisTakeIn;
        //public AxisTakeOut workProcess_AxisTakeOut;
        // public TestProcess testProcess;
        //--------------------------------------------------
        //public T getProcss<T>(T t)
        //{
        //    foreach (var item in collection)
        //    {

        //    }
        //}
        public void workProcessInitial(Base motionBase1)
        {
            //-----------------------作业流程
            workProcess_Mainflow = new Mainflow(typeof(Mainflow.Mainflow_HomeStep), typeof(Mainflow.Mainflow_WorkStep), "所有集合", motionBase1);
            motionBase1.AddBase(ref workProcess_Mainflow.m_NowAddress);
            //workProcess_DriverEnter = new DriverEnter(typeof(DriverEnter.DriverEnter_HomeStep), typeof(DriverEnter.DriverEnter_WorkStep), "Driver入料流程", motionBase1);
            //motionBase1.AddBase(ref workProcess_DriverEnter.m_NowAddress);
            //workProcess_RunnerIn = new RunnerIn(typeof(RunnerIn.RunnerIn_HomeStep), typeof(RunnerIn.RunnerIn_WorkStep), "Homepod入料流程", motionBase1);
            //motionBase1.AddBase(ref workProcess_RunnerIn.m_NowAddress);
            //workProcess_AxisTakeIn = new AxisTakeIn(typeof(AxisTakeIn.AxisTakeIn_HomeStep), typeof(AxisTakeIn.AxisTakeIn_WorkStep), "前龙门作料流程", motionBase1);
            //motionBase1.AddBase(ref workProcess_AxisTakeIn.m_NowAddress);
            //workProcess_AxisTakeOut = new AxisTakeOut(typeof(AxisTakeOut.AxisTakeOut_HomeStep), typeof(AxisTakeOut.AxisTakeOut_WorkStep), "后龙门作料流程", motionBase1);
            //motionBase1.AddBase(ref workProcess_AxisTakeOut.m_NowAddress);
            //  testProcess = new TestProcess(typeof(Base.HomeStep_Base), typeof(TestProcess.TestProcess_WorkStep), "测试流程", motionBase1);
            // motionBase1.AddBase(ref testProcess.m_NowAddress);






            //------------------------------
            processList.Clear();
            foreach (KeyValuePair<int,Base> item in motionBase1.ChildList)
            {
                processList.Add(item.Value.instanceName, item.Value);
            }
            //------------------------------
            //-------------获取步骤对应关系
           // StepRelationManager.instance.addStepRelation("D:\\数据备份1\\E盘\\视觉方案\\B520\\2024年改造\\Cowain_AutoMotion - 080\\Cowain_AutoMotion - 副本 (2)\\Cowain_AutoMotion\\Flow\\2Work\\TestProcess.cs", "测试流程");
        }
    }
}
