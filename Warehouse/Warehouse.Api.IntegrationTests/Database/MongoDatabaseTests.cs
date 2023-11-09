namespace Warehouse.Api.Tests.Database
{
    using MongoDB.Bson;
    using MongoDB.Driver;
    using Warehouse.Api.Models.StockItems;

    public class MongoDatabaseTests
    {
        private const string ConnectionString = "mongodb://localhost:27017/?replicaSet=warehouse_replSet";

        private const string DatabaseName = "warehouse";

        public async Task Indices(bool dropCollection)
        {
            var source = new CancellationTokenSource();
            source.CancelAfter(15000);

            var mongoClient = new MongoClient(MongoDatabaseTests.ConnectionString);

            var database = mongoClient.GetDatabase(MongoDatabaseTests.DatabaseName);

            var collectionNames = await database.ListCollectionNamesAsync(cancellationToken: source.Token);

            var exists = (await collectionNames.ToListAsync(source.Token)).Any(
                name => name == DatabaseStockItem.CollectionName);

            if (exists && dropCollection)
            {
                await database.DropCollectionAsync(
                    DatabaseStockItem.CollectionName,
                    source.Token);
            }

            if (dropCollection || !exists)
            {
                await database.CreateCollectionAsync(
                    DatabaseStockItem.CollectionName,
                    cancellationToken: source.Token);
            }

            var schema = @"
            {
                ""collMod"": ""stockitems"",
                ""validator"": {
                    $jsonSchema: {
                        ""bsonType"": ""object"",            
                        ""required"": [""Name"", ""Quantity"", ""MinimumQuantity"", ""StockItemId"", ""UserId""],
                        ""properties"": {
                            ""MinimumQuantity"": {
                                ""bsonType"": ""number"",
                                ""minimum"": 0,
                                ""maximum"": 9999,
                                ""description"": ""MinimumQuantity must be a number and is required""
                            },                            
                            ""Name"": {
                                ""bsonType"": ""string"",
                                ""minLength"": 1,
                                ""maxLength"": 100,
                                ""description"": ""Name must be a string and is required""
                            },
                            ""Quantity"": {
                                ""bsonType"": ""int"",
                                ""minimum"": 0,
                                ""maximum"": 9999,
                                ""description"": ""Quantity must be a number and is required""
                            },
                            ""StockItemId"": {
                                ""bsonType"": ""string""
                                ""minLength"": 36,
                                ""maxLength"": 36,
                            },
                            ""UserId"": {
                                ""bsonType"": ""string""
                                ""minLength"": 36,
                                ""maxLength"": 36,
                            },
                        },
                    }
                }
            }";

            if (!exists || dropCollection)
            {
                await database.RunCommandAsync(
                    new JsonCommand<BsonDocument>(schema),
                    cancellationToken: source.Token);
            }

            var stockItemCollection = database.GetCollection<DatabaseStockItem>(DatabaseStockItem.CollectionName);

            if (!exists || dropCollection)
            {
                var nameUniqueIndex = new CreateIndexModel<DatabaseStockItem>(
                    new JsonIndexKeysDefinition<DatabaseStockItem>("{\"Name\": 1}"),
                    new CreateIndexOptions
                    {
                        Name = "Name_unique_index",
                        Unique = true,
                        Collation = new Collation(
                            "de",
                            strength: new Optional<CollationStrength?>(CollationStrength.Primary))
                    });
                var stockItemIdUniqueIndex = new CreateIndexModel<DatabaseStockItem>(
                    new JsonIndexKeysDefinition<DatabaseStockItem>("{\"StockItemId\": 1}"),
                    new CreateIndexOptions
                    {
                        Name = "StockItemId_unique_index",
                        Unique = true,
                        Collation = new Collation(
                            "simple",
                            strength: new Optional<CollationStrength?>(CollationStrength.Primary))
                    });
                var userIdStockItemIdIndex = new CreateIndexModel<DatabaseStockItem>(
                    new JsonIndexKeysDefinition<DatabaseStockItem>("{\"StockItemId\": 1, \"UserId\": 1}"),
                    new CreateIndexOptions
                    {
                        Name = "StockItemId_UserId_unique_index",
                        Unique = true,
                        Collation = new Collation(
                            "simple",
                            strength: new Optional<CollationStrength?>(CollationStrength.Primary))
                    });

                await stockItemCollection.Indexes.CreateManyAsync(
                    new[]
                    {
                        nameUniqueIndex,
                        stockItemIdUniqueIndex,
                        userIdStockItemIdIndex
                    },
                    source.Token);
            }

            await stockItemCollection.InsertOneAsync(
                new DatabaseStockItem(
                    new StockItem(
                        Guid.NewGuid().ToString(),
                        Guid.NewGuid().ToString(),
                        0,
                        10,
                        Guid.NewGuid().ToString())),
                cancellationToken: source.Token);
            var name = Guid.NewGuid().ToString().ToLower();
            await stockItemCollection.InsertOneAsync(
                new DatabaseStockItem(
                    new StockItem(
                        Guid.NewGuid().ToString(),
                        name,
                        0,
                        10,
                        Guid.NewGuid().ToString())),
                cancellationToken: source.Token);
            var exception = await Assert.ThrowsAsync<MongoWriteException>(
                () => stockItemCollection.InsertOneAsync(
                    new DatabaseStockItem(
                        new StockItem(
                            Guid.NewGuid().ToString(),
                            name.ToUpper(),
                            0,
                            10,
                            Guid.NewGuid().ToString())),
                    cancellationToken: source.Token));
            Assert.Equal(
                11000,
                exception.WriteError.Code);
        }
    }
}
