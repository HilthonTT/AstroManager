namespace AstroManagerClient.Library.Models;
public class CredentialModel
{
    public string Id { get; set; }
    public string Title { get; set; }
    public TypeModel Type { get; set; }
    public BasicUserModel User { get; set; }
    public List<FieldModel> Fields { get; set; } = new();
    public string Notes { get; set; }
    public DateTime DateAdded { get; set; } = DateTime.UtcNow;
    public DateTime DateModified { get; set; }
}
