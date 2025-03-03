using CommunityToolkit.Mvvm.Input;
using MyPadelDesktopApp.ViewModel.ViewBaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelDesktopApp.ViewModel
{
    public partial class SettingViewModel : BaseViewModel
    {
        #region Services

        #endregion

        #region Properties

        #endregion

        #region Commands

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

        #endregion

        #region Constructor

        #endregion
    }
}