using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Threading;

namespace Chart
{
    public partial class Chartcapacity : UserControl
    {
        private DataTable dt;
        private DataTable dtshow;
        private Dictionary<int, string> hourDic = new Dictionary<int, string>();
        private List<CalculationDate> calculationDate = new List<CalculationDate>();
        Dictionary<string, List<string>> uphdic = new Dictionary<string, List<string>>();
        /// <summary>
        /// 当前选中日期总数
        /// </summary>
        private int total;
        private Thread th;
        private Thread th1;
        string[] Uphpaths;
        private int savedyas;
        /// <summary>
        /// 当前总数
        /// </summary>
        private int outTotal;
        private object locker1 = new object();
        private object locker2 = new object();
        System.Timers.Timer m_Timer;
        /// <summary>
        /// 当前产量Excel路径
        /// </summary>
        private string fullFileName = "";
        public Chartcapacity()
        {
            
        }

        private void M_Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            GetTotal();
        }

        /// <summary>
        /// 当前选中日期总数
        /// </summary>
        public int Total
        {
            get
            {
                return total;
            }
            set
            {
                if (txtTotal.InvokeRequired)
                {
                    txtTotal.BeginInvoke(new Action<int>((total) =>
                    {
                        txtTotal.Text = total.ToString();
                    }), value);
                }
                else
                {
                    txtTotal.Text = total.ToString();
                }
            }

        }

        private int dttotal;
        /// <summary>
        /// OK总数
        /// </summary>
        public int DTTotal
        {
            get
            {
                return dttotal;
            }
            set
            {
                if (txtOK.InvokeRequired)
                {
                    txtOK.BeginInvoke(new Action<int>((oktotal) =>
                    {
                        txtOK.Text = (total- dttotal).ToString();
                    }), value);
                }
                else
                {
                    txtOK.Text = (total - dttotal).ToString();
                }
            }

        }
        /// <summary>
        /// 左工位生产总数
        /// </summary>
        private int totalLetf;
        public int TotalLetf
        {
            get
            {
                return totalLetf;
            }
            set
            {
                if (txtTotalLeft.InvokeRequired)
                {
                    txtTotalLeft.BeginInvoke(new Action<int>((totalLetf) =>
                    {
                        txtTotalLeft.Text = totalLetf.ToString();
                    }), value);
                }
                else
                {
                    txtTotalLeft.Text = totalLetf.ToString();
                }
            }

        }
        private int totalright;

        /// <summary>
        /// 右工位生产总数
        /// </summary>
        public int Totalright
        {
            get
            {
                return totalright;
            }
            set
            {
                if (txtTotalRight.InvokeRequired)
                {
                    txtTotalRight.BeginInvoke(new Action<int>((totalright) =>
                    {
                        txtTotalRight.Text = totalright.ToString();
                    }), value);
                }
                else
                {
                    txtTotalRight.Text = totalright.ToString();
                }
            }

        }
        /// <summary>
        /// 当前总数
        /// </summary>
        public int OutTotal
        {
            get
            {
                return outTotal;
            }

            set
            {
                outTotal = value;
            }
        }

        /// <summary>
        /// 数据保存的天数
        /// </summary>
        /// <param name="days"></param>
        public Chartcapacity(int days)
        {
            InitializeComponent();
            m_Timer = new System.Timers.Timer(1000);
            m_Timer.Elapsed += M_Timer_Elapsed;
            m_Timer.Start();
            savedyas = days;
            try
            {
                InitTable();
                //ReadINI();
                //nudays.Value = SaveDays;
                // 加载所有文件
         

                //判断此时需要加载的文件
                int hour = DateTime.Now.Hour; ;
                int minute = DateTime.Now.Minute;
                string fileName;
                if (hour > 7)
                    fileName = string.Format("{0}.csv", DateTime.Now.ToString("yyyy_MM_dd"));
                else
                    fileName = string.Format("{0}.csv", DateTime.Now.AddDays(-1).ToString("yyyy_MM_dd"));

                string outputPath = @"D:\DATA\产量统计";
                string fullFileName = Path.Combine(outputPath, fileName);
                if (CSVFileHelper.OpenCSV(fullFileName) != null && CSVFileHelper.OpenCSV(fullFileName).Rows.Count > 0)
                    dt = CSVFileHelper.OpenCSV(fullFileName);
                //监视文件个数
                //th = new Thread(delete);
                //th.Start();

            }
            catch
            {
            }
            //th = new Thread(delete);
            //th.IsBackground = true;
            //th.Start();
       
        }

      
        private void delete()
        {
            while (true)
            {
              //if(  Common.FileManger.DeleteOverflowFile(@"D:\DATA\产量统计",savedyas))
              //      UpdateCombox();

                Thread.Sleep(500);
            }
        }
        //private void ReadINI()
        //{
        //    string iniPath = Environment.CurrentDirectory + "\\userRun.ini";
        //    string savedaystemp = null;

