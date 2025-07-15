
[XmlRoot("locations")]
public class Locations
{
    [XmlElement("location")]
    public List<Location> LocationList { get; set; }
}

public class Location
{
    [XmlElement("name")]
    public string Name { get; set; }

    [XmlElement("lat")]
    public double Latitude { get; set; }

    [XmlElement("lon")]
    public double Longitude { get; set; }

    [XmlElement("country")]
    public string Country { get; set; }

    [XmlElement("state")]
    public string State { get; set; }
}
