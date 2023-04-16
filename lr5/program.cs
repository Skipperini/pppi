using System;
using System.Threading.Tasks;

namespace lr5
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var apiKey = "b7143cda5aaa92052a77b35f26472f0b";
            var client = new IpstackApiClient(apiKey);

            
            var ipAddress = "8.8.8.8";
            var response = await client.Get<object>(ipAddress);

            Console.WriteLine($"GET request status code: {response.StatusCode}");
            Console.WriteLine($"GET request message: {response.Message}");
            Console.WriteLine($"GET request data: {response.Data[0]}");

            
            ipAddress = "127.0.0.1";
            response = await client.Post<object>(ipAddress);

            Console.WriteLine($"POST request status code: {response.StatusCode}");
            Console.WriteLine($"POST request message: {response.Message}");
            Console.WriteLine($"POST request data: {response.Data[0]}");
        }
    }
}
