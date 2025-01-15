using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Egift_main.Subscription;
using Microsoft.VisualBasic;

namespace Egift_main
{
    [Serializable]
    public class Client : User
    {
        private string name;
        private ArrayList _wishlist = new ArrayList();
        [XmlIgnore]
        private DateFormat birthday;
        private Wallet _wallet;
        [XmlArray]
        private static List<Client> _clientList = new List<Client>();
        [XmlArray("Clients")]
        public static List<Client> ClientList
        { 
            get => _clientList;
            set => _clientList = value;
        }
    
        public Wallet _Wallet { get; }

        private Subscription.Subscription _subscription = new SubscriptionStandard();
        public Subscription.Subscription Subscription
        {
            get => _subscription;
            set => _subscription = value;
        }

        public string Name
        {
            get => name;
            set => name = value;
        }

        public DateFormat Birthday
        {
            get => birthday;
            set => birthday = (DateFormat)value;
        }

        public ArrayList WishList
        {
            get => _wishlist;
            set => _wishlist = value ?? throw new ArgumentNullException(nameof(value));
        }

        public Wallet ClientWallet
        {
            get => _wallet;
            set
            {
                _wallet = value;
                _wallet.Owner = this;
            }
        }

        public void AddWallet(Wallet wallet)
        {
            ClientWallet = wallet;
            if (WalletIsAdded(wallet)) wallet.Owner = this;
        }

        public void DeleteWallet(Wallet wallet)
        {
            ClientWallet = null;
            wallet.Owner = null;
        }

        public bool WalletIsAdded(Wallet wallet)
        {
            return wallet.Owner != null;
        }

        // Subscription to Client CONNECTION
        public void AddSubscription(Subscription.Subscription subscription)
        {
            if (subscription.ClientIsConnected(this)) throw new Exception("Client has already a subscription, you need to unsubscribe first");
            _subscription = subscription;
            if (SubscriptionIsClientConnected(this)) _subscription.AddClient(this);
        }

        public void RemoveSubscription()
        {
            if (_subscription.ClientIsConnected(this)) _subscription.RemoveClient(this);
            _subscription = null;
        }

        public bool SubscriptionIsClientConnected(Client client)
        {
            return client != null;
        }

        public void DeleteClient()
        {
            _wallet = null;
            Console.WriteLine($"Client {name} and their Wallet have been removed.");
        }

        public static bool Serialize(string path = "./Users/Serialized/Client.xml")
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Client>));
                using (StreamWriter writer = new StreamWriter(path))
                {
                    serializer.Serialize(writer, _clientList);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Deserialize(string path = "./Users/Serialized/Client.xml")
        {
            try
            {
                using (StreamReader file = File.OpenText(path))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Client>));
                    _clientList = (List<Client>)xmlSerializer.Deserialize(file);
                }
                return true;
            }
            catch
            {
                _clientList.Clear();
                return false;
            }
        }

        public Client(int id, string phoneNumber, string email, Wallet userWallet, DateFormat birthday, string name)
            : base(id, phoneNumber, email)
        {
            this.birthday = birthday;
            this.name = name;
            _wallet = userWallet;
            _clientList.Add(this);
            _subscription = new SubscriptionStandard();
            _Wallet = new Wallet();
        }

        public Client(int id, string phoneNumber, string email, Wallet userWallet, string name)
            : base(id, phoneNumber, email)
        {
            this.name = name;
            _wallet = userWallet;
            _clientList.Add(this);
        }

        private static bool IsValidClient(Client client)
        {
            return client != null &&
                   !string.IsNullOrWhiteSpace(client.Name) &&
                   client.Birthday != null &&
                   client.WishList.Count > 0;
        }
    }
}
