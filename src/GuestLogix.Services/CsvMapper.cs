using CsvHelper;
using CsvHelper.Configuration;
using GuestLogix.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GuestLogix.Services
{
    /// <summary>
    /// CsvParser using nuget package 'CsvHelper'
    /// </summary>
    /// <typeparam name="T">T model type</typeparam>
    public class CsvMapper: ICsvMapper
    {
        public IList<T> ParseRecords<T>(string filePath, ClassMap classMap = null)
        {
            var records = new List<T>();

            using (var reader = new StreamReader(filePath))
            using (var csvReader = new CsvReader(reader))
            {
                if (classMap != null)
                    csvReader.Configuration.RegisterClassMap(classMap);

                records = csvReader.GetRecords<T>().ToList();
            }

            return records;
        }
    }

    public class AirportMap : ClassMap<Airport>
    {
        public AirportMap()
        {
            Map(m => m.Name).Name("Name");
            Map(m => m.City).Name("City");
            Map(m => m.Country).Name("Country");
            Map(m => m.IATA3).Name("IATA 3");
            Map(m => m.Latitute).Name("Latitute");
            Map(m => m.Longitude).Name("Longitude");
        }
    }

    public class RouteMap : ClassMap<Route>
    {
        public RouteMap(Dictionary<string, Airline> airlines, Dictionary<string, Airport> airports)
        {
            Map(m => m.AirlineId).Name("Airline Id");
            Map(m => m.Origin).Name("Origin");
            Map(m => m.Destination).Name("Destination");
            Map(m => m.OriginAirport).ConvertUsing(row => airports[row.GetField("Origin")]);
            Map(m => m.DestinationAirport).ConvertUsing(row => airports[row.GetField("Destination")]);
            Map(m => m.Airline).ConvertUsing(row => airlines[row.GetField("Airline Id")]);
        }
    }

    public class AirlineMap : ClassMap<Airline>
    {
        public AirlineMap()
        {
            Map(m => m.Name).Name("Name");
            Map(m => m.TwoDigitCode).Name("2 Digit Code");
            Map(m => m.ThreeDigitCode).Name("3 Digit Code");
            Map(m => m.Country).Name("Country");
        }
    }
}
