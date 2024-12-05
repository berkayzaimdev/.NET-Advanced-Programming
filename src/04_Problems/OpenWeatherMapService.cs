using System.Net;

namespace _04_Problems;

public class OpenWeatherMapService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public OpenWeatherMapService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<WeatherResponse?> GetWeatherForCityAsync(string city, string units)
    {
        var client = _httpClientFactory.CreateClient();

        var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&units={units}&appid=";

        var weatherResponse = await client.GetAsync(url);

        if (weatherResponse.StatusCode.Equals(HttpStatusCode.NotFound))
        {
            return default;
        }

        var weather = await weatherResponse.Content.ReadFromJsonAsync<WeatherForecast>();

        return new WeatherResponse
        {
            Temperature = weather!.Temperature,
            FeelsLike = weather!.Summary
        };
    }
}

public class WeatherResponse    
{
    public required string Temperature { get; set; }
    public required string FeelsLike { get; set; }
}

public class WeatherForecast
{
    public required string Temperature { get; set; }

    public required string Summary { get; set; }
}
