using CanVasLib.Diagram;
using Cowain;
using Cowain.DrawRelation;
using NetronLight;
using ReadAndWriteConfig_NoStatic;
using Sunny.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LightUI
{
    public partial class EditorFrm : UIForm
    {
        ManualClass myManuClass = new ManualClass();
        GetEventClass myGetEventClass;
        ZoomClassManager myZoomClassManager;
        public ShapeWork shapeWork=new ShapeWork ();
        public EditorFrm()
        {
            InitializeComponent();
            myManuClass.paste += pasteShape;
            myManuClass.delete += deleteShape;
            myManuClass.revocation += revocationShape;
            shapeWork.toolBlockManager = new ToolBlockManager(graphControl1);
            myGetEventClass = new GetEventClass();
            this.FormClosing += EditorFrm_FormClosing;
        }

        private void EditorFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            shapeWork.b_Stop = true;
        }

        private void graphControl1_OnShowProps(object ent)
        {
            this.propertyGrid1.SelectedObject = ent;
             myGetEventClass.click(ent as ShapeBase);
        }
        public bool judgePoint(DiagramPoint lineP1, DiagramPoint lineP2, DiagramPoint rectanglePoint)
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
        private void button1_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
            btnSave.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            graphControl1.tidyControl += tidyControl;
            graphControl1.deleteShape += GraphControl1_deleteShape;
            graphControl1.addShape += GraphControl1_addShape;
            myGetEventClass.mouseClick += GraphControl1_doubleClickShape;
            graphControl1.Height = 4000;
            graphControl1.Width = 4000;
            shapeWork.toolBlockManager = new ToolBlockManager(graphControl1);
            showShapeClass showShapeClass1 = new showShapeClass();

            //加载外部传进来的参数
            if(shapeWork.stepRelationParams.Count>0)
            {
                shapeWork.toolBlockManager.myToolBlock.myToolBlockParam.ToolParams.Clear();
            }
            foreach (var item in shapeWork.stepRelationParams)
            {
                ToolParam toolParam = new ToolParam();
                toolParam.shapeParam.diagramPoint = new Point(item.locationX,item.locationY);
                toolParam.shapeParam.toolTypeEnum1 = ToolTypeEnum.Funtion;
                toolParam.shapeParam.objectId = item.name;
                toolParam.shapeParam.fromShapeObjectId = item.from;
                toolParam.shapeParam.toShapeObjectId = item.to;
                toolParam.shapeParam.text= item.name;
                shapeWork.toolBlockManager.myToolBlock.myToolBlockParam.ToolParams.Add(toolParam);
            }
            //创建控件
            if (shapeWork.toolBlockManager.myToolBlock.myToolBlockParam.ToolParams.Count > 0)
            {
                foreach (var item in shapeWork.toolBlockManager.myToolBlock.myToolBlockParam.ToolParams)
                {
                    //根据类型不同，加载不同的控件
                    ShapeBase shape = showShapeClass1.autoAddShape(graphControl1, item.shapeParam.toolTypeEnum1, item.shapeParam.diagramPoint, item.shapeParam.objectId, item.shapeParam.text);
                    graphControl1.AddShape(shape);
                    item.setShapeBase(shape);
                }
                //创建连线
                foreach (var item in shapeWork.toolBlockManager.myToolBlock.myToolBlockParam.ToolParams)
                {
                    ShapeBase shapeBase = graphControl1.Shapes.FirstOrDefault((ShapeBase f) => f.ObjectId == item.shapeParam.objectId);
                    foreach (var item11 in item.shapeParam.toShapeObjectId)
                    {
                        ShapeBase toShapeBase = graphControl1.Shapes.FirstOrDefault((ShapeBase f) => f.ObjectId == item11);
                        Connection connection = graphControl1.AddConnection(((OvalShape)shapeBase).cBottom, ((OvalShape)toShapeBase).cTop);
                        connection.From.OwnerEntity = shapeBase;
                        connection.To.OwnerEntity = toShapeBase;
                        connection.InitialPath();
                    }
                }
                //执行保存一次
                button1_Click(null, null);
            }
            myZoomClassManager = new ZoomClassManager(ref graphControl1);
            shapeWork.start();
        }

        private void GraphControl1_doubleClickShape(string objID)
        {
            ToolParam toolParam = shapeWork.toolBlockManager.myToolBlock.myToolBlockParam.getToolParam(objID);
            switch (toolParam.shapeParam.toolTypeEnum1)
            {
                case ToolTypeEnum.Funtion:
                    FuntionparamFrm funtionparamFrm = new FuntionparamFrm(toolParam.toolSetParam, saveParam);
                    funtionparamFrm.Show();
                    break;
                case ToolTypeEnum.VPP:
                    VppParamFrm vppParamFrm = new VppParamFrm(toolParam.toolSetParam, saveParam);
                    vppParamFrm.Show();
                    break;
            }
        }
        private void saveParam(ToolSetParamBase toolSetParamBase)
        {
            ToolParam toolParam = shapeWork.toolBlockManager.myToolBlock.myToolBlockParam.getToolParam(toolSetParamBase.ObjectId);
            toolParam.toolSetParam = toolSetParamBase;
            shapeWork.toolBlockManager.myToolBlock.myToolBlockParam.addToolParam(toolParam);
        }

        /// <summary>
        /// 添加控件
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="NotImplementedException"></exception>

        private void GraphControl1_addShape(ShapeBase obj)
        {
            shapeWork.toolBlockManager.addShape(obj);
        }

        /// <summary>
        /// 删除控件
        /// </summary>
        /// <param name="obj"></param>
        private void GraphControl1_deleteShape(ShapeBase obj)
        {
            shapeWork.toolBlockManager.deleteShape(obj);
        }

        /// <summary>
        /// 整理控件
        /// </summary>
        private void tidyControl()
        {
            //System.Windows.Forms.Screen scr = System.Windows.Forms.Screen.PrimaryScreen;
            //int screenWidth = scr.Bounds.Width;
            //int X = 80;
            //int Y = 50;
            //// X = (int)(screenWidth / 1287.0 * X);
            //List<ShapeBase> shapeControlList = new List<ShapeBase>();//控件的所有集合
            //foreach (ShapeBase item in graphControl1.Shapes)
            //{
            //    shapeControlList.Add(item);
            //}
            ////先找到CMDshape
            ////首先以CDMshape分割　　找到所有相关联的控件，再把控件按照CMD　camera　calibration　inspection　排列　例如　０　１　２　２　３　，在进行规划
            //int controlIndex = 0;
            //foreach (ShapeBase item1 in graphControl1.Shapes)
            //{
            //    //if (item1 is SimpleRectangleForCMD)
            //    //{
            //    //    List<ShapeBase> shapes = getAllRelationShape((SimpleRectangleForCMD)item1, shapeControlList, false);
            //    //    //整理控件位置
            //    //    shapes.Insert(0, item1);
            //    //    for (int i = 0; i < shapes.Count; i++)
            //    //    {
            //    //        shapes[i].X = 20 + i * X;
            //    //        shapes[i].Y = 20 + controlIndex * Y;
            //    //    }
            //    //    controlIndex++;
            //    //}
            //}
        }
        private void graphControl1_MouseDown(object sender, MouseEventArgs e)
        {
            myManuClass.MouseDown(e);
        }
        private void graphControl1_MouseMove(object sender, MouseEventArgs e)
        {
            if (myManuClass.b_MouseDown)
            {
                Rectangle rt = myManuClass.MouseUp(e);
                drawShape(rt);
                myManuClass.b_MouseDown = true;
            }
        }
        private void graphControl1_MouseUp(object sender, MouseEventArgs e)
        {
            Rectangle rt = myManuClass.MouseUp(e);
            ArrayList list = drawShape(rt);
            if (list.Count > 0)
            {
                myManuClass.addShape(list);
            }
        }
        private ArrayList drawShape(Rectangle rt)
        {
            ArrayList list = new ArrayList();
            List<ShapeBase> shapeControlList = new List<ShapeBase>();//控件的所有集合
            foreach (ShapeBase item in graphControl1.Shapes)
            {
                if (rt.Contains(item.Location))
                {
                    shapeControlList.Add(item);
                    list.Add(item);
                }
            }
            //先找到CMDshape
            //首先以CDMshape分割　　找到所有相关联的控件，再把控件按照CMD　camera　calibration　inspection　排列　例如　０　１　２　２　３　，在进行规划
            foreach (ShapeBase item1 in shapeControlList)
            {
                Graphics g = graphControl1.CreateGraphics();
                Pen p = new Pen(Color.Yellow, 5);
                Rectangle rt1 = new Rectangle(item1.X, item1.Y, item1.Width, item1.Height);
                g.DrawRectangle(p, rt1);
                List<Connection> collection1 = this.graphControl1.connections;
                foreach (var item in shapeControlList)
                {
                    foreach (Connection item11 in collection1)
                    {
                        bool b_getRelation1 = false;
                        bool b_getRelation2 = false;
                        DiagramPoint p1 = ((Connection)item11).To.Point;
                        DiagramPoint p2 = ((Connection)item11).From.Point;
                        int n = item.Connectors.Count;
                        b_getRelation1 = judgePoint(p1, p2, item.Connectors[0].Point);
                        if (n > 1)
                        {
                            b_getRelation2 = judgePoint(p1, p2, item.Connectors[1].Point);
                        }
                        if (b_getRelation1 || b_getRelation2)
                        {
                            //  g.DrawLine(p, p1, p2);
                            bool b_Exist = list.Contains(item11);
                            if (b_Exist != true)
                            {
                                list.Add(item11);
                            }
                            break;
                        }
                    }
                }
            }
            return list;
        }
        private void EditorFrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            myManuClass.paste -= pasteShape;
            myManuClass.delete -= deleteShape;
            myManuClass.revocation -= revocationShape;
            myManuClass.close();
        }
        private void pasteShape(MouseEventArgs e, ArrayList list)
        {
            //int x = 0;
            //int y = 0;
            //if (list.Count > 0)
            //{
            //    if (list[0] is ShapeBase)
            //    {
            //        x = ((ShapeBase)list[0]).X;
            //        y = ((ShapeBase)list[0]).Y;
            //    }
            //    else if (list[0] is Connection)
            //    {
            //        if (((Connection)list[0]).From != null)
            //        {
            //            x = ((Connection)list[0]).From.Point.X;
            //            y = ((Connection)list[0]).From.Point.Y;
            //        }
            //        else
            //        {
            //            x = ((Connection)list[0]).To.Point.X;
            //            y = ((Connection)list[0]).To.Point.Y;
            //        }
            //    }
            //}
            //List<ShapeBase> newShapeBase = new List<ShapeBase>();
            //ArrayList listShape = new ArrayList();
            //foreach (var item in list)
            //{
            //    if (item is ShapeBase != true)
            //    {
            //        continue;
            //    }
            //    string type = ((ShapeBase)item).Text.Split('_')[0];
            //    if (item is SimpleRectangleForCMD)
            //    {
            //        type = "CMD";
            //    }
            //    string text = ((ShapeBase)item).Text;
            //    Point point;
            //    if (e != null)
            //    {
            //        point = new Point(((ShapeBase)item).X - x + e.X, ((ShapeBase)item).Y - y + e.Y);
            //    }
            //    else
            //    {
            //        point = new Point(((ShapeBase)item).X, ((ShapeBase)item).Y);
            //    }
            //    ShapeBase shape = null;
            //    switch (type)
            //    {
            //        case "CMD":
            //            shape = graphControl1.AddShape(ShapeTypes.CMD, point);
            //            shape.Text = text;
            //            break;
            //        case "Camera":
            //            shape = graphControl1.AddShape(ShapeTypes.Rectangular_Camera, point);
            //            shape.Text = text;
            //            break;
            //        case "Calibration":
            //            shape = graphControl1.AddShape(ShapeTypes.Rectangular_Calibration, point);
            //            shape.Text = text;
            //            break;
            //        case "Inspection":
            //            shape = graphControl1.AddShape(ShapeTypes.Rectangular_Inspection, point);
            //            shape.Text = text;
            //            break;
            //    }
            //    newShapeBase.Add(shape);
            //    listShape.Add(shape);
            //}
            //foreach (var item in list)
            //{
            //    if (item is Connection != true)
            //    {
            //        continue;
            //    }
            //    Point p1;
            //    Point p2;
            //    if (e != null)
            //    {
            //        p1 = new Point(((Connection)item).To.Point.X - x + e.X, ((Connection)item).To.Point.Y - y + e.Y);
            //        p2 = new Point(((Connection)item).From.Point.X - x + e.X, ((Connection)item).From.Point.Y - y + e.Y);
            //    }
            //    else
            //    {
            //        p1 = new Point(((Connection)item).To.Point.X, ((Connection)item).To.Point.Y);
            //        p2 = new Point(((Connection)item).From.Point.X, ((Connection)item).From.Point.Y);
            //    }
            //    Connector connector1 = getConnector(newShapeBase, p1);
            //    Connector connector2 = getConnector(newShapeBase, p2);
            //    Connection connect = graphControl1.AddConnection(connector2, connector1);
            //    listShape.Add(connect);
            //}
            //myManuClass.addCopy_ManualHistoryList(listShape);
        }
        private void deleteShape(ArrayList shapes)
        {
            foreach (var item in shapes)
            {
                if (item is ShapeBase)
                {
                    graphControl1.Shapes.Remove((ShapeBase)item);
                }
            }
            foreach (var item in shapes)
            {
                if (item is Connection)
                {
                    graphControl1.connections.Remove((Connection)item);
                }
            }
            graphControl1.Refresh();
        }
        private void revocationShape(ArrayList shapes, ManualType manualType)
        {
            try
            {
                if (shapes.Count > 0)
                {
                    if (manualType == ManualType.copy)
                    {
                        deleteShape(shapes);
                    }
                    else
                    {
                        pasteShape(null, shapes);
                    }
                }
            }
            catch { }
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
          //  shapeWork.toolBlockManager.StepAction();
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            myZoomClassManager.scale = myZoomClassManager.scale + 0.1;
        }
    }
}
