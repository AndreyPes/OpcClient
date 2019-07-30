using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace XmlManager.Models
{
    [Serializable]
    [XmlRoot(ElementName = "FBConfig")]
    public class FBConfig
    {
        [XmlElement(ElementName = "Operation")]
        public List<Operation> Operation { get; set; }

        [XmlElement(ElementName = "Process")]
        public Process Process { get; set; }

        [XmlElement(ElementName = "RegionalParameters")]
        public RegionalParameters RegionalParameters { get; set; }

        [XmlElement("DataBaseName")]
        public DataBaseName DataBaseName { get; set; }

        [XmlElement("SqlConnectionString")]
        public SqlConnectionString SqlConnectionString { get; set; }

        [XmlElement("LocalPathToDB")]
        public LocalPathToDB LocalPathToDB { get; set; }

        [XmlElement(ElementName = "EquipmentInfo")]
        public EquipmentInfo EquipmentInfo { get; set; }

        [XmlElement(ElementName = "MaterialParameters")]
        public MaterialParameters MaterialParameters { get; set; }

    }
}
