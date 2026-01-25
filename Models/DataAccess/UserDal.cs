public class UserDal: GenericRepository<User>, IUserDal
{
    public UserDal(AppDbContext context) : base(context)
    {}
}