using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace XmlManager.Models
{
    [Serializable]
    [XmlRoot(ElementName = "ReportItems")]
    public class ReportItems
    {
        [XmlElement(ElementName = "RItem")]
        public List<RItem> RItem { get; set; }
    }
}
