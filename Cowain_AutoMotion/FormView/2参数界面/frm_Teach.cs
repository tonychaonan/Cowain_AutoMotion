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
using MotionBase;

namespace Cowain_Form.FormView
{
    public partial class frm_Teach : Form
    {
        public frm_Teach(ref clsMachine pM)
        {
            InitializeComponent();
            pMachine = pM;
        }

        public clsMachine pMachine;
        enum enFormView
        {
            enParameterView=0,
            enMax,
        };
        private Form[] pFormView=new Form[(int) (enFormView.enMax) ]; 

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
           int  SelectTab = tabControl1.SelectedIndex;
            for(int i=0;i< pFormView.Length;i++)
            {
                if(pFormView[i] != null)
                    pFormView[i].Hide();
            }
            //-----------------------------
            if(SelectTab < pFormView.Length)
            {
                if (pFormView[SelectTab] != null)
                    pFormView[SelectTab].Show();
            }
        }

        private void frm_Teach_Shown(object sender, EventArgs e)
        {
            //tabControl1.SelectedIndex = 0;
            //pFormView[(int)enFormView.enIOView].Show();
            
        }


        private void frm_Teach_VisibleChanged(object sender, EventArgs e)
        {
            //if (this.Visible == false)
            //{
            //    //pMachine.m_IoVacPumpOn.SetIO(false);
            //    for (int i = 0; i < pFormView.Length; i++)
            //    {
            //        if (pFormView[i] != null)
            //        {
            //            pFormView[i].Close();
            //            pFormView[i] = null;
            //        }                       
            //    }
            //}else {
            //    pFormView[(int)enFormView.enIOView] = new frm_TeachIOView(ref pMachine);
            //    pFormView[(int)enFormView.enValveView] = new frm_TeachValveView(ref pMachine);
            //    pFormView[(int)enFormView.enMototView] = new frm_TeachMotorView(ref pMachine);
            //  //  pFormView[(int)enFormView.enTimerView] = new frm_TeachTimerView(ref pMachine);
            //    pFormView[(int)enFormView.enParameterView] = new frm_TeachParameter(ref pMachine);
            //    for (int i = 0; i < pFormView.Length; i++)
            //    {
            //        pFormView[i].TopLevel = false;
            //        pFormView[i].Parent = tabControl1.TabPages[i];
            //        pFormView[i].ControlBox = false;
            //        pFormView[i].Dock = System.Windows.Forms.DockStyle.Fill;
            //        pFormView[i].Hide();
            //    }
            //    //---------------------------
            //    tabControl1.SelectedIndex = 0;
            //    pFormView[(int)enFormView.enIOView].Show();
            //}

            ShowUserButton(pMachine.m_LoginUser);
        }

        private void frm_Teach_Load(object sender, EventArgs e)
        {
            pFormView[(int)enFormView.enParameterView] = new frm_TeachParameter();
            for (int i = 0; i < pFormView.Length; i++)
            {
                pFormView[i].TopLevel = false;
                pFormView[i].Parent = tabControl1.TabPages[i];
                pFormView[i].ControlBox = false;
                pFormView[i].Dock = System.Windows.Forms.DockStyle.Fill;
                pFormView[i].Hide();
            }
            //---------------------------
            tabControl1.SelectedIndex = 0;
            pFormView[(int)enFormView.enParameterView].Show();
        }

        private void ShowUserButton(Sys_Define.enPasswordType enLoginType)
        {
            if (enLoginType == Sys_Define.enPasswordType.Operator ||
                enLoginType == Sys_Define.enPasswordType.UnLogin)
            {
            }
            else if (enLoginType == Sys_Define.enPasswordType.Eng)
            {
            }
            else if (enLoginType == Sys_Define.enPasswordType.Maker)
            {
               // tabControl1.TabPages.Add(tabPage设备重要参数);
            }
        }
    }
}
