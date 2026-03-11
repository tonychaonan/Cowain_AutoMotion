using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Cowain_Machine.Flow;
using Cowain_AutoMotion.Flow.Hive;
using Cowain_AutoMotion;
using System.Threading;
using Chart;
using Cowain_Machine;
using Cowain_AutoMotion.Flow;

namespace Cowain_Form.FormView
{
    public partial class frm_ModeSelect : DevExpress.XtraEditors.XtraForm
    {
        public frm_ModeSelect(ref clsMachine pM)
        {
            InitializeComponent();

            pMachine = pM;
        }

        #region 自定义变量
        public clsMachine pMachine;
        public enum enWorkMode
        {
            Running = 0,
            Idle,
            Engineering,
            Planned_Downtime,
            Manually_Downtime,
            original
        }

        private enWorkMode m_CurMode = enWorkMode.original;
        /// <summary>
        /// 生产模式
        /// </summary>
        public enWorkMode CurMode
        {
            get
            {
                return m_CurMode;
            }

            set
            {
                m_CurMode = value;
            }
        }

        public enum enPlanMode
        {
            日常点检 = 0,
            更换AB胶水,
            更换HM胶水,
            更换针头,
            更换胶阀,
            压力测试,
            镭射标定,
            设备耗材更换,
            MaterialReplacement,
            周点检,
            胶水称重,
            LAD,
            其它,
            original
        }

        private enPlanMode m_CurStopMode = enPlanMode.original;
        /// <summary>
        /// 计划停机模式
        /// </summary>
        public enPlanMode CurStopMode
        {
            get
            {
                return m_CurStopMode;
            }

            set
            {
                m_CurStopMode = value;
            }
        }

        private int m_GantryType = -1;
        /// <summary>
        /// 龙门类型0：前  1：后
        /// </summary>
        public int GantryType
        {
            get
            {
                return m_GantryType;
            }

            set
            {
                m_GantryType = value;
            }
        }
        string starttime = string.Empty;
        string stoptime = string.Empty;

        /// <summary>
        /// 查询等待提示窗
        /// </summary>
        private HandleWait m_HandleWait = new HandleWait();
        #endregion

        #region 自定义方法
        /// <summary>
        /// 发送HIVE（计划停机）
        /// </summary>
        /// <param name="errorcode">错误编码</param>
        /// <param name="messgae">错误信息</param>
        /// <param name="errtype">错误类型</param>
        public void SendHive(string errorcode, string messgae, string errtype,string badge = "")
        {
            bool send = frm_Main.formData.ChartTime1.MalPlan();

            if (MachineDataDefine.machineState.b_UseHive && !HIVEDataDefine.Hive_machineState.IsSendErrorDown && send)
            {
              //  GantryParm.HIVEStateTime[0] = DateTime.Now.ToString("MM:dd:HH:mm:ss");

                HIVE.HIVEInstance.HiveSendMACHINESTATE(4, errorcode, messgae, errtype, false, badge);

                starttime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");
                Random randomaa = new Random();
                int temp_time = 1000;
                temp_time = randomaa.Next(1000, 3000);
                Thread.Sleep(temp_time);

                stoptime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");

                HIVE.HIVEInstance.HiveSendERRORDATA(errorcode, errtype, messgae, starttime, stoptime,false);
            }
            if (MachineDataDefine.machineState.b_UseRemoteQualification)
            {
                //  GantryParm.HIVEStateTime[0] = DateTime.Now.ToString("MM:dd:HH:mm:ss");

                HIVE.HIVEInstance.HiveSendMACHINESTATE(4, errorcode, messgae, errtype, true, badge);

                starttime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");
                Random randomaa = new Random();
                int temp_time = 1000;
                temp_time = randomaa.Next(1000, 3000);
                Thread.Sleep(temp_time);

                stoptime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");

                HIVE.HIVEInstance.HiveSendERRORDATA(errorcode, errtype, messgae, starttime, stoptime, true);
            }
        }
        #endregion

