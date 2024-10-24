using Microsoft.VisualBasic;

namespace Egift_main.Order;

public class Item
{
     int _ItemID;
     string _name;
     double _pricehold;
     DateFormat _date_of_production;

     public Item(int itemId, string name, double pricehold, DateFormat dateOfProduction)
     {
          _ItemID = itemId;
          _name = name;
          _pricehold = pricehold;
          _date_of_production = dateOfProduction;
     }
}