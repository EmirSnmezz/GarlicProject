using System.Diagnostics;
using System.IO.Compression;
using System.Linq.Expressions;

public class ProductCategoryService : IProductCategoryModelService
{
    IProductCategoryDal _productCategoryDal;
    public ProductCategoryService(IProductCategoryDal productCategoryDal)
    {
        _productCategoryDal = productCategoryDal;
    }

    public IResult Add(ProductCategoryModel category)
    {
        _productCategoryDal.Add(category);
        return new SuccessResult("Kategori başarıyla eklendi");
    }

    public IDataResult<List<ProductCategoryModel>> GetAll()
    {
        var result = _productCategoryDal.GetAll().ToList();

        if(result is not null)
        {
            return new SuccessDataResult<List<ProductCategoryModel>>(data: result);
        }

        return new ErrorDataResult<List<ProductCategoryModel>>("Görüntülenecek Kategori Bulunamadı.");
    }

    public IDataResult<ProductCategoryModel> GetById(Expression<Func<ProductCategoryModel, bool>> filter)
    {
        var result = _productCategoryDal.Get(filter);

        if(result is not null)
        {
            return new SuccessDataResult<ProductCategoryModel>("", result);
        }

        return new ErrorDataResult<ProductCategoryModel>("Görüntülenecek Kategori Bulunamadı");
    }

    public IResult Remove(ProductCategoryModel category)
    {
        _productCategoryDal.Delete(category);

        return new SuccessResult("Kategori silme işlemi başarılı");
    }

    public IResult Update(ProductCategoryModel category)
    {
        _productCategoryDal.Update(category);

        return new SuccessResult("Kategori Başarıyla Güncellendi");
    }
}