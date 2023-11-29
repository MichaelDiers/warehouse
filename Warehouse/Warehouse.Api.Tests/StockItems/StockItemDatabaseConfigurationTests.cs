namespace Warehouse.Api.Tests.StockItems
{
    using Warehouse.Api.StockItems;

    /// <summary>
    ///     Tests for <see cref="StockItemDatabaseConfiguration" />.
    /// </summary>
    [Trait(
        "TestType",
        "UnitTest")]
    public class StockItemDatabaseConfigurationTests
    {
        [Theory]
        [InlineData(
            "collection",
            "database")]
        public void Ctor(string collectionName, string databaseName)
        {
            var configuration = new StockItemDatabaseConfiguration(
                collectionName,
                databaseName);

            Assert.Equal(
                collectionName,
                configuration.CollectionName);
            Assert.Equal(
                databaseName,
                configuration.DatabaseName);
        }
    }
}
