using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualBasic;

namespace Egift_main.Order;

[Serializable]
public class Item
{
     public int ItemID { get; set; }
     public string name { get; set; }
     public double pricehold { get; set; }
     private DateFormat date_of_production { get; set; }
     [XmlArray]
     private static List<Item> _itemList = new List<Item>();

     public Item() { }
     public Item(int itemId, string name, double pricehold, DateFormat dateOfProduction)
     {
          ItemID = itemId;
          name = name;
          pricehold = pricehold;
          date_of_production = dateOfProduction;
          _itemList.Add(this);
     }
     
     public static void AddItem(Item item)
     {
          _itemList.Add(item);
     }
     public static List<Item> GetItems()
     {
          return _itemList;
     }
     
     public static bool RemoveItem(int ItemID){
          var item = _itemList.Find(x=>x.ItemID == ItemID);
          if (item != null)
          {
               _itemList.Remove(item);
               return true;
          }
          return false;
     }
     
     public Item FindItemById(int ItemID)
     {
          return _itemList.Find(x=>x.ItemID == ItemID);
     }
    
     public static bool Serialize(string path = "./Order/Serialized/Item.xml")
     {
          XmlSerializer serializer = new XmlSerializer(typeof(Item));
          using (StreamWriter writer = new StreamWriter(path)) {
               serializer.Serialize(writer, _itemList);
          }
          return true;
     }
     public static bool Deserialize(string path = "./Order/Serialized/Item.xml")
     {
          StreamReader file;
          try {
               file = File.OpenText(path);
          }
          catch (FileNotFoundException) {
               _itemList.Clear();
               return false;
          }
          XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Exporter>));
          using (XmlTextReader reader = new XmlTextReader(file)) {
               try {
                    _itemList = (List<Item>)xmlSerializer.Deserialize(reader);
               }
               catch (InvalidCastException) {
                    _itemList.Clear();
                    return false;
               }
               return true;
          }
     }
}