        private void labPlanned_Click(object sender, EventArgs e)
        {
            if (frm_Main.formData.ChartTime1.RunStatus == ChartTime.MachineStatus.error_down)
            {
                MsgBoxHelper.DxMsgShowErr("无法从DownTime手动切换到Plan DownTime");
                return;
            }
            if (frm_Main.formData.ChartTime1.RunStatus == ChartTime.MachineStatus.running &&
                (int)pMachine.m_LoginUser <= 1)
            {
                MsgBoxHelper.DxMsgShowErr("无法从Running手动切换到Plan DownTime");
                return;
            }
            if (frm_Main.formData.ChartTime1.RunStatus == ChartTime.MachineStatus.idle &&
                (int)pMachine.m_LoginUser <= 1)
            {
                MsgBoxHelper.DxMsgShowErr("无法从IDLE手动切换到Plan DownTime");
                return;
            }
            if (frm_Main.formData.ChartTime1.RunStatus == ChartTime.MachineStatus.engineering &&
                (int)pMachine.m_LoginUser <= 1)
            {
                MsgBoxHelper.DxMsgShowErr("无法从Engineering手动切换到Plan DownTime");
                return;
            }
            frm_Maintenance fm = new frm_Maintenance(ref pMachine);
            if(fm.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    m_HandleWait.ShowWait();
                    #region 主要逻辑
                    if (fm.CurMode == frm_Maintenance.enPlanMode.日常点检)
                    {
                        m_CurStopMode = enPlanMode.日常点检;
                        SendHive("PD-01", "Check", "Daily Maintenance");
                    }
                    else if (fm.CurMode == frm_Maintenance.enPlanMode.更换AB胶水)
                    {
                        m_CurStopMode = enPlanMode.更换AB胶水;
                        SendHive("PD-02", "AB Glue", "Glue Change");
                    }
                    else if(fm.CurMode == frm_Maintenance.enPlanMode.更换HM胶水)
                    {
                        m_CurStopMode = enPlanMode.更换HM胶水;
                        SendHive("PD-02", "HM Gule", "Glue Change");
                    }
                    else if (fm.CurMode == frm_Maintenance.enPlanMode.更换针头)
                    {
                        m_CurStopMode = enPlanMode.更换针头;
                        SendHive("PD-03", "", "Needle Change");
                    }
                    else if (fm.CurMode == frm_Maintenance.enPlanMode.更换胶阀)
                    {
                        m_CurStopMode = enPlanMode.更换胶阀;
                        SendHive("PD-04", "", "Valve Change");
                    }
                    else if (fm.CurMode == frm_Maintenance.enPlanMode.压力测试)
                    {
                        m_CurStopMode = enPlanMode.压力测试;
                        SendHive("PD-05", "", "Stress Test");
                    }
                    else if (fm.CurMode == frm_Maintenance.enPlanMode.镭射标定)
                    {
                        m_CurStopMode = enPlanMode.镭射标定;
                        SendHive("PD-06", "", "Laser Calibration");
                    }
                    else if (fm.CurMode == frm_Maintenance.enPlanMode.设备耗材更换)
                    {
                        m_CurStopMode = enPlanMode.设备耗材更换;
                        SendHive("PD-07", "", "Consumable Part Replacement");
                    }
                    else if (fm.CurMode == frm_Maintenance.enPlanMode.MaterialReplacement)
                    {
                        m_CurStopMode = enPlanMode.MaterialReplacement;
                        SendHive("PD-08", "", "Material Replacement");
                    }
                    else if (fm.CurMode == frm_Maintenance.enPlanMode.周点检)
                    {
                        m_CurStopMode = enPlanMode.周点检;
                        SendHive("PD-09", "", "Weekly Maintenance");
                    }
                    else if (fm.CurMode == frm_Maintenance.enPlanMode.胶水称重)
                    {
                        m_CurStopMode = enPlanMode.胶水称重;
                        SendHive("PD-09", "", "Weekly Maintenance");
                    }
                    else if (fm.CurMode == frm_Maintenance.enPlanMode.其它)
                    {
                        m_CurStopMode = enPlanMode.其它;
                        SendHive("PD-101", "", "Others");
                    }
                    else if (fm.CurMode == frm_Maintenance.enPlanMode.LAD)
                    {
                        m_CurStopMode = enPlanMode.LAD;
                        SendHive("PD-101", "", "Others");
                    }
                    else
                    {
                        MsgBoxHelper.DxMsgShowErr("请选择计划停机类型！");
                        return;
                    }
                    m_CurMode = enWorkMode.Planned_Downtime;
                    this.DialogResult = DialogResult.OK;
                    MachineDataDefine.hive_mac = Commons.GetMacAdd("HIVE");
                    if (MachineDataDefine.hive_mac == "00:00:00:00:00:00" || string.IsNullOrWhiteSpace(MachineDataDefine.hive_mac))
                    {
                        MachineDataDefine.NGmac = true;
                    }
                    #endregion

                }
                catch (Exception)
                {
                    
                }
                finally
                {
                    m_HandleWait.CloseWait();
                }
            }
        }

