using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class WeatherServiceXml
{
    private const string ApiKey = "3142c79fa6a019bc472cd9bc6ecaae26";
    private static readonly HttpClient client = new HttpClient()
    {
        Timeout = TimeSpan.FromSeconds(10) // ⏳ Sets request timeout to 10 seconds
    };

    public async Task<WeatherData?> GetWeatherXMLAsync(double lat, double lon)
    {
        try
        {
            string url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={ApiKey}&mode=xml&units=metric";
            var xml = await client.GetStringAsync(url); // 🧠 Use shared HttpClient here

            var serializer = new XmlSerializer(typeof(WeatherData));
            using var reader = new StringReader(xml);
            return (WeatherData?)serializer.Deserialize(reader);
        }
        catch (Exception ex)
       {
    	     Console.WriteLine($"Weather fetch error: {ex.Message}");
    	     return null;
       }
    }
}
