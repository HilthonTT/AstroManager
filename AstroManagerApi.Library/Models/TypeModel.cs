namespace AstroManagerApi.Library.Models;
public class TypeModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [Required(ErrorMessage = "The name is required.")]
    [StringLength(100, ErrorMessage = "The name is too long.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "The description is required.")]
    [StringLength(500, ErrorMessage = "The description is too long.")]
    public string Description { get; set; }
}
