using System.Xml.Serialization;
using Microsoft.VisualBasic;

namespace Egift_main.Order;

public class Refund
{
    private static DateFormat _date;
    private static bool _isApproved = false;

   public void sendRefundRequest(User user, double amount)
    {
        CheckApproval();
        user.UserWallet._AddMoney(amount);
    }

    void CheckApproval()
    {
        //Information must to be taken from the server
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