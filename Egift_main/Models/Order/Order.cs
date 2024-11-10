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
    private Trecker shippingTrecker { get; set; }
    
    private bool _TreckerAssigned = false;
    private List<Item> _items = new List<Item>();
    private double _totalPrice  { get; set; }
    private double _discount { get; set; }
    private double _finalPrice { get; set; }
    
    public Order() { }
        
    [XmlArray]
    public static List<Order> _orderList = new List<Order>();

    public Order(bool treckerAssigned, int id, List<Item> items,
        string location, string description, Trecker shippingTrecker, double discount = 0)
    {
        _TreckerAssigned = treckerAssigned;
        _id = id;
        _location = location;
        _description = description;
        _items = items ?? new List<Item>();
        _discount = discount;
        _totalPrice = CalculateTotalPrice();
        _finalPrice = _totalPrice - (_totalPrice * discount);
        this.shippingTrecker = shippingTrecker;
        
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

    public double GetFinalPrice()
    {
        return _finalPrice;
    }

    public void AssignTrecker( Trecker shippingTrecker)
    {
        _TreckerAssigned = true;
        this.shippingTrecker = shippingTrecker;
    }

    public bool IsTreckerAssigned()
    {
        return _TreckerAssigned;
    }
    
    public static int GenerateNewOrderId()
    {
        return _orderList.Count + 1;
    }
    
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
    }
    
    public void TrackOrder()
    {
        if (_TreckerAssigned)
        {
            string status = $"Tracking Order ID: {_id}\n" +
                            $"Tracking Number: {shippingTrecker.TrackerID}\n" +
                            $"Current Location: {shippingTrecker.GetLocation()}\n" +
                            $"Estimated Time of Arrival: {shippingTrecker.GetEstimatedTime()}";

            if (shippingTrecker.GetEstimatedTime() < DateTime.Now)
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
        return _orderList;
    }
    
    public static bool Serialize(string path = "./Order/Serialized/Order.xml")
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Order));
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
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Exporter>));
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
    
}