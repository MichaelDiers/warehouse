namespace Warehouse.Api.IntegrationTests.Lib
{
    using System.Net.Http.Headers;
    using System.Text;
    using Newtonsoft.Json;

    internal static class HttpClientService
    {
        public static HttpClient Create(string userId)
        {
            var token = new JwtTokenService().CreateToken(userId);
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                token);
            return httpClient;
        }

        public static async Task DeleteAsync(string userid, string url)
        {
            using var httpClient = HttpClientService.Create(userid);
            var response = await httpClient.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
        }

        public static async Task<TResponse> GetAsync<TResponse>(string userid, string url)
        {
            using var httpClient = HttpClientService.Create(userid);
            var response = await httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<TResponse>(jsonResponse);

            Assert.NotNull(responseData);
            return responseData;
        }

        public static async Task GetFailAsync(string userid, string url)
        {
            using var httpClient = HttpClientService.Create(userid);
            var response = await httpClient.GetAsync(url);

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch
            {
                return;
            }

            throw new Exception("Get should fail!");
        }

        public static async Task<TResponse> PostAsync<TRequest, TResponse>(
            string userid,
            string url,
            TRequest requestData
        )
        {
            using var httpClient = HttpClientService.Create(userid);
            var content = new StringContent(
                JsonConvert.SerializeObject(requestData),
                Encoding.UTF8,
                "application/json");

            var response = await httpClient.PostAsync(
                url,
                content);

            response.EnsureSuccessStatusCode();

            Assert.NotNull(response.Headers.Location);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<TResponse>(jsonResponse);

            Assert.NotNull(responseData);
            return responseData;
        }

        public static async Task<HttpResponseMessage> PutAsync(
            string userid,
            string url,
            bool ensureSuccessStatusCode = true
        )
        {
            return await HttpClientService.PutAsync<object>(
                userid,
                url,
                null,
                ensureSuccessStatusCode);
        }

        public static async Task<HttpResponseMessage> PutAsync<TRequest>(
            string userid,
            string url,
            TRequest? requestData = null,
            bool ensureSuccessStatusCode = true
        ) where TRequest : class
        {
            using var httpClient = HttpClientService.Create(userid);
            HttpContent? content = null;
            if (requestData != null)
            {
                content = new StringContent(
                    JsonConvert.SerializeObject(requestData),
                    Encoding.UTF8,
                    "application/json");
            }

            var response = await httpClient.PutAsync(
                url,
                content);

            if (ensureSuccessStatusCode)
            {
                response.EnsureSuccessStatusCode();
            }

            return response;
        }
    }
}
