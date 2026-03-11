using Cowain_AutoMotion.FormView._4弹窗;
using Cowain_Machine;
using MotionBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Cowain_AutoMotion.SQLSugarHelper;
using System.Runtime.InteropServices;

namespace Cowain_AutoMotion
{
    public partial class frm_PointsManage : Form
    {
        private const int SB_HORZ = 0;
        private const int SB_VERT = 1;
        private const int SB_CTL = 2;
        private const int SB_BOTH = 3;
        List<StationParam> stationParams = new List<StationParam>();
        List<MachineData> machineDatas = new List<MachineData>();
        List<ComboBox> comboBoxes = new List<ComboBox>();
        ImageList ImgList = new ImageList();

        StationParam currentStationParam = null;
        MachineData currentMachineData = null;
        public frm_PointsManage()
        {
            InitializeComponent();
            ImgList.Images.Add("Enable", Cowain_AutoMotion.Properties.Resources.SetOk);
            ImgList.Images.Add("DisEnable", Cowain_AutoMotion.Properties.Resources.SetOk_Disable);
        }
        private void frm_PointsManage_Load(object sender, EventArgs e)
        {
            listView_Station.LargeImageList = ImgList;
            listView_Station.SmallImageList = ImgList;
            listView_Points.LargeImageList = ImgList;
            listView_Points.SmallImageList = ImgList;
        stationParams = DBContext<StationParam>.GetInstance().GetList();
            machineDatas = DBContext<MachineData>.GetInstance().GetList();
            for (int i = 0; i < stationParams.Count; i++)
            {
                listView_Station.Items.Add(stationParams[i].CName.Trim(), "DisEnable");
            }
            comboBoxes.Add(cBox_X);
            comboBoxes.Add(cBox_Y);
            comboBoxes.Add(cBox_Z);
            comboBoxes.Add(cBox_R);
            comboBoxes.Add(cBox_A);
            for (int i = 0; i < comboBoxes.Count; i++)
            {
                comboBoxes[i].Items.Add("null");
            }
            foreach (var item in Base.GetMotorList())
            {
                for (int i = 0; i < comboBoxes.Count; i++)
                {
                    comboBoxes[i].Items.Add(item.Key);
                }
            }
        }

        private void btnAddStation_Click(object sender, EventArgs e)
        {
            dia_AddNewMSG dia_AddNewMSG = new dia_AddNewMSG();
            dia_AddNewMSG.ShowDialog();
            if (dia_AddNewMSG.MSG != "")
            {
                //判断是否有重复的
                bool b_Exist = false;
                foreach (var item in stationParams)
                {
                    if (item.CName.Trim() == dia_AddNewMSG.MSG)
                    {
                        MessageBox.Show("名称重复,添加失败!");
                        b_Exist = true;
                    }
                }
                if (!b_Exist)
                {
                    StationParam stationParam = new StationParam();
                    stationParam.CName = dia_AddNewMSG.MSG;
                    stationParam.ID = getOnlyIDForStation();
                    stationParams.Add(stationParam);
                    listView_Station.Items.Add(dia_AddNewMSG.MSG);
                }
            }
        }
        private void btnDelStation_Click(object sender, EventArgs e)
        {
            if (listView_Station.SelectedItems.Count > 0)
            {
                foreach (var item in stationParams)
                {
                    if (item.CName.Trim() == listView_Station.SelectedItems[0].Text.ToString())
                    {
                        stationParams.Remove(item);
                        currentStationParam = null;
                        break;
                    }
                }
                listView_Station.Items.Remove(listView_Station.SelectedItems[0]);
                listView_Points.Items.Clear();
            }
        }

