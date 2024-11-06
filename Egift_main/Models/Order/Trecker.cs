using Microsoft.VisualBasic;

namespace Egift_main.Order;

public class Trecker
{
    private static string _location;
    private int _tracker_id;
    private static DateFormat _estimated_time_for_arrival;
    
    public static DateFormat EstimatedTimeForArrival
    {
        get => _estimated_time_for_arrival;
        set => _estimated_time_for_arrival = value;
    }
    public static string Location
    {
        get => _location;
        set => _location = value;
    }
    public int TrackerID
    {
        get => _tracker_id;
        set => _tracker_id = value;
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