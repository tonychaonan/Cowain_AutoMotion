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
using Cognex.VisionPro;
using Cognex.VisionPro.PMAlign;
using Cognex.VisionPro.Exceptions;
using Cognex.VisionPro.Display;
using Cognex.VisionPro.ImageProcessing;
using Cognex.VisionPro.Caliper;
using Cognex.VisionPro.Dimensioning;
using MotionBase;


namespace Cowain_Form.FormView
{
    public partial class frm_ManulPastingHead : Form
    {
        public frm_ManulPastingHead(ref Machine refMachine)
        {
            pMachine = refMachine;
            InitializeComponent();
        }

        Machine pMachine = null;
        ImageList ImgList = new ImageList();
        int m_iStationID = 0;
        string m_strSelect = "";
        double m_dbManulSpeed = 30;
        bool m_bCCDLive = false, m_bCapture = false;
        //******************
        string[] strPastingStPos = { "待命位",  "吸嘴影像位", "吸嘴調教位", "吸嘴拋料位" };
        //******************
        DrvMotor[] pMotor = new DrvMotor[6];
        DrvValve[] pValve = new DrvValve[14];
        //******************
        int iTrayID=0, iArrayX=0, iArrayY=0;

        private void frm_ManulPastingHead_Shown(object sender, EventArgs e)
        {
            ImgList.Images.Add("UnSelect", Cowain_GelToPCB.Properties.Resources.SetOk_Disable);
            ImgList.Images.Add("Select", Cowain_GelToPCB.Properties.Resources.SetOk);
            listView_Pos.Items.Clear();
            listView_Pos.LargeImageList = ImgList;
            listView_Pos.SmallImageList = ImgList;
            //-------------------------------------
            comboBox_Pitch.SelectedIndex = 0;
            comboBox_Light.SelectedIndex = 0;
            comboBox_PickZStop.SelectedIndex = 0;
            Fun_DisplayPosList();
        }

        public void Fun_DisplayPosList()
        {
            m_strSelect = "";
            listView_Pos.Clear();
            int i, j;
            //**********顯示馬達List***************
            #region 顯示Pos_List
            for (i = 0; i < strPastingStPos.Length; i++)
                listView_Pos.Items.Add(strPastingStPos[i], "UnSelect");
            #endregion
            ShowMotorAndValve();
        }

