using TaxCalculator.Models.Shared;
using FluentResults;

namespace TaxCalculator.Repositories.Interfaces;


public interface ICountryTaxBandsRepository
{
    Result<CountryTaxBandsModel> GetByCountry(string country);
    Task<Result<CountryTaxBandsModel>> Add(CountryTaxBandsModel countryTaxBands);
    Task<Result<CountryTaxBandsModel>> Update(CountryTaxBandsModel countryTaxBands);
    Task<bool> Remove(int id);
    Result<ICollection<CountryTaxBandsModel>> GetAll();
}
