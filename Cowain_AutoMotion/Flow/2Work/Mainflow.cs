using Chart;
using Cowain;
using Cowain_AutoMotion.Flow;
using Cowain_AutoMotion.Flow._3MESAndPDCA;
using Cowain_AutoMotion.Flow.Common;
using Cowain_AutoMotion.Flow.Hive;
using Cowain_Form.FormView;
using Cowain_Machine;
using Cowain_Machine.Flow;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.XtraEditors;
using MotionBase;
using Post;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Cowain_AutoMotion.Flow._2Work.动静态;
using static Cowain_Machine.Flow.MErrorDefine;

namespace Cowain_AutoMotion.Flow._2Work
{
    //.Flow._2Work
    public class Mainflow : Base
    {
        private Mainflow_HomeStep currentHomeStep;
        private Mainflow_WorkStep currentWorkStep;
        private int gripRetryCount = 0;
        private const int MAX_GRIP_RETRY = 3;
        private System.Diagnostics.Stopwatch gripTimeStopwatch = new System.Diagnostics.Stopwatch();
        public static double speed = 80;
        public AxisCalibration axisCalibration;
        /// <summary>
        /// 前龙门可放料
        /// </summary>
        public static bool PutProduct = false;
        ProductPoint product = new ProductPoint();
        int tiaozhengjuli = 0;

        DateTime 准备放料startTime = DateTime.Now;
        DateTime 向下取料startTime = DateTime.Now;
        DateTime 准备夹取startTime = DateTime.Now;
        DateTime 取料准备向上startTime = DateTime.Now;
        private double OutX = 0;
        private double OutY = 0;
        private double OutR1 = 0;

        /// <summary>
        /// 当前循环次数
        /// </summary>
        private int currentLoopCount = 0;

        /// <summary>
        /// 目标循环次数
        /// </summary>
        private int targetLoopCount = 2;

        /// <summary>
        /// 是否启用循环模式
        /// </summary>
        private bool isLoopEnabled = true;

        /// <summary>
        /// UC
        /// </summary>
        public static string UC = "";
        /// <summary>
        /// SN
        /// </summary>
        public static string SN = "";
        /// <summary>
        /// 显示log的事件
        /// </summary>
        public static event Action<string> ShowLogEvent;
        /// <summary>
        /// 显示log的事件
        /// </summary>
        public static event Action ShowtxtSN;
        /// <summary>
        /// 显示SN的事件
        /// </summary>
        public static event Action<string> showSN_Event;
        /// <summary>
        /// 显示结果的事件
        /// </summary>
        public static event Action<ProductPoint> showResultEvent;

        /// <summary>
        /// 提示
        /// </summary>
        public static event Action<string> ShowSignificant;

        /// <summary>
        /// 显示提示的事件
        /// </summary>
        public static event Action<string> showHinttEvent;

        /// <summary>
        /// 上一片产品的结束时间
        /// </summary>
        public static DateTime lastEndTime = DateTime.Now;
        /// <summary>
        /// 是否第一次做料
        /// </summary>
        public static bool isFirstProduct = true;
        /// <summary>
        /// 是否工作中
        /// </summary>
        public static bool isWorking = false;

        public List<double> listData=new List<double>();
        string ccdResult1 = "";
        string ccdResult2 = "";

        public Dictionary<string, string> datas = new Dictionary<string, string>();

        clsMachine pMachine = null;
        Base pSelectBase;
        DrvValve pSelectValve;
        DrvValve.tyValve_Parameter ValveParameter = new DrvValve.tyValve_Parameter();
        ImageList ImgList = new ImageList();
        Dictionary<string, DrvValve> showValveList;

        public Mainflow(Type homeEnum1, Type stepEnum1, string instanceName1, Base parent) : base(homeEnum1, stepEnum1, instanceName1, parent, false)
        {
            axisCalibration =  new Cowain_AutoMotion.AxisCalibration(typeof(Base.HomeStep_Base), typeof(AxisCalibration.AxisCalibration_WorkStep), "标定流程", this);
            AddBase(ref axisCalibration.m_NowAddress);
        }

        public void addValveList()
        {
            //listView_Valve.Clear();
            int SelectTab = 0;
            for (int i = 0; i < showValveList.Count; i++)
            {
                pSelectValve = (DrvValve)showValveList[showValveList.Keys.ToList()[i]].m_NowAddress;
                pSelectValve.GetParameter(ref ValveParameter);
                string StrText = ValveParameter.strID + "  " + ValveParameter.strCName;
                StrText = JudgeLanguage.JudgeLag(ValveParameter.strID + "  " + ValveParameter.strCName);
                if (pSelectBase == null)
                {
                    pSelectBase = showValveList[showValveList.Keys.ToList()[i]].m_NowAddress;
                }
                //listView_Valve.Items.Add(StrText, "UseValve");
            }
            if (pSelectBase == null)
            {
                return;
            }
            pSelectValve = (DrvValve)pSelectBase;
            pSelectValve.GetParameter(ref ValveParameter);
            //comboBox_Mode.SelectedIndex = 0;
            //label_ValveID.Text = JudgeLanguage.JudgeLag(ValveParameter.strID + "  " + ValveParameter.strCName.Trim());

        }

        public enum Mainflow_HomeStep
        {
            Start = 0,
            Z轴回原,
            判断Z轴回原完成,
            Y轴回原,
            判断Y轴回原完成,
            R1轴回原,

            判断R轴回原完成,
            Completed,
            X轴回原,
            判断X轴回原完成,
            XY轴回原,
  
