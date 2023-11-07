namespace Warehouse.Api.Tests.Models.StockItems
{
    using Warehouse.Api.Contracts.StockItems;
    using Warehouse.Api.Models.StockItems;
    using Warehouse.Api.Tests.Utilities;

    [Trait(
        Constants.TraitType,
        Constants.TraitValueUnitTest)]
    public class StockItemTests
    {
        [Theory]
        [InlineData(
            "id",
            "name",
            10,
            20,
            "userid")]
        public void BaseCtor(
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

            Assert.IsAssignableFrom<IStockItem>(stockItem);

            Assert.Equal(
                id,
                stockItem.Id);
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

            var stockItem = new StockItem(origin);

            Assert.IsAssignableFrom<IStockItem>(stockItem);

            Assert.Equal(
                id,
                stockItem.Id);
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
            var stockItem = new StockItem();

            Assert.IsAssignableFrom<IStockItem>(stockItem);

            Assert.Equal(
                string.Empty,
                stockItem.Id);
            Assert.Equal(
                string.Empty,
                stockItem.Name);
            Assert.Equal(
                string.Empty,
                stockItem.UserId);
            Assert.Equal(
                0,
                stockItem.Quantity);
            Assert.Equal(
                0,
                stockItem.MinimumQuantity);
        }
    }
}
