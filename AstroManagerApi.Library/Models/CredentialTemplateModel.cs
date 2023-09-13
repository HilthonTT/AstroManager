using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace AstroManagerApi.Library.Models;
public class CredentialTemplateModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [Required(ErrorMessage = "The type is required.")]
    public TypeModel Type { get; set; }

    [Required(ErrorMessage = "The fields are required.")]
    public List<FieldModel> Fields { get; set; } = new();
}
