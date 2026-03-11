using Cowain_Form.FormView;
using Cowain_Machine.Flow;
using DevExpress.XtraCharts;
using MotionBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Cowain_AutoMotion
{
    public static class Commons
    {
        #region 通用变量
        /// <summary>
        /// 每天开始时间（8点）
        /// </summary>
        public static readonly int DayStarHour = 8;

        private static object locker = new object();
        private static Ping pingSender = new Ping();
        private static PingReply pingreply;
        #endregion
        /// <summary>
        /// 打开目录
        /// </summary>
        /// <param name="folderPath">文件夹路径</param>
        public static void OpenFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                return;
            }

            Process process = new Process();
            ProcessStartInfo psi = new ProcessStartInfo("Explorer.exe");
            psi.Arguments = folderPath;
            process.StartInfo = psi;
            try
            {
                process.Start();
            }
            catch
            {

            }
            finally
            {
                process.Close();
            }
        }

        /// <summary>
        /// 打开目录且选中文件
        /// </summary>
        /// <param name="folderPathAndName">文件绝对路径</param>
        public static void OpenFolderAndSelectFile(string folderPathAndName)
        {
            if (!File.Exists(folderPathAndName))
            {
                return;
            }

            Process process = new Process();
            ProcessStartInfo psi = new ProcessStartInfo("Explorer.exe");
            psi.Arguments = "/e,/select," + folderPathAndName;
            process.StartInfo = psi;
            try
            {
                process.Start();
            }
            catch
            {

            }
            finally
            {
                process.Close();
            }
        }

        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="folderPathAndName">文件绝对路径</param>
        private static void OpenFile(string folderPathAndName)
        {
            if (!File.Exists(folderPathAndName))
            {
                return;
            }
            Process process = new Process();
            ProcessStartInfo psi = new ProcessStartInfo(folderPathAndName);
            process.StartInfo = psi;

            process.StartInfo.UseShellExecute = true;
            try
            {
                process.Start();
                process.WaitForExit();
            }
            catch (Exception err)
            {
                string msg = err.Message;
            }
            finally
            {
                process.Close();
            }
        }

        /// <summary>
        /// 将文件拷贝到指定路径
        /// </summary>
        /// <param name="sourceFile">源文件绝对路径</param>
        /// <param name="toFilesPath">目标路径</param>
        /// <param name="newFileName">文件保存名称</param>
        /// <returns></returns>
        public static bool CopyFileTo(string sourceFile, string toFilesPath, string newFileName,ref string errmsg)
        {
            bool result = false;
            try
            {
                if (!File.Exists(sourceFile))
                {
                    errmsg = "缺少源文件！" + sourceFile;
                    return false;
                }
                if (!Directory.Exists(toFilesPath))
                {
                    errmsg = "目标文件夹不存在！" + toFilesPath;
                    return false;
                }
                FileInfo file = new FileInfo(sourceFile);
                file.CopyTo(toFilesPath + @"\" + newFileName, true);
                result = true;
            }
            catch (Exception err)
            {
                errmsg = err.Message;
                result = false;
            }
            return result;
        }
        /// <summary>
        /// ping一下IP，检查是否能连接的
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static bool PingIP(string IP, int timeout)
        {
            lock(locker)
            {
                bool state = false;

                try
                {
                    pingreply = pingSender.Send(IP, timeout);

                    if (pingreply.Status.ToString().Trim() == "Success")
                    {
                        state = true;
                    }
                    else
                    {
                        state = false;
                    }
                }
                catch
                {
                    state = false;
                }
                return state;
            }
        }

        /// <summary>
        /// 获取指定网卡的MAC地址
        /// </summary>
        /// <param name="netname"></param>
        /// <returns></returns>
        public static string GetMacAdd(string netname)
        {
            string mac = string.Empty;
            NetworkInterface[] nets = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface item in nets)
            {
                if (item.Name == netname)
                {
                    mac = GetMacAdd(item);
                    break;
                }
            }

            return mac;
        }

        public static string GetMacAdd(NetworkInterface net)
        {
            string mac = "00:00:00:00:00:00";
            try
            {
                byte[] bytes = net.GetPhysicalAddress().GetAddressBytes();
                mac = BitConverter.ToString(bytes).Replace("-", ":");
                if (string.IsNullOrWhiteSpace(mac))
                {
                    mac = "00:00:00:00:00:00";
                }
            }
            catch (Exception)
            {
            }

            return mac;
        }
    }

    /// <summary>
    /// 图标操作类
    /// </summary>
    public static class Charts
    {
        /// <summary>
        /// chart数据显示
        /// </summary>
        /// <param name="dic">数据</param>
        /// <param name="chart">显示容器</param>
        /// <param name="max">最大显示数量</param>
        public static void ShowDataByDic(Dictionary<string, decimal> dic, ChartControl chart,int max)
        {
            try
            {
                Series[] series = chart.SeriesSerializable;

                if (series.Length > 0)
                {
                    SeriesPointCollection colls = series[0].Points;
                    colls.Clear();
                    int index = 0;
                    foreach (string key in dic.Keys)
                    {
                        //只显示TOP10
                        if (index >= max)
                            break;
                        SeriesPoint seriesPoint = new SeriesPoint(key, new object[] { ((object)(dic[key])) });
                        series[0].Points.AddRange(new DevExpress.XtraCharts.SeriesPoint[] { seriesPoint });
                        index++;
                    }

                    //补满max数量
                    //int empitynum = max - dic.Keys.Count;
                    //for (int i = 0; i < empitynum; i++)
                    //{
                    //    SeriesPoint seriesPoint = new SeriesPoint("", new object[] { ((object)(0)) });
                    //    series[0].Points.AddRange(new DevExpress.XtraCharts.SeriesPoint[] { seriesPoint });
                    //}
                }
            }
            catch (Exception err)
            {
                MsgBoxHelper.DxMsgShowErr(err.Message);
            }
                       
        }
    }
}
