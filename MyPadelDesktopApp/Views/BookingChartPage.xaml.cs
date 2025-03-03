using MyPadelDesktopApp.Helpers;
using MyPadelDesktopApp.Models;
using MyPadelDesktopApp.ViewModel;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyPadelDesktopApp.Views;

public partial class BookingChartPage : ContentPage
{
    #region Properties

    BookingChartViewModel _bookingChartViewModel;
    private List<string> Fields = new List<string>();
    private bool IsCourtsUpdated = true;
    private List<BookedData> MyBookings = new List<BookedData>();

    #endregion
    public BookingChartPage(BookingChartViewModel bookingChartViewModel)
    {
        InitializeComponent();
        BindingContext = _bookingChartViewModel = bookingChartViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if(Utils.IsBookingChartUpdated)
        {
            IsCourtsUpdated = true;
            GenerateScheduleGrid();
            Utils.IsBookingChartUpdated = false;
        }
    }

    private async void GenerateScheduleGrid()
    {
        ContentGrid.Children.Clear();
        ContentGrid.RowDefinitions.Clear();
        ContentGrid.ColumnDefinitions.Clear();

        Grid MyGrid = new Grid();
        Border border1 = new Border
        {
            Stroke = Colors.Transparent,
            BackgroundColor = Colors.Transparent,
            VerticalOptions = LayoutOptions.Fill,
            Content = new Label
            {
                Text = "",
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Colors.Black,
                Padding = new Thickness(5,0,0,0)
            }
        };

        Grid.SetColumn(border1, 0);
        MyGrid.AddColumnDefinition(new ColumnDefinition { Width = new GridLength(100) });
        MyGrid.Add(border1);

        if (IsCourtsUpdated || Fields ==null || Fields.Count<=0)
        {
            await _bookingChartViewModel.GetAllCourts();
            if (_bookingChartViewModel.AllCourts != null && _bookingChartViewModel.AllCourts.Count > 0)
            {
                Fields = _bookingChartViewModel.AllCourts.Select(s => s.fieldName).Distinct().ToList();
                _bookingChartViewModel.FieldList = new System.Collections.ObjectModel.ObservableCollection<string>(_bookingChartViewModel.AllCourts.Select(s => s.sportsName).Distinct().ToList());
            }

            IsCourtsUpdated = false;
        }

        for (int i = 0; i < Fields.Count; i++)
        {
            MyGrid.AddColumnDefinition(new ColumnDefinition { Width = GridLength.Star });
            Border border = new Border
            {
                Stroke = Colors.Black,
                BackgroundColor = Colors.Transparent,
                VerticalOptions = LayoutOptions.Fill,
                Content = new Label
                {
                    Text = Fields[i],
                    FontSize = 16,
                    FontAttributes = FontAttributes.Bold,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    TextColor = Colors.Black,
                    Padding = new Thickness(5)
                }
            };

            Grid.SetColumn(border, i + 1);
            MyGrid.Add(border);
        }

        double rowHeight = 60;
        for (int hour = 0; hour < 24; hour++)
        {
            MyGrid.AddRowDefinition(new RowDefinition { Height = new GridLength(rowHeight) });
            Border cellBorder1 = new Border
            {
                Stroke = Colors.Black,
                BackgroundColor = Colors.Transparent,
                VerticalOptions = LayoutOptions.Fill,
                Content = new Label
                {
                    Text = $"{hour}:00",
                    FontSize = 14,
                    TextColor = Colors.Black,
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalTextAlignment = TextAlignment.End
                }
            };

            Grid.SetRow(cellBorder1, hour);
            Grid.SetColumn(cellBorder1, 0);
            MyGrid.Add(cellBorder1);

            for (int i = 0; i < Fields.Count; i++)
            {
                Border cellBorder = new Border
                {
                    Stroke = Colors.Black,
                    BackgroundColor = Colors.Transparent,
                    VerticalOptions = LayoutOptions.Fill,
                    Content = new Label
                    {
                        Text = "",
                        FontSize = 14,
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Center
                    }
                };

                var tapGesture = new TapGestureRecognizer();
                int columnIndex = i + 1;
                int rowIndex = hour;
                tapGesture.Tapped += (s, e) => OnGridCellTapped(columnIndex, rowIndex);

                cellBorder.GestureRecognizers.Add(tapGesture);

                Grid.SetColumn(cellBorder, i + 1);
                Grid.SetRow(cellBorder, hour);
                MyGrid.Add(cellBorder);
            }
        }

        MyBookings = await _bookingChartViewModel.GetAllBookingsByDate();
        if (MyBookings != null && MyBookings.Count > 0)
            AddBookingsToGrid(MyGrid);
        
        ContentGrid.Children.Add(MyGrid);

    }
    private void AddBookingsToGrid(Grid MyGrid)
    {
        double rowHeight = 60;
        foreach (var booking in MyBookings)
        {
            int columnIndex = Fields.IndexOf(booking.fieldName) + 1;
            if (columnIndex == 0) continue;

            if (booking.status.Equals("Canceled"))
                continue;

            //TimeSpan startTime = ConvertDecimalToTimeSpan(booking.StartTime);
            //TimeSpan endTime = ConvertDecimalToTimeSpan(booking.EndTime);
            TimeSpan MyDuration = (booking.endTime.Value - booking.date.Value);
            double durationInHours = MyDuration.TotalMinutes / 60.0;

            double rowStart = double.Parse(booking.date.Value.ToString("HH.mm"));
            int durationMinutes = (int)((durationInHours - Math.Floor(durationInHours)) * 60);
            double height = (rowHeight * (int)durationInHours) + (durationMinutes * rowHeight / 60);
            double fractionalPart = rowStart - (int)Math.Floor(rowStart);
            int NewH = (int)(((fractionalPart) - Math.Floor(fractionalPart)) * 100);

            Frame bookingFrame = new Frame
            {
                BackgroundColor = Color.FromRgba("#1558b3"),
                BorderColor = Colors.Transparent,
                CornerRadius = 5,
                Padding = new Thickness(5),
                HasShadow = false,
                VerticalOptions = LayoutOptions.Start,
                HeightRequest = height,
                Content = new Label
                {
                    Text = $"{booking.firstName} {booking.lastName}\n{booking.date.Value.ToString("HH:mm")} - {booking.endTime.Value.ToString("HH:mm")}",
                    FontSize = 12,
                    TextColor = Colors.White,
                    VerticalTextAlignment = TextAlignment.Center
                },
                Margin = new Thickness(0, NewH, 0, 0)
            };

            // Add tap gesture recognizer
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += (s, e) => OnBookingTapped(booking);
            bookingFrame.GestureRecognizers.Add(tapGesture);

            int rowIndex = (int)Math.Floor(rowStart);
            Grid.SetColumn(bookingFrame, columnIndex);
            Grid.SetRow(bookingFrame, rowIndex);
            Grid.SetRowSpan(bookingFrame, (int)Math.Ceiling(double.Parse(booking.endTime.Value.ToString("HH.mm")) - double.Parse(booking.date.Value.ToString("HH.mm"))));
            MyGrid.Add(bookingFrame);
        }
    }

