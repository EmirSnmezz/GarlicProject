public class SliderContentModel : ISliderContentModel
{
    public string ImageUrl {get; set;}
    public string ContentHeader { get; set; }
    public string ContentText { get; set; }
    public string? Id { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }

    public SliderContentModel()
    {
        Id = new Guid().ToString();
    }
}