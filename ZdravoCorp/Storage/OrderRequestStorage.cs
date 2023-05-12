using System.Collections.Generic;
using ZdravoCorp.Model;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Storage;

public class OrderRequestStorage
{
    private const string StoragePath = @"..\..\..\Data\OrderRequests.csv";
    private Serializer<OrderRequest> _serializer;

    public OrderRequestStorage()
    {
        _serializer = new Serializer<OrderRequest>();
    }
    public List<OrderRequest> Load()
    {
        return _serializer.FromCSV(StoragePath);
    }
    public void Save(List<OrderRequest> orders)
    {
        _serializer.ToCSV(StoragePath, orders);
    }
}
