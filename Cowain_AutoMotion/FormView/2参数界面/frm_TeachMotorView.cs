using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cowain_Machine.Flow;
using MotionBase;
using Cowain;
using System.IO;
using Cowain_AutoMotion.Flow;

namespace Cowain_Form.FormView
{
    public partial class frm_TeachMotorView : Form
    {
         public frm_TeachMotorView(ref clsMachine refMachine)
        {
            pMachine = refMachine;
            InitializeComponent();
        }
        clsMachine pMachine=null;
        Base pSelectBase=null;
        DrvMotor pSelectMotor;
        DrvMotor.tyMotor_Parameter SelectParameter = new DrvMotor.tyMotor_Parameter();
        DrvMotor.tyMotor_Parameter MotorParameter = new DrvMotor.tyMotor_Parameter();
        ImageList ImgList = new ImageList();
        Dictionary<string, DrvMotor> showMotorList;
        private void frm_MotorView_Shown(object sender, EventArgs e)
        {
            ImgList.Images.Add("Motor", Cowain_AutoMotion.Properties.Resources.Motor);
            ImgList.Images.Add("UseMotor", Cowain_AutoMotion.Properties.Resources.UseMotor);
            listView_Motor.Items.Clear();
            listView_Motor.LargeImageList = ImgList;
            listView_Motor.SmallImageList = ImgList;
            //------------------------------------------
            if (pMachine != null)
            {
                comboBox_Pitch.SelectedIndex = 0;
                showMotorList = Base.GetMotorList();
                if (showMotorList.Count > 0)
                { 
                    pSelectBase = showMotorList[showMotorList.Keys.ToList()[0]].m_NowAddress;
                    Fun_DisplayMotorList();
                    timer1.Enabled = true;
                }
            }
            //------------------------------------------
        }
        private void frm_MotorView_VisibleChanged(object sender, EventArgs e)
        {
            timer1.Enabled = this.Visible;
        }
        public void Fun_DisplayMotorList()
        {
            listView_Motor.Clear();
            int SelectTab=tabControl1.SelectedIndex;
            //**********顯示馬達List***************
            #region 顯示馬達List
            for (int i=0;i<showMotorList.Count;i++)
            {
                pSelectMotor = (DrvMotor) showMotorList[showMotorList.Keys.ToList()[i]].m_NowAddress;
                pSelectMotor.GetParameter(ref MotorParameter);

                //--------------------------
                string StrText = MotorParameter.strID + "  " + MotorParameter.strCName;
               StrText = JudgeLanguage.JudgeLag(MotorParameter.strID + "  " +  MotorParameter.strCName);
                //if (MotorParameter.i_Station == tabControl1.SelectedIndex)
                //{
                //    if (pSelectBase == showMotorList[showMotorList.Keys.ToList()[i]].m_NowAddress)
                //    {
                //        listView_Motor.Items.Add(StrText, "UseMotor");
                //        //if(listView_Motor.Items.Count ==1)
                //        //listView_Motor.Items[i].BackColor = Color.LightBlue;
                //    }else { 
                //        listView_Motor.Items.Add(StrText, "Motor");
                //    }
                //}

                if (tabControl1.SelectedIndex == 0) //Index==0 , 代表All
                {
                    //----------------------------------

                    //----------------------------------
                    if (pSelectBase == showMotorList[showMotorList.Keys.ToList()[i]].m_NowAddress)
                    {
                        listView_Motor.Items.Add(StrText, "UseMotor");
                        //listView_Motor.Items[i].BackColor = Color.LightBlue;
                    }else{
                        listView_Motor.Items.Add(StrText, "Motor");
                    }

                }
            }
            #endregion
            //*************************************     
            if (pSelectBase == null) return;
            //---------------
            pSelectMotor = (DrvMotor)pSelectBase;
            pSelectMotor.GetParameter(ref MotorParameter);
            //---------------
            textBox_APos.Text = MotorParameter.dbPos1.ToString("#0.0000");
            textBox_BPos.Text = MotorParameter.dbPos2.ToString("#0.0000");
            textBox_Delay.Text = MotorParameter.DelayTime.ToString();
        }


        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            pSelectBase = null;
            listView_Motor.Items.Clear();

