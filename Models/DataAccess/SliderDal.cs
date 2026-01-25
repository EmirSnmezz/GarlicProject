public class SliderDal : GenericRepository<SliderModel>, ISliderDal
{
    public SliderDal(AppDbContext context) : base(context)
    {
    }
}