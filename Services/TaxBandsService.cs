using TaxCalculator.Models.Shared;
using TaxCalculator.Repositories.Interfaces;
using TaxCalculator.Services.Interfaces;
using FluentResults;

namespace TaxCalculator.Services;

public class TaxBandsService : ITaxBandsService
{
    private readonly ICountryTaxBandsRepository _countryTaxBandsRepository;

    public TaxBandsService(ICountryTaxBandsRepository countryTaxBandsRepository)
    {
        _countryTaxBandsRepository = countryTaxBandsRepository ?? throw  new ArgumentNullException(nameof(countryTaxBandsRepository));
    }

    public async Task<Result<CountryTaxBandsModel>> Add(CountryTaxBandsModel countryTaxBands)
    {
        var validationResult = TaxBandsValidationHelper.ValidateTaxBandInput(countryTaxBands.TaxBands);
        if (validationResult.IsFailed)
        {
            return validationResult;
        }

        return await _countryTaxBandsRepository.Add(countryTaxBands);
    }

    public Result<ICollection<CountryTaxBandsModel>> GetAll()
    {
        return _countryTaxBandsRepository.GetAll();
    }

    public Result<CountryTaxBandsModel> GetByCountry(string country)
    {
        return _countryTaxBandsRepository.GetByCountry(country);
    }

    public async Task<bool> Remove(int id)
    {
        return await _countryTaxBandsRepository.Remove(id);
    }

    public async Task<Result<CountryTaxBandsModel>> Update(CountryTaxBandsModel countryTaxBands)
    {
        var validationResult = TaxBandsValidationHelper.ValidateTaxBandInput(countryTaxBands.TaxBands);
        if (validationResult.IsFailed)
        {
            return validationResult;
        }
        return await _countryTaxBandsRepository.Update(countryTaxBands);
    }
}
