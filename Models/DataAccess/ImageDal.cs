public class ImageDal : GenericRepository<ImageModel>, IImageDal
{
    public ImageDal(AppDbContext context) : base(context)
    {
    }
}