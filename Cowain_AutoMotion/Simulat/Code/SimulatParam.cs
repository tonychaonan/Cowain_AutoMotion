using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cowain_AutoMotion
{
    /// <summary>
    /// 仿真测试参数集合
    /// </summary>
    public class SimulatParam : JsonHelper
    {
        public List<SimulatParamItem> simulatParamItems = new List<SimulatParamItem>();
    }
    public class SimulatParamItem
    {
        public bool b_Use = true;
        private string systemName1 = "";
        public string systemName
        {
            get
            {
                return systemName1;
            }
            set
            {
                systemName1 = value;
            }
        }
        public List<SimulatParamItem1> simulatParamItem1s;
        public SimulatParamItem()
        {
            systemName1 = "SimulatParamItem" + DateTime.Now.ToString("HHmmssfff");
            simulatParamItem1s = new List<SimulatParamItem1>();
        }
    }
    public class SimulatParamItem1
    {
        /// <summary>
        /// 条件步骤
        /// </summary>
        public StepEnum stepEnum = StepEnum.Null;
        /// <summary>
        /// 步骤的实例
        /// </summary>
        public string instanceStr="Null";
        /// <summary>
        /// 步骤
        /// </summary>
        public string stepStr = "Null";
        /// <summary>
        /// 结果
        /// </summary>
        public string valueStr = "Null";
        public IfStep getIfStep()
        {
            IfStep ifStep = IfStep.Null;
            Enum.TryParse(stepStr, out ifStep);
            return ifStep;
        }
        public ExecuteStep GetSExecutetep()
        {
            ExecuteStep executeStep = ExecuteStep.Null;
            Enum.TryParse(stepStr, out executeStep);
            return executeStep;
        }
        public SimulatParamItem1(StepEnum stepEnum1, string instance1, string stepStr1, string value1)
        {
            stepEnum = stepEnum1;
            instanceStr = instance1;
            stepStr = stepStr1;
            valueStr = value1;
        }
        public SimulatParamItem1()
        {

        }
        public string getStr()
        {
            string str = "<" + stepEnum.ToString() + ">" + "<" + stepStr + ">"
                     + "<" + instanceStr + ">" + "<" + valueStr + ">";
            return str;
        }
    }
}
