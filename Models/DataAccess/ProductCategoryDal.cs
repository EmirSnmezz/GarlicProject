public class ProductCategoryModelDal : GenericRepository<ProductCategoryModel>, IProductCategoryDal
{
    public ProductCategoryModelDal(AppDbContext context) : base(context)
    {
    }
}