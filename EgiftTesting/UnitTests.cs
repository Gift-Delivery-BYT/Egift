using NUnit.Framework;
using Egift_main;
using Egift_main.Order;
using System;
using System.Reflection;
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
        private SubscriptionStandard _subscription;
        private SubscriptionPremium _subscriptionPremium;
        private Exporter _exporter;
        private Item _item;

        [SetUp]
        public void Setup()
        {
            var trecker = new Trecker(101, DateTime.Now.AddDays(3));

            
            _wallet = new Wallet();
            _user = new User(1, "1234567890", "user@example.com", _wallet);
            _employee = new Employee(2, "0987654321", "test@mail.com", _wallet, "9-5", "Garry");
            _refund = new Refund();
            _employee.Refund = _refund;
            _schedule = new Schedule();
            _client = new Client(1, "1234567890", "test@mail.com", new Wallet(), new DateFormat(), "Abdullah");
            _businessAcount = new BusinessAcount(99, "1234567890", "business@example.com", 
                new Wallet(), "China Shirts", "123 Business St", 0.15, trecker);
            _foundationAccount = new Foundation_Account(1, "1234567890", "test@mail.com", new Wallet(), "Foundation");
            _subscription = new SubscriptionStandard();
            _subscriptionPremium = new SubscriptionPremium(99.99, true, true);
            _exporter = new Exporter("Company Vinntsia", "USA", "123", 100.5f, 1234567890, DateTime.Now.AddDays(10));
            _item = new Item(1, "TestItem", 100.0, DateFormat.GeneralDate);
            
            typeof(SubscriptionStandard)
                .GetField("_subscriptionStandarts", BindingFlags.Static | BindingFlags.NonPublic)
                ?.SetValue(null, new List<SubscriptionStandard>());
        }

        [Test]
        public void ApproveRefund_ShouldReturnTrueAndEmployeeId()
        {
            double amount = 100.0;
            DateTime purchaseDate = DateTime.Now;
            var result = _employee.ApproveRefund(_user, amount, purchaseDate);
            
            Assert.AreEqual(true, result.Item1);
            Assert.AreEqual(2, result.Item2); 
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
            Assert.IsTrue(order.IsTreckerAssigned()); 
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
        public void DiscountCantBeMoreThan99()
        {
            var trecker = new Trecker(101, DateTime.Now.AddDays(3));

            var businessAccount = new BusinessAcount(1, "123-456-789", "business@example.com", new Wallet(), "My Business", "123 Business St", 0.03, trecker);

            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => businessAccount.Corparate_Discount = 1.1); 
            Assert.That(exception.ParamName, Is.EqualTo("value")); 
            Assert.That(exception.Message, Contains.Substring("The discount cannot exceed 99%")); 
        }
        
        [Test]
        public void SpendMoney_CantSpendMoreThanOwned()
        {
            var wallet = new Wallet();
            wallet._AddMoney(100); 
            var ex = Assert.Throws<InvalidOperationException>(() => wallet.SpendMoney(200));
            Assert.That(ex.Message, Is.EqualTo("Insufficient balance. Cannot spend more than the current wallet balance."));
        }
      
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
        
        [Test]
        public void User_Email1_ShouldThrowArgumentException_WhenEmailIsInvalid()
        {
            var user = new User(1, "1234567890", "test@example.com", new Wallet());

            var ex = Assert.Throws<ArgumentException>(() => user.Email1 = "invalidemail.com");
            Assert.That(ex.Message, Is.EqualTo("Email must contain '@'."));
        }
        
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
        
        [Test]
        public void SubscriptionStandartAttribute_AvailableDates()
        {
            var dates = new List<DateTime> { DateTime.Now, DateTime.Now.AddDays(1) };
            _subscription.AvailableDates = dates;
            Assert.AreEqual(dates, _subscription.AvailableDates);
        }
        
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
        /// <summary>
        /// Fix needed
        /// </summary>
        [Test]
        public void UpdateEstimationTime_NewTime_UpdateEstimatedTime()
        {
            var trecker = new Trecker(1, DateTime.Now.AddHours(5));
            var newTime = DateTime.Now.AddHours(6);

            trecker.UpdateEstimationTime(DateTime.Now.AddHours(6));

            Assert.AreEqual(newTime, trecker.GetEstimatedTime());
        }
        
        [Test]
        public void UpdateEstimationTime_SameTime_dNotUpdateEstimatedTime() {
            var trecker = new Trecker(1, DateTime.Now.AddHours(5));
            DateTime beforeUpdate = trecker.GetEstimatedTime();
    
            trecker.UpdateEstimationTime(beforeUpdate);
    
            Assert.That(trecker.GetEstimatedTime(), 
                Is.EqualTo(beforeUpdate).Within(TimeSpan.FromMilliseconds(1)));
        }
        
        
        //Serialization UnitTests
        
        [Test]
        public void Serialize_ShouldCreateXmlFile() {
            Exporter.Serialize("./Exporter.xml");
            Assert.IsTrue(File.Exists("./Exporter.xml"), "Serialized file should be created.");
            Assert.IsNotEmpty(File.ReadAllText("./Exporter.xml"), "Serialized file should not be empty.");
        }
        [Test]
        public void Deserialize_FileNotFound_ShouldClearExporterList() {
            var res = Exporter.Deserialize("NonExistentFile.xml");
            Assert.IsFalse(res, "Deserialization should return false for a non-existent file.");
        }

        [Test]
        public void AddNewExporter_ShouldThrowExceptionForInvalidExporter() {
            var invalidExporter = new Exporter();
            Assert.Throws<ArgumentException>(() => Exporter.addNewExporter(invalidExporter), 
                "Adding an invalid exporter should throw an exception.");
        }
        
        [Test]
        public void SerializeItem_ShouldCreateXmlFile()
        {
            Item.Serialize("./Item.xml");
            Assert.IsTrue(File.Exists("./Item.xml"), "Serialized file should be created.");
            Assert.IsNotEmpty(File.ReadAllText("./Item.xml"), "Serialized file should not be empty.");
        }
