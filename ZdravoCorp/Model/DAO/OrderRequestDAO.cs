using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.Observer;
using ZdravoCorp.Storage;
using ZdravoCorp.Controller;

namespace ZdravoCorp.Model.DAO;

public class OrderRequestDAO : ISubject
{
    private List<IObserver> _observers;
    private OrderRequestStorage _storage;
    private List<OrderRequest> _orders;
    
    public OrderRequestDAO()
    {
        _observers = new List<IObserver>();
        _storage = new OrderRequestStorage();
        _orders = _storage.Load();
    }

    public void Add(OrderRequest order)
    {
        _orders.Add(order);
        _storage.Save(_orders);
        NotifyObservers();
    }

    public void Remove(OrderRequest order)
    {
        _orders.Remove(order);
        _storage.Save(_orders);
        NotifyObservers();
    }

    public List<OrderRequest> GetAll()
    {
        return _orders;
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
        foreach( var observer in _observers)
        {
            observer.Update();
        }
    }

    public void ValidateOrders()
    {
        RoomController _rooms = new RoomController();
        Room warehouse = _rooms.GetByName("Warehouse", "0");
        Equipment[] eq = warehouse.PresentEquipment;
        List<Equipment> equipment = eq.ToList();
        List<OrderRequest> ordersCopy = _orders;
        foreach(var order in ordersCopy.ToList())
        {
            if (order.DateTimeOfArrival <= System.DateTime.Now && order.IsCompleted == false)
            {
                foreach (var equipmentItem in equipment)
                {
                    if (equipmentItem.Name == order.Equipment)
                    {
                        this.Remove(order);
                        equipmentItem.Quantity += order.Quantity;
                        order.IsCompleted = true;
                        _rooms.Delete(warehouse);
                        warehouse.PresentEquipment = equipment.ToArray();
                        _rooms.Create(warehouse);
                        this.Add(order);
                        break;
                    }
                }
            }
        }
    }
}
