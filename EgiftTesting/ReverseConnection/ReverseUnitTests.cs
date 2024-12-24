using System.Runtime.Serialization;
using Egift_main;
using Egift_main.Order;
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
    
    //composition
    [Test]
    public void WalletDeletedWhenClientDeleted()
    {
        _client.ClientWallet = _wallet;
        _client.DeleteClient();
        Assert.IsNull(_client.ClientWallet);
    }
    [Test]
    public void ScheduleDeletedWhenEmployeeDeleted()
    {
        var schedule = _employee.Schedule; 
        _employee.DeleteEmployee();
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
    [Test]
    public void AddOrderHavingItem_ShouldAddOrderWithQuantity()
    {
        var quantity = 2;
        _item.AddOrderHavingItem(_order, quantity);
        Assert.IsTrue(_item.OrdersHavingItems.Contains(_order), "Order was not added to the item's orders.");
        Assert.IsTrue(_order.ItemsInOrder.Contains(_item), "Item was not added to the order's items.");
    }

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
}