        //    savedaystemp = Common.Ini.GetPrivateProfileString("setting", "SaveDays", null, iniPath);
        //    int number;
        //    if (savedaystemp == null || (int.TryParse(savedaystemp, out number) == false))
        //    {
        //        SaveDays = 30;
        //        Common.Ini.WritePrivateProfileString("setting", "SaveDays", SaveDays.ToString(), iniPath);
        //    }
        //    else
        //    {
        //        SaveDays = number;
        //    }

        //}
        private void btsave_Click(object sender, EventArgs e)
        {
            //SaveDays = Convert.ToInt32(nudays.Value.ToString());
            //SaveINI();
        }
        //private void SaveINI()
        //{
        //    try
        //    {
        //        string iniPath = Environment.CurrentDirectory + "\\userRun.ini";
        //        //创建文件夹
        //        Common.Ini.CreateDirectoryEx(iniPath);
        //        Common.Ini.WritePrivateProfileString("setting", "SaveDays", SaveDays.ToString(), iniPath);
        //    }
        //    catch
        //    {


        //    }


        //}

        /// <summary>
        /// 加载所有文件
        /// </summary>
        private void UpdateCombox()
        { //加载所有文件
            string path = @"D:\DATA\产量统计";
            if (!Directory.Exists(path))//若文件夹不存在则新建文件夹   
            {
                Directory.CreateDirectory(path); //新建文件夹   
            }
            if (cbpath.InvokeRequired)
            {
                cbpath.BeginInvoke(new Action(() =>
                {
                    cbpath.Items.Clear();
                    comboBox1.Items.Clear();
                    comboBox2.Items.Clear();
                    //获取指定文件夹的所有文件  
                    string[] paths = Directory.GetFiles(path);
                    Array.Reverse(paths);
                    Uphpaths = paths;
                    foreach (var item in paths)
                    {
                        //获取文件后缀名  
                        string extension = Path.GetExtension(item).ToLower();
                        if (extension == ".csv")
                        {
                            string prj = Path.GetFileNameWithoutExtension(item);
                            cbpath.Items.Add(prj);
                            cbpath.SelectedIndex = 0;
                            comboBox1.Items.Add(prj);
                            comboBox1.SelectedIndex = 0;
                            comboBox2.Items.Add(prj);
                            comboBox2.SelectedIndex = 0;

                        }
                    }
                }));
            }
            else
            {
                cbpath.Items.Clear();
                comboBox1.Items.Clear();
                comboBox2.Items.Clear();
                //获取指定文件夹的所有文件  
                string[] paths = Directory.GetFiles(path);
              
                Array.Reverse(paths);
                Uphpaths = paths;
                foreach (var item in paths)
                {
                    //获取文件后缀名  
                    string extension = Path.GetExtension(item).ToLower();
                    if (extension == ".csv")
                    {
                        string prj = Path.GetFileNameWithoutExtension(item);
                        cbpath.Items.Add(prj);
                        cbpath.SelectedIndex = 0;
                        comboBox1.Items.Add(prj);
                        comboBox1.SelectedIndex = 0;
                        comboBox2.Items.Add(prj);
                        comboBox2.SelectedIndex = 0;
                    }
                }
            }
        }
        /// <summary>
        /// 初始化所有数据
        /// </summary>
        private void InitTable()
        {
            calculationDate.Clear();
            for (int j = 0; j < 24; j++)
            {
                calculationDate.Add(new CalculationDate());
            }
            #region
            dt = new DataTable();
            dt.Columns.Add("8:00-9:00", typeof(String));
            dt.Columns.Add("9:00-10:00", typeof(String));
            dt.Columns.Add("10:00-11:00", typeof(String));
            dt.Columns.Add("11:00-12:00", typeof(String));
            dt.Columns.Add("12:00-13:00", typeof(String));
            dt.Columns.Add("13:00-14:00", typeof(String));
            dt.Columns.Add("14:00-15:00", typeof(String));
            dt.Columns.Add("15:00-16:00", typeof(String));
            dt.Columns.Add("16:00-17:00", typeof(String));
            dt.Columns.Add("17:00-18:00", typeof(String));
            dt.Columns.Add("18:00-19:00", typeof(String));
            dt.Columns.Add("19:00-20:00", typeof(String));
            dt.Columns.Add("20:00-21:00", typeof(String));
            dt.Columns.Add("21:00-22:00", typeof(String));
            dt.Columns.Add("22:00-23:00", typeof(String));
            dt.Columns.Add("23:00-24:00", typeof(String));
            dt.Columns.Add("0:00-1:00", typeof(String));
            dt.Columns.Add("1:00-2:00", typeof(String));
            dt.Columns.Add("2:00-3:00", typeof(String));
            dt.Columns.Add("3:00-4:00", typeof(String));
            dt.Columns.Add("4:00-5:00", typeof(String));
            dt.Columns.Add("5:00-6:00", typeof(String));
            dt.Columns.Add("6:00-7:00", typeof(String));
            dt.Columns.Add("7:00-8:00", typeof(String));
         
            DataRow dr = dt.NewRow();
            //左工位OK
            for (int j = 0; j < 24; j++)
            {
                dr[j] = 0;
            }
            dt.Rows.Add(dr);
            //右工位OK
            dr = dt.NewRow();
            for (int j = 0; j < 24; j++)
            {
                dr[j] = 0;
            }
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            //左工位NG
            for (int j = 0; j < 24; j++)
            {
                dr[j] = 0;
            }
            dt.Rows.Add(dr);
            //右工位NG
            dr = dt.NewRow();
            for (int j = 0; j < 24; j++)
            {
                dr[j] = 0;
            }
            dt.Rows.Add(dr);
            hourDic.Clear();
            hourDic.Add(0, "0:00-1:00");
            hourDic.Add(1, "1:00-2:00");
            hourDic.Add(2, "2:00-3:00");
            hourDic.Add(3, "3:00-4:00");
            hourDic.Add(4, "4:00-5:00");
            hourDic.Add(5, "5:00-6:00");
            hourDic.Add(6, "6:00-7:00");
            hourDic.Add(7, "7:00-8:00");
            hourDic.Add(8, "8:00-9:00");
            hourDic.Add(9, "9:00-10:00");
            hourDic.Add(10, "10:00-11:00");
            hourDic.Add(11, "11:00-12:00");
            hourDic.Add(12, "12:00-13:00");
            hourDic.Add(13, "13:00-14:00");
            hourDic.Add(14, "14:00-15:00");
            hourDic.Add(15, "15:00-16:00");
            hourDic.Add(16, "16:00-17:00");
            hourDic.Add(17, "17:00-18:00");
            hourDic.Add(18, "18:00-19:00");
            hourDic.Add(19, "19:00-20:00");
            hourDic.Add(20, "20:00-21:00");
            hourDic.Add(21, "21:00-22:00");
            hourDic.Add(22, "22:00-23:00");
            hourDic.Add(23, "23:00-24:00");
     
            #endregion
        }


