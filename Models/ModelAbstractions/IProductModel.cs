public interface IProductModel : IModel
{
    public ImageModel Image { get; set; }
    public string ImageId { get; set; }
    public ProductCategoryModel Category { get; set; }
    public string CategoryId { get; set; }
    public string ProductTitle { get; set; }
    public string ProductDescription { get; set; }
}