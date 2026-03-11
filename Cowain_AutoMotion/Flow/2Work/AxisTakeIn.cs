using Cowain_AutoMotion.Flow;
using Cowain_AutoMotion.Flow._2Work;
using Cowain_Machine;
using MotionBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Cowain_AutoMotion.AxisCalibration;
using static Cowain_Machine.Flow.MErrorDefine;

namespace Cowain_AutoMotion
{
    public class AxisTakeIn : Base
    {
        private AxisTakeIn_HomeStep currentHomeStep;
        private AxisTakeIn_WorkStep currentWorkStep;
        public AxisCalibration axisCalibration;
        MachineDataDefine define = new MachineDataDefine();
        /// <summary>
        /// 产品
        /// </summary>
        private ProductPoint _productPoint = new ProductPoint();         
        /// <summary>
        /// 前龙门存储DriverSN
        /// </summary>
        public static string holeSN = "";

        /// <summary>
        /// 前龙门存储产品SN
        /// </summary>
        public static string ProductSN = "";
        /// <summary>
        /// 准备打螺丝
        /// </summary>
        public static bool electricReady = false;

        /// <summary>
        /// 开始打螺丝
        /// </summary>
        public static bool electricStart = false;
        /// <summary>
        /// 轴运行速度
        /// </summary>
        double speed = 80;
        /// <summary>
        /// 统计作料数量
        /// </summary>
        public int productCount = 0;
        public static bool b_Result = true ;
        public delegate void SNShowDelegate(string sn, bool ok);
        public event SNShowDelegate SNShowEvent;
        public AxisTakeIn(Type homeEnum1, Type stepEnum1, string instanceName1, Base parent) : base(homeEnum1,stepEnum1,instanceName1, parent, false)
        {
           
            axisCalibration = new Cowain_AutoMotion.AxisCalibration(typeof(Base.HomeStep_Base),typeof(AxisCalibration_WorkStep),"标定流程",this);
            AddBase(ref axisCalibration.m_NowAddress);
        }
        public enum AxisTakeIn_HomeStep
        {
            Start = 0,
            气缸回原,
            Z轴回原点,
            XY轴回原点,
            Completed
        }
        public enum AxisTakeIn_WorkStep
        {
            Start = 0,
            运动到待命位,
            等待有料信号,
            轴运动到XY取料位1到位,
            轴运动到XY取料位2到位,
            轴高速运动到Z取料位,
            轴低速运动到Z取料位,
            夹取气缸夹,
            气缸打开,
            Z轴低速抬起,
            Z轴高速抬起,
            Z轴到位,
            前龙门取料完成,
            触发相机拍照1,
            等待相机反馈1,
            运动到下相机拍照位2,
            触发相机拍照2,
            等待相机反馈2,
            下拍照完成计时,
            前龙门可放料,
            前龙门移动到组装拍照位1,
            触发上相机拍照1,
            等待上相机反馈1,
            前龙门移动到组装拍照位2,
            触发上相机拍照2,
            等待上相机反馈2,
            前龙门移动到组装拍照位3,
            触发上相机拍照3,
            等待上相机反馈3,
            前龙门移动到组装拍照位4,
            触发上相机拍照4,
            等待上相机反馈4,
            触发相机计算,
            接收相机反馈结果,
            准备打螺丝,
            前龙门移动放料位XYR,
            前龙门高速移动到放料位,
            前龙门低速移动到放料位,
            前龙门放料位到位,
            锁完第一个螺丝,
            前龙门避让,
            前龙门避让位到位,
            可复检,
            前龙门移动到复检位1,
            复检位1拍照,
            等待上相机复检反馈1,
            前龙门移动到复检位2,
            复检位2拍照,
            等待上相机复检反馈2,
            前龙门移动到复检位3,
            复检位3拍照,
            等待上相机复检反馈3,
            前龙门移动到复检位4,
            复检位4拍照,
            等待上相机复检反馈4,
            复检结束,
            Completed
        }
        public override void HomeCycle(ref double dbTime)
        {
            currentHomeStep = (AxisTakeIn_HomeStep)m_nHomeStep;
            switch (currentHomeStep)
            {
                case AxisTakeIn_HomeStep.Start:

                    holeSN = "";
                    ProductSN = "";
                    electricReady = false;
                    electricStart = false;
                    b_Result = true;

                    m_bHomeCompleted = false;
                    //   HardWareControl.getMotor(EnumParam_Axis.Z1).DoHome();
                    LogAuto.Notify("前龙门气缸&真空开始复位！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    HardWareControl.getValve(EnumParam_Valve.Driver夹取气缸ON).Close();
                    HardWareControl.getValve(EnumParam_Valve.取螺丝吸真空电磁阀ON).Close();
                    HardWareControl.getValve(EnumParam_Valve.标定吸真空电磁阀ON).Close();
                    
                    m_nHomeStep = (int)AxisTakeIn_HomeStep.Z轴回原点;
                    break;

                case AxisTakeIn_HomeStep.Z轴回原点:
                    LogAuto.Notify("前龙门Z轴回原点开始复位！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    HardWareControl.getMotor(EnumParam_Axis.Z1).DoHome();
                    m_nHomeStep = (int)AxisTakeIn_HomeStep.XY轴回原点;
                    break;


                case AxisTakeIn_HomeStep.XY轴回原点:
                    if (HardWareControl.getMotor(EnumParam_Axis.Z1).isHomeCompleted())
                    {
                        HardWareControl.getMotor(EnumParam_Axis.X1).DoHome();
                        HardWareControl.getMotor(EnumParam_Axis.Y1).DoHome();
                        HardWareControl.getMotor(EnumParam_Axis.R).DoHome();
                        m_nHomeStep = (int)AxisTakeIn_HomeStep.Completed;
                    }
                    break;
                case AxisTakeIn_HomeStep.Completed:
                    if (HardWareControl.getMotor(EnumParam_Axis.X1).isHomeCompleted() && HardWareControl.getMotor(EnumParam_Axis.Y1).isHomeCompleted() && HardWareControl.getMotor(EnumParam_Axis.R).isHomeCompleted())
                    {
                        m_bHomeCompleted = true;
                        m_Status = 狀態.待命;
                    }
                    break;
            }
            base.HomeCycle(ref dbTime);
        }
        public override void StepCycle(ref double dbTime)
        {
            currentWorkStep = (AxisTakeIn_WorkStep)m_nStep;
            switch (currentWorkStep)
            {
                case AxisTakeIn_WorkStep.Start:
                    LogAuto.Notify("Driver夹取气缸—ON关闭！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    HardWareControl.getValve(EnumParam_Valve.Driver夹取气缸ON).Close();
                    m_nStep = (int)AxisTakeIn_WorkStep.等待有料信号;
                    break;
                case AxisTakeIn_WorkStep.等待有料信号:
                    if (DriverEnter.PickDriverProduct && HardWareControl.getValve(EnumParam_Valve.Driver夹取气缸ON).isIDLE())
                    {
                        LogAuto.Notify("接收到送料Driver取料信号！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        if (DriverEnter.holelocation == 0)
                        {
                            LogAuto.Notify("把第一个穴里的driverSN传给前龙门！", (int)MachineStation.主监控, MotionLogLevel.Info);
                            holeSN = DriverEnter.driver1SN;
                            LogAuto.Notify("前龙门去第一个穴抓取driver！", (int)MachineStation.主监控, MotionLogLevel.Info);
                            HardWareControl.movePoint(EnumParam_Point.前龙门XY取料位1);
                            m_nStep = (int)AxisTakeIn_WorkStep.轴运动到XY取料位1到位;
                        }
                        else if (DriverEnter.holelocation == 1)
                        {
                            LogAuto.Notify("把第二个穴里的driverSN传给前龙门！", (int)MachineStation.主监控, MotionLogLevel.Info);
                            holeSN = DriverEnter.driver2SN;
                            HardWareControl.movePoint(EnumParam_Point.前龙门XY取料位2);
                            m_nStep = (int)AxisTakeIn_WorkStep.轴运动到XY取料位2到位;
                        }

                    }
                    break;

                case AxisTakeIn_WorkStep.轴运动到XY取料位1到位:
                    if (HardWareControl.getPointIdel(EnumParam_Point.前龙门XY取料位1))
                    {
                        LogAuto.Notify("轴运动到XY取料位1到位！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        m_nStep = (int)AxisTakeIn_WorkStep.轴高速运动到Z取料位;
                    }
                    break;
                case AxisTakeIn_WorkStep.轴运动到XY取料位2到位:
                    if (HardWareControl.getPointIdel(EnumParam_Point.前龙门XY取料位2))
                    {
                        LogAuto.Notify("轴运动到XY取料位2到位！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        m_nStep = (int)AxisTakeIn_WorkStep.轴高速运动到Z取料位;
                    }
                    break;
                case AxisTakeIn_WorkStep.轴高速运动到Z取料位:
                    LogAuto.Notify("轴高速运动到Z取料位！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    HardWareControl.movePoint(EnumParam_Point.前龙门Z取料位H);
                    m_nStep = (int)AxisTakeIn_WorkStep.轴低速运动到Z取料位;
                    break;
                case AxisTakeIn_WorkStep.轴低速运动到Z取料位:
                    if (HardWareControl.getPointIdel(EnumParam_Point.前龙门Z取料位H))
                    {
                        LogAuto.Notify("轴低速运动到Z取料位！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.movePoint(EnumParam_Point.前龙门Z取料位L);
                        m_nStep = (int)AxisTakeIn_WorkStep.夹取气缸夹;
                    }
                    break;
                case AxisTakeIn_WorkStep.夹取气缸夹:
                    if (HardWareControl.getPointIdel(EnumParam_Point.前龙门Z取料位L))
                    {
                        LogAuto.Notify("龙门位取料夹爪气缸夹！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.getValve(EnumParam_Valve.Driver夹取气缸ON).Open();
                        m_nStep = (int)AxisTakeIn_WorkStep.气缸打开;
                    }
                    break;
                case AxisTakeIn_WorkStep.气缸打开:
                    if (HardWareControl.getValve(EnumParam_Valve.Driver夹取气缸ON).isIDLE())
                    {
                        LogAuto.Notify("入料龙门位取料夹爪气缸夹到位！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        timerDelay.Enabled = false;
                        timerDelay.Interval = 500;
                        timerDelay.Start();
                        m_nStep = (int)AxisTakeIn_WorkStep.Z轴低速抬起;
                    }
                    break;

                case AxisTakeIn_WorkStep.Z轴低速抬起:
                    if (timerDelay.Enabled == false)
                    {
                        LogAuto.Notify("Z轴缓慢抬起位！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.movePoint(EnumParam_Point.Z轴抬起位L);
                        m_nStep = (int)AxisTakeIn_WorkStep.Z轴高速抬起;
                    }
                    break;
                case AxisTakeIn_WorkStep.Z轴高速抬起:
                    if (HardWareControl.getPointIdel(EnumParam_Point.Z轴抬起位L))
                    {
                        LogAuto.Notify("轴高速运动到Z高度即下相机拍照高度！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.movePoint(EnumParam_Point.前龙门Z下相机拍照位);
                        m_nStep = (int)AxisTakeIn_WorkStep.Z轴到位;
                    }
                    break;

                case AxisTakeIn_WorkStep.Z轴到位:
                    if (HardWareControl.getPointIdel(EnumParam_Point.前龙门Z下相机拍照位))
                    {
                        LogAuto.Notify("前龙门Z下相机拍照位到位！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        m_nStep = (int)AxisTakeIn_WorkStep.前龙门取料完成;
                    }
                    break;
                case AxisTakeIn_WorkStep.前龙门取料完成:
                    LogAuto.Notify("前龙门取料完成！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    DriverEnter.PickDriverProduct = false;
                    LogAuto.Notify("前龙门移动到拍照位1！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    HardWareControl.movePoint(EnumParam_Point.前龙门拍照位1);
                    m_nStep = (int)AxisTakeIn_WorkStep.触发相机拍照1;
                    break;
                case AxisTakeIn_WorkStep.触发相机拍照1:
                    if (HardWareControl.getPointIdel(EnumParam_Point.前龙门拍照位1))
                    {
                        
                        LogAuto.Notify("获取轴当前坐标！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        double axisX = HardWareControl.getMotor(EnumParam_Axis.X1).GetPosition();
                        double axisY = HardWareControl.getMotor(EnumParam_Axis.Y1).GetPosition();
                        double axisR = HardWareControl.getMotor(EnumParam_Axis.R).GetPosition();
                        HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr = "";
                        LogAuto.Notify("给相机发送拍照指令！" + "T1,1" + "," + axisX + "," + axisY + "," + axisR + "," + holeSN, (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).SendMSG("T1,1" + "," + axisX + "," + axisY + "," + axisR + "," + holeSN);

                        timerDelay.Enabled = false;
                        timerDelay.Interval = 10000;
                        timerDelay.Start();
                        m_nStep = (int)AxisTakeIn_WorkStep.等待相机反馈1;
                    }
                    break;

                case AxisTakeIn_WorkStep.等待相机反馈1:
                    if (HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr != "")
                    {
                        string ccdResult = HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr;
                        LogAuto.Notify("相机返回结果！" + ccdResult, (int)MachineStation.主监控, MotionLogLevel.Info);
                        string[] ccd = ccdResult.Split(',');

                        if (ccd[2] == "1")//结果ok
                        {
                            LogAuto.Notify("相机返回OK！" + ccdResult, (int)MachineStation.主监控, MotionLogLevel.Info);

                            m_nStep = (int)AxisTakeIn_WorkStep.运动到下相机拍照位2;

                        }
                        else
                        {
                            b_Result = false;
                            LogAuto.Notify("相机返回NG！" + ccdResult, (int)MachineStation.主监控, MotionLogLevel.Alarm);
                            Error pError = new Error(ref this.m_NowAddress, "相机返回NG", "", (int)MErrorCode.CCD_Capture1異常);
                            pError.AddErrSloution("Retry", (int)AxisTakeIn_WorkStep.触发相机拍照1);
                            pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                        }

                    }

                    else if (timerDelay.Enabled == false)
                    {
                        b_Result = false;
                        LogAuto.Notify("CCD未返回数据！", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                        Error pError = new Error(ref this.m_NowAddress, "CCD未返回数据", "", (int)MErrorCode.CCD_Capture1異常);
                        pError.AddErrSloution("Retry", (int)AxisTakeIn_WorkStep.触发相机拍照1);
                        pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    }
                    break;


                case AxisTakeIn_WorkStep.运动到下相机拍照位2:
                    LogAuto.Notify("前龙门移动到前龙门拍照位2！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    HardWareControl.movePoint(EnumParam_Point.前龙门拍照位2);
                    m_nStep = (int)AxisTakeIn_WorkStep.触发相机拍照2;

                    break;

                case AxisTakeIn_WorkStep.触发相机拍照2:
                    if (HardWareControl.getPointIdel(EnumParam_Point.前龙门拍照位2))
                    {
                        
                        LogAuto.Notify("获取下相机拍照位2轴当前坐标！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        double axisX = HardWareControl.getMotor(EnumParam_Axis.X1).GetPosition();
                        double axisY = HardWareControl.getMotor(EnumParam_Axis.Y1).GetPosition();
                        double axisR = HardWareControl.getMotor(EnumParam_Axis.R).GetPosition();
                        HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr = "";
                        LogAuto.Notify("给相机发送拍照指令！" + "T1,2" + "," + axisX + "," + axisY + "," + axisR + "," + holeSN, (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).SendMSG("T1,2" + "," + axisX + "," + axisY + "," + axisR + "," + holeSN);

                        timerDelay.Enabled = false;
                        timerDelay.Interval = 10000;
                        timerDelay.Start();
                        m_nStep = (int)AxisTakeIn_WorkStep.等待相机反馈2;
                    }
                    break;

                case AxisTakeIn_WorkStep.等待相机反馈2:
                    if (HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr != "")
                    {
                        string ccdResult = HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr;
                        LogAuto.Notify("下相机拍照2返回结果！" + ccdResult, (int)MachineStation.主监控, MotionLogLevel.Info);
                        string[] ccd = ccdResult.Split(',');

                        if (ccd[2] == "1")//结果ok
                        {
                            LogAuto.Notify("下相机拍照2返回OK！" + ccdResult, (int)MachineStation.主监控, MotionLogLevel.Info);

                            m_nStep = (int)AxisTakeIn_WorkStep.下拍照完成计时;

                        }
                        else
                        {
                            b_Result = false;
                            LogAuto.Notify("下相机拍照2返回NG！" + ccdResult, (int)MachineStation.主监控, MotionLogLevel.Alarm);
                            Error pError = new Error(ref this.m_NowAddress, "下相机拍照2返回NG", "", (int)MErrorCode.CCD_Capture1異常);
                            pError.AddErrSloution("Retry", (int)AxisTakeIn_WorkStep.触发相机拍照2);
                            pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                        }

                    }

                    else if (timerDelay.Enabled == false)
                    {
                        b_Result = false;
                        LogAuto.Notify("下CCD拍照2未返回数据！", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                        Error pError = new Error(ref this.m_NowAddress, "下CCD拍照2未返回数据", "", (int)MErrorCode.CCD_Capture1異常);
                        pError.AddErrSloution("Retry", (int)AxisTakeIn_WorkStep.触发相机拍照2);
                        pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    }
                    break;

                case AxisTakeIn_WorkStep.下拍照完成计时:

                    LogAuto.Notify("下相机拍照完成开始计时，判断从拍照完成到组装的时间是否超时（5分钟）！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    timerDelay.Enabled = false;
                    timerDelay.Interval = 300000;
                    timerDelay.Start();
                    m_nStep = (int)AxisTakeIn_WorkStep.前龙门可放料;

                    break;

                case AxisTakeIn_WorkStep.前龙门可放料:
                    if (RunnerIn.PutProduct == true)
                    {
                        ProductSN = RunnerIn.SN;
                        LogAuto.Notify("前龙门可放料！并把流道sn赋值给前龙门" + ":" + RunnerIn.SN + "," + ProductSN, (int)MachineStation.主监控, MotionLogLevel.Info);
                        SNShowEvent(holeSN+","+ProductSN,true);
                        m_nStep = (int)AxisTakeIn_WorkStep.前龙门移动到组装拍照位1;
                    }

                    else if (timerDelay.Enabled == false)
                    {
                        b_Result = false;
                        MachineDataDefine.timeout = true;
                        LogAuto.Notify("下相机拍照完成到组装超时（5分钟）！", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                        Error pError = new Error(ref this.m_NowAddress, "超时！！！点击停止按钮抓取气缸会打开，请人工接料！！！", "", (int)MErrorCode.组装超时报警);
                        pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);

                    }
                    break;
                case AxisTakeIn_WorkStep.前龙门移动到组装拍照位1:

                    LogAuto.Notify("前龙门移动到组装拍照位1！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    HardWareControl.movePoint(EnumParam_Point.组装拍照位1);
                    m_nStep = (int)AxisTakeIn_WorkStep.触发上相机拍照1;
                    break;

                case AxisTakeIn_WorkStep.触发上相机拍照1:
                    if (HardWareControl.getPointIdel(EnumParam_Point.组装拍照位1))
                    {
                       
                        LogAuto.Notify("获取上相机拍照位1轴当前坐标！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        double axisX = HardWareControl.getMotor(EnumParam_Axis.X1).GetPosition();
                        double axisY = HardWareControl.getMotor(EnumParam_Axis.Y1).GetPosition();
                        double axisR = HardWareControl.getMotor(EnumParam_Axis.R).GetPosition();
                        HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr = "";
                        LogAuto.Notify("给上相机发送拍照指令1！" + "T2,1" + "," + axisX + "," + axisY + "," + axisR + "," + ProductSN, (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).SendMSG("T2,1" + "," + axisX + "," + axisY + "," + axisR + "," + ProductSN);

                        timerDelay.Enabled = false;
                        timerDelay.Interval = 10000;
                        timerDelay.Start();
                        m_nStep = (int)AxisTakeIn_WorkStep.等待上相机反馈1;
                    }
                    break;

                case AxisTakeIn_WorkStep.等待上相机反馈1:
                    if (HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr != "")
                    {
                        string ccdResult = HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr;
                        LogAuto.Notify("上相机拍照1返回结果！" + ccdResult, (int)MachineStation.主监控, MotionLogLevel.Info);
                        string[] ccd = ccdResult.Split(',');

                        if (ccd[2] == "1")//结果ok
                        {
                            LogAuto.Notify("上相机拍照1返回OK！" + ccdResult, (int)MachineStation.主监控, MotionLogLevel.Info);

                            m_nStep = (int)AxisTakeIn_WorkStep.前龙门移动到组装拍照位2;

                        }
                        else
                        {
                            b_Result = false;
                            LogAuto.Notify("上相机拍照1返回NG！" + ccdResult, (int)MachineStation.主监控, MotionLogLevel.Alarm);
                            Error pError = new Error(ref this.m_NowAddress, "上相机拍照1返回NG", "", (int)MErrorCode.CCD_Capture1異常);
                            pError.AddErrSloution("Retry", (int)AxisTakeIn_WorkStep.触发上相机拍照1);
                            pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                        }

                    }

                    else if (timerDelay.Enabled == false)
                    {
                        b_Result = false;
                        LogAuto.Notify("上CCD拍照1未返回数据！", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                        Error pError = new Error(ref this.m_NowAddress, "上CCD拍照1未返回数据", "", (int)MErrorCode.CCD_Capture1異常);
                        pError.AddErrSloution("Retry", (int)AxisTakeIn_WorkStep.触发上相机拍照1);
                        pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    }
                    break;

                case AxisTakeIn_WorkStep.前龙门移动到组装拍照位2:

                    LogAuto.Notify("前龙门移动到组装拍照位2！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    HardWareControl.movePoint(EnumParam_Point.组装拍照位2);
                    m_nStep = (int)AxisTakeIn_WorkStep.触发上相机拍照2;
                    break;

                case AxisTakeIn_WorkStep.触发上相机拍照2:
                    if (HardWareControl.getPointIdel(EnumParam_Point.组装拍照位2))
                    {
                        
                        LogAuto.Notify("获取上相机拍照位2轴当前坐标！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        double axisX = HardWareControl.getMotor(EnumParam_Axis.X1).GetPosition();
                        double axisY = HardWareControl.getMotor(EnumParam_Axis.Y1).GetPosition();
                        double axisR = HardWareControl.getMotor(EnumParam_Axis.R).GetPosition();
                        HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr = "";
                        LogAuto.Notify("给上相机发送拍照指令2！" + "T2,2" + "," + axisX + "," + axisY + "," + axisR + "," + ProductSN, (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).SendMSG("T2,2" + "," + axisX + "," + axisY + "," + axisR + "," + ProductSN);

                        timerDelay.Enabled = false;
                        timerDelay.Interval = 10000;
                        timerDelay.Start();
                        m_nStep = (int)AxisTakeIn_WorkStep.等待上相机反馈2;
                    }
                    break;

                case AxisTakeIn_WorkStep.等待上相机反馈2:
                    if (HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr != "")
                    {
                        string ccdResult = HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr;
                        LogAuto.Notify("上相机拍照2返回结果！" + ccdResult, (int)MachineStation.主监控, MotionLogLevel.Info);
                        string[] ccd = ccdResult.Split(',');

                        if (ccd[2] == "1")//结果ok
                        {
                            LogAuto.Notify("上相机拍照2返回OK！" + ccdResult, (int)MachineStation.主监控, MotionLogLevel.Info);

                            m_nStep = (int)AxisTakeIn_WorkStep.前龙门移动到组装拍照位3;

                        }
                        else
                        {
                            b_Result = false;
                            LogAuto.Notify("上相机拍照2返回NG！" + ccdResult, (int)MachineStation.主监控, MotionLogLevel.Alarm);
                            Error pError = new Error(ref this.m_NowAddress, "上相机拍照2返回NG", "", (int)MErrorCode.CCD_Capture1異常);
                            pError.AddErrSloution("Retry", (int)AxisTakeIn_WorkStep.触发上相机拍照2);
                            pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                        }

                    }

                    else if (timerDelay.Enabled == false)
                    {
                        b_Result = false;
                        LogAuto.Notify("上CCD拍照2未返回数据！", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                        Error pError = new Error(ref this.m_NowAddress, "上CCD拍照2未返回数据", "", (int)MErrorCode.CCD_Capture1異常);
                        pError.AddErrSloution("Retry", (int)AxisTakeIn_WorkStep.触发上相机拍照2);
                        pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    }
                    break;
                case AxisTakeIn_WorkStep.前龙门移动到组装拍照位3:

                    LogAuto.Notify("前龙门移动到组装拍照位1！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    HardWareControl.movePoint(EnumParam_Point.组装拍照位3);
                    m_nStep = (int)AxisTakeIn_WorkStep.触发上相机拍照3;
                    break;

                case AxisTakeIn_WorkStep.触发上相机拍照3:
                    if (HardWareControl.getPointIdel(EnumParam_Point.组装拍照位3))
                    {
                       
                        LogAuto.Notify("获取上相机拍照位3轴当前坐标！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        double axisX = HardWareControl.getMotor(EnumParam_Axis.X1).GetPosition();
                        double axisY = HardWareControl.getMotor(EnumParam_Axis.Y1).GetPosition();
                        double axisR = HardWareControl.getMotor(EnumParam_Axis.R).GetPosition();
                        HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr = "";
                        LogAuto.Notify("给上相机发送拍照指令3！" + "T2,3" + "," + axisX + "," + axisY + "," + axisR + "," + ProductSN, (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).SendMSG("T2,3" + "," + axisX + "," + axisY + "," + axisR + "," + ProductSN);

                        timerDelay.Enabled = false;
                        timerDelay.Interval = 10000;
                        timerDelay.Start();
                        m_nStep = (int)AxisTakeIn_WorkStep.等待上相机反馈3;
                    }
                    break;

                case AxisTakeIn_WorkStep.等待上相机反馈3:
                    if (HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr != "")
                    {
                        string ccdResult = HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr;
                        LogAuto.Notify("上相机拍照3返回结果！" + ccdResult, (int)MachineStation.主监控, MotionLogLevel.Info);
                        string[] ccd = ccdResult.Split(',');

                        if (ccd[2] == "1")//结果ok
                        {
                            LogAuto.Notify("上相机拍照3返回OK！" + ccdResult, (int)MachineStation.主监控, MotionLogLevel.Info);

                            m_nStep = (int)AxisTakeIn_WorkStep.前龙门移动到组装拍照位4;

                        }
                        else
                        {
                            b_Result = false;
                            LogAuto.Notify("上相机拍照3返回NG！" + ccdResult, (int)MachineStation.主监控, MotionLogLevel.Alarm);
                            Error pError = new Error(ref this.m_NowAddress, "上相机拍照3返回NG", "", (int)MErrorCode.CCD_Capture1異常);
                            pError.AddErrSloution("Retry", (int)AxisTakeIn_WorkStep.触发上相机拍照3);
                            pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                        }

                    }

                    else if (timerDelay.Enabled == false)
                    {
                        b_Result = false;
                        LogAuto.Notify("上CCD拍照3未返回数据！", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                        Error pError = new Error(ref this.m_NowAddress, "上CCD拍照3未返回数据", "", (int)MErrorCode.CCD_Capture1異常);
                        pError.AddErrSloution("Retry", (int)AxisTakeIn_WorkStep.触发上相机拍照3);
                        pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    }
                    break;
                case AxisTakeIn_WorkStep.前龙门移动到组装拍照位4:

                    LogAuto.Notify("前龙门移动到组装拍照位4！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    HardWareControl.movePoint(EnumParam_Point.组装拍照位4);
                    m_nStep = (int)AxisTakeIn_WorkStep.触发上相机拍照4;
                    break;

                case AxisTakeIn_WorkStep.触发上相机拍照4:
                    if (HardWareControl.getPointIdel(EnumParam_Point.组装拍照位4))
                    {
                        
                        LogAuto.Notify("获取上相机拍照位4轴当前坐标！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        double axisX = HardWareControl.getMotor(EnumParam_Axis.X1).GetPosition();
                        double axisY = HardWareControl.getMotor(EnumParam_Axis.Y1).GetPosition();
                        double axisR = HardWareControl.getMotor(EnumParam_Axis.R).GetPosition();
                        HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr = "";
                        LogAuto.Notify("给上相机发送拍照指令4！" + "T2,4" + "," + axisX + "," + axisY + "," + axisR + "," + ProductSN, (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).SendMSG("T2,4" + "," + axisX + "," + axisY + "," + axisR + "," + ProductSN);

                        timerDelay.Enabled = false;
                        timerDelay.Interval = 10000;
                        timerDelay.Start();
                        m_nStep = (int)AxisTakeIn_WorkStep.等待上相机反馈4;
                    }
                    break;

                case AxisTakeIn_WorkStep.等待上相机反馈4:
                    if (HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr != "")
                    {
                        string ccdResult = HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr;
                        LogAuto.Notify("上相机拍照4返回结果！" + ccdResult, (int)MachineStation.主监控, MotionLogLevel.Info);
                        string[] ccd = ccdResult.Split(',');

                        if (ccd[2] == "1")//结果ok
                        {
                            LogAuto.Notify("上相机拍照4返回OK！" + ccdResult, (int)MachineStation.主监控, MotionLogLevel.Info);

                            m_nStep = (int)AxisTakeIn_WorkStep.触发相机计算;

                        }
                        else
                        {
                            b_Result = false;
                            LogAuto.Notify("上相机拍照4返回NG！" + ccdResult, (int)MachineStation.主监控, MotionLogLevel.Alarm);
                            Error pError = new Error(ref this.m_NowAddress, "上相机拍照4返回NG", "", (int)MErrorCode.CCD_Capture1異常);
                            pError.AddErrSloution("Retry", (int)AxisTakeIn_WorkStep.触发上相机拍照4);
                            pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                        }

                    }

                    else if (timerDelay.Enabled == false)
                    {
                        b_Result = false;
                        LogAuto.Notify("上CCD拍照4未返回数据！", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                        Error pError = new Error(ref this.m_NowAddress, "上CCD拍照4未返回数据", "", (int)MErrorCode.CCD_Capture1異常);
                        pError.AddErrSloution("Retry", (int)AxisTakeIn_WorkStep.触发上相机拍照4);
                        pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    }
                    break;


                case AxisTakeIn_WorkStep.触发相机计算:
                    LogAuto.Notify("触发相机计算！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).SendMSG("T3,2");
                    timerDelay.Enabled = false;
                    timerDelay.Interval = 10000;
                    timerDelay.Start();
                    m_nStep = (int)AxisTakeIn_WorkStep.接收相机反馈结果;
                    break;

                case AxisTakeIn_WorkStep.接收相机反馈结果:
                  
                    if (HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr != "")
                    {
                        string ccdResult = HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr; //T3,2,1,X,Y,R,X,Y,X,Y,X,Y,X,Y

                        LogAuto.Notify("接收相机反馈结果！" + ccdResult, (int)MachineStation.主监控, MotionLogLevel.Info);
                        string[] ccd = ccdResult.Split(',');

                        if (ccd[2] == "1")//结果ok
                        {
                            LogAuto.Notify("上相机计算返回OK！，接收组装坐标及锁螺丝坐标" + ccdResult, (int)MachineStation.主监控, MotionLogLevel.Info);
                            double.TryParse(ccd[3], out MachineDataDefine._productPoint.PutX);
                            double.TryParse(ccd[4], out MachineDataDefine._productPoint.PutY);
                            double.TryParse(ccd[5], out MachineDataDefine._productPoint.PutR);
                            double.TryParse(ccd[6], out MachineDataDefine._productPoint.screwX1);
                            double.TryParse(ccd[7], out MachineDataDefine._productPoint.screwY1);
                            double.TryParse(ccd[8], out MachineDataDefine._productPoint.screwX2);
                            double.TryParse(ccd[9], out MachineDataDefine._productPoint.screwY2);
                            double.TryParse(ccd[10], out MachineDataDefine._productPoint.screwX3);
                            double.TryParse(ccd[11], out MachineDataDefine._productPoint.screwY3);
                            double.TryParse(ccd[12], out MachineDataDefine._productPoint.screwX4);
                            double.TryParse(ccd[12], out MachineDataDefine._productPoint.screwY4);
                            m_nStep = (int)AxisTakeIn_WorkStep.准备打螺丝;

                        }
                        else
                        {
                            b_Result = false;
                            LogAuto.Notify("上相机计算返回NG！" + ccdResult, (int)MachineStation.主监控, MotionLogLevel.Alarm);
                            Error pError = new Error(ref this.m_NowAddress, "上相机计算返回NG", "", (int)MErrorCode.CCD_Capture1異常);
                            pError.AddErrSloution("Retry", (int)AxisTakeIn_WorkStep.触发上相机拍照4);
                            pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                        }

                    }

                    else if (timerDelay.Enabled == false)
                    {
                        b_Result = false;
                        LogAuto.Notify("上CCD计算未返回数据！", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                        Error pError = new Error(ref this.m_NowAddress, "上CCD计算未返回数据", "", (int)MErrorCode.CCD_Capture1異常);
                        pError.AddErrSloution("Retry", (int)AxisTakeIn_WorkStep.触发相机计算);
                        pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    }
                    break;

                case AxisTakeIn_WorkStep.准备打螺丝:
                    LogAuto.Notify("准备打螺丝！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    electricReady = true;
                    m_nStep = (int)AxisTakeIn_WorkStep.前龙门移动放料位XYR;

                    break;

                case AxisTakeIn_WorkStep.前龙门移动放料位XYR:
                    LogAuto.Notify("前龙门移动放料位XYR绝对坐标！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    HardWareControl.getMotor(EnumParam_Axis.X1).AbsMove(MachineDataDefine._productPoint.PutX, speed);
                    HardWareControl.getMotor(EnumParam_Axis.Y1).AbsMove(MachineDataDefine._productPoint.PutY, speed);
                    HardWareControl.getMotor(EnumParam_Axis.R).AbsMove(MachineDataDefine._productPoint.PutR, speed);

                    m_nStep = (int)AxisTakeIn_WorkStep.前龙门高速移动到放料位;
                    break;

                case AxisTakeIn_WorkStep.前龙门高速移动到放料位:
                    LogAuto.Notify("前龙门高速移动到放料位！", (int)MachineStation.主监控, MotionLogLevel.Info);

                    HardWareControl.movePoint(EnumParam_Point.前龙门Z放料位H);

                    m_nStep = (int)AxisTakeIn_WorkStep.前龙门低速移动到放料位;
                    break;
                case AxisTakeIn_WorkStep.前龙门低速移动到放料位:
                    if (HardWareControl.getPointIdel(EnumParam_Point.前龙门Z放料位H))
                    {
                        LogAuto.Notify("前龙门低速移动到放料位！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.movePoint(EnumParam_Point.前龙门Z放料位L);
                        m_nStep = (int)AxisTakeIn_WorkStep.前龙门放料位到位;
                    }
                    break;


                case AxisTakeIn_WorkStep.前龙门放料位到位:
                    if (HardWareControl.getPointIdel(EnumParam_Point.前龙门Z放料位L))
                    {
                        LogAuto.Notify("电批开始打螺丝！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        electricStart = true;
                        m_nStep = (int)AxisTakeIn_WorkStep.锁完第一个螺丝;
                    }
                    break;
     
                case AxisTakeIn_WorkStep.锁完第一个螺丝:
                    LogAuto.Notify("电批锁螺丝锁完第一个螺丝，前龙门避让！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    if (AxisTakeOut.screw)
                    {
                        LogAuto.Notify("电批锁螺丝锁完第一个螺丝，夹爪松开！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.getValve(EnumParam_Valve.Driver夹取气缸ON).Close();                     
                        timerDelay.Enabled = false;
                        timerDelay.Interval = 500;
                        timerDelay.Start();
                        m_nStep = (int)AxisTakeIn_WorkStep.前龙门避让;
                    }
                    break;


                case AxisTakeIn_WorkStep.前龙门避让:
                   if( HardWareControl.getValve(EnumParam_Valve.Driver夹取气缸ON).isIDLE())
                    {

                        LogAuto.Notify("前龙门移动到避让位！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.movePoint(EnumParam_Point.前龙门避让位);
                      
                        m_nStep = (int)AxisTakeIn_WorkStep.前龙门避让位到位;
                    }
                    break;
                case AxisTakeIn_WorkStep.前龙门避让位到位:
                    if (HardWareControl.getPointIdel(EnumParam_Point.前龙门避让位))
                    {
                        LogAuto.Notify("前龙门避让位到位！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        AxisTakeOut.screw = false;
                        m_nStep = (int)AxisTakeIn_WorkStep.可复检;
                    }
                    break;

                case AxisTakeIn_WorkStep.可复检:
                    if (AxisTakeOut.recheck)
                    {
                        LogAuto.Notify("锁完螺丝可复检！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        
                        m_nStep = (int)AxisTakeIn_WorkStep.前龙门移动到复检位1;
                    }
                    break;
                case AxisTakeIn_WorkStep.前龙门移动到复检位1:
                  
                        LogAuto.Notify("前龙门移动到复检位1！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.movePoint(EnumParam_Point.前龙门复检位1);
                    m_nStep = (int)AxisTakeIn_WorkStep.复检位1拍照;
                    break;


                case AxisTakeIn_WorkStep.复检位1拍照:
                    if (HardWareControl.getPointIdel(EnumParam_Point.前龙门复检位1))
                    {
                        LogAuto.Notify("复检位1拍照！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        //LogAuto.Notify("获取上相机复检拍照位1轴当前坐标！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        //double axisX = HardWareControl.getMotor(EnumParam_Axis.X1).GetPosition();
                        //double axisY = HardWareControl.getMotor(EnumParam_Axis.Y1).GetPosition();
                        //double axisR = HardWareControl.getMotor(EnumParam_Axis.R).GetPosition();
                        HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr = "";
                        LogAuto.Notify("给上相机发送拍照指令1！" + "S2,1", (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).SendMSG("S2,1");

                        timerDelay.Enabled = false;
                        timerDelay.Interval = 10000;
                        timerDelay.Start();
                        m_nStep = (int)AxisTakeIn_WorkStep.等待上相机复检反馈1;
                       
                    }
                    break;
                case AxisTakeIn_WorkStep.等待上相机复检反馈1:
                    if (HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr != "")
                    {
                        string ccdResult = HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr;
                        LogAuto.Notify("上相机复检拍照1返回结果！" + ccdResult, (int)MachineStation.主监控, MotionLogLevel.Info);
                        string[] ccd = ccdResult.Split(',');

                        if (ccd[2] == "1")//结果ok
                        {
                            LogAuto.Notify("上相机复检拍照1返回结果OK！" + ccdResult, (int)MachineStation.主监控, MotionLogLevel.Info);

                            m_nStep = (int)AxisTakeIn_WorkStep.前龙门移动到复检位2;

                        }
                        else
                        {
                            b_Result = false;
                            LogAuto.Notify("上相机复检拍照1返回结果NG！" + ccdResult, (int)MachineStation.主监控, MotionLogLevel.Alarm);
                            Error pError = new Error(ref this.m_NowAddress, "上相机复检拍照1返回结果NG", "", (int)MErrorCode.CCD_Capture1異常);
                            pError.AddErrSloution("Retry", (int)AxisTakeIn_WorkStep.复检位1拍照);
                            pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                        }

                    }

                    else if (timerDelay.Enabled == false)
                    {
                        b_Result = false;
                        LogAuto.Notify("上相机复检拍照1未返回数据！", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                        Error pError = new Error(ref this.m_NowAddress, "上相机复检拍照2未返回数据", "", (int)MErrorCode.CCD_Capture1異常);
                        pError.AddErrSloution("Retry", (int)AxisTakeIn_WorkStep.复检位1拍照);
                        pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    }
                    break;

                case AxisTakeIn_WorkStep.前龙门移动到复检位2:
                   
                        LogAuto.Notify("前龙门移动到复检位2！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.movePoint(EnumParam_Point.前龙门复检位2);
                    m_nStep = (int)AxisTakeIn_WorkStep.复检位2拍照;
                    break;


                case AxisTakeIn_WorkStep.复检位2拍照:
                    if (HardWareControl.getPointIdel(EnumParam_Point.前龙门复检位2))
                    {
                        LogAuto.Notify("复检位2拍照！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        //LogAuto.Notify("获取上相机复检拍照位2轴当前坐标！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        //double axisX = HardWareControl.getMotor(EnumParam_Axis.X1).GetPosition();
                        //double axisY = HardWareControl.getMotor(EnumParam_Axis.Y1).GetPosition();
                        //double axisR = HardWareControl.getMotor(EnumParam_Axis.R).GetPosition();
                        HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr = "";
                        LogAuto.Notify("给上相机发送拍照指令2！" + "S2,2", (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).SendMSG("S2,2");

                        timerDelay.Enabled = false;
                        timerDelay.Interval = 10000;
                        timerDelay.Start();
                        m_nStep = (int)AxisTakeIn_WorkStep.等待上相机复检反馈2;

                    }
                    break;
                case AxisTakeIn_WorkStep.等待上相机复检反馈2:
                    if (HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr != "")
                    {
                        string ccdResult = HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr;
                        LogAuto.Notify("上相机复检拍照2返回结果！" + ccdResult, (int)MachineStation.主监控, MotionLogLevel.Info);
                        string[] ccd = ccdResult.Split(',');

                        if (ccd[2] == "1")//结果ok
                        {
                            LogAuto.Notify("上相机复检拍照2返回结果OK！" + ccdResult, (int)MachineStation.主监控, MotionLogLevel.Info);

                            m_nStep = (int)AxisTakeIn_WorkStep.前龙门移动到复检位3;

                        }
                        else
                        {
                            b_Result = false;
                            LogAuto.Notify("上相机复检拍照2返回结果NG！" + ccdResult, (int)MachineStation.主监控, MotionLogLevel.Alarm);
                            Error pError = new Error(ref this.m_NowAddress, "上相机复检拍照2返回结果NG", "", (int)MErrorCode.CCD_Capture1異常);
                            pError.AddErrSloution("Retry", (int)AxisTakeIn_WorkStep.复检位2拍照);
                            pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                        }

                    }

                    else if (timerDelay.Enabled == false)
                    {
                        b_Result = false;
                        LogAuto.Notify("上相机复检拍照2未返回数据！", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                        Error pError = new Error(ref this.m_NowAddress, "上相机复检拍照2未返回数据", "", (int)MErrorCode.CCD_Capture1異常);
                        pError.AddErrSloution("Retry", (int)AxisTakeIn_WorkStep.复检位2拍照);
                        pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    }
                    break;

                case AxisTakeIn_WorkStep.前龙门移动到复检位3:

                    LogAuto.Notify("前龙门移动到复检位3！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    HardWareControl.movePoint(EnumParam_Point.前龙门复检位3);
                    m_nStep = (int)AxisTakeIn_WorkStep.复检位2拍照;
                    break;


                case AxisTakeIn_WorkStep.复检位3拍照:
                    if (HardWareControl.getPointIdel(EnumParam_Point.前龙门复检位3))
                    {
                        LogAuto.Notify("复检位3拍照！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        //LogAuto.Notify("获取上相机复检拍照位2轴当前坐标！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        //double axisX = HardWareControl.getMotor(EnumParam_Axis.X1).GetPosition();
                        //double axisY = HardWareControl.getMotor(EnumParam_Axis.Y1).GetPosition();
                        //double axisR = HardWareControl.getMotor(EnumParam_Axis.R).GetPosition();
                        HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr = "";
                        LogAuto.Notify("给上相机发送拍照指令3！" + "S2,3", (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).SendMSG("S2,3");

                        timerDelay.Enabled = false;
                        timerDelay.Interval = 10000;
                        timerDelay.Start();
                        m_nStep = (int)AxisTakeIn_WorkStep.等待上相机复检反馈3;

                    }
                    break;
                case AxisTakeIn_WorkStep.等待上相机复检反馈3:
                    if (HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr != "")
                    {
                        string ccdResult = HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr;
                        LogAuto.Notify("上相机复检拍照3返回结果！" + ccdResult, (int)MachineStation.主监控, MotionLogLevel.Info);
                        string[] ccd = ccdResult.Split(',');

                        if (ccd[2] == "1")//结果ok
                        {
                            LogAuto.Notify("上相机复检拍照3返回结果OK！" + ccdResult, (int)MachineStation.主监控, MotionLogLevel.Info);

                            m_nStep = (int)AxisTakeIn_WorkStep.前龙门移动到复检位4;

                        }
                        else
                        {
                            b_Result = false;
                            LogAuto.Notify("上相机复检拍照3返回结果NG！" + ccdResult, (int)MachineStation.主监控, MotionLogLevel.Alarm);
                            Error pError = new Error(ref this.m_NowAddress, "上相机复检拍照3返回结果NG", "", (int)MErrorCode.CCD_Capture1異常);
                            pError.AddErrSloution("Retry", (int)AxisTakeIn_WorkStep.复检位2拍照);
                            pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                        }

                    }

                    else if (timerDelay.Enabled == false)
                    {
                        b_Result = false;
                        LogAuto.Notify("上相机复检拍照3未返回数据！", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                        Error pError = new Error(ref this.m_NowAddress, "上相机复检拍照3未返回数据", "", (int)MErrorCode.CCD_Capture1異常);
                        pError.AddErrSloution("Retry", (int)AxisTakeIn_WorkStep.复检位2拍照);
                        pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    }
                    break;

                case AxisTakeIn_WorkStep.前龙门移动到复检位4:

                    LogAuto.Notify("前龙门移动到复检位2！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    HardWareControl.movePoint(EnumParam_Point.前龙门复检位4);
                    m_nStep = (int)AxisTakeIn_WorkStep.复检位4拍照;
                    break;


                case AxisTakeIn_WorkStep.复检位4拍照:
                    if (HardWareControl.getPointIdel(EnumParam_Point.前龙门复检位2))
                    {
                        LogAuto.Notify("复检位4拍照！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        //LogAuto.Notify("获取上相机复检拍照位2轴当前坐标！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        //double axisX = HardWareControl.getMotor(EnumParam_Axis.X1).GetPosition();
                        //double axisY = HardWareControl.getMotor(EnumParam_Axis.Y1).GetPosition();
                        //double axisR = HardWareControl.getMotor(EnumParam_Axis.R).GetPosition();
                        HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr = "";
                        LogAuto.Notify("给上相机发送拍照指令4！" + "S2,4", (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).SendMSG("S2,4");

                        timerDelay.Enabled = false;
                        timerDelay.Interval = 10000;
                        timerDelay.Start();
                        m_nStep = (int)AxisTakeIn_WorkStep.等待上相机复检反馈2;

                    }
                    break;
                case AxisTakeIn_WorkStep.等待上相机复检反馈4:
                    if (HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr != "")
                    {
                        string ccdResult = HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr;
                        LogAuto.Notify("上相机复检拍照4返回结果！" + ccdResult, (int)MachineStation.主监控, MotionLogLevel.Info);
                        string[] ccd = ccdResult.Split(',');

                        if (ccd[2] == "1")//结果ok
                        {
                            LogAuto.Notify("上相机复检拍照4返回结果OK！" + ccdResult, (int)MachineStation.主监控, MotionLogLevel.Info);

                            m_nStep = (int)AxisTakeIn_WorkStep.复检结束;

                        }
                        else
                        {
                            b_Result = false;
                            LogAuto.Notify("上相机复检拍照4返回结果NG！" + ccdResult, (int)MachineStation.主监控, MotionLogLevel.Alarm);
                            Error pError = new Error(ref this.m_NowAddress, "上相机复检拍照4返回结果NG", "", (int)MErrorCode.CCD_Capture1異常);
                            pError.AddErrSloution("Retry", (int)AxisTakeIn_WorkStep.复检位4拍照);
                            pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                        }

                    }

                    else if (timerDelay.Enabled == false)
                    {
                        b_Result = false;
                        LogAuto.Notify("上相机复检拍照4未返回数据！", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                        Error pError = new Error(ref this.m_NowAddress, "上相机复检拍照4未返回数据", "", (int)MErrorCode.CCD_Capture1異常);
                        pError.AddErrSloution("Retry", (int)AxisTakeIn_WorkStep.复检位4拍照);
                        pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    }
                    break;
                case AxisTakeIn_WorkStep.复检结束:
                    LogAuto.Notify("复检结束&&流道下料！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    AxisTakeOut.recheck = false;
                    RunnerIn.PutProduct = false;
                 
                    m_nStep = (int)AxisTakeIn_WorkStep.Start;
                    break;
            }
        }
      
    }
}
