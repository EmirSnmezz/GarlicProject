using System.Linq.Expressions;

public interface IProductService
{
    IResult Add (ProductModel product);
    IDataResult<List<ProductModel>> GetAll(Expression<Func<ProductModel, bool>> filter = null);
    IDataResult<ProductModel> GetById(Expression<Func<ProductModel, bool>> filter);
    IResult Remove(ProductModel product);
    IResult Update (ProductModel product);
}