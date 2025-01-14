using System.Xml;
using System.Xml.Serialization;
using Egift_main.Models.Users;
using Egift_main.Order;

namespace Egift_main.Models.Partnership_Accounts;

public class BusinessAccount : User,IBusinessAccount
{
    // documentation[0 *] We were instructed to get rid of that
    private String _BusinessAddress { get; set; }
    private bool _verified = false;
    private float _corporateDiscount { get; set; }
    private List<User> _authorizedUsers { get; set; }
    [XmlArray]
    private static List<BusinessAccount> _businessaccountList = new List<BusinessAccount>();

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
        get => _BusinessAddress;
        set => _BusinessAddress = value;
    }

    public float CorporateDiscount
    {
        
        set => _corporateDiscount = value;
    }
    
    public BusinessAccount() { }

    public BusinessAccount(int id, string phoneNumber, string email, bool verified, string businessAddress, float corporateDiscount, List<User> authorizedUsers) : base(id, phoneNumber, email)
    {
        _verified = verified;
        _BusinessAddress = businessAddress;
        _corporateDiscount = corporateDiscount;
        _authorizedUsers = authorizedUsers;
        BusinessUserRole = BusinessUserRole.Business;
    }

    public void AddAuthorizedUser(User user)
    {
        _authorizedUsers.Add(user);
    }
    public void PlaceBulkOrder(DateTime delivery_day, List<int> bulOrderID)
    {
        if (Item.GetItems().Count == 0)
        {
            Console.WriteLine("There are no items in the bulk order.");
            return;
        }

        var allItems = Item.GetItems();
        var itemsToOrder = allItems.Where(item => bulOrderID.Contains(item.ItemID)).ToList();
        
        if (itemsToOrder.Count == 0)
        {
            Console.WriteLine("No items found with a given id");
            return;
        }
        
        Egift_main.Order.Order newOrder = new Egift_main.Order.Order(
            id: Egift_main.Order.Order.GenerateNewOrderId(),
            location: _BusinessAddress,
            description: "Place a new Bulk Order{itemsToOrder}", 
            discount: _corporateDiscount,
            items: itemsToOrder
        );
        Console.WriteLine($"Bulk order placed {itemsToOrder.Count} " +
                          $" and items for delivery at {delivery_day.ToShortDateString()}");

        Egift_main.Order.Order._orderList.Add(newOrder);
    }
    public new static bool Serialize(string path = "./Users/Serialized/BusinessAcount.xml")
    {
        XmlSerializer serializer = new XmlSerializer(typeof(BusinessAccount));
        using (StreamWriter writer = new StreamWriter(path)) {
            serializer.Serialize(writer, _businessaccountList);
        }
        return true;
    }
    public new static bool Deserialize(string path = "./Users/Serialized/BusinessAcount.xml")
    {
        StreamReader file;
        try {
            file = File.OpenText(path);
        }
        catch (FileNotFoundException) {
            _businessaccountList.Clear();
            return false;
        }
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<BusinessAccount>));
        using (XmlTextReader reader = new XmlTextReader(file)) {
            try {
                _businessaccountList = (List<BusinessAccount>)xmlSerializer.Deserialize(reader);
            }
            catch (InvalidCastException) {
                _businessaccountList.Clear();
                return false;
            }
            return true;
        }
    }
}