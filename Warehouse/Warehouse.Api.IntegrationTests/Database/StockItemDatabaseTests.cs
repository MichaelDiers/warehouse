namespace Warehouse.Api.IntegrationTests.Database
{
    using MongoDB.Driver;
    using Warehouse.Api.IntegrationTests.Lib;
    using Warehouse.Api.Models.StockItems;

    public class StockItemDatabaseTests
    {
        [Theory]
        [InlineData(-1)]
        [InlineData(10000)]
        public async Task CreateAsyncMinimumQuantityInvalid(int minimumQuantity)
        {
            var stockItem = this.DatabaseStockItem();
            stockItem.MinimumQuantity = minimumQuantity;
            var collection = TestMongoClient.StockItemCollection();

            var exception = await Assert.ThrowsAsync<MongoWriteException>(() => collection.InsertOneAsync(stockItem));
            Assert.Equal(
                121,
                exception.WriteError.Code);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(101)]
        public async Task CreateAsyncNameInvalid(int nameLength)
        {
            var stockItem = this.DatabaseStockItem();
            stockItem.Name = new string(
                'a',
                nameLength);
            var collection = TestMongoClient.StockItemCollection();

            var exception = await Assert.ThrowsAsync<MongoWriteException>(() => collection.InsertOneAsync(stockItem));
            Assert.Equal(
                121,
                exception.WriteError.Code);
        }

        [Fact]
        public async Task CreateAsyncNameNotUnique()
        {
            var stockItem1 = this.DatabaseStockItem();
            var collection = TestMongoClient.StockItemCollection();

            await collection.InsertOneAsync(stockItem1);

            var stockItem2 = this.DatabaseStockItem();
            stockItem2.UserId = stockItem1.UserId;
            stockItem2.Name = stockItem1.Name;

            var exception = await Assert.ThrowsAsync<MongoWriteException>(() => collection.InsertOneAsync(stockItem2));
            Assert.Equal(
                11000,
                exception.WriteError.Code);
        }

        [Fact]
        public async Task CreateAsyncOk()
        {
            var stockItem = this.DatabaseStockItem();
            var collection = TestMongoClient.StockItemCollection();

            await collection.InsertOneAsync(stockItem);

            var actual = await (await collection.FindAsync(item => item.StockItemId == stockItem.StockItemId))
                .FirstOrDefaultAsync();

            Assert.NotNull(actual);

            await collection.InsertOneAsync(this.DatabaseStockItem());
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(10000)]
        public async Task CreateAsyncQuantityInvalid(int quantity)
        {
            var stockItem = this.DatabaseStockItem();
            stockItem.Quantity = quantity;
            var collection = TestMongoClient.StockItemCollection();

            var exception = await Assert.ThrowsAsync<MongoWriteException>(() => collection.InsertOneAsync(stockItem));
            Assert.Equal(
                121,
                exception.WriteError.Code);
        }

        [Theory]
        [InlineData("7c2c71da-72f0-4501-b949-3a97e4ed980")]
        [InlineData("7c2c71da-72f0-4501-b949-3a97e4ed98041")]
        public async Task CreateAsyncStockItemIdInvalid(string stockItemId)
        {
            var stockItem = this.DatabaseStockItem();
            stockItem.StockItemId = stockItemId;

            var collection = TestMongoClient.StockItemCollection();

            var exception = await Assert.ThrowsAsync<MongoWriteException>(() => collection.InsertOneAsync(stockItem));
            Assert.Equal(
                121,
                exception.WriteError.Code);
        }

        [Fact]
        public async Task CreateAsyncStockItemIdNotUnique()
        {
            var stockItem1 = this.DatabaseStockItem();
            var collection = TestMongoClient.StockItemCollection();

            await collection.InsertOneAsync(stockItem1);

            var stockItem2 = this.DatabaseStockItem();
            stockItem2.UserId = stockItem1.UserId;
            stockItem2.StockItemId = stockItem1.StockItemId;

            var exception = await Assert.ThrowsAsync<MongoWriteException>(() => collection.InsertOneAsync(stockItem2));
            Assert.Equal(
                11000,
                exception.WriteError.Code);
        }

        [Theory]
        [InlineData("7c2c71da-72f0-4501-b949-3a97e4ed980")]
        [InlineData("7c2c71da-72f0-4501-b949-3a97e4ed98041")]
        public async Task CreateAsyncUserIdInvalid(string userId)
        {
            var stockItem = this.DatabaseStockItem();
            stockItem.UserId = userId;

            var collection = TestMongoClient.StockItemCollection();

            var exception = await Assert.ThrowsAsync<MongoWriteException>(() => collection.InsertOneAsync(stockItem));
            Assert.Equal(
                121,
                exception.WriteError.Code);
        }

        private DatabaseStockItem DatabaseStockItem()
        {
            return new DatabaseStockItem
            {
                MinimumQuantity = 10,
                Name = Guid.NewGuid().ToString(),
                Quantity = 20,
                StockItemId = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString()
            };
        }
    }
}
