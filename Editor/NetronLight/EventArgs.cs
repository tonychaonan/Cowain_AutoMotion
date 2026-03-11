using CanVasLib.Diagram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetronLight
{
    public class LocationEventArgs : EventArgs
    {
        public int OffsetX = 0;

        public int OffsetY = 0;
    }
    public class AttachedToChangedEventArgs : EventArgs
    {
        public Connector New { get; set; }
    }
    public class SelectElementChangedEventArgs : EventArgs
    {
        public Entity CurrentEntity { get; set; }

        public Entity PreviousEntity { get; set; }
    }
    public class DeletingEventArgs : EventArgs
    {
        public bool Cancel { get; set; } = false;

    }
}
