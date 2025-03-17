using MyPadelDesktopApp.Models.Responses;

namespace MyPadelDesktopApp.Services.DesktopClientServices
{
    public interface IDesktopClientService
    {
        Task<GeneralResponse> CustomerStatistics(string email);
        Task<GeneralResponse> DeleteUser(string email);
        Task<GeneralResponse> GetAllUsers();
        Task<GeneralResponse> UpdateClient(object data);
        Task<GeneralResponse> UserHistory(string email);
        Task<GeneralResponse> MedicalCertificate(string email);
    }
}