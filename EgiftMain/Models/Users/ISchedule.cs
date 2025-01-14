namespace Egift_main.Models.Users;

public interface ISchedule
{
    public void AddWorkHours(DateTime workingHours);
    public void AddHolidays(DateTime holdayDays);
}