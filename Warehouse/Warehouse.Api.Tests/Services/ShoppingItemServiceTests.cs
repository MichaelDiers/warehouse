namespace Warehouse.Api.Tests.Services
{
    using Moq;
    using Warehouse.Api.Contracts;
    using Warehouse.Api.Contracts.ShoppingItems;
    using Warehouse.Api.Extensions;
    using Warehouse.Api.Models.ShoppingItems;
    using Warehouse.Api.Tests.Utilities;

    /// <summary>
    ///     Tests for <see cref="IAtomicShoppingItemService" />
    /// </summary>
    [Trait(
        Constants.TraitType,
        Constants.TraitValueUnitTest)]
    public class ShoppingItemServiceTests
    {
        [Fact]
        public async Task CreateAsync()
        {
            var shoppingItemProviderMock = new Mock<IShoppingItemProvider>();
            shoppingItemProviderMock.Setup(
                provider => provider.CreateAsync(
                    It.IsAny<IShoppingItem>(),
                    It.IsAny<CancellationToken>()));

            var service = TestHostApplicationBuilder.GetService<IAtomicShoppingItemService, IShoppingItemProvider>(
                new[] {ServiceCollectionExtensions.AddDependencies},
                shoppingItemProviderMock.Object);

            var createShoppingItem = new CreateShoppingItem(
                "name",
                100,
                Guid.NewGuid().ToString());
            var userId = Guid.NewGuid().ToString();

            var shoppingItem = await service.CreateAsync(
                createShoppingItem,
                userId,
                It.IsAny<CancellationToken>());

            Assert.Equal(
                createShoppingItem.Quantity,
                shoppingItem.Quantity);
            Assert.Equal(
                createShoppingItem.Name,
                shoppingItem.Name);
            Assert.Equal(
                userId,
                shoppingItem.UserId);
            Assert.True(
                Guid.TryParse(
                    shoppingItem.Id,
                    out var guid) &&
                guid != Guid.Empty);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task DeleteAsync(bool isDeleted)
        {
            var userId = Guid.NewGuid().ToString();
            var shoppingItemId = Guid.NewGuid().ToString();

            var shoppingItemProviderMock = new Mock<IShoppingItemProvider>();
            shoppingItemProviderMock.Setup(
                    mock => mock.DeleteAsync(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(isDeleted));

            var service = TestHostApplicationBuilder.GetService<IAtomicShoppingItemService, IShoppingItemProvider>(
                new[] {ServiceCollectionExtensions.AddDependencies},
                shoppingItemProviderMock.Object);

            Assert.Equal(
                isDeleted,
                await service.DeleteAsync(
                    userId,
                    shoppingItemId,
                    CancellationToken.None));

            shoppingItemProviderMock.Verify(
                mock => mock.DeleteAsync(
                    userId,
                    shoppingItemId,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task ReadAsync()
        {
            var userId = Guid.NewGuid().ToString();
            var expectedShoppingItems = Enumerable.Range(
                    1,
                    10)
                .Select(
                    i => new ShoppingItem(
                        Guid.NewGuid().ToString(),
                        $"{i}",
                        i,
                        userId,
                        Guid.NewGuid().ToString()))
                .ToArray();
            var shoppingItemProviderMock = new Mock<IShoppingItemProvider>();
            shoppingItemProviderMock.Setup(
                    provider => provider.ReadAsync(
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(expectedShoppingItems as IEnumerable<IShoppingItem>));

            var service = TestHostApplicationBuilder.GetService<IAtomicShoppingItemService, IShoppingItemProvider>(
                new[] {ServiceCollectionExtensions.AddDependencies},
                shoppingItemProviderMock.Object);

            var shoppingItems = (await service.ReadAsync(
                userId,
                It.IsAny<CancellationToken>())).ToArray();

            Assert.Equal(
                expectedShoppingItems.Length,
                shoppingItems.Length);
            foreach (var expectedShoppingItem in expectedShoppingItems)
            {
                Assert.NotNull(
                    shoppingItems.FirstOrDefault(
                        si => si.Quantity == expectedShoppingItem.Quantity &&
                              si.Name == expectedShoppingItem.Name &&
                              si.Id == expectedShoppingItem.Id &&
                              si.UserId == expectedShoppingItem.UserId));
            }
        }

        [Fact]
        public async Task ReadByIdAsync()
        {
            var userId = Guid.NewGuid().ToString();
            var expectedShoppingItem = new ShoppingItem(
                Guid.NewGuid().ToString(),
                "name",
                1,
                userId,
                Guid.NewGuid().ToString()) as IShoppingItem;

            var shoppingItemProviderMock = new Mock<IShoppingItemProvider>();
            shoppingItemProviderMock.Setup(
                    provider => provider.ReadByIdAsync(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IShoppingItem?>(expectedShoppingItem));

            var service = TestHostApplicationBuilder.GetService<IAtomicShoppingItemService, IShoppingItemProvider>(
                new[] {ServiceCollectionExtensions.AddDependencies},
                shoppingItemProviderMock.Object);

            var shoppingItem = await service.ReadByIdAsync(
                userId,
                expectedShoppingItem.Id,
                It.IsAny<CancellationToken>());

            Assert.NotNull(shoppingItem);

            Assert.Equal(
                expectedShoppingItem.Id,
                shoppingItem.Id);
            Assert.Equal(
                expectedShoppingItem.Name,
                shoppingItem.Name);
            Assert.Equal(
                expectedShoppingItem.Quantity,
                shoppingItem.Quantity);
            Assert.Equal(
                expectedShoppingItem.UserId,
                shoppingItem.UserId);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task UpdateAsync(bool isUpdated)
        {
            var userId = Guid.NewGuid().ToString();
            var shoppingItemId = Guid.NewGuid().ToString();

            var shoppingItemProviderMock = new Mock<IShoppingItemProvider>();
            shoppingItemProviderMock.Setup(
                    mock => mock.UpdateAsync(
                        It.IsAny<IShoppingItem>(),
                        It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(isUpdated));

            var service = TestHostApplicationBuilder.GetService<IAtomicShoppingItemService, IShoppingItemProvider>(
                new[] {ServiceCollectionExtensions.AddDependencies},
                shoppingItemProviderMock.Object);

            Assert.Equal(
                isUpdated,
                await service.UpdateAsync(
                    new UpdateShoppingItem(
                        shoppingItemId,
                        "name",
                        10,
                        Guid.NewGuid().ToString()),
                    userId,
                    CancellationToken.None));
        }

        [Theory]
        [InlineData(
            UpdateOperation.Decrease,
            10,
            true)]
        [InlineData(
            UpdateOperation.Decrease,
            10,
            false)]
        [InlineData(
            UpdateOperation.Increase,
            10,
            true)]
        [InlineData(
            UpdateOperation.Increase,
            10,
            false)]
        [InlineData(
            (UpdateOperation) int.MaxValue,
            10,
            false)]
        [InlineData(
            UpdateOperation.Decrease,
            0,
            true)]
        [InlineData(
            UpdateOperation.Increase,
            0,
            true)]
        public async Task UpdateByQuantityDeltaAsync(UpdateOperation operation, int quantity, bool isUpdated)
        {
            var userId = Guid.NewGuid().ToString();
            var shoppingItemId = Guid.NewGuid().ToString();

            var shoppingItemProviderMock = new Mock<IShoppingItemProvider>();
            shoppingItemProviderMock.Setup(
                    mock => mock.UpdateAsync(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<int>(),
                        It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(isUpdated));

            var service = TestHostApplicationBuilder.GetService<IAtomicShoppingItemService, IShoppingItemProvider>(
                new[] {ServiceCollectionExtensions.AddDependencies},
                shoppingItemProviderMock.Object);

            var isOperationValid = operation switch
            {
                UpdateOperation.Decrease => true,
                UpdateOperation.Increase => true,
                _ => false
            };

            if (isOperationValid)
            {
                Assert.Equal(
                    isUpdated,
                    await service.UpdateAsync(
                        userId,
                        shoppingItemId,
                        operation,
                        quantity,
                        new CancellationToken()));
            }
            else
            {
                await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                    () => service.UpdateAsync(
                        userId,
                        shoppingItemId,
                        operation,
                        quantity,
                        new CancellationToken()));
            }

            if (operation is UpdateOperation.Decrease or UpdateOperation.Increase && quantity != 0)
            {
                shoppingItemProviderMock.Verify(
                    mock => mock.UpdateAsync(
                        userId,
                        shoppingItemId,
                        operation == UpdateOperation.Increase ? quantity : -quantity,
                        It.IsAny<CancellationToken>()));
            }

            shoppingItemProviderMock.VerifyNoOtherCalls();
        }
    }
}
