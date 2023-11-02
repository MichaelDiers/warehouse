namespace Warehouse.Api.Tests.Services
{
    using Moq;
    using Warehouse.Api.Contracts.StockItems;
    using Warehouse.Api.Extensions;
    using Warehouse.Api.Models.StockItems;
    using Warehouse.Api.Tests.Utilities;

    /// <summary>
    ///     Tests for <see cref="IStockItemService" />
    /// </summary>
    [Trait(
        "Type",
        "Unit")]
    public class StockItemServiceTests
    {
        [Fact]
        public async Task CreateAsync()
        {
            var stockItemProviderMock = new Mock<IStockItemProvider>();
            stockItemProviderMock.Setup(provider => provider.CreateAsync(It.IsAny<IStockItem>()));

            var service = TestHostApplicationBuilder.GetService<IStockItemService, IStockItemProvider>(
                new[] {ServiceCollectionExtensions.AddDependencies},
                stockItemProviderMock.Object);

            var createStockItem = new CreateStockItem(
                "name",
                100);
            var userId = Guid.NewGuid().ToString();

            var stockItem = await service.CreateAsync(
                createStockItem,
                userId);

            Assert.Equal(
                createStockItem.Quantity,
                stockItem.Quantity);
            Assert.Equal(
                createStockItem.Name,
                stockItem.Name);
            Assert.Equal(
                userId,
                stockItem.UserId);
            Assert.True(
                Guid.TryParse(
                    stockItem.Id,
                    out var guid) &&
                guid != Guid.Empty);
        }
    }
}
