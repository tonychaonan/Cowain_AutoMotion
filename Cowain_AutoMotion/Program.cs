using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cowain_Form.FormView;
using Cowain_Machine.Flow;
using Cowain_AutoMotion.Flow;
using Cowain_AutoMotion.Flow.Hive;
using Cowain_AutoMotion.FormView;
using System.IO;
using Cowain_Machine;
using System.Diagnostics;

namespace Cowain_AutoMotion
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (!HslCommunication.Authorization.SetAuthorizationCode("whx"))
            {
                MessageBox.Show(Cowain.JudgeLanguage.JudgeLag("授权失败！当前程序只能使用8小时！"));
                return;
            }
            System.Diagnostics.Process[] myProcesses = System.Diagnostics.Process.GetProcessesByName("Cowain_AutoMotion");
            if (myProcesses.Length > 1)
            {
                MessageBox.Show("程序已启动,不可重复开启！");
                return;
            }
            Process.Start(Application.StartupPath + @"\SQLOperation\SQLOperation\bin\x64\Debug\SQLOperation.exe");
            string strPath11 = System.IO.Directory.GetCurrentDirectory();
            string strNowPath11 = strPath11.Replace("\\Cowain_AutoMotion\\bin\\x64\\Debug", "");
            StrBaseDic = strNowPath11 + "\\DataBaseData\\CowainConfig\\ParamFile";
            AccessControl = strNowPath11 + "\\DataBaseData\\CowainConfig\\Access Control";
            MESDataDefine.initial();
           // RunDataDefine.ReadParams();
           // MSystemDateDefine.SystemParameter.ReadParams();
            MachineDataDefine.ReadParams();
           // DispenserDataDefine.ReadParams();
           // CCDDataDefine.ReadParams();
            MESDataDefine.ReadParams();
           // LeaserDataDefine.ReadParams();
           // password.ReadParams();
           // ConveyorDataDefine.ReadParams();
           //// RunDataDefine.InitData();
            HIVEDataDefine.ReadParams();
            Connections.Instance.initial();
            // WeighGlueDefine.ReadParams();

            Process curProcess = Process.GetCurrentProcess();
            curProcess.PriorityClass = ProcessPriorityClass.High;
            Application.Run(new frm_Main());

        }
        public static string StrBaseDic="";
        public static string AccessControl = "";
    }
}
