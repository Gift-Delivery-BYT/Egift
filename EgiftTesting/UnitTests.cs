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

        private Subscription _subscription;
        private Subscription _subscriptionPremium;
        private Exporter _exporter;
        private Item _item;
        private Egift_main.Order.Order _order;
        private Tracker _tracker;


        [SetUp]
        public void Setup()
        {
            var trecker = new Tracker(101, DateTime.Now.AddDays(3),new Order());


            _wallet = new Wallet();
            _user = new User(1, "1234567890", "user@example.com");
            _employee = new Employee(2, "0987654321", "test@mail.com", _wallet, "9-5", "Garry");
            _refund = new Refund();
            _employee.Refund = _refund;
            _schedule = new Schedule();
            _client = new Client(1, "1234567890", "test@mail.com", new Wallet(), new DateFormat(), "Abdullah");
            _subscription = new Subscription(10.0,new List<Client>(),SubscriptionType.Standard);
            _subscriptionPremium = new Subscription(99.99, true, true);
            _exporter = new Exporter("Company Vinntsia", "USA", "123", 100.5f, 1234567890, DateTime.Now.AddDays(10),new List<Item>());
            _item = new Item(1, "TestItem", 100.0, DateFormat.GeneralDate,new Exporter());
            _order = new Order(); // Assuming an Order class exists
            _tracker = new Tracker(101, DateTime.Now.AddDays(3),new Order());
            typeof(Subscription)
                .GetField("_subscriptionStandarts", BindingFlags.Static | BindingFlags.NonPublic)
                ?.SetValue(null, new List<Subscription>());
        }

        [Test]
        public void ApproveRefund_ShouldReturnTrueAndEmployeeId()
        {
            double amount = 100.0;
            DateTime purchaseDate = DateTime.Now;
            var result = _employee.ApproveRefund(_client, amount, purchaseDate);

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

        /*[Test]
        public void AssignTrecker_True()
        {
            var order = new Order();
            var trecker = new Tracker(101, DateTime.Now.AddDays(3));

            order.AssignTrecker(trecker);
            Assert.IsTrue(order.IsTreckerAssigned());
        }
        */
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
        public void SpendMoney_CantSpendMoreThanOwned()
        {
            var wallet = new Wallet();
            wallet._AddMoney(100);
            var ex = Assert.Throws<InvalidOperationException>(() => wallet.SpendMoney(200));
            Assert.That(ex.Message,
                Is.EqualTo("Insufficient balance. Cannot spend more than the current wallet balance."));
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
            _user.Email = "user@gmail.com";
            Assert.AreEqual("user@gmail.com", _user.Email);
        }

        [Test]
        public void Email1_ThrowException_WhenSetToNull()
        {
            Assert.Throws<ArgumentNullException>(() => _user.Email = null);
        }

        [Test]
        public void User_Email1_ShouldThrowArgumentException_WhenEmailIsInvalid()
        {
            var user = new User(1, "1234567890", "test@example.com");

            var ex = Assert.Throws<ArgumentException>(() => user.Email = "invalidemail.com");
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
            var trecker = new Tracker(expectedId, DateTime.Now.AddHours(5),new Order());

            var actualId = trecker.TrackerID;

            Assert.AreEqual(expectedId, actualId);
        }

        [Test]
        public void Tracker_Location()
        {
            var tracker = new Tracker(1, DateTime.Now.AddHours(5), new Order())
            {
                Location = "Warsaw"
            };
            string actualLocation = tracker.GetLocation();
            Assert.AreEqual("Warsaw", actualLocation);
        }

        [Test]
        public void Tracker_EstimatedTimeForArrival()
        {
            var expectedTime = DateTime.Now.AddHours(5);
            var trecker = new Tracker(1, expectedTime,new Order());

            var actualTime = trecker.GetEstimatedTime();

            Assert.AreEqual(expectedTime, actualTime);
        }

        [Test]
        public void AddTreckerIsValidException()
        {
            var trecker = new Tracker(1, DateTime.Now.AddHours(-5),new Order());
            trecker.Location = null;

            Assert.Throws<ArgumentNullException>(() => trecker.AddTrecker(trecker));
        }

        [Test]
        public void AddTreckerReturnTrueIfTrackerIsValid()
        {
            var trecker = new Tracker(1, DateTime.Now.AddHours(5),new Order());
            trecker.Location = "Warsaw";

            Assert.IsTrue(trecker.AddTrecker(trecker));
        }

        [Test]
        public void UpdateCurrentLocation_NewLocation_UpdateLocation()
        {
            var trecker = new Tracker(1, DateTime.Now.AddHours(5),new Order()) { Location = "Warsaw" };

            trecker.UpdateCurrentLocation("Krakow");

            Assert.AreEqual("Krakow", trecker.GetLocation());
        }

        [Test]
        public void UpdateCurrentLocation_SameLocation_NotUpdateLocation()
        {
            var trecker = new Tracker(1, DateTime.Now.AddHours(5),new Order()) { Location = "Warsaw" };
            trecker.UpdateCurrentLocation("Warsaw");

            Assert.AreEqual("Warsaw", trecker.GetLocation());
        }

        /// <summary>
        /// Fix needed
        /// </summary>
        [Test]
        public void UpdateEstimationTime_NewTime_UpdateEstimatedTime()
        {
            var trecker = new Tracker(1, DateTime.Now.AddHours(5),new Order());
            var newTime = DateTime.Now.AddHours(6);

            trecker.UpdateEstimationTime(DateTime.Now.AddHours(6));

            Assert.AreEqual(newTime, trecker.GetEstimatedTime());
        }

        [Test]
        public void UpdateEstimationTime_SameTime_dNotUpdateEstimatedTime()
        {
            var trecker = new Tracker(1, DateTime.Now.AddHours(5),new Order());
            DateTime beforeUpdate = trecker.GetEstimatedTime();

            trecker.UpdateEstimationTime(beforeUpdate);

            Assert.That(trecker.GetEstimatedTime(),
                Is.EqualTo(beforeUpdate).Within(TimeSpan.FromMilliseconds(1)));
        }


        //Serialization UnitTests

        [Test]
        public void Serialize_ShouldCreateXmlFile()
        {
            Exporter.Serialize("./Exporter.xml");
            Assert.IsTrue(File.Exists("./Exporter.xml"), "Serialized file should be created.");
            Assert.IsNotEmpty(File.ReadAllText("./Exporter.xml"), "Serialized file should not be empty.");
        }

        [Test]
        public void Deserialize_FileNotFound_ShouldClearExporterList()
        {
            var res = Exporter.Deserialize("NonExistentFile.xml");
            Assert.IsFalse(res, "Deserialization should return false for a non-existent file.");
        }
/*
        [Test]
        public void AddNewExporter_ShouldThrowExceptionForInvalidExporter()
        {
            var invalidExporter = new Exporter();
            Assert.Throws<ArgumentException>(() => Exporter.addNewExporter(invalidExporter),
                "Adding an invalid exporter should throw an exception.");
        }
*/
        [Test]
        public void UserCantHavePhoneNumberLenghtLessThan4()
        {

            var user = new User(1, "1234567890", "test@mail.com");


            var ex = Assert.Throws<ArgumentException>(() => user.PhoneNumber1 = "123");
            Assert.AreEqual("Phone number must be at least 4 characters long.", ex.Message,
                "Exception message should match.");
        }


        [Test]
        public void CheduleCannotBeSetToPast()
        {
            var schedule = new Schedule();
            var pastDate = new List<DateTime> { DateTime.Now.AddMinutes(-10) };

            var ex = Assert.Throws<ArgumentException>(() => schedule.ScheduleDate = pastDate);
            Assert.AreEqual("Schedule date cannot be in the past.", ex.Message);
        }


        [Test]
        public void SerializeItem_ShouldCreateXmlFile()
        {
            Item.Serialize("./Item.xml");
            Assert.IsTrue(File.Exists("./Item.xml"), "Serialized file should be created.");
            Assert.IsNotEmpty(File.ReadAllText("./Item.xml"), "Serialized file should not be empty.");
        }

        [Test]
        public void TimeDeliveryCantBeChangedToPast()
        {
            var notification = new Notifications();
            var pastTime = DateTime.Now.AddMinutes(-5);


            var ex = Assert.Throws<ArgumentException>(() => notification.TimeDeliveryWasChanged(pastTime));
            Assert.AreEqual("Delivery time cannot be set to the past.", ex.Message);
        }

        /// <summary>
        /// ??? the heck it not working ???
        /// </summary>
        [Test] // have to run this one separately
        public void DeserializeItem_ShouldLoadDataFromXmlFile()
        {
            var item = new List<Item> { new Item(1, "TestItem", 10.0, DateFormat.GeneralDate, new Exporter()) };

            Item.Serialize("./Item.xml");
            var items = Item.GetItems();
            Assert.AreEqual(2, items.Count, "Item count should match after deserialization.");
        }

        [Test]
        public void SerializeOrder_ShouldCreateXmlFile()
        {
            var items = new List<Item> { new Item(1, "TestItem", 100.0, DateFormat.GeneralDate, new Exporter()) };
            var order = new Order(new User(),new Tracker(1,DateTime.Now,new Order()), 1, items, "TestLocation", "TestDescription",new List<Item>(),10.0);
            Order.Serialize("./Order.xml");
            Assert.IsTrue(File.Exists("./Order.xml"), "Serialized file should be created.");
            Assert.IsNotEmpty(File.ReadAllText("./Order.xml"), "Serialized file should not be empty.");
        }

        [Test]
        public void DeserializeOrder_ShouldLoadDataFromXmlFile()
        {
            const string filePath = "./Order.xml";


            var items = new List<Item> { new Item(1, "TestItem", 100.0, DateFormat.GeneralDate,new Exporter()) };
            var order = new Order(new User(),new Tracker(1,DateTime.Now,new Order()), 1, items, "TestLocation", "TestDescription",new List<Item>(),10.0);
            Order.Serialize(filePath);

            typeof(Order)
                .GetField("_orderList", BindingFlags.Static | BindingFlags.NonPublic)
                ?.SetValue(null, new List<Order>());

            Order.Deserialize(filePath);
            var orders = Order.GetAllOrders();
            Assert.AreEqual(1, orders.Count, "Order count should match after deserialization.");
        }

        [Test] //Fix
        public void SerializeSubscriptionPremium_ShouldCreateXmlFile()
        {
            var subs = new List<Subscription> { new Subscription(1, true, true) };

            Subscription.Serialize("./SubscriptionPremium.xml");
            Assert.IsTrue(File.Exists("./SubscriptionPremium.xml"), "Serialized file should be created.");
            Assert.IsNotEmpty(File.ReadAllText("./SubscriptionPremium.xml"), "Serialized file should not be empty.");
        }

        [Test] // have to run this one separately
        public void DeserializeSubscriptionPremium_ShouldLoadDataFromXmlFile()
        {
            Subscription.Serialize("./SubscriptionPremium.xml");


            typeof(Subscription)
                .GetField("_subscriptionPremiums", BindingFlags.Static | BindingFlags.NonPublic)
                ?.SetValue(null, new List<Subscription>());
            Subscription.Deserialize("./SubscriptionPremium.xml");


            var subscriptions = typeof(Subscription)
                .GetField("_subscriptionPremiums", BindingFlags.Static | BindingFlags.NonPublic)
                ?.GetValue(null) as List<Subscription>;
            Assert.AreEqual(1, subscriptions?.Count, "Subscription count should match after deserialization.");
        }

        /*[Test]
        public void SerializeSubscriptionStandard_ShouldCreateXmlFile()
        {
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
        }*/ 
        //Abstract class

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
            typeof(Employee)
                .GetField("_employeeList", BindingFlags.Static | BindingFlags.NonPublic)
                ?.SetValue(null, new List<Employee>());

            var employee = new Employee(1, "1234567890", "test@mail.com", new Wallet(), "123", "test");
            Employee.Serialize("./Employee.xml");

            Assert.IsTrue(File.Exists("./Employee.xml"), "Serialized file should be created.");
            Assert.IsNotEmpty(File.ReadAllText("./Employee.xml"), "Serialized file should have content.");
        }

        [Test]
        public void DeserializeEmployee_ShouldLoadDataFromXmlFile()
        {
            typeof(Employee)
                .GetField("_employeeList", BindingFlags.Static | BindingFlags.NonPublic)
                ?.SetValue(null, new List<Employee>());
            
            var employee = new Employee(1, "1234567890", "test@mail.com", new Wallet(), "123", "test");
            Employee.Serialize("./Employee.xml");
            typeof(Employee)
                .GetField("_employeeList", BindingFlags.Static | BindingFlags.NonPublic)
                ?.SetValue(null, new List<Employee>());
            
            Employee.Deserialize("./Employee.xml");

            var employees = typeof(Employee)
                .GetField("_employeeList", BindingFlags.Static | BindingFlags.NonPublic)
                ?.GetValue(null) as List<Employee>;

            Assert.AreEqual(1, employees?.Count, "Employee count should match after deserialization.");
            Assert.AreEqual("test", employees[0].Name, "Employee name should match.");
        }

        [Test]
        public void AddItemToOrder_ShouldAddItemWithCorrectQuantity()
        {
            var quantity = 2;
            _order.AddItemToOrder(_item, quantity);
            Assert.IsTrue(_order.ItemsInOrder.Contains(_item), "Item was not added to the order.");
            Assert.IsTrue(_item.OrdersHavingItems.Contains(_order), "Order was not associated with the item.");
        }
//
        [Test]
        public void RemoveItemFromOrder_ShouldRemoveItemCorrectly()
        {

            _order.AddItemToOrder(_item, 1);
            _order.RemoveItemFromOrder(_item);


            Assert.IsFalse(_order.ItemsInOrder.Contains(_item), "Item was not removed from the order.");
            Assert.IsFalse(_item.OrdersHavingItems.Contains(_order),
                "Order was not removed from the item's associations.");
        }



        [Test]
        public void ItemIsConnected_ShouldReturnTrueIfItemIsInOrder()
        {

            _order.AddItemToOrder(_item, 1);
            var isConnected = _order.ItemIsConnected(_item);

            // Assert
            Assert.IsTrue(isConnected, "ItemIsConnected returned false for an item in the order.");
        }

        [Test]
        public void ItemIsConnected_ShouldReturnFalseIfItemIsNotInOrder()
        {

            var isConnected = _order.ItemIsConnected(_item);
            Assert.IsFalse(isConnected, "ItemIsConnected returned true for an item not in the order.");
        }

    }
}
       
    

