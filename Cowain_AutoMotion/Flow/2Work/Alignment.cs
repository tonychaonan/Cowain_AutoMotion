using Chart;
using Cowain_AutoMotion.Flow;
using Cowain_AutoMotion.FormView._4弹窗;
using Cowain_Form.FormView;
using Cowain_Machine;
using Cowain_Machine.Flow;
using MotionBase;
using Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Cowain_AutoMotion.SQLSugarHelper;
using static Cowain_Machine.Flow.MErrorDefine;
using System.Windows.Forms;
using Cowain;
using System.Diagnostics;

namespace Cowain_AutoMotion
{
    public class Alignment : Base
    {
        public Alignment(Base parent) : base(parent, false)
        {
            axisCalibration = new Cowain_AutoMotion.AxisCalibration(this);
            AddBase(ref axisCalibration.m_NowAddress);
            nullModelStep = new List<double[]>();
            nullModelStep.Add(new double[] { 0.2, 0.2, 1 });
            nullModelStep.Add(new double[] { 0.1, 0.1, 0.5 });
            nullModelStep.Add(new double[] { -0.2, -0.2, -1 });
            nullModelStep.Add(new double[] { -0.1, -0.1, -0.5 });
            nullModelStep.Add(new double[] { 0.2, 0.2, -0.5 });
        }
        public static bool Photoretry = false;
        public static string[] PhotoNGmsg;
        private AlignmentMode_WorkStep currentWorkStep;
        int index = 0;
        double speed = 30;
        public static bool b_Result = true;

        public int Resetcount = 0;
        /// <summary>
        /// 下料检查拍照
        /// </summary>
        public bool b_DownloadCheck = false;
        public Dictionary<string, string> datas = new Dictionary<string, string>();
        public Dictionary<string, string> datas1 = new Dictionary<string, string>();
        public AlignmentMode_HomeStep currentHomeStep;
        public MachineData alignmengPoint = null;
        public AxisCalibration axisCalibration;
        public static string holderSN = "";
        public static string SN = "";
        /// <summary>
        /// alignment数据
        /// </summary>
        private string alignData1 = "";
        private string alignData3 = "";
        /// <summary>
        /// 插拔数据
        /// </summary>
        private string alignData2 = "";
        MachineDataDefine define = new MachineDataDefine();
        /// <summary>
        /// 调整位要料信号
        /// </summary>
        public static bool alignAskProduct = false;
        /// <summary>
        /// 调整位做料完成
        /// </summary>
        public static bool alignProductCompelet = false;
        /// <summary>
        /// 空跑的测试运动
        /// </summary>
        public List<double[]> nullModelStep = new List<double[]>();
        /// <summary>
        /// 插拔测试
        /// </summary>
        public bool b_InAndOutModel = false;
        /// <summary>
        /// 测数据模式
        /// </summary>
        public bool b_DataModel = false;
        /// <summary>
        /// 测试数据模式循环3次
        /// </summary>
        private int DataIndex = 0;
        /// <summary>
        /// 统计作料数量
        /// </summary>
        public int productCount = 0;
        /// <summary>
        /// 插拔计数代替SN
        /// </summary>
        public int num = 0;
        /// <summary>
        /// 复检指令
        /// </summary>
        string strCheck;
        string alignTimeOK;
        string dataStrOK;
        public string starttime;
        public string endtime;
        /// <summary>
        /// sn界面显示事件
        /// </summary>
        // public delegate void SNShowDelegate(string showMsg, bool ok);
        public delegate void SNShowDelegate(string sn, bool ok);
        public event SNShowDelegate SNShowEvent;
        public int cylinerstate = 0;
        /// <summary>
        /// 复检后重新调整
        /// </summary>
        public bool Rework = false;
        /// <summary>
        /// 记录每次rework数据
        /// </summary>
        string strmsg1 = "";
        string strmsg2 = "";
        string strmsg3 = "";
        /// <summary>
        /// 每次rework调整次数
        /// </summary>
        public int reworknum = 0;
        /// <summary>
        /// 每次rework调整次数
        /// </summary>
        public int reworknum2 = 0;
        /// <summary>
        /// 每次rework调整次数
        /// </summary>
        public int reworknum3 = 0;
        /// <summary>
        /// 记录膨胀气缸动作次数
        /// </summary>
        public int cylinderCount = 0;
        /// <summary>
        /// 残胶抛料统计
        /// </summary>
        public static int NGCount = 0;
        /// <summary>
        /// 超限NG下料
        /// </summary>
        //  public static bool SendFail = false;
        public static bool 下料 = false;

        public static bool NG拍照下料显示 = false;
       // public static bool NG拍照下料显示 = false;

        public static bool NG超限下料显示 = false;

        public static bool 调整超时下料显示 = false;

        public static bool NG下料 = false;

        public static 调整位要料信号 调整要料信号 = 调整位要料信号.中转;
        /// <summary>
        /// 中转NG抛料
        /// </summary>
        public static bool 流道NG抛料 = false;
        
        public static bool 膨胀气缸报警 = false;
        public Stopwatch sw = new Stopwatch();
        public string  time;
        /// <summary>
        /// LAD模式下GRR次数
        /// </summary>
        public int LadNum = 0;
        internal static bool Mes反馈超时=false;
        internal static bool Hive反馈超时=false;
        internal static bool PDCA反馈超时=false;
        internal static bool 上传PDCA失败=false;

