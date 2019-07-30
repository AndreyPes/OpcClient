using System;
using System.Xml.Serialization;

namespace XmlManager.Models
{
    [Serializable]
    [XmlRoot(ElementName = "Item")]
    public class Item
    {
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "Type")]
        public string Type { get; set; }

        [XmlAttribute(AttributeName = "ReportName")]
        public string ReportName { get; set; }

        [XmlAttribute(AttributeName = "Browsable")]
        public string Browsable { get; set; }

    }
}
