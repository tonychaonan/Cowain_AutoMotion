using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cowain_AutoMotion.Flow;
using MotionBase;
using Cowain_Form.FormView;
using Chart;
using System.IO;
using System.Threading;
using Cowain_AutoMotion.Flow.Hive;

namespace Cowain_Machine.Flow
{
    public partial class frm_DownTime : Form
    {
        // public cDownTime downTime = new cDownTime();
        string[] str1 = new string[] { "  请选择停机原因：" };
        int tab_selectIndex = 0;

        string starttime = "";
        string stoptime = "";
        CheckBox _cb;
        public frm_DownTime(string Title, int number, CheckBox cb = null)
        {
            InitializeComponent();
            lb_showMessage.Text = "请选择停机原因";
            _cb = cb;
            bt_ok.Enabled = false;
            //InitialtabControl();
            if (_cb == null)
            {
                label1.Text = Title;
                BtVisible = new Button[] { bt_21, bt_24, bt_22, bt_23, bt_25, bt_26, bt_27 };
                //tabControl1.TabPages.Clear();
                //tabControl1.TabPages.Add(tabPage2);
                for (int i = 0; i < BtVisible.Length; i++)
                {
                    BtVisible[i].Visible = false;

                }
                if (number < 2)
                {
                    BtVisible[number].Visible = true;
                    lb_showMessage.Text = BtVisible[number].Text + "---时间到，请及时停机维护！";
                    Task.Run(() => {
                        Thread.Sleep(5000);
                        BtVisible[number].PerformClick();
                        Thread.Sleep(100);
                        bt_ok.PerformClick();
                    });
                }
                else if (number == 2)
                {
                    BtVisible[number].Visible = true;
                    BtVisible[number + 1].Visible = true;
                    lb_showMessage.Text = "胶水更换时间到，请及时停机更换！";
                }
                else
                {
                    BtVisible[number + 1].Visible = true;
                    lb_showMessage.Text = BtVisible[number + 1].Text + "---时间到，请及时停机维护！";
                    Task.Run(() => {
                        Thread.Sleep(5000);
                        BtVisible[number + 1].PerformClick();
                        Thread.Sleep(100);
                        bt_ok.PerformClick();
                    });
                }
                // InitialtabControl();
            }
        }
        private void InitialtabControl()
        {
            tabControl1.Controls.Clear();
            tabControl1.Controls.Add(tabPage3);
        }
        Button[] BtVisible;
        private void bt_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            string strBtName = bt.Name;

            savelog(bt.Text + "被点击了一次");

