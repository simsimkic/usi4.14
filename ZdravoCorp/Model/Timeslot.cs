using System;

namespace ZdravoCorp.Model
{
    public class Timeslot
    {
        public DateTime DateTime { get; set; }
        public double DurationInMinutes { get; set; }

        public Timeslot(DateTime dt, double durationInMinutes)
        {
            this.DateTime = dt;
            this.DurationInMinutes = durationInMinutes;
        }
        public Timeslot()
        {

        }
        public bool IsOverlapping(Timeslot other)
        {
            DateTime firstTimeslotStart = this.DateTime;
            DateTime firstTimeslotEnd = this.DateTime.AddMinutes(this.DurationInMinutes);
            DateTime secondTimeslotStart = other.DateTime;
            DateTime secondTimeslotEnd = other.DateTime.AddMinutes(this.DurationInMinutes);

            return !(DateTime.Compare(firstTimeslotStart, secondTimeslotEnd) > 0 ||
                    DateTime.Compare(firstTimeslotEnd, secondTimeslotStart) < 0);
        }

        public bool IsAfter(DateTime dateTime) => this.DateTime.CompareTo(dateTime) > 0;

        public bool IsBefore(DateTime dateTime) => this.DateTime.CompareTo(dateTime) < 0;


        public string[] ToCSV()
        {
            string[] csvValues = { DateTime.ToString(),DurationInMinutes.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            DateTime = DateTime.Parse(values[0]);
            DurationInMinutes = Double.Parse(values[1]);
        }
    }
}
