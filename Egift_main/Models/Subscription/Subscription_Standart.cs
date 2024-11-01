using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Egift_main.Subscription
{
    [Serializable]
    public class Subscription_Standart
    {
        private List<DateTime> _availableDates = new List<DateTime>();
        private List<object> _freeGifts = new List<object>();

        [XmlArray("AvailableDates")]
        [XmlArrayItem("Date")]
        public List<DateTime> AvailableDates
        {
            get => _availableDates;
            set => _availableDates = value ?? new List<DateTime>();
        }
        
        [XmlArray("FreeGifts")]
        [XmlArrayItem("Gift")]
        public List<object> FreeGifts
        {
            get => _freeGifts;
            set => _freeGifts = value ?? new List<object>();
        }
        
        public void SaveToFile(string path = "./Subscription/Serialized/SubscriptionStandart.xml")
        {
            XmlSerializer serializer = new XmlSerializer(typeof(StandartSubInfo));
            using (StreamWriter writer = new StreamWriter(path))
            {
                var data = new StandartSubInfo
                {
                    AvailableDates = this.AvailableDates,
                    FreeGifts = this.FreeGifts
                };
                serializer.Serialize(writer, data);
            }
        }
        
        public bool Load(string path = "./Subscription/Serialized/SubscriptionStandart.xml")
        {
            if (!File.Exists(path))
            {
                AvailableDates.Clear();
                FreeGifts.Clear();
                return false;
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(StandartSubInfo));
                using (StreamReader reader = new StreamReader(path))
                {
                    var data = (StandartSubInfo)serializer.Deserialize(reader);
                    this.AvailableDates = data.AvailableDates;
                    this.FreeGifts = data.FreeGifts;
                }
                return true;
            }
            catch
            {
                AvailableDates.Clear();
                FreeGifts.Clear();
                return false;
            }
        }
        [Serializable]
        public class StandartSubInfo
        {
            [XmlArray("AvailableDates")]
            [XmlArrayItem("Date")]
            public List<DateTime> AvailableDates { get; set; } = new List<DateTime>();

            [XmlArray("FreeGifts")]
            [XmlArrayItem("Gift")]
            public List<object> FreeGifts { get; set; } = new List<object>();
        }
    }
}
