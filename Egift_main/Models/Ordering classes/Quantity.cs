using Egift_main.Order;

namespace Egift_main.Models.Order;

public class Quantity
{
    private Item _item { get; set; }
    private Egift_main.Order.Order _order { get; set; }
    private int _quantity { get; set; }

    public Quantity(Item item, Egift_main.Order.Order order, int quantity)
    {
        _item = item;
        _order = order;
        _quantity = quantity;
    }
}