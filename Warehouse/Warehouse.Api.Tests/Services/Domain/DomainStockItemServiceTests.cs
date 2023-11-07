namespace Warehouse.Api.Tests.Services.Domain
{
    using Moq;
    using Warehouse.Api.Contracts.Database;
    using Warehouse.Api.Contracts.ShoppingItems;
    using Warehouse.Api.Contracts.StockItems;
    using Warehouse.Api.Extensions;
    using Warehouse.Api.Models.ShoppingItems;
    using Warehouse.Api.Models.StockItems;
    using Warehouse.Api.Tests.Utilities;

    [Trait(
        Constants.TraitType,
        Constants.TraitValueUnitTest)]
    public class DomainStockItemServiceTests
    {
        /// <summary>
        ///     Test for <see cref="IStockItemService" />.
        /// </summary>
        [Theory]
        [InlineData(
            true,
            false)]
        [InlineData(
            false,
            false)]
        [InlineData(
            false,
            true)]
        public async Task CreateAsync(bool atomicStockItemServiceThrows, bool atomicShoppingItemServiceThrows)
        {
            var services = DomainStockItemServiceTests.Init(
                atomicStockItemServiceThrows,
                atomicShoppingItemServiceThrows);

            if (!atomicShoppingItemServiceThrows && !atomicStockItemServiceThrows)
            {
                var stockItem = await services.domainStockItemService.CreateAsync(
                    services.createStockItem,
                    services.stockItem.UserId,
                    new CancellationToken());

                Asserts.Assert(
                    services.createStockItem,
                    services.stockItem.UserId,
                    stockItem);
            }
            else
            {
                await Assert.ThrowsAsync<Exception>(
                    () => services.domainStockItemService.CreateAsync(
                        services.createStockItem,
                        services.stockItem.UserId,
                        new CancellationToken()));
            }

            services.atomicStockItemService.Verify(
                mock => mock.CreateAsync(
                    It.Is<ICreateStockItem>(
                        value => value.MinimumQuantity == services.createStockItem.MinimumQuantity &&
                                 value.Name == services.createStockItem.Name &&
                                 value.Quantity == services.createStockItem.Quantity),
                    It.Is<string>(value => Guid.Parse(value) != Guid.Empty),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<ITransactionHandle>()),
                Times.Once);

            if (!atomicStockItemServiceThrows)
            {
                services.atomicShoppingItemService.Verify(
                    mock => mock.CreateAsync(
                        It.Is<ICreateShoppingItem>(
                            value => value.Quantity ==
                                     (services.stockItem.Quantity > services.stockItem.MinimumQuantity
                                         ? 0
                                         : services.stockItem.MinimumQuantity - services.stockItem.Quantity)),
                        services.stockItem.UserId,
                        It.IsAny<CancellationToken>(),
                        It.IsAny<ITransactionHandle>()),
                    Times.Once);
            }

            services.transactionHandler.Verify(
                mock => mock.StartTransactionAsync(It.IsAny<CancellationToken>()),
                Times.Once);

            if (!atomicShoppingItemServiceThrows && !atomicStockItemServiceThrows)
            {
                services.transactionHandle.Verify(
                    mock => mock.CommitTransactionAsync(It.IsAny<CancellationToken>()),
                    Times.Once);
            }
            else
            {
                services.transactionHandle.Verify(
                    mock => mock.AbortTransactionAsync(It.IsAny<CancellationToken>()),
                    Times.Once);
            }

            services.transactionHandle.Verify(
                mock => mock.Dispose(),
                Times.Once);

            DomainStockItemServiceTests.NoOtherCalls(services);
        }

        /// <summary>
        ///     Test for <see cref="IStockItemService" />.
        /// </summary>
        [Theory]
        [InlineData(
            true,
            false,
            true)]
        [InlineData(
            false,
            false,
            true)]
        [InlineData(
            false,
            true,
            true)]
        [InlineData(
            true,
            false,
            false)]
        [InlineData(
            false,
            false,
            false)]
        [InlineData(
            false,
            true,
            false)]
        public async Task DeleteAsync(
            bool atomicStockItemServiceThrows,
            bool atomicShoppingItemServiceThrows,
            bool isDeleted
        )
        {
            var services = DomainStockItemServiceTests.Init(
                atomicStockItemServiceThrows,
                atomicShoppingItemServiceThrows,
                isDeleted);

            if (!atomicShoppingItemServiceThrows && !atomicStockItemServiceThrows)
            {
                var result = await services.domainStockItemService.DeleteAsync(
                    services.stockItem.UserId,
                    services.stockItem.Id,
                    new CancellationToken());

                Assert.Equal(
                    isDeleted,
                    result);
            }
            else
            {
                await Assert.ThrowsAsync<Exception>(
                    () => services.domainStockItemService.DeleteAsync(
                        services.stockItem.UserId,
                        services.stockItem.Id,
                        new CancellationToken()));
            }

            services.atomicStockItemService.Verify(
                mock => mock.DeleteAsync(
                    services.stockItem.UserId,
                    services.stockItem.Id,
                    It.IsAny<CancellationToken>(),
                    It.IsNotNull<ITransactionHandle?>()),
                Times.Once);

            if (!atomicStockItemServiceThrows)
            {
                services.atomicShoppingItemService.Verify(
                    mock => mock.DeleteByStockItemIdAsync(
                        services.stockItem.UserId,
                        services.stockItem.Id,
                        It.IsAny<CancellationToken>(),
                        It.IsNotNull<ITransactionHandle>()),
                    Times.Once);
            }

            services.transactionHandler.Verify(
                mock => mock.StartTransactionAsync(It.IsAny<CancellationToken>()),
                Times.Once);

            if (!atomicShoppingItemServiceThrows && !atomicStockItemServiceThrows)
            {
                services.transactionHandle.Verify(
                    mock => mock.CommitTransactionAsync(It.IsAny<CancellationToken>()),
                    Times.Once);
            }
            else
            {
                services.transactionHandle.Verify(
                    mock => mock.AbortTransactionAsync(It.IsAny<CancellationToken>()),
                    Times.Once);
            }

            services.transactionHandle.Verify(
                mock => mock.Dispose(),
                Times.Once);

            DomainStockItemServiceTests.NoOtherCalls(services);
        }

        [Fact]
        public async Task ReadAsync()
        {
            var services = DomainStockItemServiceTests.Init();

            var result = await services.domainStockItemService.ReadAsync(
                services.stockItem.UserId,
                new CancellationToken());

            Asserts.Assert(
                services.stockItem,
                result.Single());
        }

        [Fact]
        public async Task ReadByIdAsync()
        {
            var services = DomainStockItemServiceTests.Init();

            var result = await services.domainStockItemService.ReadByIdAsync(
                services.stockItem.UserId,
                services.stockItem.Id,
                new CancellationToken());

            Assert.NotNull(result);
            Asserts.Assert(
                services.stockItem,
                result);
        }

        private static (Mock<IAtomicStockItemService> atomicStockItemService, Mock<IAtomicShoppingItemService>
            atomicShoppingItemService, Mock<ITransactionHandler> transactionHandler, Mock<ITransactionHandle>
            transactionHandle, IStockItemService domainStockItemService, ICreateStockItem createStockItem, IStockItem
            stockItem) Init(
                bool atomicStockItemServiceThrows = false,
                bool atomicShoppingItemServiceThrows = false,
                bool isDeleted = true
            )
        {
            return DomainStockItemServiceTests.Init(
                new StockItem(
                    Guid.NewGuid().ToString(),
                    "name",
                    10,
                    20,
                    Guid.NewGuid().ToString()),
                atomicStockItemServiceThrows,
                atomicShoppingItemServiceThrows,
                isDeleted);
        }

        private static (Mock<IAtomicStockItemService> atomicStockItemService, Mock<IAtomicShoppingItemService>
            atomicShoppingItemService, Mock<ITransactionHandler> transactionHandler, Mock<ITransactionHandle>
            transactionHandle, IStockItemService domainStockItemService, ICreateStockItem createStockItem, IStockItem
            stockItem ) Init(
                IStockItem stockItem,
                bool atomicStockItemServiceThrows,
                bool atomicShoppingItemServiceThrows,
                bool isDeleted
            )
        {
            var createStockItem = new CreateStockItem(
                stockItem.Name,
                stockItem.Quantity,
                stockItem.MinimumQuantity);

            var atomicStockItemService = new Mock<IAtomicStockItemService>();
            var atomicStockItemServiceCreateSetup = atomicStockItemService.Setup(
                mock => mock.CreateAsync(
                    It.IsAny<ICreateStockItem>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<ITransactionHandle>()));
            var atomicStockItemServiceDeleteSetup = atomicStockItemService.Setup(
                mock => mock.DeleteAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<ITransactionHandle?>()));
            atomicStockItemService.Setup(
                    mock => mock.ReadAsync(
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<ITransactionHandle?>()))
                .Returns(Task.FromResult<IEnumerable<IStockItem>>(new[] {stockItem}));
            atomicStockItemService.Setup(
                    mock => mock.ReadByIdAsync(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<ITransactionHandle?>()))
                .Returns(Task.FromResult(stockItem));

            if (atomicStockItemServiceThrows)
            {
                atomicStockItemServiceCreateSetup.Throws<Exception>();
                atomicStockItemServiceDeleteSetup.Throws<Exception>();
            }
            else
            {
                atomicStockItemServiceCreateSetup.Returns(Task.FromResult(stockItem));
                atomicStockItemServiceDeleteSetup.Returns(Task.FromResult(isDeleted));
            }

            var atomicShoppingItemService = new Mock<IAtomicShoppingItemService>();
            var atomicShoppingItemServiceCreateSetup = atomicShoppingItemService.Setup(
                mock => mock.CreateAsync(
                    It.IsAny<ICreateShoppingItem>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<ITransactionHandle>()));
            var atomicShoppingItemServiceDeleteByStockItemIdSetup = atomicShoppingItemService.Setup(
                mock => mock.DeleteByStockItemIdAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<ITransactionHandle?>()));
            if (atomicShoppingItemServiceThrows)
            {
                atomicShoppingItemServiceCreateSetup.Throws<Exception>();
                atomicShoppingItemServiceDeleteByStockItemIdSetup.Throws<Exception>();
            }
            else
            {
                atomicShoppingItemServiceCreateSetup.Returns(
                    Task.FromResult<IShoppingItem>(
                        new ShoppingItem(
                            Guid.NewGuid().ToString(),
                            stockItem.Name,
                            stockItem.Quantity > stockItem.MinimumQuantity
                                ? 0
                                : stockItem.MinimumQuantity - stockItem.Quantity,
                            stockItem.UserId,
                            stockItem.Id)));
                atomicShoppingItemServiceDeleteByStockItemIdSetup.Returns(Task.FromResult(isDeleted));
            }

            var transactionHandle = new Mock<ITransactionHandle>();
            var transactionHandler = new Mock<ITransactionHandler>();
            transactionHandler.Setup(mock => mock.StartTransactionAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(transactionHandle.Object));

            var domainStockItemService =
                TestHostApplicationBuilder
                    .GetService<IStockItemService, IAtomicStockItemService, IAtomicShoppingItemService,
                        ITransactionHandler>(
                        new[] {ServiceCollectionExtensions.AddDependencies},
                        atomicStockItemService.Object,
                        atomicShoppingItemService.Object,
                        transactionHandler.Object);

            return (atomicStockItemService, atomicShoppingItemService, transactionHandler, transactionHandle,
                domainStockItemService, createStockItem, stockItem);
        }

        private static void NoOtherCalls(
            (Mock<IAtomicStockItemService> atomicStockItemService, Mock<IAtomicShoppingItemService>
                atomicShoppingItemService, Mock<ITransactionHandler> transactionHandler, Mock<ITransactionHandle>
                transactionHandle, IStockItemService domainStockItemService, ICreateStockItem createStockItem,
                IStockItem stockItem) services
        )
        {
            services.atomicStockItemService.VerifyNoOtherCalls();
            services.atomicShoppingItemService.VerifyNoOtherCalls();
            services.transactionHandler.VerifyNoOtherCalls();
            services.transactionHandle.VerifyNoOtherCalls();
        }
    }
}
