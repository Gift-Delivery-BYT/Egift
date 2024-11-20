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
        protected double _price { get; set; }
        private static double _taxValue = 10.2;

        protected Subscription(double price)
        {
            _price = price;
        }
        protected Subscription()
        {
        }
        private double Price
        {
            get => _price;
            set => _price = value;
        }
        private ArrayList Features
        {
            get => new ArrayList(_features);
            set => _features = value ?? throw new ArgumentNullException(nameof(value));
        }

        private void EvaluatePrice()
        {
            double sum = Price;
            Price = sum + ((Price / 100) * _taxValue);
        }

      /*  private  static List<Subscription> _subscriptions = new List<Subscription>();
        public static bool addNewSubscriptionUser(Subscription subscription)
        {
            if (SubscriptionIsValid(subscription)) {
                _subscriptions.Add(subscription);
                return true;
            }
            return false;
        }
        
        public static bool Serialize(string path = "./Subscription/Serialized/Subscription.xml")
        {
            
            XmlSerializer serializer = new XmlSerializer(typeof(Subscription));
            using (StreamWriter writer = new StreamWriter(path)) {
                serializer.Serialize(writer, _subscriptions);
            }
            return true;
        }

       
        public static bool Deserialize(string path = "./Order/Serialized/Subscription.xml")
        {
            StreamReader file;
            try {
                file = File.OpenText(path);
            }
            catch (FileNotFoundException) {
                _subscriptions.Clear();
                return false;
            }
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Subscription>));
            using (XmlTextReader reader = new XmlTextReader(file)) {
                try {
                    _subscriptions = (List<Subscription>)xmlSerializer.Deserialize(reader);
                }
                catch (InvalidCastException) {
                    _subscriptions.Clear();
                    return false;
                }
                return true;
            }
        }
        
        private static bool SubscriptionIsValid(Subscription subscription) {
            try {
                if (subscription != null &&
                    subscription.Price > 0 &&
                    subscription._features != null
                   ) return true;
            }
            catch (ArgumentNullException e) {
                Console.WriteLine("Some of arguments is not valid");
                throw;
            }
            return false;
        }*/
    }
}
