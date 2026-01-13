public class AdminContentService : IAdminContentService
{
    public ISliderContentDal _sliderContentDal;
    public AdminContentService(ISliderContentDal sliderContentDal)
    {
        _sliderContentDal = sliderContentDal;
    }

    public IResult AddSlider(SliderContentModel slider)
    {
        _sliderContentDal.Add(slider);

        return new SuccessResult(message:"Slider başarıyla eklendi");
    }
}