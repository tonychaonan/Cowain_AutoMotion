using NModbus;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NModbus.Serial;
using System.Windows.Forms;
using NModbus.Device;
using Cowain_Machine;

namespace Cowain_AutoMotion.Flow.Common
{
    public class MiSuMiControl
    {
        private SerialPort _serialPort;       // 串口（USB转485）
        private IModbusMaster _modbusMaster;  // Modbus主站

        public MiSuMiControl()
            {
            try
            {
                // 连接电爪（需要根据实际串口号修改）
               Connect("COM8"); // TODO: 从配置文件读取串口号

                // 首次使用需要激活并搜索行程
                if (!IsReady())
                {
                    EnableWithSearch();
                    WaitReady(5000);
                }
            }
            catch (Exception ex)
            {
                LogAuto.Notify($"电爪初始化失败：{ex.Message}", (int)MachineStation.主监控, MotionLogLevel.Alarm);
            }
        }


        #region 寄存器地址（根据协议文档）
        public const ushort INIT_REG = 0x0FA0;       // 初始化寄存器
        public const ushort MODE_REG = 0x0FA1;       // 控制模式选择寄存器
        public const ushort POSITION_REG = 0x0FA2;   // 位置寄存器
        public const ushort STATUS_REG = 0x1194;     // 状态寄存器（只读）
        #endregion

        #region 初始化寄存器值（0x0FA0）
        public const ushort INIT_DISABLE = 0x00;           // 去使能
        public const ushort INIT_ENABLE_SEARCH = 0x01;     // 使能并开合动作搜索行程
        public const ushort INIT_ENABLE_CLOSE = 0x02;      // 使能并闭合动作
        public const ushort INIT_ENABLE_OPEN = 0x03;       // 使能并打开动作
        #endregion

        #region 控制模式选择（0x0FA1）
        public const ushort MODE_NONE = 0x00;              // 无操作
        public const ushort MODE_NO_ACTION = 0x01;         // 无操作
        public const ushort MODE_HALF_OPEN = 0x02;         // 半力半速打开
        public const ushort MODE_HALF_CLOSE = 0x03;        // 半力半速关闭
        public const ushort MODE_FULL_OPEN = 0x04;         // 全力全速打开
        public const ushort MODE_FULL_CLOSE = 0x05;        // 全力全速关闭
        public const ushort MODE_LOW_OPEN = 0x06;          // 低力低速打开
        public const ushort MODE_LOW_CLOSE = 0x07;         // 低力低速关闭
        public const ushort MODE_PRESET_1 = 0x08;          // 执行点1命令
        public const ushort MODE_PRESET_2 = 0x09;          // 执行点2命令
        public const ushort MODE_PRESET_3 = 0x0A;          // 执行点3命令
        public const ushort MODE_PRESET_4 = 0x0B;          // 执行点4命令
        public const ushort MODE_PRESET_5 = 0x0C;          // 执行点5命令
        public const ushort MODE_PRESET_6 = 0x0D;          // 执行点6命令
        public const ushort MODE_PRESET_7 = 0x0E;          // 执行点7命令
        public const ushort MODE_PRESET_8 = 0x0F;          // 执行点8命令
        #endregion

        #region 状态寄存器值（0x1194，只读）
        public const ushort STATE_INIT = 0x00;             // 初始化选项
        public const ushort STATE_SEARCHING = 0x01;        // 搜索行程中
        public const ushort STATE_SEARCH_DONE = 0x02;      // 搜索行程完成
        public const ushort STATE_READY = 0x03;            // 激活完成（可以执行动作）
        #endregion

        #region 扩展寄存器地址（用于高级控制）
        public const ushort SPEED_REG = 0x0FA3;            // 速度寄存器
        public const ushort FORCE_REG = 0x0FA4;            // 力矩寄存器
        public const ushort TRIGGER_REG = 0x0FA5;          // 触发寄存器
        public const ushort FAULT_STATUS_REG = 0x1195;     // 故障状态寄存器
        public const ushort GRIP_STATE_REG = 0x1196;       // 夹持状态寄存器
        public const ushort CURRENT_POS_REG = 0x1197;      // 当前位置寄存器
        public const ushort CURRENT_SPEED_REG = 0x1198;    // 当前速度寄存器
        public const ushort CURRENT_FORCE_REG = 0x1199;    // 当前力矩寄存器
        #endregion

        #region 夹持状态值（0x1196）
        public const ushort GRIP_DEFAULT = 0x00;           // 默认状态
        public const ushort GRIP_MOVING = 0x01;            // 运行中
        public const ushort GRIP_ARRIVED = 0x02;           // 到位
        public const ushort GRIP_HOLDING = 0x03;           // 夹到工件
        public const ushort GRIP_DROPPED = 0x04;           // 工件掉落
        #endregion

