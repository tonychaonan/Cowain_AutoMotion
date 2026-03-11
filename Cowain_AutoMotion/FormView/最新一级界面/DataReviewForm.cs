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
using Chart;
using Cowain_AutoDispenser;
using System.IO;
using System.Threading;
using DevExpress.XtraCharts;
using DevExpress.Utils;
using System.Reflection;
using Cowain_AutoMotion;

namespace Cowain_Form.FormView
{
    public partial class DataReviewForm : DevExpress.XtraEditors.XtraForm
    {
        public DataReviewForm()
        {
            InitializeComponent();

        }

        #region 自定义变量
        /// <summary>
        /// 运行状态单天数据
        /// </summary>
        DataTable dtshow = new DataTable();
        /// <summary>
        /// 运行状态累计白班数据
        /// </summary>
        DataTable m_SumDayTable = new DataTable();
        /// <summary>
        /// 运行状态累计夜班数据
        /// </summary>
        DataTable m_SumNightTable = new DataTable();
        /// <summary>
        /// 界面刷新线程
        /// </summary>
        Thread ReFlash_Thread;
        /// <summary>
        /// 查询等待提示窗
        /// </summary>
        private HandleWait m_HandleWait = new HandleWait();



        /// <summary>
        ///  折线图和柱状图运行状态单天数据
        /// </summary>
        DataTable dtShower_Diagram = new DataTable();

        /// <summary>
        /// chartControl_dayDiagram当前选择的Index,默认为当天
        /// </summary>
        private int m_CurIndex = 6;
        
        /// <summary>
        /// chartControl_lineDiagram当前选择的Index,默认为当天
        /// </summary>
        private int mCurDayIndex = 6;

        /// <summary>
        /// 白夜切换变量
        /// </summary>
        bool isDay = false;

        /// <summary>
        /// 白班12小时的产能
        /// </summary>
        private DataTable day_detialYield = new DataTable();

        /// <summary>
        /// 夜班12小时的产能
        /// </summary>
        private DataTable night_detialYield = new DataTable();

        /// <summary>
        ///  读取生产记录txt文档的锁对象
        /// </summary>
        public object locker1 = new object();

        /// <summary>
        /// 记录最新一次刷新的时间，当到达第二天会重新刷新
        /// </summary>
        DateTime reflushTime;

        /// <summary>
        /// 判断是否重新刷新UI
        /// </summary>
        bool isReflush = false;

        /// <summary>
        /// 刷新UI的次数，卡条件的
        /// </summary>
        int reflushNum = 0;
        /// <summary>
        /// 平均CT正在刷新中
        /// </summary>
        private bool m_Refresh = false;
        /// <summary>
        /// 产量统计正在刷新中
        /// </summary>
        private bool m_RefreshDay = false;

        /// <summary>
        /// 天小时切换变量
        /// </summary>
        bool m_StatisticsByDay = true;

        public bool StatisticsByDay
        {
            get
            {
                return !ConvertHelper.GetDef_Bool(SwitchDayHour.EditValue, false);
            }
            set
            {
                m_StatisticsByDay = value;
            }
        }
        #endregion

        #region 自定义方法
        private void GetDayData()
        {
            //gridControlDay.DataSource = null;
            DataTable dayTable = new DataTable();
            dayTable.Columns.Add("TimeLine", Type.GetType("System.String"));
            dayTable.Columns.Add("Time Quantum", Type.GetType("System.String"));
            dayTable.Columns.Add("Capacity", Type.GetType("System.Int32"));
            dayTable.Columns.Add("OK", Type.GetType("System.Int32"));
            dayTable.Columns.Add("NG", Type.GetType("System.Int32"));

            dayTable.Rows.Add(new object[] { "Day","8:00-9:00", 0, 0, 0 });
            dayTable.Rows.Add(new object[] { "Day", "9:00-10:00", 0, 0, 0 });
            dayTable.Rows.Add(new object[] { "Day", "10:00-11:00", 0, 0, 0 });
            dayTable.Rows.Add(new object[] { "Day", "11:00-12:00", 0, 0, 0 });
            dayTable.Rows.Add(new object[] { "Day", "12:00-13:00", 0, 0, 0 });
            dayTable.Rows.Add(new object[] { "Day", "13:00-14:00", 0, 0, 0 });
            dayTable.Rows.Add(new object[] { "Day", "14:00-15:00", 0, 0, 0 });
            dayTable.Rows.Add(new object[] { "Day", "15:00-16:00", 0, 0, 0 });
            dayTable.Rows.Add(new object[] { "Day", "16:00-17:00", 0, 0, 0 });
            dayTable.Rows.Add(new object[] { "Day", "17:00-18:00", 0, 0, 0 });
            dayTable.Rows.Add(new object[] { "Day", "18:00-19:00", 0, 0, 0 });
            dayTable.Rows.Add(new object[] { "Day", "19:00-20:00", 0, 0, 0 });
            dayTable.Rows.Add(new object[] { "Day", "合计", 0, 0, 0 });
            //gridControlDay.DataSource = dayTable;
        }

        private void GetNightData()
        {
            //gridControlNight.DataSource = null;
            DataTable nightTable = new DataTable();
            nightTable.Columns.Add("TimeLine", Type.GetType("System.String"));
            nightTable.Columns.Add("Time Quantum", Type.GetType("System.String"));
            nightTable.Columns.Add("Capacity", Type.GetType("System.Int32"));
            nightTable.Columns.Add("OK", Type.GetType("System.Int32"));
            nightTable.Columns.Add("NG", Type.GetType("System.Int32"));

            nightTable.Rows.Add(new object[] { "Night", "20:00-21:00", 0, 0, 0 });
            nightTable.Rows.Add(new object[] { "Night", "21:00-22:00", 0, 0, 0 });
            nightTable.Rows.Add(new object[] { "Night", "22:00-23:00", 0, 0, 0 });
            nightTable.Rows.Add(new object[] { "Night", "23:00-0:00", 0, 0, 0 });
            nightTable.Rows.Add(new object[] { "Night", "0:00-1:00", 0, 0, 0 });
            nightTable.Rows.Add(new object[] { "Night", "1:00-2:00", 0, 0, 0 });
            nightTable.Rows.Add(new object[] { "Night", "2:00-3:00", 0, 0, 0 });
            nightTable.Rows.Add(new object[] { "Night", "3:00-4:00", 0, 0, 0 });
            nightTable.Rows.Add(new object[] { "Night", "4:00-5:00", 0, 0, 0 });
            nightTable.Rows.Add(new object[] { "Night", "5:00-6:00", 0, 0, 0 });
            nightTable.Rows.Add(new object[] { "Night", "6:00-7:00", 0, 0, 0 });
            nightTable.Rows.Add(new object[] { "Night", "7:00-8:00", 0, 0, 0 });
            nightTable.Rows.Add(new object[] { "Night", "合计", 0, 0, 0 });
            //gridControlNight.DataSource = nightTable;
        }

        private void InitTable()
        {
            m_SumDayTable.Columns.Add("TimeLine", Type.GetType("System.String"));
            m_SumDayTable.Columns.Add("TimeQuantum", Type.GetType("System.String"));
            m_SumDayTable.Columns.Add("Capacity", Type.GetType("System.Int32"));
            m_SumDayTable.Columns.Add("OK", Type.GetType("System.Int32"));
            m_SumDayTable.Columns.Add("NG", Type.GetType("System.Int32"));

            m_SumNightTable.Columns.Add("TimeLine", Type.GetType("System.String"));
            m_SumNightTable.Columns.Add("TimeQuantum", Type.GetType("System.String"));
            m_SumNightTable.Columns.Add("Capacity", Type.GetType("System.Int32"));
            m_SumNightTable.Columns.Add("OK", Type.GetType("System.Int32"));
            m_SumNightTable.Columns.Add("NG", Type.GetType("System.Int32"));
        }

        private void CollectData(DataTable table)
        {
            if (table.Columns.Count < 24)
                return;

            for (int j = 0; j < 24; j++)
            {
                string name = table.Columns[j].ToString();
                int okleft = ConvertHelper.GetDef_Int(table.Rows[0][j].ToString());  
                int okright = ConvertHelper.GetDef_Int(table.Rows[1][j].ToString());
                int dtleft = ConvertHelper.GetDef_Int(table.Rows[2][j].ToString());
                int dtright = ConvertHelper.GetDef_Int(table.Rows[3][j].ToString());

                int all = okleft + okright + dtleft + dtright;
                int ok = okleft + okright;
                int dt = dtleft + dtright;

                if (j < 12)
                {
                    //白班数据
                    DataRow[] rows = m_SumDayTable.Select(string.Format("TimeQuantum = '{0}'", name));
                    if (rows.Length > 0)
                    {
                        rows[0][2] = ConvertHelper.GetDef_Int(rows[0][2]) + all;
                        rows[0][3] = ConvertHelper.GetDef_Int(rows[0][3]) + ok;
                        rows[0][4] = ConvertHelper.GetDef_Int(rows[0][4]) + dt;
                    }
                    else
                    {
                        DataRow newrow = m_SumDayTable.NewRow();
                        newrow[0] = "Day";
                        newrow[1] = name;
                        newrow[2] = all;
                        newrow[3] = ok;
                        newrow[4] = dt;

                        m_SumDayTable.Rows.Add(newrow);
                    }
                }
                else
                {
                    //夜班数据
                    DataRow[] rows = m_SumNightTable.Select(string.Format("TimeQuantum = '{0}'", name));
                    if (rows.Length > 0)
                    {
                        rows[0][2] = ConvertHelper.GetDef_Int(rows[0][2]) + all;
                        rows[0][3] = ConvertHelper.GetDef_Int(rows[0][3]) + ok;
                        rows[0][4] = ConvertHelper.GetDef_Int(rows[0][4]) + dt;
                    }
                    else
                    {
                        DataRow newrow = m_SumNightTable.NewRow();
                        newrow[0] = "Night";
                        newrow[1] = name;
                        newrow[2] = all;
                        newrow[3] = ok;
                        newrow[4] = dt;

                        m_SumNightTable.Rows.Add(newrow);
                    }
                }
            }
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
                    }

