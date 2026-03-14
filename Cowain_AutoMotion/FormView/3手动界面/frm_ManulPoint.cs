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
using MotionBase;
using Cowain;
using Cowain_AutoMotion.Flow;
using System.Threading;
using Cowain_AutoMotion;
using Cowain_Machine;
using static Cowain_AutoMotion.SQLSugarHelper;

namespace Cowain_Form.FormView
{
    public partial class frm_ManulPoint : Form
    {
        public frm_ManulPoint(ref clsMachine refMachine)
        {
            pMachine = refMachine;
            InitializeComponent();
        }
        clsMachine pMachine = null;
        ImageList ImgList = new ImageList();
        double m_dbManulSpeed = 30;
        StationParam currentStationParam = new StationParam();
        MachineData currentPoint = null;
        DrvMotor[] Motors = new DrvMotor[5];
        public static List<Control> control = new List<Control>(); //Login中所有控件
        valveManual valveManual = new valveManual();
        private void frm_ManulPoint_Load(object sender, EventArgs e)
        {
            ImgList.Images.Add("Enable", Cowain_AutoMotion.Properties.Resources.SetOk);
            ImgList.Images.Add("DisEnable", Cowain_AutoMotion.Properties.Resources.SetOk_Disable);
            listView_Pos.LargeImageList = ImgList;
            listView_Pos.SmallImageList = ImgList;
            int Widths = 0, Heights = 0, Widths2 = 0, Heights2 = 0;
            //  getControl(this.Controls);
            control.Clear();
            getControl(this.Controls);
            for (int i = 0; i < control.Count; i++)
            {
                if (control[i] is Label)
                {
                    Widths = control[i].Width;
                    Heights = control[i].Height;
                    ((Label)control[i]).AutoEllipsis = true;
                }
                if (control[i] is CheckBox)
                {
                    Widths2 = control[i].Width;
                    Heights2 = control[i].Height;
                    ((CheckBox)control[i]).AutoEllipsis = true;
                }
                // }
                //将所有控件txt变成可转语言
                control[i].Text = JudgeLanguage.JudgeLag(control[i].Text);
                if (control[i] is Label)
                {
                    control[i].Width = Widths;
                    control[i].Height = Heights;
                    ((Label)control[i]).AutoSize = false;
                    ((Label)control[i]).AutoEllipsis = true;
                }
                if (control[i] is CheckBox)
                {
                    control[i].Width = Widths2;
                    control[i].Height = Heights2;
                    ((CheckBox)control[i]).AutoSize = false;
                    ((CheckBox)control[i]).AutoEllipsis = true;
                }
                else if (control[i] is Button)
                {
                    ((Button)control[i]).AutoEllipsis = true;
                }
            }
            valveManual.addControl(label_Valve1, btn_ValveOn1, btn_ValveOff1);
            valveManual.addControl(label_Valve2, btn_ValveOn2, btn_ValveOff2);
            valveManual.addControl(label_Valve3, btn_ValveOn3, btn_ValveOff3);
            valveManual.addControl(label_Valve4, btn_ValveOn4, btn_ValveOff4);
            valveManual.addControl(label_Valve5, btn_ValveOn5, btn_ValveOff5);
            valveManual.addControl(label_Valve6, btn_ValveOn6, btn_ValveOff6);
            valveManual.addControl(label_Valve7, btn_ValveOn7, btn_ValveOff7);
            valveManual.addControl(label_Valve8, btn_ValveOn8, btn_ValveOff8);
            valveManual.addControl(label_Valve9, btn_ValveOn9, btn_ValveOff9);
            valveManual.addControl(label_Valve10, btn_ValveOn10, btn_ValveOff10);
            valveManual.addControl(label_Valve11, btn_ValveOn11, btn_ValveOff11);
            valveManual.addControl(label_Valve12, btn_ValveOn12, btn_ValveOff12);
            valveManual.addControl(label_Valve13, btn_ValveOn13, btn_ValveOff13);
            valveManual.addControl(label_Valve14, btn_ValveOn14, btn_ValveOff14);
        }
        private void getControl(Control.ControlCollection etc)
        {

            foreach (Control ct in etc)
            {
                try
                {
                    control.Add(ct);
                }
                catch
                { }

                if (ct.HasChildren)
                {
                    getControl(ct.Controls);
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            frm_PointsManage frm_PointsManage = new frm_PointsManage();
            frm_PointsManage.Show();
        }
        private void frm_ManulPoint_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible == false)
            {
                timer_ReFlash.Enabled = false;
                return;
            }
            //添加工站
            comboBox_StationSelect.Items.Clear();
            for (int i = 0; i < BaseDataDefine.stationParams.Count; i++)
            {
                comboBox_StationSelect.Items.Add(BaseDataDefine.stationParams[i].CName.Trim());
            }
            timer_ReFlash.Enabled = true;
        }
        private void comboBox_StationSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_StationSelect.SelectedIndex == -1)
            {
                return;
            }
            currentStationParam = null;
            groupBox2.Enabled = false;
            listView_Pos.Items.Clear();
            string stationName = comboBox_StationSelect.Text;
            foreach (var item in BaseDataDefine.stationParams)
            {
                if (item.CName.Trim() == stationName)
                {
                    currentStationParam = item;
                    break;
                }
            }
            foreach (var item in BaseDataDefine.machineDatas)
            {
                if (item.StationName.Trim() == stationName || stationName == "所有集合")
                {
                    listView_Pos.Items.Add(item.CName, "DisEnable");
                }
            }
            //对轴进行赋值
            setParam(currentStationParam.XData1, ref groupBox_M1, ref lbMotorName1, ref Motors[0], ref label_M1Pos);
            setParam(currentStationParam.YData2, ref groupBox_M2, ref lbMotorName2, ref Motors[1], ref label_M2Pos);
            setParam(currentStationParam.ZData3, ref groupBox_M3, ref lbMotorName3, ref Motors[2], ref label_M3Pos);
            setParam(currentStationParam.RData4, ref groupBox_M4, ref lbMotorName4, ref Motors[3], ref label_M4Pos);
            setParam(currentStationParam.AData5, ref groupBox_M5, ref lbMotorName5, ref Motors[4], ref label_M5Pos);
            valveManual.dispose();
            List<CylindersStationParam> cylindersStationParams = DBContext<CylindersStationParam>.GetInstance().GetList();
            foreach (var item in cylindersStationParams)
            {
                if (item.stationName == stationName)
                {
                    valveManual.add(item.CName);
                }
            }
        }
        private void setParam(string data1, ref GroupBox groupBox1, ref Label MotorName, ref DrvMotor drvMotor, ref Label label_MPos)
        {
            drvMotor = null;
            data1 = data1.Trim();
            if (data1 == "" || data1 == "null")
            {
                groupBox1.Enabled = false;
                MotorName.Text = "null";
                label_MPos.Text = "0.00";
            }
            else
            {
                groupBox1.Enabled = true;
                MotorName.Text = data1;
                foreach (var item in Base.GetMotorList())
                {
                    if (item.Key == data1)
                    {
                        drvMotor = item.Value;
                        break;
                    }
                }
            }
        }
        private void listView_Pos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView_Pos.SelectedItems.Count == 0)
            {
                return;
            }
            currentPoint = null;
            string pointName = listView_Pos.SelectedItems[0].Text.ToString();
            foreach (var item in BaseDataDefine.machineDatas)
            {
                if (item.CName == pointName)
                {
                    currentPoint = item;
                    break;
                }
            }
            foreach (ListViewItem item in listView_Pos.Items)
            {
                item.ImageKey = "DisEnable";
            }
            listView_Pos.SelectedItems[0].ImageKey = "Enable";
            if (currentPoint != null)
            {
                groupBox2.Enabled = true;
            }
        }

        private void btn_AddM1_Click(object sender, EventArgs e)
        {
            if (comboBox_Pitch.Text == "")
            {
                MessageBox.Show("未选择寸动距离", "警告", MessageBoxButtons.OKCancel);
                return;
            }
            DrvMotor pSelectMotor = null;
            string groupName = ((GroupBox)((Button)sender).Parent).Name;
            string motorName = "";
            string pos = "";
            if (groupName == "groupBox_M1")
            {
                pSelectMotor = Motors[0];
                motorName = lbMotorName1.Text;
                pos = label_M1Pos.Text;
            }
            else if (groupName == "groupBox_M2")
            {
                pSelectMotor = Motors[1];
                motorName = lbMotorName2.Text;
                pos = label_M2Pos.Text;
            }
            else if (groupName == "groupBox_M3")
            {
                pSelectMotor = Motors[2];
                motorName = lbMotorName3.Text;
                pos = label_M3Pos.Text;
            }
            else if (groupName == "groupBox_M4")
            {
                pSelectMotor = Motors[3];
                motorName = lbMotorName4.Text;
                pos = label_M4Pos.Text;
            }
            else if (groupName == "groupBox_M5")
            {
                pSelectMotor = Motors[4];
                motorName = lbMotorName5.Text;
                pos = label_M5Pos.Text;
            }
            //-------------------------------------------------------------------
            double dbDir = 1.0;
            double dbRevDis = Convert.ToDouble(comboBox_Pitch.Text);
            if (sender == btn_SubM1 || sender == btn_SubM2 || sender == btn_SubM3 ||
                sender == btn_SubM4 || sender == btn_SubM5)
                dbDir = -1.0;
            //---------------------------
            if (pSelectMotor != null)
            {
                pSelectMotor.RevMove((dbRevDis * dbDir), m_dbManulSpeed);
                Double startPos = Convert.ToDouble(pos);
                LogAuto.SaveChangeParameterLog(comboBox_StationSelect.Text.ToString() + "移动-----" + motorName + " : " + startPos + " -> " + (startPos + dbRevDis * dbDir), ChangeParameterLogSpecies.移动更改点位记录);
            }
        }

        private void timer_ReFlash_Tick(object sender, EventArgs e)
        {
            if(this.Visible!=true)
            {
                return;
            }
            Label[] pLabel = { label_M1Pos, label_M2Pos, label_M3Pos, label_M4Pos, label_M5Pos };
            for (int i = 0; i < Motors.Length; i++)
            {
                if (Motors[i] != null)
                {
                    double dbPos = Motors[i].GetPosition();
                    pLabel[i].Text = dbPos.ToString("0.000");
                }
            }
            //如果轴在动作，则禁用控件
            GroupBox[] groupBoxes = { groupBox_M1, groupBox_M2, groupBox_M3, groupBox_M4, groupBox_M5 };
            for (int i = 0; i < groupBoxes.Length; i++)
            {
                if (Motors[i] != null)
                {
                    if (Motors[i].isIDLE())
                    {
                        groupBoxes[i].Enabled = true;
                    }
                    else
                    {
                        groupBoxes[i].Enabled = false;
                    }
                }
                else
                {
                    groupBoxes[i].Enabled = false;
                }
            }
            if (currentPoint != null)
            {
                EnumParam_Point pointName = EnumParam_Point.待命位;
                Enum.TryParse(currentPoint.CName, out pointName);
                if (BaseDataDefine.clsPointsMoveManage.getPointIdel(pointName))
                {
                    btn_Move.Enabled = true;
                }
                else
                {
                    btn_Move.Enabled = false;
                }
            }
            else
            {
                btn_Move.Enabled = false;
            }
            lock(valveManual.obj)
            {
                foreach (var item in valveManual.valveManualItems)
                {
                    if(item.drvValve?.m_OpenSR?.GetValue()==true)
                    {
                        item.lbName.BackColor = Color.LightGreen;
                    }
                    else
                    {
                        item.lbName.BackColor = Button.DefaultBackColor;
                    }
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
                    currentPoint.Data1 = Convert.ToDouble(label_M1Pos.Text);
                    currentPoint.Data2 = Convert.ToDouble(label_M2Pos.Text);
                    currentPoint.Data3 = Convert.ToDouble(label_M3Pos.Text);
                    currentPoint.Data4 = Convert.ToDouble(label_M4Pos.Text);
                    currentPoint.Data5 = Convert.ToDouble(label_M5Pos.Text);
                    DBContext<MachineData>.GetInstance().Update(currentPoint);
                    BaseDataDefine.machineDatas = DBContext<MachineData>.GetInstance().GetList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btn_Home_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("整机将进行回原,请确认！", "警告", MessageBoxButtons.OKCancel);
            if (dr == DialogResult.OK)
            {
                pMachine.DoHome();
            }
        }

        private void btn_Move_Click(object sender, EventArgs e)
        {
            if (currentPoint != null)
            {
                btn_Move.Enabled = false;
                EnumParam_Point pointName = EnumParam_Point.待命位;
                Enum.TryParse(currentPoint.CName, out pointName);
                BaseDataDefine.clsPointsMoveManage.movePoint(pointName);
            }
        }

        private void btn_Stop_Click(object sender, EventArgs e)
        {
            pMachine.Stop();
        }
    }
}
