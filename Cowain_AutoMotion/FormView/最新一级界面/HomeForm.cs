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
using System.IO;
using System.Diagnostics;
using Cowain;
using static Chart.ChartTime;
using DevExpress.XtraCharts;
using System.Threading;
using Cowain_Machine.Flow;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using MotionBase;
using System.Net;
using Cowain_AutoMotion;
using Cowain_Machine;
using Cowain_AutoMotion.Flow.Hive;
using Post;
using Cowain_AutoMotion.Flow._2Work;
using ToolTotal;
using Chart;
using DevExpress.XtraPrinting;
using CSVFileHelper = Chart.ChartTime.CSVFileHelper;

namespace Cowain_Form.FormView
{
    public partial class HomeForm : DevExpress.XtraEditors.XtraForm
    {
        public HomeForm(ref clsMachine pM)
        {
            InitializeComponent();
            pMachine = pM;
        }
        #region 自定义变量
        public frm_Main pfrmMain;
        [DllImport("User32.dll")]
        public static extern int PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public clsMachine pMachine;

        public delegate void HomeDelegate();
        public event HomeDelegate SensorEvent;
        /// <summary>
        /// 运行状态单天数据
        /// </summary>
        DataTable dtshow;
        /// <summary>
        /// 运行状态累计数据
        /// </summary>
        DataTable m_SumTable = new DataTable();

        DataTable m_ParamTable = new DataTable();
        /// <summary>
        /// 界面刷新线程
        /// </summary>
        Thread ReFlash_Thread;
        //  MESLXData mESLXData = (MESLXData)MESDataDefine.MESDatas;

        /// <summary>
        /// 查询等待提示窗
        /// </summary>
        private HandleWait m_HandleWait = new HandleWait();
        /// <summary>
        /// 当前选择的Index,默认为当天
        /// </summary>
        private int m_CurIndex = 6;
        #endregion

        /// <summary>
        /// datatable初始化
        /// </summary>
        private void CreatTable()
        {
            m_SumTable.Columns.Clear();
            m_SumTable.Columns.Add("Date", Type.GetType("System.DateTime"));
            m_SumTable.Columns.Add("Run", Type.GetType("System.Int32"));
            m_SumTable.Columns.Add("Wait", Type.GetType("System.Int32"));
            m_SumTable.Columns.Add("Engineering", Type.GetType("System.Int32"));
            m_SumTable.Columns.Add("PlanneDT", Type.GetType("System.Int32"));
            m_SumTable.Columns.Add("Error", Type.GetType("System.Int32"));
            m_ParamTable.Columns.Clear();
            m_ParamTable.Columns.Add("Name", Type.GetType("System.String"));
            //m_ParamTable.Columns.Add("Value", Type.GetType("System.Decimal"));
            //m_ParamTable.Columns.Add("LSL", Type.GetType("System.Decimal"));
            //m_ParamTable.Columns.Add("USL", Type.GetType("System.Decimal"));
            //m_ParamTable.Columns.Add("ID", Type.GetType("System.Int32"));
            m_ParamTable.Columns.Add("Valuestr", Type.GetType("System.String"));
            m_ParamTable.Columns.Add("LSLstr", Type.GetType("System.String"));
            m_ParamTable.Columns.Add("USLstr", Type.GetType("System.String"));

            // decimal lsl = 0;
            //  decimal usl = 0;
            ////胶水码
            //m_ParamTable.Rows.Add("Glue_Lot_Code_1", 0, 0, 0, 0, "", "N/A", "N/A");
            //m_ParamTable.Rows.Add("Glue_Lot_Code_2", 0, 0, 0, 0, "", "N/A", "N/A");
            ////前龙门
            //if (MESDataDefine.MESParamDatas.strABRateEnable[0] == "1")
            //{
            //    lsl = ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrABRateLimitS[0][0]);
            //    usl = ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrABRateLimitS[0][1]);
            //    m_ParamTable.Rows.Add("F_ABRate", 0, lsl, usl,0,0, lsl.ToString(),usl.ToString());
            //}
            foreach (var item in MachineDataDefine.MachineCfgS.SpecClasss)
            {
                if (item.b_Use)
                {
                    m_ParamTable.Rows.Add(item.name, 0, item.LSpec, item.USpec);
                }
            }
            gridControl.DataSource = m_ParamTable;
           // dataGridView1.AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.;
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

            DataRow[] rows = m_SumTable.Select(string.Format("Date = '{0:yyyy-MM-dd}'", date));
            if (rows.Length <= 0)
            {
                //添加新行
                DataRow newrow = m_SumTable.NewRow();
                newrow[0] = date;
                newrow[1] = runtime;
                newrow[2] = waittime;
                newrow[3] = engineeringtime;
                newrow[4] = plannedtime;
                newrow[5] = errortime;

                m_SumTable.Rows.Add(newrow);
            }
            else
            {
                //累加
                rows[0][1] = ConvertHelper.GetDef_Int(rows[0][1]) + runtime;
                rows[0][2] = ConvertHelper.GetDef_Int(rows[0][2]) + waittime;
                rows[0][3] = ConvertHelper.GetDef_Int(rows[0][3]) + engineeringtime;
                rows[0][4] = ConvertHelper.GetDef_Int(rows[0][4]) + plannedtime;
                rows[0][5] = ConvertHelper.GetDef_Int(rows[0][5]) + errortime;
            }
        }

