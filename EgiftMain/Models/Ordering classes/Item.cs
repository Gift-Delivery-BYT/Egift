using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Egift_main.Models.Order;
using Microsoft.VisualBasic;

namespace Egift_main.Order
{
    [Serializable]
    public class Item
    {
        public int ItemID { get; set; }
        public string Name { get; set; }
        public double Pricehold { get; set; }
        public DateFormat DateOfProduction { get; set; }

        [XmlArray]
        private Dictionary<Order, Quantity> _quantitiesOfItemsInOrder;
        public IReadOnlyDictionary<Order, Quantity> QuantitiesOfItemsInOrder => _quantitiesOfItemsInOrder;

        public Exporter Exporter { get; set; }

        [XmlArray]
        private List<ReviewSys> _reviewsOfItem;
        public IReadOnlyList<ReviewSys> ReviewsOfItem => _reviewsOfItem.AsReadOnly();

        [XmlArray]
        private static List<Item> _itemList = new List<Item>();
        public static IReadOnlyList<Item> ItemList => _itemList.AsReadOnly();

        [XmlArray]
        public List<Order> _ordersHavingItems;
        public IReadOnlyList<Order> OrdersHavingItems => _ordersHavingItems.AsReadOnly();

        public Item()
        {
            _quantitiesOfItemsInOrder = new Dictionary<Order, Quantity>();
            _reviewsOfItem = new List<ReviewSys>();
            _ordersHavingItems = new List<Order>();
        }

        public Item(int itemId, string name, double pricehold, DateFormat dateOfProduction, Exporter exporter)
            : this()
        {
            ItemID = itemId;
            Name = name;
            Pricehold = pricehold;
            DateOfProduction = dateOfProduction;
           // Exporter = exporter;
            _itemList.Add(this);
        }

        // REVIEW TO ITEM CONNECTION
        public void AddReview(ReviewSys reviewSys)
        {
            if (!_reviewsOfItem.Contains(reviewSys))
            {
                _reviewsOfItem.Add(reviewSys);
                reviewSys.AddReviewToItem(this);
            }
        }

        public void RemoveReview(ReviewSys reviewSys)
        {
            if (_reviewsOfItem.Contains(reviewSys))
            {
                _reviewsOfItem.Remove(reviewSys);
                reviewSys.RemoveItemOfReview(this);
            }
        }

        public bool ReviewIsConnected(ReviewSys reviewSys)
        {
            return _reviewsOfItem.Contains(reviewSys);
        }

        // EXPORTER TO ITEM CONNECTION
        public void MarkExporter(Exporter exporter)
        {
            if (exporter.IsItemMarked(this)) throw new Exception("Item is already marked, You need to UnMark it first");
            Exporter = exporter;
            exporter.MarkItem(this);
        }

        public void UnmarkExporter()
        {
            if (Exporter != null && Exporter.IsItemMarked(this))
            {
                Exporter.UnmarkItem(this);
                Exporter = null;
            }
        }

        public bool ExportIsItemMarked(Item item)
        {
            return item != null;
        }

        // ITEM TO ORDER CONNECTION
        public void AddOrderHavingItem(Order order, int quantity)
        {
            if (!_ordersHavingItems.Contains(order))
            {
                _ordersHavingItems.Add(order);
                _quantitiesOfItemsInOrder[order] = new Quantity(this, order, quantity);
                order.AddItemToOrder(this, quantity);
            }
        }

        public void RemoveOrderHavingItem(Order order)
        {
            if (_ordersHavingItems.Contains(order))
            {
                _ordersHavingItems.Remove(order);
                _quantitiesOfItemsInOrder.Remove(order);
                order.RemoveItemFromOrder(this);
            }
        }

        public bool OrderIsConnected(Order order)
        {
            return _ordersHavingItems.Contains(order);
        }

        // Methods for managing items
        public static void AddItem(Item item)
        {
            if (IsValidItem(item)) _itemList.Add(item);
            else throw new NullReferenceException("Invalid item.");
        }

        public static List<Item> GetItems() => new List<Item>(_itemList);

        public static bool RemoveItem(int itemID)
        {
            var item = _itemList.Find(x => x.ItemID == itemID);
            if (item != null)
            {
                _itemList.Remove(item);
                return true;
            }
            return false;
        }

        public static Item FindItemById(int itemID) => _itemList.Find(x => x.ItemID == itemID);

        public static bool Serialize(string path = "./Order/Serialized/Item.xml")
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Item>));
                using (StreamWriter writer = new StreamWriter(path))
                {
                    serializer.Serialize(writer, _itemList);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Deserialize(string path = "./Order/Serialized/Item.xml")
        {
            try
            {
                using (StreamReader file = File.OpenText(path))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Item>));
                    _itemList = (List<Item>)xmlSerializer.Deserialize(file);
                }
                return true;
            }
            catch
            {
                _itemList.Clear();
                return false;
            }
        }

        private static bool IsValidItem(Item item)
        {
            return item != null &&
                   !string.IsNullOrWhiteSpace(item.Name) &&
                   item.Pricehold > 0 &&
                   item.DateOfProduction != null;
        }
    }
}
