using FluentResults;
using TaxCalculator.Models.Shared;

namespace TaxCalculator.Client.Interfaces
{
    public interface ITaxBandsClient
    {
        public Task<Result<ICollection<CountryTaxBandsModel>>> GetAllTaxBands();
        public Task<Result<CountryTaxBandsModel>> CreateTaxBands(CountryTaxBandsModel inputData);
        public Task<Result<CountryTaxBandsModel>> UpdateTaxBands(CountryTaxBandsModel inputData);
        public Task<Result> DeleteTaxBands(int id);
    }
}
