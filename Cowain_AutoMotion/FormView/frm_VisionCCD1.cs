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
using MotionBase;
using Cowain;
using Cowain_Machine;

namespace Cowain_Form.FormView
{
    public partial class frm_VisionCCD1 : Form
    {
        public frm_VisionCCD1(ref clsMachine refMachine)
        {
            pMachine = refMachine;
            InitializeComponent();
        }

        clsMachine pMachine = null;
        ImageList ImgList = new ImageList();
        int m_iStationID = 0;
        string m_strSelect = "";
        double m_dbManulSpeed = 30;
        bool m_bCCDLive = false, m_bCapture = false;
        //******************
        string[] strCCDPos = { "TP1_影像位1", "TP1_影像位2", "TP2_影像位1", "TP2_影像位2", "ZStop影像起始位", "吸嘴校正板影像位" };
        //******************
        DrvMotor[] pMotor = new DrvMotor[6];
        //clsCognexCCD pCCD = null;


        private void frm_VisionCC1_Shown(object sender, EventArgs e)
        {
            ImgList.Images.Add("UnSelect", Cowain_AutoMotion.Properties.Resources.SetOk_Disable);
            ImgList.Images.Add("Select", Cowain_AutoMotion.Properties.Resources.SetOk);
            listView_Pos.Items.Clear();
            listView_Pos.LargeImageList = ImgList;
            listView_Pos.SmallImageList = ImgList;
            //-------------------------------------
            comboBox_Pitch.SelectedIndex = 0;
            comboBox_function.SelectedIndex = 0;
            //-------------------------------------
            Fun_DisplayPosList();
        }

