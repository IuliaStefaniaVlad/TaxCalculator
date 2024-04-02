using FluentResults;
using TaxCalculator.Models.Shared;

namespace TaxCalculator.Client.Interfaces
{
    public interface ITaxCalculatorClient
    {
        public Task<Result<CalculationResultModel>> Calculate(decimal grossAnnualSalary, string countryName);
    }
}
