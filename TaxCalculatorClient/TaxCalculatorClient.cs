using FluentResults;
using Microsoft.Extensions.Options;
using TaxCalculator.Client.Interfaces;
using TaxCalculator.Models.Shared;
using TaxCalculator.Models.Shared.Options;

namespace TaxCalculator.Client
{
    public class TaxCalculatorClient : ITaxCalculatorClient
    {
        private readonly HttpClient _client;
        private readonly IOptions<UriOptions> _options;

        public TaxCalculatorClient(IOptions<UriOptions> options)
        {
            _client = new HttpClient();
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }
        public async Task<Result<CalculationResultModel>> Calculate(decimal grossAnnualSalary, string countryName)
        {
            using HttpResponseMessage response = await _client.GetAsync($"{_options.Value.BaseUri}/TaxCalculator/Calculate?salary={grossAnnualSalary}&country={countryName}");
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return Result.Ok(await response.Content.ReadFromJsonAsync<CalculationResultModel>());

                }
                catch (Exception ex)
                {
                    return Result.Fail(ex.Message);
                }
            }
            return Result.Fail("Could not calculate data");
        }
    }
}
