using Chart;
using Cowain_AutoMotion.Simulat;
using DevExpress.XtraPrinting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cowain_AutoMotion
{
    public partial class simulatFrm : Form
    {
        List<buttonAndStep> buttonAndSteps = new List<buttonAndStep>();
        public simulatFrm()
        {
            InitializeComponent();
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
          try
            {
                SimulatParamItem1 simulatParamItem11 = getSimulatParamItem1(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex]);
                selectCMDItemFrm selectCMDItemFrm = new selectCMDItemFrm(simulatParamItem11);
                selectCMDItemFrm.ShowDialog();
                string str = selectCMDItemFrm.setValue(ref simulatParamItem11);
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = str;
            }
            catch
            {

            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add();
            foreach (DataGridViewCell item1 in dataGridView1.Rows[dataGridView1.Rows.Count-1].Cells)
            {
                SimulatParamItem1 simulatParamItem11 = getSimulatParamItem1(item1);
                buttonAndStep buttonAndStep1 = new buttonAndStep(item1, simulatParamItem11);
                buttonAndSteps.Add(buttonAndStep1);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Index == -1)
            {
                return;
            }
            dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            int selectedRowIndex = GetSelectedRowIndex();
            if (selectedRowIndex >= 1)
            {
                DataGridViewRow newRow = dataGridView1.Rows[selectedRowIndex];
                dataGridView1.Rows.Remove(dataGridView1.Rows[selectedRowIndex]);
                dataGridView1.Rows.Insert(selectedRowIndex - 1, newRow);
                dataGridView1.Rows[selectedRowIndex - 1].Selected = true;
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            int selectedRowIndex = GetSelectedRowIndex();
            if (selectedRowIndex < dataGridView1.Rows.Count - 1)
            {
                DataGridViewRow newRow = dataGridView1.Rows[selectedRowIndex];
                dataGridView1.Rows.Remove(dataGridView1.Rows[selectedRowIndex]);
                dataGridView1.Rows.Insert(selectedRowIndex + 1, newRow);
                dataGridView1.Rows[selectedRowIndex + 1].Selected = true;
            }
        }
        private int GetSelectedRowIndex()
        {
            if (dataGridView1.Rows.Count == 0)
            {
                return 0;
            }
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Selected)
                {
                    return row.Index;
                }
            }
            return 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SimulatDataDefine.instance().simulatParam.simulatParamItems.Clear();
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                SimulatParamItem mySimulatParamItem = new SimulatParamItem();
                foreach (DataGridViewCell item11 in item.Cells)
                {
                    SimulatParamItem1 simulatParamItem111 = getSimulatParamItem1(item11);
                    mySimulatParamItem.simulatParamItem1s.Add(simulatParamItem111);
                }
                SimulatDataDefine.instance().simulatParam.simulatParamItems.Add(mySimulatParamItem);
            }
            SimulatDataDefine.instance().saveParam();
        }
        public SimulatParamItem1 getSimulatParamItem1(DataGridViewCell btn1)
        {
            SimulatParamItem1 simulatParamItem11 = new SimulatParamItem1();
            foreach (var item in buttonAndSteps)
            {
                if (btn1 == item.btn)
                {
                    simulatParamItem11 = item.simulatParamItem1;
                    break;
                }
            }
            return simulatParamItem11;
        }
        private void simulatFrm_Load(object sender, EventArgs e)
        {
            List<SimulatParamItem> lits = SimulatDataDefine.instance().simulatParam.simulatParamItems;
            foreach (var item in lits)
            {
                int index = 0;
                dataGridView1.Rows.Add();
                foreach (var item1 in item.simulatParamItem1s)
                {
                    buttonAndStep buttonAndStep1 = new buttonAndStep(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[index], item1);
                    buttonAndSteps.Add(buttonAndStep1);
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[index].Value = item1.getStr();
                    index++;
                }
            }
        }
    }
}
