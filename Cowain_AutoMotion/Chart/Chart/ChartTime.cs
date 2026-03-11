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
using System.Threading;
using System.Collections;

namespace Chart
{
    public partial class ChartTime : UserControl
    {
        private DataTable dt;
        private DataTable dtshow;
        private Dictionary<int, string> hourDic = new Dictionary<int, string>();
        private List<CalculationTimeDate> calculationDate = new List<CalculationTimeDate>();
        private List<CalculationdayDate> calculationdayDate = new List<CalculationdayDate>();
        public static MachineStatus runStatus = MachineStatus.idle;
        private static MachineStatus towerStatus = MachineStatus.idle;
        public DateTime StatusTime = DateTime.Now;//状态切换时间

        public static MachineStatus lastRunStatus = MachineStatus.running;//上一次状态
        public static MachineStatus llastRunStatus = MachineStatus.idle;//上上一次状态
        string[] Timepaths;
        private Thread th1;
        private Thread th2;
        private int savedays;
        DateTime dtold = new DateTime();
        public bool b_Close = false;
        /// <summary>
        /// 数据保存的天数
        /// </summary>
        /// <param name="days"></param>
        public ChartTime(int days)
        {
            InitializeComponent();
            savedays = days;
            try
            {
                InitTable();
                //ReadINI();
                //nudays.Value = SaveDays;
                // 加载所有文件
                UpdateCombox();

                //判断此时需要加载的文件
                int hour = DateTime.Now.Hour; ;
                int minute = DateTime.Now.Minute;
                string fileName;
                if (hour > 7)
                    fileName = string.Format("{0}.csv", DateTime.Now.ToString("yyyy_MM_dd"));
                else
                    fileName = string.Format("{0}.csv", DateTime.Now.AddDays(-1).ToString("yyyy_MM_dd"));

                string outputPath = @"D:\DATA\运行时间";
                string fullFileName = Path.Combine(outputPath, fileName);
                if (CSVFileHelper.OpenCSV(fullFileName) != null && CSVFileHelper.OpenCSV(fullFileName).Rows.Count > 0)
                    dt = CSVFileHelper.OpenCSV(fullFileName);
            }
            catch
            {
            }
            //th = new Thread(delete);
            //th.IsBackground = true;
            //th.Start();

            th1 = new Thread(TimerCallBack);
            th1.IsBackground = true;
            th1.Start();

            th2 = new Thread(comparetodt);
            th2.IsBackground = true;
            th2.Start();



        }

        int now = 100;
        //1S执行一次
        private void TimerCallBack()
        {
            while (true)
            {
                if (b_Close)
                {
                    break;
                }
                Thread.Sleep(200);
                try
                {
                    int a = DateTime.Now.Second;
                    if (now != a)
                    {
                        now = a;
                        act();
                    }
                }
                catch
                {

                }


            }

        }

        /// <summary>
        /// 此函数用来计时，目前暂定90S，如果两分钟内未调用此函数，认为设备待料。
        /// </summary>
        public void Start()
        {
            dtold = DateTime.Now;
            if (runStatus != MachineStatus.engineering && runStatus != MachineStatus.error_down && runStatus != MachineStatus.idle)
            {
                runStatus = MachineStatus.idle;
                StatusTime = DateTime.Now;
            }
        }
        /// <summary>
        /// 此函数用来计时，目前暂定90S，如果两分钟内未调用此函数，认为设备待料。
        /// </summary>
        public void SetTime()
        {
            dtold = DateTime.Now;
        }
        
        public void SetRunStatus(MachineStatus curstatus)
        {
            //llastRunStatus = lastRunStatus;
            if (runStatus == MachineStatus.running)
            {
                lastRunStatus = MachineStatus.running;
            }
            else if (runStatus == MachineStatus.engineering)
            {
                lastRunStatus = MachineStatus.engineering;
            }
            SaveDateHIVE("从["+ runStatus + "]切换到["+ curstatus + "]", HiveLogType.Other);
            //lastRunStatus = runStatus;
            runStatus = curstatus;
            StatusTime = DateTime.Now;
        }

        private object locker1 = new object();
        public enum HiveLogType
        {
            MachineState,
            MachineError,
            MachineData,
            Other
        }

        public void SaveDateHIVE(string result, HiveLogType type)
        {
            try
            {
                lock (locker1)
                {
                    string fileName;
                    fileName = string.Format("hour_{0}.txt", DateTime.Now.ToString("HH"));

                    string outputPath = @"D:\DATA\HIVE Log\";
                    if (type == HiveLogType.MachineState)
                    {
                        outputPath += "Machine State";
                    }
                    else if (type == HiveLogType.MachineError)
                    {
                        outputPath += "Machine Error";
                    }
                    else if (type == HiveLogType.MachineData)
                    {
                        outputPath += "Machine Data";
                    }
                    else
                    {
                        outputPath = @"D:\DATA\H other log";
                    }
                    outputPath += @"\" + DateTime.Now.ToString("yyyyMMdd");

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
                        sw.WriteLine(DateTime.Now.ToString("HH:mm:ss") + result + "");
                        sw.Close();
                        fs.Close();
                    }
                    else
                    {
                        fs = new System.IO.FileStream(fullFileName, System.IO.FileMode.Append, System.IO.FileAccess.Write, FileShare.Read);
                        //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
                        sw = new StreamWriter(fs, System.Text.Encoding.Default);
                        sw.WriteLine(DateTime.Now.ToString("HH:mm:ss") + result + "");
                        sw.Close();
                        fs.Close();
                    }
                }
            }
            catch { }
        }

