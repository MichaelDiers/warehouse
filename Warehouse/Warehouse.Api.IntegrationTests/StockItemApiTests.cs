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
        public async Task DeleteAsync()
        {
            var userId = Guid.NewGuid().ToString();
            var stockItem = await StockItemApiTests.CreateStockItemAsync(userId);
            var url = $"{StockItemApiTests.Url}/{stockItem.Id}";
            await HttpClientService.DeleteAsync(
                userId,
                url);

            await HttpClientService.GetFailAsync(
                userId,
                url);
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
            var actual = await StockItemApiTests.ReadStockItemByIdAsync(
                userId,
                expected.Id);

            Assert.NotNull(actual);
            Assert.True(
                StockItemApiTests.Compare(
                    expected,
                    actual));
        }

        [Fact]
        public async Task UpdateAsync()
        {
            var userId = Guid.NewGuid().ToString();
            var stockItem = await StockItemApiTests.CreateStockItemAsync(userId);

            Assert.NotNull(stockItem);

            var update = new UpdateStockItem(
                stockItem.Id,
                $"{stockItem.Name}x",
                stockItem.Quantity + 1);

            await HttpClientService.PutAsync(
                userId,
                StockItemApiTests.Url,
                update);

            var updated = await StockItemApiTests.ReadStockItemByIdAsync(
                userId,
                stockItem.Id);

            Assert.Equal(
                stockItem.Id,
                updated.Id);
            Assert.Equal(
                stockItem.UserId,
                updated.UserId);
            Assert.Equal(
                update.Name,
                updated.Name);
            Assert.Equal(
                update.Quantity,
                updated.Quantity);
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

        private static async Task<IStockItem> ReadStockItemByIdAsync(string userId, string stockItemId)
        {
            var stockItem = await HttpClientService.GetAsync<StockItem>(
                userId,
                $"{StockItemApiTests.Url}/{stockItemId}");

            Assert.NotNull(stockItem);
            Assert.Equal(
                userId,
                stockItem.UserId);
            Assert.Equal(
                stockItemId,
                stockItem.Id);
            return stockItem;
        }
    }
}
