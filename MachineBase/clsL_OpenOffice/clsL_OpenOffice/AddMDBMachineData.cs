

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenOffice_Connect
{
    public class AddMDBMachineData : clsConnect
    {
        OleDbConnection conn = null;
        DataTable dt = null;
        OleDbDataAdapter adapter = null;
        string MDBMachinePath;

        /// <summary>
        /// 行数据信息
        /// </summary>
        public List<string> List_Msg = new List<string>();
        public AddMDBMachineData(string m_strMDBMachinePath) : base(m_strMDBMachinePath)
        {
            MDBMachinePath = m_strMDBMachinePath;
            if (conn == null)
            {
                conn = OleDbConnect();//连接数据库
                if (conn.State == ConnectionState.Closed)
                    conn.Open();//打开数据库
            }         
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="table">表格名称</param>
        /// <returns></returns>
        public DataView ReadTables(string table)
        {
            return ReadTable(table);
        }
        /// <summary>
        /// 刷新界面并保存数据
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>

        public DataView RefreshTablea(string table)
        {
            return Refresh(table);
        }
        /// <summary>
        /// 新增一行
        /// </summary>
        /// <param name="table">表格名称</param>
        /// <returns></returns>
        public DataView Add(string table)
        {
            return AddTableRow(table);
        }
        /// <summary>
        /// 修改数据库内容
        /// </summary>
        /// <param name="表名"></param>
        /// <param name="列表头"></param>
        /// <param name="原值"></param>
        /// <param name="新值"></param>
        /// <returns></returns>
        public bool ChangeDataValue(string 表名, string 列表头, string 原值, string 新值)
        {
            return ChangeData(表名, 列表头, 原值, 新值);
        }

        /// <summary>
        /// 新增一行
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="名称"></param>
        /// <param name="站号"></param>
        /// <param name="端口号"></param>
        /// <param name="IsOut">是否为输出端口，0指输入，1指输出</param>
        /// <returns></returns>
        public bool InsertDataValue(string ID, string 名称, string 站号, string 端口号, string IsOut = "0")
        {
            return Insert(ID, 名称, 站号, 端口号, IsOut);
        }
        /// <summary>
        /// 根据ID删除行
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool DeleteDataValue(string name, string table)
        {
            return Delete(name, table);
        }
        /// <summary>
        /// 查找数据
        /// </summary>
        /// <param name="strname">表格名称</param>
        /// <param name="ID">查询ID</param>
        /// <param name="list">数据集合</param>
        /// <returns></returns>
        public bool SelectData(string strname, string ID, out Dictionary<string, object> list)
        {
            list = new Dictionary<string, object>();


            return FindData(strname, ID, out list);
        }
        /// <summary>
        /// 删除table
        /// </summary>
        /// <param name="table">表格名称</param>
        /// <returns></returns>
        public bool DeleteTable(string table)
        {
            return DeleTable(table);
        }
        /// <summary>
        /// 新建table
        /// </summary>
        /// <param name="table">表格名称</param>
        /// <returns></returns>
        public bool CreateNewTable(string table)
        {
            return CreateTable(table);
        }
        /// <summary>
        /// 新增一列
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="field">列名/字段</param>
        /// <returns></returns>
        public bool InsertNewField(string table, string field)
        {
            return InsertField(table, field);
        }
        /// <summary>
        /// 新增一列
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="field">列名/字段</param>
        /// <returns></returns>
        public bool DeleteOneField(string table, string field)
        {
            return DeleteField(table, field);
        }
        /// <summary>
        /// 利用指定表头方式查找数据进行修改，当查找到有多个数据时，会修改失败。
        /// </summary>
        /// <param name="表名"></param>
        /// <param name="列表头"></param>
        /// <param name="原值"></param>
        /// <param name="新值"></param>
        /// <returns></returns>
        private bool ChangeData(string 表名, string 列表头, string 原值, string 新值)
        {
            try
            {
                if (MDBMachinePath != "")
                {

                    if (conn.State == ConnectionState.Closed)
                        conn.Open();//打开数据库
                    string sql = "update " + 表名 + " set " + 列表头 + " = '" + 新值 + "' where " + 列表头 + "='" + 原值 + "'";
                    OleDbCommand comm = new OleDbCommand(sql, conn);//创建实例
                    int count = comm.ExecuteNonQuery();//写入数据库，返回行数
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="名称"></param>
        /// <param name="站号"></param>
        /// <param name="端口号"></param>
        /// <param name="IsOut">是否为输出端口号，默认0否，1为输出</param>
        /// <returns></returns>
        private bool Insert(string ID, string 名称, string 站号, string 端口号, string IsOut = "0")
        {
            try
            {
                IsOut = IsOut == "0" ? "0" : " 1";
                if (MDBMachinePath != "")
                {

                    if (conn.State == ConnectionState.Closed)
                        conn.Open();//打开数据库

                    string str;
                    if (名称 == "IOs")
                        str = "insert into " + 名称 + "(ID,CName,EName,CardID,NodeID,PortID,PinID,isOut,isInverter)values('" + ID + "','" + 名称 + "','',0," + 站号 + ",0," + 端口号 + "," + IsOut + ",0)";

                    else
                        return false;
                    OleDbDataAdapter ole;
                    DataSet ds = new DataSet();
                    string sql = "select * from " + 名称 + " where ID = '" + ID + "'";
                    OleDbCommand olc;
                    ole = new OleDbDataAdapter(sql, conn);
                    ole.Fill(ds);//将查到的数据放到数据表ds中
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DialogResult result = MessageBox.Show(ID + "已存在，是否替换？", "提示", MessageBoxButtons.OKCancel,
             MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                        if (result != DialogResult.OK)
                        {
                            return false;
                        }
                        string delete = "DELETE FROM " + 名称 + " where ID='" + ID + "'";

                        olc = new OleDbCommand(delete, conn);
                        olc.ExecuteNonQuery();
                    }
                    olc = new OleDbCommand(str, conn);//创建实例
                    int count = olc.ExecuteNonQuery();//写入数据库，返回行数
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        private bool Delete(string ID, string strname)
        {
            try
            {
                if (MDBMachinePath != "")
                {

                    if (conn.State == ConnectionState.Closed)
                        conn.Open();//打开数据库
                    string delete = "DELETE FROM " + strname + " where ID='" + ID + "'";
                    OleDbCommand olc;
                    olc = new OleDbCommand(delete, conn);
                    olc.ExecuteNonQuery();
                }
            }
            catch { return false; }
            return true;
        }
        /// <summary>
        /// 根据ID查找行信息
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private bool FindData(string TableName, string ID, out Dictionary<string, object> list)
        {
            list = new Dictionary<string, object>();
            try
            {
                if (MDBMachinePath != "")
                {

                    if (conn.State == ConnectionState.Closed)
                        conn.Open();//打开数据库
                    string Find = "select * from " + TableName + " where  ID='" + ID + "'";
                    OleDbDataAdapter olc = new OleDbDataAdapter(Find, conn);

                    DataSet ds = new DataSet();//填充ds，保存数据
                    olc.Fill(ds);//将查到的数据放到数据表ds中

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                            list.Add(ds.Tables[0].Columns[j].ColumnName, ds.Tables[0].Rows[i][j].ToString());//需要转成string
                    }
                }
            }
            catch (Exception ex) { return false; }
            return true;
        }
        /// <summary>
        /// 获取表格列名
        /// </summary>
        /// <param name="TableName">表格名称</param>
        /// <param name="ID">ID</param>
        /// <returns></returns>
        private string GetColumnName(string TableName, string ID)
        {
            string str = "";
            try
            {
                if (MDBMachinePath != "")
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();//打开数据库
                    string Find = "select * from " + TableName + " where  ID='" + ID + "'";
                    OleDbDataAdapter olc = new OleDbDataAdapter(Find, conn);

                    DataSet ds = new DataSet();//填充ds，保存数据
                    olc.Fill(ds);//将查到的数据放到数据表ds中

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                            str += ds.Tables[0].Columns[j].ColumnName + ",";//需要转成string
                    }
                }
            }
            catch (Exception ex) { return ""; }
            return str.Substring(0, str.Length - 1);
        }

        private DataView ReadTable(string table)
        {
            try
            {
                if (MDBMachinePath != "")
                {

                    if (conn.State == ConnectionState.Closed)
                        conn.Open();//打开数据库

                    //Select*from表1为SQL语句，意思是从数据库中选择叫做“表1”的表，conn为连接
                    adapter = new OleDbDataAdapter("Select * from " + table, conn);
                    //OleDbCommandBuilder对应数据库适配器，需要传递参数
                    var smd = new OleDbCommandBuilder(adapter);
                    //在内存中创建一个DataTable，用来存放、修改数据库
                    dt = new DataTable();
                    //通过适配器把表的数据填充到内存dt
                    adapter.Fill(dt);
                    //把数据显示到界面
                    List_Msg.Clear();
                    
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        object[] str = dt.Rows[i].ItemArray;
                        List_Msg.Add(table+","+str[0].ToString().Trim()+","+str[1].ToString().Trim());
                    }
                }
            }
            catch (Exception ex) { return null; }
            return dt.DefaultView;
        }

        private DataView Refresh(string table)
        {
            try
            {
                if (MDBMachinePath != "")
                {

                    if (conn.State == ConnectionState.Closed)
                        conn.Open();//打开数据库

                    ////Select*from表1为SQL语句，意思是从数据库中选择叫做“表1”的表，conn为连接
                    //adapter = new OleDbDataAdapter("Select * from " + table, conn);
                    ////OleDbCommandBuilder对应数据库适配器，需要传递参数
                    //var smd = new OleDbCommandBuilder(adapter);

                    //通过适配器把表的数据填充到内存dt
                    adapter.Update(dt);
                    //清除内存中存放的表数据
                    dt.Clear();
                    //重新读取已经改变过的表数据
                    adapter.Fill(dt);
                    //把数据显示到界面

                }
            }
            catch (Exception ex) { return null; }
            return dt.DefaultView;
        }
        private DataView AddTableRow(string table)
        {
            try
            {
                if (MDBMachinePath != "")
                {

                    if (conn.State == ConnectionState.Closed)
                        conn.Open();//打开数据库

                    //Select*from表1为SQL语句，意思是从数据库中选择叫做“表1”的表，conn为连接
                    adapter = new OleDbDataAdapter("Select * from " + table, conn);
                    //OleDbCommandBuilder对应数据库适配器，需要传递参数
                    var smd = new OleDbCommandBuilder(adapter);
                    //在内存中创建一个DataTable，用来存放、修改数据库
                    dt = new DataTable();
                    //通过适配器把表的数据填充到内存dt
                    adapter.Fill(dt);
                    //把数据显示到界面

                    object[] str = dt.Rows[0].ItemArray;
                    str[0] = "0";
                    if (table.Contains("Put"))
                    {
                        str[1] = "新定义端口，完善相关信息后才可使用";
                        str[4] = "0";
                        str[6] = "0";
                    }
                    else if (table.Contains("Cylinder"))
                    {
                        str[1] = "新定义端口，完善相关信息后才可使用";
                        str[3] = "0";
                        str[4] = "0";
                        str[5] = "0";
                        str[6] = "0";
                    }
                    else if (table.Contains("Motor"))
                    {
                        str[1] = "新定义端口，完善相关信息后才可使用";
                        str[2] = "";
                        str[5] = "0";

                        str[8] = "0";
                        str[9] = "0";
                        str[22] = "0";
                    }
                    dt.Rows.Add(str);
                }
            }
            catch (Exception ex) { return null; }
            return dt.DefaultView;
        }

        private bool CreateTable(string table)
        {
            try
            {
                if (MDBMachinePath != "")
                {

                    if (conn.State == ConnectionState.Closed)
                        conn.Open();//打开数据库

                    string tablestr = "CREATE TABLE " + table + "(Field1 TEXT(30),Field2 DOUBLE)";//
                    //Field1 表头
                    //TEXT(30) 字符串，长度为30
                    //INTEGER 整数型
                    //DOUBLE  浮点型

                    DataTable dt = conn.GetSchema("tables");

                    OleDbCommand cmd = new OleDbCommand(tablestr, conn);

                    cmd.ExecuteNonQuery();
                    return true;

                }
            }
            catch (Exception ex) { return false; }
            return false;
        }
        private bool DeleTable(string table)
        {
            try
            {
                if (MDBMachinePath != "")
                {

                    if (conn.State == ConnectionState.Closed)
                        conn.Open();//打开数据库

                    string tablestr = "DROP TABLE " + table;//


                    OleDbCommand cmd = new OleDbCommand(tablestr, conn);

                    cmd.ExecuteNonQuery();
                    return true;

                }
            }
            catch (Exception ex) { return false; }
            return false;
        }
        /// <summary>
        /// 插入一列
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="field">列名</param>
        /// <returns></returns>
        private bool InsertField(string table, string field)
        {

            try
            {
                if (MDBMachinePath != "")
                {

                    if (conn.State == ConnectionState.Closed)
                        conn.Open();//打开数据库

                    string tablestr = "ALTER TABLE " + table + " ADD COLUMN " + field + " TEXT(30)";//


                    OleDbCommand cmd = new OleDbCommand(tablestr, conn);

                    cmd.ExecuteNonQuery();
                    return true;

                }
            }
            catch (Exception ex) { return false; }
            return false;
        }
        /// <summary>
        /// 删除一列
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="field">列名</param>
        /// <returns></returns>
        private bool DeleteField(string table, string field)
        {

            try
            {
                if (MDBMachinePath != "")
                {

                    if (conn.State == ConnectionState.Closed)
                        conn.Open();//打开数据库


                    string del = "ALTER TABLE " + table + " DROP " + field;

                    OleDbCommand cmd = new OleDbCommand(del, conn);

                    cmd.ExecuteNonQuery();
                    return true;

                }
            }
            catch (Exception ex) { return false; }
            return false;
        }
    }
}
