using FluentResults;
using TaxCalculator.Models.Shared;

namespace TaxCalculator.Services.Interfaces;


public interface ITaxCalculatorService
{
    Result<CalculationResultModel> Calculate(decimal salary, string country);

}
