using MusicMaui.WebServices.Interfaces;
using Shared.Dtos;
using System.Net.Http.Json;

namespace MusicMaui.WebServices
{
    public class UserWebService : IUserWebService
    {
        private readonly HttpClient _httpClient;

        public UserWebService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WebResult<UserExpandedDto>> GetUser(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/User/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadFromJsonAsync<UserExpandedDto>();
                    if (user == null)
                        return WebResult<UserExpandedDto>.Failure("Failed to deserialize the response.", (int)response.StatusCode);
                    return WebResult<UserExpandedDto>.Success(user, (int)response.StatusCode);
                }
                else
                {
                    var messageDto = await response.Content.ReadFromJsonAsync<MessageDto>();
                    var message = messageDto?.Message ?? "Failed to get user.";
                    return WebResult<UserExpandedDto>.Failure(message, (int)response.StatusCode);
                }
            }
            catch (Exception e)
            {
                return WebResult<UserExpandedDto>.Failure(e.Message, 500);
            }
        }

        public async Task<WebResult<LoginResponseDto>> Login(LoginDto loginDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/User/login", loginDto);
                if (response.IsSuccessStatusCode)
                {
                    var loginResponseDto = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
                    if (loginResponseDto == null)
                        return WebResult<LoginResponseDto>.Failure("Failed to deserialize the response.", (int)response.StatusCode);
                    return WebResult<LoginResponseDto>.Success(loginResponseDto, (int)response.StatusCode);
                }
                else
                {
                    var messageDto = await response.Content.ReadFromJsonAsync<MessageDto>();
                    var message = messageDto?.Message ?? "Failed to login.";
                    return WebResult<LoginResponseDto>.Failure(message, (int)response.StatusCode);
                }
            }
            catch (Exception e)
            {
                return WebResult<LoginResponseDto>.Failure(e.Message, 500);
            }
        }

        public async Task<WebResult> Register(LoginDto loginDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/User/register", loginDto);
            if (response.IsSuccessStatusCode)
            {
                return WebResult.Success((int)response.StatusCode);
            }
            else
            {
                var messageDto = await response.Content.ReadFromJsonAsync<MessageDto>();
                var message = messageDto?.Message ?? "Failed to register.";
                return WebResult.Failure(message, (int)response.StatusCode);
            }
        }
    }
}
