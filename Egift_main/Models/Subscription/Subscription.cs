using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Egift_main.Subscription
{
    [Serializable]
    public abstract class Subscription
    {
        private ArrayList _features = new ArrayList();
        private double _price;

        protected Subscription(double price)
        {
            _price = price;
        }

        [XmlElement("Price")]
        public double Price
        {
            get => _price;
            set => _price = value;
        }

        [XmlArray("Features")]
        [XmlArrayItem("Feature")]
        public ArrayList Features
        {
            get => _features;
            set => _features = value ?? throw new ArgumentNullException(nameof(value));
        }
        public void Save(string path = "./Subscription/Serialized/Subscription.xml")
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SubscriptionInfo));
            using (StreamWriter writer = new StreamWriter(path))
            {
                var data = new SubscriptionInfo { Price = this.Price, Features = this.Features };
                serializer.Serialize(writer, data);
            }
        }

       
        public bool LoadFromFile(string path = "./Subscription/Serialized/Subscription.xml")
        {
            if (!File.Exists(path))
            {
                Features.Clear();
                Price = 0;
                return false;
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SubscriptionInfo));
                using (StreamReader reader = new StreamReader(path))
                {
                    var data = (SubscriptionInfo)serializer.Deserialize(reader);
                    this.Price = data.Price;
                    this.Features = data.Features;
                }
                return true;
            }
            catch
            {
                Features.Clear();
                Price = 0;
                return false;
            }
        }
        
        [Serializable]
        public class SubscriptionInfo
        {
            public double Price { get; set; }

            [XmlArray("Features")]
            [XmlArrayItem("Feature")]
            public ArrayList Features { get; set; } = new ArrayList();
        }
    }
}
