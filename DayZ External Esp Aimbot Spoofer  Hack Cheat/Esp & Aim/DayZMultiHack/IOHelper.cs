using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace DayZMultiHack
{
    public static class IOHelper
    {
        public static void Serialize<T>(T value, string file, XmlWriterSettings xmlWriterSettings = null)
        {
            if (value == null)
            {
                throw new ArgumentException("value");
            }

            var serializer = new XmlSerializer(typeof(T));

            var settings = xmlWriterSettings ?? new XmlWriterSettings
            {
                Encoding = new UnicodeEncoding(false, false),
                Indent = false,
                OmitXmlDeclaration = false
            };

            using (var textWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    serializer.Serialize(xmlWriter, value);
                }

                File.WriteAllText(file, textWriter.ToString());
            }
        }

        public static T Deserialize<T>(string file, XmlReaderSettings xmlReaderSettings = null)
        {
            string xml = File.ReadAllText(file);
            if (string.IsNullOrEmpty(xml))
            {
                throw new ArgumentException("xml");
            }

            var serializer = new XmlSerializer(typeof(T));

            var settings = xmlReaderSettings ?? new XmlReaderSettings();

            // No settings need modifying here
            using (var textReader = new StringReader(xml))
            {
                using (var xmlReader = XmlReader.Create(textReader, settings))
                {
                    return (T)serializer.Deserialize(xmlReader);
                }
            }
        }
    }
}
