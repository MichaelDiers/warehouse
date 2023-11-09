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
        private const int MinimumQuantity = StockItemControllerTests.Quantity / 2;
        private const int Quantity = 100;

        private readonly CreateStockItem createStockItem;

        private readonly string name = Guid.NewGuid().ToString();

        private readonly StockItem stockItem;

        private readonly string stockItemId = Guid.NewGuid().ToString();

        private readonly UpdateStockItem updateStockItem;

        private readonly string userId = Guid.NewGuid().ToString();

        public StockItemControllerTests()
        {
            this.createStockItem = new CreateStockItem(
                this.name,
                StockItemControllerTests.Quantity,
                StockItemControllerTests.MinimumQuantity);

            this.stockItem = new StockItem(
                this.stockItemId,
                this.name,
                StockItemControllerTests.Quantity,
                StockItemControllerTests.MinimumQuantity,
                this.userId);

            this.updateStockItem = new UpdateStockItem(
                this.stockItemId,
                $"{this.name}_update",
                StockItemControllerTests.Quantity + 1,
                StockItemControllerTests.MinimumQuantity + 1);
        }

        [Fact]
        public async Task CreateAsync()
        {
            var stockItemService = new Mock<IStockItemService>();
            stockItemService.Setup(
                    mock => mock.CreateAsync(
                        It.IsAny<ICreateStockItem>(),
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IStockItem>(this.stockItem));

            var controller = new StockItemController(stockItemService.Object)
            {
                ControllerContext = ControllerContextService.Create(this.userId)
            };

            var result = await controller.Post(
                this.createStockItem,
                new CancellationToken());

            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            var actual = Assert.IsAssignableFrom<IStockItem>(actionResult.Value);

            Assert.Equal(
                this.userId,
                actual.UserId);
            Assert.Equal(
                this.stockItemId,
                actual.Id);
            Assert.Equal(
                StockItemControllerTests.Quantity,
                actual.Quantity);
            Assert.Equal(
                StockItemControllerTests.MinimumQuantity,
                actual.MinimumQuantity);
            Assert.Equal(
                this.name,
                actual.Name);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task DeleteAsync(bool isDeleted)
        {
            var stockItemService = new Mock<IStockItemService>();
            var deleteAsyncSetup = stockItemService.Setup(
                mock => mock.DeleteAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()));
            if (isDeleted)
            {
                deleteAsyncSetup.Returns(Task.CompletedTask);
            }
            else
            {
                deleteAsyncSetup.Throws<NotFoundException>();
            }

            var controller = new StockItemController(stockItemService.Object)
            {
                ControllerContext = ControllerContextService.Create(this.userId)
            };

            if (isDeleted)

            {
                var result = await controller.Delete(
                    this.stockItemId,
                    new CancellationToken());
                Assert.IsType<OkResult>(result);
            }
            else
            {
                await Assert.ThrowsAsync<NotFoundException>(
                    () => controller.Delete(
                        this.stockItemId,
                        new CancellationToken()));
            }
        }

        [Fact]
        public async Task ReadAsync()
        {
            var stockItemService = new Mock<IStockItemService>();
            stockItemService.Setup(
                    mock => mock.ReadAsync(
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IEnumerable<IStockItem>>(new[] {this.stockItem}));

            var controller = new StockItemController(stockItemService.Object)
            {
                ControllerContext = ControllerContextService.Create(this.userId)
            };

            var result = (await controller.Get(new CancellationToken())).ToArray();

            var actual = result.First();
            Assert.Equal(
                this.userId,
                actual.UserId);
            Assert.Equal(
                this.stockItemId,
                actual.Id);
            Assert.Equal(
                StockItemControllerTests.Quantity,
                actual.Quantity);
            Assert.Equal(
                StockItemControllerTests.MinimumQuantity,
                actual.MinimumQuantity);
            Assert.Equal(
                this.name,
                actual.Name);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task ReadByIdAsync(bool hasResult)
        {
            var stockItemService = new Mock<IStockItemService>();
            var stockItemServiceSetup = stockItemService.Setup(
                mock => mock.ReadByIdAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()));
            if (hasResult)
            {
                stockItemServiceSetup.Returns(Task.FromResult<IStockItem>(this.stockItem));
            }
            else
            {
                stockItemServiceSetup.Throws<NotFoundException>();
            }

            var controller = new StockItemController(stockItemService.Object)
            {
                ControllerContext = ControllerContextService.Create(this.userId)
            };

            if (!hasResult)
            {
                await Assert.ThrowsAsync<NotFoundException>(
                    () => controller.Get(
                        this.stockItemId,
                        new CancellationToken()));
            }
            else
            {
                var result = await controller.Get(
                    this.stockItemId,
                    new CancellationToken());
                var actionResult = Assert.IsType<OkObjectResult>(result.Result);

                var actual = Assert.IsAssignableFrom<IStockItem>(actionResult.Value);

                Assert.Equal(
                    this.userId,
                    actual.UserId);
                Assert.Equal(
                    this.stockItemId,
                    actual.Id);
                Assert.Equal(
                    StockItemControllerTests.Quantity,
                    actual.Quantity);
                Assert.Equal(
                    StockItemControllerTests.MinimumQuantity,
                    actual.MinimumQuantity);
                Assert.Equal(
                    this.name,
                    actual.Name);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task UpdateAsync(bool isUpdated)
        {
            var stockItemService = new Mock<IStockItemService>();
            var updateAsyncSetup = stockItemService.Setup(
                mock => mock.UpdateAsync(
                    It.IsAny<UpdateStockItem>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()));
            if (isUpdated)
            {
                updateAsyncSetup.Returns(Task.CompletedTask);
            }
            else
            {
                updateAsyncSetup.Throws<NotFoundException>();
            }

            var controller = new StockItemController(stockItemService.Object)
            {
                ControllerContext = ControllerContextService.Create(this.userId)
            };

            ;

            if (isUpdated)
            {
                var result = await controller.Put(
                    this.updateStockItem,
                    new CancellationToken());
                Assert.IsType<OkResult>(result);
            }
            else
            {
                await Assert.ThrowsAsync<NotFoundException>(
                    () => controller.Put(
                        this.updateStockItem,
                        new CancellationToken()));
            }
        }
    }
}
