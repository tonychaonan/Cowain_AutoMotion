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
    public class RunnerIn : Base
    {
        public RunnerIn_HomeStep currentHomeStep;
        private RunnerIn_WorkStep currentWorkStep;
        int ngTime = 0;
        public static string SN = "";

        /// <summary>
        /// 前龙门可放料
        /// </summary>
        public static bool PutProduct = false;
        //public static bool 扫码失败=false;
        //public static bool 检查UOP失败=false;
        //public static bool 获取产品SN失败=false;
        //public static bool 检查产品SN类型失败=false;

        public RunnerIn(Type homeEnum1, Type stepEnum1, string instanceName1, Base parent) : base( homeEnum1,  stepEnum1,  instanceName1, parent, false)
        {

        }
        public enum RunnerIn_HomeStep
        {
            Start = 0,
            气缸回原点,
            等待气缸回原点完成,
            Completed
        }
        public enum RunnerIn_WorkStep
        {
            Start = 0,
            判断有无料,
            延时结束,
            入料口位载具阻挡气缸缩回,
            等待载具到扫码位,
            入料口位载具阻挡气缸,
            扫码枪扫码,
            等待收到产品码,
            扫码成功,
            作料前发送指令,
            作料前发送指令接收,
            扫码失败,
            载具往拍照位移动,
            流道正转,
            对射感应有料,
            作料位顶升有料感应,
            作料位顶升气缸上,
            作料位顶升气缸上到位,
            给前龙门可放料信号,
            前龙门做料完成信号,
            进料流道气缸下,
            下料感应有料,
            下料感应无料,
            下料结束,
            气缸到位,

            载具码获取产品SN,
            获取产品SN类型,
            判断产品SN类型,
            检查UOP,
            产品在当站,
            流道阻挡气缸下,
            等待载具到达顶升气缸位置,
            入料口位载具阻挡气缸出,
            入料龙门位载具顶升气缸上,
            等待把料取走,
            NG抛料,
            ThreeAndFive,
            连三不连五变量,
            等待抛料完成,
            Completed
        }
        public override void HomeCycle(ref double dbTime)
        {
            currentHomeStep = (RunnerIn_HomeStep)m_nHomeStep;
            switch (currentHomeStep)
            {
                case RunnerIn_HomeStep.Start:

                    RunnerIn.SN = "";
                    PutProduct = false;
                    ngTime = 0;
                    m_bHomeCompleted = false;
                    m_nHomeStep = (int)RunnerIn_HomeStep.气缸回原点;
                    break;
                case RunnerIn_HomeStep.气缸回原点:
                    LogAuto.Notify("流道进料工位开始复位！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    HardWareControl.getValve(EnumParam_Valve.进料阻挡气缸).Open();
                    HardWareControl.getValve(EnumParam_Valve.进料顶升气缸).Open();
                    HardWareControl.getValve(EnumParam_Valve.作料位阻挡气缸).Open();
                    HardWareControl.getValve(EnumParam_Valve.作料位顶升气缸).Close();
                    HardWareControl.getValve(EnumParam_Valve.出料位阻挡气缸).Open();
                    HardWareControl.getValve(EnumParam_Valve.出料位顶升气缸).Close();
                    m_nHomeStep = (int)RunnerIn_HomeStep.等待气缸回原点完成;
                    break;
                case RunnerIn_HomeStep.等待气缸回原点完成:
                    if (HardWareControl.getValve(EnumParam_Valve.进料阻挡气缸).isIDLE()
                        && HardWareControl.getValve(EnumParam_Valve.进料顶升气缸).isIDLE()
                        && HardWareControl.getValve(EnumParam_Valve.作料位阻挡气缸).isIDLE()
                        && HardWareControl.getValve(EnumParam_Valve.作料位顶升气缸).isIDLE())
                    {
                        LogAuto.Notify("流道进料气缸复位到位！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        m_nHomeStep = (int)RunnerIn_HomeStep.Completed;
                    }
                    break;
                case RunnerIn_HomeStep.Completed:
                    m_bHomeCompleted = true;
                    LogAuto.Notify("流道进料工位复位完成！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    m_Status = 狀態.待命;
                    break;
            }
        }
        public override void StepCycle(ref double dbTime)
        {
            currentWorkStep = (RunnerIn_WorkStep)m_nStep;
            switch (currentWorkStep)
            {
                case RunnerIn_WorkStep.Start:
                    {
                        LogAuto.Notify("判断有无料！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        m_nStep = (int)RunnerIn_WorkStep.判断有无料;
                    }
                    break;
                case RunnerIn_WorkStep.判断有无料:
                    if (HardWareControl.getInputIO(EnumParam_InputIO.进料顶升有料感应).GetValue() && HardWareControl.getInputIO(EnumParam_InputIO.进料位有料感应).GetValue())
                    {
                        LogAuto.Notify("触发扫码枪扫码延时！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        timerDelay.Enabled = false;
                        timerDelay.Interval = 500;
                        timerDelay.Start();
                        m_nStep = (int)RunnerIn_WorkStep.延时结束;
                    }
                    break;
                case RunnerIn_WorkStep.延时结束:
                    if (timerDelay.Enabled == false)
                    {
                        m_nStep = (int)RunnerIn_WorkStep.扫码枪扫码;
                    }
                    break;
                case RunnerIn_WorkStep.扫码枪扫码:
                    if (MachineDataDefine.machineState.b_UseScaner != true)
                    {
                        LogAuto.Notify("不启用扫码枪！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        RunnerIn.SN = "TEST" + DateTime.Now.ToString("HHmmss") + "12345678";
                        m_nStep = (int)RunnerIn_WorkStep.载具往拍照位移动;
                        break;
                    }
                    if (MachineDataDefine.machineState.b_UseScaner)
                    {
                        LogAuto.Notify("启用扫码枪！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.getRS232Control(EnumParam_ConnectionName.扫码枪).strRecData = "";
                        HardWareControl.getRS232Control(EnumParam_ConnectionName.扫码枪).SetWriteDataBYTE();
                        timerDelay.Interval = 3000;
                        timerDelay.Start();
                        m_nStep = (int)RunnerIn_WorkStep.等待收到产品码;
                        break;
                    }
                    break;
                case RunnerIn_WorkStep.等待收到产品码:
                    if (HardWareControl.getRS232Control(EnumParam_ConnectionName.扫码枪).strRecData != "")
                    {
                        LogAuto.Notify("扫码枪成功！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        m_nStep = (int)RunnerIn_WorkStep.扫码成功;
                    }
                    else if (timerDelay.Enabled == false)
                    {
                        LogAuto.Notify("扫码枪失败！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        m_nStep = (int)RunnerIn_WorkStep.扫码失败;
                    }
                    break;
                case RunnerIn_WorkStep.扫码成功:
                    RunnerIn.SN = HardWareControl.getRS232Control(EnumParam_ConnectionName.扫码枪).strRecData.Trim();
                    m_nStep = (int)RunnerIn_WorkStep.载具往拍照位移动;
                    break;
                case RunnerIn_WorkStep.扫码失败:
                    // MachineDataDefine.MachineControlS.gross_COF_result = false;
                    ngTime++;
                    if (ngTime > 3)
                    {
                        ngTime = 0;
                        //RunnerIn.扫码失败 = true;
                        LogAuto.SaveNGData("扫码失败！");
                        LogAuto.Notify("扫码失败！", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                        Error pError = new Error(ref this.m_NowAddress, "扫码失败", "", (int)MErrorCode.右流道扫码枪1扫码超时);
                        pError.AddErrSloution("Retry", (int)RunnerIn_WorkStep.扫码枪扫码);
                        pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    }
                    else
                    {
                        LogAuto.Notify("重新扫码！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        m_nStep = (int)RunnerIn_WorkStep.扫码枪扫码;
                    }
                    break;

                case RunnerIn_WorkStep.作料前发送指令:
                    LogAuto.Notify("作料前发送指令,创建存储图像的文件夹！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr = "";
                    LogAuto.Notify("给相机发送拍照指令！" + "T3,1" + "," + SN, (int)MachineStation.主监控, MotionLogLevel.Info);
                    HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).SendMSG("T3,1" + "," + SN);
                    timerDelay.Enabled = false;
                    timerDelay.Interval = 10000;
                    timerDelay.Start();
                    m_nStep = (int)RunnerIn_WorkStep.作料前发送指令接收;
                    break;
                case RunnerIn_WorkStep.作料前发送指令接收:
                    if (HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr != "")
                    {
                        string ccdResult = HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr;
                        LogAuto.Notify("作料前发送指令相机返回结果！" + ccdResult, (int)MachineStation.主监控, MotionLogLevel.Info);
                        string[] ccd = ccdResult.Split(',');

                        if (ccd[2] == "1")//结果ok
                        {
                            LogAuto.Notify("作料前发送指令相机返回OK！" + ccdResult, (int)MachineStation.主监控, MotionLogLevel.Info);

                            m_nStep = (int)RunnerIn_WorkStep.载具往拍照位移动;

                        }
                        else
                        {
                            AxisTakeIn.b_Result = false;
                            LogAuto.Notify("作料前发送指令相机返回NG！" + ccdResult, (int)MachineStation.主监控, MotionLogLevel.Alarm);
                            Error pError = new Error(ref this.m_NowAddress, "作料前发送指令相机返回NG", "", (int)MErrorCode.CCD_Capture1異常);
                            pError.AddErrSloution("Retry", (int)RunnerIn_WorkStep.作料前发送指令);
                            pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                        }

                    }

                    else if (timerDelay.Enabled == false)
                    {
                        AxisTakeIn.b_Result = false;
                        LogAuto.Notify("作料前发送指令CCD未返回数据！", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                        Error pError = new Error(ref this.m_NowAddress, "作料前发送指令CCD未返回数据", "", (int)MErrorCode.CCD_Capture1異常);
                        pError.AddErrSloution("Retry", (int)RunnerIn_WorkStep.作料前发送指令);
                        pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    }

                    break;
                case RunnerIn_WorkStep.载具往拍照位移动:
                    LogAuto.Notify("进料顶升气缸下&&进料阻挡气缸下！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    HardWareControl.getValve(EnumParam_Valve.进料顶升气缸).Close();
                    HardWareControl.getValve(EnumParam_Valve.进料阻挡气缸).Close();
                    m_nStep = (int)RunnerIn_WorkStep.流道正转;
                    break;
                case RunnerIn_WorkStep.流道正转:
                    if (HardWareControl.getValve(EnumParam_Valve.进料顶升气缸).isIDLE() && HardWareControl.getValve(EnumParam_Valve.进料阻挡气缸).isIDLE())
                    {
                        LogAuto.Notify("启动流道！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        // HardWareControl.getOutputIO(EnumParam_OutputIO.流道步进方向).SetIO(false);//反转关
                        HardWareControl.getOutputIO(EnumParam_OutputIO.流道步进启动).SetIO(true);//正转关
                       
                        m_nStep = (int)RunnerIn_WorkStep.对射感应有料;
                    }
                    break;
                case RunnerIn_WorkStep.对射感应有料:
                    if (HardWareControl.getInputIO(EnumParam_InputIO.进料对射感应).GetValue())
                    {
                            LogAuto.Notify("进料对射感应触发！", (int)MachineStation.主监控, MotionLogLevel.Info);
                            HardWareControl.getValve(EnumParam_Valve.进料顶升气缸).Open();
                            HardWareControl.getValve(EnumParam_Valve.进料阻挡气缸).Open();
                            m_nStep = (int)RunnerIn_WorkStep.作料位顶升有料感应;                       
                    }                  
                    break;

                case RunnerIn_WorkStep.作料位顶升有料感应:
                    if (HardWareControl.getInputIO(EnumParam_InputIO.进料顶升有料感应).GetValue())
                    {
                        LogAuto.Notify("进料顶升有料感应延时！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        timerDelay.Enabled = false;
                        timerDelay.Interval = 500;
                        timerDelay.Start();
                        m_nStep = (int)RunnerIn_WorkStep.作料位顶升气缸上;
                    }
                    break;

                case RunnerIn_WorkStep.作料位顶升气缸上:
                    if (timerDelay.Enabled == false)
                    {
                        LogAuto.Notify("作料位顶升气缸上！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.getValve(EnumParam_Valve.作料位顶升气缸).Open();
                        timerDelay.Enabled = false;
                        timerDelay.Interval = 500;
                        timerDelay.Start();
                        m_nStep = (int)RunnerIn_WorkStep.作料位顶升气缸上到位;
                    }
                    break;
                case RunnerIn_WorkStep.作料位顶升气缸上到位:
                    if (HardWareControl.getValve(EnumParam_Valve.进料顶升气缸).isIDLE())
                    {
                        LogAuto.Notify("作料位顶升气缸到位延时！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        //HardWareControl.getValve(EnumParam_Valve.作料位顶升气缸).Open();
                        timerDelay.Enabled = false;
                        timerDelay.Interval = 500;
                        timerDelay.Start();
                        m_nStep = (int)RunnerIn_WorkStep.给前龙门可放料信号;
                    }
                    break;
                case RunnerIn_WorkStep.给前龙门可放料信号:
                    if (timerDelay.Enabled == false)
                    {
                        LogAuto.Notify("给前龙门可放料信号！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        PutProduct = true;
                        m_nStep = (int)RunnerIn_WorkStep.前龙门做料完成信号;
                    }
                    break;
                case RunnerIn_WorkStep.前龙门做料完成信号:
                    if (PutProduct == false)  //putPutProduct==fasle时证明前龙门已做料完成
                    {
                        LogAuto.Notify("前龙门放料完成，流道反转！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.getOutputIO(EnumParam_OutputIO.流道步进方向).SetIO(true);//反转关
                        HardWareControl.getOutputIO(EnumParam_OutputIO.流道步进启动).SetIO(false);//正转关
                        m_nStep = (int)RunnerIn_WorkStep.进料流道气缸下;
                    }
                    break;
                case RunnerIn_WorkStep.进料流道气缸下:
                    LogAuto.Notify("流道反转,气缸下降！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    HardWareControl.getValve(EnumParam_Valve.作料位顶升气缸).Close();
                    HardWareControl.getValve(EnumParam_Valve.进料阻挡气缸).Close();
                    HardWareControl.getValve(EnumParam_Valve.进料顶升气缸).Close();
                    m_nStep = (int)RunnerIn_WorkStep.下料感应有料;

                    break;

                case RunnerIn_WorkStep.下料感应有料:
                    if (HardWareControl.getInputIO(EnumParam_InputIO.进料位有料感应).GetValue())
                    {
                        LogAuto.Notify("进料位有料感应有信号！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        m_nStep = (int)RunnerIn_WorkStep.下料感应无料;
                    }
                    break;
                case RunnerIn_WorkStep.下料感应无料:
                    if (!HardWareControl.getInputIO(EnumParam_InputIO.进料位有料感应).GetValue())
                    {
                        LogAuto.Notify("进料位有料感应无信号！！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        m_nStep = (int)RunnerIn_WorkStep.下料结束;
                    }
                    break;
                case RunnerIn_WorkStep.下料结束:
                    LogAuto.Notify("人工取料结束！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    HardWareControl.getValve(EnumParam_Valve.进料阻挡气缸).Open();
                    HardWareControl.getValve(EnumParam_Valve.进料顶升气缸).Open();
                    m_nStep = (int)RunnerIn_WorkStep.气缸到位;

                    break;
                case RunnerIn_WorkStep.气缸到位:
                    if (HardWareControl.getValve(EnumParam_Valve.进料顶升气缸).isIDLE() && HardWareControl.getValve(EnumParam_Valve.进料阻挡气缸).isIDLE())
                    {
                        LogAuto.Notify("气缸到位，流程结束！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        m_nStep = (int)RunnerIn_WorkStep.Start;
                    }
                    break;
                    #region  旧禁用
                    //case RunnerIn_WorkStep.载具码获取产品SN:
                    //    LogAuto.Notify("载具码获取产品SN！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    //    Dictionary<string, string> dir = new Dictionary<string, string>();
                    //    dir.Add("工站", MESDataDefine.MESLXData.terminalName);
                    //    dir.Add("载具编号", RunnerIn.holderSN);
                    //    POSTClass.AddCMD(0, Post.CMDStep.UC获取SN, dir);
                    //    timerDelay.Enabled = false;
                    //    timerDelay.Interval = 3000;
                    //    timerDelay.Start();
                    //    if (MachineDataDefine.b_UseLAD)
                    //    {
                    //        LogAuto.Notify("启用LAD模式！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    //        if (MachineDataDefine.LADModel == 2)
                    //        {
                    //            LogAuto.Notify("LAD模式下GRR不检查Uop！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    //           // m_nStep = (int)RunnerIn_WorkStep.流道阻挡气缸下;
                    //            m_nStep = (int)RunnerIn_WorkStep.获取产品SN类型;
                    //        }
                    //        else
                    //        {
                    //            LogAuto.Notify("LAD模式下CPK/SCS检查Uop！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    //            m_nStep = (int)RunnerIn_WorkStep.获取产品SN类型;
                    //        }
                    //        break;
                    //    }
                    //    m_nStep = (int)RunnerIn_WorkStep.获取产品SN类型;
                    //    break;


                    //case RunnerIn_WorkStep.获取产品SN类型:
                    //    if (POSTClass.getResult(0, CMDStep.UC获取SN).Result == "OK")
                    //    {                
                    //        LogAuto.Notify("UC获取SN OK！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    //         RunnerIn.SN = Post.POSTClass.getResult(0, Post.CMDStep.UC获取SN).dataReturn["SN"];
                    //        if (MachineDataDefine.b_UseLAD && MachineDataDefine.LADModel == 2)
                    //        {
                    //            m_nStep = (int)RunnerIn_WorkStep.流道阻挡气缸下;
                    //        }
                    //        else
                    //        {
                    //            //Dictionary<string, string> dir1 = new Dictionary<string, string>();
                    //            //dir1.Add("工站", MESDataDefine.MESLXData.terminalName);
                    //            //dir1.Add("产品SN", RunnerIn.SN);
                    //            //POSTClass.AddCMD(0, CMDStep.检查SN类型, dir1);
                    //            //timerDelay.Enabled = false;
                    //            //timerDelay.Interval = 4000;
                    //            //timerDelay.Start();
                    //            m_nStep = (int)RunnerIn_WorkStep.判断产品SN类型;
                    //        }
                    //    }
                    //    else if (timerDelay.Enabled == false)
                    //    {         
                    //        LogAuto.Notify("UC获取SN OK！反馈超时！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    //        string str = "" + "," + RunnerIn.holderSN + "," + "UC获取SN OK！反馈超时";//MachineDataDefine.str;
                    //        LogAuto.SaveNGData(str);
                    //        Alignment.Mes反馈超时 = true;
                    //        m_nStep = (int)RunnerIn_WorkStep.NG抛料;
                    //        //Error pError = new Error(ref this.m_NowAddress, "UC获取SN NG！反馈超时", "", (int)MErrorCode.载具获取产品SN失败);
                    //        //pError.AddErrSloution("Retry", (int)RunnerIn_WorkStep.扫码枪扫码);
                    //        //pError.AddErrSloution("NG抛料", (int)RunnerIn_WorkStep.NG抛料);
                    //        //pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    //        // m_nStep = (int)RunnerIn_WorkStep.NG抛料;
                    //    }
                    //    else if (POSTClass.getResult(0, CMDStep.UC获取SN).Result == "NG")
                    //    {                                            
                    //        LogAuto.Notify("UC获取SN OK！反馈NG！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    //        string str = "" + "," + RunnerIn.holderSN + "," + MachineDataDefine.str;
                    //        RunnerIn.获取产品SN失败 = true;
                    //        LogAuto.SaveNGData(str);
                    //        m_nStep = (int)RunnerIn_WorkStep.NG抛料;
                    //        //Error pError = new Error(ref this.m_NowAddress, MachineDataDefine.str, "", (int)MErrorCode.载具获取产品SN失败);
                    //        //pError.AddErrSloution("Retry", (int)RunnerIn_WorkStep.扫码枪扫码);
                    //        //pError.AddErrSloution("NG抛料", (int)RunnerIn_WorkStep.NG抛料);
                    //        //pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    //        // m_nStep = (int)RunnerIn_WorkStep.NG抛料;
                    //    }
                    //    break;

                    //case RunnerIn_WorkStep.判断产品SN类型:
                    //    //if (POSTClass.getResult(0, CMDStep.检查SN类型).Result == "OK")
                    //    if (true)
                    //    {
                    //        //LogAuto.Notify("获取SN类型 OK！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    //        Dictionary<string, string> dir1 = new Dictionary<string, string>();
                    //        dir1.Add("工站", MESDataDefine.MESLXData.terminalName);
                    //        dir1.Add("产品SN", RunnerIn.SN);
                    //        POSTClass.AddCMD(0, CMDStep.检查UOP, dir1);
                    //        timerDelay.Enabled = false;
                    //        timerDelay.Interval = 4000;
                    //        timerDelay.Start();
                    //        m_nStep = (int)RunnerIn_WorkStep.产品在当站;
                    //    }
                    //    else if (timerDelay.Enabled == false)
                    //    {                      
                    //        LogAuto.Notify("获取SN类型！反馈超时！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    //        string str = "" + "," + RunnerIn.holderSN + "," + "获取SN类型！反馈超时";//MachineDataDefine.str;
                    //        LogAuto.SaveNGData(str);
                    //        Alignment.Mes反馈超时 = true;
                    //        m_nStep = (int)RunnerIn_WorkStep.NG抛料;
                    //        //Error pError = new Error(ref this.m_NowAddress, "获取SN类型！反馈超时", "", (int)MErrorCode.载具获取产品SN类型失败);
                    //        //pError.AddErrSloution("Retry", (int)RunnerIn_WorkStep.扫码枪扫码);
                    //        //pError.AddErrSloution("NG抛料", (int)RunnerIn_WorkStep.NG抛料);
                    //        //pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    //        // m_nStep = (int)RunnerIn_WorkStep.NG抛料;
                    //    }
                    //    else if (POSTClass.getResult(0, CMDStep.检查SN类型).Result == "NG")
                    //    {
                    //        LogAuto.Notify("获取SN类型！反馈NG！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    //        string str = "" + "," + RunnerIn.holderSN + "," + MachineDataDefine.str;
                    //        RunnerIn.检查产品SN类型失败 = true;
                    //        LogAuto.SaveNGData(str);
                    //        m_nStep = (int)RunnerIn_WorkStep.NG抛料;
                    //        //Error pError = new Error(ref this.m_NowAddress, MachineDataDefine.str, "", (int)MErrorCode.载具获取产品SN类型失败);
                    //        //pError.AddErrSloution("Retry", (int)RunnerIn_WorkStep.扫码枪扫码);
                    //        //pError.AddErrSloution("NG抛料", (int)RunnerIn_WorkStep.NG抛料);
                    //        //pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    //        // m_nStep = (int)RunnerIn_WorkStep.NG抛料;
                    //    }
                    //    break;
                    //#region
                    ////case RunnerIn_WorkStep.检查UOP:
                    ////    if (POSTClass.getResult(0, CMDStep.UC获取SN).Result == "OK")
                    ////    {
                    ////      //  RunnerIn.b_OutProduct = false;
                    ////        //  Alignment.NG下料 = false;
                    ////        LogAuto.Notify("UC获取SN OK！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    ////        // MESDataDefine.SN = Post.POSTClass.getResult(0, Post.CMDStep.UC获取SN).dataReturn["SN"];
                    ////        RunnerIn.SN = Post.POSTClass.getResult(0, Post.CMDStep.UC获取SN).dataReturn["SN"];
                    ////        //给时间赋值
                    ////        //TimeManages.addTimeManage(RunnerIn.SN);
                    ////        //TimeManages.getTimeManage(RunnerIn.SN).productInstartTime.refreshTime(HIVE.HIVEInstance.hivestarttime[0]);
                    ////        //TimeManages.getTimeManage(RunnerIn.SN).holderSN = RunnerIn.holderSN;
                    ////        Dictionary<string, string> dir1 = new Dictionary<string, string>();
                    ////        //TimeManages.getTimeManage(RunnerIn.SN).alignmentStartTime.refreshTime();
                    ////        dir1.Add("工站", MESDataDefine.MESLXData.terminalName);
                    ////        dir1.Add("产品SN", RunnerIn.SN);
                    ////        POSTClass.AddCMD(0, CMDStep.检查UOP, dir1);
                    ////        timerDelay.Enabled = false;
                    ////        timerDelay.Interval = 3000;
                    ////        timerDelay.Start();
                    ////        m_nStep = (int)RunnerIn_WorkStep.产品在当站;
                    ////    }
                    ////    else if (timerDelay.Enabled == false)
                    ////    {
                    ////        //if (RunnerIn.b_OutProduct)
                    ////        //{
                    ////        //    RunnerIn.b_OutProduct = false;
                    ////        //}
                    ////        //    Error pError = new Error(ref this.m_NowAddress, "载具获取SN失败", "", (int)MErrorCode.载具获取产品SN失败);
                    ////        // Error pError = new Error(ref this.m_NowAddress, POSTClass.getResult(0, CMDStep.UC获取SN).Result, "", (int)MErrorCode.载具获取产品SN失败);
                    ////        string str = "" + "," + RunnerIn.holderSN + "," + MachineDataDefine.str;
                    ////        LogAuto.SaveNGData(str);
                    ////        Error pError = new Error(ref this.m_NowAddress, "UC获取SN OK！反馈超时", "", (int)MErrorCode.载具获取产品SN失败);
                    ////        pError.AddErrSloution("Retry", (int)RunnerIn_WorkStep.扫码枪扫码);
                    ////        pError.AddErrSloution("NG抛料", (int)RunnerIn_WorkStep.NG抛料);
                    ////        pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    ////       // m_nStep = (int)RunnerIn_WorkStep.NG抛料;
                    ////    }
                    ////    else if (POSTClass.getResult(0, CMDStep.UC获取SN).Result == "NG")
                    ////    {
                    ////        //if (RunnerIn.b_OutProduct)
                    ////        //{
                    ////        //    RunnerIn.b_OutProduct = false;
                    ////        //}
                    ////        //    Error pError = new Error(ref this.m_NowAddress, "载具获取SN失败", "", (int)MErrorCode.载具获取产品SN失败);
                    ////        // Error pError = new Error(ref this.m_NowAddress, POSTClass.getResult(0, CMDStep.UC获取SN).Result, "", (int)MErrorCode.载具获取产品SN失败);
                    ////        string str = "" + "," + RunnerIn.holderSN + "," + MachineDataDefine.str;
                    ////        LogAuto.SaveNGData(str);
                    ////        Error pError = new Error(ref this.m_NowAddress, MachineDataDefine.str, "", (int)MErrorCode.载具获取产品SN失败);
                    ////        pError.AddErrSloution("Retry", (int)RunnerIn_WorkStep.扫码枪扫码);
                    ////        pError.AddErrSloution("NG抛料", (int)RunnerIn_WorkStep.NG抛料);
                    ////        pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    ////        // m_nStep = (int)RunnerIn_WorkStep.NG抛料;
                    ////    }
                    ////    break;
                    //#endregion
                    //case RunnerIn_WorkStep.产品在当站:
                    //    if (POSTClass.getResult(0, CMDStep.检查UOP).Result == "OK")
                    //    {
                    //       // RunnerIn.b_OutProduct = false;
                    //        // Alignment.NG下料 = false;
                    //        LogAuto.Notify("检查UOP OK！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    //        m_nStep = (int)RunnerIn_WorkStep.流道阻挡气缸下;
                    //    }
                    //    else if (timerDelay.Enabled == false)
                    //    {                      
                    //        LogAuto.Notify("检查UOP NG！反馈超时！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    //        string str = RunnerIn.SN + "," + RunnerIn.holderSN + "," + "检查UOP NG！反馈超时！";
                    //        LogAuto.SaveNGData(str);
                    //        Alignment.Mes反馈超时 = true;
                    //        m_nStep = (int)RunnerIn_WorkStep.NG抛料;
                    //        //Error pError = new Error(ref this.m_NowAddress, "检查UOP NG！反馈超时", "", (int)MErrorCode.UOP检查异常);
                    //        ////Error pError = new Error(ref this.m_NowAddress, POSTClass.getResult(0, CMDStep.检查UOP).Result, "", (int)MErrorCode.UOP检查异常);
                    //        //pError.AddErrSloution("Retry", (int)RunnerIn_WorkStep.扫码枪扫码);
                    //        //pError.AddErrSloution("NG抛料", (int)RunnerIn_WorkStep.NG抛料);
                    //        //pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    //    }
                    // else if (POSTClass.getResult(0, CMDStep.检查UOP).Result == "NG")
                    //    {                      
                    //        LogAuto.Notify("检查UOP NG！反馈NG！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    //        string str = RunnerIn.SN + "," + RunnerIn.holderSN + "," + MachineDataDefine.str;
                    //        RunnerIn.检查UOP失败 = true;
                    //        LogAuto.SaveNGData(str);
                    //        m_nStep = (int)RunnerIn_WorkStep.NG抛料;
                    //        //Error pError = new Error(ref this.m_NowAddress, MachineDataDefine.str, "", (int)MErrorCode.UOP检查异常);
                    //        ////Error pError = new Error(ref this.m_NowAddress, POSTClass.getResult(0, CMDStep.检查UOP).Result, "", (int)MErrorCode.UOP检查异常);
                    //        //pError.AddErrSloution("Retry", (int)RunnerIn_WorkStep.扫码枪扫码);
                    //        //pError.AddErrSloution("NG抛料", (int)RunnerIn_WorkStep.NG抛料);
                    //        //pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    //    }
                    //    break;
                    //case RunnerIn_WorkStep.流道阻挡气缸下:
                    //    LogAuto.Notify("流道阻挡气缸关闭！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    //    HardWareControl.getValve(EnumParam_Valve.流道阻挡气缸).Close();
                    //    m_nStep = (int)RunnerIn_WorkStep.等待载具到达顶升气缸位置;
                    //    break;
                    //case RunnerIn_WorkStep.等待载具到达顶升气缸位置:
                    //    if (HardWareControl.getInputIO(EnumParam_InputIO.顶升位1感应信号).GetValue() || MachineDataDefine.machineState.b_UseNullRun)
                    //    {
                    //        LogAuto.Notify("顶升位1感应信号触发！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    //        //  HardWareControl.getValve(EnumParam_Valve.流道阻挡气缸).Open();
                    //        timerDelay.Enabled = false;
                    //        timerDelay.Interval = 1000;
                    //        timerDelay.Start();
                    //        m_nStep = (int)RunnerIn_WorkStep.入料口位载具阻挡气缸出;
                    //    }
                    //    break;
                    //case RunnerIn_WorkStep.入料口位载具阻挡气缸出:
                    //    if (timerDelay.Enabled == false && HardWareControl.getValve(EnumParam_Valve.流道阻挡气缸).isIDLE())
                    //    {
                    //        LogAuto.Notify("流道阻挡气缸开&&入料龙门位载具顶升气缸上！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    //        HardWareControl.getValve(EnumParam_Valve.流道阻挡气缸).Open();
                    //        //if (!b_OutProduct)
                    //        //{
                    //        HardWareControl.getValve(EnumParam_Valve.入料龙门位载具顶升气缸上).Open();
                    //        m_nStep = (int)RunnerIn_WorkStep.入料龙门位载具顶升气缸上;
                    //        break;                  
                    //    }
                    //    break;
                    //case RunnerIn_WorkStep.入料龙门位载具顶升气缸上:
                    //    if (HardWareControl.getValve(EnumParam_Valve.入料龙门位载具顶升气缸上).isIDLE())
                    //    {
                    //        LogAuto.Notify("入料龙门位载具顶升气缸上到位，等待龙门取料！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    //        RunnerIn.runInHiveProduct = true;
                    //        m_nStep = (int)RunnerIn_WorkStep.等待把料取走;
                    //        LogAuto.Notify("流道进料完成！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    //    }
                    //    break;
                    //case RunnerIn_WorkStep.等待把料取走:
                    //    if (RunnerIn.runInHiveProduct != true)
                    //    {
                    //        LogAuto.Notify("龙门取料完成--入料龙门位载具顶升气缸下！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    //        HardWareControl.getValve(EnumParam_Valve.入料龙门位载具顶升气缸上).Close();
                    //        m_nStep = (int)RunnerIn_WorkStep.Start;
                    //    }
                    //    break;

                    //case RunnerIn_WorkStep.NG抛料:
                    //    LogAuto.Notify("NG抛料！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    //    b_OutProduct = true;
                    //   // gross_COF_result = false;
                    //    //timerDelay.Enabled = false;
                    //    //timerDelay.Interval = 3000;
                    //    //timerDelay.Start();
                    //    m_nStep = (int)RunnerIn_WorkStep.流道阻挡气缸下;
                    //    break;
                    #endregion

            }
        }
        public override void Stop()
        {
            RunnerIn.SN = "";
            PutProduct = false;
            ngTime = 0;
            HardWareControl.getOutputIO(EnumParam_OutputIO.流道步进启动).SetIO(false);
            base.Stop();
        }
    }
}
