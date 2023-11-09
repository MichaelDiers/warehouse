namespace Warehouse.Api.IntegrationTests
{
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
            var stockItems = new[]
            {
                await StockItemApiTests.CreateStockItemAsync(userId),
                await StockItemApiTests.CreateStockItemAsync(userId),
                await StockItemApiTests.CreateStockItemAsync(userId)
            };

            var response = (await ShoppingItemApiTests.ReadAllAsync(userId)).ToArray();

            Assert.Equal(
                stockItems.Length,
                response.Length);
        }

        [Fact]
        public async Task ReadByIdAsync()
        {
            var userId = Guid.NewGuid().ToString();
            var stockItem = await StockItemApiTests.CreateStockItemAsync(userId);
            var shoppingItem = (await ShoppingItemApiTests.ReadAllAsync(userId)).First();

            var actual = await ShoppingItemApiTests.ReadShoppingItemByIdAsync(
                userId,
                shoppingItem.Id);

            Assert.NotNull(actual);
            Assert.True(
                ShoppingItemApiTests.Compare(
                    shoppingItem,
                    actual));
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
