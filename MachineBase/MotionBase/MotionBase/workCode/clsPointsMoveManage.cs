using MotionBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Cowain_AutoMotion.clsPointMove;
using static Cowain_AutoMotion.SQLSugarHelper;

namespace Cowain_AutoMotion
{
    public class clsPointsMoveManage : Base
    {
        public Dictionary<string, clsPointMove> pointThreads = new Dictionary<string, clsPointMove>();
        public clsPointsMoveManage(Type homeEnum1, Type stepEnum1, string instanceName1) : base(homeEnum1,stepEnum1,instanceName1)
        {
            foreach (var item in BaseDataDefine.machineDatas)
            {
                clsPointMove clsPointMove = new clsPointMove(typeof(PointMove_HomeStep),typeof(PointMove_WorkStep), item.CName);
                AddBase(ref clsPointMove.m_NowAddress);
                pointThreads.Add(item.CName, clsPointMove);
            }
        }
        public bool getPointIdel(EnumParam_Point point)
        {
            string CName = point.ToString();
            foreach (var item in pointThreads)
            {
                if (item.Key == CName)
                {
                    return item.Value.isIDLE();
                }
            }
            return false;
        }
        public bool movePoint(EnumParam_Point point)
        {
            string CName = point.ToString();
            MachineData machineData = null;
            foreach (var item in pointThreads)
            {
                if (item.Key == CName)
                {
                    foreach (var item11 in BaseDataDefine.machineDatas)
                    {
                        if (item11.CName == CName)
                        {
                            machineData = item11;
                        }
                    }
                    item.Value.Action(0, machineData);
                    return true;
                }
            }
            if (machineData == null)
            {
                clsPointMove clsPointMove = new clsPointMove(typeof(PointMove_HomeStep), typeof(PointMove_WorkStep), "未知名称点位");
                AddBase(ref clsPointMove.m_NowAddress);
                pointThreads.Add(CName, clsPointMove);
                movePoint(point);
            }
            return false;
        }
        public override void Stop()
        {
            foreach (var item in pointThreads)
            {
                item.Value.Stop();
            }
            string[] axisName = Enum.GetNames(typeof(EnumParam_Axis));
            for (int i = 0; i < axisName.Length; i++)
            {
                EnumParam_Axis axis;
                Enum.TryParse(axisName[i], out axis);
                HardWareControl.getMotor(axis).Stop();
            }
            base.Stop();
        }
    }
}
