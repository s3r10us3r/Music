using Dal.Interfaces;
using Models;

namespace Dal;

public class ReviewRepo : BaseRepo<Review>, IReviewRepo
{
    public ReviewRepo(MusicDbContext dbContext) : base(dbContext)
    {
    }
}