            tabControl1.Enabled = false;
            if (tabControl1.SelectedTab == tabPage1)
            {

                if (strBtName == "bt_12")
                {
                    tabControl1.TabPages.Clear();
                    tabControl1.TabPages.Add(tabPage1);
                    bt_ok.Enabled = true;
                    frm_Main.formData.ChartTime1.Engineering();

                    if (MachineDataDefine.machineState.b_UseHive && !HIVEDataDefine.Hive_machineState.IsSendErrorDown)
                    {
                        HIVE.HIVEInstance.HiveSendMACHINESTATE(3, "", "", "",false);
                    }
                    if (MachineDataDefine.machineState.b_UseRemoteQualification && !HIVEDataDefine.Hive_machineState.IsSendErrorDown)
                    {
                        HIVE.HIVEInstance.HiveSendMACHINESTATE(3, "", "", "",true);
                    }
                }
                else if (strBtName == "bt_13")
                {
                    tabControl1.TabPages.Clear();
                    tabControl1.TabPages.Add(tabPage2);
                    //tab_selectIndex = 1;
                    //tabControl1.SelectedTab = tabPage2;


                }


            }
            else if (tabControl1.SelectedTab == tabPage2)
            {

                if (strBtName == "bt_21")
                {

                    bt_ok.Enabled = true;
                    frm_Main.formData.ChartTime1.StartPlan_dt();

                    if (MachineDataDefine.machineState.b_UseHive && !HIVEDataDefine.Hive_machineState.IsSendErrorDown)
                    {
                     //   GantryParm.HIVEStateTime[0] = DateTime.Now.ToString("MM:dd:HH:mm:ss");

                        HIVE.HIVEInstance.HiveSendMACHINESTATE(4, "PD-01", "Check", "Daily Maintenance",false);

                        starttime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");
                        Random randomaa = new Random();
                        int temp_time = 1000;
                        temp_time = randomaa.Next(1000, 3000);
                        Thread.Sleep(temp_time);

                        stoptime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");

                        HIVE.HIVEInstance.HiveSendERRORDATA("PD-01", "Daily Maintenance", "Check", starttime, stoptime,false);
                    }

                    if (MachineDataDefine.machineState.b_UseRemoteQualification)
                    {
                        //   GantryParm.HIVEStateTime[0] = DateTime.Now.ToString("MM:dd:HH:mm:ss");

                        HIVE.HIVEInstance.HiveSendMACHINESTATE(4, "PD-01", "Check", "Daily Maintenance",true);

                        starttime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");
                        Random randomaa = new Random();
                        int temp_time = 1000;
                        temp_time = randomaa.Next(1000, 3000);
                        Thread.Sleep(temp_time);

                        stoptime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");

                        HIVE.HIVEInstance.HiveSendERRORDATA("PD-01", "Daily Maintenance", "Check", starttime, stoptime, true);
                    }
                }
                else if (strBtName == "bt_22")
                {

                    bt_ok.Enabled = true;
                    frm_Main.formData.ChartTime1.StartPlan_dt();

                  //  GantryParm.HIVEStateTime[2] = DateTime.Now.ToString("MM:dd:HH:mm:ss");
                    if (MachineDataDefine.machineState.b_UseHive && !HIVEDataDefine.Hive_machineState.IsSendErrorDown)
                    {
                        HIVE.HIVEInstance.HiveSendMACHINESTATE(4, "PD-02", "AB Glue", "Glue Change",false);


                        starttime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");
                        Random randomaa = new Random();
                        int temp_time = 1000;
                        temp_time = randomaa.Next(1000, 3000);
                        Thread.Sleep(temp_time);

                        stoptime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");

                        HIVE.HIVEInstance.HiveSendERRORDATA("PD-02", "Glue Change", "AB Glue", starttime, stoptime,false);

                    }

                    if (MachineDataDefine.machineState.b_UseRemoteQualification)
                    {
                        HIVE.HIVEInstance.HiveSendMACHINESTATE(4, "PD-02", "AB Glue", "Glue Change",true);


                        starttime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");
                        Random randomaa = new Random();
                        int temp_time = 1000;
                        temp_time = randomaa.Next(1000, 3000);
                        Thread.Sleep(temp_time);

                        stoptime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");

                        HIVE.HIVEInstance.HiveSendERRORDATA("PD-02", "Glue Change", "AB Glue", starttime, stoptime, false);

                    }
                }
                else if (strBtName == "bt_23")
                {

                    bt_ok.Enabled = true;
                    frm_Main.formData.ChartTime1.StartPlan_dt();

                  //  GantryParm.HIVEStateTime[2] = DateTime.Now.ToString("MM:dd:HH:mm:ss");
                    if (MachineDataDefine.machineState.b_UseHive && !HIVEDataDefine.Hive_machineState.IsSendErrorDown)
                    {
                        HIVE.HIVEInstance.HiveSendMACHINESTATE(4, "PD-02", "HM Gule", "Glue Change",false);

                        starttime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");
                        Random randomaa = new Random();
                        int temp_time = 1000;
                        temp_time = randomaa.Next(1000, 3000);
                        Thread.Sleep(temp_time);

                        stoptime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");

                        HIVE.HIVEInstance.HiveSendERRORDATA("PD-02", "Glue Change", "HM Gule", starttime, stoptime, false);
                    }

                    if (MachineDataDefine.machineState.b_UseRemoteQualification)
                    {
                        HIVE.HIVEInstance.HiveSendMACHINESTATE(4, "PD-02", "HM Gule", "Glue Change",true);

                        starttime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");
                        Random randomaa = new Random();
                        int temp_time = 1000;
                        temp_time = randomaa.Next(1000, 3000);
                        Thread.Sleep(temp_time);

                        stoptime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");

                        HIVE.HIVEInstance.HiveSendERRORDATA("PD-02", "Glue Change", "HM Gule", starttime, stoptime,true);
                    }
                }
                else if (strBtName == "bt_24")
                {

                    bt_ok.Enabled = true;
                    frm_Main.formData.ChartTime1.StartPlan_dt();
                  //  GantryParm.HIVEStateTime[1] = DateTime.Now.ToString("MM:dd:HH:mm:ss");

                    if (MachineDataDefine.machineState.b_UseHive && !HIVEDataDefine.Hive_machineState.IsSendErrorDown)
                    {
                        HIVE.HIVEInstance.HiveSendMACHINESTATE(4, "PD-03", "", "Needle Change",false);

                        starttime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");
                        Random randomaa = new Random();
                        int temp_time = 1000;
                        temp_time = randomaa.Next(1000, 3000);
                        Thread.Sleep(temp_time);

                        stoptime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");

                        HIVE.HIVEInstance.HiveSendERRORDATA("PD-03", "Needle Change", "", starttime, stoptime,false);
                    }
                    if (MachineDataDefine.machineState.b_UseRemoteQualification)
                    {
                        HIVE.HIVEInstance.HiveSendMACHINESTATE(4, "PD-03", "", "Needle Change",true);

                        starttime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");
                        Random randomaa = new Random();
                        int temp_time = 1000;
                        temp_time = randomaa.Next(1000, 3000);
                        Thread.Sleep(temp_time);

                        stoptime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");

                        HIVE.HIVEInstance.HiveSendERRORDATA("PD-03", "Needle Change", "", starttime, stoptime, true);
                    }
                }
                else if (strBtName == "bt_25")
                {

                    bt_ok.Enabled = true;
                    frm_Main.formData.ChartTime1.StartPlan_dt();

                  //  GantryParm.HIVEStateTime[3] = DateTime.Now.ToString("MM:dd:HH:mm:ss");
                    if (MachineDataDefine.machineState.b_UseHive && !HIVEDataDefine.Hive_machineState.IsSendErrorDown)
                    {
                        HIVE.HIVEInstance.HiveSendMACHINESTATE(4, "PD-04", "", "Valve Change",false);


                        starttime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");
                        Random randomaa = new Random();
                        int temp_time = 1000;
                        temp_time = randomaa.Next(1000, 3000);
                        Thread.Sleep(temp_time);

                        stoptime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");

                        HIVE.HIVEInstance.HiveSendERRORDATA("PD-04", "Valve Change", "", starttime, stoptime,false);

                    }
                    if (MachineDataDefine.machineState.b_UseRemoteQualification)
                    {
                        HIVE.HIVEInstance.HiveSendMACHINESTATE(4, "PD-04", "", "Valve Change",true);


                        starttime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");
                        Random randomaa = new Random();
                        int temp_time = 1000;
                        temp_time = randomaa.Next(1000, 3000);
                        Thread.Sleep(temp_time);

                        stoptime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");

                        HIVE.HIVEInstance.HiveSendERRORDATA("PD-04", "Valve Change", "", starttime, stoptime, true);

                    }
                }
                else if (strBtName == "bt_26")
                {

                    bt_ok.Enabled = true;
                    frm_Main.formData.ChartTime1.StartPlan_dt();

                  //  GantryParm.HIVEStateTime[4] = DateTime.Now.ToString("MM:dd:HH:mm:ss");
                    if (MachineDataDefine.machineState.b_UseHive && !HIVEDataDefine.Hive_machineState.IsSendErrorDown)
                    {
                        HIVE.HIVEInstance.HiveSendMACHINESTATE(4, "PD-07", "", "Consumable Part Replacement",false);

                        starttime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");
                        Random randomaa = new Random();
                        int temp_time = 1000;
                        temp_time = randomaa.Next(1000, 3000);
                        Thread.Sleep(temp_time);

                        stoptime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");

                        HIVE.HIVEInstance.HiveSendERRORDATA("PD-07", "Consumable Part Replacement", "", starttime, stoptime,false);

                    }
                    if (MachineDataDefine.machineState.b_UseRemoteQualification)
                    {
                        HIVE.HIVEInstance.HiveSendMACHINESTATE(4, "PD-07", "", "Consumable Part Replacement",true);

                        starttime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");
                        Random randomaa = new Random();
                        int temp_time = 1000;
                        temp_time = randomaa.Next(1000, 3000);
                        Thread.Sleep(temp_time);

                        stoptime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");

                        HIVE.HIVEInstance.HiveSendERRORDATA("PD-07", "Consumable Part Replacement", "", starttime, stoptime, true);

                    }
                }
                else if (strBtName == "bt_27")
                {

                    bt_ok.Enabled = true;
                    frm_Main.formData.ChartTime1.StartPlan_dt();

                 //   GantryParm.HIVEStateTime[5] = DateTime.Now.ToString("MM:dd:HH:mm:ss");
                    if (MachineDataDefine.machineState.b_UseHive && !HIVEDataDefine.Hive_machineState.IsSendErrorDown)
                    {
                        HIVE.HIVEInstance.HiveSendMACHINESTATE(4, "PD-101", "", "Others",false);

                        starttime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");
                        Random randomaa = new Random();
                        int temp_time = 1000;
                        temp_time = randomaa.Next(1000, 3000);
                        Thread.Sleep(temp_time);

                        stoptime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");

                        HIVE.HIVEInstance.HiveSendERRORDATA("PD-101", "Others", "", starttime, stoptime,false);
                    }
                    if (MachineDataDefine.machineState.b_UseRemoteQualification)
                    {
                        HIVE.HIVEInstance.HiveSendMACHINESTATE(4, "PD-101", "", "Others",true);

                        starttime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");
                        Random randomaa = new Random();
                        int temp_time = 1000;
                        temp_time = randomaa.Next(1000, 3000);
                        Thread.Sleep(temp_time);

                        stoptime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");

                        HIVE.HIVEInstance.HiveSendERRORDATA("PD-101", "Others", "", starttime, stoptime, true);
                    }
                }


            }

