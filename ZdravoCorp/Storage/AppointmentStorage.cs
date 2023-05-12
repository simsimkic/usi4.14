using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Model;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Storage
{
    public class AppointmentStorage
    {
        private const string StoragePath = @"..\..\..\Data\Appointments.csv";

        private Serializer<Appointment> _serializer;

        public AppointmentStorage()
        {
            _serializer = new Serializer<Appointment>();
        }

        public List<Appointment> Load()
        {
            return _serializer.FromCSV(StoragePath);
        }
        
        public void Save(List<Appointment> appointments)
        {
            _serializer.ToCSV(StoragePath, appointments);
        }
    }
}
