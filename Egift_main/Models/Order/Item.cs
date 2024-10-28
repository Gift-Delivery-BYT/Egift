using Microsoft.VisualBasic;

namespace Egift_main.Order;

public class Item
{
     private int _ItemID { get; set; }
     string _name { get; set; }
     double _pricehold { get; set; }
     DateFormat _date_of_production { get; set; }

     public Item(int itemId, string name, double pricehold, DateFormat dateOfProduction)
     {
          _ItemID = itemId;
          _name = name;
          _pricehold = pricehold;
          _date_of_production = dateOfProduction;
     }
}