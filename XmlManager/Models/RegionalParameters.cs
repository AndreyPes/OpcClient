using System;
using System.Xml.Serialization;

namespace XmlManager.Models
{
    [Serializable]
    [XmlRoot(ElementName = "RegionalParameters")]
    public class RegionalParameters
    {
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

    }
}
