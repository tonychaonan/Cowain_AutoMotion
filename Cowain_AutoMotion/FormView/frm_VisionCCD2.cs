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
using Cognex.VisionPro.CNLSearch;
using Cognex.VisionPro.Exceptions;
using Cognex.VisionPro.Display;
using Cognex.VisionPro.ImageProcessing;
using Cognex.VisionPro.Caliper;
using Cognex.VisionPro.Dimensioning;
using MotionBase;
namespace Cowain_Form.FormView
{
    public partial class frm_VisionCCD2 : Form
    {
        public frm_VisionCCD2(ref Machine refMachine)
        {
            pMachine = refMachine;
            InitializeComponent();
        }

        clsCognexCCD pCCD = null;
        Machine pMachine = null;
        ImageList ImgList = new ImageList();
        int m_iStationID = 0;
        string m_strSelect = "";
        double m_dbManulSpeed = 30;
        bool m_bCCDLive = false, m_bCapture = false;
        //******************
        string[] strCCDPos = { "吸嘴影像位", "影像計算-吸嘴偏差計算" , "影像計算-ZStop偏差計算", "影像計算-旋轉中心計算" };
        //******************
        DrvMotor[] pMotor = new DrvMotor[6];

        CogTransform2DLinear goCNLOrigin1 = new CogTransform2DLinear();
        CogTransform2DLinear goCNLOrigin2 = new CogTransform2DLinear();

        private void frm_VisionCCD2_Shown(object sender, EventArgs e)
        {
            ImgList.Images.Add("UnSelect", Cowain_GelToPCB.Properties.Resources.SetOk_Disable);
            ImgList.Images.Add("Select", Cowain_GelToPCB.Properties.Resources.SetOk);
            listView_Pos.Items.Clear();
            listView_Pos.LargeImageList = ImgList;
            listView_Pos.SmallImageList = ImgList;
            //-------------------------------------
            comboBox_Pitch.SelectedIndex = 0;
            comboBox_function.SelectedIndex = 0;
            //-------------------------------------
            pMachine.m_CCDSystem.pRecDisplay = cogRecordDisplay1;
            pMachine.m_CCDSystem.pDataGridView = dataGridView_CalData;
            //-------------------------------------
            Fun_DisplayPosList();
        }

