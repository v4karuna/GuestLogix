using GuestLogix.Algrothims;
using GuestLogix.Model;
using GuestLogix.Services;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GuestLogix.UnitTests.Algrothims
{
    public class DirectedGraphUnitTests
    {
        private Dictionary<string, List<Route>> _routes;
        public DirectedGraphUnitTests()
        {
            //setup
            //note - simplifing setup by reusing routes/cache load from csv (not recommended in real-life scenario, as unit tests should be simple and not rely on too many dependencies)
            var csvMapper = new CsvMapper();
            var memoryCacheOptions = new MemoryCacheOptions();
            var cache = new MemoryCache(memoryCacheOptions);
            var cacheManager = new CacheManager(cache, csvMapper);
            cacheManager.Load();
            _routes = (Dictionary<string, List<Route>>) cache.Get(Constants.CacheKeys.RoutesGraph);
        }

        [Theory]
        [InlineData("YYZ","JFK","YYZ-JFK")]
        [InlineData("JFK", "YYZ", "JFK-YYZ")]
        [InlineData("LAX", "YVR", "LAX-YVR")]
        [InlineData("YVR", "LAX", "YVR-LAX")]
        [InlineData("LAX", "JFK", "LAX-JFK")]
        [InlineData("JFK", "LAX", "JFK-LAX")]
        [InlineData("JFK", "ABC", "JFK-ABC")]
        [InlineData("LAX", "ABC", "LAX-ABC")]
        [InlineData("YVR", "ABC", "YVR-ABC")]
        [InlineData("JFK", "ORD", "JFK-ORD")]
        [InlineData("LAX", "ORD", "LAX-ORD")]
        [InlineData("YYZ","YVR","YYZ-JFK-LAX-YVR")]
        public void ValidSingleRoutes(string origin, string destination, string expected)
        {
            var itineraries = DirectedGraph.BreadthFirstSearch(origin, destination, _routes).OrderBy(x => x.Connections.Count).ToArray();
            Assert.Equal(expected, itineraries[0].ToString());
        }

        [Fact]
        public void ValidMultipleRoutes()
        {
            var itineraries = DirectedGraph.BreadthFirstSearch("YYZ", "ABC", _routes);
            Assert.Equal(3, itineraries.Count);

            var results = itineraries.OrderBy(x => x.Connections.Count).ToArray();
            Assert.Equal("YYZ-JFK-ABC", results[0].ToString());
            Assert.Equal("YYZ-JFK-LAX-ABC", results[1].ToString());
            Assert.Equal("YYZ-JFK-LAX-YVR-ABC", results[2].ToString());

            itineraries = DirectedGraph.BreadthFirstSearch("YYZ", "ORD", _routes);
            Assert.Equal(2, itineraries.Count);

            results = itineraries.OrderBy(x => x.Connections.Count).ToArray();
            Assert.Equal("YYZ-JFK-ORD", results[0].ToString());
            Assert.Equal("YYZ-JFK-LAX-ORD", results[1].ToString());
        }

        [Theory]
        [InlineData("yyz", "JFK")] //case sensitivity
        [InlineData("XXX", "JFK")] //invalid origin
        [InlineData("YYZ", "XXX")] //invalid destination
        [InlineData("XXX", "YYY")] //invalid origin and destination
        [InlineData("", "")]       //empty origin and destination
        public void InvalidRoutes(string origin, string destination)
        {
            var itineraries = DirectedGraph.BreadthFirstSearch(origin, destination, _routes);
            Assert.Empty(itineraries);
        }


        [Theory]
        [InlineData(null, "JFK")] 
        [InlineData("JFK", null)] 
        [InlineData(null, null)] 
        public void NullChecks(string origin, string destination)
        {
            Assert.Throws<ArgumentNullException>(()=> DirectedGraph.BreadthFirstSearch(origin, destination, _routes));
        }



    }
}
