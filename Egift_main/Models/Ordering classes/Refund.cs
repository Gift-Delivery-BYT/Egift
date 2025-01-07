using System.Xml.Serialization;
using Microsoft.VisualBasic;

namespace Egift_main.Order;

public class Refund
{//
    private static DateFormat _date;
    private static bool _isApproved = false;
    private Employee _employee;
    public int RefundId { get; set; }
    public User User { get; set; }
    public Employee Employee
    {
        get => _employee;
        set => _employee = value ?? throw new ArgumentNullException(nameof(value));
    }
    
   public void sendRefundRequest(Client client, double amount, DateTime purchaseDate)
    {
        CheckApproval(purchaseDate);
        if(!_isApproved)
            client._Wallet._AddMoney(amount);
        else
            Console.WriteLine("Refund not approved");
    }

    public bool CheckApproval(DateTime purchaseDate)
    {
        DateTime currentDate = DateTime.Now;
        return (currentDate - purchaseDate).TotalDays <= 30;
    }

    public static DateFormat Date
    {
        get => _date;
        set => _date = value;
    }

    public static bool IsApproved
    {
        get => _isApproved;
        set => _isApproved = value;
    }
    
    public static void saveDate(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(DateFormat));
        using (StreamWriter writer = new StreamWriter(path))
        {
            serializer.Serialize(writer, path);
        }
    }
    
    public static void readDate(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(DateFormat));
        using (StreamReader reader = new StreamReader(path))
        {
            DateFormat element = (DateFormat)serializer.Deserialize(reader);
        }
    }
    public static void saveIsApproved(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(bool));
        using (StreamWriter writer = new StreamWriter(path))
        {
            serializer.Serialize(writer, path);
        }
    }
    
    public static bool readIsApproved(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(bool));
        using (StreamReader reader = new StreamReader(path))
        {
            bool element = (bool)serializer.Deserialize(reader);
            return element;
        }
    }
}