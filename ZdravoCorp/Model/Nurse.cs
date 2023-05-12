using ZdravoCorp.Serializer;

namespace ZdravoCorp.Model;

public class Nurse : User, ISerializable
{
   public Nurse()
   {
      
   }
   
   public Nurse(string email, string password, string name, string surname)  : base(email, password, name, surname)
   {
   
   }
   
   public string[] ToCSV()
   {
      string[] csvValues = { Email, Password, Name, Surname };
      return csvValues;
   }

   public void FromCSV(string[] values)
   {
      Email = values[0];
      Password = values[1];
      Name = values[2];
      Surname = values[3];
   }
}