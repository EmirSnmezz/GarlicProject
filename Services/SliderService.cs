using System.Linq.Expressions;

public class SliderService : ISliderService
{
    ISliderDal _sliderDal;
    public SliderService(ISliderDal sliderDal)
    {
        _sliderDal = sliderDal;
    }
    public IResult Add(SliderModel slider)
    {
        _sliderDal.Add(slider);

        return new SuccessResult("Slider başarıyla eklendi");
    }

    public IDataResult<List<SliderModel>> GetAll()
    {
        var result = _sliderDal.GetAll();

        if(result is not null)
        {
            return new SuccessDataResult<List<SliderModel>>(data: result);       
        }
        return new ErrorDataResult<List<SliderModel>>("Görüntülenecek veri bulunamadı", null);
    
    }

    public IDataResult<SliderModel> GetById(Expression<Func<SliderModel, bool>> filter)
    {
        var result = _sliderDal.Get(filter);

        if(result is not null)
        {
            return new SuccessDataResult<SliderModel>(data: result);
        }

        return new SuccessDataResult<SliderModel>("Görüntülenecek veri bulunamadı.");
    }

    public IResult Remove(SliderModel slider)
    {
        _sliderDal.Delete(slider);

        return new SuccessResult("Slider başarıyla silindi");
    }

    public IResult Update(SliderModel slider)
    {
        _sliderDal.Update(slider);
        return new SuccessResult("Slider başarıyla güncellendi");
    }
}