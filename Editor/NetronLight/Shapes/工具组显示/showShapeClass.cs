using CanVasLib.Diagram;
using Cowain;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace NetronLight
{
    public class showShapeClass
    {
        public DiagramPoint refp;
        public List<ShapeBase> initial(GraphControl graphControl, ref MenuItem menu, DiagramPoint refp1, List<ShapeBase> shapes, Action<ShapeBase> addShape)
        {
            refp = refp1;
            string[] strs = Enum.GetNames(typeof(ToolTypeEnum));
            for (int i = 0; i < strs.Length; i++)
            {
                ToolTypeEnum toolTypeEnum = ToolTypeEnum.Funtion;
                Enum.TryParse(strs[i], out toolTypeEnum);
                MenuItem mnuRecShape3 = new MenuItem(strs[i], new EventHandler((sender, e) =>
                {
                    AddShape(graphControl, toolTypeEnum, refp, ref shapes, addShape);
                }));
                menu.MenuItems.Add(mnuRecShape3);
            }
            return shapes;
        }

        public ShapeBase AddShape(GraphControl graphControl, ToolTypeEnum type, DiagramPoint location, ref List<ShapeBase> shapes, Action<ShapeBase> addShape)
        {
            ShapeBase shape = autoAddShape(graphControl, type);
            if (shape == null)
            {
                return null;
            }
            shape.Location = location.GetPoint();
            shapes.Add(shape);
            addShape?.Invoke(shape);
            return shape;
        }
        public ShapeBase autoAddShape(GraphControl graphControl, ToolTypeEnum type)
        {
            ShapeBase shape = null;
            switch (type)
            {
                case ToolTypeEnum.ToolBlock:
                    shape = new ToolBlockShape(graphControl);
                    break;
                case ToolTypeEnum.Funtion:
                    shape = new FuntionShape(graphControl);
                    break;
                case ToolTypeEnum.VPP:
                    shape = new VPPShape(graphControl);
                    break;
            }
            return shape;
        }
        public ShapeBase autoAddShape(GraphControl graphControl, ToolTypeEnum type, DiagramPoint location,string objID,string text)
        {
            ShapeBase shape = null;
            switch (type)
            {
                case ToolTypeEnum.ToolBlock:
                    shape = new ToolBlockShape(graphControl);
                    break;
                case ToolTypeEnum.Funtion:
                    shape = new FuntionShape(graphControl);
                    break;
                case ToolTypeEnum.VPP:
                    shape = new VPPShape(graphControl);
                    break;
            }
            shape.Location = new Point(location.X, location.Y);
            shape.ObjectId = objID;
            shape.Text = text;
            return shape;
        }
    }
}
