using CommunityToolkit.Mvvm.Messaging;
using Mopups.Pages;
using Mopups.Services;
using MyPadelDesktopApp.Helpers.References;
using System;
using System.ComponentModel;

namespace MyPadelDesktopApp.Views;

public partial class OpeningHoursPopup : PopupPage
{
    public OpeningHoursPopup(string OpeningHours)
    {
        InitializeComponent();

        StartTimePicker.PropertyChanged += OnTimeChanged;
        EndTimePicker.PropertyChanged += OnTimeChanged;

        try
        {
            if (OpeningHours != null)
            {
                ChkLun.IsChecked = OpeningHours.Contains("Monday");
                ChkMar.IsChecked = OpeningHours.Contains("Tuesday");
                ChkMer.IsChecked = OpeningHours.Contains("Wednesday");
                ChkGio.IsChecked = OpeningHours.Contains("Thursday");
                ChkVen.IsChecked = OpeningHours.Contains("Friday");
                ChkSab.IsChecked = OpeningHours.Contains("Saturday");
                ChkDom.IsChecked = OpeningHours.Contains("Sunday");
                var StartEndTime = OpeningHours.Split(' ').LastOrDefault();
                StartTimePicker.Time = TimeSpan.Parse(StartEndTime.Split('-')[0]);
                EndTimePicker.Time = TimeSpan.Parse(StartEndTime.Split('-')[1]);
            }
        }
        catch { }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        ValidateTime();
        string NewOpeningHours = "";
        if(!ChkLun.IsChecked && !ChkDom.IsChecked && !ChkGio.IsChecked && !ChkMar.IsChecked && !ChkMer.IsChecked && !ChkSab.IsChecked && !ChkVen.IsChecked)
        {
            await Shell.Current.DisplayAlert("Selection required", "Atleast one day must be selected", "OK");
            return;
        }

        if (TimeErrorLabel.IsVisible)
            return;

        if (ChkLun.IsChecked)
            NewOpeningHours = NewOpeningHours + "Monday-";
        if (ChkMar.IsChecked)
            NewOpeningHours = NewOpeningHours + "Tuesday-";
        if (ChkMer.IsChecked)
            NewOpeningHours = NewOpeningHours + "Wednesday-";
        if (ChkGio.IsChecked)
            NewOpeningHours = NewOpeningHours + "Thursday-";
        if (ChkVen.IsChecked)
            NewOpeningHours = NewOpeningHours + "Friday-";
        if (ChkSab.IsChecked)
            NewOpeningHours = NewOpeningHours + "Saturday-";
        if (ChkDom.IsChecked)
            NewOpeningHours = NewOpeningHours + "Sunday-";

        var data = NewOpeningHours[NewOpeningHours.Length - 1];
        if(data.Equals('-'))
            NewOpeningHours = NewOpeningHours.Substring(0, NewOpeningHours.Length - 1);
        NewOpeningHours = string.Concat(NewOpeningHours, " ", StartTimePicker.Time.ToString(@"hh\:mm"), "-", EndTimePicker.Time.ToString(@"hh\:mm"));

        WeakReferenceMessenger.Default.Send(new OpeningHoursMessage(NewOpeningHours));
        await MopupService.Instance.PopAsync();
    }

    private void OnTimeChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(TimePicker.Time))
        {
            ValidateTime();
        }
    }

    private void ValidateTime()
    {
        TimeSpan startTime = StartTimePicker.Time;
        TimeSpan endTime = EndTimePicker.Time;

        if (startTime >= endTime)
        {
            TimeErrorLabel.Text = "End time must be after start time.";
            TimeErrorLabel.IsVisible = true;
        }
        else
        {
            TimeErrorLabel.IsVisible = false;
        }
    }
    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await MopupService.Instance.PopAsync();
    }
    private void OnCancelClicked(object sender, TappedEventArgs e)
    {

    }
}