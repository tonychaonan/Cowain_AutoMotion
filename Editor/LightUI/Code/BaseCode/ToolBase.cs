using Cowain;
using NetronLight;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LightUI
{
    public class ToolBase
    {
        public ShapeBase shapeBase;
        public string ObjectId = "";
        public ToolBase(ShapeBase shapeBase1)
        {
            if(shapeBase1!=null)
            {
                shapeBase = shapeBase1;
                ObjectId = shapeBase1.ObjectId;
            }
        }
        private void start()
        {
            ((OvalShape)shapeBase).BackGroundColor = Color.LightBlue;
        }
        private void finish()
        {
            ((OvalShape)shapeBase).BackGroundColor = Color.LightGreen;
        }
        private void error()
        {
            ((OvalShape)shapeBase).BackGroundColor = Color.Red;
        }
    }
}
