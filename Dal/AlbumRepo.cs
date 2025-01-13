using Dal.Interfaces;
using Models;

namespace Dal;

public class AlbumRepo : BaseRepo<Album>, IAlbumRepo
{
    public AlbumRepo(MusicDbContext dbContext) : base(dbContext)
    {
    }
}