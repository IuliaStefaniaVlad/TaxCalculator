using FluentResults;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using TaxCalculator.Client.Interfaces;
using TaxCalculator.Models.Shared;
using Microsoft.Extensions.Options;
using TaxCalculator.Models.Shared.Options;

namespace TaxCalculator.Client
{
    public class TaxBandsClient : ITaxBandsClient
    {
        private readonly HttpClient _client;
        private readonly ILocalStorageService _localStorageService;
        private readonly IOptions<UriOptions> _options;
        public TaxBandsClient(ILocalStorageService localStorageService, IOptions<UriOptions> options)
        {
            _client = new HttpClient();
            _localStorageService = localStorageService ?? throw new ArgumentNullException(nameof(localStorageService));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }
        public async Task<Result<ICollection<CountryTaxBandsModel>>> GetAllTaxBands()
        {
            //_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getToken());
            using HttpResponseMessage response = await _client.GetAsync($"{_options.Value.BaseUri}/TaxBands/GetAllTaxBands");
            if(response.IsSuccessStatusCode)
            {
                try
                {
                    return Result.Ok(await response.Content.ReadFromJsonAsync<ICollection<CountryTaxBandsModel>>());

                }
                catch (Exception ex)
                {
                    return Result.Fail(ex.Message);
                }
            }
            return Result.Fail(response.Content.ReadAsStringAsync().Result);
        }


        public async Task<Result<CountryTaxBandsModel>> CreateTaxBands(CountryTaxBandsModel inputData)
        {
            using StringContent jsonContent = new( JsonSerializer.Serialize(inputData),
                                                Encoding.UTF8,
                                                "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getToken());
            using HttpResponseMessage response = await _client.PostAsync($"{_options.Value.BaseUri}/TaxBands/CreateTaxBands",jsonContent);
            if(response.IsSuccessStatusCode)
            {
                try
                {
                    return Result.Ok(await response.Content.ReadFromJsonAsync<CountryTaxBandsModel>());

                }
                catch (Exception ex)
                {
                    return Result.Fail(response.Content.ReadAsStringAsync().Result);
                }
            }
            return Result.Fail(response.Content.ReadAsStringAsync().Result);
        }

        public async Task<Result<CountryTaxBandsModel>> UpdateTaxBands(CountryTaxBandsModel inputData)
        {
            using StringContent jsonContent = new(JsonSerializer.Serialize(inputData),
                                                Encoding.UTF8,
                                                "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getToken());
            using HttpResponseMessage response = await _client.PutAsync($"{_options.Value.BaseUri}/TaxBands/UpdateTaxBands", jsonContent);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return Result.Ok(await response.Content.ReadFromJsonAsync<CountryTaxBandsModel>());

                }
                catch (Exception ex)
                {
                    return Result.Fail(response.Content.ReadAsStringAsync().Result);
                }
            }
            return Result.Fail(response.Content.ReadAsStringAsync().Result);
        }

        public async Task<Result> DeleteTaxBands(int id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getToken());
            using HttpResponseMessage response = await _client.DeleteAsync($"{_options.Value.BaseUri}/TaxBands/DeleteTaxBands?id={id}");
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return Result.Ok();

                }
                catch (Exception ex)
                {
                    return Result.Fail(response.Content.ReadAsStringAsync().Result);
                }
            }
            return Result.Fail(response.Content.ReadAsStringAsync().Result);
        }
        private async Task<string> getToken()
        {
            var result = await _localStorageService.GetItem<LoginResultModel>("login");
            return result.Token;
        }

    }


}
