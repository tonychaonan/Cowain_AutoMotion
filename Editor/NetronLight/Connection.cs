using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using NetronLight;
using Newtonsoft.Json;

namespace CanVasLib.Diagram
{
	public class Connection : Entity
	{
		protected Connector from;

		protected Connector to;

		private bool _useBackColor = false;

		protected Color color = Color.Silver;

		protected Color lineHoveredColor = Color.Blue;

		protected Color lineSelectedColor = Color.Green;

		[Browsable(false)]
		[JsonIgnore]
		public List<Connector> Connectors { get; set; }

		[Browsable(true)]
		[Description("始节点")]
		[Category("Layout")]
		[JsonIgnore]
		public Connector From
		{
			get
			{
				return from;
			}
			private set
			{
				from = value;
				if (site != null)
				{
					from.Site = site;
				}
				from.OwnerEntity = this;
			}
		}

		public string FromID { get; set; }

		public int FromIndex { get; set; }

		[Browsable(true)]
		[Description("终节点")]
		[Category("Layout")]
		[JsonIgnore]
		public Connector To
		{
			get
			{
				return to;
			}
			private set
			{
				to = value;
				if (site != null)
				{
					to.Site = site;
				}
				to.OwnerEntity = this;
			}
		}

		public string ToID { get; set; }

		public int ToIndex { get; set; }

		[Browsable(true)]
		[Description("始节点")]
		[Category("Layout")]
		public bool UseBackColor
		{
			get
			{
				return _useBackColor;
			}
			set
			{
				_useBackColor = value;
			}
		}

		[Browsable(true)]
		[Description("默认颜色")]
		[Category("Layout")]
		public Color Color
		{
			get
			{
				return color;
			}
			set
			{
				color = value;
				_useBackColor = true;
			}
		}

		[Browsable(true)]
		[Description("选中颜色")]
		[Category("Layout")]
		public Color LineSelectedColor
		{
			get
			{
				return lineSelectedColor;
			}
			set
			{
				lineSelectedColor = value;
				_useBackColor = true;
			}
		}

		[Browsable(true)]
		[Description("悬停颜色")]
		[Category("Layout")]
		public Color LineHoveredColor
		{
			get
			{
				return lineHoveredColor;
			}
			set
			{
				lineHoveredColor = value;
				_useBackColor = true;
			}
		}

		public Connection()
		{
		}

		public Connection(DiagramPoint from, DiagramPoint to)
		{
			Connectors = new List<Connector>();
			this.from = new Connector(from.Copy())
			{
				Name = "From",
				OwnerEntity = this
			};
			this.from.LocationChanged += Connector_LocationChanged;
			Connector connector1 = this.from;
            Connectors.Add(connector1);
			this.to = new Connector(to.Copy())
			{
				Name = "To",
				OwnerEntity = this
			};
			Connector connector2 = this.to;
			this.to.LocationChanged += Connector_LocationChanged;
			To.AttachedToChanged += To_AttachedToChanged;
            Connectors.Add(connector2);
		}

		private void Connector_LocationChanged(object sender, LocationEventArgs e)
		{
			InitialPath();
		}

		private void To_AttachedToChanged(object sender, AttachedToChangedEventArgs e)
		{
			InitialPath();
		}