        #region 故障状态值（0x1195）
        public const ushort FAULT_OK = 0x00;               // 无故障
        public const ushort FAULT_COMM = 0x04;             // 通讯故障
        #endregion

        #region Modbus参数
        public const byte SLAVE_ID = 0x09;                 // 从站地址（默认9，可修改）
        public const int BAUD_RATE = 115200;               // 波特率（默认115200）
        public const Parity PARITY = Parity.None;          // 校验位：无
        public const int DATA_BITS = 8;                    // 数据位：8
        public const StopBits STOP_BITS = StopBits.One;    // 停止位：1
        #endregion

        private bool _isConnected = false;
        
        /// <summary>
        /// 连接状态
        /// </summary>
        public bool IsConnected => _isConnected;
        
        /// <summary>
        /// 从站地址（可动态修改）
        /// </summary>
        public byte SlaveAddress { get; set; } = SLAVE_ID;
        /// <summary>
        /// 初始化串口并连接Modbus主站
        /// </summary>
        /// <param name="portName">串口号</param>
        /// <exception cref="Exception">连接失败异常</exception>
        public bool Connect(string portName)
        {
            try
            {
                if (_isConnected) Disconnect();

                _serialPort = new SerialPort(portName)
                {
                    BaudRate =BAUD_RATE,
                    Parity = PARITY,
                    DataBits = DATA_BITS,
                    StopBits = STOP_BITS,
                    ReadTimeout = 1000,
                    WriteTimeout = 1000
                };
                _serialPort.Open();

                // 创建Modbus RTU主站
                var factory = new ModbusFactory();
                _modbusMaster = factory.CreateRtuMaster(_serialPort);
                _modbusMaster.Transport.Retries = 3;
                _modbusMaster.Transport.WaitToRetryMilliseconds = 200;
                _isConnected = true;
                return _isConnected;
                MessageBox.Show("夹爪连接成功！");
            }
            catch (Exception ex)
            {
                _isConnected = false;
                return _isConnected;
                MessageBox.Show($"夹爪连接失败：{ex.Message}");
            }
        }
        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect()
        {
            _serialPort?.Close();
            _serialPort?.Dispose();
            _modbusMaster = null;
            _isConnected = false;
        }

        #region 基础控制方法
        
