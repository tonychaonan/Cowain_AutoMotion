using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Cowain_AutoMotion;
using DevExpress.XtraCharts;
using System.IO;
using Chart;
using Cowain;

namespace Cowain_Form.FormView
{
    public partial class AlarmForm : DevExpress.XtraEditors.XtraForm
    {
        public AlarmForm()
        {
            InitializeComponent();
        }

        #region 自定义变量
        /// <summary>
        /// 运行状态单天数据
        /// </summary>
        DataTable dtshow;
        /// <summary>
        /// 报错明细数据
        /// </summary>
        private DataTable m_ErrTable = new DataTable();
        /// <summary>
        /// 运行状态累计数据
        /// </summary>
        DataTable m_SumTable = new DataTable();

        /// <summary>
        /// 查询等待提示窗
        /// </summary>
        private HandleWait m_HandleWait = new HandleWait();

        /// 报错前10数据汇总
        /// </summary>
        private DataTable m_ErrTopTable = new DataTable();

        private Dictionary<string, int> recordSpanDic = new Dictionary<string, int>();
        #endregion

        #region 自定义方法

        private void GetNightData()
        {
            gridControl.DataSource = null;
            DataTable nightTable = new DataTable();
            nightTable.Columns.Add("Data", Type.GetType("System.String"));
            nightTable.Columns.Add("Time", Type.GetType("System.String"));
            nightTable.Columns.Add("Code", Type.GetType("System.String"));
            nightTable.Columns.Add("Type", Type.GetType("System.String"));
            nightTable.Columns.Add("Component", Type.GetType("System.String"));
            nightTable.Columns.Add("Duration", Type.GetType("System.String"));
            nightTable.Columns.Add("Notes", Type.GetType("System.String"));

            nightTable.Rows.Add(new object[] { "2022/10/01", "8:00:11", "ERR2002", "Motion Error;Timeout", "Material", "Ongoing", "1.请检查流线，阻挡气缸缩回感应器是否有信号" });
            nightTable.Rows.Add(new object[] { "2022/10/01", "8:00:11", "ERR2002", "Motion Error;Timeout", "Material", "Ongoing", "1.请检查流线，阻挡气缸缩回感应器是否有信号" });
            nightTable.Rows.Add(new object[] { "2022/10/01", "8:00:11", "ERR2002", "Motion Error;Timeout", "Material", "Ongoing", "1.请检查流线，阻挡气缸缩回感应器是否有信号" });
            nightTable.Rows.Add(new object[] { "2022/10/01", "8:00:11", "ERR2002", "Motion Error;Timeout", "Material", "Ongoing", "1.请检查流线，阻挡气缸缩回感应器是否有信号" });
            nightTable.Rows.Add(new object[] { "2022/10/01", "8:00:11", "ERR2002", "Motion Error;Timeout", "Material", "Ongoing", "1.请检查流线，阻挡气缸缩回感应器是否有信号" });
            nightTable.Rows.Add(new object[] { "2022/10/01", "8:00:11", "ERR2002", "Motion Error;Timeout", "Material", "Ongoing", "1.请检查流线，阻挡气缸缩回感应器是否有信号" });
            nightTable.Rows.Add(new object[] { "2022/10/01", "8:00:11", "ERR2002", "Motion Error;Timeout", "Material", "Ongoing", "1.请检查流线，阻挡气缸缩回感应器是否有信号" });
            nightTable.Rows.Add(new object[] { "2022/10/01", "8:00:11", "ERR2002", "Motion Error;Timeout", "Material", "Ongoing", "1.请检查流线，阻挡气缸缩回感应器是否有信号" });
            gridControl.DataSource = nightTable;
        }
        /// <summary>
        /// 读取报警记录
        /// </summary>
        /// <param name="selTime"></param>
        /// <returns></returns>
        private string ReadErr(DateTime selTime)
        {
            return frm_Main.formError.ErrorUnit1.readError(selTime.ToString("yyyy_MM_dd"));
        }

