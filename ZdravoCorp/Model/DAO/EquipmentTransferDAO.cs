using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.Observer;
using ZdravoCorp.Storage;
using ZdravoCorp.Controller;

namespace ZdravoCorp.Model.DAO;

public class EquipmentTransferDAO : ISubject
{
    private List<IObserver> _observers;
    private EquipmentTransferStorage _storage;
    private List<EquipmentTransfer> _transfers;

    public EquipmentTransferDAO()
    {
        _observers = new List<IObserver>();
        _storage = new EquipmentTransferStorage();
        _transfers = _storage.Load();
    }

    public void Add(EquipmentTransfer transfer)
    {
        _transfers.Add(transfer);
        _storage.Save(_transfers);
        NotifyObservers();
    }

    public void Remove(EquipmentTransfer transfer)
    {
        _transfers.Remove(transfer);
        _storage.Save(_transfers);
        NotifyObservers();
    }

    public List<EquipmentTransfer> GetAll()
    {
        return _transfers;
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

    public void ValidateTransfers()
    {
        RoomController _rooms = new RoomController();

        List<EquipmentTransfer> transfersCopy = _transfers;
        foreach (var transfer in transfersCopy.ToList())
        {
            if (transfer.TransferDate <= System.DateTime.Now && transfer.IsCompleted == false)
            {
                Room fromRoom = _rooms.GetByName(transfer.FromRoom.Type,transfer.FromRoom.Name);
                Room toRoom = _rooms.GetByName(transfer.ToRoom.Type, transfer.ToRoom.Name);

                _rooms.Delete(fromRoom);
                _rooms.Delete(toRoom);
                List<Equipment> fromRoomEquipment = fromRoom.PresentEquipment.ToList();
                List<Equipment> toRoomEquipment = toRoom.PresentEquipment.ToList();
                foreach(var fromEquipment in fromRoomEquipment)
                {
                    foreach(var toEquipment in toRoomEquipment)
                    {
                        if (fromEquipment.Name.Equals(toEquipment.Name) && fromEquipment.Name.Equals(transfer.Equipment))
                        {
                            //_rooms.Delete(transfer.FromRoom);
                            //_rooms.Delete(transfer.ToRoom);

                            toEquipment.Quantity += transfer.Quantity;
                            fromEquipment.Quantity -= transfer.Quantity;
                            fromRoom.PresentEquipment = fromRoomEquipment.ToArray();
                            toRoom.PresentEquipment = toRoomEquipment.ToArray();
                            
                            _rooms.Create(toRoom);
                            _rooms.Create(fromRoom);
                            this.Remove(transfer);
                            transfer.IsCompleted = true;
                            this.Add(transfer);
                        }
                    }
                }
            }
        }
    }
}
