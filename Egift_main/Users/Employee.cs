using System.Collections;
using Egift_main.Order;

namespace Egift_main;

public class Employee : User
{
    private string schedule;
    private Refund _refund;

    public Employee(int id, string phoneNumber, string email, string schedule, Wallet userWallet) : base(id, phoneNumber, email,userWallet)
    {
        this.schedule = schedule;
    }

    public Refund Refund
    {
        get => _refund;
        set => _refund = value ?? throw new ArgumentNullException(nameof(value));
    }

   public Tuple<bool, int> ApproveRefund(User user,double amount)
    {
        _refund.sendRefundRequest(user, amount);
        Tuple<bool, int> Info = new Tuple<bool, int>(true,this.Id);
        return Info;
    }
    
}