    private void OnGridCellTapped(int columnIndex, int rowIndex)
    {

        //string selectedField = Fields[columnIndex - 1];
        //DateTime selectedTime = _bookingChartViewModel.CurrentSelectedDate.Date.AddHours(rowIndex);
        //_bookingChartViewModel.SelectedCourt = selectedField;
        //_bookingChartViewModel.SelectedTime = selectedTime;
        //_bookingChartViewModel.IsAlreadyExist = false;
        //navigationDrawer.ToggleDrawer();
    }
    private void OnBookingTapped(BookedData booking)
    {
        _bookingChartViewModel.SelectedBooking = booking;
        _bookingChartViewModel.IsAlreadyExist = true;
        navigationDrawer.ToggleDrawer();
    }

    private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        GenerateScheduleGrid();
    }
    private void OnBackDateClicked(object sender, TappedEventArgs e)
    {
        _bookingChartViewModel.CurrentSelectedDate = _bookingChartViewModel.CurrentSelectedDate.AddDays(-1);
    }
    private void OnNextDateClicked(object sender, TappedEventArgs e)
    {
        _bookingChartViewModel.CurrentSelectedDate = _bookingChartViewModel.CurrentSelectedDate.AddDays(1);
    }

    private async void CancelReservation(object sender, EventArgs e)
    {
        navigationDrawer.IsOpen = false;
        var response = await _bookingChartViewModel.CancelBooking();
        if(response)
        {
            GenerateScheduleGrid();
            navigationDrawer.IsOpen = false;
        }
    }

    private void OnCloseDrawer(object sender, TappedEventArgs e)
    {
        navigationDrawer.IsOpen = false;
    }
    private async void ReserveBooking(object sender, EventArgs e)
    {
        var result = await _bookingChartViewModel.AddBooking();
        if(result)
        {
            navigationDrawer.IsOpen = false;
            GenerateScheduleGrid();
        }
    }
    private void AddNewBookingClicked(object sender, EventArgs e)
    {
        _bookingChartViewModel.IsAlreadyExist = false;
        navigationDrawer.ToggleDrawer();
        MainThread.BeginInvokeOnMainThread(() =>
        {
            _bookingChartViewModel.GetAllUsers();
        });
    }
    private void OnFieldNameSelected(object sender, EventArgs e)
    {
        _bookingChartViewModel.AddFieldTypeList();
    }
}