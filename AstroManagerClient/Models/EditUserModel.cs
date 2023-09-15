using System.ComponentModel.DataAnnotations;

namespace AstroManagerClient.Models;
public class EditUserModel
{
    [Required(ErrorMessage = "The display name is required.")]
    [StringLength(100, ErrorMessage = "Your display name is too long.")]
    public string DisplayName { get; set; }
}
