using NUnit.Framework;
using Egift_main;
using Egift_main.Order;
using System;
using Egift_main.Models.Order;
using Egift_main.Subscription;
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
        private SubscriptionStandart _subscription;
        private SubscriptionPremium _subscriptionPremium;

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
            _subscription = new SubscriptionStandart();
            _subscriptionPremium = new SubscriptionPremium(99.99, true, true);

        }

        [Test]
        public void ApproveRefund_ShouldReturnTrueAndEmployeeId()
        {
            double amount = 100.0;
            DateTime purchaseDate = DateTime.Now;
            var result = _employee.ApproveRefund(_user, amount, purchaseDate);
            
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
        
        // Unit tests for Standart Subscription attributes
        [Test]
        public void SubscriptionStandartAttribute_AvailableDates()
        {
            var dates = new List<DateTime> { DateTime.Now, DateTime.Now.AddDays(1) };
            _subscription.AvailableDates = dates;
            Assert.AreEqual(dates, _subscription.AvailableDates);
        }
        
        // Unit tests for Tracker attributes
        [Test]
        public void Tracker_ID()
        {
            var expectedId = 1;
            var trecker = new Trecker(expectedId, DateTime.Now.AddHours(5));

            var actualId = trecker.TrackerID;

            Assert.AreEqual(expectedId, actualId);
        }

        [Test]
        public void Tracker_Location()
        {
            Assert.AreEqual("Warsaw", 
                new Trecker(1, DateTime.Now.AddHours(5)) { Location = "Warsaw" }.GetLocation());
        }
        
        [Test]
        public void Tracker_EstimatedTimeForArrival()
        {
            var expectedTime = DateTime.Now.AddHours(5);
            var trecker = new Trecker(1, expectedTime);
            
            var actualTime = trecker.GetEstimatedTime();
            
            Assert.AreEqual(expectedTime, actualTime);
        }
        
        [Test]
        public void AddTreckerIsValidException()
        {
            var trecker = new Trecker(1, DateTime.Now.AddHours(-5));
            trecker.Location = null; 

            Assert.Throws<ArgumentNullException>(() => trecker.AddTrecker(trecker));
        }

        [Test]
        public void AddTreckerReturnTrueIfTrackerIsValid()
        {
            var trecker = new Trecker(1, DateTime.Now.AddHours(5));
            trecker.Location = "Warsaw";

            Assert.IsTrue(trecker.AddTrecker(trecker));
        }
        
        [Test]
        public void UpdateCurrentLocation_NewLocation_UpdateLocation()
        {
            var trecker = new Trecker(1, DateTime.Now.AddHours(5)) { Location = "Warsaw" };

            trecker.UpdateCurrentLocation("Krakow");

            Assert.AreEqual("Krakow", trecker.GetLocation());
        }
        
        [Test]
        public void UpdateCurrentLocation_SameLocation_NotUpdateLocation()
        {
            var trecker = new Trecker(1, DateTime.Now.AddHours(5)) { Location = "Warsaw" };
            trecker.UpdateCurrentLocation("Warsaw");

            Assert.AreEqual("Warsaw", trecker.GetLocation());
        }
        
        [Test]
        public void UpdateEstimationTime_NewTime_UpdateEstimatedTime()
        {
            var trecker = new Trecker(1, DateTime.Now.AddHours(5));
            var newTime = DateTime.Now.AddHours(6);

            trecker.UpdateEstimationTime(DateTime.Now.AddHours(6));

            Assert.AreEqual(newTime, trecker.GetEstimatedTime());
        }
        
        [Test]
        public void UpdateEstimationTime_SameTime_dNotUpdateEstimatedTime()
        {
            var trecker = new Trecker(1, DateTime.Now.AddHours(5));
            DateTime beforeUpdate = trecker.GetEstimatedTime();
    
            trecker.UpdateEstimationTime(beforeUpdate);
    
            Assert.That(trecker.GetEstimatedTime(), 
                Is.EqualTo(beforeUpdate).Within(TimeSpan.FromMilliseconds(1)));
        }
       
    }
}
