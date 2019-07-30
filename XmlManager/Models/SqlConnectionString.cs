using System;
using System.Xml.Serialization;

namespace XmlManager.Models
{
    [Serializable]
    [XmlRoot("SqlConnectionString")]
    public class SqlConnectionString
    {
        [XmlAttribute("connection")]
        public string connection { get; set; }
    }
}
