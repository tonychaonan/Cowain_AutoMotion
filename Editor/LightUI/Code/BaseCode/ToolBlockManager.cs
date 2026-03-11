using CanVasLib.Diagram;
using Cowain;
using NetronLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.Control;

namespace LightUI
{
    public class ToolBlockManager
    {
        public ToolBlock myToolBlock;
        private GraphControl graphControl1;
        public ToolBlockManager(GraphControl graphControl)
        {
            graphControl1 = graphControl;
            myToolBlock = new ToolBlock(null);
        }
        public List<ShapeBase> getShapeBase(Connector connector)
        {
            List<ShapeBase> shapeBaseList = new List<ShapeBase>();
            foreach (var item in graphControl1.connections)
            {
                DiagramPoint p1 = ((Connection)item).To.Point;
                DiagramPoint p2 = ((Connection)item).From.Point;
                bool b_getRelation1 = judgePoint(p1, p2, connector.Point);
                if (b_getRelation1)
                {
                    if (connector.ConnectorDirection == ConnectorDirection.Down)
                    {
                        shapeBaseList.Add((ShapeBase)item.To.OwnerEntity);
                    }
                    else if (connector.ConnectorDirection == ConnectorDirection.Up)
                    {
                        shapeBaseList.Add((ShapeBase)item.From.OwnerEntity);
                    }
                }
            }
            return shapeBaseList;
        }
        private bool judgePoint(DiagramPoint lineP1, DiagramPoint lineP2, DiagramPoint rectanglePoint)
        {
            bool result = false;
            if (Math.Sqrt((lineP1.X - rectanglePoint.X) * (lineP1.X - rectanglePoint.X) + (lineP1.Y - rectanglePoint.Y) * (lineP1.Y - rectanglePoint.Y)) < 10)
            {
                result = true;
            }
            if (Math.Sqrt((lineP2.X - rectanglePoint.X) * (lineP2.X - rectanglePoint.X) + (lineP2.Y - rectanglePoint.Y) * (lineP2.Y - rectanglePoint.Y)) < 10)
            {
                result = true;
            }
            return result;
        }
        public void deleteShape(ShapeBase item)
        {
            myToolBlock.removeTool(item);
        }
        public void addShape(ShapeBase item)
        {
            ToolBase toolBase = new ToolBase(item);
            myToolBlock.addToolBase(toolBase);
            ToolParam toolParam = new ToolParam(toolBase);
            myToolBlock.myToolBlockParam.addToolParam(toolParam);
        }
    }
}
