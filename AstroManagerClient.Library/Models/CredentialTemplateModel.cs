namespace AstroManagerClient.Library.Models;
public class CredentialTemplateModel
{
    public string Id { get; set; }
    public TypeModel Type { get; set; }
    public List<FieldModel> Fields { get; set; } = new();
}
