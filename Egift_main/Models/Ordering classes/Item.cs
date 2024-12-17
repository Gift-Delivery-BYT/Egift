using System.Xml;
using System.Xml.Serialization;
using Egift_main.Models.Order;
using Microsoft.VisualBasic;
using Twilio.Rest.Api.V2010.Account.Usage.Record;

namespace Egift_main.Order;

[Serializable]
public class Item
{
     public int ItemID { get; set; }
     public string name { get; set; }
     public double pricehold { get; set; }
     private DateFormat date_of_production { get; set; }
     
     [XmlArray] Dictionary<Order,Quantity> _QuantitiesOfItemsInOrder { get; }

     private Exporter _Exporter
     {
          get => _Exporter;
          set
          {
               _Exporter = value;
          }
     }

     [XmlArray]
     private List<Review_Sys> _ReviewsOfItem { get; }
     public IReadOnlyList<Review_Sys> ReviewSysList => _ReviewsOfItem.AsReadOnly();
     
     [XmlArray]
     private static List<Item> _itemList = new List<Item>();
     public IReadOnlyList<Order> OrdersHavingItems => _OrdersHavingItems.AsReadOnly();

     [XmlArray]
     public List<Order> _OrdersHavingItems { get; }

     public Item() { }
     public Item(int itemId, string name, double pricehold, DateFormat dateOfProduction)
     {
          ItemID = itemId;
          name = name;
          pricehold = pricehold;
          date_of_production = dateOfProduction;
          _itemList.Add(this);
     }
     
     //REVIEW TO ITEM CONNECTION

     public void AddReview(Review_Sys reviewSys)
     {
          _ReviewsOfItem.Add(reviewSys);
          if (!ReviewIsConnected(reviewSys)) reviewSys.AddReviewToItem(this); 
     }

     public void RemoveReview(Review_Sys reviewSys) 
     {
          _ReviewsOfItem.Remove(reviewSys);
          if (ReviewIsConnected(reviewSys))reviewSys.RemoveItemOfReview(this);
     }
     private bool ReviewIsConnected(Review_Sys reviewSys)
     {
          if (_ReviewsOfItem.Contains(reviewSys)) return true;
          return false;
     }

    
     
     //EXPORTER TO ITEM CONNECTION

     public void MarkExporter(Exporter exporter) {
          if (exporter.IsItemMarked(this)) throw new Exception("Item is already marked, You need to UnMark it first");
          _Exporter = exporter;
          if (ExportIsItemMarked(this)) _Exporter.MarkItem(this);
     }

     public void UnmarkExporter() {
          if (_Exporter.IsItemMarked(this)) _Exporter.UnmarkItem(this);
          _Exporter = null;
     }

     private bool ExportIsItemMarked(Item item) {
          if (item == null) return false;
          return true;
     }
     
     
     //ITEM TO ORDER CONNECTION

     public void AddOrderHavingItem(Order order, int quantity) {
          _OrdersHavingItems.Add(order);
          _QuantitiesOfItemsInOrder.Add(order, new Quantity(this, order, quantity));
          if (!OrderIsConnected(order)) order.AddItemToOrder(this, quantity);
     }

     public void RemoveOrderHavingItem(Order order) {
           _OrdersHavingItems.Remove(order);
           if (OrderIsConnected(order)) order.RemoveItemFromOrder(this);
     }

     private  bool OrderIsConnected(Order order) {
          if (OrdersHavingItems.Contains(order)) return true;
          return false;
     }
     
     
     //Methods adding and removing Items existing at all 
     public static void AddItem(Item item)
     {
          if (IsValidItem(item)) _itemList.Add(item);
          else throw new NullReferenceException();
     }
     public static List<Item> GetItems()
     {
          return new List<Item>(_itemList);  
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
          XmlSerializer serializer = new XmlSerializer(typeof(List<Item>));
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
          XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Item>));
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
     private static bool IsValidItem(Item item)
     {
          return item != null &&
                 item.ItemID != null &&
                 !string.IsNullOrWhiteSpace(item.name) &&
                 item.pricehold != null &&
                 item.date_of_production != null;
     }
}