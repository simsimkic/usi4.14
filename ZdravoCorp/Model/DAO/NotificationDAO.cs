using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.Observer;
using ZdravoCorp.Storage;

namespace ZdravoCorp.Model.DAO;

public class NotificationDAO : ISubject
{
    private List<IObserver> _observers;
    private NotificationStorage _storage;
    private List<Notification> _notifications;

    public NotificationDAO()
    {
        _observers = new List<IObserver>();
        _storage = new NotificationStorage();
        _notifications = _storage.Load();
    }

    public int NextId()
    {
        return _notifications.Count == 0 ? 0 : _notifications.Max(notification => notification.Id) + 1;
    }

    public void Add(Notification notification)
    {
        notification.Id = NextId();
        _notifications.Add(notification);
        _storage.Save(_notifications);
        NotifyObservers();
    }

    public void UpdateIndices(int removedId)
    {
        for (; removedId < _notifications.Count; removedId++)
        {
            _notifications[removedId].Id = removedId;
        }
    }

    public void Remove(Notification notification)
    {
        _notifications.Remove(notification);
        UpdateIndices(notification.Id);
        _storage.Save(_notifications);
        NotifyObservers();
    }

    public List<Notification> GetAll()
    {
        return _notifications;
    }

    public Notification GetById(int notificationId)
    {
        return _notifications.FirstOrDefault(notification => notification.Id == notificationId);
    }

    public void Subscribe(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void Unsubscribe(IObserver observer)
    {
        _observers.Remove(observer);
    }

    public void NotifyObservers()
    {
        foreach (var observer in _observers)
        {
            observer.Update();
        }
    }
}