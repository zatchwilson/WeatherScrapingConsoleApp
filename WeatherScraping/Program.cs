using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static System.Net.WebRequestMethods;

namespace WeatherScraping
{
    public class WeatherData
    {
        public Location location;
        public Current current;
    }

    public class Location
    {
        public string Name { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }

        public double Lat { get; set; }
        public double Lon { get; set; }

        public string Tz_id { get; set; }
        public long Localtime_epoch { get; set; }
        public string Localtime { get; set; }
    }

    public class Current
    {
        public long Last_updated_epoch { get; set; }
        public string Last_updated { get; set; }
        public double Temp_c { get; set; }
        public double Temp_f { get; set; }
        public bool Is_day { get; set; }

        public Condition Condition { get; set; }

        public double Wind_mph { get; set; }
        public double Wind_kph { get; set; }
        public double Wind_degree { get; set; }
        public string Wind_dir { get; set; }

        public double Pressure_mb { get; set; }
        public double Pressure_in { get; set; }

        public double Precip_mm { get; set; }
        public double Precip_in { get; set; }

        public double Humidity { get; set; }

        public double Cloud { get; set; }

        public double Feelslike_c { get; set; }
        public double Feelslike_f { get; set; }

        public double Vis_km { get; set; }
        public double Vis_miles { get; set; }

        public double Uv { get; set; }
        public double Gust_mph { get; set; }
        public double Gust_kph { get; set; }
    }

    public class Condition
    {
        public string Text { get; set; }
        public string Icon { get; set; }
        public int Code { get; set; }
    }

    class Program
    {
        public static WeatherData wData = new WeatherData();
        public static string rqUri;
        
        static void Main(string[] args)
        {
            Console.WriteLine("Hi! Please enter your zip code!");

            rqUri = "https://weatherapi-com.p.rapidapi.com/current.json?q=" + Console.ReadLine();

            wData = StartClient();
            
            Console.WriteLine("Your Location: " + wData.location.Name);
            Console.WriteLine("The current Temperature (F): " + wData.current.Temp_f);
            Console.WriteLine("It currently feels like: " + wData.current.Feelslike_f);
            Console.WriteLine("There is a " + wData.current.Cloud + "% of clouds in the area");
            Console.Read();
        }

        public static WeatherData StartClient()
        {
            WeatherData data = null;
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(rqUri),
                Headers =
                {
                    { "X-RapidAPI-Key", "a2584aa4b0msh56516d8ad68eb5ep1f2368jsn9af9ec5323de" },
                    { "X-RapidAPI-Host", "weatherapi-com.p.rapidapi.com" },
                },
            };

            data = TestClient(client, request, data).Result;

            return data;
        }


        static async Task<WeatherData> TestClient(HttpClient client, HttpRequestMessage request, WeatherData data)
        {
            //WeatherData data = null;
            //var client = new HttpClient();
            //var request = new HttpRequestMessage
            //{
            //    Method = HttpMethod.Get,
            //    RequestUri = new Uri("https://weatherapi-com.p.rapidapi.com/current.json?q=84037"),
            //    Headers =
            //    {
            //        { "X-RapidAPI-Key", "a2584aa4b0msh56516d8ad68eb5ep1f2368jsn9af9ec5323de" },
            //        { "X-RapidAPI-Host", "weatherapi-com.p.rapidapi.com" },
            //    },
            //};

            try
            {


                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    data = JsonConvert.DeserializeObject<WeatherData>(body);

                }
            }

            catch
            {
                Console.WriteLine("The information that was input is not valid.");
            }

            return data;
        }

        public void FinishedWithConnection(string json)
        {

        }

        public WeatherData ParseJson(string json)
        {
            WeatherData data = JsonConvert.DeserializeObject<WeatherData>(json);
            return data;
        }
    }
}