        /// <summary>
        /// 显示宕机原因
        /// </summary>
        /// <param name="dic"></param>
        private void ShowErrTop(Dictionary<string, int> dic)
        {
            Series[] series = this.chartDown.SeriesSerializable;

            if (series.Length > 0)
            {
                SeriesPointCollection colls = series[0].Points;
                colls.Clear();
                int index = 0;
                foreach (string key in dic.Keys)
                {
                    if (index > 9)
                        break;
                    SeriesPoint seriesPoint = new SeriesPoint(key, new object[] { ((object)(dic[key])) });
                    seriesPoint.Color = SetPieDiagramBackColor(index);
                    series[0].Points.AddRange(new DevExpress.XtraCharts.SeriesPoint[] { seriesPoint });
                    index++;
                }
            }
        }

        /// <summary>
        /// 设置饼状图得每一部分颜色
        /// </summary>
        /// <returns></returns>
        private  Color  SetPieDiagramBackColor(int topIndex)
        {
            Color color = Color.White;
            switch (topIndex)
            {

                case 0:
                    color = Color.FromArgb(111, 121, 128);
                    break;
                case 1:
                    color = Color.FromArgb(84, 151, 193);
                    break;
                case 2:
                    color = Color.FromArgb(83, 172, 122);
                    break;
                case 3:
                    color = Color.FromArgb(248, 195, 92);
                    break;
                case 4:
                    color = Color.FromArgb(243, 150, 91);
                    break;
                case 5:
                    color = Color.FromArgb(228, 94, 105);
                    break;
                case 6:
                    color = Color.FromArgb(125, 114, 187);
                    break;
                case 7:
                    color = Color.FromArgb(76, 170, 233);
                    break;
                case 8:
                    color = Color.FromArgb(102, 217, 56);
                    break;
                case 9:
                    color = Color.FromArgb(194, 72, 134);
                    break;

                default:
                    break;
            }


            return color;

        }

        /// <summary>
        /// 显示报警次数
        /// </summary>
        /// <param name="dic"></param>
        private void ShowAlarm(Dictionary<string, int> dic)
        {
            Series[] series = this.chartAlarm.SeriesSerializable;
            if(series.Length > 0)
            {
                SeriesPointCollection colls = series[0].Points;
                int index = 0;
                foreach (string key in dic.Keys)
                {
                    colls[index].Argument = key;
                    colls[index].Values[0] = dic[key];

                    index++;
                }
            }
        }

