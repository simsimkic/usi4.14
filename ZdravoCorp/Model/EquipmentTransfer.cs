using System;
using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.Controller;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Model;

public class EquipmentTransfer : ISerializable
{
    public string Equipment { get; set; }
    public int Quantity { get; set; }
    public Room FromRoom { get; set; }
    public Room ToRoom { get; set; }
    public DateTime TransferDate { get; set; }
    public bool IsDynamic { get; set; }
    public bool IsCompleted { get; set; }

    public EquipmentTransfer()
    {

    }
    public EquipmentTransfer(string equipment, int quantity, Room fromRoom, Room toRoom, DateTime transferDate, bool isDynamic)
    {
        Equipment = equipment;
        Quantity = quantity;
        FromRoom = fromRoom;
        ToRoom = toRoom;
        TransferDate = transferDate;
        IsDynamic = isDynamic;
        IsCompleted = false;
    }

    public string[] ToCSV()
    {
        string[] csvValues = { Equipment, Quantity.ToString(), FromRoom.Type + "," + FromRoom.Name, ToRoom.Type + "," + ToRoom.Name, TransferDate.ToString(), IsDynamic.ToString(), IsCompleted.ToString() };
        return csvValues;
    }

    public void FromCSV(string[] values)
    {
        RoomController _controller = new RoomController();
        Equipment = values[0];
        Quantity = int.Parse(values[1]);
        string[] FromRoomData = values[2].Split(",");
        FromRoom = _controller.GetByName(FromRoomData[0], FromRoomData[1]);
        string[] ToRoomData = values[3].Split(",");
        ToRoom = _controller.GetByName(ToRoomData[0], ToRoomData[1]);
        TransferDate = DateTime.Parse(values[4]);
        IsDynamic = bool.Parse(values[5]);
        IsCompleted = bool.Parse(values[6]);
    }
}
