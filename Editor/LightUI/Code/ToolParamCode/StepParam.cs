using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cowain
{
    public class StepRelationParam
    {
        public List<string> from = new List<string>();
        public List<string> to = new List<string>();
        public int locationX = 0;
        public int locationY = 0;
        public string name = "";
        public bool b_IsHead = false;
        public StepRelationParam(string name1, List<string> from1, List<string> to1, int locationX1, int locationY1,bool b_IsHead1)
        {
            name = name1;
            from = from1;
            to = to1;
            locationX = locationX1;
            locationY = locationY1;
            b_IsHead = b_IsHead1;
        }
    }
}
