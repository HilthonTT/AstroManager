using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace AstroManagerApi.Library.Models;
public class BasicUserModel
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public BasicUserModel()
    {
        
    }

    public BasicUserModel(UserModel user)
    {
        Id = user.Id;
        FirstName = user.FirstName;
        LastName = user.LastName;
    }
}
