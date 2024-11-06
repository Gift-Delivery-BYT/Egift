using System.Diagnostics;

namespace Egift_main;

public class Wallet
{
    private double _amount;

    public enum _Currency
    {
        us,
        pl,
        ua,
        eu
    }

    private _Currency _currentCurrency = _Currency.eu;

    public void _AddMoney(double toAdd)
    {
        _amount += toAdd;
    }

    public double GetAmount()
    {
        return _amount;
    }

    public void _changeCurrency(_Currency currency)
    {
        if (_currentCurrency == currency)
            Console.WriteLine("Current currency is: {_currentCurrency}");

        switch (_currentCurrency)
        {
            case _Currency.us:
                ConvertFromUS(currency);
                break;
            case _Currency.ua:
                ConvertFromPL(currency);
                break;
            case _Currency.pl:
                ConvertFromUA(currency);
                break;
            case _Currency.eu:
                ConvertFromEU(currency);
                break;
        }

        _currentCurrency = currency;
        Console.WriteLine("Current currency was changed to: {_currentCurrency}");
    }

    private void ConvertFromUS(_Currency currency)
    {
        _amount = currency switch
        {
            _Currency.eu => _amount * 0.93,
            _Currency.ua => _amount * 41.15,
            _Currency.pl => _amount * 4.07,
            _ => _amount
        };
    }
    private void ConvertFromPL(_Currency currency)
    {
        _amount = currency switch
        {
            _Currency.eu => _amount * 0.23,
            _Currency.ua => _amount * 10.22,
            _Currency.us => _amount * 0.25,
            _ => _amount
        };
    }
    private void ConvertFromEU(_Currency currency)
    {
        _amount = currency switch
        {
            _Currency.pl => _amount * 4.37,
            _Currency.ua => _amount * 44.72,
            _Currency.us => _amount * 1.07,
            _ => _amount
        };
    }
    
    private void ConvertFromUA(_Currency currency)
    {
        _amount = currency switch
        {
            _Currency.eu => _amount * 0.022,
            _Currency.pl => _amount * 0.098,
            _Currency.us => _amount * 0.024,
            _ => _amount
        };
    }

    public void DisplayCurrentInfoOfWallet()
    {
        Console.WriteLine("Current Balance: {_amount} {_currentCurrency}");
    }
}