using MyPadelDesktopApp.ViewModel;

namespace MyPadelDesktopApp.Views;

public partial class FieldComfigurationsPage : ContentPage
{
    #region Properties

    FieldConfigurationsViewModel _fieldConfigurationsViewModel;
    private bool IsAppeared = false;

    #endregion
    public FieldComfigurationsPage(FieldConfigurationsViewModel fieldConfigurationsViewModel)
	{
		InitializeComponent();
		BindingContext = _fieldConfigurationsViewModel = fieldConfigurationsViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (IsAppeared)
            return;

        IsAppeared = true;
        await _fieldConfigurationsViewModel.GetAllCourts();
    }
}