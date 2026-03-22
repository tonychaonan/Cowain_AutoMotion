using Cowain_Machine;
using MotionBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Cowain_AutoMotion.AxisCalibration;
using static Cowain_Machine.Flow.MErrorDefine;
using System.Windows.Forms;

namespace Cowain_AutoMotion.Flow._2Work
{
    public class 动静态 : Base
    {
        #region 枚举定义
        /// <summary>
        /// 相机类型枚举
        /// </summary>
        public enum CameraType
        {
            上相机 = 0,
            下相机 = 1
        }
        
        /// <summary>
        /// 数据类型枚举
        /// </summary>
        public enum DataType
        {
            静态 = 0,
            动态 = 1
        }
        #endregion
        
        #region 配置参数
        /// <summary>
        /// 当前选择的相机类型
        /// </summary>
        private CameraType selectedCamera = CameraType.上相机;
        
        /// <summary>
        /// 当前选择的数据类型
        /// </summary>
        private DataType selectedDataType = DataType.静态;
        
        /// <summary>
        /// 当前循环次数
        /// </summary>
        private int currentLoopCount = 0;
        
        /// <summary>
        /// 目标循环次数
        /// </summary>
        private int targetLoopCount = 1;
        
        /// <summary>
        /// 是否启用循环模式
        /// </summary>
        private bool isLoopEnabled = false;
        
        /// <summary>
        /// 数据采集结果列表
        /// </summary>
        private List<CameraDataResult> dataResults = new List<CameraDataResult>();
        #endregion

        public 动静态(Type homeEnum1, Type stepEnum1, string instanceName1, Base parent, int ErrCodeBase = 0) : base(homeEnum1, stepEnum1, instanceName1, parent, ErrCodeBase)
        {
        }

        private ActionAndStatic_WorkStep currentWorkStep;

        public enum ActionAndStatic_WorkStep
        {
            Start = 0,
            初始化循环,
            移动到拍照位,
            触发拍照,
            等待拍照完成信号,
            拍照完成,
            数据采集完成,
            检查循环条件,
            循环结束处理,
            Completed
        }


