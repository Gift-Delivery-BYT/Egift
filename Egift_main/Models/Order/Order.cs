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

}