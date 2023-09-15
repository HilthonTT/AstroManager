using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace AstroManagerApi.Library.Models;
public class UserModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [Required(ErrorMessage = "The oid is required.")]
    [StringLength(36, ErrorMessage = "Your oid is too long.")]
    public string ObjectIdentifier { get; set; }

    [Required(ErrorMessage = "The display name is required.")]
    [StringLength(100, ErrorMessage = "Your display name is too long.")]
    public string DisplayName { get; set; }

    [Required(ErrorMessage = "The first name is required.")]
    [StringLength(150, ErrorMessage = "Your first name is too long.")]
    public string FirstName { get; set; }
    
    [Required(ErrorMessage = "The last name is required.")]
    [StringLength(150, ErrorMessage = "Your last name is too long.")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "The email address is required.")]
    [StringLength(256, ErrorMessage = "Your email address too long.")]
    [EmailAddress(ErrorMessage = "The email address you've provided is not an email address.")]
    public string EmailAddress { get; set; }
}