        /// <summary>
        /// 左工位OK加1
        /// </summary>
        public void AddOkLeft()
        {
            Task.Run(() => addok(Dir.Left));
        }
        /// <summary>
        /// 左工位NG加1
        /// </summary>
        public void AddNGLeft()
        {
            Task.Run(() => addng(Dir.Left));
        }
        /// <summary>
        /// 右工位OK加1
        /// </summary>
        public void AddOkRight()
        {
            Task.Run(() => addok(Dir.Right));
        }
        /// <summary>
        /// 右工位NG加1
        /// </summary>
        public void AddNGRight()
        {
            Task.Run(() => addng(Dir.Right));
        }
        private void addok(Dir i)
        {
            try
            {
                lock (locker1)
                {
                    int hour = DateTime.Now.Hour;
                    int hour1 = hour;     

                    string fileName;
                    if ( hour1 > 7)
                        fileName = string.Format("{0}.csv", DateTime.Now.ToString("yyyy_MM_dd"));
                    else
                        fileName = string.Format("{0}.csv", DateTime.Now.AddDays(-1).ToString("yyyy_MM_dd"));

                    string outputPath = @"D:\DATA\产量统计";
                    if (!Directory.Exists(outputPath))
                    {
                        Directory.CreateDirectory(outputPath);
                    }
                    string fullFileName = Path.Combine(outputPath, fileName);
                    if (!File.Exists(fullFileName))
                    {
                        InitTable();
                        CSVFileHelper.SaveCSV(dt, fullFileName);
                        UpdateCombox();
                    }
                    if (i == Dir.Left)    //表格第一第二行：左右的OK数据
                    {
                        string value = dt.Rows[0][hourDic[hour]].ToString();
                        int temp = Convert.ToInt32(value) + 1;
                        dt.Rows[0][hourDic[hour]] = temp;
                    }
                    else
                    {
                        string value = dt.Rows[1][hourDic[hour]].ToString();
                        int temp = Convert.ToInt32(value) + 1;
                        dt.Rows[1][hourDic[hour]] = temp;
                    }
                    CSVFileHelper.SaveCSV(dt, fullFileName);
                }
            }
            catch
            {
            }
        }
        private void addng(Dir i)
        {
            try
            {

                lock (locker1)
                {
                    int hour = DateTime.Now.Hour;
                    int hour1 = hour;


                    string fileName;
                    if (hour1 >7)
                        fileName = string.Format("{0}.csv", DateTime.Now.ToString("yyyy_MM_dd"));
                    else
                        fileName = string.Format("{0}.csv", DateTime.Now.AddDays(-1).ToString("yyyy_MM_dd"));

                    string outputPath = @"D:\DATA\产量统计";
                    if (!Directory.Exists(outputPath))
                    {
                        Directory.CreateDirectory(outputPath);
                    }
                    string fullFileName = Path.Combine(outputPath, fileName);
                    if (!File.Exists(fullFileName))
                    {
                        InitTable();
                        CSVFileHelper.SaveCSV(dt, fullFileName);
                        UpdateCombox();
                    }

                    if (i == Dir.Left)   //表格第三第四行，左右的NG数据
                    {
                        string value = dt.Rows[2][hourDic[hour]].ToString();
                        int temp = Convert.ToInt32(value) + 1;
                        dt.Rows[2][hourDic[hour]] = temp;
                    }
                    else
                    {
                        string value = dt.Rows[3][hourDic[hour]].ToString();
                        int temp = Convert.ToInt32(value) + 1;
                        dt.Rows[3][hourDic[hour]] = temp;
                    }
                    CSVFileHelper.SaveCSV(dt, fullFileName);
                }

            }
            catch
            {

            }
        }

