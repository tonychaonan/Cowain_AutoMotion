using MotionBase;
using OpenOffice_Connect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionBase
{
  public   class clsMotors : Base
    {
        public clsMotors(Type homeEnum1, Type stepEnum1, string instanceName1,Base parent, int nStation, String strPath, String strCName, int ErrCodeBase)
           : base(homeEnum1, stepEnum1, instanceName1,parent, nStation, strPath, strCName, ErrCodeBase)
        {

            MDB = new AddMDBMachineData(strPath);
            MDB.ReadTables("Motors");

            for (int i = 0; i < MDB.List_Msg.Count; i++)
            {
                string[] str = MDB.List_Msg[i].Split(',');
                if (!MotorList.Keys.Contains(str[2]))
                {
                    DrvMotor M = new DrvECatMotor(typeof(Base.HomeStep_Base), typeof(Base.Step_Base), str[2], this, 0, str[1], "", str[2]);
                    AddBase(ref M.m_NowAddress);
                   // MotorList.Add(str[2], M);
                }
            }

        }
        ~clsMotors() { }

        AddMDBMachineData MDB;
        /// <summary>
        /// 电机数组
        /// </summary>
        private  static  DrvMotor[] m_Motor = new DrvMotor[(int)m_Axis.MaxCount];
        /// <summary>
        /// 定义电机枚举
        /// </summary>
        public enum m_Axis
        {
            Dis1_X = 0,
            Dis1_Y,

            MaxCount,
        }
    }
}
