using System.Net;
using System.Net.Mail;
using System.Xml;
using System.Xml.Serialization;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Egift_main;

[Serializable]
public class Notifications
{
    private string _text { get; set; }
    private User _user; 
    private List<User> _users { get; set; } = new List<User>();
    public List<User> Users => _users;
    public User User
    {
        get => _user;
        set
        {
            if (_user == value) return;

            _user = value;
            _user?.AddNotification(this); 
        }
    }
    public Notifications(string text, User user)
    {
        _text = text;
        User = user; 
    }

    public void AddUser(User user)
    {
        if (_users.Contains(user)) return;

        _users.Add(user);
        if (!user.Notifications.Contains(this)) user.AddNotification(this); 
    }

    public void RemoveUser(User user)
    {
        if (!_users.Contains(user)) return;

        _users.Remove(user);
        if (user.Notifications.Contains(this)) user.RemoveNotification(this); 
    }

    public enum _type
    {
        email,
        sms
    }
    [XmlArray]
    private static List<Notifications> _notificationList = new List<Notifications>();

    public Notifications() { }

    public Notifications(string text)
    {
        _text = text;
        _notificationList = new List<Notifications>();
        _notificationList.Add(this);
    }
    
    public void TimeDeliveryWasChanged(DateTime newTime)
    {
        if (newTime < DateTime.Now)
        {
            throw new ArgumentException("Delivery time cannot be set to the past.");
        }

        _text = $"The delivery time was changed to: {newTime.ToString("f")}";
    
        if (SendEmail())
            Send(_type.email);

        if (SendSms())
            Send(_type.sms); 
    }


    public bool Send(_type notifications)
    {
        if (notifications == _type.email)
            return SendEmail();
        
        else if(notifications == _type.sms)
            return SendSms();
        
        return false;
    }
    
    private bool SendEmail()
    {
        try
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587, 
                Credentials = new NetworkCredential("s27776@pjwstk.edu.pl", "passw0rd"), 
                EnableSsl = true 
            };

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress("s27776@pjwstk.edu.pl"),  
                Subject = "<h1>Notification from E-gift System<h1>", 
                Body = _text, 
                IsBodyHtml = false 
            };

            mailMessage.To.Add("s26871@pejot.edu.pl");

            smtpClient.Send(mailMessage);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
            return false;
        }
        return true;
    }

    private bool SendSms()
    {
        //twilio provides a HTTP-based API for sending and receiving phone calls and text messages
        try
        {
            TwilioClient.Init("accountSid", "authToken");

            var message = MessageResource.Create(
                body: _text,
                from: new PhoneNumber("+11234567890"), 
                to: new PhoneNumber("+10987654321")
            );

            Console.WriteLine(message.Sid);
        }
        catch (Twilio.Exceptions.ApiException ex)
        {
            Console.WriteLine($"Twilio API error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending SMS: {ex.Message}");
        }
        return true;
    }

    public void SendInTime(DateTime sendTime)
    {
        if (DateTime.Now >= sendTime)
        {
            Console.WriteLine("Sending a notification");
            Send(_type.email);
        }
        else
            Console.WriteLine("It's not time to send the notification yet");
    }
    
    public static bool Serialize(string path = "./Order/Serialized/Notifications.xml")
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Notifications));
        using (StreamWriter writer = new StreamWriter(path)) {
            serializer.Serialize(writer, _notificationList);
        }
        return true;
    }
    public static bool Deserialize(string path = "./Order/Serialized/Notifications.xml")
    {
        StreamReader file;
        try {
            file = File.OpenText(path);
        }
        catch (FileNotFoundException) {
            _notificationList.Clear();
            return false;
        }
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Notifications>));
        using (XmlTextReader reader = new XmlTextReader(file)) {
            try {
                _notificationList = (List<Notifications>)xmlSerializer.Deserialize(reader);
            }
            catch (InvalidCastException) {
                _notificationList.Clear();
                return false;
            }
            return true;
        }
    }
}