using Shared.Dtos;

namespace MusicMaui.WebServices.Interfaces
{
    public interface IReviewWebService
    {
        Task<WebResult> Create(NewReviewDto dto);
    }
}
