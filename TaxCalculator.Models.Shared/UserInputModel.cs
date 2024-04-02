using System.ComponentModel.DataAnnotations;

namespace TaxCalculator.Models.Shared;

public record UserInputModel
{
    [Required(ErrorMessage = "Name is required")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public required string Password { get; set; }
}
