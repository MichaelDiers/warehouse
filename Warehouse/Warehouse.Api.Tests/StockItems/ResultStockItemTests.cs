namespace Warehouse.Api.Tests.StockItems
{
    using Generic.Base.Api.Models;
    using Warehouse.Api.StockItems;

    /// <summary>
    ///     Tests for <see cref="ResultStockItem" />.
    /// </summary>
    public class ResultStockItemTests
    {
        [Theory]
        [InlineData(
            10,
            Urn.Options,
            "url",
            "name",
            20)]
        public void Ctor(
            int minimumQuantity,
            Urn urn,
            string url,
            string name,
            int quantity
        )
        {
            var resultStockItem = new ResultStockItem(
                new[]
                {
                    new Link(
                        urn,
                        url)
                },
                minimumQuantity,
                name,
                quantity);

            Assert.Equal(
                minimumQuantity,
                resultStockItem.MinimumQuantity);
            Assert.Single(resultStockItem.Links);
            Assert.Contains(
                resultStockItem.Links,
                link => link.Url == url && link.Urn == $"urn:{urn.ToString()}");
            Assert.Equal(
                name,
                resultStockItem.Name);
            Assert.Equal(
                quantity,
                resultStockItem.Quantity);
        }
    }
}
