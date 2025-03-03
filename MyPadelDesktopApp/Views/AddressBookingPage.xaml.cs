using MyPadelDesktopApp.ViewModel;

namespace MyPadelDesktopApp.Views;

public partial class AddressBookingPage : ContentPage
{
    AddressBookingViewModel _addressBookingViewModel;
    public AddressBookingPage(AddressBookingViewModel addressBookingViewModel)
	{
		InitializeComponent();
		BindingContext = _addressBookingViewModel = addressBookingViewModel;
	}

    private void SfSegmentedControl_SelectionChanged(object sender, Syncfusion.Maui.Toolkit.SegmentedControl.SelectionChangedEventArgs e)
    {
        try
        {
            _addressBookingViewModel.SelectedItem = e.NewValue.Text;
            _addressBookingViewModel.SelectedItemChanged(e.NewValue.Text);
        }
        catch { }
    }
}