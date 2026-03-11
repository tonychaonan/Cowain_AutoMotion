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
using static Cowain_AutoMotion.SQLSugarHelper;
using Cowain_AutoMotion;

namespace Cowain_Form.FormView
{
    public partial class frm_TeachIOView : Form
    {
        public frm_TeachIOView(ref clsMachine refMachine)
        {
            SelectTab = 0;
            pMachine = refMachine;
            InitializeComponent();
        }

        #region 參數&變數
        int SelectTab;
        clsMachine pMachine = null;
        Base pSelectBase = null;
        DrvIO pSelectIO;
        DrvIO.tyIO_Parameter IOParameter = new DrvIO.tyIO_Parameter();

        ImageList InImgList = new ImageList();
        ImageList OutImgList = new ImageList();
        Dictionary<string, DrvIO> showInputsIOList;
        Dictionary<string, DrvIO> showOutputsIOList;


        int icount = 0;
        #endregion

        private void frm_IOView_Shown(object sender, EventArgs e)
        {

            InImgList.Images.Add("On", Cowain_AutoMotion.Properties.Resources.Input_On);
            InImgList.Images.Add("Off", Cowain_AutoMotion.Properties.Resources.Input_Off);
            OutImgList.Images.Add("On", Cowain_AutoMotion.Properties.Resources.Output_On);
            OutImgList.Images.Add("Off", Cowain_AutoMotion.Properties.Resources.Output_Off);
            //------------------------------------------
            listView_Input.Items.Clear();
            listView_Input.LargeImageList = InImgList;
            listView_Input.SmallImageList = InImgList;
            //----------------
            listView_Output.Items.Clear();
            listView_Output.LargeImageList = OutImgList;
            listView_Output.SmallImageList = OutImgList;
            List<StationParam> inputsStations = DBContext<StationParam>.GetInstance().GetList();
            foreach (var item in inputsStations)
            {
                //如果此工位没有配置输入输出，则不添加此tabpage
                List<InputsStationParam> inputsStationParams = DBContext<InputsStationParam>.GetInstance().GetList();
                List<OutputsStationParam> outputsStationParams = DBContext<OutputsStationParam>.GetInstance().GetList();
                var result1 = from param in inputsStationParams where param.stationName == item.CName.Trim() select param;
                var result2 = from param in outputsStationParams where param.stationName == item.CName.Trim() select param;
                if (result1.Count() != 0 || result2.Count() != 0)
                {
                    tabControl1.TabPages.Add(item.CName.Trim());
                }
            }
            //------------------------------------------
            if (pMachine != null)
            {
                showInputsIOList = Base.GetInputsIOList();
                showOutputsIOList = Base.GetOutputsIOList();
                if (showInputsIOList.Count > 0)
                {
                    foreach (var item in showInputsIOList)
                    {
                        pSelectBase = item.Value.m_NowAddress;
                        break;
                    }
                }
                Fun_DisplayInputList();
                Fun_DisplayOutputList();
                timer1.Enabled = true;
            }
        }
        private void frm_IOView_VisibleChanged(object sender, EventArgs e)
        {
            timer1.Enabled = this.Visible;
        }
        public void Fun_DisplayInputList()
        {
            foreach (var item in showInputsIOList)
            {
                pSelectIO = (DrvIO)(item.Value).m_NowAddress;
                pSelectIO.GetParameter(ref IOParameter);
                //--------------------------
                string StrText = IOParameter.strID + " " + IOParameter.strCName.Trim();

                StrText = JudgeLanguage.JudgeLag(StrText);
                if (!IOParameter.bisOut) //&& IOParameter.i_Station == SelectTab)  //非輸出接腳
                    listView_Input.Items.Add(StrText, "Off");
            }
        }

