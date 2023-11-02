namespace Warehouse.Api.IntegrationTests
{
    using System.Net.Http.Headers;
    using System.Text;
    using Newtonsoft.Json;
    using Warehouse.Api.IntegrationTests.Lib;
    using Warehouse.Api.Models.StockItems;

    [Trait(
        "Type",
        "Integration")]
    public class StockItemApiTests : IDisposable
    {
        private const string Url = "https://localhost:7107/api/StockItem";

        private readonly HttpClient httpClient;

        public StockItemApiTests()
        {
            var token = new JwtTokenService().CreateToken();
            this.httpClient = new HttpClient();
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                token);
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            this.httpClient.Dispose();
        }

        [Fact]
        public async Task CreateAsync()
        {
            var createStockItem = new CreateStockItem(
                "name",
                10);

            var content = StockItemApiTests.CreateContent(createStockItem);

            var response = await this.httpClient.PostAsync(
                StockItemApiTests.Url,
                content);

            var stockItem = await StockItemApiTests.GetResponse<StockItem>(response);

            Assert.NotNull(stockItem);
            Assert.Equal(
                createStockItem.Name,
                stockItem.Name);
            Assert.Equal(
                createStockItem.Quantity,
                stockItem.Quantity);
        }

        [Fact]
        public async Task ReadAsync()
        {
            var response = await this.httpClient.GetAsync($"{StockItemApiTests.Url}");

            var stockItems = await StockItemApiTests.GetResponse<IEnumerable<StockItem>>(response);

            Assert.NotNull(stockItems);
        }

        [Fact]
        public async Task ReadByIdAsync()
        {
            var id = Guid.NewGuid().ToString();

            var response = await this.httpClient.GetAsync($"{StockItemApiTests.Url}/{id}");

            var stockItem = await StockItemApiTests.GetResponse<StockItem>(response);

            Assert.NotNull(stockItem);
            Assert.Equal(
                id,
                stockItem.Id);
        }

        private static StringContent CreateContent<T>(T obj)
        {
            return new StringContent(
                JsonConvert.SerializeObject(obj),
                Encoding.UTF8,
                "application/json");
        }

        private static async Task<T> GetResponse<T>(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<T>(jsonResponse);
            Assert.NotNull(obj);
            return obj;
        }
    }
}
