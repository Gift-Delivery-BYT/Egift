using System.Collections;
using System.Xml;
using System.Xml.Serialization;

namespace Egift_main;

[Serializable]
public class Foundation_Account: User
{
    private ArrayList _accountingInfo = new ArrayList();
    private string _foundation_name;
    

    [XmlArray]
    private static List<Foundation_Account> _foundationAccountList = new List<Foundation_Account>();

    public Foundation_Account() { }
    public string FoundationName
    {
        get => _foundation_name;
        set => _foundation_name = value;
    }

    public ArrayList AccountingInfo
    {
        get => _accountingInfo;
        set => _accountingInfo = value ?? throw new ArgumentNullException(nameof(value));
    }
    
    public enum foundation_type
    {
        productBased, 
        deliveryBased, 
        locationBased
    }


    public void FindFreePropositions()
    {
       
    }
    
    public static List<Foundation_Account> GetAllFoundationAccounts()
    {
        return _foundationAccountList;
    }
    
    public void AddAccountingInfo(string information)
    {
        _accountingInfo.Add(information);
    }

    public Foundation_Account(int id, string phoneNumber, 
        string email, Wallet UserWallet, string _foundation_name) : base(id, phoneNumber, email, UserWallet)
    {
        this._foundation_name = _foundation_name;
        
        _foundationAccountList.Add(this);
    }
    
    public static bool Serialize(string path = "./Users/Serialized/Foundation_Account.xml")
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Foundation_Account));
        using (StreamWriter writer = new StreamWriter(path)) {
            serializer.Serialize(writer, (_foundationAccountList));
        }
        return true;
    }
    public static bool Deserialize(string path = "./Users/Serialized/Foundation_Account.xml")
    {
        StreamReader file;
        try {
            file = File.OpenText(path);
        }
        catch (FileNotFoundException) {
            _foundationAccountList.Clear();
            return false;
        }
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<User>));
        using (XmlTextReader reader = new XmlTextReader(file)) {
            try {
                _foundationAccountList = (List<Foundation_Account>)xmlSerializer.Deserialize(reader);
            }
            catch (InvalidCastException) {
                _foundationAccountList.Clear();
                return false;
            }
            return true;
        }
    }
}