namespace TaxCalculator.Models.Shared;


//This uses set instead of init to reuse the model in UI and be able to update values when creating see table code
public record TaxBandModel
{
    public int BandOrder { get; set; }
    public int MinRange { get; set; }
    public int? MaxRange { get; set; }
    public int TaxRate { get; set; }

    public override string ToString()
    {
        return $"{{ BandOrder:{BandOrder}, MinRange:{MinRange}, MaxRange:{MaxRange}, TaxRate:{TaxRate}}}";
    }
}