        /// <summary>
        /// 产量统计
        /// </summary>
        /// <param name="time"></param>
        public DataTable GetDayYield(DateTime time)
        {
            DataTable dtshow = new DataTable();
            try
            {
                lock (locker1)
                {
                    string timestr = time.Hour > 7?time.ToString("yyyy_MM_dd"): time.AddDays(-1).ToString("yyyy_MM_dd");
                    string outputPath = @"D:\DATA\产量统计";
                    string fileName = string.Format("{0}.csv", timestr);
                    string fullFileName = Path.Combine(outputPath, fileName);

                    if (File.Exists(fullFileName))
                    {
                        dtshow = new DataTable();
                        dtshow = CSVFileHelper.OpenCSV(fullFileName);
                    }
                }
                    

            }
            catch
            {


            }
            return dtshow;
        }
        public DataTable GetDayNG(DateTime time)
        {
            DataTable dtshow = new DataTable();
            try
            {
                lock (locker1)
                {
                    string timestr = time.Hour > 7 ? time.ToString("yyyy_MM_dd") : time.AddDays(-1).ToString("yyyy_MM_dd");
                    string outputPath = @"D:\DATA\拍照NG抛料数据";
                    string fileName = string.Format("{0}.csv", timestr);
                    string fullFileName = Path.Combine(outputPath, fileName);

                    if (File.Exists(fullFileName))
                    {
                        dtshow = new DataTable();
                        dtshow = CSVFileHelper.OpenCSV(fullFileName);
                    }
                }
            }
            catch
            {
            }
            return dtshow;
        }

        /// <summary>
        /// 左侧DT次数
        /// </summary>
        public void AddDT()
        {
            //  Task.Run(() => addng(Dir.Left));
        }
        /// <summary>
        /// 右侧DT次数
        /// </summary>
        //public void AddDTRight()
        //{

        //    Task.Run(() => addng(Dir.Right));
        //}


