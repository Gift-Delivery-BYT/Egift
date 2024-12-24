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
    private Wallet _wallet;
    [XmlArray]
    private static List<Client> _clientList = new List<Client>();

    public Client() { }
    
    public Wallet _Wallet { get; }

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

    public void AddWallet(Wallet wallet) {
        ClientWallet = wallet;
        if(WalletIsAdded(wallet)) wallet.Owner = this;
    }
    public void DeleteWallet(Wallet wallet) {
        ClientWallet = null;
        wallet.Owner = null;
    }
    public bool WalletIsAdded(Wallet wallet)
    {
        if (wallet.Owner!=null) return true;
        return false;
    }
    public void DeleteClient()
    {
        _wallet = null; 
        Console.WriteLine($"Client {name} and their Wallet have been removed.");
    }

    public static bool Serialize(string path = "./Users/Serialized/Client.xml")
    {
            
        XmlSerializer serializer = new XmlSerializer(typeof(List<Client>));
        using (StreamWriter writer = new StreamWriter(path)) {
            serializer.Serialize(writer, _clientList);
        }
        return true;
    }

    public static bool Deserialize(string path = "./Users/Serialized/Client.xml")
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
        DateFormat birthday, string name) : base(id, phoneNumber, email)
    {
        this.birthday = birthday;
        this.name = name;
        
        _clientList.Add(this);
    }
    
    public Client(int id, string phoneNumber, string email, Wallet UserWallet,
        string name) : base(id, phoneNumber, email)
    {
        this.name = name;
        _clientList.Add(this);
    }
    
    private static bool IsValidClient(Client client)
    {
        if (client != null &&
            !string.IsNullOrWhiteSpace(client.Name) &&
            client.Birthday != null &&
            client.WishList.Count > 0)
        {
            return true;
        }
        throw new ArgumentNullException("Invalid client attributes");
    }
}