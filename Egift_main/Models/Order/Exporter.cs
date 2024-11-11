using System.Collections;
using System.Xml;
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
        private int PhoneNumber { get; set; }
        private DateTime TimeLeadDate { get; set; }
        public List<Object> Documentation { get; set; } = new List<Object>();
        public Exporter() { }
        
        [XmlArray]
        private static List<Exporter> _exporterlist = new List<Exporter>();
        public Exporter(string name, string country, string address, float shippingCost, int phoneNumber, DateTime timeLeadDate){
            Name = name;
            Country = country;
            Address = address;
            ShippingCost = shippingCost;
            PhoneNumber = phoneNumber;
            TimeLeadDate = timeLeadDate;
            _exporterlist.Add(this);
        }

        public static void addNewExporter(Exporter exporter)
        {
            if (exporter!=null && !IsValidExporter(exporter))
            {
                throw new ArgumentException("Empty Exporter or attribute tried to be added");
            }
            _exporterlist.Add(exporter);
        }

       
        public static bool Serialize(string path = "./Order/Serialized/Exporter.xml")
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Exporter));
            using (StreamWriter writer = new StreamWriter(path)) {
                serializer.Serialize(writer, _exporterlist);
            }
            return true;
        }
        public static bool Deserialize(string path = "./Order/Serialized/Exporter.xml")
        {
            StreamReader file;
            try {
                file = File.OpenText(path);
            }
            catch (FileNotFoundException) {
                _exporterlist.Clear();
                return false;
            }
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Exporter>));
            using (XmlTextReader reader = new XmlTextReader(file)) {
                try {
                    _exporterlist = (List<Exporter>)xmlSerializer.Deserialize(reader);
                }
                catch (InvalidCastException) {
                    _exporterlist.Clear();
                    return false;
                }
                return true;
            }
        }
        private static bool IsValidExporter(Exporter exporter)
        {
            return exporter != null &&
                   !string.IsNullOrWhiteSpace(exporter.Name) &&
                   !string.IsNullOrWhiteSpace(exporter.Country) &&
                   !string.IsNullOrWhiteSpace(exporter.Address) &&
                   exporter.ShippingCost > 0 &&
                   exporter.PhoneNumber > 0 &&
                   exporter.TimeLeadDate != default;
        }
    }

