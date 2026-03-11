using Cowain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;


namespace ReadAndWriteConfig
{
    public class ConfigHelper
    {
        public static string xmlPath;
        public static XmlDocument xmlDoc = null;
        public ConfigHelper(string XMLName)
        {
            xmlPath = XMLName; //getXMLPath(XMLName);
            if (xmlPath == "")
            {
                MessageBox.Show(JudgeLanguage.JudgeLag("配置文件不存在！"));
            }
        }
        private static object locker = new object();
        public void createXML(string XMLPath)
        {
            try
            {
                lock (locker)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    if (File.Exists(XMLPath) != true)
                    {
                        using (FileStream fs = new FileStream(xmlPath, FileMode.Create, FileAccess.Write))
                        {

                        }
                        xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null));
                        XmlElement xml = xmlDoc.CreateElement("configuration");
                        xmlDoc.AppendChild(xml);
                        xmlDoc.Save(xmlPath);
                    }
                    else
                    {
                        //配置文件已经存在，不做操作
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
                MessageBox.Show(JudgeLanguage.JudgeLag("创建配置文件失败！"));
            }

        }
        private List<string> GetNodeName()
        {
            List<string> nodeName = new List<string>();
            try
            {
                lock (locker)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    if (File.Exists(xmlPath))
                    {
                        xmlDoc.Load(xmlPath);
                        XmlNode rootNode = xmlDoc.DocumentElement;
                        foreach (XmlNode stuNode in rootNode.ChildNodes)
                        {
                            nodeName.Add(stuNode.Name);
                        }
                    }
                    else
                    {
                        MessageBox.Show(JudgeLanguage.JudgeLag("配置文件不存在！"));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
                MessageBox.Show(JudgeLanguage.JudgeLag("读取配置文件失败！"));
            }
            return nodeName;
        }
        public static List<string> GetKeyAndValue(string nodeName)
        {
            List<string> keyAndValue = new List<string>();
            try
            {
                lock (locker)
                {
                    List<string> keyList = GetAllKey(nodeName);
                    if (keyList.Count == 0)
                    {
                        keyAndValue.Add("Key" + ",Value");
                        return keyAndValue;
                    }
                    List<string> attributesList = GetAppConfigAttributes(nodeName, keyList[0]);
                    if (attributesList.Count > 0)
                    {
                        string attributes = "";
                        for (int i = 0; i < attributesList.Count; i++)
                        {
                            if (i == attributesList.Count - 1)
                            {
                                attributes += attributesList[i];
                            }
                            else
                            {
                                attributes += attributesList[i] + ",";
                            }
                        }
                        keyAndValue.Add("Key," + attributes);
                        //遍历attributes的值
                        foreach (string item in keyList)
                        {
                            List<string> attributesList1 = GetAppConfigAttributes(nodeName, item);
                            string attributes1 = "";
                            for (int i = 0; i < attributesList1.Count; i++)
                            {
                                if (i == attributesList1.Count - 1)
                                {
                                    attributes1 += GetAppConfig(nodeName, item, attributesList1[i]);
                                }
                                else
                                {
                                    attributes1 += GetAppConfig(nodeName, item, attributesList1[i]) + ",";
                                }
                            }
                            keyAndValue.Add(item + "," + attributes1);
                        }
                    }
                    else
                    {
                        keyAndValue.Add("Key," + "Value");
                        foreach (string item in keyList)
                        {
                            string attributesList1 = GetAppConfig(nodeName, item);
                            keyAndValue.Add(item + "," + attributesList1);
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
                MessageBox.Show(JudgeLanguage.JudgeLag("配置文件读取错误,请检查是否有误，并重启软件！"));
            }
            return keyAndValue;
        }

        public static List<string> GetKeyAndValue2(string nodeName)
        {
            List<string> keyAndValue = new List<string>();
            try
            {
                lock (locker)
                {
                    List<string> keyList = GetAllKey(nodeName);
                    if (keyList.Count == 0)
                    {
                        keyAndValue.Add("Key" + ";Value");
                        return keyAndValue;
                    }
                    List<string> attributesList = GetAppConfigAttributes(nodeName, keyList[0]);
                    if (attributesList.Count > 0)
                    {
                        string attributes = "";
                        for (int i = 0; i < attributesList.Count; i++)
                        {
                            if (i == attributesList.Count - 1)
                            {
                                attributes += attributesList[i];
                            }
                            else
                            {
                                attributes += attributesList[i] + ";";
                            }
                        }
                        keyAndValue.Add("Key;" + attributes);
                        //遍历attributes的值
                        foreach (string item in keyList)
                        {
                            List<string> attributesList1 = GetAppConfigAttributes(nodeName, item);
                            string attributes1 = "";
                            for (int i = 0; i < attributesList1.Count; i++)
                            {
                                if (i == attributesList1.Count - 1)
                                {
                                    attributes1 += GetAppConfig(nodeName, item, attributesList1[i]);
                                }
                                else
                                {
                                    attributes1 += GetAppConfig(nodeName, item, attributesList1[i]) + ";";
                                }
                            }
                            keyAndValue.Add(item + ";" + attributes1);
                        }
                    }
                    else
                    {
                        keyAndValue.Add("Key;" + "Value");
                        foreach (string item in keyList)
                        {
                            string attributesList1 = GetAppConfig(nodeName, item);
                            keyAndValue.Add(item + ";" + attributesList1);
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
                MessageBox.Show(JudgeLanguage.JudgeLag("配置文件读取错误,请检查是否有误，并重启软件！"));
            }
            return keyAndValue;
        }
        public static List<string> GetAppConfigAttributes(string nodeName, string key)
        {
            List<string> attributesList = new List<string>();
            try
            {
                lock (locker)
                {
                    if (File.Exists(xmlPath))
                    {
                        if (xmlDoc == null)
                        {
                            xmlDoc = new XmlDocument();
                            xmlDoc.Load(xmlPath);
                        }
                        XmlNode rootNode = xmlDoc.DocumentElement;
                        foreach (XmlNode stuNode in rootNode.ChildNodes)
                        {
                            if (stuNode.Name == nodeName)
                            {
                                foreach (XmlNode subNode in stuNode)
                                {
                                    if (subNode.Name == key)
                                    {
                                        if (subNode.Attributes.Count > 0)
                                        {
                                            foreach (XmlAttribute item in subNode.Attributes)
                                            {
                                                attributesList.Add(item.Name);
                                            }
                                        }
                                        else
                                        {
                                            return attributesList;
                                        }
                                        return attributesList;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(JudgeLanguage.JudgeLag("配置文件不存在"));
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
                MessageBox.Show(JudgeLanguage.JudgeLag("配置文件读取错误,请检查是否有误，并重启软件！"));
            }
            MessageBox.Show(JudgeLanguage.JudgeLag("配置文件读取错误,请检查是否有误，并重启软件！nodeName:") + nodeName + ",Key:" + key);
            return null;
        }
        public static string GetAppConfig(string nodeName, string key, string Attributes)
        {
            try
            {
                lock (locker)
                {
                    if (File.Exists(xmlPath))
                    {
                        if (xmlDoc == null)
                        {
                            xmlDoc = new XmlDocument();
                            xmlDoc.Load(xmlPath);
                        }
                        XmlNode rootNode = xmlDoc.DocumentElement;
                        foreach (XmlNode stuNode in rootNode.ChildNodes)
                        {
                            if (stuNode.Name == nodeName)
                            {
                                foreach (XmlNode subNode in stuNode)
                                {
                                    if (subNode.Name == key)
                                    {
                                        foreach (XmlAttribute item in subNode.Attributes)
                                        {
                                            if (item.Name == Attributes)
                                            {
                                                return subNode.Attributes[Attributes].Value;
                                            }
                                        }
                                        return "";
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(JudgeLanguage.JudgeLag("配置文件不存在"));
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
                MessageBox.Show(JudgeLanguage.JudgeLag("配置文件读取错误,请检查是否有误，并重启软件！"));
            }
            MessageBox.Show(JudgeLanguage.JudgeLag("配置文件读取错误,请检查是否有误，并重启软件！nodeName:") + nodeName + ",Key:" + key);
            return null;
        }
        public static string GetAppConfig(string nodeName, string key)
        {
            try
            {
                lock (locker)
                {
                    if (File.Exists(xmlPath))
                    {
                        if (xmlDoc == null)
                        {
                            xmlDoc = new XmlDocument();
                            xmlDoc.Load(xmlPath);
                        }
                        XmlNode rootNode = xmlDoc.DocumentElement;
                        foreach (XmlNode stuNode in rootNode.ChildNodes)
                        {
                            if (stuNode.Name == nodeName)
                            {
                                foreach (XmlNode subNode in stuNode)
                                {
                                    if (subNode.Name == key)
                                    {
                                        return subNode.InnerText;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(JudgeLanguage.JudgeLag("配置文件不存在"));
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
                MessageBox.Show(JudgeLanguage.JudgeLag("配置文件读取错误,请检查是否有误，并重启软件！"));
            }
            MessageBox.Show(JudgeLanguage.JudgeLag("配置文件读取错误,请检查是否有误，并重启软件！nodeName:") + nodeName + ",Key:" + key);
            return null;
        }
        public void UpdateAppConfig(string nodeName, string Key, string Attributes, string newValue)
        {
            try
            {
                lock (locker)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    if (File.Exists(xmlPath))
                    {
                        xmlDoc.Load(xmlPath);
                        XmlNode rootNode = xmlDoc.DocumentElement;
                        foreach (XmlNode stuNode in rootNode.ChildNodes)
                        {
                            if (stuNode.Name == nodeName)
                            {
                                foreach (XmlNode subNode in stuNode)
                                {
                                    if (subNode.Name == Key)
                                    {
                                        foreach (XmlAttribute item in subNode.Attributes)
                                        {
                                            if (item.Name == Attributes)
                                            {
                                                item.InnerText = newValue;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        xmlDoc.Save(xmlPath);
                    }
                    else
                    {
                        MessageBox.Show(JudgeLanguage.JudgeLag("配置文件不存在"));
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
                MessageBox.Show(JudgeLanguage.JudgeLag("写入配置文件失败！"));
            }

        }
        public static void UpdateAppConfig(string nodeName, string Key, string newValue)
        {
            try
            {

                lock (locker)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    if (File.Exists(xmlPath))
                    {
                        xmlDoc.Load(xmlPath);
                        XmlNode rootNode = xmlDoc.DocumentElement;
                        foreach (XmlNode stuNode in rootNode.ChildNodes)
                        {
                            if (stuNode.Name == nodeName)
                            {
                                foreach (XmlNode subNode in stuNode)
                                {
                                    if (subNode.Name == Key)
                                    {
                                        subNode.InnerText = newValue;
                                    }
                                }
                            }
                        }
                        xmlDoc.Save(xmlPath);
                    }
                    else
                    {
                        MessageBox.Show(JudgeLanguage.JudgeLag("配置文件不存在"));
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
                MessageBox.Show(JudgeLanguage.JudgeLag("写入配置文件失败！"));
            }

        }
        public void AddAppConfig(string nodeName)
        {
            try
            {
                lock (locker)
                {
                    if (JudgeKey(nodeName) != true)
                    {
                        return;
                    }
                    XmlDocument xmlDoc = new XmlDocument();
                    if (File.Exists(xmlPath))
                    {
                        xmlDoc.Load(xmlPath);
                        XmlNode rootNode = xmlDoc.DocumentElement;
                        foreach (XmlNode stuNode in rootNode.ChildNodes)
                        {
                            if (stuNode.Name == nodeName)
                            {
                                return;
                            }
                        }
                        XmlElement xml = xmlDoc.CreateElement(nodeName);
                        rootNode.AppendChild(xml);
                        xmlDoc.Save(xmlPath);
                    }
                    else
                    {
                        MessageBox.Show(JudgeLanguage.JudgeLag("配置文件不存在"));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
                MessageBox.Show(JudgeLanguage.JudgeLag("写入配置文件失败！"));
            }
        }
        public void AddAppConfig(string nodeName, string newKey, string attributes, string newValue)
        {
            try
            {
                lock (locker)
                {
                    if (JudgeKey(nodeName) != true)
                    {
                        return;
                    }
                    if (JudgeKey(newKey) != true)
                    {
                        return;
                    }
                    if (JudgeKey(attributes) != true)
                    {
                        return;
                    }
                    XmlDocument xmlDoc = new XmlDocument();
                    if (File.Exists(xmlPath))
                    {
                        xmlDoc.Load(xmlPath);
                        XmlElement member = xmlDoc.DocumentElement;
                        XmlNode xmlNode = member.SelectSingleNode(nodeName);
                        //  XmlNode rootNode = xmlDoc.DocumentElement;
                        foreach (XmlElement stuNode in member.ChildNodes)
                        {
                            if (stuNode.Name == nodeName)
                            {
                                foreach (XmlNode subNode in stuNode)
                                {
                                    if (subNode.Name == newKey)
                                    {
                                        bool b_Exist = false;
                                        foreach (XmlAttribute item in subNode.Attributes)
                                        {
                                            if (item.Name == attributes)
                                            {
                                                item.Value = newValue;
                                                b_Exist = true;
                                            }
                                        }
                                        if (b_Exist != true)//说明存在节点，而不存在这个属性，则添加这个属性到已存在的节点
                                        {
                                            XmlAttribute xmlAtt = xmlDoc.CreateAttribute(attributes);
                                            xmlAtt.Value = newValue;
                                            subNode.Attributes.Append(xmlAtt);
                                            //  stuNode.SetAttribute(attributes, newValue);
                                        }
                                        xmlDoc.Save(xmlPath);
                                        return;
                                    }
                                }
                            }
                        }
                        //member.SetAttribute("AGE", "<29");
                        //member.SetAttribute("SEX", "MAIL");
                        //当不存在节点和属性时则新建
                        XmlElement lq = xmlDoc.CreateElement(newKey);
                        xmlNode.AppendChild(lq);
                        XmlAttribute xmlAtt1 = xmlDoc.CreateAttribute(attributes);
                        xmlAtt1.InnerText = newValue;
                        foreach (XmlElement stuNode in member.ChildNodes)
                        {
                            if (stuNode.Name == nodeName)
                            {
                                stuNode[newKey].Attributes.Append(xmlAtt1);
                            }
                        }
                        //  lq.InnerText = newValue;
                        //  rootNode.AppendChild(member);
                        xmlDoc.Save(xmlPath);
                    }
                    else
                    {
                        MessageBox.Show(JudgeLanguage.JudgeLag("配置文件不存在"));
                    }
                }
            }
            catch (Exception ex)
            {
                // throw ex;
                MessageBox.Show(JudgeLanguage.JudgeLag("配置文件写入错误,请检查是否有误，并重启软件！"));
            }
        }
        public void AddAppConfig(string nodeName, string newKey, string newValue)
        {
            try
            {
                lock (locker)
                {
                    if (JudgeKey(nodeName) != true)
                    {
                        return;
                    }
                    if (JudgeKey(newKey) != true)
                    {
                        return;
                    }
                    XmlDocument xmlDoc = new XmlDocument();
                    if (File.Exists(xmlPath))
                    {
                        xmlDoc.Load(xmlPath);
                        XmlElement member = xmlDoc.DocumentElement;
                        XmlNode xmlNode = member.SelectSingleNode(nodeName);
                        //  XmlNode rootNode = xmlDoc.DocumentElement;
                        foreach (XmlElement stuNode in member.ChildNodes)
                        {
                            if (stuNode.Name == nodeName)
                            {
                                foreach (XmlNode subNode in stuNode)
                                {
                                    if (subNode.Name == newKey)
                                    {
                                        //  DialogResult productSelect = MessageBox.Show("名称已存在，是否覆盖？", "名称存在", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                                        //  if (productSelect == DialogResult.Yes)
                                        //   {
                                        subNode.InnerText = newValue;
                                        xmlDoc.Save(xmlPath);
                                        //     }
                                        //       else
                                        //      {

                                        //       }
                                        return;
                                    }
                                }
                            }
                        }
                        //member.SetAttribute("AGE", "<29");
                        //member.SetAttribute("SEX", "MAIL");
                        XmlElement lq = xmlDoc.CreateElement(newKey);
                        lq.InnerText = newValue;
                        xmlNode.AppendChild(lq);
                        //  rootNode.AppendChild(member);
                        xmlDoc.Save(xmlPath);
                    }
                    else
                    {
                        MessageBox.Show(JudgeLanguage.JudgeLag("配置文件不存在"));
                    }

                }


            }
            catch (Exception ex)
            {
                // throw ex;
                MessageBox.Show(JudgeLanguage.JudgeLag("配置文件写入错误,请检查是否有误，并重启软件！"));
            }
        }
        private List<string> GetAllConfigInNode(string nodeName)
        {
            List<string> nodeString = new List<string>();
            try
            {
                lock (locker)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    if (File.Exists(xmlPath))
                    {
                        xmlDoc.Load(xmlPath);
                        XmlNode rootNode = xmlDoc.DocumentElement;
                        foreach (XmlNode stuNode in rootNode.ChildNodes)
                        {
                            if (stuNode.Name == nodeName)
                            {
                                foreach (XmlNode subNode in stuNode)
                                {
                                    nodeString.Add(subNode.Name);
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(JudgeLanguage.JudgeLag("配置文件不存在"));
                    }
                }
            }
            catch
            {
                MessageBox.Show(JudgeLanguage.JudgeLag("读取NODE:") + nodeName + JudgeLanguage.JudgeLag("失败"));
            }
            return nodeString;
        }
        public List<string> GetAllKey()
        {
            List<string> keyString = new List<string>();
            try
            {
                lock (locker)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    if (File.Exists(xmlPath))
                    {
                        xmlDoc.Load(xmlPath);
                        XmlNode rootNode = xmlDoc.DocumentElement;
                        foreach (XmlNode stuNode in rootNode.ChildNodes)
                        {
                            keyString.Add(stuNode.Name);
                        }
                    }
                    else
                    {
                        MessageBox.Show(JudgeLanguage.JudgeLag("配置文件不存在"));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
                MessageBox.Show(JudgeLanguage.JudgeLag("配置文件读取错误,请检查是否有误，并重启软件！"));
            }
            return keyString;
        }
        public static List<string> GetAllKey(string nodeName)
        {
            List<string> keyString = new List<string>();
            try
            {
                lock (locker)
                { 
                    if (File.Exists(xmlPath))
                    {
                        if (xmlDoc == null)
                        {
                            xmlDoc = new XmlDocument();
                            xmlDoc.Load(xmlPath);
                        }
                        XmlNode rootNode = xmlDoc.DocumentElement;
                        foreach (XmlNode stuNode in rootNode.ChildNodes)
                        {
                            if (stuNode.Name == nodeName)
                            {
                                foreach (XmlNode subNode in stuNode)
                                {
                                    keyString.Add(subNode.Name);
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(JudgeLanguage.JudgeLag("配置文件不存在"));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
                MessageBox.Show(JudgeLanguage.JudgeLag("配置文件读取错误,请检查是否有误，并重启软件！"));
            }
            return keyString;
        }
        public void DeleteConfig(string nodeName)
        {
            try
            {
                lock (locker)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    if (File.Exists(xmlPath))
                    {
                        xmlDoc.Load(xmlPath);
                        XmlNode rootNode = xmlDoc.DocumentElement;
                        foreach (XmlNode stuNode in rootNode.ChildNodes)
                        {
                            if (stuNode.Name == nodeName)
                            {
                                rootNode.RemoveChild(stuNode);
                                xmlDoc.Save(xmlPath);
                                return;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(JudgeLanguage.JudgeLag("配置文件不存在"));
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
                MessageBox.Show(JudgeLanguage.JudgeLag("配置文件读取错误,请检查是否有误，并重启软件！"));
            }
        }
        public void DeleteConfig(string nodeName, string key)
        {
            try
            {
                lock (locker)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    if (File.Exists(xmlPath))
                    {
                        xmlDoc.Load(xmlPath);
                        XmlNode rootNode = xmlDoc.DocumentElement;
                        foreach (XmlNode stuNode in rootNode.ChildNodes)
                        {
                            if (stuNode.Name == nodeName)
                            {
                                foreach (XmlNode subNode in stuNode)
                                {
                                    if (subNode.Name == key)
                                    {
                                        stuNode.RemoveChild(subNode);
                                        xmlDoc.Save(xmlPath);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(JudgeLanguage.JudgeLag("配置文件不存在"));
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
                MessageBox.Show(JudgeLanguage.JudgeLag("配置文件读取错误,请检查是否有误，并重启软件！"));
            }
        }
        public void DeleteConfig(string nodeName, string key, string attributes)
        {
            try
            {
                lock (locker)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    if (File.Exists(xmlPath))
                    {
                        xmlDoc.Load(xmlPath);
                        XmlNode rootNode = xmlDoc.DocumentElement;
                        foreach (XmlNode stuNode in rootNode.ChildNodes)
                        {
                            if (stuNode.Name == nodeName)
                            {
                                foreach (XmlNode subNode in stuNode)
                                {
                                    if (subNode.Name == key)
                                    {
                                        foreach (XmlAttribute item in subNode.Attributes)
                                        {
                                            if (item.Name == attributes)
                                            {
                                                subNode.Attributes.Remove(item);
                                                xmlDoc.Save(xmlPath);
                                                return;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(JudgeLanguage.JudgeLag("配置文件不存在"));
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
                MessageBox.Show(JudgeLanguage.JudgeLag("配置文件读取错误,请检查是否有误，并重启软件！"));
            }
        }
        public bool IsExistXML()
        {
            bool result = false;
            try
            {
                lock (locker)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    if (File.Exists(xmlPath) != true)
                    {
                        result = false;
                    }
                    else
                    {
                        result = true;
                        //配置文件已经存在，不做操作
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return result;
        }
        public bool IsExistXML(bool isAppend)
        {
            bool result = false;
            try
            {
                lock (locker)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    if (File.Exists(xmlPath) != true)
                    {
                        result = false;
                        if (isAppend != true)
                        {
                            return false;
                        }
                        using (FileStream fs = new FileStream(xmlPath, FileMode.Create, FileAccess.Write))
                        {

                        }
                        xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null));
                        XmlElement xml = xmlDoc.CreateElement("configuration");
                        xmlDoc.AppendChild(xml);
                        xmlDoc.Save(xmlPath);
                    }
                    else
                    {
                        result = true;
                        //配置文件已经存在，不做操作
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
                MessageBox.Show(JudgeLanguage.JudgeLag("创建配置文件失败！"));
            }
            return result;
        }
        public bool IsExistKey(string nodeName)
        {
            bool result = false;
            try
            {
                lock (locker)
                {
                    List<string> keyList = GetAllKey();
                    foreach (string item in keyList)
                    {
                        if (item == nodeName)
                        {
                            result = true;
                            return result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
                MessageBox.Show(JudgeLanguage.JudgeLag("配置文件读取错误,请检查是否有误，并重启软件！"));
            }
            return result;
        }
        public bool IsExistKey(string nodeName, string Key)
        {
            bool result = false;
            try
            {
                lock (locker)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    if (File.Exists(xmlPath))
                    {
                        xmlDoc.Load(xmlPath);
                        XmlNode rootNode = xmlDoc.DocumentElement;
                        foreach (XmlNode stuNode in rootNode.ChildNodes)
                        {
                            if (stuNode.Name == nodeName)
                            {
                                foreach (XmlNode subNode in stuNode)
                                {
                                    if (subNode.Name == Key)
                                    {
                                        result = true;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
            return result;
        }
        public bool JudgeKey(string key)
        {
            char[] chars = key.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                char c = char.ToLower(chars[i]);
                if (i == 0)
                {
                    if (c == 'a' || c == 'b' || c == 'c' || c == 'd' || c == 'e' || c == 'f' || c == 'g' || c == 'h' || c == 'i' || c == 'j' || c == 'k' || c == 'l' || c == 'm' || c == 'n' || c == 'o' || c == 'p' || c == 'q' || c == 'r' || c == 's' || c == 't' || c == 'u' || c == 'v' || c == 'w' || c == 'x' || c == 'y' || c == 'z' || c == '_')
                    {

                    }
                    else
                    {
                        MessageBox.Show("key:" + key + JudgeLanguage.JudgeLag("  首字母不合法，添加key失败！"));
                        return false;
                    }
                }
                else
                {
                    if (c == 'a' || c == 'b' || c == 'c' || c == 'd' || c == 'e' || c == 'f' || c == 'g' || c == 'h' || c == 'i' || c == 'j' || c == 'k' || c == 'l' || c == 'm' || c == 'n' || c == 'o' || c == 'p' || c == 'q' || c == 'r' || c == 's' || c == 't' || c == 'u' || c == 'v' || c == 'w' || c == 'x' || c == 'y' || c == 'z' || c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9' || c == '-' || c == '_')
                    {

                    }
                    else
                    {
                        MessageBox.Show("key:" + key + JudgeLanguage.JudgeLag("  包含不合法字符，添加key失败！"));
                        return false;
                    }
                }
            }
            return true;
        }
        private string getXMLPath(string xmlName)
        {
            //使用递归思想遍历文件夹,拿到目标文件
            string path = Application.StartupPath;
            for (int J = 0; J < 2; J++)
            {
                int pathsNum = path.LastIndexOf('\\');
                path = path.Substring(0, pathsNum);
            }
            string destPath = getXML(path, xmlName);
            if (destPath != "")
            {
                return destPath;
            }
            return "";
        }
        private string getXML(string path, string xmlName)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            FileInfo[] filenames = directoryInfo.GetFiles();
            foreach (FileInfo item in filenames)
            {
                if (item.Name.Split('.')[0] == xmlName)
                {
                    return item.FullName;
                }
            }
            if (directoryInfo.GetDirectories().Count() != 0)
            {
                DirectoryInfo[] dires = directoryInfo.GetDirectories();
                foreach (DirectoryInfo item in dires)
                {
                    string xmlPath = getXML(item.FullName, xmlName);
                    if (xmlPath != "")
                    {
                        return xmlPath;
                    }
                }
            }
            return "";
        }
    }
}
