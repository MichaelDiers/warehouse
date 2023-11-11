namespace Warehouse.Api.Tests.Models.ShoppingItems
{
    using Warehouse.Api.Models.ShoppingItems;
    using Warehouse.Api.Tests.Utilities;

    [Trait(
        Constants.TraitType,
        Constants.TraitValueUnitTest)]
    public class DatabaseShoppingItemTests
    {
        [Fact]
        public void CheckCollectionName()
        {
            Assert.Equal(
                "DatabaseShoppingItems",
                DatabaseShoppingItem.CollectionName);
        }

        [Theory]
        [InlineData(
            "id",
            "name",
            10,
            "userid",
            "stockItemId")]
        public void CopyCtor(
            string id,
            string name,
            int quantity,
            string userId,
            string stockItemId
        )
        {
            var origin = new ShoppingItem(
                id,
                name,
                quantity,
                userId,
                stockItemId);

            var shoppingItem = new DatabaseShoppingItem(origin);

            Assert.Equal(
                origin.Id,
                shoppingItem.ShoppingItemId);
            Assert.Null(shoppingItem.Id);
            Assert.Equal(
                name,
                shoppingItem.Name);
            Assert.Equal(
                userId,
                shoppingItem.UserId);
            Assert.Equal(
                quantity,
                shoppingItem.Quantity);
            Assert.Equal(
                stockItemId,
                shoppingItem.StockItemId);
        }

        [Fact]
        public void EmptyCtor()
        {
            var shoppingItem = new DatabaseShoppingItem();

            Assert.Null(shoppingItem.Id);
            Assert.Null(shoppingItem.StockItemId);
            Assert.Null(shoppingItem.Name);
            Assert.Null(shoppingItem.UserId);
            Assert.Null(shoppingItem.Quantity);
            Assert.Null(shoppingItem.StockItemId);
        }
    }
}
