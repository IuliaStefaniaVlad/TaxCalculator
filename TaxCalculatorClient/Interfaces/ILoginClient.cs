using FluentResults;
using TaxCalculator.Models.Shared;

namespace TaxCalculator.Client.Interfaces
{
    public interface ILoginClient
    {
        public Task<Result<LoginResultModel>> Login(UserInputModel input);
    }
}
