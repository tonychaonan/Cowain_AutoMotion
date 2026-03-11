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
    public class AxisTakeOut : Base
    {
        private AxisTakeOut_HomeStep currentHomeStep;
        private AxisTakeOut_WorkStep currentWorkStep;
        /// <summary>
        /// 锁完第一个螺丝
        /// </summary>
        public static bool screw = false;
        /// <summary>
        /// 锁完螺丝可复检
        /// </summary>
        public static bool recheck = false;
        /// <summary>
        /// 扭力值
        /// </summary>
        public static string screwValue = "999";

        public static double speed = 80;
        /// <summary>
        /// 第几个螺丝
        /// </summary>
        public static int i = 1;
        /// <summary>
        /// 锁完螺丝
        /// </summary>
        public static bool finish = false;

        /// <summary>
        /// 前龙门避让只执行一次
        /// </summary>
        public static bool onec = true;

        public AxisTakeOut(Type homeEnum1, Type stepEnum1, string instanceName1, Base parent) : base(homeEnum1,stepEnum1,instanceName1, parent, false)
        {

        }
        public enum AxisTakeOut_HomeStep
        {
            Start = 0,
            Z轴回原点,
            Y轴回原点,
            Completed
        }
        public enum AxisTakeOut_WorkStep
        {
            Start = 0,
            准备锁螺丝,
            移动到取螺丝位,
            要螺丝信号,
            吸真空,
            后龙门抬到待命位,
            关闭准备打螺丝信号,
            前龙门避让完成,
            电批开始打螺丝,
            后龙门XY轴移动到锁螺丝位,
            后龙门Z轴移动到锁第一颗螺丝位H,
            后龙门Z轴移动到锁第一颗螺丝位L,
            后龙门Z轴移动到锁螺丝位,
            后龙门Z轴移动到第一颗锁螺丝位,
            判断复锁螺丝是否掉料,
            判断螺丝是否掉料,
            后龙门Z轴移动到锁螺丝复锁位L,
            后龙门Z轴移动到锁螺丝位复锁L到位,
            后龙门Z轴移动到锁螺丝位H,
            后龙门Z轴移动到锁螺丝位L,
            后龙门Z轴移动到锁螺丝位L到位,
            锁螺丝ok,
            获取扭力值,
            获取扭力值ok,
            等待收到扭力值,
            获取扭力值成功,
            获取扭力值失败,
            复锁第一颗螺丝,
            后龙门Z锁螺丝位H,
            后龙门Z锁螺丝位H到位,
            后龙门Z锁螺丝位L到位,
            后龙门移动到取螺丝待命位,
            给前龙门复检信号,





            运动到待命位,
            等待做料完成信号,
            轴运动到取料位,
            气缸打开,
            运动到待命位2,
            等待到达待命位,
            等待允许放料信号,
            运动到放料位,
            气缸关闭,
            运动到待命位3,
            结束,
            Completed
        }
        public override void HomeCycle(ref double dbTime)
        {
            currentHomeStep = (AxisTakeOut_HomeStep)m_nHomeStep;
            switch (currentHomeStep)
            {
                case AxisTakeOut_HomeStep.Start:
                    m_bHomeCompleted = false;
                    onec = true;
                    screw = false;
                    recheck = false;
                    screwValue = "999";
                    i = 1;
                    finish = false;
                    HardWareControl.getOutputIO(EnumParam_OutputIO.螺丝供料机要料信号).SetIO(false);
                    HardWareControl.getOutputIO(EnumParam_OutputIO.电批启动).SetIO(false);
                    HardWareControl.getMotor(EnumParam_Axis.Z2).DoHome();
                    m_nHomeStep = (int)AxisTakeOut_HomeStep.Y轴回原点;
                    break;
                case AxisTakeOut_HomeStep.Y轴回原点:
                    if (HardWareControl.getMotor(EnumParam_Axis.Z2).isHomeCompleted())
                    {
                        HardWareControl.getMotor(EnumParam_Axis.X2).DoHome();
                        HardWareControl.getMotor(EnumParam_Axis.Y2).DoHome();
                        m_nHomeStep = (int)AxisTakeOut_HomeStep.Completed;
                    }
                    break;
                case AxisTakeOut_HomeStep.Completed:
                    if (HardWareControl.getMotor(EnumParam_Axis.Y2).isHomeCompleted() && HardWareControl.getMotor(EnumParam_Axis.X2).isHomeCompleted())
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
            currentWorkStep = (AxisTakeOut_WorkStep)m_nStep;
            switch (currentWorkStep)
            {
                case AxisTakeOut_WorkStep.Start:
                    if (HardWareControl.getPointIdel(EnumParam_Point.后龙门XY取螺丝待命位))
                    {
                        onec = true;
                        LogAuto.Notify("锁螺丝！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        m_nStep = (int)AxisTakeOut_WorkStep.准备锁螺丝;
                    }
                    break;
                case AxisTakeOut_WorkStep.准备锁螺丝:

                    if (i == 1)
                    {
                        if (AxisTakeIn.electricReady)
                        {
                            LogAuto.Notify("螺丝供料机有料信号是否有信号！", (int)MachineStation.主监控, MotionLogLevel.Info);
                            if (HardWareControl.getInputIO(EnumParam_InputIO.螺丝供料机有料信号).GetValue())
                            {
                                LogAuto.Notify("螺丝供料机有信号,取螺丝！", (int)MachineStation.主监控, MotionLogLevel.Info);
                                m_nStep = (int)AxisTakeOut_WorkStep.移动到取螺丝位;
                            }
                            else
                            {
                                LogAuto.Notify("螺丝供料机无信号要进料！", (int)MachineStation.主监控, MotionLogLevel.Info);
                                m_nStep = (int)AxisTakeOut_WorkStep.要螺丝信号;
                            }
                        }
                    }
                    else
                    {
                        LogAuto.Notify("螺丝供料机有料信号是否有信号！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        if (HardWareControl.getInputIO(EnumParam_InputIO.螺丝供料机有料信号).GetValue())
                        {
                            LogAuto.Notify("螺丝供料机有信号,取螺丝！", (int)MachineStation.主监控, MotionLogLevel.Info);
                            m_nStep = (int)AxisTakeOut_WorkStep.移动到取螺丝位;
                        }
                        else
                        {
                            LogAuto.Notify("螺丝供料机无信号要进料！", (int)MachineStation.主监控, MotionLogLevel.Info);
                            m_nStep = (int)AxisTakeOut_WorkStep.要螺丝信号;
                        }
                    }
                    break;
                case AxisTakeOut_WorkStep.要螺丝信号:
                    LogAuto.Notify("螺丝供料机要料信号！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    HardWareControl.getOutputIO(EnumParam_OutputIO.螺丝供料机要料信号).SetIO(true);
                    m_nStep = (int)AxisTakeOut_WorkStep.准备锁螺丝;
                    break;
                case AxisTakeOut_WorkStep.移动到取螺丝位:

                    LogAuto.Notify("后龙门移动到取螺丝位！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    HardWareControl.movePoint(EnumParam_Point.后龙门取螺丝位);
                    m_nStep = (int)AxisTakeOut_WorkStep.吸真空;
                    break;

                case AxisTakeOut_WorkStep.吸真空:
                    if (HardWareControl.getPointIdel(EnumParam_Point.后龙门取螺丝位))
                    {
                        LogAuto.Notify("后龙门取螺丝位吸真空开！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.getValve(EnumParam_Valve.取螺丝吸真空电磁阀ON).Open();


                        m_nStep = (int)AxisTakeOut_WorkStep.后龙门抬到待命位;
                    }
                    break;

                case AxisTakeOut_WorkStep.后龙门抬到待命位:
                    if (i == 1)
                    {
                        LogAuto.Notify("后龙门抬到待命位！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.movePoint(EnumParam_Point.后龙门待命位);
                        //i = 2;
                        m_nStep = (int)AxisTakeOut_WorkStep.关闭准备打螺丝信号;
                    }
                    else if (i == 2)
                    {
                       if( onec)
                        {
                            LogAuto.Notify("取第二个时电批锁螺丝锁完第一个螺丝，变量给true，前龙门避让！", (int)MachineStation.主监控, MotionLogLevel.Info);
                            onec = false;
                            screw = true;
                        }
                       
                        //i = 3;
                        m_nStep = (int)AxisTakeOut_WorkStep.前龙门避让完成;
                    }
                    else if (i == 3)
                    {
                        m_nStep = (int)AxisTakeOut_WorkStep.后龙门XY轴移动到锁螺丝位;
                    }
                    else if (i == 4)
                    {

                        m_nStep = (int)AxisTakeOut_WorkStep.后龙门XY轴移动到锁螺丝位;
                    }

                    break;

                case AxisTakeOut_WorkStep.前龙门避让完成:
                    if (screw == false)
                    {
                        LogAuto.Notify("前龙门避让完成！", (int)MachineStation.主监控, MotionLogLevel.Info);

                        m_nStep = (int)AxisTakeOut_WorkStep.后龙门XY轴移动到锁螺丝位;
                    }
                    break;



                case AxisTakeOut_WorkStep.关闭准备打螺丝信号:
                    if (HardWareControl.getPointIdel(EnumParam_Point.后龙门待命位))
                    {
                       

                        m_nStep = (int)AxisTakeOut_WorkStep.电批开始打螺丝;
                    }
                    break;

                case AxisTakeOut_WorkStep.电批开始打螺丝:
                    if (AxisTakeIn.electricStart)
                    {
                        LogAuto.Notify("电批开始打螺丝！", (int)MachineStation.主监控, MotionLogLevel.Info);

                        m_nStep = (int)AxisTakeOut_WorkStep.后龙门XY轴移动到锁螺丝位;
                    }
                    break;

                case AxisTakeOut_WorkStep.后龙门XY轴移动到锁螺丝位:
                    if (i == 1)
                    {
                        LogAuto.Notify("后龙门XY轴移动到锁螺丝位1！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.getMotor(EnumParam_Axis.X2).AbsMove(MachineDataDefine._productPoint.screwX1, speed);
                        HardWareControl.getMotor(EnumParam_Axis.Y2).AbsMove(MachineDataDefine._productPoint.screwY1, speed);
                       
                    }
                    else if (i == 2)
                    {
                        LogAuto.Notify("后龙门XY轴移动到锁螺丝位2！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.getMotor(EnumParam_Axis.X2).AbsMove(MachineDataDefine._productPoint.screwX2, speed);
                        HardWareControl.getMotor(EnumParam_Axis.Y2).AbsMove(MachineDataDefine._productPoint.screwY2, speed);
                       
                    }
                    else if (i == 3)
                    {
                        LogAuto.Notify("后龙门XY轴移动到锁螺丝位3！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.getMotor(EnumParam_Axis.X2).AbsMove(MachineDataDefine._productPoint.screwX3, speed);
                        HardWareControl.getMotor(EnumParam_Axis.Y2).AbsMove(MachineDataDefine._productPoint.screwY3, speed);
                        
                    }

                    else if (i == 4)
                    {
                        LogAuto.Notify("后龙门XY轴移动到锁螺丝位4！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.getMotor(EnumParam_Axis.X2).AbsMove(MachineDataDefine._productPoint.screwX4, speed);
                        HardWareControl.getMotor(EnumParam_Axis.Y2).AbsMove(MachineDataDefine._productPoint.screwY4, speed);

                        //finish = true;
                        //i = 1;
                    }
                    //else
                    //{
                    //    LogAuto.Notify("i值异常！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    //    break;
                    //}

                    m_nStep = (int)AxisTakeOut_WorkStep.后龙门Z轴移动到锁螺丝位;
                    break;

                case AxisTakeOut_WorkStep.后龙门Z轴移动到锁螺丝位:
                    if (HardWareControl.getMotor(EnumParam_Axis.X2).isIDLE() && HardWareControl.getMotor(EnumParam_Axis.Y2).isIDLE())
                    {
                        LogAuto.Notify("后龙门Z轴移动到锁螺丝位H！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        if(i==1)
                        {                            
                            m_nStep = (int)AxisTakeOut_WorkStep.后龙门Z轴移动到第一颗锁螺丝位;
                        }
                        else
                        {
                            m_nStep = (int)AxisTakeOut_WorkStep.后龙门Z轴移动到锁螺丝位H;

                        }
                       
                    }

                    break;

                case AxisTakeOut_WorkStep.后龙门Z轴移动到第一颗锁螺丝位:
                    if (HardWareControl.getMotor(EnumParam_Axis.X2).isIDLE() && HardWareControl.getMotor(EnumParam_Axis.Y2).isIDLE())
                    {
                        LogAuto.Notify("后龙门Z轴移动到第一颗锁螺丝位！", (int)MachineStation.主监控, MotionLogLevel.Info);

                        HardWareControl.movePoint(EnumParam_Point.后龙门Z锁螺丝复锁位H);
                        m_nStep = (int)AxisTakeOut_WorkStep.判断复锁螺丝是否掉料;
                    }

                    break;


                case AxisTakeOut_WorkStep.判断复锁螺丝是否掉料:
                    if (HardWareControl.getPointIdel(EnumParam_Point.后龙门Z锁螺丝复锁位H))
                    {
                        if (HardWareControl.getInputIO(EnumParam_InputIO.吸真空1OK信号).GetValue())
                        {
                            LogAuto.Notify("后龙门Z轴移动到锁螺丝复锁位L！", (int)MachineStation.主监控, MotionLogLevel.Info);
                            i = 2;
                            LogAuto.Notify("关闭准备打螺丝变量！", (int)MachineStation.主监控, MotionLogLevel.Info);
                            AxisTakeIn.electricReady = false;
                            HardWareControl.movePoint(EnumParam_Point.后龙门Z锁螺丝复锁位L);
                            HardWareControl.getOutputIO(EnumParam_OutputIO.电批启动).SetIO(true);
                            m_nStep = (int)AxisTakeOut_WorkStep.后龙门Z轴移动到锁螺丝位复锁L到位;
                        }
                        else
                        {
                            LogAuto.Notify("复锁位螺丝掉料！", (int)MachineStation.主监控, MotionLogLevel.Info);
                           
                            Error pError1 = new Error(ref this.m_NowAddress, "复锁位螺丝掉料", "", (int)MErrorCode.机台掉料);
                            pError1.AddErrSloution("Retry", (int)AxisTakeOut_WorkStep.准备锁螺丝);
                            pError1.ErrorHappen(ref pError1, Error.ErrorType.錯誤);


                        }
                    }
                    break;
             
                case AxisTakeOut_WorkStep.后龙门Z轴移动到锁螺丝位H:
                    if(HardWareControl.getMotor(EnumParam_Axis.X2).isIDLE()&& HardWareControl.getMotor(EnumParam_Axis.Y2).isIDLE())
                    {
                        LogAuto.Notify("后龙门Z轴移动到锁螺丝位H！", (int)MachineStation.主监控, MotionLogLevel.Info);

                        HardWareControl.movePoint(EnumParam_Point.后龙门Z锁螺丝位H);
                        m_nStep = (int)AxisTakeOut_WorkStep.判断螺丝是否掉料;
                    }
                   
                    break;


                case AxisTakeOut_WorkStep.判断螺丝是否掉料:
                    if (HardWareControl.getPointIdel(EnumParam_Point.后龙门Z锁螺丝位H))
                    {
                        if (HardWareControl.getInputIO(EnumParam_InputIO.吸真空1OK信号).GetValue())
                        {
                            LogAuto.Notify("后龙门Z轴移动到锁螺丝复锁位L！", (int)MachineStation.主监控, MotionLogLevel.Info);
                            if (i == 2)
                            {
                                i = 3;
                            }
                            else if (i == 3)
                            {
                                i = 4;
                            }
                            else if (i ==4)
                            {
                                i = 1;
                                finish = true;
                            }
                            HardWareControl.movePoint(EnumParam_Point.后龙门Z锁螺丝位L);
                            HardWareControl.getOutputIO(EnumParam_OutputIO.电批启动).SetIO(true);
                            m_nStep = (int)AxisTakeOut_WorkStep.后龙门Z轴移动到锁螺丝位L到位;
                        }
                        else
                        {
                            
                            LogAuto.Notify("螺丝掉料！", (int)MachineStation.主监控, MotionLogLevel.Info);

                            Error pError1 = new Error(ref this.m_NowAddress, "螺丝掉料", "", (int)MErrorCode.机台掉料);
                            pError1.AddErrSloution("Retry", (int)AxisTakeOut_WorkStep.准备锁螺丝);
                            pError1.ErrorHappen(ref pError1, Error.ErrorType.錯誤);


                        }
                    }
                    break;
                case AxisTakeOut_WorkStep.后龙门Z轴移动到锁螺丝位L到位:
                    if (HardWareControl.getPointIdel(EnumParam_Point.后龙门Z锁螺丝位L))
                    {
                        timerDelay.Enabled = false;
                        timerDelay.Interval = 3000;
                        timerDelay.Start();
                        m_nStep = (int)AxisTakeOut_WorkStep.锁螺丝ok;
                    }
                    break;
                case AxisTakeOut_WorkStep.后龙门Z轴移动到锁螺丝位复锁L到位:
                    if (HardWareControl.getPointIdel(EnumParam_Point.后龙门Z锁螺丝复锁位L))
                    {
                        timerDelay.Enabled = false;
                        timerDelay.Interval = 3000;
                        timerDelay.Start();
                        m_nStep = (int)AxisTakeOut_WorkStep.锁螺丝ok;
                    }
                    break;
                case AxisTakeOut_WorkStep.锁螺丝ok:
                    if (!HardWareControl.getInputIO(EnumParam_InputIO.锁螺丝busy信号).GetValue())
                    {
                        LogAuto.Notify("锁螺丝信号OK&&关吸真空！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.getValve(EnumParam_Valve.取螺丝吸真空电磁阀ON).Close();
                        m_nStep = (int)AxisTakeOut_WorkStep.获取扭力值;
                    }
                    else if (timerDelay.Enabled == false)
                    {
                        HardWareControl.getOutputIO(EnumParam_OutputIO.电批启动).SetIO(false);
                        LogAuto.Notify("电批返回扭力值超时！", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                        Error pError1 = new Error(ref this.m_NowAddress, "电批返回扭力值超时！！！", "", (int)MErrorCode.电批超时报警);
                        pError1.ErrorHappen(ref pError1, Error.ErrorType.錯誤);
                    }
                    break;

                case AxisTakeOut_WorkStep.获取扭力值:
                    LogAuto.Notify("获取扭力值！", (int)MachineStation.主监控, MotionLogLevel.Info);

                    if (HardWareControl.getRS232Control(EnumParam_ConnectionName.螺丝批).strRecData != "")
                    {
                        string str = HardWareControl.getRS232Control(EnumParam_ConnectionName.螺丝批).strRecData.Trim();//"$50F940,FWD-1,OK ,832,1234,536,(2448),1368,68,1.337,1.50,kgfcm,300,3A0263,100"; // 

                        string[] strs = str.Split(',');
                        if (strs.Length < 10)
                        {
                            LogAuto.Notify("获取扭力值长度异常！", (int)MachineStation.主监控, MotionLogLevel.Info);
                            m_nStep = (int)AxisTakeOut_WorkStep.获取扭力值失败;
                        }
                        if (!strs[2].Contains("OK"))
                        {
                            LogAuto.Notify("获取扭力值失败！", (int)MachineStation.主监控, MotionLogLevel.Info);
                            m_nStep = (int)AxisTakeOut_WorkStep.获取扭力值失败;
                        }
                        if (!strs[1].Contains(MachineDataDefine.MachineCfgS.screwdatabits))
                        {
                            LogAuto.Notify("获取扭力值数据位异常！", (int)MachineStation.主监控, MotionLogLevel.Info);
                            m_nStep = (int)AxisTakeOut_WorkStep.获取扭力值失败;
                        }

                        if (strs[2].Contains("OK"))
                        {
                            LogAuto.Notify("获取扭力值OK！", (int)MachineStation.主监控, MotionLogLevel.Info);
                            screwValue = strs[9];

                            m_nStep = (int)AxisTakeOut_WorkStep.获取扭力值ok;
                        }
                    }
                    //else if (timerDelay.Enabled == false)
                    //{

                    //    LogAuto.Notify("电批返回扭力值异常！", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                    //    Error pError1 = new Error(ref this.m_NowAddress, "电批返回扭力值异常，请检查串口通讯", "", (int)MErrorCode.电批超时报警);
                    //    pError1.ErrorHappen(ref pError1, Error.ErrorType.錯誤);
                    //}
                    break;
                case AxisTakeOut_WorkStep.获取扭力值失败:

                    LogAuto.Notify("电批返回扭力值异常！", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                    Error pError = new Error(ref this.m_NowAddress, "电批返回扭力值异常，请检查串口通讯", "", (int)MErrorCode.电批超时报警);
                    pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    break;

                case AxisTakeOut_WorkStep.获取扭力值ok:
                    if (finish)
                    {
                        LogAuto.Notify("锁螺丝流程结束！", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                        finish = false;
                        m_nStep = (int)AxisTakeOut_WorkStep.复锁第一颗螺丝;
                    }
                    else
                    {
                        m_nStep = (int)AxisTakeOut_WorkStep.要螺丝信号;
                    }
                    break;

                case AxisTakeOut_WorkStep.复锁第一颗螺丝:
                   
                    LogAuto.Notify("后龙门复锁XY轴移动到锁螺丝位1！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    HardWareControl.getMotor(EnumParam_Axis.X2).AbsMove(MachineDataDefine._productPoint.screwX1, speed);
                    HardWareControl.getMotor(EnumParam_Axis.Y2).AbsMove(MachineDataDefine._productPoint.screwY1, speed);
                    m_nStep = (int)AxisTakeOut_WorkStep.后龙门Z锁螺丝位H;
                    break;

                case AxisTakeOut_WorkStep.后龙门Z锁螺丝位H:
                    if (HardWareControl.getMotor(EnumParam_Axis.X2).isIDLE() && HardWareControl.getMotor(EnumParam_Axis.Y2).isIDLE())
                    {
                        LogAuto.Notify("后龙门复锁移动到Z锁螺丝位H！", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                        HardWareControl.movePoint(EnumParam_Point.后龙门Z锁螺丝位H);
                        m_nStep = (int)AxisTakeOut_WorkStep.后龙门Z锁螺丝位H到位;
                    }
                    break;


                case AxisTakeOut_WorkStep.后龙门Z锁螺丝位H到位:

                    if (HardWareControl.getPointIdel(EnumParam_Point.后龙门Z锁螺丝位H))
                    {
                        LogAuto.Notify("后龙门复锁Z轴移动到锁螺丝位L！", (int)MachineStation.主监控, MotionLogLevel.Info);

                        HardWareControl.movePoint(EnumParam_Point.后龙门Z锁螺丝位L);
                      
                        m_nStep = (int)AxisTakeOut_WorkStep.后龙门Z锁螺丝位L到位;
                    }

                    break;
                case AxisTakeOut_WorkStep.后龙门Z锁螺丝位L到位:

                    if (HardWareControl.getPointIdel(EnumParam_Point.后龙门Z锁螺丝位L))
                    {
                        LogAuto.Notify("后龙门Z复锁锁螺丝位L到位到位！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        m_nStep = (int)AxisTakeOut_WorkStep.后龙门移动到取螺丝待命位;
                    }

                    break;


                case AxisTakeOut_WorkStep.后龙门移动到取螺丝待命位:
                   
                        LogAuto.Notify("后龙门移动到取螺丝待命位！", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                        HardWareControl.movePoint(EnumParam_Point.后龙门XY取螺丝待命位);
                        m_nStep = (int)AxisTakeOut_WorkStep.给前龙门复检信号;
                   
                    break;
                case AxisTakeOut_WorkStep.给前龙门复检信号:
                    if (HardWareControl.getPointIdel(EnumParam_Point.后龙门XY取螺丝待命位))
                    {
                        LogAuto.Notify("给前龙门复检信号！", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                        recheck = true;
                        m_nStep = (int)AxisTakeOut_WorkStep.Start;

                    }
                    break;
            }
        }
    }
}
