using Dal.Interfaces;
using Models;

namespace Dal;

public class UserRepo : BaseRepo<User>, IUserRepo
{
    public UserRepo(MusicDbContext dbContext) : base(dbContext)
    {
    }
}