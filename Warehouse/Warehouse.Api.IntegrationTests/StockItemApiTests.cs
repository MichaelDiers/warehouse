namespace Warehouse.Api.IntegrationTests
{
    using Warehouse.Api.Contracts.StockItems;
    using Warehouse.Api.IntegrationTests.Lib;
    using Warehouse.Api.Models.StockItems;

    [Trait(
        "Type",
        "Integration")]
    public class StockItemApiTests
    {
        private const string Url = "https://localhost:7107/api/StockItem";

        [Fact]
        public async Task CreateAsync()
        {
            var userId = Guid.NewGuid().ToString();
            await StockItemApiTests.CreateStockItemAsync(userId);
        }

        [Fact]
        public async Task ReadAsync()
        {
            var userId = Guid.NewGuid().ToString();
            var stockItems = new[]
            {
                await StockItemApiTests.CreateStockItemAsync(userId),
                await StockItemApiTests.CreateStockItemAsync(userId),
                await StockItemApiTests.CreateStockItemAsync(userId)
            };

            var response = (await HttpClientService.GetAsync<IEnumerable<StockItem>>(
                userId,
                $"{StockItemApiTests.Url}")).ToArray();

            Assert.Equal(
                stockItems.Length,
                response.Length);
            foreach (var stockItem in stockItems)
            {
                Assert.Contains(
                    response,
                    si => StockItemApiTests.Compare(
                        stockItem,
                        si));
            }
        }

        [Fact]
        public async Task ReadByIdAsync()
        {
            var userId = Guid.NewGuid().ToString();
            var expected = await StockItemApiTests.CreateStockItemAsync(userId);
            var actual = await HttpClientService.GetAsync<StockItem>(
                userId,
                $"{StockItemApiTests.Url}/{expected.Id}");

            Assert.NotNull(actual);
            Assert.True(
                StockItemApiTests.Compare(
                    expected,
                    actual));
        }

        private static bool Compare(IStockItem a, IStockItem b)
        {
            return a.Id == b.Id && a.Name == b.Name && a.Quantity == b.Quantity && a.UserId == b.UserId;
        }

        private static async Task<IStockItem> CreateStockItemAsync(string userId)
        {
            var createStockItem = new CreateStockItem(
                "name",
                10);

            var stockItem = await HttpClientService.PostAsync<CreateStockItem, StockItem>(
                userId,
                StockItemApiTests.Url,
                createStockItem);

            Assert.NotNull(stockItem);
            Assert.Equal(
                createStockItem.Name,
                stockItem.Name);
            Assert.Equal(
                createStockItem.Quantity,
                stockItem.Quantity);
            Assert.Equal(
                userId,
                stockItem.UserId);

            return stockItem;
        }
    }
}
