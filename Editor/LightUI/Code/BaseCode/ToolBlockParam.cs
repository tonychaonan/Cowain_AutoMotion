using CanVasLib.Diagram;
using Cowain;
using NetronLight;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightUI
{
    public class ToolBlockParam 
    {
        public List<ToolParam> ToolParams = new List<ToolParam>();
        public List<ToolParam> getStartActions()
        {
            List<ToolParam> myShapeItemParams = new List<ToolParam>();
            foreach (var item in ToolParams)
            {
                if (item.shapeParam.fromShapeObjectId.Count == 0)
                {
                    myShapeItemParams.Add(item);
                }
            }
            return myShapeItemParams;
        }
        public ToolParam getToolParam(string objectId)
        {
            foreach (var item in ToolParams)
            {
                if(item.shapeParam.objectId== objectId)
                {
                    return item;
                }
            }
            ToolParam toolParam1= new ToolParam();
            toolParam1.shapeParam.objectId = objectId;
            ToolParams?.Add(toolParam1);
            return toolParam1;
        }
        public ToolParam getToolParam(ShapeBase shapeBase)
        {
            foreach (var item in ToolParams)
            {
                if (item.shapeParam.objectId == shapeBase.ObjectId)
                {
                    return item;
                }
            }
            ToolParam toolParam1 = new ToolParam(new ToolBase(shapeBase));
            ToolParams.Add(toolParam1);
            return toolParam1;
        }
        public void clear()
        {
            ToolParams.Clear();
        }
        public void removeToolParam(string objectId)
        {
            foreach (var item in ToolParams)
            {
                if (item.shapeParam.objectId == objectId)
                {
                    ToolParams.Remove(item);
                    break;
                }
            }
        }
        public void addToolParam(ToolParam toolParam)
        {
            foreach (var item in ToolParams)
            {
                if (item.shapeParam.objectId == toolParam.shapeParam.objectId)
                {
                    ToolParams.Remove(item);
                    break;
                }
            }
            ToolParams.Add(toolParam);
        }
    }
}
