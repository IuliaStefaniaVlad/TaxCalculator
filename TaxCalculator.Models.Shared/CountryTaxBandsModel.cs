namespace TaxCalculator.Models.Shared;

public record CountryTaxBandsModel
{
    public  int? CountryTaxBandsModelId {  get; set; }
    public required string Country { get; set; }
    public required ICollection<TaxBandModel> TaxBands { get; init; }

    public override string ToString()
    {
        return CountryTaxBandsModelId.ToString() + " " + Country + "[" + string.Join(" ", TaxBands.Select(x => x.ToString())) + "]";    
    }
}