        private void labRunning_Click(object sender, EventArgs e)
        {
            if (frm_Main.formData.ChartTime1.RunStatus == ChartTime.MachineStatus.running)
            {
                //已经在运行中
                MsgBoxHelper.DxMsgShowErr("当前HIVE状态已经在Running中");
                return;
            }

            if (frm_Main.formData.ChartTime1.RunStatus == ChartTime.MachineStatus.idle &&
                pMachine.m_LoginUser != MotionBase.Sys_Define.enPasswordType.Maker)
            {
                MsgBoxHelper.DxMsgShowErr("无法从IDLE手动切换到Running");
                return;
            }

            if (frm_Main.formData.ChartTime1.RunStatus == ChartTime.MachineStatus.error_down &&
                (int)pMachine.m_LoginUser < 1)
            {
                MsgBoxHelper.DxMsgShowErr("无法从DownTime手动切换到Running");
                return;
            }

            if (frm_Main.formData.ChartTime1.RunStatus == ChartTime.MachineStatus.planned_downtime &&
                (int)pMachine.m_LoginUser < 1)
            {
                MsgBoxHelper.DxMsgShowErr("无法从Plan DownTime手动切换到Running");
                return;
            }

            if (frm_Main.formData.ChartTime1.RunStatus == ChartTime.MachineStatus.engineering)
            {
                MsgBoxHelper.DxMsgShowErr("无法从Engineering手动切换到Running");
                return;
            }

            if (MsgBoxHelper.DxMsgShowQues("确定要将HIVE切换到Running吗？") == DialogResult.Yes)
            {
                frm_PlanDoubleConfirm doubleConfm = new frm_PlanDoubleConfirm(ref pMachine);
                doubleConfm.NeedSN = false;
                doubleConfm.PlanType = "";
                if (doubleConfm.ShowDialog() == DialogResult.OK)
                {
                    m_CurMode = enWorkMode.Running;
                    this.DialogResult = DialogResult.OK;
                    MachineDataDefine.b_UseLAD = false;
                    MachineDataDefine.hive_mac = Commons.GetMacAdd("HIVE");
                    LogAuto.Notify("获取Mac地址！" + MachineDataDefine.hive_mac, (int)MachineStation.主监控, MotionLogLevel.Info);
                    if (MachineDataDefine.hive_mac == "00:00:00:00:00:00" || string.IsNullOrWhiteSpace(MachineDataDefine.hive_mac))
                    {
                        LogAuto.Notify("获取Mac地址:" + "00:00:00:00:00:00" + "-" + "操作卡号:" + MachineDataDefine.m_CardID + "_" + "历史状态:" + frm_Main.formData.ChartTime1.RunStatus + "_" + "当前状态:" + m_CurMode, (int)MachineStation.主监控, MotionLogLevel.Info);
                        MachineDataDefine.NGmac = true;
                    }
                    else
                    {
                        LogAuto.Notify("获取Mac地址:" + MachineDataDefine.hive_mac + "-" + "操作卡号:" + MachineDataDefine.m_CardID + "_" + "历史状态:" + frm_Main.formData.ChartTime1.RunStatus + "_" + "当前状态:" + m_CurMode, (int)MachineStation.主监控, MotionLogLevel.Info);
                    }
                }
                //frm_Main.formData.ChartTime1.MalRun();
                //if (!MachineDataDefine.machineState.IsDisableHive && !HIVEDataDefine.Hive_machineState.IsSendErrorDown)
                //{
                //    HIVE.HIVEInstance.HiveSendMACHINESTATE(1, "", "", "");
                //}
                
            }
        }

