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
    public partial class ErrorUnit : UserControl
    {
        Thread th;
        /// <summary>
        /// 设备站点
        /// </summary>
        public string StationID = "";
        private Stopwatch sw = new Stopwatch();
        private string dtstart;
        private string dtend;
        public object locker1 = new object();
        private object locker2 = new object();

        private object locker3 = new object();

        public string ErrorCode = "";
        public string ErrorMessage = "";
        public string ErrorType = "";
        public string ROOT_CAUSE = "";
        public string ACTION = "";
        /// <summary>
        /// 
        /// </summary>
        string[] Errorpaths;
        string[] m_Errorpaths;
        private int savedyas;
        /// <summary>
        /// 数据保存的天数
        /// </summary>
        public ErrorUnit(int days)
        {
            InitializeComponent();
            savedyas = days;
            //th = new Thread(delete);
            //th.IsBackground = true;
            //th.Start();
        }
        /// <summary>
        /// 添加运行日志
        /// </summary>
        /// <param name="massage"></param>
        public void AddActionMessage(string massage)
        {
            try
            {
                saveAction(massage);
            }
            catch
            {


            }

        }
        /// <summary>
        /// 添加开始报警信息
        /// </summary>
        /// <param name="massage"></param>
        public void StartErrorMessage(string massage)
        {
            try
            {
                sw.Restart();
                OutputInfo(listView2, massage);
            }
            catch
            {


            }

        }
        /// <summary>
        /// 添加结束报警信息
        /// </summary>
        /// <param name="massage"></param>
        public void EndErrorMessage(string massage)
        {
            try
            {
                if (!sw.IsRunning)
                    return;
                sw.Stop();
                OutputInfo1(listView2, massage);
            }
            catch
            {


            }

        }
        private delegate void OutputInfoDelegate(TextBox txtInfo, string info);
        private void OutputInfo(ListView txtInfo, string info)
        {
            if (string.IsNullOrEmpty(info)) return;
            if (Common._ConfigDT1 == null)
                Common.GetData();
            //string message = "";

            //string message1 = "";
            dtstart = DateTime.Now.ToString("HH:mm:ss");
            try
            {
                string a = Common._ConfigDT1.Rows[6][info].ToString() + Common._ConfigDT1.Rows[7][info].ToString() + Common._ConfigDT1.Rows[8][info].ToString() + Common._ConfigDT1.Rows[9][info].ToString() + Common._ConfigDT1.Rows[10][info].ToString();

                ErrorCode = a;//     ListViewItem lt = new ListViewItem(StationID);
                ErrorMessage = Common._ConfigDT1.Rows[0][info].ToString();
                ErrorType = Common._ConfigDT1.Rows[12][info].ToString();

                ROOT_CAUSE = Common._ConfigDT1.Rows[2][info].ToString();
                ACTION = Common._ConfigDT1.Rows[3][info].ToString();

            }
            catch
            {
                ErrorCode = "0000000-00-00";
                ErrorMessage = "NULL";
                ErrorType = "NULL";
                ROOT_CAUSE = "NULL";
                ACTION = "NULL";
            }


        }
        private void OutputInfo1(ListView txtInfo, string info)
        {

            //if (string.IsNullOrEmpty(info)) return;
            if (Common._ConfigDT1 == null)
                Common.GetData();
            string message = "";
            ErrorCode = "";
            string message1 = "";
            dtend = DateTime.Now.ToString("HH:mm:ss");
            ListViewItem lt = new ListViewItem(StationID);
            try
            {
                string a = Common._ConfigDT1.Rows[6][info].ToString() + Common._ConfigDT1.Rows[7][info].ToString() + Common._ConfigDT1.Rows[8][info].ToString() + Common._ConfigDT1.Rows[9][info].ToString() + Common._ConfigDT1.Rows[10][info].ToString();

                ErrorCode = a;//     ListViewItem lt = new ListViewItem(StationID);
                ErrorMessage = Common._ConfigDT1.Rows[0][info].ToString();
                lt.SubItems.Add(DateTime.Now.ToString("yyyy/MM/dd"));
                lt.SubItems.Add(a);
                lt.SubItems.Add(dtstart);
                lt.SubItems.Add(dtend);
                lt.SubItems.Add((sw.ElapsedMilliseconds / 1000.0).ToString("0.0") + "s");
                lt.SubItems.Add(Common._ConfigDT1.Rows[1][info].ToString());
                message = string.Format("{0},{1}, {2},{3}, {4},{5},{6}", StationID, DateTime.Now.ToString("yyyy/MM/dd"), a, dtstart, dtend, (sw.ElapsedMilliseconds / 1000.0).ToString("0.0") + "s", Common._ConfigDT1.Rows[1][info].ToString());
                message1 = string.Format("{0},{1},{2},{3},{4},{5},{6}", StationID, DateTime.Now.ToString("yyyy/MM/dd"), a, dtstart, dtend, (sw.ElapsedMilliseconds / 1000.0).ToString("0.0") + "s", Common._ConfigDT1.Rows[1][info].ToString());

            }
            catch (Exception ex)
            {
                ErrorCode = "0000000-00-00";
                ErrorMessage = "NULL";
                lt.SubItems.Add(DateTime.Now.ToString("yyyy/MM/dd"));
                lt.SubItems.Add("XXXXXXX-XX-XX");
                lt.SubItems.Add(dtstart);
                lt.SubItems.Add(dtend);
                lt.SubItems.Add((sw.ElapsedMilliseconds / 1000.0).ToString("0.0") + "s");
                lt.SubItems.Add("XXXX");
                message = string.Format("{0},{1}, {2},{3}, {4},{5},{6}", StationID, DateTime.Now.ToString("yyyy/MM/dd"), "XXXXXXX-XX-XX", dtstart, dtend, (sw.ElapsedMilliseconds / 1000.0).ToString("0.0") + "s", "XXXX");
                message1 = string.Format("{0},{1},{2},{3},{4},{5},{6}", StationID, DateTime.Now.ToString("yyyy/MM/dd"), "XXXXXXX-XX-XX", dtstart, dtend, (sw.ElapsedMilliseconds / 1000.0).ToString("0.0") + "s", "XXXX");

            }

            saveError(message);
            saveErrorcsv(message1);

            txtInfo.BeginInvoke(new Action(() => { txtInfo.Items.Add(lt); }));


        }
        private void OutputInfo(ListView txtInfo)
        {
            if (txtInfo.InvokeRequired)
            {
                txtInfo.BeginInvoke(new Action(() =>
                {
                    txtInfo.Items.Clear();
                }));
            }
            else
            {

                txtInfo.Items.Clear();
            }

        }

        /// <summary>
        /// 复位报警信息
        /// </summary>
        public void RestErrorMessage()
        {

            //OutputInfo(tb_showError);
        }


        private void delete()
        {
            while (true)
            {
                //if (Common.FileManger.DeleteOverflowFile(@"D:\DATA\报警记录",    savedyas))
                //    UpdateCombox();
                //Thread.Sleep(500);
                //if (Common.FileManger.DeleteOverflowFile(@"D:\DATA\操作日志", savedyas))
                //    UpdateComboxAct();
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
                    fileName = string.Format("{0}.txt", DateTime.Now.ToString("yyyy_MM_dd"));
                    string outputPath = @"D:\DATA\异常记录\报警记录";
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
                    fileName = string.Format("{0}.csv", DateTime.Now.ToString("yyyy_MM_dd"));
                    string outputPath = @"D:\DATA\异常记录\报警记录Excel";
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
                        sw.WriteLine("StationID,Date,Code,StartTime,EndTime,CT,Content");
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
        public string readError(string file)
        {
            lock (locker1)
            {
                try
                {
                    string fileName;
                    fileName = string.Format("{0}.txt", file);
                    string outputPath = @"D:\DATA\异常记录\报警记录";
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

        private void saveAction(string message)
        {
            lock (locker2)
            {
                try
                {
                    string fileName;
                    fileName = string.Format("{0}.txt", DateTime.Now.ToString("yyyy_MM_dd"));
                    string outputPath = @"D:\DATA\操作日志";
                    if (!Directory.Exists(outputPath))
                    {
                        Directory.CreateDirectory(outputPath);
                    }
                    string fullFileName = Path.Combine(outputPath, fileName);
                    System.IO.FileStream fs;
                    //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
                    StreamWriter sw;
                    string info = string.Format("{0}     {1}      {2}", DateTime.Now.ToString("yyyy/MM/dd"), DateTime.Now.ToString("HH:mm:ss"), message);
                    if (!File.Exists(fullFileName))
                    {
                        fs = new System.IO.FileStream(fullFileName, System.IO.FileMode.Append, System.IO.FileAccess.Write, FileShare.Read);
                        //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
                        sw = new StreamWriter(fs, System.Text.Encoding.Default);


                        sw.WriteLine(info);
                        sw.Close();
                        fs.Close();
                        UpdateComboxAct();
                    }
                    else
                    {
                        fs = new System.IO.FileStream(fullFileName, System.IO.FileMode.Append, System.IO.FileAccess.Write, FileShare.Read);
                        //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
                        sw = new StreamWriter(fs, System.Text.Encoding.Default);
                        sw.WriteLine(info);
                        sw.Close();
                        fs.Close();
                    }

                }
                catch
                {


                }
            }
        }


        private string readAction(string file)
        {
            lock (locker2)
            {
                try
                {
                    string fileName;
                    fileName = string.Format("{0}.txt", file);
                    string outputPath = @"D:\DATA\操作日志";
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
        /// 加载所有文件
        /// </summary>
        private void UpdateCombox()
        { //加载所有文件
            string path = @"D:\DATA\异常记录\报警记录";
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
                    comboBox3.Items.Clear();
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
                            comboBox3.Items.Add(prj);
                            comboBox3.SelectedIndex = 0;
                        }
                    }
                }));
            }
            else
            {
                comboBox1.Items.Clear();
                comboBox2.Items.Clear();
                comboBox3.Items.Clear();
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
                        comboBox3.Items.Add(prj);
                        comboBox3.SelectedIndex = 0;
                    }
                }
            }
        }


        /// <summary>
        /// 加载所有文件
        /// </summary>
        private void UpdateComboxAct()
        { //加载所有文件
            string path = @"D:\DATA\操作日志";
            if (!Directory.Exists(path))//若文件夹不存在则新建文件夹   
            {
                Directory.CreateDirectory(path); //新建文件夹   
            }
            if (comboBox4.InvokeRequired)
            {
                comboBox4.BeginInvoke(new Action(() =>
                {
                    comboBox4.Items.Clear();

                    //获取指定文件夹的所有文件  
                    string[] paths = Directory.GetFiles(path);
                    Array.Reverse(paths);

                    foreach (var item in paths)
                    {
                        //获取文件后缀名  
                        string extension = Path.GetExtension(item).ToLower();
                        if (extension == ".txt")
                        {
                            string prj = Path.GetFileNameWithoutExtension(item);
                            comboBox4.Items.Add(prj);
                            comboBox4.SelectedIndex = 0;

                        }
                    }
                }));
            }
            else
            {
                comboBox4.Items.Clear();

                //获取指定文件夹的所有文件  
                string[] paths = Directory.GetFiles(path);

                Array.Reverse(paths);

                foreach (var item in paths)
                {
                    //获取文件后缀名  
                    string extension = Path.GetExtension(item).ToLower();
                    if (extension == ".txt")
                    {
                        string prj = Path.GetFileNameWithoutExtension(item);
                        comboBox4.Items.Add(prj);
                        comboBox4.SelectedIndex = 0;

                    }
                }
            }
        }
        public void UpdateComboxConveyor()
        {
            string path = @"D:\DATA\数据设置更改记录\外流道记录\外流道报警记录";
            if (!Directory.Exists(path))//若文件夹不存在则新建文件夹   
            {
                Directory.CreateDirectory(path); //新建文件夹   
            }
            if (comboBox5.InvokeRequired)
            {
                comboBox5.BeginInvoke(new Action(() =>
                {
                    comboBox5.Items.Clear();
                    comboBox6.Items.Clear();
                    //获取指定文件夹的所有文件  
                    string[] paths = Directory.GetFiles(path);
                    Array.Reverse(paths);
                    m_Errorpaths = paths;
                    foreach (var item in paths)
                    {
                        //获取文件后缀名  
                        string extension = Path.GetExtension(item).ToLower();
                        if (extension == ".txt")
                        {
                            string prj = Path.GetFileNameWithoutExtension(item);
                            comboBox5.Items.Add(prj);
                            comboBox5.SelectedIndex = 0;
                            comboBox6.Items.Add(prj);
                            comboBox6.SelectedIndex = 0;

                        }
                    }
                }));
            }
            else
            {
                comboBox5.Items.Clear();
                comboBox6.Items.Clear();
                //获取指定文件夹的所有文件  
                string[] paths = Directory.GetFiles(path);

                Array.Reverse(paths);
                m_Errorpaths = paths;
                foreach (var item in paths)
                {
                    //获取文件后缀名  
                    string extension = Path.GetExtension(item).ToLower();
                    if (extension == ".txt")
                    {
                        string prj = Path.GetFileNameWithoutExtension(item);
                        comboBox5.Items.Add(prj);
                        comboBox5.SelectedIndex = 0;
                        comboBox6.Items.Add(prj);
                        comboBox6.SelectedIndex = 0;

                    }
                }
            }

        }
       public string ReadConveyorErrcorde(string file)
        {
            lock(locker3)
            {
                try
                {
                    string fileName;
                    fileName = string.Format("{0}.txt", file);
                    string outputPath = @"D:\DATA\数据设置更改记录\外流道记录\外流道报警记录";
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
        private void ErrorUnit_Load(object sender, EventArgs e)
        {
            //更新报警记录
            UpdateCombox();
            //更新操作日志
            UpdateComboxAct();
            //更新流道报警记录
            UpdateComboxConveyor();
            tabPage1.Parent = null;
            tabPage2.Parent = null;
            tabPage3.Parent = null;
            tabPage4.Parent = null;
            tabPage5.Parent = null;
            tabPage1.Parent = tabControl1;

        }





        //查询
        private void bT_show_Click(object sender, EventArgs e)
        {
            string a = @"D:\DATA\异常记录\报警记录\" + comboBox1.Text + ".txt";
            string b = @"D:\DATA\异常记录\报警记录\" + comboBox2.Text + ".txt";
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
                                message = message + readError(prj);
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
                                message = message + readError(prj);
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
                                message = message + readError(prj);
                            i++;
                        }
                    }
                    if (tb_showErrorhistory.InvokeRequired)
                    {
                        tb_showErrorhistory.BeginInvoke(new Action(() =>
                        {
                            tb_showErrorhistory.Items.Clear();
                            string[] st = message.Split('\r');
                            foreach (var item in st)
                            {
                                string[] temp = item.Split(',');
                                try
                                {
                                    ListViewItem lt = new ListViewItem(temp[0]);
                                    lt.SubItems.Add(temp[1]);
                                    lt.SubItems.Add(temp[2]);
                                    lt.SubItems.Add(temp[3]);
                                    lt.SubItems.Add(temp[4]);
                                    lt.SubItems.Add(temp[5]);
                                    lt.SubItems.Add(temp[6]);
                                    tb_showErrorhistory.Items.Add(lt);
                                }
                                catch
                                {


                                }

                            }

                            //   tb_showErrorhistory.Text = message;
                        }));
                    }
                    else
                    {
                        tb_showErrorhistory.Text = message;
                    }
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
        //查找Top5
        private void button5_Click(object sender, EventArgs e)
        {

            Dictionary<string, int> dic = new Dictionary<string, int>();
            string prj = comboBox3.Text;
            Task.Run(new Action(() =>
            {
                try
                {

                    button5.BeginInvoke(new Action(() =>
                    {
                        button5.Enabled = false;
                    }));
                    // Thread.Sleep(5000);
                    string message = "";

                    message = readError(prj);
                    string[] lines = message.Split('\n');

                    foreach (var item in lines)
                    {
                        if (item.Length > 2)
                        {
                            //       string temp = item.Split(',')[2];
                            string a = item.Split(',')[2] + "," + item.Split(',')[6];
                            if (!dic.Keys.Contains(a))
                                dic.Add(a, 1);
                            else
                                dic[a]++;
                        }
                    }
                    dic = dic.OrderByDescending(i => i.Value).ToDictionary(p => p.Key, o => o.Value); ;

                    chart1.BeginInvoke(new Action(() =>
                    { //更新listview
                    ListViewItem li;
                        chart1.Series[0].Points.Clear();
                        chart1.Series["异常次数Top5"].Label = "#VAL";
                        chart1.Series["异常次数Top5"].IsValueShownAsLabel = true;
                        chart1.Series["异常次数Top5"].CustomProperties = "LabelStyle=Top";
                        if (dic.Count <= 5)
                        {
                            listView1.Items.Clear();
                            int i = 0;
                            foreach (var item in dic.Keys)
                            {
                                string[] str = item.Split(',');
                                if (Common._ConfigDT1 == null)
                                    Common.GetData();
                                li = new ListViewItem(str[0]);
                                try
                                {
                                    li.SubItems.Add(str[1]);

                                }
                                catch
                                {
                                    li.SubItems.Add("XXXX");

                                }


                                listView1.Items.Add(li);
                                i++;
                                chart1.Series[0].Points.AddXY(i, dic[item]);
                            }
                        }
                        else
                        {
                            listView1.Items.Clear();
                            int i = 0;
                            foreach (var item in dic.Keys)
                            {
                                string[] str = item.Split(',');
                                if (Common._ConfigDT1 == null)
                                    Common.GetData();
                                li = new ListViewItem(str[0]);
                                try
                                {
                                    li.SubItems.Add(str[1]);

                                }
                                catch
                                {
                                    li.SubItems.Add("XXXX");

                                }
                                listView1.Items.Add(li);
                                i++;
                                chart1.Series[0].Points.AddXY(i, dic[item]);
                                if (i == 5)
                                    break;
                            }
                        }
                    //更新chart
                    button5.BeginInvoke(new Action(() =>
                        {
                            button5.Enabled = true;
                        }));

                    }));


                }
                catch
                {
                    button5.BeginInvoke(new Action(() =>
                    {
                        button5.Enabled = true;
                    }));

                }
            }));
        }

        //查询操作信息
        private void button6_Click(object sender, EventArgs e)
        {
            string prj = comboBox4.Text;
            Task.Run(new Action(() =>
            {
                try
                {
                    string message = readAction(prj);
                    if (tb_message.Text != "")
                    {
                        string[] lines = message.Split('\n');
                        message = "";
                        foreach (var item in lines)
                        {
                            if (item.Length > 2)
                            {
                                if (item.Contains(tb_message.Text))
                                    message = message + item + "\n";
                            }
                        }
                    }
                    if (tb_showActionhistory.InvokeRequired)
                    {
                        tb_showActionhistory.BeginInvoke(new Action(() =>
                        {
                            tb_showActionhistory.Text = message;
                        }));
                    }
                    else
                    { tb_showActionhistory.Text = message; }
                }
                catch
                {


                }
            }));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tabPage1.Parent = null;
            tabPage2.Parent = null;
            tabPage3.Parent = null;
            tabPage4.Parent = null;
            tabPage5.Parent = null;
            if (object.ReferenceEquals(sender, button1))
                tabPage1.Parent = tabControl1;
               
            if (object.ReferenceEquals(sender, button2))
                tabPage2.Parent = tabControl1;

            if (object.ReferenceEquals(sender, button3))
                tabPage3.Parent = tabControl1;

            if (object.ReferenceEquals(sender, button4))
                tabPage4.Parent = tabControl1;

            if (object.ReferenceEquals(sender, button9))
                tabPage5.Parent = tabControl1;

        }

        private void button7_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile1 = new SaveFileDialog();
            saveFile1.Filter = "文本文件(.txt)|*.txt";
            saveFile1.FilterIndex = 1;
            if (saveFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK && saveFile1.FileName.Length > 0)
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(saveFile1.FileName, false);
                try
                {
                    sw.WriteLine(tb_showActionhistory.Text);
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

        private void bT_openfile_Click(object sender, EventArgs e)
        {
            try
            {
                string outputPath = @"D:\DATA\异常记录\报警记录Excel";
                Process.Start(outputPath);
            }
            catch
            {

            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            listView2.BeginInvoke(new Action(() => { listView2.Items.Clear(); }));

        }

        private void button10_Click(object sender, EventArgs e)
        {
            string a = @"D:\DATA\数据设置更改记录\外流道记录\外流道报警记录\" + comboBox5.Text + ".txt";
            string b = @"D:\DATA\数据设置更改记录\外流道记录\外流道报警记录\" + comboBox6.Text + ".txt";
            Task.Run(new Action(() =>
            {
                try
                {

                    button10.BeginInvoke(new Action(() =>
                    {
                        button10.Enabled = false;
                    }));
                    string message = "";

                    int index = m_Errorpaths.ToList().IndexOf(a);

                    int index2 = m_Errorpaths.ToList().IndexOf(b);
                    int i = 0;
                    if (index > index2)
                    {
                        foreach (var item in m_Errorpaths)
                        {
                            string prj = Path.GetFileNameWithoutExtension(item);
                            if (i <= index && i >= index2)
                                message = message + ReadConveyorErrcorde(prj);
                            i++;
                        }
                    }
                    i = 0;
                    if (index < index2)
                    {
                        foreach (var item in m_Errorpaths)
                        {
                            string prj = Path.GetFileNameWithoutExtension(item);
                            if (i >= index && i <= index2)
                                message = message + ReadConveyorErrcorde(prj);
                            i++;
                        }
                    }
                    i = 0;
                    if (index == index2)
                    {
                        foreach (var item in m_Errorpaths)
                        {
                            string prj = Path.GetFileNameWithoutExtension(item);
                            if (i == index)
                                message = message + ReadConveyorErrcorde(prj);
                            i++;
                        }
                    }
                    if (listView3.InvokeRequired)
                    {
                        listView3.BeginInvoke(new Action(() =>
                        {
                            listView3.Items.Clear();
                            string[] st = message.Split('\r');
                            List<string> Input = new List<string>();
                            List<string> Ouput = new List<string>();
                            foreach (var item in st)
                            {
                                string[] temp = item.Split(' ');
                                try
                                {
                                    if (temp[4].Contains("M"))
                                    {
                                      
                                        ListViewItem lt = new ListViewItem(temp[0]);
                                        lt.SubItems.Add(temp[4]);
                                        listView3.Items.Add(lt);

                                        if (temp[4].Contains("进料1卡料") || temp[4].Contains("进料2卡料"))
                                        {
                                            Input.Add(temp[4]);
                                        }
                                        if (temp[4].Contains("D出料2向出料1放料超时") || temp[4].Contains("出料2机台载具到位超时")||temp[4].Contains("出料1机台载具到位超时"))
                                        {
                                            Ouput.Add(temp[4]);
                                        }
                                        label3.Text = Input.Count.ToString();
                                        label9.Text = Ouput.Count.ToString();
                                        label10.Text = (lt.Index+1).ToString();
                                    }
                                   
                                }
                                catch
                                {


                                }

                            }
                        }));
                    }
                    else
                    {
                        tb_showErrorhistory.Text = message;
                    }
                    button10.BeginInvoke(new Action(() =>
                    {
                        button10.Enabled = true;
                    }));
                }
                catch
                {
                    button10.BeginInvoke(new Action(() =>
                    {
                        button10.Enabled = true;
                    }));

                }

            }));
        }
    }
}
