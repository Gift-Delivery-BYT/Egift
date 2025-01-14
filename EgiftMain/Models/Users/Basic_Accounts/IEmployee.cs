namespace Egift_main;

public interface IEmployee
{
  public Tuple<bool, int> ApproveRefund(Client client, double amount, DateTime purchaseDate);
}