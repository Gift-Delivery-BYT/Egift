namespace Egift_main.Models.Partnership_Accounts;

public interface IUser
{
    public void RequestRefund();
    public void PlaceOrder();
    public void TrackDelivery();
    public void ViewNotifications();
    public void AddWishlistItem();
}