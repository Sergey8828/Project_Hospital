using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA1
{
    public class Patient
    {
        public string Name { get; set; }
        public DateTime DOB { get; set; }
        public string BloodType { get; set; }

        public List<Patient> Patients { get; set; }

        public override string ToString()
        {
            return $"{Name} {DOB.ToShortDateString()} {BloodType}";
        }
    }
}

