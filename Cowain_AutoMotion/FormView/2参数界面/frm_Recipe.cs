using Cowain;
using Cowain_AutoMotion;
using Cowain_AutoMotion.Flow;
using Cowain_AutoMotion.Flow._1AutoMachine;
using Cowain_AutoMotion.Flow.Common;
using Cowain_AutoMotion.Flow.Hive;
using Cowain_AutoMotion.Simulat.view;
using Cowain_Machine;
using Cowain_Machine.Flow;
using DevExpress.XtraEditors.Controls;
using LightUI;
using MotionBase;
using NetronLight;
using ReadAndWriteConfig;
using Sunny.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ToolTotal;

namespace Cowain_Form.FormView
{
    public partial class frm_Recipe : Form
    {
        public frm_Recipe(ref clsMachine pM)
        {
            InitializeComponent();
            pMachine = pM;
        }
        public clsMachine pMachine;
        
        private void frm_Recipe_Load(object sender, EventArgs e)
        {
            this.propertyGrid1.SelectedObject = MESDataDefine.MESLXData;
            propertyGrid_hash.SelectedObject = HIVEDataDefine.HIVE_sha1_Data;
            propertyGrid_Config.SelectedObject = MachineDataDefine.MachineCfgS;
        }

        private void frm_Recipe_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible != true)
            {
                return;
            }
            //dataGridView_Step.Rows.Clear();
            //dataGridView_Qiu.Rows.Clear();
            //for (int i = 0; i < MachineDataDefine.MachineCfgS.RelMovePoints.Count; i++)
            //{
            //    dataGridView_Step.Rows.Add(i, MachineDataDefine.MachineCfgS.RelMovePoints[i].X, MachineDataDefine.MachineCfgS.RelMovePoints[i].Y, MachineDataDefine.MachineCfgS.RelMovePoints[i].R);
            //}
            //for (int i = 0; i < MachineDataDefine.MachineCfgS.QiuMovePoints.Count; i++)
            //{
            //    dataGridView_Qiu.Rows.Add(i, MachineDataDefine.MachineCfgS.QiuMovePoints[i].X, MachineDataDefine.MachineCfgS.QiuMovePoints[i].Y, MachineDataDefine.MachineCfgS.QiuMovePoints[i].R);
            //}
            numericUpDown_AutoSpeed.Value = MachineDataDefine.machineState.machineSpeed;
            checkBoxTestMode.Checked = MachineDataDefine.machineState.b_UseTestRun;
            cBoxNolmalWork.Checked = MachineDataDefine.machineState.b_UseTestRun != true;
            cBoxUseCCD.Checked = MachineDataDefine.machineState.b_UseCCD;
            checkBox_UseScan.Checked = MachineDataDefine.machineState.b_UseScaner;
            checkBox_UseMES.Checked = MachineDataDefine.machineState.b_UseMes;
            checkBox_CheckDoorSR.Checked = MachineDataDefine.machineState.b_UseDoorCheck;
            cBox_UsePDCA.Checked = MachineDataDefine.machineState.b_UsePDCA;
            cBox_Useimage.Checked = MachineDataDefine.machineState.b_UseSendPic;
            checkBox_UseHive.Checked = MachineDataDefine.machineState.b_UseHive;
            checkBox_grating.Checked = MachineDataDefine.machineState.b_UseGratingCheck;
            //   checkBox_UseRobot.Checked = MachineDataDefine.machineState.b_UseRobot;         
            checkBox1_hummer.Checked = MachineDataDefine.machineState.b_Usehummer;
            checkBox_isNGKO.Checked = MachineDataDefine.machineState.b_isNGOK;
            dataGridView_CMD.Rows.Clear();
            foreach (var item in MachineDataDefine.MachineCfgS.CMDLists)
            {
                dataGridView_CMD.Rows.Add(item.name, item.CMD);
            }
        }
        private void btn_Save_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(JudgeLanguage.JudgeLag("确认保存吗？"), JudgeLanguage.JudgeLag("警告"), MessageBoxButtons.OKCancel);
            if (dr == DialogResult.OK)
            {
                MachineDataDefine.machineState.machineSpeed = (int)numericUpDown_AutoSpeed.Value;
                MachineDataDefine.machineState.b_UseTestRun = checkBoxTestMode.Checked;
                MachineDataDefine.machineState.b_UseCCD = cBoxUseCCD.Checked;
                MachineDataDefine.machineState.b_UseScaner = checkBox_UseScan.Checked;
                MachineDataDefine.machineState.b_UseMes = checkBox_UseMES.Checked;
                MachineDataDefine.machineState.b_isNGOK = checkBox_isNGKO.Checked;
                MachineDataDefine.machineState.b_UseGratingCheck = checkBox_grating.Checked;
                if (MachineDataDefine.machineState.b_UseMes)
                {
                    LogAuto.Notify("启用MES", (int)MachineStation.主监控, MotionLogLevel.Info);
                }
                else
                {
                    LogAuto.Notify("[Error/Alert],关闭MES", (int)MachineStation.主监控, MotionLogLevel.Info);
                }
                MachineDataDefine.machineState.b_UseDoorCheck = checkBox_CheckDoorSR.Checked;
                if (MachineDataDefine.machineState.b_UseDoorCheck)
                {
                    MachineDataDefine.IsFormOpen = false;
                }
                MachineDataDefine.machineState.b_UsePDCA = cBox_UsePDCA.Checked;
                if (MachineDataDefine.machineState.b_UsePDCA)
                {
                    LogAuto.Notify("启用PDCA", (int)MachineStation.主监控, MotionLogLevel.Info);
                }
                else
                {
                    LogAuto.Notify("[Error/Alert],关闭PDCA", (int)MachineStation.主监控, MotionLogLevel.Info);
                }
                MachineDataDefine.machineState.b_UseSendPic = cBox_Useimage.Checked;
                MachineDataDefine.machineState.b_UseHive = checkBox_UseHive.Checked;
                if (MachineDataDefine.machineState.b_UseHive)
                {
                    LogAuto.Notify("启用HIVE", (int)MachineStation.主监控, MotionLogLevel.Info);
                }
                else
                {
                    LogAuto.Notify("[Error/Alert],关闭HIVE", (int)MachineStation.主监控, MotionLogLevel.Info);
                }
                // MachineDataDefine.machineState.b_UseRemoteQualification = cBoxRemote.Checked;
                //MachineDataDefine.machineState.b_UseRobot = checkBox_UseRobot.Checked;
                MachineDataDefine.machineState.b_Usehummer = checkBox1_hummer.Checked;
                //MachineDataDefine.machineState.b_UseOtherMachie=checkBox_UseHummer.Checked;
                MachineDataDefine.machineState.SetSaveFile(Program.StrBaseDic, "MachineState", MachineDataDefine.machineState);
                MachineDataDefine.machineState.WriteParams(MachineDataDefine.machineState);
                MachineDataDefine.MachineCfgS.RelMovePoints.Clear();
                MachineDataDefine.MachineCfgS.QiuMovePoints.Clear();
                //for (int i = 0; i < dataGridView_Step.Rows.Count; i++)
                //{
                //    double x = Convert.ToDouble(dataGridView_Step.Rows[i].Cells[1].Value);
                //    double y = Convert.ToDouble(dataGridView_Step.Rows[i].Cells[2].Value);
                //    double r = Convert.ToDouble(dataGridView_Step.Rows[i].Cells[3].Value);
                //    MachineDataDefine.MachineCfgS.RelMovePoints.Add(new RelMovePoint(x, y, r));
                //}
                //for (int i = 0; i < dataGridView_Qiu.Rows.Count; i++)
                //{
                //    double x = Convert.ToDouble(dataGridView_Qiu.Rows[i].Cells[1].Value);
                //    double y = Convert.ToDouble(dataGridView_Qiu.Rows[i].Cells[2].Value);
                //    double r = Convert.ToDouble(dataGridView_Qiu.Rows[i].Cells[3].Value);
                //    MachineDataDefine.MachineCfgS.QiuMovePoints.Add(new RelMovePoint(x, y, r));
                //}
                MachineDataDefine.MachineCfgS.WriteParams(MachineDataDefine.MachineCfgS);
            }
        }

        private void cBox_ErrorDown_CheckedChanged(object sender, EventArgs e)
        {
            //if (cBox_ErrorDown.Checked)
            //{
            //    if (MachineDataDefine.machineState.b_UseHive!=true)
            //    {
            //        MessageBox.Show("禁用HIVE状态无法进入该界面！！！");
            //        return;
            //    }
            //    if (!pMachine.GetisAutoing())
            //    {
            //        frm_ErrorDown fm_errorDown = new frm_ErrorDown();
            //        fm_errorDown.ShowDialog();
            //        fm_errorDown.Close();
            //    }
            //}
        }
        private void btnSave1_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(JudgeLanguage.JudgeLag("确认保存吗？"), JudgeLanguage.JudgeLag("警告"), MessageBoxButtons.OKCancel);
            if (dr == DialogResult.OK)
            {
                MESDataDefine.MESLXData.SetSaveFile(Program.StrBaseDic, "MESParamData", MESDataDefine.MESLXData);
                MESDataDefine.MESLXData.WriteParams(MESDataDefine.MESLXData);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(JudgeLanguage.JudgeLag("确认保存吗？"), JudgeLanguage.JudgeLag("警告"), MessageBoxButtons.OKCancel);
            if (dr == DialogResult.OK)
            {
                HIVEDataDefine.HIVE_sha1_Data.SetSaveFile(Program.StrBaseDic, "HIVE_SHA1_DataClass", HIVEDataDefine.HIVE_sha1_Data);
                HIVEDataDefine.HIVE_sha1_Data.WriteParams(HIVEDataDefine.HIVE_sha1_Data);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(JudgeLanguage.JudgeLag("确认保存吗？"), JudgeLanguage.JudgeLag("警告"), MessageBoxButtons.OKCancel);
            if (dr == DialogResult.OK)
            {
                MachineDataDefine.MachineCfgS.WriteParams(MachineDataDefine.MachineCfgS);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            //DialogResult dr = MessageBox.Show(JudgeLanguage.JudgeLag("确认保存吗？"), JudgeLanguage.JudgeLag("警告"), MessageBoxButtons.OKCancel);
            //if (dr == DialogResult.OK)
            //{
            //    Connections.Instance.HardWareControlParam.WriteParams(Connections.Instance.HardWareControlParam);
            //}
        }

        private void cBoxRecheck_CheckedChanged(object sender, EventArgs e)
        {
            //if (cBoxRecheck.Checked)
            //{
            //    DialogResult dr = MessageBox.Show(JudgeLanguage.JudgeLag("确认启用复检模式吗？设备将只复检，不调整"), JudgeLanguage.JudgeLag("警告"), MessageBoxButtons.OKCancel);
            //    if (dr != DialogResult.OK)
            //    {
            //        cBoxRecheck.Checked = false;
            //    }
            //}
        }
        private void CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxTestMode.Checked == false && cBoxNolmalWork.Checked == false)
            {
                checkBoxTestMode.Checked = true;
            }
            if ((CheckBox)sender == checkBoxTestMode && ((CheckBox)sender).Checked)
            {
                checkBox_UseScan.Enabled = true;
                cBoxUseCCD.Enabled = true;
                checkBox_UseMES.Enabled = true;
                //  cBox_UsePDCA.Enabled = true;
                cBox_Useimage.Enabled = true;
                cBoxUseCCD.Checked = true;
                checkBox_UseScan.Checked = false;
                checkBox_UseMES.Checked = false;
                checkBox_CheckDoorSR.Checked = false;
                cBox_UsePDCA.Checked = false;
                cBox_Useimage.Checked = false;
                checkBox_UseHive.Enabled = true;
                checkBox_UseHive.Checked = false;               
                cBoxNolmalWork.Checked = false;
                //checkBox_UseRobot.Checked = false;
                checkBox_UseHive.Checked = true;
                cBox_UsePDCA.Enabled = true;

                // checkBox_UseHummer.Checked = false;
            }
            if ((CheckBox)sender == cBoxNolmalWork && ((CheckBox)sender).Checked)
            {
                checkBox_UseScan.Enabled = false;
                cBoxUseCCD.Enabled = true;
                checkBox_UseMES.Enabled = true;
                //cBox_UsePDCA.Enabled = false;
                cBox_Useimage.Enabled = false;
                cBoxUseCCD.Checked = true;
                checkBox_UseScan.Checked = true;
                checkBox_UseMES.Checked = true;
                checkBox_CheckDoorSR.Checked = true;
                cBox_UsePDCA.Checked = true;
                cBox_UsePDCA.Enabled = true;
                cBox_Useimage.Checked = true;
                checkBox_UseHive.Enabled = true;//不启用hive
                checkBox_UseHive.Checked = true;       
                checkBoxTestMode.Checked = false;
                //checkBox_UseRobot.Checked = true;

                // checkBox_UseHummer.Checked = false;
            }
        }

        private void model_CheckedChanged(object sender, EventArgs e)
        {
            //if (((CheckBox)sender).Checked != true)
            //{
            //    return;
            //}
            //string str = "";
            //if (((CheckBox)sender).Text == "启用标定模式" && ((CheckBox)sender).Checked)
            //{
            //    str = "确认启用相机标定模式吗？在标定完成后，请及时关闭";
            //    cBoxRecheck.Checked = false;
            //    cBoxforwardModel.Checked = false;
            //}
            //else if (((CheckBox)sender).Text == "启用复检模式" && ((CheckBox)sender).Checked)
            //{
            //    str = "确认启用复检模式吗？设备将只复检，不调整";
            //    cBoxforwardModel.Checked = false;
            //}
            //else if (((CheckBox)sender).Text == "启用进给模式" && ((CheckBox)sender).Checked)
            //{
            //    str = "确认启用进给模式吗？设备将进行设定点位运动";
            //    cBoxRecheck.Checked = false;
            //}
            //if (str != "")
            //{
            //    DialogResult dr = MessageBox.Show(JudgeLanguage.JudgeLag(str), JudgeLanguage.JudgeLag("警告"), MessageBoxButtons.OKCancel);
            //    if (dr != DialogResult.OK)
            //    {
            //        ((CheckBox)sender).Checked = false;
            //    }
            //}
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!MachineDataDefine.IsAutoing && MachineDataDefine.pMachine.GetHomeCompleted())
            {

                DialogResult dr = MessageBox.Show(JudgeLanguage.JudgeLag("将进行联合标定"), JudgeLanguage.JudgeLag("警告"), MessageBoxButtons.OKCancel);
                if (dr == DialogResult.OK)
                {
                    MachineDataDefine.electriccalib = false;
                    LogAuto.Notify("走九点标定", (int)MachineStation.主监控, MotionLogLevel.Info);
                    WorkProcessLoad.instance.workProcess_Mainflow.axisCalibration.DoStep(0);
                    WorkProcessLoad.instance.workProcess_Mainflow.axisCalibration.Action();
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(JudgeLanguage.JudgeLag("将进行轴标定"), JudgeLanguage.JudgeLag("警告"), MessageBoxButtons.OKCancel);
            if (dr == DialogResult.OK)
            {
                //MachineDataDefine.pMachine.workProcess_AlignmentMode.axisCalibration.DoStep(0);
                //MachineDataDefine.pMachine.workProcess_AlignmentMode.axisCalibration.zhou = true;
                //MachineDataDefine.pMachine.workProcess_AlignmentMode.axisCalibration.qiu = false;
                //MachineDataDefine.pMachine.workProcess_AlignmentMode.axisCalibration.Action();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(JudgeLanguage.JudgeLag("将进行插拔测试"), JudgeLanguage.JudgeLag("警告"), MessageBoxButtons.OKCancel);
            if (dr == DialogResult.OK)
            {
                //MachineDataDefine.pMachine.workProcess_AlignmentMode.b_InAndOutModel = true;
                //MachineDataDefine.pMachine.workProcess_AlignmentMode.Action();

            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(JudgeLanguage.JudgeLag("将进行进给测试"), JudgeLanguage.JudgeLag("警告"), MessageBoxButtons.OKCancel);
            if (dr == DialogResult.OK)
            {
                //MachineDataDefine.pMachine.workProcess_AlignmentMode.axisCalibration.b_StepModel = true;
                //MachineDataDefine.pMachine.workProcess_AlignmentMode.axisCalibration.DoStep(0);
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (dataGridView_CMD.SelectedRows.Count <= 0)
            {
                return;
            }
            string cmd = dataGridView_CMD.SelectedRows[0].Cells[1].Value.ToString();
            HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).SendMSG(cmd);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(JudgeLanguage.JudgeLag("将进行数据测试"), JudgeLanguage.JudgeLag("警告"), MessageBoxButtons.OKCancel);
            if (dr == DialogResult.OK)
            {
                //MachineDataDefine.pMachine.workProcess_AlignmentMode.b_DataModel = true;
                //MachineDataDefine.pMachine.workProcess_AlignmentMode.Action();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            txtBarcodeSN.Text = "";
            //Dictionary<string, string> datas11 = new Dictionary<string, string>();
            //datas11.Add("产品SN", "H5RGXW000GP0000B2Z");
            //datas11.Add("开始时间", "2023-08-14 20:50:25");
            //datas11.Add("结束时间", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            //datas11.Add("软件版本", MESDataDefine.MESLXData.SW_Version);
            //datas11.Add("载具号", "H5RGXW000GS0000B2Z");
            //datas11.Add("OffsetX值", "0");
            //datas11.Add("OffsetY值", "0");
            //datas11.Add("OffsetR值", "0");
            //datas11.Add("P1值", "0");
            //datas11.Add("P2值", "0");
            //datas11.Add("P3值", "0");
            //datas11.Add("P4值", "0");
            //datas11.Add("P5值", "0");
            //datas11.Add("P6值", "0");
            //datas11.Add("P7值", "0");
            //datas11.Add("P8值", "0");
            //datas11.Add("Shift1值", "0");
            //datas11.Add("Shift2值", "0");
            //datas11.Add("Shift3值", "0");
            //datas11.Add("Shift4值", "0");
            //datas11.Add("调整次数", "5");
            //datas11.Add("压缩图片路径", MESDataDefine.MESLXData.StrPDCAImagePath + @"Image/2023-08-14/H5RGXW000GP0000B2Z_181950.zip");
            //datas11.Add("电脑名称", MESDataDefine.MESLXData.StrUser);
            //datas11.Add("电脑密码", MESDataDefine.MESLXData.StrPassWord);
            //Post.POSTClass.AddCMD(0, Post.CMDStep.上传PDCA, datas11);

            HardWareControl.getRS232Control(EnumParam_ConnectionName.扫码枪).strRecData = "";
            HardWareControl.getRS232Control(EnumParam_ConnectionName.扫码枪).SetWriteDataBYTE();
            int num = 0;

            while (true)
            {

                num++;
                Thread.Sleep(200);
                if (HardWareControl.getRS232Control(EnumParam_ConnectionName.扫码枪).strRecData != "")
                {
                    txtBarcodeSN.Text = HardWareControl.getRS232Control(EnumParam_ConnectionName.扫码枪).strRecData.Trim();
                    break;
                }
                if (num > 2)
                {
                    txtBarcodeSN.Text = "扫码超时";
                    break;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //txtBarcodeSN.Text = HardWareControl.getRS232Control(EnumParam_ConnectionName.扫码枪).strRecData;

            //if (HardWareControl.getInputIO(EnumParam_InputIO.锁螺丝NG信号).GetValue())
            //{
            //    LogAuto.Notify("螺丝批NG信号！", (int)MachineStation.主监控, MotionLogLevel.Info);
            //    MachineDataDefine.screwNG = true;



            //}
            //else
            //{

            //}


        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(JudgeLanguage.JudgeLag("将进行进给测试"), JudgeLanguage.JudgeLag("警告"), MessageBoxButtons.OKCancel);
            if (dr == DialogResult.OK)
            {
                //MachineDataDefine.pMachine.workProcess_AlignmentMode.axisCalibration.Productb_StepModel = true;
                //MachineDataDefine.pMachine.workProcess_AlignmentMode.axisCalibration.DoStep(0);
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(JudgeLanguage.JudgeLag("将进行九点标定"), JudgeLanguage.JudgeLag("警告"), MessageBoxButtons.OKCancel);
            if (dr == DialogResult.OK)
            {
                MachineDataDefine.electriccalib = true;
                LogAuto.Notify("走九点标定", (int)MachineStation.主监控, MotionLogLevel.Info);
                WorkProcessLoad.instance.workProcess_Mainflow.axisCalibration.DoStep(0);
                WorkProcessLoad.instance.workProcess_Mainflow.axisCalibration.Action();
            }
        }

        private void cBox_UsePDCA_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void tabPage5_Click(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {
            simulatFrm simulatFrm = new simulatFrm();
            simulatFrm.Show();
        }
        private void button6_Click_1(object sender, EventArgs e)
        {
            //DialogResult dr = MessageBox.Show(JudgeLanguage.JudgeLag("确认保存吗？"), JudgeLanguage.JudgeLag("警告"), MessageBoxButtons.OKCancel);
            //if (dr == DialogResult.OK)
            //{
            //    Connections.Instance.HardWareControlParam.WriteParams(Connections.Instance.socketParams);
            //}
        }
        private void btnSimulator_Click(object sender, EventArgs e)
        {
            //让仿真开始运行
            if (btnSimulator.Text == "开始仿真测试")
            {
                SimulatDataDefine.simulatorFlowManager.start();
                btnSimulator.Text = "停止仿真测试";
                btnSimulator.BackColor = Color.Green;
            }
            else
            {
                SimulatDataDefine.simulatorFlowManager.stop();
                btnSimulator.Text = "开始仿真测试";
                btnSimulator.BackColor = Button.DefaultBackColor;
            }
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            StepShowFrm stepShowFrm = new StepShowFrm();
            stepShowFrm.Show();
        }
        EditorFrm editorFrm;
        private void button8_Click_1(object sender, EventArgs e)
        {
            editorFrm = new EditorFrm();
            StepRelation stepRelation = StepRelationManager.instance.getStepRelation("测试流程");
            foreach (StepRelationItem item in stepRelation.stepRelationItems)
            {
                editorFrm.shapeWork.stepRelationParams.Add(new StepRelationParam(item.name, item.from, item.to, item.locationX, item.locationY, item.b_IsHead));
            }
            ShowStep showStep = ShowStepManager.instance.getShowStep("测试流程");
            showStep.stepChange -= stepShow;
            showStep.stepChange += stepShow;
            editorFrm.Show();
        }
        private void stepShow(string step)
        {
            ShapeWork.addStep(step);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).returnStr = "";
            HardWareControl.getSocketControl(EnumParam_ConnectionName.CCD).SendMSG("T1,0,0,0");
        }

        //private MiSuMiControl gripperControl;
        private bool isConnected = false;

        private void btnExecute_Click(object sender, EventArgs e)
        {
            if (!CheckConnection()) return;

            if (double.TryParse(txtPosition.Text, out double position) &&
                double.TryParse(txtSpeed.Text, out double speed) &&
                double.TryParse(txtForce.Text, out double force))
            {
                ushort pos = (ushort)(position * 100);
                ushort spd = (ushort)speed;
                ushort frc = (ushort)force;

                if (MachineDataDefine.miSuMiControl.MoveWithParams(pos, spd, frc))
                {
                    txtTargetDetection.Text = "执行中...";
                }
            }
            else
            {
                MessageBox.Show("参数格式错误！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool CheckConnection()
        {
            if (!MachineDataDefine.miSuMiControl.IsConnected)
            {
                MessageBox.Show("请先连接串口和电爪！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void btnExecuteStep_Click(object sender, EventArgs e)
        {
            if (!CheckConnection()) return;

            if (double.TryParse(txtPosition.Text, out double currentPos) &&
                double.TryParse(txtStepInterval.Text, out double step))
            {
                double newPos = currentPos + step;
                txtPosition.Text = newPos.ToString("F2");
                btnExecute_Click(sender, e);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (!CheckConnection()) return;
            MachineDataDefine.miSuMiControl.Disable();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (!CheckConnection()) return;

            if (MachineDataDefine.miSuMiControl.EnableWithSearch())
            {
                MessageBox.Show("正在重新搜索行程...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MachineDataDefine.miSuMiControl.WaitReady(10000);
            }
        }

        private void btnHalfOpen_Click(object sender, EventArgs e)
        {

            if (!CheckConnection()) return;
            MachineDataDefine.miSuMiControl.HalfOpen();
        }

        private void btnHalfClose_Click(object sender, EventArgs e)
        {
            if (!CheckConnection()) return;
            MachineDataDefine.miSuMiControl.HalfClose();
        }

        private void btnFullOpen_Click(object sender, EventArgs e)
        {
            if (!CheckConnection()) return;
            MachineDataDefine.miSuMiControl.FullOpen();
        }

        private void btnFullClose_Click(object sender, EventArgs e)
        {
            if (!CheckConnection()) return;
            MachineDataDefine.miSuMiControl.FullClose();
        }

        private void btnLowOpen_Click(object sender, EventArgs e)
        {
            if (!CheckConnection()) return;
            MachineDataDefine.miSuMiControl.LowOpen();
        }

        private void btnLowClose_Click(object sender, EventArgs e)
        {
            if (!CheckConnection()) return;
            MachineDataDefine.miSuMiControl.LowClose();
        }

        private void btnResponseTimeTest_Click(object sender, EventArgs e)
        {
            if (!CheckConnection()) return;

            try
            {
                txtResponseTime.Text = "测试中...";
                Application.DoEvents();

                // 使用半力全速模式进行响应时间测试
                var stopwatch = System.Diagnostics.Stopwatch.StartNew();
                
                // 发送半力半速关闭命令（根据需求改为半力全速）
                if (MachineDataDefine.miSuMiControl.HalfForceFullSpeedClose(2600))
                {
                    // 等待动作完成并测量时间
                    if (MachineDataDefine.miSuMiControl.WaitMovementComplete(5000))
                    {
                        stopwatch.Stop();
                        long responseTimeMs = stopwatch.ElapsedMilliseconds;
                        txtResponseTime.Text = $"{responseTimeMs} ms";
                        
                        LogAuto.Notify($"电夹爪响应时间测试完成：{responseTimeMs} ms", (int)MachineStation.主监控, MotionLogLevel.Info);
                        
                        // 自动打开电爪准备下次测试
                        System.Threading.Thread.Sleep(500);
                        MachineDataDefine.miSuMiControl.HalfForceFullSpeedOpen();
                    }
                    else
                    {
                        stopwatch.Stop();
                        txtResponseTime.Text = "超时";
                        LogAuto.Notify("电夹爪响应时间测试超时", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                    }
                }
                else
                {
                    txtResponseTime.Text = "失败";
                    LogAuto.Notify("电夹爪响应时间测试失败：命令发送失败", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                }
            }
            catch (Exception ex)
            {
                txtResponseTime.Text = "错误";
                LogAuto.Notify($"电夹爪响应时间测试异常：{ex.Message}", (int)MachineStation.主监控, MotionLogLevel.Alarm);
            }
        }

        private void btnbanlibansu_Click(object sender, EventArgs e)
        {
            if (!CheckConnection()) return;

            try
            {
                txtResponseTime.Text = "测试中...";
                Application.DoEvents();

                // 使用半力全速模式进行响应时间测试
                var stopwatch = System.Diagnostics.Stopwatch.StartNew();

                // 发送半力半速关闭命令（根据需求改为半力全速）
                if (MachineDataDefine.miSuMiControl.HalfClose())
                {
                    // 等待动作完成并测量时间
                    if (MachineDataDefine.miSuMiControl.WaitMovementComplete(5000))
                    {
                        stopwatch.Stop();
                        long responseTimeMs = stopwatch.ElapsedMilliseconds;
                        txtResponseTime.Text = $"{responseTimeMs} ms";

                        LogAuto.Notify($"电夹爪响应时间测试完成：{responseTimeMs} ms", (int)MachineStation.主监控, MotionLogLevel.Info);

                        // 自动打开电爪准备下次测试
                        System.Threading.Thread.Sleep(500);
                        MachineDataDefine.miSuMiControl.HalfOpen();
                    }
                    else
                    {
                        stopwatch.Stop();
                        txtResponseTime.Text = "超时";
                        LogAuto.Notify("电夹爪响应时间测试超时", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                    }
                }
                else
                {
                    txtResponseTime.Text = "失败";
                    LogAuto.Notify("电夹爪响应时间测试失败：命令发送失败", (int)MachineStation.主监控, MotionLogLevel.Alarm);
                }
            }
            catch (Exception ex)
            {
                txtResponseTime.Text = "错误";
                LogAuto.Notify($"电夹爪响应时间测试异常：{ex.Message}", (int)MachineStation.主监控, MotionLogLevel.Alarm);
            }
        }
    }
}