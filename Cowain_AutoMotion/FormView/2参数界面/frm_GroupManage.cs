using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Cowain_AutoMotion.SQLSugarHelper;

namespace Cowain_AutoMotion
{
    public partial class frm_GroupManage : Form
    {
        ImageList ImgList = new ImageList();
        List<StationParam> stationParams = new List<StationParam>();
        List<IHardParam1> hardParams = new List<IHardParam1>();
        public frm_GroupManage()
        {
            InitializeComponent();
            ImgList.Images.Add("Enable", Cowain_AutoMotion.Properties.Resources.SetOk);
            ImgList.Images.Add("DisEnable", Cowain_AutoMotion.Properties.Resources.SetOk_Disable);
        }

        private void cBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cBoxType.SelectedIndex == -1)
            {
                return;
            }
            listView_Station.Items.Clear();
            stationParams = DBContext<StationParam>.GetInstance().GetList().ToList<StationParam>();
            if (cBoxType.SelectedIndex == 0)
            {
                hardParams = DBContext<InputsStationParam>.GetInstance().GetList().ToList<IHardParam1>();
            }
            else if (cBoxType.SelectedIndex == 1)
            {
                hardParams = DBContext<OutputsStationParam>.GetInstance().GetList().ToList<IHardParam1>();
            }
            else
            {
                hardParams = DBContext<CylindersStationParam>.GetInstance().GetList().ToList<IHardParam1>();
            }
            listView_Hard.Items.Clear();
            listView_Station.LargeImageList = ImgList;
            listView_Station.SmallImageList = ImgList;
            for (int i = 0; i < stationParams.Count; i++)
            {
                listView_Station.Items.Add(stationParams[i].CName.Trim(), "DisEnable");
            }
        }

        private void listView_Station_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listView_Station.SelectedItems.Count==0)
            {
                return;
            }
            listView_Hard.LargeImageList = ImgList;
            listView_Hard.SmallImageList = ImgList;
            listView_Hard.Items.Clear();
            foreach (ListViewItem item in listView_Station.Items)
            {
                item.ImageKey = "DisEnable";
            }
            listView_Station.SelectedItems[0].ImageKey = "Enable";
            string stationName = listView_Station.SelectedItems[0].Text.Trim();
            foreach (var item in hardParams)
            {
                if (item.stationName.Trim() == stationName)
                {
                    listView_Hard.Items.Add(item.CName, "DisEnable");
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView_Hard.SelectedItems.Count == 0)
            {
                return;
            }
            foreach (ListViewItem item in listView_Hard.Items)
            {
                item.ImageKey = "DisEnable";
            }
            listView_Hard.SelectedItems[0].ImageKey = "Enable";
        }
        private int getOnlyIDForStation()
        {
            int id = 0;
            while (true)
            {
                bool b_Exist = false;
                foreach (var item11 in stationParams)
                {
                    if (id == Convert.ToInt32(item11.ID))
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
        private void button2_Click(object sender, EventArgs e)
        {
            if (listView_Station.SelectedItems.Count == 0)
            {
                return;
            }
            frm_GroupItem frm_GroupItem = new frm_GroupItem();
            frm_GroupItem.load(cBoxType.SelectedIndex, listView_Station.SelectedItems[0].Text.Trim(), hardParams);
            frm_GroupItem.ShowDialog();
            hardParams = frm_GroupItem.hardParams;
            listView_Hard.Items.Clear();
            foreach (var item in hardParams)
            {
                ListViewItem listViewItem = new ListViewItem(item.CName);
                listViewItem.ImageKey = "DisEnable";
                listView_Hard.Items.Add(listViewItem);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (listView_Hard.SelectedItems.Count > 0)
            {
                foreach (var item in hardParams)
                {
                    if (item.CName.Trim() == listView_Hard.SelectedItems[0].Text.ToString())
                    {
                        hardParams.Remove(item);
                        break;
                    }
                }
                listView_Hard.Items.Remove(listView_Hard.SelectedItems[0]);
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确认保存吗？", "警告", MessageBoxButtons.OKCancel);
            if (dr != DialogResult.OK)
            {
                return;
            }
            string stationName11 = listView_Station.SelectedItems[0].Text.Trim();
            if (cBoxType.SelectedIndex == 0)
            {
                List<InputsStationParam> inputsStations = DBContext<InputsStationParam>.GetInstance().GetList();
                var result1 = (from input1 in inputsStations where input1.CName == stationName11 select input1).ToList<InputsStationParam>();
                foreach (var item in result1)
                {
                    DBContext<InputsStationParam>.GetInstance().Delete(item);
                }
                foreach (var item in hardParams)
                {
                    DBContext<InputsStationParam>.GetInstance().UpdateOrInsert(new List<InputsStationParam>() { (InputsStationParam)item });
                }
            }
            else if (cBoxType.SelectedIndex == 1)
            {
                List<OutputsStationParam> inputsStations = DBContext<OutputsStationParam>.GetInstance().GetList();
                var result1 = (from input1 in inputsStations where input1.CName == stationName11 select input1).ToList<OutputsStationParam>();
                foreach (var item in result1)
                {
                    DBContext<OutputsStationParam>.GetInstance().Delete(item);
                }
                foreach (var item in hardParams)
                {
                    DBContext<OutputsStationParam>.GetInstance().UpdateOrInsert(new List<OutputsStationParam>() { (OutputsStationParam)item });
                }
            }
            else
            {
                List<CylindersStationParam> inputsStations = DBContext<CylindersStationParam>.GetInstance().GetList();
                var result1 = (from input1 in inputsStations where input1.CName == stationName11 select input1).ToList<CylindersStationParam>();
                foreach (var item in result1)
                {
                    DBContext<CylindersStationParam>.GetInstance().Delete(item);
                }
                foreach (var item in hardParams)
                {
                    DBContext<CylindersStationParam>.GetInstance().UpdateOrInsert(new List<CylindersStationParam>() { (CylindersStationParam)item });
                }
            }

        }
    }
}
