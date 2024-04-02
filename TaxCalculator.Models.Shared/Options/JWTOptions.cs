using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;

namespace TaxCalculator.Models.Shared.Options;

public record JWTOptions
{
    [Required]
    [ConfigurationKeyName("ValidAudience")]
    public required string ValidAudience { get; init; }
    [Required]
    [ConfigurationKeyName("ValidIssuer")]
    public required string ValidIssuer { get; init; }
    [Required]
    [ConfigurationKeyName("Secret")]
    public required string Secret { get; init; }
}
