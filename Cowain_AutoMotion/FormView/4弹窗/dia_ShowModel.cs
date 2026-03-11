using Cowain;
using Cowain_Machine;
using Cowain_Machine.Flow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cowain_AutoMotion.FormView
{
    public partial class dia_ShowModel : Form
    {
        bool isMOdel = false; public clsMachine pMachine;
        public dia_ShowModel(clsMachine pM, bool IsMOdel)
        {
            InitializeComponent();
            isMOdel = IsMOdel;
            pMachine = pM;
            //MachineDataDefine.machineState.Isdia_ShowModelShow = true;
           
            this.TopMost = true;
        }
        private void dia_ShowModel_Shown(object sender, EventArgs e)
        {
           
            if (isMOdel == false)
            {
                this.BackColor = Color.Gold;
                textBox1.Text = JudgeLanguage.JudgeLag("当前为调机模式!");
                textBox1.ForeColor = Color.Red;
            }
            else
            {
                textBox1.Text = JudgeLanguage.JudgeLag("当前为生产模式!");
                textBox1.ForeColor = Color.Green;
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {

            button1.Enabled = false;
            this.Hide();
            pMachine.StartAuto(); //DialogResult = DialogResult.OK;
            this.Close();
            MachineDataDefine.machineState.Isdia_ShowModelShow = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pMachine.Stop();  //DialogResult = DialogResult.Cancel;
            this.Close();
            MachineDataDefine.machineState.Isdia_ShowModelShow = false;
        }

      
    }
}
