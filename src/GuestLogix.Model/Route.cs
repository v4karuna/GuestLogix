using System.Collections.Generic;
using System;

namespace GuestLogix.Model
{
    public class Route: IEquatable<Route>
    {
        public virtual string AirlineId { get; set; }
        public virtual string Origin { get; set; }
        public virtual string Destination { get; set; }
        public virtual Airline Airline { get; set; }
        public virtual Airport OriginAirport { get; set; }
        public virtual Airport DestinationAirport { get; set; }
        public virtual Dictionary<string, object> Properties { get; set; }

        public bool Equals(Route other)
        {
            return (other == null) || (other.AirlineId == AirlineId && other.Origin == Origin && other.Destination == Destination);
        }
        public override string ToString()
        {
            return Origin + "-" + Destination;
        }
    }

}
