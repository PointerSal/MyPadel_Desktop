using MyPadelDesktopApp.ViewModel;

namespace MyPadelDesktopApp.Views;

public partial class StatisticsPage : ContentPage
{
	public StatisticsPage(StatisticsViewModel statisticsViewModel)
	{
		InitializeComponent();
		BindingContext = statisticsViewModel;
    }
}