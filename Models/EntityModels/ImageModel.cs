public class ImageModel : Model, IImageModel
{
    public string ImageUrl { get; set; }
    public string SliderId { get; set; }
    public int DisplayPriority { get; set; }
}