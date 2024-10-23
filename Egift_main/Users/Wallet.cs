using System.Diagnostics;

namespace Egift_main;

public class Wallet
{
    private double _amount;
    enum _Currency
    {
        us,
        pl,
        ua,
        eu
    }

    public void _AddMoney(double toAdd)
    {
        _amount += toAdd;
    }

    public double GetAmount()
    {
        return _amount;
    }

    private void _changeCurrency()
    {
        
    }
}