using MyPadelDesktopApp.Views;

namespace MyPadelDesktopApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("AddressBookingPage", typeof(AddressBookingPage));
            Routing.RegisterRoute("AddressBookingHomePage", typeof(AddressBookingHomePage));
            Routing.RegisterRoute("Dashboard", typeof(Dashboard));
            Routing.RegisterRoute("SettingPage", typeof(SettingPage));
            Routing.RegisterRoute("StatisticsPage", typeof(StatisticsPage));
            Routing.RegisterRoute("FieldComfigurationsPage", typeof(FieldComfigurationsPage));
            Routing.RegisterRoute("ReservationsPage", typeof(ReservationsPage));
            Routing.RegisterRoute("BookingChartPage", typeof(BookingChartPage));
        }
    }
}