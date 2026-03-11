using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Cowain_AutoMotion.ButtonList;

namespace Cowain_AutoMotion
{
    public partial class frm_ButtonManageItem : Form
    {
        ButtonType buttonType1;
        bool isInputs = true;
        string columnName = "";
        ComboBox currentComboBox = null;
        public frm_ButtonManageItem(string columnName1, ButtonType buttonType)
        {
            InitializeComponent();
            columnName = columnName1;
            buttonType1 = buttonType;
            label1.Text = buttonType.ToString();
            if(buttonType== ButtonType.开始按钮灯|| buttonType == ButtonType.暂停按钮灯|| buttonType == ButtonType.停止按钮灯)
            {
                isInputs = false;
            }
            List<string> strs = new List<string>();
            switch (buttonType)
            {
                case ButtonType.开始按钮:
                    strs = (from param in MachineDataDefine1.buttonList.buttonParams select param.startButton).ToList();
                    break;
                case ButtonType.暂停按钮:
                    strs = (from param in MachineDataDefine1.buttonList.buttonParams select param.pauseButton).ToList();
                    break;
                case ButtonType.停止按钮:
                    strs = (from param in MachineDataDefine1.buttonList.buttonParams select param.stopButton).ToList();
                    break;
                case ButtonType.开始按钮灯:
                    strs = (from param in MachineDataDefine1.buttonList.buttonParams select param.startButtonLED).ToList();
                    break;
                case ButtonType.暂停按钮灯:
                    strs = (from param in MachineDataDefine1.buttonList.buttonParams select param.pauseButtonLED).ToList();
                    break;
                case ButtonType.停止按钮灯:
                    strs = (from param in MachineDataDefine1.buttonList.buttonParams select param.stopButtonLED).ToList();
                    break;
                case ButtonType.安全光幕:
                    strs = (from param in MachineDataDefine1.buttonList.buttonParams select param.safeLight).ToList();
                    break;
                case ButtonType.安全门:
                    strs = (from param in MachineDataDefine1.buttonList.buttonParams select param.safeDoor).ToList();
                    break;
                case ButtonType.急停按钮:
                    strs = (from param in MachineDataDefine1.buttonList.buttonParams select param.emgButton).ToList();
                    break;
                case ButtonType.三色灯_绿:
                    strs = (from param in MachineDataDefine1.buttonList.buttonParams select param.greenLED).ToList();
                    break;
                case ButtonType.三色灯_黄:
                    strs = (from param in MachineDataDefine1.buttonList.buttonParams select param.yellowLED).ToList();
                    break;
                case ButtonType.三色灯_红:
                    strs = (from param in MachineDataDefine1.buttonList.buttonParams select param.redLED).ToList();
                    break;
                case ButtonType.三色灯_蜂鸣器:
                    strs = (from param in MachineDataDefine1.buttonList.buttonParams select param.buzzerLED).ToList();
                    break;
            }
            foreach (var item in strs)
            {
                if (item != "" && item != "null")
                {
                    addButton(item);
                }
            }
        }

        private void frm_ButtonManageItem_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
        }
        public void getButtonParam()
        {
            List<string> strs = new List<string>();
            foreach (var item in panel1.Controls)
            {
                string str = ((ComboBox)item).Text;
                strs.Add(str);
            }
            try
            {
                SQLSugarHelper.DBContext<ButtonParam>.GetInstance().Db.Updateable<ButtonParam>().AS("ButtonParam").SetColumns(columnName, strs).Where($"ID={0}").ExecuteCommand();
            }
            catch (Exception e)
            {
                MessageBox.Show("保存失败" + e.ToString());
            }
            MachineDataDefine1.buttonList.refresh();
        }
        private void addButton(string str = "")
        {
            int count = panel1.Controls.Count;
            if(count>10)
            {
                MessageBox.Show("最多添加10个配置");
            }
            ComboBox comboBox = new ComboBox();
            comboBox.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            comboBox.FormattingEnabled = true;
            comboBox.Location = new System.Drawing.Point(15, 5 + count * 27);
            comboBox.Name = "comboBox2";
            comboBox.Size = new System.Drawing.Size(187, 27);
            comboBox.Click += ComboBox_Click;
            if (isInputs)
            {
                comboBox.Items.AddRange(Enum.GetNames(typeof(EnumParam_InputIO)));
            }
            EnumParam_InputIO enumParam_InputIO;
            bool b_Result1 = Enum.TryParse(str, out enumParam_InputIO);
            if (b_Result1)
            {
                comboBox.Text = str;
            }
            panel1.Controls.Add(comboBox);
        }

        private void ComboBox_Click(object sender, EventArgs e)
        {
            foreach (var item in panel1.Controls)
            {
                ((ComboBox)item).BackColor = Control.DefaultBackColor;
            }
            ((ComboBox)sender).BackColor = Color.Red;
            currentComboBox = ((ComboBox)sender);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            addButton();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(currentComboBox!=null)
            {
                panel1.Controls.Remove(currentComboBox);
            }
            currentComboBox = null;
        }
    }
}
