using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MotionBase;
using Cowain_Machine.Flow;
using Cowain_AutoMotion.Flow;
using System.IO;
using Cowain;
using Cowain_AutoMotion;
using Sunny.UI;

namespace Cowain_Form.FormView
{
    public partial class frm_TeachParameter : Form
    {
        HardType type;
        List<string> currentCName = new List<string>();
        public enum HardType
        {
            Inputs,
            Outputs,
            Cylinders,
            Motors,
            PLCParam,
            SerialPortParam,
            SocketParam,
        }
        public frm_TeachParameter()
        {
            InitializeComponent();
        }
        private void frm_TeachParameter_Load(object sender, EventArgs e)
        {
            string[] names = Enum.GetNames(typeof(HardType));
            comboBox1.Items.AddRange(names);
            frm_GroupManage frm_GroupManage = new frm_GroupManage();
            frm_GroupManage.TopLevel = false;
            frm_GroupManage.Parent = tabPage1;
            frm_GroupManage.ControlBox = false;
            frm_GroupManage.FormBorderStyle = FormBorderStyle.None;
            frm_GroupManage.Dock = System.Windows.Forms.DockStyle.Fill;
            frm_GroupManage.Show();

            frm_PointsManage frm_PointsManage1 = new frm_PointsManage();
            frm_PointsManage1.TopLevel = false;
            frm_PointsManage1.Parent = tabPage2;
            frm_PointsManage1.ControlBox = false;
            frm_PointsManage1.FormBorderStyle = FormBorderStyle.None;
            frm_PointsManage1.Dock = System.Windows.Forms.DockStyle.Fill;
            frm_PointsManage1.Show();
            frm_ButtonManage frm_ButtonManage = new frm_ButtonManage();
            frm_ButtonManage.TopLevel = false;
            frm_ButtonManage.Parent = tabPage4;
            frm_ButtonManage.ControlBox = false;
            frm_ButtonManage.FormBorderStyle = FormBorderStyle.None;
            frm_ButtonManage.Dock = System.Windows.Forms.DockStyle.Fill;
            frm_ButtonManage.Show();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            type = HardType.Inputs;
            Enum.TryParse(comboBox1.SelectedItem.ToString(), out type);
            dataGridView1.Columns.Clear();
            panel_Node.Visible = false;
            switch (type)
            {
                case HardType.Inputs:
                    List<Inputs> inputs = SQLSugarHelper.DBContext<Inputs>.GetInstance().GetList();
                    if (inputs.Count > 0)
                    {
                        dataGridView1.DataSource = inputs;
                    }
                    else
                    {
                        dataGridView1.DataSource = new List<Inputs>() { new Inputs() };
                    }
                    panel_Node.Visible = true;
                    break;
                case HardType.Outputs:
                    List<Outputs> outputs = SQLSugarHelper.DBContext<Outputs>.GetInstance().GetList();
                    if (outputs.Count > 0)
                    {
                        dataGridView1.DataSource = outputs;
                    }
                    else
                    {
                        dataGridView1.DataSource = new List<Outputs>() { new Outputs() };
                    }
                    panel_Node.Visible = true;
                    break;
                case HardType.Cylinders:
                    List<Cylinders> cylinders = SQLSugarHelper.DBContext<Cylinders>.GetInstance().GetList();
                    if (cylinders.Count > 0)
                    {
                        dataGridView1.DataSource = cylinders;
                    }
                    else
                    {
                        dataGridView1.DataSource = new List<Cylinders>() { new Cylinders() };
                    }
                    break;
                case HardType.Motors:
                    List<Motors> motors = SQLSugarHelper.DBContext<Motors>.GetInstance().GetList();
                    if (motors.Count > 0)
                    {
                        dataGridView1.DataSource = motors;
                    }
                    else
                    {
                        dataGridView1.DataSource = new List<Motors>() { new Motors() };
                    }
                    break;
                case HardType.PLCParam:
                    List<PLCParam> plcParam = SQLSugarHelper.DBContext<PLCParam>.GetInstance().GetList();
                    if (plcParam.Count > 0)
                    {
                        dataGridView1.DataSource = plcParam;
                    }
                    else
                    {
                        dataGridView1.DataSource = new List<PLCParam>() { new PLCParam() };
                    }
                    break;
                case HardType.SerialPortParam:
                    List<SerialPortParam> serialPortParam = SQLSugarHelper.DBContext<SerialPortParam>.GetInstance().GetList();
                    if (serialPortParam.Count > 0)
                    {
                        dataGridView1.DataSource = serialPortParam;
                    }
                    else
                    {
                        dataGridView1.DataSource = new List<SerialPortParam>() { new SerialPortParam() };
                    }
                    break;
                case HardType.SocketParam:
                    List<SocketParam> socketParam = SQLSugarHelper.DBContext<SocketParam>.GetInstance().GetList();
                    if (socketParam.Count > 0)
                    {
                        dataGridView1.DataSource = socketParam;
                    }
                    else
                    {
                        dataGridView1.DataSource = new List<SocketParam>() { new SocketParam() };
                    }
                    break;
            }
            //更新名称
            currentCName.Clear();
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                currentCName.Add(item.Cells[1].Value.ToString());
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确认增加吗？", "警告", MessageBoxButtons.OKCancel);
            if (dr != DialogResult.OK)
            {
                return;
            }
            HardType type = HardType.Inputs;
            Enum.TryParse(comboBox1.SelectedItem.ToString(), out type);
            switch (type)
            {
                case HardType.Inputs:
                    addInputs();
                    break;
                case HardType.Outputs:
                    addOutputs();
                    break;
                case HardType.Cylinders:
                    List<Cylinders> cylinders = (List<Cylinders>)dataGridView1.DataSource;
                    cylinders.Add(new Cylinders());
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = cylinders;
                    break;
                case HardType.Motors:
                    List<Motors> motors = (List<Motors>)dataGridView1.DataSource;
                    motors.Add(new Motors());
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = motors;
                    break;
                case HardType.PLCParam:
                    List<PLCParam> pLCParam = (List<PLCParam>)dataGridView1.DataSource;
                    pLCParam.Add(new PLCParam());
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = pLCParam;
                    break;
                case HardType.SerialPortParam:
                    List<SerialPortParam> serialPortParam = (List<SerialPortParam>)dataGridView1.DataSource;
                    serialPortParam.Add(new SerialPortParam());
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = serialPortParam;
                    break;
                case HardType.SocketParam:
                    List<SocketParam> socketParam = (List<SocketParam>)dataGridView1.DataSource;
                    socketParam.Add(new SocketParam());
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = socketParam;
                    break;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确认删除吗？", "警告", MessageBoxButtons.OKCancel);
            if (dr != DialogResult.OK)
            {
                return;
            }
            switch (type)
            {
                case HardType.Inputs:
                case HardType.Outputs:
                    deleteInputsAndOutPuts();
                    break;
                default:
                    deleteParam();
                    break;
            }
        }
        private void deleteInputsAndOutPuts()
        {
            if ((txtNode.Text.Contains("X") != true || txtNode.Text.Contains("Y") != true) || txtNode.Text.Length != 4)
            {
                MessageBox.Show("格式错误");
                return;
            }
            List<IHardParam> inputs = (List<IHardParam>)dataGridView1.DataSource;
            while (true)
            {
                bool b_Exist = false;
                foreach (var item in inputs)
                {
                    string id = item.ID;
                    if (id.Length == 5)
                    {
                        id = "0" + id;
                    }
                    if (id.Substring(0, 4) == txtNode.Text)
                    {
                        b_Exist = true;
                        inputs.Remove(item);
                        break;
                    }
                }
                if (b_Exist == false)
                {
                    break;
                }
            }
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = inputs;
        }
        private void deleteParam()
        {
            if (dataGridView1.SelectedRows.Count <= 0)
            {
                MessageBox.Show("请先选中参数");
                return;
            }
            List<IHardParam> hardParams = (List<IHardParam>)dataGridView1.DataSource;
            foreach (DataGridViewRow item in dataGridView1.SelectedRows)
            {
                hardParams.RemoveAt(item.Index);
            }
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = hardParams;
        }
        private void addInputs()
        {
            if (txtNode.Text.Contains("X") != true || (txtNode.Text.Length != 3 && txtNode.Text.Length != 4))
            {
                MessageBox.Show("格式错误");
                return;
            }
            List<string> currentNodes = new List<string>();
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                string value = item.Cells[0].Value.ToString();
                if (value.Length == 5)
                {
                    value = "0" + value;
                }
                if (currentNodes.Contains(value) != true)
                {
                    currentNodes.Add(value);
                }
            }
            List<Inputs> inputs = (List<Inputs>)dataGridView1.DataSource;

            for (int i = 0; i < 16; i++)
            {
                string input = txtNode.Text + i.ToString("00");
                if (currentNodes.Contains(input) != true)
                {
                    Inputs inputs1 = new Inputs();
                    inputs1.ID = input;
                    inputs1.CName = getOnlyName();
                    inputs.Add(inputs1);
                }
            }
            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = inputs;
        }
        private void addOutputs()
        {
            if (txtNode.Text.Contains("Y") != true || (txtNode.Text.Length != 3 && txtNode.Text.Length != 4))
            {
                MessageBox.Show("格式错误");
                return;
            }
            List<string> currentNodes = new List<string>();
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                string value = item.Cells[0].Value.ToString();
                if (value.Length == 5)
                {
                    value = "0" + value;
                }
                if (currentNodes.Contains(value) != true)
                {
                    currentNodes.Add(value);
                }
            }
            List<Outputs> inputs = (List<Outputs>)dataGridView1.DataSource;

