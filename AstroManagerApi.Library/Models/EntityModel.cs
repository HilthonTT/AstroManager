using System.ComponentModel.DataAnnotations;

namespace AstroManagerApi.Library.Models;
public class EntityModel
{
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    [StringLength(50)]
    public string EntityType { get; set; }
}
