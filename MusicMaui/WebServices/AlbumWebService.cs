using MusicMaui.WebServices.Interfaces;
using Shared.Dtos;
using System.Net.Http.Json;

namespace MusicMaui.WebServices
{
    public class AlbumWebService : IAlbumWebService
    {
        private readonly HttpClient _httpClient;

        public AlbumWebService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WebResult> Create(NewAlbumDto dto)
        {
            try
            {
                var result = await _httpClient.PostAsJsonAsync("api/Album", dto);
                if (result.IsSuccessStatusCode)
                {
                    return WebResult.Success((int)result.StatusCode);
                }
                else
                {
                    var messageDto = await result.Content.ReadFromJsonAsync<MessageDto>();
                    var message = messageDto?.Message ?? "An error occurred";
                    return WebResult.Failure(message, (int)result.StatusCode);
                }
            }
            catch (Exception ex)
            {
                return WebResult.Failure(ex.Message, 500);
            }
        }

        public async Task<WebResult<AlbumExpandedDto>> Get(int id)
        {
            try
            {
                var result = await _httpClient.GetAsync($"api/Album/{id}");
                if (result.IsSuccessStatusCode)
                {
                    var album = await result.Content.ReadFromJsonAsync<AlbumExpandedDto>();
                    if (album == null)
                    {
                        return WebResult<AlbumExpandedDto>.Failure("Failed to serialize the response", (int)result.StatusCode);
                    }
                    return WebResult<AlbumExpandedDto>.Success(album, (int)result.StatusCode);
                }
                else
                {
                    var messageDto = await result.Content.ReadFromJsonAsync<MessageDto>();
                    var message = messageDto?.Message ?? "An error occurred";
                    return WebResult<AlbumExpandedDto>.Failure(message, (int)result.StatusCode);
                }
            }
            catch (Exception ex)
            {
                return WebResult<AlbumExpandedDto>.Failure(ex.Message, 500);
            }
        }

        public async Task<WebResult<List<AlbumDto>>> GetPaged(AlbumPagedRequest request)
        {
            try
            {
                var result = await _httpClient.PostAsJsonAsync("api/Album/getPage", request);
                if (result.IsSuccessStatusCode)
                {
                    var albums = await result.Content.ReadFromJsonAsync<List<AlbumDto>>();
                    if (albums == null)
                    {
                        return WebResult<List<AlbumDto>>.Failure("Failed to serialize the response", (int)result.StatusCode);
                    }
                    return WebResult<List<AlbumDto>>.Success(albums, (int)result.StatusCode);
                }
                else
                {
                    var messageDto = await result.Content.ReadFromJsonAsync<MessageDto>();
                    var message = messageDto?.Message ?? "An error occurred";
                    return WebResult<List<AlbumDto>>.Failure(message, (int)result.StatusCode);
                }
            }
            catch (Exception ex)
            {
                return WebResult<List<AlbumDto>>.Failure(ex.Message, 500);
            }
        }
    }
}
