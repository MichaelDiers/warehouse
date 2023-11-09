namespace Warehouse.Api.Tests.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Warehouse.Api.Contracts.ShoppingItems;
    using Warehouse.Api.Controllers;
    using Warehouse.Api.Exceptions;
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

        private readonly string name = Guid.NewGuid().ToString();

        private readonly ShoppingItem shoppingItem;

        private readonly string shoppingItemId = Guid.NewGuid().ToString();

        private readonly string stockItemId = Guid.NewGuid().ToString();

        private readonly string userId = Guid.NewGuid().ToString();

        public ShoppingItemControllerTests()
        {
            this.shoppingItem = new ShoppingItem(
                this.shoppingItemId,
                this.name,
                ShoppingItemControllerTests.Quantity,
                this.userId,
                this.stockItemId);
        }

        [Fact]
        public async Task ReadAsync()
        {
            var shoppingItemService = new Mock<IDomainShoppingItemService>();
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
            var shoppingItemService = new Mock<IDomainShoppingItemService>();
            var shoppingItemServiceSetup = shoppingItemService.Setup(
                mock => mock.ReadByIdAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()));
            if (hasResult)
            {
                shoppingItemServiceSetup.Returns(Task.FromResult<IShoppingItem>(this.shoppingItem));
            }
            else
            {
                shoppingItemServiceSetup.Throws<NotFoundException>();
            }

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
    }
}
