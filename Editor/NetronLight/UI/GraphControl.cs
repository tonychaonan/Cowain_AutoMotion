using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using CanVasLib.Diagram;
using System.Collections.Generic;
using System.Linq;

namespace NetronLight
{
    /// <summary>
    /// A 'light' version of the Netron graph control
    /// without all the advanced diagramming stuff
    /// see however http://netron.sf.net for more info.
    /// This control shows the simplicity with which you can still achieve good results,
    /// it's a toy-model to explore and can eventually help you if you want to go for a 
    /// bigger adventure in the full Netron control.
    /// Question and comments are welcome via the forum of The Netron Project or mail me
    /// [Illumineo@users.sourceforge.net]
    /// 
    /// Thank you for downloading the code and your feedback!
    /// 
    /// </summary>
    public class GraphControl : ScrollableControl
    {
        private bool _leftMouse;
        private bool _tracking;
        private DiagramPoint _stratPoint = new DiagramPoint();
        public event EventHandler<SelectElementChangedEventArgs> OnSelectElementChanged;
        public event EventHandler<DeletingEventArgs> OnElementDeleting;
        public event EventHandler<EventArgs> OnElementDeleted;

        #region Events and delegates
        /// <summary>
        /// the info coming with the show-props event
        /// </summary>
        public delegate void ShowProps(object ent);

        /// <summary>
        /// notifies the host to show the properties usually in the property grid
        /// </summary>
        public event ShowProps OnShowProps;
        /// <summary>
        /// 整理控件时发生
        /// </summary>
        public event Action tidyControl;
        /// <summary>
        /// 删除shape时发生
        /// </summary>
        public event Action<ShapeBase> deleteShape;
        /// <summary>
        /// 新增控件时发生
        /// </summary>
        public event Action<ShapeBase> addShape;
        #endregion

        #region Fields
        [Browsable(true)]
        [Description("连线默认颜色")]
        [Category("Layout")]
        public Color LineColor { get; set; } = Color.Silver;


        [Browsable(true)]
        [Description("连线选中颜色")]
        [Category("Layout")]
        public Color LineSelectedColor { get; set; } = Color.Green;


        [Browsable(true)]
        [Description("连线悬停颜色")]
        [Category("Layout")]
        public Color LineHoveredColor { get; set; } = Color.Blue;


        [Browsable(true)]
        [Description("视觉原点，左上角坐标")]
        [Category("Layout")]
        public DiagramPoint ViewOriginPoint { get; set; } = new DiagramPoint(0, 0);
        public DiagramPoint ViewOriginPoint_Flow { get; set; } = new DiagramPoint(0, 0);
        /// <summary>
        /// the collection of shapes on the canvas
        /// </summary>
        protected List<ShapeBase> shapes;

        /// <summary>
        /// the collection of connections on the canvas
        /// </summary>
        [Browsable(false)]
        public List<Connection> connections { get; set; } = new List<Connection>();
        /// <summary>
        /// the entity hovered by the mouse
        /// </summary>
        protected Entity hoveredEntity;
        /// <summary>
        /// the unique entity currently selected
        /// </summary>
        protected Entity selectedEntity;
        /// <summary>
        /// whether we are tracking, i.e. moving something around
        /// </summary>
        protected bool tracking = false;
        DiagramPoint refp1;
        /// <summary>
        /// just a reference point for the OnMouseDown event
        /// </summary>
        protected DiagramPoint refp
        {
            get
            {
                return refp1;
            }
            set
            {
                refp1 = value;
                showShapeClass.refp = value;
            }
        }
        showShapeClass showShapeClass = new showShapeClass();
        /// <summary>
        /// the context menu of the control
        /// </summary>
        protected ContextMenu menu;
        /// <summary>
        /// A simple, general purpose random generator
        /// </summary>
        protected Random rnd;
        /// <summary>
        /// simple proxy for the propsgrid of the control
        /// </summary>
        protected Proxy proxy;

        /// <summary>
        /// drawing a grid on the canvas?
        /// </summary>
        protected bool showGrid = true;

        /// <summary>
        /// just the default gridsize used in the paint-background method
        /// </summary>
        protected Size gridSize = new Size(10, 10);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the shape collection
        /// </summary>
        public List<ShapeBase> Shapes
        {
            get { return shapes; }
            set { shapes = value; }
        }

