using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace Cowain_AutoMotion
{
    public class JsonHelper
    {
        private object locker = new object();
        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="o"></param>
        protected  void ObjectToJsonFile(string path, object o)
        {
            string str = JsonConvert.SerializeObject(o);
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(str);
                sw.Flush();
                fs.Flush(true);
                sw.Close();
            }
        }

        protected object JsonFileToObject(string path, object o)
        {
            string str = string.Empty;
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                StreamReader sr = new StreamReader(fs);
                str = sr.ReadToEnd();
                sr.Close();
            }
            return JsonConvert.DeserializeObject(str, o.GetType());
        }

       [JsonIgnore]
        private string strsaveFile="";
        [JsonIgnore]
        private string strsaveFile1 = "";
        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strBaseDic"></param>
        /// <param name="fileName"></param>
        /// <param name="objectTemp"></param>
        public bool ReaderParams<T>(string strBaseDic,string fileName, ref T objectTemp)
        {
           
            bool isResult = true;
            if (!Directory.Exists(strBaseDic))
            {
                Directory.CreateDirectory(strBaseDic);
            }
            if (File.Exists(strBaseDic + "\\" + fileName + ".json"))
            {
                objectTemp = (T)JsonFileToObject(strBaseDic + "\\" + fileName + ".json", objectTemp);
              
            }
            else
            {
                ObjectToJsonFile(strBaseDic + "\\" + fileName + ".json", objectTemp);
                isResult = false;
            }

           


            return isResult;

        }



        public void ReadBufferDate<T>(string strBaseDic, string fileName, ref T objectTemp)
        {
            
                DialogResult dlgResult = MessageBox.Show(strBaseDic + "\\" + fileName + ".json" + " 该文件数据存在异常，是否加载备用参数", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dlgResult == DialogResult.Yes)
                {
                    objectTemp = (T)JsonFileToObject(strBaseDic + "开机备份\\" + fileName + ".json", objectTemp);                   
                    ObjectToJsonFile(strBaseDic + "\\" + fileName + ".json", objectTemp);
                }
                else
                {
                    //Process[] allProcesses = Process.GetProcesses();
                    //foreach (Process item in allProcesses)
                    //{
                    //    if (item.ProcessName.Contains("Cowain_AutoMotion"))
                    //    {
                    //        item.Kill();
                    //    }
                    //}
                }
            
        }

       /// <summary>
       /// 设置参数保存路径  
       /// </summary>
       /// <param name="strBaseDic"></param>
       /// <param name="fileName"></param>
        public void SetSaveFile<T>(string strBaseDic, string fileName,T objectTemp)
        {           
            strsaveFile = strBaseDic + "\\" + fileName + ".json";
            strsaveFile1 = strBaseDic + "开机备份\\" + fileName + ".json";
            if (!Directory.Exists(strBaseDic+ "开机备份"))
            {
                Directory.CreateDirectory(strBaseDic + "开机备份");
            }
            if (!Directory.Exists(strBaseDic + "开机备份"))
            {
                Directory.CreateDirectory(strBaseDic + "开机备份");
            }
            ObjectToJsonFile(strBaseDic + "\\" + fileName + ".json", objectTemp);
            ObjectToJsonFile(strBaseDic + "开机备份\\" + fileName + ".json", objectTemp);
        }
        /// <summary>
        /// 加载json文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static void LoadConfigParams<T>(string fileName, out T obj) where T : class, new()
        {
            
                string path = GetConfigFilePath() + "\\" + "DataBaseData\\CowainConfig\\ParamFile";
                string jsonString = "";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = path + "\\" + fileName + ".json";
                if (!File.Exists(path))
                {
                    File.Create(path).Close(); ;
                }
                try
                {
                    using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            jsonString = sr.ReadToEnd();
                        }
                    }
                }
                catch
                {

                }
                T myObject = JsonConvert.DeserializeObject<T>(jsonString) ?? new T(); //如果为空，就new 一个
                obj = myObject;
            

        }
        /// <summary>
        /// 获取配置文件路径
        /// </summary>
        /// <returns>配置文件路径</returns>
        private static string GetConfigFilePath()
        {
            int pathsNum;
            string pathname = Application.StartupPath;
            for (int i = 0; i < 4; i++)
            {
                pathsNum = pathname.LastIndexOf('\\');
                pathname = pathname.Substring(0, pathsNum);
            }
            return pathname;
        }
        /// <summary>
        /// 序列化对象 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectTemp"></param>
        public virtual void WriteParams<T>( T objectTemp)
        {
            lock(locker)
            {
                ObjectToJsonFile(strsaveFile, objectTemp);                
            }
        }
    }
}
