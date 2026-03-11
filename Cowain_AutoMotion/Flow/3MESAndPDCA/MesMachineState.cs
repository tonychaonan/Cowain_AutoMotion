using Cowain_AutoMotion.Flow.Hive;
using Cowain_Machine;
using DevExpress.XtraCharts.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cowain_AutoMotion.Flow._3MESAndPDCA
{
    /// <summary>
    /// mes上传机台状态数据类
    /// </summary>
    public class MesMachineState
    {
        public string uniqueID = MachineDataDefine.settingData.Station + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        public string terminal_Name = MESDataDefine.MESLXData.terminalName;
        public string machine_Name = MachineDataDefine.settingData.Station;         //MachineDataDefine.MachineCfgS.StrStationName;
        public string machine_State = "";
        public string state_Change_Time = "";
        public string message = "";
        public string code = "";
        public string serverity = "";
        public string occurrence_Time = "";
        public string resolved_Time = "";
        public string vendor = "COWAIN";
        public string uid = "";
        public string pdline_name = MachineDataDefine.settingData.Line;
        public ErrData data = new ErrData();
    }
    public class ErrData
    {
        public string sW_Version = MESDataDefine.MESLXData.SW_Version;
        public string sequence_id = "1";
        public string code = "";
        public string error_Detail = "";
        public string mS_SHA1 = "";// HIVEDataDefine.Get_SHA1(MachineDataDefine.hashKey.MS_SHA1_Path);
        public string vS_SHA1 = "";//HIVEDataDefine.Get_SHA1(MachineDataDefine.hashKey.VS_SHA1_Path);
        public string cD_SHA1 = "";
    }
   
}