            tabControl1.Enabled = true;
        }





        private void bt_ok_Click(object sender, EventArgs e)
        {

            if (frm_Main.formData.ChartTime1.RunStatus != ChartTime.MachineStatus.engineering && frm_Main.formData.ChartTime1.RunStatus != ChartTime.MachineStatus.planned_downtime)
            {
                MessageBox.Show("必须选择一种停机类型");
                return;
            }
            if (_cb != null) _cb.Checked = false;
            this.Close();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void bt_back_Click(object sender, EventArgs e)
        {
            TabPage SelectedTabPage = null;
            if (tabControl1.SelectedTab == tabPage3)
                tab_selectIndex = 0;
            else
                tab_selectIndex = tabControl1.SelectedTab == tabPage1 ? 1 : 2;
            if (tab_selectIndex == 0)
            {
                //bt_back.Enabled = false;
            }
            else if (tab_selectIndex > 0)
            {
                tab_selectIndex--;
                tabControl1.TabPages.Clear();
                SelectedTabPage = tab_selectIndex == 0 ? tabPage3 : tabPage1;
                tabControl1.Controls.Add(SelectedTabPage);
            }
            // tabControl1.SelectedTab = tabPage1;
        }



        private void savelog(string message)
        {


            try
            {
                string fileName;
                fileName = string.Format("{0}.txt", DateTime.Now.ToString("yyyy_MM_dd"));
                string outputPath = @"D:\DATA\HIVE手动操作记录\计划停机";
                if (!Directory.Exists(outputPath))
                {
                    Directory.CreateDirectory(outputPath);
                }
                string fullFileName = Path.Combine(outputPath, fileName);
                System.IO.FileStream fs;
                //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
                StreamWriter sw;
                if (!File.Exists(fullFileName))
                {
                    fs = new System.IO.FileStream(fullFileName, System.IO.FileMode.Append, System.IO.FileAccess.Write, FileShare.Read);
                    //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
                    sw = new StreamWriter(fs, System.Text.Encoding.Default);
                    sw.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "   " + message);
                    sw.Close();
                    fs.Close();

                }
                else
                {
                    fs = new System.IO.FileStream(fullFileName, System.IO.FileMode.Append, System.IO.FileAccess.Write, FileShare.Read);
                    //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
                    sw = new StreamWriter(fs, System.Text.Encoding.Default);
                    sw.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "   " + message);
                    sw.Close();
                    fs.Close();
                }

            }
            catch
            {


            }

        }

        private void frm_DownTime_Load(object sender, EventArgs e)
        {
            tabControl1.TabPages.Clear();
            if (_cb != null)
                tabControl1.TabPages.Add(tabPage3);
            else
                tabControl1.TabPages.Add(tabPage2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tabControl1.TabPages.Clear();
            tabControl1.TabPages.Add(tabPage1);
        }

        private void button_BackGantry_Click(object sender, EventArgs e)
        {
            tabControl1.TabPages.Clear();
            tabControl1.TabPages.Add(tabPage1);
        }
    }
}
