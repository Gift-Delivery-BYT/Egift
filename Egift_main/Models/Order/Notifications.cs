﻿namespace Egift_main;

public class Notifications
{
    private string _text;

    enum _type
    {
        email,
        sms
    }

    public string Text
    {
        get => _text;
        set => _text = value;
    }
    private void ChangeTime()
    {
        //Just for full picture purpose...
    }

    private void Send()
    {
       //Same here.. 
    }

    private void SendInTime()
    {
        ///And here...
    }
}