/// <summary>
/// ??? the heck it not working ???
/// </summary>
        [Test]
        public void DeserializeItem_ShouldLoadDataFromXmlFile()
        {
            var item = new List<Item> { new Item(1, "TestItem", 10.0, DateFormat.GeneralDate) };
            
            Item.Serialize("./Item.xml");
            var items = Item.GetItems();
            Assert.AreEqual(2, items.Count, "Item count should match after deserialization.");
        }
        
        [Test]
        public void SerializeOrder_ShouldCreateXmlFile()
        {
            var items = new List<Item> { new Item(1, "TestItem", 100.0, DateFormat.GeneralDate) };
            var order = new Order(false, 1, items, "TestLocation", "TestDescription", null);
            Order.Serialize("./Order.xml");
            Assert.IsTrue(File.Exists("./Order.xml"), "Serialized file should be created.");
            Assert.IsNotEmpty(File.ReadAllText("./Order.xml"), "Serialized file should not be empty.");
        }

        [Test]
        public void DeserializeOrder_ShouldLoadDataFromXmlFile()
        {
            const string filePath = "./Order.xml";

            
            var items = new List<Item> { new Item(1, "TestItem", 100.0, DateFormat.GeneralDate) };
            var order = new Order(false, 1, items, "TestLocation", "TestDescription", null);
            Order.Serialize(filePath);
            
            typeof(Order)
                .GetField("_orderList", BindingFlags.Static | BindingFlags.NonPublic)
                ?.SetValue(null, new List<Order>());
            
            Order.Deserialize(filePath);
            var orders = Order.GetAllOrders();
            Assert.AreEqual(1, orders.Count, "Order count should match after deserialization.");
        }
        [Test]
        public void SerializeSubscriptionPremium_ShouldCreateXmlFile()
        {
            var subs = new List<SubscriptionPremium> { new SubscriptionPremium(1, true, true) };
            
            SubscriptionPremium.Serialize("./SubscriptionPremium.xml");
            Assert.IsTrue(File.Exists("./SubscriptionPremium.xml"), "Serialized file should be created.");
            Assert.IsNotEmpty(File.ReadAllText("./SubscriptionPremium.xml"), "Serialized file should not be empty.");
        }

        [Test]
        public void DeserializeSubscriptionPremium_ShouldLoadDataFromXmlFile() { 
            SubscriptionPremium.Serialize("./SubscriptionPremium.xml");

        
            typeof(SubscriptionPremium)
                .GetField("_subscriptionPremiums", BindingFlags.Static | BindingFlags.NonPublic)
                ?.SetValue(null, new List<SubscriptionPremium>()); 
            SubscriptionPremium.Deserialize("./SubscriptionPremium.xml");

           
            var subscriptions = typeof(SubscriptionPremium)
                .GetField("_subscriptionPremiums", BindingFlags.Static | BindingFlags.NonPublic)
                ?.GetValue(null) as List<SubscriptionPremium>;
            Assert.AreEqual(1, subscriptions?.Count, "Subscription count should match after deserialization.");
        }
        [Test]
        public void SerializeSubscriptionStandard_ShouldCreateXmlFile() {
            SubscriptionStandard.Serialize("./SubscriptionStandard.xml");
            
            Assert.IsTrue(File.Exists("./SubscriptionStandard.xml"), "Serialized file should be created.");
            Assert.IsNotEmpty(File.ReadAllText("./SubscriptionStandard.xml"), "Serialized file should not be empty.");
        }

        [Test]
        public void DeserializeSubscriptionStandard_ShouldLoadDataFromXmlFile()
        {
            var subs = new List<SubscriptionStandard> { new SubscriptionStandard() }; 
            SubscriptionStandard.Serialize("./SubscriptionStandart.xml");
        
            typeof(SubscriptionStandard)
                .GetField("_subscriptionStandarts", BindingFlags.Static | BindingFlags.NonPublic)
                ?.SetValue(null, new List<SubscriptionStandard>()); 
            SubscriptionStandard.Deserialize("./SubscriptionStandart.xml");
            var subscriptions = typeof(SubscriptionStandard)
                .GetField("_subscriptionStandarts", BindingFlags.Static | BindingFlags.NonPublic)
                ?.GetValue(null) as List<SubscriptionStandard>;
            Assert.AreEqual(1, subscriptions?.Count, "Subscription count should match after deserialization.");
        }
        
        [Test]
        public void SerializeBusinessAccount_ShouldCreateXmlFile()
        {
            var account = new BusinessAcount(1, "1234567890", "test@mail.com", new Wallet(), "Test Business", "123 Test St", 0.03, new Trecker(1,DateTime.Now));
            BusinessAcount.Serialize("./BusinessAcc.xml");

            Assert.IsTrue(File.Exists("./BusinessAcc.xml"), "Serialized file should be created.");
            Assert.IsNotEmpty(File.ReadAllText("./BusinessAcc.xml"), "Serialized file should not be empty.");
        }

        [Test]
        public void DeserializeBusinessAccount_ShouldLoadDataFromXmlFile()
        {
            var account = new BusinessAcount(1, "1234567890", "test@mail.com", new Wallet(), "Test Business", "123 Test St", 0.03, new Trecker(1,DateTime.Now));
            BusinessAcount.Serialize("./BusinessAcc.xml");

            // Clear the static list and deserialize
            typeof(BusinessAcount)
                .GetField("_businessaccountList", BindingFlags.Static | BindingFlags.NonPublic)
                ?.SetValue(null, new List<BusinessAcount>());
            BusinessAcount.Deserialize("./BusinessAcc.xml");

            var accounts = typeof(BusinessAcount)
                .GetField("_businessaccountList", BindingFlags.Static | BindingFlags.NonPublic)
                ?.GetValue(null) as List<BusinessAcount>;

            Assert.AreEqual(2, accounts?.Count, "Business account count should match after deserialization.");
            Assert.AreEqual("Test Business", accounts[1].BusinessName, "Business name should match.");
        }
        
        [Test]
        public void SerializeClient_ShouldCreateXmlFile()
        {
          
            Client.Serialize("./Client.xml");

            Assert.IsTrue(File.Exists("./Client.xml"), "serialized file should be  created");
            Assert.IsNotEmpty(File.ReadAllText("./Client.xml"), "serialized file should not be empty");
        }

        [Test]
        public void DeserializeClient_ShouldLoadDataFromXmlFile()
        {
            typeof(Client)
                .GetField("_clientList", BindingFlags.Static | BindingFlags.NonPublic)
                ?.SetValue(null, new List<Client>());

            var client = new Client(1, "1234567890", "test@mail.com", new Wallet(), new DateFormat(), "test");
            Client.Serialize("./Client.xml");
    
            Client.Deserialize("./Client.xml");

            var clients = typeof(Client)
                .GetField("_clientList", BindingFlags.Static | BindingFlags.NonPublic)
                ?.GetValue(null) as List<Client>;

            Assert.AreEqual(1, clients?.Count, "Client count should match after deserialization.");
            Assert.AreEqual("test", clients?[0].Name, "Client name should match.");
        }

        [Test]
        public void SerializeEmployee_ShouldCreateXmlFile()
        {
            var employee = new Employee(1, "1234567890", "test@mail.com", new Wallet(), "123", "test");
            Employee.Serialize("./Employee.xml");

            Assert.IsTrue(File.Exists("./Employee.xml"), "serialized file should be created");
            Assert.IsNotEmpty(File.ReadAllText("./Employee.xml"), "serialized file should have smth");
        }

        [Test]
        public void DeserializeEmployee_ShouldLoadDataFromXmlFile()
        {
            var employee = new Employee(1, "1234567890", "test@mail.com", new Wallet(), "123", "test");
            Employee.Serialize("./Employee.xml");

            typeof(Employee)
                .GetField("_emoloyeeList", BindingFlags.Static | BindingFlags.NonPublic)
                ?.SetValue(null, new List<Employee>());
            Employee.Deserialize("./Employee.xml");

            var employees = typeof(Employee)
                .GetField("_emoloyeeList", BindingFlags.Static | BindingFlags.NonPublic)
                ?.GetValue(null) as List<Employee>;

            Assert.AreEqual(1, employees?.Count, "Employee count should match after deserialization.");
            Assert.AreEqual("Test Employee", employees[0].Name, "Employee name should match.");
        }
        [Test]
        public void SerializeFoundationAccount_ShouldCreateXmlFile()
        {
            var foundationAccount = new Foundation_Account(1, "1234567890", "test@mail.com", new Wallet(), "test");
            Foundation_Account.Serialize("./FoundAcc.xml");

            Assert.IsTrue(File.Exists("./FoundAcc.xml"), "Serialized file should be created.");
            Assert.IsNotEmpty(File.ReadAllText("./FoundAcc.xml"), "Serialized file should not be empty.");
        }

        [Test]
        public void DeserializeFoundationAccount_ShouldLoadDataFromXmlFile()
        {
            var foundationAccount = new Foundation_Account(1, "1234567890", "test@mail.com", new Wallet(), "test");
            Foundation_Account.Serialize("./FoundAcc.xml");

         
            typeof(Foundation_Account)
                .GetField("_foundationAccountList", BindingFlags.Static | BindingFlags.NonPublic)
                ?.SetValue(null, new List<Foundation_Account>());
            Foundation_Account.Deserialize("./FoundAcc.xml");

            var foundations = typeof(Foundation_Account)
                .GetField("_foundationAccountList", BindingFlags.Static | BindingFlags.NonPublic)
                ?.GetValue(null) as List<Foundation_Account>;

            Assert.AreEqual(1, foundations?.Count, "fund_acc  no match");
            Assert.AreEqual("Test Foundation", foundations[2].FoundationName, "foundation name should match");
        }
    }
    
}
       
    

