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
using System.Threading;
using System.Diagnostics;

namespace Chart
{
    public partial class CTUnit : UserControl
    {
        Thread th;
        private Stopwatch sw = new Stopwatch();
        private string dtstart;
        private string dtend;
        private string Snleft = "";
        private string Snright = "";
        private Stopwatch sw1 = new Stopwatch();
        private string dtstart1;
        private string dtend1;
        private object locker1 = new object();
        private object locker2 = new object();

        List<double> ctlist = new List<double>();

        List<CalculationDate> datelist = new List<CalculationDate>();

        List<CTDate> Ctdate = new List<CTDate>();
        Dictionary<string, CTDate> Ctdatedic = new Dictionary<string, CTDate>();
        #region CT
        public CTEventArgs CTEventArgs_Left = new CTEventArgs();
        public CTEventArgs CTEventArgs_Right = new CTEventArgs();
        public static string CT_new = "0";
        #endregion
        /// <summary>
        /// 
        /// </summary>
        string[] Errorpaths;
        private int savedyas;
        private int gantryCount = 2;
        /// <summary>
        /// 数据保存的天数
        /// </summary>
        public CTUnit(int days,int gantry)
        {
            gantryCount = gantry;
            InitializeComponent();
            savedyas = days;
            //th = new Thread(delete);
            //th.IsBackground = true;
            //th.Start();
            dataGridView1.Rows.Clear();
            Task.Run(new Action(new Action(action1)));
            ctlist.Clear();
            for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            {
                this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                //   this.dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }

        }


        private void Initdate()
        {
            Ctdate = new List<CTDate>();
            Ctdate.Add(new CTDate() { CTTotal = 0, Total = 0, Date = "8:00-9:00" });
            Ctdate.Add(new CTDate() { CTTotal = 0, Total = 0, Date = "9:00-10:00" });
            Ctdate.Add(new CTDate() { CTTotal = 0, Total = 0, Date = "10:00-11:00" });
            Ctdate.Add(new CTDate() { CTTotal = 0, Total = 0, Date = "11:00-12:00" });
            Ctdate.Add(new CTDate() { CTTotal = 0, Total = 0, Date = "12:00-13:00" });
            Ctdate.Add(new CTDate() { CTTotal = 0, Total = 0, Date = "13:00-14:00" });
            Ctdate.Add(new CTDate() { CTTotal = 0, Total = 0, Date = "14:00-15:00" });
            Ctdate.Add(new CTDate() { CTTotal = 0, Total = 0, Date = "15:00-16:00" });
            Ctdate.Add(new CTDate() { CTTotal = 0, Total = 0, Date = "16:00-17:00" });
            Ctdate.Add(new CTDate() { CTTotal = 0, Total = 0, Date = "17:00-18:00" });
            Ctdate.Add(new CTDate() { CTTotal = 0, Total = 0, Date = "18:00-19:00" });
            Ctdate.Add(new CTDate() { CTTotal = 0, Total = 0, Date = "19:00-20:00" });
            Ctdate.Add(new CTDate() { CTTotal = 0, Total = 0, Date = "20:00-21:00" });
            Ctdate.Add(new CTDate() { CTTotal = 0, Total = 0, Date = "21:00-22:00" });

            Ctdate.Add(new CTDate() { CTTotal = 0, Total = 0, Date = "22:00-23:00" });
            Ctdate.Add(new CTDate() { CTTotal = 0, Total = 0, Date = "23:00-0:00" });
            Ctdate.Add(new CTDate() { CTTotal = 0, Total = 0, Date = "0:00-1:00" });
            Ctdate.Add(new CTDate() { CTTotal = 0, Total = 0, Date = "1:00-2:00" });
            Ctdate.Add(new CTDate() { CTTotal = 0, Total = 0, Date = "2:00-3:00" });
            Ctdate.Add(new CTDate() { CTTotal = 0, Total = 0, Date = "3:00-4:00" });
            Ctdate.Add(new CTDate() { CTTotal = 0, Total = 0, Date = "4:00-5:00" });
            Ctdate.Add(new CTDate() { CTTotal = 0, Total = 0, Date = "5:00-6:00" });
            Ctdate.Add(new CTDate() { CTTotal = 0, Total = 0, Date = "6:00-7:00" });
            Ctdate.Add(new CTDate() { CTTotal = 0, Total = 0, Date = "7:00-8:00" });
            Ctdatedic.Clear();
            Ctdatedic.Add("08", Ctdate[0]);
            Ctdatedic.Add("09", Ctdate[1]);
            Ctdatedic.Add("10", Ctdate[2]);
            Ctdatedic.Add("11", Ctdate[3]);
            Ctdatedic.Add("12", Ctdate[4]);
            Ctdatedic.Add("13", Ctdate[5]);
            Ctdatedic.Add("14", Ctdate[6]);
            Ctdatedic.Add("15", Ctdate[7]);
            Ctdatedic.Add("16", Ctdate[8]);
            Ctdatedic.Add("17", Ctdate[9]);
            Ctdatedic.Add("18", Ctdate[10]);
            Ctdatedic.Add("19", Ctdate[11]);
            Ctdatedic.Add("20", Ctdate[12]);
            Ctdatedic.Add("21", Ctdate[13]);
            Ctdatedic.Add("22", Ctdate[14]);
            Ctdatedic.Add("23", Ctdate[15]);
            Ctdatedic.Add("00", Ctdate[16]);
            Ctdatedic.Add("01", Ctdate[17]);
            Ctdatedic.Add("02", Ctdate[18]);
            Ctdatedic.Add("03", Ctdate[19]);
            Ctdatedic.Add("04", Ctdate[20]);
            Ctdatedic.Add("05", Ctdate[21]);
            Ctdatedic.Add("06", Ctdate[22]);
            Ctdatedic.Add("07", Ctdate[23]);
        }

        /// <summary>
        /// 开始作料
        /// </summary>
        /// <param name="massage"></param>
        public void StartDoLeft(string sn)
        {
            try
            {
                sw.Restart();
                dtstart = DateTime.Now.ToString("HH:mm:ss");
                if (sn != null)
                    Snleft = sn;
                else
                    Snleft = "";
            }
            catch
            {


            }

        }
        /// <summary>
        /// 继续CT计时
        /// </summary>
        /// <param name="sn"></param>
        public void ContinueLeftCT(string sn)
        {
            sw.Start();
        }
        /// <summary>
        /// 暂停CT计时
        /// </summary>
        /// <param name="sn"></param>
        public void PauseLeftCT(string sn)
        {
            sw.Stop();
        }
        /// <summary>
        /// 结束作料
        /// </summary>
        /// <param name="massage"></param>
        public void EndDoLeft(String Snleft1 )
        {
            try
            {
                //if (!sw.IsRunning)
                //    return;
                sw.Stop();
                OutputInfo(Snleft1);
            }
            catch
            {


            }

        }
        /// <summary>
        /// 结束作料
        /// </summary>
        /// <param name="massage"></param>
        public void EndDoLeft(String Snleft1,string CT,string  startime, string endtime)
        {
            try
            {
                //if (!sw.IsRunning)
                //    return;
                sw.Stop();
                OutputInfo(Snleft1,CT, startime, endtime);
            }
            catch
            {


            }

        }

        /// <summary>
        /// 开始作料
        /// </summary>
        /// <param name="massage"></param>
        public void StartDoRight(string sn)
        {
            try
            {
                sw1.Restart();
                dtstart1 = DateTime.Now.ToString("HH:mm:ss");
                if (sn != null)
                    Snright = sn;
                else
                    Snright = "";
            }
            catch
            {


            }

        }
        /// <summary>
        /// 继续CT计时
        /// </summary>
        /// <param name="sn"></param>
        public void ContinueRightCT(string sn)
        {
            sw1.Start();
        }
        /// <summary>
        /// 暂停CT计时
        /// </summary>
        /// <param name="sn"></param>
        public void PauseRightCT(string sn)
        {
            sw1.Stop();
        }
        /// <summary>
        /// 结束作料
        /// </summary>
        /// <param name="massage"></param>
        public void EndDoRight(string Snright1)
        {
            try
            {
                //if (!sw1.IsRunning)
                //    return;
                sw1.Stop();
                OutputInfo1(Snright1);
            }
            catch
            {


            }

        }
        /// <summary>
        /// 结束作料
        /// </summary>
        /// <param name="massage"></param>
        public void EndDoRight(string Snright1,string CT)
        {
            try
            {
                //if (!sw1.IsRunning)
                //    return;
                sw1.Stop();
                OutputInfo1(Snright1,CT);
            }
            catch
            {


            }

        }
        private void OutputInfo(string sn)
        {
            if (sn == "" || sn == "12345678askghj")
            {
                return;
            }
            dtend = DateTime.Now.ToString("HH:mm:ss");
            string date = DateTime.Now.ToString("yyyy/MM/dd");
            int hour = DateTime.Now.Hour;
            CTEventArgs args = new CTEventArgs() { DateTime = date, Time = dtend, SN = sn, Start = dtstart, End = dtend, Ct = (sw.ElapsedMilliseconds / 1000.0).ToString("0.0"), Dir = "Left" };
            saveError(args.DateTime + "," + args.Time + "," + args.SN + "," + args.Start + "," + args.End + "," + args.Ct + "," + args.Dir+","+CT_new);
            saveErrorcsv(args.DateTime + "," + args.Time + "," + args.SN + "," + args.Start + "," + args.End + "," + args.Ct + "," + args.Dir + "," + CT_new);
            Update(args, dataGridView1);
            Add(args);
            // saveError(message);
            //saveErrorcsv(message1);
            CTEventArgs_Left = args;
        }
        private void OutputInfo(string sn,string CT, string startime, string endtime)//传入CT的原因是CTbridge与真实CT统计右差异
        {
            if (sn==""||sn== "12345678askghj")
            {
                return;
            }
            dtend = endtime;//DateTime.Now.ToString("HH:mm:ss");
            dtstart = startime;
            string date = DateTime.Now.ToString("yyyy/MM/dd");
            int hour = DateTime.Now.Hour;
            //  CTEventArgs args = new CTEventArgs() { DateTime = date, Time = dtend, SN = sn, Start = dtstart, End = dtend, Ct = (sw.ElapsedMilliseconds / 1000.0).ToString("0.0"), Dir = "Left" };
            CTEventArgs args = new CTEventArgs() { DateTime = date, Time = dtend, SN = sn, Start = dtstart, End = dtend, Ct =CT, Dir = "Left" };
            saveError(args.DateTime + "," + args.Time + "," + args.SN + "," + args.Start + "," + args.End + "," + args.Ct + "," + args.Dir+","+CT_new);
            saveErrorcsv(args.DateTime + "," + args.Time + "," + args.SN + "," + args.Start + "," + args.End + "," + args.Ct + "," + args.Dir + "," + CT_new);
            Update(args, dataGridView1);
            Add(args);
            // saveError(message);
            //saveErrorcsv(message1);
            CTEventArgs_Left = args;
        }
        private void OutputInfo1(string sn)
        {
            if (sn == "" || sn == "12345678askghj")
            {
                return;
            }
            dtend1 = DateTime.Now.ToString("HH:mm:ss");
            string date = DateTime.Now.ToString("yyyy/MM/dd");
            int hour = DateTime.Now.Hour;

            CTEventArgs args = new CTEventArgs() { DateTime = date, Time = dtend1, SN = sn, Start = dtstart1, End = dtend1, Ct = (sw1.ElapsedMilliseconds / 1000.0).ToString("0.0"), Dir = "Right" };

            saveError(args.DateTime + "," + args.Time + "," + args.SN + "," + args.Start + "," + args.End + "," + args.Ct + "," + args.Dir+","+CT_new);
            saveErrorcsv(args.DateTime + "," + args.Time + "," + args.SN + "," + args.Start + "," + args.End + "," + args.Ct + "," + args.Dir + "," + CT_new);

            Update(args, dataGridView1);
            Add(args);
            // saveError(message);
            //saveErrorcsv(message1);
            CTEventArgs_Right = args;
        }
        private void OutputInfo1(string sn, string CT)
        {
            if (sn == "" || sn == "12345678askghj")
            {
                return;
            }
            dtend1 = DateTime.Now.ToString("HH:mm:ss");
            string date = DateTime.Now.ToString("yyyy/MM/dd");
            int hour = DateTime.Now.Hour;
            CTEventArgs args = new CTEventArgs() { DateTime = date, Time = dtend1, SN = sn, Start = dtstart1, End = dtend1, Ct = CT, Dir = "Right" };
            saveError(args.DateTime + "," + args.Time + "," + args.SN + "," + args.Start + "," + args.End + "," + args.Ct + "," + args.Dir+","+CT_new);
            saveErrorcsv(args.DateTime + "," + args.Time + "," + args.SN + "," + args.Start + "," + args.End + "," + args.Ct + "," + args.Dir + "," + CT_new);

            Update(args, dataGridView1);
            Add(args);
            // saveError(message);
            //saveErrorcsv(message1);
            CTEventArgs_Right = args;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="args"></param>
        /// <param name="dg"></param>
        public void Update(CTEventArgs args, DataGridView dg)
        {
            try
            {
                if (dg.InvokeRequired)
                {
                    dg.BeginInvoke(new Action(() => Update(args, dg)));
                }
                else
                {

                    { dg.Rows.Add(args.DateTime, args.Time, args.SN, args.Start, args.End, args.Ct, args.Dir); }

                }
            }
            catch
            {
            }

        }

        /// <summary>
        /// 清空数据
        /// </summary>
        /// <param name="args"></param>
        /// <param name="dg"></param>
        public void ClearDate(DataGridView dg)
        {
            try
            {
                if (dg.InvokeRequired)
                {
                    dg.BeginInvoke(new Action(() => { dg.Rows.Clear(); }));
                }
                else
                {

                    dg.Rows.Clear();

                }
            }
            catch
            {
            }

        }


        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="args"></param>
        private void Add(CTEventArgs args)
        {
            try
            {
                if (dataGridView1.InvokeRequired)
                {
                    dataGridView1.BeginInvoke(new Action(() => Add(args)));
                }
                else
                {
                    if (dataGridView1.Rows.Count > 25000)
                        dataGridView1.Rows.Remove(dataGridView1.Rows[0]);


                    dataGridView1.Rows[dataGridView1.Rows.Count - 2].Selected = true;
                    if (dataGridView1.IsHandleCreated)
                    {
                        //if (isupdate)
                        dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 2;
                    }
                    ctlist.Add(Convert.ToDouble(args.Ct));

                    if (textBox1.InvokeRequired)
                    {
                        textBox1.BeginInvoke(new Action(() =>
                        {
                          //textBox1.Text = ctlist.Average().ToString("0.0");
                          textBox1.Text = (ctlist.Average() / 2).ToString("0.0");

                        }));
                    }
                    else
                    {
                      //textBox1.Text = ctlist.Average().ToString("0.0");
                      textBox1.Text = (ctlist.Average() / 2).ToString("0.0");
                    }
                }
            }
            catch
            {


            }
        }

        private void delete()
        {
            while (true)
            {
                //if (Common.FileManger.DeleteOverflowFile(@"D:\DATA\CT", savedyas))
                //    UpdateCombox();
                Thread.Sleep(500);

                Thread.Sleep(500);
            }
        }

        private void saveError(string message)
        {
            lock (locker1)
            {
                try
                {
                    string fileName;
                    int hour = DateTime.Now.Hour;
                    if (hour > 7)
                        fileName = string.Format("{0}.txt", DateTime.Now.ToString("yyyy_MM_dd"));
                    else
                        fileName = string.Format("{0}.txt", DateTime.Now.AddDays(-1).ToString("yyyy_MM_dd"));
                    string outputPath = @"D:\DATA\CT";
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
                        if (hour > 7)
                            ClearDate(dataGridView1);
                        fs = new System.IO.FileStream(fullFileName, System.IO.FileMode.Append, System.IO.FileAccess.Write, FileShare.Read);
                        //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
                        sw = new StreamWriter(fs, System.Text.Encoding.Default);
                        sw.WriteLine(message);
                        sw.Close();
                        fs.Close();
                        UpdateCombox();

                    }
                    else
                    {
                        fs = new System.IO.FileStream(fullFileName, System.IO.FileMode.Append, System.IO.FileAccess.Write, FileShare.Read);
                        //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
                        sw = new StreamWriter(fs, System.Text.Encoding.Default);
                        sw.WriteLine(message);
                        sw.Close();
                        fs.Close();
                    }

                }
                catch
                {


                }
            }
        }
        private void saveErrorcsv(string message)
        {
            lock (locker1)
            {
                try
                {
                    string fileName;
                    int hour = DateTime.Now.Hour;
                    if (hour > 7)
                        fileName = string.Format("{0}.csv", DateTime.Now.ToString("yyyy_MM_dd"));
                    else
                        fileName = string.Format("{0}.csv", DateTime.Now.AddDays(-1).ToString("yyyy_MM_dd"));
                    string outputPath = @"D:\DATA\CT_Excel";
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
                        sw.WriteLine("Date,Time,SN,StartTime,EndTime,CT,Dir,HiveCT");
                        sw.WriteLine(message);
                        sw.Close();
                        fs.Close();

                    }
                    else
                    {
                        fs = new System.IO.FileStream(fullFileName, System.IO.FileMode.Append, System.IO.FileAccess.Write, FileShare.Read);
                        //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
                        sw = new StreamWriter(fs, System.Text.Encoding.Default);
                        sw.WriteLine(message);
                        sw.Close();
                        fs.Close();
                    }

                }
                catch
                {


                }
            }
        }

        
       

