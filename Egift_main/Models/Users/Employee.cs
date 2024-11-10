using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using Egift_main.Models.Order;
using Egift_main.Order;
using Egift_main.Subscription;

namespace Egift_main;
[Serializable]
public class Employee : User
{
    private Refund _refund;
    private Schedule _schedule;
    private string name;
    private string address;
    
    [XmlArray]
    private static List<Employee> _emoloyeeList = new List<Employee>();

    public Employee() { }
    
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
    private static bool Serialize(string path = "./Users/Serialized/Employee.xml")
    {
            
        XmlSerializer serializer = new XmlSerializer(typeof(Employee));
        using (StreamWriter writer = new StreamWriter(path)) {
            serializer.Serialize(writer, _emoloyeeList);
        }
        return true;
    }

    private static bool Deserialize(string path = "./Users/Serialized/Employee.xml")
    {
        StreamReader file;
        try {
            file = File.OpenText(path);
        }
        catch (FileNotFoundException) {
            _emoloyeeList.Clear();
            return false;
        }
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Employee>));
        using (XmlTextReader reader = new XmlTextReader(file)) {
            try {
                _emoloyeeList = (List<Employee>)xmlSerializer.Deserialize(reader);
            }
            catch (InvalidCastException) {
                _emoloyeeList.Clear();
                return false;
            }
            return true;
        }
    }
       

    public Employee(int id, string phoneNumber, string email, 
        Wallet UserWallet, string address, string name) : base(id, phoneNumber, email, UserWallet)
    { 
        this.address = address; 
        this.name = name;
        
        _emoloyeeList.Add(this);
    }
}