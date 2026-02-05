public class FormDal : GenericRepository<FormModel>, IFormDal
{
    public FormDal(AppDbContext context) : base(context)
    {
    }
}