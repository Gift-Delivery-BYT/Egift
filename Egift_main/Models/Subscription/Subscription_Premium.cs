using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Egift_main.Subscription
{
    [Serializable]
    public class Subscription_Premium : Subscription
    {
        private double _discount = 0.05;
        private bool _freeDelivery;
        private bool _freePriority;

        public Subscription_Premium(double price, bool freeDelivery, bool freePriority) : base(price)
        {
            _freeDelivery = freeDelivery;
            _freePriority = freePriority;
        }

        [XmlElement("Discount")]
        public double Discount
        {
            get => _discount;
            set => _discount = value;
        }

        [XmlElement("FreeDelivery")]
        public bool FreeDelivery
        {
            get => _freeDelivery;
            set => _freeDelivery = value;
        }

        [XmlElement("FreePriority")]
        public bool FreePriority
        {
            get => _freePriority;
            set => _freePriority = value;
        }
        
        public void SaveToFile(string path = "./Subscription/Serialized/SubscriptionPremium.xml")
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PremiumSubscriptionInfo));
            using (StreamWriter writer = new StreamWriter(path))
            {
                var data = new PremiumSubscriptionInfo
                {
                    Price = this.Price,
                    Features = this.Features,
                    Discount = this.Discount,
                    FreeDelivery = this.FreeDelivery,
                    FreePriority = this.FreePriority
                };
                serializer.Serialize(writer, data);
            }
        }
        
        public bool LoadFromFile(string path = "./Subscription/Serialized/SubscriptionPremium.xml")
        {
            if (!File.Exists(path))
            {
                Features.Clear();
                Price = 0;
                Discount = 0.05;
                FreeDelivery = false;
                FreePriority = false;
                return false;
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(PremiumSubscriptionInfo));
                using (StreamReader reader = new StreamReader(path))
                {
                    var data = (PremiumSubscriptionInfo)serializer.Deserialize(reader);
                    this.Price = data.Price;
                    this.Features = data.Features;
                    this.Discount = data.Discount;
                    this.FreeDelivery = data.FreeDelivery;
                    this.FreePriority = data.FreePriority;
                }
                return true;
            }
            catch
            {
                Features.Clear();
                Price = 0;
                Discount = 0.05;
                FreeDelivery = false;
                FreePriority = false;
                return false;
            }
        }
        [Serializable]
        public class PremiumSubscriptionInfo
        {
            public double Price { get; set; }

            [XmlArray("Features")]
            [XmlArrayItem("Feature")]
            public ArrayList Features { get; set; } = new ArrayList();

            public double Discount { get; set; }
            public bool FreeDelivery { get; set; }
            public bool FreePriority { get; set; }
        }
    }
}
