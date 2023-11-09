namespace Warehouse.Api.Tests.Services.Atomic
{
    using Moq;
    using Warehouse.Api.Contracts.Database;
    using Warehouse.Api.Contracts.StockItems;
    using Warehouse.Api.Exceptions;
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
            var services = AtomicStockItemServiceTests.Init();

            var result = await services.atomicStockItemService.CreateAsync(
                services.createStockItem,
                services.stockItem.UserId,
                new CancellationToken(),
                services.transactionHandle.Object);

            Asserts.Assert(
                services.createStockItem,
                services.stockItem.UserId,
                result);

            services.stockItemProvider.Verify(
                mock => mock.CreateAsync(
                    It.Is<IStockItem>(
                        value => Asserts.Assert(
                            services.createStockItem,
                            services.stockItem.UserId,
                            value)),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<ITransactionHandle>()));

            AtomicStockItemServiceTests.NoOtherCalls(services);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task DeleteAsync(bool isDeleted)
        {
            var services = AtomicStockItemServiceTests.Init(isDeleted: isDeleted);

            await services.atomicStockItemService.DeleteAsync(
                services.stockItem.UserId,
                services.stockItem.Id,
                new CancellationToken(),
                services.transactionHandle.Object);

            services.stockItemProvider.Verify(
                mock => mock.DeleteAsync(
                    services.stockItem.UserId,
                    services.stockItem.Id,
                    new CancellationToken(),
                    It.IsNotNull<ITransactionHandle>()),
                Times.Once);

            AtomicStockItemServiceTests.NoOtherCalls(services);
        }

        [Fact]
        public async Task ReadAsync()
        {
            var services = AtomicStockItemServiceTests.Init();

            var result = await services.atomicStockItemService.ReadAsync(
                services.stockItem.UserId,
                new CancellationToken());

            Asserts.Assert(
                services.stockItem,
                result.Single());

            services.stockItemProvider.Verify(
                mock => mock.ReadAsync(
                    services.stockItem.UserId,
                    It.IsAny<CancellationToken>()));

            AtomicStockItemServiceTests.NoOtherCalls(services);
        }

        [Fact]
        public async Task ReadByIdAsync()
        {
            var services = AtomicStockItemServiceTests.Init();

            var result = await services.atomicStockItemService.ReadByIdAsync(
                services.stockItem.UserId,
                services.stockItem.Id,
                new CancellationToken());

            Assert.NotNull(result);
            Asserts.Assert(
                services.stockItem,
                result);

            services.stockItemProvider.Verify(
                mock => mock.ReadByIdAsync(
                    services.stockItem.UserId,
                    services.stockItem.Id,
                    It.IsAny<CancellationToken>()));

            AtomicStockItemServiceTests.NoOtherCalls(services);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task UpdateAsync(bool isUpdated)
        {
            var services = AtomicStockItemServiceTests.Init(isUpdated: isUpdated);

            await services.atomicStockItemService.UpdateAsync(
                services.updateStockItem,
                services.stockItem.UserId,
                new CancellationToken(),
                services.transactionHandle.Object);

            services.stockItemProvider.Verify(
                mock => mock.UpdateAsync(
                    It.Is<IStockItem>(
                        value => Asserts.Assert(
                            services.stockItem,
                            value)),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<ITransactionHandle>()));

            AtomicStockItemServiceTests.NoOtherCalls(services);
        }

        [Theory]
        [InlineData(
            10,
            true)]
        [InlineData(
            10,
            false)]
        [InlineData(
            -10,
            true)]
        [InlineData(
            -10,
            false)]
        public async Task UpdateQuantityAsync_QuantityNonZero(int quantityDelta, bool isUpdated)
        {
            var services = AtomicStockItemServiceTests.Init(
                isUpdated: isUpdated,
                deltaQuantity: quantityDelta);

            var result = await services.atomicStockItemService.UpdateQuantityAsync(
                services.stockItem.UserId,
                services.stockItem.Id,
                quantityDelta,
                new CancellationToken(),
                services.transactionHandle.Object);

            if (isUpdated)
            {
                Assert.NotNull(result);
                Asserts.Assert(
                    services.stockItem,
                    new StockItem(result) {Quantity = result.Quantity - quantityDelta});
            }
            else
            {
                Assert.Null(result);
            }

            services.stockItemProvider.Verify(
                mock => mock.UpdateQuantityAsync(
                    services.stockItem.UserId,
                    services.stockItem.Id,
                    quantityDelta,
                    It.IsAny<CancellationToken>(),
                    It.IsAny<ITransactionHandle>()));

            AtomicStockItemServiceTests.NoOtherCalls(services);
        }

        [Fact]
        public async Task UpdateQuantityAsync_QuantityZero()
        {
            var services = AtomicStockItemServiceTests.Init();

            var result = await services.atomicStockItemService.UpdateQuantityAsync(
                services.stockItem.UserId,
                services.stockItem.Id,
                0,
                new CancellationToken(),
                services.transactionHandle.Object);

            Assert.NotNull(result);
            Assert.Equal(
                services.stockItem,
                result);

            services.stockItemProvider.Verify(
                mock => mock.ReadByIdAsync(
                    services.stockItem.UserId,
                    services.stockItem.Id,
                    It.IsAny<CancellationToken>(),
                    It.IsAny<ITransactionHandle>()));

            AtomicStockItemServiceTests.NoOtherCalls(services);
        }

        private static (Mock<IStockItemProvider> stockItemProvider, Mock<ITransactionHandle> transactionHandle,
            IAtomicStockItemService atomicStockItemService, IStockItem stockItem, ICreateStockItem createStockItem,
            IUpdateStockItem updateStockItem) Init(
                int quantity = 10,
                int minimumQuantity = 10,
                bool isDeleted = true,
                bool isUpdated = true,
                int deltaQuantity = 10,
                bool readByIdSucceeds = true
            )
        {
            var stockItem = new StockItem(
                Guid.NewGuid().ToString(),
                "name",
                quantity,
                minimumQuantity,
                Guid.NewGuid().ToString());

            var createStockItem = new CreateStockItem(
                stockItem.Name,
                stockItem.Quantity,
                stockItem.MinimumQuantity);

            var updateStockItem = new UpdateStockItem(
                stockItem.Id,
                stockItem.Name,
                stockItem.Quantity,
                stockItem.MinimumQuantity);

            var stockItemProvider = new Mock<IStockItemProvider>();
            stockItemProvider.Setup(
                    mock => mock.CreateAsync(
                        It.IsAny<IStockItem>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<ITransactionHandle>()))
                .Returns(Task.CompletedTask);
            stockItemProvider.Setup(
                    mock => mock.DeleteAsync(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<ITransactionHandle>()))
                .Returns(Task.FromResult(isDeleted));
            stockItemProvider.Setup(
                    mock => mock.ReadAsync(
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<ITransactionHandle>()))
                .Returns(Task.FromResult<IEnumerable<IStockItem>>(new[] {stockItem}));
            var readByIdAsync = stockItemProvider.Setup(
                mock => mock.ReadByIdAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<ITransactionHandle>()));
            if (readByIdSucceeds)
            {
                readByIdAsync.Returns(Task.FromResult<IStockItem>(stockItem));
            }
            else
            {
                readByIdAsync.Throws<NotFoundException>();
            }

            var updateQuantityAsync = stockItemProvider.Setup(
                mock => mock.UpdateQuantityAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<ITransactionHandle>()));
            if (isUpdated)
            {
                updateQuantityAsync.Returns(
                    Task.FromResult<IStockItem>(
                        new StockItem(stockItem) {Quantity = stockItem.Quantity + deltaQuantity}));
            }
            else
            {
                updateQuantityAsync.Throws<NotFoundException>();
            }

            stockItemProvider.Setup(
                    mock => mock.UpdateAsync(
                        It.IsAny<IStockItem>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<ITransactionHandle>()))
                .Returns(Task.FromResult(isUpdated));

            var atomicStockItemService =
                TestHostApplicationBuilder.GetService<IAtomicStockItemService, IStockItemProvider>(
                    new[] {ServiceCollectionExtensions.AddDependencies},
                    stockItemProvider.Object);

            var transactionHandle = new Mock<ITransactionHandle>();

            return (stockItemProvider, transactionHandle, atomicStockItemService, stockItem, createStockItem,
                updateStockItem);
        }

        private static void NoOtherCalls(
            (Mock<IStockItemProvider> stockItemProvider, Mock<ITransactionHandle> transactionHandle,
                IAtomicStockItemService atomicStockItemService, IStockItem stockItem, ICreateStockItem createStockItem,
                IUpdateStockItem updateStockItem) services
        )
        {
            services.stockItemProvider.VerifyNoOtherCalls();
            services.transactionHandle.VerifyNoOtherCalls();
        }
    }
}
