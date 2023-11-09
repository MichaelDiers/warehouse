namespace Warehouse.Api.IntegrationTests
{
    using System.Net;
    using Warehouse.Api.Contracts.ShoppingItems;
    using Warehouse.Api.IntegrationTests.Lib;
    using Warehouse.Api.Models.ShoppingItems;
    using Warehouse.Api.Tests.Utilities;

    [Trait(
        Constants.TraitType,
        Constants.TraitValueIntegration)]
    public class ShoppingItemApiTests
    {
        private const string Url = "https://localhost:7107/api/ShoppingItem";

        [Fact]
        public async Task CreateAsync()
        {
            var userId = Guid.NewGuid().ToString();
            await ShoppingItemApiTests.CreateShoppingItemAsync(userId);
        }

        [Fact]
        public async Task DeleteAsync()
        {
            var userId = Guid.NewGuid().ToString();
            var shoppingItem = await ShoppingItemApiTests.CreateShoppingItemAsync(userId);
            var url = $"{ShoppingItemApiTests.Url}/{shoppingItem.Id}";
            await HttpClientService.DeleteAsync(
                userId,
                url);

            await HttpClientService.GetFailAsync(
                userId,
                url);
        }

        public static async Task<IEnumerable<IShoppingItem>> ReadAllAsync(string userId)
        {
            var response = (await HttpClientService.GetAsync<IEnumerable<ShoppingItem>>(
                userId,
                $"{ShoppingItemApiTests.Url}")).ToArray();

            return response;
        }

        [Fact]
        public async Task ReadAsync()
        {
            var userId = Guid.NewGuid().ToString();
            var shoppingItems = new[]
            {
                await ShoppingItemApiTests.CreateShoppingItemAsync(userId),
                await ShoppingItemApiTests.CreateShoppingItemAsync(userId),
                await ShoppingItemApiTests.CreateShoppingItemAsync(userId)
            };

            var response = (await ShoppingItemApiTests.ReadAllAsync(userId)).ToArray();

            Assert.Equal(
                shoppingItems.Length,
                response.Length);
            foreach (var shoppingItem in shoppingItems)
            {
                Assert.Contains(
                    response,
                    si => ShoppingItemApiTests.Compare(
                        shoppingItem,
                        si));
            }
        }

        [Fact]
        public async Task ReadByIdAsync()
        {
            var userId = Guid.NewGuid().ToString();
            var expected = await ShoppingItemApiTests.CreateShoppingItemAsync(userId);
            var actual = await ShoppingItemApiTests.ReadShoppingItemByIdAsync(
                userId,
                expected.Id);

            Assert.NotNull(actual);
            Assert.True(
                ShoppingItemApiTests.Compare(
                    expected,
                    actual));
        }

        [Fact]
        public async Task UpdateAsync()
        {
            var userId = Guid.NewGuid().ToString();
            var shoppingItem = await ShoppingItemApiTests.CreateShoppingItemAsync(userId);

            Assert.NotNull(shoppingItem);

            var update = new UpdateShoppingItem(
                shoppingItem.Id,
                $"{shoppingItem.Name}x",
                shoppingItem.Quantity + 1,
                shoppingItem.StockItemId);

            await HttpClientService.PutAsync(
                userId,
                ShoppingItemApiTests.Url,
                update);

            var updated = await ShoppingItemApiTests.ReadShoppingItemByIdAsync(
                userId,
                shoppingItem.Id);

            Assert.Equal(
                shoppingItem.Id,
                updated.Id);
            Assert.Equal(
                shoppingItem.UserId,
                updated.UserId);
            Assert.Equal(
                update.Name,
                updated.Name);
            Assert.Equal(
                update.Quantity,
                updated.Quantity);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(0)]
        [InlineData(-10)]
        public async Task UpdateOperationAsync(int quantityDelta)
        {
            var userId = Guid.NewGuid().ToString();
            var shoppingItem = await ShoppingItemApiTests.CreateShoppingItemAsync(userId);

            Assert.NotNull(shoppingItem);

            await HttpClientService.PutAsync(
                userId,
                $"{ShoppingItemApiTests.Url}/{shoppingItem.Id}/{quantityDelta}");

            var updated = await ShoppingItemApiTests.ReadShoppingItemByIdAsync(
                userId,
                shoppingItem.Id);
            Assert.Equal(
                shoppingItem.Id,
                updated.Id);
            Assert.Equal(
                shoppingItem.UserId,
                updated.UserId);
            Assert.Equal(
                shoppingItem.Name,
                updated.Name);
            Assert.Equal(
                shoppingItem.Quantity + quantityDelta,
                updated.Quantity);
        }

        [Theory]
        [InlineData(
            int.MaxValue,
            HttpStatusCode.BadRequest)]
        [InlineData(
            int.MinValue,
            HttpStatusCode.BadRequest)]
        public async Task UpdateOperationAsyncFail(int quantityDelta, HttpStatusCode statusCode)
        {
            var userId = Guid.NewGuid().ToString();
            var shoppingItem = await ShoppingItemApiTests.CreateShoppingItemAsync(userId);

            Assert.NotNull(shoppingItem);

            var response = await Assert.ThrowsAsync<HttpRequestException>(
                () => HttpClientService.PutAsync(
                    userId,
                    $"{ShoppingItemApiTests.Url}/{shoppingItem.Id}/{quantityDelta}"));
            Assert.Equal(
                statusCode,
                response.StatusCode);

            var updated = await ShoppingItemApiTests.ReadShoppingItemByIdAsync(
                userId,
                shoppingItem.Id);
            Assert.Equal(
                shoppingItem.Id,
                updated.Id);
            Assert.Equal(
                shoppingItem.UserId,
                updated.UserId);
            Assert.Equal(
                shoppingItem.Name,
                updated.Name);
            Assert.Equal(
                shoppingItem.Quantity,
                updated.Quantity);
        }

        private static bool Compare(IShoppingItem a, IShoppingItem b)
        {
            return a.Id == b.Id && a.Name == b.Name && a.Quantity == b.Quantity && a.UserId == b.UserId;
        }

        private static async Task<IShoppingItem> CreateShoppingItemAsync(string userId)
        {
            var createShoppingItem = new CreateShoppingItem(
                Guid.NewGuid().ToString(),
                10,
                Guid.NewGuid().ToString());

            var shoppingItem = await HttpClientService.PostAsync<CreateShoppingItem, ShoppingItem>(
                userId,
                ShoppingItemApiTests.Url,
                createShoppingItem);

            Assert.NotNull(shoppingItem);
            Assert.Equal(
                createShoppingItem.Name,
                shoppingItem.Name);
            Assert.Equal(
                createShoppingItem.Quantity,
                shoppingItem.Quantity);
            Assert.Equal(
                userId,
                shoppingItem.UserId);

            return shoppingItem;
        }

        private static async Task<IShoppingItem> ReadShoppingItemByIdAsync(string userId, string shoppingItemId)
        {
            var shoppingItem = await HttpClientService.GetAsync<ShoppingItem>(
                userId,
                $"{ShoppingItemApiTests.Url}/{shoppingItemId}");

            Assert.NotNull(shoppingItem);
            Assert.Equal(
                userId,
                shoppingItem.UserId);
            Assert.Equal(
                shoppingItemId,
                shoppingItem.Id);
            return shoppingItem;
        }
    }
}
