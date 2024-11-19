using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Egift_main.Subscription
{
    [Serializable]
    public class SubscriptionPremium : Subscription
    {
        private static double _discount = 0.05;
        private bool _freeDelivery;
        private bool _freePriority;

        private static List<SubscriptionPremium> _subscriptionPremiums = new List<SubscriptionPremium>();
        
        private bool FreeDelivery
        {
            get => _freeDelivery;
            set => _freeDelivery = value;
        }
        private bool FreePriority
        {
            get => _freePriority;
            set => _freePriority = value;
        }
        private static bool Subscription_PremiumIsValid(SubscriptionPremium subscriptionPremium)
        {
            if (subscriptionPremium != null &&
                subscriptionPremium.FreePriority.Equals(null) &&
                subscriptionPremium.FreeDelivery.Equals(null)
               ) return true;
            throw new ArgumentNullException();
            return false;
        }

        public SubscriptionPremium(double price, bool freeDelivery, bool freePriority) : base(price) {
            _freeDelivery = freeDelivery;
            _freePriority = freePriority;
            _subscriptionPremiums.Add(this);
        }

        public SubscriptionPremium() 
        {
  
        }
        
        
        private static bool addNewSubscriptionUser(SubscriptionPremium subscription)
        {
            if (Subscription_PremiumIsValid(subscription)) {
                _subscriptionPremiums.Add(subscription);
                return true;
            }
            return false;
        }
        
        public static bool Serialize(string path = "./Subscription/Serialized/SubPremium.xml")
        {
            
            XmlSerializer serializer = new XmlSerializer(typeof(List<SubscriptionPremium>));
            using (StreamWriter writer = new StreamWriter(path)) {
                serializer.Serialize(writer, _subscriptionPremiums);
            }
            return true;
        }

       
        public static bool Deserialize(string path = "./Order/Serialized/SubPremium.xml")
        {
            StreamReader file;
            try {
                file = File.OpenText(path);
            }
            catch (FileNotFoundException) {
                _subscriptionPremiums.Clear();
                return false;
            }
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<SubscriptionPremium>));
            using (XmlTextReader reader = new XmlTextReader(file)) {
                try {
                    _subscriptionPremiums = (List<SubscriptionPremium>)xmlSerializer.Deserialize(reader);
                }
                catch (InvalidCastException) {
                    _subscriptionPremiums.Clear();
                    return false;
                }
                return true;
            }
        }
        private static bool SubscriptionPremiumIsValid(SubscriptionPremium subscriptionPremium)
        {
            if (subscriptionPremium != null &&
                subscriptionPremium._price > 0 &&
                subscriptionPremium.FreeDelivery != null &&
                subscriptionPremium.FreePriority != null)
            {
                return true;
            }
            throw new ArgumentNullException("error");
        }
       
    }
}
