using System.Collections.Generic;

namespace GuestLogix.Model
{
    public class Itinerary
    {
        public IList<Route> Connections { get; set; }

        public Itinerary()
        {
            Connections = new List<Route>();
        }

        public string[] ToRouteArray()
        {
            return this.ToString().Split(new char[] { '-' });
        }

        public override string ToString()
        {
            var result = string.Empty;
            
            for(int i = 0; i < Connections.Count; i++)
            {
                if (i == 0)
                    result = Connections[0].ToString();
                else
                    result += "-" + Connections[i].Destination;
            }

            return result;
        }
    }
}
