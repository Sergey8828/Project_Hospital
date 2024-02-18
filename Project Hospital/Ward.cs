using CA1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA1
{
    public class Ward
    {

        //properties
        public string Name { get; set; }
        public int Capacity { get; set; }

        // adding observable collection
        public ObservableCollection<Patient> Patients { get; set; } = new ObservableCollection<Patient>();

        // zero constructor
        public Ward() { }


        //contsructor
        public Ward (string name, int capacity)
        {
            Name = name;    
            Capacity = capacity;    
        }

        //method ovveride ToString
        public override string ToString()
        {
            return string.Format($"{Name}\t(limit:{Capacity})");
        }     
    }
}


