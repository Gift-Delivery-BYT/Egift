namespace Egift_main.Models.Partnership_Accounts;

public interface IBusinessAccount
{
    public void AddAuthorizedUser(User user);
    public void PlaceBulkOrder(DateTime deliveryDay, List<int> bulOrderID);
}