        /// <summary>
        /// 显示运行状态
        /// </summary>
        /// <param name="datasource"></param>
        private void ShowState(DataTable datasource)
        {
            Series[] series = this.chartState.SeriesSerializable;
            for (int i = 0; i < series.Length; i++)
            {
                SeriesPointCollection colls = series[i].Points;
                for (int j = 0; j < colls.Count; j++)
                {
                    //日期
                    colls[j].Argument = ConvertHelper.GetDef_Str(datasource.Rows[j][0]);
                    //数据(百分比)
                    colls[j].Values[0] = ConvertHelper.GetDef_Int(datasource.Rows[j][i + 1]);

                }
            }
        }
        /// <summary>
        /// 显示报错次数
        /// </summary>
        /// <param name="dic"></param>
        private void ShowErrTop(Dictionary<string, int> dic)
        {
            Series[] series = this.chartError.SeriesSerializable;

            if (series.Length > 0)
            {
                SeriesPointCollection colls = series[0].Points;
                colls.Clear();
                int index = 0;
                foreach (string key in dic.Keys)
                {
                    //只显示TOP10
                    if (index > 9)
                        break;
                    SeriesPoint seriesPoint = new SeriesPoint(key, new object[] { ((object)(dic[key])) });
                    series[0].Points.AddRange(new DevExpress.XtraCharts.SeriesPoint[] { seriesPoint });
                    index++;
                }
            }
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        private void SearchAll()
        {
            DateTime selTime = DateTime.Now.Date.AddDays(-6);

            #region 运行时间统计
            string outputPath = @"D:\DATA\运行时间";
            string fileName = string.Format("{0}.csv", selTime.ToString("yyyy_MM_dd"));
            string fullFileName = Path.Combine(outputPath, fileName);
            string curDay = DateTime.Now.ToString("yyyy_MM_dd");
            m_SumTable.Clear();
            DateTime curTime = selTime;
            //显示7天数据
            for (int i = 0; i < 7; i++)
            {
                fileName = string.Format("{0}.csv", curTime.ToString("yyyy_MM_dd"));
                fullFileName = Path.Combine(outputPath, fileName);
                //是否存在日志文件
                if (File.Exists(fullFileName))
                {
                    //dtshow = new DataTable();
                    dtshow = CSVFileHelper.OpenCSV(fullFileName);

                    if (dtshow.Rows.Count < 5)
                    {
                        //读取到数据格式异常时插入空行
                        DataRow newrow = m_SumTable.NewRow();
                        newrow[0] = curTime;
                        newrow[1] = 0;
                        newrow[2] = 0;
                        newrow[3] = 0;
                        newrow[4] = 0;
                        newrow[5] = 0;

                        m_SumTable.Rows.Add(newrow);
                    }
                    else
                        CollectData(dtshow, curTime);
                }
                else
                {
                    //找不到文件就插入空行
                    //添加新行
                    DataRow newrow = m_SumTable.NewRow();
                    newrow[0] = curTime;
                    newrow[1] = 0;
                    newrow[2] = 0;
                    newrow[3] = 0;
                    newrow[4] = 0;
                    newrow[5] = 0;

                    m_SumTable.Rows.Add(newrow);
                }
                curTime = curTime.AddDays(1);
            }

            #region 将实际时间转换成百分比
            for (int i = 0; i < m_SumTable.Rows.Count; i++)
            {
                DateTime time = ConvertHelper.GetDef_DateTime(m_SumTable.Rows[i][0]);
                decimal run = ConvertHelper.GetDef_Dec(m_SumTable.Rows[i][1]);
                decimal wait = ConvertHelper.GetDef_Dec(m_SumTable.Rows[i][2]);
                decimal eng = ConvertHelper.GetDef_Dec(m_SumTable.Rows[i][3]);
                decimal plan = ConvertHelper.GetDef_Dec(m_SumTable.Rows[i][4]);
                decimal error = ConvertHelper.GetDef_Dec(m_SumTable.Rows[i][5]);
                decimal sum = run + wait + eng + plan + error;

                if (sum <= 0)
                {
                    continue;
                }

                DateTime now = System.DateTime.Now.Hour < 8 ? DateTime.Now.AddDays(-1).Date : DateTime.Now.Date;
                decimal rate = 100;
                if (time.Date == now)
                {
                    //当天数据要按比例进行折算
                    rate = sum / (24 * 60 * 60) * 100;
                }

                m_SumTable.Rows[i][1] = ConvertHelper.GetDef_Dec(run / sum * rate, 2);
                m_SumTable.Rows[i][2] = ConvertHelper.GetDef_Dec(wait / sum * rate, 2);
                m_SumTable.Rows[i][3] = ConvertHelper.GetDef_Dec(eng / sum * rate, 2);
                m_SumTable.Rows[i][4] = ConvertHelper.GetDef_Dec(plan / sum * rate, 2);
                m_SumTable.Rows[i][5] = ConvertHelper.GetDef_Dec(error / sum * rate, 2);
            }
            #endregion

            ShowState(m_SumTable);
            #endregion

            //SearchErrorTop(DateTime.Now.Date.AddDays(m_CurIndex - 6));
        }

        /// <summary>
        /// Error TOP10
        /// </summary>
        /// <param name="selStime"></param>
        private void SearchErrorTop(DateTime selStime)
        {
            #region Error TOP10
            if (Chart.Common.ErrorBase == null)
                Chart.Common.GetErrorBase();
            //读取ERROR基础表
            DataTable baseTable = Chart.Common.ErrorBase;
            dateEditS.EditValue = string.Format("{0:yyyy/MM/dd 8:00}", selStime);
            dateEditE.EditValue = string.Format("{0:yyyy/MM/dd 8:00}", selStime.AddDays(1));
            string errMsg = frm_Main.formError.ErrorUnit1.readError(selStime.ToString("yyyy_MM_dd"));
            string[] lines = errMsg.Split('\n');
            Dictionary<string, decimal> dic = new Dictionary<string, decimal>();
            foreach (var item in lines)
            {
                if (item.Length > 2)
                {
                    string type = string.Empty;
                    #region 根据code找到报错类型
                    string errcode = item.Split(',')[2].Trim();
                    DataRow[] rows = baseTable.Select(string.Format("updatedCode = '{0}'", errcode));
                    if (rows.Length > 0)
                    {
                        type = ConvertHelper.GetDef_Str(rows[0][13]);
                    }
                    #endregion

                    //string a = item.Split(',')[2] + "," + item.Split(',')[6];
                    //string a = item.Split(',')[6].Replace("\r", "");
                    decimal time = ConvertHelper.GetDef_Dec(item.Split(',')[5].TrimEnd('s'));
                    if (!dic.Keys.Contains(type))
                        dic.Add(type, time);
                    else
                        dic[type] += time;
                }
            }
            dic = dic.OrderByDescending(i => i.Value).ToDictionary(p => p.Key, o => o.Value);
            //将时间转换成分钟
            Dictionary<string, decimal> dicMin = new Dictionary<string, decimal>();
            foreach (string key in dic.Keys)
            {
                //dic[key] = ConvertHelper.GetDef_Dec(dic[key]/60,0);
                if (!dicMin.ContainsKey(key))
                {
                    dicMin.Add(key, ConvertHelper.GetDef_Dec(dic[key] / 60, 0));
                }
            }

            //ShowErrTop(dic);
            Charts.ShowDataByDic(dicMin, this.chartError, 10);
            #endregion
        }
        private bool m_OnRefresh = false;
        private void SearchErrorTop(DateTime selStime, bool refresh = true)
        {
            m_OnRefresh = true;

            if (Chart.Common.ErrorBase == null)
                Chart.Common.GetErrorBase();
            //读取ERROR基础表
            DataTable baseTable = Chart.Common.ErrorBase;

            #region Error TOP10
            if (refresh)
            {
                dateEditS.EditValue = string.Format("{0:yyyy/MM/dd 8:00}", selStime);
                dateEditE.EditValue = string.Format("{0:yyyy/MM/dd 8:00}", selStime.AddDays(1));
            }

            DateTime stime = ConvertHelper.GetDef_DateTime(dateEditS.EditValue);
            DateTime etime = ConvertHelper.GetDef_DateTime(dateEditE.EditValue);
            //计算间隔时间
            DateTime curStime = stime.Hour < 8 ? stime.AddDays(-1).Date : stime.Date;
            DateTime curEtime = etime.Hour <= 8 ? etime.AddDays(-1).Date : etime.Date;

            DateTime curTime = curStime;
            TimeSpan span = curEtime - curStime;
            string errMsg = string.Empty;
            for (int i = 0; i < (int)span.TotalDays + 1; i++)
            {
                errMsg += frm_Main.formError.ErrorUnit1.readError(curTime.ToString("yyyy_MM_dd"));
                curTime = curTime.AddDays(1);
            }

            string[] lines = errMsg.Split('\n');
            Dictionary<string, decimal> dic = new Dictionary<string, decimal>();
            foreach (var item in lines)
            {
                if (item.Length > 2)
                {
                    //判断时间是否在查询时间段内
                    DateTime time = ConvertHelper.GetDef_DateTime(item.Split(',')[1] + " " + item.Split(',')[3]);
                    if (time < stime || time > etime)
                    {
                        continue;
                    }
                    string a = item.Split(',')[6].Replace("\r", "");
                    string type = string.Empty;
                    #region 根据code找到报错类型
                    string errcode = item.Split(',')[2].Trim();
                    DataRow[] rows = baseTable.Select(string.Format("updatedCode = '{0}'", errcode));
                    if (rows.Length > 0)
                    {
                        type = ConvertHelper.GetDef_Str(rows[0][13]);
                    }
                    else
                        continue;
                    #endregion

                    decimal val = ConvertHelper.GetDef_Dec(item.Split(',')[5].TrimEnd('s'));
                    if (!dic.Keys.Contains(type))
                        dic.Add(type, val);
                    else
                        dic[type] += val;
                }
            }
            dic = dic.OrderByDescending(i => i.Value).ToDictionary(p => p.Key, o => o.Value);
            //将时间转换成分钟
            Dictionary<string, decimal> dicMin = new Dictionary<string, decimal>();
            foreach (string key in dic.Keys)
            {
                //dic[key] = ConvertHelper.GetDef_Dec(dic[key]/60,0);
                if (!dicMin.ContainsKey(key))
                {
                    dicMin.Add(key, ConvertHelper.GetDef_Dec(dic[key] / 60, 0));
                }
            }

            //ShowErrTop(dic);
            Charts.ShowDataByDic(dicMin, this.chartError, 10);
            #endregion
            m_OnRefresh = false;
        }

        private void DoReflash()
        {
            while (true)
            {
                try
                {
                    if (this.Visible)
                    {
                        Refreash();
                        // 占用时间较长，放在线程中执行
                        bool connect = false;
                        if (MachineDataDefine.machineState.b_UseHive)
                        {
                            connect = Commons.PingIP(MESDataDefine.MESLXData.StrHIVEIP, 3000);
                            ShowHiveState(connect);
                        }
                        else
                        {
                            ShowHiveState(false);
                        }
                        if (MachineDataDefine.machineState.b_UseMes)
                        {
                            connect = Commons.PingIP(MESDataDefine.MESLXData.StrMesIP, 3000);
                            ShowMesState(connect);
                        }
                        else
                        {
                            ShowMesState(false);
                        }
                        if (POSTClass.socket == null || !POSTClass.socket.ClientSocket.Connected || !POSTClass.socket.connectOk)
                        {
                            //if (labPDCA.Text != JudgeLanguage.JudgeLag("PDCA Disconnected"))
                            //{
                            ShowPDCAState(false);
                            //}

                        }
                        else if (POSTClass.socket.ClientSocket.Connected && POSTClass.socket.connectOk)
                        {
                            //if (labPDCA.Text != JudgeLanguage.JudgeLag("PDCA Connected"))
                            //{
                            ShowPDCAState(true);
                            //}

                        }
                    }
                    Thread.Sleep(5000);
                }
                catch (Exception)
                {

                }
            }
        }
        /// <summary>
        /// 界面显示刷新
        /// </summary>
        private void Refreash()
        {
            if (this == null)
            {
                return;
            }

            if (this.InvokeRequired)
            {
                try
                {
                    this.Invoke(new Action(Refreash));
                    return;
                }
                catch (Exception e)
                {
                    return;
                }
            }

            dateEditS.Properties.MinValue = ConvertHelper.GetDef_DateTime(DateTime.Now.AddDays(-6).ToString("yyyy/MM/dd 08:00"));
            dateEditS.Properties.MaxValue = DateTime.Now.Hour < 8 ? ConvertHelper.GetDef_DateTime(DateTime.Now.ToString("yyyy/MM/dd 08:00")) : ConvertHelper.GetDef_DateTime(DateTime.Now.AddDays(1).ToString("yyyy/MM/dd 08:00"));
            dateEditE.Properties.MinValue = ConvertHelper.GetDef_DateTime(DateTime.Now.AddDays(-6).ToString("yyyy/MM/dd 08:00"));
            dateEditE.Properties.MaxValue = DateTime.Now.Hour < 8 ? ConvertHelper.GetDef_DateTime(DateTime.Now.ToString("yyyy/MM/dd 08:00")) : ConvertHelper.GetDef_DateTime(DateTime.Now.AddDays(1).ToString("yyyy/MM/dd 08:00"));

            #region Input Summary

            DataTable yieTable = frm_Main.formData.Chartcapacity1.GetDayYield(DateTime.Now);
            if (yieTable != null && yieTable.Rows.Count > 0)
            {
                //合计
                int dttotal = 0;
                int oktotal = 0;
                int total = 0;
                #region 全部数据累加，已禁用
                //for (int j = 0; j < 24; j++)
                //{
                //    int okleft = Convert.ToInt32(yieTable.Rows[0][j].ToString());
                //    int okright = Convert.ToInt32(yieTable.Rows[1][j].ToString());
                //    int dtleft = Convert.ToInt32(yieTable.Rows[2][j].ToString());
                //    int dtright = Convert.ToInt32(yieTable.Rows[3][j].ToString());
                //    oktotal += (okleft + okright);
                //    dttotal += (dtleft + dtright);
                //    total += (okleft + okright + dtleft + dtright);
                //}
                #endregion

                #region 只取当前小时的数据
                if (yieTable.Columns.Count >= 24)
                {
                    int hour = DateTime.Now.Hour;

                    int okleft = 0;
                    int okright = 0;
                    int dtleft = 0;
                    int dtright = 0;

                    if (hour >= 8)
                    {
                        okleft = Convert.ToInt32(yieTable.Rows[0][hour - 8].ToString());
                        okright = Convert.ToInt32(yieTable.Rows[1][hour - 8].ToString());
                        dtleft = Convert.ToInt32(yieTable.Rows[2][hour - 8].ToString());
                        dtright = Convert.ToInt32(yieTable.Rows[3][hour - 8].ToString());
                    }
                    else
                    {
                        okleft = Convert.ToInt32(yieTable.Rows[0][16 + hour].ToString());
                        okright = Convert.ToInt32(yieTable.Rows[1][16 + hour].ToString());
                        dtleft = Convert.ToInt32(yieTable.Rows[2][16 + hour].ToString());
                        dtright = Convert.ToInt32(yieTable.Rows[3][16 + hour].ToString());
                    }
                    oktotal = okleft; //+ okright);
                    dttotal = (dtleft + dtright + okright);
                    total = (okleft + okright + dtleft + dtright);
                }
                #endregion

                labInput.Text = total.ToString() + "/" + total.ToString();
                labYield.Text = ((total - dttotal) * 1.0 / total * 100).ToString("f2") + "%";
                labPass.Text = oktotal.ToString() + "/" + dttotal.ToString();
                labUPH.Text = total.ToString();
            }

            #region CT
            string message = frm_Main.formData.CTUnit1.ReadCT(DateTime.Now.Hour > 7 ? DateTime.Now.ToString("yyyy_MM_dd") : DateTime.Now.AddDays(-1).ToString("yyyy_MM_dd"));
            if (!message.Trim().Equals(""))
            {
                //解析数据
                string[] sn = message.Split('\n');

                string[] temp;
                string dates;
                List<double> ct = new List<double>();
                for (int j = 0; j < sn.Length - 1; j++)
                {

                    dates = sn[j].Trim('\r');
                    temp = dates.Split(',');
                    if (ConvertHelper.GetDef_DateTime(temp[0] + " " + temp[1]).Hour == DateTime.Now.Hour)
                    {
                        if (temp.Length > 7) //新增了一个outputCT，防止出现问题，判断一下长度  2023-5-5
                        {
                            ct.Add(Convert.ToDouble(temp[7]));
                        }
                        else
                        {
                            ct.Add(Convert.ToDouble(temp[5])); //这是原来的CT
                        }

                    }
                }
                if (ct.Count > 0)
                {
                    //labUPH.Text = (ct.Average()==0?0:3600 /ct.Average()).ToString("f0");
                    //labUPH.Text = ct.Count.ToString();
                    labCT.Text = (ct.Average()).ToString("f1");
                }


                //增加时间
                //date.Add(prj);
                //numbers.Add(ct.Count());
                //avreagect.Add(ct.Average() / gantryCount);
                //datelist.Add(new CalculationDate() { Date = prj, CT = (ct.Average() / gantryCount).ToString("0.0"), Count = ct.Count() });
            }

            #endregion

            #endregion

            #region MachineInfo
            //if (MachineDataDefine.MachineCfgS.MachineFactoryEumn == MachineFactory.立讯)
            //{
            //    labStationID.Text = mESLXData.StrStationID;
            //    //labMachineID.Text = mESLXData.StrMachineID;
            //}
            labMachineID.Text = MachineDataDefine.settingData.Station + " #" + MachineDataDefine.settingData.Machine;
            labStationID.Text = MachineDataDefine.settingData.Site;
            labVersion.Text = MESDataDefine.MESLXData.SW_Version;
            labMSHash.Text = HIVEDataDefine.HIVE_sha1_Data.Main_SW_SHA1_Name;
            labMSHash.ToolTip = HIVEDataDefine.HIVE_sha1_Data.Main_SW_SHA1_Name;
            DateTime changeTime = frm_Main.formData.ChartTime1.StatusTime;
            //labSTime.Text = changeTime.ToString("yyyy-MM-dd HH:mm:ss");
            labPath.Text = HIVEDataDefine.HIVE_sha1_Data.Main_SW_Path;
            labPath.ToolTip = HIVEDataDefine.HIVE_sha1_Data.Main_SW_Path;
            //labSN.Text = Cowain_AutoMotion.AxisTakeIn.ProductSN;
            //if (Cowain_AutoMotion.AxisTakeIn.b_Result)
            //{
            //    labSN.BackColor = Color.Lime;
            //}
            //else
            //{
            //    labSN.BackColor = Color.Red;
            //}
            #endregion

            #region StationCard
            if (MachineDataDefine.machineState.b_UseRemoteQualification)
            {
                panelState.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
                panelState.BackColor = Color.FromArgb(255, 143, 0);
            }
            else
            {
                panelState.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            }

            #endregion

            //实时显示当前数据
            //try
            //{

            //}
            //catch
            //{

            //}
            SearchAll();
            if (MachineDataDefine.machineState.b_UseCCD == true && MachineDataDefine.ShowVaule)
            {
                MachineDataDefine.ShowVaule = false;
                m_ParamTable.Rows.Clear();
                for (int i = 0; i < MachineDataDefine.ShowPram.Length; i++)
                {
                    MachineDataDefine.MachineCfgS.SpecClasss[i].SValue = Convert.ToDouble(MachineDataDefine.ShowPram[i]);
                }
                foreach (var item in MachineDataDefine.MachineCfgS.SpecClasss)
                {
                    if (item.b_Use)
                    {
                        m_ParamTable.Rows.Add(item.name, item.SValue, item.LSpec, item.USpec);
                    }
                }
                gridControl.DataSource = m_ParamTable;
            }
            #region 操作权限判定
            if ((int)pMachine.m_LoginUser < 1)
            {
                //只读权限
                labSensor.Visible = false;
                dateEditS.Enabled = false;
                dateEditE.Enabled = false;
                hScrollBar.Enabled = false;
                //labPath.Enabled = false;
                //btnMachineLog.Enabled = false;
                //btnHiveLog.Enabled = false;
            }
            else
            {
                labSensor.Visible = true;
                dateEditS.Enabled = true;
                dateEditE.Enabled = true;
                hScrollBar.Enabled = true;
                //labPath.Enabled = true;
                //btnMachineLog.Enabled = true;
                //btnHiveLog.Enabled = true;
            }
            #endregion
        }

        Point panelGlunStartPoint;

        /// <summary>
        /// 显示MES连接状态
        /// </summary>
        /// <param name="connect"></param>
        private void ShowMesState(bool connect)
        {
            if (this == null)
            {
                return;
            }

            if (this.InvokeRequired)
            {
                try
                {
                    this.Invoke(new Action<bool>(ShowMesState), new object[] { connect });
                    return;
                }
                catch (Exception e)
                {
                    return;
                }
            }

            #region MES
            if (MachineDataDefine.machineState.b_UseMes)
            {
                if (connect)
                {
                    if (labMES.Text != JudgeLanguage.JudgeLag("MES Connected"))
                    {
                        labMES.Text = JudgeLanguage.JudgeLag("MES Connected");
                        labMES.ForeColor = Color.Lime;
                        labMES.Enabled = false;
                    }

                }
                else
                {
                    if (labMES.Text != JudgeLanguage.JudgeLag("MES Disconnected"))
                    {
                        labMES.Text = JudgeLanguage.JudgeLag("MES Disconnected");
                        labMES.ForeColor = Color.Red;
                    }


                    if (MachineDataDefine.IsAutoing && /*!MachineDataDefine.machineState.IsDisableMes &&*/ !labMES.Enabled)
                    {
                        labMES.Enabled = true;
                        //armShowMessage = " MES通讯异常，请双击重新连接！";
                        //alarmCode = MErrorDefine.MErrorCode.MES通讯失败;
                        //pError = new Error(ref pMachine.m_NowAddress, armShowMessage, "", (int)alarmCode);
                        //pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    }

                }
            }
            else
            {
                labMES.Text = JudgeLanguage.JudgeLag("MES Disconnected");
                labMES.ForeColor = Color.Red;
            }
            #endregion
        }
        /// <summary>
        /// 显示HIVE连接状态
        /// </summary>
        /// <param name="connect"></param>
        private void ShowHiveState(bool connect)
        {
            if (this == null)
            {
                return;
            }

            if (this.InvokeRequired)
            {
                try
                {
                    this.Invoke(new Action<bool>(ShowHiveState), new object[] { connect });
                    return;
                }
                catch (Exception e)
                {
                    return;
                }
            }

            #region HIVE
            //if (!MachineDataDefine.machineState.IsDisableHive)
            //{
            if (MachineDataDefine.machineState.b_UseHive && connect)
            {
                if (labHIVE.Text != JudgeLanguage.JudgeLag("Hive Connected"))
                {
                    labHIVE.Text = JudgeLanguage.JudgeLag("Hive Connected");
                    labHIVE.ForeColor = Color.Lime;
                    labHIVE.Enabled = false;
                }


            }
            else
            {
                if (labHIVE.Text != JudgeLanguage.JudgeLag("Hive Disconnected"))
                {
                    labHIVE.Text = JudgeLanguage.JudgeLag("Hive Disconnected");
                    labHIVE.ForeColor = Color.Red;
                }


                if (MachineDataDefine.IsAutoing && MachineDataDefine.machineState.b_UseHive && !labHIVE.Enabled)
                {
                    labHIVE.Enabled = true;
                    //armShowMessage = " HIVE通讯异常，请双击重新连接！";
                    //alarmCode = MErrorDefine.MErrorCode.HIVE通讯失败;
                    //pError = new Error(ref pMachine.m_NowAddress, armShowMessage, "", (int)alarmCode);
                    //pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                }

            }
            //}
            #endregion
        }
        /// <summary>
        /// 显示PDCA连接状态
        /// </summary>
        /// <param name="connect"></param>
        private void ShowPDCAState(bool connect)
        {
            if (this == null)
            {
                return;
            }

            if (this.InvokeRequired)
            {
                try
                {
                    this.Invoke(new Action<bool>(ShowPDCAState), new object[] { connect });
                    return;
                }
                catch (Exception e)
                {
                    return;
                }
            }

            #region PDCA
            if (MachineDataDefine.machineState.b_UsePDCA && connect)
            {
                if (labPDCA.Text != JudgeLanguage.JudgeLag("PDCA Connected"))
                {
                    labPDCA.Text = JudgeLanguage.JudgeLag("PDCA Connected");
                    labPDCA.ForeColor = Color.Lime;
                    labPDCA.Enabled = false;
                }


            }
            else
            {
                if (labPDCA.Text != JudgeLanguage.JudgeLag("PDCA Disconnected"))
                {
                    labPDCA.Text = JudgeLanguage.JudgeLag("PDCA Disconnected");
                    labPDCA.ForeColor = Color.Red;
                    //labPDCA.Enabled = true;//无需手动重连
                }
            }
            #endregion
        }


      

        private void ShowIndex(int index)
        {
            SearchErrorTop(DateTime.Now.Date.AddDays(index - 6));
        }

        /// <summary>
        /// 显示最近一片料的做料情况（上传MES成功与否）
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="ok"></param>
        public void ShowSN(string sn, bool ok)
        {
            //if (this.Visible)
            //{
            labSN.Text = sn;
            if (ok)
            {
                labSN.BackColor = Color.FromArgb(0, 249, 0);
            }
            else
            {
                labSN.BackColor = Color.FromArgb(236, 93, 87);
            }
            //}            
        }

        private void HomeForm_Load(object sender, EventArgs e)
        {
            Mainflow.ShowLogEvent += ShowLog;
            Mainflow.showSN_Event += ShowtxtSN;
            Mainflow.showResultEvent += ShowResult;
            Mainflow.showHinttEvent += ShowHintt;
            Mainflow.ShowtxtSN += txtSN11;
            clsMachine.showHinttEvent += ShowHintt;
            label1.Text = "";

            m_HandleWait.Init(this);
            CreatTable();
            ReFlash_Thread = new Thread(DoReflash);
            ReFlash_Thread.Priority = ThreadPriority.Lowest;
            ReFlash_Thread.IsBackground = true;
            ReFlash_Thread.Start();

            hScrollBar.Value = m_CurIndex;

            if (MachineDataDefine.machineState.CheckUCgetSN)
            {
                checkUcgetSN.Checked = true;
                groupBox5.Visible = true;
                txtSN.Enabled = false;
            }
            else
            {
                checkUcgetSN.Checked=false;
                groupBox5.Visible = false;
                txtSN.Enabled = true;
            }

            foreach (Limit item in HIVEDataDefine.limitdata)
            {
                dataGridView1.Rows.Add(item.Name, item.Standard.ToString());
            }
            dataGridView1.ClearSelection();
            //dateEditS.Properties.MinValue = ConvertHelper.GetDef_DateTime(DateTime.Now.AddDays(-6).ToString("yyyy/MM/dd 08:00"));
            //dateEditS.Properties.MaxValue = DateTime.Now.Hour < 8 ? ConvertHelper.GetDef_DateTime(DateTime.Now.ToString("yyyy/MM/dd 08:00")) : ConvertHelper.GetDef_DateTime(DateTime.Now.AddDays(1).ToString("yyyy/MM/dd 08:00"));
            //dateEditE.Properties.MinValue = ConvertHelper.GetDef_DateTime(DateTime.Now.AddDays(-6).ToString("yyyy/MM/dd 08:00"));
            //dateEditE.Properties.MaxValue = DateTime.Now.Hour < 8 ? ConvertHelper.GetDef_DateTime(DateTime.Now.ToString("yyyy/MM/dd 08:00")) : ConvertHelper.GetDef_DateTime(DateTime.Now.AddDays(1).ToString("yyyy/MM/dd 08:00"));

            //dateEditS.EditValue = ConvertHelper.GetDef_DateTime(DateTime.Now.AddDays(-6).ToString("yyyy/MM/dd 08:00"));
            //dateEditE.EditValue = ConvertHelper.GetDef_DateTime(DateTime.Now.ToString("yyyy/MM/dd 08:00"));
        }

        private void ShowHintt(string str)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { label1.Text = str; }));
            }
            else
            {
                label1.Text = str;
            }

        }

