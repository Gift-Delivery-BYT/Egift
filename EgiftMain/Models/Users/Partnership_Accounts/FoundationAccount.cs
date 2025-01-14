using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using Egift_main.Models.Users;

namespace Egift_main.Models.Partnership_Accounts;

public class FoundationAccount : User, IFoundationAccount
{
    // documentation[0 *] We were instructed to get rid of that
    private String BusinessName { get; set; }
    private String _businessAddress { get; set; }
    private bool _verified = false;
    private float _corporateDiscount { get; set; }
    private List<User> _authorizedUsers { get; set; }
    public IReadOnlyList<FoundationAccount> AllFoundationAccounts => _foundationAccountList.AsReadOnly();
    [XmlArray] private static List<FoundationAccount> _foundationAccountList = new List<FoundationAccount>();


    public List<User> AuthorizedUsers
    {
        get => _authorizedUsers;
        set => _authorizedUsers = value;
    }

    public bool Verified
    {
        get => _verified;
        set => _verified = value;
    }

    public string BusinessAdress
    {
        get => _businessAddress;
        set => _businessAddress = value;
    }

    public float CorporateDiscount
    {

        set => _corporateDiscount = value;
    }

    public FoundationAccount()
    {
    }

    public FoundationAccount(int id, string phoneNumber, string email, String businessAddress, float corporateDiscount,
        List<User> authorizedUsers) : base(id, phoneNumber, email)
    {
        AuthorizedUsers = authorizedUsers;
        BusinessAdress = businessAddress;
        CorporateDiscount = corporateDiscount;
        BusinessUserRole = BusinessUserRole.Foundation;
    }

    private void AddAuthorizedUser(Client client)
    {
        AuthorizedUsers.Add(client);
    }

    public void FindFreePropositions()
    {
        throw new NotImplementedException();
    }

    public List<FoundationAccount> GetAllFoundationAccounts()
    {
        IReadOnlyList<FoundationAccount> AllFoundationAccounts = _foundationAccountList.AsReadOnly();
        List<FoundationAccount> AllFoundationAccountsCopy = new List<FoundationAccount>();
        return AllFoundationAccountsCopy;
    }

    // public void AddAccountingInfo()
    // {
    //     throw new NotImplementedException();
    // }
    public new static bool Serialize(string path = "./Users/Serialized/Foundation_Account.xml")
    {
        XmlSerializer serializer = new XmlSerializer(typeof(FoundationAccount));
        using (StreamWriter writer = new StreamWriter(path))
        {
            serializer.Serialize(writer, (_foundationAccountList));
        }

        return true;
    }

    public new static bool Deserialize(string path = "./Users/Serialized/Foundation_Account.xml")
    {
        StreamReader file;
        try
        {
            file = File.OpenText(path);
        }
        catch (FileNotFoundException)
        {
            _foundationAccountList.Clear();
            return false;
        }

        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<User>));
        using (XmlTextReader reader = new XmlTextReader(file))
        {
            try
            {
                _foundationAccountList = (List<FoundationAccount>)xmlSerializer.Deserialize(reader);
            }
            catch (InvalidCastException)
            {
                _foundationAccountList.Clear();
                return false;
            }

            return true;
        }
    }
}