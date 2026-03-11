using Cowain_Form.FormView;
using Cowain_Machine;
using Cowain_Machine.Flow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cowain_AutoMotion.FormView._4弹窗
{
    public partial class frm_SignificantAdvice : Form
    {
        clsMachine pMachine;
        private Thread th;
        private string ng_msg = "";
        public frm_SignificantAdvice()
        {
            InitializeComponent();
        }
        public frm_SignificantAdvice(ref clsMachine pm, string pError)
        {

            InitializeComponent();
            ng_msg = pError;
            pMachine = pm;
            frm_Main.UpdataSignificant += UpdataSignificant;
            //textBox1.Text = pError;
            //if (!MachineDataDefine.machineState.b_Usehummer)//蜂鸣器未禁用
            //{
            //    MachineDataDefine.pMachine.m_Buzzer.SetIO(true);
            //}
            //MachineDataDefine.pMachine.m_LightTowerR.SetIO(true);
            //MachineDataDefine.pMachine.m_LightTowerG.SetIO(false);
            //MachineDataDefine.pMachine.m_LightTowerY.SetIO(false);
        }



        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frm_SignificantAdvice_FormClosed(object sender, FormClosedEventArgs e)
        {
            //pMachine.m_Buzzer.SetIO(false);

            //pMachine.m_LightTowerR.SetIO(false);
            //pMachine.m_LightTowerG.SetIO(false);
            //pMachine.m_LightTowerY.SetIO(true);
            //pMachine.m_LightTowerY.SetIO(true);
        }

        private void frm_SignificantAdvice_Load(object sender, EventArgs e)
        {
            textBox1.Text = ng_msg;
            //if (!MachineDataDefine.machineState.b_Usehummer)//蜂鸣器未禁用
            //{
            //    pMachine.m_Buzzer.SetIO(true);
            //}
            //pMachine.m_LightTowerR.SetIO(true);
            //pMachine.m_LightTowerG.SetIO(false);
            //pMachine.m_LightTowerY.SetIO(false);
        }

        private void UpdataSignificant(string ngstr)
        {
            textBox1.Text = ngstr;
            //if (!MachineDataDefine.machineState.b_Usehummer)//蜂鸣器未禁用
            //{
            //    pMachine.m_Buzzer.SetIO(true);
            //}
            //pMachine.m_LightTowerR.SetIO(true);
            //pMachine.m_LightTowerG.SetIO(false);
            //pMachine.m_LightTowerY.SetIO(false);
        }
    }
}
