using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Serialization;
using Egift_main.Models.Order;

namespace Egift_main.Order;

public class Order
{
    public enum status
    {
        Arrived,
        Shipping,
        Added
    }

    private int _id;
    private string _location;
    private string _description;
    private bool _TreckerAssigned = false;
    private double _totalPrice;
    private double _discount;
    private double _finalPrice;
    private List<Item> _itemsInOrder;
    private List<Item> _items = new List<Item>();
    private User _userOfOrder;
    public static List<Order> _orderList = new List<Order>();
    
    public IReadOnlyList<Item> _ItemsInOrderRead  => _itemsInOrder; 
    [XmlArray] Dictionary<Item,Quantity> _QuantitiesOfItemsInOrder { get; }

    public IReadOnlyList<Item> ItemsInOrder => _itemsInOrder.AsReadOnly();
    public User UserOfOrder
    {
        get =>  _userOfOrder; 
        set => _userOfOrder = value;
    }

    private Tracker ShippingTracker
    {
        get => ShippingTracker;
        set => ShippingTracker=value;
    }


    public Order()
    {
    }
    public Order(User user,Tracker tracker, int id, List<Item> items,
        string location, string description,List<Item> ItemsInOrder,double discount = 0)
    {
     //   _TreckerAssigned = tracker;
        _id = id;
        _userOfOrder=user;
        _location = location;
        _description = description;
        _items = items ?? new List<Item>();
        _discount = discount;
        _totalPrice = CalculateTotalPrice();
        _finalPrice = _totalPrice - (_totalPrice * discount);
        _itemsInOrder = ItemsInOrder;
        ShippingTracker=tracker;
        _orderList = new List<Order>();
        _orderList.Add(this);
    }
    
    private double CalculateTotalPrice()
    {
        double total = 0;
        foreach (var item in _items)  total += item.Pricehold; 
        return total;
    }

    private void AddUserToOrder(User user)
    {
        _userOfOrder=user;
        if (!OrderIsConnected(user)) user.OrdersOfUser.Add(this);
    }

    private void RemoveUserFromOrder(User user)
    {
        user.OrdersOfUser.Remove(this);
        _userOfOrder=null;
    }

    private bool OrderIsConnected(User user) {
        if (user.OrdersOfUser.Contains(this)) return false;
        return true;
    }
    
    public void AddItemToOrder(Item item, int quantity)
    {

        _QuantitiesOfItemsInOrder.Add(item,new Quantity(item, this, quantity));
        _itemsInOrder.Add(item);
        if (!ItemIsConnected(item)) item.AddOrderHavingItem(this,quantity);
    }
    public void RemoveItemFromOrder(Item item) {
        if (item._ordersHavingItems.Count >= 1) item.RemoveOrderHavingItem(this);
        else Console.WriteLine("There must be at least one item left");
        _itemsInOrder.Remove(item);
    }

    public bool ItemIsConnected(Item item) {
        if (ItemsInOrder.Contains(item)) return true;
        return false;
    }

    public void AssignTracker( Tracker shippingTracker)
    {
        _TreckerAssigned = true;
        ShippingTracker = shippingTracker;
        shippingTracker.AssignTrackerToOrder(this);
    }

    public void RemoveTracker() {
        ShippingTracker = null;
    }

    public bool IsTrackerAssigned(Tracker tracker) {
        return _TreckerAssigned;
    }
  
    
    public static int GenerateNewOrderId()
    {
        return _orderList.Count + 1;
    }
    
   /* methods not needed? Update 23.12.24 I think we need to delete them XD
    public void AddItem(Item item)
    {
        Item.AddItem(item);
        _totalPrice = CalculateTotalPrice();
        _finalPrice = _totalPrice - (_totalPrice * _discount);
    }
    
    public void RemoveItem(Item item)
    {
        _items.Remove(item);
        _totalPrice = CalculateTotalPrice();
        _finalPrice = _totalPrice - (_totalPrice * _discount);
    } */
    
    public void TrackOrder()
    {
        if (_TreckerAssigned)
        {
            string status = $"Tracking Order ID: {_id}\n" +
                            $"Tracking Number: {ShippingTracker.TrackerID}\n" +
                            $"Current Location: {ShippingTracker.GetLocation()}\n" +
                            $"Estimated Time of Arrival: {ShippingTracker.GetEstimatedTime()}";

            if (ShippingTracker.GetEstimatedTime() < DateTime.Now)
            {
                status += "The order has been delayed";
            }

            Console.WriteLine(status);
        }
        else
        {
            Console.WriteLine("Order ID: {_id} does not have a tracker assigned");
        }
    }
    
    public status GetOrderStatus()
    {
        if (_TreckerAssigned)
            return status.Shipping;
        else if (_finalPrice == 0)
            return status.Arrived; 
        else
            return status.Added;
    }
    public static List<Order> GetAllOrders()
    {
        return new List<Order>(_orderList);  
    }

    public List<Item> GetItems()
    {
        return new List<Item>(_items); 
    }

    
    public static bool Serialize(string path = "./Order/Serialized/Order.xml")
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<Order>));
        using (StreamWriter writer = new StreamWriter(path)) {
            serializer.Serialize(writer, _orderList);
        }
        return true;
    }
    public static bool Deserialize(string path = "./Order/Serialized/Order.xml")
    {
        StreamReader file;
        try {
            file = File.OpenText(path);
        }
        catch (FileNotFoundException) {
            _orderList.Clear();
            return false;
        }
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Order>));
        using (XmlTextReader reader = new XmlTextReader(file)) {
            try {
                _orderList = (List<Order>)xmlSerializer.Deserialize(reader);
            }
            catch (InvalidCastException) {
                _orderList.Clear();
                return false;
            }
            return true;
        }
    }
    
    public double GetFinalPrice()
    {
        return _finalPrice;
    }
    
    private static bool IsValidOrder(Order order)
    {
        if (order != null &&
            order._id > 0 && 
            !string.IsNullOrWhiteSpace(order._location) && 
            !string.IsNullOrWhiteSpace(order._description) && 
            order._items.Count > 0) 
        {
            return true;
        }
        throw new ArgumentNullException("Invalid Order: One or more properties are not valid.");
    }
    
}