namespace AstroManagerApi.Library.Models;
public class RecoveryKeyModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Salt { get; set; }
    public BasicUserModel User { get; set; }
    public HashSet<string> RecoveryKeys { get; set; } = new();
}
