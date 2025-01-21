using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using Egift_main.Models.Partnership_Accounts;
using Egift_main.Models.Users;
using Egift_main.Order;
//
namespace Egift_main;
[Serializable]
public class User
{
    private int id;
    public string PhoneNumber;
    public string _email;
    private List<Order.Order> _ordersOfUser { get; set; }
    private List<Notifications> _notifications { get; set; } = new List<Notifications>();
    public List<Notifications> Notifications => _notifications;
    
    //Business
    private String _BusinessAddress { get; set; }
    private bool _verified = false;
    private float _corporateDiscount { get; set; }
    private List<User> _authorizedUsers { get; set; }
    [XmlArray]
    private static List<BusinessAccount> _businessaccountList = new List<BusinessAccount>();
    // Foundation
    private String BusinessName { get; set; }
    private String _businessAddress { get; set; }
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
    
    [XmlIgnore]
    private Dictionary<int, Refund> _refunds = new Dictionary<int, Refund>();
    [XmlArray("Refunds")]
    [XmlArrayItem("RefundEntry")]
    public List<KeyValuePair<int, Refund>> RefundsList
    {
        get => _refunds?.ToList();
        set
        {
            Console.WriteLine($"Deserializing RefundsList with {value?.Count ?? 0} items.");
            if (value != null)
            {
                _refunds = value.ToDictionary(x => x.Key, x => x.Value);
            }
            else
            {
                _refunds = new Dictionary<int, Refund>();
            }
        }
    }
    public BusinessUserRole BusinessUserRole { get; set; }

    [XmlArray] private static List<User> _userList { get; set; }

    public User() { }
    public User(int id, string phoneNumber, string email,BusinessUserRole busUserRole=BusinessUserRole.Basic)
    {
        this.id = id;
        PhoneNumber = phoneNumber;
        _email = email;
        _userList = new List<User>();
        _userList.Add(this);
        _ordersOfUser = new List<Order.Order>();
        BusinessUserRole = BusinessUserRole.Basic;
    }
    [XmlIgnore]
    public Dictionary<int, Refund> Refunds
    {
        get => _refunds;
        set => _refunds = value ?? throw new ArgumentNullException(nameof(value), "Refunds cannot be null.");
    }
    public int Id
    {
        get => id;
        set => id = value;
    }
    
    public string PhoneNumber1
    {
        get => PhoneNumber;
        set
        {
            if (value.Length < 4)
            {
                throw new ArgumentException("Phone number must be at least 4 characters long.");
            }
            PhoneNumber = value;
        }
    }
    public string Email
    {
        get => _email;
        set
        {
            if (value == null) 
            {
                throw new ArgumentNullException(nameof(value), "Email cannot be null.");
            }
        
            if (string.IsNullOrEmpty(value) || !value.Contains("@"))
            {
                throw new ArgumentException("Email must contain '@'.");
            }
        
            _email = value;
        }
    }
    public List<Order.Order> OrdersOfUser
    {
        get => _ordersOfUser;
        set
        {
            if (value == null) throw new ArgumentNullException(nameof(value), "ORDER cannot be null.");
            _ordersOfUser = value;
        } 
    }
    //Business
    public User(int id, string phoneNumber, string email, bool verified, string businessAddress, float corporateDiscount, List<User> authorizedUsers, BusinessUserRole businessUserRole=BusinessUserRole.Business) 
    {
        _verified = verified;
        _BusinessAddress = businessAddress;
        _corporateDiscount = corporateDiscount;
        _authorizedUsers = authorizedUsers;
        BusinessUserRole = BusinessUserRole.Business;
    }
    //Foundation
    public User(int id, string phoneNumber, string email, String businessAddress, float corporateDiscount,
        List<User> authorizedUsers, BusinessUserRole businessUserRole=BusinessUserRole.Foundation) 
    {
        AuthorizedUsers = authorizedUsers;
        BusinessAdress = businessAddress;
        CorporateDiscount = corporateDiscount;
        BusinessUserRole = BusinessUserRole.Foundation;
    }

