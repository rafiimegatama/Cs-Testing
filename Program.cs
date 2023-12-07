using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WeatherForecastApplication
{
    public class WeatherForecastService
    {
        public async Task<double> GetForecast(double latitude, double longitude)
        {
            string url = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}";
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    dynamic data = JsonConvert.DeserializeObject(json);
                    // Replace "forecast" with the actual property name in the API response.
                    return data.forecast;
                }
                else
                {
                    throw new Exception("Failed to get forecast from API.");
                }
            }
        }

        public async Task<double[]> GetSevenDayForecast(double latitude, double longitude)
        {
            string url = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&daily=temperature";
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    dynamic data = JsonConvert.DeserializeObject(json);
                    double[] sevenDayForecast = new double[7];
                    for (int i = 0; i < 7; i++)
                    {
                        sevenDayForecast[i] = data.forecast.daily[i].temperature;
                    }
                    return sevenDayForecast;
                }
                else
                {
                    throw new Exception("Failed to get forecast from API.");
                }
            }
        }

        public double CalculateAverageTemperature(int[] temperatures)
        {
            double total = 0;
            foreach (int temperature in temperatures)
            {
                total += temperature;
            }
            return total / temperatures.Length;
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            WeatherForecastService weatherForecast = new WeatherForecastService();

            double forecast = await weatherForecast.GetForecast(51.5074, -0.1278);
            Console.WriteLine($"The forecast for London is {forecast} degrees.");

            int[] temperatures = { 20, 25, 23, 22, 21, 24, 23 };
            double average = weatherForecast.CalculateAverageTemperature(temperatures);
            Console.WriteLine($"The average temperature is {average} degrees.");
        }
    }
}

// Test code remains the same as provided by you.
