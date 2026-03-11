using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cowain_AutoMotion
{
    public interface IHardParam
    {
        string CName { get; set; }
        string ID { get; set; }
    }
    public interface IHardParam1
    {
        string CName { get; set; }
        string ID { get; set; }
        string stationName { get; set; }

    }
    [SugarTable("PWD")]//指定数据库中的表名，要对应数据库的表名，否则会出错
    public class PWD
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]//指定主键和自动增长
        public int ID { get; set; }
        public string CName { get; set; }
        public string EName { get; set; }
        public string UserName { get; set; }
        public string thePassWord { get; set; }
        public string UserID { get; set; }
        public string CardID { get; set; }
    }
    [SugarTable("Cylinders")]
    public class Cylinders : IHardParam
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]//指定主键和自动增长
        public string ID { get; set; } = "0";
        public string CName { get; set; } = "气缸1";
        public string EName { get; set; } = "cylinder1";
        public string OpenIO_ID { get; set; } = "";
        public string OpenSR_ID { get; set; } = "";
        public string CloseIO_ID { get; set; } = "";
        public string CloseSR_ID { get; set; } = "";
        public int OpenWaitTime { get; set; } = 100;
        public int OpenTimeOut { get; set; } = 3000;
        public int CloseWaitTime { get; set; } = 100;
        public int CloseTimeOut { get; set; } = 3000;
    }

    [SugarTable("Inputs")]
    public class Inputs : IHardParam
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]//指定主键和自动增长
        public string ID { get; set; } = "0";
        public string CName { get; set; } = "输入1";
        public string EName { get; set; } = "input1";
        public int CardID { get; set; } = 0;
        public int NodeID { get; set; } = 0;
        public int PortID { get; set; } = 0;
        public int PinID { get; set; } = 0;
        public int isOut { get; set; } = 0;
        public int isInverter { get; set; } = 0;
    }

    [SugarTable("MachineData")]//指定数据库中的表名，要对应数据库的表名，否则会出错
    public class MachineData
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]//指定主键和自动增长
        public int ID { get; set; }= 0;
        public string CName { get; set; } = "点位1";
        public double Enable { get; set; } = 1;
        public double Data1 { get; set; } = 0;
        public double Data2 { get; set; } = 0;
        public double Data3 { get; set; } = 0;
        public double Data4 { get; set; } = 0;
        public double Data5 { get; set; } = 0;
        public double Data1Speed { get; set; } = 100;
        public double Data2Speed { get; set; } = 100;
        public double Data3Speed { get; set; } = 100;
        public double Data4Speed { get; set; } = 100;
        public double Data5Speed { get; set; } = 100;
        public double Data1NoUse { get; set; } = 0;
        public double Data2NoUse { get; set; } = 0;
        public double Data3NoUse { get; set; } = 0;
        public double Data4NoUse { get; set; } = 0;
        public double Data5NoUse { get; set; } = 0;
        public string EName { get; set; } = "pointData";
        public string StationName { get; set; } = "";
        public double PriorityData1 { get; set; } = 1;
        public double PriorityData2 { get; set; } = 1;
        public double PriorityData3 { get; set; } = 1;
        public double PriorityData4 { get; set; } = 1;
        public double PriorityData5 { get; set; } = 1;
        public double ZToSafe { get; set; } = 0;
        public string ZSafePoint { get; set; } = "";
    }
    [SugarTable("Motors")]
    public class Motors : IHardParam
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]//指定主键和自动增长
        public string ID { get; set; } = "M01";
        public string CName { get; set; } = "轴1";
        public string EName { get; set; } = "motor1";
        public int isPlsModule { get; set; } = 0;
        public int CardID { get; set; } = 0;
        public int AxisID { get; set; } = 0;
        public int ModuleMelLogic { get; set; } = 0;
        public int ModulePelLogic { get; set; } = 0;
        public int UnitRev { get; set; } = 10;
        public int PulseRev { get; set; } = 10000;
        public int HomeMode { get; set; } = 8;
        public byte AxisDir { get; set; } = 1;
        public int HiSpeed { get; set; } = 300000;
        public int LoSpeed { get; set; } = 50000;
        public int HomeHiSpeed { get; set; } = 100000;
        public int HomeLoSpeed { get; set; } = 50000;
        public double HiAccTime { get; set; } = 0.1;
        public double HiDesTime { get; set; } = 0.1;
        public double LoAccTime { get; set; } = 0.1;
        public double LoDesTime { get; set; } = 0.1;
        public int HomeAccTime { get; set; } = 100000;
        public int HomeDesTime { get; set; } = 10000;
        public double HomeOffset { get; set; } = 0;
        public double PMaxPos { get; set; } = 5000;
        public double MMinPos { get; set; } = -5000;
        public double P1 { get; set; } = 0;
        public double P2 { get; set; } = 0;
        public int Delay { get; set; } = 1000;
        public int HomeTime { get; set; } = 60000;
        public double ActionTime { get; set; } = 10000;
        public string Unit { get; set; } = "mm";
        public int OutPlsMode { get; set; } = 0;
        public int InPlsMode { get; set; } = 0;
    }

    [SugarTable("Outputs")]
    public class Outputs : IHardParam
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]//指定主键和自动增长
        public string ID { get; set; } = "0";
        public string CName { get; set; } = "输出1";
        public string EName { get; set; } = "output1";
        public int CardID { get; set; } = 0;
        public int NodeID { get; set; } = 0;
        public int PortID { get; set; } = 0;
        public int PinID { get; set; } = 0;
        public int isOut { get; set; } = 0;
        public int isInverter { get; set; } = 0;
    }

    [SugarTable("StationParam")]
    public class StationParam 
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]//指定主键和自动增长
        public int ID { get; set; }= 0;
        public string CName { get; set; } = "工位1";
        public string EName { get; set; } = "station1";
        public string XData1 { get; set; } = "null";
        public string YData2 { get; set; } = "null";
        public string ZData3 { get; set; } = "null";
        public string RData4 { get; set; } = "null";
        public string AData5 { get; set; } = "null";
    }
    [SugarTable("PLCParam")]
    public class PLCParam : IHardParam
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]//指定主键和自动增长
        public string ID { get; set; } = "0";
        public string CName { get; set; } = "PLC1";
        public string EName { get; set; } = "PLC1";
        public string IP { get; set; } = "192.168.1.20";
        public string Port { get; set; } = "502";
        public int station { get; set; } = 1;
        public int isAddressStartWithZero { get; set; } = 1;
        public int isStringReverse { get; set; } = 0;
        public string dataFormat { get; set; } = "CDAB";
        public string type { get; set; } = "信捷";
        public bool Used { get; set; } = false;
    }
    [SugarTable("SocketParam")]
    public class SocketParam : IHardParam
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        public string ID { get; set; } = "0";
        public string CName { get; set; } = "CCD1";
        public string EName { get; set; } = "CCD1";
        public string IP { get; set; } = "127.0.0.1";
        public string Port { get; set; } = "9001";
        public bool isServer { get; set; }= false;
        public bool Used { get; set; } = false;
    }
    [SugarTable("SerialPortParam")]
    public class SerialPortParam : IHardParam
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        public string ID { get; set; } = "0";
        public string CName { get; set; } = "扫码枪";
        public string EName { get; set; } = "scaner";
        public string COMPort { get; set; } = "COM99";
        public int BaudRate { get; set; } = 9600;
        public int DataBit { get; set; } = 8;
        public int StopBit { get; set; } = 1;
        public int ParityBit { get; set; } = 0;
        public bool Used { get; set; } = false;
    }
    [SugarTable("CylindersStationParam")]
    public class CylindersStationParam : IHardParam1
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]//指定主键和自动增长
        public string ID { get; set; } = "0";
        public string CName { get; set; } = "null";
        public string EName { get; set; } = "null";
        public string stationName { get; set; } = "null";
    }
    [SugarTable("InputsStationParam")]
    public class InputsStationParam : IHardParam1
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]//指定主键和自动增长
        public string ID { get; set; } = "0";
        public string CName { get; set; } = "null";
        public string EName { get; set; } = "null";
        public string stationName { get; set; } = "null";
    }
    [SugarTable("OutputsStationParam")]
    public class OutputsStationParam : IHardParam1
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]//指定主键和自动增长
        public string ID { get; set; } = "0";
        public string CName { get; set; } = "null";
        public string EName { get; set; } = "null";
        public string stationName { get; set; } = "null";
    }
    [SugarTable("ButtonParam")]
    public class ButtonParam
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]//指定主键和自动增长
        public string ID { get; set; } = "0";
        public string startButton { get; set; } = "null";
        public string pauseButton { get; set; } = "null";
        public string stopButton { get; set; } = "null";
        public string startButtonLED { get; set; } = "null";
        public string pauseButtonLED { get; set; } = "null";
        public string stopButtonLED { get; set; } = "null";
        public string safeLight { get; set; } = "null";
        public string safeDoor { get; set; } = "null";
        public string emgButton { get; set; } = "null";
        public string greenLED { get; set; } = "null";
        public string yellowLED { get; set; } = "null";
        public string redLED { get; set; } = "null";
        public string buzzerLED { get; set; } = "null";
    }
}
