using CA1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA1
{
    public class Ward
    {
        public string Name { get; set; }
        public int Capacity { get; set; }

        public List<Patient> Patients { get; set; }

        public Ward()
        {
            Name = string.Empty;
            //Capacity = "";
            Patients = new List<Patient>(); 
        }
    }


    public class Patient
    {
        public string Name { get; set; }
        public DateTime DOB { get; set; }
        public string BloodType { get; set; }
    }
}

