using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyPadelDesktopApp.Models;
using MyPadelDesktopApp.Services.DesktopClientServices;
using MyPadelDesktopApp.ViewModel.ViewBaseModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyPadelDesktopApp.ViewModel
{
    public partial class AddressBookingHomeViewModel : BaseViewModel
    {
        #region Services

        private readonly IDesktopClientService _desktopClientService;

        #endregion

        #region Properties

        [ObservableProperty]
        private ObservableCollection<UsersData> _userList;

        #endregion

        #region Commands

        [RelayCommand]
        public async Task Edit(UsersData usersData)
        {
            try
            {
                await Shell.Current.GoToAsync("AddressBookingPage", new Dictionary<string, object>
                {
                    {"UserList", UserList },
                    {"SelectedUser", usersData }
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
        public async void GetAllUsers()
        {
            try
            {
                IsBusy = true;
                IsEmpty = false;
                var response = await _desktopClientService.GetAllUsers();
                if (response != null && response.code != null && response.code.Equals("0000"))
                    UserList = new ObservableCollection<UsersData>(JsonSerializer.Deserialize<List<UsersData>>(response.data.ToString()));
                else if (response != null && response.code != null)
                    await Shell.Current.DisplayAlert("Errore", response.message, "OK");
                else
                    await Shell.Current.DisplayAlert("Errore", "Qualcosa è andato storto", "OK");
            }
            catch { }
            IsEmpty = UserList == null || UserList.Count == 0;
            IsBusy = false;
        }

        #endregion

        #region Constructor
        public AddressBookingHomeViewModel(IDesktopClientService desktopClientService)
        {
            _desktopClientService = desktopClientService;
            GetAllUsers();
        }

        #endregion
    }
}