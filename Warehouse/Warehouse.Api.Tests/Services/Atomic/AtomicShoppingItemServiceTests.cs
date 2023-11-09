namespace Warehouse.Api.Tests.Services.Atomic
{
    using Moq;
    using Warehouse.Api.Contracts.Database;
    using Warehouse.Api.Contracts.ShoppingItems;
    using Warehouse.Api.Exceptions;
    using Warehouse.Api.Extensions;
    using Warehouse.Api.Models.ShoppingItems;
    using Warehouse.Api.Tests.Utilities;

    /// <summary>
    ///     Tests for <see cref="IAtomicShoppingItemService" />
    /// </summary>
    [Trait(
        Constants.TraitType,
        Constants.TraitValueUnitTest)]
    public class AtomicShoppingItemServiceTests
    {
        [Fact]
        public async Task CreateAsync()
        {
            var services = AtomicShoppingItemServiceTests.Init();

            var shoppingItem = await services.atomicShoppingItemService.CreateAsync(
                services.createShoppingItem,
                services.shoppingItem.UserId,
                new CancellationToken(),
                services.transactionHandle.Object);

            Asserts.Assert(
                services.createShoppingItem,
                services.shoppingItem.UserId,
                shoppingItem);

            services.shoppingItemProvider.Verify(
                mock => mock.CreateAsync(
                    It.Is<IShoppingItem>(
                        value => Guid.Parse(value.Id) != Guid.Empty &&
                                 value.Name == services.shoppingItem.Name &&
                                 value.Quantity == services.shoppingItem.Quantity &&
                                 value.StockItemId == services.shoppingItem.StockItemId &&
                                 value.UserId == services.shoppingItem.UserId),
                    It.IsAny<CancellationToken>(),
                    It.IsNotNull<ITransactionHandle>()));

            AtomicShoppingItemServiceTests.NoOtherCalls(services);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task DeleteAsync(bool isDeleted)
        {
            var services = AtomicShoppingItemServiceTests.Init(isDeleted);

            if (isDeleted)
            {
                await services.atomicShoppingItemService.DeleteAsync(
                    services.shoppingItem.UserId,
                    services.shoppingItem.Id,
                    new CancellationToken(),
                    services.transactionHandle.Object);
            }
            else
            {
                await Assert.ThrowsAsync<NotFoundException>(
                    () => services.atomicShoppingItemService.DeleteAsync(
                        services.shoppingItem.UserId,
                        services.shoppingItem.Id,
                        new CancellationToken(),
                        services.transactionHandle.Object));
            }

            services.shoppingItemProvider.Verify(
                mock => mock.DeleteAsync(
                    services.shoppingItem.UserId,
                    services.shoppingItem.Id,
                    It.IsAny<CancellationToken>(),
                    It.IsAny<ITransactionHandle>()),
                Times.Once);

            AtomicShoppingItemServiceTests.NoOtherCalls(services);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task DeleteByStockItemIdAsync(bool isDeleted)
        {
            var services = AtomicShoppingItemServiceTests.Init(isDeleted);

            if (isDeleted)
            {
                await services.atomicShoppingItemService.DeleteByStockItemIdAsync(
                    services.shoppingItem.UserId,
                    services.shoppingItem.StockItemId,
                    new CancellationToken(),
                    services.transactionHandle.Object);
            }
            else
            {
                await Assert.ThrowsAsync<NotFoundException>(
                    () => services.atomicShoppingItemService.DeleteByStockItemIdAsync(
                        services.shoppingItem.UserId,
                        services.shoppingItem.StockItemId,
                        new CancellationToken(),
                        services.transactionHandle.Object));
            }

            services.shoppingItemProvider.Verify(
                mock => mock.DeleteByStockItemIdAsync(
                    services.shoppingItem.UserId,
                    services.shoppingItem.StockItemId,
                    It.IsAny<CancellationToken>(),
                    It.IsAny<ITransactionHandle>()),
                Times.Once);

            AtomicShoppingItemServiceTests.NoOtherCalls(services);
        }

        [Fact]
        public async Task ReadAsync()
        {
            var services = AtomicShoppingItemServiceTests.Init();

            var result = await services.atomicShoppingItemService.ReadAsync(
                services.shoppingItem.UserId,
                new CancellationToken());

            Asserts.Assert(
                services.shoppingItem,
                result.Single());

            services.shoppingItemProvider.Verify(
                mock => mock.ReadAsync(
                    services.shoppingItem.UserId,
                    It.IsAny<CancellationToken>()));
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task ReadByIdAsync(bool found)
        {
            var services = AtomicShoppingItemServiceTests.Init(readById: found);

            if (found)
            {
                var result = await services.atomicShoppingItemService.ReadByIdAsync(
                    services.shoppingItem.UserId,
                    services.shoppingItem.Id,
                    new CancellationToken());
                Assert.NotNull(result);
                Asserts.Assert(
                    services.shoppingItem,
                    result);
            }
            else
            {
                await Assert.ThrowsAsync<NotFoundException>(
                    () => services.atomicShoppingItemService.ReadByIdAsync(
                        services.shoppingItem.UserId,
                        services.shoppingItem.Id,
                        new CancellationToken()));
            }

            services.shoppingItemProvider.Verify(
                mock => mock.ReadByIdAsync(
                    services.shoppingItem.UserId,
                    services.shoppingItem.Id,
                    It.IsAny<CancellationToken>()));

            AtomicShoppingItemServiceTests.NoOtherCalls(services);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task UpdateAsync(bool isUpdated)
        {
            var services = AtomicShoppingItemServiceTests.Init(isUpdated: isUpdated);

            if (isUpdated)
            {
                await services.atomicShoppingItemService.UpdateAsync(
                    services.updateShoppingItem,
                    services.shoppingItem.UserId,
                    new CancellationToken(),
                    services.transactionHandle.Object);
            }
            else
            {
                await Assert.ThrowsAsync<NotFoundException>(
                    () => services.atomicShoppingItemService.UpdateAsync(
                        services.updateShoppingItem,
                        services.shoppingItem.UserId,
                        new CancellationToken(),
                        services.transactionHandle.Object));
            }

            services.shoppingItemProvider.Verify(
                mock => mock.UpdateAsync(
                    It.Is<IShoppingItem>(
                        value => Asserts.Assert(
                            services.shoppingItem,
                            value)),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<ITransactionHandle>()));

            AtomicShoppingItemServiceTests.NoOtherCalls(services);
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
            var services = AtomicShoppingItemServiceTests.Init(isUpdated: isUpdated);

            if (isUpdated)
            {
                await services.atomicShoppingItemService.UpdateQuantityAsync(
                    services.shoppingItem.UserId,
                    services.shoppingItem.Id,
                    quantityDelta,
                    new CancellationToken(),
                    services.transactionHandle.Object);
            }
            else
            {
                await Assert.ThrowsAsync<BadRequestException>(
                    () => services.atomicShoppingItemService.UpdateQuantityAsync(
                        services.shoppingItem.UserId,
                        services.shoppingItem.Id,
                        quantityDelta,
                        new CancellationToken(),
                        services.transactionHandle.Object));
            }

            services.shoppingItemProvider.Verify(
                mock => mock.UpdateQuantityAsync(
                    services.shoppingItem.UserId,
                    services.shoppingItem.Id,
                    quantityDelta,
                    It.IsAny<CancellationToken>(),
                    It.IsAny<ITransactionHandle>()));

            AtomicShoppingItemServiceTests.NoOtherCalls(services);
        }

        [Fact]
        public async Task UpdateQuantityAsync_QuantityZero()
        {
            var services = AtomicShoppingItemServiceTests.Init();

            await services.atomicShoppingItemService.UpdateQuantityAsync(
                services.shoppingItem.UserId,
                services.shoppingItem.Id,
                0,
                new CancellationToken(),
                services.transactionHandle.Object);

            AtomicShoppingItemServiceTests.NoOtherCalls(services);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task UpdateQuantityByStockItemIdAsync(bool isUpdated)
        {
            var services = AtomicShoppingItemServiceTests.Init(isUpdated: isUpdated);

            if (isUpdated)
            {
                await services.atomicShoppingItemService.UpdateQuantityByStockItemIdAsync(
                    services.shoppingItem.UserId,
                    services.shoppingItem.StockItemId,
                    services.shoppingItem.Quantity,
                    new CancellationToken(),
                    services.transactionHandle.Object);
            }
            else
            {
                await Assert.ThrowsAsync<NotFoundException>(
                    () => services.atomicShoppingItemService.UpdateQuantityByStockItemIdAsync(
                        services.shoppingItem.UserId,
                        services.shoppingItem.StockItemId,
                        services.shoppingItem.Quantity,
                        new CancellationToken(),
                        services.transactionHandle.Object));
            }

            services.shoppingItemProvider.Verify(
                mock => mock.UpdateQuantityByStockItemIdAsync(
                    services.shoppingItem.UserId,
                    services.shoppingItem.StockItemId,
                    services.shoppingItem.Quantity,
                    It.IsAny<CancellationToken>(),
                    It.IsAny<ITransactionHandle>()));

            AtomicShoppingItemServiceTests.NoOtherCalls(services);
        }

        private static (IAtomicShoppingItemService atomicShoppingItemService, Mock<IShoppingItemProvider>
            shoppingItemProvider, Mock<ITransactionHandle> transactionHandle, IShoppingItem shoppingItem,
            ICreateShoppingItem createShoppingItem, IUpdateShoppingItem updateShoppingItem) Init(
                bool isDeleted = true,
                bool readById = true,
                bool isUpdated = true
            )
        {
            var shoppingItem = new ShoppingItem(
                Guid.NewGuid().ToString(),
                "name",
                10,
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString());
            var createShoppingItem = new CreateShoppingItem(
                shoppingItem.Name,
                shoppingItem.Quantity,
                shoppingItem.StockItemId);
            var updateShoppingItem = new UpdateShoppingItem(
                shoppingItem.Id,
                shoppingItem.Name,
                shoppingItem.Quantity,
                shoppingItem.StockItemId);
            var transactionHandle = new Mock<ITransactionHandle>();

            var shoppingItemProvider = new Mock<IShoppingItemProvider>();
            shoppingItemProvider.Setup(
                    mock => mock.CreateAsync(
                        It.IsAny<IShoppingItem>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<ITransactionHandle>()))
                .Returns(Task.CompletedTask);
            shoppingItemProvider.Setup(
                    mock => mock.DeleteAsync(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<ITransactionHandle>()))
                .Returns(Task.FromResult(isDeleted));
            shoppingItemProvider.Setup(
                    mock => mock.ReadAsync(
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IEnumerable<IShoppingItem>>(new[] {shoppingItem}));
            var shoppingItemProviderSetup = shoppingItemProvider.Setup(
                mock => mock.ReadByIdAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<ITransactionHandle>()));
            if (readById)
            {
                shoppingItemProviderSetup.Returns(Task.FromResult<IShoppingItem>(shoppingItem));
            }
            else
            {
                shoppingItemProviderSetup.Throws<NotFoundException>();
            }

            shoppingItemProvider.Setup(
                    mock => mock.UpdateAsync(
                        It.IsAny<IShoppingItem>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<ITransactionHandle>()))
                .Returns(Task.FromResult(isUpdated));
            shoppingItemProvider.Setup(
                    mock => mock.UpdateQuantityAsync(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<int>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<ITransactionHandle>()))
                .Returns(Task.FromResult(isUpdated));
            shoppingItemProvider.Setup(
                    mock => mock.DeleteByStockItemIdAsync(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<ITransactionHandle>()))
                .Returns(Task.FromResult(isDeleted));
            shoppingItemProvider.Setup(
                    mock => mock.UpdateQuantityByStockItemIdAsync(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<int>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<ITransactionHandle>()))
                .Returns(Task.FromResult(isUpdated));

            var atomicShoppingItemService =
                TestHostApplicationBuilder
                    .GetService<IAtomicShoppingItemService, IShoppingItemProvider, ITransactionHandle>(
                        new[] {ServiceCollectionExtensions.AddDependencies},
                        shoppingItemProvider.Object,
                        transactionHandle.Object);

            return (atomicShoppingItemService, shoppingItemProvider, transactionHandle, shoppingItem,
                createShoppingItem, updateShoppingItem);
        }

        private static void NoOtherCalls(
            (IAtomicShoppingItemService atomicShoppingItemService, Mock<IShoppingItemProvider> shoppingItemProvider,
                Mock<ITransactionHandle> transactionHandle, IShoppingItem shoppingItem, ICreateShoppingItem
                createShoppingItem, IUpdateShoppingItem updateShoppingItem) services
        )
        {
            services.transactionHandle.VerifyNoOtherCalls();
            services.shoppingItemProvider.VerifyNoOtherCalls();
        }
    }
}
