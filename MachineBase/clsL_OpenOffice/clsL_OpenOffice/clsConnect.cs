using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenOffice_Connect
{
    public class clsConnect
    {
        /// <summary>
        /// 数据库路径
        /// </summary>
        string m_MDBMachinePath;
        public clsConnect(string MDBMachinePat)
        {
            m_MDBMachinePath = MDBMachinePat;
        }


        public OleDbConnection OleDbConnect()
        {
            return Connect();
        }

        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <returns></returns>
        private OleDbConnection Connect()
        {
            try
            {
                if (IntPtr.Size == 4)
                    m_MDBMachinePath = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + m_MDBMachinePath;  // +";" + "Persist Security Info=True;";
                else if (IntPtr.Size == 8)
                    m_MDBMachinePath = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + m_MDBMachinePath; // +";" + "Persist Security Info=True;"
                if (OpenOffice == null)                                                                                        //打开方式一
                {
                    OpenOffice = new OleDbConnection(m_MDBMachinePath);
                    OpenOffice.Open();
                }

                //----------打开方式二-------------------
                if (DBConn == null)                                                                                        //打开方式一
                {
                    DBConn = new OleDbConnection();
                    DBCommand = new OleDbCommand();

                    DBConn.ConnectionString = m_MDBMachinePath;
                    DBConn.Open();
                }
                return OpenOffice;
            }
            catch
            { return null; }
        }

        /// <summary>
        /// 关闭读数据
        /// </summary>
        /// <param name="oRs"></param>
        /// <returns></returns>
        public bool DisConnect()
        {
            try
            {
                DBConn.Close();
                DBConn = null;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool Fun_CloseRS(ref DbDataReader oRs)
        {
            try
            {
                oRs.Close();
                oRs = null;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public enum enTrans
        {
            BEGIN = 1,
            COMMIT = 2,
            ROLLBACK = 3
        }

        private System.Data.Common.DbConnection DBConn = null;
        private System.Data.Common.DbCommand DBCommand = null;
        private System.Data.Common.DbTransaction DBTran = null;
        OleDbConnection OpenOffice = null;




        public bool Save(string sSQL, bool bDBClose = false)
        {
            try
            {

                return Save_ExecSQL(sSQL, bDBClose);
            }
            catch
            {
                return false;

            }
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="sSQL">指令</param>
        /// <param name="bDBClose">是否关闭数据库</param>
        /// <returns></returns>
        private bool Save_ExecSQL(string sSQL, bool bDBClose = false)
        {
            try
            {
                sSQL = sSQL.Replace("''", "' '");

                if (this.DBConn == null)
                {
                    OleDbConnect();
                    if (this.DBConn.State == ConnectionState.Closed) return false;
                }
                else
                {
                    if (this.DBConn.State != System.Data.ConnectionState.Open)
                    {
                        if (this.DBConn.State == ConnectionState.Closed) return false;
                    }
                }

                if (DBConn.State != System.Data.ConnectionState.Open) return false;

                this.DBCommand.Connection = DBConn;
                this.DBCommand.CommandType = System.Data.CommandType.Text;
                this.DBCommand.CommandText = sSQL;
                this.DBCommand.Transaction = DBTran;

                if (DBCommand.ExecuteNonQuery() <= 0)
                {
                    if (bDBClose == true) DisConnect();
                    return false;
                }
                else
                {
                    if (bDBClose == true) DisConnect();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Find(string sSQL, ref System.Data.Common.DbDataReader retRS, bool bDBClose = false)
        {
            try
            {
                return Find_RsSQL(sSQL, ref retRS, bDBClose);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 查找数据
        /// </summary>
        /// <param name="sSQL">命令</param>
        /// <param name="retRS">数据源</param>
        /// <param name="bDBClose">是否关闭</param>
        /// <returns></returns>
        private bool Find_RsSQL(string sSQL, ref System.Data.Common.DbDataReader retRS, bool bDBClose = false)
        {
            try
            {
                sSQL = sSQL.Replace("''", "' '");

                if (this.DBConn == null)
                {
                    OleDbConnect();
                    if (this.DBConn.State == ConnectionState.Closed) return false;
                }
                else
                {
                    if (this.DBConn.State != System.Data.ConnectionState.Open)
                    {
                        if (this.DBConn.State == ConnectionState.Closed) return false;
                    }
                }

                if (DBConn.State != System.Data.ConnectionState.Open) return false;

                DBCommand.Connection = DBConn;
                DBCommand.CommandText = sSQL;


                retRS = DBCommand.ExecuteReader();
                if (retRS.HasRows == false)
                {
                    retRS.Close();
                    if (bDBClose == true) DisConnect();
                    return false;
                }
                else
                {
                    if (bDBClose == true) DisConnect();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
