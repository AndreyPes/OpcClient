using System;
using System.Xml.Serialization;

namespace XmlManager.Models
{
    [Serializable]
    [XmlRoot("DataBaseName")]
    public class DataBaseName
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }
    }
}
