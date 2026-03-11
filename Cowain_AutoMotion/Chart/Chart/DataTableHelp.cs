using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Common.Excel
{
   public class DataTableHelp
    {    /// <summary>
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
                string str1 = dt.Columns[i].ColumnName;
                newDataTable.Columns.Add(new DataColumn(str1));
                //string str2 = dt.Rows[StartRowsIndex][i].ToString();
                //newDataTable.Columns.Add(new DataColumn(str2));
            }
            //遍历一行一行地读入
            for (int j = StartRowsIndex; j <= EndRowsIndex; j++)
            {
                List<bool> list = new List<bool>();//判断dataRow整行是否为空
                //newDataTable.ImportRow(dt.Rows[j]);
                DataRow dataRow = newDataTable.NewRow(); //创建空行
                for (int k = 0; k < dt.Columns.Count; k++)
                {
                    dataRow[k] = dt.Rows[j][k].ToString();//单元格赋值 
                    if (dt.Rows[j][k].ToString() == "")
                    {
                        list.Add(true);
                    }  
                    else
                    {
                        list.Add(false);
                    }                             
                }
                int emptyNum = list.Count(t => t == true);
                if (list.Count!= emptyNum)// dataRow整行不为空时添加
                {
                    newDataTable.Rows.Add(dataRow);//添加一行
                }
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
}
