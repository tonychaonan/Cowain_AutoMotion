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
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Diagnostics;
using Cowain_AutoMotion.Flow;
using Cowain_AutoMotion.Flow.Hive;
using Chart;
using Cowain_Machine;
using Cowain_AutoMotion;
using Cowain;

namespace Cowain_Form.FormView
{
    public partial class dia_EMG : Form
    {

        public dia_EMG(ref clsMachine pM, dia_ShowStatus enShowStatus)
        {
            pMachine = pM;
            InitializeComponent();

            m_enShowStatus = enShowStatus;
        }
        clsMachine pMachine;

        private delegate void autoClickOk();
        private autoClickOk click;
        public void AutoClickOK()
        {
            Btn_Ok.PerformClick();
        }

        public enum dia_ShowStatus
        {
            enShowEMG,             //EMG
            enShowAirInsufficient, //壓力不足
            enShowDoorisNotClosed, //門未關
            enChangeGantry1Glue, //胶量
            enChangeGantry2Glue,
            enChangeGantry1GlueTime, //换胶
            enChangeGantry2GlueTime,
            enHIVEReveiceFail,
            enNgRunnerAlram,
        }
        dia_ShowStatus m_enShowStatus;

        string starttime = "";
        string stoptime = "";
        ChartTime.MachineStatus oldstatus = ChartTime.MachineStatus.idle;

        /// <summary>
        /// 报警开始时间
        /// </summary>
        public long Starterror_time = 0;
        /// <summary>
        /// 报警结束时间
        /// </summary>
        public long Stoperror_time = 0;

        string errorcode = "";
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
            label_Title.Text = (m_enShowStatus == dia_ShowStatus.enShowEMG) ? "EMG" : "ERR";
            label_Title.BackColor = Color.Red;

            if (m_enShowStatus == dia_ShowStatus.enShowEMG)
            {
                errorcode = "2031";
                label_異常說明.Text = JudgeLanguage.JudgeLag("急停訊號觸發 - 請確認設備狀況 , 解除後並進行設備回Home");
            }
            else if (m_enShowStatus == dia_ShowStatus.enShowAirInsufficient)
                label_異常說明.Text = JudgeLanguage.JudgeLag("空壓壓力不足 - 請確認設備Air進氣錶頭 & 廠務空壓狀況");
            else if (m_enShowStatus == dia_ShowStatus.enShowDoorisNotClosed)
            {
                errorcode = "2030";
                label_異常說明.Text = JudgeLanguage.JudgeLag("Door Sensor - 請確認設備安全門狀況");
            }
            else if (m_enShowStatus == dia_ShowStatus.enChangeGantry1Glue)
            {
                errorcode = "1";//为空会发上一个报警
                label_Title.Text = "Change Glue";
                label_異常說明.Text = JudgeLanguage.JudgeLag("Change Glue - 前龙门胶水即将用完，请更换胶水!");
            }
            else if (m_enShowStatus == dia_ShowStatus.enChangeGantry2Glue)
            {
                errorcode = "1";//为空会发上一个报警
                label_Title.Text = "Change Glue";
                label_異常說明.Text = JudgeLanguage.JudgeLag("Change Glue - 后龙门胶水即将用完，请更换胶水!");
            }
            else if (m_enShowStatus == dia_ShowStatus.enChangeGantry1GlueTime)
            {
                errorcode = "1";//为空会发上一个报警
                label_Title.Text = "Change Glue";
                label_異常說明.Text = JudgeLanguage.JudgeLag("前龙门胶水使用时间预警，请热胶!");
            }
            else if (m_enShowStatus == dia_ShowStatus.enChangeGantry2GlueTime)
            {
                errorcode = "1";//为空会发上一个报警
                label_Title.Text = "Change Glue";
                label_異常說明.Text = JudgeLanguage.JudgeLag("后龙门胶水使用时间预警，请热胶!");
            }
            else if (m_enShowStatus == dia_ShowStatus.enHIVEReveiceFail)
            {
                errorcode = "1";//为空会发上一个报警
                label_Title.Text = "HIVE Disconnect";
                label_異常說明.Text = JudgeLanguage.JudgeLag("HIVE连接超时!确认HIVE连接后重启程序!");
                return;
            }
            else if (m_enShowStatus == dia_ShowStatus.enNgRunnerAlram)
            {
                errorcode = "1";//为空会发上一个报警
                label_Title.Text = "NG Runner Alram";
                label_異常說明.Text = JudgeLanguage.JudgeLag("NG料仓有料!请及时取出!");
            }
            starttime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");
            Task.Run(() =>
            {
                frm_Main.formData.Chartcapacity1.AddDT();
                frm_Main.formError.ErrorUnit1.StartErrorMessage(errorcode);

                if (!MachineDataDefine.machineState.b_UseTestRun)
                {
                    frm_Main.formData.ChartTime1.StartError();
                }
                if (frm_Main.formError.ErrorUnit1.ErrorMessage != "NULL")
                {
                    if (MachineDataDefine.machineState.b_UseHive)
                    {
                        if (!MachineDataDefine.machineState.b_UseTestRun)
                        {
                            if (frm_Main.formError.ErrorUnit1.ErrorCode != "" && frm_Main.formError.ErrorUnit1.ErrorMessage != "" && frm_Main.formError.ErrorUnit1.ErrorType != "" && !HIVEDataDefine.Hive_machineState.IsSendErrorDown)
                            {
                                HIVE.HIVEInstance.HiveSendMACHINESTATE(5, frm_Main.formError.ErrorUnit1.ErrorCode, frm_Main.formError.ErrorUnit1.ErrorMessage, frm_Main.formError.ErrorUnit1.ErrorType,false);
                            }
                        }

                    }
                    if (MachineDataDefine.machineState.b_UseRemoteQualification)
                    {
                        if (!MachineDataDefine.machineState.b_UseTestRun)
                        {
                            if (frm_Main.formError.ErrorUnit1.ErrorCode != "" && frm_Main.formError.ErrorUnit1.ErrorMessage != "" && frm_Main.formError.ErrorUnit1.ErrorType != "" && !HIVEDataDefine.Hive_machineState.IsSendErrorDown)
                            {
                                HIVE.HIVEInstance.HiveSendMACHINESTATE(5, frm_Main.formError.ErrorUnit1.ErrorCode, frm_Main.formError.ErrorUnit1.ErrorMessage, frm_Main.formError.ErrorUnit1.ErrorType, true);
                            }
                        }

                    }
                }
            });
            //---------------------------
        }
        private void frm_ErrorDlg_Load(object sender, EventArgs e)
        {
            //新增内容
            oldstatus = frm_Main.formData.ChartTime1.RunStatus;
            frm_Main.formData.ChartTime1.StartError();
        }

