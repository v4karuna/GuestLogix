using GuestLogix.Model;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;

namespace GuestLogix.Services
{
    public sealed class CacheManager : ICacheManager
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ICsvMapper _csvMapper;
        public CacheManager(IMemoryCache memoryCache, ICsvMapper csvMapper)
        {
            _memoryCache = memoryCache;
            _csvMapper = csvMapper;
        }

        public void Load()
        {
            //load csv files
            var routesFilePath = "Resources\\routes.csv";
            var airlinesFilePath = "Resources\\airlines.csv";
            var airportsFilePath = "Resources\\airports.csv";

            var airlines = _csvMapper.ParseRecords<Airline>(airlinesFilePath, new AirlineMap()).ToDictionary(x => x.TwoDigitCode);
            var airports = _csvMapper.ParseRecords<Airport>(airportsFilePath, new AirportMap()).ToDictionary(x => x.IATA3);
            var routes = _csvMapper.ParseRecords<Route>(routesFilePath, new RouteMap(airlines, airports));


            //build the directed graph (adjacency list)
            //string = airport code, list = adjacency routes 
            var graph = new Dictionary<string, List<Route>>();

            foreach (var route in routes)
            {
                var origin = route.Origin;
                var destination = route.Destination;
                var airline = route.AirlineId;

                //add airport and associated routes
                if (graph.ContainsKey(origin))
                    graph[origin].Add(route);
                else
                    graph.Add(origin, new List<Route> { route });

                //some destination airports have no (outgoing) routes
                if (!graph.ContainsKey(destination))
                    graph.Add(destination, new List<Route>());
            }

            _memoryCache.Set(Constants.CacheKeys.RoutesGraph, graph);
        }
    }
}