        private void GetTotal()
        {
            try
            {
                lock (locker2)
                {
                    DataTable dt;
                    string outputPath = @"D:\DATA\产量统计";
                    //string fileName = string.Format("{0}.csv", cbpath.Text);
                    //string fullFileName = Path.Combine(outputPath, fileName);
                    if(fullFileName=="")
                    {
                    fullFileName = LogExcel.getLastFile(outputPath);
                    }
                    if (!File.Exists(fullFileName))
                    {
                        return;
                    }
                    dt = new DataTable();
                    dt = CSVFileHelper.OpenCSV(fullFileName);
                    ChangeDtToList1(dt);
                }
            }
            catch
            {


            }
        }

        //分析数据
        private void bt_show_Click(object sender, EventArgs e)
        {
            try
            {
                string outputPath = @"D:\DATA\产量统计";
                string fileName = string.Format("{0}.csv", cbpath.Text);
                fullFileName = Path.Combine(outputPath, fileName);
                if (File.Exists(fullFileName))
                {
                    dtshow = new DataTable();
                    dtshow = CSVFileHelper.OpenCSV(fullFileName);
                }
                ChangeDtToList(dtshow);
                //列表显示
                UpdateList(calculationDate,listView1,1);
                UpdateList(calculationDate, listView2, 2);
                txtTotal.Text = total.ToString();
                txtOK.Text = (total - DTTotal).ToString();
                txtYiled.Text = ((total - DTTotal) * 1.0 / total * 100).ToString("f2") + "%";

                UpdatChart(calculationDate);
                UpdatTtxt(calculationDate);
            }
            catch
            {


            }
        }
        private void ChangeDtToList(DataTable dt)
        {
            totalLetf = 0;
            total = 0;
            totalright = 0;
            dttotal = 0;
            for (int j = 0; j < 24; j++)
            {
                calculationDate[j].Name = dt.Columns[j].ToString();
             //   calculationDate[j].Name1 = dt.Columns[j].ToString();
                int okleft = Convert.ToInt32(dt.Rows[0][j].ToString());
                int okright = Convert.ToInt32(dt.Rows[1][j].ToString());
                int dtleft = Convert.ToInt32(dt.Rows[2][j].ToString());
                int dtright = Convert.ToInt32(dt.Rows[3][j].ToString());
                calculationDate[j].Total = okleft + okright + dtleft+ dtright;      
                calculationDate[j].DTTotal = dtleft + dtright;
                total = total + calculationDate[j].Total;
                totalLetf = totalLetf + okleft+ dtleft;
                totalright = totalright + okright+ dtright;
                dttotal = dttotal + calculationDate[j].DTTotal;
                //if (calculationDate[j].Total > 0)
                //    calculationDate[j].OkRate = (calculationDate[j].Total - calculationDate[j].DTTotal * 1.0) / calculationDate[j].Total;
                //else
                //    calculationDate[j].OkRate = 0.0;
            }
        }
        private void ChangeDtToList1(DataTable dt)
        {
            int leftTotal = 0;
            int rightTotal = 0;
            int leftNG = 0;
            int rightNG= 0;
            outTotal = 0;
            for (int j = 0; j < 24; j++)
            {
                int okleft = Convert.ToInt32(dt.Rows[0][j].ToString());
                int okright = Convert.ToInt32(dt.Rows[1][j].ToString());
                outTotal = outTotal + okleft + okright;
                leftNG += Convert.ToInt32(dt.Rows[2][j].ToString());
                rightNG += Convert.ToInt32(dt.Rows[3][j].ToString());
                leftTotal = leftTotal + Convert.ToInt32(dt.Rows[2][j].ToString())+ okleft;
                rightTotal = rightTotal + Convert.ToInt32(dt.Rows[3][j].ToString()) + okright;
            }
            txtTotal.Text = (leftTotal + rightTotal).ToString();
            txtOK.Text = (leftTotal - leftNG + rightTotal - rightNG).ToString();
            if (leftTotal + rightTotal > 0)
            {
                txtYiled.Text = ((leftTotal - leftNG+ rightTotal- rightNG) * 1.0 / (leftTotal+ rightTotal) * 100).ToString("f2") + "%";
            }
                txtTotalLeft.Text = leftTotal.ToString();
                txtOKLeft.Text = (leftTotal - leftNG).ToString();
                txtNGLeft.Text = leftNG.ToString();
                if (leftTotal > 0)
                {
                    txtYiledLeft.Text = ((leftTotal - leftNG) * 1.0 / leftTotal * 100).ToString("f2") + "%";
                }
                txtTotalRight.Text = rightTotal.ToString();
                txtOKRight.Text = (rightTotal - rightNG).ToString();
                txtNGRight.Text = rightNG.ToString();
                if (rightTotal > 0)
                {
                    txtYiledRight.Text = ((rightTotal - rightNG) * 1.0 / rightTotal * 100).ToString("f2") + "%";
                }
        }

