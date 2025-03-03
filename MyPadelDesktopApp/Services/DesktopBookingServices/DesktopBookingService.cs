using MyPadelDesktopApp.Models.Responses;
using MyPadelDesktopApp.Services.HttpClientServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelDesktopApp.Services.DesktopBookingServices
{
    public class DesktopBookingService(IHttpClientService httpClientService) : IDesktopBookingService
    {
        public async Task<GeneralResponse> GetBookings(string date)
        {
            return await httpClientService.GetAsync("desktop-booking/all?date=" + date + "", false);
        }
        public async Task<GeneralResponse> CancelBooking(int BookingId)
        {
            return await httpClientService.PostAsync("desktop-booking/cancel?bookingId=" + BookingId + "", null, false);
        }
        public async Task<GeneralResponse> AddBookings(object booking)
        {
            return await httpClientService.PostAsync("desktop-booking/reserve", booking, false);
        }
    }
}
