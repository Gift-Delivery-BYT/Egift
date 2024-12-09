using System.Xml;
using System.Xml.Serialization;

namespace Egift_main.Order;

public class Order
{
    public enum status
    {
        arrived,
        shipping,
        added
    }

    private int _id { get; set; }
    private string _location { get; set; }
    private string _description { get; set; }

    private Tracker _ShippingTracker
    {
        get => _ShippingTracker;
        set => _ShippingTracker=value;
    }

    private bool _TreckerAssigned = false;
    
    private List<Item> _items = new List<Item>();
    private double _totalPrice { get; set; }
    private double _discount { get; set; }
    private double _finalPrice { get; set; }

    public Order()
    {
    }

    [XmlArray] public static List<Order> _orderList = new List<Order>();

    [XmlArray] private List<Item> _itemsInOrder { get; }

    public IReadOnlyList<Item> ItemsInOrder => _itemsInOrder.AsReadOnly();


    public Order(bool treckerAssigned, int id, List<Item> items,
        string location, string description, double discount = 0)
    {
        _TreckerAssigned = treckerAssigned;
        _id = id;
        _location = location;
        _description = description;
        _items = items ?? new List<Item>();
        _discount = discount;
        _totalPrice = CalculateTotalPrice();
        _finalPrice = _totalPrice - (_totalPrice * discount);
        _orderList.Add(this);
    }
    
    private double CalculateTotalPrice()
    {
        double total = 0;
        foreach (var item in _items)
        {
            total += item.pricehold;
        }
        return total;
    }
    
    public void AddItemToOrder(Item item, int quantity) {
        for (int i = 0; i < quantity; i++) _itemsInOrder.Add(item);
        if (!ItemIsConnected(item)) item.AddOrderHavingItem(this);
        
    }
    public void RemoveItemFromOrder(Item item) {
        if (item._OrdersHavingItems.Count >= 1) item.RemoveOrderHavingItem(this);
        else throw new Exception("There must be at least one item in order");
        _itemsInOrder.Remove(item);
    }

    private bool ItemIsConnected(Item item) {
        if (ItemsInOrder.Contains(item)) return true;
        return false;
    }

    public void AssignTrecker( Tracker shippingTracker)
    {
        _TreckerAssigned = true;
        this._ShippingTracker = shippingTracker;
        shippingTracker.AssignTrecker(this);
    }

    public void RemoveTracker() {
        _ShippingTracker = null;
    }

    public bool IsTreckerAssigned()
    {
        return _TreckerAssigned;
    }
    
    public static int GenerateNewOrderId()
    {
        return _orderList.Count + 1;
    }
    
   /* methods not needed
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
                            $"Tracking Number: {_ShippingTracker.TrackerID}\n" +
                            $"Current Location: {_ShippingTracker.GetLocation()}\n" +
                            $"Estimated Time of Arrival: {_ShippingTracker.GetEstimatedTime()}";

            if (_ShippingTracker.GetEstimatedTime() < DateTime.Now)
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
            return status.shipping;
        else if (_finalPrice == 0)
            return status.arrived; 
        else
            return status.added;
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