using System.ComponentModel.DataAnnotations;

namespace AstroManagerApi.Library.Models;
public class AttributeModel
{
    public int Id { get; set; }
    public int UserId { get; set; }

    [Required]
    [StringLength(50)]
    public string AttributeName { get; set; }
}
