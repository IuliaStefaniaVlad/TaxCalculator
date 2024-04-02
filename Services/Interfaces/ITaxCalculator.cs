using FluentResults;
using TaxCalculator.Models.Shared;

namespace TaxCalculator.Services.Interfaces;

public interface ITaxCalculator
{
    Result<CalculationResultModel> Calculate(decimal salary, ICollection<TaxBandModel> taxBands);
}
