public class ContentModel : IContentModel
{
    public string ContentHeader { get; set; }
    public string ContentText { get; set; }
    public string Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}