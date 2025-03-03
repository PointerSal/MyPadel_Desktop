using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelDesktopApp.ViewModel.ViewBaseModel
{
    public partial class BaseViewModel : ObservableObject
    {
        #region Properties

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private bool _isEmpty;

        [ObservableProperty]
        private string _operatorName = "Admin";

        #endregion
    }
}
