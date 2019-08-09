using System;
using System.Collections.Generic;

namespace GuestLogix.Model
{
    public class Airport : IEquatable<Airport>
    {
        public virtual string Name { get; set; }

        public virtual string City { get; set; }

        public virtual string Country { get; set; }

        public virtual string IATA3 { get; set; }

        public virtual double Latitute { get; set; }

        public virtual double Longitude { get; set; }

        public virtual IList<Route> Connections { get; set; }

        public Airport()
        {
            Connections = new List<Route>();
        }

        public bool Equals(Airport other)
        {
            return (other.IATA3 == IATA3);
        }
    }
}
