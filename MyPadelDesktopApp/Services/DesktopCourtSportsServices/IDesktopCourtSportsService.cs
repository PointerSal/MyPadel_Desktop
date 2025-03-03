using MyPadelDesktopApp.Models.Responses;

namespace MyPadelDesktopApp.Services.DesktopCourtSportsServices
{
    public interface IDesktopCourtSportsService
    {
        Task<GeneralResponse> AddCourt(object court);
        Task<GeneralResponse> CourtSports();
        Task<GeneralResponse> DeleteCourt(int id);
        Task<GeneralResponse> EditCourt(object court);
    }
}