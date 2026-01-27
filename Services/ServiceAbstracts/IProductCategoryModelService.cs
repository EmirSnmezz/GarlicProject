using System.Linq.Expressions;

public interface IProductCategoryModelService
{
     IResult Add (ProductCategoryModel category);
    IDataResult<List<ProductCategoryModel>> GetAll(Expression<Func<ProductCategoryModel, bool>> filter = null);
    IDataResult<ProductCategoryModel> GetById(Expression<Func<ProductCategoryModel, bool>> filter);
    IResult Remove(ProductCategoryModel category);
    IResult Update (ProductCategoryModel category);
}