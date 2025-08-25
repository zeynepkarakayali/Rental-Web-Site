namespace aracKiralamaDeneme.Models
{
    public class WeatherInfo
    {
        public MainInfo Main { get; set; }
        public List<WeatherDesc> Weather { get; set; }
        public string Name { get; set; }
    }
}
