using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Storage;
using MyPadelDesktopApp.Helpers;
using MyPadelDesktopApp.Models;
using MyPadelDesktopApp.Services.DesktopClientServices;
using MyPadelDesktopApp.ViewModel.ViewBaseModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyPadelDesktopApp.ViewModel
{
    public partial class AddressBookingViewModel : BaseViewModel, IQueryAttributable
    {
        #region Services

        private readonly IDesktopClientService _desktopClientService;

        #endregion

        #region Properties

        [ObservableProperty]
        public string _selectedItem;

        [ObservableProperty]
        public UsersData _usersData = new();

        [ObservableProperty]
        public Statistics _userStatistics = null;

        [ObservableProperty]
        public ObservableCollection<string> _itemList;

        [ObservableProperty]
        public ObservableCollection<string> _tagList;

        [ObservableProperty]
        public bool _isVerified;

        [ObservableProperty]
        private DateTime? _expiryDate;

        [ObservableProperty]
        private bool _hasFitCardError;

        [ObservableProperty]
        private string _fitCardError;

        [ObservableProperty]
        private DateTime _currentSelectedDate = DateTime.Now;

        [ObservableProperty]
        public ObservableCollection<History> _historyList = new ObservableCollection<History>();

        private ObservableCollection<UsersData> UserList = new ObservableCollection<UsersData>();

        #endregion

        #region Commands

        [RelayCommand]
        public async Task DeleteUser()
        {
            bool isConfirmed = await Shell.Current.DisplayAlert("Conferma", "Sei sicuro di voler eliminare questo utente?", "Sì", "No");
            if (isConfirmed)
            {
                try
                {
                    IsBusy = true;
                    var response = await _desktopClientService.DeleteUser(UsersData.email);
                    if (response != null && response.code != null && response.code.Equals("0000"))
                    {
                        UserList.Remove(UsersData);
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
        private async Task UpdateUser()
        {
            try
            {
                Validate();
                if (HasFitCardError)
                    return;

                IsBusy = true;
                var response = await _desktopClientService.UpdateClient(new { fitCardExpiryDate = ExpiryDate, isVerified = IsVerified, UsersData.email });
                if (response != null && response.code != null && response.code.Equals("0000"))
                {
                    UsersData.isVerified = IsVerified;
                    UsersData.fitCardExpiryDate = ExpiryDate;
                    await Shell.Current.DisplayAlert("Successo", "Informazioni aggiornate con successo", "OK");
                }
                else if (response != null && response.code != null)
                    await Shell.Current.DisplayAlert("Errore", response.message, "OK");
                else
                    await Shell.Current.DisplayAlert("Errore", "Qualcosa è andato storto", "OK");
            }
            catch { }
            IsBusy = false;
        }
        
        [RelayCommand]
        private void VerifiedClicked()
        {
            IsVerified = !IsVerified;
        }

        [RelayCommand]
        private async Task DownloadImage()
        {
            try
            {
                IsBusy = true;
                var response = await _desktopClientService.MedicalCertificate(UsersData.email);
                if (response != null && response.code != null && response.code.Equals("0000"))
                {
                    var base64String = JsonSerializer.Deserialize<CertificatePicc>(response.data.ToString());
                    string filePath = await ImageHelper.SaveBase64ImageAsync(base64String.medicalCertificatePath,"certificate_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png");
                    if (!string.IsNullOrEmpty(filePath))
                        ImageHelper.OpenImage(filePath);
                }
                else if (response != null && response.code != null)
                    await Shell.Current.DisplayAlert("Errore", response.message, "OK");
                else
                    await Shell.Current.DisplayAlert("Errore", "Qualcosa è andato storto", "OK");
            }
            catch { }
            IsBusy = false;
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

        public async void SelectedItemChanged(string SelectedTab)
        {
            if (SelectedTab.Equals("STORIA") && HistoryList.Count <= 0)
            {
                try
                {
                    IsBusy = true;
                    IsEmpty = false;
                    var response = await _desktopClientService.UserHistory(UsersData.email);
                    if (response != null && response.code != null && response.code.Equals("0000"))
                        HistoryList = new ObservableCollection<History>(JsonSerializer.Deserialize<List<History>>(response.data.ToString()));
                    else if (response != null && response.code != null)
                        await Shell.Current.DisplayAlert("Errore", response.message, "OK");
                    else
                        await Shell.Current.DisplayAlert("Errore", "Qualcosa è andato storto", "OK");
                }
                catch { }
                IsEmpty = HistoryList ==null || HistoryList.Count == 0;
                IsBusy = false;
            }
            else if (SelectedTab.Equals("STATISTICHE") && UserStatistics == null)
            {
                try
                {
                    IsBusy = true;
                    var response = await _desktopClientService.CustomerStatistics(UsersData.email);
                    if (response != null && response.code != null && response.code.Equals("0000"))
                        UserStatistics = JsonSerializer.Deserialize<Statistics>(response.data.ToString());
                    else if (response != null && response.code != null)
                        await Shell.Current.DisplayAlert("Errore", response.message, "OK");
                    else
                        await Shell.Current.DisplayAlert("Errore", "Qualcosa è andato storto", "OK");
                }
                catch { }
                IsBusy = false;
            }
        }
       

        #region InterfaceImplementatiom
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            try
            {
                if(query.Any())
                {
                    UsersData = (UsersData)query["SelectedUser"];
                    UserList = (ObservableCollection<UsersData>)query["UserList"];
                    IsVerified = UsersData.isVerified == true;
                    ExpiryDate = UsersData.fitCardExpiryDate;
                    query.Clear();
                }
            }
            catch { }
        }

        #endregion

        #endregion

        #region Constructor
        public AddressBookingViewModel(IDesktopClientService desktopClientService)
        {
            _desktopClientService = desktopClientService;
            ItemList = new ObservableCollection<string> { "DATI PERSONALI", "STORIA", "STATISTICHE" };
            SelectedItem = ItemList.FirstOrDefault();
            TagList = new ObservableCollection<string> { "1", "2", "3" };
        }

        #endregion

        #region Validations

        public void Validate()
        {
            try
            {
                if (ExpiryDate == null)
                {
                    HasFitCardError = true;
                    FitCardError = "È richiesta la data di scadenza della tessera FIT.";
                    return;
                }
                (HasFitCardError, FitCardError) = FieldValidations.IsFitCardExpiryDateValid(ExpiryDate, "È richiesta la data di scadenza della tessera FIT.");
            }
            catch { }
        }

        #endregion

        #region ExtraCodes

        //[ObservableProperty]
        //public CustomerModel _customerInfo = new();

        //[ObservableProperty]
        //private bool _hasEmailError;

        //[ObservableProperty]
        //private string _emailError;

        //[ObservableProperty]
        //private bool _hasPhoneError;

        //[ObservableProperty]
        //private string _phoneError;

        //[ObservableProperty]
        //private bool _hasClientNameError;

        //[ObservableProperty]
        //private string _clientNameError;

        //[ObservableProperty]
        //private bool _hasSurnameError;

        //[ObservableProperty]
        //private string _surnameError;

        //[ObservableProperty]
        //private bool _hasFitCardError;

        //[ObservableProperty]
        //private string _fitCardError;

        //[ObservableProperty]
        //private bool _hasNotesError;

        //[ObservableProperty]
        //private string _notesError;

        //[ObservableProperty]
        //private bool _hasPlayerTypeError;

        //[ObservableProperty]
        //private string _playerTypeError;

        //[ObservableProperty]
        //private bool _hasTagError;

        //[ObservableProperty]
        //private string _tagError;

        //[ObservableProperty]
        //private DateTime _expiryDate = DateTime.Now;

        //[ObservableProperty]
        //public bool _marketingInformationEnabled;

        //public void Validate()
        //{
        //    (HasEmailError, EmailError) = FieldValidations.IsEmailValid(CustomerInfo.Email);
        //    (HasPhoneError, PhoneError) = FieldValidations.IsItalianPhoneNumberValid(CustomerInfo.Telephone);
        //    (HasClientNameError, ClientNameError) = FieldValidations.IsFieldNotEmpty(CustomerInfo.ClientName, "Client name is required.");
        //    (HasSurnameError, SurnameError) = FieldValidations.IsFieldNotEmpty(CustomerInfo.CustomerSurname, "Customer surname is required.");
        //    (HasFitCardError, FitCardError) = FieldValidations.IsFitCardExpiryDateValid(ExpiryDate, "FIT card expiry date is required.");
        //    (HasNotesError, NotesError) = FieldValidations.IsFieldNotEmpty(CustomerInfo.Notes, "Notes is required.");
        //    (HasPlayerTypeError, PlayerTypeError) = FieldValidations.IsFieldNotEmpty(CustomerInfo.PlayerType, "Player Types is required.");
        //    (HasTagError, TagError) = FieldValidations.IsFieldNotEmpty(CustomerInfo.Tag, "Tag must be selected.");
        //}

        //[RelayCommand]
        //private void AccountInfoClicked()
        //{
        //    MarketingInformationEnabled = !MarketingInformationEnabled;
        //}

        //[RelayCommand]
        //public async Task ButtonTap()
        //{
        //    try
        //    {
        //        Validate();
        //    }
        //    catch (Exception ex) { }
        //    IsBusy = false;
        //}


        #endregion
    }
}