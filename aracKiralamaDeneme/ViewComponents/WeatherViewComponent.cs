using aracKiralamaDeneme.Services;
using Microsoft.AspNetCore.Mvc;

public class WeatherViewComponent : ViewComponent
{
    private readonly WeatherService _weatherService;

    public WeatherViewComponent(WeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    public async Task<IViewComponentResult> InvokeAsync(string city)
    {
        var weather = await _weatherService.GetWeatherAsync(city);
        return View(weather);
    }
}
