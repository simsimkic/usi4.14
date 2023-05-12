using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.Model;
using ZdravoCorp.Model.DAO;
using ZdravoCorp.Observer;

namespace ZdravoCorp.Controller;
public class ManagerController
{
        private ManagerDAO _managers;

        public ManagerController()
        {
            _managers = new ManagerDAO();
        }

        public List<Manager> GetManagers()
        {
            return _managers.GetAll();
        }

        public Manager GetManagerByEmail(string managerEmail)
        {
            return _managers.GetByEmail(managerEmail);
        }

        public void Create(Manager manager)
        {
            _managers.Add(manager);
        }

        public void Delete(Manager manager)
        {
            _managers.Remove(manager);
        }

        public void Subscribe(IObserver observer)
        {
            _managers.Subscribe(observer);
        }
}
