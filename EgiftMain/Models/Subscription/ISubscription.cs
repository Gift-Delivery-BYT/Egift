namespace Egift_main.Subscription;

public interface ISubscription
{
    public void EvaluatePrice();
    public void AddClient(Client client);
    public void RemoveClient(Client client);
    public bool ClientIsConnected(Client client);
}