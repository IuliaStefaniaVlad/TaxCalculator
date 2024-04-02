using TaxCalculator.Models.Shared;
namespace TaxCalculator.Services.Interfaces;
using FluentResults;

public interface ITaxBandsService
{
    Result<CountryTaxBandsModel> GetByCountry(string country);
    Task<Result<CountryTaxBandsModel>> Add(CountryTaxBandsModel countryTaxBands);
    Task<Result<CountryTaxBandsModel>> Update(CountryTaxBandsModel countryTaxBands);
    Task<bool> Remove(int id);
    Result<ICollection<CountryTaxBandsModel>> GetAll();
}
