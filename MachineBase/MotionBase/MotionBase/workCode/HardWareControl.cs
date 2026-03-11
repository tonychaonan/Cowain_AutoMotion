using MotionBase;
using mySocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Cowain_AutoMotion.SQLSugarHelper;

namespace Cowain_AutoMotion
{
    public class HardWareControl
    {
        public static Action<EnumParam_InputIO> IOStatusDel;
        public static DrvIO getInputIO(EnumParam_InputIO inputIO)
        {
            Dictionary<string, DrvIO> ioList = Base.GetInputsIOList();
            foreach (var item in ioList)
            {
                if (item.Key == inputIO.ToString())
                {
                    return item.Value;
                }
            }
            IOStatusDel?.BeginInvoke(inputIO,null,null);
            return null;
        }
        public static DrvIO getOutputIO(EnumParam_OutputIO outputIO)
        {
            Dictionary<string, DrvIO> ioList = Base.GetOutputsIOList();
            foreach (var item in ioList)
            {
                if (item.Key == outputIO.ToString())
                {
                    return item.Value;
                }
            }
            return null;
        }
        public static DrvMotor getMotor(EnumParam_Axis axis)
        {
            Dictionary<string, DrvMotor> axisList = Base.GetMotorList();
            foreach (var item in axisList)
            {
                if (item.Key == axis.ToString())
                {
                    return item.Value;
                }
            }
            return null;
        }
        public static DrvValve getValve(EnumParam_Valve valve)
        {
            Dictionary<string, DrvValve> axisList = Base.GetValveList();
            foreach (var item in axisList)
            {
                if (item.Key == valve.ToString())
                {
                    return item.Value;
                }
            }
            return null;
        }
        public static MachineData getPoint(EnumParam_Point pointName)
        {
            foreach (var item11 in BaseDataDefine.machineDatas)
            {
                if (item11.CName == pointName.ToString())
                {
                    return item11;
                }
            }
            return null;
        }
        public static void movePoint(EnumParam_Point pointName)
        {
            BaseDataDefine.clsPointsMoveManage.movePoint(pointName);
        }
        public static bool getPointIdel(EnumParam_Point pointName)
        {
            return BaseDataDefine.clsPointsMoveManage.getPointIdel(pointName);
        }
        public static SerialPortClass getRS232Control(EnumParam_ConnectionName connectionName)
        {
            foreach (var item in Connections.Instance.RS232ControlList)
            {
                if (item.Key == connectionName.ToString())
                {
                    return item.Value;
                }
            }
            return null;
        }
        public static ISocket getSocketControl(EnumParam_ConnectionName connectionName)
        {
            foreach (var item in Connections.Instance.SocketControlList)
            {
                if (item.Key == connectionName.ToString())
                {
                    return item.Value;
                }
            }
            return null;
        }
    }
}
