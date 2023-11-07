namespace Warehouse.Api.Tests.Models.StockItems
{
    using Warehouse.Api.Models.StockItems;
    using Warehouse.Api.Tests.Utilities;

    [Trait(
        Constants.TraitType,
        Constants.TraitValueUnitTest)]
    public class DatabaseStockItemTests
    {
        [Theory]
        [InlineData(
            "id",
            "name",
            10,
            20,
            "userid")]
        public void CopyCtor(
            string id,
            string name,
            int quantity,
            int minimumQuantity,
            string userId
        )
        {
            var origin = new StockItem(
                id,
                name,
                quantity,
                minimumQuantity,
                userId);

            var stockItem = new DatabaseStockItem(origin);

            Assert.Null(stockItem.Id);
            Assert.Equal(
                id,
                stockItem.StockItemId);
            Assert.Equal(
                name,
                stockItem.Name);
            Assert.Equal(
                userId,
                stockItem.UserId);
            Assert.Equal(
                quantity,
                stockItem.Quantity);
            Assert.Equal(
                minimumQuantity,
                stockItem.MinimumQuantity);
        }

        [Fact]
        public void EmptyCtor()
        {
            var stockItem = new DatabaseStockItem();

            Assert.Null(stockItem.Id);
            Assert.Null(stockItem.StockItemId);
            Assert.Null(stockItem.Name);
            Assert.Null(stockItem.UserId);
            Assert.Null(stockItem.Quantity);
            Assert.Null(stockItem.MinimumQuantity);
        }
    }
}
