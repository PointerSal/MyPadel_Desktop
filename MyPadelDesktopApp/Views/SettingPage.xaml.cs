using MyPadelDesktopApp.ViewModel;

namespace MyPadelDesktopApp.Views;

public partial class SettingPage : ContentPage
{
	public SettingPage(SettingViewModel settingViewModel)
	{
		InitializeComponent();
		BindingContext = settingViewModel;
    }
}