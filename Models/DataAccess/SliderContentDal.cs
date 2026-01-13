public class SliderContentDal : GenericRepository<SliderContentModel>, ISliderContentDal
{
    public SliderContentDal(AppDbContext context) : base(context)
    {
    }
}