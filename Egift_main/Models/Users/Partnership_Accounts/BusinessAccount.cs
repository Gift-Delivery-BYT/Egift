using Egift_main.Models.Users;

namespace Egift_main.Models.Partnership_Accounts;

public class BusinessAccount : User
{
    // documentation[0 *] We were instructed to get rid of that
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
    public BusinessAccount()
    {
    }

    public BusinessAccount(int id, string phoneNumber, string email, bool verified, string businessAddress, float corporateDiscount, List<User> authorizedUsers) : base(id, phoneNumber, email)
    {
        _verified = verified;
        _businessAddress = businessAddress;
        _corporateDiscount = corporateDiscount;
        _authorizedUsers = authorizedUsers;
        BusinessUserRole = BusinessUserRole.Business;
    }
}