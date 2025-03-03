using MyPadelDesktopApp.ViewModel;

namespace MyPadelDesktopApp.Views;

public partial class Dashboard : ContentPage
{
	public Dashboard(DashboardViewModel dashboardViewModel)
	{
		InitializeComponent();
		BindingContext = dashboardViewModel;
    }
}