using System;
using System.IO;
using System.Collections;
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
using ToolTotal;

namespace Cowain_Form.FormView
{
    public partial class frm_Recipe : Form
    {
        Dictionary<string, Image> images = new Dictionary<string, Image>();
        public frm_Recipe(ref Machine pM)
        {
            InitializeComponent();
            pMachine = pM;
            String strPath = System.IO.Directory.GetCurrentDirectory();
            String strNowPath = strPath.Replace("\\Cowain_AutoDispenser\\bin\\x64\\Debug", "");
            String strBasePath = strNowPath + "\\DataBaseData\\Configuration.ini";
            myIniFile = new IniFile(strBasePath);//初始化配置文件位置 

            //更新StationNo
            StationNo.Items.Clear();
            MSystemParameter.GantryParm pGantryParm1 = MSystemParameter.m_SysParm.Gantry1Parm;
            MSystemParameter.GantryParm pGantryParm2 = MSystemParameter.m_SysParm.Gantry2Parm;
            for (int i = 0; i < pGantryParm1.strLStTakePictureOrder.Length; i++)
            {
                StationNo.Items.Add("1_" + (i + 1).ToString());
            }
            for (int i = 0; i < pGantryParm1.strRStReTakePictureOrder.Length; i++)
            {
                StationNo.Items.Add("2_" + (i + 1).ToString());
            }
            for (int i = 0; i < pGantryParm2.strLStTakePictureOrder.Length; i++)
            {
                StationNo.Items.Add("3_" + (i + 1).ToString());
            }
            for (int i = 0; i < pGantryParm2.strRStReTakePictureOrder.Length; i++)
            {
                StationNo.Items.Add("4_" + (i + 1).ToString());
            }
        }
        IniFile myIniFile;
        MDataDefine define = new MDataDefine();
        public Machine pMachine;
        int m_iDisStID = 0;
        ImageList ImgList = new ImageList();

        private void frm_Recipe_Shown(object sender, EventArgs e)
        {
            ImgList.Images.Add("SelectRecipe", Cowain_AutoDispenser.Properties.Resources.SelectRecipe);
            ImgList.Images.Add("DefaultRecipe", Cowain_AutoDispenser.Properties.Resources.DefaultRecipe);


            #region  加入啟用之Station

            m_iDisStID = 0;
            combo_StationID.Items.Clear();
            clsDispenserAuto pDis = pMachine.m_DispenserAuto;
            for (int i = 0; i < pDis.m_DispenserSt.Length; i++)
            {
                if (pDis.m_DispenserSt[i].GetStationEnable())
                {
                    string strStation = "Station_" + (i + 1).ToString();
                    combo_StationID.Items.Add(strStation);
                }
            }
            combo_StationID.SelectedIndex = m_iDisStID;
            label_DispenserID.Text = "St ID: " + (m_iDisStID + 1).ToString();
            #endregion

        }
        private void frm_Recipe_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                Fun_DisplayData();
                ////------------------
                btn_Add.Visible = false;
                btn_Load.Visible = false;
                btn_Save.Visible = false;
                //------------------
                dataGridView_PathData.Rows.Clear();
                label_PathName.Text = "";
                //------------------
                if (pMachine.m_LoginUser == MotionBase.Sys_Define.enPasswordType.Eng)
                {
                    btn_Save.Visible = true;
                }
                else if (pMachine.m_LoginUser == MotionBase.Sys_Define.enPasswordType.ItEng ||
                         pMachine.m_LoginUser == MotionBase.Sys_Define.enPasswordType.MacEng ||
                         pMachine.m_LoginUser == MotionBase.Sys_Define.enPasswordType.Maker)
                {
                    btn_Add.Visible = true;
                    btn_Load.Visible = true;
                    btn_Save.Visible = true;
                }
                ////-----------------------------------
                bool bisMaker = (pMachine.m_LoginUser == MotionBase.Sys_Define.enPasswordType.Eng ||
                                 pMachine.m_LoginUser == MotionBase.Sys_Define.enPasswordType.ItEng ||
                                 pMachine.m_LoginUser == MotionBase.Sys_Define.enPasswordType.Maker ||
                                 pMachine.m_LoginUser == MotionBase.Sys_Define.enPasswordType.MacEng);

