public class ProductDal : GenericRepository<ProductModel>, IProductDal
{
    public ProductDal(AppDbContext context) : base(context)
    {
    }
}