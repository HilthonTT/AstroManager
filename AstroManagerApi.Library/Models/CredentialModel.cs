using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace AstroManagerApi.Library.Models;
public class CredentialModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [Required(ErrorMessage = "The title is required.")]
    public string Title { get; set; }

    [Required(ErrorMessage = "The type is required.")]
    public TypeModel Type { get; set; }

    [Required(ErrorMessage = "The user is required.")]
    public BasicUserModel User { get; set; }

    [Required(ErrorMessage = "The fields are required.")]
    public List<FieldModel> Fields { get; set; } = new();
    public bool Favorited { get; set; } = false;
    public DateTime DateAdded { get; set; } = DateTime.UtcNow;
    public DateTime DateModified { get; set; } = DateTime.UtcNow;
}
