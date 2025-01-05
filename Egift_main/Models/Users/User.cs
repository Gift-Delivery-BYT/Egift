using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using Egift_main.Order;

namespace Egift_main;
[Serializable]
public class User
{
    private int id;
    private string PhoneNumber;
    private string _email;
    private List<Order.Order> _ordersOfUser { get; set; }
    private List<Notifications> _notifications { get; set; } = new List<Notifications>();
    public List<Notifications> Notifications => _notifications;

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

    public void AddNotification(Notifications notification)
    {
        if (_notifications.Contains(notification)) return;

        _notifications.Add(notification);
        if (notification.User != this) notification.User = this; 
    }

    public void RemoveNotification(Notifications notification)
    {
        if (!_notifications.Contains(notification)) return;

        _notifications.Remove(notification);
        if (notification.User == this) notification.User = null; 
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

    private bool UserIsConnected(Order.Order order) {
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