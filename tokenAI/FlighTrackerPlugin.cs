using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCopilot.Plugins.FlightTrackerPlugin
{
    public class FlightTrackerPlugin(string apiKey)
    {
        readonly HttpClient client = new HttpClient();

        [KernelFunction, Description("Tracks the flight status of a provided source and destination")]
        [return: Description("Flight details and status")]
        public async Task<string> TrackFlightAsync(
        [Description("IATA code for the source location")] string source,
        [Description("IATA code for the designation location")] string destination,
        [Description("IATA code for the flight")] string flightNumber,
        [Description("Count of flights")] int limit)
        {
            //string apiKey = "cf62b704fb573a7dbe90976ffefb9a46"; 
            //string source = "JFK";         
            //string destination = "LAX";    
            //int limit = 10;                
            //string flightNumber = "AA100"; 
            string url = $"http://api.aviationstack.com/v1/flights?access_key={apiKey}&dep_iata={source}&arr_iata={destination}&limit={limit}&flight_iata={flightNumber}";

            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            return responseBody;
        }

    }
}