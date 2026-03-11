using NetronLight;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cowain
{
    public class VPPShape : OvalShape
    {
        public VPPShape(GraphControl s) : base(s)
        {
            toolType = ToolTypeEnum.VPP;
            this.Width = 120;
            this.Height = 40;
            this.ShapeColor = Color.SlateBlue;
            this.Text = "Shape_";
            //  this.BitIcon = "";
        }
    }
}
