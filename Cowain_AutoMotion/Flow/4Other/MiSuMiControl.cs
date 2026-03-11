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

namespace Cowain_AutoMotion.Flow.Common
{
    public class MiSuMiControl
    {
        private SerialPort _serialPort;       // 串口（USB转485）
        private IModbusMaster _modbusMaster;  // Modbus主站
      //  private readonly byte _slaveAddress = 0x09;   // 夹爪从站ID
                                                      // 核心寄存器地址（新版协议）
                                                      //private const ushort SaveOn = 0x01;      // 使能
                                                      //private const ushort SaveOff = 0x00;      // 去使能
                                                      //private const ushort Position = 0x0FA2;    // 位置
                                                      //private const ushort Speed = 0x0FA3;       // 速度
                                                      //private const ushort Torque = 0x0FA4;      // 扭矩
                                                      //private const ushort State = 0x1194;  // 状态基址

        //public const byte Trigger = 0x08;           // 运动触发位（rGTO=1，对应bit3置1）

        #region 寄存器地址（旧协议，16进制转10进制）
        public const ushort CONTROL_REG = 0x03E8;    // 主控制寄存器（低字节：使能/模式；高字节：预设参数指令）
        public const ushort POS_REG = 0x03E9;        // 动态位置寄存器（高字节有效）
        public const ushort SPEED_FORCE_REG = 0x03EA;// 动态速度（低字节）/力矩（高字节）寄存器
        public const ushort STATUS_REG = 0x07D0;     // 夹爪状态寄存器
        public const ushort FAULT_REG = 0x07D1;      // 故障寄存器（低字节）+位置状态（高字节）
        public const ushort SPEED_STATUS_REG = 0x07D2;//速度状态（低）+力矩状态（高）
        #endregion

        #region 控制寄存器位定义（0x03E8低字节）
        public const byte ACT_ENABLE = 0x01;         // 使能位（rACT=1）
        public const byte ACT_DISABLE = 0x00;        // 去使能位（rACT=0）
        public const byte MODE_DYNAMIC = 0x00;       // 动态参数模式（rMODE=0）
        public const byte MODE_PRESET = 0x02;        // 预设参数模式（rMODE=1，对应bit1置1）
        public const byte GTO_MOVE = 0x08;           // 运动触发位（rGTO=1，对应bit3置1）
        #endregion

        #region 预设参数指令（0x03E8高字节）
        public const byte PRESET_FULL_OPEN = 0x03;   // 全力全速打开
        public const byte PRESET_FULL_CLOSE = 0x04;  // 全力全速关闭
        public const byte PRESET_HALF_OPEN = 0x01;   // 半力半速打开
        public const byte PRESET_HALF_CLOSE = 0x02;  // 半力半速关闭
        public const byte PRESET_LOW_OPEN = 0x05;    // 低力低速打开
        public const byte PRESET_LOW_CLOSE = 0x06;   // 低力低速关闭
        #endregion

        #region 故障码定义（0x07D1低字节）
        public const byte FAULT_NONE = 0x00;         // 无故障
        public const byte FAULT_NO_ACT = 0x01;       // 电机未激活
        public const byte FAULT_CMD_ERROR = 0x02;    // 控制指令错误
        public const byte FAULT_COMM_LOST = 0x04;    // 通讯丢失（1s无数据）
        public const byte FAULT_OVER_CURRENT = 0x08; // 过流
        public const byte FAULT_VOLTAGE = 0x10;      // 电压异常（<20V/>30V）
        public const byte FAULT_ENABLE = 0x20;       // 使能故障
        public const byte FAULT_OVER_TEMP = 0x40;    // 过温（≥85℃）
        public const byte FAULT_DEVICE = 0x80;       // 产品自身故障
        #endregion

        #region 夹爪状态（0x07D0低字节）
        public const byte STATUS_ACTIVED = 0x31;     // 激活完成（0x31=00110001）
        public const byte STATUS_MOVING = 0x71;      // 运行中
        public const byte STATUS_ARRIVED = 0xF1;     // 运动到位
        #endregion

