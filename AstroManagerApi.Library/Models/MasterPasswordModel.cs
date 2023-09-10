using MongoDB.Bson.Serialization.Attributes;

namespace AstroManagerApi.Library.Models;
public class MasterPasswordModel
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public string Id { get; set; }
    public BasicUserModel User { get; set; }
    public string HashedPassword { get; set; }
}
