using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cowain_Form.FormView;
using Cowain_Machine.Flow;
using Cowain_Machine;

namespace Cowain_Form.FormView
{
    public partial class frm_Manul : Form
    {
        public frm_Manul(ref clsMachine pM)
        {
            InitializeComponent();
            pMachine = pM;
        }

        public clsMachine pMachine;
        enum enFormView
        {
            enDispenserView = 0,
            IO_view,
            Cylinder_view,
            Axis_view,
            enMax,
        };
        private Form[] pFormView = new Form[(int)(enFormView.enMax)];
        int iSelectPage = 0;

        private void frm_Manul_Shown(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            pFormView[(int)enFormView.enDispenserView].Show();
        }
        private void frm_Manul_Load(object sender, EventArgs e)
        {
            pFormView[(int)enFormView.enDispenserView] = new frm_ManulPoint(ref pMachine);
            pFormView[(int)enFormView.IO_view] = new frm_TeachIOView(ref pMachine);
            pFormView[(int)enFormView.Cylinder_view] = new frm_TeachValveView(ref pMachine);
            pFormView[(int)enFormView.Axis_view] = new frm_TeachMotorView(ref pMachine);
            for (int i = 0; i < pFormView.Length; i++)
            {
                pFormView[i].TopLevel = false;
                pFormView[i].Parent = tabControl1.TabPages[i];
                pFormView[i].ControlBox = false;
                pFormView[i].Dock = System.Windows.Forms.DockStyle.Fill;
                pFormView[i].Show();
            }
        }
    }
}