        #region Modbus参数
        public const byte SLAVE_ID = 0x09;           // 从站地址（默认9，可修改）
        public const int BAUD_RATE = 115200;         // 波特率（默认115200）
        public const Parity PARITY = Parity.None;    // 校验位：无
        public const int DATA_BITS = 8;              // 数据位：8
        public const StopBits STOP_BITS = StopBits.One;//停止位：1

        private bool _isConnected = false;
        /// <summary>
        /// 连接状态
        /// </summary>
        public bool IsConnected => _isConnected;
        /// <summary>
        /// 初始化串口并连接Modbus主站
        /// </summary>
        /// <param name="portName">串口号</param>
        /// <exception cref="Exception">连接失败异常</exception>
        public void Connect(string portName)
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
                MessageBox.Show("夹爪连接成功！");
            }
            catch (Exception ex)
            {
                _isConnected = false;
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

        /// <summary>
        /// 夹爪去使能（清除所有故障/复位）
        /// </summary>
        public void Disable()
        {

            // 写入控制寄存器：0x0000（rACT=0，去使能）
            _modbusMaster.WriteSingleRegister(SLAVE_ID, CONTROL_REG, 0x0000);
            Console.WriteLine("夹爪已去使能！");
        }



        /// <summary>
        /// 使能夹爪（必须先执行，否则无法运动）
        /// </summary>
        /// <returns>是否成功</returns>
        public bool EnableGripper()
        {
            try
            {
                // Modbus写单个寄存器：0x0FA0 = 1（搜索行程使能）
                _modbusMaster.WriteSingleRegister(_slaveId, SaveOn, 1);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"使能失败：{ex.Message}");
                return false;
            }
        }
        /// <summary>
        /// 等待使能完成（必须等状态=3才能继续）
        /// </summary>
        /// <param name="timeoutMs">超时时间，默认3000ms</param>
        /// <returns>是否完成</returns>
        public bool WaitEnableComplete(int timeoutMs = 3000)
        {
            var start = Environment.TickCount;
            while (Environment.TickCount - start < timeoutMs)
            {
                // 读使能状态寄存器（0x1194）
                var state = _modbusMaster.ReadInputRegisters(_slaveId, State, 1)[0];
                if (state == 3) return true; // 3=使能完成
                System.Threading.Thread.Sleep(100);
            }
            return false;
        }
        /// <summary>
        /// 控制夹爪移动（核心：Modbus连续写寄存器）
        /// </summary>
        /// <param name="position">目标位置（0.01mm）</param>
        /// <param name="speed">速度1-100</param>
        /// <param name="torque">扭矩1-100</param>
        /// <returns>是否成功</returns>
        public bool MoveGripper(ushort position, ushort speed = 60, ushort torque = 50)
        {
            try
            {
                // 连续写入4个寄存器：位置、速度、扭矩、触发（1=触发）
                ushort[] data = new ushort[] { position, speed, torque, 1 };
                _modbusMaster.WriteMultipleRegisters(_slaveId, Position, data);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"移动失败：{ex.Message}");
                return false;
            }
        }
        /// <summary>
        /// 快速全开（封装常用指令）
        /// </summary>
        public bool OpenFull() => MoveGripper(0, 100, 100);

        /// <summary>
        /// 快速全闭（封装常用指令）
        /// </summary>
        public bool CloseFull() => MoveGripper(5000, 100, 100);
        /// <summary>
        /// 读取当前位置（Modbus读输入寄存器）
        /// </summary>
        /// <returns>当前位置（0.01mm）</returns>
        public ushort ReadCurrentPosition()
        {
            // 读0x1197寄存器（位置）
            return _modbusMaster.ReadInputRegisters(_slaveId, State + 3, 1)[0];
        }
        /// <summary>
        /// 读取夹持状态
        /// </summary>
        /// <returns>0=默认，1=运行中，2=到位，3=夹到工件，4=工件掉落</returns>
        public ushort ReadGripState()
        {
            // 读0x1196寄存器（夹持状态）
            return _modbusMaster.ReadInputRegisters(_slaveId, State + 2, 1)[0];
        }

        /// <summary>
        /// 读取故障码
        /// </summary>
        /// <returns>故障码（0=无故障）</returns>
        public ushort ReadFaultCode()
        {
            // 读0x1195寄存器（故障码）
            return _modbusMaster.ReadInputRegisters(_slaveId, State + 1, 1)[0];
        }



    }
}