        public void Fun_DisplayOutputList()
        {
            foreach (var item in showOutputsIOList)
            {
                pSelectIO = (DrvIO)(item.Value).m_NowAddress;
                pSelectIO.GetParameter(ref IOParameter);
                //--------------------------
                string StrText = IOParameter.strID + " " + IOParameter.strCName.Trim();

                //StrText=StrText.Replace(" ","");
                // string s = StrText.ToString();
                //string[] s1 = { s, "" };
                //File.AppendAllLines(@"C:\Users\cowain\Desktop\1.txt", s1);
                StrText = JudgeLanguage.JudgeLag(StrText);
                if (IOParameter.bisOut) //&& IOParameter.i_Station == SelectTab)  //輸出接腳
                    listView_Output.Items.Add(StrText, "Off");
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            timer1.Stop();
            listView_Input.Items.Clear();
            listView_Output.Items.Clear();
            SelectTab = tabControl1.SelectedIndex;
            if (SelectTab == 0)
            {
                showInputsIOList = Base.GetInputsIOList();
                showOutputsIOList = Base.GetOutputsIOList();
                Fun_DisplayInputList();
                Fun_DisplayOutputList();
                timer1.Enabled = true;
                return;
            }
            string stationName = tabControl1.SelectedTab.Text;
            Dictionary<string, DrvIO> showInputsIOList11 = Base.GetInputsIOList();
            Dictionary<string, DrvIO> showOutputsIOList11 = Base.GetOutputsIOList();
            Dictionary<string, DrvIO> showInputsIOList22 = new Dictionary<string, DrvIO>();
            Dictionary<string, DrvIO> showOutputsIOList22 = new Dictionary<string, DrvIO>();
            List<InputsStationParam> inputsStations = DBContext<InputsStationParam>.GetInstance().GetList();
            foreach (InputsStationParam input in inputsStations)
            {
                if (input.stationName == stationName)
                {
                    var result11 = (from input11 in showInputsIOList11 where input11.Key == input.CName select input11.Value).ToList<DrvIO>();
                    if (result11.Count() > 0)
                    {
                        showInputsIOList22.Add(input.CName, result11[0]);
                    }
                }
            }
            List<OutputsStationParam> outputsStations = DBContext<OutputsStationParam>.GetInstance().GetList();
            foreach (OutputsStationParam input in outputsStations)
            {
                if (input.stationName == stationName)
                {
                    var result11 = (from input11 in showOutputsIOList11 where input11.Key == input.CName select input11.Value).ToList<DrvIO>();
                    if (result11.Count() > 0)
                    {
                        showOutputsIOList22.Add(input.CName, result11[0]);
                    }
                }
            }
            showInputsIOList = showInputsIOList22;
            showOutputsIOList = showOutputsIOList22;
            Fun_DisplayInputList();
            Fun_DisplayOutputList();
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(this.Visible!=true)
            {
                return;
            }
            int i = 0, j = 0, nRetCode = 0;
            //-----------------------------------
            if (!pMachine.GetInitialStatus(ref nRetCode))
                return;
            //-----------------------------------
            DrvIO pRefshIO;

            #region Input Resfsh
            for (i = 0; i < listView_Input.Items.Count; i++)
            {
                foreach (var item in showInputsIOList)
                {
                    pRefshIO = (DrvIO)(item.Value).m_NowAddress;
                    pRefshIO.GetParameter(ref IOParameter);
                    //--------------------------
                    string StrText = IOParameter.strID + " " + IOParameter.strCName.Trim();
                    StrText = JudgeLanguage.JudgeLag(StrText);
                    if (listView_Input.Items[i].Text == StrText)
                    {
                        pRefshIO = (DrvIO)(item.Value).m_NowAddress;
                        if (pRefshIO.GetValue())
                        {
                            listView_Input.Items[i].ImageKey = "On";
                            listView_Input.Items[i].BackColor = Color.LimeGreen;
                        }
                        else
                        {
                            listView_Input.Items[i].ImageKey = "Off";
                            listView_Input.Items[i].BackColor = Button.DefaultBackColor;
                        }
                    }
                }
            }
            #endregion

            #region Output Resfsh
            for (i = 0; i < listView_Output.Items.Count; i++)
            {
                foreach (var item in showOutputsIOList)
                {
                    pRefshIO = (DrvIO)(item.Value).m_NowAddress;
                    pRefshIO.GetParameter(ref IOParameter);
                    //--------------------------
                    string StrText = IOParameter.strID + " " + IOParameter.strCName.Trim();
                    StrText = JudgeLanguage.JudgeLag(StrText);
                    if (listView_Output.Items[i].Text == StrText)
                    {
                        pRefshIO = (DrvIO)(item.Value).m_NowAddress;
                        if (pRefshIO.GetValue())
                        {
                            listView_Output.Items[i].ImageKey = "On";
                            listView_Output.Items[i].BackColor = Color.LimeGreen;
                        }
                        else
                        {
                            listView_Output.Items[i].ImageKey = "Off";
                            listView_Output.Items[i].BackColor = Button.DefaultBackColor;
                        }
                    }
                }
            }
            #endregion

        }

        private void listView_Output_MouseDoubleClick(object sender, EventArgs e)
        {
            int i, j, nRetCode = 0;
            //-----------------------------------
            if (!pMachine.GetInitialStatus(ref nRetCode))
                return;
            if (listView_Output.SelectedItems.Count <= 0)
                return;
            //-----------------------------------
            string SelectText = listView_Output.SelectedItems[0].Text;

            for (i = 0; i < listView_Output.Items.Count; i++)
            {
                foreach (var item in showOutputsIOList)
                {
                    pSelectIO = (DrvIO)(item.Value).m_NowAddress;
                    pSelectIO.GetParameter(ref IOParameter);
                    //--------------------------
                    string StrText = IOParameter.strID + " " + IOParameter.strCName.Trim();
                    StrText = JudgeLanguage.JudgeLag(StrText);
                    if (listView_Output.Items[i].Text == StrText && SelectText == StrText)
                    {
                        pSelectIO = (DrvIO)(item.Value).m_NowAddress;
                        bool bValue = pSelectIO.GetValue();
                        pSelectIO.SetIO(!bValue);
                    }
                }
            }
        }
    }
}
