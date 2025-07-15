using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;        
using System.Diagnostics; 

public class SavedLocationManager
{
    private readonly string filePath = "savedCities.json";

    public List<string> LoadSavedCities()
    {
        if (!File.Exists(filePath)) return new List<string>();
        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
    }

    public void SaveCity(string city)
    {
        var cities = LoadSavedCities();
        
        if (!cities.Contains(city, StringComparer.OrdinalIgnoreCase))
        {
            cities.Add(city);
            File.WriteAllText(filePath, JsonSerializer.Serialize(cities));
        }
    }
}

