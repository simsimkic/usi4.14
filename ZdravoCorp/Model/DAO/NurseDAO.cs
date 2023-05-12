using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.Observer;
using ZdravoCorp.Storage;

namespace ZdravoCorp.Model.DAO;

public class NurseDAO
{
    private List<IObserver> _observers;
    private NurseStorage _storage;
    private List<Nurse> _nurses;

    public NurseDAO()
    {
        _observers = new List<IObserver>();
        _storage = new NurseStorage();
        _nurses = _storage.Load();
    }

    public void Add(Nurse nurse)
    {
        _nurses.Add(nurse);
        _storage.Save(_nurses);
        NotifyObservers();
    }
    
    public void Remove(Nurse nurse)
    {
        _nurses.Remove(nurse);
        _storage.Save(_nurses);
        NotifyObservers();
    }

    public List<Nurse> GetAll()
    {
        return _nurses;
    }

    public Nurse GetByEmail(string nurseEmail)
    {
        return _nurses.FirstOrDefault(nurse => nurse.Email == nurseEmail);
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