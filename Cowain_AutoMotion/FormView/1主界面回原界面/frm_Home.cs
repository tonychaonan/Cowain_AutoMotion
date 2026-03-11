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
using ToolTotal;
using System.Threading;
using Cowain_AutoMotion.Flow;
using Cowain_AutoMotion.FormView;
using Cowain;
using System.Net;
using System.IO;
using Cowain_AutoMotion;
using Cowain_Machine;
using MotionBase;

namespace Cowain_Form.FormView
{
    public partial class frm_Home : Form
    {
        public frm_Home()
        {
            InitializeComponent();
        }
        public frm_Home(ref clsMachine pM)
        {
            InitializeComponent();
            pMachine = pM;
            //-------------------
            ImgList.Images.Add("HomeFail", Cowain_AutoMotion.Properties.Resources.Fail);
            ImgList.Images.Add("HomeSuccess", Cowain_AutoMotion.Properties.Resources.success);
        }
        public clsMachine pMachine;
        ImageList ImgList = new ImageList();
        int Type = -1;
        private void button1_Click(object sender, EventArgs e)
        {
            string strShowMessage = "归原";
            frm_Main.formError.ErrorUnit1.AddActionMessage(strShowMessage);
            LogAuto.Notify("窗体复位button按下", (int)MachineStation.归零, MotionLogLevel.Hint);
            pMachine.DoHome();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MachineDataDefine.Button_Stop = true;
            string strShowMessage = "归原停止";
            LogAuto.Notify("窗体回原停止button触发", (int)MachineStation.归零, MotionLogLevel.Alarm);
            frm_Main.formError.ErrorUnit1.AddActionMessage(strShowMessage);
            pMachine.Stop();
        }

        void Fun_DisplayParameter()
        {
            #region 版本判定
            String strFilePath = System.IO.Directory.GetCurrentDirectory();
            strFilePath = strFilePath + "\\Cowain_AutoMotion.exe";
            //-------------------------------
            System.IO.FileInfo fi = new System.IO.FileInfo(strFilePath);
            //---------
            String strTime = fi.LastWriteTime.ToShortDateString();
            //---------
            label_Ver.Text = JudgeLanguage.JudgeLag(" 软件 Release: ") + MESDataDefine.MESLXData.SW_Version;
            #endregion
        }

        private void frm_Home_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {

                Fun_DisplayParameter();
                //---------------
                timer_ReFlash.Enabled = true;
                //------------------------------
                listView_Home.Items.Clear();
                listView_Home.LargeImageList = ImgList;
                listView_Home.SmallImageList = ImgList;
                //----------------------------
                int index = 0;
                foreach (KeyValuePair<string, Base> item in WorkProcessLoad.instance.processList)
                {
                    if (item.Key.ToString() != "三色灯")
                    {
                        listView_Home.Items.Add(JudgeLanguage.JudgeLag(item.Key), "HomeFail");
                        index++;
                    }
                 
                }
            }
            else
            {
                timer_ReFlash.Enabled = false;
            }
        }

        private void timer_ReFlash_Tick(object sender, EventArgs e)
        {
            if (pMachine != null)
            {
                bool bHomeBtEnable = (pMachine.isIDLE()) ? true : false;
                button1.Enabled = bHomeBtEnable;
            }
            if (listView_Home.Items.Count > 0)
            {
                //回原  
                int index = 0;
                foreach (KeyValuePair<string, Base> item in WorkProcessLoad.instance.processList)
                {
                    if (item.Key.ToString() != "三色灯")
                    {
                        listView_Home.Items[index].ImageKey = item.Value.GetHomeCompleted() ? "HomeSuccess" : "HomeFail"; ;
                        index++;
                    }
                }
            }
        }


        private void frm_Home_Shown(object sender, EventArgs e)
        {

        }

        private void frm_Home_Load(object sender, EventArgs e)
        {
            //加载对应当站的图片及站名
            lbStationName.Text = MachineDataDefine.settingData.Station;
            if (MachineDataDefine.StationImage != null)
            {
                pictureBox1.Image = MachineDataDefine.StationImage;
            }
            LogAuto.Notify("打开点胶机软件", (int)MachineStation.主监控, MotionLogLevel.Info);
        }
    }
}
