public class FormModel : Model, IFormModel
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
    public bool IsActive { get; set; }
}