using System;
using System.Xml.Serialization;

namespace XmlManager.Models
{
    [Serializable]
    [XmlRoot("LocalPathToDB")]
    public class LocalPathToDB
    {
        [XmlAttribute("Path")]
        public string Path{ get; set; }
    }
}
