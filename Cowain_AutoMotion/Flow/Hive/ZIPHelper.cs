using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZIPHelper
{
    public static class myZIPHelper
    {
        public static bool ZIP(string photoPath, string ZIPFileFullPath,ref string errorMSG)
        {
            try
            {
                string[] paths = null;
                paths = Directory.GetFiles(photoPath);
                string currentTime = DateTime.Now.ToString("yyyyMMddhhmmss");
                int index = ZIPFileFullPath.LastIndexOf('\\');
                string dirPath = ZIPFileFullPath.Substring(0, index);
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
                ZipFiles(paths, ZIPFileFullPath, 1, "", currentTime);
                //DirectoryInfo subdir = new DirectoryInfo(photoPath);
                //subdir.Delete(true);
            }
            catch(Exception e)
            {
                errorMSG=e.Message;
                return false;
            }
            return true;
        }
        /// <summary>
        /// 制作压缩包（多个文件压缩到一个压缩包，支持加密、注释）
        /// </summary>
        /// <param name="fileNames">要压缩的文件</param>
        /// <param name="topDirectoryName">压缩文件目录</param>
        /// <param name="zipedFileName">压缩包文件名</param>
        /// <param name="compresssionLevel">压缩级别 1-9</param>
        /// <param name="password">密码</param>
        /// <param name="comment">注释</param>
        private static void ZipFiles(string[] fileNames, string zipedFileName, int? compresssionLevel, string password = "", string comment = "")
        {
            using (ZipOutputStream zos = new ZipOutputStream(File.Open(zipedFileName, FileMode.OpenOrCreate)))
            {
                if (compresssionLevel.HasValue)
                {
                    zos.SetLevel(compresssionLevel.Value);//设置压缩级别
                }

                if (!string.IsNullOrEmpty(password))
                {
                    zos.Password = password;//设置zip包加密密码
                }

                if (!string.IsNullOrEmpty(comment))
                {
                    zos.SetComment(comment);//设置zip包的注释
                }

                foreach (string file in fileNames)
                {
                    // string fileName = string.Format("{0}/{1}", topDirectoryName, file);
                    if (File.Exists(file))
                    {
                        FileInfo item = new FileInfo(file);
                        FileStream fs = File.OpenRead(item.FullName);
                        byte[] buffer = new byte[fs.Length];
                        fs.Read(buffer, 0, buffer.Length);
                        fs.Close();
                        fs.Dispose();
                        ZipEntry entry = new ZipEntry(item.Name);
                        zos.PutNextEntry(entry);
                        zos.Write(buffer, 0, buffer.Length);
                    }
                }
            }
        }
        public static bool DelPhoto(string photoPath)
        {
            try
            {
                string[] paths = null;
                paths = Directory.GetFiles(photoPath);
                foreach (string path in paths)
                {
                    File.Delete(path);
                }
                //DirectoryInfo subdir = new DirectoryInfo(photoPath);
                //subdir.Delete(true);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }
}
