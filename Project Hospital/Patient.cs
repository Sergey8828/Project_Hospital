using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CA1
{
    //create enum
    public enum BloodTypes { A,B,AB,O}

    public class Patient
    {
        //properties
        public string Name { get; set; }
        public DateTime DOB { get; set; }
        public string BloodType { get; set; }
        public BloodTypes BloodType_2 { get; set; }


        // zero constructor
        public Patient() { }


        //constructor
        public Patient(string name, DateTime dOB, string bloodType)
        {
            Name = name;
            DOB = dOB;
            BloodType = bloodType;
        }   


        //method ovveride ToString
        public override string ToString()
        {
            return $"{Name} {DateTime.Today.Year - DOB.Year} Type: {BloodType}";
        }
    }
}

