namespace Consulta_Terrenos.Server.Models
{
    public class GoogleGeocodeResponse
    {
        public string Status { get; set; }
        public List<GeocodeResult> Results { get; set; }
    }

    public class GeocodeResult
    {
        public Geometry Geometry { get; set; }
    }

    public class Geometry
    {
        public Location Location { get; set; }
    }

    public class Location
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}
