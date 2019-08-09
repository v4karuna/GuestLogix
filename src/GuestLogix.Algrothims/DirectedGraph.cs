using GuestLogix.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GuestLogix.Algrothims
{
    public class DirectedGraph
    {
        /// <summary>
        /// Algorithm Summary
        ///     BFS on a unweighted directed graph which is represented by an adjaceny list containing routes
        /// Steps
        /// 1 - Perform a BFS starting from origin
        ///     - Keep track of visited routes to avoid duplication
        ///     - Enqueue (adjacent node + previous route path)'s
        ///     - BFS stops when the queue is empty (this travereses the entire 'tree' like structure for the origin node
        /// 2 - Compute paths for destination using previous route paths 
        /// Notes
        ///   - This algorithm returns all possible paths from origin to destination as this provides flexibility for the caller to apply filters on routes 
        ///   - Runtime is O(V+E) where V = vertices (number of airports connected to origin) and E = edges (number of routes per airport)
        /// </summary>
        /// <param name="origin">Origin airport code</param>
        /// <param name="destination">Destination airport code</param>
        /// <param name="routes">Graph adjacency list string: airport code, list: adjacency routes</Route></param>
        /// <returns>List of itineraries</returns>
        public static IList<Itinerary> BreadthFirstSearch(string origin, string destination, Dictionary<string, List<Route>> routes)
        {
            //null checks
            if (origin == null) throw new ArgumentNullException(nameof(origin));
            if (destination == null) throw new ArgumentNullException(nameof(destination));
            if (routes == null) throw new ArgumentNullException(nameof(routes));

            var result = new List<Itinerary>();
            //invalid conditions
            if (origin == destination || !routes.ContainsKey(origin) || !routes.ContainsKey(destination))
                return result;
            //setup
            var visited = new Dictionary<Route, bool>();
            var pathTaken = new List<KeyValuePair<Route, Route>>();
            var destinationFound = false;
            var queue = new Queue<KeyValuePair<string, Route>>();
            queue.Enqueue(new KeyValuePair<string, Route>(origin, null));

            //BFS while keeping track of vistied routes and path taken to get to current node
            while (queue.Any())
            {
                var current = queue.Dequeue();
                if (current.Key == destination)
                    destinationFound = true;
                if(current.Value != null)
                {
                    if (visited[current.Value])
                        continue;
                    visited[current.Value] = true;
                }
                //enqueue adjacent routes and aviod ciruclar reference back to origin
                foreach (var route in routes[current.Key].Where(x=>x.Destination != origin))
                {
                    if (!visited.ContainsKey(route))
                    {
                        visited.Add(route, false);
                        pathTaken.Add(new KeyValuePair<Route, Route>(route, current.Value));
                        queue.Enqueue(new KeyValuePair<string, Route>(route.Destination, route));
                    }
                }
            }
            if (!destinationFound) return result;
            
            //For each matching destination, backtrack the route using the path taken to get there
            //Runtime here is neglible as the number of paths are expected to be small to get from origin->destination
            foreach (var path in pathTaken.Where(x=>x.Key.Destination == destination))
            {
                var itinerary = new Itinerary();
                var current = path;
                while (current.Value != null)
                {
                    itinerary.Connections.Add(current.Key);
                    current = pathTaken.SingleOrDefault(x => x.Key.Equals(current.Value));
                }
                itinerary.Connections.Add(current.Key);
                itinerary.Connections = itinerary.Connections.Reverse().ToList();
                result.Add(itinerary);
            }

            return result;
        }
    }
}
