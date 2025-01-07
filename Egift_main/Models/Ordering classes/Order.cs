using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Egift_main.Models.Order;

namespace Egift_main.Order
{
    public class Order
    {
        public enum Status
        {
            Arrived,
            Shipping,
            Added
        }

        private int _id;
        private string _location;
        private string _description;
        private bool _trackerAssigned = false;
        private double _totalPrice;
        private double _discount;
        private double _finalPrice;
        private List<Item> _itemsInOrder;
        private List<Item> _items;
        private User _userOfOrder;
        public static List<Order> _orderList = new List<Order>();
        private Dictionary<Item, Quantity> _quantitiesOfItemsInOrder;
        private Tracker ShippingTracker { get; set; }

        public IReadOnlyList<Item> ItemsInOrder => _itemsInOrder.AsReadOnly();
        public User UserOfOrder
        {
            get => _userOfOrder;
            set => _userOfOrder = value;
        }
        public Order()
        {
            _items = new List<Item>();
            _itemsInOrder = new List<Item>();
            _quantitiesOfItemsInOrder = new Dictionary<Item, Quantity>();
        }

        public Order(User user, Tracker tracker, int id, List<Item> items,
            string location, string description, List<Item> itemsInOrder, double discount = 0)
            : this()
        {
            _id = id;
            _userOfOrder = user;
            _location = location;
            _description = description;
            _items = items ?? new List<Item>();
            _discount = discount;
            _totalPrice = CalculateTotalPrice();
            _finalPrice = _totalPrice - (_totalPrice * discount);
            _itemsInOrder = itemsInOrder;
            ShippingTracker = tracker;
            _orderList.Add(this);
        }

        private double CalculateTotalPrice()
        {
            double total = 0;
            foreach (var item in _items)
            {
                total += item.Pricehold;
            }
            return total;
        }
        private void AddUserToOrder(User user)
        {
            _userOfOrder = user;
            if (!OrderIsConnected(user)) user.OrdersOfUser.Add(this);
        }

        private void RemoveUserFromOrder(User user)
        {
            user.OrdersOfUser.Remove(this);
            _userOfOrder = null;
        }

        private bool OrderIsConnected(User user)
        {
            return user.OrdersOfUser.Contains(this);
        }

        public void AddItemToOrder(Item item, int quantity)
        {
            _quantitiesOfItemsInOrder.Add(item, new Quantity(item, this, quantity));
            _itemsInOrder.Add(item);
            if (!ItemIsConnected(item)) item.AddOrderHavingItem(this, quantity);
        }

        public void RemoveItemFromOrder(Item item)
        {
            if (item.OrdersHavingItems.Count > 1) item.RemoveOrderHavingItem(this);
            else Console.WriteLine("There must be at least one item left");
            _itemsInOrder.Remove(item);
        }

        public bool ItemIsConnected(Item item)
        {
            return ItemsInOrder.Contains(item);
        }

        public void AssignTracker(Tracker shippingTracker)
        {
            _trackerAssigned = true;
            ShippingTracker = shippingTracker;
            shippingTracker.AssignTrackerToOrder(this);
        }

        public void RemoveTracker()
        {
            if (ShippingTracker != null)
            {
                ShippingTracker = null;
                _trackerAssigned = false;
            }
        }

        public bool IsTrackerAssigned(Tracker tracker)
        {
            return _trackerAssigned;
        }

        public static int GenerateNewOrderId()
        {
            return _orderList.Count + 1;
        }

        public void TrackOrder()
        {
            if (_trackerAssigned)
            {
                string status = $"Tracking Order ID: {_id}\n" +
                                $"Tracking Number: {ShippingTracker.TrackerID}\n" +
                                $"Current Location: {ShippingTracker.GetLocation()}\n" +
                                $"Estimated Time of Arrival: {ShippingTracker.GetEstimatedTime()}";

                if (ShippingTracker.GetEstimatedTime() < DateTime.Now)
                {
                    status += "The order has been delayed";
                }

                Console.WriteLine(status);
            }
            else
            {
                Console.WriteLine($"Order ID: {_id} does not have a tracker assigned");
            }
        }

        public Status GetOrderStatus()
        {
            if (_trackerAssigned)
                return Status.Shipping;
            else if (_finalPrice == 0)
                return Status.Arrived;
            else
                return Status.Added;
        }

        public static List<Order> GetAllOrders()
        {
            return new List<Order>(_orderList);
        }

        public List<Item> GetItems()
        {
            return new List<Item>(_items);
        }

        public static bool Serialize(string path = "./Order/Serialized/Order.xml")
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Order>));
                using (StreamWriter writer = new StreamWriter(path))
                {
                    serializer.Serialize(writer, _orderList);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Deserialize(string path = "./Order/Serialized/Order.xml")
        {
            try
            {
                using (StreamReader file = File.OpenText(path))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Order>));
                    _orderList = (List<Order>)xmlSerializer.Deserialize(file);
                }
                return true;
            }
            catch
            {
                _orderList.Clear();
                return false;
            }
        }

        public double GetFinalPrice()
        {
            return _finalPrice;
        }

        private static bool IsValidOrder(Order order)
        {
            return order != null &&
                   order._id > 0 &&
                   !string.IsNullOrWhiteSpace(order._location) &&
                   !string.IsNullOrWhiteSpace(order._description) &&
                   order._items.Count > 0;
        }
    }
}
