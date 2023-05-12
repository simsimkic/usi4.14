using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Model;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Storage
{
    public class DoctorStorage
    {
        private const string StoragePath = @"..\..\..\Data\Doctors.csv";

        private Serializer<Doctor> _serializer;

        public DoctorStorage()
        {
            _serializer = new Serializer<Doctor>();
        }

        public List<Doctor> Load()
        {
            return _serializer.FromCSV(StoragePath);
        }
        public void Save(List<Doctor> doctors)
        {
            _serializer.ToCSV(StoragePath, doctors);
        }
    }
}
