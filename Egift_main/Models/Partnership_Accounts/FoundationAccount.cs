namespace Egift_main.Models.Partnership_Accounts;

public class FoundationAccount : User
{
    // documentation[0 *] We were instructed to get rid of that
    private String BusinessName { get; set; }
    private String _businessAddress { get; set; }
    private bool _verified = false;
    private float _corporateDiscount { get; set; }
    private List<User> _authorizedUsers { get; set; }

    public List<User> AuthorizedUsers {
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
    public FoundationAccount() {
    }

    public FoundationAccount(int id, string phoneNumber, string email,String businessAddress,float corporateDiscount,List<User> authorizedUsers) : base(id, phoneNumber, email) {
        AuthorizedUsers = authorizedUsers;
        BusinessAdress = businessAddress;
        CorporateDiscount = corporateDiscount;
    }

    private void addAuthorizedUser(Client client) {
        AuthorizedUsers.Add(client);
    }
    // what the hell is that -_-?
   public void placeBlukOrder()
    {
        
    }
}