using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace XmlManager.Models
{
    [Serializable]
    [XmlRoot(ElementName = "MaterialParameters")]
    public class MaterialParameters
    {
        [XmlElement(ElementName = "Column")]
        public List<Column> Column { get; set; }
    }

}
