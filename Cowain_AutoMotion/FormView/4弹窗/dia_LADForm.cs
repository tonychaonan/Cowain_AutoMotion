using Cowain_AutoMotion;
using Cowain_Form.FormView;
using Cowain_Machine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Cowain_Form.FormView.frm_ModeSelect;

namespace Cowain_AutoMotion
{
    public partial class dia_LADForm : DevExpress.XtraEditors.XtraForm
    {
        public enWorkMode m_CurMode = enWorkMode.Idle;
        public dia_LADForm()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (radioGroup1.SelectedIndex == -1)
            {
                return;
            }
            frm_PlanDoubleConfirm doubleConfm = new frm_PlanDoubleConfirm(ref MachineDataDefine.pMachine);
            doubleConfm.PlanType = "LAD";
            doubleConfm.NeedSN = false;
            if (doubleConfm.ShowDialog() == DialogResult.OK)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                return;
            }
            m_CurMode = enWorkMode.Planned_Downtime;
            this.DialogResult = DialogResult.OK;
            MachineDataDefine.LADModel = radioGroup1.SelectedIndex + 1;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
