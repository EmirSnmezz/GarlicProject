public interface IImageModel : IModel
{
    string ImageUrl {get; set;}
    public string SliderId { get; set; }
    public int DisplayPriority { get; set; }
}