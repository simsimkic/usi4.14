using System.Collections.Generic;
using System.Windows.Documents;
using ZdravoCorp.Model.DAO;
using ZdravoCorp.Model;
using ZdravoCorp.Observer;

namespace ZdravoCorp.Controller;

public class NotificationController
{
    private NotificationDAO _notifications;

    public NotificationController()
    {
        _notifications = new NotificationDAO();
    }

    public List<Notification> GetAllNotifications()
    {
        return _notifications.GetAll();
    }

    public Notification GetNotificationById(int notificationId)
    {
        return _notifications.GetById(notificationId);
    }

    public void Create(Notification notification)
    {
        _notifications.Add(notification);
    }

    public void Delete(Notification notification)
    {
        _notifications.Remove(notification);
    }

    public void Subscribe(IObserver observer)
    {
        _notifications.Subscribe(observer);
    }
}