using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaxCalculator.Repositories.Entities;

public class CountryTaxBandsEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CountryTaxBandsId { get; set; }
    public required string Country { get; set; }
    public required ICollection<TaxBandEntity> TaxBands { get; init; }
}
