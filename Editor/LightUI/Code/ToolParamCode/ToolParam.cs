using LightUI;
using NetronLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cowain
{
    public class ToolParam
    {
        public ToolSetParamBase toolSetParam;
        public ShapeParam shapeParam;
        public ShapeBase ShapeBase;
        public ToolParam()
        {
            shapeParam = new ShapeParam();
        }
        public ToolParam(ToolBase actionItem)
        {
            shapeParam = new ShapeParam(actionItem, actionItem.shapeBase.toolType);
            switch(actionItem.shapeBase.toolType)
            {
                case ToolTypeEnum.Funtion:
                    toolSetParam = new ToolSetParam_Funtion("UnName", actionItem.shapeBase.ObjectId);
                    break;
                case ToolTypeEnum.ToolBlock:
                    toolSetParam = new ToolSetParamBase("UnName", actionItem.shapeBase.ObjectId);
                    break;
                case ToolTypeEnum.VPP:
                    toolSetParam = new ToolSetParam_VPP("UnName", actionItem.shapeBase.ObjectId);
                    break;
            }
        }
        public void setShapeBase(ShapeBase ShapeBase1)
        {
            ShapeBase = ShapeBase1;
        }
    }
}