        /// <summary>
        /// 报警时长分割
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        private string[] SplitErr(string[] lines)
        {
            List<string> errList = new List<string>();
            for (int i = 0; i < lines.Length; i++)
            {
                string[] cur = lines[i].Split(',');
                if (cur.Length == 7)
                {
                    DateTime curS = ConvertHelper.GetDef_DateTime(cur[1] + " " + cur[3]);
                    DateTime curE = ConvertHelper.GetDef_DateTime(cur[1] + " " + cur[4]);

                    if (curS.Hour != curE.AddSeconds(-1).Hour)
                    {
                        if (curS.Hour < 5)
                        {
                            #region 
                            if (curE.AddSeconds(-1).Hour < 5)
                            {
                                //不用拆分
                                errList.Add(lines[i]);
                            }
                            else if (curE.AddSeconds(-1).Hour >= 20)
                            {
                                //拆成5段
                                errList.Add(cur[0] + "," + cur[1] + "," + cur[2] + ",20:00:00," + cur[4] + ",0s" + cur[6]);
                                errList.Add(cur[0] + "," + cur[1] + "," + cur[2] + ",15:00:00," + ",20:00:00," + ",0s" + cur[6]);
                                errList.Add(cur[0] + "," + cur[1] + "," + cur[2] + ",10:00:00," + ",15:00:00," + ",0s" + cur[6]);
                                errList.Add(cur[0] + "," + cur[1] + "," + cur[2] + ",5:00:00," + ",10:00:00," + ",0s" + cur[6]);
                                errList.Add(cur[0] + "," + cur[1] + "," + cur[2] + cur[3] + ",5:00:00," + ",0s" + cur[6]);
                            }
                            else if (curE.AddSeconds(-1).Hour < 20)
                            {
                                //拆成4段
                                errList.Add(cur[0] + "," + cur[1] + "," + cur[2] + ",15:00:00," + cur[4] + ",0s" + cur[6]);
                                errList.Add(cur[0] + "," + cur[1] + "," + cur[2] + ",10:00:00," + ",15:00:00," + ",0s" + cur[6]);
                                errList.Add(cur[0] + "," + cur[1] + "," + cur[2] + ",5:00:00," + ",10:00:00," + ",0s" + cur[6]);
                                errList.Add(cur[0] + "," + cur[1] + "," + cur[2] + cur[3] + ",5:00:00," + ",0s" + cur[6]);
                            }
                            else if (curE.AddSeconds(-1).Hour < 15)
                            {
                                //拆成3段
                                errList.Add(cur[0] + "," + cur[1] + "," + cur[2] + ",10:00:00," + cur[4] + ",0s" + cur[6]);
                                errList.Add(cur[0] + "," + cur[1] + "," + cur[2] + ",5:00:00," + ",10:00:00," + ",0s" + cur[6]);
                                errList.Add(cur[0] + "," + cur[1] + "," + cur[2] + cur[3] + ",5:00:00," + ",0s" + cur[6]);
                            }
                            else if (curE.AddSeconds(-1).Hour < 10)
                            {
                                //拆成2段
                                errList.Add(cur[0] + "," + cur[1] + "," + cur[2] + ",5:00:00," + cur[4] + ",0s" + cur[6]);
                                errList.Add(cur[0] + "," + cur[1] + "," + cur[2] + cur[3] + ",5:00:00," + ",0s" + cur[6]);
                            }
                            #endregion
                        }
                        else if (curS.Hour < 10)
                        {
                            #region 
                            if (curE.AddSeconds(-1).Hour < 10)
                            {
                                //不用拆分
                                errList.Add(lines[i]);
                            }
                            else if (curE.AddSeconds(-1).Hour >= 20)
                            {
                                //拆成4段
                                errList.Add(cur[0] + "," + cur[1] + "," + cur[2] + ",20:00:00," + cur[4] + ",0s" + cur[6]);
                                errList.Add(cur[0] + "," + cur[1] + "," + cur[2] + ",15:00:00," + ",20:00:00," + ",0s" + cur[6]);
                                errList.Add(cur[0] + "," + cur[1] + "," + cur[2] + ",10:00:00," + ",15:00:00," + ",0s" + cur[6]);
                                errList.Add(cur[0] + "," + cur[1] + "," + cur[2] + cur[3] + ",10:00:00," + ",0s" + cur[6]);
                            }
                            else if (curE.AddSeconds(-1).Hour < 20)
                            {
                                //拆成3段
                                errList.Add(cur[0] + "," + cur[1] + "," + cur[2] + ",15:00:00," + cur[4] + ",0s" + cur[6]);
                                errList.Add(cur[0] + "," + cur[1] + "," + cur[2] + ",10:00:00," + ",15:00:00," + ",0s" + cur[6]);
                                errList.Add(cur[0] + "," + cur[1] + "," + cur[2] + cur[3] + ",10:00:00," + ",0s" + cur[6]);
                            }
                            else if (curE.AddSeconds(-1).Hour < 15)
                            {
                                //拆成2段
                                errList.Add(cur[0] + "," + cur[1] + "," + cur[2] + ",10:00:00," + cur[4] + ",0s" + cur[6]);
                                errList.Add(cur[0] + "," + cur[1] + "," + cur[2] + cur[3] + ",10:00:00," + ",0s" + cur[6]);
                            }
                            #endregion
                        }
                        else if (curS.Hour < 15)
                        {
                            #region 
                            if (curE.AddSeconds(-1).Hour < 15)
                            {
                                //不用拆分
                                errList.Add(lines[i]);
                            }
                            else if (curE.AddSeconds(-1).Hour >= 20)
                            {
                                //拆成3段
                                errList.Add(cur[0] + "," + cur[1] + "," + cur[2] + ",20:00:00," + cur[4] + ",0s" + cur[6]);
                                errList.Add(cur[0] + "," + cur[1] + "," + cur[2] + ",15:00:00," + ",20:00:00," + ",0s" + cur[6]);
                                errList.Add(cur[0] + "," + cur[1] + "," + cur[2] + cur[3] + ",15:00:00," + ",0s" + cur[6]);
                            }
                            else if (curE.AddSeconds(-1).Hour < 20)
                            {
                                //拆成2段
                                errList.Add(cur[0] + "," + cur[1] + "," + cur[2] + ",15:00:00," + cur[4] + ",0s" + cur[6]);
                                errList.Add(cur[0] + "," + cur[1] + "," + cur[2] + cur[3] + ",15:00:00," + ",0s" + cur[6]);
                            }
                            #endregion
                        }
                        else if (curS.Hour < 20)
                        {
                            #region 
                            if (curE.AddSeconds(-1).Hour < 20)
                            {
                                //不用拆分
                                errList.Add(lines[i]);
                            }
                            else if (curE.AddSeconds(-1).Hour >= 20)
                            {
                                //拆成3段
                                errList.Add(cur[0] + "," + cur[1] + "," + cur[2] + ",20:00:00," + cur[4] + ",0s" + cur[6]);
                                errList.Add(cur[0] + "," + cur[1] + "," + cur[2] + cur[3] + ",20:00:00," + ",0s" + cur[6]);
                            }
                            #endregion
                        }
                        else if (curS.Hour >= 20)
                        {
                            //不用处理，全部加进来
                            errList.Add(lines[i]);
                        }
                    }
                }

            }
            
            return errList.ToArray();
        }

