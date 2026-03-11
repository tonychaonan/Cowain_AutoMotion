using Cowain_Machine.Flow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cowain_AutoDispenser.Flow
{
    public class Cls_OtherMechineAuto
    {
        private static readonly Cls_OtherMechineAuto instace = new Cls_OtherMechineAuto();
        public Cls_OtherMechine otherMechine1;
        public Cls_OtherMechine otherMechine2;
        public Cls_OtherMechine otherMechine3;
        private Cls_OtherMechineAuto()
        {
            //otherMechine1 = new Flow.Cls_OtherMechine(MachineDataDefine.OtherParam.OtherMechineIP_1, MachineDataDefine.OtherParam.OtherMechinePort_1, MachineDataDefine.OtherParam.OtherMechineAdress_1, MachineDataDefine.OtherParam.b_EnableConnectToOtherMechine_1, MachineDataDefine.OtherParam.machineType_1, MachineDataDefine.OtherParam.HolderCount_1);
            //otherMechine2 = new Flow.Cls_OtherMechine(MachineDataDefine.OtherParam.OtherMechineIP_2, MachineDataDefine.OtherParam.OtherMechinePort_2, MachineDataDefine.OtherParam.OtherMechineAdress_2, MachineDataDefine.OtherParam.b_EnableConnectToOtherMechine_2, MachineDataDefine.OtherParam.machineType_2, MachineDataDefine.OtherParam.HolderCount_2);
            //otherMechine3 = new Flow.Cls_OtherMechine(MachineDataDefine.OtherParam.OtherMechineIP_3, MachineDataDefine.OtherParam.OtherMechinePort_3, MachineDataDefine.OtherParam.OtherMechineAdress_3, MachineDataDefine.OtherParam.b_EnableConnectToOtherMechine_3, MachineDataDefine.OtherParam.machineType_3, MachineDataDefine.OtherParam.HolderCount_3);
        }
        public static Cls_OtherMechineAuto CreateInstance()
        {
            return instace;
        }
        public static bool getResult()
        {
            bool b_Result = false;
            bool b_Result1 = instace.otherMechine1.getResult();
            bool b_Result2 = instace.otherMechine2.getResult();
            bool b_Result3 = instace.otherMechine3.getResult();
            if (b_Result1 && b_Result2 && b_Result3)
            {
                b_Result = true;
            }
            return b_Result;
        }
    }
}
