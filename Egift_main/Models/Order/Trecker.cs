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

    public string GetLocation()
    {
        return _location;
    }
    
    public DateFormat GetEstimatedTime()
    {
        return _estimated_time_for_arrival;
    }
    
    public void UpdateCurrentLocation(string newLocation)
    {
        if (newLocation != Location)
        {
            Location = newLocation;
            Console.WriteLine($"New location is : {Location}");
        }
        else
            Console.WriteLine("No updates, location is the same");
            
    }
    
    public void UpdateEstimationTime(DateFormat newTimeEstimation)
    {
        if (newTimeEstimation != EstimatedTimeForArrival)
        {
            EstimatedTimeForArrival = newTimeEstimation;
            Console.WriteLine($"New Estimation Time of deivelry is : {newTimeEstimation}");
        }
        else
            Console.WriteLine("No updates, time of tracker is the same");
            
    }
}