using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Data;

namespace Slate
{
    class MostRecent
    {
        string location;
        XmlReader reader;
        public DataSet dataSet;

        public MostRecent(string location)
        {
            this.location = location;
            //reader = XmlReader.Create(this.location);
            //loadXML(reader);
            dataSet = new DataSet();
            dataSet.ReadXml(this.location);
        }

        /*
        public void loadXML(System.Xml.XmlReader reader)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
                return;

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                reader.ReadStartElement("item");
                reader.ReadStartElement("key");

                TKey key = (TKey)keySerializer.Deserialize(reader);
                reader.ReadEndElement();
                reader.ReadStartElement("value");

                TValue value = (TValue)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();
                this.Add(key, value);
                reader.ReadEndElement();
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }*/

    }

    class RecentObject
    {
        string name;
        string location;

        RecentObject(string name, string location)
        {
            this.name = name;
            this.location = location;
        }
    }
}
