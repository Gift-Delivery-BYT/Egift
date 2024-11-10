using System.Xml;
using System.Xml.Serialization;
using Egift_main.Order;

namespace Egift_main;

[Serializable]
public class Notifications
{
    private string _text { get; set; }

    public enum _type
    {
        email,
        sms
    }
    [XmlArray]
    private static List<Notifications> _notificationList = new List<Notifications>();

    public Notifications() { }

    public Notifications(string text)
    {
        _text = text;
        _notificationList.Add(this);
    }
    
    public void TimeDeliveryWasChanged(DateTime newTime)
    {
        
    }

    public void Send(_type notifications)
    {
        if (notifications == _type.email)
        {
            
        }
        else if(notifications == _type.sms)
        {
            
        }
    }

    public void SendInTime()
    {
        ///And here...
    }
    
    public static bool Serialize(string path = "./Order/Serialized/Notifications.xml")
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Notifications));
        using (StreamWriter writer = new StreamWriter(path)) {
            serializer.Serialize(writer, _notificationList);
        }
        return true;
    }
    public static bool Deserialize(string path = "./Order/Serialized/Notifications.xml")
    {
        StreamReader file;
        try {
            file = File.OpenText(path);
        }
        catch (FileNotFoundException) {
            _notificationList.Clear();
            return false;
        }
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Notifications>));
        using (XmlTextReader reader = new XmlTextReader(file)) {
            try {
                _notificationList = (List<Notifications>)xmlSerializer.Deserialize(reader);
            }
            catch (InvalidCastException) {
                _notificationList.Clear();
                return false;
            }
            return true;
        }
    }
}