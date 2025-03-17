using MyPadelDesktopApp.Models;
using MyPadelDesktopApp.Models.Responses;
using MyPadelDesktopApp.Services.HttpClientServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelDesktopApp.Services.DesktopClientServices
{
    public class DesktopClientService(IHttpClientService httpClientService) : IDesktopClientService
    {
        public async Task<GeneralResponse> GetAllUsers()
        {
            return await httpClientService.GetAsync("desktop-client/get-all-users", false);
        }
        public async Task<GeneralResponse> UserHistory(string email)
        {
            return await httpClientService.GetAsync("desktop-client/booking-history?email=" + email + "", false);
        }
        public async Task<GeneralResponse> DeleteUser(string email)
        {
            return await httpClientService.DeleteAsync("desktop-client/delete-user?email=" + email + "", null, false);
        }
        public async Task<GeneralResponse> CustomerStatistics(string email)
        {
            return await httpClientService.GetAsync("desktop-client/customer-statistics?email=" + email + "", false);
        }
        public async Task<GeneralResponse> UpdateClient(Object data)
        {
            return await httpClientService.PostAsync("desktop-client/update", data, false, false);
        }
        public async Task<GeneralResponse> MedicalCertificate(string email)
        {
            return await httpClientService.GetAsync("Picture/medical-certificate?email=" + email + "", false);
        }
    }
}