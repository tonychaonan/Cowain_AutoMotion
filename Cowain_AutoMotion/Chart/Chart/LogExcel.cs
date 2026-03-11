using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolTotal;

namespace Chart
{
    public class LogExcel
    {
      //  IniFile myIniFile;
        string path;
        //public void loadLogExcelData(ref string OPID, ref string GlueSN, ref string APressure, ref string BPressure, ref string MachineID, ref string StationID)
        //{
        //    String strPath = System.IO.Directory.GetCurrentDirectory();
        //    String strNowPath = strPath.Replace("\\Cowain_AutoMotion\\bin\\x64\\Debug", "");
        //    String strBasePath = strNowPath + "\\DataBaseData\\Configuration.ini";
        //    myIniFile = new IniFile(strBasePath);//初始化配置文件位置 
        //    OPID = myIniFile.IniReadValue("Macmini", "OPID");
        //    GlueSN = myIniFile.IniReadValue("Macmini", "GlueSN1");
        //    APressure = myIniFile.IniReadValue("Macmini", "APressure");
        //    BPressure = myIniFile.IniReadValue("Macmini", "BPressure");
        //    MachineID = myIniFile.IniReadValue("Macmini", "MachineID");
        //    StationID = myIniFile.IniReadValue("Other", "StationID");
        //}
        public void saveCsvData(string head,string message)
        {
            //lock (locker1)
            {
                try
                {
                    string fileName;
                    int hour = DateTime.Now.Hour;
                    if (hour > 7)
                        fileName = string.Format("{0}.csv", DateTime.Now.ToString("yyyy_MM_dd"));
                    else
                        fileName = string.Format("{0}.csv", DateTime.Now.AddDays(-1).ToString("yyyy_MM_dd"));
                    string outputPath = @"D:\DATA\LogExcel";
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
                        //按胶阀类型分
                        //if (isAB)
                        //{
                        //    sw.WriteLine("Date,Time,OP ID,Mes Status,Software version,UC SN,SN,Dir,CT,Glue SN,Mix ratio,A pressure,B pressure,Section,Station");
                        //}
                        //else
                        //{
                        //    sw.WriteLine("Date,Time,OP ID,Mes Status,Software version,UC SN,SN,Dir,CT,Glue SN,Section,Station");
                        //}
                        sw.Write(head);
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
        /// 获取目录下最新文件
        /// </summary>
        /// <param name="dictionary">目录</param>
        /// <returns></returns>
        public static string getLastFile(string dictionary)
        {
            try
            {
                DirectoryInfo info = new DirectoryInfo(dictionary);
                FileInfo newestFile = info.GetFiles().OrderBy(n => n.LastWriteTime).Last();
                return newestFile.FullName;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}
