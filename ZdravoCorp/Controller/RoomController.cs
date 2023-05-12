using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Model.DAO;
using ZdravoCorp.Model;
using ZdravoCorp.Observer;

namespace ZdravoCorp.Controller;

public class RoomController
{
    private RoomDAO _rooms;

    public RoomController()
    {
        _rooms = new RoomDAO();
    }

    public List<Room> GetAllRooms()
    {
        return _rooms.GetAll();
    }

    public void Create(Room room)
    {
        _rooms.Add(room);
    }
    public void Delete(Room room)
    {
        _rooms.Remove(room);
    }
    public void Subscribe(IObserver observer)
    {
        _rooms.Subscribe(observer);
    }
    public List<Equipment> GetAllEquipment()
    {
        return _rooms.GetAllEquipment();
    }

    public List<Equipment> GetEquipmentSum()
    {
        return _rooms.GetEquipmentSum();
    }

    public Room GetByName(string type, string name)
    {
        return _rooms.GetByRoomNameType(type, name);
    }
}
