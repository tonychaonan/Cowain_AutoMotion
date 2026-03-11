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
    public class ToolBlock : ToolBase
    {
        public List<ToolBase> totalToolBases = new List<ToolBase>();
        public List<ToolBase> startTools = new List<ToolBase>();
        public ToolBlockParam myToolBlockParam;
        public ToolBlock(ShapeBase shapeBase1) : base(shapeBase1)
        {
            myToolBlockParam = new ToolBlockParam();
        }
        public void addStartToolBase(ShapeBase shapeBase)
        {
            foreach (var item in startTools)
            {
                if (item.ObjectId == shapeBase.ObjectId)
                {
                    startTools.Remove(item);
                    break;
                }
            }
            ToolBase toolBase = getToolBaseItem(shapeBase);
            startTools.Add(toolBase);
        }
        public void addToolBase(ToolBase shapeBase)
        {
            foreach (var item in totalToolBases)
            {
                if (item.ObjectId == shapeBase.ObjectId)
                {
                    totalToolBases.Remove(item);
                    break;
                }
            }
            totalToolBases.Add(shapeBase);
            ToolParam toolParam = new ToolParam(shapeBase);
            myToolBlockParam.addToolParam(toolParam);
        }
        public void clear()
        {
            totalToolBases.Clear();
            startTools.Clear();
        }
        public void removeTool(ShapeBase shapeBase)
        {
            foreach (var item in totalToolBases)
            {
                if (item.shapeBase == shapeBase)
                {
                    totalToolBases.Remove(item);
                    break;
                }
            }
            foreach (var item in startTools)
            {
                if (item.shapeBase == shapeBase)
                {
                    startTools.Remove(item);
                    break;
                }
            }
            myToolBlockParam.removeToolParam(shapeBase.ObjectId);
        }
        public ToolBase getToolBaseItem(ShapeBase shapeBase)
        {
            foreach (var item in totalToolBases)
            {
                if (item.shapeBase == shapeBase)
                {
                    return item;
                }
            }
            ToolBase actionItem = new ToolBase(shapeBase);
            actionItem.shapeBase = shapeBase;
            totalToolBases.Add(actionItem);
            return actionItem;
        }
    }
}
