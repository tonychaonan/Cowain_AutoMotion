using DevExpress.CodeParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cowain_AutoMotion
{
    public class SimulatorFlowManager
    {
        List<SimulatorFlowItem> simulatorFlowItems = new List<SimulatorFlowItem>();
        public SimulatorFlowManager()
        {

        }
        public void start()
        {
            foreach (SimulatParamItem item in SimulatDataDefine.instance().simulatParam.simulatParamItems)
            {
                SimulatorFlowItem simulatorFlowItem = new SimulatorFlowItem(item);
                simulatorFlowItems.Add(simulatorFlowItem);
            }
            foreach (var item in simulatorFlowItems)
            {
                item.start();
            }
        }
        public void stop()
        {
            foreach (var item in simulatorFlowItems)
            {
                item.stop();
            }
        }
    }
    public class SimulatorFlowItem
    {
        private bool b_Start = false;
        private bool b_Stop = false;
        private Thread thread;
        SimulatParamItem simulatParamItem;

        int index = 0;
        public SimulatorFlowItem(SimulatParamItem simulatParamItem1)
        {
            simulatParamItem = simulatParamItem1;
            thread = new Thread(work1);
            thread.IsBackground = true;
            thread.Start();
        }

        public void start()
        {
            b_Stop = false;
            b_Start = true;
        }
        public void stop()
        {
            b_Start = false;
            b_Stop = true;
            runItem.stop = true;
        }
        public SimulatorFlowItem()
        {
            thread = new Thread(work1);
            thread.IsBackground = true;
            thread.Start();
        }
        private void work1()
        {
            while (true)
            {
                Thread.Sleep(10);
                if (b_Stop)
                {
                    break;
                }
                if (b_Start != true)
                {
                    continue;
                }
                try
                {
                    work();
                }
                catch
                {

                }
            }
        }
        private void work()
        {
            foreach (var item in simulatParamItem.simulatParamItem1s)
            {
                StepEnum stepEnum = item.stepEnum;
                IfStep ifStep = IfStep.Null;
                ExecuteStep executeStep = ExecuteStep.Null;
                if (stepEnum == StepEnum.条件)
                {
                    bool b_Result = Enum.TryParse(item.stepStr, out ifStep);
                    if (b_Result != true)
                    {
                        ifStep = IfStep.Null;
                    }
                }
                else if (stepEnum == StepEnum.步骤)
                {
                    bool b_Result = Enum.TryParse(item.stepStr, out executeStep);
                    if (b_Result != true)
                    {
                        executeStep = ExecuteStep.Null;
                    }
                }
                runItem runItem = new runItem();
                runItem.simulatParamItem1 = item;
                //
                if (stepEnum == StepEnum.条件)
                {
                    switch (ifStep)
                    {
                        case IfStep.Null:
                            break;
                        case IfStep.等待输入IO:
                            runItem.run(等待输入IO);
                            break;
                        case IfStep.等待连续触发:
                            runItem.run(等待连续触发);
                            break;
                        case IfStep.等待TCP赋值不等于空:
                            runItem.run(等待TCP赋值不等于空);
                            break;
                        case IfStep.等待TCP赋值包含固定指令:
                            runItem.run(等待TCP赋值包含固定指令);
                            break;
                        case IfStep.等待RS232赋值不等于空:
                            runItem.run(等待RS232赋值不等于空);
                            break;
                        case IfStep.等待RS232赋值包含固定指令:
                            runItem.run(等待RS232赋值包含固定指令);
                            break;
                    }
                }
                else if (stepEnum == StepEnum.步骤)
                {
                    switch (executeStep)
                    {
                        case ExecuteStep.Null:
                            break;
                        case ExecuteStep.延时:
                            runItem.run(延时);
                            break;
                        case ExecuteStep.执行输入IO:
                            runItem.run(执行输入IO);
                            break;
                        case ExecuteStep.执行输出IO:
                            runItem.run(执行输出IO);
                            break;
                        case ExecuteStep.TCP字符串赋值:
                            runItem.run(TCP字符串赋值);
                            break;
                        case ExecuteStep.RS232字符串赋值:
                            runItem.run(RS232字符串赋值);
                            break;
                    }
                }
            }
        }
        private bool 等待输入IO(SimulatParamItem1 simulatParamItem1)
        {
            EnumParam_InputIO enumParam_InputIO;
            bool b_Input = Enum.TryParse(simulatParamItem1.instanceStr, out enumParam_InputIO);
            if (b_Input)
            {
                string value = HardWareControl.getInputIO(enumParam_InputIO).GetValue() ? "1" : "0";
                if (value == simulatParamItem1.valueStr)
                {
                    return true;
                }
            }
            return false;
        }
        private bool 等待连续触发(SimulatParamItem1 simulatParamItem1)
        {
            getContinueIOStatus.instance().startWork(simulatParamItem1);
            return getContinueIOStatus.instance().getIdel();
        }
        private bool 等待TCP赋值不等于空(SimulatParamItem1 simulatParamItem1)
        {
            EnumParam_ConnectionName enumParam_ConnectionName;
            bool b_Convert = Enum.TryParse(simulatParamItem1.instanceStr, out enumParam_ConnectionName);
            if (b_Convert)
            {
                if (HardWareControl.getSocketControl(enumParam_ConnectionName)?.returnStr != "")
                {
                    return true;
                }
            }
            return false;
        }
        private bool 等待TCP赋值包含固定指令(SimulatParamItem1 simulatParamItem1)
        {
            EnumParam_ConnectionName enumParam_ConnectionName;
            bool b_Convert = Enum.TryParse(simulatParamItem1.instanceStr, out enumParam_ConnectionName);
            if (b_Convert)
            {
                if (HardWareControl.getSocketControl(enumParam_ConnectionName)?.returnStr.Contains(simulatParamItem1.valueStr) == true)
                {
                    return true;
                }
            }
            return false;
        }
        private bool 等待RS232赋值不等于空(SimulatParamItem1 simulatParamItem1)
        {
            EnumParam_ConnectionName enumParam_ConnectionName;
            bool b_Convert = Enum.TryParse(simulatParamItem1.instanceStr, out enumParam_ConnectionName);
            if (b_Convert)
            {
                if (HardWareControl.getRS232Control(enumParam_ConnectionName)?.strRecData != "")
                {
                    return true;
                }
            }
            return false;
        }
        private bool 等待RS232赋值包含固定指令(SimulatParamItem1 simulatParamItem1)
        {
            EnumParam_ConnectionName enumParam_ConnectionName;
            bool b_Convert = Enum.TryParse(simulatParamItem1.instanceStr, out enumParam_ConnectionName);
            if (b_Convert)
            {
                if (HardWareControl.getRS232Control(enumParam_ConnectionName)?.strRecData.Contains(simulatParamItem1.valueStr) == true)
                {
                    return true;
                }
            }
            return false;
        }
        private bool 延时(SimulatParamItem1 simulatParamItem1)
        {
            int value = 10;
            bool b_Convert = int.TryParse(simulatParamItem1.valueStr, out value);
            if (b_Convert)
            {
                Thread.Sleep(value);
                return true;
            }
            return false;
        }
        private bool 执行输入IO(SimulatParamItem1 simulatParamItem1)
        {
            EnumParam_InputIO enumParam_InputIO;
            bool b_Input = Enum.TryParse(simulatParamItem1.instanceStr, out enumParam_InputIO);
            if (b_Input)
            {
                HardWareControl.getInputIO(enumParam_InputIO)?.SetIO(simulatParamItem1.valueStr == "1" ? true : false);
                return true;
            }
            return false;
        }
        private bool 执行输出IO(SimulatParamItem1 simulatParamItem1)
        {
            EnumParam_OutputIO enumParam_OutputIO;
            bool b_Input = Enum.TryParse(simulatParamItem1.instanceStr, out enumParam_OutputIO);
            if (b_Input)
            {
                HardWareControl.getOutputIO(enumParam_OutputIO)?.SetIO(simulatParamItem1.valueStr == "1" ? true : false);
                return true;
            }
            return false;
        }
        private bool TCP字符串赋值(SimulatParamItem1 simulatParamItem1)
        {
            EnumParam_ConnectionName enumParam_ConnectionName;
            bool b_Convert = Enum.TryParse(simulatParamItem1.instanceStr, out enumParam_ConnectionName);
            if (b_Convert)
            {
                HardWareControl.getSocketControl(enumParam_ConnectionName).returnStr = simulatParamItem1.valueStr;
                return true;
            }
            return false;
        }
        private bool RS232字符串赋值(SimulatParamItem1 simulatParamItem1)
        {
            EnumParam_ConnectionName enumParam_ConnectionName;
            bool b_Convert = Enum.TryParse(simulatParamItem1.instanceStr, out enumParam_ConnectionName);
            if (b_Convert)
            {
                HardWareControl.getRS232Control(enumParam_ConnectionName).strRecData = simulatParamItem1.valueStr;
                return true;
            }
            return false;
        }
    }
    public delegate bool runDel(SimulatParamItem1 simulatParamItem1);
    public class runItem
    {
        public SimulatParamItem1 simulatParamItem1;
        public static bool stop;
        public void run(runDel action1)
        {
            while (true)
            {
                Thread.Sleep(10);
                bool b_Result = action1(simulatParamItem1);
                if (b_Result)
                {
                    return;
                }
                if (stop)
                {
                    return;
                }
            }
        }
    }
    public class getContinueIOStatus
    {
        private static getContinueIOStatus getContinueIOStatus1;
        public static getContinueIOStatus instance()
        {
            if (getContinueIOStatus1 == null)
            {
                getContinueIOStatus1 = new getContinueIOStatus();
            }
            return getContinueIOStatus1;
        }
        /// <summary>
        /// 记录被询问有无信号的输入IO
        /// </summary>
        private EnumParam_InputIO ioStatus1 = 0;
        private bool getIdel1 = false;
        private DateTime lastTime = DateTime.Now;
        private bool b_Start = false;
        private double totalTime = 0;
        public void getIOStatus(EnumParam_InputIO io)
        {
            if (ioStatus1 == io)
            {
                if (b_Start == false)
                {
                    lastTime = DateTime.Now;
                    b_Start = true;
                }
                else
                {
                    if ((DateTime.Now - lastTime).TotalMilliseconds > totalTime)
                    {
                        getIdel1 = true;
                        HardWareControl.IOStatusDel -= getIOStatus;
                    }
                }
            }
        }
        public void startWork(SimulatParamItem1 simulatParamItem1)
        {
            getIdel1 = false;
            b_Start = false;
            EnumParam_InputIO inputIO;
            bool b_Result = Enum.TryParse(simulatParamItem1.instanceStr, out inputIO);
            double.TryParse(simulatParamItem1.valueStr, out totalTime);
            if (b_Result)
            {
                ioStatus1 = inputIO;
                HardWareControl.IOStatusDel += getIOStatus;
            }
        }
        public bool getIdel()
        {
            return getIdel1;
        }
    }
}
