using NUnit.Framework;
using Egift_main;
using Egift_main.Order;
using System;
using Egift_main.Models.Order;
using Microsoft.VisualBasic;

namespace EgiftTesting
{
    public class Tests
    {
        private Employee _employee;
        private User _user;
        private Wallet _wallet;
        private Refund _refund;
        private Schedule _schedule;
        private Client _client;
        private BusinessAcount _businessAcount;
        private Foundation_Account _foundationAccount;

        [SetUp]
        public void Setup()
        {
            var trecker = new Trecker(101, DateTime.Now.AddDays(3));

            
            _wallet = new Wallet();
            _user = new User(1, "1234567890", "user@example.com", _wallet);
            _employee = new Employee(2, "0987654321", "employee@example.com", _wallet, "9-5", "Garry");
            _refund = new Refund();
            _employee.Refund = _refund;
            _schedule = new Schedule();
            _client = new Client(1, "1234567890", "client@example.com", new Wallet(), new DateFormat(), "Abdullah");
            _businessAcount = new BusinessAcount(99, "1234567890", "business@example.com", 
                new Wallet(), "China Shirts", "123 Business St", 0.15, trecker);
            _foundationAccount = new Foundation_Account(1, "1234567890", "foundation@example.com", new Wallet(), "Foundation");

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
            var trecker = new Trecker(101, DateTime.Now.AddDays(3));
            
            order.AssignTrecker(trecker);
            Assert.IsTrue(order.IsTreckerAssigned()); // Assuming there's an IsTreckerAssigned method
        }
        //
        
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
        
        [Test]
        public void Wishlist_ShouldThrowArgumentNullException_WhenSetToNull()
        {
            Assert.Throws<ArgumentNullException>(() => _client.WishList = null);
        }
        
        [Test]
        public void Documentation_ThrowsArgumentNullException_WhenNull()
        {
            Assert.Throws<ArgumentNullException>(() => _businessAcount.Documentation = null);
        }
        
        [Test]
        public void SpendMoney_CantSpendMoreThanOwned()
        {
            var wallet = new Wallet();
            wallet._AddMoney(100); 
            var ex = Assert.Throws<InvalidOperationException>(() => wallet.SpendMoney(200));
            Assert.That(ex.Message, Is.EqualTo("Insufficient balance. Cannot spend more than the current wallet balance."));
        }
        // Unit tests for User attributes
        [Test]
        public void UserAttribute_ID()
        {
            _user.Id = 10;
            Assert.AreEqual(10, _user.Id);
        }
        
        [Test]
        public void UserAttribute_PhoneNumber()
        {
            _user.PhoneNumber1 = "4813345456";
            Assert.AreEqual("4813345456", _user.PhoneNumber1);
        }
        
        [Test]
        public void UserAttribute_Email()
        {
            _user.Email1 = "user@gmail.com";
            Assert.AreEqual("user@gmail.com", _user.Email1);
        }
        
        [Test]
        public void Email1_ThrowException_WhenSetToNull()
        {
            Assert.Throws<ArgumentNullException>(() => _user.Email1 = null);
        }
        
        [Test]
        public void UserWallet_ThrowException_WhenSetToNull()
        {
            Assert.Throws<ArgumentNullException>(() => _user.UserWallet = null);
        }
        // Unit tests for Client attributes
        [Test]
        public void ClientAttribute_Name()
        {
            _client.Name = "John Doe";
            Assert.AreEqual("John Doe", _client.Name);
        }
        [Test]
        public void ClientAttribute_WishList_AddItem()
        {
            _client.WishList.Add("New Item");
            Assert.Contains("New Item", _client.WishList);
        }

        [Test]
        public void ClientAttribute_WishList_RemoveItem()
        {
            _client.WishList.Add("Gift");
            _client.WishList.Remove("Gift");
            Assert.IsFalse(_client.WishList.Contains("Gift"));
        }

        [Test]
        public void WishList_ThrowException_WhenSetToNull()
        {
            Assert.Throws<ArgumentNullException>(() => _client.WishList = null);
        }
        
        // Unit tests for Foundation attributes
        [Test]
        public void FoundationAccountAttribute_FoundationName()
        {
            _foundationAccount.FoundationName = "Gift Foundation";
            Assert.AreEqual("Gift Foundation", _foundationAccount.FoundationName);
        }
        
        [Test]
        public void FoundationAccountAttribute_AccountingInfo()
        {
            string accountingInfo = "accountingInfo";
            _foundationAccount.AddAccountingInfo(accountingInfo);
            Assert.Contains(accountingInfo, _foundationAccount.AccountingInfo);
        }

        [Test]
        public void AccountingInfo_ShouldThrowException_WhenNull()
        {
            Assert.Throws<ArgumentNullException>(() => _foundationAccount.AccountingInfo = null);
        }

        [Test]
        public void AddAccountingInfo_ShouldAddNewInfo()
        {
            string newInfo = "New Info";
            _foundationAccount.AddAccountingInfo(newInfo);
            Assert.Contains(newInfo, _foundationAccount.AccountingInfo);
        }
        
    }
}
