using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace XmlManager.Models
{
    [Serializable]
    [XmlRoot(ElementName = "Items")]
    public class Items
    {
        [XmlElement(ElementName = "Item")]
        public List<Item> Item { get; set; }
    }
}
