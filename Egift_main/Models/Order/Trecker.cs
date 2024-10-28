namespace Egift_main.Order;

public class Trecker
{
    private static string location { get; set; }
    private int tracker_id { get; set; }
    private static DateTime Estimated_time_for_arrival { get; set; }

    public Trecker(int trackerId, string location)
    {
        tracker_id = trackerId;
        location = location;
    }
    public void GetLocation()
    {
        
    }
    
    public void GetEstimatedTime()
    {
        
    }
    
    public void UpdateCurrentLocation()
    {
        
    }
}