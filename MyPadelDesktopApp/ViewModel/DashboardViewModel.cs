using CommunityToolkit.Mvvm.Input;
using MyPadelDesktopApp.Helpers;
using MyPadelDesktopApp.ViewModel.ViewBaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelDesktopApp.ViewModel
{
    public partial class DashboardViewModel : BaseViewModel
    {
        #region Commands

        [RelayCommand]
        private async Task NavigateToPage(string path)
        {
            Utils.IsBookingChartUpdated = true;
            await Shell.Current.GoToAsync(path);
        }

        #endregion
    }
}
