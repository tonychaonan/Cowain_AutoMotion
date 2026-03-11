using Cowain_AutoMotion.Flow;
using Cowain_AutoMotion.Flow._2Work;
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
using static Cowain_Machine.Flow.MErrorDefine;

namespace Cowain_AutoMotion
{
    public class WorkProcess1 : Base
    {
        public AlignmentMode alignmentMode;
        private WorkProcess1_HomeStep currentHomeStep;
        private WorkProcess1_WorkStep currentWorkStep;
        private int station;
        public string result = "999";
        int ngTime = 0;
        double OffsetX, OffsetY, OffsetR = 0;
        public WorkProcess1(int station1) : base()
        {
            station = station1;
            alignmentMode = new Flow._2Work.AlignmentMode();
            AddBase(ref alignmentMode.m_NowAddress);
        }
        public enum WorkProcess1_HomeStep
        {
            Start = 0,
            等待调整模块回原完成,
            定位销顶升气缸顶起,
            调整R轴回原,
            调整XY轴回原,
            气缸回原,
            Z轴回原,
            Y轴回原,
            Completed
        }
        public enum WorkProcess1_WorkStep
        {
            Start = 0,
            入料口位载具阻挡气缸回,
            入料口位载具阻挡气缸原点信号,
            扫码枪扫码,
            获取到载具码,
            扫码成功,
            扫码失败,
            载具码获取产品SN,
            检查UOP,
            流道阻挡气缸下,
            流道阻挡气缸原点信号,
            入料口位载具阻挡气缸出,
            入料龙门位载具顶升气缸上,
            前龙门移动到前取料位,
            入料龙门位取料夹爪气缸夹,
            前龙门移动到放料位,
            入料龙门位取料夹爪气缸开,
            前龙门移动到待命位,
            横移X轴移动到拍照位,
            //-------
            调整模块开始工作,
            等待调整模块完成,
            //----------
            后龙门移动到后取料位,
            出料龙门位取料夹爪气缸夹,
            后龙门移动到下料位,
            出料龙门位夹爪气缸回,
            后龙门移动到待命位,
            出料口位前顶升气缸下,
            出料口位后顶升气缸上,
            出料口位阻挡气缸回,
            出料口位前顶升气缸上,
            出料口位后顶升气缸下,
            机械手取料,
            机械手准备取料,
            机械手取料完成,
            Completed
        }
        public override void HomeCycle(ref double dbTime)
        {
            currentHomeStep = (WorkProcess1_HomeStep)m_nStep;
            switch (currentHomeStep)
            {
                case WorkProcess1_HomeStep.Start:
                    //LogAuto.Notify("工位"+ station.ToString()+"开始复位！");
                    HardWareControl.getOutputIO(EnumParam_OutputIO.三色灯_黄灯).SetIO(true);
                    HardWareControl.getOutputIO(EnumParam_OutputIO.A流道步进运行).SetIO(false);
                    HardWareControl.getOutputIO(EnumParam_OutputIO.B流道步进运行).SetIO(false);
                    //StaticParam.OutputDictionary["A流道步进方向"].SetIO(true);
                    //StaticParam.OutputDictionary["B流道步进方向"].SetIO(true);
                    m_nStep = (int)WorkProcess1_HomeStep.等待调整模块回原完成;
                    alignmentMode.DoHomeStep(0);
                    break;
                case WorkProcess1_HomeStep.等待调整模块回原完成:
                    if (alignmentMode.isIDLE())
                    {
                        m_nStep = (int)WorkProcess1_HomeStep.定位销顶升气缸顶起;
                    }
                    break;
                case WorkProcess1_HomeStep.定位销顶升气缸顶起:
                    frm_Main.formData.ChartTime1.SetTime();
                    HardWareControl.getValve(EnumParam_Valve.调整位定位销固定顶升气缸上).Open();
                    m_nStep = (int)WorkProcess1_HomeStep.调整R轴回原;
                    break;
                case WorkProcess1_HomeStep.调整R轴回原:
                    if (HardWareControl.getValve(EnumParam_Valve.调整位定位销固定顶升气缸上).isIDLE())
                    {
                        HardWareControl.getMotor(EnumParam_Axis.R).DoHome();
                        m_nStep = (int)WorkProcess1_HomeStep.调整XY轴回原;
                    }
                    break;
                case WorkProcess1_HomeStep.调整XY轴回原:
                    if (HardWareControl.getMotor(EnumParam_Axis.R).isIDLE())
                    {
                        HardWareControl.getMotor(EnumParam_Axis.X2).DoHome();
                        HardWareControl.getMotor(EnumParam_Axis.Y3).DoHome();
                        m_nStep = (int)WorkProcess1_HomeStep.气缸回原;
                    }
                    break;
                case WorkProcess1_HomeStep.气缸回原:
                    if (HardWareControl.getMotor(EnumParam_Axis.Y3).isIDLE())
                    {
                        HardWareControl.getValve(EnumParam_Valve.入料口位载具阻挡气缸).Open();
                        HardWareControl.getValve(EnumParam_Valve.流道阻挡气缸).Open();
                        HardWareControl.getValve(EnumParam_Valve.入料龙门位载具顶升气缸上).Open();
                        HardWareControl.getValve(EnumParam_Valve.调整位压板限位气缸).Close();
                        HardWareControl.getValve(EnumParam_Valve.调整位定位销固定顶升气缸上).Close();
                        HardWareControl.getValve(EnumParam_Valve.调整位膨胀顶升气缸).Close();
                        HardWareControl.getValve(EnumParam_Valve.调整位膨胀气缸).Close();
                        HardWareControl.getValve(EnumParam_Valve.出料口位前顶升气缸上).Open();
                        HardWareControl.getValve(EnumParam_Valve.出料口位阻挡气缸).Open();
                        HardWareControl.getValve(EnumParam_Valve.出料口位后顶升气缸上).Close();
                        HardWareControl.getValve(EnumParam_Valve.出料口位上下阻挡气缸).Open();
                        m_nStep = (int)WorkProcess1_HomeStep.Z轴回原;
                    }
                    break;
                case WorkProcess1_HomeStep.Z轴回原:
                    bool a = HardWareControl.getValve(EnumParam_Valve.入料口位载具阻挡气缸).isIDLE();
                    bool a1 = HardWareControl.getValve(EnumParam_Valve.流道阻挡气缸).isIDLE();
                    bool a2 = HardWareControl.getValve(EnumParam_Valve.入料龙门位载具顶升气缸上).isIDLE();
                    bool a3 = HardWareControl.getValve(EnumParam_Valve.调整位压板限位气缸).isIDLE();
                    bool a4 = HardWareControl.getValve(EnumParam_Valve.调整位定位销固定顶升气缸上).isIDLE();
                    bool a5 = HardWareControl.getValve(EnumParam_Valve.调整位膨胀顶升气缸).isIDLE();
                    bool a6 = HardWareControl.getValve(EnumParam_Valve.调整位膨胀气缸).isIDLE();
                    bool a7 = HardWareControl.getValve(EnumParam_Valve.出料口位前顶升气缸上).isIDLE();
                    bool a8 = HardWareControl.getValve(EnumParam_Valve.出料口位阻挡气缸).isIDLE();
                    bool a9 = HardWareControl.getValve(EnumParam_Valve.出料口位后顶升气缸上).isIDLE();
                    bool a10 = HardWareControl.getValve(EnumParam_Valve.出料口位上下阻挡气缸).isIDLE();
                    if (a && a1 && a2 && a3 && a4 && a5 && a6 && a7 && a8 && a9 && a10)
                    {
                        HardWareControl.getMotor(EnumParam_Axis.Z1).DoHome();
                        HardWareControl.getMotor(EnumParam_Axis.Z2).DoHome();
                        HardWareControl.getMotor(EnumParam_Axis.X1).DoHome();
                        m_nStep = (int)WorkProcess1_HomeStep.Y轴回原;
                    }
                    break;
                case WorkProcess1_HomeStep.Y轴回原:
                    if (HardWareControl.getMotor(EnumParam_Axis.Z1).isIDLE() && HardWareControl.getMotor(EnumParam_Axis.Z2).isIDLE() && HardWareControl.getMotor(EnumParam_Axis.X1).isIDLE())
                    {
                        if ((HardWareControl.getInputIO(EnumParam_InputIO.入料龙门位夹爪气缸原点).GetValue()))
                        {
                            HardWareControl.getValve(EnumParam_Valve.入料龙门位取料夹爪气缸).Close();
                        }
                        else
                        {
                            HardWareControl.getValve(EnumParam_Valve.入料龙门位取料夹爪气缸).Open();
                        }
                        if ((HardWareControl.getInputIO(EnumParam_InputIO.出料龙门位夹爪气缸原点).GetValue()))
                        {
                            HardWareControl.getValve(EnumParam_Valve.出料龙门位夹爪气缸).Close();
                        }
                        else
                        {
                            HardWareControl.getValve(EnumParam_Valve.出料龙门位夹爪气缸).Open();
                        }
                        HardWareControl.getMotor(EnumParam_Axis.Y1).DoHome();
                        HardWareControl.getMotor(EnumParam_Axis.Y2).DoHome();
                        m_nStep = (int)WorkProcess1_HomeStep.Completed;
                    }
                    break;
                case WorkProcess1_HomeStep.Completed:
                    if (HardWareControl.getMotor(EnumParam_Axis.Y1).isIDLE() && HardWareControl.getMotor(EnumParam_Axis.Y2).isIDLE())
                    {
                        m_bHomeCompleted = true;
                        LogAuto.Notify("工位1" /*+ station.ToString()*/ + "复位完成！", (int)MachineStation.主监控, LogLevel.Info);
                        m_Status = 狀態.待命;
                    }
                    break;
            }
            base.HomeCycle(ref dbTime);
        }
        public override void StepCycle(ref double dbTime)
        {
            currentWorkStep = (WorkProcess1_WorkStep)m_nStep;
            switch (currentWorkStep)
            {
                case WorkProcess1_WorkStep.Start:
                    if (HardWareControl.getInputIO(EnumParam_InputIO.入料口载具感应信号).GetValue() || MachineDataDefine.machineState.b_UseNullRun)
                    {
                        ngTime = 0;
                        frm_Main.formData.ChartTime1.SetTime();//有料进来重置时间，90秒没有料进来，就置为idel
                        HIVE.HIVEInstance.HIVEStarttime[0] = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");
                        HIVE.HIVEInstance.hivestarttime[0] = DateTime.Now;
                        frm_Main.formData.ChartTime1.StartRun();//有料流入后再将状态设置为running 23-03-31
                        HardWareControl.getOutputIO(EnumParam_OutputIO.三色灯_绿灯).SetIO(true);
                        HardWareControl.getOutputIO(EnumParam_OutputIO.启动灯).SetIO(true);
                        HardWareControl.getOutputIO(EnumParam_OutputIO.A流道步进运行).SetIO(true);
                        HardWareControl.getOutputIO(EnumParam_OutputIO.B流道步进运行).SetIO(true);
                        m_nStep = (int)WorkProcess1_WorkStep.入料口位载具阻挡气缸回;
                    }
                    break;
                case WorkProcess1_WorkStep.入料口位载具阻挡气缸回:
                    HardWareControl.getValve(EnumParam_Valve.入料口位载具阻挡气缸).Close();//载具阻挡气缸,初始状态是伸出
                    m_nStep = (int)WorkProcess1_WorkStep.入料口位载具阻挡气缸原点信号;
                    break;
                case WorkProcess1_WorkStep.入料口位载具阻挡气缸原点信号:
                    if (HardWareControl.getValve(EnumParam_Valve.入料口位载具阻挡气缸).isIDLE())
                    {
                        m_nStep = (int)WorkProcess1_WorkStep.扫码枪扫码;
                    }
                    break;
                case WorkProcess1_WorkStep.扫码枪扫码:
                    if (HardWareControl.getInputIO(EnumParam_InputIO.扫码位到位信号).GetValue())
                    {
                        HardWareControl.getValve(EnumParam_Valve.入料口位载具阻挡气缸).Open();//载具阻挡气缸伸出
                        ConnectionControl.instance().getRS232Control(EnumParam_ConnectionName.扫码枪).strRecData = "";
                        ConnectionControl.instance().getRS232Control(EnumParam_ConnectionName.扫码枪).SetWriteDataBYTE();
                        timerDelay.Interval = 3000;
                        timerDelay.Start();
                        m_nStep = (int)WorkProcess1_WorkStep.获取到载具码;
                    }
                    break;
                case WorkProcess1_WorkStep.获取到载具码:
                    if (ConnectionControl.instance().getRS232Control(EnumParam_ConnectionName.扫码枪).strRecData != "")
                    {
                        MESDataDefine.holdSN = ConnectionControl.instance().getRS232Control(EnumParam_ConnectionName.扫码枪).strRecData.Trim();
                        Dictionary<string, string> dir = new Dictionary<string, string>();
                        dir.Add("工站", MESDataDefine.MESLXData.terminalName);
                        dir.Add("载具编号", MESDataDefine.holdSN);
                        Post.POSTClass.AddCMD(0, Post.CMDStep.UC获取SN, dir);
                        timerDelay.Enabled = false;
                        timerDelay.Interval = 1000;
                        timerDelay.Start();
                        m_nStep = (int)WorkProcess1_WorkStep.载具码获取产品SN;
                    }
                    else if (timerDelay.Enabled == false)
                    {
                        m_nStep = (int)WorkProcess1_WorkStep.扫码失败;
                    }
                    break;
                case WorkProcess1_WorkStep.扫码失败:
                    ngTime++;
                    if (ngTime > 3)
                    {
                        ngTime = 0;
                        Error pError = new Error(ref this.m_NowAddress, "扫码失败", "", (int)MErrorCode.左流道扫码枪1扫码超时);
                        pError.AddErrSloution("Retry", (int)WorkProcess1_WorkStep.扫码枪扫码);
                        pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    }
                    else
                    {
                        m_nStep = (int)WorkProcess1_WorkStep.扫码枪扫码;
                    }
                    break;
                case WorkProcess1_WorkStep.载具码获取产品SN:
                    if (Post.POSTClass.getResult(0, Post.CMDStep.UC获取SN).Result == "OK")
                    {
                        MESDataDefine.SN = Post.POSTClass.getResult(0, Post.CMDStep.UC获取SN).dataReturn["SN"];
                        Dictionary<string, string> dir1 = new Dictionary<string, string>();
                        dir1.Add("工站", MESDataDefine.MESLXData.terminalName);
                        dir1.Add("产品SN", MESDataDefine.SN);
                        POSTClass.AddCMD(0, CMDStep.检查UOP, dir1);
                        timerDelay.Enabled = false;
                        timerDelay.Interval = 1000;
                        timerDelay.Start();
                        m_nStep = (int)WorkProcess1_WorkStep.检查UOP;
                    }
                    else if (timerDelay.Enabled == false)
                    {
                        Error pError = new Error(ref this.m_NowAddress, "载具获取SN失败", "", (int)MErrorCode.载具获取产品SN失败);
                        pError.AddErrSloution("Retry", (int)WorkProcess1_WorkStep.扫码枪扫码);
                        pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    }
                    break;
                case WorkProcess1_WorkStep.检查UOP:
                    if (POSTClass.getResult(0, CMDStep.检查UOP).Result=="OK")
                    {
                        m_nStep = (int)WorkProcess1_WorkStep.流道阻挡气缸下;
                    }
                    else if(timerDelay.Enabled==false)
                    {
                        Error pError = new Error(ref this.m_NowAddress, "产品检查UOP失败", "", (int)MErrorCode.UOP检查异常);
                        pError.AddErrSloution("Retry", (int)WorkProcess1_WorkStep.载具码获取产品SN);
                        pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    }
                    break;
                case WorkProcess1_WorkStep.流道阻挡气缸下:
                    //if(!StaticParam.InputDictionary1["顶升位1感应信号"].GetValue())
                    //{
                    HardWareControl.getValve(EnumParam_Valve.流道阻挡气缸).Close();
                    m_nStep = (int)WorkProcess1_WorkStep.流道阻挡气缸原点信号;
                    //}
                    break;
                case WorkProcess1_WorkStep.流道阻挡气缸原点信号:
                    if (HardWareControl.getValve(EnumParam_Valve.流道阻挡气缸).isIDLE())
                    {
                        m_nStep = (int)WorkProcess1_WorkStep.入料龙门位载具顶升气缸上;
                    }
                    break;
                case WorkProcess1_WorkStep.入料龙门位载具顶升气缸上:
                    if (HardWareControl.getInputIO(EnumParam_InputIO.顶升位1感应信号).GetValue())
                    {
                        HardWareControl.getValve(EnumParam_Valve.流道阻挡气缸).Open();
                        HardWareControl.getValve(EnumParam_Valve.入料龙门位载具顶升气缸上).Open();//入料龙门位载具顶升气缸上
                        m_nStep = (int)WorkProcess1_WorkStep.前龙门移动到前取料位;
                    }
                    break;
                case WorkProcess1_WorkStep.前龙门移动到前取料位:
                    if (HardWareControl.getValve(EnumParam_Valve.入料龙门位载具顶升气缸上).isIDLE())//入料龙门位载具顶升气缸上到位
                    {
                        HardWareControl.movePoint(EnumParam_Point.前龙门取料位);
                        m_nStep = (int)WorkProcess1_WorkStep.入料龙门位取料夹爪气缸夹;
                    }
                    break;
                case WorkProcess1_WorkStep.入料龙门位取料夹爪气缸夹:
                    if (HardWareControl.getPointIdel(EnumParam_Point.前龙门取料位))
                    {
                        HardWareControl.getValve(EnumParam_Valve.入料龙门位取料夹爪气缸).Open();
                        m_nStep = (int)WorkProcess1_WorkStep.前龙门移动到放料位;
                    }
                    break;
                case WorkProcess1_WorkStep.前龙门移动到放料位:
                    if (HardWareControl.getValve(EnumParam_Valve.入料龙门位取料夹爪气缸).isIDLE())
                    {
                        HardWareControl.movePoint(EnumParam_Point.前龙门放料位);
                        m_nStep = (int)WorkProcess1_WorkStep.入料龙门位取料夹爪气缸开;
                    }
                    break;
                case WorkProcess1_WorkStep.入料龙门位取料夹爪气缸开:
                    if (HardWareControl.getPointIdel(EnumParam_Point.前龙门放料位))
                    {
                        HardWareControl.getValve(EnumParam_Valve.入料龙门位取料夹爪气缸).Close();
                        m_nStep = (int)WorkProcess1_WorkStep.前龙门移动到待命位;
                    }
                    break;
                case WorkProcess1_WorkStep.前龙门移动到待命位:
                    if (HardWareControl.getValve(EnumParam_Valve.入料龙门位取料夹爪气缸).isIDLE())
                    {
                        HardWareControl.movePoint(EnumParam_Point.前龙门待命位);
                        m_nStep = (int)WorkProcess1_WorkStep.横移X轴移动到拍照位;
                    }
                    break;
                case WorkProcess1_WorkStep.横移X轴移动到拍照位:
                    if (HardWareControl.getPointIdel(EnumParam_Point.前龙门待命位))
                    {
                        HardWareControl.movePoint(EnumParam_Point.横移X轴拍照位);
                        m_nStep = (int)WorkProcess1_WorkStep.调整模块开始工作;
                    }
                    break;
                case WorkProcess1_WorkStep.调整模块开始工作:
                    if (HardWareControl.getPointIdel(EnumParam_Point.横移X轴拍照位))
                    {
                        alignmentMode.Action();
                        m_nStep = (int)WorkProcess1_WorkStep.等待调整模块完成;
                    }
                    break;
                case WorkProcess1_WorkStep.等待调整模块完成:
                    if (alignmentMode.isIDLE())
                    {
                        m_nStep = (int)WorkProcess1_WorkStep.后龙门移动到后取料位;
                    }
                    break;
                case WorkProcess1_WorkStep.后龙门移动到后取料位:
                    HardWareControl.movePoint(EnumParam_Point.后龙门取料位);
                    m_nStep = (int)WorkProcess1_WorkStep.出料龙门位取料夹爪气缸夹;
                    break;
                case WorkProcess1_WorkStep.出料龙门位取料夹爪气缸夹:
                    if (HardWareControl.getPointIdel(EnumParam_Point.后龙门取料位))
                    {
                        HardWareControl.getValve(EnumParam_Valve.出料龙门位夹爪气缸).Open();
                        m_nStep = (int)WorkProcess1_WorkStep.后龙门移动到下料位;
                    }
                    break;
                case WorkProcess1_WorkStep.后龙门移动到下料位:
                    if (HardWareControl.getValve(EnumParam_Valve.出料龙门位夹爪气缸).isIDLE())
                    {
                        HardWareControl.movePoint(EnumParam_Point.后龙门放料位);
                        m_nStep = (int)WorkProcess1_WorkStep.出料龙门位夹爪气缸回;
                    }
                    break;
                case WorkProcess1_WorkStep.出料龙门位夹爪气缸回:
                    if (HardWareControl.getPointIdel(EnumParam_Point.后龙门放料位))
                    {
                        HardWareControl.getValve(EnumParam_Valve.出料龙门位夹爪气缸).Close();
                        m_nStep = (int)WorkProcess1_WorkStep.后龙门移动到待命位;
                    }
                    break;
                case WorkProcess1_WorkStep.后龙门移动到待命位:
                    if (HardWareControl.getValve(EnumParam_Valve.出料龙门位夹爪气缸).isIDLE())
                    {
                        HardWareControl.movePoint(EnumParam_Point.后龙门待命位);
                        m_nStep = (int)WorkProcess1_WorkStep.出料口位前顶升气缸下;
                    }
                    break;
                case WorkProcess1_WorkStep.出料口位前顶升气缸下:
                    //测试反馈结果
                    if (HardWareControl.getPointIdel(EnumParam_Point.后龙门待命位))
                    {
                        HardWareControl.getValve(EnumParam_Valve.出料口位前顶升气缸上).Close();
                        m_nStep = (int)WorkProcess1_WorkStep.出料口位阻挡气缸回;
                    }
                    break;
                case WorkProcess1_WorkStep.出料口位阻挡气缸回:
                    if (HardWareControl.getInputIO(EnumParam_InputIO.出料口位后到位信号).GetValue())
                    {
                        HardWareControl.getValve(EnumParam_Valve.出料口位阻挡气缸).Close();
                        m_nStep = (int)WorkProcess1_WorkStep.出料口位后顶升气缸上;
                    }
                    break;
                case WorkProcess1_WorkStep.出料口位后顶升气缸上:
                    if (HardWareControl.getInputIO(EnumParam_InputIO.出料口位后到位信号).GetValue())//入料龙门位载具顶升气缸上到位
                    {
                        HardWareControl.getValve(EnumParam_Valve.出料口位阻挡气缸).Open();
                        HardWareControl.getValve(EnumParam_Valve.出料口位后顶升气缸上).Open();
                        HardWareControl.getValve(EnumParam_Valve.出料口位前顶升气缸上).Open();
                        m_nStep = (int)WorkProcess1_WorkStep.机械手取料;
                    }
                    break;
                case WorkProcess1_WorkStep.机械手取料:
                    if (HardWareControl.getValve(EnumParam_Valve.出料口位后顶升气缸上).isIDLE())
                    {
                        //  StaticParam.OutputDictionary["机械手可取料信号"].SetIO(true);
                        // StaticParam.cylinderDictionary["出料口位后顶升气缸上"].Close();//出料口位阻挡气缸出
                        m_nStep = (int)WorkProcess1_WorkStep.机械手准备取料;
                    }
                    break;

                case WorkProcess1_WorkStep.机械手准备取料:
                    //测试反馈结果
                    //if (StaticParam.InputDictionary1["机械手准备取料"].GetValue())//入料龙门位载具顶升气缸上到位
                    //{
                    //StaticParam.OutputDictionary["机械手可取料信号"].SetIO(false);
                    Thread.Sleep(1000);
                    // StaticParam.cylinderDictionary["出料口位后顶升气缸上"].Close();//出料口位阻挡气缸出
                    m_nStep = (int)WorkProcess1_WorkStep.机械手取料完成;
                    // }
                    break;
                case WorkProcess1_WorkStep.机械手取料完成:
                    //测试反馈结果
                    //if (StaticParam.InputDictionary1["机械手取料完成信息"].GetValue())//入料龙门位载具顶升气缸上到位
                    //{
                    Thread.Sleep(1000);
                    HardWareControl.getValve(EnumParam_Valve.出料口位后顶升气缸上).Close();//出料口位后顶升气缸上
                    m_nStep = (int)WorkProcess1_WorkStep.Completed;
                    // }
                    break;
                case WorkProcess1_WorkStep.Completed:
                    HIVE.HIVEInstance.lastStoptime[0] = HIVE.HIVEInstance.hivestoptime[0];
                    HIVE.HIVEInstance.hivestoptime[0] = DateTime.Now;
                    HIVE.HIVEInstance.HIVEStoptime[0] = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");
                    LogAuto.Notify("工位" + station.ToString() + "作业完成！", (int)MachineStation.主监控, LogLevel.Info);
                    //StaticParam.OutputDictionary["启动3/4按钮灯-左机"].SetIO(false);
                    result = "1";
                    //  m_Status = 狀態.待命;
                    //result为1当前工站OK，为0当前工站NG，为999 当前工站没有做完
                    if (MachineDataDefine.machineState.b_UseRemoteQualification)
                    {
                        string SN = "TEST" + DateTime.Now.ToString("HHmmss");
                        Dictionary<string, string> dataDir = new Dictionary<string, string>();
                        dataDir.Add("产品SN", SN);
                        dataDir.Add("结果", "true");
                        dataDir.Add("开始时间", HIVE.HIVEInstance.HIVEStarttime[0]);
                        dataDir.Add("结束时间", HIVE.HIVEInstance.HIVEStoptime[0]);
                        dataDir.Add("软件版本", MESDataDefine.MESLXData.SW_Version);
                        dataDir.Add("极限版本", MESDataDefine.MESLXData.limits_version);
                        dataDir.Add("段差值1", "0");
                        dataDir.Add("段差值2", "0");
                        dataDir.Add("段差值3", "0");
                        dataDir.Add("段差值4", "0");
                        dataDir.Add("段差值5", "0");
                        dataDir.Add("段差值6", "0");
                        dataDir.Add("段差值7", "0");
                        dataDir.Add("段差值8", "0");
                        dataDir.Add("角度", "0");
                        dataDir.Add("压缩包名称", DateTime.Now.ToString("HHmmss") + "_" + SN + ".zip");
                        HIVE.HIVEInstance.HiveSendMACHINEDATA(SN, "", 0, "", "", dataDir);
                    }

                    //如果料做完了
                    frm_Main.formData.Chartcapacity1.AddOkLeft();
                    frm_Main.formData.CTUnit1.EndDoLeft("");//CT统计结束
                    MESDataDefine.CT = frm_Main.formData.CTUnit1.CTEventArgs_Left.Ct; //实时CT
                    //给MES上传测试结果
                    string offsetX = "0.000";
                    string offsetY = "0.000";
                    string offsetR = "0.000";
                    string p1 = "0";
                    string p2 = "0";
                    string p3 = "0";
                    string p4 = "0";
                    string p5 = "0";
                    string p6 = "0";
                    string p7 = "0";
                    string p8 = "0";
                    string shiftP1P5 = "0";
                    string shiftP2P6 = "0";
                    string shiftP3P7 = "0";
                    string shiftP4P8 = "0";

                    Dictionary<string, string> dataDirMES = new Dictionary<string, string>();
                    dataDirMES.Add("结果", "PASS");
                    dataDirMES.Add("产品SN", MESDataDefine.SN);
                    dataDirMES.Add("开始时间", HIVE.HIVEInstance.hivestarttime[0].ToString());
                    dataDirMES.Add("结束时间", HIVE.HIVEInstance.hivestoptime[0].ToString());
                    dataDirMES.Add("软件版本", MESDataDefine.MESLXData.SW_Version);
                    dataDirMES.Add("工站", MESDataDefine.MESLXData.terminalName);//ITKS_E03 - 4FT - 01A_1_STATION130
                    dataDirMES.Add("载具号", MESDataDefine.holdSN);//KSH36XNP1RH850XXCFX100659
                    dataDirMES.Add("OffsetX值", offsetX);
                    dataDirMES.Add("OffsetY值", offsetY);
                    dataDirMES.Add("OffsetR值", offsetR);

                    dataDirMES.Add("P1值", p1);
                    dataDirMES.Add("P2值", p2);
                    dataDirMES.Add("P3值", p3);
                    dataDirMES.Add("P4值", p4);
                    dataDirMES.Add("P5值", p5);
                    dataDirMES.Add("P6值", p6);
                    dataDirMES.Add("P7值", p7);
                    dataDirMES.Add("P8值", p8);

                    dataDirMES.Add("ShiftP1P5值", shiftP1P5);
                    dataDirMES.Add("ShiftP2P6值", shiftP2P6);
                    dataDirMES.Add("ShiftP3P7值", shiftP3P7);
                    dataDirMES.Add("ShiftP4P8值", shiftP4P8);
                    //   Post.POSTClass.AddCMD(0, Post.CMDStep.上传数据, dataDirMES);

                    //提交过站
                    Dictionary<string, string> dataDirMES1 = new Dictionary<string, string>();

                    dataDirMES1.Add("工站", MESDataDefine.MESLXData.terminalName);
                    dataDirMES1.Add("产品SN", MESDataDefine.SN);
                    //  Post.POSTClass.AddCMD(0, Post.CMDStep.提交过站, dataDirMES1);




                    m_nStep = (int)WorkProcess1_WorkStep.Start;
                    break;
            }
            base.StepCycle(ref dbTime);
        }
        public override bool DoHomeStep(int iHomeStep)
        {
            m_Status = 狀態.回HOME中;
            m_nStep = (int)WorkProcess1_HomeStep.Start;
            return base.DoHomeStep(iHomeStep);
        }
        public override bool DoStep(int iStep)
        {
            m_Status = 狀態.動作中;
            m_nStep = (int)WorkProcess1_WorkStep.Start;
            return base.DoStep(iStep);
        }
        public override void Stop()
        {
            m_Status = 狀態.待命;
            base.Stop();
        }
    }
}
