using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;
using Newtonsoft.Json;
using Utility;

namespace MagicVilla_Web.Services
{
    public class BaseService : IBaseService
    {
        public ApiResponse responseModel { get; set; }
        public IHttpClientFactory httpClient { get; set; }
        public BaseService(IHttpClientFactory httpClient)
        {
            responseModel = new();
            this.httpClient = httpClient;
        }

        public async Task<T> SendAsync<T>(ApiRequest apiRequest)
        {
            try
            {
                var client = httpClient.CreateClient("MagicVillaAPIClient");
                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.Url);
                if (apiRequest.Data != null)
                {
                    message.Content = new StringContent(
                        JsonConvert.SerializeObject(apiRequest.Data),
                        System.Text.Encoding.UTF8,
                        "application/json");
                }

                switch (apiRequest.ApiType)
                {
                    case StaticDetails.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case StaticDetails.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case StaticDetails.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get; 
                        break;
                }

                HttpResponseMessage apiResponseJSON = null;
                apiResponseJSON = await client.SendAsync(message);
                var apiContent = await apiResponseJSON.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<T>(apiContent);
                return apiResponse;
            }
            catch (Exception ex)
            {
                var dto = new ApiResponse
                {
                    ErrorMessages = new List<string> { Convert.ToString(ex.Message) },
                    IsSuccess = false
                };

                var res = JsonConvert.SerializeObject(dto);
                var apiResponse = JsonConvert.DeserializeObject<T>(res);
                return apiResponse;
            }

        }
    }
}