        private void comparetodt()
        {
            while (true)
            {
                if (b_Close)
                {
                    break;
                }
                Thread.Sleep(1000);
                DateTime dtnew = DateTime.Now;
                TimeSpan s = dtnew - dtold;
                if (s.TotalSeconds > 90)        //待料时间
                {
                    if (runStatus == MachineStatus.running || runStatus == MachineStatus.engineering)
                    {
                        SetRunStatus(MachineStatus.idle);
                        //runStatus = MachineStatus.idle;
                        //StatusTime = DateTime.Now;
                        dtold = DateTime.Now;
                    }
                }
            }
        }

        public void act()
        {
            int hour = DateTime.Now.Hour;
            int hour1 = hour;

            string fileName;
            if (hour1 > 7)
                fileName = string.Format("{0}.csv", DateTime.Now.ToString("yyyy_MM_dd"));
            else
                fileName = string.Format("{0}.csv", DateTime.Now.AddDays(-1).ToString("yyyy_MM_dd"));

            string outputPath = @"D:\DATA\运行时间";
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

            string value = "";
            int temp;
            switch (runStatus)
            {
                case MachineStatus.running:
                    value = dt.Rows[0][hourDic[hour]].ToString();
                    temp = Convert.ToInt32(value) + 1;
                    dt.Rows[0][hourDic[hour]] = temp;
                    break;
                case MachineStatus.idle:
                    value = dt.Rows[1][hourDic[hour]].ToString();
                    temp = Convert.ToInt32(value) + 1;
                    dt.Rows[1][hourDic[hour]] = temp;
                    break;
                case MachineStatus.engineering:
                    value = dt.Rows[2][hourDic[hour]].ToString();
                    temp = Convert.ToInt32(value) + 1;
                    dt.Rows[2][hourDic[hour]] = temp;
                    break;
                case MachineStatus.planned_downtime:
                    value = dt.Rows[3][hourDic[hour]].ToString();
                    temp = Convert.ToInt32(value) + 1;
                    dt.Rows[3][hourDic[hour]] = temp;
                    break;
                case MachineStatus.error_down:
                    value = dt.Rows[4][hourDic[hour]].ToString();
                    temp = Convert.ToInt32(value) + 1;
                    dt.Rows[4][hourDic[hour]] = temp;
                    break;

                default:
                    break;

            }
            CSVFileHelper.SaveCSV(dt, fullFileName);

        }



        public void StartRun()
        {
            if (runStatus == MachineStatus.idle && lastRunStatus == MachineStatus.engineering)
            {
                SetRunStatus(MachineStatus.engineering);
            }
            else if (runStatus == MachineStatus.idle && lastRunStatus == MachineStatus.running)
            {
                SetRunStatus(MachineStatus.running);
            }
            else if (runStatus == MachineStatus.error_down || runStatus == MachineStatus.planned_downtime)
            {
               // SetRunStatus(MachineStatus.running);//2023.08.07新增
            }
            else
            {
                //在planDT或DT时不改变状态
            }

        }
        /// <summary>
        /// 手动切换到Running
        /// </summary>
        public bool MalRun()
        {
            bool val = false;
            if (runStatus != MachineStatus.engineering)//if (runStatus != MachineStatus.idle)
            {
                SetRunStatus(MachineStatus.running);
                SetTime();
                val = true;
            }
            else
            {
                val = false;
            }
            return val;
        }
        /// <summary>
        /// 手动切换到Idle
        /// </summary>
        public bool MalIdle()
        {
            bool val = false;

            if (runStatus != MachineStatus.error_down)
            {
                SetRunStatus(MachineStatus.idle);
                val = true;
            }
            else
            {
                val = false;
            }
            return val;
        }
        /// <summary>
        /// 手动切换到DT
        /// </summary>
        public void MalDown()
        {
            if (runStatus != MachineStatus.planned_downtime)
            {
                SetRunStatus(MachineStatus.error_down);
            }
        }

        /// <summary>
        /// 强制切换到DT
        /// </summary>
        public void ForceDown()
        {
            SetRunStatus(MachineStatus.error_down);
        }
        /// <summary>
        /// 手动切换到Eng
        /// </summary>
        public bool MalEng()
        {
            SetRunStatus(MachineStatus.engineering);
            SetTime();
            return true;
        }
        /// <summary>
        /// 手动切换到PlanDT
        /// </summary>
        public bool MalPlan()
        {
            bool val = false;
            if (runStatus != MachineStatus.error_down)
            {
                SetRunStatus(MachineStatus.planned_downtime);
                val = true;
            }
            else
            {
                val = false;
            }
            return val;
        }

        public void StartWait()
        {
            SetRunStatus(MachineStatus.idle);
            //runStatus = MachineStatus.idle;
            //StatusTime = DateTime.Now;
        }

        public void StartError()
        {
            SetRunStatus(MachineStatus.error_down);
            //runStatus = MachineStatus.error_down;
            //StatusTime = DateTime.Now;
        }

