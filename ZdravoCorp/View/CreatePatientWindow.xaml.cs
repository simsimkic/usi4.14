using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ZdravoCorp.Controller;
using ZdravoCorp.Model;
using System.Text.RegularExpressions;

namespace ZdravoCorp.View;

public partial class CreatePatientWindow : Window
{
    private PatientController _controller;
    
    public CreatePatientWindow(PatientController controller)
    {
        InitializeComponent();
        DataContext = this;

        _controller = controller;
    }

    private void CancelBtn_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void AddBtn_OnClick(object sender, RoutedEventArgs e)
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
            Patient patient = new Patient(EmailTextBox.Text, PasswordTextBox.Text, NameTextBox.Text, SurnameTextBox.Text, AddressTextBox.Text, DateOnly.FromDateTime(DateOfBirthPicker.SelectedDate.Value), new MedicalRecord(float.Parse(HeightTextBox.Text), float.Parse(WeightTextBox.Text), PasswordTextBox.Text.Split(',').ToList(), AllergiesTextBox.Text.Split(',').ToList(), new List<MedicalReport>()));
            _controller.Create(patient);
            Close();
        }
    }
}