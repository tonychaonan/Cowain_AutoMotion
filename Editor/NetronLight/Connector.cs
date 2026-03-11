using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using NetronLight;

namespace CanVasLib.Diagram
{
    public class Connector : Entity
    {
        protected DiagramPoint point;

        public List<Connector> attachedConnectors;

        protected Connector attachedTo;

        protected string name;

        private ConnectorDirection? _connectorDirection = null;

        private string _ownerId;

        public Entity OwnerEntity { get; set; }

        public int ConnectorsIndexOfContainEntity { get; set; } = -1;


        [Browsable(false)]
        public string OwnerID
        {
            get
            {
                return _ownerId;
            }
            set
            {
                _ownerId = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        [Browsable(false)]
        public int Index { get; set; } = 0;


        public ConnectorDirection ConnectorDirection
        {
            get
            {
                if (_connectorDirection.HasValue && point != null)
                {
                    point.ConnectorDirection = _connectorDirection.Value;
                }
                if (point == null)
                {
                    return _connectorDirection ?? ConnectorDirection.Up;
                }
                return point.ConnectorDirection;
            }
            set
            {
                if (point != null)
                {
                    point.ConnectorDirection = value;
                }
                _connectorDirection = value;
            }
        }

        public Connector AttachedTo
        {
            get
            {
                return attachedTo;
            }
            set
            {
                if (attachedTo != null)
                {
                    attachedTo = value;
                    point = attachedTo.point.Copy();
                    _connectorDirection = attachedTo.ConnectorDirection;
                    OnAttachedToChanged(new AttachedToChangedEventArgs
                    {
                        New = value
                    });
                }
            }
        }

        public DiagramPoint Point
        {
            get
            {
                return point;
            }
            set
            {
                point = value;
                if (_connectorDirection.HasValue)
                {
                    point.ConnectorDirection = _connectorDirection.Value;
                }
            }
        }

        public event EventHandler<LocationEventArgs> LocationChanged;

        public event EventHandler<AttachedToChangedEventArgs> AttachedToChanged;

        public Connector()
        {
            attachedConnectors = new List<Connector>();
        }

        public Connector(DiagramPoint p)
        {
            attachedConnectors = new List<Connector>();
            Point = p;
        }

        private void OnLocationChanged(LocationEventArgs e)
        {
            this.LocationChanged?.Invoke(this, e);
        }

        private void OnAttachedToChanged(AttachedToChangedEventArgs e)
        {
            this.AttachedToChanged?.Invoke(this, e);
        }

        public override void Paint(Graphics g)
        {
            if (Hovered && site != null)
            {
                g.FillRectangle(Brushes.Cyan, point.X + site.ViewOriginPoint.GetPoint().X - 10, point.Y + site.ViewOriginPoint.GetPoint().Y - 10, 20, 20);
            }
        }

        public override bool Hit(Point p)
        {
            p.X -= site.ViewOriginPoint.GetPoint().X;
            p.Y -= site.ViewOriginPoint.GetPoint().Y;
            Point a = p;
            Point b = point.GetPoint();
            b.Offset(-18, -18);
            Rectangle r = new Rectangle(a, new Size(0, 0));
            bool b_Contain = new Rectangle(b, new Size(35, 35)).Contains(r);
            return b_Contain;
        }

        public override void Invalidate()
        {
            Point p = point.GetPoint();
            p.X += site.ViewOriginPoint.GetPoint().X;
            p.Y += site.ViewOriginPoint.GetPoint().Y;
            p.Offset(-5, -5);
            site.Invalidate(new Rectangle(p, new Size(10, 10)));
        }

        public override void Move(Point p)
        {
            OnLocationChanged(new LocationEventArgs
            {
                OffsetX = p.X,
                OffsetY = p.Y
            });
            point.X += p.X;
            point.Y += p.Y;
            for (int i = 0; i < attachedConnectors.Count; i++)
            {
                attachedConnectors[i].Move(p);
            }
        }

        public void AttachConnector(Connector c)
        {
            if (c.attachedTo != null)
            {
                c.attachedTo.attachedConnectors.Remove(c);
            }
            attachedConnectors.Add(c);
            c.AttachedTo = this;
        }

        public void DetachConnector(Connector c)
        {
            attachedConnectors.Remove(c);
        }

        public void Release()
        {
            if (attachedTo != null)
            {
                attachedTo.attachedConnectors.Remove(this);
                AttachedTo = null;
            }
        }
    }
}
