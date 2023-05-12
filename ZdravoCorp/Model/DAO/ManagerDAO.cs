using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.Observer;
using ZdravoCorp.Storage;

namespace ZdravoCorp.Model.DAO;

public class ManagerDAO
{
    private List<IObserver> _observers;
    private ManagerStorage _storage;
    private List<Manager> _managers;

    public ManagerDAO()
    {
        _observers = new List<IObserver>();
        _storage = new ManagerStorage();
        _managers = _storage.Load();
    }

    public void Add(Manager manager)
    {
        _managers.Add(manager);
        _storage.Save(_managers);
        NotifyObservers();
    }
    
    public void Remove(Manager manager)
    {
        _managers.Remove(manager);
        _storage.Save(_managers);
        NotifyObservers();
    }

    public List<Manager> GetAll()
    {
        return _managers;
    }

    public Manager GetByEmail(string managerEmail)
    {
        return _managers.FirstOrDefault(manager => manager.Email == managerEmail);
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