                gBoxEng.Visible = bisMaker;

            }

        }

        public void Fun_DisplayData()
        {
            //************************Show Recipe List ************************
            Fun_DisplayRecipeList();

            //************************Show Machine Parm ************************
            #region Show Machine Parm

            //-------
            numericUpDown_AutoSpeed.Value = Convert.ToInt16(pMachine.m_dbAutoingSpeed);
            cBoxNoCCD.Checked = pMachine.m_bTestMode;
            checkBox_CheckDoorSR.Checked = pMachine.m_bCheckDoorSR;
            checkBox_NoScan.Checked = MDataDefine.m_bScanMode;
            checkBox_NoMES.Checked = MDataDefine.m_bMESMode;
            checkBoxTestMode.Checked = pMachine.m_checkTestMode;
            cBoxAutoDis.Checked = MDataDefine.m_AutoDis;
            cBoxNullRun.Checked = MDataDefine.b_NullRun;
            cBoxNoCloth.Checked = MDataDefine.b_NoCloth;

            String strPath = System.IO.Directory.GetCurrentDirectory();
            String strNowPath = strPath.Replace("\\Cowain_AutoDispenser\\bin\\x64\\Debug", "");
            String pictPath = strNowPath + "\\DataBaseData\\Picture";
            string[] picts = Directory.GetFiles(pictPath);
            images.Clear();
            for (int i = 0; i < picts.Length; i++)
            {
                using (FileStream rs = new FileStream(picts[i], FileMode.Open, FileAccess.Read))
                {
                    Image start = Image.FromStream(rs);
                    string[] name1 = picts[i].Split('\\');
                    string[] names = name1[name1.Length - 1].Split('.');
                    if (images.Keys.Contains(names[0]) != true)
                    {
                        images.Add(names[0], start);
                    }
                }
            }
            foreach (KeyValuePair<string, Image> item in images)
            {
                cBoxPicture.Items.Add(item.Key);
            }
            if (cBoxPicture.Items.Contains(MDataDefine.stationName))
            {
                cBoxPicture.Text = MDataDefine.stationName;
            }
            else
            {
                cBoxPicture.Text = "";
            }


            //------
            #endregion
            //************************Show Dispenser Data **********************
            Fun_ShowDispenserData();
        }

        public void Fun_ShowDispenserData()
        {

            //************************Show Dispenser Data *************************
            #region Show DispenserData          
            clsDispenserSt pDisSt = pMachine.m_DispenserAuto.m_DispenserSt[m_iDisStID];
            int iArrayNum = pDisSt.m_DispenserList.iArrayNum;
            numericUpDown_PathNum.Value = iArrayNum;
            //-------------------
            label_DispenserID.Text = "St ID: " + (m_iDisStID + 1).ToString();
            //-------------------
            dataGridView_DispenserData.Rows.Clear();
            string strNoID, strX, strY, strZ, strR, strA, strPathName;
            bool bUseVisionOfx, bUse;
            double bDispenserSpeed;//点胶分段速度
            for (int i = 0; i < iArrayNum; i++)
            {
                strNoID = (i + 1).ToString();
                strX = pDisSt.m_DispenserList.DispenserData[i].tyTartgetPos.X.ToString();
                strY = pDisSt.m_DispenserList.DispenserData[i].tyTartgetPos.Y.ToString();
                strZ = pDisSt.m_DispenserList.DispenserData[i].tyTartgetPos.Z.ToString();
                strR = pDisSt.m_DispenserList.DispenserData[i].tyTartgetPos.R.ToString();
                strA = pDisSt.m_DispenserList.DispenserData[i].tyTartgetPos.A.ToString();

                bDispenserSpeed = pDisSt.m_DispenserList.DispenserData[i].m_DispenserSpeed;//点胶分段速度
                strPathName = pDisSt.m_DispenserList.DispenserData[i].strPathName;
                //-----
                bUseVisionOfx = pDisSt.m_DispenserList.DispenserData[i].m_bUseVisionOfx;
                bUse = pDisSt.m_DispenserList.DispenserData[i].m_bUse;

                if (pDisSt.m_Dispenser.m_enAxisType == Sys_Define.enAixsType.en_XYZ)
                {
                    dataGridView_DispenserData.Columns[4].Visible = false;
                    dataGridView_DispenserData.Columns[5].Visible = false;
                    dataGridView_DispenserData.Rows.Add(new object[] { strNoID, strX, strY, strZ, "-", "-", bDispenserSpeed, bUse, bUseVisionOfx, "", strPathName });
                    int[] iReadOnlyID = { 4, 5 };
                    for (int k = 0; k < iReadOnlyID.Length; k++)
                    {
                        dataGridView_DispenserData[iReadOnlyID[k], i].ReadOnly = true;
                        dataGridView_DispenserData[iReadOnlyID[k], i].Style.BackColor = Color.Pink;
                    }
                }
                else if (pDisSt.m_Dispenser.m_enAxisType == Sys_Define.enAixsType.en_XYZR)
                {
                    dataGridView_DispenserData.Columns[4].Visible = true;
                    dataGridView_DispenserData.Columns[5].Visible = false;

                    //dataGridView_DispenserData.Columns[4].HeaderText = "R";
                    //dataGridView_DispenserData.Columns[4].Width = 50;
                    //dataGridView_DispenserData.Columns[5].HeaderText = "";
                    //dataGridView_DispenserData.Columns[5].Width = 1;

                    dataGridView_DispenserData.Rows.Add(new object[] { strNoID, strX, strY, strZ, strR, "-", bDispenserSpeed, bUse, bUseVisionOfx, "", strPathName });
                    dataGridView_DispenserData[5, i].ReadOnly = true;
                    dataGridView_DispenserData[5, i].Style.BackColor = Color.Pink;
                }
                else if (pDisSt.m_Dispenser.m_enAxisType == Sys_Define.enAixsType.en_XYZA)
                {
                    dataGridView_DispenserData.Columns[4].Visible = false;
                    dataGridView_DispenserData.Columns[5].Visible = true;

                    dataGridView_DispenserData.Rows.Add(new object[] { strNoID, strX, strY, strZ, "-", strA, bDispenserSpeed, bUse, bUseVisionOfx, "", strPathName });
                    dataGridView_DispenserData[4, i].ReadOnly = true;
                    dataGridView_DispenserData[4, i].Style.BackColor = Color.Pink;
                }
                else
                {
                    dataGridView_DispenserData.Columns[4].Visible = true;
                    dataGridView_DispenserData.Columns[5].Visible = true;
                    dataGridView_DispenserData.Rows.Add(new object[] { strNoID, strX, strY, strZ, strR, strA, bDispenserSpeed, bUse, bUseVisionOfx, "", strPathName });
                }
            }

            #endregion
        }
        private void Fun_DisplayRecipeList()
        {
            string strDirectory = pMachine.GetWorkDataDirectory();
            string strFilePath = pMachine.GetWorkDataPath();
            string strShowName = strFilePath.Replace(strDirectory, "");
            strShowName = strShowName.Replace(".xml", "");
            label_PathName.Text = "Recipe Name:  " + strShowName;

            ArrayList MyFile = new ArrayList();
            DirectoryInfo directory = new DirectoryInfo(strDirectory);
            foreach (FileInfo fi in directory.GetFiles("*.xml"))
                MyFile.Add(fi.FullName);

            listView_Recipe.Items.Clear();
            listView_Recipe.LargeImageList = ImgList;
            listView_Recipe.SmallImageList = ImgList;
            int FileCount = MyFile.Count;
            for (int i = 0; i < FileCount; i++)
            {
                String DirectoryFileName = MyFile[i].ToString();
                String FileName = DirectoryFileName.Replace(".xml", "");
                FileName = FileName.Replace(strDirectory, "");
                if (strShowName == FileName)
                    listView_Recipe.Items.Add(FileName, "SelectRecipe");
                else
                    listView_Recipe.Items.Add(FileName, "DefaultRecipe");
            }
            txtXStepLength.Text = MDataDefine.CalibrationStepLengths[0].ToString();
            txtYStepLength.Text = MDataDefine.CalibrationStepLengths[1].ToString();
            txtDispenserWidth1.Text = MDataDefine.DispenserWidths[0].ToString();
            txtDispenserWidth2.Text = MDataDefine.DispenserWidths[1].ToString();
            cBoxVisionPoint.Checked = MDataDefine.m_MovePointWithHIK;
            txtOutGlueTime1.Text = MDataDefine.DisTime[0].ToString();
            txtOutGlueTime2.Text = MDataDefine.DisTime[1].ToString();
            txtAutoDelayTime1.Text = MDataDefine.GoToTime[0].ToString();
            txtAutoDelayTime2.Text = MDataDefine.GoToTime[1].ToString();

            tabControl1.TabPages.Clear();
            //加入权限管理
            if ((int)MDataDefine.m_LoginUser >= 1)//==Sys_Define.enPasswordType.Operator)
            {
                groupBox3.Visible = true;
                tabControl1.TabPages.Add(tabPage0);
            }
            if ((int)MDataDefine.m_LoginUser >= 2) //== Sys_Define.enPasswordType.Eng)
            {
                group_TestParm.Visible = true;
                gBoxEng.Visible = true;
                groupBox6.Visible = true;
                group_Pick1.Visible = true;
                group_Pick2.Visible = true;
                groupBox1.Visible = true;
                tabControl1.TabPages.Add(tabPage1);
                tabControl1.TabPages.Add(tabPage2);
            }
            if ((int)MDataDefine.m_LoginUser >= 4) //== Sys_Define.enPasswordType.MacEng)
            {
                cBoxVisionPoint.Visible = true;
                tabControl1.TabPages.Add(tabPage3);
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            string strFilePath = pMachine.GetWorkDataPath();

            DialogResult dlgResult = MessageBox.Show(" 確認是否儲存檔案? ", "Save Dlg", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dlgResult != DialogResult.Yes)
                return;
            try
            {
                //************************Save Machine Parm ************************
                #region Save Machine Parm
                //-------
                pMachine.m_dbAutoingSpeed = Convert.ToDouble(numericUpDown_AutoSpeed.Value);
                pMachine.m_bTestMode = cBoxNoCCD.Checked;
                MDataDefine.m_bTestMode = cBoxNoCCD.Checked;
                MDataDefine.m_bScanMode = checkBox_NoScan.Checked;
                MDataDefine.m_bMESMode = checkBox_NoMES.Checked;
                pMachine.m_bCheckDoorSR = checkBox_CheckDoorSR.Checked;
                MDataDefine.m_AutoDis = cBoxAutoDis.Checked;
                pMachine.m_checkTestMode = checkBoxTestMode.Checked;
                MDataDefine.m_CheckTestMode = checkBoxTestMode.Checked;
                MDataDefine.b_NullRun = cBoxNullRun.Checked;
                MDataDefine.b_NoCloth = cBoxNoCloth.Checked;
                MDataDefine.m_MovePointWithHIK = cBoxVisionPoint.Checked;
                //------
                define.WriteScan(MDataDefine.m_bScanMode);
                define.WriteMES(MDataDefine.m_bMESMode);
                pMachine.SaveWorkData(strFilePath);
                define.WriteAutoDis(MDataDefine.m_AutoDis);
                MDataDefine.CalibrationStepLengths[0] = Convert.ToDouble(txtXStepLength.Text);
                MDataDefine.CalibrationStepLengths[1] = Convert.ToDouble(txtYStepLength.Text);
                myIniFile.IniWriteValue("config", "CalibrationXStepLength", txtXStepLength.Text);
                myIniFile.IniWriteValue("config", "CalibrationYStepLength", txtYStepLength.Text);
                myIniFile.IniWriteValue("config", "DispenserWidth1", txtDispenserWidth1.Text);
                myIniFile.IniWriteValue("config", "DispenserWidth2", txtDispenserWidth2.Text);
                MDataDefine.DispenserWidths[0] = Convert.ToDouble(txtDispenserWidth1.Text);
                MDataDefine.DispenserWidths[1] = Convert.ToDouble(txtDispenserWidth2.Text);
                MDataDefine.stationName = cBoxPicture.Text;
                define.WriteStationName(cBoxPicture.Text);
                MDataDefine.DisTime[0] = Convert.ToDouble(txtOutGlueTime1.Text);
                MDataDefine.DisTime[1] = Convert.ToDouble(txtOutGlueTime2.Text);
                MDataDefine.GoToTime[0] = Convert.ToDouble(txtAutoDelayTime1.Text);
                MDataDefine.GoToTime[1] = Convert.ToDouble(txtAutoDelayTime2.Text);
                myIniFile.IniWriteValue("config", "DisOut1", txtOutGlueTime1.Text);
                myIniFile.IniWriteValue("config", "DisOut2", txtOutGlueTime2.Text);
                myIniFile.IniWriteValue("config", "DisDelay1", txtAutoDelayTime1.Text);
                myIniFile.IniWriteValue("config", "DisDelay2", txtAutoDelayTime2.Text);
                #endregion
                //******************************************************************
                //pMachine.LoadWorkData(strFilePath);  //重新LoadWorkData
            }
            catch
            {
                DialogResult Errdlg = MessageBox.Show(" 資料內容格式錯誤 ,請確認修改過之資料數值!! ");
                return;
            }
        }

        private void btn_Load_Click(object sender, EventArgs e)
        {

            //if (listView_Recipe.SelectedItems.Count <= 0)
            //{
            //    MessageBox.Show(this, "請先選擇欲設置之Recipe ID");
            //    return;
            //}



            string strInitialDirectory = pMachine.GetWorkDataDirectory();
            OpenFileDialog file = new OpenFileDialog();
            file.InitialDirectory = strInitialDirectory;
            file.ShowDialog();
            String selectRecipe = file.SafeFileName;


            String strNowRecipe = label_PathName.Text.Replace("Recipe Name:  ", "");
            String strShowRecipe = selectRecipe.Replace(".xml", "");
            if (strShowRecipe == "")
            {
                MessageBox.Show(this, "未選取Recipe , 請重新確認 !!");
                return;
            }

            if (strNowRecipe == strShowRecipe)
            {
                MessageBox.Show(this, "與目前使用之Recipe相同 , 請重新確認 !!");
                return;
            }



            DialogResult dlgResult = MessageBox.Show(" 確認是否Load    [" + selectRecipe + "]    檔案 ? ", "Load Dlg", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dlgResult != DialogResult.Yes)
                return;


            //String selectRecipe = listView_Recipe.SelectedItems[0].Text;
            //if (label_RecipeName.Text == selectRecipe)
            //    return;

            //String strNewRecipeName = strWorkPath + selectRecipe + ".xml";

            String strWorkPath = pMachine.GetWorkDataDirectory();
            String strNewRecipeName = strWorkPath + selectRecipe;

            bool bSaveOk = pMachine.SaveWorkPath(strNewRecipeName);
            if (bSaveOk)
            {
                pMachine.LoadWorkData(strNewRecipeName);
                Fun_DisplayData();
                Fun_DisplayRecipeList();
            }
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            if (listView_Recipe.SelectedItems.Count <= 0)
            {
                MessageBox.Show(this, "請先選擇欲複製之Recipe ID");
                return;
            }

            #region 跳出新增Recipe_Frm
            dia_Recipe f = new dia_Recipe();
            f.ShowDialog();
            string strNewRecipeID = f.RecipeID;
            f.Close();
            if (strNewRecipeID == "")
            {
                MessageBox.Show(this, "新ID為空值，Recipe另存新檔失敗！");
                return;
            }
            for (int i = 0; i < listView_Recipe.Items.Count; i++)
            {
                if (listView_Recipe.Items[i].Text == strNewRecipeID)
                {
                    MessageBox.Show(this, "新ID資料已經存在，Recipe另存新檔失敗！");
                    return;
                }
            }
            #endregion
            //------------
            #region 複製檔案與目錄
            String strSelectName = listView_Recipe.SelectedItems[0].Text;
            String strWorkPath = pMachine.GetWorkDataDirectory();
            String SourceFileName = strWorkPath + strSelectName + ".xml";
            String SourcePath = strWorkPath + strSelectName;
            String TargetFileName = strWorkPath + strNewRecipeID + ".xml";
            String TargetPath = strWorkPath + strNewRecipeID;
            //----------------------
            System.IO.File.Copy(SourceFileName, TargetFileName);
            //------------

            #endregion
            //**********************************************************
            Fun_DisplayRecipeList();
        }






        private void button1_Click_1(object sender, EventArgs e)
        {
            clsDispenserSt pDisSt = pMachine.m_DispenserAuto.m_DispenserSt[m_iDisStID];
            int iArrayNum = Convert.ToInt16(numericUpDown_PathNum.Value);


            dataGridView_DispenserData.Rows.Clear();
            string strNoID, strX, strY, strZ, strR, strA, strPathName;
            bool bUseVisionOfx, bUse;
            double bDispenserSpeed = 0;

            for (int i = 0; i < iArrayNum; i++)
            {
                strNoID = (i + 1).ToString();
                strX = pDisSt.m_DispenserList.DispenserData[i].tyTartgetPos.X.ToString();
                strY = pDisSt.m_DispenserList.DispenserData[i].tyTartgetPos.Y.ToString();
                strZ = pDisSt.m_DispenserList.DispenserData[i].tyTartgetPos.Z.ToString();
                strR = pDisSt.m_DispenserList.DispenserData[i].tyTartgetPos.R.ToString();
                strA = pDisSt.m_DispenserList.DispenserData[i].tyTartgetPos.A.ToString();
                strPathName = pDisSt.m_DispenserList.DispenserData[i].strPathName;
                bDispenserSpeed = pDisSt.m_DispenserList.DispenserData[i].m_DispenserSpeed;//分段速度百分比
                //-----
                bUseVisionOfx = pDisSt.m_DispenserList.DispenserData[i].m_bUseVisionOfx;
                bUse = pDisSt.m_DispenserList.DispenserData[i].m_bUse;

                dataGridView_DispenserData.Rows.Add(new object[] { strNoID, strX, strY, strZ, strR, strA, bDispenserSpeed, bUse, bUseVisionOfx, "", strPathName });

            }

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            string strInitialDirectory = pMachine.m_DispenserAuto.m_DispenserSt[m_iDisStID].GetPathDirectory();
            OpenFileDialog file = new OpenFileDialog();
            file.InitialDirectory = strInitialDirectory;
            file.ShowDialog();
            String selectRecipe = file.SafeFileName;
            String strShowRecipe = selectRecipe.Replace(".csv", "");
            label_PathName.Text = strShowRecipe;

            if (strShowRecipe == "")
            {
                MessageBox.Show(this, "未選取Recipe , 請重新確認 !!");
                return;
            }

            LoadPathFile(file.FileName);

        }


        private void LoadPathFile(string strFileName)
        {
            string strFilePath = strFileName;

            bool bRet = true;
            String strData = "", strIOID = "";
            String[] strPramData = new string[255];
            for (int i = 0; i < strPramData.Length; i++)
                strPramData[i] = null;
            //-----------
            StreamReader sr;
            try
            {
                sr = new StreamReader(strFilePath, Encoding.Default);
            }
            catch
            {
                return;
            }

            int iReadNum = 0;
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                strPramData[iReadNum] = line;
                iReadNum++;
            }
            sr.Close();

            Sys_Define.tyMotionBufferData_XYZ[] XYZMotionData = new Sys_Define.tyMotionBufferData_XYZ[iReadNum];

            string str_Data = "";
            string[] strParameter = new string[18];
            dataGridView_PathData.Rows.Clear();
           
            //-------------
            for (int i = 0; i < iReadNum; i++)
            {
                str_Data = strPramData[i];
                int num = 0;
                string[] str_GetParm = str_Data.Split(',');
                //for (int m = 0; m < str_GetParm.Length; m++)
                //    str_Data += str_GetParm[m];
                for (int j = 0; j < str_GetParm.Length; j++)
                {
                    //string str_GetParm1 = str_GetParm[j];// str_GetParm2 = "";
                    ////---------------------
                    //int iLength = str_Data.Length;
                    //int iIndex1 = str_Data.IndexOf(",");
                    //if (iIndex1 > 0)
                    //{
                    //    str_GetParm1 = str_Data.Remove(iIndex1);//, iLength);
                    //    str_Data = str_Data.Remove(0, iIndex1 + 1);
                    //}
                    //else
                    //    str_GetParm1 = str_Data;
                    ////---------------------
                    //int iIndex2 = str_GetParm1.IndexOf(":");
                    //str_GetParm2 = str_GetParm1.Remove(0, (iIndex2));
                    ////---------------------
                    if (!str_GetParm[j].Contains(":"))
                    {
                        strParameter[num] = str_GetParm[j];
                        num++;
                    }
                }
                //----------
                #region Show Path to DataGridView 

                string strNoID = strParameter[0];  //strParameter[0]  , ID
                string strMode = strParameter[1];   //type
                string strCircleMode = strParameter[2];
                if (strParameter[1] == "0")
                    strMode = "Line";
                else
                    strMode = "Arc";

                if (strParameter[2] == "0")
                    strCircleMode = "XY";
                else if (strParameter[2] == "1")
                    strCircleMode = "XZ";
                else if (strParameter[2] == "2")
                    strCircleMode = "YZ";
                else
                    strCircleMode = "";
                //--------------------
                string strMX = strParameter[3];    //strParameter[3]  , MX
                string strMY = strParameter[4];    //strParameter[4]  , MY
                string strMZ = strParameter[5];    //strParameter[5]  , MZ
                //--------------------
                string strTX = strParameter[6];    //strParameter[6]  , TX
                string strTY = strParameter[7];    //strParameter[7]  , TY
                string strTZ = strParameter[8];    //strParameter[8]  , TZ
                string strTR = strParameter[9];    //strParameter[9]  , TR
                string strTA = strParameter[10];    //strParameter[10]  , TA


                string strSpeed = strParameter[11];  //strParameter[11] ,Speed
                string strAccSpeed = strParameter[12];  //strParameter[12] ,AccSpeed
                bool bIOStatus = (strParameter[13] == "1") ? true : false; //strParameter[13] ,IOStatus
                string strStartDelay = strParameter[14];   //strParameter[14], StartDelay
                string strEndDelay = strParameter[15];    //strParameter[15], EngDelay
                bool bStartIOStatus = (strParameter[16] == "1") ? true : false;  //strParameter[16], StartDelayIOStatus
                bool bEndIOStatus = (strParameter[17] == "1") ? true : false;   //strParameter[17], EndDelayIOStatus


                if (strMode == "Arc")
                {
                    dataGridView_PathData.Rows.Add(new object[] { strNoID, "", strMode,strCircleMode,strMX, strMY,strMZ, "",
                                                                      strTX,strTY,strTZ,"-","-","", strSpeed,strAccSpeed,
                                                                      "",bIOStatus,strStartDelay,bStartIOStatus, strEndDelay ,bEndIOStatus });

                    //------------
                    int[] iReadOnlyID = { 7, 11, 12 };

                    for (int k = 0; k < iReadOnlyID.Length; k++)
                    {
                        dataGridView_PathData[iReadOnlyID[k], i].ReadOnly = true;
                        dataGridView_PathData[iReadOnlyID[k], i].Style.BackColor = Color.Pink;
                    }

                }
                else
                {
                    clsDispenserSt pDisSt = pMachine.m_DispenserAuto.m_DispenserSt[m_iDisStID];

                    dataGridView_PathData.Rows.Add(new object[] { strNoID, "", strMode,"","-", "-","-", "",
                                                                      strTX,strTY,strTZ,strTR,strTA,"", strSpeed,strAccSpeed,
                                                                      "",bIOStatus,strStartDelay,bStartIOStatus, strEndDelay ,bEndIOStatus });
                    //------------
                    int[] iReadOnlyID = { 1, 3, 4, 5, 6 };

                    for (int k = 0; k < iReadOnlyID.Length; k++)
                    {
                        dataGridView_PathData[iReadOnlyID[k], i].ReadOnly = true;
                        dataGridView_PathData[iReadOnlyID[k], i].Style.BackColor = Color.Pink;
                    }


                    //dataGridView_PathData[1, i].ReadOnly = true;
                    //dataGridView_PathData[3, i].ReadOnly = true;
                    //dataGridView_PathData[4, i].ReadOnly = true;
                    //dataGridView_PathData[5, i].ReadOnly = true;
                    //dataGridView_PathData[6, i].ReadOnly = true;
                }
                #endregion
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            string strInitialDirectory = pMachine.m_DispenserAuto.m_DispenserSt[m_iDisStID].GetPathDirectory();

            string selectRecipe = label_PathName.Text;
            if (selectRecipe == "")
            {
                MessageBox.Show(this, "未選取Recipe , 請重新確認 !!");
                return;
            }

            string strSaveFile = strInitialDirectory + selectRecipe + ".csv";

            //strSaveFile
            //-----------
            int iCount = 0;
            StreamWriter sw;
          
            try
            {

                FileStream fs = new FileStream(strSaveFile, FileMode.Truncate, FileAccess.ReadWrite);
                fs.Close();
                fs.Dispose();


                //--------
                sw = File.AppendText(strSaveFile);
                int iRowCount = dataGridView_PathData.RowCount - 1;
             
                for (int i = 0; i < iRowCount; i++)
                {
                    iCount++;
                    object obNoID = dataGridView_PathData[0, i].Value;
                    //----                  
                    object obMode = dataGridView_PathData[2, i].Value;
                    //---------
                    string strMode = "";
                    if (obMode.ToString() == "Line")
                        strMode = "0";
                    else
                        strMode = "2";
                    //---------
                    object obCircleMode = dataGridView_PathData[3, i].Value;
                    string strCircleMode = "";
                    if (obCircleMode != null && obCircleMode.ToString() == "XY")
                        strCircleMode = "0";
                    else if (obCircleMode != null && obCircleMode.ToString() == "XZ")
                        strCircleMode = "1";
                    else if (obCircleMode != null && obCircleMode.ToString() == "YZ")
                        strCircleMode = "2";
                    else
                        strCircleMode = "";
                    //---------

                    object obMX = dataGridView_PathData[4, i].Value;
                    object obMY = dataGridView_PathData[5, i].Value;
                    object obMZ = dataGridView_PathData[6, i].Value;

                    if (obMX == null || obMY == null || obMZ == null)
                    {
                        obMX = obMY = obMZ = "-";
                    }
                    
                    try
                    {//确认保存的数据是否有非法字符

                        

                        if (obMX.ToString() != "-")
                        {
                            double mx = double .Parse( dataGridView_PathData[4, i].Value.ToString());
                            double my = double.Parse(dataGridView_PathData[5, i].Value.ToString());
                            double mz = double.Parse(dataGridView_PathData[6, i].Value.ToString());
                        }
                        double tx = double.Parse(dataGridView_PathData[8, i].Value.ToString());
                        double ty = double.Parse(dataGridView_PathData[9, i].Value.ToString());
                        double tz = double.Parse(dataGridView_PathData[10, i].Value.ToString());

                        double tr, ta;
                        if (dataGridView_PathData.Columns[11].Visible == true)
                        {
                            if (dataGridView_PathData[11, i].Value.ToString() != "-")
                                tr = double.Parse(dataGridView_PathData[11, i].Value.ToString());
                        }
                        if (dataGridView_PathData.Columns[12].Visible == true)
                        {
                            if (dataGridView_PathData[12, i].Value.ToString() != "-")
                                ta = double.Parse(dataGridView_PathData[12, i].Value.ToString());
                        }

                        //double io = double.Parse(dataGridView_PathData[17, i].Value.ToString());
                        double startdelay = double.Parse(dataGridView_PathData[18, i].Value.ToString());
                        double enddelay = double.Parse(dataGridView_PathData[20, i].Value.ToString());


                        //for (int j = 0; j < 21; j++)
                        //{
                        //    double num = double.Parse(dataGridView_PathData[j, i].Value.ToString());
                        //    if (dataGridView_PathData[j, i].Value != null)
                        //    {
                        //        if (dataGridView_PathData[j, i].Value.ToString().Contains(","))
                        //        {
                        //            MessageBox.Show("点位中存在非法字符，请检查确认！");
                        //            return;
                        //        }
                        //    }
                        //}
                    }
                    catch
                    {
                        MessageBox.Show("点位中存在非法字符，请检查确认！");
                        return;
                    }
                    //-----
                    object obTX = dataGridView_PathData[8, i].Value;
                    object obTY = dataGridView_PathData[9, i].Value;
                    object obTZ = dataGridView_PathData[10, i].Value;

                    object obTR, obTA;
                    if (dataGridView_PathData.Columns[11].Visible == false)
                        obTR = 0;
                    else
                    {
                        obTR = dataGridView_PathData[11, i].Value;

                        if (obTR.ToString() == "-")
                            obTR = 0;
                    }
                    if (dataGridView_PathData.Columns[12].Visible == false)
                        obTA = 0;
                    else
                    {
                        obTA = dataGridView_PathData[12, i].Value;
                        if (obTA.ToString() == "-")
                            obTA = 0;
                    }

                    //-----
                    object obSpeed = dataGridView_PathData[14, i].Value;
                    object obAccSpeed = dataGridView_PathData[15, i].Value;
                    //----
                    bool bIOStatus = Convert.ToBoolean(dataGridView_PathData[17, i].Value);
                    string strIOStatus = (bIOStatus == true) ? "1" : "0";
                    //----
                    object obStartDelay = dataGridView_PathData[18, i].Value;

                    bool bStartIOStatus = Convert.ToBoolean(dataGridView_PathData[19, i].Value);
                    string strStartIOStatus = (bStartIOStatus == true) ? "1" : "0";
                    //----
                    object obEndDelay = dataGridView_PathData[20, i].Value;

                    bool bEndIOStatus = Convert.ToBoolean(dataGridView_PathData[21, i].Value);
                    string strEndIOStatus = (bEndIOStatus == true) ? "1" : "0";
                    //----

                    #region  存檔資料內容
                    string strSetData1, strSetData2, strSetData3, strSetData4, strSetData5, strSetData6, strSetData7, strSetData8, strSetData9;
                    string strWriteData;
                    try
                    {

                        strSetData1 = "ID:," + obNoID.ToString() + "," + "Type:," + strMode + ",";
                        strSetData2 = "CircleMode:," + strCircleMode + ",";
                        strSetData3 = "MX:," + obMX.ToString() + "," + "MY:," + obMY.ToString() + "," + "MZ:," + obMZ.ToString() + ",";
                        strSetData4 = "TX:," + obTX.ToString() + "," + "TY:," + obTY.ToString() + "," + "TZ:," + obTZ.ToString() + ",";
                        strSetData5 = "TR:," + obTR.ToString() + "," + "TA:," + obTA.ToString() + ",";
                        strSetData6 = "Speed:," + obSpeed.ToString() + "," + "AccSpeed:," + obAccSpeed.ToString() + ",";
                        strSetData7 = "IOStatus:," + strIOStatus + "," + "StartDelay:," + obStartDelay.ToString() + ",";
                        strSetData8 = "EndDelay:," + obEndDelay.ToString() + "," + "StartDelayIOStatus:," + strStartIOStatus + ",";
                        strSetData9 = "EndDelayIOStatus:," + strEndIOStatus.ToString();
                        strWriteData = strSetData1 + strSetData2 + strSetData3 + strSetData4 + strSetData5 + strSetData6 + strSetData7 + strSetData8 + strSetData9;// 
                        sw.WriteLine(strWriteData);
                        System.Threading.Thread.Sleep(10);
                    }
                    catch
                    {
                        strWriteData = "Error";
                        MessageBox.Show("格式输入异常，请检查确认！");
                    }

                    #endregion

                  
                }
                sw.Flush();
                sw.Close();
                sw.Dispose();
            }
            catch
            {

                return;
            }


        }

        private void button4_Click(object sender, EventArgs e)
        {
            string strFilePath = pMachine.GetWorkDataPath();
            //************************Save Dispenser Data ************************
            #region Save DispenserData
            clsDispenserSt pDisSt = pMachine.m_DispenserAuto.m_DispenserSt[m_iDisStID];
            int iArrayNum = Convert.ToInt16(numericUpDown_PathNum.Value);
            //---------
            MDataDefine.DispenserList Dis_List = new MDataDefine.DispenserList();
            Dis_List.Initial();
            Dis_List.iArrayNum = iArrayNum;

            for (int i = 0; i < 100; i++)
            {
                MDataDefine.DispenserList_Data Dis_Data = new MDataDefine.DispenserList_Data();
                if (i < iArrayNum)
                {
                    object obX = dataGridView_DispenserData[1, i].Value;
                    object obY = dataGridView_DispenserData[2, i].Value;
                    object obZ = dataGridView_DispenserData[3, i].Value;
                    object obR = dataGridView_DispenserData[4, i].Value;
                    object obA = dataGridView_DispenserData[5, i].Value;
                    //----------------------
                    if (obR.ToString() == "-")
                        obR = "0";
                    if (obA.ToString() == "-")
                        obA = "0";
                    //----------------------
                    object oDispenserSpeed = dataGridView_DispenserData[6, i].Value;//点胶分段速度
                    object oUse = dataGridView_DispenserData[7, i].Value;
                    object oOfxUse = dataGridView_DispenserData[8, i].Value;
                    object oPathFileName = dataGridView_DispenserData[10, i].Value;
                    //----------------------
                    Dis_Data.tyTartgetPos.X = Convert.ToDouble(obX);
                    Dis_Data.tyTartgetPos.Y = Convert.ToDouble(obY);
                    Dis_Data.tyTartgetPos.Z = Convert.ToDouble(obZ);
                    Dis_Data.tyTartgetPos.R = Convert.ToDouble(obR);
                    Dis_Data.tyTartgetPos.A = Convert.ToDouble(obA);
                    //--------------------
                    Dis_Data.m_bUse = Convert.ToBoolean(oUse);
                    Dis_Data.m_bUseVisionOfx = Convert.ToBoolean(oOfxUse);
                    Dis_Data.strPathName = oPathFileName.ToString();
                    Dis_Data.m_DispenserSpeed = Convert.ToDouble(oDispenserSpeed);//点胶分段速
                    //--------------------
                    Dis_List.DispenserData.Add(i, Dis_Data);
                }
                else
                {
                    Dis_Data = pDisSt.m_DispenserList.DispenserData[i];
                    Dis_List.DispenserData.Add(i, Dis_Data);
                }
            }

            pDisSt.m_DispenserList = Dis_List;
            pDisSt.SaveWorkData(strFilePath);
            #endregion
        }

        private void combo_StationID_SelectedIndexChanged(object sender, EventArgs e)
        {

            string strSelectText = combo_StationID.SelectedItem.ToString();
            if (strSelectText == "")
                return;

            string strSelectStation = strSelectText.Replace("Station_", "");
            int iSelectStID = Convert.ToInt16(strSelectStation);
            m_iDisStID = iSelectStID - 1;
            //--------------------------
            Fun_ShowDispenserData();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 2)
            {

                #region  判斷 Dispenser.m_enAxisType , 確認是否隱藏 TR& TA輸入空格 

                clsDispenserSt pDisSt = pMachine.m_DispenserAuto.m_DispenserSt[m_iDisStID];
                if (myIniFile.IniReadValue("GantryModes", "GantryMode") == "XYZ")//if (pDisSt.m_Dispenser.m_enAxisType == Sys_Define.enAixsType.en_XYZ)
                {
                    dataGridView_PathData.Columns[11].Visible = false;
                    dataGridView_PathData.Columns[12].Visible = false;
                }
                else if (myIniFile.IniReadValue("GantryModes", "GantryMode") == "XYZR") //if (pDisSt.m_Dispenser.m_enAxisType == Sys_Define.enAixsType.en_XYZR)
                {
                    dataGridView_PathData.Columns[11].Visible = true;
                    dataGridView_PathData.Columns[12].Visible = false;
                }
                else if (myIniFile.IniReadValue("GantryModes", "GantryMode") == "XYZA")//if (pDisSt.m_Dispenser.m_enAxisType == Sys_Define.enAixsType.en_XYZA)
                {
                    dataGridView_PathData.Columns[11].Visible = false;
                    dataGridView_PathData.Columns[12].Visible = true;
                }
                else if (myIniFile.IniReadValue("GantryModes", "GantryMode") == "XYZRA") //if (pDisSt.m_Dispenser.m_enAxisType == Sys_Define.enAixsType.en_XYZRA)
                {
                    dataGridView_PathData.Columns[11].Visible = true;
                    dataGridView_PathData.Columns[12].Visible = true;
                }

                #endregion

            }
        }

        private void CalibrateBtn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(" 确认标定? ", "标定开始", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            {
                return;
            }
            pMachine.m_DispenserAuto.m_DispenserAuto1.calibrate.pDis_DisOn = pMachine.m_DispenserAuto.m_Dis1_DisOn;
            pMachine.m_DispenserAuto.m_DispenserAuto1.calibrate.pDis_AirOn = pMachine.m_DispenserAuto.m_Dis1_AirOn;
            //-------------
            pMachine.m_DispenserAuto.m_DispenserAuto2.calibrate.pDis_DisOn = pMachine.m_DispenserAuto.m_Dis2_DisOn;
            pMachine.m_DispenserAuto.m_DispenserAuto2.calibrate.pDis_AirOn = pMachine.m_DispenserAuto.m_Dis2_AirOn;
            //****************************************************
            int pStation = int.Parse(StationNo.Text.Substring(0, 1));//工站位
            int pIdeo = int.Parse(StationNo.Text.Substring(2, 1));//拍照次
            pMachine.m_DispenserAuto.DispenserCalibrate(pStation, pIdeo, Convert.ToDouble(txtXStepLength.Text), Convert.ToDouble(txtYStepLength.Text));
            //****************************************************
            //pMachine.m_DispenserAuto.DispenserCalibrate(int.Parse(StationNo.Text));
        }

        private void CheckChange(object sender, EventArgs e)
        {
            //string strFilePath = pMachine.GetWorkDataPath();

            //DialogResult dlgResult = MessageBox.Show(" 確認是否儲存檔案? ", "Save Dlg", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            //if (dlgResult != DialogResult.Yes)
            //    return;
            //try
            //{
            //    //************************Save Machine Parm ************************
            //    #region Save Machine Parm
            //    //-------
            //    pMachine.m_dbAutoingSpeed = Convert.ToDouble(numericUpDown_AutoSpeed.Value);
            //    pMachine.m_bTestMode = checkBox_TestMode.Checked;
            //    MDataDefine.m_bTestMode = checkBox_TestMode.Checked;
            //    define.m_bScanMode = checkBox_ScanMode.Checked;
            //    pMachine.m_bCheckDoorSR = checkBox_CheckDoorSR.Checked;
            //    MDataDefine.m_AutoDis = AutoDis.Checked;
            //    //------
            //    define.WriteScan(define.m_bScanMode);
            //    pMachine.SaveWorkData(strFilePath);

            //    #endregion
            //    //******************************************************************
            //    //pMachine.LoadWorkData(strFilePath);  //重新LoadWorkData
            //}
            //catch
            //{
            //    DialogResult Errdlg = MessageBox.Show(" 資料內容格式錯誤 ,請確認修改過之資料數值!! ");
            //    return;
            //}
        }

        private void BtnCali_Click(object sender, EventArgs e)
        {
            MDataDefine.IsCailBase[int.Parse(CCDNo.Text) - 1] = false;
            pMachine.m_DispenserAuto.m_DispenserAuto1.calibrate.pDis_DisOn = pMachine.m_DispenserAuto.m_Dis1_DisOn;
            pMachine.m_DispenserAuto.m_DispenserAuto1.calibrate.pDis_AirOn = pMachine.m_DispenserAuto.m_Dis1_AirOn;
            //-------------
            pMachine.m_DispenserAuto.m_DispenserAuto2.calibrate.pDis_DisOn = pMachine.m_DispenserAuto.m_Dis2_DisOn;
            pMachine.m_DispenserAuto.m_DispenserAuto2.calibrate.pDis_AirOn = pMachine.m_DispenserAuto.m_Dis2_AirOn;
            //****************************************************
            pMachine.m_DispenserAuto.CCDCalibrate(int.Parse(CCDNo.Text));
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ckb_DownTime_CheckedChanged(object sender, EventArgs e)
        {
            if (ckb_DownTime.Checked)
            {
                frm_Main.formData.ChartTime1.Startdt();
            }
            else
            {
                frm_Main.formData.ChartTime1.StartWait();
            }
        }

        private void BtnCalibase_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("更新校准Z轴点位后，重新获取Z轴的基准值，后期Z轴方向将依赖此基准", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                MDataDefine.IsCailBase[int.Parse(CCDNo.Text) - 1] = true;
                pMachine.m_DispenserAuto.m_DispenserAuto1.calibrate.pDis_DisOn = pMachine.m_DispenserAuto.m_Dis1_DisOn;
                pMachine.m_DispenserAuto.m_DispenserAuto1.calibrate.pDis_AirOn = pMachine.m_DispenserAuto.m_Dis1_AirOn;
                //-------------
                pMachine.m_DispenserAuto.m_DispenserAuto2.calibrate.pDis_DisOn = pMachine.m_DispenserAuto.m_Dis2_DisOn;
                pMachine.m_DispenserAuto.m_DispenserAuto2.calibrate.pDis_AirOn = pMachine.m_DispenserAuto.m_Dis2_AirOn;
                //****************************************************
                pMachine.m_DispenserAuto.CCDCalibrate(int.Parse(CCDNo.Text));
            }
        }

        private void cBoxNullRun_CheckedChanged(object sender, EventArgs e)
        {
            if (cBoxNullRun.Checked)
            {
                checkBoxTestMode.Checked = true;
                cBoxNoCCD.Checked = true;
                checkBox_NoScan.Checked = true;
                checkBox_NoMES.Checked = true;
                cBoxAutoDis.Checked = false;
                cBoxNoCloth.Checked = true;
                cBoxNoCCD.Enabled = false;
                checkBox_NoScan.Enabled = false;
                checkBox_NoMES.Enabled = false;
                cBoxAutoDis.Enabled = false;
                checkBoxTestMode.Enabled = false;
                cBoxNoCloth.Enabled = false;
            }
            else
            {
                checkBoxTestMode.Checked = false;
                cBoxNoCCD.Checked = false;
                checkBox_NoScan.Checked = false;
                checkBox_NoMES.Checked = false;
                cBoxNoCloth.Checked = false;
                cBoxAutoDis.Checked = true;


                cBoxAutoDis.Enabled = true;
                checkBoxTestMode.Enabled = true;
                cBoxNoCloth.Enabled = true;
            }
        }

        private void checkBoxTestMode_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxTestMode.Checked)
            {
                cBoxNoCloth.Checked = true;
                checkBox_NoMES.Checked = true;
                cBoxNoCCD.Checked = true;
                checkBox_NoScan.Checked = true;
                cBoxAutoDis.Checked = true;
                cBoxNoCCD.Enabled = true;
                checkBox_NoScan.Enabled = true;
                cBoxAutoDis.Enabled = true;
                cBoxNoCloth.Enabled = true;
                checkBox_NoMES.Enabled = false;
            }
            else
            {
                checkBox_NoMES.Checked = false;
                checkBox_NoScan.Checked = false;
                cBoxNoCCD.Checked = false;
                cBoxNoCloth.Checked = false;
                cBoxAutoDis.Checked = true;

                cBoxNoCloth.Enabled = false;
                cBoxNoCCD.Enabled = false;
                checkBox_NoScan.Enabled = false;
                checkBox_NoMES.Enabled = false;
            }
        }

        private void CCDBtnData_Click(object sender, EventArgs e)
        {
            MSystemParameter.GantryParm pGantryParm1 = MSystemParameter.m_SysParm.Gantry1Parm;
            MSystemParameter.GantryParm pGantryParm2 = MSystemParameter.m_SysParm.Gantry1Parm;
            string cmd = ",123,20210120153733,Offline,123,123,123";
            if (GantryCom.SelectedItem.ToString() == "1")
            {
                if (pGantryParm1.enMatchMode == MSystemParameter.enMatchingMode.JustBackStation)//右侧流道指令不同
                {
                    cmd = pGantryParm1.strRStTakePictureOrder[0] + cmd;
                }
                else
                {
                    cmd = pGantryParm1.strLStTakePictureOrder[0] + cmd;
                }
                MDataDefine.m_bGetData = true;
                int n = 0;
                if (pGantryParm1.enMatchMode == MSystemParameter.enMatchingMode.JustBackStation)
                {
                    n = 1;
                }
                DrvMotor motor1 = pMachine.m_DispenserAuto.m_DispenserSt[n].m_Dispenser.pMotX;
                DrvMotor motor2 = pMachine.m_DispenserAuto.m_DispenserSt[n].m_Dispenser.pMotY;
                DrvMotor motor3 = pMachine.m_DispenserAuto.m_DispenserSt[n].m_Dispenser.pMotZ;
                DrvMotor motor4 = null;
                if (pMachine.m_DispenserAuto.m_DispenserSt[n].m_Dispenser.pMotA != null)
                    motor4 = pMachine.m_DispenserAuto.m_DispenserSt[n].m_Dispenser.pMotA;
                DrvMotor motor5 = null;
                if (pMachine.m_DispenserAuto.m_DispenserSt[n].m_Dispenser.pMotR != null)
                    motor5 = pMachine.m_DispenserAuto.m_DispenserSt[n].m_Dispenser.pMotR;
                pMachine.m_DispenserAuto.m_DispenserAuto1.ccddata.pClient = pMachine.m_DispenserAuto.m_pClient1;
                Sys_Define.tyAXIS_XYZRA Pos1 = pMachine.m_DispenserAuto.m_DispenserSt[n].m_tyPosition[(int)clsDispenserSt.enPosition.SafePos];
                Sys_Define.tyAXIS_XYZRA Pos2 = pMachine.m_DispenserAuto.m_DispenserSt[n].m_tyPosition[(int)clsDispenserSt.enPosition.CCDPos1];
                pMachine.m_DispenserAuto.m_DispenserAuto1.ccddata.GetData(ref motor1, ref motor2, ref motor3, ref motor4, ref motor5, Pos1, Pos2, pMachine.m_dbAutoingSpeed, cmd, int.Parse(Data.Text));
                //pMachine.m_DispenserAuto.CCDData(1, pMachine.m_dbAutoingSpeed, int.Parse(Data.Text));
            }
            else if (GantryCom.SelectedItem.ToString() == "2")
            {
                if (pGantryParm2.enMatchMode == MSystemParameter.enMatchingMode.JustFrontStation)//右侧流道指令不同
                {
                    cmd = pGantryParm2.strRStTakePictureOrder[0] + cmd;
                }
                else
                {
                    cmd = pGantryParm2.strLStTakePictureOrder[0] + cmd;
                }
                int n = 2;
                if (pGantryParm2.enMatchMode == MSystemParameter.enMatchingMode.JustFrontStation)
                {
                    n = 3;
                }
                MDataDefine.m_bGetData = true;
                DrvMotor motor1 = pMachine.m_DispenserAuto.m_DispenserSt[n].m_Dispenser.pMotX;
                DrvMotor motor2 = pMachine.m_DispenserAuto.m_DispenserSt[n].m_Dispenser.pMotY;
                DrvMotor motor3 = pMachine.m_DispenserAuto.m_DispenserSt[n].m_Dispenser.pMotZ;
                DrvMotor motor4 = null;
                if (pMachine.m_DispenserAuto.m_DispenserSt[n].m_Dispenser.pMotA != null)
                    motor4 = pMachine.m_DispenserAuto.m_DispenserSt[n].m_Dispenser.pMotA;
                DrvMotor motor5 = null;
                if (pMachine.m_DispenserAuto.m_DispenserSt[n].m_Dispenser.pMotR != null)
                    motor5 = pMachine.m_DispenserAuto.m_DispenserSt[n].m_Dispenser.pMotR;
                pMachine.m_DispenserAuto.m_DispenserAuto1.ccddata.pClient = pMachine.m_DispenserAuto.m_pClient2;
                Sys_Define.tyAXIS_XYZRA Pos1 = pMachine.m_DispenserAuto.m_DispenserSt[n].m_tyPosition[(int)clsDispenserSt.enPosition.SafePos];
                Sys_Define.tyAXIS_XYZRA Pos2 = pMachine.m_DispenserAuto.m_DispenserSt[n].m_tyPosition[(int)clsDispenserSt.enPosition.CCDPos1];
                pMachine.m_DispenserAuto.m_DispenserAuto1.ccddata.GetData(ref motor1, ref motor2, ref motor3, ref motor4, ref motor5, Pos1, Pos2, pMachine.m_dbAutoingSpeed, cmd, int.Parse(Data2.Text));
            }
        }

        private void CCDBtnData2_Click(object sender, EventArgs e)
        {
            MSystemParameter.GantryParm pGantryParm1 = MSystemParameter.m_SysParm.Gantry1Parm;
            MSystemParameter.GantryParm pGantryParm2 = MSystemParameter.m_SysParm.Gantry1Parm;
            string cmd = ",123,20210120153733,Offline,123,123,123";
            if (GantryCom.SelectedItem.ToString() == "1")
            {
                if (pGantryParm1.enMatchMode == MSystemParameter.enMatchingMode.JustBackStation)//右侧流道指令不同
                {
                    cmd = pGantryParm1.strRStTakePictureOrder[0] + cmd;
                }
                else
                {
                    cmd = pGantryParm1.strLStTakePictureOrder[0] + cmd;
                }
                MDataDefine.m_bGetData = true;
                int n = 0;
                if (pGantryParm1.enMatchMode == MSystemParameter.enMatchingMode.JustBackStation)
                {
                    n = 1;
                }
                DrvMotor motor1 = pMachine.m_DispenserAuto.m_DispenserSt[n].m_Dispenser.pMotX;
                DrvMotor motor2 = pMachine.m_DispenserAuto.m_DispenserSt[n].m_Dispenser.pMotY;
                DrvMotor motor3 = pMachine.m_DispenserAuto.m_DispenserSt[n].m_Dispenser.pMotZ;
                DrvMotor motor4 = null;
                DrvMotor motor5 = null;
                if (pMachine.m_DispenserAuto.m_DispenserSt[n].m_Dispenser.pMotA != null)
                    motor4 = pMachine.m_DispenserAuto.m_DispenserSt[n].m_Dispenser.pMotA;
                if (pMachine.m_DispenserAuto.m_DispenserSt[n].m_Dispenser.pMotR != null)
                    motor5 = pMachine.m_DispenserAuto.m_DispenserSt[n].m_Dispenser.pMotR;
                pMachine.m_DispenserAuto.m_DispenserAuto1.ccddata.pClient = pMachine.m_DispenserAuto.m_pClient1;
                Sys_Define.tyAXIS_XYZRA Pos1 = pMachine.m_DispenserAuto.m_DispenserSt[n].m_tyPosition[(int)clsDispenserSt.enPosition.SafePos];
                Sys_Define.tyAXIS_XYZRA Pos2 = pMachine.m_DispenserAuto.m_DispenserSt[n].m_tyPosition[(int)clsDispenserSt.enPosition.CCDPos1];
                pMachine.m_DispenserAuto.m_DispenserAuto1.ccddata.GetData2(ref motor1, ref motor2, ref motor3, ref motor4, ref motor5, Pos1, Pos2, pMachine.m_dbAutoingSpeed, cmd, int.Parse(Data2.Text));

            }
            else
            {
                if (pGantryParm2.enMatchMode == MSystemParameter.enMatchingMode.JustFrontStation)//右侧流道指令不同
                {
                    cmd = pGantryParm2.strRStTakePictureOrder[0] + cmd;
                }
                else
                {
                    cmd = pGantryParm2.strLStTakePictureOrder[0] + cmd;
                }
                MDataDefine.m_bGetData = true;
                int n = 2;
                if (pGantryParm2.enMatchMode == MSystemParameter.enMatchingMode.JustFrontStation)
                {
                    n = 3;
                }
                DrvMotor motor1 = pMachine.m_DispenserAuto.m_DispenserSt[n].m_Dispenser.pMotX;
                DrvMotor motor2 = pMachine.m_DispenserAuto.m_DispenserSt[n].m_Dispenser.pMotY;
                DrvMotor motor3 = pMachine.m_DispenserAuto.m_DispenserSt[n].m_Dispenser.pMotZ;
                DrvMotor motor4 = null;
                DrvMotor motor5 = null;
                if (pMachine.m_DispenserAuto.m_DispenserSt[n].m_Dispenser.pMotA != null)
                    motor4 = pMachine.m_DispenserAuto.m_DispenserSt[n].m_Dispenser.pMotA;
                if (pMachine.m_DispenserAuto.m_DispenserSt[n].m_Dispenser.pMotR != null)
                    motor5 = pMachine.m_DispenserAuto.m_DispenserSt[n].m_Dispenser.pMotR;
                pMachine.m_DispenserAuto.m_DispenserAuto1.ccddata.pClient = pMachine.m_DispenserAuto.m_pClient2;
                Sys_Define.tyAXIS_XYZRA Pos1 = pMachine.m_DispenserAuto.m_DispenserSt[n].m_tyPosition[(int)clsDispenserSt.enPosition.SafePos];
                Sys_Define.tyAXIS_XYZRA Pos2 = pMachine.m_DispenserAuto.m_DispenserSt[n].m_tyPosition[(int)clsDispenserSt.enPosition.CCDPos1];
                pMachine.m_DispenserAuto.m_DispenserAuto1.ccddata.GetData2(ref motor1, ref motor2, ref motor3, ref motor4, ref motor5, Pos1, Pos2, pMachine.m_dbAutoingSpeed, cmd, int.Parse(Data2.Text));


            }
        }

        private void cBoxPicture_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cBoxPicture.SelectedIndex == -1)
            {
                return;
            }
            pictureBox1.Image = images[cBoxPicture.Text];
        }
    }
}