        public override void StepCycle(ref double dbTime)
        {
            currentWorkStep = (ActionAndStatic_WorkStep)m_nStep;
            switch (currentWorkStep)
            {
                case ActionAndStatic_WorkStep.Start:
                    string cameraName = selectedCamera == CameraType.上相机 ? "上相机" : "下相机";
                    string dataTypeName = selectedDataType == DataType.静态 ? "静态" : "动态";
                    LogAuto.Notify($"开始{cameraName}{dataTypeName}数据采集流程", (int)MachineStation.主监控, MotionLogLevel.Info);
                    m_nStep = (int)ActionAndStatic_WorkStep.初始化循环;
                    break;
                    
                case ActionAndStatic_WorkStep.初始化循环:
                    // 重置循环计数器
                    currentLoopCount = 0;
                    dataResults.Clear();
                    LogAuto.Notify($"初始化循环控制，目标循环次数：{targetLoopCount}", (int)MachineStation.主监控, MotionLogLevel.Info);
                    m_nStep = (int)ActionAndStatic_WorkStep.移动到拍照位;
                    break;
                    
                case ActionAndStatic_WorkStep.移动到拍照位:
                    string currentCameraName = selectedCamera == CameraType.上相机 ? "上相机" : "下相机";
                    LogAuto.Notify($"第{currentLoopCount + 1}次循环 - 移动到{currentCameraName}拍照位", (int)MachineStation.主监控, MotionLogLevel.Info);
                    
                    // 调用通用移动方法
                    if (MoveToCameraPosition(selectedCamera))
                    {
                        m_nStep = (int)ActionAndStatic_WorkStep.触发拍照;
                    }
                    // 如果移动未完成，保持当前步骤继续等待
                    break;
                    
                case ActionAndStatic_WorkStep.触发拍照:
                    string triggerCameraName = selectedCamera == CameraType.上相机 ? "上相机" : "下相机";
                    string triggerDataType = selectedDataType == DataType.静态 ? "静态" : "动态";
                    LogAuto.Notify($"触发{triggerCameraName}{triggerDataType}拍照", (int)MachineStation.主监控, MotionLogLevel.Info);
                    
                    // 调用通用触发方法
                    if (TriggerCameraCapture(selectedCamera, selectedDataType))
                    {
                        m_nStep = (int)ActionAndStatic_WorkStep.等待拍照完成信号;
                    }
                    break;
                    
                case ActionAndStatic_WorkStep.等待拍照完成信号:
                    // 调用通用等待方法
                    if (WaitForCaptureComplete(selectedCamera, selectedDataType))
                    {
                        m_nStep = (int)ActionAndStatic_WorkStep.拍照完成;
                    }
                    break;
                    
                case ActionAndStatic_WorkStep.拍照完成:
                    string completeCameraName = selectedCamera == CameraType.上相机 ? "上相机" : "下相机";
                    string completeDataType = selectedDataType == DataType.静态 ? "静态" : "动态";
                    LogAuto.Notify($"{completeCameraName}{completeDataType}拍照完成", (int)MachineStation.主监控, MotionLogLevel.Info);
                    m_nStep = (int)ActionAndStatic_WorkStep.数据采集完成;
                    break;
                    
                case ActionAndStatic_WorkStep.数据采集完成:
                    // 记录本次循环的数据采集结果
                    var result = new CameraDataResult
                    {
                        LoopIndex = currentLoopCount + 1,
                        Timestamp = DateTime.Now,
                        CameraType = selectedCamera,
                        DataType = selectedDataType,
                        CameraResult = GetCameraData(selectedCamera, selectedDataType)
                    };
                    dataResults.Add(result);
                    
                    currentLoopCount++;
                    LogAuto.Notify($"第{currentLoopCount}次数据采集完成", (int)MachineStation.主监控, MotionLogLevel.Info);
                    m_nStep = (int)ActionAndStatic_WorkStep.检查循环条件;
                    break;
                    
                case ActionAndStatic_WorkStep.检查循环条件:
                    if (isLoopEnabled && currentLoopCount < targetLoopCount)
                    {
                        LogAuto.Notify($"继续循环，当前：{currentLoopCount}/{targetLoopCount}", (int)MachineStation.主监控, MotionLogLevel.Info);
                        m_nStep = (int)ActionAndStatic_WorkStep.移动到拍照位;
                    }
                    else
                    {
                        LogAuto.Notify($"循环结束，共完成{currentLoopCount}次数据采集", (int)MachineStation.主监控, MotionLogLevel.Info);
                        m_nStep = (int)ActionAndStatic_WorkStep.循环结束处理;
                    }
                    break;
                    
                case ActionAndStatic_WorkStep.循环结束处理:
                    // 处理所有采集到的数据
                    ProcessCameraDataResults();
                    string finalCameraName = selectedCamera == CameraType.上相机 ? "上相机" : "下相机";
                    string finalDataType = selectedDataType == DataType.静态 ? "静态" : "动态";
                    LogAuto.Notify($"{finalCameraName}{finalDataType}数据采集流程完成", (int)MachineStation.主监控, MotionLogLevel.Info);
                    m_nStep = (int)ActionAndStatic_WorkStep.Completed;
                    break;
                    
                case ActionAndStatic_WorkStep.Completed:
                    m_Status = 狀態.待命;
                    break;
            }
            base.StepCycle(ref dbTime);
        }

        #region 通用相机控制方法
        
