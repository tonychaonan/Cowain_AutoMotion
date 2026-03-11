using Cowain;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetronLight
{
    public class FuntionShape : OvalShape
    {
        public FuntionShape(GraphControl s) : base(s)
        {
            toolType = ToolTypeEnum.Funtion;
            this.Width = 120;
            this.Height = 40;
            this.ShapeColor = Color.SlateBlue;
            this.Text = "Shape_";
            //  this.BitIcon = "";
        }
    }
}
