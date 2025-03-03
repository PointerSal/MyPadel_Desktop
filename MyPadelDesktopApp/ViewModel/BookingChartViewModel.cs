using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyPadelDesktopApp.Helpers;
using MyPadelDesktopApp.Models;
using MyPadelDesktopApp.Services.DesktopBookingServices;
using MyPadelDesktopApp.Services.DesktopClientServices;
using MyPadelDesktopApp.Services.DesktopCourtSportsServices;
using MyPadelDesktopApp.ViewModel.ViewBaseModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyPadelDesktopApp.ViewModel
{
    public partial class BookingChartViewModel : BaseViewModel
    {
        #region Services

        private readonly IDesktopBookingService _desktopBookingService;
        private readonly IDesktopCourtSportsService _desktopCourtSportsService;
        private readonly IDesktopClientService _desktopClientService;

        #endregion

        #region Properties

        [ObservableProperty]
        private DateTime _currentSelectedDate = DateTime.Now;

        [ObservableProperty]
        private BookedData _selectedBooking;

        [ObservableProperty]
        private BookedData _newBooking = new();

        [ObservableProperty]
        private bool _isAlreadyExist = false;

        public ObservableCollection<Booking> AllCourts = null;

        [ObservableProperty]
        private ObservableCollection<string> _fieldList = new();
        
        [ObservableProperty]
        private ObservableCollection<string> _usersEmailList = new();

        [ObservableProperty]
        private ObservableCollection<string> _fieldTypeList = new();

        [ObservableProperty]
        private string _selectedField;

        [ObservableProperty]
        private string _selectedFieldType;

        [ObservableProperty]
        private string _selectedPaymentMethod;

        [ObservableProperty]
        private string _fieldType;
        
        [ObservableProperty]
        private string _email;
        
        [ObservableProperty]
        private string _emailError;

        [ObservableProperty]
        private bool _hasEmailError;

        [ObservableProperty]
        private DateTime _selectedDate = DateTime.Today;

        [ObservableProperty]
        private TimeSpan _selectedTime = TimeSpan.Zero;

        [ObservableProperty]
        private int _duration;

        [ObservableProperty]
        private string _durationError;

        [ObservableProperty]
        private bool _hasDurationError;

        [ObservableProperty]
        private double _amount;

        [ObservableProperty]
        private string _amountError;

        [ObservableProperty]
        private bool _hasAmountError;

        [ObservableProperty]
        private string _fieldListError;

        [ObservableProperty]
        private bool _hasFieldListError;

        [ObservableProperty]
        private string fieldTypeListError;

        [ObservableProperty]
        private bool hasFieldTypeListError;

        [ObservableProperty]
        private string dateError;

        [ObservableProperty]
        private bool hasDateError;

        [ObservableProperty]
        private string paymentError;

        [ObservableProperty]
        private bool hasPaymentError;

        [ObservableProperty]
        private string timeError;

        [ObservableProperty]
        private bool hasTimeError;

        #endregion

        #region Commands

        [RelayCommand]
        public async Task<bool> AddBooking()
        {
            try
            {
                IsBusy = true;
                Validate();

                if (!HasFieldListError && !HasFieldTypeListError && !HasDurationError && !HasAmountError && !HasPaymentError && !HasEmailError)
                {
                    var NewTime = SelectedTime.ToString(@"hh\:mm"); 
                    var NewDateString = SelectedDate.ToString("yyyy-MM-dd") + $"T{NewTime}:00.000Z";
                    var FieldId = AllCourts.FirstOrDefault(e => e.fieldName.Equals(FieldType, StringComparison.OrdinalIgnoreCase))?.fieldType;
                    var booking = new
                    {
                        sportType = NewBooking.fieldName,
                        date = NewDateString,
                        NewBooking.duration,
                        timeSlot = NewTime != null ? NewTime : "00:00",
                        fieldId = FieldId ?? "UnknownField",
                        NewBooking.paymentMethod,
                        NewBooking.amount,
                        email = Email ?? "unknown@example.com" 
                    };
                    var response = await _desktopBookingService.AddBookings(booking);
                    if (response != null && response.code != null && response.code.Equals("0000"))
                    {
                        IsBusy = false;
                        return true;
                    }
                    else if (response != null && response.code != null)
                        await Shell.Current.DisplayAlert("Errore", response.message, "OK");
                    else
                        await Shell.Current.DisplayAlert("Errore", "Qualcosa è andato storto", "OK");
                }
            }
            catch (Exception ex) { }
            IsBusy = false;
            return false;
        }

        [RelayCommand]
        public async Task<bool> CancelBooking()
        {
            try
            {
                IsBusy = true;
                var response = await _desktopBookingService.CancelBooking(SelectedBooking.id ?? 0);
                if (response != null && response.code != null && response.code.Equals("0000"))
                {
                    IsBusy = true;
                    await Shell.Current.DisplayAlert("Successo", "La prenotazione è stata cancellata con successo", "OK");
                    return true;
                }
                else if (response != null && response.code != null)
                    await Shell.Current.DisplayAlert("Errore", response.message, "OK");
                else
                    await Shell.Current.DisplayAlert("Errore", "Qualcosa è andato storto", "OK");
            }
            catch { }
            IsBusy = false;
            return false;
        }

        [RelayCommand]
        public async Task FieldsConfiguration()
        {
            try
            {
                await Shell.Current.GoToAsync("FieldComfigurationsPage");
            }
            catch { }
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

        public async void GetAllUsers()
        {
            try
            {
                var response = await _desktopClientService.GetAllUsers();
                if (response != null && response.code != null && response.code.Equals("0000"))
                    UsersEmailList = new ObservableCollection<string>((JsonSerializer.Deserialize<List<UsersData>>(response.data.ToString())).Select(s=>s.email).ToList());
                else if (response != null && response.code != null)
                    await Shell.Current.DisplayAlert("Errore", response.message, "OK");
                else
                    await Shell.Current.DisplayAlert("Errore", "Qualcosa è andato storto", "OK");
            }
            catch { }
        }
        public async Task GetAllCourts()
        {
            try
            {
                IsBusy = true;
                IsEmpty = false;
                var response = await _desktopCourtSportsService.CourtSports();
                if (response != null && response.code != null && response.code.Equals("0000"))
                    AllCourts = new ObservableCollection<Booking>(JsonSerializer.Deserialize<List<Booking>>(response.data.ToString()));
                else if (response != null && response.code != null)
                    await Shell.Current.DisplayAlert("Errore", response.message, "OK");
                else
                    await Shell.Current.DisplayAlert("Errore", "Qualcosa è andato storto", "OK");
            }
            catch { }
            IsBusy = false;
        }

        public async Task<List<BookedData>> GetAllBookingsByDate()
        {
            try
            {
                IsBusy = true;
                IsEmpty = false;
                var response = await _desktopBookingService.GetBookings(CurrentSelectedDate.ToString("yyyy-MM-d"));
                if (response != null && response.code != null && response.code.Equals("0000"))
                {
                    IsBusy = false;
                    return JsonSerializer.Deserialize<List<BookedData>>(response.data.ToString());
                }
                else if (response != null && response.code != null)
                    await Shell.Current.DisplayAlert("Errore", response.message, "OK");
                else
                    await Shell.Current.DisplayAlert("Errore", "Qualcosa è andato storto", "OK");
            }
            catch(Exception ex) { }
            IsBusy = false;
            return null;
        }
        public void AddFieldTypeList()
        {
            try
            {
                FieldTypeList = new ObservableCollection<string>(AllCourts.Where(e => e.sportsName.Equals(NewBooking.fieldName)).Select(s => s.fieldName).ToList());
            }
            catch { }
        }

        public void Validate()
        {
            (HasFieldListError, FieldListError) = FieldValidations.IsFieldNotEmpty(NewBooking.fieldName, "Il nome del campo è obbligatorio.");
            (HasFieldTypeListError, FieldTypeListError) = FieldValidations.IsFieldNotEmpty(FieldType, "Il tipo di campo è obbligatorio.");
            (HasDurationError, DurationError) = FieldValidations.IsNumericAndPositive(NewBooking.duration.ToString(), "La durata deve essere un numero positivo.");
            (HasAmountError, AmountError) = FieldValidations.IsNumericAndPositive(NewBooking.amount.ToString(), "La quantità deve essere un numero positivo.");
            (HasPaymentError, PaymentError) = FieldValidations.IsFieldNotEmpty(NewBooking.paymentMethod, "Il metodo di pagamento è obbligatorio.");
            (HasEmailError, EmailError) = FieldValidations.IsEmailValid(Email);

            if (SelectedTime == TimeSpan.Zero)
            {
                HasTimeError = true;
                TimeError = "L'orario della prenotazione non può essere zero.";
            }
            else
            {
                HasTimeError = false;
                TimeError = string.Empty;
            }
        }

        #endregion

        #region Constructor
        public BookingChartViewModel(IDesktopBookingService desktopBookingService, IDesktopCourtSportsService desktopCourtSportsService, IDesktopClientService desktopClientService)
        {
            _desktopCourtSportsService = desktopCourtSportsService;
            _desktopBookingService = desktopBookingService;
            _desktopClientService = desktopClientService;
        }

        #endregion
    }
}