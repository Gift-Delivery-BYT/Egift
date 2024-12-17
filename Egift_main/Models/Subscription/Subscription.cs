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
        protected static double _taxValue = 10.2;
        private List<Client> _clients_subscription { get; }
        public IReadOnlyList<Client> Clients_subscription => _clients_subscription.AsReadOnly();

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

         private  static List<Subscription> _subscriptions = new List<Subscription>();
         
         //Subscription Client connection

         public void AddClient(Client client)
         {
             _clients_subscription.Add(client);
             if (!ClientIsConnected(client)) client.AddSubscription(this);
         }

         public void RemoveClient(Client client)
         {
             _clients_subscription.Remove(client);
             if (ClientIsConnected(client)) client.RemoveSubscription();
         }

         public bool ClientIsConnected(Client client)
         {
             if (Clients_subscription.Contains(client)) return true;
             return false;
         }

         // public void ModifyClient(Client old_client, Client modified_client)
         // {
         //     RemoveClient(old_client);
         //     AddClient(modified_client);
         // }
         
         public static bool addNewSubscriptionUser(Subscription subscription, Client client)
         {
             if (SubscriptionIsValid(subscription)) {
                 _subscriptions.Add(subscription);
                 subscription.AddClient(client);
                 client.AddSubscription(subscription);
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

         private static bool SubscriptionIsValid(Subscription subscription)
         {
             try
             {
                 if (subscription != null &&
                     subscription.Price > 0 &&
                     subscription._features != null
                    ) return true;
             }
             catch (ArgumentNullException e)
             {
                 Console.WriteLine("Some of arguments is not valid");
                 throw;
             }

             return false;
         }
    }
}