            for (int i = 0; i < 16; i++)
            {
                string input = txtNode.Text + i.ToString("00");
                if (currentNodes.Contains(input) != true)
                {
                    Outputs inputs1 = new Outputs();
                    inputs1.ID = input;
                    inputs1.CName = getOnlyName();
                    inputs.Add(inputs1);
                }
            }
            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = inputs;
        }
        private string getOnlyName()
        {
            int n = 0;
            while (true)
            {
                string name = "备用" + n++.ToString();
                if (currentCName.Contains(name) != true)
                {
                    currentCName.Add(name);
                    return name;
                }
            }

        }
        private void btn_SaveData_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确认保存吗？", "警告", MessageBoxButtons.OKCancel);
            if (dr != DialogResult.OK)
            {
                return;
            }
            try
            {
                saveData();
                comboBox1_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void saveData()
        {
            switch (type)
            {
                case HardType.Inputs:
                    {
                        List<Inputs> inputs = (List<Inputs>)dataGridView1.DataSource;
                        List<Inputs> inputs_MDB = SQLSugarHelper.DBContext<Inputs>.GetInstance().GetList();
                        foreach (Inputs input1 in inputs)
                        {
                            if (inputs_MDB.Select(x => x.ID).Distinct().ToList().Contains(input1.ID))
                            {
                                SQLSugarHelper.DBContext<Inputs>.GetInstance().Update(input1);//包含的数据，只更新
                            }
                            else
                            {
                                SQLSugarHelper.DBContext<Inputs>.GetInstance().Insert(input1);
                            }
                        }
                        foreach (Inputs input1 in inputs_MDB)
                        {
                            if (inputs.Select(x => x.ID).Distinct().ToList().Contains(input1.ID)!=true)
                            {
                                SQLSugarHelper.DBContext<Inputs>.GetInstance().Delete(input1);
                            }
                        }
                    }
                    break;
                case HardType.Outputs:
                    {
                        List<Outputs> inputs = (List<Outputs>)dataGridView1.DataSource;
                        List<Outputs> inputs_MDB = SQLSugarHelper.DBContext<Outputs>.GetInstance().GetList();
                        dataGridView1.DataSource = SQLSugarHelper.DBContext<Outputs>.GetInstance().GetList();
                        foreach (Outputs input1 in inputs)
                        {
                            if (inputs_MDB.Select(x => x.ID).Distinct().ToList().Contains(input1.ID))
                            {
                                SQLSugarHelper.DBContext<Outputs>.GetInstance().Update(input1);//包含的数据，只更新
                            }
                            else
                            {
                                SQLSugarHelper.DBContext<Outputs>.GetInstance().Insert(input1);
                            }
                        }
                        foreach (Outputs input1 in inputs_MDB)
                        {
                            if (inputs.Select(x => x.ID).Distinct().ToList().Contains(input1.ID) != true)
                            {
                                SQLSugarHelper.DBContext<Outputs>.GetInstance().Delete(input1);
                            }
                        }
                    }
                    break;
                case HardType.Cylinders:
                    {
                        List<Cylinders> inputs = (List<Cylinders>)dataGridView1.DataSource;
                        List<Cylinders> inputs_MDB = SQLSugarHelper.DBContext<Cylinders>.GetInstance().GetList();
                        foreach (Cylinders input1 in inputs)
                        {
                            var result = from r in inputs_MDB where (r.CName == input1.CName) select r;
                            if (result.Count() > 0)
                            {
                                SQLSugarHelper.DBContext<Cylinders>.GetInstance().Update(input1);//包含的数据，只更新
                            }
                            else
                            {
                                SQLSugarHelper.DBContext<Cylinders>.GetInstance().Insert(input1);
                            }
                        }
                        foreach (Cylinders input1 in inputs_MDB)
                        {
                            var result = from r in inputs where (r.CName == input1.CName) select r;
                            if (result.Count() == 0)
                            {
                                SQLSugarHelper.DBContext<Cylinders>.GetInstance().Delete(input1);
                            }
                        }
                    }
                    break;
                case HardType.Motors:
                    {
                        List<Motors> inputs = (List<Motors>)dataGridView1.DataSource;
                        List<Motors> inputs_MDB = SQLSugarHelper.DBContext<Motors>.GetInstance().GetList();
                        foreach (Motors input1 in inputs)
                        {
                            if (inputs_MDB.Select(x => x.CName).Distinct().ToList().Contains(input1.CName))
                            {
                                SQLSugarHelper.DBContext<Motors>.GetInstance().Update(input1);//包含的数据，只更新
                            }
                            else
                            {
                                SQLSugarHelper.DBContext<Motors>.GetInstance().Insert(input1);
                            }
                        }
                        foreach (Motors input1 in inputs_MDB)
                        {
                            if (inputs.Select(x => x.CName).Distinct().Contains(input1.CName) != true)
                            {
                                SQLSugarHelper.DBContext<Motors>.GetInstance().Delete(input1);
                            }
                        }
                    }
                    break;
                case HardType.PLCParam:
                    {
                        List<PLCParam> inputs = (List<PLCParam>)dataGridView1.DataSource;
                        List<PLCParam> inputs_MDB = SQLSugarHelper.DBContext<PLCParam>.GetInstance().GetList();
                        foreach (PLCParam input1 in inputs)
                        {
                            if (inputs_MDB.Select(x => x.CName).Distinct().ToList().Contains(input1.CName))
                            {
                                SQLSugarHelper.DBContext<PLCParam>.GetInstance().Update(input1);//包含的数据，只更新
                            }
                            else
                            {
                                SQLSugarHelper.DBContext<PLCParam>.GetInstance().Insert(input1);
                            }
                        }
                        foreach (PLCParam input1 in inputs_MDB)
                        {
                            if (inputs.Select(x => x.CName).Distinct().ToList().Contains(input1.CName) != true)
                            {
                                SQLSugarHelper.DBContext<PLCParam>.GetInstance().Delete(input1);
                            }
                        }
                    }
                    break;
                case HardType.SerialPortParam:
                    {
                        List<SerialPortParam> inputs = (List<SerialPortParam>)dataGridView1.DataSource;
                        List<SerialPortParam> inputs_MDB = SQLSugarHelper.DBContext<SerialPortParam>.GetInstance().GetList();
                        foreach (SerialPortParam input1 in inputs)
                        {
                            if (inputs_MDB.Select(x => x.CName).Distinct().ToList().Contains(input1.CName))
                            {
                                SQLSugarHelper.DBContext<SerialPortParam>.GetInstance().Update(input1);//包含的数据，只更新
                            }
                            else
                            {
                                SQLSugarHelper.DBContext<SerialPortParam>.GetInstance().Insert(input1);
                            }
                        }
                        foreach (SerialPortParam input1 in inputs_MDB)
                        {
                            if (inputs.Select(x => x.CName).Distinct().ToList().Contains(input1.CName) != true)
                            {
                                SQLSugarHelper.DBContext<SerialPortParam>.GetInstance().Delete(input1);
                            }
                        }
                    }
                    break;
                case HardType.SocketParam:
                    {
                        List<SocketParam> inputs = (List<SocketParam>)dataGridView1.DataSource;
                        List<SocketParam> inputs_MDB = SQLSugarHelper.DBContext<SocketParam>.GetInstance().GetList();
                        foreach (SocketParam input1 in inputs)
                        {
                            if (inputs_MDB.Select(x => x.CName).Distinct().ToList().Contains(input1.CName))
                            {
                                SQLSugarHelper.DBContext<SocketParam>.GetInstance().Update(input1);//包含的数据，只更新
                            }
                            else
                            {
                                SQLSugarHelper.DBContext<SocketParam>.GetInstance().Insert(input1);
                            }
                        }
                        foreach (SocketParam input1 in inputs_MDB)
                        {
                            if (inputs.Select(x => x.CName).Distinct().ToList().Contains(input1.CName) != true)
                            {
                                SQLSugarHelper.DBContext<SocketParam>.GetInstance().Delete(input1);
                            }
                        }
                    }
                    break;
            }
        }
    }
}