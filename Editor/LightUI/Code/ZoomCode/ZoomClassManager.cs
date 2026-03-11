using NetronLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cowain
{
    public class ZoomClassManager
    {
        private double scale1 = 1;
        GraphControl graphControl;
        public double scale
        {
            get
            {
                return scale1;
            }
            set
            {
                if (scale1 != value)
                {
                    scale1 = value;
                    changeSize(scale1);
                }
            }
        }
        public ZoomClassManager(ref GraphControl graphControl1)
        {
            graphControl = graphControl1;
        }
        private void changeSize(double scale_)
        {
            foreach (var item in graphControl.Shapes)
            {
                int width = (int)(item.Width * scale_);
                int height = (int)(item.Height * scale_);
                item.Resize(width, height);
                double scale1 = (scale_ - 1) / 2;
                int x = (int)(item.Location.X * (scale1 + 1));
                int y = (int)(item.Location.Y * (scale1 + 1));
                item.Location = new System.Drawing.Point(x, y);
            }
            //更新connect
            foreach (var item in graphControl.connections)
            {
                if (item.From.OwnerEntity != null)
                {
                    ShapeBase shapeBase = (ShapeBase)item.From.OwnerEntity;
                    item.From.Point = new CanVasLib.Diagram.DiagramPoint();
                }
            }
        }
    }
}
