using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cowain.DrawRelation
{
    public enum ManualType
    {
        copy,
        delete,
    }
    public class ManualHistory
    {
        public ArrayList revocationArrayList = new ArrayList();//用于撤回
        public ManualType manualType = ManualType.copy;
        public ManualHistory(ArrayList revocationArrayList1, ManualType manualType1)
        {
            revocationArrayList = revocationArrayList1.GetRange(0, revocationArrayList1.Count);
            manualType = manualType1;
        }
    }
    public class ManualClass
    {
        public bool b_MouseDown = false;
        private Point downPoint = new Point(0, 0);
        private MouseEventArgs e1;
        KeyboardHook myKeyboardHook = new KeyboardHook();
        public event Action copy;
        public event Action cut;
        public event Action<ArrayList> delete;
        public event Action<ArrayList, ManualType> revocation;
        public event Action<MouseEventArgs, ArrayList> paste;
        private ArrayList momentArrayList = new ArrayList();//用于删除
        private ArrayList myArrayList = new ArrayList();//用于复制
        private ConcurrentStack<ManualHistory> manualHistoryList = new ConcurrentStack<ManualHistory>();//用于撤回
     
        private bool b_Control = false;
        public ManualClass()
        {
            myKeyboardHook.Start();
            myKeyboardHook.KeyDownEvent += MyKeyboardHook_KeyDownEvent;
            copy += copyShape;
        }
        public Rectangle MouseUp(MouseEventArgs e)
        {
            b_MouseDown = false;
            Rectangle rt = new Rectangle();
            int x1 = e.X;
            int y1 = e.Y;
            if (x1 >= downPoint.X)
            {
                rt.X = downPoint.X;
                rt.Y = downPoint.Y;
            }
            else
            {
                rt.X = x1;
                rt.Y = y1;
            }
            rt.Width = Math.Abs(x1 - downPoint.X);
            rt.Height = Math.Abs(y1 - downPoint.Y);
            return rt;
        }
        public void MouseDown(MouseEventArgs e)
        {
            b_MouseDown = true;
            downPoint.X = e.Location.X;
            downPoint.Y = e.Location.Y;
            e1 = e;
        }
        private void MyKeyboardHook_KeyDownEvent(object sender, KeyEventArgs e)
        {
            bool b_Ctrl = b_Control;
            if (e.KeyCode.ToString().ToLower() == "c" && b_Ctrl)
            {
                copy?.BeginInvoke(null, null);
            }
            else if (e.KeyCode.ToString().ToLower() == "v" && b_Ctrl)
            {
                paste?.BeginInvoke(e1, myArrayList, null, null);
            }
            else if (e.KeyCode.ToString().ToLower() == "x" && b_Ctrl)
            {
                copy?.Invoke();
                delete?.BeginInvoke(momentArrayList, null, null);
            }
            else if(e.KeyCode.ToString().ToLower() == "z" && b_Ctrl)
            {
                if(manualHistoryList.Count>0)
                {
                    ManualHistory myManualHistory = null;
                    manualHistoryList.TryPop(out myManualHistory);
                    revocation?.Invoke(myManualHistory.revocationArrayList, myManualHistory.manualType);
                }
            }
            else if (e.KeyCode.ToString().ToLower() == "delete")
            {
                delete?.BeginInvoke(momentArrayList, null, null);
                ManualHistory myManualHistory = new ManualHistory(momentArrayList, ManualType.delete);
                manualHistoryList.Push(myManualHistory);
            }
            if (e.KeyCode.ToString().ToLower().Contains("control"))
            {
                b_Control = true;
            }
            else
            {
                b_Control = false;
            }
        }
        public void addShape(ArrayList list)
        {
            momentArrayList.Clear();
            momentArrayList.AddRange(list.GetRange(0, list.Count));
        }
        public void addCopy_ManualHistoryList(ArrayList list)
        {
            ManualHistory myManualHistory = new ManualHistory(list, ManualType.copy);
            manualHistoryList.Push(myManualHistory);
        }
        public void close()
        {
            myKeyboardHook.KeyDownEvent -= MyKeyboardHook_KeyDownEvent;
            myKeyboardHook.Stop();
        }
        public void copyShape()
        {
            myArrayList.Clear();
            myArrayList.AddRange(momentArrayList.GetRange(0, momentArrayList.Count));
        }
    }
}
