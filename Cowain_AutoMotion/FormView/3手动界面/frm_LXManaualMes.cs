using Cowain;
using Cowain_AutoMotion;
using Cowain_AutoMotion.Flow;
using Cowain_Machine;
using MotionBase;
using Post;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ToolTotal;

namespace Cowain_Form.FormView
{

    public partial class frm_LXManaualMes : Form
    {
       // public LXPOSTClass post;
        public frm_LXManaualMes()
        {
            InitializeComponent();
           // post = LXPOSTClass.CreateInstance();
        }
        int ngTime = 0;
        private void button9_Click(object sender, System.EventArgs e)
        {
            tb_uc.Text = "";
            tb_sn.Text = "";

            //if (HardWareControl.getRS232Control(EnumParam_ConnectionName.扫码枪).strRecData != "")
            //{
            //    MESDataDefine.holdSN = HardWareControl.getRS232Control(EnumParam_ConnectionName.扫码枪).strRecData.Trim();
            //}
            //else
            //{

            //        ngTime = 0;
            //        MsgBoxHelper.DxMsgShowWarn("扫码失败！");
            //        LogAuto.Notify("扫码失败！", (int)MachineStation.主监控, LogLevel.Alarm);
            //        return;
            //}
            try
            {

                Task.Run(new Action(() =>
                {
                    HardWareControl.getRS232Control(EnumParam_ConnectionName.扫码枪).strRecData = "";
                    HardWareControl.getRS232Control(EnumParam_ConnectionName.扫码枪).SetWriteDataBYTE();
                    int num = 0;
                    while (true)
                    {
                        num++;
                        Thread.Sleep(200);
                        if (HardWareControl.getRS232Control(EnumParam_ConnectionName.扫码枪).strRecData != "")
                        {
                            tb_uc.Text = HardWareControl.getRS232Control(EnumParam_ConnectionName.扫码枪).strRecData.Trim();
                            break;
                        }
                        if (num > 20)
                        {
                            tb_uc.Text = "扫码超时";
                            break;
                        }
                    }
                    button9.Enabled = true;
                }));
            }
            catch
            {
                button9.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> dirr = new Dictionary<string, string>();
            dirr.Add("工站", MESDataDefine.MESLXData.terminalName);
            dirr.Add("载具编号", tb_uc.Text);
            POSTClass.AddCMD(0, Post.CMDStep.UC获取SN, dirr);
            tb_send.Text = MachineDataDefine.loginstr;
            if (POSTClass.getResult(0, CMDStep.UC获取SN).Result == "OK")
            {
                tb_sn.Text = Post.POSTClass.getResult(0, Post.CMDStep.UC获取SN).dataReturn["SN"];
            }
            else
            {
                MessageBox.Show(JudgeLanguage.JudgeLag("获取SN失败"));
                // tb_receive.Text = ;
            }
            tb_receive.Text = MachineDataDefine.str1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> dir12 = new Dictionary<string, string>();
            dir12.Add("工站", MESDataDefine.MESLXData.terminalName);
            dir12.Add("产品SN", tb_sn.Text);
            POSTClass.AddCMD(0, CMDStep.检查UOP, dir12);
            tb_send.Text = MachineDataDefine.loginstr;
            tb_receive.Text = MachineDataDefine.str1;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            tb_send.Text = "";
            tb_receive.Text = "";
            Dictionary<string, string> datass = new Dictionary<string, string>();
            datass.Add("工站", MESDataDefine.MESLXData.terminalName);
            datass.Add("产品SN", tb_sn.Text);
            datass.Add("结果", "PASS");
            datass.Add("开始时间", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            datass.Add("结束时间", DateTime.Now.AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss"));
            datass.Add("软件版本", MESDataDefine.MESLXData.SW_Version);
            datass.Add("载具号", tb_uc.Text);
            datass.Add("OffsetX值", "999");
            datass.Add("OffsetY值", "999");
            datass.Add("OffsetR值", "999");
            datass.Add("P1值", "999");
            datass.Add("P2值", "999");
            datass.Add("P3值", "999");
            datass.Add("P4值", "999");
            datass.Add("P5值", "999");
            datass.Add("P6值", "999");
            datass.Add("P7值", "999");
            datass.Add("P8值", "999");
            datass.Add("ShiftP1P5值", "999");
            datass.Add("ShiftP2P6值", "999");
            datass.Add("ShiftP3P7值", "999");
            datass.Add("ShiftP4P8值", "999");
            datass.Add("调整次数", 5.ToString());
            Post.POSTClass.AddCMD(0, Post.CMDStep.上传数据, datass);
            tb_send.Text = MachineDataDefine.loginstr;

            if (POSTClass.getResult(0, CMDStep.上传数据).Result == "OK")
            {
                Dictionary<string, string> datas2 = new Dictionary<string, string>();
                datas2.Add("工站", MESDataDefine.MESLXData.terminalName);
                datas2.Add("产品SN", tb_sn.Text);
                Post.POSTClass.AddCMD(0, Post.CMDStep.提交过站, datas2);
            }
            else
            {
                MessageBox.Show(JudgeLanguage.JudgeLag("上传数据给MES失败"));
            }
            tb_receive.Text = MachineDataDefine.str1;
        }

        private void btn_PDCA_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> datass11 = new Dictionary<string, string>();
            datass11.Add("产品SN", ""/*AxisTakeIn.ProductSN*/);
            datass11.Add("开始时间", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            datass11.Add("结束时间", DateTime.Now.AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss"));
            datass11.Add("软件版本", MESDataDefine.MESLXData.SW_Version);
            datass11.Add("载具号", ""/*AxisTakeIn.holeSN*/);
            datass11.Add("OffsetX值", "999");
            datass11.Add("OffsetY值", "999");
            datass11.Add("OffsetR值", "999");
            datass11.Add("P1值", "999");
            datass11.Add("P2值", "999");
            datass11.Add("P3值", "999");
            datass11.Add("P4值", "999");
            datass11.Add("P5值", "999");
            datass11.Add("P6值", "999");
            datass11.Add("P7值", "999");
            datass11.Add("P8值", "999");
            datass11.Add("Shift1值", "999");
            datass11.Add("Shift2值", "999");
            datass11.Add("Shift3值", "999");
            datass11.Add("Shift4值", "999");
            datass11.Add("调整次数", 5.ToString());
            datass11.Add("压缩图片路径", MESDataDefine.MESLXData.StrPDCAImagePath + MachineDataDefine.ZipFilePath);
            datass11.Add("电脑名称", MESDataDefine.MESLXData.StrUser);
            datass11.Add("电脑密码", MESDataDefine.MESLXData.StrPassWord);
            Post.POSTClass.AddCMD(0, Post.CMDStep.上传PDCA, datass11);
            tb_send.Text = MachineDataDefine.pdcastr;
            if (Post.POSTClass.getResult(0, CMDStep.上传PDCA).Result == "NG")
            {
                MessageBox.Show(JudgeLanguage.JudgeLag("上传数据给PDCA失败"));
            }
            tb_receive.Text = MachineDataDefine.str1;
        }
        public static List<Control> control = new List<Control>(); //Login中所有控件
        private void frm_LXManaualMes_Load(object sender, EventArgs e)
        {
            getControl(this.Controls);
            for (int i = 0; i < control.Count; i++)
            {
                control[i].Text = JudgeLanguage.JudgeLag(control[i].Text);
            }
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
    }
}