        /// <summary>
        /// 移动到指定相机拍照位
        /// </summary>
        /// <param name="cameraType">相机类型</param>
        /// <returns>是否移动完成</returns>
        private bool MoveToCameraPosition(CameraType cameraType)
        {
            // TODO: 根据相机类型调用对应的轴移动方法
            switch (cameraType)
            {
                case CameraType.上相机:
                    return true; 
                    
                case CameraType.下相机:
                    return true; // 临时返回true
                    
                default:
                    LogAuto.Notify($"未知的相机类型：{cameraType}", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                    return false;
            }
        }
        
        /// <summary>
        /// 触发相机拍照
        /// </summary>
        /// <param name="cameraType">相机类型</param>
        /// <param name="dataType">数据类型</param>
        /// <returns>是否触发成功</returns>
        private bool TriggerCameraCapture(CameraType cameraType, DataType dataType)
        {
            string cameraName = cameraType == CameraType.上相机 ? "上相机" : "下相机";
            string dataTypeName = dataType == DataType.静态 ? "静态" : "动态";
            
            switch (cameraType)
            {
                case CameraType.上相机:
                    if (dataType == DataType.静态)
                    {

                    }
                    else
                    {
                        // TODO: 触发上相机动态拍照
                        // 例如：return CameraControl.TriggerUpperCameraDynamic();
                    }
                    break;
                    
                case CameraType.下相机:
                    if (dataType == DataType.静态)
                    {
                        // TODO: 触发下相机静态拍照
                        // 例如：return CameraControl.TriggerLowerCameraStatic();
                    }
                    else
                    {
                        // TODO: 触发下相机动态拍照
                        // 例如：return CameraControl.TriggerLowerCameraDynamic();
                    }
                    break;
            }
            
            LogAuto.Notify($"触发{cameraName}{dataTypeName}拍照成功", (int)MachineStation.主监控, MotionLogLevel.Info);
            return true; // 临时返回true
        }
        
        /// <summary>
        /// 等待拍照完成
        /// </summary>
        /// <param name="cameraType">相机类型</param>
        /// <param name="dataType">数据类型</param>
        /// <returns>是否拍照完成</returns>
        private bool WaitForCaptureComplete(CameraType cameraType, DataType dataType)
        {
            // TODO: 根据相机类型和数据类型检查拍照完成状态
            switch (cameraType)
            {
                case CameraType.上相机:
                    // TODO: 检查上相机拍照完成信号
                    // 例如：return CameraControl.IsUpperCameraCaptureComplete();
                    return true; // 临时返回true
                    
                case CameraType.下相机:
                    // TODO: 检查下相机拍照完成信号
                    // 例如：return CameraControl.IsLowerCameraCaptureComplete();
                    return true; // 临时返回true
                    
                default:
                    return false;
            }
        }
        
        /// <summary>
        /// 获取相机数据
        /// </summary>
        /// <param name="cameraType">相机类型</param>
        /// <param name="dataType">数据类型</param>
        /// <returns>相机数据</returns>
        private string GetCameraData(CameraType cameraType, DataType dataType)
        {
            // TODO: 根据相机类型和数据类型获取实际数据
            string cameraName = cameraType == CameraType.上相机 ? "上相机" : "下相机";
            string dataTypeName = dataType == DataType.静态 ? "静态" : "动态";
            
            // 临时返回模拟数据
            return $"{cameraName}{dataTypeName}数据_{DateTime.Now:HHmmss}";
        }
        
        #endregion
        
        #region 公共配置和接口方法
        
        /// <summary>
        /// 配置相机数据采集参数
        /// </summary>
        /// <param name="cameraType">相机类型</param>
        /// <param name="dataType">数据类型</param>
        /// <param name="loopCount">循环次数</param>
        /// <param name="enableLoop">是否启用循环</param>
        public void ConfigureCapture(CameraType cameraType, DataType dataType, int loopCount = 1, bool enableLoop = true)
        {
            selectedCamera = cameraType;
            selectedDataType = dataType;
            targetLoopCount = Math.Max(1, loopCount);
            isLoopEnabled = enableLoop;
            
            string cameraName = cameraType == CameraType.上相机 ? "上相机" : "下相机";
            string dataTypeName = dataType == DataType.静态 ? "静态" : "动态";
            LogAuto.Notify($"配置采集参数：{cameraName}{dataTypeName}，循环{targetLoopCount}次，启用循环={isLoopEnabled}", 
                (int)MachineStation.主监控, MotionLogLevel.Info);
        }
        
        /// <summary>
        /// 启动上相机静态数据采集
        /// </summary>
        /// <param name="loopCount">循环次数</param>
        public void StartUpperCameraStatic(int loopCount = 1)
        {
            ConfigureCapture(CameraType.上相机, DataType.静态, loopCount);
            DoStep((int)ActionAndStatic_WorkStep.Start);
        }
        
        /// <summary>
        /// 启动上相机动态数据采集
        /// </summary>
        /// <param name="loopCount">循环次数</param>
        public void StartUpperCameraDynamic(int loopCount = 1)
        {
            ConfigureCapture(CameraType.上相机, DataType.动态, loopCount);
            DoStep((int)ActionAndStatic_WorkStep.Start);
        }
        
        /// <summary>
        /// 启动下相机静态数据采集
        /// </summary>
        /// <param name="loopCount">循环次数</param>
        public void StartLowerCameraStatic(int loopCount = 1)
        {
            ConfigureCapture(CameraType.下相机, DataType.静态, loopCount);
            DoStep((int)ActionAndStatic_WorkStep.Start);
        }
        
        /// <summary>
        /// 启动下相机动态数据采集
        /// </summary>
        /// <param name="loopCount">循环次数</param>
        public void StartLowerCameraDynamic(int loopCount = 1)
        {
            ConfigureCapture(CameraType.下相机, DataType.动态, loopCount);
            DoStep((int)ActionAndStatic_WorkStep.Start);
        }
        
        /// <summary>
        /// 获取当前循环进度
        /// </summary>
        /// <returns>当前循环次数/目标循环次数</returns>
        public string GetLoopProgress()
        {
            return $"{currentLoopCount}/{targetLoopCount}";
        }
        
        /// <summary>
        /// 获取数据采集结果
        /// </summary>
        /// <returns>采集结果列表</returns>
        public List<CameraDataResult> GetCameraDataResults()
        {
            return new List<CameraDataResult>(dataResults);
        }
        
        /// <summary>
        /// 重置循环状态
        /// </summary>
        public void ResetLoop()
        {
            currentLoopCount = 0;
            dataResults.Clear();
            LogAuto.Notify("循环状态已重置", (int)MachineStation.主监控, MotionLogLevel.Info);
        }
        
        /// <summary>
        /// 处理相机数据采集结果
        /// </summary>
        private void ProcessCameraDataResults()
        {
            LogAuto.Notify($"开始处理{dataResults.Count}条相机数据", (int)MachineStation.主监控, MotionLogLevel.Info);
            
            foreach (var result in dataResults)
            {
                string cameraName = result.CameraType == CameraType.上相机 ? "上相机" : "下相机";
                string dataTypeName = result.DataType == DataType.静态 ? "静态" : "动态";
                LogAuto.Notify($"循环{result.LoopIndex}: {result.Timestamp:HH:mm:ss.fff} - {cameraName}{dataTypeName}:{result.CameraResult}", 
                    (int)MachineStation.主监控, MotionLogLevel.Info);
            }
        }
        
        #endregion
    }
    
    /// <summary>
    /// 相机数据采集结果
    /// </summary>
    public class CameraDataResult
    {
        /// <summary>
        /// 循环索引（第几次循环）
        /// </summary>
        public int LoopIndex { get; set; }
        
        /// <summary>
        /// 采集时间戳
        /// </summary>
        public DateTime Timestamp { get; set; }
        
        /// <summary>
        /// 相机类型
        /// </summary>
        public 动静态.CameraType CameraType { get; set; }
        
        /// <summary>
        /// 数据类型
        /// </summary>
        public 动静态.DataType DataType { get; set; }
        
        /// <summary>
        /// 相机采集结果
        /// </summary>
        public string CameraResult { get; set; }
        
        /// <summary>
        /// 其他扩展数据
        /// </summary>
        public Dictionary<string, object> ExtendedData { get; set; } = new Dictionary<string, object>();
    }
}
