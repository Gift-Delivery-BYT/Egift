using System.Xml.Serialization;

namespace Egift_main.Order;

public class Order
{
    // public Order(bool treckerAssigned, int id, string location, string description, Trecker shippingTrecker)
    // {
    //     _TreckerAssigned = treckerAssigned;
    //     _id = id;
    //     _location = location;
    //     _description = description;
    //     this.shippingTrecker = shippingTrecker;
    // }

    enum status
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
    
    

    public void AssignTrecker( Trecker shippingTrecker)
    {
        _TreckerAssigned = true;
        this.shippingTrecker = shippingTrecker;
    }

    public bool IsTreckerAssigned()
    {
        return _TreckerAssigned;
    }

    public void AddItem()
    {
        
    }
    
    public void TrackOrder()
    {
        
    }
    
    public void Save(string path = "./Order/Serialized/Order.xml")
    {
        XmlSerializer serializer = new XmlSerializer(typeof(OrderInfo));
        using (StreamWriter writer = new StreamWriter(path))
        {
            var data = new OrderInfo
            {
                Id = _id,
                Location = _location,
                Description = _description,
                ShippingTrecker = shippingTrecker,
                TreckerAssigned = _TreckerAssigned
            };
            serializer.Serialize(writer, data);
        }
    }

    
    public bool LoadFromFile(string path = "./Order/Serialized/Order.xml")
    {
            XmlSerializer serializer = new XmlSerializer(typeof(OrderInfo));
            using (StreamReader reader = new StreamReader(path))
            {
                var data = (OrderInfo)serializer.Deserialize(reader);
                _id = data.Id;
                _location = data.Location;
                _description = data.Description;
                shippingTrecker = data.ShippingTrecker;
                _TreckerAssigned = data.TreckerAssigned;
            }
            return true;
    }
    
    [Serializable]
    public class OrderInfo
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public Trecker ShippingTrecker { get; set; }
        public bool TreckerAssigned { get; set; }
    }

}