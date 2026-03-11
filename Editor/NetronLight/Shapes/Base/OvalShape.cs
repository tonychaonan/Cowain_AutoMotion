using System;
using System.Drawing;
using static System.Windows.Forms.AxHost;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using CanVasLib.Diagram;

namespace NetronLight
{
    /// <summary>
    /// A simple rectangular shape
    /// </summary>
    public class OvalShape : ShapeBase
    {
        #region Fields
        /// <summary>
        /// holds the bottom connector
        /// </summary>
        public Connector cBottom, cLeft, cRight, cTop;
        int width1 = 120;
        int height1 = 43;
        #endregion

        #region Constructor
        /// <summary>
        /// Default ctor
        /// </summary>
        /// <param name="s"></param>
        public OvalShape(GraphControl s) : base(s)
        {
            rectangle = new Rectangle(0, 0, width1, height1);
            cBottom = new Connector(new DiagramPoint((int)(rectangle.Left + rectangle.Width / 2), rectangle.Bottom));
            cBottom.Site = this.site;
            cBottom.OwnerEntity = this;
            cBottom.Name = "Bottom connector";
            cBottom.ConnectorDirection = ConnectorDirection.Down;  
            connectors.Add(cBottom);

            cLeft = new Connector(new DiagramPoint(rectangle.Left, (int)(rectangle.Top + rectangle.Height / 2)));
            cLeft.Site = this.site;
            cLeft.OwnerEntity = this;
            cLeft.Name = "Left connector";
            cLeft.ConnectorDirection = ConnectorDirection.Left;
            connectors.Add(cLeft);

            cRight = new Connector(new DiagramPoint(rectangle.Right, (int)(rectangle.Top + rectangle.Height / 2)));
            cRight.Site = this.site;
            cRight.OwnerEntity = this;
            cRight.Name = "Right connector";
            cRight.ConnectorDirection = ConnectorDirection.Right;
            connectors.Add(cRight);

            cTop = new Connector(new DiagramPoint((int)(rectangle.Left + rectangle.Width / 2), rectangle.Top));
            cTop.Site = this.site;
            cTop.OwnerEntity = this;
            cTop.Name = "Top connector";
            cTop.ConnectorDirection = ConnectorDirection.Up;
            connectors.Add(cTop);
        }
        #endregion
        #region Methods
        private int _borderLength = 20;
        private int _marginBottom = 5;
        [Browsable(false)]
        public double Scale = 1.0;
        private int _cornerRadius = 10;
        [Browsable(true)]
        [Description("圆角率")]
        [Category("Layout")]
        public int CornerRadius
        {
            get
            {
                return _cornerRadius;
            }
            set
            {
                _cornerRadius = value;
            }
        }
        protected Color borderSelectedColor = Color.GreenYellow;
        [Browsable(true)]
        [Description("边框选中后颜色")]
        [Category("Layout")]
        public Color BoderSelectedColor
        {
            get
            {
                return borderSelectedColor;
            }
            set
            {
                borderSelectedColor = value;
                Invalidate();
            }
        }
        protected Color borderColor = Color.Black;
        [Browsable(true)]
        [Description("边框颜色")]
        [Category("Layout")]
        public Color BoderColor
        {
            get
            {
                return borderColor;
            }
            set
            {
                borderColor = value;
                Invalidate();
            }
        }
        private bool showBorder = true;
        [Browsable(true)]
        [Description("是否显示边框")]
        [Category("Layout")]
        public bool ShowBorder
        {
            get
            {
                return showBorder;
            }
            set
            {
                showBorder = value;
                Invalidate();
            }
        }
        protected Color backGroundColor = Color.White;
        [Browsable(true)]
        [Description("背景颜色")]
        [Category("Layout")]
        public Color BackGroundColor
        {
            get
            {
                return backGroundColor;
            }
            set
            {
                backGroundColor = value;
                Invalidate();
            }
        }
        [Browsable(true)]
        [Description("显示底部源点")]
        [Category("Layout")]
        public bool EnableBottomSourceConnector { get; set; } = true;