        private void listView_Station_Click(object sender, EventArgs e)
        {
            if (listView_Station.SelectedItems.Count == 0)
            {
                return;
            }
            currentStationParam = null;
            foreach (ListViewItem item in listView_Station.Items)
            {
                item.ImageKey = "DisEnable";
            }
            listView_Station.SelectedItems[0].ImageKey = "Enable";
            string stationName = listView_Station.SelectedItems[0].Text.Trim();
            foreach (var item in stationParams)
            {
                if (item.CName.Trim() == stationName)
                {
                    currentStationParam = item;
                    //加载点位
                    listView_Points.Items.Clear();
                    cBoxSafePoint.Items.Clear();
                    foreach (var item11 in machineDatas)
                    {
                        if (item11.StationName.Trim() == stationName)
                        {
                            listView_Points.Items.Add(item11.CName, "DisEnable");
                            cBoxSafePoint.Items.Add(item11.CName);
                        }
                    }
                    if (cBox_X.Items.Contains(item.XData1.Trim()))
                    {
                        cBox_X.Text = item.XData1.Trim();
                    }
                    else
                    {
                        cBox_X.Text = "";
                    }
                    if (cBox_Y.Items.Contains(item.YData2.Trim()))
                    {
                        cBox_Y.Text = item.YData2.Trim();
                    }
                    else
                    {
                        cBox_Y.Text = "";
                    }
                    if (cBox_Z.Items.Contains(item.ZData3.Trim()))
                    {
                        cBox_Z.Text = item.ZData3.Trim();
                    }
                    else
                    {
                        cBox_Z.Text = "";
                    }
                    if (cBox_R.Items.Contains(item.RData4.Trim()))
                    {
                        cBox_R.Text = item.RData4.Trim();
                    }
                    else
                    {
                        cBox_R.Text = "";
                    }
                    if (cBox_A.Items.Contains(item.AData5.Trim()))
                    {
                        cBox_A.Text = item.AData5.Trim();
                    }
                    else
                    {
                        cBox_A.Text = "";
                    }
                }
            }
            if (stationName == "所有集合")
            {
                listView_Points.Items.Clear();
                foreach (var item in stationParams)
                {
                    if (item.CName.Trim() == "所有集合")
                    {
                        currentStationParam = item;
                        break;
                    }
                }
                foreach (var item11 in machineDatas)
                {
                    listView_Points.Items.Add(item11.CName, "DisEnable");
                }
            }
        }
        private void listView_Points_Click(object sender, EventArgs e)
        {
            if (listView_Points.SelectedItems.Count == 0)
            {
                return;
            }
            currentMachineData = null;
            foreach (ListViewItem item in listView_Points.Items)
            {
                item.ImageKey = "DisEnable";
            }
            listView_Points.SelectedItems[0].ImageKey = "Enable";
            string pointName = listView_Points.SelectedItems[0].Text.ToString();
            MachineData machineData = new MachineData();
            foreach (var item in machineDatas)
            {
                if (item.CName.Trim() == pointName)
                {
                    machineData = item;
                    currentMachineData = item;
                    break;
                }
            }
            cBox_PopX.Text = ((int)machineData.PriorityData1).ToString();
            cBox_PopY.Text = ((int)machineData.PriorityData2).ToString();
            cBox_PopZ.Text = ((int)machineData.PriorityData3).ToString();
            cBox_PopR.Text = ((int)machineData.PriorityData4).ToString();
            cBox_PopA.Text = ((int)machineData.PriorityData5).ToString();
            numXSpeed.Value = (int)machineData.Data1Speed;
            numYSpeed.Value = (int)machineData.Data2Speed;
            numZSpeed.Value = (int)machineData.Data3Speed;
            numRSpeed.Value = (int)machineData.Data4Speed;
            numASpeed.Value = (int)machineData.Data5Speed;
            cBoxNoEnablePoint.Checked = machineData.Enable == 1 ? false : true;
            cBoxNoUseX.Checked = machineData.Data1NoUse == 1 ? true : false;
            cBoxNoUseY.Checked = machineData.Data2NoUse == 1 ? true : false;
            cBoxNoUseZ.Checked = machineData.Data3NoUse == 1 ? true : false;
            cBoxNoUseR.Checked = machineData.Data4NoUse == 1 ? true : false;
            cBoxNoUseA.Checked = machineData.Data5NoUse == 1 ? true : false;
            cBoxZSafe.Checked = machineData.ZToSafe == 1 ? true : false;
            txtPoint1.Text = machineData.Data1.ToString();
            txtPoint2.Text = machineData.Data2.ToString();
            txtPoint3.Text = machineData.Data3.ToString();
            txtPoint4.Text = machineData.Data4.ToString();
            txtPoint5.Text = machineData.Data5.ToString();
            cBoxSafePoint.Text = machineData.ZSafePoint.ToString();
        }
        private int getOnlyIDForPoint()
        {
            int id = 0;
            while (true)
            {
                bool b_Exist = false;
                foreach (var item11 in BaseDataDefine.machineDatas)
                {
                    if (id == item11.ID)
                    {
                        b_Exist = true;
                    }
                }
                if (b_Exist)
                {
                    id++;
                }
                else
                {
                    return id;
                }
            }
        }
        private int getOnlyIDForStation()
        {
            int id = 0;
            while (true)
            {
                bool b_Exist = false;
                foreach (var item11 in BaseDataDefine.stationParams)
                {
                    if (id == item11.ID)
                    {
                        b_Exist = true;
                        break;
                    }
                }
                if (b_Exist)
                {
                    id++;
                }
                else
                {
                    return id;
                }
            }
        }
        private void btn_Save_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确认保存吗？", "警告", MessageBoxButtons.OKCancel);
            if (dr == DialogResult.OK)
            {
                try
                {
                    //保存工位
                    if (currentStationParam != null)
                    {
                        currentStationParam.XData1 = comboBoxes[0].Text;
                        currentStationParam.YData2 = comboBoxes[1].Text;
                        currentStationParam.ZData3 = comboBoxes[2].Text;
                        currentStationParam.RData4 = comboBoxes[3].Text;
                        currentStationParam.AData5 = comboBoxes[4].Text;
                    }
                    //如果现有集合比原有集合多数据，则新增
                    foreach (var item in stationParams)
                    {
                        bool b_Exist = false;
                        foreach (var item11 in BaseDataDefine.stationParams)
                        {
                            if (item.CName.Trim() == item11.CName.Trim())
                            {
                                b_Exist = true;
                            }
                        }
                        if (b_Exist != true)
                        {
                            DBContext<StationParam>.GetInstance().Insert(item);
                        }
                    }
                    //如果现有集合比原有集合少数据，则删除
                    foreach (var item in BaseDataDefine.stationParams)
                    {
                        bool b_Exist = false;
                        foreach (var item11 in stationParams)
                        {
                            if (item.CName.Trim() == item11.CName.Trim())
                            {
                                b_Exist = true;
                            }
                        }
                        if (b_Exist != true)
                        {
                            DBContext<StationParam>.GetInstance().Delete(item);
                        }
                    }
                    DBContext<StationParam>.GetInstance().Update(stationParams);
                    BaseDataDefine.stationParams = DBContext<StationParam>.GetInstance().GetList();
                    if (currentMachineData != null)
                    {
                        currentMachineData.StationName = currentStationParam.CName.Trim();
                        currentMachineData.PriorityData1 = Convert.ToDouble(cBox_PopX.Text);
                        currentMachineData.PriorityData2 = Convert.ToDouble(cBox_PopY.Text);
                        currentMachineData.PriorityData3 = Convert.ToDouble(cBox_PopZ.Text);
                        currentMachineData.PriorityData4 = Convert.ToDouble(cBox_PopR.Text);
                        currentMachineData.PriorityData5 = Convert.ToDouble(cBox_PopA.Text);
                        currentMachineData.Data1Speed = (double)numXSpeed.Value;
                        currentMachineData.Data2Speed = (double)numYSpeed.Value;
                        currentMachineData.Data3Speed = (double)numZSpeed.Value;
                        currentMachineData.Data4Speed = (double)numRSpeed.Value;
                        currentMachineData.Data5Speed = (double)numASpeed.Value;
                        currentMachineData.Enable = cBoxNoEnablePoint.Checked ? 0 : 1;
                        currentMachineData.Data1NoUse = cBoxNoUseX.Checked ? 1 : 0;
                        currentMachineData.Data2NoUse = cBoxNoUseY.Checked ? 1 : 0;
                        currentMachineData.Data3NoUse = cBoxNoUseZ.Checked ? 1 : 0;
                        currentMachineData.Data4NoUse = cBoxNoUseR.Checked ? 1 : 0;
                        currentMachineData.Data5NoUse = cBoxNoUseA.Checked ? 1 : 0;
                        currentMachineData.ZToSafe = cBoxZSafe.Checked ? 1 : 0;
                        currentMachineData.Data1 = Convert.ToDouble(txtPoint1.Text);
                        currentMachineData.Data2 = Convert.ToDouble(txtPoint2.Text);
                        currentMachineData.Data3 = Convert.ToDouble(txtPoint3.Text);
                        currentMachineData.Data4 = Convert.ToDouble(txtPoint4.Text);
                        currentMachineData.Data5 = Convert.ToDouble(txtPoint5.Text);
                        currentMachineData.ZSafePoint = cBoxSafePoint.Text;
                    }
                    //如果现有集合比原有集合多数据，则新增
                    foreach (var item in machineDatas)
                    {
                        bool b_Exist = false;
                        foreach (var item11 in BaseDataDefine.machineDatas)
                        {
                            if (item.CName.Trim() == item11.CName.Trim())
                            {
                                b_Exist = true;
                            }
                        }
                        if (b_Exist != true)
                        {
                            DBContext<MachineData>.GetInstance().Insert(item);
                        }
                    }
                    //如果现有集合比原有集合少数据，则删除
                    foreach (var item in BaseDataDefine.machineDatas)
                    {
                        bool b_Exist = false;
                        foreach (var item11 in machineDatas)
                        {
                            if (item.CName.Trim() == item11.CName.Trim())
                            {
                                b_Exist = true;
                            }
                        }
                        if (b_Exist != true)
                        {
                            DBContext<MachineData>.GetInstance().Delete(item);
                        }
                    }
                    DBContext<MachineData>.GetInstance().Update(machineDatas);
                    BaseDataDefine.machineDatas = DBContext<MachineData>.GetInstance().GetList();
                }
                catch (Exception e1)
                {
                    MessageBox.Show("保存失败" + e1.Message);
                }
            }
        }

