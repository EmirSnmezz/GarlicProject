public class AdminContentService : IAdminContentService
{
    public ISliderDal _sliderContentDal;
    public AdminContentService(ISliderDal sliderContentDal)
    {
        _sliderContentDal = sliderContentDal;
    }

    public IResult AddSlider(SliderModel slider)
    {
        _sliderContentDal.Add(slider);

        return new SuccessResult(message:"Slider başarıyla eklendi");
    }
}