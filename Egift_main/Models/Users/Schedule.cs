using System.Collections;

namespace Egift_main.Models.Order;

public class Schedule
{
    private List<DateTime> scheduleDate = new List<DateTime>();
    private List<DateTime> holidays = new List<DateTime>();
    public Employee _owner;

    
    public List<DateTime> ScheduleDate
    {
        get => scheduleDate;
        set
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            foreach (var date in value)
            {
                if (date < DateTime.Now)
                {
                    throw new ArgumentException("Schedule date cannot be in the past.");
                }
            }

            scheduleDate = value;
        }
    }
    
    public List<DateTime> Holidays
    {
        get => holidays;
        set => holidays = value ?? throw new ArgumentNullException(nameof(value));
    }
    public Schedule(Employee owner)
    {
        _owner = owner;
    }
    public Schedule()
    { }
    
    public Employee Owner
    {
        get => _owner;
    }

    public void AssignEmployee(Employee employee)
    {
        if (_owner == employee)
            return;

        if (_owner != null)
            _owner.RemoveSchedule();

        _owner = employee;
    }

    public void RemoveEmployee()
    {
        if (_owner == null)
            return;

        Employee previousOwner = _owner;
        _owner = null;

        if (previousOwner.Schedule == this)
            previousOwner.RemoveSchedule();
    }
    public void AddWorkHours(DateTime workingHours)
    {
        if (!scheduleDate.Contains(workingHours))
        {
            scheduleDate.Add(workingHours);
            Console.WriteLine("Work hour {workingHours} added");
        }
        else
            Console.WriteLine("Work hour {workingHours} is already exist");
    }

    public void AddHolidays(DateTime holdayDays)
    {
        if (!holidays.Contains(holdayDays))
        {
            holidays.Add(holdayDays);
            Console.WriteLine("Holiday {holdayDays.ToShortDateString()} added");
        }
        else
            Console.WriteLine("Holiday {holdayDays.ToShortDateString()} is already in a list");
    }
}