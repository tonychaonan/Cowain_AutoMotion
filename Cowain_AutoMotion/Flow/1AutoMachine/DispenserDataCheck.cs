using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cowain_AutoMotion.Flow._1AutoMachine
{
    public class DispenserPoint
    {
        public double[] point = new double[] { 0, 0, 0, 0, 0, 0, 0, 0 };
        public DispenserPoint(DataGridViewRow row)
        {
            point[0] = getValue(row.Cells[4].Value);
            point[1] = getValue(row.Cells[5].Value);
            point[2] = getValue(row.Cells[6].Value);
            point[3] = getValue(row.Cells[8].Value);
            point[4] = getValue(row.Cells[9].Value);
            point[5] = getValue(row.Cells[10].Value);
            point[6] = getValue(row.Cells[11].Value);
            point[7] = getValue(row.Cells[12].Value);
        }
        public bool judgeDispenserPoint(DispenserPoint dispenserPoint, double difMaxValue,int rowIndex, ref string MSG)
        {
            string msg = "";
            bool b_Result = true;
            for (int i = 0; i < point.Length; i++)
            {
                double dif = Math.Abs(point[i] - dispenserPoint.point[i]);
                if (dif > difMaxValue)
                {
                    msg += "在第"+(rowIndex+1).ToString()+"行：数据改变前为" + point[i].ToString() + "," + "数据改变后为" + dispenserPoint.point[i].ToString() + ",超出设定差值" + difMaxValue.ToString() + "\r\n";
                    b_Result = false;
                }
            }
            MSG = msg;
            return b_Result;
        }
        private double getValue(object obj)
        {
            double value = 0;
            try
            {
                double.TryParse(obj.ToString(), out value);
            }
            catch
            {

            }
            return value;
        }
    }
    public class DispenserDataCheck
    {
        private static List<DispenserPoint> lastDispenserPoints = new List<DispenserPoint>();
        public static void addDispenserPoint(DataGridViewRowCollection collection)
        {
            lastDispenserPoints.Clear();
            foreach (DataGridViewRow item in collection)
            {
                DispenserPoint dispenserPoint = new _1AutoMachine.DispenserPoint(item);
                lastDispenserPoints.Add(dispenserPoint);
            }
        }
        public static bool JudgeDataChanged(DataGridViewRowCollection collection, ref string showErrMSG)
        {
            bool b_Result = true;
            string errMSG = "";
            try
            {
                for (int i = 0; i < collection.Count; i++)
                {
                    DispenserPoint dispenserPoint = new _1AutoMachine.DispenserPoint(collection[i]);
                    string errMSG11 = "";
                    bool b_Result11 = lastDispenserPoints[i].judgeDispenserPoint(dispenserPoint, 2,i, ref errMSG11);
                    if (b_Result11 != true)
                    {
                        errMSG += errMSG11;
                        b_Result = false;
                    }
                }
            }
            catch
            {
                b_Result = false;
            }
            if (b_Result == false)
            {
                if(errMSG.Length>200)
                {
                    errMSG = errMSG.Substring(0, 200);
                }
                showErrMSG = errMSG;
                showErrMSG += "选择 是 则保存胶路，选择 否 则不保存胶路";
            }
            return b_Result;
        }
    }
}
