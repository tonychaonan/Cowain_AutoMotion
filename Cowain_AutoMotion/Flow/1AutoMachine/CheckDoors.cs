using Cowain_Machine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cowain_AutoMotion
{
    public class CheckDoors
    {
        /// <summary>
        /// 安全们被打开，为true
        /// </summary>
        /// <returns></returns>
        public static bool openDoor()
        {
            if (MachineDataDefine.machineState.b_UseDoorCheck)
            {
                //bool door1 = HardWareControl.getInputIO(EnumParam_InputIO.门禁1).GetValue();
                //bool door2 = HardWareControl.getInputIO(EnumParam_InputIO.门禁2).GetValue();
                //bool door3 = HardWareControl.getInputIO(EnumParam_InputIO.门禁3).GetValue();
                //bool door4 = HardWareControl.getInputIO(EnumParam_InputIO.门禁4).GetValue();
                //bool door5 = HardWareControl.getInputIO(EnumParam_InputIO.门禁5).GetValue();
                //bool door6 = HardWareControl.getInputIO(EnumParam_InputIO.门禁6).GetValue();
                //bool door7 = HardWareControl.getInputIO(EnumParam_InputIO.门禁7).GetValue();
                //bool door8 = HardWareControl.getInputIO(EnumParam_InputIO.门禁8).GetValue();

                //if ((door1 && door2 && door3 && door4 && door5 && door6 && door7 && door8) != true)
                //{
                //    return true;
                //}
                bool door1 = HardWareControl.getInputIO(EnumParam_InputIO.右后安全门).GetValue();
                bool door2 = HardWareControl.getInputIO(EnumParam_InputIO.左后安全门).GetValue();
                bool door3 = HardWareControl.getInputIO(EnumParam_InputIO.左安全门).GetValue();
                bool door4 = HardWareControl.getInputIO(EnumParam_InputIO.右安全门).GetValue();
                if ((door1 && door2 && door3 && door4) != true)
                {
                    return true;
                }
                MachineDataDefine.IsFormOpen = false;
            }
            return false;
        }
    }
}
