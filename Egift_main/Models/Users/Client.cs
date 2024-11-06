using System.Collections;
using System.Runtime.InteropServices.JavaScript;
using System.Xml.Serialization;
using Microsoft.VisualBasic;

namespace Egift_main;

[Serializable]
public class Client: User
{
    private string name;
    private ArrayList _wishlist = new ArrayList();
    private DateFormat birthday;
    private Wallet _UserWallet;

    public Client(Wallet UserWallet, DateFormat birthday, string name,
        int id, string phoneNumber, string email) : base(id, phoneNumber, email)
        {
            _UserWallet = UserWallet;
            this.birthday = birthday;
            this.name = name;
        }

    [XmlElement("Name")]
    public string Name
    {
        get => name;
        set => name = value;
    }
    
    [XmlElement("Birthday")]
    public DateFormat? Birthday
    {
        get => birthday;
        set => birthday = (DateFormat)value;
    }
    
    [XmlElement("UserWallet")]
    public Wallet UserWallet
    {
        get => _UserWallet;
        set => _UserWallet = value ?? throw new ArgumentNullException(nameof(value));
    }
    
    [XmlArray("WishList")]
    [XmlArrayItem("WishList")]
    public ArrayList WishList
    {
        get => _wishlist;
        set => _wishlist = value ?? throw new ArgumentNullException(nameof(value));
    }
    
     public void SaveToFile(string path = "./Users/Serialized/Client.xml")
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ClientInfo));
            using (StreamWriter writer = new StreamWriter(path))
            {
                var data = new ClientInfo
                {
                    Id = this.Id,
                    PhoneNumber1 = this.PhoneNumber1,
                    Email1 = this.Email1,
                    UserWallet = this.UserWallet,
                    Birthday = this.Birthday,
                    WishList = this.WishList,
                    Name = this.Name
                };
                serializer.Serialize(writer, data);
            }
        }
        
        public bool LoadFromFile(string path = "./Users/Serialized/Client.xml")
        {
            if (!File.Exists(path))
            {
                Id = 0;
                PhoneNumber1 = String.Empty;
                Email1 = String.Empty;
                UserWallet = null;
                Birthday = null;
                Name = null;
                WishList = null;
                return false;
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ClientInfo));
                using (StreamReader reader = new StreamReader(path))
                {
                    var data = (ClientInfo)serializer.Deserialize(reader);
                    this.Id = data.Id;
                    this.PhoneNumber1 = data.PhoneNumber1;
                    this.Email1 = data.Email1;
                    this. UserWallet = data.UserWallet;
                    this.Birthday = data.Birthday;
                    this.WishList = data.WishList;
                    this.Name = data.Name;
                }
                return true;
            }
            catch
            {
                Id = 0;
                PhoneNumber1 = String.Empty;
                Email1 = String.Empty;
                UserWallet = null;
                Birthday = null;
                Name = null;
                WishList = null;
                return false;
            }
        }
        [Serializable]
        public class ClientInfo
        {
            public int Id { get; set; }

            [XmlArray("WishList")]
            [XmlArrayItem("WishList")]
            public ArrayList WishList { get; set; } = new ArrayList();
            public string PhoneNumber1 { get; set; }
            public string Email1 { get; set; }
            public Wallet UserWallet  { get; set; }
            public DateFormat? Birthday  { get; set; }
            public string Name  { get; set; }

        }
}