        private void frm_VisionCC1_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                timer_ReFlash.Enabled = true;
                timer_CCDLive.Enabled = true;
                //------------
                comboBox_Pitch.SelectedIndex = 0;
                comboBox_function.SelectedIndex = 0;
                //------------
                //pCCD = pMachine.m_CCDSystem.m_CCD1;
                //------------
                Fun_DisplayPosList();
                m_bCapture = false;
                m_bCCDLive = false; //開啟CCD Live
                btn_live.BackColor = Color.White;
                //---------------------------------------------------------------
                bool bVisible = (pMachine.m_LoginUser == MotionBase.Sys_Define.enPasswordType.Maker ||
                                 pMachine.m_LoginUser == MotionBase.Sys_Define.enPasswordType.MacEng||
                                 pMachine.m_LoginUser == MotionBase.Sys_Define.enPasswordType.Eng);
                btn_Save.Visible = bVisible;
                //---------------------------------------------------------------
                groupBox_Setting.Visible = (pMachine.m_LoginUser == MotionBase.Sys_Define.enPasswordType.Maker ||
                                            pMachine.m_LoginUser == MotionBase.Sys_Define.enPasswordType.MacEng) ? true : false;

            }
            else
            {
                //pMachine.m_CCDSystem.m_LightControlBox.SetIntensity(0, 0); //全部亮度切成0
                m_bCCDLive = m_bCapture = false;
                //pCCD = null;
                timer_ReFlash.Enabled = false;
                timer_CCDLive.Enabled = false;
            }
        }

        public void Fun_DisplayPosList()
        {
            m_strSelect = "";
            listView_Pos.Clear();
            int i, j;
            //**********顯示馬達List***************
            #region 顯示Pos_List
            for (i = 0; i < strCCDPos.Length; i++)
                listView_Pos.Items.Add(strCCDPos[i], "UnSelect");
            #endregion
            ShowMotorAndValve();
        }

        private void ShowMotorAndValve()
        {
            int i, j;
            //-------------------------------------
            for (i = 0; i < pMotor.Length; i++)
                pMotor[i] = null;
            //-----------------------

            //--------指定Motor & Valve------------
            //pMotor[0] = pMachine.m_CCDMoveSt.m_CCDMove.m_motX;
            //pMotor[3] = pMachine.m_TpStationAuto[0].m_TpStation.m_TpStage.m_Stage.m_motY;
            //pMotor[4] = pMachine.m_TpStationAuto[1].m_TpStation.m_TpStage.m_Stage.m_motY;
            //pMotor[5] = pMachine.m_ZStopStationAuto.m_ZStopStation.m_Stage.m_motY;
            //-----------------------
            #region 顯示馬達

            MotionBase.DrvValve.tyValve_Parameter ptyParameter = new MotionBase.DrvValve.tyValve_Parameter();
            bool bVisible = false;
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

        private void btn_Snap_Click(object sender, EventArgs e)
        {
            //if (pCCD.GetisFindCCDAndInitial())
            //    pCCD.OneShot(ref cogRecordDisplay1);
            //else
            //{
            //    OpenFileDialog file = new OpenFileDialog();
            //    file.ShowDialog();
            //    String selectBmpFile = file.FileName; // file.SafeFileName;
            //    //---------------------
            //    if (selectBmpFile == "")
            //    {
            //        MessageBox.Show(this, "未選取BMP檔案 , 請重新確認 !!");
            //        return;
            //    }
            //    //---------------------
            //    Bitmap pImage = new Bitmap(selectBmpFile);
            //    CogImage8Grey cogImg = new CogImage8Grey(pImage);
            //    cogRecordDisplay1.Image = cogImg;
            //}
        }

        private void btn_live_Click(object sender, EventArgs e)
        {
            m_bCCDLive = !m_bCCDLive;

        }

        private void btn_Stop_Click(object sender, EventArgs e)
        {
            pMachine.Stop();
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


        private void btn_FunctionRun_Click(object sender, EventArgs e)
        {
            int iFunctionIndex ;
            //CogImage8Grey cogImage = (CogImage8Grey)(cogRecordDisplay1.Image);
            //if (cogRecordDisplay1.Image != null)
            {
                if (comboBox_function.SelectedIndex <= 3)  //0,1,2,3 ->TP搜尋使用
                {
                    //iFunctionIndex = comboBox_function.SelectedIndex;
                    //pMachine.m_CCDSystem.m_CCD1Function.RunTBFunction(ref cogImage, ref pMachine.m_CCDSystem.m_VisionTB[iFunctionIndex],
                    //                ref cogRecordDisplay1, clsCCDSystem.enCCDFunction.en_TP1_1);
                }
                else if (comboBox_function.SelectedIndex == 4)
                {
                    //iFunctionIndex = (int)clsCCDSystem.enCCDFunction.en_ZStopOffset_6Head;
                    //pMachine.m_CCDSystem.m_CCD1Function.RunTBFunction(ref cogImage, ref pMachine.m_CCDSystem.m_VisionTB[iFunctionIndex],
                    //                ref cogRecordDisplay1, clsCCDSystem.enCCDFunction.en_ZStopOffset_6Head);
                }
                else if (comboBox_function.SelectedIndex == 5)
                {
                    //iFunctionIndex = (int)clsCCDSystem.enCCDFunction.en_ZStopOffset_1Head;
                    //pMachine.m_CCDSystem.m_CCD1Function.RunTBFunction(ref cogImage, ref pMachine.m_CCDSystem.m_VisionTB[iFunctionIndex],
                    //                ref cogRecordDisplay1, clsCCDSystem.enCCDFunction.en_ZStopOffset_1Head);
                }

            }


        }

        private void timer_ReFlash_Tick(object sender, EventArgs e)
        {
            if (pMachine == null)
                return;
            if (!MachineDataDefine.m_bUiTimerEnable)
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
            //bool bCCDMoveStIdle = pMachine.m_CCDMoveSt.isIDLE();
            //bool bCCDFunctionIdle = pMachine.m_CCDSystem.m_CCD1Function.isIDLE();
            bool bBtnEnable = (bMototisIdle && bMacIdle );
            //--------------------------------
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
            //---------------------
            Button[] pBtn = { btn_Move, btn_Save };
            //--------------------------------
            if (!pMachine.GetHomeCompleted() )
            {
                for (i = 0; i < pBtn.Length; i++)
                    pBtn[i].Enabled = false;
            }
            //---------------------
            btn_Snap.Enabled = (m_bCCDLive) ? false : true;
            btn_FunctionRun.Enabled = (m_bCCDLive) ? false : true;
            btn_live.BackColor = (m_bCCDLive) ? Color.Gray : Color.White;
        }

        private void btn_Move_Click(object sender, EventArgs e)
        {
            if (m_strSelect == "")
            {
                MessageBox.Show(this, JudgeLanguage.JudgeLag("請選擇移動點位"));
                return;
            }

            pMachine.Stop();
            //-----------------------
            Sys_Define.tyAXIS_XY tyTargetPos = new Sys_Define.tyAXIS_XY();
            string SelectText = listView_Pos.SelectedItems[0].Text;
            if (SelectText == strCCDPos[0])        //strCCDPos[0] = "TP1_影像位1"
            {
                //pMachine.m_CCDMoveSt.pYMotor = pMotor[3]; // TP StageY
                //tyTargetPos.X = pMachine.m_CCDMoveSt.m_tyTP1Vision1.X;
                //tyTargetPos.Y = pMachine.m_CCDMoveSt.m_tyTP1Vision1.Y;
            }
            else if (SelectText == strCCDPos[1])        //strCCDPos[1] = "TP1_影像位2"
            {
                //pMachine.m_CCDMoveSt.pYMotor = pMotor[3]; // TP StageY
                //tyTargetPos.X = pMachine.m_CCDMoveSt.m_tyTP1Vision2.X;
                //tyTargetPos.Y = pMachine.m_CCDMoveSt.m_tyTP1Vision2.Y;
            }
            else if (SelectText == strCCDPos[2])        //strCCDPos[2] = "TP2_影像位1"
            {
                //pMachine.m_CCDMoveSt.pYMotor = pMotor[4]; // TP StageY
                //tyTargetPos.X = pMachine.m_CCDMoveSt.m_tyTP2Vision1.X;
                //tyTargetPos.Y = pMachine.m_CCDMoveSt.m_tyTP2Vision1.Y;
            }
            else if (SelectText == strCCDPos[3])        //strCCDPos[3] = "TP2_影像位2"
            {
                //pMachine.m_CCDMoveSt.pYMotor = pMotor[4]; // TP StageY
                //tyTargetPos.X = pMachine.m_CCDMoveSt.m_tyTP2Vision2.X;
                //tyTargetPos.Y = pMachine.m_CCDMoveSt.m_tyTP2Vision2.Y;
            }
            else if (SelectText == strCCDPos[4])        //strCCDPos[4] = "ZStop影像位"
            {
                //pMachine.m_CCDMoveSt.pYMotor = pMotor[5]; // ZStop StageY
                //tyTargetPos.X = pMachine.m_CCDMoveSt.m_tyZStopVision.X;
                //tyTargetPos.Y = pMachine.m_CCDMoveSt.m_tyZStopVision.Y;
            }
            else if (SelectText == strCCDPos[5])        //strCCDPos[5] = "吸嘴校正板影像位"
            {
                //pMachine.m_CCDMoveSt.pYMotor = pMotor[5]; // ZStop StageY
                //tyTargetPos.X = pMachine.m_CCDMoveSt.m_tyHeadCalibrationBoardPos.X;
                //tyTargetPos.Y = pMachine.m_CCDMoveSt.m_tyHeadCalibrationBoardPos.Y;
            }
            //-----------移動--------------------
            //pMachine.m_CCDMoveSt.CCDStXYMove(tyTargetPos, m_dbManulSpeed);
        }

        private void btn_Home_Click(object sender, EventArgs e)
        {
            //pMachine.m_CCDMoveSt.m_CCDMove.DoHome();
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            string strMachinePath = pMachine.GetMachineDataPath();

            string SelectText = listView_Pos.SelectedItems[0].Text;
            DialogResult dlgResult = MessageBox.Show(JudgeLanguage.JudgeLag(" 確認是否SAVE   [ " + SelectText + " ]    點位 ? "), "Save Dlg", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dlgResult != DialogResult.Yes)
                return;
            //---------------------------
            Sys_Define.tyAXIS_XY tyTargetPos = new Sys_Define.tyAXIS_XY();

            if (SelectText == strCCDPos[0])        //strCCDPos[0] = "TP1_影像位1"
            {
                //pMachine.m_CCDMoveSt.m_tyTP1Vision1.X = pMotor[0].GetPosition();   // pMotor[0]=CCDX
                //pMachine.m_CCDMoveSt.m_tyTP1Vision1.Y = pMotor[3].GetPosition();  // pMotor[3]= TP1 Y
            }
            else if (SelectText == strCCDPos[1])        //strCCDPos[1] = "TP1_影像位2"
            {
                //pMachine.m_CCDMoveSt.m_tyTP1Vision2.X = pMotor[0].GetPosition();   // pMotor[0]=CCDX
                //pMachine.m_CCDMoveSt.m_tyTP1Vision2.Y = pMotor[3].GetPosition();  // pMotor[3]= TP1 Y
            }
            else if (SelectText == strCCDPos[2])        //strCCDPos[2] = "TP2_影像位1"
            {
                //pMachine.m_CCDMoveSt.m_tyTP2Vision1.X = pMotor[0].GetPosition();   // pMotor[0]=CCDX
                //pMachine.m_CCDMoveSt.m_tyTP2Vision1.Y = pMotor[4].GetPosition();  // pMotor[4]= TP2 Y
            }
            else if (SelectText == strCCDPos[3])        //strCCDPos[3] = "TP2_影像位2"
            {
                //pMachine.m_CCDMoveSt.m_tyTP2Vision2.X = pMotor[0].GetPosition();   // pMotor[0]=CCDX
                //pMachine.m_CCDMoveSt.m_tyTP2Vision2.Y = pMotor[4].GetPosition();  // pMotor[4]= TP2 Y
            }
            else if (SelectText == strCCDPos[4])        //strCCDPos[4] = "ZStop影像位"
            {
                //pMachine.m_CCDMoveSt.m_tyZStopVision.X = pMotor[0].GetPosition();   // pMotor[0]=CCDX
                //pMachine.m_CCDMoveSt.m_tyZStopVision.Y = pMotor[5].GetPosition();  // pMotor[5]= ZStop Y
            }
            else if (SelectText == strCCDPos[5])        //strCCDPos[5] = "吸嘴校正板影像位"
            {
                //pMachine.m_CCDMoveSt.m_tyHeadCalibrationBoardPos.X = pMotor[0].GetPosition();   // pMotor[0]=CCDX
                //pMachine.m_CCDMoveSt.m_tyHeadCalibrationBoardPos.Y = pMotor[5].GetPosition();  // pMotor[5]= ZStop Y
            }
            //------------------------

            //pMachine.m_CCDMoveSt.SaveMachineData(strMachinePath);
        }

        private void comboBox_function_SelectedIndexChanged(object sender, EventArgs e)
        {
            //cogRecordDisplay1.InteractiveGraphics.Clear();
            //cogRecordDisplay1.StaticGraphics.Clear();
            //----------------
            //pMachine.m_CCDSystem.m_LightControlBox.TrunOff(0);  //全部Channel關閉
            int iChannelID = 0, LightValue;
            //-------------------------
            if (comboBox_function.SelectedIndex <= 3)  //0,1,2,3 ->TP搜尋使用
            {
                groupBox_Setting.Visible = true;
                //iChannelID = (comboBox_function.SelectedIndex + 1);
                //LightValue = pMachine.m_CCDSystem.m_iLightValue[iChannelID];
                //pMachine.m_CCDSystem.m_LightControlBox.SetIntensity(iChannelID, LightValue);
            }
            else {
                groupBox_Setting.Visible = false;
                //iChannelID =(int) clsCCDSystem.enLightControlBox.en_Channel5;
                //LightValue = pMachine.m_CCDSystem.m_iLightZStopSearch;
                //pMachine.m_CCDSystem.m_LightControlBox.SetIntensity(iChannelID, LightValue);
            }
            //trackBar_Light.Value = LightValue;
            //-------------------------
        }

        private void trackBar_Light_ValueChanged(object sender, EventArgs e)
        {
            //pMachine.m_CCDSystem.m_LightControlBox.SetIntensity(0, 0);
            int iLightValue = trackBar_Light.Value;


            int iChannelID = 0; ;
            //-------------------------
            //if (comboBox_function.SelectedIndex <= 3)  //0,1,2,3 ->TP搜尋使用
            //    iChannelID = (comboBox_function.SelectedIndex + 1);
            //else
            //    iChannelID = (int)clsCCDSystem.enLightControlBox.en_Channel5;
            ////-------------------------
            //pMachine.m_CCDSystem.m_LightControlBox.SetIntensity(iChannelID, iLightValue);
            //-------------
            double dbiValue = Convert.ToDouble(iLightValue);
            double dbMaxValue = Convert.ToDouble(trackBar_Light.Maximum);
            double dbRatio = (dbiValue / dbMaxValue) * 100.0;
            int iSetRatio = Convert.ToInt16(dbRatio);
            label_Light.Text = "光源(" + iSetRatio.ToString() + "%):";
            //-------------
        }

        private void btn_LightSet_Click(object sender, EventArgs e)
        {
            int iLightValue = trackBar_Light.Value;
            //-------------------------
            int iChannelID;
            if (comboBox_function.SelectedIndex <= 3)  //0,1,2,3 ->TP搜尋使用
            {
                iChannelID = (comboBox_function.SelectedIndex + 1);
                //pMachine.m_CCDSystem.m_iLightValue[iChannelID] = iLightValue;
            }
            else
            {
                //iChannelID = (int)clsCCDSystem.enLightControlBox.en_Channel5;
               // pMachine.m_CCDSystem.m_iLightZStopSearch = iLightValue;
            }
            //-------------------------
            //-------------------------
            string strWorkPath = pMachine.GetWorkDataPath();
            //pMachine.m_CCDSystem.SaveWorkData(strWorkPath);
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

        private void button1_Click(object sender, EventArgs e)
        {

                if (comboBox_function.SelectedIndex <= 3)  //0,1,2,3 ->TP搜尋使用
                {

                //cogRecordDisplay1.StaticGraphics.Clear();
                //cogRecordDisplay1.InteractiveGraphics.Clear();

                int iFunctionIndex = comboBox_function.SelectedIndex;
                   // CogFindLineTool pLine1 = (CogFindLineTool)pMachine.m_CCDSystem.m_VisionTB[iFunctionIndex].Tools["CogFindLineTool1"];
                  //  CogFindLineTool pLine2 = (CogFindLineTool)pMachine.m_CCDSystem.m_VisionTB[iFunctionIndex].Tools["CogFindLineTool2"];


                //ICogRecord LineRec1 = pLine1.CreateCurrentRecord();
                //ICogRecord LineRec2 = pLine2.CreateCurrentRecord();
                //--------------------------

                //// 畫線
                //CogLineSegment LineSeg1 = (CogLineSegment)LineRec1.SubRecords["InputImage"].SubRecords["ExpectedShapeSegment"].Content;
                //CogLineSegment LineSeg2 = (CogLineSegment)LineRec2.SubRecords["InputImage"].SubRecords["ExpectedShapeSegment"].Content;

                // 畫Calipers
                //CogGraphicCollection LineRegions1 = (CogGraphicCollection)LineRec1.SubRecords["InputImage"].SubRecords["CaliperRegions"].Content;
                //CogGraphicCollection LineRegions2 = (CogGraphicCollection)LineRec2.SubRecords["InputImage"].SubRecords["CaliperRegions"].Content;

                //cogRecordDisplay1.InteractiveGraphics.Add(LineSeg1, "", false);
                //cogRecordDisplay1.InteractiveGraphics.Add(LineSeg2, "", false);

                //if (LineRegions1 != null)
                //    foreach (ICogGraphic g in LineRegions1)
                //        cogRecordDisplay1.InteractiveGraphics.Add((ICogGraphicInteractive)g, "", false);

                //if (LineRegions2 != null)
                //    foreach (ICogGraphic g in LineRegions2)
                //        cogRecordDisplay1.InteractiveGraphics.Add((ICogGraphicInteractive)g, "", false);

            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            int iFunctionIndex = comboBox_function.SelectedIndex;
            #region 載入VPP
            string strPath2 = pMachine.GetWorkDataPath().Replace(".xml", ""); ;
            //string[] strFilePath = new string[(int)clsCCDSystem.enCCDFunction.en_Max];
            //strFilePath[0] = strPath2 + "\\TP1_1.vpp";
            //strFilePath[1] = strPath2 + "\\TP1_2.vpp";
            //strFilePath[2] = strPath2 + "\\TP2_1.vpp";
            //strFilePath[3] = strPath2 + "\\TP2_2.vpp";
            ////----------
            //strFilePath[4] = strPath2 + "\\NonZStop_Head1.vpp";
            //strFilePath[5] = strPath2 + "\\NonZStop_Head2.vpp";
            //strFilePath[6] = strPath2 + "\\NonZStop_Head3.vpp";
            //strFilePath[7] = strPath2 + "\\NonZStop_Head4.vpp";
            //strFilePath[8] = strPath2 + "\\NonZStop_Head5.vpp";
            //strFilePath[9] = strPath2 + "\\NonZStop_Head6.vpp";
            ////----------
            //strFilePath[10] = strPath2 + "\\ZStopCheck_Head1.vpp";
            //strFilePath[11] = strPath2 + "\\ZStopCheck_Head2.vpp";
            //strFilePath[12] = strPath2 + "\\ZStopCheck_Head3.vpp";
            //strFilePath[13] = strPath2 + "\\ZStopCheck_Head4.vpp";
            //strFilePath[14] = strPath2 + "\\ZStopCheck_Head5.vpp";
            //strFilePath[15] = strPath2 + "\\ZStopCheck_Head6.vpp";
            ////----------
            //strFilePath[16] = strPath2 + "\\ZStopSearch_6Head.vpp";
            //strFilePath[17] = strPath2 + "\\ZStopSearch_1Head.vpp";
            ////-----------
            //strFilePath[18] = strPath2 + "\\HeadRotCenter.vpp";
            ////---------------------
            #endregion     

            //string strVppfilePath = strFilePath[iFunctionIndex];
            //CogSerializer.SaveObjectToFile(pMachine.m_CCDSystem.m_VisionTB[iFunctionIndex], strVppfilePath, typeof(System.Runtime.Serialization.Formatters.Binary.BinaryFormatter), CogSerializationOptionsConstants.All);

            //cogRecordDisplay1.StaticGraphics.Clear();
            //cogRecordDisplay1.InteractiveGraphics.Clear();

        }

        private void timer_CCDLive_Tick(object sender, EventArgs e)
        {
            if (!MachineDataDefine.m_bUiTimerEnable)
                return;
            //---------------------

            #region CCD Live

            if (m_bCCDLive)
            {
                if (m_bCapture == false)
                {
                    m_bCapture = true;
                    //pCCD.OneShot();
                }
                else
                {
                    //if (pCCD.GetOneShotReady())
                    //{
                    //    CogImage8Grey image = pCCD.GetOnShotImage();
                    //    //-------------------
                    //    if (image == null)
                    //        return;
                    //    //----------------------------
                    //    cogRecordDisplay1.Image = image;
                    //    cogRecordDisplay1.InteractiveGraphics.Clear();
                    //    cogRecordDisplay1.StaticGraphics.Clear();

                    //    double dbImageWidth = Convert.ToDouble(image.Width);
                    //    double dbImageHeight = Convert.ToDouble(image.Height);
                    //    //-----------------
                    //    double dbCenterX = dbImageWidth / 2.0;
                    //    double dbCenterY = dbImageHeight / 2.0;
                    //    //-----------------
                    //    #region 畫中心十字線

                    //    CogCreateLineTool Line1 = new CogCreateLineTool();
                    //    CogCreateLineTool Line2 = new CogCreateLineTool();
                    //    Line1.InputImage = image;
                    //    Line2.InputImage = image;
                    //    Line1.Line.X = dbCenterX;
                    //    Line1.Line.Y = dbCenterY;
                    //    //--------
                    //    Line2.Line.X = dbCenterX;
                    //    Line2.Line.Y = dbCenterY;
                    //    Line2.Line.Rotation = (Math.PI / 180.0) * 90.0;
                    //    //--------------------
                    //    Line1.OutputColor = CogColorConstants.Green;
                    //    Line2.OutputColor = CogColorConstants.Green;
                    //    Line1.Run();
                    //    Line2.Run();
                    //    //--------------------
                    //    cogRecordDisplay1.StaticGraphics.Add(Line1.GetOutputLine(), "Result");
                    //    cogRecordDisplay1.StaticGraphics.Add(Line2.GetOutputLine(), "Result");
                    //    #endregion
                    //    //-----------------
                    //    m_bCapture = false;
                    //}
                }
            }
            #endregion
        }


    }
}
