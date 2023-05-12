using System.Collections.Generic;
using ZdravoCorp.Model;
using ZdravoCorp.Model.DAO;
using ZdravoCorp.Observer;

namespace ZdravoCorp.Controller;

public class OrderRequestController
{
    private OrderRequestDAO _orders;

    public OrderRequestController()
    {
        _orders = new OrderRequestDAO();
    }

    public List<OrderRequest> GetAllOrders()
    {
        return _orders.GetAll();
    }

    public void Create(OrderRequest order)
    {
        _orders.Add(order);
    }

    public void Delete(OrderRequest order)
    {
        _orders.Remove(order);
    }

    public void Subscribe(IObserver observer)
    {
        _orders.Subscribe(observer);
    }

    public void ValidateOrders()
    {
        _orders.ValidateOrders();
    }
}