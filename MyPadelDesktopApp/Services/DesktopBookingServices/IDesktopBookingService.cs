using MyPadelDesktopApp.Models.Responses;

namespace MyPadelDesktopApp.Services.DesktopBookingServices
{
    public interface IDesktopBookingService
    {
        Task<GeneralResponse> AddBookings(object booking);
        Task<GeneralResponse> CancelBooking(int BookingId);
        Task<GeneralResponse> GetBookings(string date);
    }
}