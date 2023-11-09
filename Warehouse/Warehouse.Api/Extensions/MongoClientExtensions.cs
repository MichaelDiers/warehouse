namespace Warehouse.Api.Extensions
{
    using MongoDB.Bson;
    using MongoDB.Driver;
    using Warehouse.Api.Contracts.Config;
    using Warehouse.Api.Models.ShoppingItems;
    using Warehouse.Api.Models.StockItems;

    /// <summary>
    ///     Extensions for <see cref="IMongoClient" />.
    /// </summary>
    public static class MongoClientExtensions
    {
        /// <summary>
        ///     Initializes the warehouse database.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="configuration">The database configuration.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result is the given <paramref name="client" />.</returns>
        public static async Task<IMongoClient> InitializeWarehouseAsync(
            this IMongoClient client,
            IDatabaseConfiguration configuration,
            CancellationToken cancellationToken
        )
        {
            var database = client.GetDatabase(configuration.DatabaseName);

            await MongoClientExtensions.InitializeStockItemCollection(
                database,
                cancellationToken);
            await MongoClientExtensions.InitializeShoppingItemCollection(
                database,
                cancellationToken);

            return client;
        }

        private static async Task InitializeCollection<T>(
            IMongoDatabase database,
            string collectionName,
            string schema,
            IEnumerable<CreateIndexModel<T>> createIndexModels,
            IEnumerable<T> entries,
            CancellationToken cancellationToken
        )
        {
            var collectionNames = await database.ListCollectionNamesAsync(cancellationToken: cancellationToken);
            if (collectionNames.ToEnumerable(cancellationToken)
                .Any(
                    name => string.Equals(
                        name,
                        collectionName,
                        StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }

            await database.CreateCollectionAsync(
                collectionName,
                cancellationToken: cancellationToken);

            await database.RunCommandAsync(
                new JsonCommand<BsonDocument>(schema),
                cancellationToken: cancellationToken);

            var collection = database.GetCollection<T>(collectionName);

            var models = createIndexModels.ToArray();
            if (models.Any())
            {
                await collection.Indexes.CreateManyAsync(
                    models,
                    cancellationToken);
            }

            var newEntries = entries.ToArray();
            if (newEntries.Any())
            {
                await collection.InsertManyAsync(
                    newEntries,
                    cancellationToken: cancellationToken);
            }
        }

        private static async Task InitializeShoppingItemCollection(
            IMongoDatabase database,
            CancellationToken cancellationToken
        )
        {
            var schema = $@"
            {{
                ""collMod"": ""{DatabaseShoppingItem.CollectionName}"",
                ""validator"": {{
                    $jsonSchema: {{
                        ""bsonType"": ""object"",            
                        ""required"": [
                            ""{nameof(DatabaseShoppingItem.Name)}"",
                            ""{nameof(DatabaseShoppingItem.Quantity)}"",
                            ""{nameof(DatabaseShoppingItem.ShoppingItemId)}"",
                            ""{nameof(DatabaseShoppingItem.StockItemId)}"",
                            ""{nameof(DatabaseShoppingItem.UserId)}""
                        ],
                        ""properties"": {{
                            ""{nameof(DatabaseShoppingItem.Name)}"": {{
                                ""bsonType"": ""string"",
                                ""minLength"": {CreateStockItem.NameMinLength},
                                ""maxLength"": {CreateStockItem.NameMaxLength},
                                ""description"": ""Name must be a string and is required""
                            }},                           
                            ""{nameof(DatabaseShoppingItem.Quantity)}"": {{
                                ""bsonType"": ""int"",
                                ""minimum"": {CreateStockItem.MinQuantity},
                                ""maximum"": {CreateStockItem.MaxQuantity},
                                ""description"": ""Quantity must be an int and is required""
                            }},
                            ""{nameof(DatabaseShoppingItem.ShoppingItemId)}"": {{
                                ""bsonType"": ""string"",
                                ""minLength"": 36,
                                ""maxLength"": 36,
                                ""description"": ""ShoppingItemId must be a string and is required""
                            }},
                            ""{nameof(DatabaseShoppingItem.StockItemId)}"": {{
                                ""bsonType"": ""string""
                                ""minLength"": 36,
                                ""maxLength"": 36,
                                ""description"": ""StockItemId must be a string and is required""
                            }},
                            ""{nameof(DatabaseShoppingItem.UserId)}"": {{
                                ""bsonType"": ""string""
                                ""minLength"": 36,
                                ""maxLength"": 36,
                                ""description"": ""UserId must be a string and is required""
                            }},
                        }},
                    }}
                }}
            }}";

            var indices = new[]
            {
                new CreateIndexModel<DatabaseShoppingItem>(
                    new JsonIndexKeysDefinition<DatabaseShoppingItem>(
                        $"{{\"{nameof(DatabaseShoppingItem.UserId)}\": 1, \"{nameof(DatabaseShoppingItem.Name)}\": 1}}"),
                    new CreateIndexOptions
                    {
                        Name =
                            $"{nameof(DatabaseShoppingItem.UserId)}_{nameof(DatabaseShoppingItem.Name)}_unique_index",
                        Unique = true,
                        Collation = new Collation(
                            "de",
                            strength: new Optional<CollationStrength?>(CollationStrength.Primary))
                    }),
                new CreateIndexModel<DatabaseShoppingItem>(
                    new JsonIndexKeysDefinition<DatabaseShoppingItem>(
                        $"{{\"{nameof(DatabaseShoppingItem.UserId)}\": 1, \"{nameof(DatabaseShoppingItem.StockItemId)}\": 1}}"),
                    new CreateIndexOptions
                    {
                        Name =
                            $"{nameof(DatabaseShoppingItem.UserId)}_{nameof(DatabaseShoppingItem.StockItemId)}_unique_index",
                        Unique = true,
                        Collation = new Collation(
                            "simple",
                            strength: new Optional<CollationStrength?>(CollationStrength.Primary))
                    }),
                new CreateIndexModel<DatabaseShoppingItem>(
                    new JsonIndexKeysDefinition<DatabaseShoppingItem>(
                        $"{{\"{nameof(DatabaseShoppingItem.UserId)}\": 1, \"{nameof(DatabaseShoppingItem.ShoppingItemId)}\": 1}}"),
                    new CreateIndexOptions
                    {
                        Name =
                            $"{nameof(DatabaseShoppingItem.UserId)}_{nameof(DatabaseShoppingItem.ShoppingItemId)}_unique_index",
                        Unique = true,
                        Collation = new Collation(
                            "simple",
                            strength: new Optional<CollationStrength?>(CollationStrength.Primary))
                    })
            };

            await MongoClientExtensions.InitializeCollection(
                database,
                DatabaseShoppingItem.CollectionName,
                schema,
                indices,
                Enumerable.Empty<DatabaseShoppingItem>(),
                cancellationToken);
        }

        private static async Task InitializeStockItemCollection(
            IMongoDatabase database,
            CancellationToken cancellationToken
        )
        {
            var schema = $@"
            {{
                ""collMod"": ""{DatabaseStockItem.CollectionName}"",
                ""validator"": {{
                    $jsonSchema: {{
                        ""bsonType"": ""object"",            
                        ""required"": [
                            ""{nameof(DatabaseStockItem.Name)}"",
                            ""{nameof(DatabaseStockItem.Quantity)}"",
                            ""{nameof(DatabaseStockItem.MinimumQuantity)}"",
                            ""{nameof(DatabaseStockItem.StockItemId)}"",
                            ""{nameof(DatabaseStockItem.UserId)}""
                        ],
                        ""properties"": {{
                            ""{nameof(DatabaseStockItem.MinimumQuantity)}"": {{
                                ""bsonType"": ""int"",
                                ""minimum"": {CreateStockItem.MinQuantity},
                                ""maximum"": {CreateStockItem.MaxQuantity},
                                ""description"": ""MinimumQuantity must be an int and is required""
                            }},                            
                            ""{nameof(DatabaseStockItem.Name)}"": {{
                                ""bsonType"": ""string"",
                                ""minLength"": {CreateStockItem.NameMinLength},
                                ""maxLength"": {CreateStockItem.NameMaxLength},
                                ""description"": ""Name must be a string and is required""
                            }},
                            ""{nameof(DatabaseStockItem.Quantity)}"": {{
                                ""bsonType"": ""int"",
                                ""minimum"": {CreateStockItem.MinQuantity},
                                ""maximum"": {CreateStockItem.MaxQuantity},
                                ""description"": ""Quantity must be an int and is required""
                            }},
                            ""{nameof(DatabaseStockItem.StockItemId)}"": {{
                                ""bsonType"": ""string""
                                ""minLength"": 36,
                                ""maxLength"": 36,
                                ""description"": ""StockItemId must be a string and is required""
                            }},
                            ""{nameof(DatabaseStockItem.UserId)}"": {{
                                ""bsonType"": ""string""
                                ""minLength"": 36,
                                ""maxLength"": 36,
                                ""description"": ""UserId must be a string and is required""
                            }},
                        }},
                    }}
                }}
            }}";

            var indices = new[]
            {
                new CreateIndexModel<DatabaseStockItem>(
                    new JsonIndexKeysDefinition<DatabaseStockItem>(
                        $"{{\"{nameof(DatabaseStockItem.UserId)}\": 1, \"{nameof(DatabaseStockItem.Name)}\": 1}}"),
                    new CreateIndexOptions
                    {
                        Name =
                            $"{nameof(DatabaseStockItem.UserId)}_{nameof(DatabaseStockItem.Name)}_unique_index",
                        Unique = true,
                        Collation = new Collation(
                            "de",
                            strength: new Optional<CollationStrength?>(CollationStrength.Primary))
                    }),
                new CreateIndexModel<DatabaseStockItem>(
                    new JsonIndexKeysDefinition<DatabaseStockItem>(
                        $"{{\"{nameof(DatabaseStockItem.UserId)}\": 1, \"{nameof(DatabaseStockItem.StockItemId)}\": 1}}"),
                    new CreateIndexOptions
                    {
                        Name =
                            $"{nameof(DatabaseStockItem.UserId)}_{nameof(DatabaseStockItem.StockItemId)}_unique_index",
                        Unique = true,
                        Collation = new Collation(
                            "simple",
                            strength: new Optional<CollationStrength?>(CollationStrength.Primary))
                    })
            };

            await MongoClientExtensions.InitializeCollection(
                database,
                DatabaseStockItem.CollectionName,
                schema,
                indices,
                Enumerable.Empty<DatabaseStockItem>(),
                cancellationToken);
        }
    }
}
