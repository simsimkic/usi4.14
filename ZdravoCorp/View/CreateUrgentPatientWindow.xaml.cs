using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ZdravoCorp.Controller;
using ZdravoCorp.Model;

namespace ZdravoCorp.View;

public partial class CreateUrgentPatientWindow
{
    private PatientController _controller;
    
    public CreateUrgentPatientWindow(PatientController controller)
    {
        InitializeComponent();

        _controller = controller;
    }

    private void AddBtn_OnClick(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(NameTextBox.Text) || string.IsNullOrEmpty(SurnameTextBox.Text) ||
            DateOfBirthPicker.SelectedDate == null || string.IsNullOrEmpty(HeightTextBox.Text) ||
            string.IsNullOrEmpty(WeightTextBox.Text) || string.IsNullOrEmpty(PastConditionsTextBox.Text) ||
            string.IsNullOrEmpty(AllergiesTextBox.Text))
        {
            MessageBox.Show("You must fill out all the fields before adding the patient.");
        }
        else
        {
            _controller.Create(new Patient(NameTextBox.Text, SurnameTextBox.Text, DateOnly.FromDateTime(DateOfBirthPicker.SelectedDate.Value), new MedicalRecord(float.Parse(HeightTextBox.Text), float.Parse(WeightTextBox.Text), PastConditionsTextBox.Text.Split(',').ToList(), AllergiesTextBox.Text.Split(',').ToList(), new List<MedicalReport>())));
            Close();
        }
    }

    private void CancelBtn_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}