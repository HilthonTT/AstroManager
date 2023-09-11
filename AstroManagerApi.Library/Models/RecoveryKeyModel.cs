using AstroManagerApi.Library.Models.Request;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AstroManagerApi.Library.Models;
public class RecoveryKeyModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public BasicUserModel User { get; set; }
    public HashSet<string> RecoveryKeys { get; set; } = new();

    public RecoveryKeyModel()
    {
        
    }

    public RecoveryKeyModel(RecoveryRequestModel request)
    {
        User = request.Recovery.User;
        RecoveryKeys = request.Recovery.RecoveryKeys;
    }
}
