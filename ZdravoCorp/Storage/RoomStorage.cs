using System.Collections.Generic;
using ZdravoCorp.Model;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Storage;

public class RoomStorage
{
    private const string StoragePath = @"..\..\..\Data\Rooms.csv";

    private Serializer<Room> _serializer;

    public RoomStorage()
    {
        _serializer = new Serializer<Room>();
    }

    public List<Room> Load()
    {
        return _serializer.FromCSV(StoragePath);
    }

    public void Save(List<Room> rooms)
    {
        _serializer.ToCSV(StoragePath, rooms);
    }
}
