using MyPadelDesktopApp.Models.Responses;
using MyPadelDesktopApp.Services.HttpClientServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelDesktopApp.Services.DesktopCourtSportsServices
{
    public class DesktopCourtSportsService(IHttpClientService httpClientService) : IDesktopCourtSportsService
    {
        public async Task<GeneralResponse> CourtSports()
        {
            return await httpClientService.GetAsync("courtsports/all", false);
        }
        public async Task<GeneralResponse> DeleteCourt(int id)
        {
            return await httpClientService.DeleteAsync("courtsports/deletefield", new { id }, false);
        }
        public async Task<GeneralResponse> AddCourt(object court)
        {
            return await httpClientService.PostAsync("courtsports/add", court, false);
        }
        public async Task<GeneralResponse> EditCourt(object court)
        {
            return await httpClientService.PutAsync("courtsports/update", court, false);
        }
    }
}