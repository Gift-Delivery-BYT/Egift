namespace Egift_main.Models.Ordering_classes;

public interface INotifications
{
    public void TimeDeliveryWasChanged(DateTime newTime);
    public bool Send(Notifications._type notifications);
    public void SendInTime(DateTime sendTime);
}