                    Thread.Sleep(1000);
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

            #region Input Summary
            DataTable yieTable = frm_Main.formData.Chartcapacity1.GetDayYield(DateTime.Now);
            DataTable ngTable = frm_Main.formData.Chartcapacity1.GetDayNG(DateTime.Now);
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

                    //int okleft = Convert.ToInt32(yieTable.Rows[0][hour - 8].ToString());
                    //int okright = Convert.ToInt32(yieTable.Rows[1][hour - 8].ToString());
                    //int dtleft = Convert.ToInt32(yieTable.Rows[2][hour - 8].ToString());
                    //int dtright = Convert.ToInt32(yieTable.Rows[3][hour - 8].ToString());
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

                    oktotal = (okleft + okright);
                    dttotal = (dtleft + dtright);
                    total = (okleft + okright + dtleft + dtright);
                }
                #endregion

                //labInput.Text = total.ToString() + "/" + total.ToString();
                //labYield.Text = total == 0?"0":(((total - dttotal) * 1.0 / total * 100).ToString("f2") + "%");
                //labPass.Text = oktotal.ToString() + "/" + dttotal.ToString();
                labInput.Text = total.ToString() + "/" + total.ToString();
                labYield.Text = ((total - dttotal) * 1.0 / total * 100).ToString("f2") + "%";
                labPass.Text = oktotal.ToString() + "/" + dttotal.ToString();
                labUPH.Text = total.ToString();

            }
            if(ngTable.Rows.Count>=0)
            {
                labelControl8.Text = ngTable.Rows.Count.ToString();
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
                        if (temp.Length>7)
                        {
                            ct.Add(Convert.ToDouble(temp[7]));
                        }
                        else
                        {
                            ct.Add(Convert.ToDouble(temp[5]));
                        }
                       
                    }

                }
                if (ct.Count > 0)
                {
                    //labUPH.Text = (ct.Average() == 0 ? 0 : 3600 / ct.Average()).ToString("f0");
                  //  labUPH.Text = ct.Count.ToString();
                    labCT.Text = ct.Average().ToString("f1");
                }
                
                //增加时间
                //date.Add(prj);
                //numbers.Add(ct.Count());
                //avreagect.Add(ct.Average() / gantryCount);
                //datelist.Add(new CalculationDate() { Date = prj, CT = (ct.Average() / gantryCount).ToString("0.0"), Count = ct.Count() });
            }

            #endregion

            #endregion

            #region  gridView数据显示程序
            //gridControl_productRecord.DataSource = null;
            DateTime selTime = DateTime.Now;
            if (!isReflush)
            {
                reflushTime = selTime;
                isReflush = true;
                reflushNum = 0;
            }
            if (isReflush && selTime.Day - reflushTime.Day > 0)
            {
                isReflush = false;
            }
            string msg = string.Empty;
            for (int i = 0; i < 7; i++)
            {
                DateTime curTime = selTime.AddDays(-i);
                msg += frm_Main.formData.CTUnit1.ReadCT(curTime.ToString("yyyy_MM_dd"));
            }
            //msg = readError(selTime.ToString("yyyy_MM_dd"));
            string[] lines = msg.Split('\n');

            DataTable dt = null;
            List<RecordInformation> oneRowInfor = new List<RecordInformation>();
            foreach (var item in lines)
            {
                if (item.Length > 2)
                {
                    RecordInformation infor = new RecordInformation();
                    infor.Date = item.Split(',')[0];
                    infor.Unit_SN = item.Split(',')[2];
                    infor.Component_SN = "";
                    infor.Start_Time = item.Split(',')[3];
                    infor.End_Time = item.Split(',')[4];
                    infor.CT = item.Split(',')[5];
                    //后面需改成实际值
                    infor.Pass = true;
                    infor.HiveState = "1";
                    oneRowInfor.Add(infor);
                }
            }

            int height = tableLayoutPanel_graphPanel.Height;
            int width = tableLayoutPanel_graphPanel.Width;
            int y = tableLayoutPanel_graphPanel.PointToClient(MousePosition).Y;
            int x = tableLayoutPanel_graphPanel.PointToClient(MousePosition).X;
            if (y > 2 * height / 3 && y < height && x > 0 && x < width && reflushNum != 0)
            {

            }
            else
            {
                dt = ListToTable(oneRowInfor);
                //ClearAllRows();
                gridControl_productRecord.DataSource = dt;
                //this.gridView.PopulateColumns();
                reflushNum = 1;
            }


            #endregion

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
        #endregion



        private void DataReviewForm_Load(object sender, EventArgs e)
        {
            m_HandleWait.Init(this);
            //InitTable();

            dateEditS.Properties.MinValue = ConvertHelper.GetDef_DateTime(DateTime.Now.AddDays(-6).ToString("yyyy/MM/dd 08:00"));
            dateEditS.Properties.MaxValue = DateTime.Now.Hour < 8 ? ConvertHelper.GetDef_DateTime(DateTime.Now.ToString("yyyy/MM/dd 08:00")) : ConvertHelper.GetDef_DateTime(DateTime.Now.AddDays(1).ToString("yyyy/MM/dd 08:00"));
            dateEditE.Properties.MinValue = ConvertHelper.GetDef_DateTime(DateTime.Now.AddDays(-6).ToString("yyyy/MM/dd 08:00"));
            dateEditE.Properties.MaxValue = DateTime.Now.Hour < 8 ? ConvertHelper.GetDef_DateTime(DateTime.Now.ToString("yyyy/MM/dd 08:00")) : ConvertHelper.GetDef_DateTime(DateTime.Now.AddDays(1).ToString("yyyy/MM/dd 08:00"));

            dateEditDayS.Properties.MinValue = ConvertHelper.GetDef_DateTime(DateTime.Now.AddDays(-6).ToString("yyyy/MM/dd 08:00"));
            dateEditDayS.Properties.MaxValue = DateTime.Now.Hour < 8 ? ConvertHelper.GetDef_DateTime(DateTime.Now.ToString("yyyy/MM/dd 08:00")) : ConvertHelper.GetDef_DateTime(DateTime.Now.AddDays(1).ToString("yyyy/MM/dd 08:00"));
            dateEditDayE.Properties.MinValue = ConvertHelper.GetDef_DateTime(DateTime.Now.AddDays(-6).ToString("yyyy/MM/dd 08:00"));
            dateEditDayE.Properties.MaxValue = DateTime.Now.Hour < 8 ? ConvertHelper.GetDef_DateTime(DateTime.Now.ToString("yyyy/MM/dd 08:00")) : ConvertHelper.GetDef_DateTime(DateTime.Now.AddDays(1).ToString("yyyy/MM/dd 08:00"));

            //加载和显示label按钮状态
            hScrollBar.Value = 6;//当天数据
            
            //判断是否是白天还是晚上
            if (DateTime.Now.Hour>=8 && DateTime.Now.Hour<20)
            {
                isDay = true;
            }
            if(isDay)
            {
                hScrollBarHour.Value = 12;//当天白班数据
            }
            else
            {
                hScrollBarHour.Value = 13;//当天夜班数据
            }            

            ReFlash_Thread = new Thread(DoReflash);
            ReFlash_Thread.Priority = ThreadPriority.Lowest;
            ReFlash_Thread.IsBackground = true;
            ReFlash_Thread.Start();

        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.InitialDirectory = @"D:\";//打开初始目录
            saveFile.Title = "选择保存文件";
            saveFile.Filter = "csv files (*.csv)|*.csv|All files (*.*)|";//过滤条件
            saveFile.FilterIndex = 1;//获取第二个过滤条件开始的文件拓展名
            saveFile.FileName = "";//默认保存名称
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                DataTable sumTable = m_SumDayTable.Clone();
                //数据合并
                sumTable.Merge(m_SumDayTable);
                sumTable.Merge(m_SumNightTable);
                CSVFileHelper.SaveCSV(sumTable, saveFile.FileName);
            }
        }                
        
        /// <summary>
        /// 柱状图选择某天按钮click事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                case "7":
                    m_CurIndex = 7;
                    break;
                default:
                    break;
            }
            
            ShowIndex(m_CurIndex);

        }

        /// <summary>
        /// 柱状图左行按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLeft_Click(object sender, EventArgs e)
        {
            if (m_CurIndex <= 0)
            {
                return;
            }
            if (m_CurIndex > 0)
            {
                m_CurIndex--;
            }
            ShowIndex(m_CurIndex);
        }

        /// <summary>
        /// 柱状图右行按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRight_Click(object sender, EventArgs e)
        {
            if (m_CurIndex>=6)
            {
                return;
            }
            if (m_CurIndex < 6)
            {
                m_CurIndex++;
            }
            ShowIndex(m_CurIndex);
        }

        /// <summary>
        /// 显示柱状图label状态的方法
        /// </summary>
        /// <param name="index"></param>
        private void ShowIndex(int index,bool refresh = true)
        {
            m_RefreshDay = true;
            DateTime m_CurrentTime = DateTime.Now;
            if (DateTime.Now.Hour >= 0 && DateTime.Now.Hour <= 7)
            {
                m_CurrentTime = DateTime.Now.AddDays(-1);
            }

            if (refresh)
            {
                if (StatisticsByDay)
                {
                    dateEditDayS.EditValue = string.Format("{0:yyyy/MM/dd 8:00}", m_CurrentTime.AddDays(-6));
                    dateEditDayE.EditValue = string.Format("{0:yyyy/MM/dd 8:00}", m_CurrentTime.AddDays(1));
                }
                else
                {
                    dateEditDayS.EditValue = string.Format("{0:yyyy/MM/dd 8:00}", m_CurrentTime.AddDays(m_CurIndex - 6));
                    dateEditDayE.EditValue = ConvertHelper.GetDef_DateTime(string.Format("{0:yyyy/MM/dd 8:00}", m_CurrentTime.AddDays(m_CurIndex - 6))).AddHours(12);
                }
            }
            

            if (StatisticsByDay)
            {
                ShowDailyCapacityToChart();
            }
            else
            {
                ShowDailyDetail();
                //ShowDailyDetailBac();
            }

            m_RefreshDay = false;
        }

        /// <summary>
        /// 显示某天的时间段明细数据
        /// </summary>
        private void ShowDailyDetail()
        {
            string outputPath = @"D:\DATA\CT";
            string fileName = string.Empty;
            string fullFileName = string.Empty;

            DateTime stime = ConvertHelper.GetDef_DateTime(dateEditDayS.EditValue);//开始时间
            DateTime etime = ConvertHelper.GetDef_DateTime(dateEditDayE.EditValue);//结束时间
            DateTime searchstime = stime;
            DateTime searchetime = ConvertHelper.GetDef_DateTime(string.Format("{0:yyyy-MM-dd HH:mm:00}",stime.AddHours(12)));

            if (searchstime.Hour >= 0 && searchstime.Hour <= 7)
            {
                searchstime = searchstime.AddDays(-1).Date;
            }
            else
            {
                searchstime = searchstime.Date;
            }
            if (searchetime.Hour >= 0 && searchetime.Hour <= 7)
            {
                searchetime = searchetime.AddDays(-1).Date;
            }
            else
            {
                searchetime = searchetime.Date;
            }

            #region 读取源数据
            string msg = string.Empty;
            for (int i = 0; i < (searchetime - searchstime).TotalDays+1; i++)
            {
                DateTime curTime = searchstime.AddDays(i);
                fileName = string.Format("{0}.txt", curTime.ToString("yyyy_MM_dd"));
                fullFileName = Path.Combine(outputPath, fileName);

                //是否存在日志文件
                if (File.Exists(fullFileName))
                {
                    msg += frm_Main.formData.CTUnit1.ReadCT(curTime.ToString("yyyy_MM_dd"));
                }
            }
            string[] lines = msg.Split('\n');

            DataTable dt = null;
            List<RecordInformation> oneRowInfor = new List<RecordInformation>();
            foreach (var item in lines)
            {
                string[] datas = item.Split(',');
                if (datas.Length >= 8)
                {
                    DateTime curdate = ConvertHelper.GetDef_DateTime(datas[0] + " " + datas[1]);
                    if (curdate < stime && curdate >= etime)
                    {
                        continue;
                    }

                    RecordInformation infor = new RecordInformation();
                    infor.Date = datas[0];
                    infor.Unit_SN = datas[2];
                    infor.Component_SN = "";
                    infor.Start_Time = datas[3];
                    infor.End_Time = datas[4];
                    infor.CT = datas[5];
                    //后面需改成实际值
                    infor.Pass = true;
                    infor.HiveState = "1";
                    
                    if (curdate.Hour >= 8 && curdate.Hour < 20)
                    {
                        infor.Shift = "Day";
                    }
                    else
                    {
                        infor.Shift = "Night";
                    }
                    oneRowInfor.Add(infor);
                }
            }

            dt = ListToTable(oneRowInfor);            
            #endregion

            #region 转换最终结果集
            DataTable resulttable = new DataTable();
            resulttable.Columns.Add("DateTime", Type.GetType("System.DateTime"));
            resulttable.Columns.Add("TimeStr", Type.GetType("System.String"));
            resulttable.Columns.Add("Num", Type.GetType("System.Int32"));
            resulttable.Columns.Add("Shift", Type.GetType("System.String"));

            for (int i = 0; i < (etime - stime).TotalHours; i++)
            {
                string timestr = string.Format("{0:HH:00}-{1:HH:00}", stime.AddHours(i), stime.AddHours(i+1)) + "\r\n" + string.Format("{0:yyyy/MM/dd}", stime.AddHours(i));
                DataRow row = resulttable.NewRow();
                row["DateTime"] = stime.AddHours(i);
                row["TimeStr"] = timestr;
                row["Num"] = 0;
                row["Shift"] = stime.AddHours(i).Hour >= 8 && stime.AddHours(i).Hour < 20 ? "D" : "N";
                resulttable.Rows.Add(row);
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DateTime curdate = ConvertHelper.GetDef_DateTime(dt.Rows[i]["Date"] + " " + dt.Rows[i]["End_Time"]);
                //string timestr = string.Format("{0}\r\n{1}",string.Format("{0:HH:00}-{1:HH:00}", curdate, curdate.AddHours(1)),string.Format("{0:yyyy/MM/dd}", curdate));
                string timestr = string.Format("{0:HH:00}-{1:HH:00}", curdate, curdate.AddHours(1)) + "\r\n" + string.Format("{0:yyyy/MM/dd}", curdate);

                DataRow[] rows = resulttable.Select(string.Format("TimeStr = '{0}'",timestr));
                if (rows.Length > 0)
                {
                    rows[0]["Num"] = ConvertHelper.GetDef_Int(rows[0]["Num"]) + 1;
                }
            }
            #endregion

            Series[] series = this.chartControl_dayDiagram.SeriesSerializable;
            for (int i = 0; i < series.Length; i++)
            {
                SeriesPointCollection colls = series[i].Points;
                colls.Clear();
            }

            if (series.Length < 2)
            {
                return;
            }

            for (int i = 0; i < resulttable.Rows.Count; i++)
            {
                string name = ConvertHelper.GetDef_Str(resulttable.Rows[i]["TimeStr"]);
                int num = ConvertHelper.GetDef_Int(resulttable.Rows[i]["Num"]);

                
                if (resulttable.Rows[i]["Shift"].ToString() == "D")
                {
                    //白班
                    SeriesPoint seriesPointD = new SeriesPoint(name, new object[] { ((object)(num)) });
                    seriesPointD.ColorSerializable = "#0080FF";
                    series[0].Points.AddRange(new DevExpress.XtraCharts.SeriesPoint[] { seriesPointD });
                    //夜班
                    SeriesPoint seriesPointN = new SeriesPoint(name, new object[] { ((object)(0)) });
                    seriesPointN.ColorSerializable = "Black";
                    series[1].Points.AddRange(new DevExpress.XtraCharts.SeriesPoint[] { seriesPointN });
                }
                else
                {
                    //白班
                    SeriesPoint seriesPointD = new SeriesPoint(name, new object[] { ((object)(0)) });
                    seriesPointD.ColorSerializable = "#0080FF";
                    series[0].Points.AddRange(new DevExpress.XtraCharts.SeriesPoint[] { seriesPointD });
                    //夜班
                    SeriesPoint seriesPointN = new SeriesPoint(name, new object[] { ((object)(num)) });
                    seriesPointN.ColorSerializable = "Black";
                    series[1].Points.AddRange(new DevExpress.XtraCharts.SeriesPoint[] { seriesPointN });
                }
            }
            
        }

        private void ShowDailyDetailBac()
        {
            string outputPath = @"D:\DATA\产量统计";
            string fileName = string.Empty;
            string fullFileName = string.Empty;

            DataTable dayTable = null;

            DateTime searchTime = DateTime.Now;

            if (searchTime.Hour >= 0 && searchTime.Hour <= 7)
            {
                searchTime = searchTime.AddDays(-1);
            }

            searchTime = searchTime.AddDays(m_CurIndex - 6);

            fileName = string.Format("{0}.csv", searchTime.ToString("yyyy_MM_dd"));
            fullFileName = Path.Combine(outputPath, fileName);

            //是否存在日志文件
            if (File.Exists(fullFileName))
            {
                dayTable = CSVFileHelper.OpenCSV(fullFileName);
            }

            Series[] series = this.chartControl_dayDiagram.SeriesSerializable;
            if (dayTable == null)
            {
                for (int i = 0; i < series.Length; i++)
                {
                    SeriesPointCollection colls = series[i].Points;
                    colls.Clear();
                }
                return;
            }


            for (int i = 0; i < series.Length; i++)
            {
                SeriesPointCollection colls = series[i].Points;
                colls.Clear();
                if (i == 0)
                {
                    if (dayTable.Rows.Count < 4)
                    {
                        return;
                    }
                    int num = 0;
                    string name = string.Empty;
                    for (int j = 0; j < dayTable.Columns.Count; j++)
                    {
                        num = ConvertHelper.GetDef_Int(dayTable.Rows[0][j]) + ConvertHelper.GetDef_Int(dayTable.Rows[1][j]);//第一行左工位+第二行右工位                        
                        switch (j)
                        {
                            case 0:
                                name = "D08-09\r\nN20-21";
                                break;
                            case 1:
                                name = "D09-10\r\nN21-22";
                                break;
                            case 2:
                                name = "D10-11\r\nN22-23";
                                break;
                            case 3:
                                name = "D11-12\r\nN23-00";
                                break;
                            case 4:
                                name = "D12-13\r\nN00-01";
                                break;
                            case 5:
                                name = "D13-14\r\nN01-02";
                                break;
                            case 6:
                                name = "D14-15\r\nN02-03";
                                break;
                            case 7:
                                name = "D15-16\r\nN03-04";
                                break;
                            case 8:
                                name = "D16-17\r\nN04-05";
                                break;
                            case 9:
                                name = "D17-18\r\nN05-06";
                                break;
                            case 10:
                                name = "D18-19\r\nN06-07";
                                break;
                            case 11:
                                name = "D19-20\r\nN07-08";
                                break;
                            default:
                                break;
                        }
                        if (j < 12)
                        {
                            //白班数据
                            SeriesPoint seriesPoint = new SeriesPoint(name, new object[] { ((object)(num)) });
                            seriesPoint.ColorSerializable = "#0080FF";
                            series[i].Points.AddRange(new DevExpress.XtraCharts.SeriesPoint[] { seriesPoint });
                        }
                    }
                }
                else if (i == 1)
                {
                    if (dayTable.Rows.Count < 4)
                    {
                        return;
                    }
                    int num = 0;
                    string name = string.Empty;
                    for (int j = 0; j < dayTable.Columns.Count; j++)
                    {
                        num = ConvertHelper.GetDef_Int(dayTable.Rows[0][j]) + ConvertHelper.GetDef_Int(dayTable.Rows[1][j]);//第一行左工位+第二行右工位                        
                        switch (j)
                        {
                            case 12:
                                name = "D08-09\r\nN20-21";
                                break;
                            case 13:
                                name = "D09-10\r\nN21-22";
                                break;
                            case 14:
                                name = "D10-11\r\nN22-23";
                                break;
                            case 15:
                                name = "D11-12\r\nN23-00";
                                break;
                            case 16:
                                name = "D12-13\r\nN00-01";
                                break;
                            case 17:
                                name = "D13-14\r\nN01-02";
                                break;
                            case 18:
                                name = "D14-15\r\nN02-03";
                                break;
                            case 19:
                                name = "D15-16\r\nN03-04";
                                break;
                            case 20:
                                name = "D16-17\r\nN04-05";
                                break;
                            case 21:
                                name = "D17-18\r\nN05-06";
                                break;
                            case 22:
                                name = "D18-19\r\nN06-07";
                                break;
                            case 23:
                                name = "D19-20\r\nN07-08";
                                break;
                            default:
                                break;
                        }
                        if (j > 11)
                        {
                            //夜班数据
                            SeriesPoint seriesPoint = new SeriesPoint(name, new object[] { ((object)(num)) });
                            seriesPoint.ColorSerializable = "Gray";
                            series[i].Points.AddRange(new DevExpress.XtraCharts.SeriesPoint[] { seriesPoint });
                        }
                    }
                }

            }
        }

        /// <summary>
        /// 显示每天总产能到chartControl_dayDiagram
        /// </summary>
        private void ShowDailyCapacityToChart()
        {
            string outputPath = @"D:\DATA\产量统计";
            string fileName = string.Empty;
            string fullFileName = string.Empty;

            DateTime stime = ConvertHelper.GetDef_DateTime(dateEditDayS.EditValue);
            DateTime etime = ConvertHelper.GetDef_DateTime(dateEditDayE.EditValue);
            Dictionary<string,int> dic_Day =new Dictionary<string, int>();
            Dictionary<string, int> dic_Night = new Dictionary<string, int>();
            string key;
            int dayCapcity = 0;
            int nightCapcaty = 0;
            

            for (int i = 0; i < 7; i++)
            {
                //key = "D" + (i + 1).ToString();
                DateTime searchTime = DateTime.Now;

                //if (isReadFile[i])
                //{
                    if (searchTime.Hour>=0 && searchTime.Hour<=7)
                    {
                        searchTime = searchTime.AddDays(-1);
                    }

                    searchTime = searchTime.AddDays(i-6);
                    key = searchTime.ToString("yyyy/MM/dd");

                if (searchTime < stime || searchTime >= etime)
                {
                    continue;
                }

                    fileName = string.Format("{0}.csv", searchTime.ToString("yyyy_MM_dd"));
                    fullFileName = Path.Combine(outputPath, fileName);

                    //是否存在日志文件
                    if (File.Exists(fullFileName))
                    {
                        dtShower_Diagram = CSVFileHelper.OpenCSV(fullFileName);
                        dayCapcity = GetDayShiftData(dtShower_Diagram, 12);
                        nightCapcaty = GetDayShiftData(dtShower_Diagram, 24);
                        dic_Day.Add(key, dayCapcity);
                        dic_Night.Add(key, nightCapcaty);
                    }
                    else
                    {
                        dic_Day.Add(key, 0);
                        dic_Night.Add(key, 0);
                    }
                //}
                //else
                //{
                //    dic_Day.Add(key, 0);
                //    dic_Night.Add(key, 0);
                //}

            }

            //字段转dataTable
            DataTable dt_Day = DictionaryOfObjectsToDataTable(dic_Day, "日期","产能");
            DataTable dt_Niht = DictionaryOfObjectsToDataTable(dic_Night, "日期", "产能");

            ShowStateByDay(dt_Day, dt_Niht);
        }

        /// <summary>
        /// 获得白或者夜班的总产能
        /// </summary>
        /// <param name="table"></param>
        /// <param name="hourIndex"></param>
        /// <returns></returns>
        private  int GetDayShiftData(DataTable table,int hourIndex)
        {
            int capcityNum = 0;
            if (table.Columns.Count < 24)

                return capcityNum;
            for (int j = hourIndex-12; j < hourIndex; j++)
            {                   
                int okleft = ConvertHelper.GetDef_Int(table.Rows[0][j].ToString());
                int okright = ConvertHelper.GetDef_Int(table.Rows[1][j].ToString());
                capcityNum = capcityNum + okleft + okright;
            }
            return capcityNum;
        }

        /// <summary>
        /// 字段转成数据表结构
        /// </summary>
        /// <param name="input"></param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public DataTable DictionaryOfObjectsToDataTable(Dictionary<string, int> input, string keyName, string valueName)
        {
            try
            {
                DataTable output = new DataTable();

                DataColumn key = new DataColumn(keyName);
                output.Columns.Add(key);

                DataColumn value = new DataColumn(valueName);
                output.Columns.Add(value);

                foreach (KeyValuePair<string, int> kvp in input)
                {
                    DataRow dr = output.NewRow();
                    dr[keyName] = kvp.Key;
                    dr[valueName] = kvp.Value;
                    //foreach (PropertyInfo pi in kvp.Value.GetType().GetProperties())
                    //{
                    //    if (!output.Columns.Contains(pi.Name)) output.Columns.Add(pi.Name);
                    //    dr[pi.Name] = pi.GetValue(kvp.Value, null);
                    //}
                    output.Rows.Add(dr);
                }

                return output;
            }
            catch (Exception ex)
            {
                MsgBoxHelper.DxMsgShowErr("字典转dataTable出错！" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 显示运行状态(天)
        /// </summary>
        /// <param name="datasource"></param>
        private void ShowStateByDay(DataTable datasource1 , DataTable datasource2)
        {
            Series[] series = this.chartControl_dayDiagram.SeriesSerializable;
            for (int i = 0; i < series.Length; i++)
            {
                SeriesPointCollection colls = series[i].Points;
                colls.Clear();
                if (i==0)
                {
                    for (int j = 0; j < datasource1.Rows.Count; j++)
                    {
                        string name = ConvertHelper.GetDef_Str(datasource1.Rows[j][0]);
                        int val = ConvertHelper.GetDef_Int(datasource1.Rows[j][1]);

                        SeriesPoint seriesPoint = new SeriesPoint(name, new object[] { ((object)(val)) });
                        seriesPoint.ColorSerializable = "#0080FF";
                        series[i].Points.AddRange(new DevExpress.XtraCharts.SeriesPoint[] { seriesPoint });
                    }
                }
                else if (i == 1)
                {
                    for (int j = 0; j < datasource1.Rows.Count; j++)
                    {
                        string name = ConvertHelper.GetDef_Str(datasource2.Rows[j][0]);
                        int val = ConvertHelper.GetDef_Int(datasource2.Rows[j][1]);

                        SeriesPoint seriesPoint = new SeriesPoint(name, new object[] { ((object)(val)) });
                        seriesPoint.ColorSerializable = "Gray";
                        series[i].Points.AddRange(new DevExpress.XtraCharts.SeriesPoint[] { seriesPoint });
                    }
                }           
            }
        }
        
        /// <summary>
        /// 白夜班转换显示
        /// </summary>
        private void DayShiftDataTableToChart(int currentDayIndex)
        {
            string outputPath = @"D:\DATA\产量统计";
            string fileName = string.Empty;
            string fullFileName = string.Empty;

            DateTime searchTime = DateTime.Now;
            if (searchTime.Hour >= 0 && searchTime.Hour <= 7)
            {
                searchTime = searchTime.AddDays(-1);
            }
            searchTime = searchTime.AddDays(currentDayIndex - 6);

            fileName = string.Format("{0}.csv", searchTime.ToString("yyyy_MM_dd"));
            fullFileName = Path.Combine(outputPath, fileName);

            //是否存在日志文件
            if (File.Exists(fullFileName))
            {
                dtShower_Diagram = CSVFileHelper.OpenCSV(fullFileName);
                GetDayOrNightYieldData(dtShower_Diagram);
            }
            else
            {
                GetNullYieldData();
            }

            //展示数据折线图
            if (isDay)
            {
                ShowLineChart(day_detialYield);
            }
            else
            {
                ShowLineChart(night_detialYield);
            }           
        }

        private void HourDataToChart1(int index)
        {
            string outputPath = @"D:\DATA\产量统计";
            string fileName = string.Empty;
            string fullFileName = string.Empty;

            DateTime searchTime = DateTime.Now;
            if (searchTime.Hour >= 0 && searchTime.Hour <= 7)
            {
                searchTime = searchTime.AddDays(-1);
            }
            int currentDayIndex = index / 2;
            bool day = index % 2 == 0 ? true : false;
            searchTime = searchTime.AddDays(currentDayIndex - 6);

            fileName = string.Format("{0}.csv", searchTime.ToString("yyyy_MM_dd"));
            fullFileName = Path.Combine(outputPath, fileName);

            //是否存在日志文件
            if (File.Exists(fullFileName))
            {
                dtShower_Diagram = CSVFileHelper.OpenCSV(fullFileName);
                GetDayOrNightYieldData(dtShower_Diagram);
            }
            else
            {
                GetNullYieldData();
            }

            DateTime m_CurrentTime = DateTime.Now;
            if (DateTime.Now.Hour >= 0 && DateTime.Now.Hour <= 7)
            {
                m_CurrentTime = DateTime.Now.AddDays(-1);
            }
            if (day)
            {
                //labTStime.Text = m_CurrentTime.AddDays(currentDayIndex - 6).ToString("d") + " 8:00:00";
                //labTEtime.Text = m_CurrentTime.AddDays(currentDayIndex - 6).ToString("d") + " 20:00:00";
            }
            else
            {
                //labTStime.Text = m_CurrentTime.AddDays(currentDayIndex - 6).ToString("d") + " 20:00:00";
                //labTEtime.Text = m_CurrentTime.AddDays(currentDayIndex - 6).ToString("d") + " 8:00:00";
            }

            //展示数据折线图
            if (day)
            {
                ShowLineChart(day_detialYield);
            }
            else
            {
                ShowLineChart(night_detialYield);
            }
        }
        
        private void HourDataToChart(int index,bool refresh = true)
        {
            m_Refresh = true;
            DateTime searchTime = DateTime.Now;            
            
            if (refresh)
            {
                if (StatisticsByDay)
                {
                    //dateEditDayS.EditValue = string.Format("{0:yyyy/MM/dd 8:00}", m_CurrentTime.AddDays(-6));
                    //dateEditDayE.EditValue = string.Format("{0:yyyy/MM/dd 8:00}", m_CurrentTime.AddDays(1));
                    dateEditS.EditValue = string.Format("{0:yyyy/MM/dd 8:00}", searchTime.AddDays(-6));
                    dateEditE.EditValue = string.Format("{0:yyyy/MM/dd 8:00}", searchTime.AddDays(1));
                }
                else
                {
                    if (searchTime.Hour >= 0 && searchTime.Hour <= 7)
                    {
                        searchTime = searchTime.AddDays(-1);
                    }
                    int currentDayIndex = index / 2;
                    bool day = index % 2 == 0 ? true : false;
                    searchTime = searchTime.AddDays(currentDayIndex - 6);
                    //dateEditDayS.EditValue = string.Format("{0:yyyy/MM/dd 8:00}", m_CurrentTime.AddDays(m_CurIndex - 6));
                    //dateEditDayE.EditValue = ConvertHelper.GetDef_DateTime(string.Format("{0:yyyy/MM/dd 8:00}", m_CurrentTime.AddDays(m_CurIndex - 6))).AddHours(12);
                    if (day)
                    {
                        dateEditS.EditValue = string.Format("{0:yyyy/MM/dd 8:00}", searchTime);
                        dateEditE.EditValue = string.Format("{0:yyyy/MM/dd 20:00}", searchTime);
                    }
                    else
                    {
                        dateEditS.EditValue = string.Format("{0:yyyy/MM/dd 20:00}", searchTime);
                        dateEditE.EditValue = string.Format("{0:yyyy/MM/dd 08:00}", searchTime.AddDays(1));
                    }
                }

                
            }

            if (StatisticsByDay)
            {
                //按天
                ShowCTDaily();
            }
            else
            {
                //按小时
                ShowCTDailyDetail();
            }
            m_Refresh = false;
        }

        private void ShowCTDailyDetailbac(DateTime searchTime,bool day)
        {
            DateTime stime = ConvertHelper.GetDef_DateTime(dateEditS.EditValue);
            DateTime etime = ConvertHelper.GetDef_DateTime(dateEditE.EditValue);

            string message = frm_Main.formData.CTUnit1.ReadCT(searchTime.ToString("yyyy_MM_dd"));
            if (!message.Trim().Equals(""))
            {
                //解析数据
                string[] sn = message.Split('\n');

                string[] temp;
                string dates;

                Dictionary<DateTime, List<double>> dic = new Dictionary<DateTime, List<double>>();
                for (int j = 0; j < sn.Length - 1; j++)
                {

                    dates = sn[j].Trim('\r');
                    temp = dates.Split(',');

                    DateTime time = ConvertHelper.GetDef_DateTime(temp[0] + " " + temp[1]);
                    time = ConvertHelper.GetDef_DateTime(string.Format("{0:yyyy-MM-dd HH:00:00}", time));
                    if (!dic.ContainsKey(time))
                    {
                        List<double> ct = new List<double>();
                        if (temp.Length > 7)
                        {
                            ct.Add(ConvertHelper.GetDef_Double(temp[7]));
                        }
                        else
                        {
                            ct.Add(ConvertHelper.GetDef_Double(temp[5]));
                        }

                        dic.Add(time, ct);
                    }
                    else
                    {
                        List<double> ct = dic[time];
                        if (temp.Length > 7)
                        {
                            ct.Add(ConvertHelper.GetDef_Double(temp[7]));
                        }
                        else
                        {
                            ct.Add(ConvertHelper.GetDef_Double(temp[5]));
                        }

                        dic[time] = ct;
                    }
                }

                //清除dataTable数据
                day_detialYield.Columns.Clear();
                night_detialYield.Columns.Clear();
                day_detialYield.Clear();
                night_detialYield.Clear();

                //添加列数据
                day_detialYield.Columns.Add("小时");
                day_detialYield.Columns.Add("产能");
                night_detialYield.Columns.Add("小时");
                night_detialYield.Columns.Add("产能");

                DataRow row;

                if (day)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        DateTime time = searchTime.Date.AddHours(i + 8);
                        if (dic.ContainsKey(time))
                        {
                            List<double> ct = dic[time];

                            row = day_detialYield.NewRow();
                            row[0] = string.Format("{0:HH:mm}", time) + "\r\n" + string.Format("{0:HH:mm}", time.AddHours(1));
                            row[1] = ct.Count > 0 ? ConvertHelper.GetDef_Dec(ct.Average(), 1) : 0;
                            day_detialYield.Rows.Add(row);
                        }
                        else
                        {
                            row = day_detialYield.NewRow();
                            row[0] = string.Format("{0:HH:mm}", time) + "\r\n" + string.Format("{0:HH:mm}", time.AddHours(1));
                            row[1] = 0;
                            day_detialYield.Rows.Add(row);
                        }
                    }


                    ShowLineChart(day_detialYield);
                }
                else
                {
                    for (int i = 0; i < 12; i++)
                    {
                        DateTime time = searchTime.Date.AddHours(i + 20);
                        if (dic.ContainsKey(time))
                        {
                            List<double> ct = dic[time];

                            row = night_detialYield.NewRow();
                            row[0] = string.Format("{0:HH:mm}", time) + "\r\n" + string.Format("{0:HH:mm}", time.AddHours(1));
                            row[1] = ct.Count > 0 ? ConvertHelper.GetDef_Dec(ct.Average(), 1) : 0;
                            night_detialYield.Rows.Add(row);
                        }
                        else
                        {
                            row = night_detialYield.NewRow();
                            row[0] = string.Format("{0:HH:mm}", time) + "\r\n" + string.Format("{0:HH:mm}", time.AddHours(1));
                            row[1] = 0;
                            night_detialYield.Rows.Add(row);
                        }
                    }

                    ShowLineChart(night_detialYield);
                }
            }
            else
            {
                #region 空数据显示
                //清除dataTable数据
                day_detialYield.Columns.Clear();
                night_detialYield.Columns.Clear();
                day_detialYield.Clear();
                night_detialYield.Clear();

                //添加列数据
                day_detialYield.Columns.Add("小时");
                day_detialYield.Columns.Add("产能");
                night_detialYield.Columns.Add("小时");
                night_detialYield.Columns.Add("产能");

                DataRow row;

                if (day)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        DateTime time = searchTime.Date.AddHours(i + 8);
                        row = day_detialYield.NewRow();
                        row[0] = string.Format("{0:HH:mm}", time) + "\r\n" + string.Format("{0:HH:mm}", time.AddHours(1));
                        row[1] = 0;
                        day_detialYield.Rows.Add(row);
                    }

                    ShowLineChart(day_detialYield);
                }
                else
                {
                    for (int i = 0; i < 12; i++)
                    {
                        DateTime time = searchTime.Date.AddHours(i + 20);
                        row = night_detialYield.NewRow();
                        row[0] = string.Format("{0:HH:mm}", time) + "\r\n" + string.Format("{0:HH:mm}", time.AddHours(1));
                        row[1] = 0;
                        night_detialYield.Rows.Add(row);
                    }

                    ShowLineChart(night_detialYield);
                }
                #endregion
            }
        }

        private void ShowCTDailyDetail()
        {
            string outputPath = @"D:\DATA\CT";
            string fileName = string.Empty;
            string fullFileName = string.Empty;

            DateTime stime = ConvertHelper.GetDef_DateTime(dateEditS.EditValue);//开始时间
            DateTime etime = ConvertHelper.GetDef_DateTime(dateEditE.EditValue);//结束时间
            DateTime searchstime = stime;
            DateTime searchetime = ConvertHelper.GetDef_DateTime(string.Format("{0:yyyy-MM-dd HH:mm:00}", stime.AddHours(12)));

            if (searchstime.Hour >= 0 && searchstime.Hour <= 7)
            {
                searchstime = searchstime.AddDays(-1).Date;
            }
            else
            {
                searchstime = searchstime.Date;
            }
            if (searchetime.Hour >= 0 && searchetime.Hour <= 7)
            {
                searchetime = searchetime.AddDays(-1).Date;
            }
            else
            {
                searchetime = searchetime.Date;
            }

            #region 读取源数据
            string msg = string.Empty;
            for (int i = 0; i < (searchetime - searchstime).TotalDays + 1; i++)
            {
                DateTime curTime = searchstime.AddDays(i);
                fileName = string.Format("{0}.txt", curTime.ToString("yyyy_MM_dd"));
                fullFileName = Path.Combine(outputPath, fileName);

                //是否存在日志文件
                if (File.Exists(fullFileName))
                {                   
                    msg += frm_Main.formData.CTUnit1.ReadCT(curTime.ToString("yyyy_MM_dd"));
                }
            }
            string[] lines = msg.Split('\n');

            #region 明细汇总
            string[] temp;
            string dates;
            Dictionary<DateTime, List<double>> dic = new Dictionary<DateTime, List<double>>();
            for (int j = 0; j < lines.Length - 1; j++)
            {
                dates = lines[j].Trim('\r');
                temp = dates.Split(',');

                DateTime time = ConvertHelper.GetDef_DateTime(temp[0] + " " + temp[1]);
                if (time < stime && time >= etime)
                {
                    continue;
                }
                
                time = ConvertHelper.GetDef_DateTime(string.Format("{0:yyyy-MM-dd HH:00:00}", time));
                if (!dic.ContainsKey(time))
                {
                    List<double> ct = new List<double>();
                    if (temp.Length > 7)
                    {
                        ct.Add(ConvertHelper.GetDef_Double(temp[7]));
                    }
                    else
                    {
                        ct.Add(ConvertHelper.GetDef_Double(temp[5]));
                    }

                    dic.Add(time, ct);
                }
                else
                {
                    List<double> ct = dic[time];
                    if (temp.Length > 7)
                    {
                        ct.Add(ConvertHelper.GetDef_Double(temp[7]));
                    }
                    else
                    {
                        ct.Add(ConvertHelper.GetDef_Double(temp[5]));
                    }

                    dic[time] = ct;
                }
            }
            #endregion

            //dt = ListToTable(oneRowInfor);
            #endregion

            #region 转换最终结果集
            DataTable resulttable = new DataTable();
            resulttable.Columns.Add("DateTime", Type.GetType("System.DateTime"));
            resulttable.Columns.Add("TimeStr", Type.GetType("System.String"));
            resulttable.Columns.Add("AvgCT", Type.GetType("System.Int32"));
            resulttable.Columns.Add("Shift", Type.GetType("System.String"));

            for (int i = 0; i < (etime - stime).TotalHours; i++)
            {
                DateTime time = ConvertHelper.GetDef_DateTime(string.Format("{0:yyyy/MM/dd HH:00:00}",stime.AddHours(i)));
                decimal avgct = 0;
                if (dic.ContainsKey(time))
                {
                    List<double> ct = dic[time];
                    avgct = ct.Count > 0 ? ConvertHelper.GetDef_Dec(ct.Average(), 1) : 0;
                }
                
                string timestr = string.Format("{0:HH:00}-{1:HH:00}", stime.AddHours(i), stime.AddHours(i + 1)) + "\r\n" + string.Format("{0:yyyy/MM/dd}", stime.AddHours(i));
                DataRow row = resulttable.NewRow();
                row["DateTime"] = stime.AddHours(i);
                row["TimeStr"] = timestr;
                row["AvgCT"] = avgct;
                row["Shift"] = stime.AddHours(i).Hour >= 8 && stime.AddHours(i).Hour < 20 ? "D" : "N";
                resulttable.Rows.Add(row);
            }
            #endregion

            Series[] series = this.chartControl_lineDiagram.SeriesSerializable;
            for (int i = 0; i < series.Length; i++)
            {
                SeriesPointCollection colls = series[i].Points;
                colls.Clear();
            }

            if (series.Length < 2)
            {
                return;
            }

            for (int i = 0; i < resulttable.Rows.Count; i++)
            {
                string name = ConvertHelper.GetDef_Str(resulttable.Rows[i]["TimeStr"]);
                int num = ConvertHelper.GetDef_Int(resulttable.Rows[i]["AvgCT"]);


                if (resulttable.Rows[i]["Shift"].ToString() == "D")
                {
                    //白班
                    SeriesPoint seriesPointD = new SeriesPoint(name, new object[] { ((object)(num)) });
                    seriesPointD.ColorSerializable = "#0080FF";
                    series[0].Points.AddRange(new DevExpress.XtraCharts.SeriesPoint[] { seriesPointD });
                    //夜班
                    SeriesPoint seriesPointN = new SeriesPoint(name, new object[] { ((object)(0)) });
                    seriesPointN.ColorSerializable = "Black";
                    series[1].Points.AddRange(new DevExpress.XtraCharts.SeriesPoint[] { seriesPointN });
                }
                else
                {
                    //白班
                    SeriesPoint seriesPointD = new SeriesPoint(name, new object[] { ((object)(0)) });
                    seriesPointD.ColorSerializable = "#0080FF";
                    series[0].Points.AddRange(new DevExpress.XtraCharts.SeriesPoint[] { seriesPointD });
                    //夜班
                    SeriesPoint seriesPointN = new SeriesPoint(name, new object[] { ((object)(num)) });
                    seriesPointN.ColorSerializable = "Black";
                    series[1].Points.AddRange(new DevExpress.XtraCharts.SeriesPoint[] { seriesPointN });
                }
            }

        }

        private void ShowCTDaily()
        {
            string outputPath = @"D:\DATA\CT";
            string fileName = string.Empty;
            string fullFileName = string.Empty;

            DateTime stime = ConvertHelper.GetDef_DateTime(dateEditS.EditValue);//开始时间
            DateTime etime = ConvertHelper.GetDef_DateTime(dateEditE.EditValue);//结束时间
            DateTime searchstime = stime;
            DateTime searchetime = etime;

            if (searchstime.Hour >= 0 && searchstime.Hour <= 7)
            {
                searchstime = searchstime.AddDays(-1).Date;
            }
            else
            {
                searchstime = searchstime.Date;
            }
            if (searchetime.Hour >= 0 && searchetime.Hour <= 7)
            {
                searchetime = searchetime.AddDays(-1).Date;
            }
            else
            {
                searchetime = searchetime.Date;
            }

            #region 读取源数据
            string msg = string.Empty;
            for (int i = 0; i < (searchetime - searchstime).TotalDays; i++)
            {
                fileName = string.Format("{0}.txt", searchstime.AddDays(i).ToString("yyyy_MM_dd"));
                fullFileName = Path.Combine(outputPath, fileName);

                //是否存在日志文件
                if (File.Exists(fullFileName))
                {
                    //dayTable = CSVFileHelper.OpenCSV(fullFileName);
                    //DateTime curTime = searchstime.AddDays(-i);
                    msg += frm_Main.formData.CTUnit1.ReadCT(searchstime.AddDays(i).ToString("yyyy_MM_dd"));
                }
            }
            string[] lines = msg.Split('\n');

            #region 明细汇总
            string[] temp;
            string dates;
            Dictionary<DateTime, List<double>> dic = new Dictionary<DateTime, List<double>>();
            for (int j = 0; j < lines.Length - 1; j++)
            {
                dates = lines[j].Trim('\r');
                temp = dates.Split(',');

                DateTime time = ConvertHelper.GetDef_DateTime(temp[0] + " " + temp[1]);
                
                time = ConvertHelper.GetDef_DateTime(string.Format("{0:yyyy-MM-dd HH:00:00}", time));
                if (time.Hour < 8)
                {
                    //夜班
                    time = ConvertHelper.GetDef_DateTime(string.Format("{0:yyyy-MM-dd 20:00:00}",time.AddDays(-1)));
                }
                else if (time.Hour >= 20)
                {
                    //夜班
                    time = ConvertHelper.GetDef_DateTime(string.Format("{0:yyyy-MM-dd 20:00:00}", time));
                }
                else
                {
                    //白班
                    time = ConvertHelper.GetDef_DateTime(string.Format("{0:yyyy-MM-dd 08:00:00}", time));
                }

                if (!dic.ContainsKey(time))
                {
                    List<double> ct = new List<double>();
                    if (temp.Length > 7)
                    {
                        ct.Add(ConvertHelper.GetDef_Double(temp[7]));
                    }
                    else
                    {
                        ct.Add(ConvertHelper.GetDef_Double(temp[5]));
                    }

                    dic.Add(time, ct);
                }
                else
                {
                    List<double> ct = dic[time];
                    if (temp.Length > 7)
                    {
                        ct.Add(ConvertHelper.GetDef_Double(temp[7]));
                    }
                    else
                    {
                        ct.Add(ConvertHelper.GetDef_Double(temp[5]));
                    }

                    dic[time] = ct;
                }
            }
            #endregion

            //dt = ListToTable(oneRowInfor);
            #endregion

            #region 转换最终结果集
            DataTable resulttable = new DataTable();
            resulttable.Columns.Add("DateTime", Type.GetType("System.DateTime"));
            resulttable.Columns.Add("TimeStr", Type.GetType("System.String"));
            resulttable.Columns.Add("AvgCT", Type.GetType("System.Int32"));
            resulttable.Columns.Add("Shift", Type.GetType("System.String"));

            for (int i = 0; i < (searchetime - searchstime).TotalDays; i++)
            {
                //白班
                DateTime time = ConvertHelper.GetDef_DateTime(string.Format("{0:yyyy/MM/dd 08:00:00}", stime.AddDays(i)));
                decimal avgct = 0;
                if (dic.ContainsKey(time))
                {
                    List<double> ct = dic[time];
                    avgct = ct.Count > 0 ? ConvertHelper.GetDef_Dec(ct.Average(), 1) : 0;
                }

                string timestr = string.Format("{0:yyyy/MM/dd}", stime.AddDays(i));
                DataRow[] rows = resulttable.Select(string.Format("TimeStr = '{0}' and Shift = '{1}'", timestr, "D"));
                if (rows.Length == 0)
                {
                    DataRow row = resulttable.NewRow();
                    row["DateTime"] = stime.AddDays(i);
                    row["TimeStr"] = timestr;
                    row["AvgCT"] = avgct;
                    row["Shift"] =  "D" ;
                    resulttable.Rows.Add(row);
                }
                //夜班
                time = ConvertHelper.GetDef_DateTime(string.Format("{0:yyyy/MM/dd 20:00:00}", stime.AddDays(i)));
                avgct = 0;
                if (dic.ContainsKey(time))
                {
                    List<double> ct = dic[time];
                    avgct = ct.Count > 0 ? ConvertHelper.GetDef_Dec(ct.Average(), 1) : 0;
                }
                timestr = string.Format("{0:yyyy/MM/dd}", stime.AddDays(i));
                rows = resulttable.Select(string.Format("TimeStr = '{0}' and Shift = '{1}'", timestr, "N"));
                if (rows.Length == 0)
                {
                    DataRow row = resulttable.NewRow();
                    row["DateTime"] = stime.AddDays(i);
                    row["TimeStr"] = timestr;
                    row["AvgCT"] = avgct;
                    row["Shift"] = "N";
                    resulttable.Rows.Add(row);
                }
            }
            #endregion

            Series[] series = this.chartControl_lineDiagram.SeriesSerializable;
            for (int i = 0; i < series.Length; i++)
            {
                SeriesPointCollection colls = series[i].Points;
                colls.Clear();
            }

            if (series.Length < 2)
            {
                return;
            }

            for (int i = 0; i < resulttable.Rows.Count; i++)
            {
                string name = ConvertHelper.GetDef_Str(resulttable.Rows[i]["TimeStr"]);
                //string name = ConvertHelper.GetDef_DateTime(resulttable.Rows[i]["TimeStr"]).ToString("MM：dd");
                int num = ConvertHelper.GetDef_Int(resulttable.Rows[i]["AvgCT"]);
                
                if (resulttable.Rows[i]["Shift"].ToString() == "D")
                {
                    //白班
                    SeriesPoint seriesPointD = new SeriesPoint(name, new object[] { ((object)(num)) });
                    seriesPointD.ColorSerializable = "#0080FF";
                    series[0].Points.AddRange(new DevExpress.XtraCharts.SeriesPoint[] { seriesPointD });
                    //夜班
                    //SeriesPoint seriesPointN = new SeriesPoint(name, new object[] { ((object)(0)) });
                    //seriesPointN.ColorSerializable = "Black";
                    //series[1].Points.AddRange(new DevExpress.XtraCharts.SeriesPoint[] { seriesPointN });
                }
                else
                {
                    //白班
                    //SeriesPoint seriesPointD = new SeriesPoint(name, new object[] { ((object)(0)) });
                    //seriesPointD.ColorSerializable = "#0080FF";
                    //series[0].Points.AddRange(new DevExpress.XtraCharts.SeriesPoint[] { seriesPointD });
                    //夜班
                    SeriesPoint seriesPointN = new SeriesPoint(name, new object[] { ((object)(num)) });
                    seriesPointN.ColorSerializable = "Black";
                    series[1].Points.AddRange(new DevExpress.XtraCharts.SeriesPoint[] { seriesPointN });
                }
            }

        }


        /// <summary>
        /// 获得白天或者晚上的具体每小时产能
        /// </summary>
        private void GetDayOrNightYieldData(DataTable table)
        {
            if (table.Columns.Count < 24)
                return;

            //清除dataTable数据
            day_detialYield.Columns.Clear();
            night_detialYield.Columns.Clear();
            day_detialYield.Clear();
            night_detialYield.Clear();

            //添加列数据
            day_detialYield.Columns.Add("小时");
            day_detialYield.Columns.Add("产能");
            night_detialYield.Columns.Add("小时");
            night_detialYield.Columns.Add("产能");

            for (int j = 0; j < 24; j++)
            {
                string key;
                DataRow row;

                if (j < 12)
                {
                    key = "Hr" + (j + 1).ToString();
                    //白班数据
                    row = day_detialYield.NewRow();
                    row[0] = key;
                    row[1] = ConvertHelper.GetDef_Int(table.Rows[0][j].ToString());

                    day_detialYield.Rows.Add(row);
                    
                }
                else
                {
                    key = "Hr" + (j + 1-12).ToString();
                    //夜班数据
                    row = night_detialYield.NewRow();
                    row[0] = key;
                    row[1] = ConvertHelper.GetDef_Int(table.Rows[0][j].ToString());

                    night_detialYield.Rows.Add(row);                    
                }
            }
        }

        /// <summary>
        /// 获得白天晚上都是空产能
        /// </summary>
        private void GetNullYieldData()
        {
            //清除dataTable数据
            day_detialYield.Columns.Clear();
            night_detialYield.Columns.Clear();
            day_detialYield.Clear();
            night_detialYield.Clear();

            //添加列数据
            day_detialYield.Columns.Add("小时");
            day_detialYield.Columns.Add("产能");
            night_detialYield.Columns.Add("小时");
            night_detialYield.Columns.Add("产能");

            for (int j = 0; j < 24; j++)
            {
                string key;
                DataRow row;

                if (j < 12)
                {
                    key = "Hr" + (j + 1).ToString();
                    //白班数据
                    row = day_detialYield.NewRow();
                    row[0] = key;
                    row[1] = 0;

                    day_detialYield.Rows.Add(row);

                }
                else
                {
                    key = "Hr" + (j + 1 - 12).ToString();
                    //夜班数据
                    row = night_detialYield.NewRow();
                    row[0] = key;
                    row[1] = 0;

                    night_detialYield.Rows.Add(row);
                }
            }
        }

        /// <summary>
        /// 展示数据到折线图中
        /// </summary>
        private  void  ShowLineChart(DataTable datasource1)
        {
            Series[] series = this.chartControl_lineDiagram.SeriesSerializable;
            for (int i = 0; i < series.Length; i++)
            {
                SeriesPointCollection colls = series[i].Points;

                for (int j = 0; j < colls.Count; j++)
                {
                    //日期
                    colls[j].Argument = ConvertHelper.GetDef_Str(datasource1.Rows[j][0]);
                    //数据
                    colls[j].Values[0] = ConvertHelper.GetDef_Double(datasource1.Rows[j][1]);

                }

            }

            //((XYDiagram)chartControl_lineDiagram.Diagram).AxisX.Title.Text = "小时";
            //((XYDiagram)chartControl_lineDiagram.Diagram).AxisY.Title.Text = "产能";
            //((XYDiagram)chartControl_lineDiagram.Diagram).AxisY.Title.Visibility = DefaultBoolean.True;
            //((XYDiagram)chartControl_lineDiagram.Diagram).AxisX.Title.Visibility = DefaultBoolean.True;
        }
        
        /// <summary>
        /// 清除gridControl中所有行
        /// </summary>
        private void ClearAllRows()
        {
            bool _mutilSelected = gridView.OptionsSelection.MultiSelect;//获取当前是否可以多选
            if (!_mutilSelected)
                gridView.OptionsSelection.MultiSelect = true;
            gridView.SelectAll();
            gridView.DeleteSelectedRows();
            gridView.OptionsSelection.MultiSelect = _mutilSelected;//还原之前是否可以多选状态
        }

        /// <summary>
        /// list泛型转dataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public DataTable ListToTable<T>(IList<T> list) where T : class, new()
        {
            if (list == null) return null;
            Type type = typeof(T);
            DataTable dt = new DataTable();

            PropertyInfo[] properties = Array.FindAll(type.GetProperties(), p => p.CanRead);//判断此属性是否有Getter
            Array.ForEach(properties, prop => { dt.Columns.Add(prop.Name, prop.PropertyType); });//添加到列
            foreach (T t in list)
            {
                DataRow row = dt.NewRow();
                Array.ForEach(properties, prop =>
                {
                    row[prop.Name] = prop.GetValue(t, null);
                });//添加到行
                dt.Rows.Add(row);
            }
            return dt;
        }

        private void hScrollBarHour_ValueChanged(object sender, EventArgs e)
        {
            int val = hScrollBarHour.Value;
            //mCurDayIndex = val;
            //HourDataToChart1(val);
            HourDataToChart(val);
        }

        private void hScrollBar_ValueChanged(object sender, EventArgs e)
        {
            int val = hScrollBar.Value;
            m_CurIndex = val;
            ShowIndex(val);
        }

        private void dateEditS_EditValueChanged(object sender, EventArgs e)
        {
            if(m_Refresh)
            {
                return;
            }
            DateTime stime = ConvertHelper.GetDef_DateTime(dateEditS.EditValue);
            if (StatisticsByDay)
            {
                dateEditE.EditValue = ConvertHelper.GetDef_DateTime(dateEditS.EditValue).AddDays(7);
            }
            else
            {
                dateEditE.EditValue = ConvertHelper.GetDef_DateTime(dateEditS.EditValue).AddHours(12);
            }
            DateTime etime = ConvertHelper.GetDef_DateTime(dateEditE.EditValue);
            if(stime > etime)
            {
                return;
            }
            DateTime searchtime = stime;
            if(stime.Hour < 8)
            {
                //8点前属于上一天夜班
                searchtime = stime.AddDays(1);
            }
            
            bool isDay = false;//是否是白班
            if (stime.Hour < 8 || stime.Hour >= 20)
            {
                isDay = false;
            }
            else
            {
                isDay = true;
            }
            //计算滑动条value
            TimeSpan span = searchtime - dateEditS.Properties.MinValue;
            int index = ConvertHelper.GetDef_Int((int)span.TotalDays) * 2;
            if (!isDay)
            {
                index += 1;
            }
            HourDataToChart(index, false);
        }

        private void dateEditDayS_EditValueChanged(object sender, EventArgs e)
        {
            if (m_RefreshDay)
            {
                return;
            }

            DateTime stime = ConvertHelper.GetDef_DateTime(dateEditDayS.EditValue);
            if (StatisticsByDay)
            {
                dateEditDayE.EditValue = ConvertHelper.GetDef_DateTime(dateEditDayS.EditValue).AddDays(7);
            }
            else
            {
                dateEditDayE.EditValue = ConvertHelper.GetDef_DateTime(dateEditDayS.EditValue).AddHours(12);
            }
            DateTime etime = ConvertHelper.GetDef_DateTime(dateEditDayE.EditValue);
            if (stime > etime)
            {
                return;
            }
            DateTime searchtime = stime;
            if (stime.Hour < 8)
            {
                //8点前属于上一天夜班
                searchtime = stime.AddDays(1);
            }

            //计算滑动条value
            TimeSpan span = searchtime - dateEditDayS.Properties.MinValue;
            int index = ConvertHelper.GetDef_Int((int)span.TotalDays);
            m_CurIndex = index;
            ShowIndex(index, false);
        }

        private void SwitchDayHour_Toggled(object sender, EventArgs e)
        {

            //MessageBox.Show(StatisticsByDay.ToString());
            if (StatisticsByDay)
            {
                #region 按天统计
                #region 平均CT
                m_Refresh = true;
                hScrollBarHour.Maximum = 6;
                hScrollBarHour.Value = 6;

                HourDataToChart(hScrollBarHour.Value);
                m_Refresh = false ;
                #endregion
                #region 产量统计
                m_RefreshDay = true;
                DateTime m_CurrentTime = DateTime.Now;
                if (DateTime.Now.Hour >= 0 && DateTime.Now.Hour <= 7)
                {
                    m_CurrentTime = DateTime.Now.AddDays(-1);
                }

                dateEditDayS.EditValue = string.Format("{0:yyyy/MM/dd 8:00}", m_CurrentTime.AddDays(-6));
                dateEditDayE.EditValue = string.Format("{0:yyyy/MM/dd 8:00}", m_CurrentTime.AddDays(1));

                ShowDailyCapacityToChart();

                m_RefreshDay = false;
                #endregion
                #endregion
            }
            else
            {
                #region 按小时统计
                #region 平均CT
                m_Refresh = true;
                hScrollBarHour.Maximum = 13;
                //判断是否是白天还是晚上
                if (DateTime.Now.Hour >= 8 && DateTime.Now.Hour < 20)
                {
                    isDay = true;
                }
                if (isDay)
                {
                    hScrollBarHour.Value = 12;//当天白班数据
                }
                else
                {
                    hScrollBarHour.Value = 13;//当天夜班数据
                }

                HourDataToChart(hScrollBarHour.Value);
                m_Refresh = false;
                #endregion
                #region 产量统计
                m_RefreshDay = true;
                DateTime m_CurrentTime = DateTime.Now;
                if (DateTime.Now.Hour >= 0 && DateTime.Now.Hour <= 7)
                {
                    m_CurrentTime = DateTime.Now.AddDays(-1);
                }
                
                dateEditDayS.EditValue = string.Format("{0:yyyy/MM/dd 8:00}", m_CurrentTime.AddDays(m_CurIndex - 6));
                dateEditDayE.EditValue = ConvertHelper.GetDef_DateTime(string.Format("{0:yyyy/MM/dd 8:00}", m_CurrentTime.AddDays(m_CurIndex - 6))).AddHours(12);

                ShowDailyDetail();

                m_RefreshDay = false;
                #endregion
                #endregion
            }
        }

        private void dateEditE_EditValueChanged(object sender, EventArgs e)
        {
            if (m_Refresh)
            {
                return;
            }
            HourDataToChart(0, false);
        }

        private void dateEditDayE_EditValueChanged(object sender, EventArgs e)
        {
            if (m_RefreshDay)
            {
                return;
            }
            ShowIndex(0, false);
        }
    }


    public class RecordInformation
    {
        public string Date { get; set; }
        public string Shift { get; set; }
        public string Unit_SN { get; set; }
        public string Component_SN { get; set; }
        public string Start_Time { get; set; }
        public string End_Time { get; set; }
        public string CT { get; set; }
        public bool Pass { get; set; }
        public string HiveState { get; set; }
    }



}