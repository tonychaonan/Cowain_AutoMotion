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

namespace Cowain_Form.FormView
{
    public partial class frm_Teach2 : Form
    {
        public frm_Teach2(ref Machine pM)
        {
            InitializeComponent();
            pMachine = pM;
        }

        public Machine pMachine;

        private void frm_Teach2_VisibleChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //NS_UCLA.FormView.dia_NozzleOfx pNozzleOfx = new Cowain_GelToPCB.FormView.dia_NozzleOfx(ref pMachine);
            //pNozzleOfx.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //NS_UCLA.FormView.dia_TorChangeSet pTorChangeSet = new NS_UCLA.FormView.dia_TorChangeSet(ref pMachine);
            //pTorChangeSet.ShowDialog();
        }
    }
}
