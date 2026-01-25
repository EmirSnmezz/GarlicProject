using System.Linq.Expressions;

public class ProductService : IProductService
{
    IProductDal _productDal;
    public ProductService(IProductDal productDal)
    {
        _productDal = productDal;
    }
    public IResult Add(ProductModel product)
    {
        _productDal.Add(product);

        return new SuccessResult("Ürün başarıyla oluşturuldu");
    }

    public IDataResult<List<ProductModel>> GetAll()
    {
        var result = _productDal.GetAll(null, x=> x.Category).ToList();

        if(result is not null)
        {
            return new SuccessDataResult<List<ProductModel>>("Ürünler başarıyla getirildi.", result);
        }

        return new ErrorDataResult<List<ProductModel>>("Görüntülenecek ürün bulunamadı");
    }

    public IDataResult<ProductModel> GetById(Expression<Func<ProductModel, bool>> filter)
    {
        var result = _productDal.Get(filter);

        if(result is not null)
        {
            return new SuccessDataResult<ProductModel>("Ürünler başarıyla getirildi.", result);
        }

        return new ErrorDataResult<ProductModel>("Görüntülenecek ürün bulunamadı");
    }

    public IResult Remove(ProductModel product)
    {
        _productDal.Delete(product);

        return new SuccessResult("Ürün silme işlemi başarılı");
    }

    public IResult Update(ProductModel product)
    {
        _productDal.Update(product);

        return new SuccessResult("Ürün başarıyla güncellendi");
    }
}