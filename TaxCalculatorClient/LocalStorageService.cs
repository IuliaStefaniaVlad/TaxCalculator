using Microsoft.JSInterop;
using System.Text.Json;

namespace TaxCalculator.Client
{

    //NOTE: https://stackoverflow.com/questions/63698112/storing-a-jwt-token-in-blazor-client-side
    // The ISRuntime is used to actually tun specific Javascript scripts.
    // In this situations we're using it to write and read from the sessionStorage.
    // I could've done it differently and used the default Blazor stuff, but that code only works in partial classes that represent the code behind of the components
    // And I wanted to use this directly within a service (that ultimately runs within a component anyway).
    public interface ILocalStorageService
    {
        Task<T> GetItem<T>(string key);
        Task SetItem<T>(string key, T value);
        Task RemoveItem(string key);
    }

    public class LocalStorageService : ILocalStorageService
    {
        private IJSRuntime _jsRuntime;

        public LocalStorageService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<T> GetItem<T>(string key)
        {
            try
            {
                var json = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", key);

                if (json == null)
                    return default;

                return JsonSerializer.Deserialize<T>(json);
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        public async Task SetItem<T>(string key, T value)
        {
            await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", key, JsonSerializer.Serialize(value));
        }

        public async Task RemoveItem(string key)
        {
            await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", key);
        }
    }
}