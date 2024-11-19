using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using Egift_main.Order;

namespace Egift_main;

[Serializable]
public class BusinessAcount: User
{
    private ArrayList documentation = new ArrayList();
    private string business_name;
    private string business_address;
    private bool isVerified;
    private double coorparate_discount = 0.03;
    private Trecker shippingTrecker;
    private ArrayList authorizedUsers = new ArrayList();
        
    [XmlArray]
    private static List<BusinessAcount> _businessaccountList = new List<BusinessAcount>();
    public BusinessAcount() { }
    public bool Verified
    {
        get => isVerified;
        set => isVerified = value;
    }
    
    public ArrayList Documentation
    {
        get => documentation;
        set => documentation = value ?? throw new ArgumentNullException(nameof(value));
    }
    
    public ArrayList AuthorizedUsers
    {
        get => authorizedUsers;
        set => authorizedUsers = value ?? throw new ArgumentNullException(nameof(value));
    }
    
    public double Corparate_Discount
    {
        get => coorparate_discount;
        set => coorparate_discount = value;
    }
    
    public string BusinessName
    {
        get => business_name;
        set => business_name = value;
    }
    public string BusinessAddress
    {
        get => business_address;
        set => business_address = value;
    }
    public void AddAuthorizedUser(int user_Id)
    {
        authorizedUsers.Add(user_Id);
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
        
        Order.Order newOrder = new Order.Order(
            id: Order.Order.GenerateNewOrderId(),
            treckerAssigned: true,
            location: BusinessAddress,
            description: "Place a new Bulk Order{itemsToOrder}", 
            shippingTrecker: shippingTrecker,
            discount: Corparate_Discount,
            items: itemsToOrder
        );

        Console.WriteLine($"Bulk order placed {itemsToOrder.Count} " +
                          $" and items for delivery at {delivery_day.ToShortDateString()}");

        Order.Order._orderList.Add(newOrder);
    }
    
    public static bool Serialize(string path = "./Users/Serialized/BusinessAcount.xml")
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<BusinessAcount>));
        using (StreamWriter writer = new StreamWriter(path)) {
            serializer.Serialize(writer, _businessaccountList);
        }
        return true;
    }
    public static bool Deserialize(string path = "./Users/Serialized/BusinessAcount.xml")
    {
        StreamReader file;
        try {
            file = File.OpenText(path);
        }
        catch (FileNotFoundException) {
            _businessaccountList.Clear();
            return false;
        }
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<BusinessAcount>));
        using (XmlTextReader reader = new XmlTextReader(file)) {
            try {
                _businessaccountList = (List<BusinessAcount>)xmlSerializer.Deserialize(reader);
            }
            catch (InvalidCastException) {
                _businessaccountList.Clear();
                return false;
            }
            return true;
        }
    }
    
    
    public BusinessAcount(int id, string phoneNumber, string email, 
        Wallet UserWallet, string business_name, string business_address,
        double coorparate_discount, Trecker tracker) : base(id, phoneNumber, email, UserWallet)
    {
        this.business_name = business_name;
        this.business_address = business_address;
        this.coorparate_discount = coorparate_discount;
        this.shippingTrecker = tracker;
        
        _businessaccountList.Add(this);
    }
    
    private static bool IsValidBusinessAccount(BusinessAcount account)
    {
        return account != null &&
               !string.IsNullOrWhiteSpace(account.BusinessName) &&
               !string.IsNullOrWhiteSpace(account.BusinessAddress) &&
               account.Corparate_Discount >= 0 && 
               account.AuthorizedUsers.Count > 0;
    }
}
