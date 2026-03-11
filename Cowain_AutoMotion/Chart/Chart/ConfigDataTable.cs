using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Common.Excel
{
 public   class ConfigDataTable
    {
        public static DataTable GetConfigDT(string XlsxFilePath, string SheetName, bool IsHasColumnName)
        {
            DataTable result = null;
            try
            {
                bool flag = SheetName == null || SheetName.Length == 0;
                if (flag)
                {
                    MessageBox.Show("读取Excel文件未指定Sheet名称");
                }
                else
                {
                    result = ExcelHelper.ExcelToDataTable(XlsxFilePath, SheetName, IsHasColumnName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return result;
        }
    }
}

