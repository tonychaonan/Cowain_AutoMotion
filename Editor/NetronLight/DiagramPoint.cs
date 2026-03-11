using System.ComponentModel;
using System.Drawing;

namespace CanVasLib.Diagram
{
	public class DiagramPoint : Component
	{
		private Point _point = default(Point);

		public ConnectorDirection ConnectorDirection = ConnectorDirection.Up;

		[Description("X偏移量")]
		[Category("Layout")]
		public int X
		{
			get
			{
				return _point.X;
			}
			set
			{
				_point.X = value;
			}
		}

		[Description("Y偏移量")]
		[Category("Layout")]
		public int Y
		{
			get
			{
				return _point.Y;
			}
			set
			{
				_point.Y = value;
			}
		}

		public DiagramPoint()
		{
		}

		public DiagramPoint(int x, int y)
		{
			X = x;
			Y = y;
		}

		public Point GetPoint()
		{
			return _point;
		}

		public override string ToString()
		{
			return $"{X},{Y}";
		}

		public static implicit operator DiagramPoint(Point point)
		{
			return new DiagramPoint(point.X, point.Y);
		}

		public DiagramPoint Copy()
		{
			DiagramPoint diagramPoint = new DiagramPoint(X, Y);
			diagramPoint.ConnectorDirection = ConnectorDirection;
			return diagramPoint;
		}
	}
}
