using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.VisualBasic.FileIO;
using Mopups.Services;
using MyPadelDesktopApp.Helpers;
using MyPadelDesktopApp.Helpers.References;
using MyPadelDesktopApp.Models;
using MyPadelDesktopApp.Models.Responses;
using MyPadelDesktopApp.Services.DesktopCourtSportsServices;
using MyPadelDesktopApp.ViewModel.ViewBaseModel;
using MyPadelDesktopApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyPadelDesktopApp.ViewModel
{
    public partial class ReservationsViewModel : BaseViewModel, IQueryAttributable
    {
        #region Services

        private readonly IDesktopCourtSportsService _desktopCourtSportsService;

        #endregion

        #region Properties

        [ObservableProperty]
        public Booking _courts = new();

        [ObservableProperty]
        private bool _hasNameError;

        [ObservableProperty]
        private string _nameError;

        [ObservableProperty]
        private bool _hasFieldTypeError;

        [ObservableProperty]
        private string _fieldTypeError;

        [ObservableProperty]
        private bool _hasTerrainTypeError;

        [ObservableProperty]
        private string _terrainTypeError;

        [ObservableProperty]
        private bool _hasFieldCapacityError;

        [ObservableProperty]
        private string _fieldCapacityError;

        [ObservableProperty]
        private bool _hasDuration1Error;

        [ObservableProperty]
        private string _duration1Error;

        [ObservableProperty]
        private bool _hasDuration2Error;

        [ObservableProperty]
        private string _duration2Error;

        [ObservableProperty]
        private bool _hasDuration3Error;

        [ObservableProperty]
        private string _duration3Error;

        [ObservableProperty]
        private bool _hasPrice1Error;

        [ObservableProperty]
        private string _price1Error;

        [ObservableProperty]
        private bool _hasPrice2Error;

        [ObservableProperty]
        private string _price2Error;

        [ObservableProperty]
        private bool _hasPrice3Error;

        [ObservableProperty]
        private string _price3Error;

        [ObservableProperty]
        private string _openingHours = "Monday-Sunday 1:00-23:00";

        [ObservableProperty]
        private bool _isEdit;

        [ObservableProperty]
        private bool _canBooked;

        private Booking SelectedBooking;
        private ObservableCollection<Booking> CourtList = new ObservableCollection<Booking>();

        #endregion

        #region Commands

        [RelayCommand]
        public async Task DeleteField()
        {
            bool isConfirmed = await Shell.Current.DisplayAlert("Conferma", "Sei sicuro di voler eliminare questo tribunale?", "Sì", "No");

            if (isConfirmed)
            {
                try
                {
                    IsBusy = true;
                    var response = await _desktopCourtSportsService.DeleteCourt(SelectedBooking.id ?? 0);
                    if (response != null && response.code != null && response.code.Equals("0000"))
                    {
                        Utils.IsBookingChartUpdated = true;
                        CourtList.Remove(SelectedBooking);
                        await Shell.Current.GoToAsync("..");
                    }
                    else if (response != null && response.code != null)
                        await Shell.Current.DisplayAlert("Errore", response.message, "OK");
                    else
                        await Shell.Current.DisplayAlert("Errore", "Qualcosa è andato storto", "OK");
                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlert("Errore", $"Qualcosa è andato storto: {ex.Message}", "OK");
                }

                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task SaveField()
        {
            try
            {
                Validate();
                if (!HasNameError && !HasFieldTypeError && !HasTerrainTypeError && !HasFieldCapacityError && !HasDuration1Error && !HasDuration2Error && !HasDuration3Error && !HasPrice1Error && !HasPrice2Error && !HasPrice3Error)
                {
                    object Court = null;
                    if (IsEdit)
                        Court = new { Courts.id, Courts.sportsName, Courts.fieldName, Courts.fieldType, Courts.terrainType, Courts.fieldCapacity, Courts.slot1Price, Courts.slot2Price, Courts.slot3Price, Courts.slot1Duration, Courts.slot2Duration, Courts.slot3Duration, canBeBooked = CanBooked, openingHours = OpeningHours };
                    else
                    {
                        var existingNumbers = CourtList
                        .Where(e => e.sportsName.Equals(Courts.sportsName))
                        .Select(e => int.Parse(e.fieldType))
                        .OrderBy(n => n)
                        .ToList();

                        int nextFieldType = 1;
                        foreach (var num in existingNumbers)
                        {
                            if (num != nextFieldType) break;
                            nextFieldType++;
                        }
                        var data = CourtList.Where(e => e.sportsName.Equals(Courts.sportsName)).ToList();
                        Court = new { Courts.sportsName, Courts.fieldName, Courts.terrainType, fieldType = nextFieldType.ToString(), Courts.fieldCapacity, Courts.slot1Price, Courts.slot2Price, Courts.slot3Price, Courts.slot1Duration, Courts.slot2Duration, Courts.slot3Duration, canBeBooked = CanBooked, openingHours = OpeningHours };
                    }

                    IsBusy = true;
                    GeneralResponse response = null;
                    if (IsEdit)
                        response = await _desktopCourtSportsService.EditCourt(Court);
                    else
                        response = await _desktopCourtSportsService.AddCourt(Court);
                    if (response != null && response.code != null && response.code.Equals("0000"))
                    {
                        Utils.IsBookingChartUpdated = true;
                        if (IsEdit)
                        {
                            var EditedCourt = JsonSerializer.Deserialize<Booking>(response.data.ToString());
                            var index = CourtList.IndexOf(SelectedBooking);
                            if (index != -1)
                                CourtList[index] = EditedCourt;
                        }
                        else
                        {
                            var AddedCourt = JsonSerializer.Deserialize<Booking>(response.data.ToString());
                            CourtList.Add(AddedCourt);
                        }

                        await Shell.Current.GoToAsync("..");
                    }
                    else if (response != null && response.code != null)
                        await Shell.Current.DisplayAlert("Errore", response.message, "OK");
                    else
                        await Shell.Current.DisplayAlert("Errore", "Qualcosa è andato storto", "OK");
                }
            }
            catch { }
            IsBusy = false;
        }
        
        [RelayCommand]
        public async Task OpenOpeningHours()
        {
            try
            {
                await MopupService.Instance.PushAsync(new OpeningHoursPopup(OpeningHours));
            }
            catch { }
        }

        [RelayCommand]
        private void BookingClicked()
        {
            CanBooked = !CanBooked;
        }

        [RelayCommand]
        public async Task Back()
        {
            try
            {
                await Shell.Current.GoToAsync("..");
            }
            catch { }
        }

        #endregion

        #region Methods

        public void Validate()
        {
            (HasNameError, NameError) = FieldValidations.IsFieldNotEmpty(Courts.sportsName, "Il nome del campo è obbligatorio.");
            (HasFieldTypeError, FieldTypeError) = FieldValidations.IsFieldNotEmpty(Courts.fieldName, "Il tipo di campo è obbligatorio.");
            (HasTerrainTypeError, TerrainTypeError) = FieldValidations.IsFieldNotEmpty(Courts.terrainType, "Il tipo di terreno è obbligatorio.");
            (HasFieldCapacityError, FieldCapacityError) = FieldValidations.IsNumericAndPositive(Courts.fieldCapacity.ToString(), "La capacità del campo deve essere un numero positivo e maggiore di zero.");

            (HasDuration1Error, Duration1Error) = FieldValidations.IsNumericAndPositive(Courts.slot1Duration.ToString(), "La durata dello slot 1 deve essere un numero positivo e maggiore di zero.");
            (HasDuration2Error, Duration2Error) = FieldValidations.IsNumericAndPositive(Courts.slot2Duration.ToString(), "La durata dello slot 2 deve essere un numero positivo e maggiore di zero.");
            (HasDuration3Error, Duration3Error) = FieldValidations.IsNumericAndPositive(Courts.slot3Duration.ToString(), "La durata dello slot 3 deve essere un numero positivo e maggiore di zero.");

            (HasPrice1Error, Price1Error) = FieldValidations.IsNumericAndPositive(Courts.slot1Price.ToString(), "Il prezzo dello slot 1 deve essere un numero positivo e maggiore di zero.");
            (HasPrice2Error, Price2Error) = FieldValidations.IsNumericAndPositive(Courts.slot2Price.ToString(), "Il prezzo dello slot 2 deve essere un numero positivo e maggiore di zero.");
            (HasPrice3Error, Price3Error) = FieldValidations.IsNumericAndPositive(Courts.slot3Price.ToString(), "Il prezzo dello slot 3 deve essere un numero positivo e maggiore di zero.");
        }
        private void AssignValues()
        {
            try
            {
                Courts = SelectedBooking;
                CanBooked = Courts.canBeBooked;
                OpeningHours = Courts.openingHours;
            }
            catch { }
        }

        #endregion

        #region InterfaceImplementatiom
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            try
            {
                if (query.Any())
                {
                    var Type = (string)query["CourtType"];
                    CourtList = (ObservableCollection<Booking>)query["CourtList"];
                    IsEdit = Type.Equals("Edit");
                    if(IsEdit)
                    {
                        SelectedBooking = (Booking)query["SelectedBookingCourt"];
                        AssignValues();
                    }
                    query.Clear();
                }
            }
            catch { }
        }

        #endregion

        #region Constructor
        public ReservationsViewModel(IDesktopCourtSportsService desktopCourtSportsService)
        {
            _desktopCourtSportsService = desktopCourtSportsService;
            WeakReferenceMessenger.Default.Register<OpeningHoursMessage>(this, (recipient, message) =>
            {
                try
                {
                    OpeningHours = message.Value;
                }
                catch { }
            });
        }

        #endregion
    }
}