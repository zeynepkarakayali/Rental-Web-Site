using aracKiralamaDeneme.Models;
using System.Text.Json;

namespace aracKiralamaDeneme.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey; // OpenWeatherMap API key

        public WeatherService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["ApiKeys:OpenWeatherMap"];
        }

        //public async Task<dynamic> GetWeatherAsync(string city)
        //{
        //    var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&units=metric&lang=tr&appid={_apiKey}";
        //    var response = await _httpClient.GetStringAsync(url);
        //    return System.Text.Json.JsonSerializer.Deserialize<dynamic>(response);
        //}
        public async Task<WeatherInfo> GetWeatherAsync(string city)
        {
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&units=metric&lang=tr&appid={_apiKey}";
            var response = await _httpClient.GetStringAsync(url);

            return JsonSerializer.Deserialize<WeatherInfo>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }

}