        private void labEngineering_Click(object sender, EventArgs e)
        {
            //if (pMachine.m_LoginUser != MotionBase.Sys_Define.enPasswordType.Maker)
            //    return;

            if (frm_Main.formData.ChartTime1.RunStatus == ChartTime.MachineStatus.running &&
                pMachine.m_LoginUser != MotionBase.Sys_Define.enPasswordType.Maker)
            {
                MsgBoxHelper.DxMsgShowErr("无法从Running手动切换到Engineering");
                return;
            }
            if (frm_Main.formData.ChartTime1.RunStatus == ChartTime.MachineStatus.idle &&
                pMachine.m_LoginUser != MotionBase.Sys_Define.enPasswordType.Maker)
            {
                MsgBoxHelper.DxMsgShowErr("无法从IDLE手动切换到Engineering");
                return;
            }
            if (frm_Main.formData.ChartTime1.RunStatus == ChartTime.MachineStatus.error_down &&
                (int)pMachine.m_LoginUser <= 1)
            {
                MsgBoxHelper.DxMsgShowErr("无法从DownTime手动切换到Engineering");
                return;
            }
            if (frm_Main.formData.ChartTime1.RunStatus == ChartTime.MachineStatus.planned_downtime &&
                (int)pMachine.m_LoginUser <= 1)
            {
                MsgBoxHelper.DxMsgShowErr("无法从Plan DownTime手动切换到Engineering");
                return;
            }

            frm_PlanDoubleConfirm doubleConfm = new frm_PlanDoubleConfirm(ref pMachine);
            doubleConfm.NeedSN = false;
            doubleConfm.PlanType = "";
            if (doubleConfm.ShowDialog() == DialogResult.OK)
            {
                m_CurMode = enWorkMode.Engineering;
                this.DialogResult = DialogResult.OK;
                MachineDataDefine.hive_mac = Commons.GetMacAdd("HIVE");
                LogAuto.Notify("获取Mac地址！" + MachineDataDefine.hive_mac, (int)MachineStation.主监控, MotionLogLevel.Info);
                if (MachineDataDefine.hive_mac == "00:00:00:00:00:00" || string.IsNullOrWhiteSpace(MachineDataDefine.hive_mac))
                {
                    LogAuto.Notify("获取Mac地址:" + "00:00:00:00:00:00" + "-" + "操作卡号:" + MachineDataDefine.m_CardID + "_" + "历史状态:" + frm_Main.formData.ChartTime1.RunStatus + "_" + "当前状态:" + m_CurMode, (int)MachineStation.主监控, MotionLogLevel.Info);
                    MachineDataDefine.NGmac = true;
                }
                else
                {
                    LogAuto.Notify("获取Mac地址:" + MachineDataDefine.hive_mac + "-" + "操作卡号:" + MachineDataDefine.m_CardID + "_" + "历史状态:" + frm_Main.formData.ChartTime1.RunStatus + "_" + "当前状态:" + m_CurMode, (int)MachineStation.主监控, MotionLogLevel.Info);
                }
            }
                
        }