        /// <summary>
        /// 初始化DataTable
        /// </summary>
        private void InitTable()
        {
            m_ErrTable.Columns.Add("日期", Type.GetType("System.DateTime"));
            m_ErrTable.Columns.Add("开始时间", Type.GetType("System.DateTime"));
            m_ErrTable.Columns.Add("结束时间", Type.GetType("System.DateTime"));
            m_ErrTable.Columns.Add("报错代码", Type.GetType("System.String"));
            m_ErrTable.Columns.Add("报错信息", Type.GetType("System.String"));
            m_ErrTable.Columns.Add("报错类型", Type.GetType("System.String"));
            m_ErrTable.Columns.Add("持续时间", Type.GetType("System.String"));
            m_ErrTable.Columns.Add("处理方法", Type.GetType("System.String"));
            
            m_SumTable.Columns.Add("Run", Type.GetType("System.Int32"));
            m_SumTable.Columns.Add("Wait", Type.GetType("System.Int32"));
            m_SumTable.Columns.Add("Engineering", Type.GetType("System.Int32"));
            m_SumTable.Columns.Add("PlanneDT", Type.GetType("System.Int32"));
            m_SumTable.Columns.Add("Error", Type.GetType("System.Int32"));

            m_ErrTopTable.Columns.Add("颜色", Type.GetType("System.String"));
            m_ErrTopTable.Columns.Add("报错信息", Type.GetType("System.String"));
            m_ErrTopTable.Columns.Add("持续时间", Type.GetType("System.Decimal"));
            m_ErrTopTable.Columns.Add("次数", Type.GetType("System.Int32"));
        }

