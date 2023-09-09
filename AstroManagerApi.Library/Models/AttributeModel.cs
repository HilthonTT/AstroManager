using System.ComponentModel.DataAnnotations;

namespace AstroManagerApi.Library.Models;
public class AttributeModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The attribute name is required.")]
    [StringLength(50, ErrorMessage = "The attribute name is too long.")]
    public string AttributeName { get; set; }
}
