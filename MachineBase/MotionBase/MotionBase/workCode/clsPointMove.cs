using MotionBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Cowain_AutoMotion.SQLSugarHelper;

namespace Cowain_AutoMotion
{
    public class clsPointMove : Base
    {
        private PointMove_HomeStep currentHomeStep;
        private PointMove_WorkStep currentWorkStep;
        MachineData machineData;
        DrvMotor[] drvMotors = new DrvMotor[5];//XYZRA
        double[] PriorityDatas = new double[5];
        double[] DataNoUses = new double[5];
        double[] datas = new double[5];
        double[] speeds = new double[5] { 100, 100, 100, 100, 100 };
        int index = 0;
        public clsPointMove(Type homeEnum1, Type stepEnum1, string instanceName1) 
            : base( homeEnum1,  stepEnum1,  instanceName1)
        {
        }
        public enum PointMove_HomeStep
        {
            Start = 0,
            Completed
        }
        public enum PointMove_WorkStep
        {
            Start = 0,
            运动到安全位,
            等待安全位运动结束,
            按照轴的顺序开始运动,
            等待轴运动结束,
            Completed
        }
        public override void HomeCycle(ref double dbTime)
        {
            currentHomeStep = (PointMove_HomeStep)m_nHomeStep;
            switch (currentHomeStep)
            {
                case PointMove_HomeStep.Start:
                    m_nHomeStep = (int)PointMove_HomeStep.Completed;
                    break;
                case PointMove_HomeStep.Completed:
                    m_Status = 狀態.待命;
                    break;
            }
            base.HomeCycle(ref dbTime);
        }
        public override void StepCycle(ref double dbTime)
        {
            currentWorkStep = (PointMove_WorkStep)m_nStep;
            switch (currentWorkStep)
            {
                case PointMove_WorkStep.Start:
                    index = 0;
                    //  LogAuto.Notify("点位" + machineData.CName + "开始运动", (int)MachineStation.主监控, LogLevel.Info);
                    m_nStep = (int)PointMove_WorkStep.运动到安全位;
                    break;
                case PointMove_WorkStep.运动到安全位:
                    if (machineData.ZToSafe == 1)
                    {
                        if (machineData != null)
                        {
                            EnumParam_Point point1;
                            bool b_Result = Enum.TryParse(machineData.ZSafePoint, out point1);
                            if (b_Result)
                            {
                                HardWareControl.movePoint(point1);
                            }
                        }
                    }
                    m_nStep = (int)PointMove_WorkStep.等待安全位运动结束;
                    break;
                case PointMove_WorkStep.等待安全位运动结束:
                    if (machineData.ZToSafe == 1)
                    {
                        EnumParam_Point point1;
                        bool b_Result = Enum.TryParse(machineData.ZSafePoint.Trim(), out point1);
                        if (b_Result)
                        {
                            bool idel1 = HardWareControl.getPointIdel(point1);
                            if (idel1)
                            {
                                m_nStep = (int)PointMove_WorkStep.按照轴的顺序开始运动;
                            }
                        }
                    }
                    else
                    {
                        m_nStep = (int)PointMove_WorkStep.按照轴的顺序开始运动;
                    }
                    break;
                case PointMove_WorkStep.按照轴的顺序开始运动:
                    for (int i = 0; i < drvMotors.Length; i++)
                    {
                        if (PriorityDatas[i] == (index + 1) && DataNoUses[i] == 0 && drvMotors[i] != null)
                        {
                            drvMotors[i].AbsMove(datas[i], speeds[i]);
                        }
                    }
                    m_nStep = (int)PointMove_WorkStep.等待轴运动结束;
                    break;
                case PointMove_WorkStep.等待轴运动结束:
                    bool b_Idel = true;
                    for (int i = 0; i < drvMotors.Length; i++)
                    {
                        if (PriorityDatas[i] == (index + 1) && DataNoUses[i] == 0 && drvMotors[i] != null)
                        {
                            b_Idel = b_Idel && drvMotors[i].isIDLE();
                        }
                    }
                    if (b_Idel)
                    {
                        if (index == drvMotors.Length - 1)
                        {
                            m_nStep = (int)PointMove_WorkStep.Completed;
                        }
                        else
                        {
                            index++;
                            m_nStep = (int)PointMove_WorkStep.按照轴的顺序开始运动;
                        }
                    }
                    break;
                case PointMove_WorkStep.Completed:
                    // LogAuto.Notify("点位" + machineData.CName + "完成运动", (int)MachineStation.主监控, LogLevel.Info);
                    m_Status = 狀態.待命;
                    break;
            }
            base.StepCycle(ref dbTime);
        }
        public override bool DoHomeStep(int iHomeStep)
        {
            m_Status = 狀態.回HOME中;
            m_nStep = (int)PointMove_HomeStep.Start;
            return base.DoHomeStep(iHomeStep);
        }
        public bool Action(int iStep, MachineData machineData1)
        {
            m_Status = 狀態.動作中;
            m_nStep = (int)PointMove_WorkStep.Start;
            machineData = machineData1;
            StationParam stationParam = null;
            foreach (var item in BaseDataDefine.stationParams)
            {
                if (machineData1.StationName == item.CName)
                {
                    stationParam = item;
                    break;
                }
            }
            setMotorValue(ref drvMotors[0], stationParam.XData1);
            setMotorValue(ref drvMotors[1], stationParam.YData2);
            setMotorValue(ref drvMotors[2], stationParam.ZData3);
            setMotorValue(ref drvMotors[3], stationParam.RData4);
            setMotorValue(ref drvMotors[4], stationParam.AData5);
            speeds[0] = BaseDataDefine.machineSpeed * machineData.Data1Speed / 100.0;
            speeds[1] = BaseDataDefine.machineSpeed * machineData.Data2Speed / 100.0;
            speeds[2] = BaseDataDefine.machineSpeed * machineData.Data3Speed / 100.0;
            speeds[3] = BaseDataDefine.machineSpeed * machineData.Data4Speed / 100.0;
            speeds[4] = BaseDataDefine.machineSpeed * machineData.Data5Speed / 100.0;
            PriorityDatas[0] = machineData.PriorityData1;
            PriorityDatas[1] = machineData.PriorityData2;
            PriorityDatas[2] = machineData.PriorityData3;
            PriorityDatas[3] = machineData.PriorityData4;
            PriorityDatas[4] = machineData.PriorityData5;
            DataNoUses[0] = machineData.Data1NoUse;
            DataNoUses[1] = machineData.Data2NoUse;
            DataNoUses[2] = machineData.Data3NoUse;
            DataNoUses[3] = machineData.Data4NoUse;
            DataNoUses[4] = machineData.Data5NoUse;
            datas[0] = machineData.Data1;
            datas[1] = machineData.Data2;
            datas[2] = machineData.Data3;
            datas[3] = machineData.Data4;
            datas[4] = machineData.Data5;
            return base.DoStep(iStep);
        }
        public override void Stop()
        {
            //foreach (var item in drvMotors)
            //{
            //    item?.Stop();
            //}
            m_Status = 狀態.待命;
            base.Stop();
        }
        private void setMotorValue(ref DrvMotor drvMotor, string data1)
        {
            drvMotor = null;
            foreach (var item in Base.GetMotorList())
            {
                if (item.Key == data1.Trim())
                {
                    drvMotor = item.Value;
                    break;
                }
            }
        }
    }
}
