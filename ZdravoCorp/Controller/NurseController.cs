using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.Model;
using ZdravoCorp.Model.DAO;
using ZdravoCorp.Observer;

namespace ZdravoCorp.Controller;

public class NurseController
{
   private NurseDAO _nurses;

   public NurseController()
   {
      _nurses = new NurseDAO();
   }

   public List<Nurse> GetAllNurses()
   {
      return _nurses.GetAll();
   }

   public Nurse GetNurseByEmail(string nurseEmail)
   {
      return _nurses.GetByEmail(nurseEmail);
   }

   public void Create(Nurse nurse)
   {
      _nurses.Add(nurse);
   }

   public void Delete(Nurse nurse)
   {
      _nurses.Remove(nurse);
   }

   public void Subscribe(IObserver observer)
   {
      _nurses.Subscribe(observer);
   }
}