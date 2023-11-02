namespace Warehouse.Api.IntegrationTests
{
    using System.Text;
    using Newtonsoft.Json;
    using Warehouse.Api.Models.StockItems;

    public class StockItemApiTests
    {
        [Fact]
        public async Task CreateAsync()
        {
            const string url = "https://localhost:7107/api/StockItem";

            using var client = new HttpClient();
            var createStockItem = new CreateStockItem
            {
                Name = new string(
                    'a',
                    100),
                Quantity = 100
            };
            var content = new StringContent(
                JsonConvert.SerializeObject(createStockItem),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync(
                url,
                content);
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var stockItem = JsonConvert.DeserializeObject<StockItem>(jsonResponse);

            Assert.NotNull(stockItem);
            Assert.Equal(
                createStockItem.Name,
                stockItem.Name);
            Assert.Equal(
                createStockItem.Quantity,
                stockItem.Quantity);
        }
    }
}