        private void ShowMotorAndValve()
        {
            int i, j;
            //-------------------------------------
            for (i = 0; i < pMotor.Length; i++)
                pMotor[i] = null;
            for (i = 0; i < pValve.Length; i++)
                pValve[i] = null;
            //-----------------------

            //--------指定Motor & Valve------------
            pMotor[0] = pMachine.m_PastingSt.m_PastingHead.m_motX;
            pMotor[1] = pMachine.m_PastingSt.m_PastingHead.m_motZ;
            pMotor[2] = pMachine.m_PastingSt.m_PastingHead.m_motT;
            pMotor[3] = pMachine.m_ZStopStationAuto.m_ZStopStation.m_Stage.m_motY;
            //--------
            int iUpDownCount, iVacCount;
            for (i=0;i<4;i++)
            {
                iUpDownCount = (i );
                iVacCount = (i ) + 7;
                pValve[iUpDownCount] = pMachine.m_PastingSt.m_PastingHead.m_HeadUpDown[i];
                pValve[iVacCount] = pMachine.m_PastingSt.m_PastingHead.m_HeadVac[i];
            }

            //-----------------------
            #region 顯示電磁閥 & 馬達

            Label[] pValveLabel = { label_Valve1 , label_Valve2 , label_Valve3, label_Valve4, label_Valve5, label_Valve6 , label_Valve7,
                                    label_Valve8 , label_Valve9 , label_Valve10,label_Valve11,label_Valve12,label_Valve13, label_Valve14};
            Button[] pBtnOn = { btn_ValveOn1, btn_ValveOn2, btn_ValveOn3, btn_ValveOn4, btn_ValveOn5, btn_ValveOn6, btn_ValveOn7 ,
                                btn_ValveOn8, btn_ValveOn9, btn_ValveOn10,btn_ValveOn11,btn_ValveOn12,btn_ValveOn13,btn_ValveOn14};
            Button[] pBtnOff = { btn_ValveOff1, btn_ValveOff2, btn_ValveOff3, btn_ValveOff4, btn_ValveOff5, btn_ValveOff6, btn_ValveOff7 ,
                                 btn_ValveOff8, btn_ValveOff9, btn_ValveOff10,btn_ValveOff11,btn_ValveOff12,btn_ValveOff13,btn_ValveOff14};

            MotionBase.DrvValve.tyValve_Parameter ptyParameter = new MotionBase.DrvValve.tyValve_Parameter();
            bool bVisible = false;
            groupBox9.Visible = false;
            for (i = 0; i < pValve.Length; i++)
            {

                bVisible = (pValve[i] != null) ? true : false;
                pValveLabel[i].Visible = bVisible;
                pBtnOn[i].Visible = bVisible;
                pBtnOff[i].Visible = bVisible;
                //---------------------------
                if (pValve[i] != null)
                {
                    groupBox9.Visible = true;
                    pValve[i].GetParameter(ref ptyParameter);  //取得電磁閥參數
                    pValveLabel[i].Text = ptyParameter.strCName;
                }
            }
            //*******************************
            GroupBox[] pGroup = { groupBox_M1, groupBox_M2, groupBox_M3, groupBox_M4, groupBox_M5, groupBox_M6 };
            for (i = 0; i < pMotor.Length; i++)
            {
                bVisible = (pMotor[i] != null) ? true : false;
                pGroup[i].Visible = bVisible;
            }

            #endregion
            //-----------------------
        }

