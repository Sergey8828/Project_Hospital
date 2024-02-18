using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace CA1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //create observable collections
        ObservableCollection<Ward> wards = new ObservableCollection<Ward>();
        ObservableCollection<Patient> patients = new ObservableCollection<Patient>();

        public MainWindow()
        {
            InitializeComponent();
        }

             //display capacity
        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            string s = String.Format("{0:F0}", slider.Value);
            TxtBoxSliderCapacity.Text = s;
        }

            //startup code
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            //create 2 wards objects
            Ward w1 = new Ward("Marx Brothers Ward", 3);
            Ward w2 = new Ward("Adams Family Ward", 7);

            //add wards to collection
            wards.Add(w1);
            wards.Add(w2);

            //display on screen
            lbxWards.ItemsSource = wards;
            TxtboxWard.Text = $"Wards List ({wards.Count})";

            //create 3 wards objects
            Patient p1 = new Patient() { Name = "Chico", DOB = new DateTime(1975, 01, 15), BloodType = "A" };
            Patient p2 = new Patient() { Name = "Graucho", DOB = new DateTime(1975, 01, 15), BloodType = "AB" };
            Patient p3 = new Patient() { Name = "Harpo", DOB = new DateTime(1975, 01, 15), BloodType = "B" };

            //add patients to ward
            w1.Patients.Add(p1);
            w1.Patients.Add(p2);
            w2.Patients.Add(p3);
        }


            //add new ward
        private void btnAddWard_Click(object sender, RoutedEventArgs e)
        {
            //read info from screen
            string name = tbxName.Text;
            int capacity = Convert.ToInt32(slider.Value);

            //create ward object with info
            Ward newWard = new Ward(name, capacity);

            //add to collection
            wards.Add(newWard);
            lbxWards.ItemsSource = wards;
            TxtboxWard.Text = $"Wards List ({wards.Count})";
            tbxName.Clear();           
        }

                //method of adding patients
        private void btnAddPatients_Click(object sender, RoutedEventArgs e)
        {

            //read info from screen
            string name = txtboxName.Text;

            //Reading date from screen
            DateTime dob = DatePicker.SelectedDate.Value;

            string bloodType = "";

            //Reading what blood type was chosen
            if (rbA.IsChecked == true)
                bloodType = "A";
            if (rbB.IsChecked == true)
                bloodType = "B";
            if (rbAB.IsChecked == true)
                bloodType = "AB";
            if (rbO.IsChecked == true)
                bloodType = "O";

           
            Ward selectedWard = lbxWards.SelectedItem as Ward;
            if (selectedWard != null)
            {
                //Check wards patients number and wards capacity
                if (selectedWard.Capacity > selectedWard.Patients.Count)
                {
                    Patient p = new Patient(name, dob, bloodType);
                    selectedWard.Patients.Add(p);


                    patients.Clear();
                    foreach (Patient patient in selectedWard.Patients)
                    {
                        patients.Add(patient);
                    }
                    lbxPatients.ItemsSource = patients;

                    MessageBox.Show("You successfully added a patient");
                }
                else MessageBox.Show("You can't add any patients to this ward, It's full");
            }
            txtboxName.Clear();          
        }


        //save objects to Json
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
                // get string of objects Json formatted
            string json = JsonConvert.SerializeObject(wards, Formatting.Indented);

                //save that to file
            using (StreamWriter sw = new StreamWriter(@"c:\temp\wards.json"))
            {
                sw.Write(json);
            }
            MessageBox.Show("Save completed");

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
                wards = JsonConvert.DeserializeObject<ObservableCollection<Ward>>(json);
            }
            //refresh  the display
            lbxWards.ItemsSource = wards;

        }


        //display name and blood type image when a patient is selected
        private void lbxPatients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Patient selectedPatient = lbxPatients.SelectedItem as Patient;

            if (selectedPatient != null)
            {
                imgBlood.Source = new BitmapImage(new Uri($"\\Images\\{selectedPatient.BloodType}.png", UriKind.Relative));
                TxtBoxPatientName.Text = selectedPatient.Name;  
            }        
        }

        //display patients in ward
        private void lbxWards_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Ward selectedWard = lbxWards.SelectedItem as Ward;  

            if (selectedWard != null)
            {
                patients.Clear();   
                foreach (Patient patient in selectedWard.Patients) 
                { 
                    patients.Add(patient);
                }
                lbxPatients.ItemsSource = patients;    
            }
        }

        //method to make btnAddWard active
        private void tbxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbxName.Text.Length > 0)
            {
                btnAddWard.IsEnabled = true;
            }
            else { btnAddWard.IsEnabled = false;}
        }


        //method to make btnAddPatients active
        private void txtboxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            Check_Date();
        }

        //calling method to check the date
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Check_Date();
        }
        
        //method to avoid forgetting to pick date
        private void Check_Date()
        {
            bool dateSelected = DatePicker.SelectedDate.HasValue;
            if (txtboxName.Text.Length > 0 && dateSelected)
            {
                btnAddPatients.IsEnabled = true;
            }            
            else { btnAddPatients.IsEnabled = false; }
        }
    }
    }



