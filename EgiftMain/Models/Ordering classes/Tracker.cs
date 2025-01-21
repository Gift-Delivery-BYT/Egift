using Microsoft.VisualBasic;

namespace Egift_main.Order;
[Serializable]
public class Tracker
{
    private string _location;
    private int _tracker_id;
    private DateTime _estimated_time_for_arrival;
    private Order _AssignedOrder { get; set; }
    private static List<Tracker> _treckers = new List<Tracker>();
    private Employee _assignedEmployee;
    private DateTime EstimatedTimeForArrival
    {
        get => _estimated_time_for_arrival;
        set => _estimated_time_for_arrival = value;
    }

    public Order AssignedOrder
    {
        get => _AssignedOrder;
        set => _AssignedOrder = value;
    }
    public Tracker(int trackerId, DateTime estimatedTimeForArrival, Order assignedOrder)
    {
        _tracker_id = trackerId;
        _estimated_time_for_arrival = estimatedTimeForArrival;
        _AssignedOrder = assignedOrder;
        _treckers.Add(this);
    }

    public void AssignEmployee(Employee employee)
    {
        if (employee == null)
            throw new ArgumentNullException(nameof(employee), "Employee cannot be null.");
   
        if (_assignedEmployee != employee)
        {
            _assignedEmployee = employee;
            if (!employee._trackers.Contains(this))
                employee.AddTracker(this);
        }
    }
   
    public void RemoveEmployee()
    {
        if (_assignedEmployee != null)
        {
            var tempEmployee = _assignedEmployee;
            _assignedEmployee = null;
   
            if (tempEmployee._trackers.Contains(this))
                tempEmployee.RemoveTracker(this); 
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
    public void AssignTrackerToOrder(Order order)
    {
        AssignedOrder = order;
        if (!_OrderIsAssigned(order))
        {
            order.AssignTracker(this);
        }
    }

    private bool _OrderIsAssigned(Order order)
    {
        if (_AssignedOrder != null) return true;
        else return false;
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

    public Tracker()
    {
        
    }
}