        private void frm_ManulPastingHead_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                timer_ReFlash.Enabled = true;
                comboBox_Pitch.SelectedIndex = 0;
                comboBox_PickZStop.SelectedIndex = 0;
                comboBox_Light.SelectedIndex = 0;
                Fun_DisplayPosList();
                m_bCapture = false;
                m_bCCDLive = true; //開啟CCD Live
                btn_CCDLive.BackColor = Color.White;
                //---------------------------------------------------------------
                bool bVisible = (pMachine.m_LoginUser == MotionBase.Sys_Define.enPasswordType.Maker || 
                                 pMachine.m_LoginUser == MotionBase.Sys_Define.enPasswordType.MacEng||
                                 pMachine.m_LoginUser == MotionBase.Sys_Define.enPasswordType.Eng);
                btn_Save.Visible = bVisible;
                //---------------------------------------------------------------
            }
            else
            {
                pMachine.m_CCDSystem.m_LightControlBox.SetIntensity(0, 0); //全部亮度切成0
                m_bCCDLive = m_bCapture = false;
                timer_ReFlash.Enabled = false;
            }

        }

        private void timer_ReFlash_Tick(object sender, EventArgs e)
        {
            if (pMachine == null)
                return;
            if (!MDataDefine.m_bUiTimerEnable)
                return;
            //---------------------
            int i = 0;
            //---------------------
            #region 馬達Position更新

            Label[] pLabel = { label_M1Pos, label_M2Pos, label_M3Pos, label_M4Pos, label_M5Pos, label_M6Pos };
            for (i = 0; i < pMotor.Length; i++)
            {
                if (pMotor[i] != null)
                {
                    double dbPos = pMotor[i].GetPosition();
                    pLabel[i].Text = dbPos.ToString("0.000");
                }
            }
            #endregion
            //---------------------
            #region 更新電磁閥狀態
            Button[] pBtnOn = { btn_ValveOn1, btn_ValveOn2, btn_ValveOn3, btn_ValveOn4, btn_ValveOn5, btn_ValveOn6, btn_ValveOn7 ,
                                btn_ValveOn8, btn_ValveOn9, btn_ValveOn10,btn_ValveOn11,btn_ValveOn12,btn_ValveOn13,btn_ValveOn14};
            Button[] pBtnOff = { btn_ValveOff1, btn_ValveOff2, btn_ValveOff3, btn_ValveOff4, btn_ValveOff5, btn_ValveOff6, btn_ValveOff7 ,
                                 btn_ValveOff8, btn_ValveOff9, btn_ValveOff10,btn_ValveOff11,btn_ValveOff12,btn_ValveOff13,btn_ValveOff14};

            MotionBase.DrvValve.tyValve_Parameter ptyParameter = new MotionBase.DrvValve.tyValve_Parameter();
            for (i = 0; i < pValve.Length; i++)
            {
                if (pValve[i] != null)
                {
                    if (pValve[i].GetisOpen())
                        pBtnOn[i].Image = Cowain_GelToPCB.Properties.Resources.SetOk;
                    else
                        pBtnOn[i].Image = Cowain_GelToPCB.Properties.Resources.SetCanel;
                    //----------------------
                    if (pValve[i].GetisClose())
                        pBtnOff[i].Image = Cowain_GelToPCB.Properties.Resources.SetOk;
                    else
                        pBtnOff[i].Image = Cowain_GelToPCB.Properties.Resources.SetCanel;
                }
                else
                {
                    pBtnOn[i].Visible = false;
                    pBtnOff[i].Visible = false;
                }

            }

            #endregion
            //---------------------

            #region Button Enable(動作時將Button Disable)
            bool bMototisIdle = true;
            for (i = 0; i < pMotor.Length; i++)
            {
                if (pMotor[i] != null)
                {
                    if (!pMotor[i].isIDLE())
                        bMototisIdle = false;
                }
            }
            //--------------------------------
            bool bMacIdle = pMachine.isIDLE();
            bool bPastingHeadStIdle = pMachine.m_PastingSt.isIDLE();
            bool bPastingHeadIdle = pMachine.m_PastingSt.m_PastingHead.isIDLE();

            bool bBtnEnable = (bMototisIdle && bMacIdle && bPastingHeadIdle && bPastingHeadIdle);

            foreach (Control control in this.Controls) //取得所有Control元件 , 進行判斷
            {
                if (control is Button)
                {
                    Button btn = control as Button;
                    btn.Enabled = bBtnEnable;
                }

                if (control is GroupBox)
                {
                    GroupBox grp = control as GroupBox;
                    if (grp == groupBox2)
                        grp.Enabled = bBtnEnable;
                    //---------------------------
                    foreach (Control gControl in control.Controls)
                    {
                        if (gControl is Button)
                        {
                            Button GroupBtn = gControl as Button;
                            if (GroupBtn == btn_Stop)
                                GroupBtn.Enabled = true;
                            else
                                GroupBtn.Enabled = bBtnEnable;

                        }
                    }
                }
            }

            #endregion
            //-----------
            Button[] pBtn = { btn_Move, btn_Save };
            //--------------------------------
            if (!pMachine.m_PastingSt.GetHomeCompleted())
            {
                for (i = 0; i < pBtn.Length; i++)
                    pBtn[i].Enabled = false;
            }
            //--------------------
            #region CCD Live

            if (m_bCCDLive)
            {
                if (m_bCapture == false)
                {
                    m_bCapture = true;
                    pMachine.m_CCDSystem.m_CCD2.OneShot();
                }
                else
                {
                    if (pMachine.m_CCDSystem.m_CCD2.GetOneShotReady())
                    {
                        CogImage8Grey image = pMachine.m_CCDSystem.m_CCD2.GetOnShotImage();
                        //-------------------
                        if (image == null)
                            return;
                        //-------------------
                        cogRecordDisplay1.Image = image; // image;
                        cogRecordDisplay1.StaticGraphics.Clear();
                        //-------------------
                        double dbImageWidth = Convert.ToDouble(image.Width);
                        double dbImageHeight = Convert.ToDouble(image.Height);
                        //-----------------
                        double dbCenterX = dbImageWidth / 2.0;
                        double dbCenterY = dbImageHeight / 2.0;
                        //-----------------
                        #region 畫中心十字線

                        CogCreateLineTool Line1 = new CogCreateLineTool();
                        CogCreateLineTool Line2 = new CogCreateLineTool();
                        Line1.InputImage = image;
                        Line2.InputImage = image;
                        Line1.Line.X = dbCenterX;
                        Line1.Line.Y = dbCenterY;
                        //--------
                        Line2.Line.X = dbCenterX;
                        Line2.Line.Y = dbCenterY;
                        Line2.Line.Rotation = (Math.PI / 180.0) * 90.0;
                        //--------------------
                        Line1.OutputColor = CogColorConstants.Green;
                        Line2.OutputColor = CogColorConstants.Green;
                        Line1.Run();
                        Line2.Run();
                        //--------------------
                        cogRecordDisplay1.StaticGraphics.Add(Line1.GetOutputLine(), "Result");
                        cogRecordDisplay1.StaticGraphics.Add(Line2.GetOutputLine(), "Result");
                        #endregion
                        //-----------------
                        m_bCapture = false;
                    }
                }
            }
            #endregion
        }

        private void listView_Pos_DoubleClick(object sender, EventArgs e)
        {
            int i, j;
            if (listView_Pos.SelectedItems.Count <= 0)
                return;
            //-----------------------------------------
            string SelectText = listView_Pos.SelectedItems[0].Text;
            m_strSelect = SelectText;
            for (i = 0; i < listView_Pos.Items.Count; i++)
            {
                //--------------------------
                if (listView_Pos.Items[i].Text == SelectText)
                {
                    listView_Pos.Items[i].ImageKey = "Select";
                    listView_Pos.Items[i].BackColor = Color.LightBlue;
                }
                else
                {
                    listView_Pos.Items[i].ImageKey = "UnSelect";
                    listView_Pos.Items[i].BackColor = Color.White;
                }

            }
        }

        private void btn_Home_Click(object sender, EventArgs e)
        {
            pMachine.m_PastingSt.DoHome();
        }

        private void btn_Stop_Click(object sender, EventArgs e)
        {
            pMachine.Stop();
        }



        private void btn_ManulValveOnOff_Click(object sender, EventArgs e)
        {
            int i;
            ////--------------------------------------
            Button[] pBtnOn = { btn_ValveOn1, btn_ValveOn2, btn_ValveOn3, btn_ValveOn4, btn_ValveOn5, btn_ValveOn6, btn_ValveOn7 ,
                                btn_ValveOn8, btn_ValveOn9, btn_ValveOn10,btn_ValveOn11,btn_ValveOn12,btn_ValveOn13,btn_ValveOn14 };
            Button[] pBtnOff = { btn_ValveOff1, btn_ValveOff2, btn_ValveOff3, btn_ValveOff4, btn_ValveOff5, btn_ValveOff6, btn_ValveOff7 ,
                                 btn_ValveOff8, btn_ValveOff9, btn_ValveOff10,btn_ValveOff11,btn_ValveOff12,btn_ValveOff13,btn_ValveOff14  };
            ////--------------------------------------
            int iSenderCount = 0;
            bool bisOn = false, bisOff = false;
            for (i = 0; i < pBtnOn.Length; i++)
            {
                if (sender == pBtnOn[i])
                {
                    bisOn = true;
                    iSenderCount = i;
                }
                else if (sender == pBtnOff[i])
                {
                    bisOff = true;
                    iSenderCount = i;
                }
            }

            if (bisOn)
                pValve[iSenderCount].Open(MotionBase.DrvValve.enActionMode.Mode_Test); //手動下Pass TimeOut檢查時間
            else if (bisOff)
                pValve[iSenderCount].Close(MotionBase.DrvValve.enActionMode.Mode_Test);
        }


        private void btn_ManulMove_Click(object sender, EventArgs e)
        {
            //------------------
            MotionBase.DrvMotor pSelectMotor = null;
            if (sender == btn_AddM1 || sender == btn_SubM1)
                pSelectMotor = pMotor[0];
            else if (sender == btn_AddM2 || sender == btn_SubM2)
                pSelectMotor = pMotor[1];
            else if (sender == btn_AddM3 || sender == btn_SubM3)
                pSelectMotor = pMotor[2];
            else if (sender == btn_AddM4 || sender == btn_SubM4)
                pSelectMotor = pMotor[3];
            else if (sender == btn_AddM5 || sender == btn_SubM5)
                pSelectMotor = pMotor[4];
            else if (sender == btn_AddM6 || sender == btn_SubM6)
                pSelectMotor = pMotor[5];
            //-------------------------------------------------------------------
            double dbDir = 1.0;
            double dbRevDis = Convert.ToDouble(comboBox_Pitch.Text);
            if (sender == btn_SubM1 || sender == btn_SubM2 || sender == btn_SubM3 ||
                sender == btn_SubM4 || sender == btn_SubM5 || sender == btn_SubM6)
                dbDir = -1.0;
            //---------------------------
            if (pMotor != null)
                pSelectMotor.RevMove((dbRevDis * dbDir), m_dbManulSpeed);
        }

        private void btn_ManulSelectZStop_Click(object sender, EventArgs e)
        {
            Button[] pBtn = { btn_A1 , btn_A2 ,  btn_A3, btn_B1, btn_B2, btn_B3,
                              btn_C1 , btn_C2 ,  btn_C3, btn_D1, btn_D2, btn_D3,
                              btn_E1 , btn_E2 ,  btn_E3, btn_F1, btn_F2, btn_F3 };
            //------------------
            for (int i = 0; i < pBtn.Length; i++)
                pBtn[i].BackColor = Color.White;
            //---------------
            Button pSelectBt = (Button)sender;
            pSelectBt.BackColor = Color.DarkGray;
            //---------------
            if (sender == btn_A1 || sender == btn_A2 || sender == btn_A3)
            {
                iTrayID = iArrayX = 0;
                iArrayY = Convert.ToInt16(pSelectBt.Tag) -1;
            }
            else if (sender == btn_B1 || sender == btn_B2 || sender == btn_B3)
            {
                iTrayID = 0;
                iArrayX = 1;
                iArrayY = Convert.ToInt16(pSelectBt.Tag) - 1;
            }
            else if (sender == btn_C1 || sender == btn_C2 || sender == btn_C3)
            {
                iTrayID = 1;
                iArrayX = 0;
                iArrayY = Convert.ToInt16(pSelectBt.Tag) - 1;
            }
            else if (sender == btn_D1 || sender == btn_D2 || sender == btn_D3)
            {
                iTrayID = 1;
                iArrayX = 1;
                iArrayY = Convert.ToInt16(pSelectBt.Tag) - 1;
            }
            else if (sender == btn_E1 || sender == btn_E2 || sender == btn_E3)
            {
                iTrayID = 2;
                iArrayX = 0;
                iArrayY = Convert.ToInt16(pSelectBt.Tag) - 1;
            }
            else if (sender == btn_F1 || sender == btn_F2 || sender == btn_F3)
            {
                iTrayID = 2;
                iArrayX = 1;
                iArrayY = Convert.ToInt16(pSelectBt.Tag) - 1;
            }

        }

        private void btn_Move_Click(object sender, EventArgs e)
        {
            if (m_strSelect == "")
            {
                MessageBox.Show(this, "請選擇移動點位");
                return;
            }

            pMachine.Stop();
            //-----------------------
            Sys_Define.tyAXIS_XYZT tyTargetPos = new Sys_Define.tyAXIS_XYZT();
            string SelectText = listView_Pos.SelectedItems[0].Text;
            if (SelectText == strPastingStPos[0])             //strPastingStPos[0] = "待命位"
            {
                pMachine.m_PastingSt.pYMotor = null;
                tyTargetPos.X = pMachine.m_PastingSt.m_tyHeadIdlePos.X;
                tyTargetPos.Z = pMachine.m_PastingSt.m_tyHeadIdlePos.Z;
                tyTargetPos.T = pMachine.m_PastingSt.m_tyHeadIdlePos.T;
            }
            else if (SelectText == strPastingStPos[1])        //strPastingStPos[1] = "吸嘴影像位"
            {
                pMachine.m_PastingSt.pYMotor = null;
                tyTargetPos.X = pMachine.m_PastingSt.m_tyHeadVisionPos.X;
                tyTargetPos.Z = pMachine.m_PastingSt.m_tyHeadVisionPos.Z;
                tyTargetPos.T = pMachine.m_PastingSt.m_tyHeadVisionPos.T;
            }
            else if (SelectText == strPastingStPos[2])        //strPastingStPos[2] = "吸嘴調教位"
            {
                pMachine.m_PastingSt.pYMotor = pMotor[3];
                tyTargetPos.X = pMachine.m_PastingSt.m_tyHeadCalibrationBoardPos.X;
                tyTargetPos.Y = pMachine.m_PastingSt.m_tyHeadCalibrationBoardPos.Y;
                tyTargetPos.Z = pMachine.m_PastingSt.m_tyHeadCalibrationBoardPos.Z;
                tyTargetPos.T = pMachine.m_PastingSt.m_tyHeadCalibrationBoardPos.T;
            }
            else if (SelectText == strPastingStPos[3])        //strPastingStPos[3] = "吸嘴拋料位"
            {
                pMachine.m_PastingSt.pYMotor = pMotor[3];
                tyTargetPos.X = pMachine.m_PastingSt.m_tyHeadDisCardPos.X;
                tyTargetPos.Y = pMachine.m_PastingSt.m_tyHeadDisCardPos.Y;
                tyTargetPos.Z = pMachine.m_PastingSt.m_tyHeadDisCardPos.Z;
                tyTargetPos.T = pMachine.m_PastingSt.m_tyHeadDisCardPos.T;
            }
            //-----------移動--------------------
            pMachine.m_PastingSt.PastingStMove(tyTargetPos, m_dbManulSpeed);
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {          
            int iLightValue = trackBar_Light.Value;
            int iChannelID = (comboBox_Light.SelectedIndex + 6);
            pMachine.m_CCDSystem.m_LightControlBox.SetIntensity(iChannelID, iLightValue);
            //-------------
            double dbiValue = Convert.ToDouble(iLightValue);
            double dbMaxValue = Convert.ToDouble(trackBar_Light.Maximum);
            double dbRatio = (dbiValue / dbMaxValue) * 100.0;
            int iSetRatio =Convert.ToInt16(dbRatio);
            label_Light.Text = "光源(" + iSetRatio.ToString() + "%):" ;
            //-------------
        }

        private void comboBox_Light_SelectedIndexChanged(object sender, EventArgs e)
        {
            pMachine.m_CCDSystem.m_LightControlBox.TrunOff(0);  //全部Channel關閉
            //-------------------------
            int iChannelID = (comboBox_Light.SelectedIndex + 6);
            int LightValue = pMachine.m_CCDSystem.m_iLightValue[iChannelID];
            //-------------------------
            trackBar_Light.Value = LightValue;
        }

        private void btn_LightSet_Click(object sender, EventArgs e)
        {
            int iLightValue = trackBar_Light.Value;
            //-------------------------
            int iChannelID = (comboBox_Light.SelectedIndex + 6);
            pMachine.m_CCDSystem.m_iLightValue[iChannelID] = iLightValue;
            //-------------------------
            string strWorkPath= pMachine.GetWorkDataPath();
            pMachine.m_CCDSystem.SaveWorkData(strWorkPath);
        }

        private void btn_CCDLive_Click(object sender, EventArgs e)
        {

        }

        private void btn_TTT_Click(object sender, EventArgs e)
        {
            bool[] bUse = {checkBox_HeadDown1.Checked , checkBox_HeadDown2.Checked,checkBox_HeadDown3.Checked,
                           checkBox_HeadDown4.Checked,checkBox_HeadDown5.Checked,checkBox_HeadDown6.Checked };
            //--------------------------------------
            if (comboBox_PickZStop.SelectedIndex == 0)
                pMachine.m_PastingSt.PastingStZStopPosMove(iTrayID, iArrayX, iArrayY, bUse, m_dbManulSpeed);        
            //else  if(comboBox_PickZStop.SelectedIndex == 1)
                //pMachine.m_PastingSt.PastingStNonMoevJustPickZStop(bUse, m_dbManulSpeed);
            else
                pMachine.m_PastingSt.PastingStPickZStop(iTrayID, iArrayX, iArrayY, bUse, m_dbManulSpeed);
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            string strMachinePath = pMachine.GetMachineDataPath();

            string SelectText = listView_Pos.SelectedItems[0].Text;
            DialogResult dlgResult = MessageBox.Show(" 確認是否SAVE   [ " + SelectText + " ]    點位 ? ", "Save Dlg", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dlgResult != DialogResult.Yes)
                return;
            //---------------------------
            Sys_Define.tyAXIS_XY tyTargetPos = new Sys_Define.tyAXIS_XY();


            if (SelectText == strPastingStPos[0])             //strPastingStPos[0] = "待命位"
            {
                pMachine.m_PastingSt.m_tyHeadIdlePos.X = pMotor[0].GetPosition();
                pMachine.m_PastingSt.m_tyHeadIdlePos.Z = pMotor[1].GetPosition();
                pMachine.m_PastingSt.m_tyHeadIdlePos.T = pMotor[2].GetPosition();
            }
            else if (SelectText == strPastingStPos[1])        //strPastingStPos[1] = "吸嘴影像位"
            {
                pMachine.m_PastingSt.m_tyHeadVisionPos.X = pMotor[0].GetPosition();
                pMachine.m_PastingSt.m_tyHeadVisionPos.Z = pMotor[1].GetPosition();
                pMachine.m_PastingSt.m_tyHeadVisionPos.T = pMotor[2].GetPosition();
            }
            else if (SelectText == strPastingStPos[2])        //strPastingStPos[2] = "吸嘴調教位"
            {
                pMachine.m_PastingSt.m_tyHeadCalibrationBoardPos.X = pMotor[0].GetPosition();
                pMachine.m_PastingSt.m_tyHeadCalibrationBoardPos.Z = pMotor[1].GetPosition();
                pMachine.m_PastingSt.m_tyHeadCalibrationBoardPos.T = pMotor[2].GetPosition();
                //------------
                pMachine.m_PastingSt.m_tyHeadCalibrationBoardPos.Y = pMotor[3].GetPosition();
            }
            else if (SelectText == strPastingStPos[3])        //strPastingStPos[3] = "吸嘴拋料位"
            {
                pMachine.m_PastingSt.m_tyHeadDisCardPos.X = pMotor[0].GetPosition();
                pMachine.m_PastingSt.m_tyHeadDisCardPos.Z = pMotor[1].GetPosition();
                pMachine.m_PastingSt.m_tyHeadDisCardPos.T = pMotor[2].GetPosition();
                //------------
                pMachine.m_PastingSt.m_tyHeadDisCardPos.Y = pMotor[3].GetPosition();
            }
            //------------------------
            pMachine.m_PastingSt.SaveMachineData(strMachinePath);
        }
    }
}
