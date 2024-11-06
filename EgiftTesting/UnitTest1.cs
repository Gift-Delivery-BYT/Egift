using NUnit.Framework;
using Egift_main;
using Egift_main.Order;
using System;
using Egift_main.Models.Order;

namespace EgiftTesting
{
    public class Tests
    {
        private Employee _employee;
        private User _user;
        private Wallet _wallet;
        private Refund _refund;
        private Schedule _schedule;
        [SetUp]
        public void Setup()
        {
            _wallet = new Wallet();
            _user = new User(1, "1234567890", "user@example.com", _wallet);
            _employee = new Employee(2, "0987654321", "employee@example.com", _wallet, "9-5", "Garry");
            _refund = new Refund();
            _employee.Refund = _refund;
            _schedule = new Schedule();
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
        public void AddMoney_IncreasesWalletAmount()
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
            
            order.AssignTrecker(trecker);
            Assert.IsTrue(order.IsTreckerAssigned()); // Assuming there's an IsTreckerAssigned method
        }
        //
        
        [Test]
        public void Email1_ThrowException_WhenSetToNull()
        {
            Assert.Throws<ArgumentNullException>(() => _user.Email1 = null);
        }
        [Test]
        public void UserWallet_ShouldThrowException_WhenSetToNull()
        {
            Assert.Throws<ArgumentNullException>(() => _user.UserWallet = null);
        }
        [Test]
        public void Wallet_InitialBalance_ShouldBeZero()
        {
            var newWallet = new Wallet();
            Assert.AreEqual(0.0, newWallet.GetAmount());
        }
        
        [Test]
        public void AddWorkHours_WorkHourAlreadyExists()
        {
            DateTime workDate = new DateTime(2024, 12, 1);
            
            _schedule.AddWorkHours(workDate);
            _schedule.AddWorkHours(workDate);
    
            Assert.AreEqual(1, _schedule.ScheduleDate.Count);
        }
        
        [Test]
        public void AddHolidays_HolidayIsAdded()
        {
            DateTime holiday = new DateTime(2024, 12, 25);
    
            _schedule.AddHolidays(holiday);
    
            Assert.Contains(holiday, _schedule.Holidays);
        }
        
        [Test]
        public void AddHolidays_HolidayDoesntDuplicate()
        {
            DateTime holiday = new DateTime(2024, 12, 25);
    
            _schedule.AddHolidays(holiday);
            _schedule.AddHolidays(holiday);
    
            Assert.AreEqual(1, _schedule.Holidays.Count);
        }

    }
}
