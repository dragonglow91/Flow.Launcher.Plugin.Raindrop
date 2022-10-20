using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Flow.Launcher.Plugin.Raindrop.Core
{
    public class RaindropApi
    {
        private readonly string _apiToken;
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public RaindropApi(string apiToken)
        {
            _apiToken = apiToken;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://api.raindrop.io");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _apiToken);

            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<List<Raindrop>> SearchForAsync(string keyword, CancellationToken token)
        {
            if (string.IsNullOrEmpty(keyword))
                return new List<Raindrop>();
            
            var response = await _httpClient.GetAsync($"/rest/v1/raindrops/0?search={HttpUtility.UrlEncode(keyword)}", token);
            if (response.IsSuccessStatusCode == false)
            {
                return new List<Raindrop>();
            }

            var raindropResponse = await response.Content.ReadFromJsonAsync<RaindropResponse>(_jsonSerializerOptions, token);

            return raindropResponse.Items.ToList();
        }
    }
}