        private void ShowResult(ProductPoint product)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    for (int i = 0; i < product.datas.Count; i++)
                    {
                        dataGridView1.Rows[i].Cells[1].Value = product.datas[i].value.ToString();
                        if (product.datas[i].value < product.datas[i].lower_limit || product.datas[i].value > product.datas[i].upper_limit)
                        {
                            dataGridView1.Rows[i].Cells[1].Style.BackColor = Color.Red;
                        }
                        else
                        {
                            dataGridView1.Rows[i].Cells[1].Style.BackColor = Color.Lime;
                        }
                    }
                    if (product.pass)
                    {
                        label_result.Text = "PASS";
                        label_result.BackColor = Color.Lime;
                    }
                    else
                    {
                        label_result.Text = "FAIL";
                        label_result.BackColor = Color.Red;
                    }
                    dataGridView1.ClearSelection();
                }));
            }
        }

        private void ShowtxtSN(string sn)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { txtSN.Text = sn; }));
            }
            else
            {
                txtSN.Text = sn;
            }
        }

        private void ShowLog(string log)
        {
            try
            {
                string message = DateTime.Now.ToString("HH:mm:ss  ") + log;
                if (InvokeRequired)
                {

                    this.Invoke(new Action(() =>
                    {
                        if (listBox_log.Items.Count > 43)
                        {
                            listBox_log.Items.Clear();
                        }
                        listBox_log.Items.Add(message);
                    }));
                }
                else
                {
                    if (listBox_log.Items.Count > 43)
                    {
                        listBox_log.Items.Clear();
                    }
                    listBox_log.Items.Add(message);
                }
            }
            catch (Exception errMessage)
            {
                //LogSave.LogSaveInstance.ErrorQueue.Enqueue(errMessage.ToString());
            }
        }

        private void labPath_Click(object sender, EventArgs e)
        {
            Commons.OpenFolderAndSelectFile(labPath.Text);
        }

        private void labSensor_Click(object sender, EventArgs e)
        {
            if (pMachine.m_LoginUser == MotionBase.Sys_Define.enPasswordType.UnLogin)
            {
                //没登录直接退出
                return;
            }

            SensorEvent();
        }

        private void HomeForm_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                try
                {
                    m_HandleWait.ShowWait();
                    //   txtop.Text = MESDataDefine.MESDatas.StrOp;
                }
                catch (Exception err)
                {
                    MsgBoxHelper.DxMsgShowErr(err.Message);
                }
                finally
                {
                    m_HandleWait.CloseWait();
                }
            }
        }


        private void labLeft_Click(object sender, EventArgs e)
        {
            if (m_CurIndex > 0)
            {
                m_CurIndex--;
            }
            ShowIndex(m_CurIndex);
        }

        private void labRight_Click(object sender, EventArgs e)
        {
            if (m_CurIndex < 6)
            {
                m_CurIndex++;
            }
            ShowIndex(m_CurIndex);
        }

        private void lab0_Click(object sender, EventArgs e)
        {
            string strTemp = (((LabelControl)sender).Name.Substring(3)).ToString();
            switch (strTemp)
            {
                case "0":
                    m_CurIndex = 0;
                    break;
                case "1":
                    m_CurIndex = 1;
                    break;
                case "2":
                    m_CurIndex = 2;
                    break;
                case "3":
                    m_CurIndex = 3;
                    break;
                case "4":
                    m_CurIndex = 4;
                    break;
                case "5":
                    m_CurIndex = 5;
                    break;
                case "6":
                    m_CurIndex = 6;
                    break;
                default:
                    break;
            }

            ShowIndex(m_CurIndex);
        }

        private void chartError_DoubleClick(object sender, EventArgs e)
        {
            if ((int)pMachine.m_LoginUser < 1)
            {
                //权限不够直接退出
                return;
            }
            //Error pError = new Error(ref pMachine.m_NowAddress);
            //pError.AddErrSloution("屏蔽报警 ", 0);//Retry，再試一次
            //pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);

            //跳转到ERR TAB
            PostMessage(pfrmMain.Handle, frm_Main.WM_ChangeToAlarm, 0, 0);
        }

        private void panelIO_DoubleClick(object sender, EventArgs e)
        {
            if ((int)pMachine.m_LoginUser < 1)
            {
                //权限不够直接退出
                return;
            }
            //跳转到DATA TAB
            PostMessage(pfrmMain.Handle, frm_Main.WM_ChangeToData, 0, 0);
        }

        private void hScrollBar_ValueChanged(object sender, EventArgs e)
        {
            int val = hScrollBar.Value;
            m_CurIndex = val;
            //ShowIndex(val);
            SearchErrorTop(DateTime.Now.Date.AddDays(val - 6), true);
        }

        private void dateEditS_EditValueChanged(object sender, EventArgs e)
        {
            if (!m_OnRefresh)
            {
                SearchErrorTop(ConvertHelper.GetDef_DateTime(dateEditS.EditValue), false);
            }
        }
        private void btnMachineLog_Click(object sender, EventArgs e)
        {
            string dirName = @"D:\DATA\Machine Log";
            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }
            Commons.OpenFolder(dirName);
        }

        private void btnHiveLog_Click(object sender, EventArgs e)
        {
            string dirName = @"D:\DATA\HIVE Log";
            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }
            Commons.OpenFolder(dirName);
        }

        private void labPDCA_DoubleClick(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
            }
        }


        private void txtUC_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //DriverEnter._driver1SN = txtUC.Text.Trim();
                if (txtUC.Text.Length != MachineDataDefine.MachineCfgS.UCLength)
                {
                    MessageBox.Show(JudgeLanguage.JudgeLag("driver1SN码长度有误，请检查英文输入"));
                    txtUC.Clear();
                    txtUC.Focus();
                }

                else
                {
                    //frm_Main.workFlow._UC = txtUC.Text.Trim();
                    Thread.Sleep(200);
                    txtUC.Focus();
                    txtUC.SelectAll();
                    Mainflow.UC=txtUC.Text;
                    //frm_Main.workFlow.Start();
                }
            }
        }

        private void txtSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //DriverEnter._driver2SN = txt_driver2SN_KeyDown.Text.Trim();
                if (txtSN.Text.Length != MachineDataDefine.MachineCfgS.SNLength)
                {
                    MessageBox.Show(JudgeLanguage.JudgeLag("driver1SN码长度有误，请检查英文输入"));
                    txtSN.Clear();
                    txtSN.Focus();
                }

                else
                {
                    //frm_Main.workFlow._SN = txtSN.Text.Trim();
                    Thread.Sleep(200);
                    txtSN.Focus();
                    //txtSN.SelectAll();
                    Mainflow.SN=txtSN.Text;
                    txtSN.Enabled=false;
                    //frm_Main.workFlow.Startsn();

                }
            }
        }
        private void txtSN11()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { 
                    txtSN.Clear();
                    if (!MachineDataDefine.machineState.CheckUCgetSN)
                    {
                        txtSN.Enabled = true;
                        txtSN.Focus();
                    }
                }));
            }
            else
            {
                txtSN.Clear();
                if (!MachineDataDefine.machineState.CheckUCgetSN)
                {
                    txtSN.Enabled = true;
                    txtSN.Focus();
                }
            }
            if (InvokeRequired)
            {
                Invoke(new Action(() => {
                    txtUC.Clear();
                    if (MachineDataDefine.machineState.CheckUCgetSN)
                    {
                        txtUC.Enabled = true;
                        txtUC.Focus();
                    }
                }));
            }
            else
            {
                txtUC.Clear();
                if (MachineDataDefine.machineState.CheckUCgetSN)
                {
                    txtUC.Enabled = true;
                    txtUC.Focus();
                }
                
            }
        }

        private void checkUcgetSN_CheckedChanged(object sender, EventArgs e)
        {
            MachineDataDefine.machineState.CheckUCgetSN= checkUcgetSN.Checked;
            MachineDataDefine.machineState.WriteParams(MachineDataDefine.machineState);
            if (checkUcgetSN.Checked)
            {
                groupBox5.Visible = true;
                txtSN.Enabled = false;
            }
            else
            {
                groupBox5.Visible = false;
                txtSN.Enabled = true;
            }
        }
    }
}