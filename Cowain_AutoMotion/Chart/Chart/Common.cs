using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chart
{
    public class Common
    {

        public static DataTable _ConfigDT1 = null;
        public static DataTable _ConfigDTSTEP = null;
        public static void GetData()
        {
            _ConfigDT1 = Common.ConfigDataTable.GetConfigDT("ERROR", true);
            _ConfigDT1 = Common.ConfigDataTable.SwapTable(_ConfigDT1);//将表格行列 

        }
        public static void GetStepData()
        {
            _ConfigDTSTEP = Common.ConfigDataTable.GetConfigDT("STEP", true);
            _ConfigDTSTEP = Common.ConfigDataTable.SwapTable(_ConfigDTSTEP);//将表格行列 对调
        }
        public static DataTable ErrorBase = null;
        public static void GetErrorBase()
        {
            ErrorBase = ConfigDataTable.GetConfigDT("ERROR", true);
            #region 拼装CODE
            for (int i = 0; i < ErrorBase.Rows.Count; i++)
            {
                ErrorBase.Rows[i][12] = GetDef_Str(ErrorBase.Rows[i][7]) + GetDef_Str(ErrorBase.Rows[i][8]) + GetDef_Str(ErrorBase.Rows[i][9]) + GetDef_Str(ErrorBase.Rows[i][10]) + GetDef_Str(ErrorBase.Rows[i][11]);
            }
            #endregion
        }
        /// <summary>
        /// 将值转换成string
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static string GetDef_Str(object obj, string def = "")
        {
            string val = def;
            try
            {
                val = obj.ToString();
            }
            catch
            {

            }
            return val;
        }
        public static void saveCsvData(string message)
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
                    string outputPath = @"D:\DATA\Log";
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
                        sw.WriteLine("Date,Time,OP ID,Mes 上传,Software version,SN,CT,Glue SN,Mix ratio,A pressure,B pressure");
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

        public class ExcelHelper
        {
            /// <summary>
            /// 参数配置表1
            /// </summary>

            /// <summary>
            /// 把DataTable的数据写入到指定的excel文件中
            /// </summary>
            /// <param name="TargetFileNamePath">目标文件excel的路径</param>
            /// <param name="sourceData">要写入的数据</param>
            /// <param name="sheetName">excel表中的sheet的名称，可以根据情况自己起</param>
            /// <param name="IsWriteColumnName">是否写入DataTable的列名称</param>
            /// <returns>返回写入的行数</returns>
            public static int DataTableToExcel(string TargetFileNamePath, DataTable sourceData, string sheetName, bool IsWriteColumnName)
            {
                //数据验证
                if (!File.Exists(TargetFileNamePath))
                {
                    //excel文件的路径不存在
                    throw new ArgumentException("excel文件的路径不存在或者excel文件没有创建好");
                }
                if (sourceData == null)
                {
                    throw new ArgumentException("要写入的DataTable不能为空");
                }

                if (sheetName == null && sheetName.Length == 0)
                {
                    throw new ArgumentException("excel中的sheet名称不能为空或者不能为空字符串");
                }
                //根据Excel文件的后缀名创建对应的workbook
                IWorkbook workbook = null;
                if (TargetFileNamePath.IndexOf(".xlsx") > 0)
                {  //2007版本的excel
                    workbook = new XSSFWorkbook();
                }
                else if (TargetFileNamePath.IndexOf(".xls") > 0) //2003版本的excel
                {
                    workbook = new HSSFWorkbook();
                }
                else
                {
                    return -1;    //都不匹配或者传入的文件根本就不是excel文件，直接返回
                }

                //excel表的sheet名
                ISheet sheet = workbook.CreateSheet(sheetName);
                if (sheet == null) return -1;   //无法创建sheet，则直接返回
                                                //写入Excel的行数
                int WriteRowCount = 0;
                //指明需要写入列名，则写入DataTable的列名,第一行写入列名
                if (IsWriteColumnName)
                {
                    //sheet表创建新的一行,即第一行
                    IRow ColumnNameRow = sheet.CreateRow(0); //0下标代表第一行
                                                             //进行写入DataTable的列名
                    for (int colunmNameIndex = 0; colunmNameIndex < sourceData.Columns.Count; colunmNameIndex++)
                    {
                        ColumnNameRow.CreateCell(colunmNameIndex).SetCellValue(sourceData.Columns[colunmNameIndex].ColumnName);
                    }
                    WriteRowCount++;
                }


                //写入数据
                for (int row = 0; row < sourceData.Rows.Count; row++)
                {
                    //sheet表创建新的一行
                    IRow newRow = sheet.CreateRow(WriteRowCount);
                    for (int column = 0; column < sourceData.Columns.Count; column++)
                    {
                        newRow.CreateCell(column).SetCellValue(sourceData.Rows[row][column].ToString());
                    }
                    WriteRowCount++;  //写入下一行
                }
                //写入到excel中
                FileStream fs = new FileStream(TargetFileNamePath, FileMode.Open, FileAccess.Write);
                workbook.Write(fs);

                fs.Flush();
                fs.Close();

                workbook.Close();
                return WriteRowCount;
            }

            /// <summary>
            /// 从Excel中读入数据到DataTable中
            /// </summary>
            /// <param name="FilePath">Excel文件的路径</param>
            /// <param name="sheetName">excel文件中工作表名称</param>
            /// <param name="IsHasColumnName">文件是否有列名</param>
            /// <returns>从Excel读取到数据的DataTable结果集</returns>
            public static DataTable ExcelToDataTable(string FilePath, string sheetName, bool IsHasColumnName)
            {
                IWorkbook workbook = null;
                ISheet sheet = null;
                DataTable newDataTable = new DataTable();
                int startRow = 0;
                try
                {
                    if (!File.Exists(FilePath))
                    {
                        MessageBox.Show("Excel文件不存在,地址为:" + FilePath);
                        return null;
                    }
                    //打开文件，读取workbook；             
                    using (FileStream fs = File.OpenRead(FilePath))   //打开xlsx文件
                    {
                        //根据Excel文件的后缀名创建对应的workbook
                        if (FilePath.IndexOf(".xlsx") > 0)//2007版本的excel
                        {
                            workbook = new XSSFWorkbook(fs);
                        }
                        else if (FilePath.IndexOf(".xls") > 0) //2003版本的excel
                        {
                            workbook = new HSSFWorkbook(fs);
                        }
                        else
                        {
                            MessageBox.Show("传入的文件不是Excel文件,地址为:" + FilePath);
                            return null; //都不匹配或者传入的文件根本就不是excel文件，直接返回
                        }
                    }
                    if (sheetName == "")
                    {
                        //没有指定sheet名称，直接取第一个
                        sheet = workbook.GetSheetAt(0);
                        //获取不到，直接返回
                        if (sheet == null) return null;
                    }
                    else
                    {
                        //获取工作表sheet
                        sheet = workbook.GetSheet(sheetName);
                        //获取不到，直接返回
                        if (sheet == null) return null;
                    }
                    IRow FirstRow = sheet.GetRow(0);//取第一行数据
                    int cellCount = FirstRow.LastCellNum; //第一行最后一个cell的编号 即总的列数
                    for (int i = FirstRow.FirstCellNum; i < cellCount; ++i)//给newDataTable添加列
                    {
                        if (IsHasColumnName)//第一行是否为表头
                        {
                            ICell cell = FirstRow.GetCell(i);
                            if (cell != null)//没有数据的单元格都默认是null
                            {
                                string cellValue = cell.StringCellValue.Replace(" ","");
                                if (cellValue != null)
                                {
                                    DataColumn column = new DataColumn(cellValue);//创建带列名的列
                                    newDataTable.Columns.Add(column);//添加一列
                                }
                                else
                                {
                                    DataColumn column = new DataColumn();//创建空列
                                    newDataTable.Columns.Add(column);
                                }
                            }
                        }
                        else
                        {
                            DataColumn column = new DataColumn();//创建空列
                            newDataTable.Columns.Add(column);
                        }
                    }
                    startRow = IsHasColumnName ? sheet.FirstRowNum + 1 : sheet.FirstRowNum;//确定起始行                   
                    int rowCount = sheet.LastRowNum; //最后一行的标号即行数
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);//得到一行数据

                        if (row == null) continue; //没有数据的行默认是null
                        DataRow dataRow = newDataTable.NewRow(); //创建空行
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null                              
                                dataRow[j] = row.GetCell(j).ToString();//单元格赋值
                        }
                        newDataTable.Rows.Add(dataRow);
                    }
                    workbook.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                return newDataTable;
            }

        }
        /// <summary>
        /// 配置表类
        /// </summary>
        public class ConfigDataTable
        {

            /// <summary>
            /// 配置表路径
            /// </summary>
            //private string XlsxFilePath = string.Format("{0}\\Config\\Config.xlsx", Application.StartupPath);

            /// <summary>
            /// 获取配置相应的表
            /// </summary>
            /// <param name="SheetName"></param>
            /// <param name="IsHasColumnName">有无表头</param>
            /// <returns></returns>
            public static DataTable GetConfigDT(string SheetName, bool IsHasColumnName)
            {
                DataTable oldDataTable = null;
                try
                {
                    if (SheetName == null || SheetName.Length == 0)
                    {
                        MessageBox.Show("读取Excel文件未指定Sheet名称");
                    }
                    else
                    {
                        //string XlsxFilePath = string.Format("{0}\\Config\\{1}", Application.StartupPath, "ERROR.xlsx");

                        string XlsxFilePath = string.Format("{0}\\Config\\{1}", Application.StartupPath, "ERROR.xls");
                        //"E:\\CrownSystem\\CrownSystem\\bin\\Debug\\Config\\Config1424-001.xlsx"
                        //"E:\\crown_1号机 - 1021\\Test\\bin\\Debug\\Config\\Config1424-001.xlsx"
                        oldDataTable = ExcelHelper.ExcelToDataTable(XlsxFilePath, SheetName, IsHasColumnName);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                return oldDataTable;
            }

            /// <summary>
            /// 将表行列对换,不含表头
            /// </summary>
            /// <param name="tableData"></param>
            /// <returns></returns>
            public static DataTable SwapTable(DataTable tableData)
            {
                try
                {
                    int intRows = tableData.Rows.Count;
                    int intColumns = tableData.Columns.Count;

                    //转二维数组
                    string[,] arrayData = new string[intRows, intColumns];
                    for (int i = 0; i < intRows; i++)
                    {
                        for (int j = 0; j < intColumns; j++)
                        {
                            arrayData[i, j] = tableData.Rows[i][j].ToString();
                        }
                    }
                    //下标对换
                    string[,] arrSwap = new string[intColumns, intRows];
                    for (int m = 0; m < intColumns; m++)
                    {
                        for (int n = 0; n < intRows; n++)
                        {
                            arrSwap[m, n] = arrayData[n, m];
                        }
                    }
                    DataTable dt = new DataTable();
                    //添加列
                    for (int k = 0; k < intRows; k++)
                    {
                        dt.Columns.Add(
                                new DataColumn(arrSwap[0, k])
                            );
                    }
                    //添加行
                    for (int r = 1; r < intColumns; r++)
                    {
                        DataRow dr = dt.NewRow();
                        for (int c = 0; c < intRows; c++)
                        {
                            dr[c] = arrSwap[r, c].ToString();
                        }
                        dt.Rows.Add(dr);
                    }
                    return dt;
                }
                catch (Exception ex)
                {
                    return null;
                }

            }
        }
        /// <summary>
        /// DataTable操作类
        /// </summary>
        class DataTableHelper
        {
            /// <summary>
            /// 获取DataTable的列名
            /// </summary>
            /// <param name="dt"></param>
            /// <returns></returns>
            public static string[] GetColumns(DataTable dt)
            {
                List<string> oldlist = new List<string>();
                string[] strColumns = null;
                if (dt.Columns.Count > 0)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        oldlist.Add(dt.Columns[i].ColumnName);
                    }
                }
                strColumns = oldlist.ToArray();
                return strColumns;
            }

            /// <summary>
            /// 获取DataTable的冒列的数据
            /// </summary>
            /// <param name="dt">DataTable</param>
            /// <param name="ColumnName">列名</param>
            ///  <param name="DeleteEmpty">除去空字符，默认不除去</param>
            /// <returns></returns>
            public static string[] GetRows(DataTable dt, string ColumnName, bool DeleteEmpty)
            {
                List<string> oldlist = new List<string>();
                string[] strRows = null;
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i][ColumnName].ToString() == "" && DeleteEmpty)
                        {
                            continue;
                        }
                        else
                        {
                            oldlist.Add(dt.Rows[i][ColumnName].ToString());
                        }
                    }
                }
                strRows = oldlist.ToArray();
                return strRows;
            }

            /// <summary>
            /// 根据起始结束索引获取DataTable的冒列的数据
            /// </summary>
            /// <param name="dt"></param>
            /// <param name="ColumnName"></param>
            /// <param name="StartRowsIndex"></param>
            /// <param name="EndRowsIndex"></param>
            /// <param name="DeleteEmpty"></param>
            /// <returns></returns>
            public static string[] GetRows(DataTable dt, string ColumnName, int StartRowsIndex, int EndRowsIndex, bool DeleteEmpty)
            {
                List<string> oldlist = new List<string>();
                string[] strRows = null;
                if (dt.Rows.Count > 0)
                {
                    for (int i = StartRowsIndex; i < EndRowsIndex + 1; i++)
                    {
                        if (dt.Rows[i][ColumnName].ToString() == "" && DeleteEmpty)
                        {
                            continue;
                        }
                        else
                        {
                            oldlist.Add(dt.Rows[i][ColumnName].ToString());
                        }
                    }
                }
                strRows = oldlist.ToArray();
                return strRows;
            }

            /// <summary>
            /// 根据列名和行的内容得到第一个匹配的行索引,未找到返回-1
            /// </summary>
            /// <param name="dt">DataTable</param>
            /// <param name="ColumnName">列名</param>
            /// <param name="RowsValue">行的内容</param>
            /// <param name="StartRowIndex">起始索引</param>
            /// <returns>行索引</returns>
            public static int GetRowsIndex(DataTable dt, string ColumnName, string RowsValue, int StartRowIndex = 0)
            {
                int RowIndex = -1;
                for (int i = StartRowIndex; i < dt.Rows.Count; i++)
                {

                    string str = dt.Rows[i][ColumnName].ToString();
                    if (dt.Rows[i][ColumnName].ToString() == RowsValue)
                    {
                        RowIndex = i;//得到索引
                        return RowIndex;
                    }
                }
                return RowIndex;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="dt"></param>
            /// <param name="ColumnIndex"></param>
            /// <param name="RowsValue"></param>
            /// <param name="StartRowIndex"></param>
            /// <returns></returns>
            public static int GetRowsIndex(DataTable dt, int ColumnIndex, string RowsValue, int StartRowIndex = 0)
            {
                int RowIndex = -1;
                for (int i = StartRowIndex; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i][ColumnIndex].ToString() == RowsValue)
                    {
                        RowIndex = i;//得到索引
                        return RowIndex;
                    }
                }
                return RowIndex;
            }

            /// <summary>
            /// 根据起始索引和结束索引创建一个新的DataTable,默认第一行为表头
            /// </summary>
            /// <param name="dt"></param>
            /// <param name="StartRowsIndex"></param>
            /// <param name="EndRowsIndex"></param>
            /// <returns></returns>
            public static DataTable SplitDT(DataTable dt, int StartRowsIndex, int EndRowsIndex)
            {
                DataTable newDataTable = new DataTable();
                if (StartRowsIndex > EndRowsIndex)
                {
                    MessageBox.Show("在调用DataTableHelper中SplitDT方法时，StartRowsIndex> EndRowsIndex不满足执行条件");
                    return null;
                }
                if (EndRowsIndex > dt.Rows.Count)
                {
                    MessageBox.Show("在调用DataTableHelper中SplitDT方法时，EndRowsIndex> DataTable.Rows.Count不满足执行条件");
                    return null;
                }
                //添加列名                  
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    //newDataTable.Columns.Add(new DataColumn(dt.Columns[i].ColumnName));
                    newDataTable.Columns.Add(new DataColumn(dt.Rows[StartRowsIndex][i].ToString()));
                }
                //遍历一行一行地读入
                for (int j = StartRowsIndex + 1; j <= EndRowsIndex; j++)
                {
                    //newDataTable.ImportRow(dt.Rows[j]);
                    DataRow dataRow = newDataTable.NewRow(); //创建空行
                    for (int k = 0; k < dt.Columns.Count; k++)
                    {
                        dataRow[k] = dt.Rows[j][k].ToString();//单元格赋值                                
                    }
                    newDataTable.Rows.Add(dataRow);//添加一行
                }
                return newDataTable;
            }

            /// <summary>
            /// 添加列数据
            /// </summary>
            /// <param name="dt"></param>
            /// <param name="ColumnsName"></param>
            /// <param name="ColumnsData"></param>
            public static void AddColumnsData(ref DataTable dt, string ColumnsName, string[] ColumnsData)
            {
                dt.Columns.Add(ColumnsName);//添加一列
                                            //行数目不够是添加新行
                for (int i = dt.Rows.Count; i < ColumnsData.Length; i++)
                {
                    //创建DataTable的数据行
                    DataRow newdataRow = dt.NewRow();
                    dt.Rows.Add(newdataRow);
                }
                int StartRows = 0;//开始行
                                  //添加行内容
                foreach (DataRow row in dt.Rows)
                {
                    //如果新添加的列数据不够直接跳出
                    if (StartRows >= ColumnsData.Length)
                    {
                        return;
                    }
                    row[ColumnsName] = ColumnsData[StartRows];
                    StartRows++;
                }

            }

        }

        public class Ini
        {

            #region ini 文件读写函数

            /// <summary>
            /// 读取INI文件中指定的Key的值
            /// </summary>
            /// <param name="lpAppName">节点名称。如果为null,则读取INI中所有节点名称,每个节点名称之间用\0分隔</param>
            /// <param name="lpKeyName">Key名称。如果为null,则读取INI中指定节点中的所有KEY,每个KEY之间用\0分隔</param>
            /// <param name="lpDefault">读取失败时的默认值</param>
            /// <param name="lpReturnedString">读取的内容缓冲区，读取之后，多余的地方使用\0填充</param>
            /// <param name="nSize">内容缓冲区的长度</param>
            /// <param name="lpFileName">INI文件名</param>
            /// <returns>实际读取到的长度</returns>
            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            public static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, [In, Out] char[] lpReturnedString, uint nSize, string lpFileName);

            //另一种声明方式,使用 StringBuilder 作为缓冲区类型的缺点是不能接受\0字符，会将\0及其后的字符截断,
            //所以对于lpAppName或lpKeyName为null的情况就不适用
            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            public static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, uint nSize, string lpFileName);

            //再一种声明，使用string作为缓冲区的类型同char[]
            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            public static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, string lpReturnedString, uint nSize, string lpFileName);

            /// <summary>
            /// 将指定的键和值写到指定的节点，如果已经存在则替换
            /// </summary>
            /// <param name="lpAppName">节点名称</param>
            /// <param name="lpKeyName">键名称。如果为null，则删除指定的节点及其所有的项目</param>
            /// <param name="lpString">值内容。如果为null，则删除指定节点中指定的键。</param>
            /// <param name="lpFileName">INI文件</param>
            /// <returns>操作是否成功</returns>
            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);


            #endregion


            public static double GetPrivateProfileDouble(string lpAppName, string lpKeyName, double Default, string lpFileName)
            {
                // char[] lpReturnedString = new char[1024];
                StringBuilder lpReturnedString = new StringBuilder(1024);
                GetPrivateProfileString(lpAppName, lpKeyName, Convert.ToString(Default), lpReturnedString, 1024, lpFileName);
                return Convert.ToDouble(lpReturnedString.ToString());


            }
            public static int GetPrivateProfileInt(string lpAppName, string lpKeyName, int Default, string lpFileName)
            {
                //char[] lpReturnedString = new char[1024];
                StringBuilder lpReturnedString = new StringBuilder(1024);
                GetPrivateProfileString(lpAppName, lpKeyName, Convert.ToString(Default), lpReturnedString, 1024, lpFileName);

                return Convert.ToInt32(lpReturnedString.ToString());
            }

            public static string GetPrivateProfileString(string lpAppName, string lpKeyName, string Default, string lpFileName)
            {
                StringBuilder lpReturnedString = new StringBuilder(1024);
                GetPrivateProfileString(lpAppName, lpKeyName, Default, lpReturnedString, 1024, lpFileName);
                return lpReturnedString.ToString();
            }


            public static void CreateDirectoryEx(string Path)
            {
                int nPos;
                string PathTemp;
                nPos = Path.LastIndexOf('\\');
                if (nPos < 0)
                    nPos = Path.LastIndexOf('/');

                if (nPos < 0)
                {
                    return;
                }
                nPos = Path.LastIndexOf('\\');
                if (nPos > -1)
                {
                    PathTemp = Path.Substring(0, nPos);
                }
                else
                {
                    nPos = Path.LastIndexOf('/');
                    PathTemp = Path.Substring(0, nPos);
                }

                Directory.CreateDirectory(PathTemp);

            }
        }

        #region 文件夹删除

        class FileComparer : IComparer
        {
            int IComparer.Compare(Object o1, Object o2)
            {
                FileInfo fi1 = o1 as FileInfo;
                FileInfo fi2 = o2 as FileInfo;
                return fi1.CreationTime.CompareTo(fi2.CreationTime);//小于返回负值  不调换顺序
            }
        }


        class DicComparer : IComparer
        {
            int IComparer.Compare(Object o1, Object o2)
            {
                DirectoryInfo fi1 = o1 as DirectoryInfo;
                DirectoryInfo fi2 = o2 as DirectoryInfo;
                return fi1.CreationTime.CompareTo(fi2.CreationTime);//小于返回负值  不调换顺序
            }
        }

        public class FileManger
        {
            public static bool DeleteOverflowFile(string path, int maxFileNumber)
            {
                bool isdelete = false;
                return false;
                try
                {

                    DirectoryInfo di = new DirectoryInfo(path);
                    FileInfo[] files = di.GetFiles();
                    if (files.Length < maxFileNumber + 1)
                    {
                        return isdelete;
                    }
                    FileComparer fc = new FileComparer();
                    Array.Sort(files, fc);
                    try
                    {
                        int i = 0;
                        while (true)
                        {
                            try
                            {
                                File.Delete(files[i].FullName);
                                isdelete = true;
                                break;
                            }
                            catch
                            {

                                i++;
                                if (i == 5)
                                    break;
                            }
                        }
                    }
                    catch
                    {

                    }
                    return isdelete;
                }
                catch
                {
                    return isdelete;
                }
            }

            public static bool DeleteOverflowFile1(string path, int maxFileNumber)
            {
                bool isdelete = false;
                try
                {

                    DirectoryInfo di = new DirectoryInfo(path);
                    FileInfo[] files = di.GetFiles();
                    if (files.Length < maxFileNumber + 1)
                    {
                        return isdelete;
                    }
                    FileComparer fc = new FileComparer();
                    Array.Sort(files, fc);
                    try
                    {
                        int i = 0;
                        while (true)
                        {
                            try
                            {
                                File.Delete(files[i].FullName);
                                isdelete = true;
                                break;
                            }
                            catch
                            {

                                i++;
                                if (i == 5)
                                    break;
                            }
                        }
                    }
                    catch
                    {

                    }
                    return isdelete;
                }
                catch
                {
                    return isdelete;
                }
            }
            public static void DeleteOverflowDic(string path, int maxFileNumber)
            {
                try
                {
                    DirectoryInfo di = new DirectoryInfo(path);
                    DirectoryInfo[] dics = di.GetDirectories();
                    if (dics.Length < maxFileNumber)
                    {
                        return;
                    }
                    DicComparer fc = new DicComparer();
                    Array.Sort(dics, fc);
                    //   File.Delete(dics[0].FullName);
                    try
                    {
                        int i = 0;
                        while (true)
                        {
                            try
                            {
                                Directory.Delete(dics[i].FullName, true);
                                break;
                            }
                            catch
                            {

                                i++;
                                if (i == 5)
                                    break;
                            }
                        }
                    }
                    catch
                    {

                    }


                }
                catch
                {

                }
            }

            public static void DeleteOverflowDicFile(string path, int maxFileNumber)
            {
                try
                {
                    DirectoryInfo di = new DirectoryInfo(path);
                    DirectoryInfo[] dics = di.GetDirectories();
                    foreach (var pathtemp in dics)
                    {
                        DeleteOverflowFile1(pathtemp.FullName, maxFileNumber);
                    }
                }
                catch
                {

                }
            }
        }
        #endregion

        public static double string16ToInt(string S)
        {
            string str1 = string.Empty;
            string[] str = S.Trim().Split(' '); //S变成字符串数组
            foreach (var item in str)
            {
                str1 += item;
            }
            int temparture = Convert.ToInt32(str1, 16);//将接受的16进制字符串转成整数
            double Now_temparture = Math.Round(Convert.ToDouble(temparture) / 10.0, 1);  //double类型的，保留小数点后一位
            return Now_temparture;
        }

        #region CRC校验
        /// <summary>
        /// CRC校验码
        /// </summary>
        private static byte ucCRCHi = 0xFF;
        /// <summary>
        /// CRC校验码
        /// </summary>
        private static byte ucCRCLo = 0xFF;
        private static readonly byte[] aucCRCHi = {
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
             0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
             0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
             0x00, 0xC1, 0x81, 0x40
         };
        private static readonly byte[] aucCRCLo = {
             0x00, 0xC0, 0xC1, 0x01, 0xC3, 0x03, 0x02, 0xC2, 0xC6, 0x06, 0x07, 0xC7,
             0x05, 0xC5, 0xC4, 0x04, 0xCC, 0x0C, 0x0D, 0xCD, 0x0F, 0xCF, 0xCE, 0x0E,
             0x0A, 0xCA, 0xCB, 0x0B, 0xC9, 0x09, 0x08, 0xC8, 0xD8, 0x18, 0x19, 0xD9,
             0x1B, 0xDB, 0xDA, 0x1A, 0x1E, 0xDE, 0xDF, 0x1F, 0xDD, 0x1D, 0x1C, 0xDC,
             0x14, 0xD4, 0xD5, 0x15, 0xD7, 0x17, 0x16, 0xD6, 0xD2, 0x12, 0x13, 0xD3,
             0x11, 0xD1, 0xD0, 0x10, 0xF0, 0x30, 0x31, 0xF1, 0x33, 0xF3, 0xF2, 0x32,
             0x36, 0xF6, 0xF7, 0x37, 0xF5, 0x35, 0x34, 0xF4, 0x3C, 0xFC, 0xFD, 0x3D,
             0xFF, 0x3F, 0x3E, 0xFE, 0xFA, 0x3A, 0x3B, 0xFB, 0x39, 0xF9, 0xF8, 0x38,
             0x28, 0xE8, 0xE9, 0x29, 0xEB, 0x2B, 0x2A, 0xEA, 0xEE, 0x2E, 0x2F, 0xEF,
             0x2D, 0xED, 0xEC, 0x2C, 0xE4, 0x24, 0x25, 0xE5, 0x27, 0xE7, 0xE6, 0x26,
             0x22, 0xE2, 0xE3, 0x23, 0xE1, 0x21, 0x20, 0xE0, 0xA0, 0x60, 0x61, 0xA1,
             0x63, 0xA3, 0xA2, 0x62, 0x66, 0xA6, 0xA7, 0x67, 0xA5, 0x65, 0x64, 0xA4,
             0x6C, 0xAC, 0xAD, 0x6D, 0xAF, 0x6F, 0x6E, 0xAE, 0xAA, 0x6A, 0x6B, 0xAB,
             0x69, 0xA9, 0xA8, 0x68, 0x78, 0xB8, 0xB9, 0x79, 0xBB, 0x7B, 0x7A, 0xBA,
             0xBE, 0x7E, 0x7F, 0xBF, 0x7D, 0xBD, 0xBC, 0x7C, 0xB4, 0x74, 0x75, 0xB5,
             0x77, 0xB7, 0xB6, 0x76, 0x72, 0xB2, 0xB3, 0x73, 0xB1, 0x71, 0x70, 0xB0,
             0x50, 0x90, 0x91, 0x51, 0x93, 0x53, 0x52, 0x92, 0x96, 0x56, 0x57, 0x97,
             0x55, 0x95, 0x94, 0x54, 0x9C, 0x5C, 0x5D, 0x9D, 0x5F, 0x9F, 0x9E, 0x5E,
             0x5A, 0x9A, 0x9B, 0x5B, 0x99, 0x59, 0x58, 0x98, 0x88, 0x48, 0x49, 0x89,
             0x4B, 0x8B, 0x8A, 0x4A, 0x4E, 0x8E, 0x8F, 0x4F, 0x8D, 0x4D, 0x4C, 0x8C,
             0x44, 0x84, 0x85, 0x45, 0x87, 0x47, 0x46, 0x86, 0x82, 0x42, 0x43, 0x83,
             0x41, 0x81, 0x80, 0x40
         };

        /// <summary>
        /// CRC校验
        /// </summary>
        /// <param name="pucFrame"></param>
        /// <param name="usLen"></param>
        /// <returns></returns>
        public static byte[] trueCrc16(byte[] pucFrame, int usLen)
        {
            int i = 0;
            byte[] bt = new byte[2];
            ucCRCHi = 0xFF;
            ucCRCLo = 0xFF;
            UInt16 iIndex = 0x0000;

            while (usLen-- > 0)
            {
                iIndex = (UInt16)(ucCRCLo ^ pucFrame[i++]);
                ucCRCLo = (byte)(ucCRCHi ^ aucCRCHi[iIndex]);
                ucCRCHi = aucCRCLo[iIndex];
            }
            bt[0] = ucCRCLo;
            bt[1] = ucCRCHi;
            return bt;
        }
        #endregion
    }
}