        private void labManually_Click(object sender, EventArgs e)
        {
            if (MsgBoxHelper.DxMsgShowQues("确定要将HIVE切换到DownTime吗？") != DialogResult.Yes)
            {
                return;
            }

            if (frm_Main.formData.ChartTime1.RunStatus == ChartTime.MachineStatus.running &&
                (int)pMachine.m_LoginUser <= 1)
            {
                MsgBoxHelper.DxMsgShowErr("无法从Running手动切换到DownTime");
                return;
            }

            if (frm_Main.formData.ChartTime1.RunStatus == ChartTime.MachineStatus.idle &&
                (int)pMachine.m_LoginUser <= 1)
            {
                MsgBoxHelper.DxMsgShowErr("无法从IDLE手动切换到DownTime");
                return;
            }

            if (frm_Main.formData.ChartTime1.RunStatus == ChartTime.MachineStatus.engineering &&
                (int)pMachine.m_LoginUser <= 1)
            {
                MsgBoxHelper.DxMsgShowErr("无法从Engineering手动切换到DownTime");
                return;
            }

            if (frm_Main.formData.ChartTime1.RunStatus == ChartTime.MachineStatus.planned_downtime)
            {
                MsgBoxHelper.DxMsgShowErr("无法从Plan DownTime手动切换到DownTime");
                return;
            }
            frm_PlanDoubleConfirm doubleConfm = new frm_PlanDoubleConfirm(ref pMachine);
            doubleConfm.NeedSN = false;
            doubleConfm.PlanType = "";
            if (doubleConfm.ShowDialog() == DialogResult.OK)
            {
                m_CurMode = enWorkMode.Manually_Downtime;
                this.DialogResult = DialogResult.OK;
                MachineDataDefine.hive_mac = Commons.GetMacAdd("HIVE");
                LogAuto.Notify("获取Mac地址！" + MachineDataDefine.hive_mac, (int)MachineStation.主监控, MotionLogLevel.Info);
                if (MachineDataDefine.hive_mac == "00:00:00:00:00:00" || string.IsNullOrWhiteSpace(MachineDataDefine.hive_mac))
                {
                    LogAuto.Notify("获取Mac地址:" + "00:00:00:00:00:00" + "-" + "操作卡号:" + MachineDataDefine.m_CardID + "_" + "历史状态:" + frm_Main.formData.ChartTime1.RunStatus + "_" + "当前状态:" + m_CurMode, (int)MachineStation.主监控, MotionLogLevel.Info);
                    MachineDataDefine.NGmac = true;
                }
                else
                {
                    LogAuto.Notify("获取Mac地址:" + MachineDataDefine.hive_mac + "-" + "操作卡号:" + MachineDataDefine.m_CardID + "_" + "历史状态:" + frm_Main.formData.ChartTime1.RunStatus + "_" + "当前状态:" + m_CurMode, (int)MachineStation.主监控, MotionLogLevel.Info);
                }
                //  HIVE.HIVEInstance.HiveSendMACHINESTATE_Error(5, "O09OOOO-99-99", "Manually press the stop button", "Manually Downtime", false, "");
                HIVE.HIVEInstance.HiveSendERRORDATA("O09OOOO-99-99", "Manually press the stop button", "Manually Downtime", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800"), DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800"), 0, 0, false);
            }
        }

        private void frm_ModeSelect_Load(object sender, EventArgs e)
        {
            m_HandleWait.Init(this);

            if(pMachine.m_LoginUser == MotionBase.Sys_Define.enPasswordType.Maker)
           // if (pMachine.m_LoginUser == MotionBase.Sys_Define.enPasswordType.Eng)
            {
                labEngineering.Enabled = true;
                labIdle.Enabled = true;
            }
            else
            {
                labEngineering.Enabled = false;
                labIdle.Enabled = false;
            }
        }

        private void labIdle_Click(object sender, EventArgs e)
        {
            //if (pMachine.m_LoginUser != MotionBase.Sys_Define.enPasswordType.Maker)
            //    return;

            if (frm_Main.formData.ChartTime1.RunStatus == ChartTime.MachineStatus.planned_downtime)
            {
                MsgBoxHelper.DxMsgShowErr("无法从Plan DownTime手动切换到IDLE");
                return;
            }
            if (frm_Main.formData.ChartTime1.RunStatus == ChartTime.MachineStatus.error_down)
            {
                MsgBoxHelper.DxMsgShowErr("无法从DownTime手动切换到IDLE");
                return;
            }

            if (frm_Main.formData.ChartTime1.RunStatus == ChartTime.MachineStatus.running &&
                pMachine.m_LoginUser != MotionBase.Sys_Define.enPasswordType.Maker)
            {
                MsgBoxHelper.DxMsgShowErr("无法从Running手动切换到IDLE");
                return;
            }

            if (frm_Main.formData.ChartTime1.RunStatus == ChartTime.MachineStatus.engineering &&
                pMachine.m_LoginUser != MotionBase.Sys_Define.enPasswordType.Maker)
            {
                MsgBoxHelper.DxMsgShowErr("无法从Engineering手动切换到IDLE");
                return;
            }
            frm_PlanDoubleConfirm doubleConfm = new frm_PlanDoubleConfirm(ref pMachine);
            doubleConfm.NeedSN = false;
            doubleConfm.PlanType = "";
            if (doubleConfm.ShowDialog() == DialogResult.OK)
            {
                m_CurMode = enWorkMode.Idle;
                this.DialogResult = DialogResult.OK;
                MachineDataDefine.hive_mac = Commons.GetMacAdd("HIVE");
                LogAuto.Notify("获取Mac地址！" + MachineDataDefine.hive_mac, (int)MachineStation.主监控, MotionLogLevel.Info);
                if (MachineDataDefine.hive_mac == "00:00:00:00:00:00" || string.IsNullOrWhiteSpace(MachineDataDefine.hive_mac))
                {
                    LogAuto.Notify("获取Mac地址:" + "00:00:00:00:00:00" + "-" + "操作卡号:" + MachineDataDefine.m_CardID + "_" + "历史状态:" + frm_Main.formData.ChartTime1.RunStatus + "_" + "当前状态:" + m_CurMode, (int)MachineStation.主监控, MotionLogLevel.Info);
                    MachineDataDefine.NGmac = true;
                }
                else
                {
                    LogAuto.Notify("获取Mac地址:" + MachineDataDefine.hive_mac + "-" + "操作卡号:" + MachineDataDefine.m_CardID + "_" + "历史状态:" + frm_Main.formData.ChartTime1.RunStatus + "_" + "当前状态:" + m_CurMode, (int)MachineStation.主监控, MotionLogLevel.Info);
                }
            }
        }
    }
}