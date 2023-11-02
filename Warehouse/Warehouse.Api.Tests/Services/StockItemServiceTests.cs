namespace Warehouse.Api.Tests.Services
{
    using Moq;
    using Warehouse.Api.Contracts.StockItems;
    using Warehouse.Api.Extensions;
    using Warehouse.Api.Models.StockItems;
    using Warehouse.Api.Tests.Utilities;

    /// <summary>
    ///     Tests for <see cref="IStockItemService" />
    /// </summary>
    [Trait(
        "Type",
        "Unit")]
    public class StockItemServiceTests
    {
        [Fact]
        public async Task CreateAsync()
        {
            var stockItemProviderMock = new Mock<IStockItemProvider>();
            stockItemProviderMock.Setup(provider => provider.CreateAsync(It.IsAny<IStockItem>()));

            var service = TestHostApplicationBuilder.GetService<IStockItemService, IStockItemProvider>(
                new[] {ServiceCollectionExtensions.AddDependencies},
                stockItemProviderMock.Object);

            var createStockItem = new CreateStockItem(
                "name",
                100);
            var userId = Guid.NewGuid().ToString();

            var stockItem = await service.CreateAsync(
                createStockItem,
                userId);

            Assert.Equal(
                createStockItem.Quantity,
                stockItem.Quantity);
            Assert.Equal(
                createStockItem.Name,
                stockItem.Name);
            Assert.Equal(
                userId,
                stockItem.UserId);
            Assert.True(
                Guid.TryParse(
                    stockItem.Id,
                    out var guid) &&
                guid != Guid.Empty);
        }

        [Fact]
        public async Task ReadAsync()
        {
            var userId = Guid.NewGuid().ToString();
            var expectedStockItems = Enumerable.Range(
                    1,
                    10)
                .Select(
                    i => new StockItem(
                        Guid.NewGuid().ToString(),
                        $"{i}",
                        i,
                        userId))
                .ToArray();
            var stockItemProviderMock = new Mock<IStockItemProvider>();
            stockItemProviderMock.Setup(provider => provider.ReadAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(expectedStockItems as IEnumerable<IStockItem>));

            var service = TestHostApplicationBuilder.GetService<IStockItemService, IStockItemProvider>(
                new[] {ServiceCollectionExtensions.AddDependencies},
                stockItemProviderMock.Object);

            var stockItems = (await service.ReadAsync(userId)).ToArray();

            Assert.Equal(
                expectedStockItems.Length,
                stockItems.Length);
            foreach (var expectedStockItem in expectedStockItems)
            {
                Assert.NotNull(
                    stockItems.FirstOrDefault(
                        si => si.Quantity == expectedStockItem.Quantity &&
                              si.Name == expectedStockItem.Name &&
                              si.Id == expectedStockItem.Id &&
                              si.UserId == expectedStockItem.UserId));
            }
        }

        [Fact]
        public async Task ReadByIdAsync()
        {
            var userId = Guid.NewGuid().ToString();
            var expectedStockItem = new StockItem(
                Guid.NewGuid().ToString(),
                "name",
                1,
                userId) as IStockItem;

            var stockItemProviderMock = new Mock<IStockItemProvider>();
            stockItemProviderMock.Setup(
                    provider => provider.ReadByIdAsync(
                        It.IsAny<string>(),
                        It.IsAny<string>()))
                .Returns(Task.FromResult(expectedStockItem));

            var service = TestHostApplicationBuilder.GetService<IStockItemService, IStockItemProvider>(
                new[] {ServiceCollectionExtensions.AddDependencies},
                stockItemProviderMock.Object);

            var stockItem = await service.ReadByIdAsync(
                userId,
                expectedStockItem.Id);

            Assert.Equal(
                expectedStockItem.Id,
                stockItem.Id);
            Assert.Equal(
                expectedStockItem.Name,
                stockItem.Name);
            Assert.Equal(
                expectedStockItem.Quantity,
                stockItem.Quantity);
            Assert.Equal(
                expectedStockItem.UserId,
                stockItem.UserId);
        }
    }
}
