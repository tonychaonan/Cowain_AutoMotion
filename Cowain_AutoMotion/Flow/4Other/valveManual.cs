using MotionBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cowain_AutoMotion
{
    public class valveManual
    {
        public List<valveManualItem> valveManualItems = new List<valveManualItem>();
        public List<valveManualItem> totalvalveManualItems = new List<valveManualItem>();
        public object obj = new object();
        public void add(string valveName)
        {
            lock(obj)
            {
                if (valveManualItems.Count >= 14)
                {
                    return;
                }
                int index = valveManualItems.Count;
                valveManualItem valveManualItem = new valveManualItem();
                valveManualItem.lbName = totalvalveManualItems[index].lbName;
                valveManualItem.btnOpen = totalvalveManualItems[index].btnOpen;
                valveManualItem.btnClose = totalvalveManualItems[index].btnClose;
                valveManualItem.lbName.Text = valveName;
                valveManualItem.lbName.Visible = true;
                valveManualItem.btnOpen.Visible = true;
                valveManualItem.btnClose.Visible = true;
                EnumParam_Valve enumParam_Valve;
                Enum.TryParse(valveName, out enumParam_Valve);
                valveManualItem.drvValve = HardWareControl.getValve(enumParam_Valve);
                valveManualItems.Add(valveManualItem);
            }
        }
        public void doEvent(Button button)
        {
            foreach (var item in valveManualItems)
            {
                if (item.btnOpen == button)
                {
                    item.drvValve?.Open();
                }
                else if (item.btnClose == button)
                {
                    item.drvValve?.Close();
                }
            }
        }
        public void dispose()
        {
            lock(obj)
            {
                foreach (var item in valveManualItems)
                {
                    item.lbName.Visible = false;
                    item.btnOpen.Visible = false;
                    item.btnClose.Visible = false;
                }
                valveManualItems.Clear();
            }
        }
        public void addControl(Label lbControl1, Button btnOpen1, Button btnClose1)
        {
            valveManualItem valveManualItem = new valveManualItem();
            lbControl1.Visible = false;
            btnOpen1.Visible = false;
            btnClose1.Visible = false;
            btnOpen1.Click += Btn_Click;
            btnClose1.Click += Btn_Click;
            valveManualItem.lbName = lbControl1;
            valveManualItem.btnOpen = btnOpen1;
            valveManualItem.btnClose = btnClose1;
            totalvalveManualItems.Add(valveManualItem);
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            doEvent((Button)sender);
        }
    }
    public class valveManualItem
    {
        public Label lbName;
        public Button btnOpen;
        public Button btnClose;
        public DrvValve drvValve;
    }
}