		public void InitialPath()
		{
			Connector connector1 = from;
			Connector connector2 = to;
			int x = 0;
			int y = 0;
			List<Connector> connectors = new List<Connector> { from };
			if (connector1.ConnectorDirection == ConnectorDirection.Down && connector2.ConnectorDirection == ConnectorDirection.Up)
			{
				if (connector1.Point.Y < connector2.Point.Y && connector1.Point.X != connector2.Point.X)
				{
					x = connector1.Point.X;
					y = (connector1.Point.Y + connector2.Point.Y) / 2;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					x = connector2.Point.X;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else if (connector1.Point.Y >= connector2.Point.Y)
				{
					x = connector1.Point.X;
					y = connector1.Point.Y + 10;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					x = (connector1.Point.X + connector2.Point.X) / 2;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					y -= 30 + connector1.Point.Y - connector2.Point.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					x = connector2.Point.X;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
			}
			else if (connector1.ConnectorDirection == ConnectorDirection.Down && connector2.ConnectorDirection == ConnectorDirection.Left)
			{
				if (connector1.Point.Y < connector2.Point.Y && connector1.Point.X >= connector2.Point.X)
				{
					x = connector1.Point.X;
					y = (connector1.Point.Y + connector2.Point.Y) / 2;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					x = connector2.Point.X - 20;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					y = connector2.Point.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else if (connector1.Point.Y < connector2.Point.Y)
				{
					x = connector1.Point.X;
					y = connector2.Point.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else if (connector1.Point.X != connector2.Point.X)
				{
					x = connector1.Point.X;
					y = connector1.Point.Y + 10;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					x = connector2.Point.X - 20;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					y = connector2.Point.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
			}
			else if (connector1.ConnectorDirection == ConnectorDirection.Down && connector2.ConnectorDirection == ConnectorDirection.Right)
			{
				if (connector1.Point.Y < connector2.Point.Y && connector1.Point.X <= connector2.Point.X)
				{
					x = connector1.Point.X;
					y = (connector1.Point.Y + connector2.Point.Y) / 2;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					x = connector2.Point.X + 20;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					y = connector2.Point.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else if (connector1.Point.Y < connector2.Point.Y)
				{
					x = connector1.Point.X;
					y = connector2.Point.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else if (connector1.Point.X != connector2.Point.X)
				{
					x = connector1.Point.X;
					y = connector1.Point.Y + 10;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					x = connector2.Point.X + 20;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					y = connector2.Point.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
			}
			else if (connector1.ConnectorDirection == ConnectorDirection.Down && connector2.ConnectorDirection == ConnectorDirection.Down)
			{
				if (connector1.Point.Y < connector2.Point.Y && connector1.Point.X != connector2.Point.X)
				{
					x = connector1.Point.X;
					y = connector2.Point.Y + 20;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					x = connector2.Point.X;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else if (connector1.Point.Y < connector2.Point.Y)
				{
					x = connector1.Point.X;
					y = (connector1.Point.Y + connector2.Point.Y) / 2;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					x = connector2.Point.X + 20;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					y = connector2.Point.Y + 20;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					x = connector2.Point.X;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else if (connector1.Point.X != connector2.Point.X)
				{
					x = connector1.Point.X;
					y = connector1.Point.Y + 10;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					x = connector2.Point.X;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else
				{
					x = connector1.Point.X;
					y = connector1.Point.Y + 10;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					x = connector1.Point.X + 20;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					y -= 10 + (connector1.Point.Y - connector2.Point.Y) / 2;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					x = connector2.Point.X;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
			}
			else if (connector1.ConnectorDirection == ConnectorDirection.Right && connector2.ConnectorDirection == ConnectorDirection.Up)
			{
				if (connector1.Point.Y < connector2.Point.Y && connector1.Point.X + 10 >= connector2.Point.X)
				{
					x = connector1.Point.X + 10;
					y = connector1.Point.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					y = (connector1.Point.Y + connector2.Point.Y) / 2;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					x = connector2.Point.X;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else if (connector1.Point.Y < connector2.Point.Y)
				{
					x = connector2.Point.X;
					y = connector1.Point.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else
				{
					x = connector1.Point.X + 10;
					y = connector1.Point.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					y = connector2.Point.Y - 20;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					x = connector2.Point.X;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
			}
			else if (connector1.ConnectorDirection == ConnectorDirection.Right && connector2.ConnectorDirection == ConnectorDirection.Right)
			{
				x = Math.Max(connector1.Point.X + 10, connector2.Point.X + 20);
				y = connector1.Point.Y;
				connectors.Add(new Connector(new DiagramPoint(x, y)));
				y = connector2.Point.Y;
				connectors.Add(new Connector(new DiagramPoint(x, y)));
			}
			else if (connector1.ConnectorDirection == ConnectorDirection.Right && connector2.ConnectorDirection == ConnectorDirection.Down)
			{
				if (connector1.Point.Y < connector2.Point.Y)
				{
					x = connector1.Point.X + 10;
					y = connector1.Point.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					y = connector2.Point.Y + 20;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					x = connector2.Point.X;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else if (connector1.Point.Y == connector2.Point.Y)
				{
					x = connector1.Point.X + 10;
					y = connector1.Point.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					y = connector1.Point.Y + 20;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					x = connector2.Point.X;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else if (connector1.Point.X + 10 < connector2.Point.X)
				{
					x = connector2.Point.X;
					y = connector1.Point.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else
				{
					x = connector1.Point.X + 10;
					y = connector1.Point.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					y = (connector1.Point.Y + connector2.Point.Y) / 2;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					x = connector2.Point.X;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
			}
			else if (connector1.ConnectorDirection == ConnectorDirection.Right && connector2.ConnectorDirection == ConnectorDirection.Left)
			{
				if (connector1.Point.X + 10 >= connector2.Point.X)
				{
					x = connector1.Point.X + 10;
					y = connector1.Point.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					y = (connector1.Point.Y + connector2.Point.Y) / 2;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					x = connector2.Point.X - 20;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					y = connector2.Point.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else
				{
					x = connector1.Point.X + 10;
					y = connector1.Point.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					y = connector2.Point.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
			}
			else if (connector1.ConnectorDirection == ConnectorDirection.Up && connector2.ConnectorDirection == ConnectorDirection.Up)
			{
				x = connector1.Point.X;
				y = Math.Min(connector1.Point.Y - 10, connector2.Point.Y - 20);
				connectors.Add(new Connector(new DiagramPoint(x, y)));
				x = connector2.Point.X;
				connectors.Add(new Connector(new DiagramPoint(x, y)));
			}
			else if (connector1.ConnectorDirection == ConnectorDirection.Up && connector2.ConnectorDirection == ConnectorDirection.Right)
			{
				if (connector1.Point.Y > connector2.Point.Y && connector1.Point.X > connector2.Point.X)
				{
					x = connector1.Point.X;
					y = connector2.Point.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else if (connector1.Point.Y > connector2.Point.Y)
				{
					x = connector1.Point.X;
					y = (connector1.Point.Y + connector2.Point.Y) / 2;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					x = connector2.Point.X + 20;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					y = connector2.Point.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else if (connector1.Point.Y <= connector2.Point.Y)
				{
					x = connector1.Point.X;
					y = connector1.Point.Y - 10;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					x = connector2.Point.X + 20;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					y = connector2.Point.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
			}
			else if (connector1.ConnectorDirection == ConnectorDirection.Up && connector2.ConnectorDirection == ConnectorDirection.Down)
			{
				if (connector1.Point.Y > connector2.Point.Y && connector1.Point.X != connector2.Point.X)
				{
					x = connector1.Point.X;
					y = (connector1.Point.Y + connector2.Point.Y) / 2;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					x = connector2.Point.X;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else if (connector1.Point.Y < connector2.Point.Y && connector1.Point.X != connector2.Point.X)
				{
					x = connector1.Point.X;
					y = connector1.Point.Y - 10;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					x = (connector1.Point.X + connector2.Point.X) / 2;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					y = connector2.Point.Y + 20;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					x = connector2.Point.X;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
			}
			else if (connector1.ConnectorDirection == ConnectorDirection.Up && connector2.ConnectorDirection == ConnectorDirection.Left)
			{
				if (connector1.Point.Y > connector2.Point.Y && connector1.Point.X < connector2.Point.X)
				{
					x = connector1.Point.X;
					y = connector2.Point.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else if (connector1.Point.Y > connector2.Point.Y)
				{
					x = connector1.Point.X;
					y = (connector1.Point.Y + connector2.Point.Y) / 2;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					x = connector2.Point.X - 20;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					y = connector2.Point.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else if (connector1.Point.Y <= connector2.Point.Y)
				{
					x = connector1.Point.X;
					y = connector1.Point.Y - 10;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					x = connector2.Point.X - 20;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					y = connector2.Point.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
			}
			else if (connector1.ConnectorDirection == ConnectorDirection.Left && connector2.ConnectorDirection == ConnectorDirection.Up)
			{
				if (connector1.Point.Y < connector2.Point.Y && connector1.Point.X - 10 >= connector2.Point.X)
				{
					x = connector2.Point.X;
					y = connector1.Point.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else if (connector1.Point.Y < connector2.Point.Y)
				{
					x = connector1.Point.X - 10;
					y = connector1.Point.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					y = (connector1.Point.Y + connector2.Point.Y) / 2;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					x = connector2.Point.X;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else if (connector1.Point.Y > connector2.Point.Y)
				{
					x = connector1.Point.X - 10;
					y = connector1.Point.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					y = connector2.Point.Y - 20;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					x = connector2.Point.X;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
			}
			else if (connector1.ConnectorDirection == ConnectorDirection.Left && connector2.ConnectorDirection == ConnectorDirection.Right)
			{
				if (connector1.Point.X - 10 >= connector2.Point.X)
				{
					x = (connector1.Point.X + connector2.Point.X) / 2;
					y = connector1.Point.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					y = connector2.Point.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else
				{
					x = connector1.Point.X - 10;
					y = connector1.Point.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					y = (connector1.Point.Y + connector2.Point.Y) / 2;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					x = connector2.Point.X + 20;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					y = connector2.Point.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
			}
			else if (connector1.ConnectorDirection == ConnectorDirection.Left && connector2.ConnectorDirection == ConnectorDirection.Down)
			{
				if (connector1.Point.Y > connector2.Point.Y && connector1.Point.X > connector2.Point.X)
				{
					x = connector2.Point.X;
					y = connector1.Point.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else if (connector1.Point.X != connector2.Point.X)
				{
					x = connector1.Point.X - 10;
					y = connector1.Point.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					y = connector2.Point.Y + 20;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
					x = connector2.Point.X;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
			}
			else if (connector1.ConnectorDirection == ConnectorDirection.Left && connector2.ConnectorDirection == ConnectorDirection.Left)
			{
				x = Math.Min(connector1.Point.X - 10, connector2.Point.X - 20);
				y = connector1.Point.Y;
				connectors.Add(new Connector(new DiagramPoint(x, y)));
				y = connector2.Point.Y;
				connectors.Add(new Connector(new DiagramPoint(x, y)));
			}
			connectors.Add(to);
			Connectors = connectors;
		}

		protected override void SiteChanged()
		{
			if (from != null)
			{
				from.Site = site;
			}
			if (to != null)
			{
				to.Site = site;
			}
		}

		public override void Paint(Graphics g)
		{
			if (site != null)
			{
				Pen p = new Pen(UseBackColor ? Color : site.LineColor);
				if (isSelected)
				{
					p.Color = (UseBackColor ? LineSelectedColor : site.LineSelectedColor);
					p.Width = 2f;
				}
				else if (Hovered)
				{
					p.Color = (UseBackColor ? LineHoveredColor : site.LineHoveredColor);
					p.Width = 2f;
				}
				Connectors[0].Paint(g);
				for (int i = 1; i < Connectors.Count - 1; i++)
				{
					g.DrawLine(p, Connectors[i - 1].Point.X + site.ViewOriginPoint.GetPoint().X, Connectors[i - 1].Point.Y + site.ViewOriginPoint.GetPoint().Y, Connectors[i].Point.X + site.ViewOriginPoint.GetPoint().X, Connectors[i].Point.Y + site.ViewOriginPoint.GetPoint().Y);
					Connectors[i].Paint(g);
				}
				Connectors[Connectors.Count - 1].Paint(g);
				p.CustomEndCap = new AdjustableArrowCap(p.Width * 5f, p.Width * 5f, isFilled: true);
				g.DrawLine(p, Connectors[Connectors.Count - 2].Point.X + site.ViewOriginPoint.GetPoint().X, Connectors[Connectors.Count - 2].Point.Y + site.ViewOriginPoint.GetPoint().Y, Connectors[Connectors.Count - 1].Point.X + site.ViewOriginPoint.GetPoint().X, Connectors[Connectors.Count - 1].Point.Y + site.ViewOriginPoint.GetPoint().Y);
			}
		}

		public override void Invalidate()
		{
			Rectangle f = new Rectangle(from.Point.GetPoint(), new Size(10, 10));
			f.X += site.ViewOriginPoint.GetPoint().X;
			f.Y += site.ViewOriginPoint.GetPoint().Y;
			Rectangle t = new Rectangle(to.Point.GetPoint(), new Size(10, 10));
			t.X += site.ViewOriginPoint.GetPoint().X;
			t.Y += site.ViewOriginPoint.GetPoint().Y;
			site.Invalidate(Rectangle.Union(f, t));
		}

		public override bool Hit(Point p)
		{
			p.Offset(-site.ViewOriginPoint.GetPoint().X, -site.ViewOriginPoint.GetPoint().Y);
			for (int i = 1; i < Connectors.Count; i++)
			{
				if (GetPointIsInLine(p, Connectors[i - 1].Point.GetPoint(), Connectors[i].Point.GetPoint(), 3.0))
				{
					return true;
				}
			}
			return false;
		}

		private bool GetPointIsInLine(Point pf, Point p1, Point p2, double range)
		{
			double cross = (p2.X - p1.X) * (pf.X - p1.X) + (p2.Y - p1.Y) * (pf.Y - p1.Y);
			if (cross <= 0.0)
			{
				return false;
			}
			double d2 = (p2.X - p1.X) * (p2.X - p1.X) + (p2.Y - p1.Y) * (p2.Y - p1.Y);
			if (cross >= d2)
			{
				return false;
			}
			double r = cross / d2;
			double px = (double)p1.X + (double)(p2.X - p1.X) * r;
			double py = (double)p1.Y + (double)(p2.Y - p1.Y) * r;
			return Math.Sqrt(((double)pf.X - px) * ((double)pf.X - px) + (py - (double)pf.Y) * (py - (double)pf.Y)) <= range;
		}

		public override void Move(Point p)
		{
		}
	}
}