        /// <summary>
        /// 数据合并
        /// </summary>
        /// <param name="table"></param>
        private void CollectData(DataTable table, DateTime date)
        {
            int runtime = 0;
            int waittime = 0;
            int engineeringtime = 0;
            int plannedtime = 0;
            int errortime = 0;

            if (table == null || table.Rows.Count < 5)
            {
                return;
            }

            //求总数
            for (int i = 0; i < 24; i++)
            {
                runtime += ConvertHelper.GetDef_Int(table.Rows[0][i].ToString());
                engineeringtime += ConvertHelper.GetDef_Int(table.Rows[2][i].ToString());
                plannedtime += ConvertHelper.GetDef_Int(table.Rows[3][i].ToString());
                errortime += ConvertHelper.GetDef_Int(table.Rows[4][i].ToString());

                if (date.Date == DateTime.Now.Date)
                {
                    int curHour = DateTime.Now.Hour;
                    int k = -1;
                    if (curHour > 7 && curHour < 24)//8-23
                    {
                        k = curHour - 8;
                    }
                    else//0-7
                    {
                        k = curHour + 16;
                    }
                    int curSecond = DateTime.Now.Minute * 60 + DateTime.Now.Second;
                    if (k == i)//当前时间区间
                    {
                        waittime += curSecond - ConvertHelper.GetDef_Int(table.Rows[0][i].ToString()) - ConvertHelper.GetDef_Int(table.Rows[2][i].ToString()) - ConvertHelper.GetDef_Int(table.Rows[3][i].ToString()) - ConvertHelper.GetDef_Int(table.Rows[4][i].ToString());
                    }
                    else if (k > i)//当前时间区间之前
                    {
                        waittime += (3600 - ConvertHelper.GetDef_Int(table.Rows[0][i].ToString()) - ConvertHelper.GetDef_Int(table.Rows[2][i].ToString()) - ConvertHelper.GetDef_Int(table.Rows[3][i].ToString()) - ConvertHelper.GetDef_Int(table.Rows[4][i].ToString()));
                    }
                    else
                    {
                        //当前时间区间之后，待料时间为0，无需累加
                    }
                }
                else
                {
                    //待料时间= 3600-runtime-engineeringtime-plannedtime-errortime
                    waittime += (3600 - ConvertHelper.GetDef_Int(table.Rows[0][i].ToString()) - ConvertHelper.GetDef_Int(table.Rows[2][i].ToString()) - ConvertHelper.GetDef_Int(table.Rows[3][i].ToString()) - ConvertHelper.GetDef_Int(table.Rows[4][i].ToString()));
                }
            }

            //DataRow[] rows = m_SumTable.Select(string.Format("Date = '{0:yyyy-MM-dd}'", date));
            if (m_SumTable.Rows.Count <= 0)
            {
                //添加新行
                DataRow newrow = m_SumTable.NewRow();
                newrow[0] = runtime;
                newrow[1] = waittime;
                newrow[2] = engineeringtime;
                newrow[3] = plannedtime;
                newrow[4] = errortime;

                m_SumTable.Rows.Add(newrow);
            }
            else
            {
                //累加
                m_SumTable.Rows[0][0] = ConvertHelper.GetDef_Int(m_SumTable.Rows[0][0]) + runtime;
                m_SumTable.Rows[0][1] = ConvertHelper.GetDef_Int(m_SumTable.Rows[0][1]) + waittime;
                m_SumTable.Rows[0][2] = ConvertHelper.GetDef_Int(m_SumTable.Rows[0][2]) + engineeringtime;
                m_SumTable.Rows[0][3] = ConvertHelper.GetDef_Int(m_SumTable.Rows[0][3]) + plannedtime;
                m_SumTable.Rows[0][4] = ConvertHelper.GetDef_Int(m_SumTable.Rows[0][4]) + errortime;
            }
        }
        #endregion
        public static List<Control> control = new List<Control>(); //Login中所有控件
        private void AlarmForm_Load(object sender, EventArgs e)
        {
            //GetNightData();
            m_HandleWait.Init(this);
            dateEditS.EditValue = DateTime.Now.Date.AddDays(-1);
            dateEditE.EditValue = DateTime.Now.Date;
            InitTable();
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
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                m_HandleWait.ShowWait();
                
                if (dateEditS.EditValue == null ||
                dateEditS.EditValue.ToString() == "")
                {
                    MsgBoxHelper.DxMsgShowErr("请选择开始时间！");
                    return;
                }
                if (dateEditE.EditValue == null ||
                    dateEditE.EditValue.ToString() == "")
                {
                    MsgBoxHelper.DxMsgShowErr("请选择结束时间！");
                    return;
                }
                DateTime stime = ConvertHelper.GetDef_DateTime(dateEditS.EditValue).Date;
                DateTime etime = ConvertHelper.GetDef_DateTime(dateEditE.EditValue).Date;

                if (Chart.Common.ErrorBase == null)
                    Chart.Common.GetErrorBase();
                //读取ERROR基础表
                DataTable baseTable = Chart.Common.ErrorBase;
                //计算天数
                double days = (etime - stime).TotalDays;
                if (days >= 7)
                {
                    MsgBoxHelper.DxMsgShowErr("时间间隔不能大于7天！");
                    return;
                }
                DateTime curTime = stime;
                //读取数据源
                string errMsg = string.Empty;
                for (int i = 0; i <= days; i++)
                {
                    errMsg += ReadErr(curTime);
                    curTime = curTime.AddDays(1);
                }
                string[] lines = errMsg.Split('\n');
                m_ErrTopTable.Clear();
                #region 宕机原因
                Dictionary<string, int> dic = new Dictionary<string, int>();
                foreach (var item in lines)
                {
                    if (item.Length > 2)
                    {
                        //string a = item.Split(',')[2] + "," + item.Split(',')[6];
                        //string a = item.Split(',')[6].Replace("\r", "");
                        string errcode = item.Split(',')[2];
                        DataRow[] rows = baseTable.Select(string.Format("updatedCode = '{0}'", errcode.Trim()));

                        string type = string.Empty;
                        if (rows.Length > 0)
                        {
                            type = ConvertHelper.GetDef_Str(rows[0][13]);
                        }
                        else
                        {
                            continue;
                        }
                        if (!dic.Keys.Contains(type))
                            dic.Add(type, 1);
                        else
                            dic[type]++;

                        DataRow[] toprows = m_ErrTopTable.Select(string.Format("报错信息 = '{0}'", type));
                        if (toprows.Length == 0)
                        {
                            //新增
                            DataRow newrow = m_ErrTopTable.NewRow();
                            newrow["颜色"] = "";
                            newrow["报错信息"] = type;
                            newrow["持续时间"] = item.Split(',')[5].TrimEnd('s');
                            newrow["次数"] = 1;
                            m_ErrTopTable.Rows.Add(newrow);
                        }
                        else
                        {
                            toprows[0]["持续时间"] = ConvertHelper.GetDef_Dec(toprows[0]["持续时间"]) + ConvertHelper.GetDef_Dec(item.Split(',')[5].TrimEnd('s'));
                            toprows[0]["次数"] = ConvertHelper.GetDef_Int(toprows[0]["次数"]) + 1;
                        }
                    }
                }
                m_ErrTopTable.DefaultView.Sort = "次数 desc";
                DataTable resulttable = m_ErrTopTable.DefaultView.ToTable();
                for (int i = 0; i < resulttable.Rows.Count; i++)
                {
                    resulttable.Rows[i]["颜色"] = i + 1;
                }
                gridControlDT.DataSource = resulttable;
                dic = dic.OrderByDescending(i => i.Value).ToDictionary(p => p.Key, o => o.Value);
                //统计其他项（除了前9的数据）
                Dictionary<string, int> dicOther = new Dictionary<string, int>();
                int index = 0;
                int others = 0;
                foreach (string key in dic.Keys)
                {
                    if (index < 9)
                    {
                        dicOther.Add(key, dic[key]);
                    }
                    else
                    {
                        others += dic[key];
                    }
                    index++;
                }
                if(!dicOther.ContainsKey("其它")&& others > 0)
                {
                    dicOther.Add("其它", others);
                }

                ShowErrTop(dicOther);
                #endregion
                
                #region  报警次数统计
                recordSpanDic.Clear();
                recordSpanDic.Add("0-4", 0);
                recordSpanDic.Add("5-9", 0);
                recordSpanDic.Add("10-14", 0);
                recordSpanDic.Add("15-19", 0);
                recordSpanDic.Add("20-24", 0);
                recordSpanDic.Add("25+", 0);
                foreach (var item in lines)
                {
                    if (item.Length > 2)
                    {
                        double timeSpan = Convert.ToDouble((item.Split(',')[5].Replace("s", "").Trim())) / 60;
                        if (timeSpan < 4)
                        {
                            recordSpanDic["0-4"]++;
                        }
                        else if (timeSpan < 9 && timeSpan >= 4)
                        {
                            recordSpanDic["5-9"]++;
                        }
                        else if (timeSpan < 14 && timeSpan >= 9)
                        {
                            recordSpanDic["10-14"]++;
                        }
                        else if (timeSpan < 19 && timeSpan >= 14)
                        {
                            recordSpanDic["15-19"]++;
                        }
                        else if (timeSpan < 24 && timeSpan >= 19)
                        {
                            recordSpanDic["20-24"]++;
                        }
                        else if (timeSpan >= 24)
                        {
                            recordSpanDic["25+"]++;
                        }
                    }
                }

                ShowAlarm(recordSpanDic);
                #endregion

                #region 表格显示
                //读取ERROR基础表
               // DataTable baseTable1 = Chart.Common.ConfigDataTable.GetConfigDT("ERROR", true);
               //// #region 拼装CODE
               // for (int i = 0; i < baseTable1.Rows.Count; i++)
               // {
               //     baseTable1.Rows[i][12] = ConvertHelper.GetDef_Str(baseTable1.Rows[i][7]) + ConvertHelper.GetDef_Str(baseTable1.Rows[i][8]) + ConvertHelper.GetDef_Str(baseTable1.Rows[i][9]) + ConvertHelper.GetDef_Str(baseTable1.Rows[i][10]) + ConvertHelper.GetDef_Str(baseTable1.Rows[i][11]);
               // }
                #endregion
                m_ErrTable.Clear();
                gridControl.DataSource = null;
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] strs = lines[i].Split(',');
                    if (strs.Length == 7)
                    {
                        string errcode = strs[2];
                        DataRow[] rows = baseTable.Select(string.Format("updatedCode = '{0}'", errcode.Trim()));
                        string type = string.Empty;
                        string description = string.Empty;
                        string action = string.Empty;
                        if (rows.Length > 0)
                        {
                            type = ConvertHelper.GetDef_Str(rows[0][13]);
                            description = ConvertHelper.GetDef_Str(rows[0][2]);
                            action = ConvertHelper.GetDef_Str(rows[0][4]);
                        }
                        else
                        {
                            continue;
                        }
                        //插入新行
                        DataRow newrow = m_ErrTable.NewRow();
                        newrow["日期"] = ConvertHelper.GetDef_DateTime(strs[1]);
                        newrow["开始时间"] = ConvertHelper.GetDef_DateTime(strs[1] + " " + strs[3]);
                        newrow["结束时间"] = ConvertHelper.GetDef_DateTime(strs[1] + " " + strs[4])< ConvertHelper.GetDef_DateTime(strs[1] + " " + strs[3])? ConvertHelper.GetDef_DateTime(strs[1] + " " + strs[4]).AddDays(1): ConvertHelper.GetDef_DateTime(strs[1] + " " + strs[4]);
                        newrow["报错代码"] = errcode;
                        newrow["报错信息"] = description;
                        newrow["报错类型"] = type;
                        newrow["持续时间"] = (ConvertHelper.GetDef_DateTime(newrow["结束时间"]) - ConvertHelper.GetDef_DateTime(strs[1] + " " + strs[3])).TotalMinutes.ToString("f1");
                        newrow["处理方法"] = action;
                        m_ErrTable.Rows.Add(newrow);
                    }
                }

