using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace HR_sistem.Models
{
    public class Zaposleni
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }
        public string Position { get; set; }
        public int WorkingHours { get; set; }
        public float PayingHour { get; set; }
        public DateTime DateOfStart { get; set; }
        public Odeljenje Odeljenje { get; set; }
        public string FullName => $"{Name} {Surname}";

        public List<PerformanceReview> Reviews { get; set; }

        public List<Benefits> Benefits { get; set; }


        public Zaposleni()
        {
            Reviews = new List<PerformanceReview>();
            Benefits = new List<Benefits>();
        }

        public Zaposleni(int id, string name, string surname, string contact, string address, string position, int workingHours, float payingHour, DateTime dateOfStart, Odeljenje odeljenje, List<PerformanceReview> reviews, List<Benefits> benefits)
        {
            ID = id;
            Name = name;
            Surname = surname;
            Contact = contact;
            Address = address;
            Position = position;
            WorkingHours = workingHours;
            PayingHour = payingHour;
            DateOfStart = dateOfStart;
            Odeljenje = odeljenje;
            Reviews = reviews ?? new List<PerformanceReview>();
            Benefits = benefits ?? new List<Benefits>();
        }
    }
}