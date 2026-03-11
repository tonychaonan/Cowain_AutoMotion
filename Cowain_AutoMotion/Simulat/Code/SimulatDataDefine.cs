using Cowain_Machine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cowain_AutoMotion
{
    public class SimulatDataDefine
    {
        private static SimulatDataDefine simulatDataDefine;
        public SimulatParam simulatParam;
        public static SimulatorFlowManager simulatorFlowManager = new SimulatorFlowManager();
        public static SimulatDataDefine instance()
        {
            if (simulatDataDefine == null)
            {
                simulatDataDefine = new SimulatDataDefine();
            }
            return simulatDataDefine;
        }
        public SimulatDataDefine()
        {
            if (simulatParam == null)
            {
                simulatParam = new SimulatParam();
                simulatParam.ReaderParams(Program.StrBaseDic, "SimulatParam", ref simulatParam);
            }
        }
        public void saveParam()
        {
            simulatParam.SetSaveFile(Program.StrBaseDic, "SimulatParam", simulatParam);
        }
    }
    public class buttonAndStep
    {
        public DataGridViewCell btn;
        public SimulatParamItem1 simulatParamItem1;
        public buttonAndStep(DataGridViewCell btn1, SimulatParamItem1 simulatParamItem11)
        {
            btn = btn1;
            simulatParamItem1 = simulatParamItem11;
        }
    }
}
