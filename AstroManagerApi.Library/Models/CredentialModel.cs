using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AstroManagerApi.Library.Models;
public class CredentialModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Title { get; set; }
    public TypeModel Type { get; set; }
    public BasicUserModel User { get; set; }
    public List<FieldModel> Fields { get; set; } = new();
    public string Notes { get; set; }
    public DateTime DateAdded { get; set; } = DateTime.UtcNow;

    public CredentialModel()
    {
        
    }

    public CredentialModel(CredentialTemplateModel template)
    {
        Type = template.Type;
        Fields = template.Fields;
    }
}