        /// <summary>
        /// Gets or sets whether the grid is drawn on the canvas
        /// </summary>
        public bool ShowGrid
        {
            get { return showGrid; }
            set { showGrid = value; Invalidate(); }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Default ctor
        /// </summary>
        public GraphControl()
        {
            //double-buffering
            //SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //SetStyle(ControlStyles.DoubleBuffer, true);
            //SetStyle(ControlStyles.UserPaint, true);
            //SetStyle(ControlStyles.ResizeRedraw, true);

            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();

            //init the collections
            shapes = new List<ShapeBase>();
            connections = new List<Connection>();

            //menu
            menu = new ContextMenu();
            BuildMenu();
            this.ContextMenu = menu;

            //init the randomizer
            rnd = new Random();

            //init the proxy
            proxy = new Proxy(this);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Builds the context menu
        /// </summary>
        private void BuildMenu()
        {
            MenuItem mnuDelete = new MenuItem("delete", new EventHandler(OnDelete));
            menu.MenuItems.Add(mnuDelete);

            MenuItem mnuProps = new MenuItem("Attributes", new EventHandler(OnProps));
            menu.MenuItems.Add(mnuProps);

            MenuItem mnuDash = new MenuItem("-");
            menu.MenuItems.Add(mnuDash);

            MenuItem mnuNewConnection = new MenuItem("AddLine", new EventHandler(OnNewConnection));
            menu.MenuItems.Add(mnuNewConnection);

            MenuItem mnuTidy = new MenuItem("Tidy", new EventHandler(OnTidyControls));
            menu.MenuItems.Add(mnuTidy);

            MenuItem mnuShapes = new MenuItem("Control");
            menu.MenuItems.Add(mnuShapes);
            shapes = showShapeClass.initial(this, ref mnuShapes, refp, shapes, addShapeEvent);

            // MenuItem mnuOvalShape = new MenuItem("Oval", new EventHandler(OnOvalShape));
            //  mnuShapes.MenuItems.Add(mnuOvalShape);

            //MenuItem mnuTLShape = new MenuItem("Text", new EventHandler(OnTextLabelShape));
            //mnuShapes.MenuItems.Add(mnuTLShape);
        }
        private void addShapeEvent(ShapeBase shapeBase)
        {
            addShape?.Invoke(shapeBase);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            Graphics g = e.Graphics;

            if (showGrid)
            {
                ControlPaint.DrawGrid(g, this.ClientRectangle, gridSize, this.BackColor);
            }
        }


        #region Menu handlers
        /// <summary>
        /// Deletes the currently selected object from the canvas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDelete(object sender, EventArgs e)
        {
            if (selectedEntity != null)
            {
                if (typeof(ShapeBase).IsInstanceOfType(selectedEntity))
                {
                    deleteShape?.Invoke(selectedEntity as ShapeBase);
                    this.shapes.Remove(selectedEntity as ShapeBase);
                    this.Invalidate();
                }
                else if (typeof(Connection).IsInstanceOfType(selectedEntity))
                {
                    this.connections.Remove((Connection)selectedEntity);
                    this.Invalidate();
                }
            }
        }
        /// <summary>
        /// Asks the host to show the props
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnProps(object sender, EventArgs e)
        {
            object thing;
            if (this.selectedEntity == null)
                thing = this.proxy;
            else
                thing = selectedEntity;
            if (this.OnShowProps != null)
                OnShowProps(thing);

        }

        private void OnNewConnection(object sender, EventArgs e)
        {
            AddConnection(refp);
        }
        private void OnTidyControls(object sender, EventArgs e)
        {
            tidyControl?.BeginInvoke(null, null);
        }
        #endregion

        /// <summary>
        /// Paints the control
        /// </summary>
        /// <remarks>
        /// If you switch the painting order of connections and shapes the connection line
        /// will be underneath/above the shape
        /// </remarks>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            //use the best quality, with a performance penalty
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //similarly for the connections
            for (int k = 0; k < connections.Count; k++)
            {
                connections[k].Paint(g);
                connections[k].From.Paint(g);
                connections[k].To.Paint(g);
            }
            //loop over the shapes and draw
            for (int k = 0; k < shapes.Count; k++)
            {
                shapes[k].Paint(g);
            }
        }


        /// <summary>
        /// Adds a shape to the canvas or diagram
        /// </summary>
        /// <param name="shape"></param>
        public ShapeBase AddShape(ShapeBase shape)
        {
            shapes.Add(shape);
            shape.Site = this;
            this.Invalidate();
            return shape;
        }
        /// <summary>
        /// Adds a predefined shape
        /// </summary>
        /// <param name="type"></param>



        #region AddConnection overloads
        /// <summary>
        /// Adds a connection to the diagram
        /// </summary>
        /// <param name="con"></param>
        public Connection AddConnection(Connection con)
        {
            connections.Add(con);
            con.Site = this;
            con.From.Site = this;
            con.To.Site = this;
            this.Invalidate();
            return con;
        }
        public Connection AddConnection(DiagramPoint startPoint)
        {
            //let's take a random point and assume this control is not infinitesimal (bigger than 20x20).
            startPoint.ConnectorDirection = ConnectorDirection.Down;
            //  DiagramPoint rndPoint = new Point(rnd.Next(20, this.Width - 20), rnd.Next(20, this.Height - 20));
            DiagramPoint rndPoint = new Point(startPoint.X + 1, startPoint.Y + 1);
            rndPoint.ConnectorDirection = ConnectorDirection.Up;
            Connection con = new Connection(startPoint, rndPoint);
            AddConnection(con);
            //use the end-point and simulate a dragging operation, see the OnMouseDown handler
            selectedEntity = con.To;
            tracking = true;
            refp = rndPoint;
            this.Invalidate();
            return con;

        }

        public Connection AddConnection(Connector from, Connector to)
        {
            Connection con = AddConnection11(from.Point, to.Point);
            from.AttachConnector(con.From);
            to.AttachConnector(con.To);
            return con;

        }

        public Connection AddConnection11(DiagramPoint from, DiagramPoint to)
        {
            Connection con = new Connection(from, to);
            this.AddConnection(con);
            return con;
        }
        #endregion

        #region Mouse event handlers

        /// <summary>
        /// Thanks to Mark Johnson and Radek Cervinka for corrections!
        /// </summary>
        /// <param name="oEnt"></param>
        protected void UpdateHovered(Entity oEnt)
        {
            if (hoveredEntity != null)
            {
                if (hoveredEntity is ShapeBase)
                {
                    List<Connector> connectorList = ((ShapeBase)hoveredEntity).Connectors;
                    foreach (var item in connectorList)
                    {
                        item.Hovered = false;
                    }
                }
                hoveredEntity.Hovered = false;
                hoveredEntity.Invalidate();
            }
            oEnt.Hovered = true;//20221230
            hoveredEntity = oEnt;
            hoveredEntity.Invalidate();
        }

        /// <summary>
        /// Thanks to Mark Johnson and Radek Cervinka for corrections!
        /// </summary>
        /// <param name="oEnt"></param>
        private void UpdateSelected(Entity oEnt)
        {
            if (selectedEntity != null)
            {
                selectedEntity.IsSelected = false;
                selectedEntity.Invalidate();
            }
            selectedEntity = oEnt;
            oEnt.IsSelected = true;
            oEnt.Invalidate();
        }
        /// <summary>
        /// Handles the mouse-down event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Point p = new Point(e.X, e.Y);

            #region LMB & RMB

            //test for connectors
            //  for (int k = 0; k < connections.Count; k++)
            //  {
            //if (connections[k].From.Hit(p))
            //{
            //    UpdateSelected(connections[k].From);
            //    tracking = true;
            //    refp = p;
            //    return;
            //}
            //if (connections[k].To.Hit(p))
            //{
            //    UpdateSelected(connections[k].To);
            //    tracking = true;
            //    refp = p;
            //    return;
            //}
            //  }
            //test for shapes
            for (int k = 0; k < shapes.Count; k++)
            {
                if (shapes[k].Hit(p))
                {
                    shapes[k].ShapeColor = Color.WhiteSmoke;
                    tracking = true;
                    UpdateSelected(shapes[k]);
                    refp = p;
                    if (OnShowProps != null)
                        OnShowProps(this.shapes[k]);
                    if (e.Button == MouseButtons.Right)
                    {
                        if (OnShowProps != null)
                            OnShowProps(this);
                    }
                    return;
                }
                else
                {
                    shapes[k].IsSelected = false;
                    shapes[k].Hovered = false;
                    foreach (var item in shapes[k].Connectors)
                    {
                        item.Hovered = false;
                    }
                    shapes[k].Invalidate();
                }
            }
            Invalidate();
            refp = p; //useful for all kind of things
                      //nothing was selected but we'll show the props of the control in this case
            if (OnShowProps != null)
                OnShowProps(this.proxy);
            #endregion

            //20221229
            //  base.OnMouseDown(e);
            _leftMouse = e.Button == MouseButtons.Left;
            _stratPoint = e.Location;
            int entityTYpe = -1;
            Entity hoverEdentity = shapes.FirstOrDefault((ShapeBase f) => f.Hit(e.Location));
            int num;
            ShapeBase tool = default(ShapeBase);
            if (hoverEdentity == null)
            {
                tool = selectedEntity as ShapeBase;
                num = ((tool != null) ? 1 : 0);
            }
            else
            {
                num = 0;
            }
            if (num != 0)
            {
                Connector 连接点 = tool.HitConnector(e.Location);
                if (连接点 != null)
                {
                    Point point = e.Location;
                    point.Offset(-ViewOriginPoint.GetPoint().X, -ViewOriginPoint.GetPoint().Y);
                    Connection 连接 = AddConnection(连接点.Point);
                    连接.From.OwnerEntity = 连接点.OwnerEntity;
                    连接.From.ConnectorsIndexOfContainEntity = 连接点.ConnectorsIndexOfContainEntity;
                    连接.FromID = 连接点.OwnerEntity.ObjectId;
                    连接.FromIndex = 连接点.ConnectorsIndexOfContainEntity;
                    UpdateSelected(连接.To);
                    连接点.AttachConnector(连接.From);
                    _tracking = true;
                    Invalidate(invalidateChildren: true);
                    Console.WriteLine("情况1,连线");
                    return;
                }
            }
            if (hoverEdentity != null)
            {
                Console.WriteLine("情况2,移动节点");
                _tracking = true;
                OnSelectChanged(hoverEdentity, new SelectElementChangedEventArgs
                {
                    CurrentEntity = hoverEdentity,
                    PreviousEntity = selectedEntity
                });
            }
            else
            {
                hoverEdentity = connections.FirstOrDefault(delegate (Connection f)
                {
                    if (f.Hit(e.Location))
                    {
                        entityTYpe = 2;
                        return true;
                    }
                    if (f.From.Hit(e.Location))
                    {
                        entityTYpe = 3;
                        return true;
                    }
                    if (f.To.Hit(e.Location))
                    {
                        entityTYpe = 4;
                        return true;
                    }
                    return false;
                });
                if (entityTYpe == 3)
                {
                    hoverEdentity = ((Connection)hoverEdentity)?.From;
                    Console.WriteLine("情况3,移动连接线起点");
                    _tracking = true;
                }
                else if (entityTYpe == 4)
                {
                    hoverEdentity = ((Connection)hoverEdentity)?.To;
                    Console.WriteLine("情况4，移动连接线终点");
                    _tracking = true;
                }
                else if (entityTYpe == 2)
                {
                    Console.WriteLine("情况5，点击连接线拖动整个画布");
                    OnSelectChanged(hoverEdentity, new SelectElementChangedEventArgs
                    {
                        CurrentEntity = hoverEdentity,
                        PreviousEntity = selectedEntity
                    });
                }
                else
                {
                    Console.WriteLine("情况6，移动画布");
                }
            }
            if (hoverEdentity == null)
            {
                OnSelectChanged(proxy, new SelectElementChangedEventArgs
                {
                    CurrentEntity = hoverEdentity,
                    PreviousEntity = selectedEntity
                });
            }
            if (hoverEdentity != null)
            {
                UpdateSelected(hoverEdentity);
            }
            Invalidate(invalidateChildren: true);
            if (selectedEntity != null)
            {
                selectedEntity.IsSelected = false;
            }
            //  selectedEntity = null;

            //test for connections
            for (int k = 0; k < this.connections.Count; k++)
            {
                if (connections[k].Hit(p))
                {
                    UpdateSelected(this.connections[k]);
                    if (OnShowProps != null)
                        OnShowProps(this.connections[k]);
                    if (e.Button == MouseButtons.Right)
                    {
                        if (OnShowProps != null)
                            OnShowProps(this);
                    }
                    return;
                }
            }
        }
        private void OnSelectChanged(object sender, SelectElementChangedEventArgs e)
        {
            this.OnSelectElementChanged?.Invoke(sender, e);
        }
        /// <summary>
        /// Handles the mouse-up event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            //test if we connected a connection
            if (tracking)
            {
                Point p1 = new Point(e.X, e.Y);
                if (typeof(Connector).IsInstanceOfType(selectedEntity))
                {
                    Connector con;
                    for (int k = 0; k < shapes.Count; k++)
                    {
                        if ((con = shapes[k].HitConnector(p1)) != null)
                        {
                            con.AttachConnector(selectedEntity as Connector);
                            tracking = false;
                            // return;
                        }
                    }
                    if (tracking)
                    {
                        (selectedEntity as Connector).Release();
                    }
                }
                tracking = false;
            }
            //20221229
            //  base.OnMouseUp(e);
            _leftMouse = false;
            if (!_tracking)
            {
                return;
            }
            Point p = new Point(e.X, e.Y);
            Connector 箭头 = selectedEntity as Connector;
            if (箭头 != null)
            {
                for (int i = 0; i < shapes.Count; i++)
                {
                    Connector tool连接点;
                    if ((tool连接点 = shapes[i].HitConnector(p)) != null)
                    {
                        // tool连接点.AttachConnector(箭头);
                        Connection connection = 箭头.OwnerEntity as Connection;
                        if (connection != null)
                        {
                            connection.ToID = tool连接点.OwnerID;
                            connection.ToIndex = tool连接点.ConnectorsIndexOfContainEntity;
                        }
                        箭头.OwnerEntity = tool连接点.OwnerEntity;
                        箭头.ConnectorsIndexOfContainEntity = tool连接点.ConnectorsIndexOfContainEntity;
                        // 箭头.ConnectorDirection = (ConnectorDirection)箭头.ConnectorsIndexOfContainEntity;
                        箭头.ConnectorDirection = ConnectorDirection.Up;
                        tool连接点.Hovered = false;
                        _tracking = false;
                        ViewOriginPoint_Flow = ViewOriginPoint.GetPoint();
                        return;
                    }
                }
                箭头.Release();
                DeleteElement(箭头.OwnerEntity);
            }
            _tracking = false;
        }
        private void DeleteElement(Entity entity)
        {
            if (entity == null)
            {
                return;
            }
            ShapeBase tool = entity as ShapeBase;
            //if (tool != null)
            //{
            //    DeletingEventArgs deletingEventArgs2 = new DeletingEventArgs();
            //    OnDeleting(entity, deletingEventArgs2);
            //    if (!deletingEventArgs2.Cancel)
            //    {
            //        shapes.Remove(tool);
            //        Invalidate(invalidateChildren: true);
            //        OnDeleted(entity, EventArgs.Empty);
            //    }
            //    return;
            //}
            Connection connection = entity as Connection;
            if (connection == null)
            {
                return;
            }
            DeletingEventArgs deletingEventArgs = new DeletingEventArgs();
            OnDeleting(entity, deletingEventArgs);
            if (deletingEventArgs.Cancel)
            {
                return;
            }
            //int j = -1;
            //for (int i = 0; i < EditFLow.Connections.Count; i++)
            //{
            //    if (EditFLow.Connections[i].ObjectId == connection.ObjectId)
            //    {
            //        j = i;
            //        break;
            //    }
            //}
            //if (j > -1)
            //{
            //    EditFLow.Connections.RemoveAt(j);
            //}
            connections.Remove(connection);
            Invalidate(invalidateChildren: true);
            OnDeleted(entity, EventArgs.Empty);
        }
        private void OnDeleting(object sender, DeletingEventArgs e)
        {
            this.OnElementDeleting?.Invoke(sender, e);
        }
        private void OnDeleted(object sender, EventArgs e)
        {
            this.OnElementDeleted?.Invoke(sender, e);
        }
        /// <summary>
        /// Handles the mouse-move event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Point p = new Point(e.X, e.Y);
            if (tracking)
            {
                if (selectedEntity is Connector)
                {
                    int offsetX = ViewOriginPoint_Flow.GetPoint().X;
                    int offsetY = ViewOriginPoint_Flow.GetPoint().Y;
                    int X1 = p.X - refp.X;
                    int Y1 = p.Y - refp.Y;
                    selectedEntity.Move(new Point(p.X - refp.X - ViewOriginPoint_Flow.GetPoint().X, p.Y - refp.Y - ViewOriginPoint_Flow.GetPoint().Y));
                    refp = new DiagramPoint(e.X - ViewOriginPoint_Flow.GetPoint().X, e.Y - ViewOriginPoint_Flow.GetPoint().Y);
                }
                else
                {
                    selectedEntity.Move(new Point(p.X - refp.X, p.Y - refp.Y));
                    refp = p;
                }

                Invalidate();
                if (typeof(Connector).IsInstanceOfType(selectedEntity))
                {
                    //test the connecting points of hovered shapes
                    for (int k = 0; k < shapes.Count; k++)
                        shapes[k].HitConnector(p);
                }   // if(typeof(Connector).IsInstanceOfType(selectedEntity))
            }

