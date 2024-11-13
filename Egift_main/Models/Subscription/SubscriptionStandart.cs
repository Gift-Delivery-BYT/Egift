using System.Xml;
using System.Xml.Serialization;

namespace Egift_main.Subscription
{
    [Serializable]
    public class SubscriptionStandart
    {
        private List<DateTime> _availableDates = new List<DateTime>();
        private List<Object> _freeGifts = new List<Object>();

        private static List<SubscriptionStandart> _subscriptionStandarts = new List<SubscriptionStandart>();

        public List<DateTime> AvailableDates
        {
            get => _availableDates;
            set => _availableDates = value ?? new List<DateTime>();
        }

        public List<object> FreeGifts
        {
            get => _freeGifts;
            set => _freeGifts = value ?? new List<object>();
        }

        public SubscriptionStandart()
        {
            _subscriptionStandarts.Add(this);
        }

        private static bool SubscriptionStandartIsValid(SubscriptionStandart subscriptionStandart)
        {
            if (subscriptionStandart != null) return true;
            throw new ArgumentNullException();
        }

        public static bool Serialize(string path = "./Subscription/Serialized/SubPremium.xml")
        {

            XmlSerializer serializer = new XmlSerializer(typeof(Subscription));
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

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<SubscriptionStandart>));
            using (XmlTextReader reader = new XmlTextReader(file))
            {
                try
                {
                    _subscriptionStandarts = (List<SubscriptionStandart>)xmlSerializer.Deserialize(reader);
                }
                catch (InvalidCastException)
                {
                    _subscriptionStandarts.Clear();
                    return false;
                }

                return true;
            }
        }
    }
}
