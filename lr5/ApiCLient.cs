using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace lr5
{
    public class ApiResponse<T>
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public List<T> Data { get; set; }
    }

    public class IpstackApiClient
    {
        private readonly string apiKey;
        private readonly HttpClient httpClient;

        public IpstackApiClient(string apiKey)
        {
            this.apiKey = apiKey;
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://api.ipstack.com/");
        }

        public async Task<ApiResponse<T>> Get<T>(string ipAddress)
        {
            var url = $"{ipAddress}?access_key={this.apiKey}";

            try
            {
                var response = await httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new ApiResponse<T>
                    {
                        StatusCode = (int)response.StatusCode,
                        Message = response.ReasonPhrase
                    };
                }

                var data = JsonSerializer.Deserialize<T>(content);
                return new ApiResponse<T>
                {
                    StatusCode = (int)response.StatusCode,
                    Data = new List<T> { data }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ApiResponse<T>
                {
                    StatusCode = 500,
                    Message = "Internal server error"
                };
            }
        }

        public async Task<ApiResponse<T>> Post<T>(string ipAddress)
        {
            var url = $"http://api.ipstack.com/check?access_key={this.apiKey}";
            var data = new Dictionary<string, string>
            {
                { "ip", ipAddress }
            };

            try
            {
                var json = JsonSerializer.Serialize(data);
                var content = new StringContent(json);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await httpClient.PostAsync(url, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new ApiResponse<T>
                    {
                        StatusCode = (int)response.StatusCode,
                        Message = response.ReasonPhrase
                    };
                }

                var responseData = JsonSerializer.Deserialize<T>(responseContent);
                return new ApiResponse<T>
                {
                    StatusCode = (int)response.StatusCode,
                    Data = new List<T> { responseData }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ApiResponse<T>
                {
                    StatusCode = 500,
                    Message = "Internal server error"
                };
            }
        }
    }
}
