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
    public class DriverEnter : Base
    {
        public DriverEnter_HomeStep currentHomeStep;
        private DriverEnter_WorkStep currentWorkStep;
        public DriverEnter(Type homeEnum1, Type stepEnum1, string instanceName1, Base parent) : base( homeEnum1, stepEnum1, instanceName1, parent, false)
        {

        }
        /// <summary>
        /// 放入Driver数量
        /// </summary>
        public static int driverNum = 0;

        /// <summary>
        /// 让前龙门到哪个穴位取料
        /// </summary>
        public static int holelocation = 0;

        /// <summary>
        /// 前龙门可取料变量
        /// </summary>
        public static bool PickDriverProduct = false;
        /// <summary>
        /// 存放driver1SN
        /// </summary>
        public static string driver1SN = "";

        public static string _driver1SN { set { driver1SN = value; } }
        /// <summary>
        /// 存放driver2SN
        /// </summary>
        public static string driver2SN = "";

        public static string _driver2SN { set { driver2SN = value; } }


        private string driver1SN1 = "";

        public enum DriverEnter_HomeStep
        {
            Start = 0,
            气缸回原点,
            等待气缸回原点完成,
            Completed
        }
        public enum DriverEnter_WorkStep
        {
            Start = 0,
            GetSN,
            光栅被触发,
            光栅未触发,
            对射感应,
            左侧Driver进料滑台气缸到位,
            等待前龙门取料,
            等待前龙门取料2,
            左侧Driver进料滑台气缸回,
            左侧Driver进料滑台气缸回到位,
            Completed
        }
        public override void HomeCycle(ref double dbTime)
        {
            currentHomeStep = (DriverEnter_HomeStep)m_nHomeStep;
            switch (currentHomeStep)
            {
                case DriverEnter_HomeStep.Start:
                    driverNum = 0;
                    holelocation = 0;
                    PickDriverProduct = false;
                    m_bHomeCompleted = false;
                    m_nHomeStep = (int)DriverEnter_HomeStep.气缸回原点;
                    break;
                case DriverEnter_HomeStep.气缸回原点:
                    HardWareControl.getValve(EnumParam_Valve.左侧Driver进料滑台气缸).Close();
                    m_nHomeStep = (int)DriverEnter_HomeStep.等待气缸回原点完成;
                    break;
                case DriverEnter_HomeStep.等待气缸回原点完成:
                    if (HardWareControl.getValve(EnumParam_Valve.左侧Driver进料滑台气缸).isIDLE())
                    {
                        m_nHomeStep = (int)DriverEnter_HomeStep.Completed;
                    }
                    break;
                case DriverEnter_HomeStep.Completed:
                    m_bHomeCompleted = true;
                    LogAuto.Notify("送料工位复位完成！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    m_Status = 狀態.待命;
                    break;
            }
        }
        public override void StepCycle(ref double dbTime)
        {
            currentWorkStep = (DriverEnter_WorkStep)m_nStep;
            switch (currentWorkStep)
            {
                case DriverEnter_WorkStep.Start:
                    {
                        LogAuto.Notify("判断光栅是否被触发！", (int)MachineStation.主监控, MotionLogLevel.Info);

                        m_nStep = (int)DriverEnter_WorkStep.GetSN;
                    }
                    break;
                case DriverEnter_WorkStep.GetSN:
                  
                        //if (MachineDataDefine.machineState.b_UseScaner != true)
                        //{
                        //    LogAuto.Notify("driver不启用扫码枪！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        //    m_nStep = (int)DriverEnter_WorkStep.光栅被触发;
                        //    break;
                        //}
                        if (!MachineDataDefine.machineState.b_UseClear)
                        {
                            if (driver1SN != "" && driver2SN != "")
                            {
                                LogAuto.Notify("非清料模式获取到SN判断光栅是否被触发！", (int)MachineStation.主监控, MotionLogLevel.Info);
                                m_nStep = (int)DriverEnter_WorkStep.光栅被触发;
                            }
                        }
                        else  //清料模式（只有一颗料时）
                        {
                            if (driver1SN != "")//driver1是里面的那个穴位
                            {
                                LogAuto.Notify("清料模式获取到SN判断光栅是否被触发！", (int)MachineStation.主监控, MotionLogLevel.Info);
                                m_nStep = (int)DriverEnter_WorkStep.光栅被触发;
                            }
                        }
                  
                    break;
                case DriverEnter_WorkStep.光栅被触发:
                    if (!HardWareControl.getInputIO(EnumParam_InputIO.光栅).GetValue())
                    {
                        LogAuto.Notify("光栅被触发无信号！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        m_nStep = (int)DriverEnter_WorkStep.光栅未触发;
                    }
                    break;
                case DriverEnter_WorkStep.光栅未触发:
                    if (HardWareControl.getInputIO(EnumParam_InputIO.光栅).GetValue())
                    {
                        LogAuto.Notify("光栅未触发无信号！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        m_nStep = (int)DriverEnter_WorkStep.对射感应;
                    }
                    break;

                case DriverEnter_WorkStep.对射感应:

                   
                    if (!MachineDataDefine.machineState.b_UseClear)
                    {
                        if (HardWareControl.getInputIO(EnumParam_InputIO.左侧Driver进料对射感应1).GetValue() && HardWareControl.getInputIO(EnumParam_InputIO.左侧Driver进料对射感应2).GetValue())
                        {
                            LogAuto.Notify("左侧Driver进料对射感应1&&左侧Driver进料对射感应2！", (int)MachineStation.主监控, MotionLogLevel.Info);
                            driverNum = 2;
                            HardWareControl.getValve(EnumParam_Valve.左侧Driver进料滑台气缸).Open();
                            m_nStep = (int)DriverEnter_WorkStep.左侧Driver进料滑台气缸到位;
                        }
                    }
                    else
                    {
                        if (HardWareControl.getInputIO(EnumParam_InputIO.左侧Driver进料对射感应1).GetValue())
                        {
                            LogAuto.Notify("清料模式左侧Driver进料对射感应1", (int)MachineStation.主监控, MotionLogLevel.Info);
                            driverNum = 1;
                            HardWareControl.getValve(EnumParam_Valve.左侧Driver进料滑台气缸).Open();
                            m_nStep = (int)DriverEnter_WorkStep.左侧Driver进料滑台气缸到位;
                        }
                    }
                    break;
                case DriverEnter_WorkStep.左侧Driver进料滑台气缸到位:
                    if (HardWareControl.getValve(EnumParam_Valve.左侧Driver进料滑台气缸).isIDLE())
                    {
                        LogAuto.Notify("左侧Driver进料滑台气缸到位,前龙门可取料第一个穴！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        holelocation = 0;
                        PickDriverProduct = true;
                        m_nStep = (int)DriverEnter_WorkStep.等待前龙门取料;
                    }
                    break;
                case DriverEnter_WorkStep.等待前龙门取料:
                    if (!PickDriverProduct)  //前龙门取料完成后会把此变量置为fasle
                    {
                        LogAuto.Notify("前龙门取料完成！", (int)MachineStation.主监控, MotionLogLevel.Info);

                        if (driverNum == 1)
                        {
                            LogAuto.Notify("清料模式，前龙门取料完成气缸退出！", (int)MachineStation.主监控, MotionLogLevel.Info);

                            m_nStep = (int)DriverEnter_WorkStep.左侧Driver进料滑台气缸回;
                        }
                        else if (driverNum != 1)
                        {
                            LogAuto.Notify("不是清料模式，继续给前龙门发取料信号可取料第二个穴！", (int)MachineStation.主监控, MotionLogLevel.Info);
                            holelocation = 1;
                            PickDriverProduct = true;
                            m_nStep = (int)DriverEnter_WorkStep.等待前龙门取料2;
                        }
                    }
                    break;
                case DriverEnter_WorkStep.等待前龙门取料2:

                    if (!PickDriverProduct)  //前龙门取料完成后会把此变量置为fasle
                    {
                        m_nStep = (int)DriverEnter_WorkStep.左侧Driver进料滑台气缸回;
                    }
                    break;
                case DriverEnter_WorkStep.左侧Driver进料滑台气缸回:
                    HardWareControl.getValve(EnumParam_Valve.左侧Driver进料滑台气缸).Open();
                    driver1SN = "";
                    driver2SN = "";
                    m_nStep = (int)DriverEnter_WorkStep.左侧Driver进料滑台气缸回到位;
                    break;
                case DriverEnter_WorkStep.左侧Driver进料滑台气缸回到位:
                    if (HardWareControl.getValve(EnumParam_Valve.左侧Driver进料滑台气缸).isIDLE())
                    {
                        m_nStep = (int)DriverEnter_WorkStep.Start;
                    }
                    break;

            }
        }
        public override void Stop()
        {
            driverNum = 0;
            holelocation = 0;
            PickDriverProduct = false;
            driver1SN = "";
            driver2SN = "";
            base.Stop();
        }
    }
}
