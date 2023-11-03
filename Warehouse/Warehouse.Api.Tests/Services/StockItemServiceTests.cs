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
            stockItemProviderMock.Setup(
                provider => provider.CreateAsync(
                    It.IsAny<IStockItem>(),
                    It.IsAny<CancellationToken>()));

            var service = TestHostApplicationBuilder.GetService<IStockItemService, IStockItemProvider>(
                new[] {ServiceCollectionExtensions.AddDependencies},
                stockItemProviderMock.Object);

            var createStockItem = new CreateStockItem(
                "name",
                100);
            var userId = Guid.NewGuid().ToString();

            var stockItem = await service.CreateAsync(
                createStockItem,
                userId,
                It.IsAny<CancellationToken>());

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

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task DeleteAsync(bool isDeleted)
        {
            var userId = Guid.NewGuid().ToString();
            var stockItemId = Guid.NewGuid().ToString();

            var stockItemProviderMock = new Mock<IStockItemProvider>();
            stockItemProviderMock.Setup(
                    mock => mock.DeleteAsync(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(isDeleted));

            var service = TestHostApplicationBuilder.GetService<IStockItemService, IStockItemProvider>(
                new[] {ServiceCollectionExtensions.AddDependencies},
                stockItemProviderMock.Object);

            Assert.Equal(
                isDeleted,
                await service.DeleteAsync(
                    userId,
                    stockItemId,
                    CancellationToken.None));

            stockItemProviderMock.Verify(
                mock => mock.DeleteAsync(
                    userId,
                    stockItemId,
                    It.IsAny<CancellationToken>()),
                Times.Once);
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
            stockItemProviderMock.Setup(
                    provider => provider.ReadAsync(
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(expectedStockItems as IEnumerable<IStockItem>));

            var service = TestHostApplicationBuilder.GetService<IStockItemService, IStockItemProvider>(
                new[] {ServiceCollectionExtensions.AddDependencies},
                stockItemProviderMock.Object);

            var stockItems = (await service.ReadAsync(
                userId,
                It.IsAny<CancellationToken>())).ToArray();

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
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IStockItem?>(expectedStockItem));

            var service = TestHostApplicationBuilder.GetService<IStockItemService, IStockItemProvider>(
                new[] {ServiceCollectionExtensions.AddDependencies},
                stockItemProviderMock.Object);

            var stockItem = await service.ReadByIdAsync(
                userId,
                expectedStockItem.Id,
                It.IsAny<CancellationToken>());

            Assert.NotNull(stockItem);

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

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task UpdateAsync(bool isUpdated)
        {
            var userId = Guid.NewGuid().ToString();
            var stockItemId = Guid.NewGuid().ToString();

            var stockItemProviderMock = new Mock<IStockItemProvider>();
            stockItemProviderMock.Setup(
                    mock => mock.UpdateAsync(
                        It.IsAny<IStockItem>(),
                        It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(isUpdated));

            var service = TestHostApplicationBuilder.GetService<IStockItemService, IStockItemProvider>(
                new[] {ServiceCollectionExtensions.AddDependencies},
                stockItemProviderMock.Object);

            Assert.Equal(
                isUpdated,
                await service.UpdateAsync(
                    new UpdateStockItem(
                        stockItemId,
                        "name",
                        10),
                    userId,
                    CancellationToken.None));
        }
    }
}
