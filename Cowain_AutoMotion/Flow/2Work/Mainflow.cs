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
using System.Threading.Tasks;
using System.Windows.Forms;
using static Cowain_Machine.Flow.MErrorDefine;

namespace Cowain_AutoMotion.Flow._2Work
{
    //.Flow._2Work
    public class Mainflow : Base
    {
        private Mainflow_HomeStep currentHomeStep;
        private Mainflow_WorkStep currentWorkStep;
        private MiSuMiControl miSuMiControl;
        private int gripRetryCount = 0;
        private const int MAX_GRIP_RETRY = 3;
        public static double speed = 80;
        /// <summary>
        /// 前龙门可放料
        /// </summary>
        public static bool PutProduct = false;
        ProductPoint product = new ProductPoint();
        int tiaozhengjuli = 0;

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
        public Mainflow(Type homeEnum1, Type stepEnum1, string instanceName1, Base parent) : base(homeEnum1, stepEnum1, instanceName1, parent, false)
        {
            // 初始化电爪控制器
            miSuMiControl = new MiSuMiControl();
            try
            {
                // 连接电爪（需要根据实际串口号修改）
                miSuMiControl.Connect("COM8"); // TODO: 从配置文件读取串口号

                // 首次使用需要激活并搜索行程
                if (!miSuMiControl.IsReady())
                {
                    miSuMiControl.EnableWithSearch();
                    miSuMiControl.WaitReady(5000);
                }
            }
            catch (Exception ex)
            {
                LogAuto.Notify($"电爪初始化失败：{ex.Message}", (int)MachineStation.主监控, MotionLogLevel.Alarm);
            }
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
            移动到组装位,
            等待引导点位运动完成,
            电夹爪打开,
            电夹爪打开状态结束,
            触发侧复检相机拍照,
            等待复检相机返回数据,
            解析复检数据,
            移动到待命位,
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
                        //showHinttEvent("");
                        //ShowLogEvent("开始作料");
                        isWorking = true;
                        listData.Clear();
                        gripRetryCount = 0;  // 重置电爪重试计数
                        LogAuto.Notify("开始作料！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        //product.startTime = DateTime.Now;

                        m_nStep = (int)Mainflow_WorkStep.移动到待命位;
                    }
                    break;

                case Mainflow_WorkStep.移动到待命位:
                    LogAuto.Notify("XY移动到夹取位！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    HardWareControl.movePoint(EnumParam_Point.待命位);
                    m_nStep = (int)Mainflow_WorkStep.移动到夹取位;
                    break;
                case Mainflow_WorkStep.移动到夹取位:
                    if (HardWareControl.getPointIdel(EnumParam_Point.待命位))
                    {
                        LogAuto.Notify("XY移动到夹取位！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.movePoint(EnumParam_Point.物料夹取XY位);
                        m_nStep = (int)Mainflow_WorkStep.夹取向下夹取;
                    }
                    break;
                case Mainflow_WorkStep.夹取向下夹取:
                    if (HardWareControl.getPointIdel(EnumParam_Point.物料夹取XY位))
                    {
                        LogAuto.Notify("Z移动到夹取位！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.movePoint(EnumParam_Point.物料夹取Z位);
                        m_nStep = (int)Mainflow_WorkStep.电夹爪夹;

                    }
                    break;
                case Mainflow_WorkStep.电夹爪夹:
                    if (HardWareControl.getPointIdel(EnumParam_Point.物料夹取Z位))
                    {
                        if (MachineDataDefine.machineState.b_UseTestRun)
                        {
                            m_nStep = (int)Mainflow_WorkStep.取料完成Z轴向上;
                            break;
                        }
                        LogAuto.Notify("电夹爪开始夹取！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        
                        // 全速全力关闭到 26mm 位置（0x0A28 = 2600 = 26.00mm）
                        if (miSuMiControl.CloseToPosition(2600))
                        {
                            m_nStep = (int)Mainflow_WorkStep.判断电夹爪夹取状态;
                        }
                        else
                        {
                            LogAuto.Notify("电夹爪关闭指令发送失败！", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                            m_nStep = (int)Mainflow_WorkStep.Completed;
                        }
                    }
                    break;
                    
                case Mainflow_WorkStep.判断电夹爪夹取状态:
                    LogAuto.Notify("等待电夹爪夹取完成！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    
                    // 等待电爪动作完成（最多10秒）
                    if (miSuMiControl.WaitMovementComplete(10000))
                    {
                        // 读取详细状态
                        var status = miSuMiControl.ReadDetailedStatus();
                        if (status != null)
                        {
                            LogAuto.Notify($"电夹爪状态：{status.ToString()}", (int)MachineStation.主监控, MotionLogLevel.Info);
                            
                            // 只有 GRIP_HOLDING (0x03) 才表示真正夹到工件
                            if (status.GripState == MiSuMiControl.GRIP_HOLDING)
                            {
                                LogAuto.Notify("电夹爪夹取成功！", (int)MachineStation.主监控, MotionLogLevel.Info);
                                gripRetryCount = 0;  // 重置重试计数
                                m_nStep = (int)Mainflow_WorkStep.取料完成Z轴向上;
                            }
                            else
                            {
                                // 夹取失败，尝试重试
                                gripRetryCount++;
                                LogAuto.Notify($"电夹爪夹取失败(状态={status.GripState})，第{gripRetryCount}次尝试", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                                
                                if (gripRetryCount < MAX_GRIP_RETRY)
                                {
                                    // 自动重试：先打开电爪，然后重新夹取
                                    LogAuto.Notify($"准备第{gripRetryCount + 1}次重试夹取", (int)MachineStation.主监控, MotionLogLevel.Info);
                                    miSuMiControl.OpenToZero();
                                    System.Threading.Thread.Sleep(500);  // 等待打开完成
                                    m_nStep = (int)Mainflow_WorkStep.电夹爪夹;  // 重新夹取
                                }
                                else
                                {
                                    // 超过最大重试次数
                                    LogAuto.Notify($"电夹爪夹取失败超过最大重试次数({MAX_GRIP_RETRY}次)！", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                                    showHinttEvent($"电夹爪夹取失败，已重试{MAX_GRIP_RETRY}次，请检查物料是否正常！");
                                    miSuMiControl.OpenToZero();  // 打开电爪释放
                                    gripRetryCount = 0;  // 重置计数
                                    m_nStep = (int)Mainflow_WorkStep.Completed;
                                }
                            }
                        }
                        else
                        {
                            LogAuto.Notify("无法读取电夹爪状态！", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                            showHinttEvent("无法读取电夹爪状态，请检查通讯！");
                            miSuMiControl.OpenToZero();  // 打开电爪
                            gripRetryCount = 0;
                            m_nStep = (int)Mainflow_WorkStep.Completed;
                        }
                    }
                    else
                    {
                        // 超时处理
                        LogAuto.Notify("电夹爪夹取超时！", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                        showHinttEvent("电夹爪夹取超时，请检查！");
                        miSuMiControl.OpenToZero();  // 打开电爪
                        gripRetryCount = 0;
                        m_nStep = (int)Mainflow_WorkStep.Completed;
                    }
                    break;
                case Mainflow_WorkStep.取料完成Z轴向上:
                    HardWareControl.movePoint(EnumParam_Point.Z轴安全位);
                    m_nStep = (int)Mainflow_WorkStep.Y轴移动到下相机;
                   
                    break;
                case Mainflow_WorkStep.Y轴移动到下相机:
                    if (HardWareControl.getPointIdel(EnumParam_Point.Z轴安全位))
                    {
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
                            //string str = "T1,1," + product.startTime.ToString("yyyyMMddHHmmss") + "," + product.SN;
                            LogAuto.Notify("获取轴当前坐标！", (int)MachineStation.主监控, MotionLogLevel.Info);
                            double axisX = HardWareControl.getMotor(EnumParam_Axis.X).GetPosition();
                            double axisY = HardWareControl.getMotor(EnumParam_Axis.Y).GetPosition();
                            double axisR = HardWareControl.getMotor(EnumParam_Axis.R1).GetPosition();
                            string str = "T1,1,"+axisX + "," + axisY + ","+ axisR;
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

                        if (ccd[2] == "1")//结果ok
                        {
                            LogAuto.Notify("下相机拍照返回结果OK！" + ccdResult1, (int)MachineStation.主监控, MotionLogLevel.Info);
                          
                           
                            m_nStep = (int)Mainflow_WorkStep.到上相机拍照位;
                        }
                        else
                        {
                            product.pass = false;
                            LogAuto.Notify("下相机拍照结果NG!" + ccdResult1, (int)MachineStation.主监控, MotionLogLevel.Info);
                            showHinttEvent("下相机拍照结果999！请重新拍照");
                            m_nStep = (int)Mainflow_WorkStep.Completed;

                        }

                    }

                    else if (timerDelay.Enabled == false)
                    {
                        showHinttEvent("下相机拍照返回结果超时");
                        LogAuto.Notify("下相机拍照返回结果超时！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        m_nStep = (int)Mainflow_WorkStep.Completed;
                    }
                    break;
                case Mainflow_WorkStep.到上相机拍照位:
                    HardWareControl.movePoint(EnumParam_Point.上相机拍照位);
                    m_nStep = (int)Mainflow_WorkStep.触发2拍照;
                    break;
                case Mainflow_WorkStep.触发2拍照:

                    if (HardWareControl.getPointIdel(EnumParam_Point.上相机拍照位))
                    {
                        LogAuto.Notify("已到达上相机拍照位！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        if (MachineDataDefine.machineState.b_UseCCD)
                        {
                            double axisX = HardWareControl.getMotor(EnumParam_Axis.X).GetPosition();
                            double axisY = HardWareControl.getMotor(EnumParam_Axis.Y).GetPosition();
                            double axisR = HardWareControl.getMotor(EnumParam_Axis.R1).GetPosition();
                            string str = "T2,1," + axisX + "," + axisY + "," + axisR;
                            HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr = "";
                            HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).SendMSG(str);
                            m_nStep = (int)Mainflow_WorkStep.等待相机返回数据2;
                        }
                        else
                        {
                            LogAuto.Notify("未启用相机！", (int)MachineStation.主监控, MotionLogLevel.Info);
                            m_nStep = (int)Mainflow_WorkStep.移动到组装位;
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

                        if (ccd[2] == "1" )//结果ok
                        {
                            LogAuto.Notify("上相机拍照返回结果OK！" + ccdResult2, (int)MachineStation.主监控, MotionLogLevel.Info);
                            product.pass = true;
                          
                            #region
                            //List<data> datas = new List<data>();
                            //for (int i = 0; i < HIVEDataDefine.limitdata.Count; i++)
                            //{
                            //    data data = new data
                            //    {
                            //        test = HIVEDataDefine.limitdata[i].Name,
                            //        lower_limit = HIVEDataDefine.limitdata[i].lower_limit,
                            //        upper_limit = HIVEDataDefine.limitdata[i].upper_limit,
                            //        value = listData[i]
                            //    };
                            //    datas.Add(data);
                            //}
                            //foreach (var item in datas)
                            //{
                            //    if (item.value > item.upper_limit || item.value < item.lower_limit)
                            //    {
                            //        product.pass = false;
                            //        break;
                            //    }
                            //}
                            //product.datas = datas;
                            #endregion
                            if (ccdResult2.Contains("99999")||ccdResult2.Contains("99999"))
                            {
                                //product.pass = false;
                                LogAuto.Notify("上相机拍照结果NG!" + ccdResult2, (int)MachineStation.主监控, MotionLogLevel.Info);
                                showHinttEvent("上相机拍照结果999！请重新拍照");
                                m_nStep = (int)Mainflow_WorkStep.Completed;
                                break;
                            }
                            m_nStep = (int)Mainflow_WorkStep.触发相机计算;
                        }
                        else
                        {
                            product.pass = false;
                            LogAuto.Notify("上相机拍照结果NG!" + ccdResult2, (int)MachineStation.主监控, MotionLogLevel.Info);
                            showHinttEvent("上相机拍照结果999！请重新拍照");
                            m_nStep = (int)Mainflow_WorkStep.Completed;
                            //b_Result = false;
                            //LogAuto.Notify("上相机复检拍照4返回结果NG！" + ccdResult, (int)MachineStation.主监控, MotionLogLevel.Alarm);
                            //Error pError = new Error(ref this.m_NowAddress, "上相机复检拍照4返回结果NG", "", (int)MErrorCode.CCD_Capture1異常);
                            //pError.AddErrSloution("Retry", (int)AxisTakeIn_WorkStep.复检位4拍照);
                            //pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                        }

                    }

                    else if (timerDelay.Enabled == false)
                    {
                        LogAuto.Notify("上相机返回结果超时！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        m_nStep = (int)Mainflow_WorkStep.Completed;
                    }

                    break;


                case Mainflow_WorkStep.触发相机计算:
                    LogAuto.Notify("触发相机计算！", (int)MachineStation.主监控, MotionLogLevel.Info);
                 
                    HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr = "";
                    HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).SendMSG("T2");
                    timerDelay.Enabled = false;
                    timerDelay.Interval = 10000;
                    timerDelay.Start();
                    m_nStep = (int)Mainflow_WorkStep.接收相机反馈结果;
                    break;

                case Mainflow_WorkStep.接收相机反馈结果:

                    if (HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr != "")
                    {
                        string ccdResult = HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr; //T3,2,1,X,Y,R,X,Y,X,Y,X,Y,X,Y

                        LogAuto.Notify("接收相机反馈结果！" + ccdResult, (int)MachineStation.主监控, MotionLogLevel.Info);
                        string[] ccd = ccdResult.Split(',');

                        if (ccd[2] == "1")//结果ok
                        {
                            LogAuto.Notify("上相机计算返回OK！，接收组装坐标及锁螺丝坐标" + ccdResult, (int)MachineStation.主监控, MotionLogLevel.Info);
                          
                            datas.Clear();
                            datas.Add("OutX", ccd[2]);
                            datas.Add("OutY", ccd[3]);
                            datas.Add("OutR", ccd[4]);
                            m_nStep = (int)Mainflow_WorkStep.移动到组装位;

                        }
                           else
                            {
                                product.pass = false;
                                LogAuto.Notify("上相机计算返回结果NG!" + ccdResult2, (int)MachineStation.主监控, MotionLogLevel.Info);
                                showHinttEvent("上相机计算返回结果999！请重新拍照");
                                m_nStep = (int)Mainflow_WorkStep.Completed;
                                //b_Result = false;
                                //LogAuto.Notify("上相机复检拍照4返回结果NG！" + ccdResult, (int)MachineStation.主监控, MotionLogLevel.Alarm);
                                //Error pError = new Error(ref this.m_NowAddress, "上相机复检拍照4返回结果NG", "", (int)MErrorCode.CCD_Capture1異常);
                                //pError.AddErrSloution("Retry", (int)AxisTakeIn_WorkStep.复检位4拍照);
                                //pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                            }
                      

                    }

                    else if (timerDelay.Enabled == false)
                    {
                        LogAuto.Notify("上相机计算返回结果超时！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        m_nStep = (int)Mainflow_WorkStep.Completed;
                    }
                    break;
                case Mainflow_WorkStep.移动到组装位:
                    if (MachineDataDefine.machineState.b_UseTestRun)
                    {
                        LogAuto.Notify("空跑模式下直接走固定位置！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.movePoint(EnumParam_Point.组装位);
                    }
                    else
                    {
                        LogAuto.Notify("移动组装位XYR绝对坐标！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        double OutX = Convert.ToDouble(datas["OutX"]);
                        double OutY = Convert.ToDouble(datas["OutY"]);
                        double OutR = Convert.ToDouble(datas["OutR"]);
                        HardWareControl.getMotor(EnumParam_Axis.X).AbsMove(OutX, MachineDataDefine.machineState.machineSpeed);
                        HardWareControl.getMotor(EnumParam_Axis.Y).AbsMove(OutY, MachineDataDefine.machineState.machineSpeed);
                        HardWareControl.getMotor(EnumParam_Axis.R1).AbsMove(OutY, MachineDataDefine.machineState.machineSpeed);
                    }
       
                    m_nStep = (int)Mainflow_WorkStep.等待引导点位运动完成; 
                    break;

                case Mainflow_WorkStep.等待引导点位运动完成:
                    if (!MachineDataDefine.machineState.b_UseTestRun)
                    {

                        if (HardWareControl.getMotor(EnumParam_Axis.X).isIDLE() && HardWareControl.getMotor(EnumParam_Axis.Y).isIDLE()
                        && HardWareControl.getMotor(EnumParam_Axis.R1).isIDLE())
                        {
                            LogAuto.Notify("电夹爪开始打开！", (int)MachineStation.主监控, MotionLogLevel.Info);
                           
                                // 全速全力打开到 0mm 位置
                                if (miSuMiControl.OpenToZero())
                            {
                                m_nStep = (int)Mainflow_WorkStep.电夹爪打开状态结束;
                            }
                            else
                            {
                                LogAuto.Notify("电夹爪打开指令发送失败！", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                                m_nStep = (int)Mainflow_WorkStep.Completed;
                            }
                        }
                    }
                    else
                    {

                        if (HardWareControl.getPointIdel(EnumParam_Point.组装位))
                        {
                            LogAuto.Notify("电夹爪开始打开！", (int)MachineStation.主监控, MotionLogLevel.Info);
                            HardWareControl.movePoint(EnumParam_Point.上相机拍照位);

                            m_nStep = (int)Mainflow_WorkStep.触发侧复检相机拍照;
                            break;

                            //// 全速全力打开到 0mm 位置
                            //if (miSuMiControl.OpenToZero())
                            //{
                            //    m_nStep = (int)Mainflow_WorkStep.电夹爪打开状态结束;
                            //}
                            //else
                            //{
                            //    LogAuto.Notify("电夹爪打开指令发送失败！", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                            //    m_nStep = (int)Mainflow_WorkStep.Completed;
                            //}
                          
                        }
                    }
                    break;

                case Mainflow_WorkStep.电夹爪打开状态结束:
                    LogAuto.Notify("等待电夹爪打开完成！", (int)MachineStation.主监控, MotionLogLevel.Info);
                    
                    // 等待电爪打开完成
                    if (miSuMiControl.WaitMovementComplete(10000))
                    {
                        LogAuto.Notify("电夹爪打开成功，移动到复检位！", (int)MachineStation.主监控, MotionLogLevel.Info);
                        HardWareControl.movePoint(EnumParam_Point.上相机拍照位);
                        m_nStep = (int)Mainflow_WorkStep.触发侧复检相机拍照;
                    }
                    else
                    {
                        LogAuto.Notify("电夹爪打开超时！", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                        showHinttEvent("电夹爪打开超时，请检查！");
                        m_nStep = (int)Mainflow_WorkStep.Completed;
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
                            string str = "T3,1," + axisX + "," + axisY + "," + axisR;
                            HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr = "";
                            HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).SendMSG(str);
                            m_nStep = (int)Mainflow_WorkStep.等待复检相机返回数据;
                        }
                        else
                        {
                            LogAuto.Notify("未启用相机！", (int)MachineStation.主监控, MotionLogLevel.Info);
                            m_nStep = (int)Mainflow_WorkStep.Completed;
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

                        if (ccd[2] == "1")//结果ok
                        {
                            LogAuto.Notify("上相机拍照返回结果OK！" + ccdResult2, (int)MachineStation.主监控, MotionLogLevel.Info);
                         

                            //记录坐标值在本地

                         
                            if (ccdResult2.Contains("99999") || ccdResult2.Contains("99999"))
                            {
                                //product.pass = false;
                                LogAuto.Notify("上相机拍照结果NG!" + ccdResult2, (int)MachineStation.主监控, MotionLogLevel.Info);
                                showHinttEvent("上相机拍照结果999！请重新拍照");
                                m_nStep = (int)Mainflow_WorkStep.Completed;
                                break;
                            }
                            m_nStep = (int)Mainflow_WorkStep.Completed;
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
                case Mainflow_WorkStep.Completed:
                    
                        isWorking = false;
                        m_nStep = (int)Mainflow_WorkStep.Start;
                   
                    break;
            }
        }
        public override void Stop()
        {
            //RunnerIn.SN = "";
            PutProduct = false;
            isWorking=false;
            
            // 停止时断开电爪连接
            try
            {
                miSuMiControl?.Disconnect();
            }
            catch (Exception ex)
            {
                LogAuto.Notify($"断开电爪连接失败：{ex.Message}", (int)MachineStation.主监控, MotionLogLevel.Alarm);
            }
            
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


