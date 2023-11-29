namespace Warehouse.Api.Tests.StockItems
{
    using Warehouse.Api.StockItems;

    /// <summary>
    ///     Tests for <see cref="UpdateStockItem" />.
    /// </summary>
    [Trait(
        "TestType",
        "UnitTest")]
    public class UpdateStockItemTests
    {
        [Theory]
        [InlineData(
            "name",
            10,
            20)]
        public void Ctor(string name, int quantity, int minimumQuantity)
        {
            var update = new UpdateStockItem(
                name,
                quantity,
                minimumQuantity);

            Assert.Equal(
                name,
                update.Name);
            Assert.Equal(
                quantity,
                update.Quantity);
            Assert.Equal(
                minimumQuantity,
                update.MinimumQuantity);
        }
    }
}
