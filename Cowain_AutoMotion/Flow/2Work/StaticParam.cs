using MotionBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cowain_AutoMotion.Flow._2Work
{
    public class IOPort
    {
        public string ID { get; set; }

        public string CName { get; set; }

        public DrvIO Port { get; set; }
    }
    public class StaticParam
    {
        public static DrvMotor AxisY1 = null;
        public static DrvMotor AxisZ1 = null;
        public static DrvMotor AxisX1 = null;
        public static DrvMotor AxisY2 = null;
        public static DrvMotor AxisZ2 = null;
        public static DrvMotor AxisX2 = null;
        public static DrvMotor AxisY3 = null;
        public static DrvMotor AxisR = null;
        public static Dictionary<string, DrvIO> InputDictionary1 = new Dictionary<string, DrvIO>();
        public static Dictionary<string, DrvIO> OutputDictionary = new Dictionary<string, DrvIO>();
        public static Dictionary<string, DrvValve> cylinderDictionary = new Dictionary<string, DrvValve>();
        public static Dictionary<string, DrvMotor> drvMotorDictionary = new Dictionary<string, DrvMotor>();
        public static List<IOPort> InputIOList1 = new List<IOPort>();
        public static List<IOPort> OutputIOList = new List<IOPort>();
        /// <summary>
        /// 轴点位字典
        /// </summary>
        public static Dictionary<string, clsMachinePoint> MachineAxesPoints = new Dictionary<string, clsMachinePoint>();
        /// <summary>
        /// 各点位数组
        /// </summary>
        public static Sys_Define.tyAXIS_XYZRA[] Points = new Sys_Define.tyAXIS_XYZRA[Enum.GetNames(typeof(EnumPosition)).Length];
    }
    public enum EnumPosition
    {
        前龙门Z1轴待命位 = 0,
        前龙门Z1轴前取料位,
        前龙门Y1轴放料位,
        前龙门Y1轴前取料位,
        前龙门Z1轴放料位,
        横移X轴拍照位,
        调整X轴,
        调整Y轴,
        调整R轴,
        调整X轴待命位,
        调整Y轴待命位,
        调整R轴待命位,
        后龙门Z1轴待命位,
        后龙门Z1轴后取料位,
        后龙门Y1轴待命位,
        后龙门Y1轴后取料位,
        后龙门Y1轴下料位,
        后龙门Z1轴下料位,
        横移X1轴待待命位,
    }
}
