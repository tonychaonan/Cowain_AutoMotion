using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightUI
{
  public  class PathClass
    {
      private string CMDStr1 = "";
      public string CMDStr
      {
          get
          {
              return CMDStr1;
          }
          set
          {
              CMDStr1 = value;
              if (CMDStr1 == "")
              {
                  return;
              }
              strsList.Add(CMDStr1);
          }
      }
      public string CameraStr1;
      public string CameraStr
      {
          get
          {
              return CameraStr1;
          }
          set
          {
              CameraStr1 = value;
              if (CameraStr1 == "")
              {
                  return;
              }
              strsList.Add(CameraStr1);
          }
      }
      public string CalibrationStr1 = "";
      public string CalibrationStr
      {
          get
          {
              return CalibrationStr1;
          }
          set
          {
              CalibrationStr1 = value;
              if (CalibrationStr1 == "")
              {
                  return;
              }
              strsList.Add(CalibrationStr1);
          }
      }
      public string CalibrationStrSecond1 = "";
      public string CalibrationStrSecond
      {
          get
          {
              return CalibrationStrSecond1;
          }
          set
          {
              CalibrationStrSecond1 = value;
              if (CalibrationStrSecond1 == "")
              {
                  return;
              }
              strsList.Add(CalibrationStrSecond1);
          }
      }
      public string InspectionStr1 = "";
      public string InspectionStr
      {
          get
          {
              return InspectionStr1;
          }
          set
          {
              InspectionStr1 = value;
              if (InspectionStr1 == "")
              {
                  return;
              }
              strsList.Add(InspectionStr);
          }
      }
      public List<string> strsList = new List<string>();

      public PathClass copy( )
      {
          PathClass other = new PathClass();
          other.CMDStr = CMDStr;
          other.CameraStr = CameraStr;
          other.CalibrationStr = CalibrationStr;
          other.InspectionStr = InspectionStr;
          for (int i = 0; i < strsList.Count; i++)
          {
              other.strsList.Add(strsList[i]);
          }
          return other;
      }
    }
}
