using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Model
{
    public class TreatmentReferral
    {
        public Patient Patient { get; set; }
        public int Duration { get; set; }
        public string InitialTherapy { get; set; }
        public List<string>AdditionalTests { get; set; }

        public TreatmentReferral(string patientEmail, int duration, string initialTherapy, List<string> additionalTests)
        {
            Patient = Patient.GetPatientByEmail(patientEmail);
            Duration = duration;
            InitialTherapy = initialTherapy;
            AdditionalTests = additionalTests;
        }

    }
}
