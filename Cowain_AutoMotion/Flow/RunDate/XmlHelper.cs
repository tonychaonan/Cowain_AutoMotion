using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Linq;

namespace Cowain_AutoDispenser
{
    //public class XmlHelper<T> where T : class, new()
    //{
    //    public static T XmlFromFile(string filename)
    //    {
    //        if (!File.Exists(filename))
    //        {
    //            return null;
    //        }
    //        using (FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read))
    //        {

    //            XmlSerializer serialReader = new XmlSerializer(typeof(T));
    //            return serialReader.Deserialize(file) as T;
    //        }
    //    }
    //    public static void XmlToFile(string filename, T obj)
    //    {
    //        using (FileStream file = new FileStream(filename, FileMode.Create, FileAccess.Write))
    //        {
    //            XmlSerializer serialWriter = new XmlSerializer(typeof(T));
    //            serialWriter.Serialize(file, obj);
    //        }
    //    }
    //}
}
