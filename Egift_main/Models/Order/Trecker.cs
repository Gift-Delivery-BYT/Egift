using System.Xml;
using System.Xml.Serialization;
using Egift_main.Subscription;
using Microsoft.VisualBasic;

namespace Egift_main.Order;

public class Trecker
{
    private  string _location;
    private int _tracker_id;
    private  DateFormat _estimated_time_for_arrival;

    private static List<Trecker> _treckers = new List<Trecker>();

    
    private  DateFormat EstimatedTimeForArrival
    {
        get => _estimated_time_for_arrival;
        set => _estimated_time_for_arrival = value;
    }
    private  string Location
    {
        get => _location;
        set => _location = value;
    }
    private int TrackerID
    {
        get => _tracker_id;
        set => _tracker_id = value;
    }

    public Trecker(string location, int trackerId, DateFormat estimatedTimeForArrival)
    {
        _location = location;
        _tracker_id = trackerId;
        _estimated_time_for_arrival = estimatedTimeForArrival;
        _treckers.Add(this);
    }

    private static bool addNewTrecker(Trecker trecker)
    {
        if (TrackerIsValid(trecker)) {
            _treckers.Add(trecker);
            return true;
        }
        return false;
    }
    
    private static bool TrackerIsValid(Trecker trecker)
    {
        if (trecker.Equals(null) &&
            trecker.Location.Equals(null) &&
            trecker._tracker_id.Equals(null)
           ) return true;
        else throw new ArgumentNullException();
    }

    
    private static bool Serialize(string path = "./Subscription/Serialized/Trecker.xml")
    {
            
        XmlSerializer serializer = new XmlSerializer(typeof(Trecker));
        using (StreamWriter writer = new StreamWriter(path)) {
            serializer.Serialize(writer, _treckers);
        }
        return true;
    }

       
    private static bool Deserialize(string path = "./Order/Serialized/Trecker.xml")
    {
        StreamReader file;
        try {
            file = File.OpenText(path);
        }
        catch (FileNotFoundException) {
            _treckers.Clear();
            return false;
        }
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Trecker>));
        using (XmlTextReader reader = new XmlTextReader(file)) {
            try {
                _treckers = (List<Trecker>)xmlSerializer.Deserialize(reader);
            }
            catch (InvalidCastException) {
                _treckers.Clear();
                return false;
            }
            return true;
        }
    }

    public void GetLocation(string location)
    {
        
    }
    
    public void GetEstimatedTime()
    {
        
    }
    
    public void UpdateCurrentLocation()
    {
        
    }
}