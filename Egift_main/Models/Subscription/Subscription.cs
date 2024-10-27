using System.Collections;
using System.Xml;
using System.Xml.Serialization;

namespace Egift_main.Subdcription;

[Serializable]
public abstract class Subscription
{

    private static ArrayList _features = new ArrayList();
    private static double _price;

    protected Subscription(double price)
    {
        _price = price;
    }

    public static double Price
    {
        get => _price;
        set => _price = value;
    }

    public static ArrayList Features
    {
        get => _features;
        set => _features = value ?? throw new ArgumentNullException(nameof(value));
    }

    public static void SaveFeatures(string path = "./Subscription/Serialized/StandardFeatures.xml")
    {
        StreamWriter toSerialize = File.CreateText(path);
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(ArrayList));
        using (XmlTextWriter writer = new XmlTextWriter(toSerialize))
        {
            xmlSerializer.Serialize(writer, path);
        }
    }

    public static bool GetFeatures(string path = "./Subscription/Serialized/StandardFeatures.xml")
    {
        StreamReader file;
        try
        {
            file = File.OpenText(path);
        }
        catch (FileNotFoundException)
        {
            Features.Clear();
            return false;
        }

        XmlSerializer xmlSerializer = new XmlSerializer(typeof(ArrayList));
        using (XmlTextReader reader = new XmlTextReader(file))
        {
            try
            {
                _features = (ArrayList)xmlSerializer.Deserialize(reader);
            }
            catch (InvalidCastException)
            {
                Features.Clear();
                return false;
            }
            catch (Exception)
            {
                Features.Clear();
                return false;
            }
        }
        return true;
    }
    
    public static void savePrice(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(double));
        using (StreamWriter writer = new StreamWriter(path))
        {
            serializer.Serialize(writer, path);
        }
    }
    
    public static void ReadPrice(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(double));
        using (StreamReader reader = new StreamReader(path))
        {
            double element = (double)serializer.Deserialize(reader);
        }
    }
}