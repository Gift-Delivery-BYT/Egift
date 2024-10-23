using NUnit.Framework;
using Egift_main;
using Egift_main.Order;
using System;

namespace EgiftTesting
{
    public class Tests
    {
        private Employee _employee;
        private User _user;
        private Wallet _wallet;
        private Refund _refund;

        [SetUp]
        public void Setup()
        {
            _wallet = new Wallet();
            _user = new User(1, "1234567890", "user@example.com", _wallet);
            _employee = new Employee(2, "0987654321", "employee@example.com", "9-5", _wallet);
            _refund = new Refund();
            _employee.Refund = _refund;
        }

        [Test]
        public void ApproveRefund_ShouldReturnTrueAndEmployeeId()
        {
            double amount = 100.0;
            var result = _employee.ApproveRefund(_user, amount);
            
            Assert.AreEqual(true, result.Item1);
            Assert.AreEqual(2, result.Item2); // Assuming employee's ID is 2
        }

        [Test]
        public void AddMoney_ShouldIncreaseWalletAmount()
        {
            
            double initialAmount = 50.0;
            double toAdd = 25.0;
            _wallet._AddMoney(initialAmount);
            
            _wallet._AddMoney(toAdd);
            Assert.AreEqual(75.0, _wallet.GetAmount()); 
        }

        [Test]
        public void RefundThrowExceptionWhenNull()
        {
            Assert.Throws<ArgumentNullException>(() => _employee.Refund = null);
        }

        [Test]
        public void AssignTrecker_True()
        {
            var order = new Order();
            var trecker = new Trecker();
            
            order.assignTrecker(trecker);
            Assert.IsTrue(order.IsTreckerAssigned()); // Assuming there's an IsTreckerAssigned method
        }
        //
    }
}