        private void frm_VisionCCD2_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                timer_ReFlash.Enabled = true;
                timer_CCDLive.Enabled = true;
                //------------
                comboBox_Pitch.SelectedIndex = 0;
                comboBox_function.SelectedIndex = 0;
                comboBox_Head.SelectedIndex = 0;
                //------------
                pCCD = pMachine.m_CCDSystem.m_CCD2;
                //------------
                pMachine.m_TpStationAuto[0].m_TpStation.pDisplayCheckZStop = cogRecordDisplay1;
                pMachine.m_TpStationAuto[1].m_TpStation.pDisplayCheckZStop = cogRecordDisplay1;
                //------------
                pMachine.m_CCDSystem.pRecDisplay = cogRecordDisplay1;
                pMachine.m_CCDSystem.pDataGridView = dataGridView_CalData;
                pMachine.m_CCDSystem.pRecZStopDisplay = cogRecordDisplay1;
                pMachine.m_CCDSystem.pRecRotCenterDisplay = cogRecordDisplay1;
                pMachine.m_PastingSt.pListView = listView_RotCenter;
                //------------
                fun_ShowDataGridView();
                Fun_DisplayPosList();
                m_bCapture = false;
                m_bCCDLive = false; //開啟CCD Live
                btn_live.BackColor = Color.White;
                //---------------------------------------------------------------
                bool bVisible = (pMachine.m_LoginUser == MotionBase.Sys_Define.enPasswordType.Maker || 
                                 pMachine.m_LoginUser == MotionBase.Sys_Define.enPasswordType.MacEng||
                                 pMachine.m_LoginUser == MotionBase.Sys_Define.enPasswordType.Eng);
                btn_Save.Visible = bVisible;
                btn_SaveHeadCenter.Visible = bVisible;
                button1.Visible = bVisible;
                //---------------------------------------------------------------
                groupBox_Setting.Visible = (pMachine.m_LoginUser == MotionBase.Sys_Define.enPasswordType.Maker||
                                            pMachine.m_LoginUser == MotionBase.Sys_Define.enPasswordType.MacEng) ? true : false;
            }
            else
            {
                pMachine.m_CCDSystem.m_LightControlBox.SetIntensity(0, 0); //全部亮度切成0
                m_bCCDLive = m_bCapture = false;
                pCCD = null;
                timer_ReFlash.Enabled = false;
                timer_CCDLive.Enabled = false;
            }
        }
        public void fun_ShowDataGridView()
        {
             dataGridView_CalData.Rows.Clear();
            for (int i = 0; i < 4; i++)
            {
                //---------結果Show至DataGrid上----------
                string strHeadID = (i + 1).ToString();
                string strX = pMachine.m_PastingSt.m_tyNozzleToCenter[i].X.ToString("0.000");
                string strY = pMachine.m_PastingSt.m_tyNozzleToCenter[i].Y.ToString("0.000");
                string strT = pMachine.m_PastingSt.m_tyNozzleToCenter[i].T.ToString("0.000");
                string strOK = "OK";
                //--------------------------------
                dataGridView_CalData.Rows.Add(new object[] { strHeadID, strX, strY, strT, strOK });
                dataGridView_CalData.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.White;
                //---------------------------------------
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
            pMotor[0] = pMachine.m_PastingSt.m_PastingHead.m_motX;
            pMotor[1] = pMachine.m_PastingSt.m_PastingHead.m_motZ;
            pMotor[2] = pMachine.m_PastingSt.m_PastingHead.m_motT;
            //pMotor[3] = pMachine.m_ZStopStationAuto.m_ZStopStation.m_Stage.m_motY;
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

        private void btn_live_Click(object sender, EventArgs e)
        {
            m_bCCDLive = !m_bCCDLive;
        }

        private void btn_Stop_Click(object sender, EventArgs e)
        {
            pMachine.Stop();
        }

        private void btn_Home_Click(object sender, EventArgs e)
        {
            pMachine.m_PastingSt.DoHome();
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
            bool bCCDSystemIdle = pMachine.m_CCDSystem.isIDLE();
            bool bCCDFunctionIdle = pMachine.m_CCDSystem.m_CCD2Function.isIDLE();
            bool bBtnEnable = (bMototisIdle && bMacIdle && bPastingHeadStIdle&& bPastingHeadIdle && bCCDFunctionIdle && bCCDSystemIdle);
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
            if (!pMachine.GetHomeCompleted())
            {
                for (i = 0; i < pBtn.Length; i++)
                    pBtn[i].Enabled = false;
            }
            //---------------------


            btn_Snap.Enabled = (m_bCCDLive) ? false : true;
            btn_FunctionRun.Enabled = (m_bCCDLive) ? false : true;
            btn_live.BackColor = (m_bCCDLive) ? Color.Gray : Color.White;

            //-------------------
            label_CX.Text ="CX: " + pMachine.m_PastingSt.m_tyRotCenter.X.ToString("0.000");
            label_CY.Text = "CY: " + pMachine.m_PastingSt.m_tyRotCenter.Y.ToString("0.000");

        }

        private void timer_CCDLive_Tick(object sender, EventArgs e)
        {
            if (!MDataDefine.m_bUiTimerEnable)
                return;
            //---------------------

            #region CCD Live

            if (m_bCCDLive)
            {
                if (m_bCapture == false)
                {
                    m_bCapture = true;
                    pCCD.OneShot();
                }
                else
                {
                    if (pCCD.GetOneShotReady())
                    {
                        CogImage8Grey image = pCCD.GetOnShotImage();
                        //-------------------
                        if (image == null)
                            return;
                        //----------------------------
                        cogRecordDisplay1.Image = image;
                        cogRecordDisplay1.StaticGraphics.Clear();

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
            if (SelectText == strCCDPos[0])        //strCCDPos[0] = "吸嘴影像位"
            {
                pMachine.m_PastingSt.pYMotor = null;
                tyTargetPos.X = pMachine.m_PastingSt.m_tyHeadVisionPos.X;
                tyTargetPos.Z = pMachine.m_PastingSt.m_tyHeadVisionPos.Z;
                tyTargetPos.T = pMachine.m_PastingSt.m_tyHeadVisionPos.T;
                //-----------移動--------------------
                pMachine.m_PastingSt.PastingStMove(tyTargetPos, m_dbManulSpeed);
            } else if (SelectText == strCCDPos[1])    //strCCDPos[1] = "影像計算-吸嘴偏差計算"
            {
                Tab_ShowData.SelectedIndex = 0;
                pMachine.m_CCDSystem.pDataGridView = dataGridView_CalData;
                pMachine.m_PastingSt.CaptureHeadAndCCDCenter(m_dbManulSpeed);
            } else if (SelectText == strCCDPos[2])   //strCCDPos[2] = "影像計算-ZStop偏差計算"
            {
                Tab_ShowData.SelectedIndex = 1;
                pMachine.m_CCDSystem.pDataGridView = dataGridView_ZStopSearch;
                pMachine.m_CCDSystem.pListView = listView_CalZStopResult;
                pMachine.m_PastingSt.CaptureZStopCenter(m_dbManulSpeed);
            }
            else if (SelectText == strCCDPos[3])   //strCCDPos[2] =  "影像計算-旋轉中心計算"
            {
                Tab_ShowData.SelectedIndex = 2;                 
                pMachine.m_PastingSt.CalHeadRotCenter(m_dbManulSpeed);
            }


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
        private void btn_Save_Click(object sender, EventArgs e)
        {
            string strMachinePath = pMachine.GetMachineDataPath();

            string SelectText = listView_Pos.SelectedItems[0].Text;
            DialogResult dlgResult = MessageBox.Show(" 確認是否SAVE   [ " + SelectText + " ]    點位 ? ", "Save Dlg", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dlgResult != DialogResult.Yes)
                return;
            //---------------------------
            Sys_Define.tyAXIS_XY tyTargetPos = new Sys_Define.tyAXIS_XY();

            if (SelectText == strCCDPos[0])        //strPastingStPos[1] = "吸嘴影像位"
            {
                pMachine.m_PastingSt.m_tyHeadVisionPos.X = pMotor[0].GetPosition();
                pMachine.m_PastingSt.m_tyHeadVisionPos.Z = pMotor[1].GetPosition();
                pMachine.m_PastingSt.m_tyHeadVisionPos.T = pMotor[2].GetPosition();
            }
            //------------------------
            pMachine.m_PastingSt.SaveMachineData(strMachinePath);
        }

        private void comboBox_function_SelectedIndexChanged(object sender, EventArgs e)
        {
            pMachine.m_CCDSystem.m_LightControlBox.TrunOff(0);  //全部Channel關閉
            //-------------------------
            int iChannelID = (int) clsCCDSystem.enLightControlBox.en_Channel6; //(comboBox_function.SelectedIndex + 6);
            int LightValue = 0;
            if (comboBox_function.SelectedIndex == 0 || comboBox_function.SelectedIndex == 2)
                LightValue = pMachine.m_CCDSystem.m_iLightValue[iChannelID];
            else
                LightValue = pMachine.m_CCDSystem.m_iLightCheckHeadZStop;
            //-------------------------
            comboBox_Head.Visible = (comboBox_function.SelectedIndex != 2) ? true : false;
            //-------------------------
            trackBar_Light.Value = LightValue;

            //------------ROI顯示部分---------------
            button_ShowROI.Enabled = false;
            button_CircleROI.Enabled = false;
            button_LineROI.Enabled = false;
            //----------
            #region Enable/Disable ROI Button

            cogRecordDisplay1.StaticGraphics.Clear();
            cogRecordDisplay1.InteractiveGraphics.Clear();
            tabControl1.SelectedIndex = 0;

            if (comboBox_function.SelectedIndex == 0)
            {
                button_ShowROI.Enabled = true;
                button_CircleROI.Enabled = true;
                button_LineROI.Enabled = false;
            }
            else if (comboBox_function.SelectedIndex == 1)
            {
                button_ShowROI.Enabled = true;
                button_CircleROI.Enabled = true;
                button_LineROI.Enabled = true;
            }
            else {
                button_ShowROI.Enabled = true;
                button_CircleROI.Enabled = false;
                button_LineROI.Enabled = false;
            }
            #endregion

        }

        private void trackBar_Light_ValueChanged(object sender, EventArgs e)
        {
            int iLightValue = trackBar_Light.Value;
            int iChannelID = (int)clsCCDSystem.enLightControlBox.en_Channel6;  // (comboBox_function.SelectedIndex + 6 );
            pMachine.m_CCDSystem.m_LightControlBox.SetIntensity(iChannelID, iLightValue);
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
            int iChannelID = (int)clsCCDSystem.enLightControlBox.en_Channel6;   //(comboBox_function.SelectedIndex + 6);
            if (comboBox_function.SelectedIndex == 0)
                pMachine.m_CCDSystem.m_iLightValue[iChannelID] = iLightValue;
            else
                pMachine.m_CCDSystem.m_iLightCheckHeadZStop = iLightValue;
            //-------------------------
            string strWorkPath = pMachine.GetWorkDataPath();
            pMachine.m_CCDSystem.SaveWorkData(strWorkPath);
        }

        private void btn_FunctionRun_Click(object sender, EventArgs e)
        {
            int iFunctionIndexBase = 0;
            int iFunctionIndex = 0;

            if (comboBox_function.SelectedIndex == 0)
                iFunctionIndexBase = (int)clsCCDSystem.enCCDFunction.en_NonZStop_Head1;
            else if (comboBox_function.SelectedIndex == 1)
                iFunctionIndexBase = (int)clsCCDSystem.enCCDFunction.en_ZStopCheck_Head1;
            else 
                iFunctionIndexBase = (int)clsCCDSystem.enCCDFunction.en_HeadRotCenter;

            //--------------------
            if (comboBox_function.SelectedIndex == 0 || comboBox_function.SelectedIndex == 1)
            {
                iFunctionIndex = iFunctionIndexBase + comboBox_Head.SelectedIndex;
                pMachine.m_CCDSystem.m_CCD2Function.enRetPoint = pMachine.m_CCDSystem.m_enZStopRetPoint[comboBox_Head.SelectedIndex];
            }
            else {
                iFunctionIndex = iFunctionIndexBase ;
            }
            //--------------------
            CogImage8Grey cogImage = (CogImage8Grey)(cogRecordDisplay1.Image);
            if (cogRecordDisplay1.Image != null)
                pMachine.m_CCDSystem.m_CCD2Function.RunTBFunction(ref cogImage, ref pMachine.m_CCDSystem.m_VisionTB[iFunctionIndex],
                    ref cogRecordDisplay1, (clsCCDSystem.enCCDFunction)iFunctionIndex);


        }

        private void comboBox_Head_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btn_SaveHeadCenter_Click(object sender, EventArgs e)
        {
            string strMachinePath = pMachine.GetMachineDataPath();
            //---------------------
            DialogResult dlgResult = MessageBox.Show(" 確認是否SAVE   [ 吸頭與CCD中心 ]  偏差值? ", "Save Dlg", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dlgResult != DialogResult.Yes)
                return;
            //--------------------------------------
            string strNoID, strNozzleID, strX, strY, strRotate;
            bool bUse;

                for (int i = 0; i < 6; i++)
                {
                    object obX = dataGridView_CalData[1, i].Value;
                    object obY = dataGridView_CalData[2, i].Value;
                    object obT = dataGridView_CalData[3, i].Value;
                    //-------------------
                    pMachine.m_PastingSt.m_tyNozzleToCenter[i].X = Convert.ToDouble(obX);
                    pMachine.m_PastingSt.m_tyNozzleToCenter[i].Y = Convert.ToDouble(obY);
                    pMachine.m_PastingSt.m_tyNozzleToCenter[i].T = Convert.ToDouble(obT);
                    //-------------------
                }
            //--------------------------------------
            pMachine.m_PastingSt.SaveMachineData(strMachinePath);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strMachinePath = pMachine.GetMachineDataPath();
            //---------------------
            DialogResult dlgResult = MessageBox.Show(" 確認是否SAVE   [ 旋轉中心 ]  值? ", "Save Dlg", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dlgResult != DialogResult.Yes)
                return;
            //--------------------------------------
            pMachine.m_PastingSt.SaveMachineData(strMachinePath);
        }

        private void button_ShowROI_Click(object sender, EventArgs e)
        {
            int iFunctionIndexBase = 0;
            int iFunctionIndex = 0;
            //---------------------
            if (comboBox_function.SelectedIndex == 0)
                iFunctionIndexBase = (int)clsCCDSystem.enCCDFunction.en_NonZStop_Head1;
            else if(comboBox_function.SelectedIndex == 1)
                iFunctionIndexBase = (int)clsCCDSystem.enCCDFunction.en_ZStopCheck_Head1;
            else
                iFunctionIndexBase = (int)clsCCDSystem.enCCDFunction.en_HeadRotCenter;
            //---------------------
            if (comboBox_function.SelectedIndex == 0 || comboBox_function.SelectedIndex == 1)
                iFunctionIndex = iFunctionIndexBase + comboBox_Head.SelectedIndex;
            else
                iFunctionIndex = iFunctionIndexBase;
            //---------------------
            cogRecordDisplay1.StaticGraphics.Clear();
            cogRecordDisplay1.InteractiveGraphics.Clear();

            if (comboBox_function.SelectedIndex == 0 )
            {
                #region 空吸嘴 PMAlign ROI
                //--------------------
                CogPMAlignTool pAlign1 = (CogPMAlignTool)pMachine.m_CCDSystem.m_VisionTB[iFunctionIndex].Tools["CogPMAlignTool1"];
                CogPMAlignTool pAlign2 = (CogPMAlignTool)pMachine.m_CCDSystem.m_VisionTB[iFunctionIndex].Tools["CogPMAlignTool2"];
                //--------------------
                CogRectangleAffine cogRectAffine1 = pAlign1.Pattern.TrainRegion as CogRectangleAffine;
                CogRectangleAffine cogRectAffine2 = pAlign2.Pattern.TrainRegion as CogRectangleAffine;

                cogRecordDisplay1.InteractiveGraphics.Add(cogRectAffine1, "cogRectAffine1", false);
                cogRecordDisplay1.InteractiveGraphics.Add(cogRectAffine2, "cogRectAffine2", false);
                //-------------
                CogCoordinateAxes goOriginAxis1 = new CogCoordinateAxes();
                CogTransform2DLinear goOrigin1 = pAlign1.Pattern.Origin;
                //------
                goOriginAxis1.Transform = goOrigin1;
                goOriginAxis1.Interactive = true;
                goOriginAxis1.GraphicDOFEnable = CogCoordinateAxesDOFConstants.Position;
                cogRecordDisplay1.InteractiveGraphics.Add(goOriginAxis1, "Origin1", false);


                CogCoordinateAxes goOriginAxis2 = new CogCoordinateAxes();
                CogTransform2DLinear goOrigin2 = pAlign2.Pattern.Origin;
                //------
                goOriginAxis2.Transform = goOrigin2;
                goOriginAxis2.Interactive = true;
                goOriginAxis2.GraphicDOFEnable = CogCoordinateAxesDOFConstants.Position;
                cogRecordDisplay1.InteractiveGraphics.Add(goOriginAxis2, "Origin2", false);

                #endregion
            } else if (comboBox_function.SelectedIndex == 1)
            {
                #region 有料Patten CNLSearch ROI
                //--------------------
                CogCNLSearchTool pCnlSearch1 = (CogCNLSearchTool)pMachine.m_CCDSystem.m_VisionTB[iFunctionIndex].Tools["CogCNLSearchTool1"];
                CogCNLSearchTool pCnlSearch2 = (CogCNLSearchTool)pMachine.m_CCDSystem.m_VisionTB[iFunctionIndex].Tools["CogCNLSearchTool2"];
                //--------------------
                CogRectangle cogRectAngle1 = pCnlSearch1.Pattern.TrainRegion as CogRectangle;
                CogRectangle cogRectAngle2 = pCnlSearch2.Pattern.TrainRegion as CogRectangle;

                cogRecordDisplay1.InteractiveGraphics.Add(cogRectAngle1, "cogRectAffine1", false);
                cogRecordDisplay1.InteractiveGraphics.Add(cogRectAngle2, "cogRectAffine2", false);
                //-------------
                CogCoordinateAxes goOriginAxis1 = new CogCoordinateAxes();
                //CogTransform2DLinear goCNLOrigin1 = new CogTransform2DLinear();
                goCNLOrigin1.TranslationX = pCnlSearch1.Pattern.OriginX;
                goCNLOrigin1.TranslationY = pCnlSearch1.Pattern.OriginY;
                //------
                goOriginAxis1.Transform = goCNLOrigin1;
                goOriginAxis1.Interactive = true;
                goOriginAxis1.GraphicDOFEnable = CogCoordinateAxesDOFConstants.Position;
                cogRecordDisplay1.InteractiveGraphics.Add(goOriginAxis1, "Origin1", false);



                CogCoordinateAxes goOriginAxis2 = new CogCoordinateAxes();
                //CogTransform2DLinear goCNLOrigin2 = new CogTransform2DLinear();
                goCNLOrigin2.TranslationX = pCnlSearch2.Pattern.OriginX;
                goCNLOrigin2.TranslationY = pCnlSearch2.Pattern.OriginY;
                //------
                goOriginAxis2.Transform = goCNLOrigin2;
                goOriginAxis2.Interactive = true;
                goOriginAxis2.GraphicDOFEnable = CogCoordinateAxesDOFConstants.Position;
                cogRecordDisplay1.InteractiveGraphics.Add(goOriginAxis2, "Origin2", false);

                #endregion
            }
            else {
                #region 吸嘴旋轉中心ROI設定
                //--------------------
                CogPMAlignTool pAlign1 = (CogPMAlignTool)pMachine.m_CCDSystem.m_VisionTB[iFunctionIndex].Tools["CogPMAlignTool1"];
                //--------------------
                //--------------------
                CogRectangleAffine cogRectAffine1 = pAlign1.Pattern.TrainRegion as CogRectangleAffine;

                cogRecordDisplay1.InteractiveGraphics.Add(cogRectAffine1, "cogRectAffine1", false);
                //-------------
                CogCoordinateAxes goOriginAxis1 = new CogCoordinateAxes();
                CogTransform2DLinear goOrigin1 = pAlign1.Pattern.Origin;
                //------
                goOriginAxis1.Transform = goOrigin1;
                goOriginAxis1.Interactive = true;
                goOriginAxis1.GraphicDOFEnable = CogCoordinateAxesDOFConstants.Position;
                cogRecordDisplay1.InteractiveGraphics.Add(goOriginAxis1, "Origin1", false);

                #endregion
            }


        }

        private void button_SaveROI_Click(object sender, EventArgs e)
        {
            int iFunctionIndexBase = 0;
            int iFunctionIndex = 0;
            //---------------------
            if (comboBox_function.SelectedIndex == 0)
                iFunctionIndexBase = (int)clsCCDSystem.enCCDFunction.en_NonZStop_Head1;
            else
                iFunctionIndexBase = (int)clsCCDSystem.enCCDFunction.en_ZStopCheck_Head1;
            //---------------------
            iFunctionIndex = iFunctionIndexBase + comboBox_Head.SelectedIndex;

            #region 載入VPP
            string strPath2 = pMachine.GetWorkDataPath().Replace(".xml", ""); ;
            string[] strFilePath = new string[(int)clsCCDSystem.enCCDFunction.en_Max];
            strFilePath[0] = strPath2 + "\\TP1_1.vpp";
            strFilePath[1] = strPath2 + "\\TP1_2.vpp";
            strFilePath[2] = strPath2 + "\\TP2_1.vpp";
            strFilePath[3] = strPath2 + "\\TP2_2.vpp";
            //----------
            strFilePath[4] = strPath2 + "\\NonZStop_Head1.vpp";
            strFilePath[5] = strPath2 + "\\NonZStop_Head2.vpp";
            strFilePath[6] = strPath2 + "\\NonZStop_Head3.vpp";
            strFilePath[7] = strPath2 + "\\NonZStop_Head4.vpp";
            strFilePath[8] = strPath2 + "\\NonZStop_Head5.vpp";
            strFilePath[9] = strPath2 + "\\NonZStop_Head6.vpp";
            //----------
            strFilePath[10] = strPath2 + "\\ZStopCheck_Head1.vpp";
            strFilePath[11] = strPath2 + "\\ZStopCheck_Head2.vpp";
            strFilePath[12] = strPath2 + "\\ZStopCheck_Head3.vpp";
            strFilePath[13] = strPath2 + "\\ZStopCheck_Head4.vpp";
            strFilePath[14] = strPath2 + "\\ZStopCheck_Head5.vpp";
            strFilePath[15] = strPath2 + "\\ZStopCheck_Head6.vpp";
            //----------
            strFilePath[16] = strPath2 + "\\ZStopSearch_6Head.vpp";
            strFilePath[17] = strPath2 + "\\ZStopSearch_1Head.vpp";
            //-----------
            strFilePath[18] = strPath2 + "\\HeadRotCenter.vpp";
            //---------------------
            #endregion     

            string strVppfilePath = strFilePath[iFunctionIndex];
            CogSerializer.SaveObjectToFile(pMachine.m_CCDSystem.m_VisionTB[iFunctionIndex], strVppfilePath, typeof(System.Runtime.Serialization.Formatters.Binary.BinaryFormatter), CogSerializationOptionsConstants.All);

            cogRecordDisplay1.StaticGraphics.Clear();
            cogRecordDisplay1.InteractiveGraphics.Clear();
        }

        private void button_TrainPatten_Click(object sender, EventArgs e)
        {
            int iFunctionIndexBase = 0;
            int iFunctionIndex = 0;
            //---------------------
            if (comboBox_function.SelectedIndex == 0)
                iFunctionIndexBase = (int)clsCCDSystem.enCCDFunction.en_NonZStop_Head1;
            else if (comboBox_function.SelectedIndex == 1)
                iFunctionIndexBase = (int)clsCCDSystem.enCCDFunction.en_ZStopCheck_Head1;
            else
                iFunctionIndexBase = (int)clsCCDSystem.enCCDFunction.en_HeadRotCenter;
            //---------------------

            if (comboBox_function.SelectedIndex == 0 || comboBox_function.SelectedIndex == 1)
                iFunctionIndex = iFunctionIndexBase + comboBox_Head.SelectedIndex;
            else
                iFunctionIndex = iFunctionIndexBase;
            //---------------------


            if (comboBox_function.SelectedIndex == 0)
            {
                CogPMAlignTool pAlign1 = (CogPMAlignTool)pMachine.m_CCDSystem.m_VisionTB[iFunctionIndex].Tools["CogPMAlignTool1"];
                pAlign1.InputImage = cogRecordDisplay1.Image;
                pAlign1.Pattern.TrainImage = cogRecordDisplay1.Image;
                pAlign1.Pattern.Train();
                //---------------------

                CogPMAlignTool pAlign2 = (CogPMAlignTool)pMachine.m_CCDSystem.m_VisionTB[iFunctionIndex].Tools["CogPMAlignTool2"];
                pAlign2.InputImage = cogRecordDisplay1.Image;
                pAlign2.Pattern.TrainImage = cogRecordDisplay1.Image;
                pAlign2.Pattern.Train();
            }
            else if (comboBox_function.SelectedIndex == 1)
            {
                CogCNLSearchTool pCnlSearch1 = (CogCNLSearchTool)pMachine.m_CCDSystem.m_VisionTB[iFunctionIndex].Tools["CogCNLSearchTool1"];
                CogCoordinateAxes goOriginAxis1 = new CogCoordinateAxes();

                pCnlSearch1.Pattern.OriginX = goCNLOrigin1.TranslationX;
                pCnlSearch1.Pattern.OriginY = goCNLOrigin1.TranslationY;

                pCnlSearch1.InputImage = (CogImage8Grey)cogRecordDisplay1.Image;
                pCnlSearch1.Pattern.TrainImage = (CogImage8Grey)cogRecordDisplay1.Image;
                pCnlSearch1.Pattern.Train();
                //---------------------
                CogCNLSearchTool pCnlSearch2 = (CogCNLSearchTool)pMachine.m_CCDSystem.m_VisionTB[iFunctionIndex].Tools["CogCNLSearchTool2"];

                pCnlSearch2.Pattern.OriginX = goCNLOrigin2.TranslationX;
                pCnlSearch2.Pattern.OriginY = goCNLOrigin2.TranslationY;

                pCnlSearch2.InputImage = (CogImage8Grey)cogRecordDisplay1.Image;
                pCnlSearch2.Pattern.TrainImage = (CogImage8Grey)cogRecordDisplay1.Image;
                pCnlSearch2.Pattern.Train();

            }
            else
            {
                CogPMAlignTool pAlign1 = (CogPMAlignTool)pMachine.m_CCDSystem.m_VisionTB[iFunctionIndex].Tools["CogPMAlignTool1"];
                pAlign1.InputImage = cogRecordDisplay1.Image;
                pAlign1.Pattern.TrainImage = cogRecordDisplay1.Image;
                pAlign1.Pattern.Train();
                //---------------------
            }



        }

        private void button_TBRunAndShowResult_Click(object sender, EventArgs e)
        {
            int iFunctionIndexBase = 0;
            int iFunctionIndex = 0;
            //---------------------
            if (comboBox_function.SelectedIndex == 0)
                iFunctionIndexBase = (int)clsCCDSystem.enCCDFunction.en_NonZStop_Head1;
            else if (comboBox_function.SelectedIndex == 1)
                iFunctionIndexBase = (int)clsCCDSystem.enCCDFunction.en_ZStopCheck_Head1;
            else
                iFunctionIndexBase = (int)clsCCDSystem.enCCDFunction.en_HeadRotCenter;
            //---------------------
            if (comboBox_function.SelectedIndex == 0 || comboBox_function.SelectedIndex == 1)
                iFunctionIndex = iFunctionIndexBase + comboBox_Head.SelectedIndex;
            else
                iFunctionIndex = iFunctionIndexBase;
            //---------------------
            cogRecordDisplay1.StaticGraphics.Clear();
            cogRecordDisplay1.InteractiveGraphics.Clear();
            pMachine.m_CCDSystem.m_VisionTB[iFunctionIndex].Inputs[0].Value =(cogRecordDisplay1.Image);

            pMachine.m_CCDSystem.m_VisionTB[iFunctionIndex].Run();

            cogRecordDisplay1.Record = pMachine.m_CCDSystem.m_VisionTB[iFunctionIndex].CreateLastRunRecord();
        }

        private void button_CircleROI_Click(object sender, EventArgs e)
        {
            int iFunctionIndexBase = 0;
            int iFunctionIndex = 0;
            //---------------------
            if (comboBox_function.SelectedIndex == 0)
                iFunctionIndexBase = (int)clsCCDSystem.enCCDFunction.en_NonZStop_Head1;
            else if (comboBox_function.SelectedIndex == 1)
                iFunctionIndexBase = (int)clsCCDSystem.enCCDFunction.en_ZStopCheck_Head1;
            else
                iFunctionIndexBase = (int)clsCCDSystem.enCCDFunction.en_HeadRotCenter;
            //---------------------
            if (comboBox_function.SelectedIndex == 0 || comboBox_function.SelectedIndex == 1)
                iFunctionIndex = iFunctionIndexBase + comboBox_Head.SelectedIndex;
            else
                iFunctionIndex = iFunctionIndexBase;
            //---------------------
            cogRecordDisplay1.StaticGraphics.Clear();
            cogRecordDisplay1.InteractiveGraphics.Clear();

            if (comboBox_function.SelectedIndex == 0 || comboBox_function.SelectedIndex == 1)
            {
                #region 空吸嘴搜尋
                CogFindCircleTool pCircle1 = (CogFindCircleTool)pMachine.m_CCDSystem.m_VisionTB[iFunctionIndex].Tools["CogFindCircleTool1"];
                CogFindCircleTool pCircle2 = (CogFindCircleTool)pMachine.m_CCDSystem.m_VisionTB[iFunctionIndex].Tools["CogFindCircleTool2"];
                //--------------------
                  //--------------------
                ICogRecord CircleRec1, CircleRec2;
                CogGraphicCollection CircleRegions1, CircleRegions2;
                CogCircularArc CircleSeg1, CircleSeg2;
                //--------------------
                #region Show Circle1
                //Obtain the current record, then use the record keys to obtain the graphics
                CircleRec1 = pCircle1.CreateCurrentRecord();
                CircleRegions1 = (CogGraphicCollection)CircleRec1.SubRecords["InputImage"].SubRecords["CaliperRegions"].Content;
                CircleSeg1 = (CogCircularArc)CircleRec1.SubRecords["InputImage"].SubRecords["ExpectedShapeSegment"].Content;
                CircleSeg1.GraphicDOFEnable = CogCircularArcDOFConstants.Bend | CogCircularArcDOFConstants.Position;

                //The expected CircularArc which is used to position the calipers for finding the Circle.
                cogRecordDisplay1.InteractiveGraphics.Add(CircleSeg1, "", false);

                if (CircleRegions1 != null)
                {
                    foreach (ICogGraphic g in CircleRegions1)
                    {
                        if (g != null)
                            cogRecordDisplay1.InteractiveGraphics.Add((ICogGraphicInteractive)g, "", false);
                    }
                }

                #endregion
                //--------------------
                #region Show Circle2
                //Obtain the current record, then use the record keys to obtain the graphics
                CircleRec2 = pCircle2.CreateCurrentRecord();
                CircleRegions2 = (CogGraphicCollection)CircleRec2.SubRecords["InputImage"].SubRecords["CaliperRegions"].Content;
                CircleSeg2 = (CogCircularArc)CircleRec2.SubRecords["InputImage"].SubRecords["ExpectedShapeSegment"].Content;
                CircleSeg2.GraphicDOFEnable = CogCircularArcDOFConstants.Bend | CogCircularArcDOFConstants.Position;

                //The expected CircularArc which is used to position the calipers for finding the Circle.
                cogRecordDisplay1.InteractiveGraphics.Add(CircleSeg2, "", false);

                if (CircleRegions2 != null)
                {
                    foreach (ICogGraphic g in CircleRegions2)
                    {
                        if (g != null)
                            cogRecordDisplay1.InteractiveGraphics.Add((ICogGraphicInteractive)g, "", false);
                    }
                }

                #endregion
                //--------------------
                 #endregion
            }

        }

        private void button_LineROI_Click(object sender, EventArgs e)
        {
            int iFunctionIndexBase = 0;
            int iFunctionIndex = 0;
            //---------------------
            if (comboBox_function.SelectedIndex == 0)
                iFunctionIndexBase = (int)clsCCDSystem.enCCDFunction.en_NonZStop_Head1;
            else if (comboBox_function.SelectedIndex == 1)
                iFunctionIndexBase = (int)clsCCDSystem.enCCDFunction.en_ZStopCheck_Head1;
            else
                iFunctionIndexBase = (int)clsCCDSystem.enCCDFunction.en_HeadRotCenter;
            //---------------------
            if (comboBox_function.SelectedIndex == 0 || comboBox_function.SelectedIndex == 1)
                iFunctionIndex = iFunctionIndexBase + comboBox_Head.SelectedIndex;
            else
                iFunctionIndex = iFunctionIndexBase;
            //---------------------
            cogRecordDisplay1.StaticGraphics.Clear();
            cogRecordDisplay1.InteractiveGraphics.Clear();

            if (comboBox_function.SelectedIndex == 1)
            {
                #region Draw FindLine ROI
                //CogFindLineTool pLine1 = (CogFindLineTool)pMachine.m_CCDSystem.m_VisionTB[iFunctionIndex].Tools["CogFindLineTool1"];
                CogFindLineTool pLine2 = (CogFindLineTool)pMachine.m_CCDSystem.m_VisionTB[iFunctionIndex].Tools["CogFindLineTool2"];

                //ICogRecord LineRec1 = pLine1.CreateCurrentRecord();
                ICogRecord LineRec2 = pLine2.CreateCurrentRecord();
                //--------------------------

                // 畫線
                //CogLineSegment LineSeg1 = (CogLineSegment)LineRec1.SubRecords["InputImage"].SubRecords["ExpectedShapeSegment"].Content;
                CogLineSegment LineSeg2 = (CogLineSegment)LineRec2.SubRecords["InputImage"].SubRecords["ExpectedShapeSegment"].Content;

                // 畫Calipers
                //CogGraphicCollection LineRegions1 = (CogGraphicCollection)LineRec1.SubRecords["InputImage"].SubRecords["CaliperRegions"].Content;
                CogGraphicCollection LineRegions2 = (CogGraphicCollection)LineRec2.SubRecords["InputImage"].SubRecords["CaliperRegions"].Content;

                //cogRecordDisplay1.InteractiveGraphics.Add(LineSeg1, "", false);
                cogRecordDisplay1.InteractiveGraphics.Add(LineSeg2, "", false);

                //if (LineRegions1 != null)
                //    foreach (ICogGraphic g in LineRegions1)
                //        cogRecordDisplay1.InteractiveGraphics.Add((ICogGraphicInteractive)g, "", false);

                if (LineRegions2 != null)
                    foreach (ICogGraphic g in LineRegions2)
                        cogRecordDisplay1.InteractiveGraphics.Add((ICogGraphicInteractive)g, "", false);
                #endregion
            }
        }

        private void tabControl1_TabIndexChanged(object sender, EventArgs e)
        {
            cogRecordDisplay1.StaticGraphics.Clear();
            cogRecordDisplay1.InteractiveGraphics.Clear();          
        }

        private void btn_Snap_Click(object sender, EventArgs e)
        {
            if (pCCD.GetisFindCCDAndInitial())
                pCCD.OneShot(ref cogRecordDisplay1);
            else
            {
                OpenFileDialog file = new OpenFileDialog();
                file.ShowDialog();
                String selectBmpFile = file.FileName; // file.SafeFileName;
                //---------------------
                if (selectBmpFile == "")
                {
                    MessageBox.Show(this, "未選取BMP檔案 , 請重新確認 !!");
                    return;
                }
                //---------------------
                Bitmap pImage = new Bitmap(selectBmpFile);
                CogImage8Grey cogImg = new CogImage8Grey(pImage);
                cogRecordDisplay1.Image = cogImg;
            }
        }



    }
}
