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
    public User(int id, string phoneNumber, string email)
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
   
}