            for (int i = 0; i < showMotorList.Count; i++)
            {
                pSelectMotor = (DrvMotor) showMotorList[showMotorList.Keys.ToList()[i]].m_NowAddress;
                pSelectMotor.GetParameter(ref MotorParameter);
                //--------------------------
                string StrText = MotorParameter.strID + "  " + MotorParameter.strCName;
                string s = StrText.ToString();
                string[] s1 = { s, "" };
                File.AppendAllLines(@"C:\Users\cowain\Desktop\1.txt", s1);
                StrText = JudgeLanguage.JudgeLag(MotorParameter.strID + "  " + MotorParameter.strCName);
                //if (MotorParameter.i_Station == tabControl1.SelectedIndex)
                //{
                //    if(pSelectBase==null)
                //        pSelectBase = showMotorList[showMotorList.Keys.ToList()[i]].m_NowAddress;
                //    //----------------------------------
                //    if (pSelectBase == showMotorList[showMotorList.Keys.ToList()[i]].m_NowAddress)
                //        listView_Motor.Items.Add(StrText, "UseMotor");
                //    else
                //        listView_Motor.Items.Add(StrText, "Motor");
                //}

                if ( tabControl1.SelectedIndex==0) //Index==0 , 代表All
                {
                    if (pSelectBase == null)
                        pSelectBase = showMotorList[showMotorList.Keys.ToList()[i]].m_NowAddress;
                    //----------------------------------
                    if (pSelectBase == showMotorList[showMotorList.Keys.ToList()[i]].m_NowAddress)
                        listView_Motor.Items.Add(StrText, "UseMotor");
                    else
                        listView_Motor.Items.Add(StrText, "Motor");
                }
            }

            //---------------------
            Fun_DisplayMotorList();
        }

        private void listView_Motor_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int i, j;
            if (listView_Motor.SelectedItems.Count <= 0)
                return;
            //-----------------------------------------
            string SelectText = listView_Motor.SelectedItems[0].Text;
            for (i = 0; i < listView_Motor.Items.Count; i++)
            {
                for (j = 0; j < showMotorList.Count; j++)
                {
                    pSelectMotor = (DrvMotor)showMotorList[showMotorList.Keys.ToList()[j]].m_NowAddress;
                    pSelectMotor.GetParameter(ref MotorParameter);
                    //--------------------------
                    string StrText = MotorParameter.strID + "  " + MotorParameter.strCName;
                    if (listView_Motor.Items[i].Text == StrText && SelectText == StrText)
                        pSelectBase = (DrvMotor)showMotorList[showMotorList.Keys.ToList()[j]].m_NowAddress;
                }
            }
            Fun_DisplayMotorList();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int i,nRetCode=0;
            //----------------------
            if (!pMachine.GetInitialStatus(ref nRetCode))
                return;
            if (pSelectBase==null)
                return;
            //----------------------       
            DrvMotor pUseMotor = (DrvMotor) pSelectBase;
            pUseMotor.GetParameter(ref SelectParameter);
            //---------------------
            label_MotorID.Text = SelectParameter.strID + "  " + SelectParameter.strCName.Trim();

            double dbPos= pUseMotor.GetPosition();
            label_Pos.Text = dbPos.ToString("0.0000");

            uint uErrorCode = pUseMotor.GetErrorCode();
            label_ErrorCode.Text = uErrorCode.ToString();

            bool bSevOn= pUseMotor.isSevOn();
            btn_ServoOn.Image = (bSevOn)? Cowain_AutoMotion.Properties.Resources.SetOk : Cowain_AutoMotion.Properties.Resources.SetOk_Disable ;
            btn_ServoOn.Text = (bSevOn) ? "Servo ON" : "Servo OFF";
            //---------------------
            bool[] bStatus = new bool[5];
            bStatus[0] = pUseMotor.isPEL();
            bStatus[1] = pUseMotor.isMEL();
            bStatus[2] = pUseMotor.isAlarm();
            bStatus[3] = pUseMotor.isMotionDone();
            bStatus[4] = pUseMotor.isHome();


            Label[] pLabel = { label_Pel,label_Mel,label_Alm,label_MotionDone, label_Org };

            for (i = 0; i < pLabel.Length; i++)
            {
                if (bStatus[i])
                    pLabel[i].BackColor = Color.LightGreen;
                else
                    pLabel[i].BackColor = Color.White;
            }

