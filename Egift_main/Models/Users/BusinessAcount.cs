using System.Collections;
using System.Xml.Serialization;

namespace Egift_main;

[Serializable]
public class BusinessAcount: User
{
    private ArrayList documentation = new ArrayList();
    private string business_name;
    private string business_address;
    private bool isVerified;
    private double coorparate_discount = 0.03;
    private ArrayList authorizedUsers = new ArrayList();

    [XmlElement("Verified")]
    public bool Verified
    {
        get => isVerified;
        set => isVerified = value;
    }
    
    [XmlArray("Documentation")]
    [XmlArrayItem("Document")]
    public ArrayList Documentation
    {
        get => documentation;
        set => documentation = value ?? throw new ArgumentNullException(nameof(value));
    }
    
    [XmlArray("AuthorizedUsers")]
    [XmlArrayItem("User_Id")]
    public ArrayList AuthorizedUsers
    {
        get => authorizedUsers;
        set => authorizedUsers = value ?? throw new ArgumentNullException(nameof(value));
    }
    
    [XmlElement("Corparate_Discount")]
    public double Corparate_Discount
    {
        get => coorparate_discount;
        set => coorparate_discount = value;
    }
    
    [XmlElement("BusinessName")]
    public string BusinessName
    {
        get => business_name;
        set => business_name = value;
    }
    [XmlElement("BusinessAddress")]
    public string BusinessAddress
    {
        get => business_address;
        set => business_address = value;
    }
    public void AddAuthorizedUser(int user_Id)
    {
        authorizedUsers.Add(user_Id);
    }

    public void PlaceBulkOrder(DateTime delivery_daye)
    {
        
    }
    
    
     public void SaveToFile(string path = "./Users/Serialized/BusinessAcount.xml")
        {
            XmlSerializer serializer = new XmlSerializer(typeof(BusinessAcountInfo));
            using (StreamWriter writer = new StreamWriter(path))
            {
                var data = new BusinessAcountInfo
                {
                    Id = this.Id,
                    PhoneNumber1 = this.PhoneNumber1,
                    Email1 = this.Email1,
                    BusinessName = this.BusinessName,
                    Corparate_Discount = this.Corparate_Discount,
                    BusinessAddress = this.BusinessAddress,
                    Verified = this.Verified,
                    Documentation = this.Documentation,
                    AuthorizedUsers = this.AuthorizedUsers
                };
                serializer.Serialize(writer, data);
            }
        }
        
        public bool LoadFromFile(string path = "./Users/Serialized/BusinessAcount.xml")
        {
            if (!File.Exists(path))
            {
                Id = 0;
                PhoneNumber1 = String.Empty;
                Email1 = String.Empty;
                BusinessName = String.Empty;
                Corparate_Discount = 0.03;
                BusinessAddress = String.Empty;
                Verified = false;
                Documentation.Clear();
                AuthorizedUsers.Clear();
                return false;
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(BusinessAcountInfo));
                using (StreamReader reader = new StreamReader(path))
                {
                    var data = (BusinessAcountInfo)serializer.Deserialize(reader);
                    this.Id = data.Id;
                    this.PhoneNumber1 = data.PhoneNumber1;
                    this.Email1 = data.Email1;
                    this. BusinessName = data.BusinessName;
                    this.Corparate_Discount = data.Corparate_Discount;
                    this.BusinessAddress = data.BusinessAddress;
                    this.Verified = data.Verified;
                    this.Documentation = data.Documentation;
                    this.AuthorizedUsers = data.AuthorizedUsers;
                }
                return true;
            }
            catch
            {
                Id = 0;
                PhoneNumber1 = String.Empty;
                Email1 = String.Empty;
                BusinessName = String.Empty;
                Corparate_Discount = 0.03;
                BusinessAddress = String.Empty;
                Verified = false;
                Documentation.Clear();
                AuthorizedUsers.Clear();
                return false;
            }
        }
    
    [Serializable]
    public class BusinessAcountInfo
    {
        public int Id { get; set; }
        public string PhoneNumber1 { get; set; }
        public string BusinessName { get; set; }
        public string BusinessAddress { get; set; }
        public double Corparate_Discount { get; set; }
        public bool Verified { get; set; }
        public string Email1 { get; set; }
        [XmlArray("Documentation")]
        [XmlArrayItem("Document")]
        public ArrayList Documentation { get; set; } = new ArrayList();
        [XmlArray("AuthorizedUsers")]
        [XmlArrayItem("User_Id")]
        public ArrayList AuthorizedUsers { get; set; } = new ArrayList();
    }

    public BusinessAcount(int id, string phoneNumber, string email, 
        Wallet UserWallet, string business_name, string business_address,
        double coorparate_discount) : base(id, phoneNumber, email, UserWallet)
    {
        this.business_name = business_name;
        this.business_address = business_address;
        this.coorparate_discount = coorparate_discount;
    }
}
