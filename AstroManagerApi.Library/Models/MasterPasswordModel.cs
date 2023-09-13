using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace AstroManagerApi.Library.Models;
public class MasterPasswordModel
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public string Id { get; set; }

    [Required]
    public BasicUserModel User { get; set; }
    public string HashedPassword { get; set; }
}
