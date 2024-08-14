using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        private bool isEditing = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ReassignOrderNumbers()
        {
            // Count numbers
            for (int i = 0; i < patients.Count; i++)
            {
                patients[i].OrderNumber = i + 1; // count from 1
            }
        }

        private void ReassignOrderNumbersForWards()
        {
            // Count numbers
            for (int i = 0; i < wards.Count; i++)
            {
                wards[i].OrderNumber = i + 1; // count from 1
            }
        }

        //display capacity
        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //btnCancel.IsEnabled = true;
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
            //TxtboxWard.Text = $"Wards List ({wards.Count})";
            ReassignOrderNumbersForWards();

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
            string name = tbxWardName.Text;
            int capacity = Convert.ToInt32(slider.Value);

            //create ward object with info
            Ward newWard = new Ward(name, capacity);

            //add to collection
            wards.Add(newWard);
            lbxWards.ItemsSource = wards;
            ReassignOrderNumbersForWards();
            TxtboxWard.Text = $"Wards List ({wards.Count})";
            tbxWardName.Clear();
            slider.Value = slider.Minimum;
            MessageBox.Show("You successfully added a ward");
            btnSave.IsEnabled = true;
        }

        //method of adding patients
        private void btnAddPatients_Click(object sender, RoutedEventArgs e)
        {
            if (lbxWards.SelectedItem == null)
            {
                MessageBox.Show("Please pick out the ward.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                //read info from screen
                string name = txtPatientName.Text;

                //Reading date from screen
                DateTime dob = DatePicker.SelectedDate.Value;

                //Date validation
                if (DatePicker.SelectedDate.HasValue && DatePicker.SelectedDate > DateTime.Today)
                {
                    MessageBox.Show("Date of birth can not be in future, choose correct date, please", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    DatePicker.SelectedDate = DateTime.Today;
                }

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

                        ReassignOrderNumbers();

                        MessageBox.Show("You successfully added a patient");
                        btnSave.IsEnabled = true;
                    }
                    else MessageBox.Show("You can't add any patients to this ward, It's full");

                    txtPatientName.Clear();
                    DatePicker.SelectedDate = null;
                    rbA.IsChecked = true;
                }
            }
        }

        //save objects to Json
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //// get string of objects Json formatted
            //string json = JsonConvert.SerializeObject(wards, Formatting.Indented);

            ////save that to file
            //using (StreamWriter sw = new StreamWriter(@"c:\temp\wards.json"))
            //{
            //    sw.Write(json);
            //}
            //MessageBox.Show("Save completed");
            //btnCancel.IsEnabled = false;
            //btnSave.IsEnabled = false;

            // Update the selected patient's data if in edit mode
            var selectedPatient = lbxPatients.Tag as Patient;
            if (selectedPatient != null)
            {
                selectedPatient.Name = txtPatientName.Text;
                selectedPatient.DOB = DatePicker.SelectedDate.Value;
                selectedPatient.BloodType = (rbA.IsChecked == true) ? "A" :
                                            (rbB.IsChecked == true) ? "B" :
                                            (rbAB.IsChecked == true) ? "AB" : "O";

                // Clear the input fields after saving
                txtPatientName.Clear();
                DatePicker.SelectedDate = null;
                rbA.IsChecked = true;
                lbxPatients.Tag = null;
                lbxPatients.Items.Refresh();
            }

            // Save all data to the JSON file
            string json = JsonConvert.SerializeObject(wards, Formatting.Indented);
            using (StreamWriter sw = new StreamWriter(@"c:\temp\wards.json"))
            {
                sw.Write(json);
            }
            MessageBox.Show("Save completed");

            // Reassign order numbers
            ReassignOrderNumbers();

            // Disable the save button after saving
            btnSave.IsEnabled = false;
            btnCancel.IsEnabled = false;
            isEditing = false;
        }

        //loads json file
        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            Ward previouslySelectedWard = lbxWards.SelectedItem as Ward;
            //connect to a file 
            using (StreamReader sr = new StreamReader(@"c:\temp\wards.json"))
            {
                //read text
                string json = sr.ReadToEnd();

                //convert from json to objects 
                wards = JsonConvert.DeserializeObject<ObservableCollection<Ward>>(json);
            }

            lbxWards.ItemsSource = wards;

            if (previouslySelectedWard != null && wards.Any(w => w.Name == previouslySelectedWard.Name))
            {
                //  Select the ward that was selected
                Ward selectedWard = wards.First(w => w.Name == previouslySelectedWard.Name);
                lbxWards.SelectedItem = selectedWard;

                // Display the patients from the selected ward
                patients.Clear();
                foreach (Patient patient in selectedWard.Patients)
                {
                    patients.Add(patient);
                }
                lbxPatients.ItemsSource = patients;
                //lbxPatients.Items.Refresh();
            }
            else if (wards.Count > 0)
            {
                // If the ward hadn't been selected, select the 1st ward
                lbxWards.SelectedIndex = 0;
                Ward selectedWard = wards[0];

                patients.Clear();
                foreach (Patient patient in selectedWard.Patients)
                {
                    patients.Add(patient);
                }
                lbxPatients.ItemsSource = patients;
                //lbxPatients.Items.Refresh();
            }
            //reasign ordinals numbers
            ReassignOrderNumbers();
            btnLoad.IsEnabled = false;
        }

        //display name and blood type image when a patient is selected
        private void lbxPatients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Patient selectedPatient = lbxPatients.SelectedItem as Patient;

            if (selectedPatient != null)
            {
                btnCancel.IsEnabled = true;
                imgBlood.Source = new BitmapImage(new Uri($"\\Images\\{selectedPatient.BloodType}.png", UriKind.Relative));
                TxtBoxPatientName.Text = selectedPatient.Name;
                btnDelete.IsEnabled = true;
                btnEditPatient.IsEnabled = true;
            }
            else
            {
                btnDelete.IsEnabled = false;
                btnEditPatient.IsEnabled = false;
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

                ReassignOrderNumbers();
            }
            //btnCancel.IsEnabled = true;
        }

        //method to make btnAddWard active
        private void tbxWardName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbxWardName.Text.Length > 0)
            {
                btnAddWard.IsEnabled = true;
                btnCancel.IsEnabled = true;
            }
            else
            {
                btnAddWard.IsEnabled = false; btnCancel.IsEnabled = false;
            }
        }

        //method to make btnAddPatients active
        private void txtPatientName_TextChanged(object sender, TextChangedEventArgs e)
        {
            Check_Date();
            EnableAddPatientButton();
            //EnableSaveButton();
            if (!isEditing && txtPatientName.Text.Length > 0 && DatePicker.SelectedDate.HasValue)
            {
                btnAddPatients.IsEnabled = true;
                btnCancel.IsEnabled = true;
            }
            else
            {
                btnAddPatients.IsEnabled = false;
                btnCancel.IsEnabled = false;
            }
        }

        //calling method to check the date
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Check_Date();
            if (!isEditing && txtPatientName.Text.Length > 0 && DatePicker.SelectedDate.HasValue)
            {
                btnAddPatients.IsEnabled = true;
            }
            else
            {
                btnAddPatients.IsEnabled = false;
            }
        }

        //method to avoid forgetting to select date
        private void Check_Date()
        {
            bool dateSelected = DatePicker.SelectedDate.HasValue;

            if (txtPatientName.Text.Length > 0 && dateSelected)
            {
                btnAddPatients.IsEnabled = true;
            }
            else
            {
                btnAddPatients.IsEnabled = false;
            }
            btnCancel.IsEnabled = true;
        }

        private void DeletePatient(Patient patient, Ward ward)
        {
            ward.Patients.Remove(patient);
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            {
                var selectedPatient = lbxPatients.SelectedItem as Patient;
                var selectedWard = lbxWards.SelectedItem as Ward;

                DeletePatient(selectedPatient, selectedWard);
                patients.Clear();
                btnLoad.IsEnabled = true;

                foreach (var patient in selectedWard.Patients)
                {
                    patients.Add(patient);
                }
                //lbxPatients.ItemsSource = patients;
                //lbxPatients.Items.Refresh();
                btnSave.IsEnabled = true;

                ReassignOrderNumbers();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            // Clear all inputs
            tbxWardName.Clear();
            txtPatientName.Clear();
            DatePicker.SelectedDate = null;
            rbA.IsChecked = true;
            btnCancel.IsEnabled = false;
            //btnSave.IsEnabled = false;

            // Clear ListBox
            lbxPatients.SelectedIndex = -1;
            lbxPatients.ItemsSource = null;

            // Clear inputs and images
            TxtBoxPatientName.Text = string.Empty;
            TxtBoxSliderCapacity.Text = string.Empty;
            slider.Value = slider.Minimum;
            imgBlood.Source = null;
        }

        private void RadioButton_CheckedChanged(object sender, RoutedEventArgs e)
        {
            //Check selection of radiobuttons
            if (rbA.IsChecked == true)
            {
                btnCancel.IsEnabled = false;
            }
            else
            {
                btnCancel.IsEnabled = true;
            }
        }
        private Patient originalPatientData;
        private void btnEditPatient_Click(object sender, RoutedEventArgs e)
        {
            btnEditPatient.IsEnabled = false;
            var selectedPatient = lbxPatients.SelectedItem as Patient;
            if (selectedPatient != null)
            {
                btnAddPatients.IsEnabled = false;
                // Store the original patient data
                originalPatientData = new Patient
                {
                    Name = selectedPatient.Name,
                    DOB = selectedPatient.DOB,
                    BloodType = selectedPatient.BloodType
                };

                // Populate the input fields with the selected patient's data
                txtPatientName.Text = selectedPatient.Name;
                DatePicker.SelectedDate = selectedPatient.DOB;
                switch (selectedPatient.BloodType)
                {
                    case "A":
                        rbA.IsChecked = true;
                        break;
                    case "B":
                        rbB.IsChecked = true;
                        break;
                    case "AB":
                        rbAB.IsChecked = true;
                        break;
                    case "O":
                        rbO.IsChecked = true;
                        break;
                }
                // Set the editing flag
                isEditing = true;
                // Store the selected patient for later reference
                lbxPatients.Tag = selectedPatient;

                // Disable the add button initially
                btnAddPatients.IsEnabled = false;

                // Ensure the save button is enabled since we are in edit mode
                btnSave.IsEnabled = true;
            }
        }
        private void EnableAddPatientButton()
        {
            // Enable Add Patient button if not editing and all input fields are valid
            bool isInputValid = txtPatientName.Text.Length > 0 &&
                                DatePicker.SelectedDate.HasValue &&
                                (rbA.IsChecked == true || rbB.IsChecked == true || rbAB.IsChecked == true || rbO.IsChecked == true);

            if (!isEditing && isInputValid)
            {
                btnAddPatients.IsEnabled = true;
            }
            else
            {
                btnAddPatients.IsEnabled = false;
            }
        }
    }
}




