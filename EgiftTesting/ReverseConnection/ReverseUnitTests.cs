using System.Runtime.Serialization;
using Egift_main;
using Egift_main.Models.Order;
using Egift_main.Order;
using Egift_main.Subscription;
using Microsoft.VisualBasic;

namespace EgiftTesting.ReverseConnection;

public class ReverseUnitTests
{
    
    private Wallet _wallet;
    private Employee _employee;
    private Refund _refund;
    private Client _client;
    private Order _order;
    private Item _item;
    private Review_Sys _review;
    private Exporter _exporter;
    private Subscription _subscription;
    private Tracker _tracker;
    private User _user;
    private Schedule _schedule;

    [SetUp]
    public void Setup()
    {
        _wallet = new Wallet();
        _employee = new Employee(2, "987-654", "employee@example.com", _wallet, "Koszykowska", "Abdullah");
        _refund = new Refund();
        _client = new Client(1, "123-456", "client@example.com", _wallet, "John Doe");
        _item = new Item(1,"",0.5,new DateFormat(), new Exporter());
        _order = new Order(new User(),new Tracker(1,new DateTime(2004,12,12),new Order()),1,new List<Item>() { _item },"5234s st.","blah blah blah",new List<Item>(),0.00); 
        _review = new Review_Sys(1,"great!");
        _exporter = new Exporter("Company Vinntsia", "USA", "123", 100.5f, 1234567890, DateTime.Now.AddDays(10),new List<Item>());
        _employee = new Employee(1, "1234567890", "test@example.com", new Wallet(), "123 Main St", "John Doe");
        _tracker = new Tracker(1, DateTime.Now.AddHours(2), new Order());
        _user = new User(399, "1234567890", "user@poop.com");
        _schedule = new Schedule();
    }
    //Qualified - User-Refund
    [Test]
    public void AddRefund_AdssRefundTohUser()
    {
        _user.AddRefund(_refund);
        Assert.IsTrue(_user.Refunds.ContainsKey(_refund.RefundId), "Refund should be added to the user's refunds collection.");
        Assert.AreEqual(_user, _refund.User, "Refund's User property should reference the correct user.");
    }

    [Test]
    public void RemoveRefund_removesRefundFromUser()
    {
        _user.AddRefund(_refund);
        _user.RemoveRefund(_refund.RefundId);
        Assert.IsFalse(_user.Refunds.ContainsKey(_refund.RefundId), "Refund should be removed from the user's refunds collection.");
        Assert.IsNull(_refund.User, "Refund's User property should be null after removal.");
    }

    [Test]
    public void Refund_HasEmployee()
    {
        _employee.Refund = _refund;
        Assert.NotNull(_refund.Employee);
        Assert.AreEqual(_employee, _refund.Employee);
    }

    [Test]
    public void Employee_HasRefund()
    {
        _employee.Refund = _refund;
        Assert.NotNull(_employee.Refund);
        Assert.AreEqual(_refund, _employee.Refund);
    }


    [Test]
    public void ReverseConnection_EmployeeAndRefund()
    {
        _employee.Refund = _refund;
            
        Assert.AreEqual(_employee, _refund.Employee);
        Assert.AreEqual(_refund, _employee.Refund);
    }
    
    //Employee to Tracker reverse
    
