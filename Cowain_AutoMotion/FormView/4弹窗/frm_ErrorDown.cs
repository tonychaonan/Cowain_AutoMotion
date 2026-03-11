using Cowain_AutoMotion.Flow.Hive;
using Cowain_Form.FormView;
using Cowain_Machine;
using Cowain_Machine.Flow;
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

namespace Cowain_AutoMotion.FormView._4弹窗
{
    public partial class frm_ErrorDown : Form
    {
        public frm_ErrorDown()
        {
            InitializeComponent();
            if (HIVEDataDefine.Hive_machineState.IsSendErrorDown)
            {
                cbBox_ErrorDown.Enabled = false;
                cbBox_ErrorMessage.Enabled = false;
                btn_ErrorDown_SendState.Enabled = false;
                btn_ErrorDown_SendErrorData.Enabled = true;
                cbBox_ErrorDown.SelectedIndex = HIVEDataDefine.Hive_machineState.ErrorDownSelectedIndex;
                cbBox_ErrorMessage.SelectedIndex = HIVEDataDefine.Hive_machineState.ErrorMessageSelectedIndex;
            }
            else
            {
                cbBox_ErrorDown.Enabled = true;
                cbBox_ErrorMessage.Enabled = true;
                btn_ErrorDown_SendState.Enabled = true;
                btn_ErrorDown_SendErrorData.Enabled = false;
                cbBox_ErrorDown.SelectedIndex = HIVEDataDefine.Hive_machineState.ErrorDownSelectedIndex;
                cbBox_ErrorMessage.SelectedIndex = HIVEDataDefine.Hive_machineState.ErrorMessageSelectedIndex;
            }
        }
        public int ErrorTypeIndex = 0;
        public string ErrorType = "";
        public string ErrorMessage = "";

        public long Starterror_time = 0;
        public long Stoperror_time = 0;


        string[] CylinderErrorMessage = { "1135_前龙门擦胶夾子报警", "1137_后龙门擦胶夾子报警", "1131_左流道顶升气缸报警","1139_左流道读码‎阻挡气缸报警", "1141_左流道CCD阻挡气缸报警", "1143_左流道压合气缸报警", "1147_左流道压合顶升气缸报警", "1133_右流道顶升气缸报警", "1151_右流道读码‎阻挡气缸报警", "1153_右流道CCD阻挡气缸报警", "1155_右流道压合气缸报警", "1159_右流道压合顶升气缸报警" };
        string[] SensorErrorMessage = { "4005_X1正极限感应器异常", "4006_X1负极限感应器异常" , "4007_X1原点感应器异常", "4008_Y1正极限感应器异常", "4009_Y1负极限感应器异常", "4010_Y1原点感应器异常", "4011_Z1正极限感应器异常", "4012_Z1负极限感应器异常", "4013_Z1原点感应器异常", "4014_A1正极限感应器异常", "4015_A1负极限感应器异常", "4016_A1原点感应器异常", "4017_R1原点感应器异常" ,
        "4018_X2正极限感应器异常","4019_X2负极限感应器异常","4020_X2原点感应器异常","4021_Y2正极限感应器异常","4022_Y2负极限感应器异常","4023_Y2原点感应器异常","4024_Z2正极限感应器异常","4025_Z2负极限感应器异常","4026_Z2原点感应器异常","4027_A2正极限感应器异常","4028_A2负极限感应器异常","4029_A2原点感应器异常","4030_R2原点感应器异常",
        "4031_左流道入料感应器" ,"4032_左流道CCD阻挡感应器","4033_左流道出料感应器","4034_右流道入料感应器","4035_右流道CCD阻挡感应器","4036_右流道出料感应器","4037_前龙门擦胶感应器异常","4038_后龙门擦胶感应器异常"};

