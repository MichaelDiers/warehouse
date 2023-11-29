namespace Warehouse.Api.Tests.StockItems
{
    using Warehouse.Api.StockItems;

    /// <summary>
    ///     Tests for <see cref="CreateStockItem" />.
    /// </summary>
    [Trait(
        "TestType",
        "UnitTest")]
    public class CreateStockItemTests
    {
        [Theory]
        [InlineData(
            10,
            "name",
            20)]
        public void Ctor(int minimumQuantity, string name, int quantity)
        {
            var createStockItem = new CreateStockItem(
                minimumQuantity,
                name,
                quantity);

            Assert.Equal(
                minimumQuantity,
                createStockItem.MinimumQuantity);
            Assert.Equal(
                name,
                createStockItem.Name);
            Assert.Equal(
                quantity,
                createStockItem.Quantity);
        }
    }
}
