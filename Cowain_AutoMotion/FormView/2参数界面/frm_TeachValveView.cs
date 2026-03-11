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
using Cowain_Machine;
using Cowain_AutoMotion;
using static Cowain_AutoMotion.SQLSugarHelper;

namespace Cowain_Form.FormView
{
    public partial class frm_TeachValveView : Form
    {
        public frm_TeachValveView(ref clsMachine refMachine)
        {
            pMachine = refMachine;
            InitializeComponent();
        }

        clsMachine pMachine = null;
        Base pSelectBase;
        DrvValve pSelectValve;
        DrvValve.tyValve_Parameter ValveParameter = new DrvValve.tyValve_Parameter();
        ImageList ImgList = new ImageList();
        Dictionary<string, DrvValve> showValveList;
        int iSelectCount = 0;

        private void frm_ValveView_Shown(object sender, EventArgs e)
        {
            ImgList.Images.Add("Valve", Cowain_AutoMotion.Properties.Resources.valve);
            ImgList.Images.Add("UseValve", Cowain_AutoMotion.Properties.Resources.UseValve);
            listView_Valve.Items.Clear();
            listView_Valve.LargeImageList = ImgList;
            listView_Valve.SmallImageList = ImgList;
            if (pMachine != null)
            {
                showValveList = Base.GetValveList();
                if (showValveList.Count > 0)
                {
                    pSelectBase = showValveList[showValveList.Keys.ToList()[0]].m_NowAddress;
                    addValveList();
                    timer1.Enabled = true;
                }
            }
            List<StationParam> inputsStations = DBContext<StationParam>.GetInstance().GetList();
            foreach (var item in inputsStations)
            {
                //如果此工位没有配置输入输出，则不添加此tabpage
                List<CylindersStationParam> cylindersStationParam = DBContext<CylindersStationParam>.GetInstance().GetList();
                var result1 = from param in cylindersStationParam where param.stationName == item.CName.Trim() select param;
                if (result1.Count() != 0)
                {
                    tabControl1.TabPages.Add(item.CName.Trim());
                }
            }
        }
        private void frm_ValveView_VisibleChanged(object sender, EventArgs e)
        {
            timer1.Enabled = this.Visible;
        }
        public void addValveList()
        {
            listView_Valve.Clear();
            int SelectTab = tabControl1.SelectedIndex;
            for (int i = 0; i < showValveList.Count; i++)
            {
                pSelectValve = (DrvValve)showValveList[showValveList.Keys.ToList()[i]].m_NowAddress;
                pSelectValve.GetParameter(ref ValveParameter);
                string StrText = ValveParameter.strID + "  " + ValveParameter.strCName;
                StrText = JudgeLanguage.JudgeLag(ValveParameter.strID + "  " + ValveParameter.strCName);
                if (pSelectBase == null)
                {
                    pSelectBase = showValveList[showValveList.Keys.ToList()[i]].m_NowAddress;
                }
                listView_Valve.Items.Add(StrText, "UseValve");
            }
            if (pSelectBase == null)
            {
                return;
            }
            pSelectValve = (DrvValve)pSelectBase;
            pSelectValve.GetParameter(ref ValveParameter);
            comboBox_Mode.SelectedIndex = 0;
            label_ValveID.Text = JudgeLanguage.JudgeLag(ValveParameter.strID + "  " + ValveParameter.strCName.Trim());

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.Visible != true)
            {
                return;
            }
            int nRetCode = 0;
            if (pSelectValve == null) return;
            if (!pMachine.GetInitialStatus(ref nRetCode))
                return;

            bool bisOpen = pSelectValve.GetisOpen();
            bool bisClose = pSelectValve.GetisClose();
            bool bisIdle = pSelectValve.isIDLE();
            label_Open.BackColor = (bisOpen) ? Color.LightGreen : Color.White;
            label_Close.BackColor = (bisClose) ? Color.LightGreen : Color.White;
            label_Action.BackColor = (!bisIdle) ? Color.LightGreen : Color.White;
            //-------------
            btn_Open.Enabled = bisIdle;
            btn_Repeat.Enabled = bisIdle;
            btn_Close.Enabled = bisIdle;
            btn_Off.Enabled = bisIdle;
            //--------------------------------
            DrvIO[] pIO = { pSelectValve.m_OpenIO, pSelectValve.m_CloseIO, pSelectValve.m_OpenSR, pSelectValve.m_CloseSR };
            Label[] pLabel = { label_OpenIO, label_CloseIO, label_OpenSR, label_CloseSR };
            Label[] ptxLabel = { label_txOpen, label_txClose, label_txOpenSR, label_txCloseSR };
            for (int i = 0; i < pIO.Length; i++)
            {
                if (pIO[i] == null)
                {
                    pLabel[i].Visible = ptxLabel[i].Visible = false;
                }
                else
                {
                    pLabel[i].Visible = ptxLabel[i].Visible = true;
                    bool bValue = pIO[i].GetValue();
                    pLabel[i].BackColor = (bValue) ? Color.LightGreen : Color.White;
                }
            }
            //----------------------------------

        }

