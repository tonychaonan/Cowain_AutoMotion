using Cowain_Machine.Flow;
using MotionBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cowain_AutoMotion.Flow
{
    public class CheckDoorSR : Base
    {
        public CheckDoorSR(Base parent, int nStation, String strEName, String strCName, int ErrCodeBase)
           : base(parent, nStation, strEName, strCName, ErrCodeBase)
        {
        }

        ~CheckDoorSR()
        {
        }
        public bool DoorClose = false;
        public string AlarmStr = "";
        public bool DoorSR(int i)
        {
            DoorClose = (UseDoorSR[i] || DoorInput[i].GetValue());
            if (!DoorClose)
                AlarmStr = DoorStr[i];
            return !DoorClose;
        }
        string[] DoorStr =
         {
            "右上门禁SR1",
            "右上门禁SR2",
            "左上门禁SR1",
            "左上门禁SR2",
            //--------------
            "前下门禁SR1",
            "前下门禁SR2",
            "后下门禁SR1",
            "后下门禁SR2",
            //--------------
            "左下门禁SR1",
            "左下门禁SR2",
            "右下门禁SR1",
            "右下门禁SR2"
        };
       public DrvIO[] DoorInput = new DrvIO[12];
        /// <summary>
        /// 禁用门禁  1:true检查     0:false不检查
        /// </summary>
        public bool[] UseDoorSR = { false, false, false, false, false, false, false, false, false, false, false, false };
        public void InitialInPut(string station)
        {
            //string In1 = "X01", In2 = "X05";

            //for (int i = 0; i < 4; i++)
            //    DoorInput[i] = new DrvECatIO(this, m_nStation, In1 + (i + 12).ToString("00"), "", DoorStr[i]);//X0112 X0113 X0114 X0115

            //if (MSystemDateDefine.SystemParameter.Gantry1Parm.turnPartParms[0].bTurnEnable || MSystemDateDefine.SystemParameter.Gantry1Parm.turnPartParms[1].bTurnEnable)
            //{
            //    for (int i = 4; i < 8; i++)
            //        DoorInput[i] = new DrvECatIO(this, m_nStation, In2 + (i).ToString("00"), "", DoorStr[i]);//X0112 X0113 X0114 X0115

            //    for (int i = 8; i < 12; i++)
            //        DoorInput[i] = new DrvECatIO(this, m_nStation, In2 + (i+4).ToString("00"), "", DoorStr[i]);//X0112 X0113 X0114 X0115
            //}
            //else
            //{
            //    for (int i = 4; i < 8; i++)
            //        DoorInput[i] = new DrvECatIO(this, m_nStation, In2 + (i - 4).ToString("00"), "", DoorStr[i]);//X0112 X0113 X0114 X0115

            //    for (int i = 8; i < 12; i++)
            //        DoorInput[i] = new DrvECatIO(this, m_nStation, In2 + (i - 4).ToString("00"), "", DoorStr[i]);//X0112 X0113 X0114 X0115
            //}
            }
    }
}
