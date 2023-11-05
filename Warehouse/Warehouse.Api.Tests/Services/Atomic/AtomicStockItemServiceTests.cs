namespace Warehouse.Api.Tests.Services.Atomic
{
    using Moq;
    using Warehouse.Api.Contracts;
    using Warehouse.Api.Contracts.StockItems;
    using Warehouse.Api.Extensions;
    using Warehouse.Api.Models.StockItems;
    using Warehouse.Api.Tests.Utilities;

    /// <summary>
    ///     Tests for <see cref="IAtomicStockItemService" />
    /// </summary>
    [Trait(
        Constants.TraitType,
        Constants.TraitValueUnitTest)]
    public class AtomicStockItemServiceTests
    {
        [Fact]
        public async Task CreateAsync()
        {
            var stockItemProviderMock = new Mock<IStockItemProvider>();
            stockItemProviderMock.Setup(
                provider => provider.CreateAsync(
                    It.IsAny<IStockItem>(),
                    It.IsAny<CancellationToken>()));

            var service = TestHostApplicationBuilder.GetService<IAtomicStockItemService, IStockItemProvider>(
                new[] {ServiceCollectionExtensions.AddDependencies},
                stockItemProviderMock.Object);

            var createStockItem = new CreateStockItem(
                "name",
                100,
                50);
            var userId = Guid.NewGuid().ToString();

            var stockItem = await service.CreateAsync(
                createStockItem,
                userId,
                It.IsAny<CancellationToken>());

            Assert.Equal(
                createStockItem.Quantity,
                stockItem.Quantity);
            Assert.Equal(
                createStockItem.MinimumQuantity,
                stockItem.MinimumQuantity);
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

            var service = TestHostApplicationBuilder.GetService<IAtomicStockItemService, IStockItemProvider>(
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
                        i + 1,
                        userId))
                .ToArray();
            var stockItemProviderMock = new Mock<IStockItemProvider>();
            stockItemProviderMock.Setup(
                    provider => provider.ReadAsync(
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(expectedStockItems as IEnumerable<IStockItem>));

            var service = TestHostApplicationBuilder.GetService<IAtomicStockItemService, IStockItemProvider>(
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
                              si.MinimumQuantity == expectedStockItem.MinimumQuantity &&
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
                2,
                userId) as IStockItem;

            var stockItemProviderMock = new Mock<IStockItemProvider>();
            stockItemProviderMock.Setup(
                    provider => provider.ReadByIdAsync(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IStockItem?>(expectedStockItem));

            var service = TestHostApplicationBuilder.GetService<IAtomicStockItemService, IStockItemProvider>(
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
                expectedStockItem.MinimumQuantity,
                stockItem.MinimumQuantity);
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

            var service = TestHostApplicationBuilder.GetService<IAtomicStockItemService, IStockItemProvider>(
                new[] {ServiceCollectionExtensions.AddDependencies},
                stockItemProviderMock.Object);

            Assert.Equal(
                isUpdated,
                await service.UpdateAsync(
                    new UpdateStockItem(
                        stockItemId,
                        "name",
                        10,
                        11),
                    userId,
                    CancellationToken.None));
        }

        [Theory]
        [InlineData(
            UpdateOperation.Decrease,
            10,
            true)]
        [InlineData(
            UpdateOperation.Decrease,
            10,
            false)]
        [InlineData(
            UpdateOperation.Increase,
            10,
            true)]
        [InlineData(
            UpdateOperation.Increase,
            10,
            false)]
        [InlineData(
            (UpdateOperation) int.MaxValue,
            10,
            false)]
        [InlineData(
            UpdateOperation.Decrease,
            0,
            true)]
        [InlineData(
            UpdateOperation.Increase,
            0,
            true)]
        public async Task UpdateByQuantityDeltaAsync(UpdateOperation operation, int quantity, bool isUpdated)
        {
            var userId = Guid.NewGuid().ToString();
            var stockItemId = Guid.NewGuid().ToString();

            var stockItemProviderMock = new Mock<IStockItemProvider>();
            stockItemProviderMock.Setup(
                    mock => mock.UpdateAsync(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<int>(),
                        It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(isUpdated));

            var service = TestHostApplicationBuilder.GetService<IAtomicStockItemService, IStockItemProvider>(
                new[] {ServiceCollectionExtensions.AddDependencies},
                stockItemProviderMock.Object);

            var isOperationValid = operation switch
            {
                UpdateOperation.Decrease => true,
                UpdateOperation.Increase => true,
                _ => false
            };

            if (isOperationValid)
            {
                Assert.Equal(
                    isUpdated,
                    await service.UpdateAsync(
                        userId,
                        stockItemId,
                        operation,
                        quantity,
                        new CancellationToken()));
            }
            else
            {
                await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                    () => service.UpdateAsync(
                        userId,
                        stockItemId,
                        operation,
                        quantity,
                        new CancellationToken()));
            }

            if (operation is UpdateOperation.Decrease or UpdateOperation.Increase && quantity != 0)
            {
                stockItemProviderMock.Verify(
                    mock => mock.UpdateAsync(
                        userId,
                        stockItemId,
                        operation == UpdateOperation.Increase ? quantity : -quantity,
                        It.IsAny<CancellationToken>()));
            }

            stockItemProviderMock.VerifyNoOtherCalls();
        }
    }
}