            Button[] btnStatus = { btn_MoveLeft, btn_MoveRight, btn_APos, btn_BPos, btn_Repeat, btn_Home , btn_ServoOn };
            bool bisIdle=  pUseMotor.isIDLE();
            for (i = 0; i < btnStatus.Length; i++)
                btnStatus[i].Enabled = bisIdle;
           
        }


        private void btn_Home_Click(object sender, EventArgs e)
        {
            DrvMotor pUseMotor = (DrvMotor)pSelectBase;

            if (pUseMotor.isIDLE())
                pUseMotor.DoHome();
        }

        private void btn_Stop_Click(object sender, EventArgs e)
        {
            DrvMotor pUseMotor = (DrvMotor)pSelectBase;
            pUseMotor.Stop();
        }

        private void btn_Alarm_Click(object sender, EventArgs e)
        {
            DrvMotor pUseMotor = (DrvMotor)pSelectBase;
            pUseMotor.AlarmReset();
        }

        private void btn_Repeat_Click(object sender, EventArgs e)
        {
            DrvMotor pUseMotor = (DrvMotor)pSelectBase;
            double dbAPos = Convert.ToDouble(textBox_APos.Text);
            double dbBPos = Convert.ToDouble(textBox_BPos.Text);
            uint uDelayTime = Convert.ToUInt32(textBox_Delay.Text);
            double dbSpeed = Convert.ToDouble(numericUpDown_Speed.Value);
            pUseMotor.Repeat(dbAPos, dbBPos,uDelayTime,dbSpeed);

            LogAuto.SaveChangeParameterLog("移动-----" + label_MotorID.Text + " (A、B往返) : " + dbAPos + " <-> " + dbBPos, ChangeParameterLogSpecies.移动更改点位记录);
        }

        private void btn_APos_Click(object sender, EventArgs e)
        {
            DrvMotor pUseMotor = (DrvMotor)pSelectBase;
            double dbPos= Convert.ToDouble(textBox_APos.Text);
            double dbSpeed = Convert.ToDouble(numericUpDown_Speed.Value);
            pUseMotor.AbsMove(dbPos,dbSpeed);
        }

        private void btn_BPos_Click(object sender, EventArgs e)
        {
            DrvMotor pUseMotor = (DrvMotor)pSelectBase;
            double dbPos = Convert.ToDouble(textBox_BPos.Text);
            double dbSpeed = Convert.ToDouble(numericUpDown_Speed.Value);
            pUseMotor.AbsMove(dbPos,dbSpeed);
        }

        private void btn_MoveLeft_Click(object sender, EventArgs e)
        {
            DrvMotor pUseMotor = (DrvMotor)pSelectBase;
            double dbPos = Convert.ToDouble(comboBox_Pitch.Text);
            double dbSpeed= Convert.ToDouble(numericUpDown_Speed.Value);
            pUseMotor.RevMove(dbPos * -1, dbSpeed);

            double startPos = Convert.ToDouble(label_Pos.Text);
            LogAuto.SaveChangeParameterLog("移动-----" + label_MotorID.Text + " : " + startPos + " -> " + (startPos + dbPos * -1), ChangeParameterLogSpecies.移动更改点位记录);
        }

        private void btn_MoveRight_Click(object sender, EventArgs e)
        {
            DrvMotor pUseMotor = (DrvMotor)pSelectBase;
            double dbPos = Convert.ToDouble(comboBox_Pitch.Text);
            double dbSpeed = Convert.ToDouble(numericUpDown_Speed.Value);
            pUseMotor.RevMove(dbPos,dbSpeed);

            double startPos = Convert.ToDouble(label_Pos.Text);
            LogAuto.SaveChangeParameterLog("移动-----" + label_MotorID.Text + " : " + startPos + " -> " + (startPos + dbPos * 1), ChangeParameterLogSpecies.移动更改点位记录);
        }

        private void btn_ServoOn_Click(object sender, EventArgs e)
        {
            DrvMotor pUseMotor = (DrvMotor)pSelectBase;
            bool bSevON=pUseMotor.isSevOn();
            pUseMotor.SetSevON(!bSevON);           
        }

        private void btn_TorSet_Click(object sender, EventArgs e)
        {
            DrvMotor pUseMotor = (DrvMotor)pSelectBase;
            //----------------------
            double dbTorRate = Convert.ToDouble(numericUpDown1.Value);

            double dbSetTorRate = (dbTorRate / 100);
            int iTorRate = Convert.ToInt32(dbSetTorRate * 3000);
            pUseMotor.SetMaxTorque(iTorRate);
        }

        private void btn_SaveData_Click(object sender, EventArgs e)
        {

        }
    }
}
