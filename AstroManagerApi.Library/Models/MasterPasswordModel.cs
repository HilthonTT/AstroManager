﻿namespace AstroManagerApi.Library.Models;
public class MasterPasswordModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [Required]
    public BasicUserModel User { get; set; }
    public string HashedPassword { get; set; }
    public string Salt { get; set; }
}