            移动到待命位
        }
        public enum Mainflow_WorkStep
        {
            Start = 0,
            移动到夹取位,
            夹取向下夹取,
            避障物料夹取XY位,
            前进夹取,
            电夹爪夹,
            判断电夹爪夹取状态,
            等待双启按钮被按下,
            取料完成Z轴向上,
            Y轴移动到下相机,
            触发下相机拍照,
            等待相机返回数据1,
            解析数据1,
            到上相机拍照位,
            触发2拍照,
            等待相机返回数据2,
            解析数据2,
            触发相机计算,
            接收相机反馈结果,
            移动到组装XY位,
            移动到组装Z位,
            移动到组装Z位到位,
            等待引导点位运动完成,
            电夹爪打开,
            电夹爪打开状态结束,
            抬Z轴去安全位,
            等待Z轴到位,
            晃动前触发侧复检相机拍照,
            晃动前触发后相机拍照,
            晃动前等待复检后相机返回数据,
            晃动前解析复检后相机数据,
            晃动前等待复检相机返回数据,
            晃动前解析复检数据,
            晃动前触发上相机拍照,
            晃动前等待复检上相机返回数据,
            晃动前解析复检上相机数据,
            检查循环条件,
            Y轴往后,
            Y轴往前,
            触发侧复检相机拍照,
            触发后相机拍照,
            等待复检后相机返回数据,
            解析复检后相机数据,
            等待复检相机返回数据,
            解析复检数据,
            触发上相机拍照,
            等待复检上相机返回数据,
            解析复检上相机数据,
            移动到待命位,
            回到组装XY位,
            判断夹取放料状态,
            取料Z轴向上,
            回到组装Z位,
            夹取放料耳机,
            重新回到夹取XY位,
            重新回到夹取Z位,
            重新松开电夹爪,
            重新松开电夹爪结果,
            Completed,
            压缩图片,
            mes上传数据,
            hive上传数据,
            mes提交过站,
            UC获取SN,
            检查产品类型,
            MesCheckSN,
            删除图片,
            BobcatCheckSN,
            Bobcat提交过站
        }
        public override void HomeCycle(ref double dbTime)
        {
            currentHomeStep = (Mainflow_HomeStep)m_nHomeStep;
            switch (currentHomeStep)
            {
                case Mainflow_HomeStep.Start:
                    m_bHomeCompleted = false;

                   
                    m_nHomeStep = (int)Mainflow_HomeStep.Z轴回原;
                    break;
                case Mainflow_HomeStep.Z轴回原:

                    LogAuto.Notify("Z轴回原点开始复位！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    HardWareControl.getMotor(EnumParam_Axis.Z).DoHome();

                    m_nHomeStep = (int)Mainflow_HomeStep.判断Z轴回原完成;
                    break;
                
                case Mainflow_HomeStep.判断Z轴回原完成:
                    if (HardWareControl.getMotor(EnumParam_Axis.Z).isHomeCompleted())
                    {
                        LogAuto.Notify("Z轴回原点完成&气缸复位！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        ///HardWareControl.getValve(EnumParam_Valve.载具打开气缸).Close();
                        m_nHomeStep = (int)Mainflow_HomeStep.Y轴回原;
                    }
                    break;
                case Mainflow_HomeStep.Y轴回原:

                    LogAuto.Notify("Y轴回原点开始复位！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    //HardWareControl.getMotor(EnumParam_Axis.X).DoHome();
                    HardWareControl.getMotor(EnumParam_Axis.Y).DoHome();
                    m_nHomeStep = (int)Mainflow_HomeStep.判断Y轴回原完成;
                    break;
                case Mainflow_HomeStep.判断Y轴回原完成:
                    if ( HardWareControl.getMotor(EnumParam_Axis.Y).isHomeCompleted())//& HardWareControl.getValve(EnumParam_Valve.载具打开气缸).isIDLE())
                    {
                        LogAuto.Notify("R2轴回原点开始复位！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.getMotor(EnumParam_Axis.R2).DoHome();
                        m_nHomeStep = (int)Mainflow_HomeStep.R1轴回原;
                    }
                    break;
                case Mainflow_HomeStep.R1轴回原:
                    if (HardWareControl.getMotor(EnumParam_Axis.R2).isHomeCompleted())
                    {
                        LogAuto.Notify("R1轴回原点开始复位！", (int)MachineStation.主监控, MotionLogLevel.Info);

                        HardWareControl.getMotor(EnumParam_Axis.R1).DoHome();
                        m_nHomeStep = (int)Mainflow_HomeStep.X轴回原;
                    }
                    break;
                case Mainflow_HomeStep.X轴回原:
                    if (HardWareControl.getMotor(EnumParam_Axis.R1).isHomeCompleted() )
                    {
                        LogAuto.Notify("X轴回原点开始复位！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.getMotor(EnumParam_Axis.X).DoHome();
                        m_nHomeStep = (int)Mainflow_HomeStep.移动到待命位;
                    }
                    break;

                case Mainflow_HomeStep.移动到待命位:
                    if (HardWareControl.getMotor(EnumParam_Axis.X).isHomeCompleted())
                    {
                        LogAuto.Notify("XY轴回原点开始复位！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.movePoint(EnumParam_Point.待命位);
                        MachineDataDefine.miSuMiControl.OpenToZero();
                        HardWareControl.getInputIO(EnumParam_InputIO.吸真空静点).SetIO(true);// 气缸后退
                        m_nHomeStep = (int)Mainflow_HomeStep.Completed;
                    }
                    break;

                case Mainflow_HomeStep.Completed:
                    if (HardWareControl.getPointIdel(EnumParam_Point.待命位))
                    {
                        m_bHomeCompleted = true;
                        isWorking=false;
                        LogAuto.Notify("主流程工位复位完成！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        m_Status = 狀態.待命;
                    }
                    break;
            }
        }
        public override void StepCycle(ref double dbTime)
        {
            if ((Mainflow_WorkStep)m_nStep != currentWorkStep)
            {
                LogAuto.Notify($"{currentWorkStep}", (int)MachineStation.主监控, MotionLogLevel.Info);
            }
            currentWorkStep = (Mainflow_WorkStep)m_nStep;
            switch (currentWorkStep)
            {
                case Mainflow_WorkStep.Start:

                    if (HardWareControl.getInputIO(EnumParam_InputIO.启动按钮).GetValue())
                    {
                        isWorking = true;
                        listData.Clear();
                        gripRetryCount = 0;  // 重置电爪重试计数
                        currentLoopCount = 0; // 循环次数
                        LogAuto.Notify("开始作料！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        gripTimeStopwatch.Restart();
                        m_nStep = (int)Mainflow_WorkStep.移动到夹取位;
                    }
                    break;
                case Mainflow_WorkStep.移动到夹取位:

                     LogAuto.Notify("XY移动到夹取位！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    HardWareControl.movePoint(EnumParam_Point.物料夹取XY位);
                    m_nStep = (int)Mainflow_WorkStep.避障物料夹取XY位;

                    break;
                case Mainflow_WorkStep.避障物料夹取XY位:
                    if (HardWareControl.getPointIdel(EnumParam_Point.物料夹取XY位))
                    {
                        向下取料startTime = DateTime.Now;
                        HardWareControl.movePoint(EnumParam_Point.避障物料夹取XY位);
                        m_nStep = (int)Mainflow_WorkStep.夹取向下夹取;

                    }
                    break;
                case Mainflow_WorkStep.夹取向下夹取:
                    if (HardWareControl.getPointIdel(EnumParam_Point.避障物料夹取XY位))
                    {
                        向下取料startTime = DateTime.Now;
                        HardWareControl.movePoint(EnumParam_Point.物料夹取Z位);
                        m_nStep = (int)Mainflow_WorkStep.前进夹取;

                    }
                    break;
                case Mainflow_WorkStep.前进夹取:
                    if (HardWareControl.getPointIdel(EnumParam_Point.物料夹取Z位))
                    {
                        HardWareControl.movePoint(EnumParam_Point.前进夹取);
                        m_nStep = (int)Mainflow_WorkStep.电夹爪夹;

                    }
                    break;
                case Mainflow_WorkStep.电夹爪夹:
                    if (HardWareControl.getPointIdel(EnumParam_Point.前进夹取))
                    {
                        //product.startTime = DateTime.Now;
                        DateTime endTime = DateTime.Now;
                        double OpenTime = double.Parse((endTime - 向下取料startTime).TotalSeconds.ToString("0.00"));
                        LogAuto.Notify($"向下取料！耗时：{OpenTime} s", (int)MachineStation.主监控, MotionLogLevel.Info);
                        准备夹取startTime = DateTime.Now;
                        if (MachineDataDefine.miSuMiControl.MoveWithParams(MachineDataDefine.miSuMiControl.Position, MachineDataDefine.miSuMiControl.Speed, MachineDataDefine.miSuMiControl.Force)) // 1650对应短柄耳机
                        {
                            m_nStep = (int)Mainflow_WorkStep.判断电夹爪夹取状态;
                            LogAuto.Notify("电夹爪开始夹取！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        }
                        else
                        {
                            LogAuto.Notify("电夹爪关闭指令发送失败！", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                            m_nStep = (int)Mainflow_WorkStep.移动到待命位;
                        }

                        break;
                    }
                    break;

                case Mainflow_WorkStep.判断电夹爪夹取状态:
                    LogAuto.Notify("等待电夹爪夹取完成！", (int)MachineStation.主监控, MotionLogLevel.Info);

                    // 等待电爪进入夹持状态（GRIP_HOLDING）后立即放行
                    if (MachineDataDefine.miSuMiControl.WaitGripHolding(10000, 5))
                    {
                        DateTime 动作完成endTime = DateTime.Now;
                        double Time = double.Parse((动作完成endTime - 准备夹取startTime).TotalSeconds.ToString("0.00"));
                        LogAuto.Notify($"夹取动作完成！耗时：{Time} s", (int)MachineStation.主监控, MotionLogLevel.Info);
                        DateTime endTime = DateTime.Now;
                        double OpenTime = double.Parse((endTime - 准备夹取startTime).TotalSeconds.ToString("0.00"));
                        LogAuto.Notify($"判断真正夹到工件！耗时：{OpenTime} s", (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.getOutputIO(EnumParam_OutputIO.载具打开气缸).SetIO(false);// 气缸后退
                        gripRetryCount = 0;  // 重置重试计数
                        m_nStep = (int)Mainflow_WorkStep.取料完成Z轴向上;
                    }
                    else
                    {
                        // 夹取失败或超时处理
                        LogAuto.Notify("电夹爪夹取超时或未进入夹持状态！", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                        showHinttEvent("电夹爪夹取超时，请检查！");
                        MachineDataDefine.miSuMiControl.OpenToZero();  // 打开电爪
                        gripRetryCount = 0;
                        m_nStep = (int)Mainflow_WorkStep.移动到待命位;
                    }
                    break;
                case Mainflow_WorkStep.取料完成Z轴向上:

                    bool bisOpen = HardWareControl.getInputIO(EnumParam_InputIO.载具打开气缸原点).GetValue();// 

                    //if (bisOpen)
                    {
                        取料准备向上startTime = DateTime.Now;
                        HardWareControl.movePoint(EnumParam_Point.Z轴安全位);
                        m_nStep = (int)Mainflow_WorkStep.Y轴移动到下相机;
                    }

                    break;
                case Mainflow_WorkStep.Y轴移动到下相机:
                    if (HardWareControl.getPointIdel(EnumParam_Point.Z轴安全位))
                    {
                        DateTime endTime = DateTime.Now;
                        double closeTime = double.Parse((endTime - 取料准备向上startTime).TotalSeconds.ToString("0.00"));
                        LogAuto.Notify($"取料完成向上！耗时：{closeTime} s", (int)MachineStation.主监控, MotionLogLevel.Info);
                        LogAuto.Notify("Y轴移动到下相机位！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.movePoint(EnumParam_Point.下相机拍照位);
                        m_nStep = (int)Mainflow_WorkStep.触发下相机拍照;
                    }
                    break;

                case Mainflow_WorkStep.触发下相机拍照:
                    if (HardWareControl.getPointIdel(EnumParam_Point.下相机拍照位))
                    {
                        LogAuto.Notify("触发下相机拍照！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        if (MachineDataDefine.machineState.b_UseCCD)
                        {
                            LogAuto.Notify("获取轴当前坐标！", (int)MachineStation.主监控, MotionLogLevel.Info);
                            //double axisX = HardWareControl.getMotor(EnumParam_Axis.X).GetPosition();
                            //double axisY = HardWareControl.getMotor(EnumParam_Axis.Y).GetPosition();
                            //double axisR = HardWareControl.getMotor(EnumParam_Axis.R1).GetPosition();
                            string SN = "1";
                            string str = "CCD1," + "jiupian" + "," + SN; // 下相机纠偏
                            HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr = "";
                            HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).SendMSG(str);
                            ShowLogEvent("发送拍照指令：" + str);
                            m_nStep = (int)Mainflow_WorkStep.等待相机返回数据1;
                        }
                        else
                        {
                            LogAuto.Notify("未启用相机！", (int)MachineStation.主监控, MotionLogLevel.Info);
                            m_nStep = (int)Mainflow_WorkStep.到上相机拍照位;
                        }
                    }
                    break;
                case Mainflow_WorkStep.等待相机返回数据1:

                    timerDelay.Enabled = false;
                    timerDelay.Interval = 2000;
                    timerDelay.Start();
                    m_nStep = (int)Mainflow_WorkStep.解析数据1;

                    break;

                case Mainflow_WorkStep.解析数据1:
                    if (HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr != "")
                    {
                        ccdResult1 = HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr;
                        LogAuto.Notify("下相机拍照返回结果！" + ccdResult1, (int)MachineStation.主监控, MotionLogLevel.Info);
                        ShowLogEvent("接受下相机拍照返回结果：" + ccdResult1);
                        string[] ccd = ccdResult1.Split(',');

                        if (ccd[1] == "OK")//结果ok
                        {
                            LogAuto.Notify("下相机拍照返回结果OK！" + ccdResult1, (int)MachineStation.主监控, MotionLogLevel.Info);
                            m_nStep = (int)Mainflow_WorkStep.到上相机拍照位;
                        }
                        else
                        {
                            product.pass = false;
                            LogAuto.Notify("下相机拍照结果NG!" + ccdResult1, (int)MachineStation.主监控, MotionLogLevel.Info);
                            showHinttEvent("下相机拍照结果999！请重新拍照");
                            m_nStep = (int)Mainflow_WorkStep.移动到待命位;
                        }
                    }
                    else if (timerDelay.Enabled == false)
                    {
                        showHinttEvent("下相机拍照返回结果超时");
                        LogAuto.Notify("下相机拍照返回结果超时！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        m_nStep = (int)Mainflow_WorkStep.移动到待命位;
                    }
                    break;
                case Mainflow_WorkStep.到上相机拍照位:
                    HardWareControl.movePoint(EnumParam_Point.上相机拍照位);
                    m_nStep = (int)Mainflow_WorkStep.触发2拍照;
                    break;
                case Mainflow_WorkStep.触发2拍照:
                    Thread.Sleep(50);
                    if (HardWareControl.getPointIdel(EnumParam_Point.上相机拍照位))
                    {
                        LogAuto.Notify("已到达上相机拍照位！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        if (MachineDataDefine.machineState.b_UseCCD)
                        {
                            double axisX = HardWareControl.getMotor(EnumParam_Axis.X).GetPosition();
                            double axisY = HardWareControl.getMotor(EnumParam_Axis.Y).GetPosition();
                            double axisR = HardWareControl.getMotor(EnumParam_Axis.R1).GetPosition();

                            string SN = "1";
                            string str = "CCD2," + "dingwei" + "," + SN + "," + "1"; // 上相机定位空拍
                            HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr = "";
                            HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).SendMSG(str);
                            Thread.Sleep(300);
                            m_nStep = (int)Mainflow_WorkStep.等待相机返回数据2;
                        }
                        else
                        {
                            LogAuto.Notify("未启用相机！", (int)MachineStation.主监控, MotionLogLevel.Info);
                            m_nStep = (int)Mainflow_WorkStep.移动到组装XY位;
                        }
                    }
                    break;
                case Mainflow_WorkStep.等待相机返回数据2:

                    timerDelay.Enabled = false;
                    timerDelay.Interval = 2000;
                    timerDelay.Start();
                    m_nStep = (int)Mainflow_WorkStep.解析数据2;

                    break;
                case Mainflow_WorkStep.解析数据2:
                    if (HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr != "")
                    {
                        ccdResult2 = HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr;
                        LogAuto.Notify("上相机拍照返回结果！" + ccdResult2, (int)MachineStation.主监控, MotionLogLevel.Info);
                        ShowLogEvent("接受上相机拍照返回结果：" + ccdResult2);
                        string[] ccd = ccdResult2.Split(',');

                        if (ccd[1] == "OK")//结果ok
                        {
                            LogAuto.Notify("上相机拍照返回结果OK！" + ccdResult2, (int)MachineStation.主监控, MotionLogLevel.Info);
                            product.pass = true;

                            if (double.TryParse(ccd[2], out double X) && double.TryParse(ccd[3], out double Y) &&
                            double.TryParse(ccd[4], out double R1))
                            {
                                OutX = X;
                                OutY = Y;
                                OutR1 = R1;
                            }

                            if (ccdResult2.Contains("99999") || ccdResult2.Contains("99999"))
                            {
                                LogAuto.Notify("上相机拍照结果NG!" + ccdResult2, (int)MachineStation.主监控, MotionLogLevel.Info);
                                showHinttEvent("上相机拍照结果999！请重新拍照");
                                m_nStep = (int)Mainflow_WorkStep.移动到待命位;
                                break;
                            }
                            m_nStep = (int)Mainflow_WorkStep.移动到组装XY位;
                        }
                        else
                        {
                            product.pass = false;
                            LogAuto.Notify("上相机拍照结果NG!" + ccdResult2, (int)MachineStation.主监控, MotionLogLevel.Info);
                            showHinttEvent("上相机拍照结果999！请重新拍照");
                            m_nStep = (int)Mainflow_WorkStep.Completed;
                        }
                    }
                    else if (timerDelay.Enabled == false)
                    {
                        LogAuto.Notify("上相机返回结果超时！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        m_nStep = (int)Mainflow_WorkStep.Completed;
                    }
                    break;
                case Mainflow_WorkStep.移动到组装XY位:
                    //if (!MachineDataDefine.machineState.b_UseCCD)
                    //if(OutX!=0 && OutY!=0&& OutR1!=0)
                    //{
                    //    HardWareControl.getMotor(EnumParam_Axis.X).AbsMove(OutX, MachineDataDefine.machineState.machineSpeed);
                    //    HardWareControl.getMotor(EnumParam_Axis.Y).AbsMove(OutY, MachineDataDefine.machineState.machineSpeed);
                    //    HardWareControl.getMotor(EnumParam_Axis.R1).AbsMove(OutR1, MachineDataDefine.machineState.machineSpeed);
                        
                    //}

                    HardWareControl.movePoint(EnumParam_Point.放料组装XY位);
                    //else
                    //{
                    //    LogAuto.Notify("移动XY组装XY位XYR绝对坐标！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    //    double OutX = HardWareControl.getPoint(EnumParam_Point.组装XY位).Data1;
                    //    double OutY = HardWareControl.getPoint(EnumParam_Point.组装XY位).Data2;
                    //    double OutR1 = HardWareControl.getPoint(EnumParam_Point.组装XY位).Data4;
                    //    HardWareControl.getMotor(EnumParam_Axis.X).AbsMove(OutX, MachineDataDefine.machineState.machineSpeed);
                    //    HardWareControl.getMotor(EnumParam_Axis.Y).AbsMove(OutY, MachineDataDefine.machineState.machineSpeed);
                    //    HardWareControl.getMotor(EnumParam_Axis.R1).AbsMove(OutR1, MachineDataDefine.machineState.machineSpeed);
                    //}

                    m_nStep = (int)Mainflow_WorkStep.移动到组装Z位;
                    break;
                case Mainflow_WorkStep.移动到组装Z位:

                    //if (MachineDataDefine.machineState.b_UseCCD)
                    //{
                    //    if (HardWareControl.getMotor(EnumParam_Axis.X).isIDLE() && HardWareControl.getMotor(EnumParam_Axis.Y).isIDLE()
                    //   && HardWareControl.getMotor(EnumParam_Axis.R1).isIDLE())
                    //    {
                    //        LogAuto.Notify("移动到组装Z位！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    //        HardWareControl.movePoint(EnumParam_Point.组装Z位);
                    //        m_nStep = (int)Mainflow_WorkStep.移动到组装Z位到位;
                    //    }
                    //}
                    //else
                    //if (HardWareControl.getMotor(EnumParam_Axis.X).isIDLE() && HardWareControl.getMotor(EnumParam_Axis.Y).isIDLE()
                    //   && HardWareControl.getMotor(EnumParam_Axis.R1).isIDLE())
                    {
                        if (HardWareControl.getPointIdel(EnumParam_Point.放料组装XY位))
                        {
                            LogAuto.Notify("未启用相机移动到组装Z位！", (int)MachineStation.主监控, MotionLogLevel.Info);
                            HardWareControl.movePoint(EnumParam_Point.组装Z位);
                            m_nStep = (int)Mainflow_WorkStep.移动到组装Z位到位;
                        }
                    }

                    break;
                case Mainflow_WorkStep.移动到组装Z位到位:
                    if (HardWareControl.getPointIdel(EnumParam_Point.组装Z位))
                    {
                        HardWareControl.getOutputIO(EnumParam_OutputIO.吸真空).SetIO(true); // 黄色真空吸放料载具打开
                        //HardWareControl.getOutputIO(EnumParam_OutputIO.载具打开气缸).SetIO(true);// 黑色放料载具需要气缸前进
                        LogAuto.Notify("移动到组装Z位到位！", (int)MachineStation.主监控, MotionLogLevel.Info);

                        m_nStep = (int)Mainflow_WorkStep.等待引导点位运动完成;
                    }
                    break;

                case Mainflow_WorkStep.等待引导点位运动完成:
                    LogAuto.Notify("电夹爪开始打开,准备放料！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    准备放料startTime = DateTime.Now;
                    if (MachineDataDefine.miSuMiControl.MoveWithParams(500, 100, 1))
                    {
                        m_nStep = (int)Mainflow_WorkStep.电夹爪打开状态结束;
                    }
                    else
                    {
                        LogAuto.Notify("电夹爪打开指令发送失败！", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                        m_nStep = (int)Mainflow_WorkStep.移动到待命位;
                    }

                    break;

                case Mainflow_WorkStep.电夹爪打开状态结束:
                    LogAuto.Notify("等待电夹爪打开完成！", (int)MachineStation.主监控, MotionLogLevel.Info);

                    // 等待电爪打开完成
                    if (MachineDataDefine.miSuMiControl.WaitMovementComplete(10000))
                    {
                        DateTime endTime = DateTime.Now;
                        double OpenTime = double.Parse((endTime - 准备放料startTime).TotalSeconds.ToString("0.00"));
                        LogAuto.Notify($"放料松开夹爪！耗时：{OpenTime} s", (int)MachineStation.主监控, MotionLogLevel.Info);
                        LogAuto.Notify("电夹爪打开成功，移动到复检位！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        m_nStep = (int)Mainflow_WorkStep.抬Z轴去安全位;
                    }
                    else
                    {
                        LogAuto.Notify("电夹爪打开超时！", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                        showHinttEvent("电夹爪打开超时，请检查！");
                        m_nStep = (int)Mainflow_WorkStep.移动到待命位;
                    }
                    break;
                case Mainflow_WorkStep.抬Z轴去安全位:
                    {
                        HardWareControl.movePoint(EnumParam_Point.Z轴安全位);
                        m_nStep = (int)Mainflow_WorkStep.等待Z轴到位;
                    }
                    break;
                case Mainflow_WorkStep.等待Z轴到位:
                    if (HardWareControl.getPointIdel(EnumParam_Point.Z轴安全位))
                    {
                        //DateTime endTime = DateTime.Now;
                        //double OpenTime = double.Parse((endTime - startTime).TotlSeconds.ToString("0.00"));
                        //LogAuto.Notify($"放料完抬到Z轴！耗时：{OpenTime} s", (int)MachineStation.主监控, MotionLogLevel.Info);                     
                        HardWareControl.movePoint(EnumParam_Point.上相机拍照位);// 上相机拍照位就是左相机拍照位
                        m_nStep = (int)Mainflow_WorkStep.晃动前触发侧复检相机拍照;
                    }
                    break;
                case Mainflow_WorkStep.晃动前触发侧复检相机拍照:

                    if (HardWareControl.getPointIdel(EnumParam_Point.上相机拍照位))
                    {
                        if (MachineDataDefine.machineState.b_UseCCD)
                        {
                            double axisX = HardWareControl.getMotor(EnumParam_Axis.X).GetPosition();
                            double axisY = HardWareControl.getMotor(EnumParam_Axis.Y).GetPosition();
                            double axisR = HardWareControl.getMotor(EnumParam_Axis.R1).GetPosition();

                            string SN = "1";
                            string str = "CCD3," + "zuo" + "," + SN + "," + "1";
                            HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr = "";
                            HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).SendMSG(str);
                            m_nStep = (int)Mainflow_WorkStep.晃动前等待复检相机返回数据;
                        }
                        else
                        {
                            LogAuto.Notify("未启用相机！", (int)MachineStation.主监控, MotionLogLevel.Info);
                            m_nStep = (int)Mainflow_WorkStep.移动到待命位;
                        }
                    }
                    break;
                case Mainflow_WorkStep.晃动前等待复检相机返回数据:

                    timerDelay.Enabled = false;
                    timerDelay.Interval = 2000;
                    timerDelay.Start();
                    m_nStep = (int)Mainflow_WorkStep.晃动前解析复检数据;

                    break;

                case Mainflow_WorkStep.晃动前解析复检数据:
                    if (HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr != "")
                    {
                        ccdResult2 = HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr;
                        string[] ccd = ccdResult2.Split(',');
                        if (ccd[1] == "OK")//结果ok
                        {
                            LogAuto.Notify("上相机拍照返回结果OK！" + ccdResult2, (int)MachineStation.主监控, MotionLogLevel.Info);
                            //记录坐标值在本地                      
                            if (ccdResult2.Contains("99999") || ccdResult2.Contains("99999"))
                            {
                                //product.pass = false;
                                LogAuto.Notify("上相机拍照结果NG!" + ccdResult2, (int)MachineStation.主监控, MotionLogLevel.Info);
                                showHinttEvent("上相机拍照结果999！请重新拍照");

                            }

                            //HardWareControl.movePoint(EnumParam_Point.后相机拍照位); 黑色放料载具都是上相机拍照位
                            m_nStep = (int)Mainflow_WorkStep.晃动前触发后相机拍照;
                        }
                        else
                        {
                            product.pass = false;
                            LogAuto.Notify("上相机拍照结果NG!" + ccdResult2, (int)MachineStation.主监控, MotionLogLevel.Info);
                            showHinttEvent("上相机拍照结果999！请重新拍照");
                            m_nStep = (int)Mainflow_WorkStep.移动到待命位;

                        }
                    }
                    else if (timerDelay.Enabled == false)
                    {
                        LogAuto.Notify("上相机返回结果超时！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        m_nStep = (int)Mainflow_WorkStep.移动到待命位;
                    }

                    break;
                case Mainflow_WorkStep.晃动前触发后相机拍照:

                    Thread.Sleep(300);
                    //if (HardWareControl.getPointIdel(EnumParam_Point.后相机拍照位))
                    {
                        if (MachineDataDefine.machineState.b_UseCCD)
                        {
                            double axisX = HardWareControl.getMotor(EnumParam_Axis.X).GetPosition();
                            double axisY = HardWareControl.getMotor(EnumParam_Axis.Y).GetPosition();
                            double axisR = HardWareControl.getMotor(EnumParam_Axis.R1).GetPosition();

                            string SN = "1";
                            string str = "CCD4," + "hou" + "," + SN + "," + "1";
                            HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr = "";
                            HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).SendMSG(str);
                            m_nStep = (int)Mainflow_WorkStep.晃动前等待复检后相机返回数据;
                        }
                        else
                        {
                            LogAuto.Notify("未启用相机！", (int)MachineStation.主监控, MotionLogLevel.Info);
                            m_nStep = (int)Mainflow_WorkStep.移动到待命位;
                        }
                    }
                    break;
                case Mainflow_WorkStep.晃动前等待复检后相机返回数据:

                    timerDelay.Enabled = false;
                    timerDelay.Interval = 2000;
                    timerDelay.Start();
                    m_nStep = (int)Mainflow_WorkStep.晃动前解析复检后相机数据;

                    break;

                case Mainflow_WorkStep.晃动前解析复检后相机数据:
                    if (HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr != "")
                    {
                        ccdResult2 = HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr;
                        LogAuto.Notify("复检拍照返回结果！" + ccdResult2, (int)MachineStation.主监控, MotionLogLevel.Info);
                        ShowLogEvent("接受复检拍照返回结果：" + ccdResult2);
                        string[] ccd = ccdResult2.Split(',');

                        if (ccd[1] == "OK")//结果ok
                        {
                            LogAuto.Notify("上相机拍照返回结果OK！" + ccdResult2, (int)MachineStation.主监控, MotionLogLevel.Info);
                            //记录坐标值在本地
                            if (ccdResult2.Contains("99999") || ccdResult2.Contains("99999"))
                            {
                                LogAuto.Notify("上相机拍照结果NG!" + ccdResult2, (int)MachineStation.主监控, MotionLogLevel.Info);
                                showHinttEvent("上相机拍照结果999！请重新拍照");
                            }

                            //HardWareControl.movePoint(EnumParam_Point.上相机产品拍照位);
                            m_nStep = (int)Mainflow_WorkStep.晃动前触发上相机拍照;
                        }
                        else
                        {
                            product.pass = false;
                            LogAuto.Notify("上相机拍照结果NG!" + ccdResult2, (int)MachineStation.主监控, MotionLogLevel.Info);
                            showHinttEvent("上相机拍照结果999！请重新拍照");
                            m_nStep = (int)Mainflow_WorkStep.移动到待命位;

                        }
                    }
                    else if (timerDelay.Enabled == false)
                    {
                        LogAuto.Notify("上相机返回结果超时！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        m_nStep = (int)Mainflow_WorkStep.移动到待命位;
                    }

                    break;
                case Mainflow_WorkStep.晃动前触发上相机拍照:

                    //if (HardWareControl.getPointIdel(EnumParam_Point.上相机产品拍照位))
                    {
                        LogAuto.Notify("已到达上相机复检拍照位！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        if (MachineDataDefine.machineState.b_UseCCD)
                        {
                            double axisX = HardWareControl.getMotor(EnumParam_Axis.X).GetPosition();
                            double axisY = HardWareControl.getMotor(EnumParam_Axis.Y).GetPosition();
                            double axisR = HardWareControl.getMotor(EnumParam_Axis.R1).GetPosition();

                            string SN = "1";
                            string str = "CCD2," + "dingwei" + "," + SN + "," + "2"; // 上相机晃动前拍耳机
                            HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr = "";
                            HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).SendMSG(str);
                            m_nStep = (int)Mainflow_WorkStep.晃动前等待复检上相机返回数据;
                        }
                        else
                        {
                            LogAuto.Notify("未启用相机！", (int)MachineStation.主监控, MotionLogLevel.Info);
                            m_nStep = (int)Mainflow_WorkStep.移动到待命位;
                        }
                    }
                    break;
                case Mainflow_WorkStep.晃动前等待复检上相机返回数据:

                    timerDelay.Enabled = false;
                    timerDelay.Interval = 2000;
                    timerDelay.Start();
                    m_nStep = (int)Mainflow_WorkStep.晃动前解析复检上相机数据;

                    break;

                case Mainflow_WorkStep.晃动前解析复检上相机数据:
                    if (MachineDataDefine.machineState.b_UseCCD)
                    {
                        if (HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr != "")
                        {
                            ccdResult2 = HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr;
                            string[] ccd = ccdResult2.Split(',');

                            if (ccd[1] == "OK")//结果ok
                            {
                                LogAuto.Notify("上相机拍照返回结果OK！" + ccdResult2, (int)MachineStation.主监控, MotionLogLevel.Info);
                                //记录坐标值在本地
                                if (ccdResult2.Contains("99999") || ccdResult2.Contains("99999"))
                                {
                                    LogAuto.Notify("上相机拍照结果NG!" + ccdResult2, (int)MachineStation.主监控, MotionLogLevel.Info);
                                    showHinttEvent("上相机拍照结果999！请重新拍照");
                                }

                                HardWareControl.movePoint(EnumParam_Point.Y轴往后);
                                m_nStep = (int)Mainflow_WorkStep.Y轴往前;
                            }
                            else
                            {
                                product.pass = false;
                                LogAuto.Notify("上相机拍照结果NG!" + ccdResult2, (int)MachineStation.主监控, MotionLogLevel.Info);
                                showHinttEvent("上相机拍照结果999！请重新拍照");
                                m_nStep = (int)Mainflow_WorkStep.移动到待命位;
                            }
                        }
                        else if (timerDelay.Enabled == false)
                        {
                            LogAuto.Notify("上相机返回结果超时！", (int)MachineStation.主监控, MotionLogLevel.Info);
                            m_nStep = (int)Mainflow_WorkStep.移动到待命位;
                        }
                    }
                    else
                    {
                        HardWareControl.movePoint(EnumParam_Point.Y轴往后);
                        m_nStep = (int)Mainflow_WorkStep.Y轴往前;
                    }
                    
                    break;
                case Mainflow_WorkStep.Y轴往后:
                    if (HardWareControl.getPointIdel(EnumParam_Point.Y轴往前))
                    {
                        HardWareControl.movePoint(EnumParam_Point.Y轴往后);
                        m_nStep = (int)Mainflow_WorkStep.Y轴往前;
                    }               
                    break;                 
                case Mainflow_WorkStep.Y轴往前:
                    if (HardWareControl.getPointIdel(EnumParam_Point.Y轴往后))
                    {
                        currentLoopCount++;
                        HardWareControl.movePoint(EnumParam_Point.Y轴往前);
                        m_nStep = (int)Mainflow_WorkStep.检查循环条件;
                    }                      
                    break;
                case Mainflow_WorkStep.检查循环条件:
                    if (isLoopEnabled && currentLoopCount < (targetLoopCount + 1))
                    {                      
                        m_nStep = (int)Mainflow_WorkStep.Y轴往后;
                    }
                    else
                    {
                        if (HardWareControl.getPointIdel(EnumParam_Point.Y轴往前))
                        {
                            HardWareControl.movePoint(EnumParam_Point.上相机拍照位);
                            m_nStep = (int)Mainflow_WorkStep.触发侧复检相机拍照;
                        }                     
                    }
                    break;
                case Mainflow_WorkStep.触发侧复检相机拍照:

                    if (HardWareControl.getPointIdel(EnumParam_Point.上相机拍照位))
                    {
                        LogAuto.Notify("已到达上相机复检拍照位！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        if (MachineDataDefine.machineState.b_UseCCD)
                        {
                            double axisX = HardWareControl.getMotor(EnumParam_Axis.X).GetPosition();
                            double axisY = HardWareControl.getMotor(EnumParam_Axis.Y).GetPosition();
                            double axisR = HardWareControl.getMotor(EnumParam_Axis.R1).GetPosition();

                            string SN = "1";
                            string str = "CCD3," + "zuo" + "," + SN + "," + "2"; // 左相机第二次拍
                            HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr = "";
                            HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).SendMSG(str);
                            m_nStep = (int)Mainflow_WorkStep.等待复检相机返回数据;
                        }
                        else
                        {
                            LogAuto.Notify("未启用相机！", (int)MachineStation.主监控, MotionLogLevel.Info);
                            m_nStep = (int)Mainflow_WorkStep.移动到待命位;
                        }
                    }
                    break;
                case Mainflow_WorkStep.等待复检相机返回数据:

                    timerDelay.Enabled = false;
                    timerDelay.Interval = 2000;
                    timerDelay.Start();
                    m_nStep = (int)Mainflow_WorkStep.解析复检数据;

                    break;

                case Mainflow_WorkStep.解析复检数据:
                    if (HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr != "")
                    {
                        ccdResult2 = HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr;
                        LogAuto.Notify("复检拍照返回结果！" + ccdResult2, (int)MachineStation.主监控, MotionLogLevel.Info);
                        ShowLogEvent("接受复检拍照返回结果：" + ccdResult2);
                        string[] ccd = ccdResult2.Split(',');

                        if (ccd[1] == "OK")//结果ok
                        {
                            LogAuto.Notify("上相机拍照返回结果OK！" + ccdResult2, (int)MachineStation.主监控, MotionLogLevel.Info);
                         
                            //记录坐标值在本地                      
                            if (ccdResult2.Contains("99999") || ccdResult2.Contains("99999"))
                            {
                                //product.pass = false;
                                LogAuto.Notify("上相机拍照结果NG!" + ccdResult2, (int)MachineStation.主监控, MotionLogLevel.Info);
                                showHinttEvent("上相机拍照结果999！请重新拍照");
                                
                            }
                            //HardWareControl.movePoint(EnumParam_Point.后相机拍照位);
                            m_nStep = (int)Mainflow_WorkStep.触发后相机拍照;
                        }
                        else
                        {
                            product.pass = false;
                            LogAuto.Notify("上相机拍照结果NG!" + ccdResult2, (int)MachineStation.主监控, MotionLogLevel.Info);
                            showHinttEvent("上相机拍照结果999！请重新拍照");
                            m_nStep = (int)Mainflow_WorkStep.移动到待命位;
                          
                        }
                    }
                    else if (timerDelay.Enabled == false)
                    {
                        LogAuto.Notify("上相机返回结果超时！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        m_nStep = (int)Mainflow_WorkStep.移动到待命位;
                    }

                    break;
                case Mainflow_WorkStep.触发后相机拍照:

                    if (HardWareControl.getPointIdel(EnumParam_Point.后相机拍照位))
                    {
                        LogAuto.Notify("已到达后相机复检拍照位！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        if (MachineDataDefine.machineState.b_UseCCD)
                        {
                            double axisX = HardWareControl.getMotor(EnumParam_Axis.X).GetPosition();
                            double axisY = HardWareControl.getMotor(EnumParam_Axis.Y).GetPosition();
                            double axisR = HardWareControl.getMotor(EnumParam_Axis.R1).GetPosition();

                            string SN = "1";
                            string str = "CCD4," + "hou" + "," + SN + "," + "2"; // 后相机第二拍
                            HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr = "";
                            HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).SendMSG(str);
                            m_nStep = (int)Mainflow_WorkStep.等待复检后相机返回数据;
                        }
                        else
                        {
                            LogAuto.Notify("未启用相机！", (int)MachineStation.主监控, MotionLogLevel.Info);
                            m_nStep = (int)Mainflow_WorkStep.移动到待命位;
                        }
                    }
                    break;
                case Mainflow_WorkStep.等待复检后相机返回数据:

                    timerDelay.Enabled = false;
                    timerDelay.Interval = 2000;
                    timerDelay.Start();
                    m_nStep = (int)Mainflow_WorkStep.解析复检后相机数据;

                    break;

                case Mainflow_WorkStep.解析复检后相机数据:
                    if (HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr != "")
                    {
                        ccdResult2 = HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr;
                        LogAuto.Notify("复检拍照返回结果！" + ccdResult2, (int)MachineStation.主监控, MotionLogLevel.Info);
                        ShowLogEvent("接受复检拍照返回结果：" + ccdResult2);
                        string[] ccd = ccdResult2.Split(',');

                        if (ccd[1] == "OK")//结果ok
                        {
                            LogAuto.Notify("上相机拍照返回结果OK！" + ccdResult2, (int)MachineStation.主监控, MotionLogLevel.Info);
                            //记录坐标值在本地
                            if (ccdResult2.Contains("99999") || ccdResult2.Contains("99999"))
                            {
                                //product.pass = false;
                                LogAuto.Notify("上相机拍照结果NG!" + ccdResult2, (int)MachineStation.主监控, MotionLogLevel.Info);
                                showHinttEvent("上相机拍照结果999！请重新拍照");

                            }

                            //HardWareControl.movePoint(EnumParam_Point.上相机产品拍照位);
                            m_nStep = (int)Mainflow_WorkStep.触发上相机拍照;
                        }
                        else
                        {
                            product.pass = false;
                            LogAuto.Notify("上相机拍照结果NG!" + ccdResult2, (int)MachineStation.主监控, MotionLogLevel.Info);
                            showHinttEvent("上相机拍照结果999！请重新拍照");
                            m_nStep = (int)Mainflow_WorkStep.移动到待命位;

                        }
                    }
                    else if (timerDelay.Enabled == false)
                    {
                        LogAuto.Notify("上相机返回结果超时！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        m_nStep = (int)Mainflow_WorkStep.移动到待命位;
                    }

                    break;
                case Mainflow_WorkStep.触发上相机拍照:

                    //if (HardWareControl.getPointIdel(EnumParam_Point.上相机产品拍照位))
                    {
                        LogAuto.Notify("已到达上相机复检拍照位！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        if (MachineDataDefine.machineState.b_UseCCD)
                        {
                            double axisX = HardWareControl.getMotor(EnumParam_Axis.X).GetPosition();
                            double axisY = HardWareControl.getMotor(EnumParam_Axis.Y).GetPosition();
                            double axisR = HardWareControl.getMotor(EnumParam_Axis.R1).GetPosition();

                            string SN = "1";
                            string str = "CCD2," + "dingwei" + "," + SN + "," + "3"; // 上相机晃动后第三次拍
                            HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr = "";
                            HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).SendMSG(str);
                            m_nStep = (int)Mainflow_WorkStep.等待复检上相机返回数据;
                        }
                        else
                        {
                            LogAuto.Notify("未启用相机！", (int)MachineStation.主监控, MotionLogLevel.Info);
                            m_nStep = (int)Mainflow_WorkStep.移动到待命位;
                        }
                    }
                    break;
                case Mainflow_WorkStep.等待复检上相机返回数据:

                    timerDelay.Enabled = false;
                    timerDelay.Interval = 2000;
                    timerDelay.Start();
                    m_nStep = (int)Mainflow_WorkStep.解析复检上相机数据;

                    break;

                case Mainflow_WorkStep.解析复检上相机数据:
                    if (HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr != "")
                    {
                        ccdResult2 = HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr;
                        string[] ccd = ccdResult2.Split(',');

                        if (ccd[1] == "OK")//结果ok
                        {
                            LogAuto.Notify("上相机拍照返回结果OK！" + ccdResult2, (int)MachineStation.主监控, MotionLogLevel.Info);
                            //记录坐标值在本地
                            if (ccdResult2.Contains("99999") || ccdResult2.Contains("99999"))
                            {
                                //product.pass = false;
                                LogAuto.Notify("上相机拍照结果NG!" + ccdResult2, (int)MachineStation.主监控, MotionLogLevel.Info);
                                showHinttEvent("上相机拍照结果999！请重新拍照");

                            }
                            m_nStep = (int)Mainflow_WorkStep.移动到待命位;
                        }
                        else
                        {
                            product.pass = false;
                            LogAuto.Notify("上相机拍照结果NG!" + ccdResult2, (int)MachineStation.主监控, MotionLogLevel.Info);
                            showHinttEvent("上相机拍照结果999！请重新拍照");
                            m_nStep = (int)Mainflow_WorkStep.移动到待命位;

                        }
                    }
                    else if (timerDelay.Enabled == false)
                    {
                        LogAuto.Notify("上相机返回结果超时！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        m_nStep = (int)Mainflow_WorkStep.移动到待命位;
                    }

                    break;
                case Mainflow_WorkStep.移动到待命位:

                    LogAuto.Notify("XY移动到夹取位！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    HardWareControl.movePoint(EnumParam_Point.待命位);
                    MachineDataDefine.miSuMiControl.LowOpen();
                    //HardWareControl.getOutputIO(EnumParam_OutputIO.载具打开气缸).SetIO(true);// 夹紧气缸
                    HardWareControl.getOutputIO(EnumParam_OutputIO.吸真空).SetIO(false);
                    gripTimeStopwatch.Stop();
                    long gripTimeMs = gripTimeStopwatch.ElapsedMilliseconds;
                    LogAuto.Notify($"取放料全流程！耗时：{gripTimeMs} ms", (int)MachineStation.主监控, MotionLogLevel.Info);
                    m_nStep = (int)Mainflow_WorkStep.回到组装XY位;
                    break;
                case Mainflow_WorkStep.回到组装XY位:
                    // 移动到组装XY位
                    if (HardWareControl.getPointIdel(EnumParam_Point.待命位))
                    {
                        HardWareControl.movePoint(EnumParam_Point.组装XY位);
                        m_nStep = (int)Mainflow_WorkStep.回到组装Z位;
                    }
                    
                    break;
                case Mainflow_WorkStep.回到组装Z位:
                    // 移动到组装XY位
                    if (HardWareControl.getPointIdel(EnumParam_Point.组装XY位))
                    {
                        HardWareControl.movePoint(EnumParam_Point.组装Z位);
                        m_nStep = (int)Mainflow_WorkStep.夹取放料耳机;
                    }    
                    // 移动到Z轴组装位，准备夹取

                    break;
                case Mainflow_WorkStep.夹取放料耳机:
                    if (HardWareControl.getPointIdel(EnumParam_Point.组装Z位))
                    {
                        if (MachineDataDefine.miSuMiControl.MoveWithParams(MachineDataDefine.miSuMiControl.Position, MachineDataDefine.miSuMiControl.Speed, MachineDataDefine.miSuMiControl.Force)) // 1650对应短柄耳机
                        {
                            m_nStep = (int)Mainflow_WorkStep.判断夹取放料状态;
                        }
                        else
                        {
                            LogAuto.Notify("电夹爪关闭指令发送失败！", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                            m_nStep = (int)Mainflow_WorkStep.移动到待命位;
                        }

                        break;
                    }
                    break;

                case Mainflow_WorkStep.判断夹取放料状态:
                    
                    if (MachineDataDefine.miSuMiControl.WaitGripHolding(10000, 5))
                    {
                        HardWareControl.getOutputIO(EnumParam_OutputIO.吸真空).SetIO(false);//黄色吸真空 关闭吸真空
                        //HardWareControl.getOutputIO(EnumParam_OutputIO.载具打开气缸).SetIO(false);// 黑色硅胶气缸后退
                        m_nStep = (int)Mainflow_WorkStep.取料Z轴向上;
                    }
                    else
                    {
                        LogAuto.Notify("电夹爪放料超时或未确认释放！", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                        showHinttEvent("电夹爪放料超时，请检查！");
                        MachineDataDefine.miSuMiControl.OpenToZero();  // 打开电爪
                        gripRetryCount = 0;
                        m_nStep = (int)Mainflow_WorkStep.移动到待命位;
                    }
                    break;
                case Mainflow_WorkStep.取料Z轴向上:
                    //  取料完成          
                    HardWareControl.movePoint(EnumParam_Point.Z轴安全位);
                    m_nStep = (int)Mainflow_WorkStep.重新回到夹取XY位;       
                    break;
                case Mainflow_WorkStep.重新回到夹取XY位:
                    // 物料夹取XY位
                    if (HardWareControl.getPointIdel(EnumParam_Point.Z轴安全位))
                    {
                        HardWareControl.movePoint(EnumParam_Point.物料夹取XY位);
                        m_nStep = (int)Mainflow_WorkStep.重新回到夹取Z位;
                    }
                    break;
                case Mainflow_WorkStep.重新回到夹取Z位:
                    // 移动到Z轴位置前打开气缸
                    if (HardWareControl.getPointIdel(EnumParam_Point.物料夹取XY位))
                    {                    
                        HardWareControl.movePoint(EnumParam_Point.物料夹取Z位);
                        m_nStep = (int)Mainflow_WorkStep.重新松开电夹爪;
                    }
                    break;
                case Mainflow_WorkStep.重新松开电夹爪:
                    // 先打开气缸，固定夹爪
                    if (HardWareControl.getPointIdel(EnumParam_Point.物料夹取Z位))
                    {
                        HardWareControl.getOutputIO(EnumParam_OutputIO.载具打开气缸).SetIO(true);// 推进气缸夹上

                        // 再松开电夹爪
                        if (MachineDataDefine.miSuMiControl.MoveWithParams(0, 100, 1))
                        {
                            m_nStep = (int)Mainflow_WorkStep.重新松开电夹爪结果;
                        }
                        else
                        {
                            LogAuto.Notify("电夹爪打开指令发送失败！", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                            m_nStep = (int)Mainflow_WorkStep.移动到待命位;
                        }
                    }   
                    
                    break ;
                case Mainflow_WorkStep.重新松开电夹爪结果:
                    // 等待电夹爪确认已释放后再结束
                    if (MachineDataDefine.miSuMiControl.WaitGripReleased(10000, 5))
                    {
                        m_nStep = (int)Mainflow_WorkStep.Completed;
                    }
                    else
                    {
                        LogAuto.Notify("电夹爪打开超时！", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                        showHinttEvent("电夹爪打开超时，请检查！");
                        m_nStep = (int)Mainflow_WorkStep.Completed;
                    }

                    break;
                case Mainflow_WorkStep.Completed:
                    if (HardWareControl.getPointIdel(EnumParam_Point.待命位))
                    {
                        isWorking = false;
                        m_nStep = (int)Mainflow_WorkStep.Start;
                    }              
                    break;
            }
        }
        public override void Stop()
        {
            //RunnerIn.SN = "";
            PutProduct = false;
            isWorking=false;
            
            // 停止时断开电爪连接
            //try
            //{
            //    MachineDataDefine.miSuMiControl?.Disconnect();
            //}
            //catch (Exception ex)
            //{
            //    LogAuto.Notify($"断开电爪连接失败：{ex.Message}", (int)MachineStation.主监控, MotionLogLevel.Alarm);
            //}
            
            base.Stop();
        }
        private string SubSN(string str)
        {
            if (str.Contains("SN="))
            {
                string[] strings = str.Split('=');
                // {"Result":true,"Retmsg":"SN=****************"}
                return strings[1];
            }
            else
            {
                return "";
            }
        }
    }
}


