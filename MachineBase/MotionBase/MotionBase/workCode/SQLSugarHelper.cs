using SqlSugar.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SqlSugar;
using System.Windows.Forms;

namespace Cowain_AutoMotion
{
    public class SQLSugarHelper
    {
        public class DBContext<T> where T : class, new()
        {
            public SqlSugarClient Db;
            private static DBContext<T> mSingle = null;
            private static DBContext<T> m_Source = null;
            public static DBContext<T> GetInstance(string path)
            {
                if (m_Source == null)
                    m_Source = new DBContext<T>(path);
                return m_Source;
            }
            public static DBContext<T> GetInstance()
            {
                string path = Application.StartupPath.Replace(@"Cowain_AutoMotion\bin\x64\Debug", @"DataBaseData\Machine.mdb");         // @"D:\Cowain_AutoMotion\DataBaseData\Machine.mdb";
                if (mSingle == null)
                    mSingle = new DBContext<T>(path);
                return mSingle;
            }
            protected DBContext(string path)
            {  //通过这个可以直接连接数据库
                Db = new SqlSugarClient(new ConnectionConfig()
                {
                    // ConnectionString = "provider=Microsoft.Jet.OLEDB.12.0;" + @"Data Source=D:\Cowain_AutoMotion\DataBaseData\Machine.mdb",
                    //ConnectionString = "Provider = Microsoft.ACE.OLEDB.12.0;" + @"Data Source=D:\Cowain_AutoMotion\DataBaseData\Machine.mdb",
                    ConnectionString = "Provider = Microsoft.ACE.OLEDB.12.0;" + @"Data Source="+ path,

                    DbType = SqlSugar.DbType.Access,
                    IsAutoCloseConnection = true,//自动关闭连接
                    InitKeyType = InitKeyType.Attribute
                });
                //调式代码 用来打印SQL
                //Db.Aop.OnLogExecuting = (Access, pars) =>
                //{
                //    Console.WriteLine(Access + "\r\n" +
                //        Db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                //    Console.WriteLine();
                //};
            }

            public void Dispose()
            {
                if (Db != null)
                {
                    Db.Dispose();
                }
            }
            public SimpleClient<T> CurrentDb { get { return new SimpleClient<T>(Db); } }

            /// <summary>
            /// 获取所有
            /// </summary>
            /// <returns></returns>
            public virtual List<T> GetList()
            {
                if (isExistTable(typeof(T)) != true)
                {
                    creatTable(typeof(T));
                }
                return CurrentDb.GetList();
            }

            /// <summary>
            /// 根据表达式查询
            /// </summary>
            /// <returns></returns>
            public virtual List<T> GetList(Expression<Func<T, bool>> whereExpression)
            {
                return CurrentDb.GetList(whereExpression);
            }


            /// <summary>
            /// 根据表达式查询分页
            /// </summary>
            /// <returns></returns>
            public virtual List<T> GetPageList(Expression<Func<T, bool>> whereExpression, PageModel pageModel)
            {
                return CurrentDb.GetPageList(whereExpression, pageModel);
            }

            /// <summary>
            /// 根据表达式查询分页并排序
            /// </summary>
            /// <param name="whereExpression">it</param>
            /// <param name="pageModel"></param>
            /// <param name="orderByExpression">it=>it.id或者it=>new{it.id,it.name}</param>
            /// <param name="orderByType">OrderByType.Desc</param>
            /// <returns></returns>
            public virtual List<T> GetPageList(Expression<Func<T, bool>> whereExpression, PageModel pageModel, Expression<Func<T, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc)
            {
                return CurrentDb.GetPageList(whereExpression, pageModel, orderByExpression, orderByType);
            }


            /// <summary>
            /// 根据主键查询
            /// </summary>
            /// <returns></returns>
            public virtual List<T> GetById(dynamic id)
            {
                return CurrentDb.GetById(id);
            } 

            /// <summary>
            /// 根据主键删除
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public virtual bool Delete(dynamic id)
            {
                if (string.IsNullOrEmpty(id.ObjToString))
                {
                    Console.WriteLine(string.Format("要删除的主键id不能为空值！"));
                }
                return CurrentDb.Delete(id);
            }


            /// <summary>
            /// 根据实体删除
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public virtual bool Delete(T data)
            {
                if (data == null)
                {
                    Console.WriteLine(string.Format("要删除的实体对象不能为空值！"));
                }
                return CurrentDb.Delete(data);
            }

            /// <summary>
            /// 根据主键删除
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public virtual bool Delete(dynamic[] ids)
            {
                if (ids.Count() <= 0)
                {
                    Console.WriteLine(string.Format("要删除的主键ids不能为空值！"));
                }
                return CurrentDb.AsDeleteable().In(ids).ExecuteCommand() > 0;
            }

            /// <summary>
            /// 根据表达式删除
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public virtual bool Delete(Expression<Func<T, bool>> whereExpression)
            {
                return CurrentDb.Delete(whereExpression);
            }
            /// <summary>
            /// 根据实体更新，实体需要有主键
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public virtual bool Update(T obj)
            {
                if (obj == null)
                {
                    Console.WriteLine(string.Format("要更新的实体不能为空，必须带上主键！"));
                }
                return CurrentDb.Update(obj);
            }

            /// <summary>
            ///批量更新
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public virtual bool Update(List<T> objs)
            {
                if (objs.Count <= 0)
                {
                    Console.WriteLine(string.Format("要批量更新的实体不能为空，必须带上主键！"));
                }
                return CurrentDb.UpdateRange(objs);
            }

            /// <summary>
            /// 插入
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public virtual bool Insert(T obj)
            {
                return CurrentDb.Insert(obj);
            }
            /// <summary>
            /// 批量
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public virtual bool Insert(List<T> objs)
            {
                return CurrentDb.InsertRange(objs);
            }
            /// <summary>
            /// 创建表
            /// </summary>
            /// <param name="type"></param>
            public void creatTable(Type type)
            {
                CurrentDb.Context.CodeFirst.InitTables(type);
            }
            public bool isExistTable(Type type)
            {
                string tableName = ((SugarTable)(type.GetCustomAttributes(true)[0])).TableName;
                return Db.DbMaintenance.IsAnyTable(tableName);
            }

            /// <summary>
            /// 根据实体更新，实体需要有主键
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public virtual bool UpdateOrInsert(List<T> objs)
            {
                if (objs == null)
                {
                    Console.WriteLine(string.Format("要更新的实体不能为空，必须带上主键！"));
                }
                return CurrentDb.InsertOrUpdate(objs);
            }
            //public int Update(string columnName, int value)
            //{
            //    int i = -1;
            //    try
            //    {
            //            i = Db.Updateable<SNList>().AS("StationResult").SetColumns(columnName, value).Where($"ID={0}").ExecuteCommand();
            //    }
            //    catch (Exception e)
            //    {
            //        LogClass.AddMsg(0, e.Message);
            //    }
            //    return i;
            //}
            //可以扩展更多方法 
        }
    }
}