        string[] SWErrorMessage = { "4002_软件加载失败" };
        string[] VisionErrorMessage = { "2019_前龙门相机报警", "2020_后龙门相机报警", "2078_相机连接失败" };
        string[] SafetyErrorMessage = { "2030_安全门报警" };
        string[] ScanningErrorMessage = { "2015_左流道扫码枪报警", "2017_右流道扫码枪报警" };
        string[] MotionErrorMessage = { "0078_X1驱动器报警", "0088_Z1驱动器报警" , "0098_Y1驱动器报警", "0108_R1驱动器报警", "0118_A1驱动器报警" , "0128_X2驱动器报警" , "0138_Z2驱动器报警" , "0148_Y2驱动器报警", "0158_R2驱动器报警" , "0168_A2驱动器报警", "4003_前龙门擦胶电机异常", "4004_后龙门擦胶电机异常" };
        string[] GlueErrorMessage = { "4000_胶路异常" };
        string[] MaterialShortageErrorMessage = { "4001_缺料报警" };
        string[] MESErrorMessage = { "2025_MES报警" };
        string[] ExternalRunnerMessage = { "4039_外部流道报警" };
        private void cbBox_ErrorDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            HIVEDataDefine.Hive_machineState.ErrorDownSelectedIndex = cbBox_ErrorDown.SelectedIndex;
            string[] array = cbBox_ErrorDown.Text.Split('_');
            ErrorType = array[0];
            ErrorDownChangeSeclect();
            cbBox_ErrorMessage.SelectedIndex = 0;
        }
        private void ErrorDownChangeSeclect()
        {
            try
            {
                cbBox_ErrorMessage.Items.Clear();
                if (cbBox_ErrorDown.SelectedIndex == -1 || cbBox_ErrorDown.SelectedIndex == 0)
                {
                    for (int i = 0; i < CylinderErrorMessage.Length; i++)
                    {
                        cbBox_ErrorMessage.Items.Add(CylinderErrorMessage[i]);
                    }
                }
                if (cbBox_ErrorDown.SelectedIndex == 1)
                {
                    for (int i = 0; i < SensorErrorMessage.Length; i++)
                    {
                        cbBox_ErrorMessage.Items.Add(SensorErrorMessage[i]);
                    }
                }
                if (cbBox_ErrorDown.SelectedIndex == 2)
                {
                    for (int i = 0; i < SWErrorMessage.Length; i++)
                    {
                        cbBox_ErrorMessage.Items.Add(SWErrorMessage[i]);
                    }
                }
                if (cbBox_ErrorDown.SelectedIndex == 3)
                {
                    for (int i = 0; i < VisionErrorMessage.Length; i++)
                    {
                        cbBox_ErrorMessage.Items.Add(VisionErrorMessage[i]);
                    }
                }
                if (cbBox_ErrorDown.SelectedIndex == 4)
                {
                    for (int i = 0; i < SafetyErrorMessage.Length; i++)
                    {
                        cbBox_ErrorMessage.Items.Add(SafetyErrorMessage[i]);
                    }
                }
                if (cbBox_ErrorDown.SelectedIndex == 5)
                {
                    for (int i = 0; i < ScanningErrorMessage.Length; i++)
                    {
                        cbBox_ErrorMessage.Items.Add(ScanningErrorMessage[i]);
                    }
                }
                if (cbBox_ErrorDown.SelectedIndex == 6)
                {
                    for (int i = 0; i < MotionErrorMessage.Length; i++)
                    {
                        cbBox_ErrorMessage.Items.Add(MotionErrorMessage[i]);
                    }
                }
                if (cbBox_ErrorDown.SelectedIndex == 7)
                {
                    for (int i = 0; i < GlueErrorMessage.Length; i++)
                    {
                        cbBox_ErrorMessage.Items.Add(GlueErrorMessage[i]);
                    }
                }
                if (cbBox_ErrorDown.SelectedIndex == 8)
                {
                    for (int i = 0; i < MaterialShortageErrorMessage.Length; i++)
                    {
                        cbBox_ErrorMessage.Items.Add(MaterialShortageErrorMessage[i]);
                    }
                }
                if (cbBox_ErrorDown.SelectedIndex == 9)
                {
                    for (int i = 0; i < MESErrorMessage.Length; i++)
                    {
                        cbBox_ErrorMessage.Items.Add(MESErrorMessage[i]);
                    }
                }
                if (cbBox_ErrorDown.SelectedIndex == 10)
                {
                    for (int i = 0; i < ExternalRunnerMessage.Length; i++)
                    {
                        cbBox_ErrorMessage.Items.Add(ExternalRunnerMessage[i]);
                    }
                }
            }
            catch
            {
            }
            
        }
        private void cbBox_ErrorMessage_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                HIVEDataDefine.Hive_machineState.ErrorMessageSelectedIndex = cbBox_ErrorMessage.SelectedIndex;
            }
            catch
            {
            }

        }
        private void btn_ErrorDown_SendState_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dlgResult = MessageBox.Show("再次确定发送报警状态", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (dlgResult == DialogResult.Yes)
                {
                    string[] array = cbBox_ErrorMessage.Text.Split('_');
                    string errorcode = array[0];
                    frm_Main.formError.ErrorUnit1.StartErrorMessage(errorcode);
                    HIVEDataDefine.Hive_machineState.ErrorDownstarttime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");
                    if (cbBox_ErrorDown.SelectedIndex == -1 || cbBox_ErrorMessage.SelectedIndex == -1)
                    {
                        MessageBox.Show("报警类型及报警内容不得为空！！！");
                        return;
                    }
                    if (MachineDataDefine.machineState.b_UseHive)
                    {
                        cbBox_ErrorDown.Enabled = false;
                        cbBox_ErrorMessage.Enabled = false;
                        btn_ErrorDown_SendState.Enabled = false;
                        btn_ErrorDown_SendErrorData.Enabled = true;
                        HIVEDataDefine.Hive_machineState.IsSendErrorDown = true;
                        HIVEDataDefine.Hive_machineState.WriteParams(HIVEDataDefine.Hive_machineState);
                        if (frm_Main.formError.ErrorUnit1.ErrorMessage != "NULL")
                        {
                            if (frm_Main.formError.ErrorUnit1.ErrorCode != "" && frm_Main.formError.ErrorUnit1.ErrorMessage != "" && frm_Main.formError.ErrorUnit1.ErrorType != "")
                            {
                                frm_Main.formData.ChartTime1.StartError();
                                HIVE.HIVEInstance.HiveSendMACHINESTATE(5, frm_Main.formError.ErrorUnit1.ErrorCode, frm_Main.formError.ErrorUnit1.ErrorMessage, frm_Main.formError.ErrorUnit1.ErrorType,false);
                                savelog("发送报警状态成功!!!"+ "报警代码：" + frm_Main.formError.ErrorUnit1.ErrorCode + ", 报警类型" + frm_Main.formError.ErrorUnit1.ErrorType + ", 报警内容" + frm_Main.formError.ErrorUnit1.ErrorMessage);
                                //MessageBox.Show("发送报警状态成功!!!"+ "报警代码：" + frm_Main.formError.ErrorUnit1.ErrorCode + ",报警类型" + frm_Main.formError.ErrorUnit1.ErrorType + ",报警内容" + frm_Main.formError.ErrorUnit1.ErrorMessage);
                            }
                        }
                    }
                    if (MachineDataDefine.machineState.b_UseRemoteQualification)
                    {
                        cbBox_ErrorDown.Enabled = false;
                        cbBox_ErrorMessage.Enabled = false;
                        btn_ErrorDown_SendState.Enabled = false;
                        btn_ErrorDown_SendErrorData.Enabled = true;
                        HIVEDataDefine.Hive_machineState.IsSendErrorDown = true;
                        HIVEDataDefine.Hive_machineState.WriteParams(HIVEDataDefine.Hive_machineState);
                        if (frm_Main.formError.ErrorUnit1.ErrorMessage != "NULL")
                        {
                            if (frm_Main.formError.ErrorUnit1.ErrorCode != "" && frm_Main.formError.ErrorUnit1.ErrorMessage != "" && frm_Main.formError.ErrorUnit1.ErrorType != "")
                            {
                                frm_Main.formData.ChartTime1.StartError();
                                HIVE.HIVEInstance.HiveSendMACHINESTATE(5, frm_Main.formError.ErrorUnit1.ErrorCode, frm_Main.formError.ErrorUnit1.ErrorMessage, frm_Main.formError.ErrorUnit1.ErrorType,true);
                                savelog("发送报警状态成功!!!" + "报警代码：" + frm_Main.formError.ErrorUnit1.ErrorCode + ", 报警类型" + frm_Main.formError.ErrorUnit1.ErrorType + ", 报警内容" + frm_Main.formError.ErrorUnit1.ErrorMessage);
                                //MessageBox.Show("发送报警状态成功!!!"+ "报警代码：" + frm_Main.formError.ErrorUnit1.ErrorCode + ",报警类型" + frm_Main.formError.ErrorUnit1.ErrorType + ",报警内容" + frm_Main.formError.ErrorUnit1.ErrorMessage);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("发送失败，当前为禁用HIVE状态！！！");
                    }
                    //this.Close();
                }
            }
            catch
            {
            }

            
        }
        private void btn_ErrorDown_SendErrorData_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dlgResult = MessageBox.Show("再次确定结束报警状态", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (dlgResult == DialogResult.Yes)
                {
                    HIVEDataDefine.Hive_machineState.ErrorDownstoptime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");
                    Stoperror_time = DateTime.Now.Ticks;
                    if (MachineDataDefine.machineState.b_UseHive)
                    {
                        cbBox_ErrorDown.Enabled = true;
                        cbBox_ErrorMessage.Enabled = true;
                        btn_ErrorDown_SendState.Enabled = true;
                        btn_ErrorDown_SendErrorData.Enabled = false;
                        HIVEDataDefine.Hive_machineState.ErrorDownSelectedIndex = 0;
                        HIVEDataDefine.Hive_machineState.ErrorMessageSelectedIndex = 0;
                        HIVEDataDefine.Hive_machineState.IsSendErrorDown = false;
                        HIVEDataDefine.Hive_machineState.WriteParams(HIVEDataDefine.Hive_machineState);

                        string[] array = cbBox_ErrorMessage.Text.Split('_');
                        string errorcode = array[0];
                        frm_Main.formError.ErrorUnit1.StartErrorMessage(errorcode);

                        if (frm_Main.formError.ErrorUnit1.ErrorMessage != "NULL" && frm_Main.formError.ErrorUnit1.ErrorType != "")
                        {
                              //HIVE.HIVEInstance.HiveSendERRORDATA(frm_Main.formError.ErrorUnit1.ErrorCode, frm_Main.formError.ErrorUnit1.ErrorType, frm_Main.formError.ErrorUnit1.ErrorMessage, HIVEDataDefine.Hive_machineState.ErrorDownstarttime, HIVEDataDefine.Hive_machineState.ErrorDownstoptime, Starterror_time, Stoperror_time);
                            HIVE.HIVEInstance.HiveSend(frm_Main.formError.ErrorUnit1.ErrorCode, frm_Main.formError.ErrorUnit1.ErrorType, frm_Main.formError.ErrorUnit1.ErrorMessage, HIVEDataDefine.Hive_machineState.ErrorDownstarttime, HIVEDataDefine.Hive_machineState.ErrorDownstoptime, Starterror_time, Stoperror_time);
                            frm_Main.formData.ChartTime1.StartWait();
                            savelog("结束报警状态成功" + "报警代码：" + frm_Main.formError.ErrorUnit1.ErrorCode + ",报警类型" + frm_Main.formError.ErrorUnit1.ErrorType + ",报警内容" + frm_Main.formError.ErrorUnit1.ErrorMessage);
                            //MessageBox.Show("结束报警状态成功"+"报警代码：" + frm_Main.formError.ErrorUnit1.ErrorCode + ",报警类型" + frm_Main.formError.ErrorUnit1.ErrorType + ",报警内容" + frm_Main.formError.ErrorUnit1.ErrorMessage);
                        }
                    }
                    else
                    {
                        MessageBox.Show("发送失败，当前为禁用HIVE状态！！！");
                    }
                }
            }
            catch
            {
            }
            
        }
        private void frm_ErrorDown_FormClosing(object sender, FormClosingEventArgs e)
        {
            HIVEDataDefine.Hive_machineState.WriteParams(HIVEDataDefine.Hive_machineState);
        }
        private void savelog(string message)
        {
            try
            {
                string fileName;
                fileName = string.Format("{0}.txt", DateTime.Now.ToString("yyyy_MM_dd"));
                string outputPath = @"D:\DATA\HIVE手动操作记录\手动报警停机";
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
    }
}
