using System;
using System.Xml.Serialization;

namespace XmlManager.Models
{
    [Serializable]
    [XmlRoot(ElementName = "Column")]
    public class Column
    {
        [XmlAttribute(AttributeName = "Type")]
        public string Type { get; set; }

        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "ReportName")]
        public string ReportName { get; set; }

        [XmlAttribute(AttributeName = "Size")]
        public int Size { get; set; }

        [XmlAttribute(AttributeName = "OperationName")]
        public string OperationName { get; set; }

        [XmlElement(ElementName = "Items")]
        public Items Items { get; set; }
    }
}
