using GuestLogix.Algrothims;
using GuestLogix.Model;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;

namespace GuestLogix.Services
{
    public sealed class RouteService : IRouteService
    {
        private readonly Dictionary<string, List<Route>> _routes;

        public RouteService(IMemoryCache memoryCache)
        {
            _routes = (Dictionary<string, List<Route>>) memoryCache.Get(Constants.CacheKeys.RoutesGraph);
        }

        public SearchResult<string[]> ShortestRouteByConnectingFlights(string origin, string destination)
        {
            origin = origin?.ToUpper();
            destination = destination?.ToUpper();

            //validate origin and destination
            var result = new SearchResult<string[]>();
            if(!_routes.ContainsKey(origin))
            {
                result.Message = Constants.SearchResult.InvalidOrigin;
                return result;
            }
            if(!_routes.ContainsKey(destination))
            {
                result.Message = Constants.SearchResult.InvalidDestination;
                return result;
            }

            //compute itineraries
            var itineraries = DirectedGraph.BreadthFirstSearch(origin, destination, _routes);

            //return first matching itinerary by least number of connection flights
            if (itineraries.Any())
            {
                result.Data = itineraries.OrderBy(x => x.Connections.Count).First().ToRouteArray();
                result.Success = true;
            }
            else
                result.Message = Constants.SearchResult.NoRoute;

            return result;
        }

    }
}