            // hovering stuff
            for (int k = 0; k < shapes.Count; k++)
            {
                if (shapes[k].Hit(p))
                {
                    UpdateHovered(shapes[k]);
                }
                else
                {
                    shapes[k].Hovered = false;
                }
            }

            for (int k = 0; k < connections.Count; k++)
            {
                if (connections[k].Hit(p))
                {
                    UpdateHovered(connections[k]);
                }
                else
                {
                    connections[k].Hovered = false;
                }
            }

            for (int k = 0; k < connections.Count; k++)
            {
                if (connections[k].From.Hit(p))
                {
                    UpdateHovered(connections[k].From);
                }
                else
                {
                    connections[k].From.Hovered = false;
                    connections[k].From.IsSelected = false;
                }

                if (connections[k].To.Hit(p))
                {
                    UpdateHovered(connections[k].To);
                }
                else
                {
                    connections[k].To.Hovered = false;
                    connections[k].To.IsSelected = false;
                }
            }

            //20221229
            //  base.OnMouseMove(e);
            if (_tracking)
            {
                // selectedEntity.Move(new Point(e.X - _stratPoint.GetPoint().X, e.Y - _stratPoint.GetPoint().Y));
                //if (selectedEntity is Connector)
                //{
                //    for (int i = 0; i < shapes.Count; i++)
                //    {
                //        shapes[i].HitConnector(e.Location);
                //    }
                //}
            }
            else if (_leftMouse)
            {
                ViewOriginPoint = new DiagramPoint(ViewOriginPoint.GetPoint().X + e.X - _stratPoint.GetPoint().X, ViewOriginPoint.GetPoint().Y + e.Y - _stratPoint.GetPoint().Y);
                ViewOriginPoint_Flow = ViewOriginPoint.GetPoint();
            }
            int entityTYpe = -1;
            Entity hoverEdentity = shapes.Cast<Entity>().FirstOrDefault((Entity f) => f.Hit(e.Location));
            if (hoverEdentity == null)
            {
                hoverEdentity = connections.FirstOrDefault(delegate (Connection f)
                {
                    if (f.Hit(e.Location))
                    {
                        entityTYpe = 2;
                        return true;
                    }
                    if (f.From.Hit(e.Location))
                    {
                        entityTYpe = 3;
                        return true;
                    }
                    if (f.To.Hit(e.Location))
                    {
                        entityTYpe = 4;
                        return true;
                    }
                    return false;
                });
            }
            if (entityTYpe == 3)
            {
                hoverEdentity = ((Connection)hoverEdentity)?.From;
            }
            if (entityTYpe == 4)
            {
                hoverEdentity = ((Connection)hoverEdentity)?.To;
            }
            if (hoverEdentity != null)
            {
                UpdateHovered(hoverEdentity);
            }
            _stratPoint = e.Location;
            Invalidate(invalidateChildren: true);
        }

        #endregion


        #endregion

    }

    /// <summary>
    /// Simple proxy class for the control to display only specific properties.
    /// Not as sophisticated as the property-bag of the full Netron-control
    /// but does the job in this simple context.
    /// </summary>
    public class Proxy
    {
        #region Fields
        private GraphControl site;
        #endregion

        #region Constructor
        public Proxy(GraphControl site)
        { this.site = site; }
        #endregion

        #region Methods
        [Browsable(false)]
        public GraphControl Site
        {
            get { return site; }
            set { site = value; }
        }
        [Browsable(true), Description("The backcolor of the canvas"), Category("Layout")]
        public Color BackColor
        {
            get { return this.site.BackColor; }
            set { this.site.BackColor = value; }
        }

        [Browsable(true), Description("Gets or sets whether the grid is shown"), Category("Layout")]
        public bool ShowGrid
        {
            get { return this.site.ShowGrid; }
            set { this.site.ShowGrid = value; }
        }
        #endregion
    }
}