        /// <summary>
        /// 加载数据
        /// </summary>
        private void action1()
        {
            string text = "";
            try
            {
                string fileName;
                int hour = DateTime.Now.Hour;
                if (hour > 7)
                    fileName = string.Format("{0}", DateTime.Now.ToString("yyyy_MM_dd"));
                else
                    fileName = string.Format("{0}", DateTime.Now.AddDays(-1).ToString("yyyy_MM_dd"));
                using (StreamReader sr = new StreamReader(@"D:\DATA\CT\" + fileName + ".txt", false))
                {
                    text = sr.ReadToEnd();
                }
                string[] sn = text.Split('\n');
                string date = "";
                string[] temp;
                for (int i = 0; i < sn.Length - 1; i++)
                {
                    date = sn[i].Trim('\r');
                    temp = date.Split(',');
                    Update(new CTEventArgs() { DateTime = temp[0], Time = temp[1], SN = temp[2], Start = temp[3], End = temp[4], Ct = temp[5], Dir = temp[6] }, dataGridView1);
                    ctlist.Add(Convert.ToDouble(temp[5]));
                }
                if (textBox1.InvokeRequired)
                {
                    textBox1.BeginInvoke(new Action(() =>
                    {
                        textBox1.Text = ctlist.Average().ToString("0.0");


                    }));
                }
                else
                {
                    textBox1.Text = ctlist.Average().ToString("0.0");

                }

            }
            catch (Exception ex)
            {


            }
        }

        private string readError(string file)
        {
            lock (locker1)
            {
                try
                {
                    string fileName;
                    fileName = string.Format("{0}.txt", file);
                    string outputPath = @"D:\DATA\CT";
                    if (!Directory.Exists(outputPath))
                    {
                        Directory.CreateDirectory(outputPath);
                    }
                    string fullFileName = Path.Combine(outputPath, fileName);
                    FileStream fs = new FileStream(fullFileName, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite, FileShare.Write);
                    StreamReader sr = new StreamReader(fs, Encoding.Default);
                    //string strLine = "";
                    string message = "";
                    //while ((strLine = sr.ReadLine()) != null)
                    //{
                    //    message = message + strLine+"\n";
                    //}
                    message = sr.ReadToEnd();
                    sr.Close();
                    fs.Close();
                    return message;

                }
                catch
                {

                    return "";
                }
            }
        }

        /// <summary>
        /// 读取CT信息
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string ReadCT(string file)
        {
            return readError(file);
        }


        /// <summary>
        /// 加载所有文件
        /// </summary>
        private void UpdateCombox()
        { //加载所有文件
            string path = @"D:\DATA\CT";
            if (!Directory.Exists(path))//若文件夹不存在则新建文件夹   
            {
                Directory.CreateDirectory(path); //新建文件夹   
            }
            if (comboBox1.InvokeRequired)
            {
                comboBox1.BeginInvoke(new Action(() =>
                {
                    comboBox1.Items.Clear();
                    comboBox2.Items.Clear();
                    cbpath.Items.Clear();
                    //获取指定文件夹的所有文件  
                    string[] paths = Directory.GetFiles(path);
                    Array.Reverse(paths);
                    Errorpaths = paths;
                    foreach (var item in paths)
                    {
                        //获取文件后缀名  
                        string extension = Path.GetExtension(item).ToLower();
                        if (extension == ".txt")
                        {
                            string prj = Path.GetFileNameWithoutExtension(item);
                            comboBox1.Items.Add(prj);
                            comboBox1.SelectedIndex = 0;
                            comboBox2.Items.Add(prj);
                            comboBox2.SelectedIndex = 0;
                            cbpath.Items.Add(prj);
                            cbpath.SelectedIndex = 0;

                        }
                    }
                }));
            }
            else
            {
                comboBox1.Items.Clear();
                comboBox2.Items.Clear();
                cbpath.Items.Clear();
                //获取指定文件夹的所有文件  
                string[] paths = Directory.GetFiles(path);

                Array.Reverse(paths);
                Errorpaths = paths;
                foreach (var item in paths)
                {
                    //获取文件后缀名  
                    string extension = Path.GetExtension(item).ToLower();
                    if (extension == ".txt")
                    {
                        string prj = Path.GetFileNameWithoutExtension(item);
                        comboBox1.Items.Add(prj);
                        comboBox1.SelectedIndex = 0;
                        comboBox2.Items.Add(prj);
                        comboBox2.SelectedIndex = 0;
                        cbpath.Items.Add(prj);
                        cbpath.SelectedIndex = 0;
                    }
                }
            }
        }

        private bool locker = false;

        private void ErrorUnit_Load(object sender, EventArgs e)
        {
            locker = true;
            //更新报警记录
            UpdateCombox();
            ////更新操作日志

            radioButton1.Checked = true;
            tabPage4.Parent = null;
            tabPage5.Parent = null;
            tabPage4.Parent = tabControl2;
            locker = false;
        }





        //查询
        private void bT_show_Click(object sender, EventArgs e)
        {
            string a = @"D:\DATA\CT\" + comboBox1.Text + ".txt";
            string b = @"D:\DATA\CT\" + comboBox2.Text + ".txt";

            List<string> date = new List<string>();
            List<double> avreagect = new List<double>();
            List<int> numbers = new List<int>();
            List<double> ct = new List<double>();
            datelist = new List<CalculationDate>();
            Task.Run(new Action(() =>
            {
                try
                {

                    bT_show.BeginInvoke(new Action(() =>
                    {
                        bT_show.Enabled = false;
                    }));
                    string message = "";

                    int index = Errorpaths.ToList().IndexOf(a);

                    int index2 = Errorpaths.ToList().IndexOf(b);
                    int i = 0;
                    if (index > index2)
                    {
                        foreach (var item in Errorpaths)
                        {
                            string prj = Path.GetFileNameWithoutExtension(item);
                            if (i <= index && i >= index2)
                            {
                                message = readError(prj);

                                //解析数据
                                string[] sn = message.Split('\n');

                                string[] temp;
                                string dates;
                                ct = new List<double>();
                                for (int j = 0; j < sn.Length - 1; j++)
                                {
                                    dates = sn[j].Trim('\r');
                                    temp = dates.Split(',');
                                    ct.Add(Convert.ToDouble(temp[5]));
                                }

                                //增加时间
                                date.Add(prj);
                                numbers.Add(ct.Count());
                                avreagect.Add(ct.Average()/ gantryCount);
                                datelist.Add(new CalculationDate() { Date = prj, CT = (ct.Average()/ gantryCount).ToString("0.0"), Count = ct.Count() });
                            }

                            i++;
                        }
                    }
                    i = 0;
                    if (index < index2)
                    {
                        foreach (var item in Errorpaths)
                        {
                            string prj = Path.GetFileNameWithoutExtension(item);
                            if (i >= index && i <= index2)
                            {
                                message = readError(prj);

                                //解析数据
                                string[] sn = message.Split('\n');

                                string[] temp;
                                string dates;
                                ct = new List<double>();
                                for (int j = 0; j < sn.Length - 1; j++)
                                {
                                    dates = sn[j].Trim('\r');
                                    temp = dates.Split(',');
                                    ct.Add(Convert.ToDouble(temp[5]));
                                }

                                //增加时间
                                date.Add(prj);
                                numbers.Add(ct.Count());
                                avreagect.Add(ct.Average()/ gantryCount);
                                datelist.Add(new CalculationDate() { Date = prj, CT = (ct.Average()/ gantryCount).ToString("0.0"), Count = ct.Count() });
                            }
                            i++;
                        }
                    }
                    i = 0;
                    if (index == index2)
                    {
                        foreach (var item in Errorpaths)
                        {
                            string prj = Path.GetFileNameWithoutExtension(item);
                            if (i == index)
                            {
                                message = readError(prj);


                                //解析数据
                                string[] sn = message.Split('\n');

                                string[] temp;
                                string dates;
                                ct = new List<double>();
                                for (int j = 0; j < sn.Length - 1; j++)
                                {
                                    dates = sn[j].Trim('\r');
                                    temp = dates.Split(',');
                                    ct.Add(Convert.ToDouble(temp[5]));
                                }

                                //增加时间
                                date.Add(prj);
                                numbers.Add(ct.Count());
                                avreagect.Add(ct.Average()/ gantryCount);
                                datelist.Add(new CalculationDate() { Date = prj, CT = (ct.Average()/ gantryCount).ToString("0.0"), Count = ct.Count() });

                                break;
                            }
                            i++;
                        }
                    }
                    listView1.BeginInvoke(new Action(() =>
                    {
                        int m = 0;
                        listView1.Items.Clear();
                        foreach (var item in date)
                        {

                            ListViewItem li = new ListViewItem(item);
                            li.SubItems.Add(numbers[m].ToString());

                            li.SubItems.Add(avreagect[m].ToString("0.0"));
                            listView1.Items.Add(li);
                            m++;
                        }
                        UpdatChart(datelist);
                    }));




                    bT_show.BeginInvoke(new Action(() =>
                    {
                        bT_show.Enabled = true;
                    }));
                }
                catch
                {
                    bT_show.BeginInvoke(new Action(() =>
                    {
                        bT_show.Enabled = true;
                    }));

                }

            }));
        }
        //刷新柱状图
        private void UpdatChart(List<CalculationDate> cal)
        {
            chart1.DataSource = cal;

            chart1.Series["产出数量/小时"].XValueMember = "Date";
            chart1.Series["产出数量/小时"].YValueMembers = "Count";
            chart1.Series["产出数量/小时"].Label = "#VAL";
            chart1.Series["产出数量/小时"].IsValueShownAsLabel = true;
            chart1.Series["产出数量/小时"].CustomProperties = "LabelStyle=Top";
            chart2.DataSource = cal;
            chart2.Series["日平均CT"].XValueMember = "Date";
            chart2.Series["日平均CT"].YValueMembers = "CT";
            chart2.Series["日平均CT"].Label = "#VAL";
            chart2.Series["日平均CT"].IsValueShownAsLabel = true;
            chart2.Series["日平均CT"].CustomProperties = "LabelStyle=Top";


        }




        private void bT_openfile_Click(object sender, EventArgs e)
        {
            try
            {
                string outputPath = @"D:\DATA\CT_Excel";
                Process.Start(outputPath);
            }
            catch
            {

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string outputPath = @"D:\DATA\CT_Excel";
                Process.Start(outputPath);
            }
            catch
            {

            }
        }

        //
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                button1.BeginInvoke(new Action(() =>
                {
                    button1.Enabled = false;
                }));
                Initdate();
                string prj = cbpath.Text;

                Task.Run(new Action(() =>
                {
                    try
                    {


                        string message = readError(prj);

                        //解析数据
                        string[] sn = message.Split('\n');

                        string[] temp;
                        string dates;

                        for (int j = 0; j < sn.Length - 1; j++)
                        {
                            dates = sn[j].Trim('\r');
                            temp = dates.Split(',');
                            string value = temp[1].Substring(0, 2);
                            Ctdatedic[value].Total++;
                            Ctdatedic[value].CTTotal += Convert.ToDouble(temp[5]);
                        }

                        if (listView3.InvokeRequired)
                        {
                            listView3.BeginInvoke(new Action(() =>
                            {
                                UpdateList(Ctdate, listView3, 1);
                                UpdateList(Ctdate, listView2, 2);
                                UpdatChart(Ctdate);
                            }));
                        }
                        else
                        {
                            UpdateList(Ctdate, listView3, 1);
                            UpdateList(Ctdate, listView2, 2);
                            UpdatChart(Ctdate);
                        }
                    }
                    catch
                    {


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

        }


        private void UpdatChart(List<CTDate> cal)
        {
            chart3.DataSource = cal;

            chart3.Series["产量"].XValueMember = "Date";
            chart3.Series["产量"].YValueMembers = "Total";
            chart3.Series["产量"].Label = "#VAL";
            chart3.Series["产量"].IsValueShownAsLabel = true;
            chart3.Series["产量"].CustomProperties = "LabelStyle=Top";
            chart4.DataSource = cal;
            chart4.Series["Average CT By Hour"].XValueMember = "Date";
            chart4.Series["Average CT By Hour"].YValueMembers = "CT";
            chart4.Series["Average CT By Hour"].Label = "#VAL";
            chart4.Series["Average CT By Hour"].IsValueShownAsLabel = true;
            chart4.Series["Average CT By Hour"].CustomProperties = "LabelStyle=Top";


        }

        //查找Top5
        private void UpdateList(List<CTDate> cal, ListView lv, int type)
        {

            int num = 0;
            int num1 = 12;
            int ok = 0;
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
                li = new ListViewItem(cal[j].Date);
                li.SubItems.Add(cal[j].Total.ToString());
                li.SubItems.Add(cal[j].CT.ToString("0.0"));
                lv.Items.Add(li);
            }

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (locker)
                return;
            if (radioButton1.Checked)
            {
                tabPage4.Parent = null;
                tabPage5.Parent = null;
                tabPage4.Parent = tabControl2;
            }

            else if (radioButton2.Checked)

            {
                tabPage4.Parent = null;
                tabPage5.Parent = null;
                tabPage5.Parent = tabControl2;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (locker)
                return;
            if (radioButton1.Checked)
            {
                tabPage4.Parent = null;
                tabPage5.Parent = null;
                tabPage4.Parent = tabControl2;
            }

            else if (radioButton2.Checked)

            {
                tabPage4.Parent = null;
                tabPage5.Parent = null;
                tabPage5.Parent = tabControl2;
            }
        }
    }


    public class CTEventArgs
    {
        public string DateTime;
        public string Time;
        public string SN;
        public string Start;
        public string End;
        public string  Ct;
        public string Dir;
    }

    public class CalculationDate
    {
        private int count;

        public int Count
        {
            get
            {
                return count;
            }
            set
            {
                count = value;
            }
        }


        private string date;

        public string Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
            }
        }

        private string ct;

        public string CT
        {
            get
            {
                return ct;
            }
            set
            {
                ct = value;
            }
        }

    }

    public class CTDate
    {
        private int total = 0;
        public int Total
        {
            get { return total; }
            set { total = value; }
        }


        private double ctTotal = 0;
        public double CTTotal
        {
            get { return ctTotal; }
            set { ctTotal = value; }
        }
        private double ct = 0;
        public double CT
        {
            get
            {
                if (total > 0)
                {

                    return Convert.ToDouble((ctTotal / total).ToString("0.0"));
                }
                else
                    return 0;
            }
            set { ct = value; }
        }
        private string date;

        public string Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
            }
        }
    }

}
