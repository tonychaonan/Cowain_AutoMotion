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
using Chart;   //EVT三色灯新增
using System.Threading.Tasks;
using System.Diagnostics;
using Cowain_Machine;
using Cowain_AutoMotion.Flow.Hive;
using Cowain_AutoMotion;
using Cowain_AutoMotion.Flow;

namespace Cowain_Form.FormView
{
    public partial class frm_ErrorDlg : Form
    {
        public frm_ErrorDlg()
        {
            InitializeComponent();
        }

        public frm_ErrorDlg(ref Error pError, ref clsMachine pM)
        {
            m_Error = pError;
            pMachine = pM;
            InitializeComponent();
        }
        clsMachine pMachine;
        
        ImageList ImgList = new ImageList();
        int m_iSlectet = 0;
        Error m_Error;
        //public frm_Auto pAutoForm=null;
        string starttime = "";
        string stoptime = "";
        ChartTime.MachineStatus oldstatus = ChartTime.MachineStatus.idle;

        /// <summary>
        /// 报警计时器
        /// </summary>
        private Stopwatch sw;
        /// <summary>
        /// 报警开始时间
        /// </summary>
        public long Starterror_time = 0;
        /// <summary>
        /// 报警结束时间
        /// </summary>
        public long Stoperror_time = 0;
        /// <summary>
        /// 报警弹窗显示超过一定时间，HIVE自动上传Data
        /// </summary>
        int AlarmShowTime = 0;
        /// <summary>
        /// HIVEErrorData上传完成标识
        /// </summary>
        private bool HIVEErrorDataCompelet = false;

        private void frm_ErrorDlg_Shown(object sender, EventArgs e)
        {
            AlarmShowTime = 0;
            clsMSignalTower.NoUse_m_bBuzzer = false;
            //MachineDataDefine.ChkStatus.UpLoadError++;
            Starterror_time = DateTime.Now.Ticks;
            starttime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");
            ImgList.Images.Add("SelectRecipe", Cowain_AutoMotion.Properties.Resources.SetOk);
            ImgList.Images.Add("DefaultRecipe", Cowain_AutoMotion.Properties.Resources.SetOk_Disable);
            Task.Run(() =>
            {
                {
                    frm_Main.formData.Chartcapacity1.AddDT();
                    frm_Main.formError.ErrorUnit1.StartErrorMessage(m_Error.m_ErrorCode.ToString("0000"));

                    if (!MachineDataDefine.machineState.b_UseTestRun)
                    {
                        frm_Main.formData.ChartTime1.StartError();
                    }

                    if (!HIVE.HIVEInstance.NotUploadErrorMessageList.Exists(t => t == frm_Main.formError.ErrorUnit1.ErrorMessage) && frm_Main.formError.ErrorUnit1.ErrorCode != "" && frm_Main.formError.ErrorUnit1.ErrorType != "")
                    {
                        if (MachineDataDefine.machineState.b_UseHive)
                        {
                            //if (!MachineDataDefine.machineState.b_UseTestRun && MachineDataDefine.machineState.ErrorCode != m_Error.m_ErrorCode.ToString("0000")
                            ///*&& MachineDataDefine.ChkStatus.UpLoadError == 1*/ && !HIVEDataDefine.Hive_machineState.IsSendErrorDown&& oldstatus!= ChartTime.MachineStatus.idle&& oldstatus != ChartTime.MachineStatus.planned_downtime)
                            if ( m_Error.m_ErrorCode.ToString("0000")!="0000"
                          /*&& MachineDataDefine.ChkStatus.UpLoadError == 1*/ && !HIVEDataDefine.Hive_machineState.IsSendErrorDown && oldstatus != ChartTime.MachineStatus.idle)

                            {
                                HIVE.HIVEInstance.HiveSendMACHINESTATE(5, frm_Main.formError.ErrorUnit1.ErrorCode, frm_Main.formError.ErrorUnit1.ErrorMessage, frm_Main.formError.ErrorUnit1.ErrorType,false);
                            }
                            //if (MachineDataDefine.ChkStatus.UpLoadError >= 6)
                            //    MachineDataDefine.ChkStatus.UpLoadError = 0;
                        }
                        if (MachineDataDefine.machineState.b_UseRemoteQualification)
                        {
                            if (!MachineDataDefine.machineState.b_UseTestRun && MachineDataDefine.machineState.ErrorCode != m_Error.m_ErrorCode.ToString("0000")
                            /*&& MachineDataDefine.ChkStatus.UpLoadError == 1*/ && !HIVEDataDefine.Hive_machineState.IsSendErrorDown && oldstatus != ChartTime.MachineStatus.idle && oldstatus != ChartTime.MachineStatus.planned_downtime)
                            {
                                HIVE.HIVEInstance.HiveSendMACHINESTATE(5, frm_Main.formError.ErrorUnit1.ErrorCode, frm_Main.formError.ErrorUnit1.ErrorMessage, frm_Main.formError.ErrorUnit1.ErrorType,true);
                            }
                            //if (MachineDataDefine.ChkStatus.UpLoadError >= 6)
                            //    MachineDataDefine.ChkStatus.UpLoadError = 0;
                        }
                    }
                }
            });
            if (!MachineDataDefine.machineState.b_Usehummer)
            {
                pMachine.m_Buzzer.SetIO(true);
            }

            Fun_DisplayList();
            Task.Run(() =>
            {
                SaveErrorLog();
            });

            Task.Run(() =>
            {
                UpLoadHIVEErrorData();
            });
        }

