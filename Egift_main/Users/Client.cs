using System.Collections;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.VisualBasic;

namespace Egift_main;

public class Client
{
    private string name;
    private ArrayList Wishlist = new ArrayList();
    private DateFormat birthday;

    public Client(string name, DateFormat birthday)
    {
        this.name = name;
        this.birthday = birthday;
    }
    
}