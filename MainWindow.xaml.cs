using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WeatherViewer
{
  public partial class MainWindow : Window
  {
    private readonly GeoCodingServiceXml geoService = new GeoCodingServiceXml();
    private readonly WeatherServiceXml weatherService = new WeatherServiceXml();
    private readonly SavedLocationManager savedManager = new SavedLocationManager();

      public MainWindow()
      {
          InitializeComponent();
          LoadSavedLocations();
      }

       private async void GetWeather_Click(object sender, RoutedEventArgs e)
      {
        	string input = CityTextBox.Text.Trim();
       	  if (string.IsNullOrWhiteSpace(input))
        	{
            		WeatherInfo.Text = "❗ Please enter a valid city name.";
            		return;
        	}
          await FetchAndDisplayWeatherAsync(input, true);        	
     }

    private async Task FetchAndDisplayWeatherAsync(string input, bool saveCity)
    {
        
        SavedList.IsEnabled = false; 
        StatusText.Text = "🔍 Getting coordinates...";
        var (lat, lon) = await geoService.GetCoordinatesAsync(input);
    
        if (lat == 0 && lon == 0)
        {
            WeatherInfo.Text = "⚠️ Could not find coordinates.";
            StatusText.Text = "";
            SavedList.IsEnabled = true;
            return;
        }
    
        StatusText.Text = "⏳ Fetching weather data...";
        WeatherInfo.Text = "";
    
        var weather = await weatherService.GetWeatherXmlAsync(lat, lon);
        if (weather == null)
        {
            WeatherInfo.Text = "⚠️ Weather data not found. Check the city or network.";
        }
        else
        {
            WeatherInfo.Text = $"📍 {weather.City.Name}, {weather.City.Country}\n" +
                               $"🌡️ Temp: {weather.Temperature.Value}°{weather.Temperature.Unit}\n" +
                               $"🌥️ Condition: {weather.Weather.Description}\n" +
                               $"💨 Wind: {weather.Wind.Speed.Value} {weather.Wind.Speed.Unit}  
                                         ({weather.Wind.Direction.Name})\n" +
                               $"🌅 Sunrise: {weather.City.Sun.Rise:t} — 🌇 Sunset: 
                                      {weather.City.Sun.Set:t}";
        }
    
       if (saveCity)
        {
          savedManager.SaveCity(input);
        }
    
        LoadSavedLocations();
        StatusText.Text = "";
        SavedList.IsEnabled = true;
    }

     private void LoadSavedLocations()
     {
            var sortedCities = savedManager.LoadSavedCities()
                                       .OrderBy(c => c, StringComparer.OrdinalIgnoreCase)
                                       .ToList();
    
            SavedList.ItemsSource = sortedCities;
     }

      private async void SavedList_SelectionChanged(object sender, SelectionChangedEventArgs e)
     {
          if (SavedList.SelectedItem is string cityEntry)
          {
            await FetchAndDisplayWeatherAsync(cityEntry, false);
          }
         SavedList.SelectedItem = null;
  
    }
  }
}


