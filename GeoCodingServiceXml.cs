using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class GeoCodingServiceXml
{
    private const string ApiKey = "3142c79fa6a019bc472cd9bc6ecaae26";

    public async Task<(double lat, double lon)> GetCoordinatesAsync(string city)
    {
         	try
        	{
                string url = $"http://api.openweathermap.org/geo/1.0/direct?q={city}&limit=1&appid={ApiKey}&mode=xml";
            		using var client = new HttpClient();
            		var xml = await client.GetStringAsync(url);
    
           		 var serializer = new XmlSerializer(typeof(Locations));
            		using var reader = new StringReader(xml);
           		 var result = (Locations?)serializer.Deserialize(reader);
            		var location = result?.LocationList?.FirstOrDefault();
            		return location != null ? (location.Latitude, location.Longitude) : (0, 0);
        	}
        	catch
       	  {
                return (0, 0);
       	 }
    }
}
