using Cowain_Machine;
using Cowain_Machine.Flow;
using MotionBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Cowain_AutoMotion.SQLSugarHelper;
using static Cowain_Machine.Flow.MErrorDefine;

namespace Cowain_AutoMotion.Flow._2Work
{
    public class AlignmentMode : Base
    {
        public AlignmentMode() : base()
        {

        }
        private AlignmentMode_WorkStep currentWorkStep;
        int index = 0;
        double speed = 30;
        public bool b_Result = true;
        public bool b_Recheck = false;
        public Dictionary<string, string> datas = new Dictionary<string, string>();
        public AlignmentMode_HomeStep currentHomeStep;
        public MachineData alignmengPoint = null;
        public enum AlignmentMode_HomeStep
        {
            Start = 0,
            调整位定位销固定顶升气缸上1,
            调整位膨胀气缸回,
            调整位膨胀顶升气缸下,
            调整位定位销固定顶升气缸下1,
            调整位压板限位气缸回,
            Completed
        }
        public enum AlignmentMode_WorkStep
        {
            Start = 0,
            调整位压板限位气缸出,
            调整位定位销固定顶升气缸上,
            调整位膨胀顶升气缸上,
            调整位膨胀气缸膨胀,
            调整位定位销固定顶升气缸下,
            触发相机拍照,
            等待相机反馈,
            判断超时时间,
            运动引导点位,
            等待引导点位运动完成,
            调整位定位销固定顶升气缸上1,
            调整位膨胀气缸回,
            调整位膨胀顶升气缸下,
            调整位定位销固定顶升气缸下1,
            调整位压板限位气缸回,
            Completed
        }
        public override void HomeCycle(ref double dbTime)
        {
            currentHomeStep = (AlignmentMode_HomeStep)m_nStep;
            switch (currentHomeStep)
            {
                case AlignmentMode_HomeStep.调整位定位销固定顶升气缸上1:
                    StaticParam.cylinderDictionary[EnumParam_Valve.调整位定位销固定顶升气缸上.ToString()].Open();
                    m_nStep = (int)AlignmentMode_HomeStep.调整位膨胀气缸回;
                    break;
                case AlignmentMode_HomeStep.调整位膨胀气缸回:
                    if (StaticParam.cylinderDictionary[EnumParam_Valve.调整位定位销固定顶升气缸上.ToString()].isIDLE())
                    {
                        StaticParam.cylinderDictionary[EnumParam_Valve.调整位膨胀气缸.ToString()].Close();
                        m_nStep = (int)AlignmentMode_HomeStep.调整位膨胀顶升气缸下;
                    }
                    break;
                case AlignmentMode_HomeStep.调整位膨胀顶升气缸下:
                    if (StaticParam.cylinderDictionary[EnumParam_Valve.调整位膨胀气缸.ToString()].isIDLE())
                    {
                        StaticParam.cylinderDictionary[EnumParam_Valve.调整位膨胀顶升气缸.ToString()].Close();
                        m_nStep = (int)AlignmentMode_HomeStep.调整位定位销固定顶升气缸下1;
                    }
                    break;
                case AlignmentMode_HomeStep.调整位定位销固定顶升气缸下1:
                    if (StaticParam.cylinderDictionary[EnumParam_Valve.调整位膨胀顶升气缸.ToString()].isIDLE())
                    {
                        StaticParam.cylinderDictionary[EnumParam_Valve.调整位定位销固定顶升气缸上.ToString()].Close();
                        m_nStep = (int)AlignmentMode_HomeStep.调整位压板限位气缸回;
                    }
                    break;
                case AlignmentMode_HomeStep.调整位压板限位气缸回:
                    if (StaticParam.cylinderDictionary[EnumParam_Valve.调整位定位销固定顶升气缸上.ToString()].isIDLE())
                    {
                        StaticParam.cylinderDictionary[EnumParam_Valve.调整位压板限位气缸.ToString()].Close();
                        m_nStep = (int)AlignmentMode_HomeStep.Completed;
                    }
                    break;
                case AlignmentMode_HomeStep.Completed:
                    if (StaticParam.cylinderDictionary[EnumParam_Valve.调整位压板限位气缸.ToString()].isIDLE())
                    {
                        m_Status = 狀態.待命;
                    }
                    break;
            }
            base.HomeCycle(ref dbTime);
        }
        public override void StepCycle(ref double dbTime)
        {
            currentWorkStep = (AlignmentMode_WorkStep)m_nStep;
            switch (currentWorkStep)
            {
                case AlignmentMode_WorkStep.Start:
                    index = 0;
                    b_Result = true;
                    b_Recheck = false;
                    foreach (var item in MachineDataDefine.machineDatas)
                    {
                        if(item.CName== EnumParam_Point.调整轴原点位.ToString())
                        {
                            alignmengPoint = item;
                            break;
                        }
                    }
                    m_nStep = (int)AlignmentMode_WorkStep.调整位压板限位气缸出;
                    break;
                case AlignmentMode_WorkStep.调整位压板限位气缸出:
                    StaticParam.cylinderDictionary[EnumParam_Valve.调整位压板限位气缸.ToString()].Open();
                    m_nStep = (int)AlignmentMode_WorkStep.调整位定位销固定顶升气缸上;
                    break;
                case AlignmentMode_WorkStep.调整位定位销固定顶升气缸上:
                    if (StaticParam.cylinderDictionary[EnumParam_Valve.调整位压板限位气缸.ToString()].isIDLE())
                    {
                        StaticParam.cylinderDictionary[EnumParam_Valve.调整位定位销固定顶升气缸上.ToString()].Open();
                        m_nStep = (int)AlignmentMode_WorkStep.调整位膨胀顶升气缸上;
                    }
                    break;
                case AlignmentMode_WorkStep.调整位膨胀顶升气缸上:
                    if (StaticParam.cylinderDictionary[EnumParam_Valve.调整位定位销固定顶升气缸上.ToString()].isIDLE())
                    {
                        LogAuto.Notify("横移X轴移动到待命位！", (int)MachineStation.主监控, LogLevel.Info);
                        double 横移X1轴待命位 = StaticParam.Points[(int)EnumPosition.横移X1轴待待命位].X;
                        StaticParam.AxisX1.AbsMove(横移X1轴待命位, MachineDataDefine.ChkStatus.machineSpeed);
                        StaticParam.cylinderDictionary[EnumParam_Valve.调整位膨胀顶升气缸.ToString()].Open();
                        m_nStep = (int)AlignmentMode_WorkStep.调整位膨胀气缸膨胀;
                    }
                    break;
                case AlignmentMode_WorkStep.调整位膨胀气缸膨胀:
                    if (StaticParam.cylinderDictionary[EnumParam_Valve.调整位膨胀顶升气缸.ToString()].isIDLE())
                    {
                        StaticParam.cylinderDictionary[EnumParam_Valve.调整位膨胀气缸.ToString()].Open();
                        m_nStep = (int)AlignmentMode_WorkStep.调整位定位销固定顶升气缸下;
                    }
                    break;
                case AlignmentMode_WorkStep.调整位定位销固定顶升气缸下:
                    if(StaticParam.cylinderDictionary[EnumParam_Valve.调整位膨胀气缸.ToString()].isIDLE())
                    {
                       // StaticParam.cylinderDictionary[EnumParam_Valve.调整位定位销固定顶升气缸上.ToString()].Close();
                        m_nStep = (int)AlignmentMode_WorkStep.触发相机拍照;
                    }
                    break;
                case AlignmentMode_WorkStep.触发相机拍照:
                    timerDelay.Interval = 5000;
                    timerDelay.Start();
                    MachineDataDefine.CCDSocket.StrBack = "";
                    string SN = "";
                    if (MachineDataDefine.ChkStatus.b_UseTestRun)
                    {
                        SN = "1234567890";
                    }
                    else
                    {
                        SN = MESDataDefine.SN;
                    }
                    MachineDataDefine.CCDSocket.SendMsg("@,start,Adjustment," + SN + "," + index.ToString() + ",#");
                    m_nStep = (int)AlignmentMode_WorkStep.等待相机反馈;
                    break;
                case AlignmentMode_WorkStep.等待相机反馈:
                    if (MachineDataDefine.CCDSocket.StrBack != "")
                    {
                        string[] result = MachineDataDefine.CCDSocket.StrBack.Split(',');
                        if (result[2].ToLower() == "ng")//ng不上传
                        {
                            b_Result = false;
                            m_nStep = (int)AlignmentMode_WorkStep.调整位定位销固定顶升气缸上1;
                            break;
                        }
                        else
                        {
                            datas.Clear();
                            datas.Add("OffsetX", result[4]);
                            datas.Add("OffsetY", result[5]);
                            datas.Add("OffsetR", result[6]);
                            datas.Add("Blob1", result[7]);
                            datas.Add("Blob2", result[8]);
                            datas.Add("Blob3", result[9]);

                            datas.Add("P1", result[10]);
                            datas.Add("P2", result[11]);
                            datas.Add("P3", result[12]);
                            datas.Add("P4", result[13]);
                            datas.Add("P5", result[14]);
                            datas.Add("P6", result[15]);
                            datas.Add("P7", result[16]);
                            datas.Add("P8", result[17]);

                            datas.Add("ShiftP1P5", result[18]);
                            datas.Add("ShiftP2P6", result[19]);
                            datas.Add("ShiftP3P7", result[20]);
                            datas.Add("ShiftP4P8", result[21]);

                            //判断是否是复检
                            if(b_Recheck)
                            {
                                b_Recheck = false;
                                m_nStep = (int)AlignmentMode_WorkStep.调整位定位销固定顶升气缸下1;
                                break;
                            }
                            else
                            {
                                if (result[3] == "1")//1 允许下料,0 不允许下料
                                {
                                    b_Recheck = true;
                                    m_nStep = (int)AlignmentMode_WorkStep.调整位定位销固定顶升气缸上1;
                                }
                                else
                                {
                                    m_nStep = (int)AlignmentMode_WorkStep.判断超时时间;
                                }
                            }
                        }
                    }
                    else if (timerDelay.Enabled == false)
                    {
                        Error pError = new Error(ref this.m_NowAddress, "CCD未返回数据", "", (int)MErrorCode.CCD_Capture1異常);
                        pError.AddErrSloution("Retry", (int)AlignmentMode_WorkStep.触发相机拍照);
                        pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    }
                    break;
                case AlignmentMode_WorkStep.判断超时时间:
                    double totalTime = (DateTime.Now - HIVE.HIVEInstance.hivestoptime[0]).TotalSeconds;
                    if (MachineDataDefine.MachineCfgS.outTime< totalTime)//超时，则下料
                    {
                        m_nStep = (int)AlignmentMode_WorkStep.调整位定位销固定顶升气缸上1;
                    }
                    else
                    {
                        m_nStep = (int)AlignmentMode_WorkStep.运动引导点位;
                    }
                    break;
                case AlignmentMode_WorkStep.运动引导点位:
                    //如果当前点位，超过了设定的运动范围,则报警下料
                    double difX = Math.Abs(StaticParam.AxisX2.GetPosition()- alignmengPoint.Data1);
                    double difY = Math.Abs(StaticParam.AxisX2.GetPosition() - alignmengPoint.Data2);
                    double difR = Math.Abs(StaticParam.AxisX2.GetPosition() - alignmengPoint.Data4);
                    bool b_True = true;
                    if(difX>MachineDataDefine.MachineCfgS.moveXMax)
                    {
                        b_True = false;
                        LogAuto.Notify("调整X轴调整范围超限！", (int)MachineStation.主监控, LogLevel.Alarm);
                    }
                    if (difY > MachineDataDefine.MachineCfgS.moveYMax)
                    {
                        b_True = false;
                        LogAuto.Notify("调整Y轴调整范围超限！", (int)MachineStation.主监控, LogLevel.Alarm);
                    }
                    if (difR > MachineDataDefine.MachineCfgS.moveRMax)
                    {
                        b_True = false;
                        LogAuto.Notify("调整R轴调整范围超限！", (int)MachineStation.主监控, LogLevel.Alarm);
                    }
                    if(b_True!=true)
                    {
                        b_Result = false;
                        m_nStep = (int)AlignmentMode_WorkStep.调整位定位销固定顶升气缸上1;
                        break;
                    }
                    double offsetX =Convert.ToDouble(datas["OffsetX"]);
                    double offsetY = Convert.ToDouble(datas["OffsetY"]);
                    double offsetR = Convert.ToDouble(datas["OffsetR"]);
                    StaticParam.AxisX2.RevMove(offsetX, speed);
                    StaticParam.AxisY3.RevMove(offsetY, speed);
                    StaticParam.AxisR.RevMove(offsetR, speed);
                    m_nStep = (int)AlignmentMode_WorkStep.等待引导点位运动完成;
                    break;
                case AlignmentMode_WorkStep.等待引导点位运动完成:
                    if(StaticParam.AxisX2.isIDLE()&& StaticParam.AxisY3.isIDLE()&& StaticParam.AxisR.isIDLE())
                    {
                        m_nStep = (int)AlignmentMode_WorkStep.触发相机拍照;
                    }
                    break;
                case AlignmentMode_WorkStep.调整位定位销固定顶升气缸上1:
                    StaticParam.cylinderDictionary[EnumParam_Valve.调整位定位销固定顶升气缸上.ToString()].Open();
                    m_nStep = (int)AlignmentMode_WorkStep.调整位膨胀气缸回;
                    break;
                case AlignmentMode_WorkStep.调整位膨胀气缸回:
                    if (StaticParam.cylinderDictionary[EnumParam_Valve.调整位定位销固定顶升气缸上.ToString()].isIDLE())
                    {
                        StaticParam.cylinderDictionary[EnumParam_Valve.调整位膨胀气缸.ToString()].Close();
                        m_nStep = (int)AlignmentMode_WorkStep.调整位膨胀顶升气缸下;
                    }
                    break;
                case AlignmentMode_WorkStep.调整位膨胀顶升气缸下:
                    if(StaticParam.cylinderDictionary[EnumParam_Valve.调整位膨胀气缸.ToString()].isIDLE())
                    {
                        StaticParam.cylinderDictionary[EnumParam_Valve.调整位膨胀顶升气缸.ToString()].Close();
                        if(b_Recheck!=true)
                        {
                            m_nStep = (int)AlignmentMode_WorkStep.调整位定位销固定顶升气缸下1;
                        }
                        else
                        {
                            m_nStep = (int)AlignmentMode_WorkStep.触发相机拍照;
                        }
                    }
                    break;
                case AlignmentMode_WorkStep.调整位定位销固定顶升气缸下1:
                    if(StaticParam.cylinderDictionary[EnumParam_Valve.调整位膨胀顶升气缸.ToString()].isIDLE())
                    {
                        StaticParam.cylinderDictionary[EnumParam_Valve.调整位定位销固定顶升气缸上.ToString()].Close();
                        m_nStep = (int)AlignmentMode_WorkStep.调整位压板限位气缸回;
                    }
                    break;
                case AlignmentMode_WorkStep.调整位压板限位气缸回:
                    if(StaticParam.cylinderDictionary[EnumParam_Valve.调整位定位销固定顶升气缸上.ToString()].isIDLE())
                    {
                        StaticParam.cylinderDictionary[EnumParam_Valve.调整位压板限位气缸.ToString()].Close();
                        m_nStep = (int)AlignmentMode_WorkStep.Completed;
                    }
                    break;
                case AlignmentMode_WorkStep.Completed:
                    if(StaticParam.cylinderDictionary[EnumParam_Valve.调整位压板限位气缸.ToString()].isIDLE())
                    {
                        m_Status = 狀態.待命;
                    }
                    break;
            }
        }
        public bool Action()
        {
            m_Status = 狀態.動作中;
            m_nStep = (int)AlignmentMode_WorkStep.Start;
            return base.DoStep(m_nStep);
        }
        public override void Stop()
        {
            StaticParam.AxisX2.Stop();
            StaticParam.AxisY3.Stop();
            StaticParam.AxisR.Stop();
            base.Stop();
        }
        public override bool DoHomeStep(int iHomeStep)
        {
            m_Status = 狀態.動作中;
            return base.DoHomeStep(0);
        }
    }
}
