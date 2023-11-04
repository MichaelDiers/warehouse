namespace Warehouse.Api.Tests.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Warehouse.Api.Contracts.ShoppingItems;
    using Warehouse.Api.Controllers;
    using Warehouse.Api.Models.ShoppingItems;
    using Warehouse.Api.Tests.Utilities;

    /// <summary>
    ///     Tests for <see cref="ShoppingItemController" />.
    /// </summary>
    [Trait(
        Constants.TraitType,
        Constants.TraitValueUnitTest)]
    public class ShoppingItemControllerTests
    {
        private const int Quantity = 100;

        private readonly CreateShoppingItem createShoppingItem;

        private readonly string name = Guid.NewGuid().ToString();

        private readonly ShoppingItem shoppingItem;

        private readonly string shoppingItemId = Guid.NewGuid().ToString();

        private readonly UpdateShoppingItem updateShoppingItem;

        private readonly string userId = Guid.NewGuid().ToString();

        public ShoppingItemControllerTests()
        {
            this.createShoppingItem = new CreateShoppingItem(
                this.name,
                ShoppingItemControllerTests.Quantity);

            this.shoppingItem = new ShoppingItem(
                this.shoppingItemId,
                this.name,
                ShoppingItemControllerTests.Quantity,
                this.userId);

            this.updateShoppingItem = new UpdateShoppingItem(
                this.shoppingItemId,
                $"{this.name}_update",
                ShoppingItemControllerTests.Quantity + 1);
        }

        [Fact]
        public async Task CreateAsync()
        {
            var shoppingItemService = new Mock<IShoppingItemService>();
            shoppingItemService.Setup(
                    mock => mock.CreateAsync(
                        It.IsAny<ICreateShoppingItem>(),
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IShoppingItem>(this.shoppingItem));

            var controller = new ShoppingItemController(shoppingItemService.Object)
            {
                ControllerContext = ControllerContextService.Create(this.userId)
            };

            var result = await controller.Post(
                this.createShoppingItem,
                new CancellationToken());

            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            var actual = Assert.IsAssignableFrom<IShoppingItem>(actionResult.Value);

            Assert.Equal(
                this.userId,
                actual.UserId);
            Assert.Equal(
                this.shoppingItemId,
                actual.Id);
            Assert.Equal(
                ShoppingItemControllerTests.Quantity,
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
            var shoppingItemService = new Mock<IShoppingItemService>();
            shoppingItemService.Setup(
                    mock => mock.DeleteAsync(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(isDeleted));

            var controller = new ShoppingItemController(shoppingItemService.Object)
            {
                ControllerContext = ControllerContextService.Create(this.userId)
            };

            var result = await controller.Delete(
                this.shoppingItemId,
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
            var shoppingItemService = new Mock<IShoppingItemService>();
            shoppingItemService.Setup(
                    mock => mock.ReadAsync(
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IEnumerable<IShoppingItem>>(new[] {this.shoppingItem}));

            var controller = new ShoppingItemController(shoppingItemService.Object)
            {
                ControllerContext = ControllerContextService.Create(this.userId)
            };

            var result = (await controller.Get(new CancellationToken())).ToArray();

            var actual = result.First();
            Assert.Equal(
                this.userId,
                actual.UserId);
            Assert.Equal(
                this.shoppingItemId,
                actual.Id);
            Assert.Equal(
                ShoppingItemControllerTests.Quantity,
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
            var shoppingItemService = new Mock<IShoppingItemService>();
            shoppingItemService.Setup(
                    mock => mock.ReadByIdAsync(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IShoppingItem?>(hasResult ? this.shoppingItem : null));

            var controller = new ShoppingItemController(shoppingItemService.Object)
            {
                ControllerContext = ControllerContextService.Create(this.userId)
            };

            var result = await controller.Get(
                this.shoppingItemId,
                new CancellationToken());

            if (!hasResult)
            {
                Assert.IsType<NotFoundResult>(result.Result);
            }
            else
            {
                var actionResult = Assert.IsType<OkObjectResult>(result.Result);

                var actual = Assert.IsAssignableFrom<IShoppingItem>(actionResult.Value);

                Assert.Equal(
                    this.userId,
                    actual.UserId);
                Assert.Equal(
                    this.shoppingItemId,
                    actual.Id);
                Assert.Equal(
                    ShoppingItemControllerTests.Quantity,
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
            var shoppingItemService = new Mock<IShoppingItemService>();
            shoppingItemService.Setup(
                    mock => mock.UpdateAsync(
                        It.IsAny<UpdateShoppingItem>(),
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(isUpdated));

            var controller = new ShoppingItemController(shoppingItemService.Object)
            {
                ControllerContext = ControllerContextService.Create(this.userId)
            };

            var result = await controller.Put(
                this.updateShoppingItem,
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
