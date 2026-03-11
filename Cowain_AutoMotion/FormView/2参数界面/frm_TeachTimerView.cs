using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MotionBase;
using Cowain_Machine.Flow;
using System.IO;
using Cowain;

namespace Cowain_Form.FormView
{
    public partial class frm_TeachTimerView : Form
    {
        public frm_TeachTimerView(ref clsMachine refMachine)
        {
            pMachine = refMachine;
            InitializeComponent();
        }

        #region 參數&變數
        clsMachine pMachine = null;
        ScanTimer pTimer = null;
        Dictionary<string, Base> showTimerList;
        ScanTimer.tyTimer_Parameter TimerParameter= new ScanTimer.tyTimer_Parameter();
        #endregion

        private void frm_TimerView_Shown(object sender, EventArgs e)
        {
            if (pMachine != null)
            {
              //  showTimerList = pMachine.GetTimerList();
                if (showTimerList.Count > 0)
                {
                    Fun_DisplayTimerList();
                    timer1.Enabled = true;
                }
            }
        }

        void Fun_DisplayTimerList()
        {
            dataGridViewTimer.Rows.Clear();
            for (int i = 0; i < showTimerList.Count; i++)
            {
                pTimer = (ScanTimer) showTimerList[showTimerList.Keys.ToList()[i]].m_NowAddress;
                pTimer.GetParameter(ref TimerParameter);
               
                dataGridViewTimer.Rows.Add(new object[] { TimerParameter.strTimerID , TimerParameter.strCName ,
                                                          TimerParameter.strEName , TimerParameter.strInterval });
                

            }

        }

        private void frm_TeachTimerView_Load(object sender, EventArgs e)
        {
            //foreach (Control item in Controls)
            //{
            //    string s = item.Text.ToString();
            //    string[] s1 = { s, "" };
            //    File.AppendAllLines(@"C:\Users\cowain\Desktop\1.txt", s1);
            //    item.Text = JudgeLanguage.JudgeLag(item.Text);
            //}
        }
    }
}
