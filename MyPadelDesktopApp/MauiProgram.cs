using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Microsoft.Maui.Platform;
using MyPadelDesktopApp.Views;
using Syncfusion.Maui.Toolkit.Hosting;
using CommunityToolkit.Maui;
using MyPadelDesktopApp.ViewModel;
using Microsoft.Maui.Controls.PlatformConfiguration;
using MyPadelDesktopApp.Services.HttpClientServices;
using MyPadelDesktopApp.Services.DesktopClientServices;
using MyPadelDesktopApp.Services.DesktopCourtSportsServices;
using Mopups.Hosting;
using MyPadelDesktopApp.Services.DesktopBookingServices;


#if WINDOWS
using WindowsUI = Windows.UI;
using XamlUI = Microsoft.UI.Xaml.Media;
#endif

namespace MyPadelDesktopApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureSyncfusionToolkit()
                .UseMauiCommunityToolkit()
                .ConfigureMopups()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(Entry), (handler, view) =>
            {
#if ANDROID
                handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Q)
                    handler.PlatformView.TextCursorDrawable.SetTint(Android.Graphics.Color.Black);
#elif WINDOWS
                handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
                handler.PlatformView.Background = null;
                handler.PlatformView.FocusVisualMargin = new Microsoft.UI.Xaml.Thickness(0);
                handler.PlatformView.Padding = new Microsoft.UI.Xaml.Thickness(0);
#elif IOS || MACCATALYST
                handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
                handler.PlatformView.Layer.BorderWidth = 0;
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
            });

            Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping(nameof(Picker), (handler, view) =>
            {
#if ANDROID
                handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Q)
                    handler.PlatformView.TextCursorDrawable.SetTint(Android.Graphics.Color.Black);
#elif WINDOWS
                handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
                handler.PlatformView.Background = null;
                handler.PlatformView.FocusVisualMargin = new Microsoft.UI.Xaml.Thickness(0);
                handler.PlatformView.Padding = new Microsoft.UI.Xaml.Thickness(0);
                handler.PlatformView.HeaderTemplate = new Microsoft.UI.Xaml.DataTemplate();
                handler.PlatformView.PlaceholderText = view.Title;
                view.TitleColor.ToRgba(out byte r, out byte g, out byte b, out byte a);
                handler.PlatformView.PlaceholderForeground = new Microsoft.UI.Xaml.Media.SolidColorBrush(WindowsUI.Color.FromArgb(a, r, g, b));
#elif IOS || MACCATALYST
                handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
                handler.PlatformView.Layer.BorderWidth = 0;
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
            });

            Microsoft.Maui.Handlers.DatePickerHandler.Mapper.AppendToMapping(nameof(DatePicker), (handler, view) =>
            {
#if ANDROID
                handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Q)
                    handler.PlatformView.TextCursorDrawable.SetTint(Android.Graphics.Color.Black);
#elif WINDOWS
                handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
                handler.PlatformView.Background = null;
                handler.PlatformView.FocusVisualMargin = new Microsoft.UI.Xaml.Thickness(0);
                handler.PlatformView.Padding = new Microsoft.UI.Xaml.Thickness(0);
                handler.PlatformView.HeaderTemplate = new Microsoft.UI.Xaml.DataTemplate();
                handler.PlatformView.Header = null;
#elif IOS || MACCATALYST
                handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
                handler.PlatformView.Layer.BorderWidth = 0;
#endif
            });

            builder.Services.AddSingleton<IHttpClientService, HttpClientService>();
            builder.Services.AddSingleton<IDesktopClientService, DesktopClientService>();
            builder.Services.AddSingleton<IDesktopCourtSportsService, DesktopCourtSportsService>();
            builder.Services.AddSingleton<IDesktopBookingService, DesktopBookingService>();

            builder.Services.AddTransient<AddressBookingPage>();
            builder.Services.AddTransient<AddressBookingViewModel>();
            builder.Services.AddTransient<AddressBookingHomePage>();
            builder.Services.AddTransient<AddressBookingHomeViewModel>();
            builder.Services.AddTransient<Dashboard>();
            builder.Services.AddTransient<DashboardViewModel>();
            builder.Services.AddTransient<SettingPage>();
            builder.Services.AddTransient<SettingViewModel>();
            builder.Services.AddTransient<StatisticsPage>();
            builder.Services.AddTransient<StatisticsViewModel>();
            builder.Services.AddTransient<FieldComfigurationsPage>();
            builder.Services.AddTransient<FieldConfigurationsViewModel>();
            builder.Services.AddTransient<ReservationsPage>();
            builder.Services.AddTransient<ReservationsViewModel>();
            builder.Services.AddTransient<BookingChartPage>();
            builder.Services.AddTransient<BookingChartViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
