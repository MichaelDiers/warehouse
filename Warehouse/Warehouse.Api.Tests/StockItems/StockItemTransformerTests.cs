namespace Warehouse.Api.Tests.StockItems
{
    using Generic.Base.Api.Models;
    using Generic.Base.Api.Transformer;
    using Warehouse.Api.StockItems;

    /// <summary>
    ///     Tests for <see cref="StockItemTransformer" />.
    /// </summary>
    public class StockItemTransformerTests
    {
        [Fact]
        public void IsIControllerTransformer()
        {
            Assert.IsAssignableFrom<IControllerTransformer<StockItem, ResultStockItem>>(new StockItemTransformer());
        }

        [Fact]
        public void IsIProviderEntryTransformer()
        {
            Assert.IsAssignableFrom<IProviderEntryTransformer<StockItem, StockItem>>(new StockItemTransformer());
        }

        [Fact]
        public void IsIUserBoundAtomicTransformer()
        {
            Assert.IsAssignableFrom<IUserBoundAtomicTransformer<CreateStockItem, StockItem, UpdateStockItem>>(
                new StockItemTransformer());
        }

        [Theory]
        [InlineData(
            "name",
            10,
            11,
            "userId")]
        public void TransformCreateStockItemToStockItem(
            string name,
            int quantity,
            int minimumQuantity,
            string userId
        )
        {
            var createStockItem = new CreateStockItem(
                minimumQuantity,
                name,
                quantity);

            var stockItem = new StockItemTransformer().Transform(
                createStockItem,
                userId);

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

        [Fact]
        public void TransformStockItemToResultStockItem()
        {
            var stockItem = new StockItem(
                "id",
                "name",
                10,
                20,
                "userId");
            var links = new[]
            {
                new Link(
                    "urn",
                    "url")
            };

            var resultStockItem = new StockItemTransformer().Transform(
                stockItem,
                links);

            Assert.Equal(
                stockItem.Name,
                resultStockItem.Name);
            Assert.Equal(
                stockItem.MinimumQuantity,
                resultStockItem.MinimumQuantity);
            Assert.Equal(
                stockItem.Quantity,
                resultStockItem.Quantity);
            Assert.Single(resultStockItem.Links);
            Assert.Contains(
                resultStockItem.Links,
                link => link.Url == links.First().Url && link.Urn == links.First().Urn);
        }

        [Theory]
        [InlineData(
            "id",
            "name",
            10,
            11,
            "userId")]
        public void TransformStockItemToStockItem(
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

            var converted = new StockItemTransformer().Transform(stockItem);

            Assert.Equal(
                id,
                converted.Id);
            Assert.Equal(
                name,
                converted.Name);
            Assert.Equal(
                quantity,
                converted.Quantity);
            Assert.Equal(
                minimumQuantity,
                converted.MinimumQuantity);
            Assert.Equal(
                userId,
                converted.UserId);
        }

        [Theory]
        [InlineData(
            "id",
            "name",
            10,
            11,
            "userId")]
        public void TransformUpdateStockItemToStockItem(
            string id,
            string name,
            int quantity,
            int minimumQuantity,
            string userId
        )
        {
            var updateStockItem = new UpdateStockItem(
                name,
                quantity,
                minimumQuantity);

            var stockItem = new StockItemTransformer().Transform(
                updateStockItem,
                userId,
                id);

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
            Assert.Equal(
                id,
                stockItem.Id);
        }
    }
}
