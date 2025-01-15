using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using Egift_main.Models.Order;
using Egift_main.Order;
using Egift_main.Subscription;

namespace Egift_main;
[Serializable]
public class Employee : User,IEmployee
{
    private Refund _refund;
    private Schedule _schedule;
    private string name;
    private string address;
    [XmlIgnore]
    private Employee advisor;
    private List<Employee> _advisorOfEmployees;
    public List<Tracker> _trackers = new List<Tracker>();

    [XmlArray]
    private static List<Employee> _employeeList = new List<Employee>();

    public Employee() { }

    public Employee Advisor
    {
        get=> advisor;
        set => advisor = value ?? throw new ArgumentNullException(nameof(value));
    }

    public List<Employee> AdvisorOfEmployees
    {
        get => _advisorOfEmployees;
    }
    
    [XmlIgnore]
    public Refund Refund
    {
        get => _refund;
        set
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            _refund = value;
            _refund.Employee = this;
        }
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
        set
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (_schedule != null && _schedule.Owner == this)
            {
                _schedule.RemoveEmployee();
            }

            _schedule = value;
            _schedule.AssignEmployee(this);
        }
    }

    public void AddSchedule(Schedule schedule)
    {
        if (schedule == null)
            throw new ArgumentNullException(nameof(schedule));

        if (_schedule != null)
            _schedule.RemoveEmployee();

        _schedule = schedule;
        _schedule.AssignEmployee(this);
    }

    public void RemoveSchedule()
    {
        if (_schedule == null)
            return;

        _schedule.RemoveEmployee();
        _schedule = null;
    }

    
    public void AddTracker(Tracker tracker)
    {
        if (tracker == null)
            throw new ArgumentNullException(nameof(tracker), "Tracker cannot be null.");
   
        if (!_trackers.Contains(tracker))
        {
            _trackers.Add(tracker);
            tracker.AssignEmployee(this);
        }
    }
   
    public void RemoveTracker(Tracker tracker)
    {
        if (tracker == null)
            throw new ArgumentNullException(nameof(tracker), "Tracker cannot be null.");
   
        if (_trackers.Contains(tracker))
        {
            _trackers.Remove(tracker);
            tracker.RemoveEmployee(); 
        }
    }
   private void AddEmployeeToEdvice(Employee employee)
    {
        if (AdvisorOfEmployees.Count>10) throw new Exception("More than 10 employees already added");
        AdvisorOfEmployees.Add(employee);
        employee.Advisor= this;
    }
    private void RemoveAdvisorFromEmployee(Employee employee)
    {
        employee.Advisor = null;
        _advisorOfEmployees.Remove(employee);
    }
    

   public Tuple<bool, int> ApproveRefund(Client client,double amount, DateTime purchaseDate)
    {
        _refund.sendRefundRequest(client, amount, purchaseDate);
        Tuple<bool, int> Info = new Tuple<bool, int>(true,this.Id);
        return Info;
    }
    public void DeleteEmployee()
    {
        _schedule = null;

        _employeeList.Remove(this);

        Console.WriteLine($"Employee {name} and their schedule have been deleted.");
    }
    public static bool Serialize(string path = "./Users/Serialized/Employee.xml")
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<Employee>));
        using (StreamWriter writer = new StreamWriter(path))
        {
            serializer.Serialize(writer, _employeeList);
        }
        return true;
    }

    public static bool Deserialize(string path = "./Users/Serialized/Employee.xml")
    {
        StreamReader file;
        try {
            file = File.OpenText(path);
        }
        catch (FileNotFoundException) {
            _employeeList.Clear();
            return false;
        }
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Employee>));
        using (XmlTextReader reader = new XmlTextReader(file)) {
            try {
                _employeeList = (List<Employee>)xmlSerializer.Deserialize(reader);
            }
            catch (InvalidCastException) {
                _employeeList.Clear();
                return false;
            }
            return true;
        }
    }
       

    public Employee(int id, string phoneNumber, string email, 
        Wallet UserWallet, string address, string name) : base(id, phoneNumber, email)
    { 
        this.address = address; 
        this.name = name;
        
        _employeeList.Add(this);
    }
    
    private static bool IsValidEmployee(Employee employee)
    {
        if (employee != null &&
            !string.IsNullOrWhiteSpace(employee.Name) &&
            !string.IsNullOrWhiteSpace(employee.Address) &&
            employee.Refund != null)
        {
            return true;
        }
        throw new ArgumentNullException("invalid employee");
    }
}