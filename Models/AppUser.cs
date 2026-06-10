using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace League_of_Legends_Tournament_Hosting.Models;

public class AppUser : IdentityUser
{
    [Required]
    [StringLength(11, MinimumLength = 11)]
    [RegularExpression("^[0-9]*$", ErrorMessage = "OIB smije sadržavati samo brojeve.")]
    [Display(Name = "OIB")]
    public string OIB { get; set; } = string.Empty;

    [Required]
    [StringLength(13, MinimumLength = 13)]
    [RegularExpression("^[0-9]*$", ErrorMessage = "JMBG smije sadržavati samo brojeve.")]
    [Display(Name = "JMBG")]
    public string JMBG { get; set; } = string.Empty;
}