        private void Btn_Ok_Click(object sender, EventArgs e)
        {

            if (m_enShowStatus == dia_ShowStatus.enShowEMG && pMachine.m_EmgIO.GetValue())   //急停訊號解除, 才關閉視窗
            {
                pMachine.m_Buzzer.SetIO(false);
                pMachine.ReMoveEmgStatus();
                this.Close();

            }
            if (m_enShowStatus == dia_ShowStatus.enShowAirInsufficient && pMachine.m_AirOk.GetValue())   //急停訊號解除, 才關閉視窗
            {
                pMachine.m_Buzzer.SetIO(false);
                pMachine.ReMoveAirInsufficient();
                this.Close();
            }

            if (m_enShowStatus == dia_ShowStatus.enShowDoorisNotClosed)
            //pMachine.m_DoorSR1.GetValue() && pMachine.m_DoorSR2.GetValue() &&
            //pMachine.m_DoorSR3.GetValue()  )   //急停訊號解除, 才關閉視窗
            {
                bool b_Open = CheckDoors.openDoor();
                if (b_Open != true)
                {
                    pMachine.m_Buzzer.SetIO(false);
                    pMachine.DisablePause();
                    pMachine.m_SgTower.SetLightStatus(false, true, false, false);//停止-黄灯亮     
                    this.Close();
                }
            }
            if (m_enShowStatus == dia_ShowStatus.enChangeGantry1Glue || m_enShowStatus == dia_ShowStatus.enChangeGantry1GlueTime)
            {
                //  pMachine.m_SgTower.TowerLight(clsMSignalTower.enTowerMode.enAlarm);
                pMachine.m_Buzzer.SetIO(false);
                Close();
            }
            if (m_enShowStatus == dia_ShowStatus.enChangeGantry2Glue || m_enShowStatus == dia_ShowStatus.enChangeGantry2GlueTime)
            {
                // pMachine.m_SgTower.TowerLight(clsMSignalTower.enTowerMode.enAlarm);
                pMachine.m_Buzzer.SetIO(false);
                Close();
            }
            if (m_enShowStatus == dia_ShowStatus.enNgRunnerAlram)
            {
                pMachine.m_Buzzer.SetIO(false);
                Close();
            }
            if (m_enShowStatus == dia_ShowStatus.enHIVEReveiceFail)
            {
                // if(new Ping().Send("10.0.0.2", 3000).Status.ToString().Trim() == "Success")
                {
                    HIVE.HIVEInstance.HIVE_Error = false;
                    HIVE.HIVEInstance.HIVE_Reveice_Status = true;
                    pMachine.m_Buzzer.SetIO(false);
                    Close();
                    return;
                }
            }
            stoptime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");
            Stoperror_time = DateTime.Now.Ticks;
            if (frm_Main.formError.ErrorUnit1.ErrorMessage != "NULL" && frm_Main.formError.ErrorUnit1.ErrorType != "")
            {
                HIVE.HIVEInstance.HiveSend(frm_Main.formError.ErrorUnit1.ErrorCode, frm_Main.formError.ErrorUnit1.ErrorType, frm_Main.formError.ErrorUnit1.ErrorMessage, starttime, stoptime, Starterror_time, Stoperror_time);
                //if (MachineDataDefine.machineState.b_UseHive && !MachineDataDefine.machineState.b_UseTestRun)
                //{
                //    HIVE.HIVEInstance.HiveSendERRORDATA(frm_Main.formError.ErrorUnit1.ErrorCode, frm_Main.formError.ErrorUnit1.ErrorType, frm_Main.formError.ErrorUnit1.ErrorMessage, starttime, stoptime, Starterror_time, Stoperror_time,false);
                //}
                //if (MachineDataDefine.machineState.b_UseRemoteQualification)
                //{
                //    HIVE.HIVEInstance.HiveSendERRORDATA(frm_Main.formError.ErrorUnit1.ErrorCode, frm_Main.formError.ErrorUnit1.ErrorType, frm_Main.formError.ErrorUnit1.ErrorMessage, starttime, stoptime, Starterror_time, Stoperror_time,true);
                //}
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

        private void dia_EMG_FormClosing(object sender, FormClosingEventArgs e)
        {
            //新增内容
            //frm_Main.formData.ChartTime1.StartWait();
            //pMachine.m_Buzzer.SetIO(false);
            pMachine.ReMoveEmgStatus();
            //this.Close();
        }
    }
}
