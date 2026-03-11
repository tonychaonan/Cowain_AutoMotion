using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cowain
{
    public class ToolSetParam_Funtion : ToolSetParamBase
    {
        public string eventParam = "";
        public string eventName = "";
        public string action = "";
        public string jumpParam = "";
        public string jumpStep = "";
        public ToolSetParam_Funtion(string stepName1, string ObjectId1) : base(stepName1, ObjectId1)
        {

        }
    }
}
