namespace Warehouse.Api.IntegrationTests
{
    using System.Net;
    using Warehouse.Api.Contracts.StockItems;
    using Warehouse.Api.IntegrationTests.Lib;
    using Warehouse.Api.Models.StockItems;
    using Warehouse.Api.Tests.Utilities;

    [Trait(
        Constants.TraitType,
        Constants.TraitValueIntegration)]
    public class StockItemApiTests
    {
        private const string Url = "https://localhost:7107/api/StockItem";

        [Theory]
        [InlineData(
            0,
            0)]
        [InlineData(
            1,
            1)]
        [InlineData(
            1,
            2)]
        [InlineData(
            2,
            1)]
        public async Task CreateAsync(int quantity, int minimumQuantity)
        {
            var userId = Guid.NewGuid().ToString();
            var stockItem = await StockItemApiTests.CreateStockItemAsync(
                userId,
                quantity,
                minimumQuantity);

            var shoppingItems = await ShoppingItemApiTests.ReadAllAsync(userId);
            var shoppingItem = shoppingItems.First(si => si.StockItemId == stockItem.Id);

            Assert.Equal(
                stockItem.Id,
                shoppingItem.StockItemId);
            Assert.Equal(
                stockItem.Name,
                shoppingItem.Name);
            Assert.Equal(
                stockItem.UserId,
                shoppingItem.UserId);
            Assert.Equal(
                stockItem.Quantity > stockItem.MinimumQuantity ? 0 : stockItem.MinimumQuantity - stockItem.Quantity,
                shoppingItem.Quantity);
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
                stockItem.Quantity + 1,
                stockItem.MinimumQuantity + 1);

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
            Assert.Equal(
                update.MinimumQuantity,
                updated.MinimumQuantity);
        }

        [Theory]
        [InlineData("increase")]
        [InlineData("decrease")]
        [InlineData("foo")]
        public async Task UpdateOperationAsync(string operation)
        {
            var userId = Guid.NewGuid().ToString();
            var stockItem = await StockItemApiTests.CreateStockItemAsync(userId);
            const int delta = 2;

            Assert.NotNull(stockItem);

            if (operation is "increase" or "decrease")
            {
                await HttpClientService.PutAsync(
                    userId,
                    $"{StockItemApiTests.Url}/{operation}/{stockItem.Id}/{delta}");
            }
            else
            {
                var response = await Assert.ThrowsAsync<HttpRequestException>(
                    () => HttpClientService.PutAsync(
                        userId,
                        $"{StockItemApiTests.Url}/{operation}/{stockItem.Id}/{delta}"));
                Assert.Equal(
                    HttpStatusCode.BadRequest,
                    response.StatusCode);
            }

            var update = operation switch
            {
                "increase" => delta,
                "decrease" => -delta,
                _ => 0
            };

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
                stockItem.Name,
                updated.Name);
            Assert.Equal(
                stockItem.Quantity + update,
                updated.Quantity);
            Assert.Equal(
                stockItem.MinimumQuantity,
                updated.MinimumQuantity);
        }

        [Theory]
        [InlineData(
            "increase",
            HttpStatusCode.BadRequest,
            -1)]
        [InlineData(
            "decrease",
            HttpStatusCode.BadRequest,
            0)]
        [InlineData(
            "foo",
            HttpStatusCode.BadRequest,
            1)]
        public async Task UpdateOperationAsyncFail(string operation, HttpStatusCode statusCode, int delta)
        {
            var userId = Guid.NewGuid().ToString();
            var stockItem = await StockItemApiTests.CreateStockItemAsync(userId);

            Assert.NotNull(stockItem);

            var response = await Assert.ThrowsAsync<HttpRequestException>(
                () => HttpClientService.PutAsync(
                    userId,
                    $"{StockItemApiTests.Url}/{operation}/{stockItem.Id}/{delta}"));
            Assert.Equal(
                statusCode,
                response.StatusCode);

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
                stockItem.Name,
                updated.Name);
            Assert.Equal(
                stockItem.Quantity,
                updated.Quantity);
            Assert.Equal(
                stockItem.MinimumQuantity,
                updated.MinimumQuantity);
        }

        private static bool Compare(IStockItem a, IStockItem b)
        {
            return a.Id == b.Id &&
                   a.Name == b.Name &&
                   a.Quantity == b.Quantity &&
                   a.UserId == b.UserId &&
                   a.MinimumQuantity == b.MinimumQuantity;
        }

        private static async Task<IStockItem> CreateStockItemAsync(
            string userId,
            int quantity = 10,
            int minimumQuantity = 20
        )
        {
            var createStockItem = new CreateStockItem(
                "name",
                quantity,
                minimumQuantity);

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
                createStockItem.MinimumQuantity,
                stockItem.MinimumQuantity);
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
