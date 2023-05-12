using System;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Model;

public class Notification : ISerializable
{
    public enum NotificationStatus
    {
        Unread,
        Read
    }
    
    public int Id { get; set; }
    public string Content { get; set; }
    public NotificationStatus Status { get; set; }

    public Notification()
    {
        
    }

    public Notification(string content)
    {
        Content = content;
        Status = NotificationStatus.Unread;
    }

    public bool IsRead()
    {
        return Status == NotificationStatus.Read;
    }

    public string[] ToCSV()
    {
        string[] csvValues = { Id.ToString(), Content, Status.ToString() };
        return csvValues;
    }

    public void FromCSV(string[] values)
    {
        Id = int.Parse(values[0]);
        Content = values[1];

        Enum.TryParse(values[2], out NotificationStatus temporaryStatus);
        Status = temporaryStatus;
    }
}