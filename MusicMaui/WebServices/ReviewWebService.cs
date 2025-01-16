using MusicMaui.WebServices.Interfaces;
using Shared.Dtos;
using System.Net.Http.Json;

namespace MusicMaui.WebServices
{
    public class ReviewWebService : IReviewWebService
    {
        private readonly HttpClient _client;

        public ReviewWebService(HttpClient client)
        {
            _client = client;
        }

        public async Task<WebResult> Create(NewReviewDto dto)
        {
            try
            {
                var response = await _client.PostAsJsonAsync("api/Review", dto);
                if (response.IsSuccessStatusCode)
                {
                    return WebResult.Success((int)response.StatusCode);
                }
                return WebResult.Failure("Failed to create review", (int)response.StatusCode);
            }
            catch
            {
                return WebResult.Failure("Failed to create review", 500);
            }
        }
    }
}
