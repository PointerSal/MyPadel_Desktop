using CommunityToolkit.Mvvm.Messaging;
using MyPadelDesktopApp.Helpers.References;
using MyPadelDesktopApp.ViewModel;

namespace MyPadelDesktopApp.Views;

public partial class ReservationsPage : ContentPage
{
	public ReservationsPage(ReservationsViewModel reservationsViewModel)
	{
		InitializeComponent();
		BindingContext =  reservationsViewModel;
	}

    protected override bool OnBackButtonPressed()
    {
        WeakReferenceMessenger.Default.Unregister<OpeningHoursMessage>(this);
        return base.OnBackButtonPressed();
    }
}