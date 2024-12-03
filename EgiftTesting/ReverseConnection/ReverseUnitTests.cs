using Egift_main;
using Egift_main.Order;

namespace EgiftTesting.ReverseConnection;

public class ReverseUnitTests
{
    
    private Wallet _wallet;
    private Employee _employee;
    private Refund _refund;

    [SetUp]
    public void SetUp()
    {
        _wallet = new Wallet();
        _employee = new Employee(2, "987-654", "employee@example.com", _wallet, "Koszykowska", "Abdullah");
        _refund = new Refund();
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
}