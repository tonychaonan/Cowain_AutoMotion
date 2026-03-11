using System;
using System.Drawing;
using System.ComponentModel;
using Newtonsoft.Json;

namespace NetronLight
{
    /// <summary>
    /// Abstract base class for every object part of the diagram
    /// </summary>
    public abstract class Entity : BindableBase
    {
        protected internal bool Hovered = false;

        protected GraphControl site;

        protected bool isSelected = false;

        protected Font Font = new Font("Verdana", 10f);

        protected Pen BlackPen = new Pen(Brushes.Black, 1f);

        [Browsable(false)]
        public string ObjectId { get; set; } = Guid.NewGuid().ToString();


        [Browsable(false)]
        [JsonIgnore]
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
            }
        }

        [Browsable(false)]
        [JsonIgnore]
        public GraphControl Site
        {
            get
            {
                return site;
            }
            set
            {
                site = value;
                SiteChanged();
            }
        }

        public Entity()
        {
        }

        public Entity(GraphControl site)
        {
            this.site = site;
        }

        protected virtual void SiteChanged()
        {
        }

        public abstract void Paint(Graphics g);

        public abstract bool Hit(Point p);

        public abstract void Invalidate();

        public abstract void Move(Point p);
    }
}
