using Microsoft.VisualBasic;

namespace Egift_main.Order;

public class Refund
{
    private DateFormat _date;
    private bool _isApproved=false;

   public void sendRefundRequest(User user, double amount)
    {
        CheckApproval();
        user.UserWallet._AddMoney(amount);
    }

    void CheckApproval()
    {
        //Information must to be taken from the server
    }

    public DateFormat Date
    {
        get => _date;
        set => _date = value;
    }

    public bool IsApproved
    {
        get => _isApproved;
        set => _isApproved = value;
    }
}