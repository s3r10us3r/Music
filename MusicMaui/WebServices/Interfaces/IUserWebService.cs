using Shared.Dtos;

namespace MusicMaui.WebServices.Interfaces
{
    public interface IUserWebService
    {
        Task<WebResult<LoginResponseDto>> Login(LoginDto loginDto);
        Task<WebResult> Register(LoginDto loginDto);
        Task<WebResult<UserExpandedDto>> GetUser(int id);
    }
}
