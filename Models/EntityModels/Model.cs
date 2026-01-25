public class Model : IModel
{
    public string Id { get;}
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }

    public Model()
    {
        Id = Guid.NewGuid().ToString();
    }
}