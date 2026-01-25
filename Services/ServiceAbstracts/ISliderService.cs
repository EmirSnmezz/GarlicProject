using System.Linq.Expressions;

public interface ISliderService
{
    IResult Add (SliderModel slider);
    IDataResult<List<SliderModel>> GetAll();
    IDataResult<SliderModel> GetById(Expression<Func<SliderModel, bool>> filter);
    IResult Remove(SliderModel slider);
    IResult Update (SliderModel slider);
}