    public void AddRefund(Refund refund)
    {
        if (refund == null) throw new ArgumentNullException(nameof(refund), "Refund cannot be null.");
        if (_refunds.ContainsKey(refund.RefundId))
        {
            throw new InvalidOperationException($"Refund with ID {refund.RefundId} already exists for this user.");
        }

        _refunds[refund.RefundId] = refund;
        refund.User = this;
    }
    public void RemoveRefund(int refundId)
    {
        if (_refunds.ContainsKey(refundId))
        {
            var refund = _refunds[refundId];
            refund.User = null;
            _refunds.Remove(refundId);
        }
    }
    private Refund GetRefund(int refundId)
    {
        return _refunds.ContainsKey(refundId) ? _refunds[refundId] : null;
    }

    internal void AddNotification(Notifications notification)
    {
        if (_notifications.Contains(notification)) return;

        _notifications.Add(notification);
        if (!notification.Users.Contains(this)) notification.Users.Add(this); 
    }

    internal void RemoveNotification(Notifications notification)
    {
        if (!_notifications.Contains(notification)) return;

        _notifications.Remove(notification);
        if (notification.Users.Contains(this)) notification.Users.Remove(this); 
    }
    
    private void AddOrderToUser(Order.Order order)
    {
       _ordersOfUser.Add(order);
       if (!UserIsConnected(order)) order.UserOfOrder = this;
    }

    private void RemoveOrderFromUser(Order.Order order)
    {
        order.UserOfOrder=null;
        _ordersOfUser.Remove(order);
    }

    public bool UserIsConnected(Order.Order order) {
        if (order.UserOfOrder==null) return false;
        return true;
    }

    
    public static bool Serialize(string path = "./Users/Serialized/User.xml")
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<User>));
        using (StreamWriter writer = new StreamWriter(path)) {
            serializer.Serialize(writer, (_userList));
        }
        return true;
    }
    public static bool Deserialize(string path = "./Users/Serialized/User.xml")
    {
        StreamReader file;
        try {
            file = File.OpenText(path);
        }
        catch (FileNotFoundException) {
            _userList.Clear();
            return false;
        }
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<User>));
        using (XmlTextReader reader = new XmlTextReader(file)) {
            try {
                _userList = (List<User>)xmlSerializer.Deserialize(reader);
            }
            catch (InvalidCastException) {
                _userList.Clear();
                return false;
            }
            return true;
        }
    }
    //Business
    public void AddAuthorizedUser(User user)
    {
        if(this.BusinessUserRole != BusinessUserRole.Business) throw new UnauthorizedAccessException();
        _authorizedUsers.Add(user);
    }
    public void PlaceBulkOrder(DateTime delivery_day, List<int> bulOrderID)
    {
        if(this.BusinessUserRole != BusinessUserRole.Business) throw new UnauthorizedAccessException();
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
    //Foundation
    private void AddAuthorizedUser(Client client)
    {
        if(this.BusinessUserRole != BusinessUserRole.Foundation) throw new UnauthorizedAccessException();
        AuthorizedUsers.Add(client);
    }

    public void FindFreePropositions()
    {
        if(this.BusinessUserRole != BusinessUserRole.Business) throw new UnauthorizedAccessException();
        throw new NotImplementedException();
    }

    public List<FoundationAccount> GetAllFoundationAccounts()
    {
        if(this.BusinessUserRole != BusinessUserRole.Foundation) throw new UnauthorizedAccessException();
        IReadOnlyList<FoundationAccount> AllFoundationAccounts = _foundationAccountList.AsReadOnly();
        List<FoundationAccount> AllFoundationAccountsCopy = new List<FoundationAccount>();
        return AllFoundationAccountsCopy;
    }
   
}