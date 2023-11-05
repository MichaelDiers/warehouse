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

                Assert.Equal(
                    services.createStockItem.Quantity,
                    stockItem.Quantity);
                Assert.Equal(
                    services.createStockItem.MinimumQuantity,
                    stockItem.MinimumQuantity);
                Assert.Equal(
                    services.createStockItem.Name,
                    stockItem.Name);
                Assert.Equal(
                    services.stockItem.UserId,
                    stockItem.UserId);
                Assert.True(
                    Guid.TryParse(
                        stockItem.Id,
                        out var guid) &&
                    guid != Guid.Empty);
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

            services.atomicStockItemService.VerifyNoOtherCalls();
            services.atomicShoppingItemService.VerifyNoOtherCalls();
            services.transactionHandler.VerifyNoOtherCalls();
            services.transactionHandle.VerifyNoOtherCalls();
        }

        private static (Mock<IAtomicStockItemService> atomicStockItemService, Mock<IAtomicShoppingItemService>
            atomicShoppingItemService, Mock<ITransactionHandler> transactionHandler, Mock<ITransactionHandle>
            transactionHandle, IStockItemService domainStockItemService, ICreateStockItem createStockItem, IStockItem
            stockItem) Init(bool atomicStockItemServiceThrows = false, bool atomicShoppingItemServiceThrows = false)
        {
            return DomainStockItemServiceTests.Init(
                new StockItem(
                    Guid.NewGuid().ToString(),
                    "name",
                    10,
                    20,
                    Guid.NewGuid().ToString()),
                atomicStockItemServiceThrows,
                atomicShoppingItemServiceThrows);
        }

        private static (Mock<IAtomicStockItemService> atomicStockItemService, Mock<IAtomicShoppingItemService>
            atomicShoppingItemService, Mock<ITransactionHandler> transactionHandler, Mock<ITransactionHandle>
            transactionHandle, IStockItemService domainStockItemService, ICreateStockItem createStockItem, IStockItem
            stockItem ) Init(
                IStockItem stockItem,
                bool atomicStockItemServiceThrows,
                bool atomicShoppingItemServiceThrows
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

            if (atomicStockItemServiceThrows)
            {
                atomicStockItemServiceCreateSetup.Throws<Exception>();
            }
            else
            {
                atomicStockItemServiceCreateSetup.Returns(Task.FromResult(stockItem));
            }

            var atomicShoppingItemService = new Mock<IAtomicShoppingItemService>();
            var atomicShoppingItemServiceCreateSetup = atomicShoppingItemService.Setup(
                mock => mock.CreateAsync(
                    It.IsAny<ICreateShoppingItem>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<ITransactionHandle>()));
            if (atomicShoppingItemServiceThrows)
            {
                atomicShoppingItemServiceCreateSetup.Throws<Exception>();
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
    }
}
