using System;
using System.Xml.Serialization;

namespace XmlManager.Models
{
    [Serializable]
    [XmlRoot(ElementName = "Operation")]
    public class Operation
    {
        [XmlElement(ElementName = "Items")]
        public Items Items { get; set; }

        [XmlElement(ElementName = "ReportItems")]
        public ReportItems ReportItems { get; set; }

        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "ColumnName")]
        public string ColumnName { get; set; }

        [XmlAttribute(AttributeName = "ReportEnabled")]
        public string ReportEnabled { get; set; }
    }
}
