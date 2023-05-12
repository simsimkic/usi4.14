using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.Observer;
using ZdravoCorp.Storage;

namespace ZdravoCorp.Model.DAO;

public class RoomDAO : ISubject
{
    private List<IObserver> _observers;
    private RoomStorage _storage;
    private List<Room> _rooms;
    
    public RoomDAO()
    {
        _observers = new List<IObserver>();
        _storage = new RoomStorage();
        _rooms = _storage.Load();
    }

    public void Add(Room room)
    {
        _rooms.Add(room);
        _storage.Save(_rooms);
        NotifyObservers();
    }

    public void Remove(Room room)
    {
        _rooms.Remove(room);
        _storage.Save(_rooms);
        NotifyObservers();
    }

    public List<Room> GetAll()
    {
        return _rooms;
    }

    public Room GetByRoomNameType(string type, string name)
    {
        return _rooms.FirstOrDefault(room => room.Type == type && room.Name == name);
    }

    public List<Equipment> GetAllEquipment()
    {
        List<Equipment> Equipment = new List<Equipment>();
        foreach (Room room in _rooms)
        {
            foreach (Equipment eq in room.PresentEquipment)
                Equipment.Add(eq);
        }
        return Equipment;
    }

    public List<Equipment> GetEquipmentSum()
    {
        List<Equipment> EquipmentSum = new List<Equipment>();
        EquipmentSum.Add(new Equipment("Pens", 0, "/", true, "Room Furniture"));
        EquipmentSum.Add(new Equipment("Gauze", 0, "/", true, "Medical Exam Equipment"));
        EquipmentSum.Add(new Equipment("Band-Aid", 0, "/", true, "Medical Exam Equipment"));
        EquipmentSum.Add(new Equipment("Syringe", 0, "/", true, "Surgery Equipment"));
        EquipmentSum.Add(new Equipment("Tissues", 0, "/", true, "Hallway Equipment"));
        List<Equipment> AllEquipment = new List<Equipment>(this.GetAllEquipment());
        foreach(Equipment eq1 in EquipmentSum)
        {
            foreach(Equipment eq2 in AllEquipment)
            {
                if(eq1.Name == eq2.Name)
                {
                    eq1.Quantity += eq2.Quantity;
                }
            }
        }
        return EquipmentSum;
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