        public void Engineering()//wsd
        {
            SetRunStatus(MachineStatus.engineering);
            //runStatus = MachineStatus.engineering;
            //StatusTime = DateTime.Now;
        }
        public void StartPlan_dt()//wsd
        {
            SetRunStatus(MachineStatus.planned_downtime);
            //runStatus = MachineStatus.planned_downtime;
            //StatusTime = DateTime.Now;
        }
        /// <summary>
        /// 加载所有文件
        /// </summary>
        private void UpdateCombox()
        { //加载所有文件
            string path = @"D:\DATA\运行时间";
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
                    Timepaths = paths;
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
                Timepaths = paths;
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
                calculationDate.Add(new CalculationTimeDate());
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
            //运行时间
            for (int j = 0; j < 24; j++)
            {
                dr[j] = 0;
            }
            dt.Rows.Add(dr);
            //报警时间
            dr = dt.NewRow();
            for (int j = 0; j < 24; j++)
            {
                dr[j] = 0;
            }
            dt.Rows.Add(dr);


            //wsd

            dr = dt.NewRow();
            //工程维修时间
            for (int j = 0; j < 24; j++)
            {
                dr[j] = 0;
            }

            dt.Rows.Add(dr);

            dr = dt.NewRow();
            //计划停机时间
            for (int j = 0; j < 24; j++)
            {
                dr[j] = 0;
            }

            dt.Rows.Add(dr);

            dr = dt.NewRow();
            //异常时间
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

        //private void ReadINI()
        //{
        //    string iniPath = Environment.CurrentDirectory + "\\userTimer.ini";
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
        //        string iniPath = Environment.CurrentDirectory + "\\userTimer.ini";
        //        //创建文件夹
        //        Common.Ini.CreateDirectoryEx(iniPath);
        //        Common.Ini.WritePrivateProfileString("setting", "SaveDays", SaveDays.ToString(), iniPath);
        //    }
        //    catch 
        //    {


        //    }


        //}

        private void delete()
        {
            while (true)
            {
                if (b_Close)
                {
                    break;
                }
                Common.FileManger.DeleteOverflowDicFile(@"D:\DATA", savedays);
                Thread.Sleep(1000);
            }
        }

        //分析数据
        private void bt_show_Click(object sender, EventArgs e)
        {
            try
            {
                string outputPath = @"D:\DATA\运行时间";
                string fileName = string.Format("{0}.csv", cbpath.Text);
                string fullFileName = Path.Combine(outputPath, fileName);
                string curDay = DateTime.Now.ToString("yyyy_MM_dd");
                bool isCurDay = false;//下拉列表所选日期是当前日期
                if (curDay == cbpath.Text)
                {
                    isCurDay = true;
                }
                else
                {
                    isCurDay = false;
                }
                if (File.Exists(fullFileName))
                {
                    dtshow = new DataTable();
                    dtshow = CSVFileHelper.OpenCSV(fullFileName);
                }
                ChangeDtToList(dtshow, isCurDay);
                //列表显示
                UpdateList(calculationDate, listView1, 1);
                UpdateList(calculationDate, listView2, 2);


                UpdatChart(calculationDate);

            }
            catch (Exception ex)
            {


            }
        }


        private void ChangeDtToList1(DataTable dt1)
        {

            for (int j = 0; j < 24; j++)
            {
                calculationDate[j].Name = dt1.Columns[j].ToString();
                //   calculationDate[j].Name1 = dt.Columns[j].ToString();
                int runtime1 = Convert.ToInt32(dt1.Rows[0][j].ToString());

                int waittime2 = Convert.ToInt32(dt1.Rows[1][j].ToString());


                int Planned_downtime = Convert.ToInt32(dt1.Rows[2][j].ToString());
                int Engineering = Convert.ToInt32(dt1.Rows[3][j].ToString());

                int errortime1 = Convert.ToInt32(dt1.Rows[4][j].ToString());

                if (runtime1 == 0 && errortime1 == 0 && waittime2 == 0)
                {
                    int waittime1 = 0;
                    calculationDate[j].RunTime = runtime1;
                    calculationDate[j].ErrorTime = errortime1;
                    //calculationDate[j].WaitTime = waittime1;
                    calculationDate[j].WaitTime = waittime1;

                    calculationDate[j].Planned_downtime = Planned_downtime;
                    calculationDate[j].Engineering = Engineering;

                    calculationDate[j].RunTime1 = Convert.ToDouble((runtime1 / 60.0).ToString("0.0"));
                    calculationDate[j].ErrorTime1 = Convert.ToDouble((errortime1 / 60.0).ToString("0.0"));
                    calculationDate[j].WaitTime1 = Convert.ToDouble((waittime1 / 60.0).ToString("0.0"));

                    calculationDate[j].Planned_downtime1 = Convert.ToDouble((Planned_downtime / 60.0).ToString("0.0"));
                    calculationDate[j].Engineering1 = Convert.ToDouble((Engineering / 60.0).ToString("0.0"));



                }
                else
                {
                    int waittime1 = 3600 - runtime1 - errortime1 - Planned_downtime - Engineering;
                    calculationDate[j].RunTime = runtime1;
                    calculationDate[j].ErrorTime = errortime1;
                    //calculationDate[j].WaitTime = waittime1;
                    calculationDate[j].WaitTime = 3600 - runtime1 - errortime1 - Planned_downtime - Engineering;

                    calculationDate[j].Planned_downtime = Planned_downtime;
                    calculationDate[j].Engineering = Engineering;


                    calculationDate[j].RunTime1 = Convert.ToDouble((runtime1 / 60.0).ToString("0.0"));
                    calculationDate[j].ErrorTime1 = Convert.ToDouble((errortime1 / 60.0).ToString("0.0"));
                    calculationDate[j].WaitTime1 = Convert.ToDouble((waittime1 / 60.0).ToString("0.0"));


                    calculationDate[j].Planned_downtime1 = Convert.ToDouble((Planned_downtime / 60.0).ToString("0.0"));
                    calculationDate[j].Engineering1 = Convert.ToDouble((Engineering / 60.0).ToString("0.0"));



                }


            }
        }//原版备份
        private void ChangeDtToList(DataTable dt1, bool isCurDay)
        {

            for (int j = 0; j < 24; j++)
            {
                calculationDate[j].Name = dt1.Columns[j].ToString();
                //   calculationDate[j].Name1 = dt.Columns[j].ToString();
                int runtime1 = Convert.ToInt32(dt1.Rows[0][j].ToString());
                int waittime1 = Convert.ToInt32(dt1.Rows[1][j].ToString());

                int Engineering = Convert.ToInt32(dt1.Rows[2][j].ToString());
                int Planned_downtime = Convert.ToInt32(dt1.Rows[3][j].ToString());
                int errortime1 = Convert.ToInt32(dt1.Rows[4][j].ToString());

                //方便理解，list,chart完全分开
                #region list
                {
                    calculationDate[j].RunTime = runtime1;
                    calculationDate[j].ErrorTime = errortime1;

                    calculationDate[j].Planned_downtime = Planned_downtime;
                    calculationDate[j].Engineering = Engineering;


                    //calculationDate[j].WaitTime = waittime1;
                    if (isCurDay)
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
                        if (k == j)//当前时间区间
                        {
                            if (waittime1 != 0)
                            {
                                calculationDate[j].WaitTime = curSecond - calculationDate[j].RunTime - calculationDate[j].ErrorTime - calculationDate[j].Planned_downtime - calculationDate[j].Engineering;
                            }
                        }
                        else if (k > j)//当前时间区间之前
                        {
                            calculationDate[j].WaitTime = 3600 - calculationDate[j].RunTime - calculationDate[j].ErrorTime - calculationDate[j].Planned_downtime - calculationDate[j].Engineering;
                        }
                        else//当前时间区间之后
                        {
                            calculationDate[j].WaitTime = 0;
                        }
                    }
                    else
                    {
                        calculationDate[j].WaitTime = 3600 - calculationDate[j].RunTime - calculationDate[j].ErrorTime - calculationDate[j].Planned_downtime - calculationDate[j].Engineering;
                    }
                }
                #endregion
                #region chart
                {
                    calculationDate[j].RunTime1 = Convert.ToDouble((runtime1 / 60.0).ToString("0.0"));
                    calculationDate[j].ErrorTime1 = Convert.ToDouble((errortime1 / 60.0).ToString("0.0"));

                    calculationDate[j].Planned_downtime1 = Convert.ToDouble((Planned_downtime / 60.0).ToString("0.0"));
                    calculationDate[j].Engineering1 = Convert.ToDouble((Engineering / 60.0).ToString("0.0"));


                    //calculationDate[j].WaitTime1 = Convert.ToDouble((waittime1 / 60.0).ToString("0.0"));
                    //double res = calculationDate[j].RunTime1 + calculationDate[j].ErrorTime1 + calculationDate[j].WaitTime1;
                    //if (res>60.0)
                    //{
                    //    calculationDate[j].WaitTime1 = 60.0 - calculationDate[j].RunTime1 - calculationDate[j].ErrorTime1;
                    //}
                    if (isCurDay)//当前天
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

                        double curMin = DateTime.Now.Minute + Convert.ToDouble((DateTime.Now.Second / 60.0).ToString("0.0"));
                        if (k == j)
                        {
                            if (waittime1 != 0)
                            {
                                calculationDate[j].WaitTime1 = Convert.ToDouble((curMin - calculationDate[j].RunTime1 - calculationDate[j].ErrorTime1 - calculationDate[j].Planned_downtime1 - calculationDate[j].Engineering1).ToString("0.0"));
                            }
                        }
                        else if (k > j)
                        {
                            calculationDate[j].WaitTime1 = Convert.ToDouble((60.0 - calculationDate[j].RunTime1 - calculationDate[j].ErrorTime1 - calculationDate[j].Planned_downtime1 - calculationDate[j].Engineering1).ToString("0.0"));
                        }
                        else
                        {
                            calculationDate[j].WaitTime1 = 0.0;
                        }
                    }
                    else//当前天以前
                    {
                        calculationDate[j].WaitTime1 = Convert.ToDouble((60.0 - calculationDate[j].RunTime1 - calculationDate[j].ErrorTime1 - calculationDate[j].Planned_downtime1 - calculationDate[j].Engineering1).ToString("0.0"));
                    }
                }
                #endregion 
            }
        }
        //刷新报表
        private void UpdateList(List<CalculationTimeDate> cal, ListView lv, int type)
        {
            int num = 0;
            int num1 = 12;

            int runtotal = 0;
            int waittotal = 0;
            int errortotal = 0;
            int Engineeringtotal = 0;
            int Plannedtotal = 0;

            string run = "";
            string wait = "";
            string error = "";
            string Engineering = "";
            string Planned = "";



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

                runtotal = runtotal + cal[j].RunTime;
                run = (cal[j].RunTime / 60).ToString() + ":" + (cal[j].RunTime % 60).ToString();
                li.SubItems.Add(run);

                waittotal = waittotal + cal[j].WaitTime;
                wait = (cal[j].WaitTime / 60).ToString() + ":" + (cal[j].WaitTime % 60).ToString();
                li.SubItems.Add(wait);



                Engineeringtotal = Engineeringtotal + cal[j].Engineering;
                Engineering = (cal[j].Engineering / 60).ToString() + ":" + (cal[j].Engineering % 60).ToString();
                li.SubItems.Add(Engineering);

                Plannedtotal = Plannedtotal + cal[j].Planned_downtime;
                Planned = (cal[j].Planned_downtime / 60).ToString() + ":" + (cal[j].Planned_downtime % 60).ToString();
                li.SubItems.Add(Planned);


                errortotal = errortotal + cal[j].ErrorTime;
                error = (cal[j].ErrorTime / 60).ToString() + ":" + (cal[j].ErrorTime % 60).ToString();
                li.SubItems.Add(error);

                lv.Items.Add(li);



            }


            li = new ListViewItem("合计");

            li.SubItems.Add((runtotal / 3600).ToString() + ":" + ((runtotal % 3600) / 60).ToString() + ":" + ((runtotal % 3600) % 60).ToString());
            li.SubItems.Add((waittotal / 3600).ToString() + ":" + ((waittotal % 3600) / 60).ToString() + ":" + ((waittotal % 3600) % 60).ToString());

            li.SubItems.Add((Engineeringtotal / 3600).ToString() + ":" + ((Engineeringtotal % 3600) / 60).ToString() + ":" + ((Engineeringtotal % 3600) % 60).ToString());
            li.SubItems.Add((Plannedtotal / 3600).ToString() + ":" + ((Plannedtotal % 3600) / 60).ToString() + ":" + ((Plannedtotal % 3600) % 60).ToString());
            li.SubItems.Add((errortotal / 3600).ToString() + ":" + ((errortotal % 3600) / 60).ToString() + ":" + ((errortotal % 3600) % 60).ToString());

            lv.Items.Add(li);

            li = new ListViewItem("稼动率");
            if ((runtotal + waittotal + errortotal + Engineeringtotal + Plannedtotal) > 0)
                li.SubItems.Add(((runtotal + waittotal) / ((runtotal + waittotal + errortotal + Engineeringtotal + Plannedtotal) * 1.0) * 100.0).ToString("0.00") + "%");
            else
                li.SubItems.Add(0.ToString());

            lv.Items.Add(li);
            lv.Refresh();
        }

        //刷新柱状图
        private void UpdatChart(List<CalculationTimeDate> cal)
        {
            chart1.DataSource = cal;

            chart1.Series["运行时间"].XValueMember = "Name";
            chart1.Series["运行时间"].YValueMembers = "RunTime1";
            chart1.Series["运行时间"].Label = "#VAL";
            chart1.Series["运行时间"].IsValueShownAsLabel = true;
            chart1.Series["运行时间"].CustomProperties = "LabelStyle=Top";

            chart1.Series["待料时间"].XValueMember = "Name";
            chart1.Series["待料时间"].YValueMembers = "WaitTime1";
            chart1.Series["待料时间"].Label = "#VAL";
            chart1.Series["待料时间"].IsValueShownAsLabel = true;
            chart1.Series["待料时间"].CustomProperties = "LabelStyle=Top";

            chart1.Series["异常时间"].XValueMember = "Name";
            chart1.Series["异常时间"].YValueMembers = "ErrorTime1";
            chart1.Series["异常时间"].Label = "#VAL";
            chart1.Series["异常时间"].IsValueShownAsLabel = true;
            chart1.Series["异常时间"].CustomProperties = "LabelStyle=Top";


            chart1.Series["维修时间"].XValueMember = "Name";
            chart1.Series["维修时间"].YValueMembers = "Engineering1";
            chart1.Series["维修时间"].Label = "#VAL";
            chart1.Series["维修时间"].IsValueShownAsLabel = true;
            chart1.Series["维修时间"].CustomProperties = "LabelStyle=Top";


            chart1.Series["计划停机"].XValueMember = "Name";
            chart1.Series["计划停机"].YValueMembers = "Planned_downtime1";
            chart1.Series["计划停机"].Label = "#VAL";
            chart1.Series["计划停机"].IsValueShownAsLabel = true;
            chart1.Series["计划停机"].CustomProperties = "LabelStyle=Top";

        }

        private void UpdatChart(List<CalculationdayDate> cal)
        {
            chart2.DataSource = cal;

            chart2.Series["运行时间"].XValueMember = "Name";
            chart2.Series["运行时间"].YValueMembers = "RunTime";
            chart2.Series["运行时间"].Label = "#VAL";
            chart2.Series["运行时间"].IsValueShownAsLabel = true;
            chart2.Series["运行时间"].CustomProperties = "LabelStyle=Top";

            chart2.Series["待料时间"].XValueMember = "Name";
            chart2.Series["待料时间"].YValueMembers = "WaitTime";
            chart2.Series["待料时间"].Label = "#VAL";
            chart2.Series["待料时间"].IsValueShownAsLabel = true;
            chart2.Series["待料时间"].CustomProperties = "LabelStyle=Top";

            chart2.Series["异常时间"].XValueMember = "Name";
            chart2.Series["异常时间"].YValueMembers = "ErrorTime";
            chart2.Series["异常时间"].Label = "#VAL";
            chart2.Series["异常时间"].IsValueShownAsLabel = true;
            chart2.Series["异常时间"].CustomProperties = "LabelStyle=Top";



            chart2.Series["维修时间"].XValueMember = "Name";
            chart2.Series["维修时间"].YValueMembers = "Engineeringtime";
            chart2.Series["维修时间"].Label = "#VAL";
            chart2.Series["维修时间"].IsValueShownAsLabel = true;
            chart2.Series["维修时间"].CustomProperties = "LabelStyle=Top";


            chart2.Series["计划停机"].XValueMember = "Name";
            chart2.Series["计划停机"].YValueMembers = "Plannedtime";
            chart2.Series["计划停机"].Label = "#VAL";
            chart2.Series["计划停机"].IsValueShownAsLabel = true;
            chart2.Series["计划停机"].CustomProperties = "LabelStyle=Top";

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

            radioButton1.Checked = true;
            tabPage1.Parent = null;
            tabPage2.Parent = null;
            tabPage1.Parent = tabControl1;
            locker = false;
        }

        public enum MachineStatus
        {
            running,
            idle,
            engineering,
            planned_downtime,
            error_down

        }
        #region 数据保存类

        public class CSVFileHelper
        {

            private static object lockerwrite = new object();


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
                        fs.Flush();
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



        #endregion 数据保存类
        Dictionary<string, List<int>> uphdic = new Dictionary<string, List<int>>();

        public MachineStatus RunStatus
        {
            get
            {
                return runStatus;
            }
        }
        public MachineStatus LastRunStatus
        {
            get
            {
                return lastRunStatus;
            }
        }
        public MachineStatus MSignalTowerstatus
        {
            set
            {
                towerStatus = value;
            }
            get
            {
                return towerStatus;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string a = @"D:\DATA\运行时间\" + comboBox1.Text + ".csv";
            string b = @"D:\DATA\运行时间\" + comboBox2.Text + ".csv";
            DataTable dtshow;
            //----------------
            string curDay = DateTime.Now.ToString("yyyy_MM_dd");
            bool isCurDay = false;//下拉列表所选日期是当前日期
            uphdic = new Dictionary<string, List<int>>();
            calculationdayDate = new List<CalculationdayDate>();

            Task.Run(new Action(() =>
            {
                try
                {
                    //button1.BeginInvoke(new Action(() =>
                    //{
                    //    button1.Enabled = false;
                    //}));
                    int index = Timepaths.ToList().IndexOf(a);
                    int index2 = Timepaths.ToList().IndexOf(b);
                    int i = 0;
                    int runtime1 = 0;
                    int waittime1 = 0;
                    int errortime1 = 0;
                    int engineeringtime1 = 0;
                    int plannedtime1 = 0;


                    if (index < index2)
                    {
                        return;
                    }
                    //if (index <= index2)
                    {
                        dtshow = new DataTable();
                        foreach (var item in Timepaths)
                        {
                            string prj = Path.GetFileNameWithoutExtension(item);
                            if (curDay == prj)
                            {
                                isCurDay = true;
                            }
                            else
                            {
                                isCurDay = false;
                            }
                            if (i >= index2 && i <= index)
                            {
                                dtshow = CSVFileHelper.OpenCSV(item);
                                runtime1 = 0;
                                waittime1 = 0;
                                errortime1 = 0;
                                engineeringtime1 = 0;
                                plannedtime1 = 0;

                                //解析数据
                                List<int> dt = new List<int>();
                                for (int j = 0; j < dtshow.Columns.Count; j++)
                                {
                                    runtime1 = runtime1 + Convert.ToInt32(dtshow.Rows[0][j].ToString());

                                    engineeringtime1 = engineeringtime1 + Convert.ToInt32(dtshow.Rows[2][j].ToString());
                                    plannedtime1 = plannedtime1 + Convert.ToInt32(dtshow.Rows[3][j].ToString());

                                    errortime1 = errortime1 + Convert.ToInt32(dtshow.Rows[4][j].ToString());


                                    int waittimeTmp = 0;

                                    if (isCurDay)
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
                                        if (k == j)//当前时间区间
                                        {
                                            if (waittime1 != 0)
                                            {
                                                waittimeTmp = curSecond - Convert.ToInt32(dtshow.Rows[0][j].ToString()) - Convert.ToInt32(dtshow.Rows[2][j].ToString()) - Convert.ToInt32(dtshow.Rows[3][j].ToString()) - Convert.ToInt32(dtshow.Rows[4][j].ToString());
                                            }
                                        }
                                        else if (k > j)//当前时间区间之前
                                        {
                                            waittimeTmp = 3600 - Convert.ToInt32(dtshow.Rows[0][j].ToString()) - Convert.ToInt32(dtshow.Rows[2][j].ToString()) - Convert.ToInt32(dtshow.Rows[3][j].ToString()) - Convert.ToInt32(dtshow.Rows[4][j].ToString());
                                        }
                                        else//当前时间区间之后
                                        {
                                            waittimeTmp = 0;
                                        }
                                    }
                                    else
                                    {
                                        waittimeTmp = 3600 - Convert.ToInt32(dtshow.Rows[0][j].ToString()) - Convert.ToInt32(dtshow.Rows[2][j].ToString()) - Convert.ToInt32(dtshow.Rows[3][j].ToString()) - Convert.ToInt32(dtshow.Rows[4][j].ToString());
                                    }
                                    waittime1 = waittime1 + waittimeTmp;
                                }
                                dt.Add(runtime1);
                                dt.Add(waittime1);

                                dt.Add(engineeringtime1);
                                dt.Add(plannedtime1);
                                dt.Add(errortime1);

                                //增加时间
                                uphdic.Add(prj, dt);
                            }
                            i++;
                        }
                    }
                    #region 屏蔽
                    //if (index > index2)
                    //{
                    //    dtshow = new DataTable();
                    //    foreach (var item in Timepaths)
                    //    {
                    //        string prj = Path.GetFileNameWithoutExtension(item);

                    //        if (i <= index && i >= index2)
                    //        {
                    //            dtshow = CSVFileHelper.OpenCSV(item);
                    //            runtime1 = 0;
                    //            waittime1 = 0;
                    //            errortime1 = 0;
                    //            //解析数据
                    //            List<int> dt = new List<int>();
                    //            for (int j = 0; j < dtshow.Columns.Count; j++)
                    //            {
                    //                runtime1 = runtime1 + Convert.ToInt32(dtshow.Rows[0][j].ToString());
                    //                waittime1 = waittime1 + Convert.ToInt32(dtshow.Rows[1][j].ToString());
                    //                errortime1 = errortime1 + Convert.ToInt32(dtshow.Rows[2][j].ToString());
                    //            }
                    //            dt.Add(runtime1);
                    //            dt.Add(waittime1);
                    //            dt.Add(errortime1);
                    //            //增加时间
                    //            uphdic.Add(prj, dt);
                    //        }
                    //        i++;
                    //    }
                    //}
                    //i = 0;
                    //if (index <= index2)
                    //{
                    //    dtshow = new DataTable();
                    //    foreach (var item in Timepaths)
                    //    {
                    //        string prj = Path.GetFileNameWithoutExtension(item);
                    //        if (i >= index && i <= index2)
                    //        {
                    //            dtshow = CSVFileHelper.OpenCSV(item);
                    //            runtime1 = 0;
                    //            waittime1 = 0;
                    //            errortime1 = 0;
                    //            //解析数据
                    //            List<int> dt = new List<int>();
                    //            for (int j = 0; j < dtshow.Columns.Count; j++)
                    //            {
                    //                runtime1 = runtime1 + Convert.ToInt32(dtshow.Rows[0][j].ToString());
                    //                waittime1 = waittime1 + Convert.ToInt32(dtshow.Rows[1][j].ToString());
                    //                errortime1 = errortime1 + Convert.ToInt32(dtshow.Rows[2][j].ToString());

                    //            }
                    //            dt.Add(runtime1);
                    //            dt.Add(waittime1);
                    //            dt.Add(errortime1);
                    //            //增加时间
                    //            uphdic.Add(prj, dt);
                    //        }
                    //        i++;
                    //    }
                    //}
                    //i = 0;
                    //if (index == index2)
                    //{
                    //    dtshow = new DataTable();
                    //    foreach (var item in Timepaths)
                    //    {
                    //        string prj = Path.GetFileNameWithoutExtension(item);
                    //        if (i == index)
                    //        {
                    //            dtshow = CSVFileHelper.OpenCSV(item);
                    //            runtime1 = 0;
                    //            waittime1 = 0;
                    //            errortime1 = 0;
                    //            List<int> dt = new List<int>();
                    //            for (int j = 0; j < dtshow.Columns.Count; j++)
                    //            {
                    //                runtime1 = runtime1 + Convert.ToInt32(dtshow.Rows[0][j].ToString());
                    //                errortime1 = errortime1 + Convert.ToInt32(dtshow.Rows[2][j].ToString());
                    //                //waittime1 = waittime1 + Convert.ToInt32(dtshow.Rows[1][j].ToString());
                    //                waittime1 = waittime1 + Convert.ToInt32(dtshow.Rows[1][j].ToString());
                    //            }
                    //            dt.Add(runtime1);
                    //            dt.Add(waittime1);
                    //            dt.Add(errortime1);
                    //            //增加时间
                    //            uphdic.Add(prj, dt);
                    //        }
                    //        i++;
                    //    }
                    //}
                    #endregion
                    listView1.BeginInvoke(new Action(() =>
                    {

                        listView3.Items.Clear();
                        int runt1 = 0;
                        int wait1 = 0;
                        int error1 = 0;

                        foreach (var item in uphdic.Keys)
                        {

                            ListViewItem li = new ListViewItem(item);
                            CalculationdayDate daytemp = new CalculationdayDate();

                            foreach (var item1 in uphdic[item])
                            {
                                li.SubItems.Add((item1 / 3600).ToString() + ":" + ((item1 % 3600) / 60).ToString() + ":" + ((item1 % 3600) % 60).ToString());
                            }
                            int total = uphdic[item][0] + uphdic[item][1] + uphdic[item][2] + uphdic[item][3] + uphdic[item][4];
                            int run = uphdic[item][0] + uphdic[item][1];
                            runt1 = runt1 + uphdic[item][0];
                            wait1 = wait1 + uphdic[item][1];
                            engineeringtime1 = engineeringtime1 + uphdic[item][2];
                            plannedtime1 = plannedtime1 + uphdic[item][3];
                            error1 = error1 + uphdic[item][4];


                            if (total > 0)
                            {
                                double value = (run * 100.0 / total);
                                li.SubItems.Add(value.ToString("0.00") + "%");
                            }
                            else
                            { li.SubItems.Add(("100%")); }
                            listView3.Items.Add(li);
                            daytemp.Name = item;
                            daytemp.RunTime = (uphdic[item][0] / 3600.0).ToString("0.0");
                            daytemp.WaitTime = (uphdic[item][1] / 3600.0).ToString("0.0");

                            daytemp.Engineeringtime = (uphdic[item][2] / 3600.0).ToString("0.0");
                            daytemp.Plannedtime = (uphdic[item][3] / 3600.0).ToString("0.0");

                            daytemp.ErrorTime = (uphdic[item][4] / 3600.0).ToString("0.0");



                            calculationdayDate.Add(daytemp);
                        }
                        ListViewItem li2 = new ListViewItem("合计");
                        li2.SubItems.Add((runt1 / 3600).ToString() + ":" + ((runt1 % 3600) / 60).ToString() + ":" + ((runt1 % 3600) % 60).ToString());
                        li2.SubItems.Add((wait1 / 3600).ToString() + ":" + ((wait1 % 3600) / 60).ToString() + ":" + ((wait1 % 3600) % 60).ToString());
                        li2.SubItems.Add((engineeringtime1 / 3600).ToString() + ":" + ((engineeringtime1 % 3600) / 60).ToString() + ":" + ((engineeringtime1 % 3600) % 60).ToString());
                        li2.SubItems.Add((plannedtime1 / 3600).ToString() + ":" + ((plannedtime1 % 3600) / 60).ToString() + ":" + ((plannedtime1 % 3600) % 60).ToString());




                        li2.SubItems.Add((error1 / 3600).ToString() + ":" + ((error1 % 3600) / 60).ToString() + ":" + ((error1 % 3600) % 60).ToString());




                        int totalall = runt1 + wait1 + error1 + engineeringtime1 + plannedtime1;
                        int runall = runt1 + wait1;
                        if (totalall > 0)
                        {
                            double value = (runall * 100.0 / totalall);
                            li2.SubItems.Add(value.ToString("0.00") + "%");
                        }
                        else
                        { li2.SubItems.Add(("100%")); }

                        listView3.Items.Add(li2);
                        UpdatChart(calculationdayDate);
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




    }

    public class CalculationTimeDate
    {


        private string _name = "";
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
            }

        }
        private int runtime = 0;
        public int RunTime
        {
            get { return runtime; }
            set
            {
                runtime = value;
            }

        }

        private Double runtime1 = 0;
        public Double RunTime1
        {
            get { return runtime1; }
            set
            {
                runtime1 = value;
            }

        }
        private int waittime = 0;
        public int WaitTime
        {
            get { return waittime; }
            set
            {
                waittime = value;
            }

        }

        private Double waittime1 = 0;
        public Double WaitTime1
        {
            get { return waittime1; }
            set
            {
                waittime1 = value;
            }

        }

        private int errortime = 0;
        public int ErrorTime
        {
            get { return errortime; }
            set
            {
                errortime = value;
            }

        }

        private Double errortime1 = 0;
        public Double ErrorTime1
        {
            get { return errortime1; }
            set
            {
                errortime1 = value;
            }

        }


        private int engineeringtime = 0;
        public int Engineering
        {
            get { return engineeringtime; }
            set
            {
                engineeringtime = value;
            }

        }

        private Double engineeringtime1 = 0;
        public Double Engineering1
        {
            get { return engineeringtime1; }
            set
            {
                engineeringtime1 = value;
            }

        }


        private int planned_downtime = 0;
        public int Planned_downtime
        {
            get { return planned_downtime; }
            set
            {
                planned_downtime = value;
            }

        }

        private Double planned_downtime1 = 0;
        public Double Planned_downtime1
        {
            get { return planned_downtime1; }
            set
            {
                planned_downtime1 = value;
            }

        }


        


    }

    public class CalculationdayDate
    {


        private string _name = "";
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
            }

        }
        private string runtime = "0";
        public string RunTime
        {
            get { return runtime; }
            set
            {
                runtime = value;
            }

        }


        private string waittime = "0";
        public string WaitTime
        {
            get { return waittime; }
            set
            {
                waittime = value;
            }

        }


        private string engineeringtime = "0";
        public string Engineeringtime
        {
            get { return engineeringtime; }
            set
            {
                engineeringtime = value;
            }

        }


        private string plannedtime = "0";
        public string Plannedtime
        {
            get { return plannedtime; }
            set
            {
                plannedtime = value;
            }

        }

        private string errortime = "0";
        public string ErrorTime
        {
            get { return errortime; }
            set
            {
                errortime = value;
            }

        }

        
    }
}
