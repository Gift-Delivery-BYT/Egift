using System.Collections;
using System.Runtime.InteropServices.JavaScript;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualBasic;

namespace Egift_main;

[Serializable]
public class Client: User
{
    private string name;
    private ArrayList _wishlist = new ArrayList();
    private DateFormat birthday;
    
    [XmlArray]
    private static List<Client> _clientList = new List<Client>();

    public Client() { }

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

    private static bool Serialize(string path = "./Users/Serialized/Client.xml")
    {
            
        XmlSerializer serializer = new XmlSerializer(typeof(Client));
        using (StreamWriter writer = new StreamWriter(path)) {
            serializer.Serialize(writer, _clientList);
        }
        return true;
    }

    private static bool Deserialize(string path = "./Users/Serialized/Client.xml")
    {
        StreamReader file;
        try {
            file = File.OpenText(path);
        }
        catch (FileNotFoundException) {
            _clientList.Clear();
            return false;
        }
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Client>));
        using (XmlTextReader reader = new XmlTextReader(file)) {
            try {
                _clientList = (List<Client>)xmlSerializer.Deserialize(reader);
            }
            catch (InvalidCastException) {
                _clientList.Clear();
                return false;
            }
            return true;
        }
    }

    public Client(int id, string phoneNumber, string email, Wallet UserWallet,
        DateFormat birthday, string name) : base(id, phoneNumber, email, UserWallet)
    {
        this.birthday = birthday;
        this.name = name;
        
        _clientList.Add(this);
    }
}