using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicMaui.WebServices.Interfaces
{
    public interface IAlbumWebService
    {
        Task<WebResult> Create(NewAlbumDto dto);
        Task<WebResult<AlbumExpandedDto>> Get(int id);
        Task<WebResult<List<AlbumDto>>> GetPaged(AlbumPagedRequest request);
    }
}
