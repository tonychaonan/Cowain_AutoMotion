using Cowain_AutoMotion.Flow;
using Cowain_Machine;
using DevExpress.XtraPrinting;
using MotionBase;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Cowain_Machine.Flow.MErrorDefine;
using static DevExpress.Utils.Drawing.Helpers.NativeMethods;

namespace Cowain_AutoMotion
{
    public class AxisCalibration : Base
    {

        public AxisCalibration(Type homeEnum1, Type stepEnum1, string instanceName1, Base parent) : base(homeEnum1, stepEnum1, instanceName1, parent, false)
        {

        }
        public Queue<CMDClass> cmds = new Queue<CMDClass>();
        public CMDClass currentCMDClass = null;
        private AxisCalibration_WorkStep currentWorkStep;
        double speed = 30;
        
        public enum AxisCalibration_WorkStep
        {
            Start = 0,
            触发相机开始标定,
            开始标定,
            标定完成,
            前龙门走到拍照高度,
            前龙门走到拍照高度到位,
            触发相机拍照,
            等待相机反馈,
            前龙门移动到拍照位1,
            移动到下相机拍照位,
            下相机拍照,
            R轴正转5度,
            触发相机拍照旋转中心1,
            等待相机反馈1,
            等待相机旋转反馈1,
            R轴负转5度,
            触发相机拍照旋转中心2,
            等待相机反馈2,
            前龙门运动到下相机拍照位2,
            触发相机拍照3,
            等待相机反馈3,
            前龙门XY运动到放标定片位置,
            前龙门Z高速运动到放标定片位置,
            前龙门Z低速运动到放标定片位置,
            到位延时,
            延时结束,
            关闭真空,
            前龙门Z低速抬离标定片位置,
            前龙门移动到组装拍照位1,
            组装触发相机拍照1,
            组装等待相机反馈1,
            前龙门移动到组装拍照位2,
            组装触发相机拍照2,
            组装等待相机反馈2,
            前龙门移动到组装拍照位3,
            组装触发相机拍照3,
            组装等待相机反馈3,
            前龙门移动到组装拍照位4,
            组装触发相机拍照4,
            组装等待相机反馈4,
            Completed
        }
        public override void StepCycle(ref double dbTime)
        {
            currentWorkStep = (AxisCalibration_WorkStep)m_nStep;
            switch (currentWorkStep)
            {
                case AxisCalibration_WorkStep.Start:
                    LogAuto.Notify("开始标定！！！", (int)MachineStation.主监控, MotionLogLevel.Info);
                 
                    m_nStep = (int)AxisCalibration_WorkStep.触发相机开始标定;
                    break;
                case AxisCalibration_WorkStep.触发相机开始标定:
                    //currentCMDClass = cmds.Dequeue();
                    HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr = "";
                    if (MachineDataDefine.electriccalib)
                    {
                    
                        LogAuto.Notify("触发相机开始标定！" + "," + "SC1" + "," + "0" + "," + "0" + "," + 0, (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).SendMSG("SC1" + "," + "0" + "," + "0" + "," + 0);
                    }
                    else
                    {
                        LogAuto.Notify("触发相机开始标定！" + "," + "SC2" + "," + "0" + "," + "0" + "," + 0, (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).SendMSG("SC2" + "," + "0" + "," + "0" + "," + 0);
                    }
                    timerDelay.Enabled = false;
                    timerDelay.Interval = 3000;
                    timerDelay.Start();
                    m_nStep = (int)AxisCalibration_WorkStep.等待相机反馈;
                  
                    break;
                case AxisCalibration_WorkStep.等待相机反馈:
                    if (HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr != "")
                    {
                        string getStr = HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr;
                        LogAuto.Notify(getStr, (int)MachineStation.主监控, MotionLogLevel.Info);
                        string[] str = getStr.Split(',');

                        if (str[1] == "1")
                        {
                            if (MachineDataDefine.electriccalib)
                            {
                                m_nStep = (int)AxisCalibration_WorkStep.开始标定;
                            }
                            else
                            {
                                m_nStep = (int)AxisCalibration_WorkStep.移动到下相机拍照位;
                            }
                        }
                        else
                        {
                            MessageBox.Show("标定失败！！！");
                            LogAuto.Notify("标定失败！！！", (int)MachineStation.主监控, MotionLogLevel.Info);
                            m_nStep = (int)AxisCalibration_WorkStep.Completed;
                        }
                    }
                    else if (timerDelay.Enabled == false)
                    {
                        Error pError = new Error(ref this.m_NowAddress, "CCD未返回数据", "", (int)MErrorCode.CCD_Capture1異常);

                        pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    }
                    break;


                case AxisCalibration_WorkStep.开始标定:
                  
                    if (cmds.Count > 0)
                    {
                        currentCMDClass = cmds.Dequeue();
                        LogAuto.Notify("开始走点位！！！", (int)MachineStation.主监控, MotionLogLevel.Info);
                     
                       
                            LogAuto.Notify("走点位！！！", (int)MachineStation.主监控, MotionLogLevel.Info);
                            HardWareControl.getMotor(EnumParam_Axis.X).AbsMove(currentCMDClass.X, speed);
                            HardWareControl.getMotor(EnumParam_Axis.Y).AbsMove(currentCMDClass.Y, speed);
                            HardWareControl.getMotor(EnumParam_Axis.R1).AbsMove(currentCMDClass.R, speed);
                       
                        m_nStep = (int)AxisCalibration_WorkStep.前龙门走到拍照高度;
                    }
                    else
                    {
                           

                            LogAuto.Notify("后龙门标定完成！！！", (int)MachineStation.主监控, MotionLogLevel.Info);
                            MachineDataDefine.electriccalib = false;

                        HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr = "";
                     
                        LogAuto.Notify("九点标定完成！" + "," + "EC1" + "," + currentCMDClass.X + "," + currentCMDClass.Y + "," + 0, (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).SendMSG("EC1" + "," + currentCMDClass.X + "," + currentCMDClass.Y + "," + 0);

                        MessageBox.Show("标定成功！！！");
                        m_nStep = (int)AxisCalibration_WorkStep.标定完成;
                    
                    }
                    break;
                case AxisCalibration_WorkStep.标定完成:
                    if (HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr != "")
                    {
                        string getStr = HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr;
                        LogAuto.Notify(getStr, (int)MachineStation.主监控, MotionLogLevel.Info);
                        string[] str = getStr.Split(',');

                        if (str[1] == "1")
                        {
                            m_nStep = (int)AxisCalibration_WorkStep.Completed;
                        }
                        else
                        {
                            MessageBox.Show("标定失败！！！");
                            LogAuto.Notify("标定失败！！！", (int)MachineStation.主监控, MotionLogLevel.Info);
                            m_nStep = (int)AxisCalibration_WorkStep.Completed;
                        }
                    }
                    else if (timerDelay.Enabled == false)
                    {
                        Error pError = new Error(ref this.m_NowAddress, "CCD未返回数据", "", (int)MErrorCode.CCD_Capture1異常);

                        pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    }
                    break;

                case AxisCalibration_WorkStep.前龙门走到拍照高度:
                        LogAuto.Notify("后龙门走标定拍照高度！！！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.getMotor(EnumParam_Axis.Z).AbsMove(currentCMDClass.Z, speed);                
                    m_nStep = (int)AxisCalibration_WorkStep.前龙门走到拍照高度到位;
                    break;

                case AxisCalibration_WorkStep.前龙门走到拍照高度到位:

                        if (HardWareControl.getMotor(EnumParam_Axis.Z).isIDLE())
                        {
                            LogAuto.Notify("前龙门走到拍照高度到位！！！", (int)MachineStation.主监控, MotionLogLevel.Info);

                            m_nStep = (int)AxisCalibration_WorkStep.触发相机拍照;
                        }
                   
                   
                    break;
                case AxisCalibration_WorkStep.触发相机拍照:

                        if (HardWareControl.getMotor(EnumParam_Axis.X).isIDLE() && HardWareControl.getMotor(EnumParam_Axis.Y).isIDLE()
                         && HardWareControl.getMotor(EnumParam_Axis.Z).isIDLE())
                        {
                        HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr = "";
                      
                        LogAuto.Notify("后龙门给相机发送拍照指令！" + "," + currentCMDClass.CMDHead + "," + currentCMDClass.X + "," + currentCMDClass.Y + "," + 0, (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).SendMSG(currentCMDClass.CMDHead + "," + currentCMDClass.X + "," + currentCMDClass.Y + "," + 0);

                        timerDelay.Enabled = false;
                        timerDelay.Interval = 3000;
                        timerDelay.Start();
                        m_nStep = (int)AxisCalibration_WorkStep.等待相机反馈1;
                        }
                   
                    break;
                case AxisCalibration_WorkStep.等待相机反馈1:
                    if (HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr != "")
                    {
                        string getStr = HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr;
                        LogAuto.Notify(getStr, (int)MachineStation.主监控, MotionLogLevel.Info);
                        string[] str = getStr.Split(',');

                        if (str[1] == "1")
                        {
                            m_nStep = (int)AxisCalibration_WorkStep.开始标定;
                        }
                        else
                        {
                            MessageBox.Show("标定失败！！！");
                            LogAuto.Notify("标定失败！！！", (int)MachineStation.主监控, MotionLogLevel.Info);
                            m_nStep = (int)AxisCalibration_WorkStep.Completed;
                        }
                    }
                    else if (timerDelay.Enabled == false)
                    {
                        Error pError = new Error(ref this.m_NowAddress, "CCD未返回数据", "", (int)MErrorCode.CCD_Capture1異常);

                        pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    }
                    break;

                case AxisCalibration_WorkStep.移动到下相机拍照位:
                    LogAuto.Notify("前龙门移动到拍照位1！", (int)MachineStation.主监控, MotionLogLevel.Info);

                    HardWareControl.movePoint(EnumParam_Point.下相机拍照位);
                    m_nStep = (int)AxisCalibration_WorkStep.R轴负转5度;
                    break;

                case AxisCalibration_WorkStep.下相机拍照:

                    if (HardWareControl.getPointIdel(EnumParam_Point.下相机拍照位))
                    {
                        HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr = "";
                      double  X = HardWareControl.getPoint(EnumParam_Point.下相机拍照位).Data1;
                        double Y = HardWareControl.getPoint(EnumParam_Point.下相机拍照位).Data2;
                        double R = HardWareControl.getPoint(EnumParam_Point.下相机拍照位).Data4;
                        double Z = HardWareControl.getPoint(EnumParam_Point.下相机拍照位).Data3;
                        LogAuto.Notify("给相机发送拍照指令旋转中心正转！" + "T2" + "," + X + "," + Y + "," + R, (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).SendMSG("T2"+","+ X + ","+ Y+","+ R);
                        timerDelay.Enabled = false;
                        timerDelay.Interval = 3000;
                        timerDelay.Start();
                        m_nStep = (int)AxisCalibration_WorkStep.等待相机反馈2;
                    }
                    break;
                case AxisCalibration_WorkStep.等待相机反馈2:
                    if (HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr != "")
                    {
                        string getStr = HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr;
                        LogAuto.Notify(getStr, (int)MachineStation.主监控, MotionLogLevel.Info);
                        string[] str = getStr.Split(',');

                        if (str[1] == "1")
                        {
                            m_nStep = (int)AxisCalibration_WorkStep.R轴正转5度;
                        }
                        else
                        {
                            MessageBox.Show("标定失败！！！");
                            LogAuto.Notify("标定失败！！！", (int)MachineStation.主监控, MotionLogLevel.Info);
                            m_nStep = (int)AxisCalibration_WorkStep.Completed;
                        }
                    }
                    else if (timerDelay.Enabled == false)
                    {
                        Error pError = new Error(ref this.m_NowAddress, "CCD未返回数据", "", (int)MErrorCode.CCD_Capture1異常);

                        pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    }
                    break;




                case AxisCalibration_WorkStep.R轴正转5度:
                    if (HardWareControl.getPointIdel(EnumParam_Point.下相机拍照位))
                    {
                        LogAuto.Notify("R轴正转5度！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.getMotor(EnumParam_Axis.R1).AbsMove(2, speed);
                        m_nStep = (int)AxisCalibration_WorkStep.触发相机拍照旋转中心1;
                    }
                    break;

                case AxisCalibration_WorkStep.触发相机拍照旋转中心1:
                    if (HardWareControl.getMotor(EnumParam_Axis.R1).isIDLE())
                    {
                        double X = HardWareControl.getPoint(EnumParam_Point.下相机拍照位).Data1;
                        double Y = HardWareControl.getPoint(EnumParam_Point.下相机拍照位).Data2;
                        double R = HardWareControl.getPoint(EnumParam_Point.下相机拍照位).Data4+2;
                        double Z = HardWareControl.getPoint(EnumParam_Point.下相机拍照位).Data3;
                        HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr = "";
                        //LogAuto.Notify("给相机发送拍照指令旋转中心正转！" + "T2" + "," + currentCMDClass.CMDHead + "," + currentCMDClass.X + "," + currentCMDClass.Y + "," + currentCMDClass.R, (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).SendMSG("T2" + "," + X + "," + Y + "," + R);
                        timerDelay.Enabled = false;
                        timerDelay.Interval = 3000;
                        timerDelay.Start();
                        m_nStep = (int)AxisCalibration_WorkStep.等待相机旋转反馈1;
                    }
                    break;
                case AxisCalibration_WorkStep.等待相机旋转反馈1:
                    if (HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr != "")
                    {
                        string getStr = HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr;
                        LogAuto.Notify(getStr, (int)MachineStation.主监控, MotionLogLevel.Info);
                        string[] str = getStr.Split(',');

                        if (str[1] == "1")
                        {
                            m_nStep = (int)AxisCalibration_WorkStep.Completed;
                            HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).SendMSG("EC2,0,0,0");
                        }
                        else
                        {
                            MessageBox.Show("标定失败！！！");
                            LogAuto.Notify("标定失败！！！", (int)MachineStation.主监控, MotionLogLevel.Info);
                            m_nStep = (int)AxisCalibration_WorkStep.Completed;
                        }
                    }
                    else if (timerDelay.Enabled == false)
                    {
                        Error pError = new Error(ref this.m_NowAddress, "CCD未返回数据", "", (int)MErrorCode.CCD_Capture1異常);

                        pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    }
                    break;

                case AxisCalibration_WorkStep.R轴负转5度:

                    LogAuto.Notify("R轴负转5度！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    HardWareControl.getMotor(EnumParam_Axis.R1).AbsMove(-2, speed);
                    m_nStep = (int)AxisCalibration_WorkStep.触发相机拍照旋转中心2;

                    break;

                case AxisCalibration_WorkStep.触发相机拍照旋转中心2:
                    if (HardWareControl.getMotor(EnumParam_Axis.R1).isIDLE())
                    {
                        HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr = "";
                        double X = HardWareControl.getPoint(EnumParam_Point.下相机拍照位).Data1;
                        double Y = HardWareControl.getPoint(EnumParam_Point.下相机拍照位).Data2;
                        double R = HardWareControl.getPoint(EnumParam_Point.下相机拍照位).Data4-2;
                        double Z = HardWareControl.getPoint(EnumParam_Point.下相机拍照位).Data3;
                        //LogAuto.Notify("给相机发送拍照指令旋转中心正转！" + "T2" + "," + currentCMDClass.CMDHead + "," + currentCMDClass.X + "," + currentCMDClass.Y + "," + currentCMDClass.R, (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).SendMSG("T2" + "," + X + "," + Y + "," + R);
                        timerDelay.Enabled = false;
                        timerDelay.Interval = 3000;
                        timerDelay.Start();
                        m_nStep = (int)AxisCalibration_WorkStep.等待相机反馈3;
                    }
                    break;
                case AxisCalibration_WorkStep.等待相机反馈3:
                    if (HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr != "")
                    {
                        string getStr = HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr;
                        LogAuto.Notify(getStr, (int)MachineStation.主监控, MotionLogLevel.Info);
                        string[] str = getStr.Split(',');

                        if (str[1] == "1")
                        {
                            HardWareControl.movePoint(EnumParam_Point.下相机拍照位);
                            m_nStep = (int)AxisCalibration_WorkStep.下相机拍照;
                          
                        }
                        else
                        {
                            MessageBox.Show("标定失败！！！");
                            LogAuto.Notify("标定失败！！！", (int)MachineStation.主监控, MotionLogLevel.Info);
                            m_nStep = (int)AxisCalibration_WorkStep.Completed;
                        }
                    }
                    else if (timerDelay.Enabled == false)
                    {
                        Error pError = new Error(ref this.m_NowAddress, "CCD未返回数据", "", (int)MErrorCode.CCD_Capture1異常);

                        pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    }
                    break;

                case AxisCalibration_WorkStep.Completed:


                    m_Status = 狀態.待命;

                    break;
            }
            base.StepCycle(ref dbTime);
        }
        public bool Action()
        {
            m_Status = 狀態.動作中;

            cmds.Clear();
            double X = 0;
            double Y = 0;
            double R = 0;
            double Z = 0;
            int Xstep = 4;
            int YStep = 4;
            int center1 = 5;
            int center2 = -5;
            if (!MachineDataDefine.electriccalib)
            {

               // LogAuto.Notify("获取前龙门九个点坐标", (int)MachineStation.主监控, MotionLogLevel.Info);
             
             
            }
            else
            {

                LogAuto.Notify("获取九个点坐标", (int)MachineStation.主监控, MotionLogLevel.Info);
                X = HardWareControl.getPoint(EnumParam_Point.上相机拍照位).Data1;
                Y = HardWareControl.getPoint(EnumParam_Point.上相机拍照位).Data2;
                R = HardWareControl.getPoint(EnumParam_Point.上相机拍照位).Data3;
                Z = HardWareControl.getPoint(EnumParam_Point.上相机拍照位).Data3;

                cmds.Enqueue(new CMDClass("T1", X - Xstep, Y - YStep, R, Z, center1, center2));
                cmds.Enqueue(new CMDClass("T1", X, Y - YStep, R, Z, center1, center2));
                cmds.Enqueue(new CMDClass("T1", X + Xstep, Y - YStep, R, Z, center1, center2));
                cmds.Enqueue(new CMDClass("T1", X + Xstep, Y, R, Z, center1, center2));
                cmds.Enqueue(new CMDClass("T1", X, Y, R, Z, center1, center2));
                cmds.Enqueue(new CMDClass("T1", X - Xstep, Y, R, Z, center1, center2));
                cmds.Enqueue(new CMDClass("T1", X - Xstep, Y + YStep, R, Z, center1, center2));
                cmds.Enqueue(new CMDClass("T1", X, Y + YStep, R, Z, center1, center2));          
                cmds.Enqueue(new CMDClass("T1", X + Xstep, Y + YStep, R, Z, center1, center2));
              
              
            }
           
              m_nStep = (int)AxisCalibration_WorkStep.Start;
            return base.DoStep(m_nStep);
        }
        public override void Stop()
        {
            
            MachineDataDefine.electriccalib = false;
            HardWareControl.getMotor(EnumParam_Axis.X).Stop();
            HardWareControl.getMotor(EnumParam_Axis.Y).Stop();
            HardWareControl.getMotor(EnumParam_Axis.R1).Stop();
            base.Stop();
        }
    }
    public class CMDClass
    {
        public string CMDHead = "";
        public double X = 0;
        public double Y = 0;
        public double R = 0;
        public double Z = 0;
        public double center1 = 0;
        public double center2 = 0;

        public CMDClass(string CMDHead1, double X1, double Y1, double R1, double Z1, double center11, double center22)
        {
            CMDHead = CMDHead1;
            X = X1;
            Y = Y1;
            R = R1;
            Z = Z1;
            center1 = center11;
            center2 = center22;
        }
    }
}