        [Browsable(true)]
        [Description("显示左侧源点")]
        [Category("Layout")]
        public bool EnableLeftSourceConnector { get; set; } = false;


        [Browsable(true)]
        [Description("显示右侧源点")]
        [Category("Layout")]
        public bool EnableRightSourceConnector { get; set; } = false;


        [Browsable(true)]
        [Description("显示顶部源点")]
        [Category("Layout")]
        public bool EnableTopSourceConnector { get; set; } = true;

        [Browsable(true)]
        [Description("显示底部目标点")]
        [Category("Layout")]
        public bool EnableBottomTargetConnector { get; set; } = false;


        [Browsable(true)]
        [Description("显示左侧目标点")]
        [Category("Layout")]
        public bool EnableLeftTargetConnector { get; set; } = false;


        [Browsable(true)]
        [Description("显示右侧目标点")]
        [Category("Layout")]
        public bool EnableRightTargetConnector { get; set; } = false;


        [Browsable(true)]
        [Description("显示顶部目标点")]
        [Category("Layout")]
        public bool EnableTopTargetConnector { get; set; } = false;

        private Bitmap _bitIcon;
        public Bitmap BitIcon
        {
            get
            {
                //if (ToolMgr.Ins.Images.ContainsKey(Icon))
                //{
                //    _bitIcon = ToolMgr.Ins.Images[Icon];
                //}
                //  return _bitIcon;
                //string imagePath = @"icon\\dll.png";
                //Image bitmap = Image.FromFile(imagePath);
                //return (Bitmap)bitmap;
                return _bitIcon;
            }
            set
            {
                _bitIcon = value;
            }
        }
        protected virtual GraphicsPath CreatePath(Rectangle rect, int cornerRadius)
        {
            GraphicsPath roundedRect = new GraphicsPath();
            roundedRect.AddArc(rect.X, rect.Y, cornerRadius * 2, cornerRadius * 2, 180f, 90f);
            roundedRect.AddLine(rect.X + cornerRadius, rect.Y, rect.Right - cornerRadius * 2, rect.Y);
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y, cornerRadius * 2, cornerRadius * 2, 270f, 90f);
            roundedRect.AddLine(rect.Right, rect.Y + cornerRadius * 2, rect.Right, rect.Y + rect.Height - cornerRadius * 2);
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y + rect.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0f, 90f);
            roundedRect.AddLine(rect.Right - cornerRadius * 2, rect.Bottom, rect.X + cornerRadius * 2, rect.Bottom);
            roundedRect.AddArc(rect.X, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90f, 90f);
            roundedRect.AddLine(rect.X, rect.Bottom - cornerRadius * 2, rect.X, rect.Y + cornerRadius * 2);
            roundedRect.CloseFigure();
            return roundedRect;
        }
        public override Connector HitConnector(Point p)
        {
            bool targetNode = false;
            Point point0 = new Point(0, 0);
            if (site != null)
            {
                point0 = site.ViewOriginPoint.GetPoint();
            }
            Rectangle rect = new Rectangle(p, new Size(5, 5));
            Rectangle contentionRectangle = new Rectangle((rectangle.Width - _borderLength) / 2 + rectangle.X + point0.X, (int)((double)(rectangle.Y + point0.Y - _marginBottom) - (double)_borderLength * Math.Sin(Math.PI / 3.0)), _borderLength, _borderLength);
            if (contentionRectangle.Contains(rect))
            {
                if (!EnableTopSourceConnector)
                {
                    return null;
                }
                cTop.Point.GetPoint().Offset(point0.X - 7, point0.Y - 7);
                return cTop;
            }
            contentionRectangle = new Rectangle((rectangle.Width - _borderLength) / 2 + rectangle.X + point0.X, rectangle.Y + point0.Y + rectangle.Height + _marginBottom, _borderLength, _borderLength);
            if (contentionRectangle.Contains(rect))
            {
                if (!EnableBottomSourceConnector)
                {
                    return null;
                }
                cBottom.Point.GetPoint().Offset(point0.X - 7, point0.Y - 7);
                return cBottom;
            }
            contentionRectangle = new Rectangle(rectangle.Width + rectangle.X + point0.X + _marginBottom, (rectangle.Height - _borderLength) / 2 + rectangle.Y + point0.Y, _borderLength, _borderLength);
            if (contentionRectangle.Contains(rect))
            {
                if (!EnableRightSourceConnector)
                {
                    return null;
                }
                cRight.Point.GetPoint().Offset(point0.X - 7, point0.Y - 7);
                return cRight;
            }
            contentionRectangle = new Rectangle(rectangle.X + point0.X - _marginBottom - _borderLength, (rectangle.Height - _borderLength) / 2 + rectangle.Y + point0.Y, _borderLength, _borderLength);
            if (contentionRectangle.Contains(rect))
            {
                if (!EnableLeftSourceConnector)
                {
                    return null;
                }
                cLeft.Point.GetPoint().Offset(point0.X - 7, point0.Y - 7);
                return cLeft;
            }
            for (int i = 0; i < connectors.Count; i++)
            {
                if (connectors[i].Hit(p))
                {
                    if (!targetNode)
                    {
                        if (!EnableBottomSourceConnector && connectors[i].ConnectorDirection == ConnectorDirection.Down)
                        {
                            return null;
                        }
                        if (!EnableTopSourceConnector && connectors[i].ConnectorDirection == ConnectorDirection.Up)
                        {
                            return null;
                        }
                        if (!EnableRightSourceConnector && connectors[i].ConnectorDirection == ConnectorDirection.Right)
                        {
                            return null;
                        }
                        if (!EnableLeftSourceConnector && connectors[i].ConnectorDirection == ConnectorDirection.Left)
                        {
                            return null;
                        }
                    }
                    else
                    {
                        if (!EnableBottomTargetConnector && connectors[i].ConnectorDirection == ConnectorDirection.Down)
                        {
                            return null;
                        }
                        if (!EnableTopTargetConnector && connectors[i].ConnectorDirection == ConnectorDirection.Up)
                        {
                            return null;
                        }
                        if (!EnableRightTargetConnector && connectors[i].ConnectorDirection == ConnectorDirection.Right)
                        {
                            return null;
                        }
                        if (!EnableLeftTargetConnector && connectors[i].ConnectorDirection == ConnectorDirection.Left)
                        {
                            return null;
                        }
                    }
                    connectors[i].Hovered = true;
                    connectors[i].Invalidate();
                    return connectors[i];
                }
                connectors[i].Hovered = false;
                connectors[i].Invalidate();
            }
            return null;
        }

        public override void Resize(int width, int height)
        {
            rectangle.Width = width;
            rectangle.Height = height;
            if (cBottom != null)
            {
                cBottom.Point = new Point(rectangle.Left + rectangle.Width / 2, rectangle.Bottom);
            }
            if (cLeft != null)
            {
                cLeft.Point = new Point(rectangle.Left, rectangle.Top + rectangle.Height / 2);
            }
            if (cRight != null)
            {
                cRight.Point = new Point(rectangle.Right, rectangle.Top + rectangle.Height / 2);
            }
            if (cTop != null)
            {
                cTop.Point = new Point(rectangle.Left + rectangle.Width / 2, rectangle.Top);
            }
            Invalidate();
        }

        protected override void SiteChanged()
        {
            connectors.Remove(cBottom);
            cBottom = new Connector(new Point(rectangle.Left + rectangle.Width / 2, rectangle.Bottom))
            {
                OwnerEntity = this,
                ConnectorsIndexOfContainEntity = connectors.Count,
                Site = site,
                OwnerID = base.ObjectId,
                Index = connectors.Count,
                Name = "Bottom connector",
                ConnectorDirection = ConnectorDirection.Down
            };
            connectors.Add(cBottom);
            connectors.Remove(cLeft);
            cLeft = new Connector(new Point(rectangle.Left, rectangle.Top + rectangle.Height / 2))
            {
                OwnerEntity = this,
                ConnectorsIndexOfContainEntity = connectors.Count,
                Site = site,
                OwnerID = base.ObjectId,
                Index = connectors.Count,
                Name = "Left connector",
                ConnectorDirection = ConnectorDirection.Left
            };
            connectors.Add(cLeft);
            connectors.Remove(cRight);
            cRight = new Connector(new Point(rectangle.Right, rectangle.Top + rectangle.Height / 2))
            {
                OwnerEntity = this,
                ConnectorsIndexOfContainEntity = connectors.Count,
                Site = site,
                OwnerID = base.ObjectId,
                Index = connectors.Count,
                Name = "Right connector",
                ConnectorDirection = ConnectorDirection.Right
            };
            connectors.Add(cRight);
            connectors.Remove(cTop);
            cTop = new Connector(new Point(rectangle.Left + rectangle.Width / 2, rectangle.Top))
            {
                OwnerEntity = this,
                ConnectorsIndexOfContainEntity = connectors.Count,
                Site = site,
                OwnerID = base.ObjectId,
                Index = connectors.Count,
                Name = "Top connector",
                ConnectorDirection = ConnectorDirection.Up
            };
            connectors.Add(cTop);
        }

        public override void Paint(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Point point = new Point(0, 0);
            if (site != null)
            {
                point = site.ViewOriginPoint.GetPoint();
            }
            Brush brush = new SolidBrush(BackGroundColor);
            Rectangle rect = new Rectangle(rectangle.X + point.X, rectangle.Y + point.Y, Width, Height);
            using (GraphicsPath graphicsPath = CreatePath(rect, (int)((double)_cornerRadius * Scale)))
            {
                g.FillPath(brush, graphicsPath);
                if (Hovered || isSelected)
                {
                    Pen p = new Pen(BoderSelectedColor, 2f);
                    p.StartCap = LineCap.Round;
                    p.EndCap = LineCap.Round;
                    p.LineJoin = LineJoin.Round;
                    g.DrawPath(p, graphicsPath);
                }
                else if (ShowBorder)
                {
                    Pen p2 = new Pen(BoderColor);
                    p2.StartCap = LineCap.Round;
                    p2.EndCap = LineCap.Round;
                    p2.LineJoin = LineJoin.Round;
                    g.DrawPath(p2, graphicsPath);
                }
            }
            if (site != null)
            {
                point = site.ViewOriginPoint.GetPoint();
            }
            if (isSelected)
            {
                Point point2;
                Point point3;
                Point point4;
                if (EnableTopSourceConnector)
                {
                    point2 = new Point((rectangle.Width - _borderLength) / 2 + rectangle.X + point.X, rectangle.Y + point.Y - _marginBottom);
                    point3 = new Point((rectangle.Width + _borderLength) / 2 + rectangle.X + point.X, rectangle.Y + point.Y - _marginBottom);
                    point4 = new Point(rectangle.Width / 2 + rectangle.X + point.X, (int)((double)(rectangle.Y + point.Y - _marginBottom) - (double)_borderLength * Math.Sin(Math.PI / 3.0)));
                    Point[] pntArr = new Point[3] { point2, point3, point4 };
                    g.FillPolygon(Brushes.Gold, pntArr);
                }
                if (EnableBottomSourceConnector)
                {
                    point2 = new Point((rectangle.Width - _borderLength) / 2 + rectangle.X + point.X, rectangle.Y + point.Y + rectangle.Height + _marginBottom);
                    point3 = new Point((rectangle.Width + _borderLength) / 2 + rectangle.X + point.X, rectangle.Y + point.Y + rectangle.Height + _marginBottom);
                    point4 = new Point(rectangle.Width / 2 + rectangle.X + point.X, (int)((double)(rectangle.Y + point.Y + rectangle.Height + _marginBottom) + (double)_borderLength * Math.Sin(Math.PI / 3.0)));
                    Point[] pntArr = new Point[3] { point2, point3, point4 };
                    g.FillPolygon(Brushes.Gold, pntArr);
                }
                if (EnableRightSourceConnector)
                {
                    point2 = new Point(rectangle.Width + rectangle.X + point.X + _marginBottom, (rectangle.Height - _borderLength) / 2 + rectangle.Y + point.Y);
                    point3 = new Point(rectangle.Width + rectangle.X + point.X + _marginBottom, (rectangle.Height + _borderLength) / 2 + rectangle.Y + point.Y);
                    point4 = new Point((int)((double)(rectangle.Width + rectangle.X + point.X + _marginBottom) + (double)_borderLength * Math.Sin(Math.PI / 3.0)), rectangle.Height / 2 + rectangle.Y + point.Y);
                    Point[] pntArr = new Point[3] { point2, point3, point4 };
                    g.FillPolygon(Brushes.Gold, pntArr);
                }
                if (EnableLeftSourceConnector)
                {
                    point2 = new Point(rectangle.X + point.X - _marginBottom, (rectangle.Height - _borderLength) / 2 + rectangle.Y + point.Y);
                    point3 = new Point(rectangle.X + point.X - _marginBottom, (rectangle.Height + _borderLength) / 2 + rectangle.Y + point.Y);
                    point4 = new Point((int)((double)(rectangle.X + point.X - _marginBottom) - (double)_borderLength * Math.Sin(Math.PI / 3.0)), rectangle.Height / 2 + rectangle.Y + point.Y);
                    Point[] pntArr = new Point[3] { point2, point3, point4 };
                    g.FillPolygon(Brushes.Gold, pntArr);
                }
            }
            for (int i = 0; i < connectors.Count; i++)
            {
                connectors[i].Paint(g);
            }
            if (text != string.Empty)
            {
                StringFormat stringFormat = new StringFormat();
                stringFormat.LineAlignment = StringAlignment.Center;
                stringFormat.Alignment = StringAlignment.Center;
                Rectangle rectangle11 = new Rectangle(rectangle.X + point.X, rectangle.Y + point.Y, rectangle.Width, rectangle.Height);
                if (BitIcon != null)
                {
                    g.DrawImage(BitIcon, rectangle.X + point.X + 5, rectangle.Y + point.Y, rectangle.Height - 1, rectangle.Height - 1);
                    Rectangle rectangle2 = new Rectangle(rectangle.X + point.X + 45, rectangle.Y + point.Y, rectangle.Width-40, rectangle.Height);
                    g.DrawString(text, Font, Brushes.Black, rectangle2, stringFormat);
                }
                else
                {
                    g.DrawString(text, Font, Brushes.Black, rectangle11, stringFormat);
                }
            }
        }
        public override bool Hit(Point p)
        {
            if (site == null)
            {
                return false;
            }
            Point point = new Point(0, 0);
            if (site != null)
            {
                point = site.ViewOriginPoint.GetPoint();
            }
            p.X -= point.X;
            p.Y -= point.Y;
            Rectangle r = new Rectangle(p, new Size(5, 5));
            bool b_Contain = rectangle.Contains(r);
            return b_Contain;
        }

        public override void Invalidate()
        {
            if (site != null)
            {
                Point point = site.ViewOriginPoint.GetPoint();
                Rectangle r = rectangle;
                r.X += point.X;
                r.Y += point.Y;
                r.Offset(-5, -5);
                r.Inflate(20, 20);
                site.Invalidate(r);
            }
        }

        public override void Move(Point p)
        {
            if (site != null)
            {
                rectangle = new Rectangle(rectangle.X + p.X, rectangle.Y + p.Y, rectangle.Width, rectangle.Height);
                for (int i = 0; i < connectors.Count; i++)
                {
                    connectors[i].Move(p);
                }
                Invalidate();
            }
        }
        #endregion
    }
}
