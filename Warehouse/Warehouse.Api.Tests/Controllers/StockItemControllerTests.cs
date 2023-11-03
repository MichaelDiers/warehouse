namespace Warehouse.Api.Tests.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Warehouse.Api.Contracts.StockItems;
    using Warehouse.Api.Controllers;
    using Warehouse.Api.Models.StockItems;
    using Warehouse.Api.Tests.Utilities;

    /// <summary>
    ///     Tests for <see cref="StockItemController" />.
    /// </summary>
    public class StockItemControllerTests
    {
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
                StockItemControllerTests.Quantity);

            this.stockItem = new StockItem(
                this.stockItemId,
                this.name,
                StockItemControllerTests.Quantity,
                this.userId);

            this.updateStockItem = new UpdateStockItem(
                this.stockItemId,
                $"{this.name}_update",
                StockItemControllerTests.Quantity + 1);
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
                this.name,
                actual.Name);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task DeleteAsync(bool isDeleted)
        {
            var stockItemService = new Mock<IStockItemService>();
            stockItemService.Setup(
                    mock => mock.DeleteAsync(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(isDeleted));

            var controller = new StockItemController(stockItemService.Object)
            {
                ControllerContext = ControllerContextService.Create(this.userId)
            };

            var result = await controller.Delete(
                this.stockItemId,
                new CancellationToken());

            if (isDeleted)
            {
                Assert.IsType<OkResult>(result);
            }
            else
            {
                Assert.IsType<NotFoundResult>(result);
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
                this.name,
                actual.Name);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task ReadByIdAsync(bool hasResult)
        {
            var stockItemService = new Mock<IStockItemService>();
            stockItemService.Setup(
                    mock => mock.ReadByIdAsync(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IStockItem?>(hasResult ? this.stockItem : null));

            var controller = new StockItemController(stockItemService.Object)
            {
                ControllerContext = ControllerContextService.Create(this.userId)
            };

            var result = await controller.Get(
                this.stockItemId,
                new CancellationToken());

            if (!hasResult)
            {
                Assert.IsType<NotFoundResult>(result.Result);
            }
            else
            {
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
            stockItemService.Setup(
                    mock => mock.UpdateAsync(
                        It.IsAny<UpdateStockItem>(),
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(isUpdated));

            var controller = new StockItemController(stockItemService.Object)
            {
                ControllerContext = ControllerContextService.Create(this.userId)
            };

            var result = await controller.Put(
                this.updateStockItem,
                new CancellationToken());

            if (isUpdated)
            {
                Assert.IsType<OkResult>(result);
            }
            else
            {
                Assert.IsType<NotFoundResult>(result);
            }
        }
    }
}