        //刷新报表
        private void UpdateList(List<CalculationDate> cal,ListView lv,int type)
        {
            int num = 0;
            int num1 = 12;
            int totalNum = 0;
            int dt = 0;
            //double rate = 0.0;
            lv.Items.Clear();
            ListViewItem li;
            if (type != 1)
            {
                num1 = 24;
                   num = 12;
            }
            for (int j = num; j < num1; j++)
            {
                li = new ListViewItem(cal[j].Name);
                li.SubItems.Add(cal[j].Total.ToString());
                totalNum = totalNum + cal[j].Total;
                li.SubItems.Add((cal[j].Total-cal[j].DTTotal).ToString());
                li.SubItems.Add(cal[j].DTTotal.ToString());
                dt = dt + cal[j].DTTotal;
                //li.SubItems.Add((cal[j].OkRate * 100).ToString("0.00") + "%");

                lv.Items.Add(li);
            }

            //if (ok != 0)
            //    rate = (ok - ng * 1.0) / ok;
            li = new ListViewItem("合计");
            li.SubItems.Add(totalNum.ToString());
            li.SubItems.Add((totalNum - dt).ToString());
            li.SubItems.Add(dt.ToString());
            //li.SubItems.Add((rate * 100).ToString("0.00") + "%");
            lv.Items.Add(li);
        }

        //刷新柱状图
        private void UpdatChart(List<CalculationDate> cal)
        {
            chart1.DataSource = cal;

            //chart1.Series["DownTimen次数"].XValueMember = "Name";
            //chart1.Series["DownTimen次数"].YValueMembers = "DTTotal";
            //chart1.Series["DownTimen次数"].Label = "#VAL";
            //chart1.Series["DownTimen次数"].IsValueShownAsLabel = true;
            //chart1.Series["DownTimen次数"].CustomProperties = "LabelStyle=Top";

            chart1.Series["产出数量"].XValueMember = "Name";
            chart1.Series["产出数量"].YValueMembers = "Total";
            chart1.Series["产出数量"].Label = "#VAL";
            chart1.Series["产出数量"].IsValueShownAsLabel = true;
            chart1.Series["产出数量"].CustomProperties = "LabelStyle=Top";
        }

        //刷新生产统计
        private void UpdatTtxt(List<CalculationDate> cal)
        {

            Total = total;
            DTTotal = dttotal;
            TotalLetf = totalLetf;
            Totalright = totalright;

        }
        public class CalculationDate
        {
            private string _name="";
            public string Name
            {
                get { return _name; }
                set
                {
                    _name = value;
                }

            }
            private int taotal = 0;
            public int Total
            {
                get { return taotal; }
                set
                {
                    taotal = value;
                }

            }

            private int dtTotal = 0;
            public int DTTotal
            {
                get { return dtTotal; }
                set
                {
                    dtTotal = value;
                }

            }
            //private int okTotal = 0;
            //public int OKTotal
            //{
            //    get { return okTotal; }
            //    set
            //    {
            //        okTotal = value;
            //    }

            //}
            //private double okRate = 0;
            //public double OkRate
            //{
            //    get { return okRate; }
            //    set
            //    {
            //        okRate = value;
            //    }

            //}
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (locker)
                return;
            if (radioButton1.Checked)
            {
                tabPage1.Parent = null;
                tabPage2.Parent = null;
            
                tabPage1.Parent = tabControl1;
            }

            else if (radioButton2.Checked)

            {
                tabPage1.Parent = null;
                tabPage2.Parent = null;
                tabPage2.Parent = tabControl1;
            }
        }

        bool locker = false;
        private void Chartcapacity_Load(object sender, EventArgs e)
        {
            locker = true;

            UpdateCombox();

            radioButton1.Checked = true;
            tabPage1.Parent = null;
            tabPage2.Parent = null;
            tabPage1.Parent = tabControl1;
            locker = false;
            m_Timer = new System.Timers.Timer(1000);
            m_Timer.Elapsed += M_Timer_Elapsed;
            m_Timer.Start();
        }

        public enum Dir
        {
            Left,
            Right
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string a = @"D:\DATA\产量统计\" + comboBox1.Text + ".csv";
            string b = @"D:\DATA\产量统计\" + comboBox2.Text + ".csv";
            DataTable dtshow ;

