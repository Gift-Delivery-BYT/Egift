namespace Egift_main;

public class User
{
    
    private int id;
    private string PhoneNumber;
    private string Email;
    private Wallet _UserWallet;

    public User(int id, string phoneNumber, string email, Wallet userWallet)
    {
        this.id = id;
        PhoneNumber = phoneNumber;
        Email = email;
        _UserWallet = userWallet;
    }

    public int Id
    {
        get => id;
        set => id = value;
    }

    public string PhoneNumber1
    {
        get => PhoneNumber;
        set => PhoneNumber = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Email1
    {
        get => Email;
        set => Email = value ?? throw new ArgumentNullException(nameof(value));
    }

    public Wallet UserWallet
    {
        get => _UserWallet;
        set => _UserWallet = value ?? throw new ArgumentNullException(nameof(value));
    }

    public User()
    {
    }
}