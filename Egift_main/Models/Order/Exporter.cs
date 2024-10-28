using System.Collections;
using Microsoft.VisualBasic;

namespace Egift_main.Order;

public class Exporter
{
    public Exporter(string name, string country, string address, float shippingCost, float itemsSupplied, int phoneNumber, DateTime timeLeadDate)
    {
        this.name = name;
        this.country = country;
        this.address = address;
        shipping_cost = shippingCost;
        items_supplied = itemsSupplied;
        phone_number = phoneNumber;
        time_lead_date = timeLeadDate;
    }

    private string name { set; get; }
    private string country { set; get; }
    private string address { set; get; }
    private float shipping_cost { set; get; }
    private float items_supplied { set; get; }
    private int phone_number { set; get; }
    private DateTime time_lead_date { set; get; }
    private List<Object> documentation = new List<Object>();
    
}