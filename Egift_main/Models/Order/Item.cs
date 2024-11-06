using System.Xml.Serialization;
using Microsoft.VisualBasic;

namespace Egift_main.Order;

[Serializable]
public class Item
{
     private int ItemID { get; set; }
     private string name { get; set; }
     private double pricehold { get; set; }
     private DateFormat date_of_production { get; set; }

     public Item(int itemId, string name, double pricehold, DateFormat dateOfProduction)
     {
          ItemID = itemId;
          name = name;
          pricehold = pricehold;
          date_of_production = dateOfProduction;
     }
     
     
     public void Save(string path = "./Item/Serialized/Item.xml")
     {
          XmlSerializer serializer = new XmlSerializer(typeof(ItemInfo));
          using (StreamWriter writer = new StreamWriter(path))
          {
               var data = new ItemInfo
               {
                    ItemID = this.ItemID,
                    Name = this.name,
                    PriceHold = this.pricehold,
                    DateOfProduction = this.date_of_production
               };
               serializer.Serialize(writer, data);
          }
     }

    
     public bool LoadFromFile(string path = "./Item/Serialized/Item.xml")
     {
          if (!File.Exists(path))
          {
               return false;
          }

          try
          {
               XmlSerializer serializer = new XmlSerializer(typeof(ItemInfo));
               using (StreamReader reader = new StreamReader(path))
               {
                    var data = (ItemInfo)serializer.Deserialize(reader);
                    this.ItemID = data.ItemID;
                    name = data.Name;
                    pricehold = data.PriceHold;
                    date_of_production = data.DateOfProduction;
               }
               return true;
          }
          catch
          {
               return false;
          }
     }
     
     [Serializable]
     public class ItemInfo
     {
          public int ItemID { get; set; }
          public string Name { get; set; }
          public double PriceHold { get; set; }
          public DateFormat DateOfProduction { get; set; }
     }
}