using MyPadelDesktopApp.ViewModel;

namespace MyPadelDesktopApp.Views;

public partial class AddressBookingHomePage : ContentPage
{
	public AddressBookingHomePage(AddressBookingHomeViewModel addressBookingHomeViewModel)
	{
		InitializeComponent();
		BindingContext = addressBookingHomeViewModel;
    }
}