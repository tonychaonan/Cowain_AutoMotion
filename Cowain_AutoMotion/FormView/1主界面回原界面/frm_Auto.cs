using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Cowain_Machine.Flow;
using MotionBase;
using System.Threading;
using System.Data.OleDb;
using ToolTotal;
using Chart;
using Cowain_AutoMotion.Flow;
using Cowain;
using Cowain_AutoMotion.FormView;
using static Cowain_Machine.Flow.MErrorDefine;
using System.Net.NetworkInformation;
using ToolTotal_1;
using HslCommunication.ModBus;
using OmronFinsUI;
using Cowain_AutoMotion.Flow.Hive;
using Cowain_AutoMotion;
using Cowain_Machine;

namespace Cowain_Form.FormView
{
    public partial class frm_Auto : Form
    {
        int iii = 0;
        public static frm_Auto m_frm_Auto;
        MessageQueue messageQueue;
        Thread th;
        MErrorDefine.MErrorCode alarmCode;
        string armShowMessage = string.Empty;
        clsMachine pMachine;
        public frm_Auto()
        {
            InitializeComponent();
        }
        List<RichTextBox> rtb = new List<RichTextBox>();
        public frm_Auto(ref clsMachine pM)
        {
            InitializeComponent();
            pMachine = pM;
            m_frm_Auto = this;

            rtb.Add(this.richTextBox1);
            rtb.Add(this.richTextBox8);

            messageQueue = new MessageQueue(rtb);

            LogAuto.MessageEvent += Util_MessageEvent;

            th = new Thread(Recont);
            th.Priority = ThreadPriority.Lowest;
            th.IsBackground = true;
            th.Start();

            bt_mesStatus.Enabled = false;

            Task.Run(new Action(() =>
            {
                if (PingMESIP(MESDataDefine.MESLXData.StrMesIP, 3000))
                {
                    bt_mesStatus.Text = JudgeLanguage.JudgeLag("On Line");
                    bt_mesStatus.BackColor = Color.Lime;
                    bt_mesStatus.Enabled = false;
                }
                else
                {
                    bt_mesStatus.Text = JudgeLanguage.JudgeLag("Off Line");
                    bt_mesStatus.BackColor = Color.Red;
                    bt_mesStatus.Enabled = true;
                }

                // if (MachineDataDefine.machineState.b_UseHive && PingMESIP(MESDataDefine.MESLXData.StrMesIP, 3000))
                if (PingMESIP(MESDataDefine.MESLXData.StrHIVEIP, 3000))
                {
                    button_HIVE.Text = JudgeLanguage.JudgeLag("On Line");
                    button_HIVE.BackColor = Color.Lime;
                    button_HIVE.Enabled = false;
                }
                else
                {
                    button_HIVE.Text = JudgeLanguage.JudgeLag("Off Line");
                    button_HIVE.BackColor = Color.Red;
                    button_HIVE.Enabled = true;
                }

                // if (MachineDataDefine.machineState.b_UsePDCA && PingMESIP(MESDataDefine.MESLXData.StrMiniIP, 3000))
                if (PingMESIP(MESDataDefine.MESLXData.StrMiniIP, 3000))
                {
                    btnPDCA.Text = JudgeLanguage.JudgeLag("On Line");
                    btnPDCA.BackColor = Color.Lime;
                    btnPDCA.Enabled = false;
                }
                else
                {
                    btnPDCA.Text = JudgeLanguage.JudgeLag("Off Line");
                    btnPDCA.BackColor = Color.Red;
                    btnPDCA.Enabled = true;
                }
                if (HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).str_ClientConnectionOK == "OK")
                {

                    btnCCD1Line.Text = JudgeLanguage.JudgeLag("On Line");
                    btnCCD1Line.BackColor = Color.Lime;
                   // btnCCD1Line.Enabled = false;
                }
                else
                {
                    btnCCD1Line.Text = JudgeLanguage.JudgeLag("Off Line");
                    btnCCD1Line.BackColor = Color.Red;
                    btnCCD1Line.Enabled = true;
                }
               // if (HardWareControl.getRS232Control(EnumParam_ConnectionName.扫码枪).COMPort_Connect ())
               if(Cowain_AutoMotion.RS232.connect232)
                {
                    btnScan.Text = JudgeLanguage.JudgeLag("On Line");
                    btnScan.BackColor = Color.Lime;
                   // btnScan.Enabled = false;
                }
                else
                {
                    btnScan.Text = JudgeLanguage.JudgeLag("Off Line");
                    btnScan.BackColor = Color.Red;
                   // btnScan.Enabled = true;
                }
                //if (HardWareControl.getSocketControl(EnumParam_ConnectionName.串机).strConnectionOK == "OK")
                //{

                //    btnOtherMachine.Text = JudgeLanguage.JudgeLag("On Line");
                //    btnOtherMachine.BackColor = Color.Lime;
                //    btnOtherMachine.Enabled = false;
                //}
                //else
                //{
                //    btnOtherMachine.Text = JudgeLanguage.JudgeLag("Off Line");
                //    btnOtherMachine.BackColor = Color.Red;
                //    btnOtherMachine.Enabled = true;
                //}
            }));
        }

