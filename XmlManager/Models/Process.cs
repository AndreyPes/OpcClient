using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace XmlManager.Models
{
    [Serializable]
    [XmlRoot(ElementName = "Process")]
    public class Process
    {
        [XmlElement(ElementName = "Details")]
        public string Details { get; set; }

        [XmlElement(ElementName = "Item")]
        public List<Item> Item { get; set; }

        [XmlElement(ElementName = "Column")]
        public List<Column> Column { get; set; }

        [XmlElement(ElementName = "RItem")]
        public List<RItem> RItem { get; set; }
    }

}
