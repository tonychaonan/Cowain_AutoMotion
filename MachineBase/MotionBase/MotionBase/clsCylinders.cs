using MotionBase;
using OpenOffice_Connect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionBase
{
    public class clsCylinders : Base
    {
        public clsCylinders(Type homeEnum1, Type stepEnum1, string instanceName1, Base parent, int nStation, String strPath, String strCName, int ErrCodeBase)
           : base(homeEnum1,stepEnum1,instanceName1, parent, nStation, strPath, strCName, ErrCodeBase)
        {

            MDB = new AddMDBMachineData(strPath);

            MDB.ReadTables("Cylinders");

            for (int i = 0; i < MDB.List_Msg.Count; i++)
            {
                string[] str = MDB.List_Msg[i].Split(',');
                if (!ValveList.Keys.Contains(str[2]))
                {
                    DrvValve drvValve = new DrvValve(typeof(HomeStep_Base),typeof(DrvValve.enStep), str[2],this, 0, str[1], str[0], str[2]);
                   // ValveList.Add(str[1], drvValve);
                    AddBase(ref drvValve.m_NowAddress);
                }
            }

        }

        ~clsCylinders() { }
        AddMDBMachineData MDB;

        /// <summary>
        /// 气缸数组
        /// </summary>
        private static DrvValve[] m_Cylinders = new DrvValve[(int)Cylinders.MaxCount];

        /// <summary>
        /// 气缸枚举
        /// </summary>
        public enum Cylinders
        {
            m_vaStage1Up = 0,
            m_vaStage2Up,
            MaxCount
        }
    }

}
