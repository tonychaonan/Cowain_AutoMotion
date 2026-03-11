using MotionBase;
using OpenOffice_Connect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cowain_AutoMachine.Flow.IO_Cylinder
{
    public class clsIO_Ports : Base
    {
        public clsIO_Ports(Type homeEnum1, Type stepEnum1, string instanceName1, Base parent, int nStation, String strPath, String strCName, int ErrCodeBase)
           : base(homeEnum1,stepEnum1,instanceName1, parent, nStation, strPath, strCName, ErrCodeBase)
        {
            
            MDB = new AddMDBMachineData(strPath);

            MDB.ReadTables("InPuts");

            for (int i = 0; i < MDB.List_Msg.Count; i++)
            {
                string[] str = MDB.List_Msg[i].Split(',');
                if (!IOList.Keys.Contains(str[1]))
                    IOList.Add(str[1], new DrvECatIO(typeof(Base.HomeStep_Base),typeof(Base.Step_Base), str[1], this, 0, str[1], str[0], str[2]));
            }


            MDB.ReadTables("OutPuts");

            for (int i = 0; i < MDB.List_Msg.Count; i++)
            {
                string[] str = MDB.List_Msg[i].Split(',');
                if (!IOList.Keys.Contains(str[1]))
                    IOList.Add(str[1], new DrvECatIO(typeof(Base.HomeStep_Base), typeof(Base.Step_Base), str[1], this, 0, str[1], str[0], str[2]));
            }
        
        }
        ~clsIO_Ports() { }
        AddMDBMachineData MDB;
        /// <summary>
        /// IO集合，利用ID拿到IO
        /// </summary>
        public static Dictionary<string, DrvECatIO> IOList = new Dictionary<string, DrvECatIO>();

        /// <summary>
        /// 输出端口数组
        /// </summary>
        private static DrvIO[] IOPort_Out = new DrvIO[(int)IOPorts_OutPut.MaxCount];
        /// <summary>
        /// 输入端口数组
        /// </summary>
        private static DrvIO[] IOPort_In = new DrvIO[(int)IOPorts_InPut.MaxCount];
        /// <summary>
        /// 定义输入端口枚举
        /// </summary>
        public enum IOPorts_InPut
        {
            /// <summary>
            /// 急停
            /// </summary>
            m_EmgIO = 0,
            /// <summary>
            /// 启动按钮
            /// </summary>
            m_Start,
            /// <summary>
            /// 停止按钮
            /// </summary>
            m_Stop,
            /// <summary>
            /// 复位按钮
            /// </summary>
            m_Reset,
            /// <summary>
            /// 安全光栅
            /// </summary>
            m_Safety,

            /// <summary>
            /// 气压信号
            /// </summary>
            m_AirOk,
            /// <summary>
            /// 安全门
            /// </summary>
            m_DoorSR1,
            m_DoorSR2,
            m_DoorSR3,
            m_DoorSR4,

            MaxCount,
        }
        /// <summary>
        /// 定义输出端口枚举
        /// </summary>
        public enum IOPorts_OutPut
        {
            /// <summary>
            /// 三色灯-红灯
            /// </summary>
            m_LightTowerR,
            /// <summary>
            /// 三色灯-绿灯
            /// </summary>
            m_LightTowerG,
            /// <summary>
            /// 三色灯-黄灯
            /// </summary>
            m_LightTowerY,
            /// <summary>
            /// 蜂鸣器
            /// </summary>
            m_Buzzer,
            /// <summary>
            /// 启动指示灯
            /// </summary>
            m_LightStart,
            /// <summary>
            /// 停止指示灯
            /// </summary>
            m_LightStop,
            /// <summary>
            /// 复位指示灯
            /// </summary>
            m_LightReset,
            MaxCount,
        }
    }
}
