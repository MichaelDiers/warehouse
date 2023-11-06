namespace Warehouse.Api.Tests.Services.Atomic
{
    using Moq;
    using Warehouse.Api.Contracts;
    using Warehouse.Api.Contracts.Database;
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
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task CreateAsync(bool hasTransactionHandle)
        {
            var services = AtomicStockItemServiceTests.Init();

            var result = await services.atomicStockItemService.CreateAsync(
                services.createStockItem,
                services.stockItem.UserId,
                new CancellationToken(),
                hasTransactionHandle ? services.transactionHandle.Object : null);

            Asserts.Assert(
                services.createStockItem,
                services.stockItem.UserId,
                result);

            if (hasTransactionHandle)
            {
                services.stockItemProvider.Verify(
                    mock => mock.CreateAsync(
                        It.Is<IStockItem>(
                            value => Asserts.Assert(
                                services.createStockItem,
                                services.stockItem.UserId,
                                value)),
                        It.IsAny<CancellationToken>(),
                        It.IsNotNull<ITransactionHandle?>()));
            }
            else
            {
                services.stockItemProvider.Verify(
                    mock => mock.CreateAsync(
                        It.Is<IStockItem>(
                            value => Asserts.Assert(
                                services.createStockItem,
                                services.stockItem.UserId,
                                value)),
                        It.IsAny<CancellationToken>(),
                        null),
                    Times.Once);
            }

            AtomicStockItemServiceTests.NoOtherCalls(services);
        }

        [Theory]
        [InlineData(
            true,
            true)]
        [InlineData(
            true,
            false)]
        [InlineData(
            false,
            true)]
        [InlineData(
            false,
            false)]
        public async Task DeleteAsync(bool isDeleted, bool hasTransactionHandle)
        {
            var services = AtomicStockItemServiceTests.Init(isDeleted: isDeleted);

            var result = await services.atomicStockItemService.DeleteAsync(
                services.stockItem.UserId,
                services.stockItem.Id,
                new CancellationToken(),
                hasTransactionHandle ? services.transactionHandle.Object : null);

            Assert.Equal(
                isDeleted,
                result);

            if (hasTransactionHandle)
            {
                services.stockItemProvider.Verify(
                    mock => mock.DeleteAsync(
                        services.stockItem.UserId,
                        services.stockItem.Id,
                        new CancellationToken(),
                        It.IsNotNull<ITransactionHandle?>()),
                    Times.Once);
            }
            else
            {
                services.stockItemProvider.Verify(
                    mock => mock.DeleteAsync(
                        services.stockItem.UserId,
                        services.stockItem.Id,
                        new CancellationToken(),
                        null),
                    Times.Once);
            }

            AtomicStockItemServiceTests.NoOtherCalls(services);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task ReadAsync(bool hasTransactionHandle)
        {
            var services = AtomicStockItemServiceTests.Init();

            var result = await services.atomicStockItemService.ReadAsync(
                services.stockItem.UserId,
                new CancellationToken(),
                hasTransactionHandle ? services.transactionHandle.Object : null);

            Asserts.Assert(
                services.stockItem,
                result.Single());

            if (hasTransactionHandle)
            {
                services.stockItemProvider.Verify(
                    mock => mock.ReadAsync(
                        services.stockItem.UserId,
                        It.IsAny<CancellationToken>(),
                        It.IsNotNull<ITransactionHandle?>()));
            }
            else
            {
                services.stockItemProvider.Verify(
                    mock => mock.ReadAsync(
                        services.stockItem.UserId,
                        It.IsAny<CancellationToken>(),
                        null));
            }

            AtomicStockItemServiceTests.NoOtherCalls(services);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task ReadByIdAsync(bool hasTransactionHandle)
        {
            var services = AtomicStockItemServiceTests.Init();

            var result = await services.atomicStockItemService.ReadByIdAsync(
                services.stockItem.UserId,
                services.stockItem.Id,
                new CancellationToken(),
                hasTransactionHandle ? services.transactionHandle.Object : null);

            Assert.NotNull(result);
            Asserts.Assert(
                services.stockItem,
                result);

            if (hasTransactionHandle)
            {
                services.stockItemProvider.Verify(
                    mock => mock.ReadByIdAsync(
                        services.stockItem.UserId,
                        services.stockItem.Id,
                        It.IsAny<CancellationToken>(),
                        It.IsNotNull<ITransactionHandle?>()));
            }
            else
            {
                services.stockItemProvider.Verify(
                    mock => mock.ReadByIdAsync(
                        services.stockItem.UserId,
                        services.stockItem.Id,
                        It.IsAny<CancellationToken>(),
                        null));
            }

            AtomicStockItemServiceTests.NoOtherCalls(services);
        }

        [Theory]
        [InlineData(
            true,
            true)]
        [InlineData(
            true,
            false)]
        [InlineData(
            false,
            false)]
        [InlineData(
            false,
            true)]
        public async Task UpdateAsync(bool isUpdated, bool hasTransactionHandle)
        {
            var services = AtomicStockItemServiceTests.Init(isUpdated: isUpdated);

            var result = await services.atomicStockItemService.UpdateAsync(
                services.updateStockItem,
                services.stockItem.UserId,
                new CancellationToken(),
                hasTransactionHandle ? services.transactionHandle.Object : null);

            Assert.Equal(
                isUpdated,
                result);

            if (hasTransactionHandle)
            {
                services.stockItemProvider.Verify(
                    mock => mock.UpdateAsync(
                        It.Is<IStockItem>(
                            value => Asserts.Assert(
                                services.stockItem,
                                value)),
                        It.IsAny<CancellationToken>(),
                        It.IsNotNull<ITransactionHandle?>()));
            }
            else
            {
                services.stockItemProvider.Verify(
                    mock => mock.UpdateAsync(
                        It.Is<IStockItem>(
                            value => Asserts.Assert(
                                services.stockItem,
                                value)),
                        It.IsAny<CancellationToken>(),
                        null));
            }

            AtomicStockItemServiceTests.NoOtherCalls(services);
        }

        [Theory]
        [InlineData(
            UpdateOperation.Decrease,
            10,
            true,
            true)]
        [InlineData(
            UpdateOperation.Decrease,
            10,
            false,
            true)]
        [InlineData(
            UpdateOperation.Increase,
            10,
            true,
            true)]
        [InlineData(
            UpdateOperation.Increase,
            10,
            false,
            true)]
        [InlineData(
            (UpdateOperation) int.MaxValue,
            10,
            false,
            true)]
        [InlineData(
            UpdateOperation.Decrease,
            0,
            true,
            true)]
        [InlineData(
            UpdateOperation.Increase,
            0,
            true,
            true)]
        [InlineData(
            UpdateOperation.Decrease,
            10,
            true,
            false)]
        [InlineData(
            UpdateOperation.Decrease,
            10,
            false,
            false)]
        [InlineData(
            UpdateOperation.Increase,
            10,
            true,
            false)]
        [InlineData(
            UpdateOperation.Increase,
            10,
            false,
            false)]
        [InlineData(
            (UpdateOperation) int.MaxValue,
            10,
            false,
            false)]
        [InlineData(
            UpdateOperation.Decrease,
            0,
            true,
            false)]
        [InlineData(
            UpdateOperation.Increase,
            0,
            true,
            false)]
        public async Task UpdateByQuantityDeltaAsync(
            UpdateOperation operation,
            int quantity,
            bool isUpdated,
            bool hasTransactionHandle
        )
        {
            var services = AtomicStockItemServiceTests.Init(isUpdated: isUpdated);

            var isOperationValid = operation switch
            {
                UpdateOperation.Decrease => true,
                UpdateOperation.Increase => true,
                _ => false
            };

            if (isOperationValid || quantity == 0)
            {
                var result = await services.atomicStockItemService.UpdateAsync(
                    services.stockItem.UserId,
                    services.stockItem.Id,
                    operation,
                    quantity,
                    new CancellationToken(),
                    hasTransactionHandle ? services.transactionHandle.Object : null);

                Assert.Equal(
                    isUpdated,
                    result);
            }
            else
            {
                await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                    () => services.atomicStockItemService.UpdateAsync(
                        services.stockItem.UserId,
                        services.stockItem.Id,
                        operation,
                        quantity,
                        new CancellationToken(),
                        hasTransactionHandle ? services.transactionHandle.Object : null));
            }

            if (operation is UpdateOperation.Decrease or UpdateOperation.Increase && quantity != 0)
            {
                if (hasTransactionHandle)
                {
                    services.stockItemProvider.Verify(
                        mock => mock.UpdateAsync(
                            services.stockItem.UserId,
                            services.stockItem.Id,
                            operation == UpdateOperation.Increase ? quantity : -quantity,
                            It.IsAny<CancellationToken>(),
                            It.IsNotNull<ITransactionHandle?>()));
                }
                else
                {
                    services.stockItemProvider.Verify(
                        mock => mock.UpdateAsync(
                            services.stockItem.UserId,
                            services.stockItem.Id,
                            operation == UpdateOperation.Increase ? quantity : -quantity,
                            It.IsAny<CancellationToken>(),
                            null));
                }
            }

            AtomicStockItemServiceTests.NoOtherCalls(services);
        }

        private static (Mock<IStockItemProvider> stockItemProvider, Mock<ITransactionHandle> transactionHandle,
            IAtomicStockItemService atomicStockItemService, IStockItem stockItem, ICreateStockItem createStockItem,
            IUpdateStockItem updateStockItem) Init(
                int quantity = 10,
                int minimumQuantity = 10,
                bool isDeleted = true,
                bool isUpdated = true
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
                        It.IsAny<ITransactionHandle?>()))
                .Returns(Task.CompletedTask);
            stockItemProvider.Setup(
                    mock => mock.DeleteAsync(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<ITransactionHandle?>()))
                .Returns(Task.FromResult(isDeleted));
            stockItemProvider.Setup(
                    mock => mock.ReadAsync(
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<ITransactionHandle?>()))
                .Returns(Task.FromResult<IEnumerable<IStockItem>>(new[] {stockItem}));
            stockItemProvider.Setup(
                    mock => mock.ReadByIdAsync(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<ITransactionHandle?>()))
                .Returns(Task.FromResult<IStockItem?>(stockItem));
            stockItemProvider.Setup(
                    mock => mock.UpdateAsync(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<int>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<ITransactionHandle?>()))
                .Returns(Task.FromResult(isUpdated));
            stockItemProvider.Setup(
                    mock => mock.UpdateAsync(
                        It.IsAny<IStockItem>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<ITransactionHandle?>()))
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
