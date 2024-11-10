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
    
    private DateTime _scheduledTimeDelivery { get; set; }
    
    public Order() { }
        
    [XmlArray]
    private static List<Order> _orderList = new List<Order>();

    public Order(bool treckerAssigned, int id, string location, string description, Trecker shippingTrecker, DateTime scheduledTimeDelivery)
    {
        _TreckerAssigned = treckerAssigned;
        _id = id;
        _location = location;
        _description = description;
        this.shippingTrecker = shippingTrecker;
        _scheduledTimeDelivery = scheduledTimeDelivery;
        
        _orderList.Add(this);
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
    
    public void AddItem(Item item)
    {
        Item.AddItem(item);
    }
    
    public void TrackOrder()
    {
        
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