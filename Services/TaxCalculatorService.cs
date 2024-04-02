using FluentResults;
using TaxCalculator.Models.Shared;
using TaxCalculator.Repositories.Interfaces;
using TaxCalculator.Services.Interfaces;

namespace TaxCalculator.Services
{
    public class TaxCalculatorService : ITaxCalculatorService
    {
        private readonly ICountryTaxBandsRepository _countryTaxBandsRepository;
        private readonly ITaxCalculator _taxCalculator;

        public TaxCalculatorService(ICountryTaxBandsRepository countryTaxBandsRepository, ITaxCalculator taxCalculator)
        {
            _countryTaxBandsRepository = countryTaxBandsRepository ?? throw new ArgumentNullException(nameof(countryTaxBandsRepository));
            _taxCalculator = taxCalculator ?? throw new ArgumentNullException(nameof(taxCalculator));
        }

        public Result<CalculationResultModel> Calculate(decimal salary, string country)
        {
            var taxBandsResult = _countryTaxBandsRepository.GetByCountry(country);

            if(taxBandsResult.IsFailed)
            {
                return Result.Fail(taxBandsResult.Errors);
            }

            return _taxCalculator.Calculate(salary, taxBandsResult.Value.TaxBands);
        }
    }
}