        [Test]
            public void AddTracker_EmpAndTrackerReverse()
            {
                _employee.AddTracker(_tracker);
                Assert.Contains(_tracker, _employee._trackers, "Tracker was not added to Employee.");
                Assert.AreEqual(_employee, _tracker.GetType().GetProperty("_assignedEmployee", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(_tracker), "fail.");
            }
            
            [Test]
            public void RemoveTracker_TrackerRemovedFromEmployee()
            {
                _employee.AddTracker(_tracker);
                _employee.RemoveTracker(_tracker);
                Assert.IsFalse(_employee._trackers.Contains(_tracker), "Tracker was not removed from Employee.");
                Assert.IsNull(_tracker.GetType().GetProperty("_assignedEmployee", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(_tracker), "fail");
            }
            
            [Test]
            public void Subscription_AddClient_ReverseConnection()
            {
                _subscription.AddClient(_client);

                Assert.Contains(_client, _subscription.Clients_subscription, "Client should be added to the subscription.");
                Assert.AreEqual(_subscription, _client.Subscription, "Subscription should be assigned to the client.");
            }

            [Test]
            public void Subscription_RemoveClient_ReverseConnection()
            {
                _subscription.AddClient(_client);

                _subscription.RemoveClient(_client);

                Assert.IsFalse(_subscription.ClientIsConnected(_client), "Client should be removed from the subscription.");
                Assert.IsNull(_client.Subscription, "Subscription should be null after removal.");
            }
    //composition
    [Test]
    public void AssignScheduleToEmployee_ShouldSetOwner()
    {
        _employee.AddSchedule(_schedule);
        Assert.AreEqual(_employee, _schedule.Owner);
        Assert.AreEqual(_schedule, _employee.Schedule);
    }

    [Test]
    public void RemoveScheduleFromEmployee_ShouldClearOwner()
    {
        _employee.AddSchedule(_schedule);
        _employee.RemoveSchedule();
        Assert.IsNull(_schedule.Owner);
        Assert.IsNull(_employee.Schedule);
    }

    // REVIEW TO ITEM CONNECTION
    [Test]
    public void AddReview_ShouldAddReviewToItem()
    {
        
        _item.AddReview(_review);
        Assert.IsTrue(_item.ReviewsOfItem.Contains(_review), "Review was not added to the item.");
    }
    

    [Test]
    public void ReviewIsConnected_ShouldReturnTrueForConnectedReview()
    {
        _item.AddReview(_review);
        var isConnected = _item.ReviewIsConnected(_review);
        Assert.IsTrue(isConnected, "ReviewIsConnected returned false for a connected review.");
    }

    [Test]
    public void ReviewIsConnected_ShouldReturnFalseForDisconnectedReview()
    {
        var isConnected = _item.ReviewIsConnected(_review);
        Assert.IsFalse(isConnected, "ReviewIsConnected returned true for a disconnected review.");
    }

    // EXPORTER TO ITEM CONNECTION
   

    [Test]
    public void MarkExporter_ShouldThrowExceptionIfItemAlreadyMarked()
    {
        
        _item.MarkExporter(_exporter);
        Assert.Throws<Exception>(() => _item.MarkExporter(_exporter), "Item is already marked, You need to UnMark it first");
    }
    

    [Test]
    public void ExportIsItemMarked_ShouldReturnTrueIfItemIsNotNull()
    {
        var isMarked = _item.ExportIsItemMarked(_item);
        Assert.IsTrue(isMarked, "ExportIsItemMarked returned false for a non-null item.");
    }

    [Test]
    public void ExportIsItemMarked_ShouldReturnFalseIfItemIsNull()
    {
        var isMarked = _item.ExportIsItemMarked(null);
        Assert.IsFalse(isMarked, "ExportIsItemMarked returned true for a null item.");
    }

    // ITEM TO ORDER CONNECTION
    /*[Test]
    public void AddOrderHavingItem_ShouldAddOrderWithQuantity()
    {
        var quantity = 2;
        _item.AddOrderHavingItem(_order, quantity);
        Assert.IsTrue(_item.OrdersHavingItems.Contains(_order), "Order was not added to the item's orders.");
        Assert.IsTrue(_order.ItemsInOrder.Contains(_item), "Item was not added to the order's items.");
    }*/

    [Test]
    public void RemoveOrderHavingItem_ShouldRemoveOrderFromItem()
    {
        _item.AddOrderHavingItem(_order, 1);
        _item.RemoveOrderHavingItem(_order);
        Assert.IsFalse(_item.OrdersHavingItems.Contains(_order), "Order was not removed from the item's orders.");
        Assert.IsFalse(_order.ItemsInOrder.Contains(_item), "Item was not removed from the order's items.");
    }

    [Test]
    public void OrderIsConnected_ShouldReturnTrueIfOrderIsConnected()
    {
        _item.AddOrderHavingItem(_order, 1);
        var isConnected = _item.OrderIsConnected(_order);
        Assert.IsTrue(isConnected, "OrderIsConnected returned false for a connected order.");
    }

    [Test]
    public void OrderIsConnected_ShouldReturnFalseIfOrderIsNotConnected()
    {
        
        var isConnected = _item.OrderIsConnected(_order);
        Assert.IsFalse(isConnected, "OrderIsConnected returned true for a disconnected order.");
    }
    
    //Client to Subscription connection

    [Test]
    public void AddSubscription_ShouldThrowExceptionIfClientAlreadyHaveSubscription()
    {
        _client.AddSubscription(_subscription);
        Assert.Throws<Exception>(() => _client.AddSubscription(_subscription), "Client has already a subscription, " +
                                                                               "you need to unsubscribe  first");
    }
    
    [Test]
    public void RemoveItemFromOrder_WhenNoOrdersLeft_ShouldNotRemove()
    {

        _order.AddItemToOrder(_item, 1);
        _item.RemoveOrderHavingItem(_order); // Remove the order to simulate edge case
        using var sw = new StringWriter();
        Console.SetOut(sw);
        _order.RemoveItemFromOrder(_item);
        Assert.AreEqual("There must be at least one item left\r\n", sw.ToString());
        Assert.IsTrue(_order.ItemsInOrder.Contains(_item),
            "Item was incorrectly removed when it should not have been.");
    }
    [Test]
    public void AssignTrackerToOrder_ShouldAssignOrder()
    {
        var order = new Order();
        var tracker = new Tracker(1, DateTime.Now.AddHours(1), order);
        tracker.AssignTrackerToOrder(order);
        Assert.AreEqual(order, tracker.AssignedOrder);
    }
}


