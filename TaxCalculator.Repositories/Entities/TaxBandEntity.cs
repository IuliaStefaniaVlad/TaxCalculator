using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TaxCalculator.Repositories.Entities
{
    public class TaxBandEntity
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int? TaxBandId { get; set; }
        public int BandOrder {  get; set; }
        public int MinRange { get; set; }
        public int? MaxRange { get; set;}
        public int TaxRate { get; set; }
        public int CountryTaxBandId { get; set; }
        public CountryTaxBandsEntity CountryTaxBand{ get; set; }

    }
}
