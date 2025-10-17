using Microsoft.AspNetCore.Mvc;
using ShortRouteApi.Models;
using ShortRouteApi.Services;

namespace ShortRouteApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RouteController : ControllerBase
    {
        private readonly GraphService _graphService;

        public RouteController(GraphService graphService)
        {
            _graphService = graphService;
        }

        [HttpGet("nodes")]
        public ActionResult<List<string>> GetNodes()
        {
            return Ok(_graphService.GetAllNodes());
        }

        [HttpGet]
        public ActionResult<RouteResponse> GetShortestRoute(
            [FromQuery] string start,
            [FromQuery] string end)
        {
            if (string.IsNullOrEmpty(start) || string.IsNullOrEmpty(end))
            {
                return BadRequest(new RouteResponse
                {
                    ShortestPath = new List<string>(),
                    TotalDistance = -1,
                    Message = "Start and End nodes are required"
                });
            }

            var result = _graphService.FindShortestPath(start.ToUpper(), end.ToUpper());

            if (result.TotalDistance == -1)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
    }
}