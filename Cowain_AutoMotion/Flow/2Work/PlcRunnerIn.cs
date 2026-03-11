using Cowain_AutoMotion.Flow;
using Cowain_Form.FormView;
using Cowain_Machine;
using Cowain_Machine.Flow;
using MotionBase;
using Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Cowain_Machine.Flow.MErrorDefine;

namespace Cowain_AutoMotion
{
    public class PlcRunnerIn : Base
    {
        public PlcRunnerIn_HomeStep currentHomeStep;
        private PlcRunnerIn_WorkStep currentWorkStep;
        int ngTime = 0;
        public static string holderSN = "";
        public static string SN = "";
        public static bool PlcInComplete = false;
        
        /// <summary>
        /// 进料流道有料
        /// </summary>
        public static bool runInHiveProduct = false;
        bool b_Once = true;
        public PlcRunnerIn(Base parent) : base(parent, false)
        {

        }
        public enum PlcRunnerIn_HomeStep
        {
            Start = 0,
            气缸回原点,
            等待气缸回原点完成,
            Completed
        }
        public enum PlcRunnerIn_WorkStep
        {
            Start = 0,
            等待延时结束,
            PLC要料,
            流道要料结束,
            等待进料完成,
        }

        public override void HomeCycle(ref double dbTime)
        {
            currentHomeStep = (PlcRunnerIn_HomeStep)m_nHomeStep;
            switch (currentHomeStep)
            {
                case PlcRunnerIn_HomeStep.Start:
                    m_bHomeCompleted = false;
                    MachineDataDefine.是否感应 = false;
                    HardWareControl.getOutputIO(EnumParam_OutputIO.流道入料要料信号).SetIO(false);
                    m_nHomeStep = (int)PlcRunnerIn_HomeStep.Completed;
                    break;
                case PlcRunnerIn_HomeStep.Completed:
                    m_bHomeCompleted = true;
                    m_Status = 狀態.待命;
                    break;
            }
        }
        public override void StepCycle(ref double dbTime)
        {
            currentWorkStep = (PlcRunnerIn_WorkStep)m_nStep;
            switch (currentWorkStep)
            {
                case PlcRunnerIn_WorkStep.Start:
                    HardWareControl.getOutputIO(EnumParam_OutputIO.A流道步进运行).SetIO(true);
                    HardWareControl.getOutputIO(EnumParam_OutputIO.C流道步进运行).SetIO(true);
                    if (b_Once)
                    {
                        LogAuto.Notify("第一次作料！", (int)MachineStation.主监控, LogLevel.Info);
                        b_Once = false;
                        timerDelay.Interval = 1500;
                        timerDelay.Start();
                    }
                    m_nStep = (int)PlcRunnerIn_WorkStep.等待延时结束;
                    break;
                case PlcRunnerIn_WorkStep.等待延时结束:
                    if (timerDelay.Enabled)
                    {
                        break;
                    }
                    if (!HardWareControl.getInputIO(EnumParam_InputIO.入料口载具感应信号).GetValue())
                    {
                        LogAuto.Notify("等待料进来！", (int)MachineStation.主监控, LogLevel.Info);
                        ngTime = 0;
                        HardWareControl.getOutputIO(EnumParam_OutputIO.三色灯_绿灯).SetIO(true);
                        HardWareControl.getOutputIO(EnumParam_OutputIO.启动灯).SetIO(true);
                        //HardWareControl.getOutputIO(EnumParam_OutputIO.备用流道入料要料信号).SetIO(true);
                        //HardWareControl.getOutputIO(EnumParam_OutputIO.B流道步进运行).SetIO(true);
                        m_nStep = (int)PlcRunnerIn_WorkStep.PLC要料;
                    }
                    else
                    {
                        LogAuto.Notify("已经感应到来料！", (int)MachineStation.主监控, LogLevel.Info);
                        m_nStep = (int)PlcRunnerIn_WorkStep.流道要料结束;
                    }
                    break;
                case PlcRunnerIn_WorkStep.PLC要料:
                    LogAuto.Notify("触发PLC要料信号！", (int)MachineStation.主监控, LogLevel.Info);
                    HardWareControl.getOutputIO(EnumParam_OutputIO.流道入料要料信号).SetIO(true);
                    m_nStep = (int)PlcRunnerIn_WorkStep.流道要料结束;
                    break;
                case PlcRunnerIn_WorkStep.流道要料结束:
                    // bool b=  HardWareControl.getInputIO(EnumParam_InputIO.外部流道放料完成).GetValue();
                    //bool a = HardWareControl.getInputIO(EnumParam_InputIO.入料口载具感应信号).GetValue();
                    // LogAuto.Notify("入料口载具感应信号！" + b, (int)MachineStation.主监控, LogLevel.Info);
                    if ((HardWareControl.getInputIO(EnumParam_InputIO.入料口载具感应信号).GetValue() /*&& HardWareControl.getInputIO(EnumParam_InputIO.外部流道放料完成).GetValue()*/) || MachineDataDefine.machineState.b_UseNullRun)
                    {
                       MachineDataDefine.是否感应 = true;
                            LogAuto.Notify("流道要料结束并已接收到放料完成信号！", (int)MachineStation.主监控, LogLevel.Info);
                            frm_Main.formData.ChartTime1.SetTime();//有料进来重置时间，90秒没有料进来，就置为idel
                            HIVE.HIVEInstance.HIVEStarttime[0] = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");
                            HIVE.HIVEInstance.hivestarttime[0] = DateTime.Now;
                            if (MachineDataDefine.b_UseLAD != true)
                            {
                                LogAuto.Notify("不是LAD模式下有料流入后再将状态设置为running！", (int)MachineStation.主监控, LogLevel.Info);
                                frm_Main.formData.ChartTime1.StartRun();//有料流入后再将状态设置为running 23-03-31
                            }
                            HardWareControl.getOutputIO(EnumParam_OutputIO.流道入料要料信号).SetIO(false);
                        LogAuto.Notify("流道入料要料信号完成信号！", (int)MachineStation.主监控, LogLevel.Info);
                        PlcRunnerIn.PlcInComplete = true;
                            m_nStep = (int)PlcRunnerIn_WorkStep.等待进料完成;                     
                        }
                    break;
                case PlcRunnerIn_WorkStep.等待进料完成:
                    if (PlcRunnerIn.PlcInComplete == false)
                    {
                        MachineDataDefine.是否感应 = false;
                        LogAuto.Notify("等待进料完成！", (int)MachineStation.主监控, LogLevel.Info);
                        m_nStep = (int)PlcRunnerIn_WorkStep.Start;
                    }
                    break;
            }
        }
        public override void Stop()
        {
            HardWareControl.getOutputIO(EnumParam_OutputIO.流道入料要料信号).SetIO(false);
            HardWareControl.getOutputIO(EnumParam_OutputIO.A流道步进运行).SetIO(false);
            HardWareControl.getOutputIO(EnumParam_OutputIO.B流道步进运行).SetIO(false);
            HardWareControl.getOutputIO(EnumParam_OutputIO.C流道步进运行).SetIO(false);
            base.Stop();
        }
        public override bool DoStep(int iStep)
        {
            b_Once = true;
            return base.DoStep(iStep);
        }
    }
}
