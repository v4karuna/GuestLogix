using GuestLogix.Services;
using Microsoft.AspNetCore.Mvc;

namespace GuestLogix.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutesController : ControllerBase
    {
        private readonly IRouteService _routeService;

        public RoutesController(IRouteService routeService)
        {
            _routeService = routeService;
        }

        [HttpGet]
        public ActionResult ShortestConnectingFlights(string origin, string destination)
        {
            if (origin == null || destination == null)
                return BadRequest();

            var result = _routeService.ShortestRouteByConnectingFlights(origin, destination);

            if (result.Success)
                return Ok(result.Data);
            //keeping it simple, if necessary, different http status codes can be returned for invalid/no data etc.
            else
                return BadRequest(result.Message);
        }
    }
}