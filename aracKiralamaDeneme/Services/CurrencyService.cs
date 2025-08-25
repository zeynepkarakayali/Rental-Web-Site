using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using aracKiralamaDeneme.Models;

namespace aracKiralamaDeneme.Services
{
    public class CurrencyService
    {
        private readonly HttpClient _httpClient;

        public CurrencyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CurrencyRate?> GetRatesAsync(string from = "USD", string to = "TRY,EUR")
        {
            var url = $"https://api.frankfurter.app/latest?from={from}&to={to}";
            var response = await _httpClient.GetStringAsync(url);

            return JsonSerializer.Deserialize<CurrencyRate>(response,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
