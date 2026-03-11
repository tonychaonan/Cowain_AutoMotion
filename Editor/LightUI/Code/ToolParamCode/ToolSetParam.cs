using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cowain
{
    public class ToolSetParamBase
    {
        public string stepName = "";
        public string ObjectId = "";
        public ToolSetParamBase(string stepName1, string ObjectId1)
        {
            stepName = stepName1;
            ObjectId = ObjectId1;
        }
    }
}