           uphdic=new Dictionary<string, List<string>>();
                     
            Task.Run(new Action(() =>
            {
                try
                {

                    button1.BeginInvoke(new Action(() =>
                    {
                        button1.Enabled = false;
                    }));
       

                    int index = Uphpaths.ToList().IndexOf(a);

                    int index2 = Uphpaths.ToList().IndexOf(b);
                    int i = 0;
                    if (index > index2)
                    {
                        dtshow = new DataTable();
                        foreach (var item in Uphpaths)
                        {
                            string prj = Path.GetFileNameWithoutExtension(item);
                            if (i <= index && i >= index2)
                            {
                                dtshow = CSVFileHelper.OpenCSV(item);

                                //解析数据
                                List<string> dt = new List<string>();
                                for (int j = 0; j < dtshow.Columns.Count; j++)
                                {
                                    int okleft = Convert.ToInt32(dtshow.Rows[0][j].ToString());
                                    int okright = Convert.ToInt32(dtshow.Rows[1][j].ToString());
                                    dt.Add((okleft+ okright).ToString());
                                }
                                //增加时间
                                uphdic.Add(prj, dt);
                             
                            }

                            i++;
                        }



                    }
                    i = 0;
                    if (index < index2)
                    {
                        dtshow = new DataTable();
                        foreach (var item in Uphpaths)
                        {
                            string prj = Path.GetFileNameWithoutExtension(item);
                            if (i >= index && i <= index2)
                            {
                                dtshow = CSVFileHelper.OpenCSV(item);

                                //解析数据
                                List<string> dt = new List<string>();
                                for (int j = 0; j < dtshow.Columns.Count; j++)
                                {
                                    int okleft = Convert.ToInt32(dtshow.Rows[0][j].ToString());
                                    int okright = Convert.ToInt32(dtshow.Rows[1][j].ToString());
                                    dt.Add((okleft + okright).ToString());
                                }
                                //增加时间
                                uphdic.Add(prj, dt);
                            }
                            i++;
                        }
                    }
                    i = 0;
                    if (index == index2)
                    {
                        dtshow = new DataTable();
                        foreach (var item in Uphpaths)
                        {
                            string prj = Path.GetFileNameWithoutExtension(item);
                            if (i == index)
                            {
                                dtshow = CSVFileHelper.OpenCSV(item);

                                //解析数据
                                List<string> dt = new List<string>();
                                for (int j = 0; j < dtshow.Columns.Count; j++)
                                {
                                    int okleft = Convert.ToInt32(dtshow.Rows[0][j].ToString());
                                    int okright = Convert.ToInt32(dtshow.Rows[1][j].ToString());
                                    dt.Add((okleft + okright).ToString());
                                }
                                //增加时间
                                uphdic.Add(prj, dt);
                            }
                            i++;
                        }
                    }
                    listView1.BeginInvoke(new Action(() =>
                    {

                        listView3.Items.Clear();
                     
                        foreach (var item in uphdic.Keys)
                        {
                            int id = 0;
                            int num = 0;
                            ListViewItem li = new ListViewItem(item);
                            foreach (var item1 in uphdic[item])
                            {
                                li.SubItems.Add(item1);
                                num = num + Convert.ToInt32(item1);
                                if (id == 11)
                                {
                                    
                                    li.SubItems.Add(num.ToString());
                                    num = 0;
                                }
                                if (id == 23)
                                    li.SubItems.Add(num.ToString());
                                id++;
                            }
                            listView3.Items.Add(li);
                       
                        }

                    }));




                    button1.BeginInvoke(new Action(() =>
                    {
                        button1.Enabled = true;
                    }));
                }
                catch
                {
                    button1.BeginInvoke(new Action(() =>
                    {
                        button1.Enabled = true;
                    }));

                }

            }));
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile1 = new SaveFileDialog();
            saveFile1.Filter = "文本文件(.csv)|*.csv";
            saveFile1.FilterIndex = 1;
            if (saveFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK && saveFile1.FileName.Length > 0)
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(saveFile1.FileName, false);
                try
                {
                    string messge = "时间,8:00,9:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,白班合计,20:00,21:00,22:00,23:00,0:00,1:00,2:00,3:00,4:00,5:00,6:00,7:00,白班合计";
                    sw.WriteLine(messge);
                   
                    foreach (var item in uphdic.Keys)
                    {
                        int i = 0;
                        int num = 0;
                        messge = item;
                        foreach (var item1 in uphdic[item])
                        {
                            messge = messge + "," + item1;
                            num = num + Convert.ToInt32(item1);
                            if (i == 11)
                            {
                             
                                messge = messge + "," + num;
                                num = 0;
                            }
                            if (i==23)
                                messge = messge + "," + num;
                            i++;
                        }
                        sw.WriteLine(messge);
                  
                    }
                
                }
                catch
                {

                }
                finally
                {
                    sw.Close();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            addng(Dir.Left);
        }
    }

