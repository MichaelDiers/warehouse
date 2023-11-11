namespace Warehouse.Api.Tests.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Warehouse.Api.Contracts.StockItems;
    using Warehouse.Api.Controllers;
    using Warehouse.Api.Exceptions;
    using Warehouse.Api.Models.StockItems;
    using Warehouse.Api.Tests.Utilities;

    /// <summary>
    ///     Tests for <see cref="StockItemController" />.
    /// </summary>
    [Trait(
        Constants.TraitType,
        Constants.TraitValueUnitTest)]
    public class StockItemControllerTests
    {
        [Fact]
        public async Task CreateAsyncFail()
        {
            var services = StockItemControllerTests.Init(false);

            await Assert.ThrowsAsync<ConflictException>(
                () => services.controller.Post(
                    services.createStockItem,
                    new CancellationToken()));

            services.stockItemService.Verify(
                mock => mock.CreateAsync(
                    It.Is<CreateStockItem>(
                        value => Asserts.Assert(
                            services.createStockItem,
                            value)),
                    services.userId,
                    It.IsAny<CancellationToken>()),
                Times.Once);
            services.stockItemService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task CreateAsyncOk()
        {
            var services = StockItemControllerTests.Init(true);

            var result = await services.controller.Post(
                services.createStockItem,
                new CancellationToken());

            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            var actual = Assert.IsAssignableFrom<IStockItem>(actionResult.Value);

            Asserts.Assert(
                services.stockItem,
                actual);

            services.stockItemService.Verify(
                mock => mock.CreateAsync(
                    It.Is<CreateStockItem>(
                        value => Asserts.Assert(
                            services.createStockItem,
                            value)),
                    services.userId,
                    It.IsAny<CancellationToken>()),
                Times.Once);
            services.stockItemService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task DeleteAsyncFail()
        {
            var services = StockItemControllerTests.Init(false);

            await Assert.ThrowsAsync<NotFoundException>(
                () => services.controller.Delete(
                    services.stockItem.Id,
                    new CancellationToken()));

            services.stockItemService.Verify(
                mock => mock.DeleteAsync(
                    services.userId,
                    services.stockItem.Id,
                    It.IsAny<CancellationToken>()));
            services.stockItemService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task DeleteAsyncOk()
        {
            var services = StockItemControllerTests.Init(true);

            var result = await services.controller.Delete(
                services.stockItem.Id,
                new CancellationToken());

            Assert.IsType<OkResult>(result);

            services.stockItemService.Verify(
                mock => mock.DeleteAsync(
                    services.userId,
                    services.stockItem.Id,
                    It.IsAny<CancellationToken>()));
            services.stockItemService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ReadAsyncFail()
        {
            var services = StockItemControllerTests.Init(false);

            var result = (await services.controller.Get(new CancellationToken())).ToArray();

            Assert.Empty(result);

            services.stockItemService.Verify(
                mock => mock.ReadAsync(
                    services.userId,
                    It.IsAny<CancellationToken>()),
                Times.Once);

            services.stockItemService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ReadAsyncOk()
        {
            var services = StockItemControllerTests.Init(true);

            var result = (await services.controller.Get(new CancellationToken())).ToArray();

            Assert.Equal(
                services.stockItem,
                result.Single());

            services.stockItemService.Verify(
                mock => mock.ReadAsync(
                    services.userId,
                    It.IsAny<CancellationToken>()),
                Times.Once);

            services.stockItemService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ReadByIdAsyncFail()
        {
            var services = StockItemControllerTests.Init(false);

            await Assert.ThrowsAsync<NotFoundException>(
                () => services.controller.Get(
                    services.stockItem.Id,
                    new CancellationToken()));

            services.stockItemService.Verify(
                mock => mock.ReadByIdAsync(
                    services.userId,
                    services.stockItem.Id,
                    It.IsAny<CancellationToken>()));
            services.stockItemService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ReadByIdAsyncOk()
        {
            var services = StockItemControllerTests.Init(true);

            var result = await services.controller.Get(
                services.stockItem.Id,
                new CancellationToken());

            var actionResult = Assert.IsType<OkObjectResult>(result.Result);

            var actual = Assert.IsAssignableFrom<IStockItem>(actionResult.Value);

            Asserts.Assert(
                services.stockItem,
                actual);

            services.stockItemService.Verify(
                mock => mock.ReadByIdAsync(
                    services.userId,
                    services.stockItem.Id,
                    It.IsAny<CancellationToken>()));
            services.stockItemService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateAsyncFail()
        {
            var services = StockItemControllerTests.Init(false);

            await Assert.ThrowsAsync<NotFoundException>(
                () => services.controller.Put(
                    services.updateStockItem,
                    new CancellationToken()));

            services.stockItemService.Verify(
                mock => mock.UpdateAsync(
                    It.Is<IUpdateStockItem>(
                        value => Asserts.Assert(
                            services.updateStockItem,
                            value)),
                    services.userId,
                    It.IsAny<CancellationToken>()),
                Times.Once);
            services.stockItemService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateAsyncOk()
        {
            var services = StockItemControllerTests.Init(true);

            var result = await services.controller.Put(
                services.updateStockItem,
                new CancellationToken());
            Assert.IsType<OkResult>(result);

            services.stockItemService.Verify(
                mock => mock.UpdateAsync(
                    It.Is<IUpdateStockItem>(
                        value => Asserts.Assert(
                            services.updateStockItem,
                            value)),
                    services.userId,
                    It.IsAny<CancellationToken>()),
                Times.Once);
            services.stockItemService.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateQuantityAsyncFail()
        {
            var services = StockItemControllerTests.Init(false);

            const int delta = 20;

            await Assert.ThrowsAsync<NotFoundException>(
                () => services.controller.Put(
                    services.stockItem.Id,
                    delta,
                    new CancellationToken()));

            services.stockItemService.Verify(
                mock => mock.UpdateQuantityAsync(
                    services.userId,
                    services.stockItem.Id,
                    delta,
                    It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task UpdateQuantityAsyncOk()
        {
            var services = StockItemControllerTests.Init(true);

            const int delta = 20;
            var result = await services.controller.Put(
                services.stockItem.Id,
                delta,
                new CancellationToken());

            Assert.IsType<OkResult>(result);

            services.stockItemService.Verify(
                mock => mock.UpdateQuantityAsync(
                    services.userId,
                    services.stockItem.Id,
                    delta,
                    It.IsAny<CancellationToken>()));
        }

        private static (Mock<IStockItemService> stockItemService, StockItemController controller, string userId,
            IStockItem stockItem, CreateStockItem createStockItem, UpdateStockItem updateStockItem) Init(bool succeed)
        {
            var userId = Guid.NewGuid().ToString();

            var stockItem = new StockItem
            {
                Id = Guid.NewGuid().ToString(),
                MinimumQuantity = 10,
                Name = Guid.NewGuid().ToString(),
                UserId = userId,
                Quantity = 10
            };

            var createStockItem = new CreateStockItem(
                stockItem.Name,
                stockItem.Quantity,
                stockItem.MinimumQuantity);

            var updateStockItem = new UpdateStockItem(
                stockItem.Id,
                stockItem.Name,
                stockItem.Quantity,
                stockItem.MinimumQuantity);

            var stockItemService = new Mock<IStockItemService>();
            var createAsyncSetup = stockItemService.Setup(
                mock => mock.CreateAsync(
                    It.IsAny<ICreateStockItem>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()));
            var deleteAsyncSetup = stockItemService.Setup(
                mock => mock.DeleteAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()));
            var readAsyncSetup = stockItemService.Setup(
                mock => mock.ReadAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()));
            var readByIdAsyncSetup = stockItemService.Setup(
                mock => mock.ReadByIdAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()));
            var updateAsyncSetup = stockItemService.Setup(
                mock => mock.UpdateAsync(
                    It.IsAny<IUpdateStockItem>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()));
            var updateQuantityAsyncSetup = stockItemService.Setup(
                mock => mock.UpdateQuantityAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<CancellationToken>()));
            if (succeed)
            {
                createAsyncSetup.Returns(Task.FromResult<IStockItem>(stockItem));
                deleteAsyncSetup.Returns(Task.CompletedTask);
                readAsyncSetup.Returns(Task.FromResult<IEnumerable<IStockItem>>(new[] {stockItem}));
                readByIdAsyncSetup.Returns(Task.FromResult<IStockItem>(stockItem));
                updateAsyncSetup.Returns(Task.CompletedTask);
                updateQuantityAsyncSetup.Returns(Task.CompletedTask);
            }
            else
            {
                createAsyncSetup.Throws<ConflictException>();
                deleteAsyncSetup.Throws<NotFoundException>();
                readAsyncSetup.Returns(Task.FromResult(Enumerable.Empty<IStockItem>()));
                readByIdAsyncSetup.Throws<NotFoundException>();
                updateAsyncSetup.Throws<NotFoundException>();
                updateQuantityAsyncSetup.Throws<NotFoundException>();
            }

            var controller = new StockItemController(stockItemService.Object)
            {
                ControllerContext = ControllerContextService.Create(userId)
            };

            return (stockItemService, controller, userId, stockItem, createStockItem, updateStockItem);
        }
    }
}
