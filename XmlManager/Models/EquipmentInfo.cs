using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace XmlManager.Models
{
    [Serializable]
    [XmlRoot(ElementName = "EquipmentInfo")]
    public class EquipmentInfo
    {
        [XmlElement(ElementName = "Item")]
        public List<Item> Item { get; set; }
         
    }   
}
