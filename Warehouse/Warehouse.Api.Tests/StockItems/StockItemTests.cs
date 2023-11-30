namespace Warehouse.Api.Tests.StockItems
{
    using Warehouse.Api.StockItems;

    /// <summary>
    ///     Tests for <see cref="StockItem" />.
    /// </summary>
    [Trait(
        "TestType",
        "UnitTest")]
    public class StockItemTests
    {
        [Theory]
        [InlineData(
            "id",
            "name",
            10,
            20,
            "userId")]
        public void Ctor(
            string id,
            string name,
            int quantity,
            int minimumQuantity,
            string userId
        )
        {
            var stockItem = new StockItem(
                id,
                name,
                quantity,
                minimumQuantity,
                userId);

            Assert.Equal(
                id,
                stockItem.Id);
            Assert.Equal(
                name,
                stockItem.Name);
            Assert.Equal(
                quantity,
                stockItem.Quantity);
            Assert.Equal(
                minimumQuantity,
                stockItem.MinimumQuantity);
            Assert.Equal(
                userId,
                stockItem.UserId);
        }
    }
}
