using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cowain_Machine.Flow;
using Cowain_Form.FormView;
using MotionBase;
using System.IO;
using Cowain;
using Cowain_Machine;

namespace Cowain_Form.FormView
{
    public partial class frm_Vision : Form
    {
        public frm_Vision(ref clsMachine pM)
        {
            InitializeComponent();
            pMachine = pM;
        }

        public clsMachine pMachine;
        enum enFormVisionView
        {
            enCCD1_VisionView = 0,
            //enCCD2_VisionView ,
            //enPickSt2_VisionView,
            //enDispVisionView,
            enMax,
        };
        private Form[] pFormView = new Form[(int)(enFormVisionView.enMax)];
        private void frm_Vision_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                if (pFormView[(int)enFormVisionView.enCCD1_VisionView] == null)
                    pFormView[(int)enFormVisionView.enCCD1_VisionView] = new frm_VisionCCD1(ref pMachine);
                //if (pFormView[(int)enFormVisionView.enCCD2_VisionView] == null)
                //    pFormView[(int)enFormVisionView.enCCD2_VisionView] = new frm_VisionCCD2(ref pMachine);
                //if (pFormView[(int)enFormVisionView.enDispVisionView] == null)
                //    pFormView[(int)enFormVisionView.enDispVisionView] = new frm_VisionDisSt(ref pMachine);
                //-------------------------------------------
                for (int i = 0; i < pFormView.Length; i++)
                {
                    pFormView[i].TopLevel = false;
                    pFormView[i].Parent = tabControl1.TabPages[i];
                    pFormView[i].ControlBox = false;
                    pFormView[i].Dock = System.Windows.Forms.DockStyle.Fill;
                    pFormView[i].Hide();
                    pFormView[i].Enabled = true;
                }
                //-------------------------------------------
                tabControl1.SelectedIndex = 0;
                pFormView[(int)enFormVisionView.enCCD1_VisionView].Show();
                //--------
                MachineDataDefine.m_bUiTimerEnable = true;
                //-------------------------------------------
            }
            else
            {
                //pMachine.m_IoVacPumpOn.SetIO(false);
                for (int i = 0; i < pFormView.Length; i++)
                {
                    if (pFormView[i] != null)
                    {
                        pFormView[i].Visible = false;
                        pFormView[i].Enabled = false;
                        MachineDataDefine.m_bUiTimerEnable = false;
                        //pFormView[i].Close();
                        //pFormView[i] = null;
                    }
                }
                //pMachine.m_CCDSystem.m_LightControlBox.SetIntensity(0, 0);
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int SelectTab = tabControl1.SelectedIndex;
            for (int i = 0; i < pFormView.Length; i++)
                pFormView[i].Hide();
            //-----------------------------
            if (SelectTab < pFormView.Length)
                pFormView[SelectTab].Show();
        }

        private void frm_Vision_Load(object sender, EventArgs e)
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
