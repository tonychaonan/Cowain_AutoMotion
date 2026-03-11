using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cowain_Machine.Flow;
using System.Threading;
using MotionBase;
using Cowain_Machine;
using Cowain;

namespace Cowain_Form.FormView
{
    public partial class dia_Pause : Form
    {

        public dia_Pause(ref clsMachine pM, dia_ShowStatus enShowStatus)
        {
            pMachine = pM;
            InitializeComponent();
            m_enShowStatus = enShowStatus;
        }
        clsMachine pMachine;

        public enum dia_ShowStatus
        {
            enShowEMG,             //EMG
            enShowAirInsufficient, //壓力不足
        }
        dia_ShowStatus m_enShowStatus;


        private void frm_ErrorDlg_Shown(object sender, EventArgs e)
        {
            pMachine.m_LightTowerR.SetIO(true);
            pMachine.m_LightTowerG.SetIO(false);
            pMachine.m_LightTowerY.SetIO(false);
            if (!MachineDataDefine.machineState.b_Usehummer)
            {
                pMachine.m_Buzzer.SetIO(true);
            }
            //---------------------------
            label_Title.Text = (m_enShowStatus == dia_ShowStatus.enShowEMG) ? "EMG" : "Insufficient pressure";
            label_Title.BackColor = (m_enShowStatus == dia_ShowStatus.enShowEMG) ? Color.Red : Color.YellowGreen;

            if (m_enShowStatus == dia_ShowStatus.enShowEMG)
                label_異常說明.Text = JudgeLanguage.JudgeLag("急停訊號觸發 - 請確認設備狀況 , 解除後並進行設備回Home");
            else
                label_異常說明.Text = JudgeLanguage.JudgeLag("空壓壓力不足 - 請確認設備Air進氣錶頭 & 廠務空壓狀況");
            //---------------------------
        }
        private void frm_ErrorDlg_Load(object sender, EventArgs e)
        {
 
        }

        private void Btn_Ok_Click(object sender, EventArgs e)
        {
            if(m_enShowStatus == dia_ShowStatus.enShowEMG &&  pMachine.m_EmgIO.GetValue())   //急停訊號解除, 才關閉視窗
            {
                pMachine.ReMoveEmgStatus();
                this.Close();
            }

            if (m_enShowStatus == dia_ShowStatus.enShowAirInsufficient && pMachine.m_AirOk.GetValue())   //急停訊號解除, 才關閉視窗
            {
                pMachine.ReMoveAirInsufficient();
                this.Close();
            }
        }
        private void frm_ErrorDlg_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.F4)  //Pass Alt+F4按鈕
                e.Handled = true;
        }

        private void btn_BuzzerOff_Click(object sender, EventArgs e)
        {
            pMachine.m_Buzzer.SetIO(false);
        }
    }
}
