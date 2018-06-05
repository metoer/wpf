using Hytera.EEMS.Log;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Hytera.EEMS.Common
{
    public class XmlUnityConvert
    {
        /// <summary>
        /// XML系列化对象
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public static string XmlSerialize(Object Obj)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            //去除xml声明
            settings.OmitXmlDeclaration = true;
            settings.Encoding = Encoding.UTF8;
            System.IO.MemoryStream mem = new MemoryStream();
             
            using (XmlWriter writer = XmlWriter.Create(mem, settings))
            {
                //去除默认命名空间xmlns:xsd和xmlns:xsi
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add(string.Empty, string.Empty);
                XmlSerializer formatter = new XmlSerializer(Obj.GetType());
                formatter.Serialize(writer, Obj, ns);
            }

            return Encoding.UTF8.GetString(mem.ToArray());
        }


        /// <summary>
        /// XML反系列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T XmlDeserialize<T>(string value)
        {
            T t = default(T);
            try
            {
                if (!string.IsNullOrEmpty(value))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                    using (Stream xmlSteam = new MemoryStream(Encoding.UTF8.GetBytes(value)))
                    {
                        using (XmlReader xmlReader = XmlReader.Create(xmlSteam))
                        {
                            object obj = xmlSerializer.Deserialize(xmlReader);
                            t = (T)obj;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Instance.WirteErrorMsg(e.Message);
            }
            return t;
        }
    }
}
