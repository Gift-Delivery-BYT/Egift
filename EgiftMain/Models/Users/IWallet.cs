namespace Egift_main.Models.Users;

public interface IWallet
{
    public void _AddMoney(double toAdd);
   // public void UpdateQuantity();
    public void SpendMoney(double amountToSpend);
    public double  GetAmount();
    public void DisplayCurrentInfoOfWallet();
    public void AddClient(Client client);
    public void RemoveClient(Client client);
    public void _changeCurrency(Wallet._Currency currency);
}