        /// <summary>
        /// 调用LOG信息入队方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">log的信息，索引，等级</param>
        private void Util_MessageEvent(object sender, MessageEventArgs e)
        {
            messageQueue.ShowMessage(e);
        }



        public Error pError = null;
        private bool m_bPause = false;
        private double m_ManulSpeed = 30;

        /// <summary>
        /// 界面刷新线程
        /// </summary>
        Thread ReFlash_Thread;
        private void frm_Auto_VisibleChanged(object sender, EventArgs e)
        {
            //timer_Reflash.Enabled = this.Visible;
            //timer_Step.Enabled = this.Visible;
            //---------------------------------
            bool bShow = (pMachine.m_LoginUser == MotionBase.Sys_Define.enPasswordType.Maker ||
                         pMachine.m_LoginUser == MotionBase.Sys_Define.enPasswordType.MacEng ||
                         pMachine.m_LoginUser == MotionBase.Sys_Define.enPasswordType.ItEng) ? true : false;
        }
        string str = "";
        int num = 0;
        /// <summary>
        /// 刷新点胶状态 界面按钮状态  马达点位更新状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Reflash_Tick(object sender, EventArgs e)
        {
            if (pMachine == null)
                return;
            #region 产量,CT更新
            tx_ZStopDisCardCounts.Text = frm_Main.formData.Chartcapacity1.OutTotal.ToString();
            if (frm_Main.formData.CTUnit1.CTEventArgs_Left != null)
            {
                tx_CycleTime_Left.Text = frm_Main.formData.CTUnit1.CTEventArgs_Left.Ct;
            }
            else
            {
                tx_CycleTime_Left.Text = "0";
            }
            #endregion;
            #region  按钮状态
            bool bisPausing = pMachine.GetisPausing();
            btn_Stop.Visible = bisPausing;

            if (MachineDataDefine.IsAutoing)
            {
                btn_CycleStop.Visible = true;
                btn_Pause.Visible = true;
            }
            else
            {
                btn_CycleStop.Visible = false;
                btn_Pause.Visible = false;
                m_bPause = false;
                btn_CycleStop.Enabled = true;
                btn_Pause.Text = "       Pause";
                btn_Pause.BackColor = Color.White;
            }

            #endregion
        }