        public enum 调整位要料信号
        {
            要料,
            不要料,
            中转
        }
        public enum AlignmentMode_HomeStep
        {
            Start = 0,
            调整位定位销固定顶升气缸上1,
            调整位膨胀气缸回,
            X1轴回原,
            调整位膨胀顶升气缸下,
            调整位压板限位气缸回,
            横移轴回原点,
            横移轴移动到待命位延时,
            横移轴移动到待命位,
            R轴回原点,
            X轴和Y轴回原点,
            X轴回原点延时,
            X轴回原点到位延时,
            调整位定位销固定顶升气缸下1,
            Completed
        }
        public enum AlignmentMode_WorkStep
        {
            Start = 0,
            调整位运动到调整原点,
            横移X轴待待命位,
            置位要料信号,
            等待有料,
            横移X运动到拍照位,
            调整位压板限位气缸出,
            调整位定位销固定顶升气缸上,
            调整位膨胀顶升气缸上,
            调整位膨胀气缸膨胀,
            调整位定位销固定顶升气缸下,
            触发相机拍照,
            相机重连,
            相机NG抛料,
            NG抛料,
            置位相机触发IOTrue,
            等待延时,
            置位相机触发IOFalse,
            等待相机反馈,
            判断超时时间,
            CheckUop,
            产品在当站,
            运动引导点位,
            等待引导点位运动完成,
            调整位定位销固定顶升气缸上1,
            调整位定位销固定顶升气缸上1延时,
            调整位膨胀气缸回,
            调整位膨胀气缸到位,
            调整位膨胀顶升气缸下,
            调整位膨胀顶升气缸到位,
            调整位定位销固定顶升气缸下1,
            调整位压板限位气缸缩回,
            等待调整位压板限位气缸缩回,
            做料完成,
            等待做料完成信号置位,
            上传数据,
            提交过站,
            上传失败下料,
            等待提交过站完成,
            上传HIVE,
            上传PDCA,
            等待PDCA上传完成,
            调整位顶升气缸抬,
            LAD等待延时,
            LAD等待延时1,
            ThreeAndFive,
            连三不连五变量,
            Completed
        }
        public override void HomeCycle(ref double dbTime)
        {
            currentHomeStep = (AlignmentMode_HomeStep)m_nHomeStep;
            switch (currentHomeStep)
            {
                case AlignmentMode_HomeStep.Start:
                    MachineDataDefine.StopNG = false;
                    m_bHomeCompleted = false;
                    下料 = false;
                    Alignment.holderSN = "";
                    Alignment.SN = "";
                    流道NG抛料 = false;
                    NG超限下料显示 = false;
                    NG拍照下料显示 = false;
                    调整超时下料显示 = false;
                    Photoretry = false;
                    膨胀气缸报警 = false;
                    Resetcount = 0;
                    Rework = false;
                    m_nHomeStep = (int)AlignmentMode_HomeStep.调整位定位销固定顶升气缸上1;
                    break;
                case AlignmentMode_HomeStep.调整位定位销固定顶升气缸上1:
                    HardWareControl.getValve(EnumParam_Valve.调整位定位销固定顶升气缸上).Open();
                    m_nHomeStep = (int)AlignmentMode_HomeStep.调整位膨胀气缸回;
                    break;
                case AlignmentMode_HomeStep.调整位膨胀气缸回:
                    if (HardWareControl.getValve(EnumParam_Valve.调整位定位销固定顶升气缸上).isIDLE())
                    {
                        HardWareControl.getValve(EnumParam_Valve.调整位膨胀气缸).Close();
                        HardWareControl.getMotor(EnumParam_Axis.X1).DoHome();
                        timerDelay.Enabled = false;
                        timerDelay.Interval = 500;
                        timerDelay.Start();
                        m_nHomeStep = (int)AlignmentMode_HomeStep.调整位膨胀顶升气缸下;
                    }
                    break;
                case AlignmentMode_HomeStep.调整位膨胀顶升气缸下:
                    if (HardWareControl.getValve(EnumParam_Valve.调整位膨胀气缸).isIDLE() && timerDelay.Enabled == false)
                    {

                        HardWareControl.getValve(EnumParam_Valve.调整位膨胀顶升气缸).Close();
                        m_nHomeStep = (int)AlignmentMode_HomeStep.横移轴移动到待命位延时;
                    }
                    break;
                case AlignmentMode_HomeStep.横移轴移动到待命位延时:
                    bool b = HardWareControl.getMotor(EnumParam_Axis.X1).isHomeCompleted();
                    bool a = HardWareControl.getMotor(EnumParam_Axis.X1).isIDLE();
                    // if (HardWareControl.getMotor(EnumParam_Axis.X1).isHomeCompleted()&& HardWareControl.getMotor(EnumParam_Axis.X1).isIDLE())
                    if (HardWareControl.getMotor(EnumParam_Axis.X1).isHomeCompleted() && HardWareControl.getMotor(EnumParam_Axis.X1).isIDLE())
                    {
                        //  HardWareControl.movePoint(EnumParam_Point.横移X轴待待命位);
                        timerDelay.Enabled = false;
                        timerDelay.Interval = 500;
                        timerDelay.Start();
                        m_nHomeStep = (int)AlignmentMode_HomeStep.横移轴移动到待命位;
                    }
                    break;
                case AlignmentMode_HomeStep.横移轴移动到待命位:
                    if (timerDelay.Enabled == false)
                    {
                        HardWareControl.movePoint(EnumParam_Point.横移X轴待待命位);
                        m_nHomeStep = (int)AlignmentMode_HomeStep.R轴回原点;
                    }
                    break;
                case AlignmentMode_HomeStep.R轴回原点:
                    // if (HardWareControl.getMotor(EnumParam_Axis.X1).isHomeCompleted())
                    if (HardWareControl.getValve(EnumParam_Valve.调整位膨胀顶升气缸).isIDLE())
                    {
                        HardWareControl.getMotor(EnumParam_Axis.R).DoHome();
                        m_nHomeStep = (int)AlignmentMode_HomeStep.X轴和Y轴回原点;
                    }
                    break;
                case AlignmentMode_HomeStep.X轴和Y轴回原点:
                    if (HardWareControl.getMotor(EnumParam_Axis.R).isHomeCompleted())
                    {
                        HardWareControl.getMotor(EnumParam_Axis.X2).DoHome();
                        HardWareControl.getMotor(EnumParam_Axis.Y3).DoHome();
                        timerDelay.Enabled = false;
                        timerDelay.Interval = 500;
                        timerDelay.Start();
                        m_nHomeStep = (int)AlignmentMode_HomeStep.X轴回原点延时;
                    }
                    break;
                case AlignmentMode_HomeStep.X轴回原点延时:
                    if (timerDelay.Enabled == false)
                    {
                        m_nHomeStep = (int)AlignmentMode_HomeStep.X轴回原点到位延时;
                    }
                    break;
                case AlignmentMode_HomeStep.X轴回原点到位延时:
                    if (HardWareControl.getMotor(EnumParam_Axis.X2).isHomeCompleted() && HardWareControl.getMotor(EnumParam_Axis.Y3).isHomeCompleted() && HardWareControl.getMotor(EnumParam_Axis.X2).isIDLE() && HardWareControl.getMotor(EnumParam_Axis.Y3).isIDLE())
                    {
                        timerDelay.Enabled = false;
                        timerDelay.Interval = 500;
                        timerDelay.Start();
                        m_nHomeStep = (int)AlignmentMode_HomeStep.调整位定位销固定顶升气缸下1;
                    }
                    break;
                case AlignmentMode_HomeStep.调整位定位销固定顶升气缸下1:
                    //bool a1=   HardWareControl.getMotor(EnumParam_Axis.X2).isHomeCompleted();
                    //   bool a2 = HardWareControl.getMotor(EnumParam_Axis.Y3).isHomeCompleted();
                    //   bool a3 = HardWareControl.getMotor(EnumParam_Axis.X2).isIDLE();
                    //   bool a4= HardWareControl.getMotor(EnumParam_Axis.Y3).isIDLE();
                    if (timerDelay.Enabled == false)
                    {
                        HardWareControl.getValve(EnumParam_Valve.调整位定位销固定顶升气缸上).Close();
                        m_nHomeStep = (int)AlignmentMode_HomeStep.调整位压板限位气缸回;
                    }
                    break;
                case AlignmentMode_HomeStep.调整位压板限位气缸回:
                    if (HardWareControl.getValve(EnumParam_Valve.调整位定位销固定顶升气缸上).isIDLE())
                    {
                        HardWareControl.getValve(EnumParam_Valve.调整位压板限位气缸).Close();
                        m_nHomeStep = (int)AlignmentMode_HomeStep.Completed;
                    }
                    break;

                case AlignmentMode_HomeStep.Completed:
                    if (HardWareControl.getValve(EnumParam_Valve.调整位压板限位气缸).isIDLE() && HardWareControl.getPointIdel(EnumParam_Point.横移X轴待待命位))
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
            currentWorkStep = (AlignmentMode_WorkStep)m_nStep;
            switch (currentWorkStep)
            {
                #region
                //case AlignmentMode_WorkStep.Start:
                //    DataIndex = 0;
                //    index = 0;
                //    b_Result = true;
                //    b_DownloadCheck = false;
                //    HardWareControl.getOutputIO(EnumParam_OutputIO.CCD相机触发拍照).SetIO(false);
                //    m_nStep = (int)AlignmentMode_WorkStep.调整位膨胀气缸初始状态回;
                //     break;
                //case AlignmentMode_WorkStep.调整位膨胀气缸初始状态回:

                //        LogAuto.Notify("调整位膨胀气缸关闭！", (int)MachineStation.主监控, LogLevel.Info);
                //        HardWareControl.getValve(EnumParam_Valve.调整位膨胀气缸).Close();
                //        timerDelay.Enabled = false;
                //        timerDelay.Interval = 100;
                //        timerDelay.Start();
                //        m_nStep = (int)AlignmentMode_WorkStep.调整位膨胀顶升气缸初始状态下;

                //    break;
                //case AlignmentMode_WorkStep.调整位膨胀顶升气缸初始状态下:
                //    if (HardWareControl.getValve(EnumParam_Valve.调整位膨胀气缸).isIDLE())
                //    {
                //        LogAuto.Notify("调整位膨胀顶升气缸下！", (int)MachineStation.主监控, LogLevel.Info);
                //        HardWareControl.getValve(EnumParam_Valve.调整位膨胀顶升气缸).Close();
                //        m_nStep = (int)AlignmentMode_WorkStep.调整位定位销固定顶升气缸初始状态下1;
                //    }
                //    break;
                //case AlignmentMode_WorkStep.调整位定位销固定顶升气缸初始状态下1:
                //    if (HardWareControl.getValve(EnumParam_Valve.调整位膨胀顶升气缸).isIDLE())
                //    {
                //        LogAuto.Notify("调整位定位销固定顶升气缸下", (int)MachineStation.主监控, LogLevel.Info);
                //        HardWareControl.getValve(EnumParam_Valve.调整位定位销固定顶升气缸上).Close();
                //        m_nStep = (int)AlignmentMode_WorkStep.调整位压板限位气缸初始状态缩回;
                //    }
                //    break;
                //case AlignmentMode_WorkStep.调整位压板限位气缸初始状态缩回:
                //    if (HardWareControl.getValve(EnumParam_Valve.调整位定位销固定顶升气缸上).isIDLE())
                //    {
                //        LogAuto.Notify("调整位压板限位气缸回", (int)MachineStation.主监控, LogLevel.Info);
                //        HardWareControl.getValve(EnumParam_Valve.调整位压板限位气缸).Close();
                //        m_nStep = (int)AlignmentMode_WorkStep.调整位压板限位气缸缩回到位;
                //    }
                //    break;
                //case AlignmentMode_WorkStep.调整位压板限位气缸缩回到位:
                //    if (HardWareControl.getValve(EnumParam_Valve.调整位压板限位气缸).isIDLE())
                //    {
                //        LogAuto.Notify("调整位压板限位气缸回", (int)MachineStation.主监控, LogLevel.Info);
                //       // HardWareControl.getValve(EnumParam_Valve.调整位压板限位气缸).Close();
                //        m_nStep = (int)AlignmentMode_WorkStep.调整回原;
                //    }
                //    break;
                #endregion

                case AlignmentMode_WorkStep.Start:
                    DataIndex = 0;
                    index = 0;
                    b_Result = true;
                    b_DownloadCheck = false;
                    MachineDataDefine.remoteFirstProduct = false;
                    HardWareControl.getOutputIO(EnumParam_OutputIO.CCD相机触发拍照).SetIO(false);
                    foreach (var item in MachineDataDefine.machineDatas)
                    {
                        if (item.CName == EnumParam_Point.调整原点.ToString())
                        {
                            alignmengPoint = item;
                            break;
                        }
                    }
                    LogAuto.Notify("调整位调整原点！", (int)MachineStation.主监控, LogLevel.Info);
                    HardWareControl.movePoint(EnumParam_Point.调整原点);
                    m_nStep = (int)AlignmentMode_WorkStep.调整位运动到调整原点;
                    break;
                case AlignmentMode_WorkStep.调整位运动到调整原点:
                    if (HardWareControl.getPointIdel(EnumParam_Point.调整原点))
                    {
                        //if(MachineDataDefine.machineState.b_CCDCalibration)
                        //{
                        //    m_nStep = (int)AlignmentMode_WorkStep.横移X运动到拍照位;
                        //    break;
                        //}
                        //HardWareControl.movePoint(EnumParam_Point.横移X轴待待命位);
                        m_nStep = (int)AlignmentMode_WorkStep.横移X轴待待命位;
                    }
                    break;
                case AlignmentMode_WorkStep.横移X轴待待命位:
                    if (/*HardWareControl.getPointIdel(EnumParam_Point.横移X轴待待命位) &&*/ HardWareControl.getMotor(EnumParam_Axis.X2).isIDLE())
                    {
                        m_nStep = (int)AlignmentMode_WorkStep.置位要料信号;
                    }
                    break;
                case AlignmentMode_WorkStep.置位要料信号:
                    LogAuto.Notify("置位要料信号！", (int)MachineStation.主监控, LogLevel.Info);
                    // Alignment.alignAskProduct = true;
                    //if(RunnerIn.b_OutProduct!=true)
                    // {
                    m_nStep = (int)AlignmentMode_WorkStep.等待有料;
                    //} 
                    break;
                case AlignmentMode_WorkStep.等待有料:
                    //if(NG下料)
                    //{
                    //    m_nStep = (int)AlignmentMode_WorkStep.Start;
                    //    break;
                    //}
                    //if ((Alignment.alignAskProduct == false&& Alignment.alignProductCompelet == false) || b_InAndOutModel || b_DataModel)
                    if ((Alignment.调整要料信号 == 调整位要料信号.不要料 && Alignment.alignProductCompelet == false) || b_InAndOutModel || b_DataModel)
                    {
                        Alignment.调整要料信号 = 调整位要料信号.中转;
                        LogAuto.Notify("前龙门放料完成或插拔测试或数据测试模式下横移X轴移动到拍照位！", (int)MachineStation.主监控, LogLevel.Info);
                        Alignment.holderSN = AxisTakeIn.holderSN;
                        Alignment.流道NG抛料 = AxisTakeIn.runnerNG;
                       // Alignment.gross_COF_result =AxisTakeIn.gross_COF_result;
                        if (AxisTakeIn.runnerNG)
                        {
                            AxisTakeIn.runnerNG = false;
                        }
                        //if (AxisTakeIn.gross_COF_result)
                        //{
                        //    AxisTakeIn.gross_COF_result = false;
                        //}
                        if (b_InAndOutModel || b_DataModel)
                        {
                            // num++;
                            Alignment.SN = "TEST" + DateTime.Now.ToString("yyyyMMddHHmmss");
                        }
                        else
                        {
                            Alignment.SN = AxisTakeIn.SN;
                        }
                        //HardWareControl.getValve(EnumParam_Valve.调整位压板限位气缸).Open();
                        HardWareControl.movePoint(EnumParam_Point.横移X轴拍照位);
                        if (!MachineDataDefine.b_UseLAD)
                        {
                            TimeManages.addTimeManage(Alignment.SN);
                            //   TimeManages.getTimeManage(Alignment.SN).productInstartTime.refreshTime(HIVE.HIVEInstance.hivestarttime[0]);
                            TimeManages.getTimeManage(Alignment.SN).holderSN = Alignment.holderSN;
                            TimeManages.getTimeManage(Alignment.SN).alignmentStartTime.refreshTime();
                            starttime = DateTime.Now.AddSeconds(1).ToString("HH:mm:ss.fff");
                        }
                        m_nStep = (int)AlignmentMode_WorkStep.横移X运动到拍照位;
                        //TimeManages.getTimeManage(Alignment.SN).alignmentStartTime.refreshTime2();
                    }
                    break;
                case AlignmentMode_WorkStep.横移X运动到拍照位:
                    //if (MachineDataDefine.machineState.b_CCDCalibration)
                    //{
                    //    timerDelay.Interval = 2000;
                    //    timerDelay.Start();
                    //    m_nStep = (int)AlignmentMode_WorkStep.调整位压板限位气缸出;
                    //    break;
                    //}
                    if (HardWareControl.getPointIdel(EnumParam_Point.横移X轴拍照位))
                    {
                        //if (RunnerIn.b_OutProduct)
                        //{
                        //    m_nStep = (int)AlignmentMode_WorkStep.调整位定位销固定顶升气缸上;
                        //    break;
                        //}
                        //else
                        //{

                        m_nStep = (int)AlignmentMode_WorkStep.调整位压板限位气缸出;
                        //}
                        break;
                    }
                    break;
                case AlignmentMode_WorkStep.调整位压板限位气缸出:

                    LogAuto.Notify("横移X轴移动到拍照位调整位压板限位气缸定位！", (int)MachineStation.主监控, LogLevel.Info);
                     HardWareControl.getValve(EnumParam_Valve.调整位压板限位气缸).Open();
                    m_nStep = (int)AlignmentMode_WorkStep.调整位定位销固定顶升气缸上;
                    break;
                case AlignmentMode_WorkStep.调整位定位销固定顶升气缸上:
                    if (HardWareControl.getValve(EnumParam_Valve.调整位压板限位气缸).isIDLE())
                    {
                        LogAuto.Notify("调整位定位销固定顶升气缸上！", (int)MachineStation.主监控, LogLevel.Info);
                        HardWareControl.getValve(EnumParam_Valve.调整位定位销固定顶升气缸上).Open();
                        m_nStep = (int)AlignmentMode_WorkStep.调整位膨胀顶升气缸上;
                    }
                    break;
                case AlignmentMode_WorkStep.调整位膨胀顶升气缸上:
                    if (HardWareControl.getValve(EnumParam_Valve.调整位定位销固定顶升气缸上).isIDLE())
                    {
                        if (b_DataModel)
                        {
                            LogAuto.Notify("测试模式下横移X轴移动到待命位！", (int)MachineStation.主监控, LogLevel.Info);
                            m_nStep = (int)AlignmentMode_WorkStep.触发相机拍照;
                            HardWareControl.movePoint(EnumParam_Point.横移X轴待待命位);
                            break;
                        }
                        //if (!MachineDataDefine.machineState.b_CCDCalibration)
                        //{
                        LogAuto.Notify("横移X轴移动到待命位！", (int)MachineStation.主监控, LogLevel.Info);
                        HardWareControl.movePoint(EnumParam_Point.横移X轴待待命位);
                        //}
                        if (MachineDataDefine.machineState.b_RecheckModel)
                        {
                            LogAuto.Notify("复投模式！", (int)MachineStation.主监控, LogLevel.Info);
                            dia_AddNewMSG dia_AddNewMSG1 = new dia_AddNewMSG();
                            dia_AddNewMSG1.ShowDialog();
                            Alignment.SN = dia_AddNewMSG1.MSG;
                            Alignment.调整要料信号 = 调整位要料信号.要料;
                            m_nStep = (int)AlignmentMode_WorkStep.触发相机拍照;
                            break;
                        }

                        else if (MachineDataDefine.b_UseLAD)
                        {
                            if (MachineDataDefine.LADModel == 2)
                            {
                                TimeManages.addTimeManage(Alignment.SN);
                                //   TimeManages.getTimeManage(Alignment.SN).productInstartTime.refreshTime(HIVE.HIVEInstance.hivestarttime[0]);
                                TimeManages.getTimeManage(Alignment.SN).holderSN = Alignment.holderSN;
                                TimeManages.getTimeManage(Alignment.SN).alignmentStartTime.refreshTime();
                                starttime = DateTime.Now.AddSeconds(1).ToString("HH:mm:ss.fff");
                                LogAuto.Notify("LAD GRR模式拍照第一次！", (int)MachineStation.主监控, LogLevel.Info);
                                LadNum++;
                                if (LadNum > 1)
                                {
                                    #region
                                    // LogAuto.Notify("LAD GRR模式拍照大于第一次！", (int)MachineStation.主监控, LogLevel.Info);
                                    // if(LadNum==2)
                                    // {
                                    //     TimeManages.addTimeManage(Alignment.SN);
                                    //     //   TimeManages.getTimeManage(Alignment.SN).productInstartTime.refreshTime(HIVE.HIVEInstance.hivestarttime[0]);
                                    //     TimeManages.getTimeManage(Alignment.SN).holderSN = Alignment.holderSN;
                                    //     TimeManages.getTimeManage(Alignment.SN).alignmentStartTime.refreshTime();
                                    //     starttime = DateTime.Now.AddSeconds(1).ToString("HH:mm:ss.fff");
                                    // }
                                    //else if (LadNum == 3)
                                    // {
                                    //     TimeManages.addTimeManage(Alignment.SN);
                                    //     //   TimeManages.getTimeManage(Alignment.SN).productInstartTime.refreshTime(HIVE.HIVEInstance.hivestarttime[0]);
                                    //     TimeManages.getTimeManage(Alignment.SN).holderSN = Alignment.holderSN;
                                    //     TimeManages.getTimeManage(Alignment.SN).alignmentStartTime.refreshTime();
                                    //     starttime = DateTime.Now.AddSeconds(1).ToString("HH:mm:ss.fff");
                                    // }
                                    #endregion
                                    m_nStep = (int)AlignmentMode_WorkStep.触发相机拍照;
                                }
                                else
                                {
                                    //TimeManages.addTimeManage(Alignment.SN);
                                    ////   TimeManages.getTimeManage(Alignment.SN).productInstartTime.refreshTime(HIVE.HIVEInstance.hivestarttime[0]);
                                    //TimeManages.getTimeManage(Alignment.SN).holderSN = Alignment.holderSN;
                                    //TimeManages.getTimeManage(Alignment.SN).alignmentStartTime.refreshTime();
                                    //starttime = DateTime.Now.AddSeconds(1).ToString("HH:mm:ss.fff");
                                    //LogAuto.Notify("LAD GRR模式拍照第一次！", (int)MachineStation.主监控, LogLevel.Info);
                                    Alignment.调整要料信号 = 调整位要料信号.要料;
                                    m_nStep = (int)AlignmentMode_WorkStep.触发相机拍照;
                                }
                                break;
                            }
                        }
                        else
                        {
                            //string model = "Adjustment";
                            //if (index == 0)
                            //{

                            //}
                            if (!Rework)
                            {
                                Alignment.调整要料信号 = 调整位要料信号.要料;
                            }

                            if (流道NG抛料)
                            {
                                // 流道NG抛料 = false;
                               m_nStep = (int)AlignmentMode_WorkStep.调整位定位销固定顶升气缸下1;
                                ///先判断连三不连五
                                //m_nStep = (int)AlignmentMode_WorkStep.ThreeAndFive;
                                break;
                            }
                            LogAuto.Notify("调整位膨胀顶升气缸上！", (int)MachineStation.主监控, LogLevel.Info);
                            HardWareControl.getValve(EnumParam_Valve.调整位膨胀顶升气缸).Open();
                            m_nStep = (int)AlignmentMode_WorkStep.调整位膨胀气缸膨胀;
                            break;
                        }
                    }
                    break;
                case AlignmentMode_WorkStep.调整位膨胀气缸膨胀:
                    if (HardWareControl.getValve(EnumParam_Valve.调整位膨胀顶升气缸).isIDLE())
                    {
                        //Alignment.alignAskProduct = true;

                        //  Alignment.调整要料信号 = 调整位要料信号.要料;
                        //IO触发提前
                        if (MachineDataDefine.machineState.b_UseCCD)
                        {
                            LogAuto.Notify("使用CCD！", (int)MachineStation.主监控, LogLevel.Info);
                            ConnectionControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr = "";
                            string SN1 = "";
                            if (MachineDataDefine.machineState.b_UseTestRun || MachineDataDefine.machineState.b_UseNullRun)
                            {
                                //SN1 = "1234567890";
                                //Alignment.SN = SN1;
                                LogAuto.Notify("调机或空跑模式SN赋值固定值！", (int)MachineStation.主监控, LogLevel.Info);
                                if (string.IsNullOrWhiteSpace(Alignment.SN))
                                {
                                    SN1 = Alignment.SN;//"TEST" + DateTime.Now.ToString("HHmmss");
                                    Alignment.SN = SN1;
                                }
                                SN1 = Alignment.SN;
                            }
                            else
                            {
                                //  HardWareControl.getValve(EnumParam_Valve.调整位膨胀气缸).Open();
                                LogAuto.Notify("其他模式SN赋值getSN！", (int)MachineStation.主监控, LogLevel.Info);
                                SN1 = Alignment.SN;
                            }
                            string model1 = "Adjustment";
                            if (MachineDataDefine.machineState.b_UseLYG)
                            {
                                if (Rework)
                                {
                                    model1 = "ReAdjustment";
                                    LogAuto.Notify("ReAdjustment模式触发拍照！", (int)MachineStation.主监控, LogLevel.Info);
                                }
                            }
                            LogAuto.Notify("Adjustment模式触发拍照！", (int)MachineStation.主监控, LogLevel.Info);
                            ConnectionControl.getSocketControl(EnumParam_ConnectionName.CCD).SendMSG("@,start," + model1 + "," + SN1 + "," + index.ToString() + ",#");
                        }
                        if (!MachineDataDefine.machineState.b_UseNullRun)
                        {
                            HardWareControl.getValve(EnumParam_Valve.调整位膨胀气缸).Open();
                        }

                        //  m_nStep = (int)AlignmentMode_WorkStep.调整位定位销固定顶升气缸下;
                        m_nStep = (int)AlignmentMode_WorkStep.调整位定位销固定顶升气缸下;
                    }
                    break;
                case AlignmentMode_WorkStep.调整位定位销固定顶升气缸下:
                    if (HardWareControl.getValve(EnumParam_Valve.调整位膨胀气缸).isIDLE() || MachineDataDefine.machineState.b_UseNullRun)
                    {
                        // cylinderCount++;
                        MachineDataDefine.MachineCfgS.CurrentCylinderCount++;
                        if (MachineDataDefine.MachineCfgS.CurrentCylinderCount == MachineDataDefine.MachineCfgS.CylinderCount)
                        {
                            MachineDataDefine.MachineCfgS.CurrentCylinderCount = 0;
                            膨胀气缸报警 = true;
                        }
                        if (!Rework)
                        {
                            TimeManages.getTimeManage(Alignment.SN).alignmentStartTime.retime();
                        }
                        // StaticParam.cylinderDictionary[EnumParam_Valve.调整位定位销固定顶升气缸上.ToString()].Close();
                        if (MachineDataDefine.machineState.b_ForwardModel)
                        {
                            LogAuto.Notify("进给模式！", (int)MachineStation.主监控, LogLevel.Info);
                            axisCalibration.Action();
                            m_nStep = (int)AlignmentMode_WorkStep.调整位定位销固定顶升气缸上1;
                            break;
                        }
                        m_nStep = (int)AlignmentMode_WorkStep.触发相机拍照;
                    }
                    break;
                //m_nStep = (int)AlignmentMode_WorkStep.触发相机拍照;
                // break;
                case AlignmentMode_WorkStep.触发相机拍照:
                    #region 空跑
                    if (MachineDataDefine.machineState.b_UseNullRun)
                    {
                        LogAuto.Notify("空跑模式！", (int)MachineStation.主监控, LogLevel.Info);
                        if (index >= nullModelStep.Count)
                        {
                            LogAuto.Notify("空跑模式调整次数！" + index, (int)MachineStation.主监控, LogLevel.Info);
                            m_nStep = (int)AlignmentMode_WorkStep.调整位定位销固定顶升气缸上1;
                            break;
                        }
                        else
                        {
                            m_nStep = (int)AlignmentMode_WorkStep.运动引导点位;
                            break;
                        }
                    }
                    #endregion
                    #region 不使用CCD
                    if (MachineDataDefine.machineState.b_UseCCD != true)
                    {
                        if (string.IsNullOrWhiteSpace(Alignment.SN))
                        {
                            //Alignment.SN = "TEST" + DateTime.Now.ToString("HHmmss");                         
                        }
                        LogAuto.Notify("不启用CCD！", (int)MachineStation.主监控, LogLevel.Info);
                        m_nStep = (int)AlignmentMode_WorkStep.调整位定位销固定顶升气缸上1;
                        break;
                    }
                    #endregion 
                    //timerDelay.Enabled = false;
                    //timerDelay.Interval = 10000;
                    //timerDelay.Start();

                    ConnectionControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr = "";
                    if (b_DataModel)
                    {
                        ConnectionControl.getSocketControl(EnumParam_ConnectionName.CCD).SendMSG("@,start," + "Adjustment" + "," + SN + "," + index.ToString() + ",#");
                        timerDelay.Enabled = false;
                        timerDelay.Interval = 50;
                        timerDelay.Start();
                    }
                    if (b_DownloadCheck)
                    {
                        LogAuto.Notify("复检！", (int)MachineStation.主监控, LogLevel.Info);
                        string SN = "";
                        if (MachineDataDefine.machineState.b_UseTestRun)
                        {
                            LogAuto.Notify("调机模式下复检！", (int)MachineStation.主监控, LogLevel.Info);
                            if (MachineDataDefine.machineState.b_UseMes)
                            {
                                SN = Alignment.SN;
                            }
                            else
                            {
                                SN = Alignment.SN;// "TEST" + DateTime.Now.ToString("HHmmss");
                            }

                        }
                        else
                        {
                            LogAuto.Notify("正常生产模式下复检！", (int)MachineStation.主监控, LogLevel.Info);
                            SN = Alignment.SN;
                        }
                        LogAuto.Notify("发送复检拍照指令！", (int)MachineStation.主监控, LogLevel.Info);
                        string model = "Check";
                        if (MachineDataDefine.machineState.b_UseLYG)
                        {
                            if (Rework)
                            {
                                model = "Check1";
                                LogAuto.Notify("Check1模式触发拍照！", (int)MachineStation.主监控, LogLevel.Info);
                            }
                        }
                        ConnectionControl.getSocketControl(EnumParam_ConnectionName.CCD).SendMSG("@,start," + model + "," + SN + "," + index.ToString() + ",#");
                        timerDelay.Enabled = false;
                        timerDelay.Interval = 50;
                        timerDelay.Start();
                        //  m_nStep = (int)AlignmentMode_WorkStep.调整位定位销固定顶升气缸上1;
                    }
                    if (MachineDataDefine.machineState.b_RecheckModel)//复投
                    {
                        LogAuto.Notify("发送复投拍照指令！", (int)MachineStation.主监控, LogLevel.Info);
                        string model = "Recheck";
                        ConnectionControl.getSocketControl(EnumParam_ConnectionName.CCD).SendMSG("@,start," + model + "," + Alignment.SN + "," + index.ToString() + ",#");
                        timerDelay.Enabled = false;
                        timerDelay.Interval = 50;
                        timerDelay.Start();
                        //  m_nStep = (int)AlignmentMode_WorkStep.调整位定位销固定顶升气缸上1;
                    }
                    if (MachineDataDefine.b_UseLAD)
                    {
                        if (MachineDataDefine.LADModel == 2)
                        {
                            //TimeManages.getTimeManage(Alignment.SN).alignmentStartTime.refreshTime();
                            LogAuto.Notify("发送LAD拍照指令！", (int)MachineStation.主监控, LogLevel.Info);
                            string model = "Recheck";
                            ConnectionControl.getSocketControl(EnumParam_ConnectionName.CCD).SendMSG("@,start," + model + "," + Alignment.SN + "," + index.ToString() + ",#");
                            timerDelay.Enabled = false;
                            timerDelay.Interval = 50;
                            timerDelay.Start();
                        }
                    }
                    if (Photoretry)
                    {

                        timerDelay.Enabled = false;
                        timerDelay.Interval = 50;
                        timerDelay.Start();
                    }

                    m_nStep = (int)AlignmentMode_WorkStep.置位相机触发IOTrue;
                    break;
                case AlignmentMode_WorkStep.置位相机触发IOTrue:
                    if (b_DownloadCheck)
                    {
                        LogAuto.Notify("复检模式触发拍照不适用io！", (int)MachineStation.主监控, LogLevel.Info);
                        if (timerDelay.Enabled)
                        {
                            break;
                        }
                    }
                    if (Photoretry)
                    {
                        if (timerDelay.Enabled)
                        {
                            Photoretry = false;
                            break;
                        }
                    }

                    LogAuto.Notify("io触发拍照！", (int)MachineStation.主监控, LogLevel.Info);
                    HardWareControl.getOutputIO(EnumParam_OutputIO.CCD相机触发拍照).SetIO(true);
                    timerDelay.Enabled = false;
                    timerDelay.Interval = 50;
                    timerDelay.Start();
                    m_nStep = (int)AlignmentMode_WorkStep.等待延时;
                    break;
                case AlignmentMode_WorkStep.等待延时:
                    if (timerDelay.Enabled == false)
                    {
                        m_nStep = (int)AlignmentMode_WorkStep.置位相机触发IOFalse;
                    }
                    break;
                case AlignmentMode_WorkStep.置位相机触发IOFalse:
                    LogAuto.Notify("关闭io触发拍照指令！", (int)MachineStation.主监控, LogLevel.Info);
                    HardWareControl.getOutputIO(EnumParam_OutputIO.CCD相机触发拍照).SetIO(false);
                    timerDelay.Enabled = false;
                    timerDelay.Interval = 10000;
                    timerDelay.Start();
                    m_nStep = (int)AlignmentMode_WorkStep.等待相机反馈;
                    break;
                case AlignmentMode_WorkStep.等待相机反馈:
                    if (ConnectionControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr != "")
                    {
                        LogAuto.Notify("相机返回结果！", (int)MachineStation.主监控, LogLevel.Info);
                        string[] result = ConnectionControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr.Split(',');
                        //string s=ConnectionControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr;
                        //byte[] bytes=Encoding.Default.GetBytes(s);
                        //string utf = Encoding.UTF8.GetString(bytes);
                        //string[] result = utf.Split(',');
                        //   if (result[2].ToLower() == "ng")//ng不上传数据
                        if (result[2].ToLower().Contains("ng"))
                        {// 1;1;1;1;1
                         // result[3].ToString("uft-8");  
                            MachineDataDefine.PhotoMSG = result[3];
                            PhotoNGmsg = result[3].Split(';');
                            //byte[] bytes = Encoding.GetEncoding("GBK").GetBytes(PhotoNGmsg[1]);
                            //MachineDataDefine.PhotoNGMSG = Encoding.GetEncoding("UTF-8").GetString(bytes);
                            LogAuto.Notify("拍照NG！", (int)MachineStation.主监控, LogLevel.Info);
                            b_Result = false;
                            下料 = true;
                            NG拍照下料显示 = true;
                            b_DownloadCheck = false;
                            string str = Alignment.SN + "," + Alignment.holderSN + "," + "拍照NG！" + ":" + MachineDataDefine.PhotoMSG;
                            LogAuto.SaveNGData(str);
                            if (PhotoNGmsg[0] == "G3D_Run_Error" || PhotoNGmsg[0] == "Proucts_NG")
                            {
                                NGCount++;
                                LogAuto.Save3DNGData(str);
                            }

                            m_nStep = (int)AlignmentMode_WorkStep.调整位定位销固定顶升气缸上1;
                            break;
                        }
                        // else if (result[2].ToLower().Contains("Retry"))
                        else if (result[2].Contains("Retry"))
                        {
                            Photoretry = true;
                            LogAuto.Notify("CCD未返回数据重新拍照获取！", (int)MachineStation.主监控, LogLevel.Info);
                            // string model1 = "Adjustment";
                            LogAuto.Notify("Adjustment模式触发拍照！", (int)MachineStation.主监控, LogLevel.Info);
                            ConnectionControl.getSocketControl(EnumParam_ConnectionName.CCD).SendMSG("@,start," + "Adjustment" + "," + Alignment.SN + "," + index.ToString() + ",#");
                            m_nStep = (int)AlignmentMode_WorkStep.触发相机拍照;
                        }


                        else if (result[2].Contains("ReAdjust"))  //复检后是否重拍
                        {
                            Rework = true;
                            sw.Restart();
                            LogAuto.Notify("开始ReAdjust", (int)MachineStation.主监控, LogLevel.Info);
                            Resetcount++;
                            if (Resetcount > 3)
                            {
                                Resetcount = 0;
                            }
                            b_DownloadCheck = false;
                            if (MachineDataDefine.machineState.b_UseLMI)
                            {
                                index = 1;
                            }
                            else if (MachineDataDefine.machineState.b_UseLYG)
                            {
                                index = 0;
                            }
                            LogAuto.Notify("CCD未返回ReAdjust数据重新插拔拍照！", (int)MachineStation.主监控, LogLevel.Info);
                            m_nStep = (int)AlignmentMode_WorkStep.调整位膨胀顶升气缸上;
                        }
                        else
                        {
                            if (result[2].Contains("Station_Warn"))
                            {
                                //  下料 = true;
                                调整超时下料显示 = true;
                            }
                            LogAuto.Notify("将相机结果添加到键值对！", (int)MachineStation.主监控, LogLevel.Info);
                            datas.Clear();
                            datas.Add("OutX", result[4]);
                            datas.Add("OutY", result[5]);
                            datas.Add("OutR", result[6]);
                            datas.Add("OffsetX", result[7]);
                            datas.Add("OffsetY", result[8]);
                            datas.Add("OffsetR", result[9]);

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
                            datas.Add("最优解", result[22]);
                            if (MachineDataDefine.machineState.b_RecheckModel || MachineDataDefine.b_UseLAD || b_DataModel)//静止后复拍，不调整
                            {
                                LogAuto.Notify("复投或者GRR或者数据测试模式保存数据！", (int)MachineStation.主监控, LogLevel.Info);
                                DataIndex++;
                                string dataStr = ConnectionControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr;
                                LogAuto.Notify(dataStr, (int)MachineStation.主监控, LogLevel.Info);
                                // string alignTime = (DateTime.Now - TimeManages.getTimeManage(Alignment.SN).alignmentStartTime.time22).TotalSeconds.ToString();
                                string str = Alignment.SN + "," + Alignment.holderSN + "," + TimeManages.getTimeManage(Alignment.SN).alignmentStartTime.time22 + "," + DateTime.Now.ToString() + "," + index.ToString() + "," + dataStr;
                                LogAuto.SaveCCDData(str, CCDDataType.复检数据);
                                if (b_DataModel)
                                {
                                    if (DataIndex >= 9)
                                    {
                                        //b_DataModel = false;
                                        m_nStep = (int)AlignmentMode_WorkStep.调整位定位销固定顶升气缸下1;
                                        break;
                                    }
                                    else
                                    {
                                        m_nStep = (int)AlignmentMode_WorkStep.触发相机拍照;
                                        break;
                                    }
                                }
                                else
                                {
                                    timerDelay.Enabled = false;
                                    timerDelay.Interval = 2000;
                                    timerDelay.Start();
                                    m_nStep = (int)AlignmentMode_WorkStep.LAD等待延时1;
                                    break;
                                    //  m_nStep = (int)AlignmentMode_WorkStep.调整位定位销固定顶升气缸下1;
                                }
                                break;
                            }
                            //判断是否是复检
                            if (b_DownloadCheck)//调整后复拍
                            {

                                LogAuto.Notify("复检模式保存数据！", (int)MachineStation.主监控, LogLevel.Info);
                                string dataStr = ConnectionControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr;
                                LogAuto.Notify(dataStr, (int)MachineStation.主监控, LogLevel.Info);
                                string alignTime = (DateTime.Now - TimeManages.getTimeManage(Alignment.SN).alignmentStartTime.time22).TotalSeconds.ToString();
                                strCheck = dataStr;
                                // LogAuto.SaveCCDData(alignData1 + strCheck, CCDDataType.Alignment数据);
                                if (b_InAndOutModel)
                                {
                                    b_DownloadCheck = false;
                                    LogAuto.SaveCCDData(alignData2 + strCheck, CCDDataType.插拔数据);
                                    m_nStep = (int)AlignmentMode_WorkStep.调整位定位销固定顶升气缸上1;
                                    break;
                                }

                                b_DownloadCheck = false;
                                timerDelay.Enabled = false;
                                timerDelay.Interval = 100;
                                timerDelay.Start();
                                if (result[2].Contains("Station_Stop"))
                                {
                                    MachineDataDefine.StopNG = true;
                                    Error pError = new Error(ref this.m_NowAddress, "连续三次调整超时！请清料停机检查！！！", "", (int)MErrorCode.CCD_Capture1異常);
                                    /// pError.AddErrSloution("Retry", (int)AlignmentMode_WorkStep.相机重连);
                                    pError.AddErrSloution("连续三次调整超时", (int)AlignmentMode_WorkStep.调整位定位销固定顶升气缸下1);
                                    pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                                }
                                else
                                {
                                    m_nStep = (int)AlignmentMode_WorkStep.调整位定位销固定顶升气缸下1;
                                }
                                break;
                            }

                            else
                            {
                                // LogAuto.Notify("正常模式保存数据！", (int)MachineStation.主监控, LogLevel.Info);
                                if (result[3] == "1")//1 允许下料,0 不允许下料
                                {
                                    LogAuto.Notify("相机反馈下料结果！", (int)MachineStation.主监控, LogLevel.Info);
                                    dataStrOK = ConnectionControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr;
                                    LogAuto.Notify(dataStrOK, (int)MachineStation.主监控, LogLevel.Info);
                                    alignTimeOK = (DateTime.Now - TimeManages.getTimeManage(Alignment.SN).alignmentStartTime.time22).TotalSeconds.ToString();
                                    LogAuto.Notify("第一次时间！" + alignTimeOK, (int)MachineStation.主监控, LogLevel.Info);

                                    string str = Alignment.SN + "," + Alignment.holderSN + "," + TimeManages.getTimeManage(Alignment.SN).alignmentStartTime.time22 + "," + DateTime.Now.ToString() + "," + index.ToString() + "," + alignTimeOK + "," + dataStrOK;
                                    alignData2 = str + ",";
                                    LogAuto.Notify("将相机拍照最后一次结果添加到键值对！", (int)MachineStation.主监控, LogLevel.Info);
                                    datas1.Clear();
                                    datas1.Add("OutX", result[4]);
                                    datas1.Add("OutY", result[5]);
                                    datas1.Add("OutR", result[6]);
                                    datas1.Add("OffsetX", result[7]);
                                    datas1.Add("OffsetY", result[8]);
                                    datas1.Add("OffsetR", result[9]);

                                    datas1.Add("P1", result[10]);
                                    datas1.Add("P2", result[11]);
                                    datas1.Add("P3", result[12]);
                                    datas1.Add("P4", result[13]);
                                    datas1.Add("P5", result[14]);
                                    datas1.Add("P6", result[15]);
                                    datas1.Add("P7", result[16]);
                                    datas1.Add("P8", result[17]);

                                    datas1.Add("ShiftP1P5", result[18]);
                                    datas1.Add("ShiftP2P6", result[19]);
                                    datas1.Add("ShiftP3P7", result[20]);
                                    datas1.Add("ShiftP4P8", result[21]);
                                    datas1.Add("最优解1", result[22]);
                                    for (int i = 0; i < MachineDataDefine.ShowPram.Length; i++)
                                    {
                                        MachineDataDefine.ShowPram[i] = result[i + 4];
                                    }
                                    MachineDataDefine.ShowVaule = true;
                                    b_DownloadCheck = true;
                                    sw.Stop();
                                    time = (sw.ElapsedMilliseconds / 1000.0).ToString("0.0");
                                    string str2 = Alignment.SN + "," + Alignment.holderSN + "," + Resetcount + "," + time;
                                    LogAuto.Notify("结束readjust", (int)MachineStation.主监控, LogLevel.Info);
                                    LogAuto.SaveCCDReWorkData(str2);
                                    if (Rework)
                                    {
                                        if (Resetcount == 1)
                                        {
                                            strmsg1 = reworknum + "," + time;
                                            strmsg2 = "" + "," + "";
                                            strmsg3 = "" + "," + "";
                                            reworknum = 0;
                                        }
                                        else if (Resetcount == 2)
                                        {
                                            strmsg1 = reworknum + "," + time;
                                            strmsg2 = reworknum2 + "," + time;
                                            // strmsg1 = "" + "," + "";
                                            // strmsg3 = "" + "," + "";
                                            reworknum2 = 0;
                                        }
                                        else if (Resetcount == 3)
                                        {
                                            strmsg1 = reworknum + "," + time;
                                            strmsg2 = reworknum2 + "," + time;
                                            strmsg3 = reworknum3 + "," + time;
                                            //strmsg1 = "" + "," + "";
                                            //strmsg2 = "" + "," + "";
                                            reworknum3 = 0;
                                        }
                                    }
                                    else
                                    {
                                        #region
                                        //if (Resetcount == 1)
                                        //{
                                        //    strmsg1 = reworknum + "," + time;
                                        //    strmsg2 = "" + "," + "";
                                        //    strmsg3 = "" + "," + "";
                                        //    reworknum = 0;
                                        //}
                                        //else if (Resetcount == 2)
                                        //{
                                        //    strmsg1 = reworknum + "," + time;
                                        //    strmsg2 = reworknum + "," + time;
                                        //    // strmsg1 = "" + "," + "";
                                        //    // strmsg3 = "" + "," + "";
                                        //    reworknum = 0;
                                        //}
                                        //else if (Resetcount == 3)
                                        //{
                                        //    strmsg1 = reworknum + "," + time;
                                        //    strmsg2 = reworknum + "," + time;
                                        //    strmsg3 = reworknum + "," + time;
                                        //    //strmsg1 = "" + "," + "";
                                        //    //strmsg2 = "" + "," + "";
                                        //    reworknum = 0;
                                        //}
                                        //else
                                        //{
                                        //if(Resetcount<1)
                                        //{
                                        #endregion
                                        strmsg1 = "" + "," + "";
                                        strmsg2 = "" + "," + "";
                                        strmsg3 = "" + "," + "";
                                    }

                                    m_nStep = (int)AlignmentMode_WorkStep.调整位定位销固定顶升气缸上1;
                                }
                                else
                                {

                                    m_nStep = (int)AlignmentMode_WorkStep.判断超时时间;
                                    //}

                                }
                            }
                        }
                    }
                    else if (timerDelay.Enabled == false)
                    {
                        //Error pError = new Error(ref this.m_NowAddress, "CCD未返回数据", "", (int)MErrorCode.CCD_Capture1異常);
                        //pError.AddErrSloution("Retry", (int)AlignmentMode_WorkStep.相机重连);
                        //pError.AddErrSloution("相机连接失败下料", (int)AlignmentMode_WorkStep.相机NG抛料);
                        //pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                        LogAuto.Notify("CCD未返回数据!&&相机连接失败下料", (int)MachineStation.主监控, LogLevel.Info);
                        m_nStep = (int)AlignmentMode_WorkStep.相机NG抛料;
                    }
                    break;

                case AlignmentMode_WorkStep.判断超时时间:
                    //时间计算异常导致一直超时，暂时屏蔽超时逻辑 Modify by 范
                    double totalTime = (DateTime.Now - TimeManages.getTimeManage(Alignment.SN).alignmentStartTime.time22).TotalSeconds;
                    if (MachineDataDefine.MachineCfgS.outTime < totalTime)//超时，则下料
                    {
                        LogAuto.Notify("拍照超时！", (int)MachineStation.主监控, LogLevel.Info);
                        b_DownloadCheck = true;
                        下料 = true;
                        调整超时下料显示 = true;
                        m_nStep = (int)AlignmentMode_WorkStep.调整位定位销固定顶升气缸上1;
                    }
                    else
                    {
                        if (index == 0 && MachineDataDefine.machineState.b_UseMes)
                        {
                            LogAuto.Notify("二次检查UOP OK！", (int)MachineStation.主监控, LogLevel.Info);
                            m_nStep = (int)AlignmentMode_WorkStep.CheckUop;
                            break;
                        }
                        else
                        {
                            m_nStep = (int)AlignmentMode_WorkStep.运动引导点位;
                        }
                    }
                    break;
                case AlignmentMode_WorkStep.CheckUop:
                    LogAuto.Notify("开始二次检查UOP OK！", (int)MachineStation.主监控, LogLevel.Info);
                    Dictionary<string, string> dir1 = new Dictionary<string, string>();
                    dir1.Add("工站", MESDataDefine.MESLXData.terminalName);
                    dir1.Add("产品SN", Alignment.SN);
                    POSTClass.AddCMD(0, CMDStep.检查UOP, dir1);
                    timerDelay.Enabled = false;
                    timerDelay.Interval = 4000;
                    timerDelay.Start();
                    m_nStep = (int)AlignmentMode_WorkStep.产品在当站;
                    break;
                case AlignmentMode_WorkStep.产品在当站:
                    if (POSTClass.getResult(0, CMDStep.检查UOP).Result == "OK")
                    {
                        LogAuto.Notify("二次检查UOP OK！", (int)MachineStation.主监控, LogLevel.Info);
                        m_nStep = (int)AlignmentMode_WorkStep.运动引导点位;
                    }
                    else if (timerDelay.Enabled == false)
                    {
                        LogAuto.Notify("开始二次检查UOP 超时！", (int)MachineStation.主监控, LogLevel.Info);
                        string str = Alignment.SN + "," + Alignment.holderSN + "," + "开始二次检查UOP 超时！";
                        LogAuto.SaveNGData(str);
                        Alignment.Mes反馈超时 = true;
                        //Error pError = new Error(ref this.m_NowAddress, "二次检查UOP OK！反馈超时", "", (int)MErrorCode.UOP检查异常);
                        //pError.AddErrSloution("Retry", (int)AlignmentMode_WorkStep.CheckUop);
                        //pError.AddErrSloution("二次上传Check Uop失败下料", (int)AlignmentMode_WorkStep.NG抛料);
                        //pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                        m_nStep = (int)AlignmentMode_WorkStep.NG抛料;
                    }
                    else if (POSTClass.getResult(0, CMDStep.检查UOP).Result == "NG")
                    {
                        LogAuto.Notify("开始二次检查UOP NG！", (int)MachineStation.主监控, LogLevel.Info);
                        string str = Alignment.SN + "," + Alignment.holderSN + "," + MachineDataDefine.str;
                        LogAuto.SaveNGData(str);
                        RunnerIn.检查UOP失败 = true;
                        //Error pError = new Error(ref this.m_NowAddress, MachineDataDefine.str, "", (int)MErrorCode.UOP检查异常);
                        //pError.AddErrSloution("Retry", (int)AlignmentMode_WorkStep.CheckUop);
                        //pError.AddErrSloution("二次上传Check Uop失败下料", (int)AlignmentMode_WorkStep.NG抛料);
                        //pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                        m_nStep = (int)AlignmentMode_WorkStep.NG抛料;
                    }
                    break;
                case AlignmentMode_WorkStep.运动引导点位:
                    if (MachineDataDefine.machineState.b_UseNullRun)
                    {
                        LogAuto.Notify("空跑模式走固定次数和点位！", (int)MachineStation.主监控, LogLevel.Info);
                        double stepX = nullModelStep[index][0];
                        double stepY = nullModelStep[index][1];
                        double stepR = nullModelStep[index][2];
                        HardWareControl.getMotor(EnumParam_Axis.X2).RevMove(stepX, speed);
                        HardWareControl.getMotor(EnumParam_Axis.Y3).RevMove(stepY, speed);
                        HardWareControl.getMotor(EnumParam_Axis.R).RevMove(stepR, speed);
                        m_nStep = (int)AlignmentMode_WorkStep.等待引导点位运动完成;
                        break;
                    }
                    //如果当前点位，超过了设定的运动范围,则报警下料
                    double difX = Math.Abs(HardWareControl.getMotor(EnumParam_Axis.X2).GetPosition() - alignmengPoint.Data1);
                    double difY = Math.Abs(HardWareControl.getMotor(EnumParam_Axis.Y3).GetPosition() - alignmengPoint.Data2);
                    double difR = Math.Abs(HardWareControl.getMotor(EnumParam_Axis.R).GetPosition() - alignmengPoint.Data4);

                    double offsetX = Convert.ToDouble(datas["OutX"]);
                    double offsetY = Convert.ToDouble(datas["OutY"]);
                    double offsetR = Convert.ToDouble(datas["OutR"]);

                    bool b_True = true;
                    if (difX > MachineDataDefine.MachineCfgS.moveXMax || offsetX > MachineDataDefine.MachineCfgS.moveXMax)
                    {
                        b_True = false;
                        LogAuto.Notify("调整X轴调整范围超限！" + "difX值:" + difX + "offsetX值:" + offsetX + "moveXMax值" + MachineDataDefine.MachineCfgS.moveXMax, (int)MachineStation.主监控, LogLevel.Alarm);
                    }
                    if (difY > MachineDataDefine.MachineCfgS.moveYMax || offsetY > MachineDataDefine.MachineCfgS.moveYMax)
                    {
                        b_True = false;
                        LogAuto.Notify("调整Y轴调整范围超限！" + "difY值:" + difY + "offsetY值:" + offsetY + "moveYMax值" + MachineDataDefine.MachineCfgS.moveYMax, (int)MachineStation.主监控, LogLevel.Alarm);
                    }
                    if (difR > MachineDataDefine.MachineCfgS.moveRMax || offsetR > MachineDataDefine.MachineCfgS.moveRMax)
                    {
                        b_True = false;
                        LogAuto.Notify("调整R轴调整范围超限！" + "difR值:" + difR + "offsetR值:" + offsetR + "moveRMax值" + MachineDataDefine.MachineCfgS.moveRMax, (int)MachineStation.主监控, LogLevel.Alarm);
                    }
                    if (b_True != true)
                    {
                        下料 = true;
                        NG超限下料显示 = true;
                        string str = Alignment.SN + "," + Alignment.holderSN + "," + "调整位置超出设定范围！";
                        LogAuto.SaveNGData(str);
                        //Error pError = new Error(ref this.m_NowAddress, "调整位置超出设定范围！", "", (int)MErrorCode.调整位置超出设定范围);
                        //pError.AddErrSloution("下料", (int)AlignmentMode_WorkStep.调整位定位销固定顶升气缸上1);
                        //pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                        //b_Result = false;
                        m_nStep = (int)AlignmentMode_WorkStep.调整位定位销固定顶升气缸上1;
                        break;
                    }
                    string model11 = "Adjustment";
                    if (MachineDataDefine.machineState.b_UseLYG)
                    {
                        if (Rework)
                        {
                            model11 = "ReAdjustment";
                            LogAuto.Notify("ReAdjustment模式触发拍照！", (int)MachineStation.主监控, LogLevel.Info);
                        }
                    }
                    LogAuto.Notify("Adjustment模式触发拍照！", (int)MachineStation.主监控, LogLevel.Info);
                    ConnectionControl.getSocketControl(EnumParam_ConnectionName.CCD).SendMSG("@,start," + model11 + "," + Alignment.SN + "," + (index + 1).ToString() + ",#");

                    LogAuto.Notify("轴调整第！" + (index + 1) + "次！", (int)MachineStation.主监控, LogLevel.Info);
                    HardWareControl.getMotor(EnumParam_Axis.X2).RevMove(offsetX, speed);
                    HardWareControl.getMotor(EnumParam_Axis.Y3).RevMove(offsetY, speed);
                    HardWareControl.getMotor(EnumParam_Axis.R).RevMove(offsetR, speed);

                    m_nStep = (int)AlignmentMode_WorkStep.等待引导点位运动完成;
                    break;
                case AlignmentMode_WorkStep.等待引导点位运动完成:
                    if (HardWareControl.getMotor(EnumParam_Axis.X2).isIDLE() && HardWareControl.getMotor(EnumParam_Axis.Y3).isIDLE()
                        && HardWareControl.getMotor(EnumParam_Axis.R).isIDLE())
                    {
                        LogAuto.Notify("记录轴调整次数！", (int)MachineStation.主监控, LogLevel.Info);
                        if (index == 0)
                        {
                            LogAuto.Notify("记录index次数！" + index, (int)MachineStation.主监控, LogLevel.Info);
                            TimeManages.getTimeManage(Alignment.SN).alignmentEndTime.refreshTime3();
                        }
                        if (Rework)
                        {
                            if (Resetcount == 1)
                            {

                                reworknum++;
                                LogAuto.Notify("记录rework次数！" + reworknum, (int)MachineStation.主监控, LogLevel.Info);
                            }
                            else if (Resetcount == 2)
                            {
                                reworknum2++;
                                LogAuto.Notify("记录rework次数！" + reworknum2, (int)MachineStation.主监控, LogLevel.Info);
                            }
                            else if (Resetcount == 3)
                            {
                                reworknum3++;
                                LogAuto.Notify("记录rework次数！" + reworknum3, (int)MachineStation.主监控, LogLevel.Info);
                            }

                            //LogAuto.Notify("记录rework次数！" + reworknum, (int)MachineStation.主监控, LogLevel.Info);
                        }

                        index++;//调整次数加1
                        m_nStep = (int)AlignmentMode_WorkStep.触发相机拍照;
                    }
                    break;
                case AlignmentMode_WorkStep.调整位定位销固定顶升气缸上1:
                    // HardWareControl.getValve(EnumParam_Valve.调整位定位销固定顶升气缸上).Open();
                    //timerDelay.Enabled = false;
                    //timerDelay.Interval = 1000;
                    //timerDelay.Start();
                    m_nStep = (int)AlignmentMode_WorkStep.调整位定位销固定顶升气缸上1延时;
                    break;
                case AlignmentMode_WorkStep.调整位定位销固定顶升气缸上1延时:
                    // HardWareControl.getValve(EnumParam_Valve.调整位定位销固定顶升气缸上).Open();
                    //if(timerDelay.Enabled == false)
                    //{
                    m_nStep = (int)AlignmentMode_WorkStep.调整位膨胀气缸回;
                    // }
                    break;
                case AlignmentMode_WorkStep.调整位膨胀气缸回:
                    if (HardWareControl.getValve(EnumParam_Valve.调整位定位销固定顶升气缸上).isIDLE())
                    {
                        LogAuto.Notify("调整位膨胀气缸关闭！", (int)MachineStation.主监控, LogLevel.Info);
                        if (!MachineDataDefine.machineState.b_UseNullRun)
                        {
                            HardWareControl.getValve(EnumParam_Valve.调整位膨胀气缸).Close();
                        }
                        m_nStep = (int)AlignmentMode_WorkStep.调整位膨胀气缸到位;
                    }
                    break;
                case AlignmentMode_WorkStep.调整位膨胀气缸到位:
                    if (HardWareControl.getValve(EnumParam_Valve.调整位膨胀气缸).isIDLE())
                    {
                        LogAuto.Notify("调整位膨胀气缸关闭到位！", (int)MachineStation.主监控, LogLevel.Info);

                        timerDelay.Enabled = false;
                        timerDelay.Interval = 300;
                        timerDelay.Start();
                        m_nStep = (int)AlignmentMode_WorkStep.调整位膨胀顶升气缸下;
                    }
                    break;
                case AlignmentMode_WorkStep.调整位膨胀顶升气缸下:
                    if (timerDelay.Enabled == false || MachineDataDefine.machineState.b_UseNullRun)
                    {
                        LogAuto.Notify("调整位膨胀顶升气缸下！", (int)MachineStation.主监控, LogLevel.Info);
                        HardWareControl.getValve(EnumParam_Valve.调整位膨胀顶升气缸).Close();
                        m_nStep = (int)AlignmentMode_WorkStep.调整位膨胀顶升气缸到位;
                    }
                    break;

                case AlignmentMode_WorkStep.调整位膨胀顶升气缸到位:
                    if (HardWareControl.getValve(EnumParam_Valve.调整位膨胀顶升气缸).isIDLE())
                    {
                        LogAuto.Notify("调整位膨胀顶升气缸下到位！", (int)MachineStation.主监控, LogLevel.Info);                
                        if (b_DownloadCheck)
                        {
                            LogAuto.Notify("复检模式下再次触发拍照！", (int)MachineStation.主监控, LogLevel.Info);
                            m_nStep = (int)AlignmentMode_WorkStep.触发相机拍照;
                            break;
                        }
                        else
                        {
                            LogAuto.Notify("不复检直接下料", (int)MachineStation.主监控, LogLevel.Info);
                            //if(下料||NG下料)
                            //{
                            //    m_nStep = (int)AlignmentMode_WorkStep.连三不连五变量;
                            //    break;
                            //}
                            //else
                            //{
                                m_nStep = (int)AlignmentMode_WorkStep.调整位定位销固定顶升气缸下1;
                                //break;
                       //     }                                                     
                        }
                    }
                    break;

                case AlignmentMode_WorkStep.调整位定位销固定顶升气缸下1:
                    if (HardWareControl.getValve(EnumParam_Valve.调整位膨胀顶升气缸).isIDLE() && HardWareControl.getPointIdel(EnumParam_Point.横移X轴待待命位))
                    {
                        LogAuto.Notify("调整位定位销固定顶升气缸下", (int)MachineStation.主监控, LogLevel.Info);
                        HardWareControl.getValve(EnumParam_Valve.调整位定位销固定顶升气缸上).Close();

                        if (MachineDataDefine.b_UseLAD)
                        {
                            if (MachineDataDefine.LADModel == 2)
                            {
                                if (LadNum < 3 && !下料)                               
                                {
                                    LogAuto.Notify("LAD模式下GRR作料"+ LadNum, (int)MachineStation.主监控, LogLevel.Info);
                                    m_nStep = (int)AlignmentMode_WorkStep.做料完成;
                                }
                                else
                                {
                                    LogAuto.Notify("LAD模式下GRR第三次拍照结束" + LadNum, (int)MachineStation.主监控, LogLevel.Info);
                                    m_nStep = (int)AlignmentMode_WorkStep.调整位压板限位气缸缩回;
                                }
                                TimeManages.getTimeManage(Alignment.SN).alignmentEndTime.refreshTime3();
                                break;
                            }
                        }
                        else
                        {
                            if (MachineDataDefine.machineState.b_UseCCD != true)
                            {
                                TimeManages.getTimeManage(Alignment.SN).alignmentEndTime.refreshTime3();
                            }
                            m_nStep = (int)AlignmentMode_WorkStep.调整位压板限位气缸缩回;
                            break;
                        }
                      //  m_nStep = (int)AlignmentMode_WorkStep.调整位压板限位气缸缩回;
                    }
                    break;
                case AlignmentMode_WorkStep.调整位压板限位气缸缩回:
                    if (HardWareControl.getValve(EnumParam_Valve.调整位定位销固定顶升气缸上).isIDLE())
                    {
                        LogAuto.Notify("调整位压板限位气缸回", (int)MachineStation.主监控, LogLevel.Info);
                        HardWareControl.getValve(EnumParam_Valve.调整位压板限位气缸).Close();
                        m_nStep = (int)AlignmentMode_WorkStep.等待调整位压板限位气缸缩回;
                    }
                    break;
                case AlignmentMode_WorkStep.等待调整位压板限位气缸缩回:
                    if (HardWareControl.getValve(EnumParam_Valve.调整位压板限位气缸).isIDLE())
                    {

                        if (HardWareControl.getPointIdel(EnumParam_Point.横移X轴待待命位))
                        {
                            if (b_InAndOutModel|| b_DataModel)
                            {
                                b_DataModel = false;
                                m_nStep = (int)AlignmentMode_WorkStep.Completed;
                                break;
                            }
                            //if (MachineDataDefine.machineState.b_UseLMI)
                            //{
                            //    m_nStep = (int)AlignmentMode_WorkStep.做料完成;
                            //}
                            //else if (MachineDataDefine.machineState.b_UseLYG)
                            //{
                            
                                HardWareControl.getValve(EnumParam_Valve.调整位定位销固定顶升气缸上).Open();
                                m_nStep = (int)AlignmentMode_WorkStep.调整位顶升气缸抬;
                            
                            //}

                            break;
                        }
                    }
                    break;
                case AlignmentMode_WorkStep.调整位顶升气缸抬:
                    if (HardWareControl.getValve(EnumParam_Valve.调整位定位销固定顶升气缸上).isIDLE())
                    {
                        m_nStep = (int)AlignmentMode_WorkStep.做料完成;
                    }
                    break;
                case AlignmentMode_WorkStep.做料完成:
                    Rework = false;
                    TimeManages.getTimeManage(Alignment.SN).alignmentEndTime.refreshTime2();
                    endtime = DateTime.Now.AddSeconds(6).ToString("HH:mm:ss.fff");
                    if (!MachineDataDefine.machineState.b_RecheckModel && !下料 && !流道NG抛料 && MachineDataDefine.machineState.b_UseCCD)
                    {
                        string ct1 = (Convert.ToDateTime(TimeManages.getTimeManage(Alignment.SN).alignmentEndTime.timeForStradd) - Convert.ToDateTime(TimeManages.getTimeManage(Alignment.SN).alignmentStartTime.timeForStr)).TotalSeconds.ToString();
                        string str1 = Alignment.SN + "," + Alignment.holderSN + "," + TimeManages.getTimeManage(Alignment.SN).alignmentStartTime.timeForStr + "," + TimeManages.getTimeManage(Alignment.SN).alignmentEndTime.timeForStradd + "," + index.ToString() + "," + alignTimeOK + "," + ct1 +","+ strmsg1 + ","+ strmsg2 + ","+ strmsg3 + "," + dataStrOK;
                        alignData1 = str1 + ",";
                        // LogAuto.SaveCCDData(alignData1 + strCheck + "," + Resetcount + "," + time, CCDDataType.Alignment数据);
                        LogAuto.SaveCCDData(alignData1 + strCheck, CCDDataType.Alignment数据);
                        string str2 = Alignment.SN + "," + Alignment.holderSN + "," + TimeManages.getTimeManage(Alignment.SN).alignmentStartTime.timeForStr + "," + TimeManages.getTimeManage(Alignment.SN).alignmentEndTime.timeForStradd + "," + index.ToString() + "," + alignTimeOK + "," + ct1 + "," + dataStrOK;
                        alignData3 = str2 + ",";
                        LogAuto.SaveCCDData(alignData3, CCDDataType.Plugout数据);
                        strmsg1 = "" + "," + "";
                        strmsg2 = "" + "," + "";
                        strmsg3 = "" + "," + "";
                    }
                    if (MachineDataDefine.b_UseLAD && MachineDataDefine.LADModel == 2)
                    {
                        if (LadNum < 3 && !下料)
                        {
                            LogAuto.Notify("GRR模式下拍照小于3次不要料", (int)MachineStation.主监控, LogLevel.Info);
                        }
                        else
                        {
                            LogAuto.Notify("GRR模式下拍照第3次要料", (int)MachineStation.主监控, LogLevel.Info);
                            Alignment.alignProductCompelet = true;
                        }
                    }
                    else
                    {
                        Alignment.alignProductCompelet = true;
                    }
                   // Alignment.alignProductCompelet = true;
                    HIVE.HIVEInstance.lastStoptime[0] = HIVE.HIVEInstance.hivestoptime[0];
                    if (MachineDataDefine.FirstProduct)
                    {
                        if (MachineDataDefine.machineState.b_UseHive || MachineDataDefine.machineState.b_UseRemoteQualification)
                        {
                            MachineDataDefine.FirstProduct = false;
                        }
                        HIVE.HIVEInstance.lastStoptime[0] = DateTime.Now;
                    }
                    HIVE.HIVEInstance.hivestoptime[0] = DateTime.Now;
                    HIVE.HIVEInstance.HIVEStoptime[0] = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");
                    CTUnit.CT_new = (HIVE.HIVEInstance.hivestoptime[0] - HIVE.HIVEInstance.lastStoptime[0]).TotalSeconds.ToString("f3");
                   if(Convert.ToDouble( CTUnit.CT_new)>3600)
                    {
                        CTUnit.CT_new = "3600";
                    }
                    
                    //string ct = (Convert.ToDateTime(TimeManages.getTimeManage(Alignment.SN).alignmentEndTime.timeForStr) - Convert.ToDateTime(TimeManages.getTimeManage(Alignment.SN).alignmentStartTime.timeForStr)).TotalSeconds.ToString();
                    //frm_Main.formData.CTUnit1.EndDoLeft("Test123", ct);
                    productCount++;
                    // frm_Main.formData.CTUnit1.EndDoLeft("1234567890", 10.ToString());
                    #region  LAD模式禁用
                    //if (MachineDataDefine.b_UseLAD)
                    //{
                    //    LogAuto.Notify("使用LAD模式", (int)MachineStation.主监控, LogLevel.Info);
                    //    if (MachineDataDefine.LADModel == 1)//CPK
                    //    {
                    //        LogAuto.Notify("进入CPK模式", (int)MachineStation.主监控, LogLevel.Info);
                    //        if (productCount == 32)
                    //        {
                    //            LogAuto.Notify("已做完32片料", (int)MachineStation.主监控, LogLevel.Info);
                    //            MachineDataDefine.LADOPID = 1;
                    //            //  dia_ChooseFrm dia = new FormView._4弹窗.dia_ChooseFrm();
                    //            //dia.ShowDialog();
                    //            //if (dia.DialogResult == System.Windows.Forms.DialogResult.OK)
                    //            //{
                    //            //    productCount = 0;
                    //            //}
                    //            //else
                    //            //{
                    //            //    //frm_Main.formData?.ChartTime1.SetRunStatus(Chart.ChartTime.MachineStatus.idle);
                    //            //    MachineDataDefine.b_UseLAD = false;
                    //            //}
                    //            Thread th = new Thread(new ThreadStart(() =>
                    //            {
                    //                dia_ChooseFrm dia = new FormView._4弹窗.dia_ChooseFrm(this);
                    //                Application.Run(dia);

                    //            }));
                    //        }
                    //    }
                    //    else if (MachineDataDefine.LADModel == 2)//GRR
                    //    {
                    //        LogAuto.Notify("进入GRR模式", (int)MachineStation.主监控, LogLevel.Info);
                    //        if (productCount == 30)
                    //        {
                    //            LogAuto.Notify("已做完30片料", (int)MachineStation.主监控, LogLevel.Info);
                    //            //dia_ChooseFrm dia = new FormView._4弹窗.dia_ChooseFrm();
                    //            //dia.ShowDialog();
                    //            //if (dia.DialogResult == System.Windows.Forms.DialogResult.OK)
                    //            //{
                    //            //    productCount = 0;
                    //            //    MachineDataDefine.LADOPID = 1;
                    //            //}
                    //            //else
                    //            //{
                    //            //    MachineDataDefine.LADOPID = 2;
                    //            //}
                    //            Thread th = new Thread(new ThreadStart(() =>
                    //            {
                    //                dia_ChooseFrm dia = new FormView._4弹窗.dia_ChooseFrm(this);
                    //                Application.Run(dia);

                    //            }));
                    //        }
                    //        else if (productCount == 60)
                    //        {
                    //            LogAuto.Notify("已做完60片料", (int)MachineStation.主监控, LogLevel.Info);
                    //            //dia_ChooseFrm dia = new FormView._4弹窗.dia_ChooseFrm();
                    //            //dia.ShowDialog();
                    //            //if (dia.DialogResult == System.Windows.Forms.DialogResult.OK)
                    //            //{
                    //            //    productCount = 30;
                    //            //    MachineDataDefine.LADOPID = 2;
                    //            //}
                    //            //else
                    //            //{
                    //            //    MachineDataDefine.LADOPID = 3;
                    //            //}
                    //            Thread th = new Thread(new ThreadStart(() =>
                    //            {
                    //                dia_ChooseFrm dia = new FormView._4弹窗.dia_ChooseFrm(this);
                    //                Application.Run(dia);

                    //            }));
                    //        }
                    //        else if (productCount == 90)
                    //        {
                    //            LogAuto.Notify("已做完90片料", (int)MachineStation.主监控, LogLevel.Info);
                    //            //dia_ChooseFrm dia = new FormView._4弹窗.dia_ChooseFrm();
                    //            //dia.ShowDialog();
                    //            //if (dia.DialogResult == System.Windows.Forms.DialogResult.OK)
                    //            //{
                    //            //    productCount = 60;
                    //            //    MachineDataDefine.LADOPID = 3;
                    //            //}
                    //            //else
                    //            //{
                    //            //   // frm_Main.formData?.ChartTime1.SetRunStatus(Chart.ChartTime.MachineStatus.idle); frm_Main.formData?.ChartTime1.SetRunStatus(Chart.ChartTime.MachineStatus.idle);
                    //            //    MachineDataDefine.b_UseLAD = false;
                    //            //}
                    //            Thread th = new Thread(new ThreadStart(() =>
                    //            {
                    //                dia_ChooseFrm dia = new FormView._4弹窗.dia_ChooseFrm(this);
                    //                Application.Run(dia);

                    //            }));
                    //        }
                    //    }
                    //    else if (MachineDataDefine.LADModel == 3)//SCS
                    //    {
                    //        if (productCount == 10)
                    //        {
                    //            LogAuto.Notify("已做完10片料", (int)MachineStation.主监控, LogLevel.Info);
                    //            MachineDataDefine.LADOPID = 1;
                    //            //dia_ChooseFrm dia = new FormView._4弹窗.dia_ChooseFrm();
                    //            //dia.ShowDialog();
                    //            //if (dia.DialogResult == System.Windows.Forms.DialogResult.OK)
                    //            //{
                    //            //    productCount = 0;
                    //            //}
                    //            //else
                    //            //{
                    //            //    //frm_Main.formData?.ChartTime1.SetRunStatus(Chart.ChartTime.MachineStatus.idle);
                    //            //    MachineDataDefine.b_UseLAD = false;
                    //            //}
                    //            Thread th = new Thread(new ThreadStart(() =>
                    //            {
                    //                dia_ChooseFrm dia = new FormView._4弹窗.dia_ChooseFrm(this);
                    //                Application.Run(dia);

                    //            }));
                    //        }
                    //    }
                    //}
                    #endregion
                   m_nStep = (int)AlignmentMode_WorkStep.等待做料完成信号置位;                 
                    break;
                case AlignmentMode_WorkStep.等待做料完成信号置位:
                    if (流道NG抛料)
                    {
                        流道NG抛料 = false;
                        // NG下料 = true;
                        //  m_nStep = (int)AlignmentMode_WorkStep.Start;
                        //MachineDataDefine.MachineControlS.gross_COF_result = false;
                        m_nStep = (int)AlignmentMode_WorkStep.连三不连五变量;
                        break;
                    }
                    else if (下料)
                    {
                        下料 = false;
                        if (b_DataModel)
                        {
                            m_nStep = (int)AlignmentMode_WorkStep.Completed;
                            break;
                        }
                        //m_nStep = (int)AlignmentMode_WorkStep.Start;
                     //   MachineDataDefine.MachineControlS.gross_COF_result = false;
                        m_nStep = (int)AlignmentMode_WorkStep.连三不连五变量;
                        break;
                    }
                    else if (NG下料)
                    {
                        NG下料 = false;
                        //m_nStep = (int)AlignmentMode_WorkStep.Start;
                        //MachineDataDefine.MachineControlS.gross_COF_result = false;
                        m_nStep = (int)AlignmentMode_WorkStep.连三不连五变量;
                        break;
                    }
                    else
                    {
                       
                        if (MachineDataDefine.machineState.b_UseHive || MachineDataDefine.machineState.b_UseRemoteQualification)
                        {
                            double ctt = double.Parse((Convert.ToDateTime(TimeManages.getTimeManage(Alignment.SN).alignmentEndTime.timeForJ81add) - Convert.ToDateTime(TimeManages.getTimeManage(Alignment.SN).alignmentStartTime.timeForJ8)).TotalSeconds.ToString("0.00"));
                            if (ctt>3600)
                            {
                                ctt = 3600;
                            }
                            string ct = ctt.ToString("0.000");
                            frm_Main.formData.CTUnit1.EndDoLeft(Alignment.SN, ct, starttime, endtime);
                        }
                        if (MachineDataDefine.machineState.b_UseNullRun || MachineDataDefine.machineState.b_UseTestRun)
                        {
                            MachineDataDefine.MachineControlS.gross_COF_result = true;
                            MachineDataDefine.MachineControlS.continueNG_three.Clear();
                            if (MachineDataDefine.machineState.b_UseHive || MachineDataDefine.machineState.b_UseRemoteQualification)
                            {
                                LogAuto.Notify("空跑或者调机模式下启用HIVE", (int)MachineStation.主监控, LogLevel.Info);
                                m_nStep = (int)AlignmentMode_WorkStep.上传HIVE;
                                break;
                            }
                            else
                            {
                                SNShowEvent(Alignment.SN, true);
                                m_nStep = (int)AlignmentMode_WorkStep.Start;
                                break;
                            }
                        }
                        else
                        {
                            if (MachineDataDefine.machineState.b_UseMes)
                            {
                                LogAuto.Notify("正常模式下启用HIVE", (int)MachineStation.主监控, LogLevel.Info);
                                m_nStep = (int)AlignmentMode_WorkStep.上传数据;
                            }
                            else if (MachineDataDefine.machineState.b_UseHive)
                            {
                                m_nStep = (int)AlignmentMode_WorkStep.上传HIVE;
                            }
                            else if (MachineDataDefine.machineState.b_UsePDCA)
                            {
                                m_nStep = (int)AlignmentMode_WorkStep.上传PDCA;
                            }
                        }
                    }
                    break;
                case AlignmentMode_WorkStep.上传数据:
                    if (MachineDataDefine.machineState.b_UseMes)
                    {
                        LogAuto.Notify("上传mes数据", (int)MachineStation.主监控, LogLevel.Info);
                        Dictionary<string, string> datas11 = new Dictionary<string, string>();
                        datas11.Add("工站", MESDataDefine.MESLXData.terminalName);
                        datas11.Add("产品SN", Alignment.SN);
                        //datas11.Add("载具号", Alignment.holderSN);
                        datas11.Add("结果", "PASS");
                        datas11.Add("开始时间", TimeManages.getTimeManage(Alignment.SN).alignmentStartTime.timeForStr);
                        datas11.Add("结束时间", TimeManages.getTimeManage(Alignment.SN).alignmentEndTime.timeForStraddTime);
                        datas11.Add("软件版本", MESDataDefine.MESLXData.SW_Version);
                        datas11.Add("载具号", Alignment.holderSN);
                        datas11.Add("OffsetX值", datas["OffsetX"]);
                        datas11.Add("OffsetY值", datas["OffsetY"]);
                        datas11.Add("OffsetR值", datas["OffsetR"]);
                        datas11.Add("P1值", datas["P1"]);
                        datas11.Add("P2值", datas["P2"]);
                        datas11.Add("P3值", datas["P3"]);
                        datas11.Add("P4值", datas["P4"]);
                        datas11.Add("P5值", datas["P5"]);
                        datas11.Add("P6值", datas["P6"]);
                        datas11.Add("P7值", datas["P7"]);
                        datas11.Add("P8值", datas["P8"]);
                        datas11.Add("ShiftP15值", datas["ShiftP1P5"]);
                        datas11.Add("ShiftP26值", datas["ShiftP2P6"]);
                        datas11.Add("ShiftP37值", datas["ShiftP3P7"]);
                        datas11.Add("ShiftP48值", datas["ShiftP4P8"]);
                        datas11.Add("最优解值", datas["最优解"]);
                        datas11.Add("OffsetX_1值", datas1["OffsetX"]);
                        datas11.Add("OffsetY_1值", datas1["OffsetY"]);
                        datas11.Add("OffsetR_1值", datas1["OffsetR"]);
                        datas11.Add("P1_1值", datas1["P1"]);
                        datas11.Add("P2_1值", datas1["P2"]);
                        datas11.Add("P3_1值", datas1["P3"]);
                        datas11.Add("P4_1值", datas1["P4"]);
                        datas11.Add("P5_1值", datas1["P5"]);
                        datas11.Add("P6_1值", datas1["P6"]);
                        datas11.Add("P7_1值", datas1["P7"]);
                        datas11.Add("P8_1值", datas1["P8"]);
                        datas11.Add("ShiftP15_1值", datas1["ShiftP1P5"]);
                        datas11.Add("ShiftP26_1值", datas1["ShiftP2P6"]);
                        datas11.Add("ShiftP37_1值", datas1["ShiftP3P7"]);
                        datas11.Add("ShiftP48_1值", datas1["ShiftP4P8"]);
                        datas11.Add("最优3", datas1["最优解1"]);
                        string ct = (Convert.ToDateTime(TimeManages.getTimeManage(Alignment.SN).alignmentEndTime.timeForStradd) - Convert.ToDateTime(TimeManages.getTimeManage(Alignment.SN).alignmentStartTime.timeForStr)).TotalSeconds.ToString();
                        datas11.Add("设备CT", ct);
                        datas11.Add("调整次数", index.ToString());
                        Post.POSTClass.AddCMD(0, Post.CMDStep.上传数据, datas11);
                        m_nStep = (int)AlignmentMode_WorkStep.提交过站;
                        timerDelay.Enabled = false;
                        timerDelay.Interval = 3000;
                        timerDelay.Start();
                    }
                    break;
                case AlignmentMode_WorkStep.提交过站:
                    if (POSTClass.getResult(0, CMDStep.上传数据).Result == "OK")
                    {
                        LogAuto.Notify("上传mes数据OK", (int)MachineStation.主监控, LogLevel.Info);
                        Dictionary<string, string> datas = new Dictionary<string, string>();
                        datas.Add("工站", MESDataDefine.MESLXData.terminalName);
                        datas.Add("产品SN", Alignment.SN);
                        Post.POSTClass.AddCMD(0, Post.CMDStep.提交过站, datas);
                        SNShowEvent(Alignment.SN, true);
                        timerDelay.Enabled = false;
                        timerDelay.Interval = 3000;
                        timerDelay.Start();
                        m_nStep = (int)AlignmentMode_WorkStep.等待提交过站完成;
                    }
                    else if (timerDelay.Enabled == false)
                    {
                        SNShowEvent(Alignment.SN, false);
                        string str = Alignment.SN + "," + Alignment.holderSN + "," + "上传mes数据反馈超时";
                        LogAuto.SaveNGData(str);
                        Alignment.Mes反馈超时 = true;
                        LogAuto.Notify("上传mes数据反馈超时", (int)MachineStation.主监控, LogLevel.Info);
                        m_nStep = (int)AlignmentMode_WorkStep.连三不连五变量;
                        // Error pError = new Error(ref this.m_NowAddress, "上传数据给MES失败！", "", (int)MErrorCode.MES上传参数信息失败);
                        //Error pError = new Error(ref this.m_NowAddress, "上传mes数据反馈超时", "", (int)MErrorCode.MES上传参数信息失败);
                        //pError.AddErrSloution("Retry", (int)AlignmentMode_WorkStep.上传数据);
                        //pError.AddErrSloution("下料", (int)AlignmentMode_WorkStep.上传失败下料);
                        //pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    }
                    else if (POSTClass.getResult(0, CMDStep.上传数据).Result == "NG")
                    {
                        SNShowEvent(Alignment.SN, false);
                        string str = Alignment.SN + "," + Alignment.holderSN + "," + MachineDataDefine.str;
                        LogAuto.SaveNGData(str);
                        // Error pError = new Error(ref this.m_NowAddress, "上传数据给MES失败！", "", (int)MErrorCode.MES上传参数信息失败);
                        //Error pError = new Error(ref this.m_NowAddress, MachineDataDefine.str, "", (int)MErrorCode.MES上传参数信息失败);
                        //pError.AddErrSloution("Retry", (int)AlignmentMode_WorkStep.上传数据);
                        //pError.AddErrSloution("下料", (int)AlignmentMode_WorkStep.上传失败下料);
                        //pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                        LogAuto.Notify("上传数据给MES失败", (int)MachineStation.主监控, LogLevel.Info);
                        m_nStep = (int)AlignmentMode_WorkStep.连三不连五变量;
                    }
                    break;
                case AlignmentMode_WorkStep.等待提交过站完成:
                    if (Post.POSTClass.getResult(0, CMDStep.提交过站).Result == "OK")
                    {
                        LogAuto.Notify("mes过站ok", (int)MachineStation.主监控, LogLevel.Info);
                        if (MachineDataDefine.machineState.b_UseHive)
                        {
                            m_nStep = (int)AlignmentMode_WorkStep.上传HIVE;
                        }
                        else if (MachineDataDefine.machineState.b_UsePDCA)
                        {
                            m_nStep = (int)AlignmentMode_WorkStep.上传PDCA;
                        }
                        else
                        {
                            m_nStep = (int)AlignmentMode_WorkStep.Start;
                        }
                    }
                    else if (timerDelay.Enabled == false)
                    {
                        string str = Alignment.SN + "," + Alignment.holderSN + "," + MachineDataDefine.str;
                        LogAuto.SaveNGData(str);
                        //   Error pError = new Error(ref this.m_NowAddress, "上传数据给MES失败！", "", (int)MErrorCode.提交过站失败);
                        //Error pError = new Error(ref this.m_NowAddress, "mes过站反馈超时", "", (int)MErrorCode.提交过站失败);
                        //pError.AddErrSloution("Retry", (int)AlignmentMode_WorkStep.提交过站);
                        //pError.AddErrSloution("下料", (int)AlignmentMode_WorkStep.上传失败下料);
                        //pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                        LogAuto.Notify("mes过站反馈超时", (int)MachineStation.主监控, LogLevel.Info);
                        m_nStep = (int)AlignmentMode_WorkStep.连三不连五变量;
                    }
                    else if (Post.POSTClass.getResult(0, CMDStep.提交过站).Result == "NG")
                    {
                        string str = Alignment.SN + "," + Alignment.holderSN + "," + MachineDataDefine.str;
                        LogAuto.SaveNGData(str);
                        //   Error pError = new Error(ref this.m_NowAddress, "上传数据给MES失败！", "", (int)MErrorCode.提交过站失败);
                        //Error pError = new Error(ref this.m_NowAddress, MachineDataDefine.str, "", (int)MErrorCode.提交过站失败);
                        //pError.AddErrSloution("Retry", (int)AlignmentMode_WorkStep.提交过站);
                        //pError.AddErrSloution("下料", (int)AlignmentMode_WorkStep.上传失败下料);
                        //pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                        LogAuto.Notify("提交过站失败", (int)MachineStation.主监控, LogLevel.Info);
                        m_nStep = (int)AlignmentMode_WorkStep.连三不连五变量;
                    }
                    break;
                case AlignmentMode_WorkStep.上传HIVE:
                    if (MachineDataDefine.machineState.b_UseHive)
                    {
                      string[] ver_dig_value=MESDataDefine.MESLXData.SW_Version.Split('_');
                      string ver_dig=  ver_dig_value[1].Replace(".", "") + "." + ver_dig_value[2];
                        LogAuto.Notify("上传HIVE数据", (int)MachineStation.主监控, LogLevel.Info);
                        Dictionary<string, string> dataDir = new Dictionary<string, string>();
                        dataDir.Add("产品SN", Alignment.SN);
                        dataDir.Add("工站", MESDataDefine.MESLXData.terminalName);
                        dataDir.Add("mac地址", MachineDataDefine.hive_mac);
                        dataDir.Add("载具号", Alignment.holderSN);
                        dataDir.Add("结果", "true");
                        //dataDir.Add("开始时间", HIVE.HIVEInstance.HIVEStarttime[0]);
                        //dataDir.Add("结束时间", HIVE.HIVEInstance.HIVEStoptime[0]);
                        dataDir.Add("开始时间", TimeManages.getTimeManage(Alignment.SN).alignmentStartTime.timeForJ8);
                        dataDir.Add("结束时间", TimeManages.getTimeManage(Alignment.SN).alignmentEndTime.timeForJ81addTime);
                        dataDir.Add("软件版本", MESDataDefine.MESLXData.SW_Version);
                        dataDir.Add("极限版本", MESDataDefine.MESLXData.limits_version);
                        dataDir.Add("浮点版本", ver_dig);
                        dataDir.Add("浮点上限", ver_dig);
                        dataDir.Add("浮点下限", ver_dig);
                        if (!MachineDataDefine.machineState.b_UseCCD)
                        {
                            // datas.Clear();
                            dataDir.Add("OffsetX值", "999");
                            dataDir.Add("OffsetY值", "999");
                            dataDir.Add("OffsetR值", "999");
                            dataDir.Add("段差值1", "0");
                            dataDir.Add("段差值2", "0");
                            dataDir.Add("段差值3", "0");
                            dataDir.Add("段差值4", "0");
                            dataDir.Add("段差值5", "0");
                            dataDir.Add("段差值6", "0");
                            dataDir.Add("段差值7", "0");
                            dataDir.Add("段差值8", "0");
                            dataDir.Add("ShiftP1P5值", "0");
                            dataDir.Add("ShiftP2P6值", "0");
                            dataDir.Add("ShiftP3P7值", "0");
                            dataDir.Add("ShiftP4P8值", "0");
                            dataDir.Add("最优解值", "0");
                            dataDir.Add("OffsetX_1值", "999");
                            dataDir.Add("OffsetY_1值", "999");
                            dataDir.Add("OffsetR_1值", "999");
                            dataDir.Add("段差_1值", "0");
                            dataDir.Add("段差_2值", "0");
                            dataDir.Add("段差_3值", "0");
                            dataDir.Add("段差_4值", "0");
                            dataDir.Add("段差_5值", "0");
                            dataDir.Add("段差_6值", "0");
                            dataDir.Add("段差_7值", "0");
                            dataDir.Add("段差_8值", "0");
                            dataDir.Add("ShiftP11P15值", "999");
                            dataDir.Add("ShiftP12P16值", "999");
                            dataDir.Add("ShiftP13P17值", "999");
                            dataDir.Add("ShiftP14P18值", "999");
                            dataDir.Add("最优解3", "0");
                            dataDir.Add("调整次数", index.ToString());
                        }
                        else
                        {
                            dataDir.Add("OffsetX值", datas["OffsetX"]);
                            dataDir.Add("OffsetY值", datas["OffsetY"]);
                            dataDir.Add("OffsetR值", datas["OffsetR"]);
                            dataDir.Add("段差值1", datas["P1"]);
                            dataDir.Add("段差值2", datas["P2"]);
                            dataDir.Add("段差值3", datas["P3"]);
                            dataDir.Add("段差值4", datas["P4"]);
                            dataDir.Add("段差值5", datas["P5"]);
                            dataDir.Add("段差值6", datas["P6"]);
                            dataDir.Add("段差值7", datas["P7"]);
                            dataDir.Add("段差值8", datas["P8"]);
                            dataDir.Add("ShiftP1P5值", datas["ShiftP1P5"]);
                            dataDir.Add("ShiftP2P6值", datas["ShiftP2P6"]);
                            dataDir.Add("ShiftP3P7值", datas["ShiftP3P7"]);
                            dataDir.Add("ShiftP4P8值", datas["ShiftP4P8"]);
                            dataDir.Add("最优解值", datas["最优解"]);
                            if (MachineDataDefine.b_UseLAD || b_DataModel)
                            {
                                if (MachineDataDefine.LADModel == 2)
                                {
                                    dataDir.Add("OffsetX_1值", "0");
                                    dataDir.Add("OffsetY_1值", "0");
                                    dataDir.Add("OffsetR_1值", "0");
                                    dataDir.Add("段差_1值", "0");
                                    dataDir.Add("段差_2值", "0");
                                    dataDir.Add("段差_3值", "0");
                                    dataDir.Add("段差_4值", "0");
                                    dataDir.Add("段差_5值", "0");
                                    dataDir.Add("段差_6值", "0");
                                    dataDir.Add("段差_7值", "0");
                                    dataDir.Add("段差_8值", "0");
                                    dataDir.Add("ShiftP11P15值", "0");
                                    dataDir.Add("ShiftP12P16值", "0");
                                    dataDir.Add("ShiftP13P17值", "0");
                                    dataDir.Add("ShiftP14P18值", "0");
                                    dataDir.Add("最优解3", "0");
                                }
                                else if(b_DataModel)
                                {
                                    dataDir.Add("OffsetX_1值", "0");
                                    dataDir.Add("OffsetY_1值", "0");
                                    dataDir.Add("OffsetR_1值", "0");
                                    dataDir.Add("段差_1值", "0");
                                    dataDir.Add("段差_2值", "0");
                                    dataDir.Add("段差_3值", "0");
                                    dataDir.Add("段差_4值", "0");
                                    dataDir.Add("段差_5值", "0");
                                    dataDir.Add("段差_6值", "0");
                                    dataDir.Add("段差_7值", "0");
                                    dataDir.Add("段差_8值", "0");
                                    dataDir.Add("ShiftP11P15值", "0");
                                    dataDir.Add("ShiftP12P16值", "0");
                                    dataDir.Add("ShiftP13P17值", "0");
                                    dataDir.Add("ShiftP14P18值", "0");
                                    dataDir.Add("最优解3", "0");
                                }
                            }
                            else
                            {
                                dataDir.Add("OffsetX_1值", datas1["OffsetX"]);
                                dataDir.Add("OffsetY_1值", datas1["OffsetY"]);
                                dataDir.Add("OffsetR_1值", datas1["OffsetR"]);
                                dataDir.Add("段差_1值", datas1["P1"]);
                                dataDir.Add("段差_2值", datas1["P2"]);
                                dataDir.Add("段差_3值", datas1["P3"]);
                                dataDir.Add("段差_4值", datas1["P4"]);
                                dataDir.Add("段差_5值", datas1["P5"]);
                                dataDir.Add("段差_6值", datas1["P6"]);
                                dataDir.Add("段差_7值", datas1["P7"]);
                                dataDir.Add("段差_8值", datas1["P8"]);
                                dataDir.Add("ShiftP11P15值", datas1["ShiftP1P5"]);
                                dataDir.Add("ShiftP12P16值", datas1["ShiftP2P6"]);
                                dataDir.Add("ShiftP13P17值", datas1["ShiftP3P7"]);
                                dataDir.Add("ShiftP14P18值", datas1["ShiftP4P8"]);
                                dataDir.Add("最优解3", datas1["最优解1"]);
                            }
                            dataDir.Add("调整次数", index.ToString());
                        }
                        // dataDir.Add("产品CT", TimeManages.getTimeManage(Alignment.SN).getAlignmentCT());
                        double time = Convert.ToDouble((HIVE.HIVEInstance.hivestoptime[0] - HIVE.HIVEInstance.lastStoptime[0]).TotalSeconds.ToString("f4"));
                        if (time > 3600)
                        {
                            time = 3600;
                        }
                        //  dataDir.Add("产品CT", (HIVE.HIVEInstance.hivestoptime[0] - HIVE.HIVEInstance.lastStoptime[0]).TotalSeconds.ToString());
                        dataDir.Add("产品CT", time.ToString());
                        string ct = (Convert.ToDateTime(TimeManages.getTimeManage(Alignment.SN).alignmentEndTime.timeForStraddTime) - Convert.ToDateTime(TimeManages.getTimeManage(Alignment.SN).alignmentStartTime.timeForStr)).TotalSeconds.ToString();
                        dataDir.Add("整体CT", ct);
                        string date = DateTime.Now.ToString("yyyy-MM-dd");
                        if (string.IsNullOrWhiteSpace(Alignment.SN))
                        {
                            Alignment.SN = "TEST" + DateTime.Now.ToString("HHmmss");
                        }
                        Alignment.SN = Alignment.SN.PadLeft(10, '0');
                        dataDir["产品SN"] = Alignment.SN;
                        string ZIPName = Alignment.SN + "_" + DateTime.Now.ToString("HHmmss") + ".zip";
                        string fullFileName = MESDataDefine.MESLXData.StrLocalPictureZipPath + "\\" + date + "\\" + ZIPName;
                        MachineDataDefine.ZipFilePath = MESDataDefine.MESLXData.StrLocalPictureZipPathName + "//" + date + "//" + ZIPName;
                        string errMSG = "";
                        ZIPHelper.myZIPHelper.ZIP(MESDataDefine.MESLXData.StrCCDPicturePath, fullFileName, ref errMSG);
                        dataDir.Add("压缩包全路径", fullFileName);
                        dataDir.Add("压缩包名称", ZIPName);
                        HIVE.HIVEInstance.HiveSendMACHINEDATA(Alignment.SN, "", 0, "", "", dataDir, false);
                    }
                    if (MachineDataDefine.machineState.b_UseRemoteQualification)
                    {
                        MachineDataDefine.remoteFirstProduct = true;
                        string date = DateTime.Now.ToString("yyyy-MM-dd");
                        string SN11 = "TEST" + DateTime.Now.ToString("HHmmss");
                        Dictionary<string, string> dataDir = new Dictionary<string, string>();
                        dataDir.Add("产品SN", SN11);
                        dataDir.Add("工站", MESDataDefine.MESLXData.terminalName);
                        dataDir.Add("mac地址", MachineDataDefine.hive_mac);
                        dataDir.Add("载具号", Alignment.holderSN);
                        dataDir.Add("结果", "true");
                        // dataDir.Add("开始时间", HIVE.HIVEInstance.HIVEStarttime[0]);
                        //  dataDir.Add("结束时间", HIVE.HIVEInstance.HIVEStoptime[0]);
                        dataDir.Add("开始时间", TimeManages.getTimeManage(Alignment.SN).alignmentStartTime.timeForJ8);
                        dataDir.Add("结束时间", TimeManages.getTimeManage(Alignment.SN).alignmentEndTime.timeForJ81addTime);
                        dataDir.Add("软件版本", MESDataDefine.MESLXData.SW_Version);
                        dataDir.Add("极限版本", MESDataDefine.MESLXData.limits_version);
                        if (!MachineDataDefine.machineState.b_UseCCD)
                        {
                            // datas.Clear();
                            dataDir.Add("OffsetX值", "999");
                            dataDir.Add("OffsetY值", "999");
                            dataDir.Add("OffsetR值", "999");
                            dataDir.Add("段差值1", "0");
                            dataDir.Add("段差值2", "0");
                            dataDir.Add("段差值3", "0");
                            dataDir.Add("段差值4", "0");
                            dataDir.Add("段差值5", "0");
                            dataDir.Add("段差值6", "0");
                            dataDir.Add("段差值7", "0");
                            dataDir.Add("段差值8", "0");
                            dataDir.Add("ShiftP1P5值", "0");
                            dataDir.Add("ShiftP2P6值", "0");
                            dataDir.Add("ShiftP3P7值", "0");
                            dataDir.Add("ShiftP4P8值", "0");
                            dataDir.Add("最优解值", "0");
                            dataDir.Add("OffsetX_1值", "999");
                            dataDir.Add("OffsetY_1值", "999");
                            dataDir.Add("OffsetR_1值", "999");
                            dataDir.Add("段差_1值", "0");
                            dataDir.Add("段差_2值", "0");
                            dataDir.Add("段差_3值", "0");
                            dataDir.Add("段差_4值", "0");
                            dataDir.Add("段差_5值", "0");
                            dataDir.Add("段差_6值", "0");
                            dataDir.Add("段差_7值", "0");
                            dataDir.Add("段差_8值", "0");
                            dataDir.Add("ShiftP11P15值", "0");
                            dataDir.Add("ShiftP12P16值", "0");
                            dataDir.Add("ShiftP13P17值", "0");
                            dataDir.Add("ShiftP14P18值", "0");
                            dataDir.Add("最优解3", "0");
                            dataDir.Add("调整次数", index.ToString());
                        }
                        else
                        {
                            dataDir.Add("OffsetX值", datas["OffsetX"]);
                            dataDir.Add("OffsetY值", datas["OffsetY"]);
                            dataDir.Add("OffsetR值", datas["OffsetR"]);
                            dataDir.Add("段差值1", datas["P1"]);
                            dataDir.Add("段差值2", datas["P2"]);
                            dataDir.Add("段差值3", datas["P3"]);
                            dataDir.Add("段差值4", datas["P4"]);
                            dataDir.Add("段差值5", datas["P5"]);
                            dataDir.Add("段差值6", datas["P6"]);
                            dataDir.Add("段差值7", datas["P7"]);
                            dataDir.Add("段差值8", datas["P8"]);
                            dataDir.Add("ShiftP1P5值", datas["ShiftP1P5"]);
                            dataDir.Add("ShiftP2P6值", datas["ShiftP2P6"]);
                            dataDir.Add("ShiftP3P7值", datas["ShiftP3P7"]);
                            dataDir.Add("ShiftP4P8值", datas["ShiftP4P8"]);
                            dataDir.Add("最优解值", datas["最优解"]);

                            if (MachineDataDefine.b_UseLAD)
                            {
                                if (MachineDataDefine.LADModel == 2)
                                {
                                    dataDir.Add("OffsetX_1值", "0");
                                    dataDir.Add("OffsetY_1值", "0");
                                    dataDir.Add("OffsetR_1值", "0");
                                    dataDir.Add("段差_1值", "0");
                                    dataDir.Add("段差_2值", "0");
                                    dataDir.Add("段差_3值", "0");
                                    dataDir.Add("段差_4值", "0");
                                    dataDir.Add("段差_5值", "0");
                                    dataDir.Add("段差_6值", "0");
                                    dataDir.Add("段差_7值", "0");
                                    dataDir.Add("段差_8值", "0");
                                    dataDir.Add("ShiftP11P15值", "0");
                                    dataDir.Add("ShiftP12P16值", "0");
                                    dataDir.Add("ShiftP13P17值", "0");
                                    dataDir.Add("ShiftP14P18值", "0");
                                    dataDir.Add("最优解3", "0");
                                }

                            }
                            else
                            {
                                dataDir.Add("OffsetX_1值", datas1["OffsetX"]);
                                dataDir.Add("OffsetY_1值", datas1["OffsetY"]);
                                dataDir.Add("OffsetR_1值", datas1["OffsetR"]);
                                dataDir.Add("段差_1值", datas1["P1"]);
                                dataDir.Add("段差_2值", datas1["P2"]);
                                dataDir.Add("段差_3值", datas1["P3"]);
                                dataDir.Add("段差_4值", datas1["P4"]);
                                dataDir.Add("段差_5值", datas1["P5"]);
                                dataDir.Add("段差_6值", datas1["P6"]);
                                dataDir.Add("段差_7值", datas1["P7"]);
                                dataDir.Add("段差_8值", datas1["P8"]);
                                dataDir.Add("ShiftP11P15值", datas1["ShiftP1P5"]);
                                dataDir.Add("ShiftP12P16值", datas1["ShiftP2P6"]);
                                dataDir.Add("ShiftP13P17值", datas1["ShiftP3P7"]);
                                dataDir.Add("ShiftP14P18值", datas1["ShiftP4P8"]);
                                dataDir.Add("最优解3", datas1["最优解1"]);
                            }
                            dataDir.Add("调整次数", index.ToString());
                        }
                        //dataDir.Add("产品CT", TimeManages.getTimeManage(Alignment.SN).getAlignmentCT());
                        double time = Convert.ToDouble((HIVE.HIVEInstance.hivestoptime[0] - HIVE.HIVEInstance.lastStoptime[0]).TotalSeconds.ToString("f4"));
                        if (time > 3600)
                        {
                            time = 3600;
                        }
                        //dataDir.Add("产品CT", (HIVE.HIVEInstance.hivestoptime[0]- HIVE.HIVEInstance.lastStoptime[0]).TotalSeconds.ToString());
                        dataDir.Add("产品CT", time.ToString());
                        string ct = (Convert.ToDateTime(TimeManages.getTimeManage(Alignment.SN).alignmentEndTime.timeForStradd) - Convert.ToDateTime(TimeManages.getTimeManage(Alignment.SN).alignmentStartTime.timeForStr)).TotalSeconds.ToString();
                        dataDir.Add("整体CT", ct);
                        dataDir.Add("压缩包名称", DateTime.Now.ToString("HHmmss") + "_" + SN11 + ".zip");
                        string ZIPName = Alignment.SN + "_" + DateTime.Now.ToString("HHmmss") + ".zip";
                        string fullFileName = MESDataDefine.MESLXData.StrLocalPictureZipPath + "\\" + date + "\\" + ZIPName;
                        dataDir.Add("压缩包全路径", fullFileName);
                        // dataDir.Add("压缩包名称", ZIPName);
                        HIVE.HIVEInstance.HiveSendMACHINEDATA(SN11, "", 0, "", "", dataDir, true);

                    }
                    #region  LAD模式

                    // frm_Main.formData.CTUnit1.EndDoLeft("1234567890", 10.ToString());
                    if (MachineDataDefine.b_UseLAD)
                    {
                        LogAuto.Notify("使用LAD模式", (int)MachineStation.主监控, LogLevel.Info);
                        if (MachineDataDefine.LADModel == 1)//CPK
                        {
                            LogAuto.Notify("进入CPK模式", (int)MachineStation.主监控, LogLevel.Info);
                            if (productCount == 32)
                            {
                                LogAuto.Notify("已做完32片料", (int)MachineStation.主监控, LogLevel.Info);
                                MachineDataDefine.LADOPID = 1;
                                dia_ChooseFrm dia = new FormView._4弹窗.dia_ChooseFrm(this);
                                dia.ShowDialog();
                                #region  禁用
                                //if (dia.DialogResult == System.Windows.Forms.DialogResult.OK)
                                //{
                                //    productCount = 0;
                                //}
                                //else
                                //{
                                //    //frm_Main.formData?.ChartTime1.SetRunStatus(Chart.ChartTime.MachineStatus.idle);
                                //    MachineDataDefine.b_UseLAD = false;
                                //}
                                //Thread th = new Thread(new ThreadStart(() =>
                                //{
                                //    dia_ChooseFrm dia = new FormView._4弹窗.dia_ChooseFrm(this);
                                //    Application.Run(dia);

                                //}));
                                #endregion
                            }
                        }
                        else if (MachineDataDefine.LADModel == 2)//GRR
                        {
                            LogAuto.Notify("进入GRR模式", (int)MachineStation.主监控, LogLevel.Info);
                            //  if (productCount ==30)
                            if (productCount == 30)
                            {
                                LogAuto.Notify("已做完30片料", (int)MachineStation.主监控, LogLevel.Info);
                                dia_ChooseFrm dia = new FormView._4弹窗.dia_ChooseFrm(this);
                                dia.ShowDialog();
                                #region  禁用
                                //if (dia.DialogResult == System.Windows.Forms.DialogResult.OK)
                                //{
                                //    productCount = 0;
                                //    MachineDataDefine.LADOPID = 1;
                                //}
                                //else
                                //{
                                //    MachineDataDefine.LADOPID = 2;
                                //}
                                //Thread th = new Thread(new ThreadStart(() =>
                                //{
                                //    dia_ChooseFrm dia = new FormView._4弹窗.dia_ChooseFrm(this);

                                //    Application.Run(dia);
                                //    dia.ShowDialog();

                                //}));
                                #endregion
                            }
                            // else if (productCount == 60)
                            else if (productCount == 60)
                            {
                                LogAuto.Notify("已做完60片料", (int)MachineStation.主监控, LogLevel.Info);
                                dia_ChooseFrm dia = new FormView._4弹窗.dia_ChooseFrm(this);
                                dia.ShowDialog();
                                #region  禁用
                                //if (dia.DialogResult == System.Windows.Forms.DialogResult.OK)
                                //{
                                //    productCount = 30;
                                //    MachineDataDefine.LADOPID = 2;
                                //}
                                //else
                                //{
                                //    MachineDataDefine.LADOPID = 3;
                                //}
                                //Thread th = new Thread(new ThreadStart(() =>
                                //{
                                //    dia_ChooseFrm dia = new FormView._4弹窗.dia_ChooseFrm(this);
                                //    Application.Run(dia);

                                //}));
                                #endregion
                            }
                            //  else if (productCount == 90)
                            else if (productCount == 90)
                            {
                                LogAuto.Notify("已做完90片料", (int)MachineStation.主监控, LogLevel.Info);
                                // dia_ChooseFrm dia = new FormView._4弹窗.dia_ChooseFrm();
                                dia_ChooseFrm dia = new FormView._4弹窗.dia_ChooseFrm(this);
                                dia.ShowDialog();
                                #region 禁用
                                //if (dia.DialogResult == System.Windows.Forms.DialogResult.OK)
                                //{
                                //    productCount = 60;
                                //    MachineDataDefine.LADOPID = 3;
                                //}
                                //else
                                //{
                                //   // frm_Main.formData?.ChartTime1.SetRunStatus(Chart.ChartTime.MachineStatus.idle); frm_Main.formData?.ChartTime1.SetRunStatus(Chart.ChartTime.MachineStatus.idle);
                                //    MachineDataDefine.b_UseLAD = false;
                                //}
                                //Thread th = new Thread(new ThreadStart(() =>
                                //{
                                //    dia_ChooseFrm dia = new FormView._4弹窗.dia_ChooseFrm(this);
                                //    Application.Run(dia);

                                //}));
                                #endregion
                            }
                            if (MachineDataDefine.downLad)
                            {
                                LadNum = 0;
                                MachineDataDefine.downLad = false;
                               // Alignment.alignProductCompelet = true;
                                m_nStep = (int)AlignmentMode_WorkStep.Start;
                                break;
                            }
                            else
                            {
                                if (LadNum < 3 && !下料)
                                {
                                    timerDelay.Enabled = false;
                                    timerDelay.Interval = 1500;
                                    timerDelay.Start();
                                    m_nStep = (int)AlignmentMode_WorkStep.LAD等待延时;
                                    break;
                                }
                                else
                                {
                                    LadNum = 0;
                                    m_nStep = (int)AlignmentMode_WorkStep.Start;
                                    break;
                                }
                            }
                        }
                        else if (MachineDataDefine.LADModel == 3)//SCS
                        {
                            if (productCount == 10)
                            {
                                LogAuto.Notify("已做完10片料", (int)MachineStation.主监控, LogLevel.Info);
                                MachineDataDefine.LADOPID = 1;
                                dia_ChooseFrm dia = new FormView._4弹窗.dia_ChooseFrm(this);
                                dia.ShowDialog();
                                #region 禁用
                                //if (dia.DialogResult == System.Windows.Forms.DialogResult.OK)
                                //{
                                //    productCount = 0;
                                //}
                                //else
                                //{
                                //    //frm_Main.formData?.ChartTime1.SetRunStatus(Chart.ChartTime.MachineStatus.idle);
                                //    MachineDataDefine.b_UseLAD = false;
                                //}
                                //Thread th = new Thread(new ThreadStart(() =>
                                //{
                                //    dia_ChooseFrm dia = new FormView._4弹窗.dia_ChooseFrm(this);
                                //    Application.Run(dia);

                                //}));
                                #endregion
                            }
                        }
                    }
                    #endregion
                    if (MachineDataDefine.machineState.b_UsePDCA)
                    {
                        m_nStep = (int)AlignmentMode_WorkStep.上传PDCA;
                    }
                    else
                    {
                        // string s = endtime;   //TimeManages.getTimeManage(Alignment.SN).alignmentEndTime.timeForStradd;
                        // string s2 = starttime;// TimeManages.getTimeManage(Alignment.SN).alignmentStartTime.timeForStr;

                        //string  ss = (Convert.ToDateTime(s) - Convert.ToDateTime(s2)).TotalSeconds.ToString("0.00");
                        //  frm_Main.formData.Chartcapacity1.AddOkLeft();

                        //  double ctt =double.Parse((Convert.ToDateTime(TimeManages.getTimeManage(Alignment.SN).alignmentEndTime.timeForJ81add) - Convert.ToDateTime(TimeManages.getTimeManage(Alignment.SN).alignmentStartTime.timeForJ8)).TotalSeconds.ToString("0.00"));

                        //  string ct = ctt.ToString();
                        ////   frm_Main.formData.CTUnit1.EndDoLeft(Alignment.SN, ct,);
                        //// string ct = (Convert.ToDateTime(endtime) - Convert.ToDateTime(starttime)).ToString("0.00");
                        //  frm_Main.formData.CTUnit1.EndDoLeft(Alignment.SN, ct,starttime,endtime);
                        m_nStep = (int)AlignmentMode_WorkStep.Start;
                    }
                    break;
                case AlignmentMode_WorkStep.上传PDCA:
                    if (true)
                    {
                        //LogAuto.Notify("上传HIVE数据", (int)MachineStation.主监控, LogLevel.Info);
                        LogAuto.Notify("上传PDCA！", (int)MachineStation.主监控, LogLevel.Info);
                        Dictionary<string, string> datas11 = new Dictionary<string, string>();
                        datas11.Add("产品SN", Alignment.SN);
                        datas11.Add("开始时间", TimeManages.getTimeManage(Alignment.SN).alignmentStartTime.timeForStr);
                        datas11.Add("结束时间", TimeManages.getTimeManage(Alignment.SN).alignmentEndTime.timeForStraddTime);
                        datas11.Add("软件版本", MESDataDefine.MESLXData.SW_Version);
                        datas11.Add("载具号", Alignment.holderSN);
                        datas11.Add("OffsetX值", datas["OffsetX"]);
                        datas11.Add("OffsetY值", datas["OffsetY"]);
                        datas11.Add("OffsetR值", datas["OffsetR"]);
                        datas11.Add("P1值", datas["P1"]);
                        datas11.Add("P2值", datas["P2"]);
                        datas11.Add("P3值", datas["P3"]);
                        datas11.Add("P4值", datas["P4"]);
                        datas11.Add("P5值", datas["P5"]);
                        datas11.Add("P6值", datas["P6"]);
                        datas11.Add("P7值", datas["P7"]);
                        datas11.Add("P8值", datas["P8"]);
                        datas11.Add("Shift1值", datas["ShiftP1P5"]);
                        datas11.Add("Shift2值", datas["ShiftP2P6"]);
                        datas11.Add("Shift3值", datas["ShiftP3P7"]);
                        datas11.Add("Shift4值", datas["ShiftP4P8"]);
                        datas11.Add("最优解值", datas["最优解"]);
                        datas11.Add("OffsetX_1值", datas1["OffsetX"]);
                        datas11.Add("OffsetY_1值", datas1["OffsetY"]);
                        datas11.Add("OffsetR_1值", datas1["OffsetR"]);
                        datas11.Add("P1_1值", datas1["P1"]);
                        datas11.Add("P2_1值", datas1["P2"]);
                        datas11.Add("P3_1值", datas1["P3"]);
                        datas11.Add("P4_1值", datas1["P4"]);
                        datas11.Add("P5_1值", datas1["P5"]);
                        datas11.Add("P6_1值", datas1["P6"]);
                        datas11.Add("P7_1值", datas1["P7"]);
                        datas11.Add("P8_1值", datas1["P8"]);
                        datas11.Add("Shift1_1值", datas1["ShiftP1P5"]);
                        datas11.Add("Shift2_1值", datas1["ShiftP2P6"]);
                        datas11.Add("Shift3_1值", datas1["ShiftP3P7"]);
                        datas11.Add("Shift4_1值", datas1["ShiftP4P8"]);
                        datas11.Add("最优解3", datas1["最优解1"]);
                        datas11.Add("调整次数", index.ToString());
                        string ct = (Convert.ToDateTime(TimeManages.getTimeManage(Alignment.SN).alignmentEndTime.timeForStradd) - Convert.ToDateTime(TimeManages.getTimeManage(Alignment.SN).alignmentStartTime.timeForStr)).TotalSeconds.ToString();
                        datas11.Add("设备CT", ct);
                        datas11.Add("压缩图片路径", MESDataDefine.MESLXData.StrPDCAImagePath + MachineDataDefine.ZipFilePath);
                        datas11.Add("电脑名称", MESDataDefine.MESLXData.StrUser);
                        datas11.Add("电脑密码", MESDataDefine.MESLXData.StrPassWord);
                        Post.POSTClass.AddCMD(0, Post.CMDStep.上传PDCA, datas11);
                        timerDelay.Enabled = false;
                        timerDelay.Interval = 8000;
                        timerDelay.Start();
                        m_nStep = (int)AlignmentMode_WorkStep.等待PDCA上传完成;
                        
                    }
                    break;
                case AlignmentMode_WorkStep.等待PDCA上传完成:
                    if (Post.POSTClass.getResult(0, CMDStep.上传PDCA).Result == "OK")
                    {
                        LogAuto.Notify("上传PDCA OK！", (int)MachineStation.主监控, LogLevel.Info);
                        //  b_InAndOutModel = false;
                        frm_Main.formData.Chartcapacity1.AddOkLeft();
                        //string ct = (Convert.ToDateTime(TimeManages.getTimeManage(Alignment.SN).alignmentEndTime.timeForStradd) - Convert.ToDateTime(TimeManages.getTimeManage(Alignment.SN).alignmentStartTime.timeForStr)).ToString("0.00");
                        MachineDataDefine.MachineControlS.gross_COF_result = true;
                        MachineDataDefine.MachineControlS.continueNG_three.Clear();
                        m_nStep = (int)AlignmentMode_WorkStep.Start;
                    }
                    else if (timerDelay.Enabled == false)
                    {
                        LogAuto.Notify("上传数据给PDCA反馈超时！", (int)MachineStation.主监控, LogLevel.Info);
                        string str = Alignment.SN + "," + Alignment.holderSN + "," + "上传数据给PDCA反馈超时！";
                        LogAuto.SaveNGData(str);
                        Alignment.PDCA反馈超时 = true;
                        //Error pError = new Error(ref this.m_NowAddress, "上传数据给PDCA反馈超时！", "", (int)MErrorCode.MES上传参数信息失败);
                        //pError.AddErrSloution("Retry", (int)AlignmentMode_WorkStep.上传PDCA);
                        //pError.AddErrSloution("下料", (int)AlignmentMode_WorkStep.上传失败下料);
                        //pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                        m_nStep = (int)AlignmentMode_WorkStep.连三不连五变量;

                    }
                    else if (Post.POSTClass.getResult(0, CMDStep.上传PDCA).Result == "NG")
                    {
                        LogAuto.Notify("上传数据给PDCA失败！", (int)MachineStation.主监控, LogLevel.Info);
                        string str = Alignment.SN + "," + Alignment.holderSN + "," + "上传数据给PDCA失败！";
                        LogAuto.SaveNGData(str);
                        Alignment.上传PDCA失败 = true;
                        //Error pError = new Error(ref this.m_NowAddress, "上传数据给PDCA失败！", "", (int)MErrorCode.MES上传参数信息失败);
                        //pError.AddErrSloution("Retry", (int)AlignmentMode_WorkStep.上传PDCA);
                        //pError.AddErrSloution("下料", (int)AlignmentMode_WorkStep.上传失败下料);
                        //pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);

                        m_nStep = (int)AlignmentMode_WorkStep.连三不连五变量;
                    }
                    break;
                case AlignmentMode_WorkStep.上传失败下料:
                    //  SendFail = true;
                    timerDelay.Enabled = false;
                    timerDelay.Interval = 3000;
                    timerDelay.Start();
                    m_nStep = (int)AlignmentMode_WorkStep.Start;
                    break;
                case AlignmentMode_WorkStep.相机重连:
                    try
                    {
                        if (ConnectionControl.getSocketControl(EnumParam_ConnectionName.CCD).strConnectionOK != "OK")
                        {
                            ConnectionControl.getSocketControl(EnumParam_ConnectionName.CCD).Start();
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    m_nStep = (int)AlignmentMode_WorkStep.触发相机拍照;
                    break;
                case AlignmentMode_WorkStep.相机NG抛料:
                    b_Result = false;
                    下料 = true;
                    NG拍照下料显示 = true;
                    b_DownloadCheck = false;
                    string str12 = Alignment.SN + "," + Alignment.holderSN + "," + "CCD未返回数据！";
                    LogAuto.SaveNGData(str12);
                    m_nStep = (int)AlignmentMode_WorkStep.调整位定位销固定顶升气缸上1;
                    break;
                case AlignmentMode_WorkStep.NG抛料:
                    LogAuto.Notify("NG抛料！", (int)MachineStation.主监控, LogLevel.Info);
                    NG下料 = true;
                    //timerDelay.Enabled = false;
                    //timerDelay.Interval = 3000;
                    //timerDelay.Start();
                    m_nStep = (int)AlignmentMode_WorkStep.调整位定位销固定顶升气缸上1;
                    break;
                case AlignmentMode_WorkStep.LAD等待延时:
                    if (timerDelay.Enabled == false)
                    {
                        m_nStep = m_nStep = (int)AlignmentMode_WorkStep.调整位定位销固定顶升气缸上;
                    }
                    break;
                case AlignmentMode_WorkStep.LAD等待延时1:
                    if (timerDelay.Enabled == false)
                    {
                        m_nStep = m_nStep = m_nStep = (int)AlignmentMode_WorkStep.调整位定位销固定顶升气缸下1;
                    }
                    break;
                case AlignmentMode_WorkStep.Completed:
                    m_Status = 狀態.待命;
                    break;
                case AlignmentMode_WorkStep.ThreeAndFive:
                    {
                        if (MachineDataDefine.machineState.UseUpLoadMesErr)  //是否启用连三不连五
                        {
                            if (!MachineDataDefine.MachineControlS.gross_COF_result)
                            {
                                //Alignment.gross_COF_result = true;
                                #region  连3不练5
                                MachineDataDefine.MachineControlS.continueNG_three.Add(DateTime.Now);
                                MachineDataDefine.MachineControlS.continueNG_five.Add(DateTime.Now);
                                TimeSpan time = MachineDataDefine.MachineControlS.continueNG_five.Last() - MachineDataDefine.MachineControlS.continueNG_five[0];
                                if (time.TotalSeconds > MachineDataDefine.machineState.NGNumTime*60)  //1个小时
                                {
                                    MachineDataDefine.MachineControlS.continueNG_five.Remove(MachineDataDefine.MachineControlS.continueNG_five[0]);
                                }
                                if (MachineDataDefine.MachineControlS.continueNG_three.Count >= MachineDataDefine.machineState.ContinuousNum || MachineDataDefine.MachineControlS.continueNG_five.Count >= MachineDataDefine.machineState.NGNumInTime)
                                {
                                    string strShowMessage = "";
                                    if (MachineDataDefine.MachineControlS.continueNG_three.Count >= 3)
                                    {
                                        strShowMessage = "连续报警3次";
                                    }
                                    else if (MachineDataDefine.MachineControlS.continueNG_five.Count >= 5)
                                    {
                                        strShowMessage = "一小时不连续报警5次";
                                    }
                                    MachineDataDefine.MachineControlS.continueNG_three.Clear();
                                    MachineDataDefine.MachineControlS.continueNG_five.Clear();
                                    Error pError = new Error(ref this.m_NowAddress, strShowMessage, "", (int)MErrorCode.连续3次NG报警);
                                    pError.AddErrSloution("确认OK,继续生产", (int)AlignmentMode_WorkStep.Start);
                                    pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                                }
                                #endregion
                            }
                        }
                        //else
                        //{
                        //    MachineDataDefine.MachineControlS.continueNG_three.Clear();
                        //}
                                                            
                    }
                   
                    m_nStep = (int)AlignmentMode_WorkStep.Start;
                    break;

                case AlignmentMode_WorkStep.连三不连五变量:
                    LogAuto.Notify("连三不连五变量NG变量记录！", (int)MachineStation.主监控, LogLevel.Info);
                    //if (MachineDataDefine.machineState.UseUpLoadMesErr)
                    //{
                    MachineDataDefine.MachineControlS.gross_COF_result = false;
                //}
                m_nStep = (int)AlignmentMode_WorkStep.ThreeAndFive;
                    break;
            }
        }
        public bool Action()
        {
            productCount = 0;
            MachineDataDefine.LADOPID = 1;
            m_Status = 狀態.動作中;
            m_nStep = (int)AlignmentMode_WorkStep.Start;
            return base.DoStep(m_nStep);
        }
        public override void Stop()
        {
            reworknum = 0;
            reworknum2 = 0;
            reworknum3 = 0;
            productCount = 0;
            LadNum = 0;
            MachineDataDefine.downLad = false;
            MachineDataDefine.StopNG = false;
            b_DownloadCheck = false;
            DataIndex = 0;
            b_DataModel = false;
            b_InAndOutModel = false;
            下料 = false;
            Alignment.holderSN = "";
            Alignment.SN = "";
            流道NG抛料 = false;
            // Alignment.NG下料 = false;
            NG超限下料显示 = false;
            NG拍照下料显示 = false;
            调整超时下料显示 = false;
            Photoretry = false;
            膨胀气缸报警 = false;
            //m_bHomeCompleted = true;
            Resetcount = 0;
            Rework = false;
            MachineDataDefine.remoteFirstProduct = false;
            axisCalibration.Stop();
            HardWareControl.getMotor(EnumParam_Axis.X2).Stop();
            HardWareControl.getMotor(EnumParam_Axis.Y3).Stop();
            HardWareControl.getMotor(EnumParam_Axis.R).Stop();
            HardWareControl.getMotor(EnumParam_Axis.X1).Stop();
            base.Stop();
        }
        public override bool DoHomeStep(int iHomeStep)
        {
            m_Status = 狀態.回HOME中;
            m_nHomeStep = (int)AlignmentMode_HomeStep.Start;
            return base.DoHomeStep(0);
        }
    }
}