        private void listView_Valve_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView_Valve.SelectedItems.Count <= 0)
                return;
            for (int i = 0; i < showValveList.Count; i++)
            {
                pSelectValve = (DrvValve)showValveList[showValveList.Keys.ToList()[i]].m_NowAddress;
                pSelectValve.GetParameter(ref ValveParameter);
                //--------------------------
                string StrText = ValveParameter.strID + "  " + ValveParameter.strCName.Trim();
                StrText = JudgeLanguage.JudgeLag(StrText);
                if (listView_Valve.SelectedItems[0].Text == StrText)
                {
                    pSelectBase = (DrvValve)showValveList[showValveList.Keys.ToList()[i]].m_NowAddress;
                    break;
                }
            }
            label_ValveID.Text = JudgeLanguage.JudgeLag(ValveParameter.strID + "  " + ValveParameter.strCName.Trim());
        }

        private void btn_Open_Click(object sender, EventArgs e)
        {
            MachineDataDefine.IsReset = false;
            int iSelect = comboBox_Mode.SelectedIndex;
            DrvValve.enActionMode enSelectMode = (DrvValve.enActionMode)(iSelect);
            pSelectValve.Open(enSelectMode);
        }

        private void btn_Repeat_Click(object sender, EventArgs e)
        {
            int iSelect = comboBox_Mode.SelectedIndex;
            DrvValve.enActionMode enSelectMode = (DrvValve.enActionMode)(iSelect);
            pSelectValve.Repeat(enSelectMode);
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            int iSelect = comboBox_Mode.SelectedIndex;
            DrvValve.enActionMode enSelectMode = (DrvValve.enActionMode)(iSelect);
            pSelectValve.Close(enSelectMode);
        }

        private void btn_Stop_Click(object sender, EventArgs e)
        {
            pSelectValve.Stop();
        }

        private void btn_Off_Click(object sender, EventArgs e)
        {
            pSelectValve.OffAction();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            timer1.Stop();
            pSelectBase = null;
            listView_Valve.Items.Clear();

            if (tabControl1.SelectedIndex == 0)
            {
                showValveList = Base.GetValveList();
                addValveList();
                timer1.Enabled = true;
                return;
            }
            string stationName = tabControl1.SelectedTab.Text;
            Dictionary<string, DrvValve> showValveList11 = Base.GetValveList();
            Dictionary<string, DrvValve> showValveIOList22 = new Dictionary<string, DrvValve>();
            List<CylindersStationParam> inputsStations = DBContext<CylindersStationParam>.GetInstance().GetList();
            foreach (CylindersStationParam input in inputsStations)
            {
                if (input.stationName == stationName)
                {
                    var result11 = (from input11 in showValveList11 where input11.Key == input.CName select input11.Value).ToList<DrvValve>();
                    if (result11.Count() > 0)
                    {
                        showValveIOList22.Add(input.CName, result11[0]);
                    }
                }
            }
            showValveList = showValveIOList22;
            iSelectCount = 0;
            addValveList();
            timer1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //pMachine.m_IoVacPumpOn.SetIO(true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //pMachine.m_IoVacPumpOn.SetIO(false);
        }

        private void frm_TeachValveView_Load(object sender, EventArgs e)
        {
            //    foreach (Control item in Controls)
            //    {
            //        string s = item.Text.ToString();
            //        string[] s1 = { s, "" };
            //        File.AppendAllLines(@"C:\Users\cowain\Desktop\1.txt", s1);
            //        item.Text = JudgeLanguage.JudgeLag(item.Text);
            //    }
            //}
        }
    }
}
