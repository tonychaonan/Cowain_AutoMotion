using Cowain_AutoMotion.Flow;
using Cowain_Form.FormView;
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
    public class RunnerOut : Base
    {
        public RunnerOut_HomeStep currentHomeStep;
        private RunnerOut_WorkStep currentWorkStep;
        /// <summary>
        /// 允许出料放料信号
        /// </summary>
        public static bool outputDownProduct = true;
        /// <summary>
        ///     启动第一次作料
        /// </summary>
        public static bool first = true;
        public RunnerOut(Base parent) : base(parent, false)
        {

        }
        public enum RunnerOut_HomeStep
        {
            Start = 0,
            气缸复位,
            出料口位后顶升气缸复位,
            等待气缸复位完成,
            Completed
        }
        public enum RunnerOut_WorkStep
        {
            Start = 0,
            等待允许放料信号为false,
            出料口位气缸下降,
            出料口位阻挡气缸回,
            等待气缸下降完成,
            等待光电感应到物料,
            出料口位阻挡气缸出,
            出料口位前顶升气缸上,
            气缸2顶升,
            机械手取料,
            等待机械手取料完成,
            切手动
        }
        public override void HomeCycle(ref double dbTime)
        {
            currentHomeStep = (RunnerOut_HomeStep)m_nHomeStep;
            switch (currentHomeStep)
            {
                case RunnerOut_HomeStep.Start:
                    first = true;
                    m_bHomeCompleted = false;
                    HardWareControl.getOutputIO(EnumParam_OutputIO.机械手取料信号).SetIO(false);
                    m_nHomeStep = (int)RunnerOut_HomeStep.气缸复位;
                    break;
                case RunnerOut_HomeStep.气缸复位:
                    HardWareControl.getValve(EnumParam_Valve.出料口位前顶升气缸上).Open();
                    HardWareControl.getValve(EnumParam_Valve.出料口位阻挡气缸).Open();
                   
                    m_nHomeStep = (int)RunnerOut_HomeStep.出料口位后顶升气缸复位;
                    break;
                case RunnerOut_HomeStep.出料口位后顶升气缸复位:
                   
                    if (HardWareControl.getValve(EnumParam_Valve.出料口位阻挡气缸).isIDLE())
                    {
                        HardWareControl.getValve(EnumParam_Valve.出料口位后顶升气缸上).Close();
                        m_nHomeStep = (int)RunnerOut_HomeStep.等待气缸复位完成;
                    }
                    
                    break;
                case RunnerOut_HomeStep.等待气缸复位完成:
                    if (HardWareControl.getValve(EnumParam_Valve.出料口位前顶升气缸上).isIDLE()
                        && HardWareControl.getValve(EnumParam_Valve.出料口位阻挡气缸).isIDLE()
                        && HardWareControl.getValve(EnumParam_Valve.出料口位后顶升气缸上).isIDLE()
                       )
                    {
                        m_nHomeStep = (int)RunnerOut_HomeStep.Completed;
                    }
                    break;
                case RunnerOut_HomeStep.Completed:
                    m_bHomeCompleted = true;
                    LogAuto.Notify("流道出料工位复位完成！", (int)MachineStation.主监控, LogLevel.Info);
                    m_Status = 狀態.待命;
                    break;
            }
        }
        public override void StepCycle(ref double dbTime)
        {
            currentWorkStep = (RunnerOut_WorkStep)m_nStep;
            switch (currentWorkStep)
            {
                case RunnerOut_WorkStep.Start:    
                    if (HardWareControl.getInputIO(EnumParam_InputIO.出料口位后到位信号).GetValue()&& first==true)
                    {
                        
                       // bool b=  HardWareControl.getInputIO(EnumParam_InputIO.出料口位后到位信号).GetValue();
                        LogAuto.Notify("启动时，出料口位后顶升气缸上！", (int)MachineStation.主监控, LogLevel.Info);
                        HardWareControl.getValve(EnumParam_Valve.出料口位后顶升气缸上).Open();
                        m_nStep = (int)RunnerOut_WorkStep.机械手取料;
                       
                    }
                    else if (HardWareControl.getInputIO(EnumParam_InputIO.出料口位阻挡感应信号).GetValue() && first == true)
                    {
                        LogAuto.Notify("启动时，出料口位阻挡气缸回！", (int)MachineStation.主监控, LogLevel.Info);
                        m_nStep = (int)RunnerOut_WorkStep.出料口位阻挡气缸回;
                       
                    }
                    //else if(HardWareControl.getInputIO(EnumParam_InputIO.出料龙门位到位信号).GetValue() && first == true)
                    //{
                    //    LogAuto.Notify("启动时，出料口位气缸下降！", (int)MachineStation.主监控, LogLevel.Info);
                    //    m_nStep = (int)RunnerOut_WorkStep.出料口位气缸下降;
                       
                    //}
                    else
                    {
                        m_nStep = (int)RunnerOut_WorkStep.等待允许放料信号为false;
                    }
                    first = false;
                    break;
                case RunnerOut_WorkStep.等待允许放料信号为false:
                    //if(RunnerIn.b_OutProduct)//抛料
                    //{
                    //    RunnerIn.b_OutProduct = false;
                    //    HardWareControl.getValve(EnumParam_Valve.出料口位前顶升气缸上).Close();
                    //    m_nStep = (int)RunnerOut_WorkStep.出料口位阻挡气缸回;
                    //    break;
                    //}
                    if (RunnerOut.outputDownProduct == false)
                    {
                        m_nStep = (int)RunnerOut_WorkStep.出料口位气缸下降;
                    }
                    break;
                case RunnerOut_WorkStep.出料口位气缸下降:
                    bool B = HardWareControl.getInputIO(EnumParam_InputIO.出料口位阻挡感应信号).GetValue();
                    LogAuto.Notify("出料口位前顶升气缸上返回值！"+B, (int)MachineStation.主监控, LogLevel.Info);
                    if (!HardWareControl.getInputIO(EnumParam_InputIO.出料口位阻挡感应信号).GetValue() || MachineDataDefine.machineState.b_UseNullRun)
                    {
                        LogAuto.Notify("出料口位前顶升气缸上！", (int)MachineStation.主监控, LogLevel.Info);
                        HardWareControl.getValve(EnumParam_Valve.出料口位前顶升气缸上).Close();
                        timerDelay.Enabled = false;
                        timerDelay.Interval = 2000;
                        timerDelay.Start();
                        m_nStep = (int)RunnerOut_WorkStep.出料口位阻挡气缸回;
                    }
                    break;
                case RunnerOut_WorkStep.出料口位阻挡气缸回:
                    //bool b = HardWareControl.getInputIO(EnumParam_InputIO.出料口位阻挡感应信号).GetValue();
                    bool a = HardWareControl.getValve(EnumParam_Valve.出料口位前顶升气缸上).isIDLE();
                    if(a!=true)
                    {
                       
                        break;
                    }
                    //if ((HardWareControl.getInputIO(EnumParam_InputIO.出料口位阻挡感应信号).GetValue() && !HardWareControl.getInputIO(EnumParam_InputIO.出料口位后到位信号).GetValue()) || MachineDataDefine.machineState.b_UseNullRun)
                    if (timerDelay.Enabled == false)
                    {
                        LogAuto.Notify("出料口位前顶升气缸上&&出料口位阻挡气缸下！", (int)MachineStation.主监控, LogLevel.Info);
                        HardWareControl.getValve(EnumParam_Valve.出料口位前顶升气缸上).Open();
                        HardWareControl.getValve(EnumParam_Valve.出料口位阻挡气缸).Close();
                        //HardWareControl.getValve(EnumParam_Valve.出料口位后顶升气缸上).Close();
                        //  HardWareControl.getValve(EnumParam_Valve.出料口位上下阻挡气缸).Close();
                        m_nStep = (int)RunnerOut_WorkStep.等待气缸下降完成;
                    }

                    break;
                case RunnerOut_WorkStep.等待气缸下降完成:
                    if (HardWareControl.getValve(EnumParam_Valve.出料口位前顶升气缸上).isIDLE()
                        &&
                        /* && HardWareControl.getValve(EnumParam_Valve.出料口位后顶升气缸上).isIDLE()*/
                        HardWareControl.getValve(EnumParam_Valve.出料口位阻挡气缸).isIDLE()
                        /*&& HardWareControl.getValve(EnumParam_Valve.出料口位上下阻挡气缸).isIDLE()*/)
                    {
                        LogAuto.Notify("后龙门可放料！", (int)MachineStation.主监控, LogLevel.Info);
                        RunnerOut.outputDownProduct = true;
                        m_nStep = (int)RunnerOut_WorkStep.等待光电感应到物料;
                    }
                    break;
                case RunnerOut_WorkStep.等待光电感应到物料:
                    if (HardWareControl.getInputIO(EnumParam_InputIO.出料口位后到位信号).GetValue() || MachineDataDefine.machineState.b_UseNullRun)
                    {
                        timerDelay.Enabled = false;
                        timerDelay.Interval = 1000;
                        timerDelay.Start();
                       
                        m_nStep = (int)RunnerOut_WorkStep.出料口位阻挡气缸出;
                    }
                    break;
                case RunnerOut_WorkStep.出料口位阻挡气缸出:
                    if (timerDelay.Enabled == false)
                    {
                        LogAuto.Notify("出料口位阻挡气缸上！", (int)MachineStation.主监控, LogLevel.Info);
                        //HardWareControl.getValve(EnumParam_Valve.出料口位前顶升气缸上).Open();
                        HardWareControl.getValve(EnumParam_Valve.出料口位阻挡气缸).Open();
                        timerDelay.Enabled = false;
                        timerDelay.Interval = 500;
                        timerDelay.Start();
                        m_nStep = (int)RunnerOut_WorkStep.气缸2顶升;
                    }
                    break;
                case RunnerOut_WorkStep.气缸2顶升:
                    if (timerDelay.Enabled == false)
                    {
                        LogAuto.Notify("出料口位后顶升气缸上！", (int)MachineStation.主监控, LogLevel.Info);
                        HardWareControl.getValve(EnumParam_Valve.出料口位后顶升气缸上).Open();
                        m_nStep = (int)RunnerOut_WorkStep.机械手取料;
                    }
                    break;
                case RunnerOut_WorkStep.机械手取料:
                    if (HardWareControl.getValve(EnumParam_Valve.出料口位后顶升气缸上).isIDLE())
                    {
                        
                        if (MachineDataDefine.machineState.b_UseRobot)
                        {
                            LogAuto.Notify("启用机械手，给机械手取料信号！", (int)MachineStation.主监控, LogLevel.Info);
                            HardWareControl.getOutputIO(EnumParam_OutputIO.机械手取料信号).SetIO(true);
                        }
                        //给机械手发送IO
                        m_nStep = (int)RunnerOut_WorkStep.等待机械手取料完成;
                    }
                    break;
                case RunnerOut_WorkStep.等待机械手取料完成:

                    if (MachineDataDefine.machineState.b_UseRobot)
                    {
                        LogAuto.Notify("等待机械手取料完成！", (int)MachineStation.主监控, LogLevel.Info);
                        if (HardWareControl.getInputIO(EnumParam_InputIO.出料口位后到位信号).GetValue() != true && HardWareControl.getInputIO(EnumParam_InputIO.机械手取料完成信号).GetValue())
                        {
                            //  LogAuto.Notify("机械手取料信号关闭！" + bb, (int)MachineStation.主监控, LogLevel.Info);
                            LogAuto.Notify("机械手取料信号关闭&&出料口位后顶升气缸下！", (int)MachineStation.主监控, LogLevel.Info);
                            //取料完成后，把气缸下降
                            HardWareControl.getOutputIO(EnumParam_OutputIO.机械手取料信号).SetIO(false);
                            HardWareControl.getValve(EnumParam_Valve.出料口位后顶升气缸上).Close();
                            m_nStep = (int)RunnerOut_WorkStep.Start;
                            break;
                        }
                        if (MachineDataDefine.RobotDownError)
                        {
                            LogAuto.Notify("机械手宕机！", (int)MachineStation.主监控, LogLevel.Info);
                            HardWareControl.getOutputIO(EnumParam_OutputIO.机械手取料信号).SetIO(false);
                            Error pError = new Error(ref this.m_NowAddress, "机械手宕机", "", (int)MErrorCode.后站机械手宕机);
                            pError.AddErrSloution("Retry", (int)RunnerOut_WorkStep.机械手取料);
                            pError.AddErrSloution("手动取料", (int)RunnerOut_WorkStep.切手动);
                            pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                            break;
                        }

                    }
                    else
                    {
                      //  LogAuto.Notify("未启用机械手！", (int)MachineStation.主监控, LogLevel.Info);
                        if (HardWareControl.getInputIO(EnumParam_InputIO.出料口位后到位信号).GetValue() != true)
                        {
                            LogAuto.Notify("未启用机械手，出料口位后顶升气缸下！", (int)MachineStation.主监控, LogLevel.Info);
                            HardWareControl.getValve(EnumParam_Valve.出料口位后顶升气缸上).Close();

                            //RunnerOut.outputDownProduct = true;
                            m_nStep = (int)RunnerOut_WorkStep.Start;
                        }
                    }
                    break;

                case RunnerOut_WorkStep.切手动:
                    MachineDataDefine.machineState.b_UseRobot = false;
                    HardWareControl.getOutputIO(EnumParam_OutputIO.机械手取料信号).SetIO(false);
                   // MachineDataDefine.RobotDownForm = false;
                    m_nStep = (int)RunnerOut_WorkStep.等待机械手取料完成;
                    break;
            }


        }

    }
}
