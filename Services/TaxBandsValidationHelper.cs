using FluentResults;
using TaxCalculator.Models.Shared;

namespace TaxCalculator.Services;

public static class TaxBandsValidationHelper
{


    public static Result ValidateTaxBandInput(ICollection<TaxBandModel> taxBands)
    {
        if (taxBands is null)
        {
            return Result.Fail("Invalid null input");
        }

        var sortedTaxBands = taxBands.OrderBy(x => x.BandOrder).ToArray(); ;

        for (int i = 1; i < sortedTaxBands.Count(); i++)
        {
            if (!IndividualBandCheck(sortedTaxBands[i]) || !IndividualBandCheck(sortedTaxBands[i - 1]))
            {
                return Result.Fail("Invalid tax band data");
            }
            if (!ConsecutiveBandChecks(sortedTaxBands[i - 1], sortedTaxBands[i]))
            {
                return Result.Fail("Inconsistent consecutive tax bands data");
            }
        }

        if (!CheckLastTaxBand(sortedTaxBands.Last()))
        {
            return Result.Fail("Last tax band must have null max range");
        }

        return Result.Ok();
    }

    private static bool CheckLastTaxBand(TaxBandModel taxBand)
    {
        return taxBand.MaxRange is null; 
    }

    private static bool ConsecutiveBandChecks(TaxBandModel taxBand1, TaxBandModel taxBand2)
    {

        if (taxBand2.BandOrder - taxBand1.BandOrder != 1) //validate they are consecutive
        {
            return false;
        }

        if (taxBand1.MaxRange != taxBand2.MinRange)
        {

            return false;
        }

        return true;
    }


    private static bool IndividualBandCheck(TaxBandModel taxBand)
    {
        if (taxBand.MinRange < 0 || taxBand.TaxRate < 0)
        {
            return false;
        }

        if (taxBand.MaxRange is not null && taxBand.MaxRange <= taxBand.MinRange)
        {
            return false;
        }

        return true;
    }

}