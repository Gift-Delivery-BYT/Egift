using Microsoft.VisualBasic;

namespace Egift_main.Order;

public class Tracker
{
    private string _location;
    private int _tracker_id;
    private DateTime _estimated_time_for_arrival;
    private Order _AssignedOrder;
    
    private DateTime EstimatedTimeForArrival
    {
        get => _estimated_time_for_arrival;
        set => _estimated_time_for_arrival = value;
    }

    private Order AssignedOrder
    {
        get => _AssignedOrder;
        set
        {
            if (value == null)
                throw new ArgumentException("Order is NULL");
            _AssignedOrder = value;
        }
    }


    public string Location
    {
        get => _location;
        set => _location = value;
    }
    public int TrackerID
    {
        get => _tracker_id;
        set => _tracker_id = value;
    }

    private static List<Tracker> _treckers = new List<Tracker>();

    public Tracker(int trackerId, DateTime estimatedTimeForArrival)
    {
        _tracker_id = trackerId;
        _estimated_time_for_arrival = estimatedTimeForArrival;
        _treckers.Add(this);
    }


    public void AssignTrecker(Order order)
    {
        AssignedOrder = order;
    }

    public string GetLocation()
    {
        return _location;
    }

    public bool AddTrecker(Tracker tracker) {
        if (TreckerIsValid(tracker)) {
            _treckers.Add(tracker);
            return true;
        }
        return false;
    }


    public bool TreckerIsValid(Tracker tracker)
    {
        if (!string.IsNullOrEmpty(_location) && tracker._tracker_id > 0 &&
            tracker._estimated_time_for_arrival > DateTime.Now)
            return true;
        
        throw new ArgumentNullException();
    }
    public DateTime GetEstimatedTime()
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
    
    public void UpdateEstimationTime(DateTime newTimeEstimation)
    {
        if (newTimeEstimation != EstimatedTimeForArrival)
        {
            EstimatedTimeForArrival = newTimeEstimation;
            Console.WriteLine($"New Estimation Time of deivelry is : {newTimeEstimation}");
        }
        else
            Console.WriteLine("No updates, time of tracker is the same");
            
    }
    
    private static bool IsValidTrecker(Tracker tracker)
    {
        if (tracker != null &&
            !string.IsNullOrEmpty(tracker.Location) &&
            tracker.TrackerID > 0 &&
            tracker.GetEstimatedTime() > DateTime.Now) 
        {
            return true;
        }
        throw new ArgumentNullException("invalid variables");
    }
}