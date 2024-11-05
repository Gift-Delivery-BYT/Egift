using System.Collections;
using Microsoft.VisualBasic;

namespace Egift_main.Order;

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;



    [Serializable]
    public class Exporter
    {
        private string Name { get; set; }
        private string Country { get; set; }
        private string Address { get; set; }
        private float ShippingCost { get; set; }
        private float ItemsSupplied { get; set; }
        private int PhoneNumber { get; set; }
        private DateTime TimeLeadDate { get; set; }

        [XmlArray("Documentation")]
        [XmlArrayItem("Document")]
        public List<Object> Documentation { get; set; } = new List<Object>();
        public Exporter() { }

        public Exporter(string name, string country, string address, float shippingCost, float itemsSupplied, int phoneNumber, DateTime timeLeadDate)
        {
            Name = name;
            Country = country;
            Address = address;
            ShippingCost = shippingCost;
            ItemsSupplied = itemsSupplied;
            PhoneNumber = phoneNumber;
            TimeLeadDate = timeLeadDate;
        }

       
        public void Save(string path = "./Order/Serialized/Exporter.xml")
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ExporterInfo));
            using (StreamWriter writer = new StreamWriter(path))
            {
                var data = new ExporterInfo
                {
                    Name = this.Name,
                    Country = this.Country,
                    Address = this.Address,
                    ShippingCost = this.ShippingCost,
                    ItemsSupplied = this.ItemsSupplied,
                    PhoneNumber = this.PhoneNumber,
                    TimeLeadDate = this.TimeLeadDate,
                    Documentation = this.Documentation
                };
                serializer.Serialize(writer, data);
            }
        }
        public bool LoadFromFile(string path = "./Order/Serialized/Exporter.xml")
        {
            if (!File.Exists(path))
            {
                Documentation.Clear();
                return false;
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ExporterInfo));
                using (StreamReader reader = new StreamReader(path))
                {
                    var data = (ExporterInfo)serializer.Deserialize(reader);
                    Name = data.Name;
                    Country = data.Country;
                    Address = data.Address;
                    ShippingCost = data.ShippingCost;
                    ItemsSupplied = data.ItemsSupplied;
                    PhoneNumber = data.PhoneNumber;
                    TimeLeadDate = data.TimeLeadDate;
                    Documentation = data.Documentation;
                }
                return true;
            }
            catch
            {
                Documentation.Clear();
                return false;
            }
        }
        
        [Serializable]
        public class ExporterInfo
        {
            public string Name { get; set; }
            public string Country { get; set; }
            public string Address { get; set; }
            public float ShippingCost { get; set; }
            public float ItemsSupplied { get; set; }
            public int PhoneNumber { get; set; }
            public DateTime TimeLeadDate { get; set; }

            [XmlArray("Documentation")]
            [XmlArrayItem("Document")]
            public List<Object> Documentation { get; set; } = new List<Object>();
        }
    }

