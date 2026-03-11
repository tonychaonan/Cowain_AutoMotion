using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System;

namespace lixun_upload_HIVE {
    public class UploadHelper {

        //通过文件后缀获取MIME类型
        private static string GetMimeMapping(string fileName)
        {
            string mimeType = "application/octet-stream";
            string ext = Path.GetExtension(fileName).ToLower();
            RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
            {
                mimeType = regKey.GetValue("Content Type").ToString();
            }
            return mimeType;
        }


        private string UPdataFile(string url, List<string> FilePath, string Jsonstr)
        {
            string strValue = string.Empty, StrDate = string.Empty;
            try
            {
                var memStream = new MemoryStream();
                // 边界符  
                var boundary = DateTime.Now.Ticks.ToString("X");
                // 边界符  
                var beginBoundary = Encoding.ASCII.GetBytes("--" + boundary + "\r\n");
                // 最后的结束符  
                var endBoundary = Encoding.ASCII.GetBytes("--" + boundary + "--\r\n");

                // 将所有图片文件写入内存缓存
                if (FilePath != null && FilePath.Count > 0)
                {
                    for (int i = 0; i < FilePath.Count; i++)
                    {
                        if (string.IsNullOrEmpty(FilePath[i]))
                        {
                            continue;
                        }
                        if (!System.IO.File.Exists(FilePath[i]))
                        {
                            continue;
                        }
                        var fileStream = new FileStream(FilePath[i], FileMode.Open, FileAccess.Read);
                        // 写入文件  
                        const string filePartHeader =
                            "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n" +
                             "Content-Type: {2}\r\n\r\n";
                        var header = string.Format(filePartHeader, "file" + (i + 1), FilePath[i].Substring(FilePath[i].LastIndexOf("\\") + 1), GetMimeMapping(FilePath[i]));
                        var headerbytes = Encoding.UTF8.GetBytes(header);

                        // 开始标记
                        memStream.Write(beginBoundary, 0, beginBoundary.Length);
                        // 文件头标记
                        //--8DB3824D15AAAF6
                        //Content - Disposition: form - data; name = "file1"; filename = "cat.jpg"
                        //Content - Type: image / jpeg
                        memStream.Write(headerbytes, 0, headerbytes.Length);
                        byte[] buffer = new byte[4096];
                        int bytesRead = 0;
                        // 读取文件流并写入内存
                        //while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)//将文件内容写入流
                        //{
                        //    memStream.Write(buffer, 0, bytesRead);
                        //}
                        fileStream.Close();
                        var enterbytes = Encoding.UTF8.GetBytes("\r\n");
                        memStream.Write(enterbytes, 0, enterbytes.Length);
                    }
                }

                // 开始标记
                memStream.Write(beginBoundary, 0, beginBoundary.Length);
                var databytes = Encoding.UTF8.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"" +
                    "\r\n\r\n{1}\r\n", "jsondata", Jsonstr));
                //Comvaria.tabelname = newdata;
                memStream.Write(databytes, 0, databytes.Length);
                memStream.Write(endBoundary, 0, endBoundary.Length);

                memStream.Position = 0;
                var tempBuffer = new byte[memStream.Length];
                memStream.Read(tempBuffer, 0, tempBuffer.Length);
                memStream.Close();

                // System.Net.ServicePointManager.DefaultConnectionLimit = 200;
                System.GC.Collect();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 60 * 1000;
                //request.ServicePoint.Expect100Continue = false;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Ssl3;
                request.Method = "POST";
                request.ContentType = "multipart/form-data;boundary=" + boundary;//multipart/form-data模式
                                                                                 //request.UserAgent;

                using (Stream writer = request.GetRequestStream())
                {
                    writer.Write(tempBuffer, 0, tempBuffer.Length);
                    writer.Close();
                }

                var tempStreamStr = Encoding.UTF8.GetString(tempBuffer);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream s = response.GetResponseStream();
                StreamReader Reader = new StreamReader(s, Encoding.UTF8);

                while ((StrDate = Reader.ReadLine()) != null)
                {
                    strValue += StrDate;
                }
                response.Close();
                response = null;
            }
            catch (Exception ex)
            {

                //Console.WriteLine("Error:{0}", ex.Message.ToString());
                strValue = ex.Message.ToString();
            }
            return strValue;


        }

    }
}