        /// <summary>
        /// 去使能（复位电爪）
        /// </summary>
        /// <returns>是否成功</returns>
        public bool Disable()
        {
            try
            {
                _modbusMaster.WriteSingleRegister(SlaveAddress, INIT_REG, INIT_DISABLE);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"去使能失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// 使能并搜索行程（首次使用必须执行）
        /// </summary>
        /// <returns>是否成功</returns>
        public bool EnableWithSearch()
        {
            try
            {
                _modbusMaster.WriteSingleRegister(SlaveAddress, INIT_REG, INIT_ENABLE_SEARCH);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"使能失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// 使能并闭合
        /// </summary>
        /// <returns>是否成功</returns>
        public bool EnableWithClose()
        {
            try
            {
                _modbusMaster.WriteSingleRegister(SlaveAddress, INIT_REG, INIT_ENABLE_CLOSE);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"使能失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// 使能并打开
        /// </summary>
        /// <returns>是否成功</returns>
        public bool EnableWithOpen()
        {
            try
            {
                _modbusMaster.WriteSingleRegister(SlaveAddress, INIT_REG, INIT_ENABLE_OPEN);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"使能失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// 等待使能完成（状态变为3）
        /// </summary>
        /// <param name="timeoutMs">超时时间（毫秒），默认5000ms</param>
        /// <returns>是否完成</returns>
        public bool WaitReady(int timeoutMs = 5000)
        {
            var start = Environment.TickCount;
            while (Environment.TickCount - start < timeoutMs)
            {
                try
                {
                    ushort state = ReadStatus();
                    if (state == STATE_READY) return true;
                    System.Threading.Thread.Sleep(100);
                }
                catch
                {
                    System.Threading.Thread.Sleep(100);
                }
            }
            return false;
        }

        #endregion

        #region 预设模式控制

        /// <summary>
        /// 半力半速打开
        /// </summary>
        /// <returns>是否成功</returns>
        public bool HalfOpen()
        {
            try
            {
                _modbusMaster.WriteSingleRegister(SlaveAddress, MODE_REG, MODE_HALF_OPEN);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"半力半速打开失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// 半力半速关闭
        /// </summary>
        /// <returns>是否成功</returns>
        public bool HalfClose()
        {
            try
            {
                _modbusMaster.WriteSingleRegister(SlaveAddress, MODE_REG, MODE_HALF_CLOSE);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"半力半速关闭失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// 全力全速打开
        /// </summary>
        /// <returns>是否成功</returns>
        public bool FullOpen()
        {
            try
            {
                _modbusMaster.WriteSingleRegister(SlaveAddress, MODE_REG, MODE_FULL_OPEN);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"全力全速打开失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// 全力全速关闭
        /// </summary>
        /// <returns>是否成功</returns>
        public bool FullClose()
        {
            try
            {
                _modbusMaster.WriteSingleRegister(SlaveAddress, MODE_REG, MODE_FULL_CLOSE);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"全力全速关闭失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// 低力低速打开
        /// </summary>
        /// <returns>是否成功</returns>
        public bool LowOpen()
        {
            try
            {
                _modbusMaster.WriteSingleRegister(SlaveAddress, MODE_REG, MODE_LOW_OPEN);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"低力低速打开失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// 低力低速关闭
        /// </summary>
        /// <returns>是否成功</returns>
        public bool LowClose()
        {
            try
            {
                _modbusMaster.WriteSingleRegister(SlaveAddress, MODE_REG, MODE_LOW_CLOSE);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"低力低速关闭失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// 执行预设点命令（1-8）
        /// </summary>
        /// <param name="presetNumber">预设点编号（1-8）</param>
        /// <returns>是否成功</returns>
        public bool ExecutePreset(int presetNumber)
        {
            if (presetNumber < 1 || presetNumber > 8)
            {
                MessageBox.Show("预设点编号必须在1-8之间", "参数错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            try
            {
                ushort modeValue = (ushort)(MODE_PRESET_1 + presetNumber - 1);
                _modbusMaster.WriteSingleRegister(SlaveAddress, MODE_REG, modeValue);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"执行预设点{presetNumber}失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        #endregion

        #region 位置控制

        /// <summary>
        /// 移动到指定位置（需要先设置好预设点）
        /// </summary>
        /// <param name="position">目标位置值</param>
        /// <returns>是否成功</returns>
        public bool MoveToPosition(ushort position)
        {
            try
            {
                _modbusMaster.WriteSingleRegister(SlaveAddress, POSITION_REG, position);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"移动到位置{position}失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        #endregion

        #region 状态读取

        /// <summary>
        /// 读取当前状态
        /// </summary>
        /// <returns>状态值（0=初始化，1=搜索中，2=搜索完成，3=就绪）</returns>
        public ushort ReadStatus()
        {
            try
            {
                return _modbusMaster.ReadInputRegisters(SlaveAddress, STATUS_REG, 1)[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"读取状态失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }

        /// <summary>
        /// 获取状态描述
        /// </summary>
        /// <returns>状态描述字符串</returns>
        public string GetStatusDescription()
        {
            ushort status = ReadStatus();
            switch (status)
            {
                case STATE_INIT:
                    return "初始化状态";
                case STATE_SEARCHING:
                    return "搜索行程中";
                case STATE_SEARCH_DONE:
                    return "搜索行程完成";
                case STATE_READY:
                    return "就绪（可执行动作）";
                default:
                    return $"未知状态({status})";
            }
        }

        /// <summary>
        /// 检查是否就绪
        /// </summary>
        /// <returns>是否就绪</returns>
        public bool IsReady()
        {
            return ReadStatus() == STATE_READY;
        }

        #endregion

        #region 高级控制方法（带参数）

        /// <summary>
        /// 移动到指定位置（带速度和力矩参数）
        /// </summary>
        /// <param name="position">目标位置（单位：0.01mm，例如2600表示26.00mm）</param>
        /// <param name="speed">速度（0-100，100为最高速度）</param>
        /// <param name="force">力矩（0-100，100为最大力矩）</param>
        /// <returns>是否成功</returns>
        public bool MoveWithParams(ushort position, ushort speed, ushort force)
        {
            try
            {
                // 写入4个寄存器：位置(0x0FA2)、速度(0x0FA3)、力矩(0x0FA4)、触发(0x0FA5)
                ushort[] data = new ushort[] { position, speed, force, 0x0001 };
                _modbusMaster.WriteMultipleRegisters(SlaveAddress, POSITION_REG, data);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"移动失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// 全速全力关闭到指定位置
        /// </summary>
        /// <param name="position">目标位置（单位：0.01mm）</param>
        /// <returns>是否成功</returns>
        public bool CloseToPosition(ushort position)
        {
            return MoveWithParams(position, 100, 100);
        }

        /// <summary>
        /// 全速全力打开（到0位置）
        /// </summary>
        /// <returns>是否成功</returns>
        public bool OpenToZero()
        {
            return MoveWithParams(0, 100, 100);
        }

        #endregion

        #region 详细状态读取

        /// <summary>
        /// 读取详细状态（6个寄存器）
        /// </summary>
        /// <returns>状态数据结构</returns>
        public GripperStatus ReadDetailedStatus()
        {
            try
            {
                // 读取0x1194-0x1199共6个寄存器
                ushort[] data = _modbusMaster.ReadInputRegisters(SlaveAddress, STATUS_REG, 6);
                
                return new GripperStatus
                {
                    InitStatus = data[0],      // 0x1194: 初始化状态
                    FaultCode = data[1],       // 0x1195: 故障码
                    GripState = data[2],       // 0x1196: 夹持状态
                    CurrentPosition = data[3], // 0x1197: 当前位置
                    CurrentSpeed = data[4],    // 0x1198: 当前速度
                    CurrentForce = data[5]     // 0x1199: 当前力矩
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"读取详细状态失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        /// <summary>
        /// 读取当前位置
        /// </summary>
        /// <returns>当前位置（0.01mm）</returns>
        public ushort ReadCurrentPosition()
        {
            try
            {
                return _modbusMaster.ReadInputRegisters(SlaveAddress, CURRENT_POS_REG, 1)[0];
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 读取夹持状态
        /// </summary>
        /// <returns>夹持状态值</returns>
        public ushort ReadGripState()
        {
            try
            {
                return _modbusMaster.ReadInputRegisters(SlaveAddress, GRIP_STATE_REG, 1)[0];
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 读取故障码
        /// </summary>
        /// <returns>故障码</returns>
        public ushort ReadFaultCode()
        {
            try
            {
                return _modbusMaster.ReadInputRegisters(SlaveAddress, FAULT_STATUS_REG, 1)[0];
            }
            catch
            {
                return 0;
            }
        }

        #endregion

        #region 等待动作完成

        /// <summary>
        /// 等待移动完成（轮询夹持状态直到到位或超时）
        /// </summary>
        /// <param name="timeoutMs">超时时间（毫秒），默认10000ms</param>
        /// <returns>是否成功到位</returns>
        public bool WaitMovementComplete(int timeoutMs = 10000)
        {
            var start = Environment.TickCount;
            while (Environment.TickCount - start < timeoutMs)
            {
                try
                {
                    var status = ReadDetailedStatus();
                    if (status != null)
                    {
                        // 检查是否到位（GripState = 1表示运行中，其他值表示完成）
                        if (status.GripState != GRIP_MOVING)
                        {
                            return true;
                        }
                    }
                    System.Threading.Thread.Sleep(100);
                }
                catch
                {
                    System.Threading.Thread.Sleep(100);
                }
            }
            return false;
        }

        /// <summary>
        /// 检查是否正在运动
        /// </summary>
        /// <returns>是否正在运动</returns>
        public bool IsMoving()
        {
            return ReadGripState() == GRIP_MOVING;
        }

        #endregion

        #region 组合动作（示例流程）

        /// <summary>
        /// 完整的夹取流程（激活→关闭→等待完成）
        /// </summary>
        /// <param name="closePosition">关闭位置（0.01mm）</param>
        /// <returns>是否成功</returns>
        public bool GripSequence(ushort closePosition = 2600)
        {
            // 1. 确保已就绪
            if (!IsReady())
            {
                MessageBox.Show("电爪未就绪，请先激活！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // 2. 全速全力关闭到指定位置
            if (!CloseToPosition(closePosition))
            {
                return false;
            }

            // 3. 等待动作完成
            if (!WaitMovementComplete(10000))
            {
                MessageBox.Show("夹取超时！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // 4. 检查是否夹到工件
            var status = ReadDetailedStatus();
            if (status != null && status.GripState == GRIP_HOLDING)
            {
                return true; // 成功夹到工件
            }

            return true; // 动作完成（即使没夹到工件）
        }

        /// <summary>
        /// 完整的释放流程（打开→等待完成）
        /// </summary>
        /// <returns>是否成功</returns>
        public bool ReleaseSequence()
        {
            // 1. 全速全力打开
            if (!OpenToZero())
            {
                return false;
            }

            // 2. 等待动作完成
            if (!WaitMovementComplete(10000))
            {
                MessageBox.Show("打开超时！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        #endregion
    }

    /// <summary>
    /// 电爪详细状态数据结构
    /// </summary>
    public class GripperStatus
    {
        public ushort InitStatus { get; set; }      // 初始化状态（0x1194）
        public ushort FaultCode { get; set; }       // 故障码（0x1195）
        public ushort GripState { get; set; }       // 夹持状态（0x1196）
        public ushort CurrentPosition { get; set; } // 当前位置（0x1197）
        public ushort CurrentSpeed { get; set; }    // 当前速度（0x1198）
        public ushort CurrentForce { get; set; }    // 当前力矩（0x1199）

        public override string ToString()
        {
            return $"状态={InitStatus}, 故障={FaultCode}, 夹持={GripState}, 位置={CurrentPosition}, 速度={CurrentSpeed}, 力矩={CurrentForce}";
        }
    }
}