        //跟新UI频率计数
        int updateMes = 0;
        int updateDis1 = 0;
        int updateDis2 = 0;
        int updateDis3 = 0;
        int updateStstus = 0;
        int updateOther = 0;
        Point panelGlunStartPoint;
        //记录上次状态，如果当前值与上次一致，无需刷新界面
        MesStatus lastmesupload1 = MesStatus.Init;
        MesStatus lastmesupload2 = MesStatus.Init;
        /// <summary>
        /// 刷新mes状态  更新胶水
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Step_Tick(object sender, EventArgs e)
        {
            try
            {
                if (pMachine == null)
                    return;
                updateMes++;
                #region  //刷新mes状态  500ms
                try
                {
                    if (updateMes > 5)
                    {
                        updateMes = 0;
                        //if (MESDataDefine.MesUploadStatusS[0] != lastmesupload1)
                        //{
                        //    if (MESDataDefine.MesUploadStatusS[0] == MesStatus.CheckSN)
                        //    {
                        //        Bt_mesupload1.Text = "CheckSN";
                        //        Bt_mesupload1.BackColor = Color.Yellow;
                        //    }
                        //    else if (MESDataDefine.MesUploadStatusS[0] == MesStatus.UpLoadMesOK)
                        //    {
                        //        Bt_mesupload1.Text = "PASS";
                        //        Bt_mesupload1.BackColor = Color.Lime;
                        //    }
                        //    else if (MESDataDefine.MesUploadStatusS[0] == MesStatus.UpLoadMesNG)
                        //    {
                        //        Bt_mesupload1.Text = "FAIL";
                        //        Bt_mesupload1.BackColor = Color.Red;
                        //    }

                        //    lastmesupload1 = MESDataDefine.MesUploadStatusS[0];
                        //}

                        ////-----------------------------------------------------
                        //if(txtLeftUC.Text != MESDataDefine.StrCarryBarCodeS[0])
                        //{
                        //    txtLeftUC.Text = MESDataDefine.StrCarryBarCodeS[0];
                        //}
                        //if (txtLeftSN.Text != MESDataDefine.StrSNS[0])
                        //{
                        //    txtLeftSN.Text = MESDataDefine.StrSNS[0];
                        //}             
                    }
                }
                catch (Exception)
                {


                }
                #endregion

                updateDis1++;

                updateDis2++;


                updateDis3++;

                updateOther++;
                #region //产量,CT更新    1500ms
                try
                {
                    if (updateOther > 15)
                    {
                        updateOther = 0;
                        #region 
                        tx_ZStopDisCardCounts.Text = frm_Main.formData.Chartcapacity1.OutTotal.ToString();
                        if (frm_Main.formData.CTUnit1.CTEventArgs_Left != null)
                        {
                            //  tx_CycleTime_Left.Text = frm_Main.formData.CTUnit1.CTEventArgs_Left.Ct;
                        }
                        else
                        {
                            //   tx_CycleTime_Left.Text = "0";
                        }
                        #endregion
                        //****************************************
                        if (pMachine.m_LoginUser == MotionBase.Sys_Define.enPasswordType.Maker)
                        {
                            textBox_UserID.Text = "Maker";
                        }
                        #region 显示当前流道


                        //MES上传开, 相机拍照开,扫码开,相机存图调试模式 开
                        if (MachineDataDefine.machineState.b_UseMes && MachineDataDefine.machineState.b_UseCCD && MachineDataDefine.machineState.b_UseScaner && !MachineDataDefine.machineState.b_UseTestRun)
                        {
                            txt_macchine.Text = JudgeLanguage.JudgeLag("生产模式");
                            txt_macchine.BackColor = Color.Lime;
                        }
                        else
                        {
                            txt_macchine.Text = JudgeLanguage.JudgeLag("调试模式");
                            txt_macchine.BackColor = Color.Red;
                        }
                        //*****************刷新PDCA状态*******************

                        #endregion
                    }


                }
                catch (Exception)
                {

                }
                #endregion

                updateStstus++;
                #region 更新设备状态和硬件状态
                if (updateStstus > 30)
                {
                    updateStstus = 0;
                    //****************************************
                    //刷新界面的流程
                    RefreshShieldStatus(btSignal扫码, MachineDataDefine.machineState.b_UseScaner);
                    RefreshShieldStatus(btSignalMES, MachineDataDefine.machineState.b_UseMes);
                    RefreshShieldStatus(btSignalCCD, MachineDataDefine.machineState.b_UseCCD);
                    RefreshShieldStatus(btSignalHIVE, MachineDataDefine.machineState.b_UseHive);
                    RefreshShieldStatus(btSignalPDCA, MachineDataDefine.machineState.b_UsePDCA);
                    
                  //  RefreshShieldStatus(btnRobot, MachineDataDefine.machineState.b_Usehummer);
                }
                #endregion
            }
            catch (Exception ex)
            { }
        }

