using System.Linq.Expressions;

public interface IImageService
{
    
    IResult Add (ImageModel image);
    IDataResult<List<ImageModel>> GetAll(Expression<Func<ImageModel, bool>> filter);
    IDataResult<ImageModel> GetById(Expression<Func<ImageModel, bool>> filter);
    IResult Remove(ImageModel image);
    IResult Update (ImageModel image);
}