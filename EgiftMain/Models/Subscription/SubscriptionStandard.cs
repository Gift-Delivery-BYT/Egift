using System.Xml;
using System.Xml.Serialization;

namespace Egift_main.Subscription
{
    [Serializable]
    public class SubscriptionStandard : Subscription
    {
        private List<DateTime> _availableDates = new List<DateTime>();
        private List<Object> _freeGifts = new List<Object>();

        private static List<SubscriptionStandard> _subscriptionStandarts = new List<SubscriptionStandard>();

        public List<DateTime> AvailableDates
        {
            get => new List<DateTime>(_availableDates);  
            set => _availableDates = value ?? new List<DateTime>(); 
        }

        public List<object> FreeGifts
        {
            get => new List<object>(_freeGifts);  
            set => _freeGifts = value ?? new List<object>();  
        }

        public SubscriptionStandard()
        {
            _subscriptionStandarts.Add(this);
            ThisSubscriptionType = SubscriptionType.Standard;
        }


        private static bool SubscriptionStandartIsValid(SubscriptionStandard subscriptionStandard)
        {
            if (subscriptionStandard != null) return true;
            throw new ArgumentNullException();
        }

        public static bool Serialize(string path = "./Subscription/Serialized/SubPremium.xml")
        {

            XmlSerializer serializer = new XmlSerializer(typeof(List<SubscriptionStandard>));
            using (StreamWriter writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, _subscriptionStandarts);
            }

            return true;
        }


        public static bool Deserialize(string path = "./Order/Serialized/SubPremium.xml")
        {
            StreamReader file;
            try
            {
                file = File.OpenText(path);
            }
            catch (FileNotFoundException)
            {
                _subscriptionStandarts.Clear();
                return false;
            }

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<SubscriptionStandard>));
            using (XmlTextReader reader = new XmlTextReader(file))
            {
                try
                {
                    _subscriptionStandarts = (List<SubscriptionStandard>)xmlSerializer.Deserialize(reader);
                }
                catch (InvalidCastException)
                {
                    _subscriptionStandarts.Clear();
                    return false;
                }

                return true;
            }
        }
        
        
        private static bool SubscriptionStandardIsValid(SubscriptionStandard subscriptionStandard)
        {
            if (subscriptionStandard != null &&
                subscriptionStandard.AvailableDates.Count > 0 &&
                subscriptionStandard.FreeGifts.Count > 0)
            {
                return true;
            }
            throw new ArgumentNullException("error");
        }
    }
}
