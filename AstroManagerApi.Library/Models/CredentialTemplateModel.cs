using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AstroManagerApi.Library.Models;
public class CredentialTemplateModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Type { get; set; }
    public List<FieldModel> Fields { get; set; } = new();
}
