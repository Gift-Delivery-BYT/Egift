using System.Xml.Serialization;

namespace Egift_main.Subdcription;

[Serializable]
public class Subscription_Premium : Subscription
{
    private double _discount=0.05;
    private bool _freeDelivery;
    private bool _freePriority;

    public Subscription_Premium(double price, bool freeDelivery, bool freePriority) : base(price)
    {
        _freeDelivery = freeDelivery;
        _freePriority = freePriority;
    }

    public double Discount
    {
        get => _discount;
        set => _discount = value;
    }

    public bool FreeDelivery
    {
        get => _freeDelivery;
        set => _freeDelivery = value;
    }

    public bool FreePriority
    {
        get => _freePriority;
        set => _freePriority = value;
    }
    public static void saveFreeDelivery(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(bool));
        using (StreamWriter writer = new StreamWriter(path))
        {
            serializer.Serialize(writer, path);
        }
    }
    
    public static object readFreeDelivery(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(bool));
        using (StreamReader reader = new StreamReader(path))
        {
            bool element = (bool)serializer.Deserialize(reader);
            return element;
        }
    }
    
    public static void readFreePriority(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(bool));
        using (StreamWriter writer = new StreamWriter(path))
        {
            serializer.Serialize(writer, path);
        }
    }
    
    public static object writeFreePriority(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(bool));
        using (StreamReader reader = new StreamReader(path))
        {
            bool element = (bool)serializer.Deserialize(reader);
            return element;
        }
    }
    public static void saveDiscount(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(double));
        using (StreamWriter writer = new StreamWriter(path))
        {
            serializer.Serialize(writer, path);
        }
    }
    
    public static void readDiscount(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(double));
        using (StreamReader reader = new StreamReader(path))
        {
            double element = (double)serializer.Deserialize(reader);
        }
    }
}