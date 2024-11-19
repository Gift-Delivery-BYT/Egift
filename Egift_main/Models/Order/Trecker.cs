using Microsoft.VisualBasic;

namespace Egift_main.Order;

public class Trecker
{
    private string _location;
    private int _tracker_id;
    private DateTime _estimated_time_for_arrival;
    
    private DateTime EstimatedTimeForArrival
    {
        get => _estimated_time_for_arrival;
        set => _estimated_time_for_arrival = value;
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

    private static List<Trecker> _treckers = new List<Trecker>();

    public Trecker(int trackerId, DateTime estimatedTimeForArrival)
    {
        _tracker_id = trackerId;
        _estimated_time_for_arrival = estimatedTimeForArrival;
        _treckers.Add(this);
    }

    public string GetLocation()
    {
        return _location;
    }

    public bool AddTrecker(Trecker trecker) {
        if (TreckerIsValid(trecker)) {
            _treckers.Add(trecker);
            return true;
        }
        return false;
    }


    public bool TreckerIsValid(Trecker trecker)
    {
        if (!string.IsNullOrEmpty(_location) && trecker._tracker_id > 0 &&
            trecker._estimated_time_for_arrival > DateTime.Now)
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
    
    private static bool IsValidTrecker(Trecker trecker)
    {
        if (trecker != null &&
            !string.IsNullOrEmpty(trecker.Location) &&
            trecker.TrackerID > 0 &&
            trecker.GetEstimatedTime() > DateTime.Now) 
        {
            return true;
        }
        throw new ArgumentNullException("invalid variables");
    }
}