    public class CSVFileHelper
    {

        private static object lockerwrite = new object();


        private static object lockerread = new object();
        /// <summary>
        /// 将DataTable中数据写入到CSV文件中
        /// </summary>
        /// <param name="dt">提供保存数据的DataTable</param>
        /// <param name="fileName">CSV的文件路径</param>
        public static void SaveCSV(DataTable dt, string fullPath)
        {
            lock (lockerwrite)
            {
                try
                {
                    FileInfo fi = new FileInfo(fullPath);
                    if (!fi.Directory.Exists)
                    {
                        fi.Directory.Create();
                    }
                    System.IO.FileStream fs = new System.IO.FileStream(fullPath, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite, FileShare.Read);
                    //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
                    StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
                    string data = "";
                    //写出列名称
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        data += dt.Columns[i].ColumnName.ToString();
                        if (i < dt.Columns.Count - 1)
                        {
                            data += ",";
                        }
                    }
                    sw.WriteLine(data);
                    //写出各行数据
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        data = "";
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            string str = dt.Rows[i][j].ToString();
                            str = str.Replace("\"", "\"\"");//替换英文冒号 英文冒号需要换成两个冒号
                            if (str.Contains(',') || str.Contains('"')
                                || str.Contains('\r') || str.Contains('\n')) //含逗号 冒号 换行符的需要放到引号中
                            {
                                str = string.Format("\"{0}\"", str);
                            }

                            data += str;
                            if (j < dt.Columns.Count - 1)
                            {
                                data += ",";
                            }
                        }
                        sw.WriteLine(data);
                    }
                    sw.Close();
                    fs.Close();

                }
                catch
                {
                }
            }
        }


        /// <summary>
        /// 将CSV文件的数据读取到DataTable中
        /// </summary>
        /// <param name="fileName">CSV文件路径</param>
        /// <returns>返回读取了CSV数据的DataTable</returns>
        public static DataTable OpenCSV(string filePath)
        {
            lock (lockerwrite)
            {
                DataTable dt = new DataTable();
                try
                {   //    Encoding encoding = Common.GetType(filePath); //Encoding.ASCII;//

                    FileStream fs = new FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite, FileShare.Write);

                    //StreamReader sr = new StreamReader(fs, Encoding.UTF8);
                    StreamReader sr = new StreamReader(fs, Encoding.Default);
                    //string fileContent = sr.ReadToEnd();
                    //encoding = sr.CurrentEncoding;
                    //记录每次读取的一行记录
                    string strLine = "";
                    //记录每行记录中的各字段内容
                    string[] aryLine = null;
                    string[] tableHead = null;
                    //标示列数
                    int columnCount = 0;
                    //标示是否是读取的第一行
                    bool IsFirst = true;
                    //逐行读取CSV中的数据
                    while ((strLine = sr.ReadLine()) != null)
                    {
                        //strLine = Common.ConvertStringUTF8(strLine, encoding);
                        //strLine = Common.ConvertStringUTF8(strLine);

                        if (IsFirst == true)
                        {
                            tableHead = strLine.Split(',');
                            IsFirst = false;
                            columnCount = tableHead.Length;
                            //创建列
                            for (int i = 0; i < columnCount; i++)
                            {
                                DataColumn dc = new DataColumn(tableHead[i]);
                                dt.Columns.Add(dc);
                            }
                        }
                        else
                        {
                            aryLine = strLine.Split(',');
                            DataRow dr = dt.NewRow();
                            for (int j = 0; j < columnCount; j++)
                            {
                                dr[j] = aryLine[j];

                            }
                            dt.Rows.Add(dr);
                        }
                    }
                    if (aryLine != null && aryLine.Length > 0)
                    {
                        dt.DefaultView.Sort = tableHead[0] + " " + "asc";
                    }

                    sr.Close();
                    fs.Close();
                    return dt;

                }
                catch
                {
                    dt = null;
                    return dt;

                }

            }
        }
    }
}
