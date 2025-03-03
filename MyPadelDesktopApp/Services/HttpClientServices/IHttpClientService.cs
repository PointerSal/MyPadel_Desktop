using MyPadelDesktopApp.Models.Responses;

namespace MyPadelDesktopApp.Services.HttpClientServices
{
    public interface IHttpClientService
    {
        Task<GeneralResponse> DeleteAsync(string url, object data, bool isToken);
        Task<GeneralResponse> GetAsync(string url, bool isToken);
        Task<GeneralResponse> PostAsync(string url, object data, bool isToken, bool IsPost = true);
        Task<GeneralResponse> PutAsync(string url, object data, bool isToken);
    }
}