                gridControl.DataSource = m_ErrTable;
               

                #region 运行时间统计
                string outputPath = @"D:\DATA\运行时间";
                string fileName = string.Format("{0}.csv", stime.ToString("yyyy_MM_dd"));
                string fullFileName = Path.Combine(outputPath, fileName);
                m_SumTable.Clear();
                curTime = stime;
                //显示7天数据
                for (int i = 0; i <= days; i++)
                {
                    fileName = string.Format("{0}.csv", curTime.ToString("yyyy_MM_dd"));
                    fullFileName = Path.Combine(outputPath, fileName);
                    //是否存在日志文件
                    if (File.Exists(fullFileName))
                    {
                        //dtshow = new DataTable();
                        dtshow = CSVFileHelper.OpenCSV(fullFileName);

                        if (dtshow.Rows.Count >= 5)
                        {
                            CollectData(dtshow, curTime);
                        }

                    }
                    curTime = curTime.AddDays(1);
                }
                //if (m_SumTable.Rows.Count > 0)
                //{
                //    labRun.Text = (ConvertHelper.GetDef_Dec(m_SumTable.Rows[0][0]) / 3600).ToString("f1");
                //    labIdle.Text = (ConvertHelper.GetDef_Dec(m_SumTable.Rows[0][1]) / 3600).ToString("f1");
                //    labDown.Text = (ConvertHelper.GetDef_Dec(m_SumTable.Rows[0][4]) / 3600).ToString("f1");
                //    labStop.Text = ((ConvertHelper.GetDef_Dec(m_SumTable.Rows[0][2]) + ConvertHelper.GetDef_Dec(m_SumTable.Rows[0][3]) + ConvertHelper.GetDef_Dec(m_SumTable.Rows[0][4])) / 3600).ToString("f1");
                //    labTotal.Text = ((ConvertHelper.GetDef_Dec(m_SumTable.Rows[0][0]) + ConvertHelper.GetDef_Dec(m_SumTable.Rows[0][1]) + ConvertHelper.GetDef_Dec(m_SumTable.Rows[0][2]) + ConvertHelper.GetDef_Dec(m_SumTable.Rows[0][3]) + ConvertHelper.GetDef_Dec(m_SumTable.Rows[0][4]) * 2) / 3600).ToString("f1");
                //}
                //else
                //{
                //    labRun.Text = "0";
                //    labIdle.Text = "0";
                //    labDown.Text = "0";
                //    labStop.Text = "0";
                //    labTotal.Text = "0";
                //}


                #endregion
            }
            catch (Exception err)
            {
                MsgBoxHelper.DxMsgShowErr("查询时出错！"+ err.Message);
            }
            finally
            {
                m_HandleWait.CloseWait();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.InitialDirectory = @"D:\";//打开初始目录
            saveFile.Title = "选择保存文件";
            saveFile.Filter = "csv files (*.csv)|*.csv|All files (*.*)|";//过滤条件
            saveFile.FilterIndex = 1;//获取第二个过滤条件开始的文件拓展名
            saveFile.FileName = "";//默认保存名称
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                CSVFileHelper.SaveCSV(m_ErrTable, saveFile.FileName);
            }
        }

        private void AlarmForm_Shown(object sender, EventArgs e)
        {
            btnSearch.PerformClick();
        }
    }
}