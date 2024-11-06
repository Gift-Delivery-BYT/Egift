using System.Collections;
using System.Xml.Serialization;
using Egift_main.Models.Order;
using Egift_main.Order;

namespace Egift_main;
[Serializable]
public class Employee : User
{
    private Refund _refund;
    private Schedule _schedule;
    private string name;
    private string address;
    
    public Refund Refund
    {
        get => _refund;
        set => _refund = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Name
    {
        get => name;
        set => name = value;
    }
    public string Address
    {
        get => address;
        set => address = value;
    }
    public Schedule Schedule
    {
        get => _schedule;
        set => _schedule = value ?? throw new ArgumentNullException(nameof(value));
    }

   public Tuple<bool, int> ApproveRefund(User user,double amount)
    {
        _refund.sendRefundRequest(user, amount);
        Tuple<bool, int> Info = new Tuple<bool, int>(true,this.Id);
        return Info;
    }
    
    public void SaveToFile(string path = "./Users/Serialized/Employee.xml")
        {
            XmlSerializer serializer = new XmlSerializer(typeof(EmployeeInfo));
            using (StreamWriter writer = new StreamWriter(path))
            {
                var data = new EmployeeInfo
                {
                    Id = this.Id,
                    PhoneNumber1 = this.PhoneNumber1,
                    Email1 = this.Email1,
                    Schedule = this.Schedule,
                    Refund = this.Refund,
                    Name = this.Name,
                    Address = this.Address
                };
                serializer.Serialize(writer, data);
            }
        }
        
        public bool LoadFromFile(string path = "./Users/Serialized/Employee.xml")
        {
            if (!File.Exists(path))
            {
                Id = 0;
                PhoneNumber1 = string.Empty;
                Email1 = string.Empty;
                Refund = null;
                Schedule = null;
                Address = string.Empty;
                Name = string.Empty;
                return false;
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(EmployeeInfo));
                using (StreamReader reader = new StreamReader(path))
                {
                    var data = (EmployeeInfo)serializer.Deserialize(reader);
                    this.Id = data.Id;
                    this.PhoneNumber1 = data.PhoneNumber1;
                    this.Email1 = data.Email1;
                    this.Schedule = data.Schedule;
                    this.Refund = data.Refund;
                    this.Name = data.Name;
                    this.address = data.Address;
                }
                return true;
            }
            catch
            {
                Id = 0;
                PhoneNumber1 = string.Empty;
                Email1 = string.Empty;
                Refund = null;
                Schedule = null;
                Address = string.Empty;
                Name = string.Empty;
                return false;
            }
        }
        [Serializable]
        public class EmployeeInfo
        {
            public int Id { get; set; }
            public string PhoneNumber1 { get; set; }
            public string Email1 { get; set; }
            public Refund Refund  { get; set; }
            public Schedule Schedule { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
        }

        public Employee(int id, string phoneNumber, string email, Wallet UserWallet,
            string address, string name) : base(id, phoneNumber, email, UserWallet)
        {
            this.address = address;
            this.name = name;
        }
}