        private void btnAddPoint_Click(object sender, EventArgs e)
        {
            dia_AddNewMSG dia_AddNewMSG = new dia_AddNewMSG();
            dia_AddNewMSG.ShowDialog();
            if (dia_AddNewMSG.MSG != "")
            {
                //判断是否有重复的
                bool b_Exist = false;
                foreach (var item in machineDatas)
                {
                    if (item.CName.Trim() == dia_AddNewMSG.MSG)
                    {
                        MessageBox.Show("名称重复,添加失败!");
                        b_Exist = true;
                    }
                }
                if (!b_Exist)
                {
                    MachineData machineData = new MachineData();
                    machineData.CName = dia_AddNewMSG.MSG;
                    machineData.ID = getOnlyIDForPoint();
                    machineData.StationName = currentStationParam.CName.Trim();
                    machineDatas.Add(machineData);
                    listView_Points.Items.Add(dia_AddNewMSG.MSG);
                    cBoxSafePoint.Items.Add(dia_AddNewMSG.MSG);
                }
            }
        }

        private void btnDelePoint_Click(object sender, EventArgs e)
        {
            if (listView_Points.SelectedItems.Count == 0)
            {
                return;
            }
            string pointName = listView_Points.SelectedItems[0].Text.ToString();
            foreach (var item in machineDatas)
            {
                if (pointName == item.CName.Trim())
                {
                    machineDatas.Remove(item);
                    break;
                }
            }
            int index = listView_Points.SelectedItems[0].Index;
            listView_Points.Items.Remove(listView_Points.SelectedItems[0]);
            cBoxSafePoint.Items.RemoveAt(index);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> total = new List<string>();
                total.Add("输入-----------");
                foreach (var item in Base.GetInputsIOList())
                {
                    total.Add(item.Key + ",");
                }
                total.Add("输出-----------");
                foreach (var item in Base.GetOutputsIOList())
                {
                    total.Add(item.Key + ",");
                }
                total.Add("轴-----------");
                foreach (var item in Base.GetMotorList())
                {
                    total.Add(item.Key + ",");
                }
                total.Add("气缸-----------");
                foreach (var item in Base.GetValveList())
                {
                    total.Add(item.Key + ",");
                }
                total.Add("点位-----------");
                foreach (var item in BaseDataDefine.machineDatas)
                {
                    total.Add(item.CName + ",");
                }
                string path = @"D:\hardParam.txt";
                File.AppendAllLines(path, total);
                MessageBox.Show("参数已经保存在 " + @"D:\hardParam.txt");
            }
            catch (Exception e11)
            {
                MessageBox.Show("参数保存失败 " + e11.Message);
            }
        }

        private void cBoxSafePoint_Click(object sender, EventArgs e)
        {
           
        }
    }
}
