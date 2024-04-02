using FluentResults;
using System.Text.Json;
using System.Text;
using TaxCalculator.Client.Interfaces;
using TaxCalculator.Models.Shared;
using Microsoft.Extensions.Options;
using TaxCalculator.Models.Shared.Options;

namespace TaxCalculator.Client
{
    public class LoginClient : ILoginClient
    {
        private readonly HttpClient _client;
        private readonly IOptions<UriOptions> _options;
        public LoginClient(IOptions<UriOptions> options)
        {
            _client = new HttpClient();
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }
        public async Task<Result<LoginResultModel>> Login(UserInputModel input)
        {
            using StringContent jsonContent = new(JsonSerializer.Serialize(input),
                                                Encoding.UTF8,
                                                "application/json");
            using HttpResponseMessage response = await _client.PostAsync($"{_options.Value.BaseUri}/Authentication/login", jsonContent);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return Result.Ok(await response.Content.ReadFromJsonAsync<LoginResultModel>());

                }
                catch (Exception)
                {
                    return Result.Fail(response.Content.ReadAsStringAsync().Result);
                }
            }
            return Result.Fail("Could not authenticate");
        }
    }
}
