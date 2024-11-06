using System.Collections;
using System.Xml.Serialization;

namespace Egift_main;

[Serializable]
public class Foundation_Account: User
{
    private ArrayList _accountingInfo = new ArrayList();
    private string _foundation_name;
    
    [XmlElement("FoundationName")]
    public string FoundationName
    {
        get => _foundation_name;
        set => _foundation_name = value;
    }
    [XmlArray("AccountingInfo")]
    [XmlArrayItem("AccountingInfo")]
    public ArrayList AccountingInfo
    {
        get => _accountingInfo;
        set => _accountingInfo = value ?? throw new ArgumentNullException(nameof(value));
    }
    
    public void SaveToFile(string path = "./Users/Serialized/Foundation_Account.xml")
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Foundation_AccountInfo));
            using (StreamWriter writer = new StreamWriter(path))
            {
                var data = new Foundation_AccountInfo
                {
                    Id = this.Id,
                    PhoneNumber1 = this.PhoneNumber1,
                    Email1 = this.Email1,
                    AccountingInfo = this.AccountingInfo,
                    FoundationName = this.FoundationName
                };
                serializer.Serialize(writer, data);
            }
        }
        
        public bool LoadFromFile(string path = "./Users/Serialized/Foundation_Account.xml")
        {
            if (!File.Exists(path))
            {
                Id = 0;
                PhoneNumber1 = String.Empty;
                Email1 = String.Empty;
                AccountingInfo.Clear();
                FoundationName = String.Empty;
                return false;
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Foundation_AccountInfo));
                using (StreamReader reader = new StreamReader(path))
                {
                    var data = (Foundation_AccountInfo)serializer.Deserialize(reader);
                    this.Id = data.Id;
                    this.PhoneNumber1 = data.PhoneNumber1;
                    this.Email1 = data.Email1;
                    this. AccountingInfo = data.AccountingInfo;
                    this.FoundationName = data.FoundationName;
                }
                return true;
            }
            catch
            {
                Id = 0;
                PhoneNumber1 = String.Empty;
                Email1 = String.Empty;
                FoundationName = String.Empty;
                AccountingInfo.Clear();
                FoundationName = String.Empty;
                return false;
            }
        }
    
    [Serializable]
    public class Foundation_AccountInfo
    {
        public string FoundationName { get; set; }
        public int Id { get; set; }
        public string PhoneNumber1 { get; set; }
        public string Email1 { get; set; }
        [XmlArray("AccountingInfo")]
        [XmlArrayItem("AccountingInfo")]
        public ArrayList AccountingInfo { get; set; } = new ArrayList();
    }
    private enum foundation_type
    {
        productBased, 
        deliveryBased, 
        locationBased
    }


    public void FindFreePropositions()
    {
        
    }
    
    public void AddAccountingInfo()
    {
        
    }

    public Foundation_Account(int id, string phoneNumber, 
        string email, string _foundation_name) : base(id, phoneNumber, email)
    {
        this._foundation_name = _foundation_name;
    }
}