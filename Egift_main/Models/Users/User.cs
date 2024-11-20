using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using Egift_main.Order;

namespace Egift_main;
[Serializable]
public class User
{
    private int id;
    private string PhoneNumber;
    private string Email;
    private Wallet _UserWallet;

    [XmlArray]
    private static List<User> _userList = new List<User>();

    public User() { }
    public User(int id, string phoneNumber, string email, Wallet UserWallet)
    {
        this.id = id;
        PhoneNumber = phoneNumber;
        Email = email;
        _UserWallet = UserWallet;
        
        _userList.Add(this);
    }

    public int Id
    {
        get => id;
        set => id = value;
    }
     
    public Wallet UserWallet
    {
        get => _UserWallet;
        set => _UserWallet = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string PhoneNumber1
    {
        get => PhoneNumber;
        set
        {
            if (value.Length < 4)
            {
                throw new ArgumentException("Phone number must be at least 4 characters long.");
            }
            PhoneNumber = value;
        }
    }

    
    public string Email1
    {
        get => Email;
        set
        {
            if (value == null) 
            {
                throw new ArgumentNullException(nameof(value), "Email cannot be null.");
            }
        
            if (string.IsNullOrEmpty(value) || !value.Contains("@"))
            {
                throw new ArgumentException("Email must contain '@'.");
            }
        
            Email = value;
        }
    }

    
    public static bool Serialize(string path = "./Users/Serialized/User.xml")
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<User>));
        using (StreamWriter writer = new StreamWriter(path)) {
            serializer.Serialize(writer, (_userList));
        }
        return true;
    }
    public static bool Deserialize(string path = "./Users/Serialized/User.xml")
    {
        StreamReader file;
        try {
            file = File.OpenText(path);
        }
        catch (FileNotFoundException) {
            _userList.Clear();
            return false;
        }
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<User>));
        using (XmlTextReader reader = new XmlTextReader(file)) {
            try {
                _userList = (List<User>)xmlSerializer.Deserialize(reader);
            }
            catch (InvalidCastException) {
                _userList.Clear();
                return false;
            }
            return true;
        }
    }
   
}