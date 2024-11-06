using System.Collections;
using System.Xml.Serialization;

namespace Egift_main;
[Serializable]
public class User
{
    private int id;
    private string PhoneNumber;
    private string Email;
    private Wallet _UserWallet;

    public User(int id, string phoneNumber, string email, Wallet UserWallet)
    {
        this.id = id;
        PhoneNumber = phoneNumber;
        Email = email;
        _UserWallet = UserWallet;
    }
    
    [XmlElement("Id")]
    public int Id
    {
        get => id;
        set => id = value;
    }
     
    [XmlElement("UserWallet")]
    public Wallet UserWallet
    {
        get => _UserWallet;
        set => _UserWallet = value ?? throw new ArgumentNullException(nameof(value));
    }

    [XmlElement("PhoneNumber1")]
    public string PhoneNumber1
    {
        get => PhoneNumber;
        set => PhoneNumber = value;
    }
    
    [XmlElement("Email1")]
    public string Email1
    {
        get => Email;
        set => Email = value ?? throw new ArgumentNullException(nameof(value));
    }
    
    public void Save(string path = "./Users/Serialized/User.xml")
    {
        XmlSerializer serializer = new XmlSerializer(typeof(UserInfo));
        using (StreamWriter writer = new StreamWriter(path))
        {
            var data = new UserInfo
            {
                Id = this.Id, PhoneNumber1 = this.PhoneNumber1,
                Email1 = this.Email1,
                UserWallet = this.UserWallet,
                    
            };
            serializer.Serialize(writer, data);
        }
    }
    
    public bool LoadFromFile(string path = "./Users/Serialized/User.xml")
    {
        if (!File.Exists(path))
        {
            Id = 0;
            PhoneNumber1 = String.Empty;
            Email1 = String.Empty;
            UserWallet = null;
            return false;
        }

        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(UserInfo));
            using (StreamReader reader = new StreamReader(path))
            {
                var data = (UserInfo)serializer.Deserialize(reader);
                this.Id = data.Id;
                this.PhoneNumber1 = data.PhoneNumber1;
                this.Email1 = data.Email1;
                this.UserWallet = data.UserWallet;
            }
            return true;
        }
        catch
        {
            Id = 0;
            PhoneNumber1 = String.Empty;
            Email1 = String.Empty;
            UserWallet = null;
            return false;
        }
    }

    [Serializable]
    public class UserInfo
    {
        public int Id { get; set; }
        public string PhoneNumber1 { get; set; }
        public string Email1 { get; set; }
        public Wallet UserWallet  { get; set; }
    }
}