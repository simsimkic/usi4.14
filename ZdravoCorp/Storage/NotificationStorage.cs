using System.Collections.Generic;
using ZdravoCorp.Model;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Storage;

public class NotificationStorage
{
    private const string StoragePath = @"..\..\..\Data\Notifications.csv";

    private Serializer<Notification> _serializer;

    public NotificationStorage()
    {
        _serializer = new Serializer<Notification>();
    }

    public List<Notification> Load()
    {
        return _serializer.FromCSV(StoragePath);
    }

    public void Save(List<Notification> notifications)
    {
        _serializer.ToCSV(StoragePath, notifications);
    }
}