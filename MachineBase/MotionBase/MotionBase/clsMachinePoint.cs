using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionBase
{
    public class clsMachinePoint : Base
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable;
        public string TableName = "MachineData";
        /// <summary>
        /// 点位名称
        /// </summary>
        public string PointName;
        /// <summary>
        /// 新的点位名称
        /// </summary>
        public string NewPointName = "";
        /// <summary>
        /// 数据库路径
        /// </summary>
        public static string Path;
        public Sys_Define.tyAXIS_XYZRA Point;
        public clsMachinePoint(Type homeEnum1, Type stepEnum1, string instanceName1, string name):base(homeEnum1,stepEnum1,instanceName1)
        {
            Enable = false;

            Point = new Sys_Define.tyAXIS_XYZRA();
            ReadPoint(name);
        }

        public bool ReadPoint(string name)
        {
            try
            {
                double EnableData = 0;
                PointName = name;
                GetDataBaseData(Path, TableName, "CName", name, "Enable", ref EnableData);
                GetDataBaseData(Path, TableName, "CName", name, "Data1", ref Point.X);
                GetDataBaseData(Path, TableName, "CName", name, "Data2", ref Point.Y);
                GetDataBaseData(Path, TableName, "CName", name, "Data3", ref Point.Z);
                GetDataBaseData(Path, TableName, "CName", name, "Data4", ref Point.R);
                GetDataBaseData(Path, TableName, "CName", name, "Data5", ref Point.A);
                Enable = EnableData == 0 ? false : true;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 保存数据，参数有路径
        /// </summary>
        /// <returns></returns>
        public bool SavePoint()
        {
            try
            {
                SaveToDataBase(Path, TableName, "CName", PointName, "Enable", Enable ? "1" : "0");
                SaveToDataBase(Path, TableName, "CName", PointName, "Data1", Point.X.ToString());
                SaveToDataBase(Path, TableName, "CName", PointName, "Data2", Point.Y.ToString());
                SaveToDataBase(Path, TableName, "CName", PointName, "Data3", Point.Z.ToString());
                SaveToDataBase(Path, TableName, "CName", PointName, "Data4", Point.R.ToString());
                SaveToDataBase(Path, TableName, "CName", PointName, "Data5", Point.A.ToString());
                if (NewPointName != "")
                {
                    SaveToDataBase(Path, TableName, "CName", PointName, "CName", NewPointName);
                    PointName = NewPointName;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