        private void frm_ErrorDlg_Load(object sender, EventArgs e)
        {  //EVT三色灯新增
            Task.Run(() =>
            {
                oldstatus = frm_Main.formData.ChartTime1.RunStatus;
                frm_Main.formData.ChartTime1.StartError();
            });
            //if (m_Error.m_ErrorType == Error.ErrorType.錯誤)
            //    label_Title.Text = "ErrorCode: " + m_Error.m_ErrorCode.ToString("0000");
            //if (m_Error.m_ErrorType == Error.ErrorType.警告)
            //    label_Title.Text = "WarrningCode: " + m_Error.m_ErrorCode.ToString("0000");

            labCode.Text = m_Error.m_ErrorCode.ToString("0000");
            labCategory.Text = m_Error.m_ErrorType.ToString();
            labCode.ToolTip = m_Error.m_strCDescript;
            labDetail.Text = m_Error.m_strCDescript;
            labDetail.ToolTip = m_Error.m_strCDescript;
            labUser.Text = MachineDataDefine.m_LoginUserName.Trim();
            //状态开始时间
            DateTime stime = frm_Main.formData.ChartTime1.StatusTime;
            labSTime.Text = stime.ToString("yyyy/MM/dd HH:mm:ss");

            //label_station.Text = m_Error.
            int iErrorStationNo = m_Error.GetErrorStation();
            string strStCName = m_Error.GetErrorStationCName();

            label_station.Text = "站号:" + iErrorStationNo.ToString() + " , " + strStCName;
            label_異常說明.Text = m_Error.m_strCDescript;
            Btn_help.Visible = false;
            if (m_Error.m_strCDescript.Contains("胶") || m_Error.m_strCDescript.Contains("喷嘴"))
                Btn_help.Visible = true;
            //pMachine.m_SgTower.SetBuzzerOn();

        }
        public void Fun_DisplayList()
        {
            comboBox_Sloution.Items.Clear();
            listView_Sloution.Items.Clear();
            listView_Sloution.LargeImageList = ImgList;
            listView_Sloution.SmallImageList = ImgList;
            int SloutionCount = m_Error.SloutionList.Count;

            if (SloutionCount != 0)
            {
                for (int i = 0; i < SloutionCount; i++)
                {
                    string strSloution = m_Error.SloutionList[i].Sloution;
                    comboBox_Sloution.Items.Add(strSloution);
                    if (m_iSlectet == i)
                        listView_Sloution.Items.Add(strSloution, "SelectRecipe");
                    else
                        listView_Sloution.Items.Add(strSloution, "DefaultRecipe");
                }
                if (m_iSlectet == SloutionCount)
                    listView_Sloution.Items.Add("停机/Stop", "SelectRecipe");
                else
                    listView_Sloution.Items.Add("停机/Stop", "DefaultRecipe");
                //--------------------------------------
                comboBox_Sloution.Items.Add("停机/Stop");
            }
            else
            {
                m_iSlectet = 0;
                comboBox_Sloution.Items.Add("停机/Stop");
                listView_Sloution.Items.Add("停机/Stop", "SelectRecipe");
            }
            comboBox_Sloution.SelectedIndex = m_iSlectet;
        }
        private void comboBox_Sloution_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_iSlectet != comboBox_Sloution.SelectedIndex)
            {
                m_iSlectet = comboBox_Sloution.SelectedIndex;
                Fun_DisplayList();
            }
        }
        private void listView_Sloution_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_iSlectet != listView_Sloution.FocusedItem.Index)
                {
                    m_iSlectet = listView_Sloution.FocusedItem.Index;
                    Fun_DisplayList();
                }
            }
            catch (Exception)
            {
            }
        }

        private void Btn_Ok_Click(object sender, EventArgs e)
        {

            #region
            //if ((int)pMachine.m_LoginUser <= 1)
            //{
            //    #region
            //    //
            //    if (!MachineDataDefine.machineState.b_UseMesLogin)
            //    {
            //        dia_Login_New m_LoginDlg = new dia_Login_New(ref pMachine);
            //        if (m_LoginDlg.ShowDialog() == DialogResult.OK)
            //        {
            //            if (pMachine.m_LoginUser == Sys_Define.enPasswordType.UnLogin || MachineDataDefine.Login_Name.Trim().Equals(""))
            //            {
            //                MsgBoxHelper.DxMsgShowErr("登录失败！");
            //                LogAuto.Notify("登录失败" + ":" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + $" User:{MachineDataDefine.Login_Name} Function:{MachineDataDefine.Login_Function} UID:{MachineDataDefine.Login_CardID} Authorize:{MachineDataDefine.Authorize_Name} Function:{MachineDataDefine.Authorize_Function} UID:{MachineDataDefine.Authorize_CardID} ", (int)MachineStation.主监控, LogLevel.Info);
            //                string ss = $" User:{MachineDataDefine.Login_Name} Function:{MachineDataDefine.Login_Function} UID:{MachineDataDefine.Login_CardID} Authorize:{MachineDataDefine.Authorize_Name} Function:{MachineDataDefine.Authorize_Function} UID:{MachineDataDefine.Authorize_CardID} ";
            //                MachineDataDefine.msg = ss;
            //                return;
            //            }
            //            else if ((int)pMachine.m_LoginUser <= 1)
            //            {
            //                MsgBoxHelper.DxMsgShowErr("权限不够，请登录Level2或Level3用户！");
            //                LogAuto.Notify("权限不够，请登录Level2或Level3用户" + ":" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + $" User:{MachineDataDefine.Login_Name} Function:{MachineDataDefine.Login_Function} UID:{MachineDataDefine.Login_CardID} Authorize:{MachineDataDefine.Authorize_Name} Function:{MachineDataDefine.Authorize_Function} UID:{MachineDataDefine.Authorize_CardID} ", (int)MachineStation.主监控, LogLevel.Info);
            //                string ss = $" User:{MachineDataDefine.Login_Name} Function:{MachineDataDefine.Login_Function} UID:{MachineDataDefine.Login_CardID} Authorize:{MachineDataDefine.Authorize_Name} Function:{MachineDataDefine.Authorize_Function} UID:{MachineDataDefine.Authorize_CardID} ";
            //                MachineDataDefine.msg = ss;
            //                return;
            //            }
            //            else
            //            {
            //                labUser.Text = MachineDataDefine.Login_Name.Trim();
            //                pMachine.NeedRef = true;
            //                LogAuto.Notify("登录成功" + ":" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + $" User:{MachineDataDefine.Login_Name} Function:{MachineDataDefine.Login_Function} UID:{MachineDataDefine.Login_CardID} Authorize:{MachineDataDefine.Authorize_Name} Function:{MachineDataDefine.Authorize_Function} UID:{MachineDataDefine.Authorize_CardID} ", (int)MachineStation.主监控, LogLevel.Info);
            //                MachineDataDefine.msg = $" User:{MachineDataDefine.Login_Name} Function:{MachineDataDefine.Login_Function} UID:{MachineDataDefine.Login_CardID} Authorize:{MachineDataDefine.Authorize_Name} Function:{MachineDataDefine.Authorize_Function} UID:{MachineDataDefine.Authorize_CardID} ";
            //                frm_PlanDoubleConfirm doubleConfm = new frm_PlanDoubleConfirm(ref pMachine);
            //                doubleConfm.NeedSN = false;
            //                doubleConfm.PlanType = "";
            //                if (doubleConfm.ShowDialog() == DialogResult.OK)
            //                {
            //                    this.DialogResult = DialogResult.OK;
            //                    LogAuto.Notify("二次确认登录" + ":" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + $" User:{null} Function:{null} UID:{MachineDataDefine.m_CardID} Authorize:{MachineDataDefine.Authorize_Name} Function:{MachineDataDefine.Authorize_Function} UID:{MachineDataDefine.Authorize_CardID} ", (int)MachineStation.主监控, LogLevel.Info);
            //                    MachineDataDefine.msg = $" User:{null} Function:{null} UID:{MachineDataDefine.m_CardID} Authorize:{MachineDataDefine.Authorize_Name} Function:{MachineDataDefine.Authorize_Function} UID:{MachineDataDefine.Authorize_CardID} ";
            //                }
            //            }
            //        }
            //        else
            //        {
            //            return;
            //        }
            //    }
            //    #endregion
            //    #region 
            //    else
            //    {
            //        dia_Login_Remote m_LoginDlg = new dia_Login_Remote(ref pMachine);
            //        if (m_LoginDlg.ShowDialog() == DialogResult.OK)
            //        {
            //            if (pMachine.m_LoginUser == Sys_Define.enPasswordType.UnLogin || MachineDataDefine.Login_Name.Trim().Equals(""))
            //            {
            //                MsgBoxHelper.DxMsgShowErr("登录失败！");
            //                LogAuto.Notify("登录失败" + ":" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + $" User:{MachineDataDefine.Login_Name} Function:{MachineDataDefine.Login_Function} UID:{MachineDataDefine.Login_CardID} Authorize:{MachineDataDefine.Authorize_Name} Function:{MachineDataDefine.Authorize_Function} UID:{MachineDataDefine.Authorize_CardID} ", (int)MachineStation.主监控, LogLevel.Info);
            //                MachineDataDefine.msg = $" User:{MachineDataDefine.Login_Name} Function:{MachineDataDefine.Login_Function} UID:{MachineDataDefine.Login_CardID} Authorize:{MachineDataDefine.Authorize_Name} Function:{MachineDataDefine.Authorize_Function} UID:{MachineDataDefine.Authorize_CardID} ";
            //                return;
            //            }
            //            else if ((int)pMachine.m_LoginUser <= 1)
            //            {
            //                MsgBoxHelper.DxMsgShowErr("权限不够，请登录Level2或Level3用户！");
            //                LogAuto.Notify("权限不够，请登录Level2或Level3用户" + ":" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + $" User:{MachineDataDefine.Login_Name} Function:{MachineDataDefine.Login_Function} UID:{MachineDataDefine.Login_CardID} Authorize:{MachineDataDefine.Authorize_Name} Function:{MachineDataDefine.Authorize_Function} UID:{MachineDataDefine.Authorize_CardID} ", (int)MachineStation.主监控, LogLevel.Info);
            //                MachineDataDefine.msg = $" User:{MachineDataDefine.Login_Name} Function:{MachineDataDefine.Login_Function} UID:{MachineDataDefine.Login_CardID} Authorize:{MachineDataDefine.Authorize_Name} Function:{MachineDataDefine.Authorize_Function} UID:{MachineDataDefine.Authorize_CardID} ";
            //                return;
            //            }
            //            else
            //            {
            //                labUser.Text = MachineDataDefine.Login_Name.Trim();
            //                pMachine.NeedRef = true;
            //                LogAuto.Notify("登录成功" + ":" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + $" User:{MachineDataDefine.Login_Name} Function:{MachineDataDefine.Login_Function} UID:{MachineDataDefine.Login_CardID} Authorize:{MachineDataDefine.Authorize_Name} Function:{MachineDataDefine.Authorize_Function} UID:{MachineDataDefine.Authorize_CardID} ", (int)MachineStation.主监控, LogLevel.Info);
            //                MachineDataDefine.msg = $" User:{MachineDataDefine.Login_Name} Function:{MachineDataDefine.Login_Function} UID:{MachineDataDefine.Login_CardID} Authorize:{MachineDataDefine.Authorize_Name} Function:{MachineDataDefine.Authorize_Function} UID:{MachineDataDefine.Authorize_CardID} ";
            //                frm_PlanDoubleConfirm doubleConfm = new frm_PlanDoubleConfirm(ref pMachine);
            //                doubleConfm.NeedSN = false;
            //                doubleConfm.PlanType = "";
            //                if (doubleConfm.ShowDialog() == DialogResult.OK)
            //                {
            //                    this.DialogResult = DialogResult.OK;
            //                    LogAuto.Notify("二次确认登录" + ":" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + $" User:{"null"} Function:{"null"} UID:{MachineDataDefine.m_CardID} Authorize:{MachineDataDefine.Authorize_Name} Function:{MachineDataDefine.Authorize_Function} UID:{MachineDataDefine.Authorize_CardID} ", (int)MachineStation.主监控, LogLevel.Info);
            //                    MachineDataDefine.msg = $" User:{"null"} Function:{"null"} UID:{MachineDataDefine.m_CardID} Authorize:{MachineDataDefine.Authorize_Name} Function:{MachineDataDefine.Authorize_Function} UID:{MachineDataDefine.Authorize_CardID} ";
            //                }
            //                //else if (doubleConfm.ShowDialog() != DialogResult.OK)
            //                // {
            //                //     return;
            //                // }
            //            }
            //        }
            //        else
            //        {
            //            return;
            //        }
            //    }
            //    #endregion
            //    //------------
            //    HIVE.HIVEInstance.HiveSendMACHINESTATE_Error(5, frm_Main.formError.ErrorUnit1.ErrorCode, frm_Main.formError.ErrorUnit1.ErrorMessage, frm_Main.formError.ErrorUnit1.ErrorType, false, MachineDataDefine.m_CardID);
            //}
            //else if ((int)pMachine.m_LoginUser > 1)
            //{
            //    frm_PlanDoubleConfirm doubleConfm = new frm_PlanDoubleConfirm(ref pMachine);
            //    doubleConfm.NeedSN = false;
            //    doubleConfm.PlanType = "";
            //    if (doubleConfm.ShowDialog() == DialogResult.OK)
            //    {
            //        this.DialogResult = DialogResult.OK;
            //        LogAuto.Notify("二次确认登录" + ":" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + $" User:{"null"} Function:{"null"} UID:{MachineDataDefine.m_CardID} Authorize:{MachineDataDefine.Authorize_Name} Function:{MachineDataDefine.Authorize_Function} UID:{MachineDataDefine.Authorize_CardID} ", (int)MachineStation.主监控, LogLevel.Info);
            //        MachineDataDefine.msg = $" User:{"null"} Function:{"null"} UID:{MachineDataDefine.m_CardID} Authorize:{MachineDataDefine.Authorize_Name} Function:{MachineDataDefine.Authorize_Function} UID:{MachineDataDefine.Authorize_CardID} ";
            //    }

            //    #region
            //    //
            //    //if (!MachineDataDefine.machineState.b_UseMesLogin)
            //    //{
            //    //    dia_Login_New m_LoginDlg = new dia_Login_New(ref pMachine);
            //    //    if (m_LoginDlg.ShowDialog() == DialogResult.OK)
            //    //    {
            //    //        if (pMachine.m_LoginUser == Sys_Define.enPasswordType.UnLogin || MachineDataDefine.Login_Name.Trim().Equals(""))
            //    //        {
            //    //            MsgBoxHelper.DxMsgShowErr("登录失败！");
            //    //            LogAuto.Notify("登录失败" + ":" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + $" User:{MachineDataDefine.Login_Name} Function:{MachineDataDefine.Login_Function} UID:{MachineDataDefine.Login_CardID} Authorize:{MachineDataDefine.Authorize_Name} Function:{MachineDataDefine.Authorize_Function} UID:{MachineDataDefine.Authorize_CardID} ", (int)MachineStation.主监控, LogLevel.Info);
            //    //            return;
            //    //        }
            //    //        else if ((int)pMachine.m_LoginUser <= 1)
            //    //        {
            //    //            MsgBoxHelper.DxMsgShowErr("权限不够，请登录Level2或Level3用户！");
            //    //            LogAuto.Notify("权限不够，请登录Level2或Level3用户" + ":" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + $" User:{MachineDataDefine.Login_Name} Function:{MachineDataDefine.Login_Function} UID:{MachineDataDefine.Login_CardID} Authorize:{MachineDataDefine.Authorize_Name} Function:{MachineDataDefine.Authorize_Function} UID:{MachineDataDefine.Authorize_CardID} ", (int)MachineStation.主监控, LogLevel.Info);
            //    //            return;
            //    //        }
            //    //        else
            //    //        {
            //    //            labUser.Text = MachineDataDefine.Login_Name.Trim();
            //    //            pMachine.NeedRef = true;
            //    //            MachineDataDefine.msg = $" User:{MachineDataDefine.Login_Name} Function:{MachineDataDefine.Login_Function} UID:{MachineDataDefine.Login_CardID} Authorize:{MachineDataDefine.Authorize_Name} Function:{MachineDataDefine.Authorize_Function} UID:{MachineDataDefine.Authorize_CardID} ";
            //    //            LogAuto.Notify("登录成功" + ":" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + $" User:{MachineDataDefine.Login_Name} Function:{MachineDataDefine.Login_Function} UID:{MachineDataDefine.Login_CardID} Authorize:{MachineDataDefine.Authorize_Name} Function:{MachineDataDefine.Authorize_Function} UID:{MachineDataDefine.Authorize_CardID} ", (int)MachineStation.主监控, LogLevel.Info);
            //    //        }
            //    //    }
            //    //    else
            //    //    {
            //    //        return;
            //    //    }
            //    //}
            //    #endregion
            //    #region 
            //    //else
            //    //{
            //    //    dia_Login_Remote m_LoginDlg = new dia_Login_Remote(ref pMachine);
            //    //    if (m_LoginDlg.ShowDialog() == DialogResult.OK)
            //    //    {
            //    //        if (pMachine.m_LoginUser == Sys_Define.enPasswordType.UnLogin || MachineDataDefine.Login_Name.Trim().Equals(""))
            //    //        {
            //    //            MsgBoxHelper.DxMsgShowErr("登录失败！");
            //    //            LogAuto.Notify("登录失败" + ":" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + $" User:{MachineDataDefine.Login_Name} Function:{MachineDataDefine.Login_Function} UID:{MachineDataDefine.Login_CardID} Authorize:{MachineDataDefine.Authorize_Name} Function:{MachineDataDefine.Authorize_Function} UID:{MachineDataDefine.Authorize_CardID} ", (int)MachineStation.主监控, LogLevel.Info);
            //    //            return;
            //    //        }
            //    //        else if ((int)pMachine.m_LoginUser <= 1)
            //    //        {
            //    //            MsgBoxHelper.DxMsgShowErr("权限不够，请登录Level2或Level3用户！");
            //    //            LogAuto.Notify("权限不够，请登录Level2或Level3用户" + ":" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + $" User:{MachineDataDefine.Login_Name} Function:{MachineDataDefine.Login_Function} UID:{MachineDataDefine.Login_CardID} Authorize:{MachineDataDefine.Authorize_Name} Function:{MachineDataDefine.Authorize_Function} UID:{MachineDataDefine.Authorize_CardID} ", (int)MachineStation.主监控, LogLevel.Info);
            //    //            return;
            //    //        }
            //    //        else
            //    //        {
            //    //            labUser.Text = MachineDataDefine.Login_Name.Trim();
            //    //            pMachine.NeedRef = true;
            //    //            MachineDataDefine.msg = $" User:{MachineDataDefine.Login_Name} Function:{MachineDataDefine.Login_Function} UID:{MachineDataDefine.Login_CardID} Authorize:{MachineDataDefine.Authorize_Name} Function:{MachineDataDefine.Authorize_Function} UID:{MachineDataDefine.Authorize_CardID} ";
            //    //            LogAuto.Notify("登录成功" + ":" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + $" User:{MachineDataDefine.Login_Name} Function:{MachineDataDefine.Login_Function} UID:{MachineDataDefine.Login_CardID} Authorize:{MachineDataDefine.Authorize_Name} Function:{MachineDataDefine.Authorize_Function} UID:{MachineDataDefine.Authorize_CardID} ", (int)MachineStation.主监控, LogLevel.Info);
            //    //        }
            //    //    }
            //    //    else
            //    //    {
            //    //        return;
            //    //    }
            //    //}
            //    #endregion
            //    //------------
            //    HIVE.HIVEInstance.HiveSendMACHINESTATE_Error(5, frm_Main.formError.ErrorUnit1.ErrorCode, frm_Main.formError.ErrorUnit1.ErrorMessage, frm_Main.formError.ErrorUnit1.ErrorType, false, MachineDataDefine.m_CardID);
            //}
            //int iNextStep;
            //if (m_iSlectet != m_Error.SloutionList.Count)
            //{
            //    pMachine.m_Buzzer.SetIO(false);
            //    iNextStep = m_Error.SloutionList[m_iSlectet].SloutionStep;
            //    m_Error.SetNextSetp(iNextStep);
            //    //frm_Main.formData.ChartTime1.StartRun();
            //}
            //frm_Main.formError.ErrorUnit1.EndErrorMessage(m_Error.m_ErrorCode.ToString("0000"));
            //LogAuto.SaveErrorInfo("常规报警内容是：" + label_異常說明.Text + "-------报警后选项：" + comboBox_Sloution.Text+"-------处理人工号："+ labUser.Text.Trim());
            //pMachine.m_SgTower.Stop();
            //if (comboBox_Sloution.Text == "停机/Stop")
            //{
            //    if(MachineDataDefine.timeout==true)
            //    {
            //        MachineDataDefine.timeout = false;
            //        HardWareControl.getValve(EnumParam_Valve.Driver夹取气缸ON).Close();
            //    }
            //    pMachine.StopAuto();
            //    pMachine.m_Buzzer.SetIO(false);
            //    pMachine.m_LightTowerG.SetIO(false);
            //    pMachine.m_LightTowerR.SetIO(true);
            //    pMachine.m_LightTowerY.SetIO(false);
            //    pMachine.StopAuto();
            //    //frm_Main.formData.ChartTime1.StartWait();
            //}

            //else
            //{
            //    //pMachine.m_SgTower.TowerLight(clsMSignalTower.enTowerMode.enAutoRuning);
            //    //frm_Main.formData.ChartTime1.StartRun();
            //}
            #endregion

            if ((int)pMachine.m_LoginUser <= 1)
            {

                //
                if (!MachineDataDefine.machineState.b_UseMesLogin)
                {
                    dia_Login_New m_LoginDlg = new dia_Login_New(ref pMachine);
                    if (m_LoginDlg.ShowDialog() == DialogResult.OK)
                    {
                        if (pMachine.m_LoginUser == Sys_Define.enPasswordType.UnLogin || MachineDataDefine.m_LoginUserName.Trim().Equals(""))
                        {
                            MsgBoxHelper.DxMsgShowErr("登录失败！");
                            return;
                        }
                        else if ((int)pMachine.m_LoginUser <= 1)
                        {
                            MsgBoxHelper.DxMsgShowErr("权限不够，请登录Level2或Level3用户！");
                            return;
                        }
                        else
                        {
                            labUser.Text = MachineDataDefine.m_LoginUserName.Trim();
                            pMachine.NeedRef = true;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    dia_Login_Remote m_LoginDlg = new dia_Login_Remote(ref pMachine);
                    if (m_LoginDlg.ShowDialog() == DialogResult.OK)
                    {
                        if (pMachine.m_LoginUser == Sys_Define.enPasswordType.UnLogin || MachineDataDefine.m_LoginUserName.Trim().Equals(""))
                        {
                            MsgBoxHelper.DxMsgShowErr("登录失败！");
                            return;
                        }
                        else if ((int)pMachine.m_LoginUser <= 1)
                        {
                            MsgBoxHelper.DxMsgShowErr("权限不够，请登录Level2或Level3用户！");
                            return;
                        }
                        else
                        {
                            labUser.Text = MachineDataDefine.m_LoginUserName.Trim();
                            pMachine.NeedRef = true;
                        }
                    }
                    else
                    {
                        return;
                    }
                }

                //------------

            }
            int iNextStep;
            if (m_iSlectet != m_Error.SloutionList.Count)
            {
                pMachine.m_Buzzer.SetIO(false);
                iNextStep = m_Error.SloutionList[m_iSlectet].SloutionStep;
                m_Error.SetNextSetp(iNextStep);
                //frm_Main_New.formData.ChartTime1.StartRun();
            }
            frm_Main.formError.ErrorUnit1.EndErrorMessage(m_Error.m_ErrorCode.ToString("0000"));
            LogAuto.SaveErrorInfo("常规报警内容是：" + label_異常說明.Text + "-------报警后选项：" + comboBox_Sloution.Text + "-------处理人工号：" + labUser.Text.Trim());
            pMachine.m_SgTower.Stop();
            if (comboBox_Sloution.Text == "停机/Stop")
            {
                pMachine.StopAuto();
                pMachine.m_Buzzer.SetIO(false);
                pMachine.m_LightTowerG.SetIO(false);
                pMachine.m_LightTowerR.SetIO(true);
                pMachine.m_LightTowerY.SetIO(false);
                pMachine.StopAuto();
                //frm_Main_New.formData.ChartTime1.StartWait();
            }

            else
            {
                //pMachine.m_SgTower.TowerLight(clsMSignalTower.enTowerMode.enAutoRuning);
                //frm_Main_New.formData.ChartTime1.StartRun();
            }


            this.Close();
        }
        private void frm_ErrorDlg_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.F4)  //Pass Alt+F4按鈕
                e.Handled = true;
        }

        private void btn_BuzzerOff_Click(object sender, EventArgs e)
        {
            clsMSignalTower.NoUse_m_bBuzzer = true;
            pMachine.m_SgTower.SetBuzzerOff();
        }

        private void SaveErrorLog()
        {

            DateTime dtNow = DateTime.Now;
            String strPath = System.IO.Directory.GetCurrentDirectory();
            String strNowPath = strPath.Replace("\\bin\\x64\\Debug", "");
            String strRecordPath = strNowPath + "\\Record";
            String strErrorLogPath = strRecordPath + "\\ErrorLog";
            String strRecordYear = strErrorLogPath + "\\" + dtNow.Year.ToString();
            String strRecordMonth = strRecordYear + "\\" + string.Format("{0:D2}", dtNow.Month);
            //-------------------------
            System.IO.Directory.CreateDirectory(strRecordPath);
            System.IO.Directory.CreateDirectory(strErrorLogPath);
            System.IO.Directory.CreateDirectory(strRecordYear);
            System.IO.Directory.CreateDirectory(strRecordMonth);
            //-------------------------
            string strTxtFileName = "";
            string strWriteValue = "";

            //-------------------------
            string strRecordDate = dtNow.Year.ToString() + "/" + string.Format("{0:D2}", dtNow.Month) +
                        "/" + string.Format("{0:D2}", dtNow.Day) + "\t";
            string strRecordTime = dtNow.ToLongTimeString() + "\t";
            //-------------------------
            //string strStepID = "StepID:" + iStepID.ToString() + "\t";
            //-----------------------------------
            string[] strStName = { "Machine", "点胶站", "流道系统", "ZStop入料", "取料頭", "CCD移載", "CCD系統" };
            int iErrorStationNo = m_Error.GetErrorStation();
            //------------------
            string strStation = "StationID:" + label_station.Text + "\t";
            string strStationName = "Station:" + strStName[iErrorStationNo] + "\t";
            string strErrorCode = string.Empty;
            if (m_Error.m_ErrorType == Error.ErrorType.錯誤)
                strErrorCode = "ErrorCode: " + m_Error.m_ErrorCode.ToString("0000") + "\t";
            if (m_Error.m_ErrorType == Error.ErrorType.警告)
                strErrorCode = "WarrningCode: " + m_Error.m_ErrorCode.ToString("0000") + "\t";
            string strError = "异常说明:" + label_異常說明.Text;
            //----------------------------------
            strTxtFileName = dtNow.Year.ToString() + "-" + dtNow.Month.ToString() + "-" + dtNow.Day.ToString();
            strTxtFileName = strTxtFileName + ".txt";
            strTxtFileName = strRecordMonth + "\\" + strTxtFileName;
            //-------------------------------------------      
            strWriteValue = strRecordDate + strRecordTime + strStation + strStationName + strErrorCode + strError;
            //------------------------
            {
                System.IO.FileStream fs = new System.IO.FileStream(strTxtFileName,
                System.IO.FileMode.Create | System.IO.FileMode.Append);
                System.IO.StreamWriter sw = new System.IO.StreamWriter(fs);
                sw.WriteLine(strWriteValue);
                sw.Flush();
                sw.Close();
                fs.Close();
            }
        }

        private void frm_ErrorDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (sw.IsRunning)
                {
                    sw.Stop();
                    pMachine.m_Buzzer.SetIO(false);
                    stoptime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");
                    Stoperror_time = DateTime.Now.Ticks;
                    if (!HIVE.HIVEInstance.NotUploadErrorMessageList.Exists(t => t == frm_Main.formError.ErrorUnit1.ErrorMessage) && frm_Main.formError.ErrorUnit1.ErrorCode != "" && frm_Main.formError.ErrorUnit1.ErrorType != "")
                    {
                        HIVE.HIVEInstance.HiveSend(frm_Main.formError.ErrorUnit1.ErrorCode, frm_Main.formError.ErrorUnit1.ErrorType, frm_Main.formError.ErrorUnit1.ErrorMessage, starttime, stoptime, Starterror_time, Stoperror_time);
                        //if (MachineDataDefine.machineState.b_UseHive && !MachineDataDefine.machineState.b_UseTestRun /*&& MachineDataDefine.ChkStatus.UpLoadError == 1*/)
                        //{
                        //    HIVE.HIVEInstance.HiveSendERRORDATA(frm_Main.formError.ErrorUnit1.ErrorCode, frm_Main.formError.ErrorUnit1.ErrorType, frm_Main.formError.ErrorUnit1.ErrorMessage, starttime, stoptime, Starterror_time, Stoperror_time);
                        //}
                    }
                    MachineDataDefine.machineState.ErrorCode = m_Error.m_ErrorCode.ToString("0000");
                    clsMSignalTower.NoUse_m_bBuzzer = true;
                }
            }
            catch(Exception ex)
            {

            }
   

        }

        private void Btn_help_Click(object sender, EventArgs e)
        {
            //MachineDataDefine.ChkStatus.IsDisableAlarmBack = true;
            //MachineDataDefine.ChkStatus.IsDisableAlarmFront = true;
            //MachineDataDefine.ChkStatus.IsDisableAlarmBackIO = true;
            //MachineDataDefine.ChkStatus.IsDisableAlarmFrontIO = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //Thread.Sleep(1000);
            pMachine.m_SgTower.Stop();
            pMachine.m_SgTower.TowerLight(clsMSignalTower.enTowerMode.enAutoRuning);
            frm_Main.formData.ChartTime1.StartRun();
            this.Close();
        }
        private void UpLoadHIVEErrorData()
        {
            try
            {
                sw = Stopwatch.StartNew();
                while (sw.IsRunning && sw.ElapsedMilliseconds < 16000)
                {
                    Thread.Sleep(10);
                }
                if (sw.IsRunning && sw.ElapsedMilliseconds >= 16000)
                {
                    sw.Stop();
                    stoptime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");

                    Stoperror_time = DateTime.Now.Ticks;
                    if (!HIVE.HIVEInstance.NotUploadErrorMessageList.Exists(t => t == frm_Main.formError.ErrorUnit1.ErrorMessage) && frm_Main.formError.ErrorUnit1.ErrorCode != "" && frm_Main.formError.ErrorUnit1.ErrorType != "")
                    {
                           //if (MachineDataDefine.machineState.b_UseHive && !MachineDataDefine.machineState.b_UseTestRun /*&& MachineDataDefine.ChkStatus.UpLoadError == 1*/)
                            //{
                            //    HIVE.HIVEInstance.HiveSendERRORDATA(frm_Main.formError.ErrorUnit1.ErrorCode, frm_Main.formError.ErrorUnit1.ErrorType, frm_Main.formError.ErrorUnit1.ErrorMessage, starttime, stoptime, Starterror_time, Stoperror_time);
                            //}
                            HIVE.HIVEInstance.HiveSend(frm_Main.formError.ErrorUnit1.ErrorCode, frm_Main.formError.ErrorUnit1.ErrorType, frm_Main.formError.ErrorUnit1.ErrorMessage, starttime, stoptime, Starterror_time, Stoperror_time);
                       
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }

        private void labUser_Click(object sender, EventArgs e)
        {
            if (!MachineDataDefine.machineState.b_UseMesLogin)
            {
                dia_Login_New m_LoginDlg = new dia_Login_New(ref pMachine);
                //dia_Login_Remote m_LoginDlg = new dia_Login_Remote(ref pMachine);
                //------------
                if (m_LoginDlg.ShowDialog() == DialogResult.OK)
                {
                    if (pMachine.m_LoginUser == Sys_Define.enPasswordType.UnLogin || MachineDataDefine.m_LoginUserName.Trim().Equals(""))
                    {
                        MsgBoxHelper.DxMsgShowErr("登录失败！");
                        return;
                    }
                    else if ((int)pMachine.m_LoginUser <= 1)
                    {
                        MsgBoxHelper.DxMsgShowErr("权限不够，请登录Level2或Level3用户！");
                        return;
                    }
                    else
                    {
                        labUser.Text = MachineDataDefine.m_LoginUserName.Trim();
                    }
                }
        }
            else
            {
                dia_Login_Remote m_LoginDlg = new dia_Login_Remote(ref pMachine);
                //------------
                if (m_LoginDlg.ShowDialog() == DialogResult.OK)
                {
                    if (pMachine.m_LoginUser == Sys_Define.enPasswordType.UnLogin || MachineDataDefine.m_LoginUserName.Trim().Equals(""))
                    {
                        MsgBoxHelper.DxMsgShowErr("登录失败！");
                        return;
                    }
                    else if ((int)pMachine.m_LoginUser <= 1)
                    {
                        MsgBoxHelper.DxMsgShowErr("权限不够，请登录Level2或Level3用户！");
                        return;
                    }
                    else
                    {
                        labUser.Text = MachineDataDefine.m_LoginUserName.Trim();
                    }
                }
            }
            
            
        }
    }
}
