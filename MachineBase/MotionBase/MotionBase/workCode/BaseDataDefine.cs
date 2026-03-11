using Cowain_AutoMotion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Cowain_AutoMotion.SQLSugarHelper;

namespace MotionBase
{
    public class BaseDataDefine
    {
        /// <summary>
        /// 点位的站位
        /// </summary>
        public static List<StationParam> stationParams = DBContext<StationParam>.GetInstance().GetList();
        /// <summary>
        /// 点位的所有集合
        /// </summary>
        public static List<MachineData> machineDatas = DBContext<MachineData>.GetInstance().GetList();
        public static clsPointsMoveManage clsPointsMoveManage = new clsPointsMoveManage(typeof(Base.HomeStep_Base), typeof(Base.Step_Base),"点位集合");
        /// <summary>
        /// 设备的速度
        /// </summary>
        public static int machineSpeed = 100;
    }
}
