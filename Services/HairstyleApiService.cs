using RestSharp;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Hairdresser_Website.Services // Daha uygun bir namespace olabilir
{
    public class HairstyleApiService
    {
        private const string ApiUrl = "https://www.ailabapi.com/api/portrait/effects/hairstyle-editor";
        private readonly string ApiKey;

        // API anahtarını buraya parametre olarak alıyoruz
        public HairstyleApiService(string apiKey)
        {
            ApiKey = apiKey;
        }

        public async Task<string> ChangeHairstyleAsync(string imagePath, int hairType = 101)
        {
            try
            {
                var options = new RestClientOptions(ApiUrl)
                {
                    ThrowOnAnyError = true,
                    Timeout = TimeSpan.FromSeconds(10)
                };
                var client = new RestClient(options);

                var request = new RestRequest()
                    .AddHeader("ailabapi-api-key", ApiKey)
                    .AddFile("image_target", imagePath)
                    .AddParameter("hair_type", hairType);

                var response = await client.PostAsync(request);

                if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var result = Newtonsoft.Json.Linq.JObject.Parse(response.Content);
                    return result["data"]["image"]?.ToString();
                }
                else
                {
                    throw new Exception($"API Error: {response?.Content}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while calling hairstyle API: " + ex.Message);
            }
        }
    }
}
