using Cowain_AutoMotion.Flow.Hive;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cowain_AutoMotion.Flow
{
    public class Remote
    {
        private static object obj = new object();
        private static Remote instance = null;
        public static Remote RemoteInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (obj)
                    {
                        if (instance == null)
                        {
                            instance = new Remote();
                        }
                    }
                }
                return instance;
            }
        }
        private static object locker = new object();
        private static object locker1 = new object();
        public enum RemoteLogType
        {
            MachineState,
            MachineError,
            MachineData,
            Other
        }

        public void SaveDateRemote(string result, RemoteLogType type)
        {
            try
            {
                lock (locker)
                {
                    string fileName;
                    fileName = string.Format("hour_{0}.txt", DateTime.Now.ToString("HH"));

                    string outputPath = @"D:\DATA\Hive Log\Remote Qualification\";
                    if (type == RemoteLogType.MachineState)
                    {
                        outputPath += "Machine State";
                    }
                    else if (type == RemoteLogType.MachineError)
                    {
                        outputPath += "Machine Error";
                    }
                    else if (type == RemoteLogType.MachineData)
                    {
                        outputPath += "Machine Data";
                    }
                    else
                    {
                        outputPath += @"Other";
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

                    fs = new System.IO.FileStream(fullFileName, System.IO.FileMode.Append, System.IO.FileAccess.Write, FileShare.Read);
                    //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
                    sw = new StreamWriter(fs, System.Text.Encoding.Default);
                    sw.WriteLine(result);
                    sw.Close();
                    fs.Close();
                }
            }
            catch { }
        }
        
    }
}
