using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.Odbc;

namespace MotionBase
{
  public class Sys_DataBase
   {
      private System.Data.Common.DbConnection DBConn = null;
      private System.Data.Common.DbCommand DBCommand = null;
      private System.Data.Common.DbTransaction DBTran = null;
      //private System.Data.Common.DbDataAdapter DBDataAdapter = null;
      private string DBPath = "";

      public enum enTrans
      {
         BEGIN = 1,
         COMMIT = 2,
         ROLLBACK = 3
      }

      public bool Fun_CloseDB()
      {
         try
         {
            DBConn.Close();
            return true;
         }
         catch(Exception)
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
         catch(Exception)
         {
            return false;
         }
      }

      public bool Fun_ExecSQL(string sSQL, bool bDBClose = false)
      {
         try
         {
            sSQL = sSQL.Replace("''", "' '");

            if(this.DBConn == null)
            {
               if(this.Fun_OpenDataBase() == false) return false;
            }
            else
            {
               if(this.DBConn.State != System.Data.ConnectionState.Open)
               {
                  if(this.Fun_OpenDataBase() == false) return false;
               }
            }

            if(DBConn.State != System.Data.ConnectionState.Open) return false;

            this.DBCommand.Connection = DBConn;
            this.DBCommand.CommandType = System.Data.CommandType.Text;
            this.DBCommand.CommandText = sSQL;
            this.DBCommand.Transaction = DBTran;

            if(DBCommand.ExecuteNonQuery() <= 0)
            {
               if(bDBClose == true) Fun_CloseDB();
               return false;
            }
            else
            {
               if(bDBClose == true) Fun_CloseDB();
               return true;
            }
         }
         catch(Exception)
         {
            return false;
         }
      }

      public bool Fun_OpenDataBase()
      {
         String sConnect = "";

         try
         {
            if(this.DBConn != null) if(this.DBConn.State == System.Data.ConnectionState.Open) this.DBConn.Close();


            if(IntPtr.Size == 4)
                sConnect = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DBPath;  // +";" + "Persist Security Info=True;";
            else if(IntPtr.Size == 8)
                sConnect = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + DBPath; // +";" + "Persist Security Info=True;"
            //-----------------------------
            DBConn = new OleDbConnection();
            DBCommand = new OleDbCommand();

            DBConn.ConnectionString = sConnect; 
            DBConn.Open();

            return true;
         }
         catch(Exception)
         {
            return false;
         }
      }

      public bool Fun_RsSQL(string sSQL, ref System.Data.Common.DbDataReader retRS, bool bDBClose = false)
      {
         try
         {
            sSQL = sSQL.Replace("''", "' '");

            if(this.DBConn == null)
            {
               if(this.Fun_OpenDataBase() == false) return false;
            }
            else
            {
               if(this.DBConn.State != System.Data.ConnectionState.Open)
               {
                  if(this.Fun_OpenDataBase() == false) return false;
               }
            }

            if(DBConn.State != System.Data.ConnectionState.Open) return false;

            DBCommand.Connection = DBConn;
            DBCommand.CommandText = sSQL;


            retRS = DBCommand.ExecuteReader();
            if(retRS.HasRows == false)
            {
               retRS.Close();
               if(bDBClose == true) Fun_CloseDB();
               return false;
            }
            else
            {
               if(bDBClose == true) Fun_CloseDB();
               return true;
            }
         }
         catch(Exception)
         {
            return false;
         }
      }

      public bool Fun_TransCTRL(enTrans iTrans, System.Data.IsolationLevel LockLevel = System.Data.IsolationLevel.Unspecified, bool bDBClose = false)
      {
         try
         {
            if(this.DBConn == null)
            {
               if(this.Fun_OpenDataBase() == false) return false;
            }
            else
            {
               if(this.DBConn.State != System.Data.ConnectionState.Open)
               {
                  if(this.Fun_OpenDataBase() == false) return false;
               }
            }

            if(DBConn.State != System.Data.ConnectionState.Open) return false;

            switch(iTrans)
            {
               case enTrans.BEGIN:
                  if(LockLevel == System.Data.IsolationLevel.Unspecified)
                     DBTran = DBConn.BeginTransaction();
                  else
                     DBTran = DBConn.BeginTransaction(LockLevel);
                  break;
               case enTrans.COMMIT:
                  DBTran.Commit();
                  break;
               case enTrans.ROLLBACK:
                  DBTran.Rollback();
                  break;
               default:
                  return false;
            }

            if(bDBClose == true && (iTrans == enTrans.COMMIT || iTrans == enTrans.ROLLBACK)) Fun_CloseDB();

            return false;
         }
         catch(Exception)
         {
            DBTran.Rollback();
            return false;
         }
      }

      public bool Fun_GetDBConnectStatus()
      {
         if(DBConn.State == System.Data.ConnectionState.Open) return true; else return false;
      }

      public Sys_DataBase(string Path)
      {
         this.DBPath =Path;
      }
   }
}
