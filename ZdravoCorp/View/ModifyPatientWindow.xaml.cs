using System;
using System.Linq;
using System.Windows;
using ZdravoCorp.Controller;
using ZdravoCorp.Model;
using System.Text.RegularExpressions;

namespace ZdravoCorp.View;

public partial class ModifyPatientWindow : Window
{
    private PatientController _controller;
    public Patient Patient { get; set; }
    
    public ModifyPatientWindow(Patient patient, PatientController controller)
    {
        InitializeComponent();
        DataContext = this;
        Patient = patient;

        _controller = controller;

        EmailTextBox.Text = Patient.Email;
        PasswordTextBox.Text = Patient.Password;
        NameTextBox.Text = Patient.Name;
        SurnameTextBox.Text = Patient.Surname;
        DateOfBirthPicker.SelectedDate = Patient.DateOfBirth.ToDateTime(TimeOnly.MinValue);
        AddressTextBox.Text = Patient.Address;
        HeightTextBox.Text = Patient.MedicalRecord.Height.ToString();
        WeightTextBox.Text = Patient.MedicalRecord.Weight.ToString();
        PastConditionsTextBox.Text = string.Join(',', Patient.MedicalRecord.PastConditions);
        AllergiesTextBox.Text = string.Join(',', Patient.MedicalRecord.Allergies);
    }

    private void ModifyBtn_OnClick(object sender, RoutedEventArgs e)
    {
        Regex EmailRegex = new Regex("^\\S+@\\S+\\.\\S+$");
        if (string.IsNullOrEmpty(EmailTextBox.Text) || string.IsNullOrEmpty(PasswordTextBox.Text) ||
            string.IsNullOrEmpty(NameTextBox.Text) || string.IsNullOrEmpty(SurnameTextBox.Text) ||
            DateOfBirthPicker.SelectedDate == null || string.IsNullOrEmpty(AddressTextBox.Text) ||
            string.IsNullOrEmpty(HeightTextBox.Text) || string.IsNullOrEmpty(WeightTextBox.Text) ||
            string.IsNullOrEmpty(PastConditionsTextBox.Text) || string.IsNullOrEmpty(AllergiesTextBox.Text))
        {
            MessageBox.Show("You must fill out all the fields before adding the patient.");
        }
        else if (!EmailRegex.IsMatch(EmailTextBox.Text))
        {
            MessageBox.Show("Email is not in the correct format.");
        }
        else
        {
            _controller.Delete(Patient);
            Patient.Email = EmailTextBox.Text;
            Patient.Password = PasswordTextBox.Text;
            Patient.Name = NameTextBox.Text;
            Patient.Surname = SurnameTextBox.Text;
            Patient.DateOfBirth = DateOnly.FromDateTime(DateOfBirthPicker.SelectedDate.Value);
            Patient.Address = AddressTextBox.Text;
            Patient.MedicalRecord.Height = float.Parse(HeightTextBox.Text);
            Patient.MedicalRecord.Weight = float.Parse(WeightTextBox.Text);
            Patient.MedicalRecord.PastConditions = PastConditionsTextBox.Text.Split(',').ToList();
            Patient.MedicalRecord.Allergies = AllergiesTextBox.Text.Split(',').ToList();
            _controller.Create(Patient);
            Close();
        }
    }

    private void CancelBtn_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}