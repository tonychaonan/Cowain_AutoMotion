using LightUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cowain
{
    public class ShapeParam
    {
        public bool b_Use = true;
        public string text = "";
        public Point diagramPoint = new Point();
        public string objectId = "";
        public List<string> fromShapeObjectId = new List<string>();
        public List<string> toShapeObjectId = new List<string>();
        public ToolTypeEnum toolTypeEnum1 = ToolTypeEnum.Funtion;
        public ShapeParam()
        {

        }
        public ShapeParam(ToolBase actionItem,ToolTypeEnum toolTypeEnum)
        {
            toolTypeEnum1 = toolTypeEnum;
            text = actionItem.shapeBase.Text;
            diagramPoint = actionItem.shapeBase.Location;
            objectId = actionItem.shapeBase.ObjectId;
            //foreach (var item in actionItem.from)
            //{
            //    fromShapeObjectId.Add(item.shapeBase.ObjectId);
            //}
            //foreach (var item in actionItem.to)
            //{
            //    toShapeObjectId.Add(item.shapeBase.ObjectId);
            //}
        }
    }
}
