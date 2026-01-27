using System.Linq.Expressions;

public class ImageService : IImageService
{
    IImageDal _imageDal;
    public ImageService(IImageDal imageDal)
    {
        _imageDal = imageDal;
    }
    public IResult Add(ImageModel image)
    {
        _imageDal.Add(image);
        
        return new SuccessResult("Resim başarıyla eklendi");
    }

    public IDataResult<List<ImageModel>> GetAll(Expression<Func<ImageModel, bool>> filter = null)
    {
        if(filter is null)
        {
            var result = _imageDal.GetAll(filter);

            if(result is not null)
            {
               return new SuccessDataResult<List<ImageModel>>(data : result);
            }   
        }
        else
        {
            var result = _imageDal.GetAll();

            if(result is not null)
            {
               return new SuccessDataResult<List<ImageModel>>(data : result);
            }
        }

        return new ErrorDataResult<List<ImageModel>>(data:null);
    }

    public IDataResult<ImageModel> GetById(Expression<Func<ImageModel, bool>> filter)
    {
         var result = _imageDal.Get(filter);

        if(result is not null)
        {
            return new SuccessDataResult<ImageModel>(data : result);
        }

        return new ErrorDataResult<ImageModel>(data : null);
    }

    public IResult Remove(ImageModel image)
    {
        _imageDal.Delete(image);
        
        return new SuccessResult("Resim başarıyla Silindi");
    }

    public IResult Update(ImageModel image)
    {
        _imageDal.Update(image);
        
        return new SuccessResult("Resim başarıyla Güncellendi");
    }
}