        private void DoReflash()
        {
            while (true)
            {
                try
                {
                    if (this.Visible)
                    {
                        Refreash();
                    }
                    Thread.Sleep(500);
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        private void Refreash()
        {
            if (this == null)
            {
                return;
            }

            if (this.InvokeRequired)
            {
                try
                {
                    this.Invoke(new Action(Refreash));
                    return;
                }
                catch (Exception e)
                {
                    return;
                }
            }
            //Thread.Sleep(3000);
            timer_Reflash_Tick(null, null);
            timer_Step_Tick(null, null);
        }


        //刷新状态
        private void RefreshShieldStatus(Button bt, bool _status, bool isChange = true)
        {
            if (isChange)
                bt.BackColor = _status ? Color.Lime : Color.Red;
        }

        Ping pingSender = new Ping();
        PingReply pingreply;
        /// <summary>
        /// ping一下mes网络，检查是否能连接的
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public bool PingMESIP(string IP, int timeout)
        {


            bool MES_state = false;

            try
            {
                pingreply = pingSender.Send(IP, timeout);

                if (pingreply.Status.ToString().Trim() == "Success")
                {
                    MES_state = true;
                }
                else
                {
                    MES_state = false;
                }
            }
            catch
            {
                MES_state = false;
            }
            return MES_state;

        }

        //停止运行
        public void Stop()
        {
            pMachine.StopAuto();
        }

        //开启程序
        private void btn_Auto_Click(object sender, EventArgs e)
        {
            if (MachineDataDefine.machineState.b_UseHive)
            {
                if (new Ping().Send("10.0.0.2", 3000).Status.ToString().Trim() != "Success")
                {
                    HIVE.HIVEInstance.HIVE_Reveice_Status = false;
                    return;
                }
            }
            if (HIVEDataDefine.Hive_machineState.IsSendErrorDown && MachineDataDefine.machineState.b_UseHive)
            {
                MessageBox.Show(JudgeLanguage.JudgeLag("发送报警状态时不允许启动！"), JudgeLanguage.JudgeLag("提示"), MessageBoxButtons.OK,
                   MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                return;
            }
            if (MachineDataDefine.IsFormOpen)
            {

                MessageBox.Show(JudgeLanguage.JudgeLag("请检查设备安全门是否已全部关闭！"), JudgeLanguage.JudgeLag("提示"), MessageBoxButtons.OK,
                   MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                LogAuto.Notify("设备安全门存在未关闭状况！", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                return;
            }
            LogAuto.Notify("窗体启动button按下", (int)MachineStation.主监控, MotionLogLevel.Info);
            pMachine.StateChoose();
            //pMachine.StartAuto();
        }

        private void btn_Pause_Click(object sender, EventArgs e)
        {
            if (m_bPause == false)
            {
                pMachine.EnablePause();
                m_bPause = true;
                btn_CycleStop.Enabled = false;
                btn_Pause.Text = "       恢復自動";
                btn_Pause.BackColor = Color.Gray;
            }
            else
            {
                pMachine.DisablePause();
                m_bPause = false;
                btn_CycleStop.Enabled = true;
                btn_Pause.Text = "       暫停";
                btn_Pause.BackColor = Color.White;
            }
        }


        private void btn_CycleStop_Click(object sender, EventArgs e)
        {
            MachineDataDefine.IsCycleStop = true;
        }

        //加载参数
        private void frm_Auto_Load(object sender, EventArgs e)
        {
            lbStationName.Text = MachineDataDefine.settingData.Station;
            if (MachineDataDefine.StationImage != null)
            {
                pictureBox1.Image = MachineDataDefine.StationImage;
            }
            string strDirectory = pMachine.GetWorkDataDirectory();
            string strFilePath = pMachine.GetWorkDataPath();
            string strShowName = strFilePath.Replace(strDirectory, "");
            strShowName = strShowName.Replace(".xml", "");
            ReFlash_Thread = new Thread(DoReflash);
            ReFlash_Thread.Priority = ThreadPriority.Lowest;
            ReFlash_Thread.IsBackground = true;
            ReFlash_Thread.Start();
        }
        //停止运行
        private void btn_Stop_Click(object sender, EventArgs e)
        {
            MachineDataDefine.Button_Stop = true;
            LogAuto.Notify("停止button按下", (int)MachineStation.主监控, MotionLogLevel.Alarm);
            pMachine.StopAuto();
        }
        private void button8_Click(object sender, EventArgs e)
        {
            string strInitialDirectory = pMachine.GetWorkDataDirectory();
            OpenFileDialog file = new OpenFileDialog();
            file.InitialDirectory = strInitialDirectory;
            file.ShowDialog();
            String selectRecipe = file.SafeFileName;


            String strShowRecipe = selectRecipe.Replace(".csv", "");
            if (strShowRecipe == "")
            {
                MessageBox.Show(this, JudgeLanguage.JudgeLag("未选取Excel档案 , 请重新确认 !!"));
                return;
            }


            DialogResult dlgResult = MessageBox.Show(JudgeLanguage.JudgeLag(" 确认载入   [" + selectRecipe + "]    档案 ? "), "Load Dlg", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dlgResult == DialogResult.Yes)
            {
                tx_OpIDFile.Text = file.FileName;
            }
        }
        bool b = true;
        /// <summary>
        ///相机自动重连  mes
        /// </summary>
        public void Recont()
        {
            while (true)
            {
                try
                {
                   
                    Thread.Sleep(1000);
                    #region 实时连接相机
                    //-------------间歇性排胶（相机重连）----------//自动重连相机
                    //if (!pMachine.m_DispenserAuto.m_pClient1.connectOk)
                    //    pMachine.m_DispenserAuto.m_pClient1.Open("127.0.0.1", 8001);

                    //if (m_enGantryType == enGantryType.DualGantry)
                    //{
                    //    if (!pMachine.m_DispenserAuto.m_pClient2.connectOk)
                    //        pMachine.m_DispenserAuto.m_pClient2.Open("127.0.0.1", 8002);
                    //}
                }
                catch
                {
                }

                #endregion
            }
        }
        /// <summary>
        /// 重新连接CCD1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCCD1Line_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dlgResult = MessageBox.Show(JudgeLanguage.JudgeLag("确定重新连接CCD1相机"), JudgeLanguage.JudgeLag("重连CCD"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (dlgResult == DialogResult.Yes)
                {
                    if (HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).str_ClientConnectionOK != "OK")
                    {
                        HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).Start();
                        btnCCD1Line.Text = JudgeLanguage.JudgeLag("On Line");
                        btnCCD1Line.BackColor = Color.Lime;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// PDCA重新连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPDCA_Click(object sender, EventArgs e)
        {
            DialogResult dlgResult = MessageBox.Show(JudgeLanguage.JudgeLag(" 测试PDCA网络通讯是否正常? "), JudgeLanguage.JudgeLag("Hint"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dlgResult != DialogResult.Yes)
            {
                return;
            }

            Task.Run(new Action(() =>
            {
                if (PingMESIP(MESDataDefine.MESLXData.StrMiniIP, 3000))
                {
                    btnPDCA.Text = JudgeLanguage.JudgeLag("On Line");
                    btnPDCA.BackColor = Color.Lime;
                    btnPDCA.Enabled = false;
                }
                else
                {
                    btnPDCA.Text = JudgeLanguage.JudgeLag("Off Line");
                    btnPDCA.BackColor = Color.Red;
                    btnPDCA.Enabled = true;
                }
            }));
        }
        /// <summary>
        /// 测试MES网络通讯是否正常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click_1(object sender, EventArgs e)
        {
            DialogResult dlgResult = MessageBox.Show(JudgeLanguage.JudgeLag(" 测试MES网络通讯是否正常? "), JudgeLanguage.JudgeLag("Hint"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dlgResult != DialogResult.Yes)
            {
                return;
            }

            Task.Run(new Action(() =>
            {
                if (PingMESIP(MESDataDefine.MESLXData.StrMesIP, 3000))
                {
                    bt_mesStatus.Text = JudgeLanguage.JudgeLag("On Line");
                    bt_mesStatus.BackColor = Color.Lime;
                    bt_mesStatus.Enabled = false;
                }
                else
                {
                    bt_mesStatus.Text = JudgeLanguage.JudgeLag("Off Line");
                    bt_mesStatus.BackColor = Color.Red;
                    bt_mesStatus.Enabled = true;
                }
            }));

        }
        private void timer_CheckStationAndSW_Tick(object sender, EventArgs e)
        {
        }
        private void button5_Click_1(object sender, EventArgs e)
        {

            //MachineDataDefine.machineState.WriteParams(MachineDataDefine.machineState);
            //MessageBox.Show("保存成功");
        }

        private void 清空显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                richTextBox1.Clear();
            }
            catch
            {

            }
        }

        private void button_HIVE_Click(object sender, EventArgs e)
        {
            DialogResult dlgResult = MessageBox.Show(JudgeLanguage.JudgeLag(" 测试HIVE网络通讯是否正常? "), JudgeLanguage.JudgeLag("Hint"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dlgResult != DialogResult.Yes)
            {
                return;
            }

            Task.Run(new Action(() =>
            {
                if (PingMESIP("10.0.0.2", 3000))
                {
                    button_HIVE.Text = JudgeLanguage.JudgeLag("On Line");
                    button_HIVE.BackColor = Color.Lime;
                    button_HIVE.Enabled = false;
                }
                else
                {
                    button_HIVE.Text = JudgeLanguage.JudgeLag("Off Line");
                    button_HIVE.BackColor = Color.Red;
                    button_HIVE.Enabled = true;
                }
            }));
        }

        private Bitmap ReadFileToImage(string path)
        {
            if (!File.Exists(path))
                return new Bitmap(1, 1);

            FileStream fs = File.OpenRead(path);
            int ilength = (int)fs.Length;
            Byte[] bt = new Byte[ilength];
            fs.Read(bt, 0, ilength);
            Image image = Image.FromStream(fs);
            fs.Close();
            return new Bitmap(image);
        }

        private void groupBox14_Enter(object sender, EventArgs e)
        {

        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dlgResult = MessageBox.Show(JudgeLanguage.JudgeLag("确定重新连接扫码枪"), JudgeLanguage.JudgeLag("重连扫码枪"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (dlgResult == DialogResult.Yes)
                {
                    if(HardWareControl.getRS232Control(EnumParam_ConnectionName.扫码枪).serialPort.IsOpen)
                    {
                        MessageBox.Show(JudgeLanguage.JudgeLag("已连接状态"));
                        return;
                    }
                        if (HardWareControl.getRS232Control(EnumParam_ConnectionName.扫码枪).REConnect("COM11"))
                        {
                            btnScan.Text = JudgeLanguage.JudgeLag("On Line");
                            btnScan.BackColor = Color.Lime;
                            btnScan.Enabled = false;
                            MessageBox.Show(JudgeLanguage.JudgeLag("重新连接扫码枪成功"));
                        }
                        else
                        {
                            btnScan.Text = JudgeLanguage.JudgeLag("Off Line");
                            btnScan.BackColor = Color.Red;
                            btnScan.Enabled = true;
                            MessageBox.Show(JudgeLanguage.JudgeLag("重新连接扫码枪失败"));
                        }
                    }
                   
               // }
            }
            catch (Exception ex)
            {
            }    
         }

        private void btnOtherMachine_Click(object sender, EventArgs e)
        {

        }
    }
}
