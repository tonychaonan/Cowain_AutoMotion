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
    public partial class frm_GroupItem : Form
    {
        public List<IHardParam1> hardParams = new List<IHardParam1>();
        private List<IHardParam> hardParams1 = new List<IHardParam>();
        int type_Index = 0;
        string stationName = "";
        public frm_GroupItem()
        {
            InitializeComponent();
        }

        private void frm_GroupItem_Load(object sender, EventArgs e)
        {

        }
        public void load(int n, string stationName1, List<IHardParam1> hardParams2)
        {
            type_Index = n;
            stationName = stationName1;
            if (n == 0)
            {
                hardParams1 = DBContext<Inputs>.GetInstance().GetList().ToList<IHardParam>();
            }
            else if (n == 1)
            {
                hardParams1 = DBContext<Outputs>.GetInstance().GetList().ToList<IHardParam>();
            }
            else
            {
                hardParams1 = DBContext<Cylinders>.GetInstance().GetList().ToList<IHardParam>();
            }
            checkedListBox1.Items.Clear();
            List<IHardParam1> list = new List<IHardParam1>();
            foreach (IHardParam1 hardParam in hardParams2)
            {
                if(hardParam.stationName== stationName1)
                {
                    list.Add(hardParam);
                }
            }

            foreach (var item in hardParams1)
            {
                var result = (from param in list where param.CName == item.CName select param).ToList<IHardParam1>();
                if (result.Count > 0)
                {
                    checkedListBox1.Items.Add(item.CName, true);
                }
                else
                {
                    checkedListBox1.Items.Add(item.CName, false);
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frm_GroupItem_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (var item in checkedListBox1.CheckedItems)
            {
                IHardParam1 inputsStationParam = null;
                if (type_Index == 0)
                {
                    inputsStationParam = new InputsStationParam();
                }
                else if (type_Index == 1)
                {
                    inputsStationParam = new OutputsStationParam();
                }
                else
                {
                    inputsStationParam = new CylindersStationParam();
                }
                var result1 = (from param in hardParams1 where param.CName == item.ToString() select param).ToList<IHardParam>();
                inputsStationParam.CName = result1[0].CName;
                inputsStationParam.ID = result1[0].ID;
                inputsStationParam.stationName = stationName;
                hardParams.Add(inputsStationParam);
            }
        }
    }
}
