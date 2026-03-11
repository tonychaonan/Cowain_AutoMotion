using NetronLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cowain
{
    public class GetEventClass
    {
        public DateTime mouseClickTime = DateTime.Now;
        public Action<string> mouseClick;
        private string lastObjID = "";
        public void click(ShapeBase shape)
        {
            if(shape==null)
            {
                lastObjID = "";
                return;
            }
            //触发鼠标双击事件
            if (((DateTime.Now - mouseClickTime).TotalMilliseconds < 500)&&(lastObjID== shape.ObjectId))
            {
                mouseClickTime = DateTime.Now;
                mouseClick?.Invoke(shape.ObjectId);
            }
            else
            {
                mouseClickTime = DateTime.Now;
                lastObjID = shape.ObjectId;
            }
        }
    }
}
