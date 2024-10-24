namespace Egift_main.Order;

public class Order
{
    enum status
    {
        arrived,
        shipping,
        added
    }

    private int _id;
    private string _location;
    private string _description;
    private Trecker shippingTrecker;
    private bool _TreckerAssigned = false;

    public void assignTrecker( Trecker shippingTrecker)
    {
        _TreckerAssigned = true;
        this.shippingTrecker = shippingTrecker;
    }

  public  bool IsTreckerAssigned()
    {
        return _TreckerAssigned;
    }

}