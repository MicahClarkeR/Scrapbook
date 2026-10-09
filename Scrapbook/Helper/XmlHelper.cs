using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Serialization;

namespace Scrapbook.Helper
{
    public static class XmlHelper
    {
        public static string ToXml(object value)
        {
            StringBuilder xml = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(xml, new XmlWriterSettings()
            {
                Indent = false,
                NewLineChars = string.Empty,
                NewLineHandling = NewLineHandling.Replace,
                NewLineOnAttributes = false
            });
            XmlSerializer serializer = new XmlSerializer(value.GetType());

            serializer.Serialize(writer, value);

            return xml.ToString();
        }

        public static T? FromXml<T>(string xml)
        {
            TextReader reader = new StringReader(xml);
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            T? value = (T?)serializer.Deserialize(reader);

            return value;
        }

        public static T? FromXml<T>(byte[] xml) => FromXml<T>(Encoding.UTF8.GetString(xml));
    }
}
