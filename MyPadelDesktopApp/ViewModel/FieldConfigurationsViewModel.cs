using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyPadelDesktopApp.Models;
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
    public partial class FieldConfigurationsViewModel : BaseViewModel
    {
        #region Services

        private readonly IDesktopCourtSportsService _desktopCourtSportsService;

        #endregion

        #region Properties

        [ObservableProperty]
        public ObservableCollection<Booking> _bookings;

        #endregion

        #region Commands

        [RelayCommand]
        public async Task AddNew()
        {
            try
            {
                await Shell.Current.GoToAsync("ReservationsPage", new Dictionary<string, object>
                {
                    {"CourtType", "Add" },
                    {"CourtList", Bookings }
                });
            }
            catch { }
        }

        [RelayCommand]
        public async Task Edit(Booking booking)
        {
            try
            {
                await Shell.Current.GoToAsync("ReservationsPage", new Dictionary<string, object>
                {
                    {"CourtType", "Edit" },
                    {"SelectedBookingCourt", booking },
                    {"CourtList", Bookings }
                });
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

        public async Task GetAllCourts()
        {
            try
            {
                IsBusy = true;
                IsEmpty = false;
                var response = await _desktopCourtSportsService.CourtSports();
                if (response != null && response.code != null && response.code.Equals("0000"))
                    Bookings = new ObservableCollection<Booking>(JsonSerializer.Deserialize<List<Booking>>(response.data.ToString()));
                else if (response != null && response.code != null)
                    await Shell.Current.DisplayAlert("Errore", response.message, "OK");
                else
                    await Shell.Current.DisplayAlert("Errore", "Qualcosa è andato storto", "OK");
            }
            catch { }
            IsEmpty = Bookings == null || Bookings.Count == 0;
            IsBusy = false;
        }

        #endregion

        #region Constructor
        public FieldConfigurationsViewModel(IDesktopCourtSportsService desktopCourtSportsService)
        {
            _desktopCourtSportsService = desktopCourtSportsService;
        }

        #endregion
    }
}