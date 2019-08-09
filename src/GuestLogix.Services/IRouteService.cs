using GuestLogix.Model;

namespace GuestLogix.Services
{
    public interface IRouteService
    {
        /// <summary>
        /// Computes the shortest route between origin and destination by number of connection flights
        /// </summary>
        /// <param name="origin">Origin airport code</param>
        /// <param name="destination">Destination airport code</param>
        /// <returns>Search Result of data type string[]</returns>
        SearchResult<string[]> ShortestRouteByConnectingFlights(string origin, string destination);
    }
}
