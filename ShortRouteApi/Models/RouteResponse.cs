namespace ShortRouteApi.Models
{
    public class RouteResponse
    {
        public List<string> ShortestPath { get; set; }
        public int TotalDistance { get; set; }
        public string Message { get; set; }
    }
}