using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

using Newtonsoft.Json;
using System.IO;
using System.Collections.ObjectModel;

namespace CA1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Ward> wards = new List<Ward>();


        public MainWindow()
        {
            InitializeComponent();
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            string s = String.Format("{0:F0}", slider.Value);
            TxtBoxCapacity.Text = "Capacity " + s;
        }

        // startup code
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //string[] names = { "Marx Brothers Ward", "Adams Family Ward" };

          //  lbxWards.ItemsSource = names;


            //create 2 wards objects
            Ward w1 = new Ward() { Name = "Marx Brothers Ward", Capacity = 3};
            Ward w2 = new Ward() { Name = "Adams Family Ward", Capacity = 7};

           

            // add to collection
            wards.Add(w1);
            wards.Add(w2);

            ////display on screen
            lbxWards.ItemsSource = wards;

            //create 3 wards objects
            Patient p1 = new Patient() { Name = "Chico", DOB = new DateTime(1975,01,15), BloodType = "A" };
            Patient p2 = new Patient() { Name = "Graucho", DOB = new DateTime(1975, 01, 15), BloodType = "AB" };
            Patient p3 = new Patient() { Name = "Harpo", DOB = new DateTime(1975, 01, 15), BloodType = "B" };

            List <Patient> patients = new List<Patient>();


            // add to collection
            patients.Add(p1);
            patients.Add(p2);
            patients.Add(p3);

            //display on screen
            lbxPatients.ItemsSource = patients;


            
            
            // CurrentYear - BirthDate


        }


        //add new ward
        private void btnAddWard_Click(object sender, RoutedEventArgs e)
        {
            //read info from screen
            string name = tbxName.Text;
            //int capacity = slider.Value;

            //create ward object with info
            Ward newWard = new Ward() { Name = name, Capacity = 1};

            //add to collection
            //wards.Add(newWard);
            //lbxWards.Text = GetTotalWages().ToString();
        }


        //add new patient
        private void btnAddPatients_Click(object sender, RoutedEventArgs e)
        {
            {
                //read info from screen
                string name = txtboxName.Text;

                //Reading date from screen
                DateTime dob = DatePicker.SelectedDate.Value;

                //int age = dpAge.SelectedDate.Value;
                //string bloodType =   ;

                //create object with info
                //Patient newPatient = new Patient() { Name = name, Age = DateTime.Now-dob, BloodType = bloodType};

                //add to collection
                //patients.Add(newPatient);
                //tblkTotalWages.Text = GetTotalWages().ToString();

            }

        }


        // save objects to Json
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // get string of objects Json formatted
            string json = JsonConvert.SerializeObject(wards, Formatting.Indented);



            // write that to file
            using (StreamWriter sw = new StreamWriter(@"c:\temp\wards.json"))
            {
                sw.Write(json);
            }
        }

        //loads json file
        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            //connect to a file 
            using (StreamReader sr = new StreamReader(@"c:\temp\wards.json"))
            {
                //read text
                string json = sr.ReadToEnd();

                //convert from json to objects 
                wards = JsonConvert.DeserializeObject<List<Ward>>(json);
            }
            //refresh  the display
            lbxWards.ItemsSource = wards;
        
        }
    }

}
