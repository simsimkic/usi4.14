using System;

namespace ZdravoCorp.Model
{
    public class Referral
    {
        public Patient Patient { get; set; }

        public Doctor Doctor { get; set; }

        public string Specialization { get; set; }

        public Referral(string patientEmail, string doctorId, string specialization)
        {   
            if(doctorId=="")
            {
                Doctor = new Doctor();
            }
            else
            {
                Doctor = Doctor.GetDoctorByEmail(doctorId);
            }
            Patient = Patient.GetPatientByEmail(patientEmail);
            Specialization = specialization;
        }
        public Referral()
        {
            Patient = new Patient();
            Doctor = new Doctor();
            Specialization=""; 
        }
    }
}
