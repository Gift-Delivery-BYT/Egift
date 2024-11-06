using System.Collections;

namespace Egift_main.Models.Order;

public class Schedule
{
    private List<DateTime> scheduleDate = new List<DateTime>();
    private List<DateTime> holidays = new List<DateTime>();


    public List<DateTime> ScheduleDate
    {
        get => scheduleDate;
        set => scheduleDate = value ?? throw new ArgumentNullException(nameof(value));
    }
    
    public List<DateTime> Holidays
    {
        get => holidays;
        set => holidays = value ?? throw new ArgumentNullException(nameof(value));
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