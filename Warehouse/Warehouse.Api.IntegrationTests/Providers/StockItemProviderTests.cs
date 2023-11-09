namespace Warehouse.Api.IntegrationTests.Providers
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Warehouse.Api.Contracts.Database;
    using Warehouse.Api.Contracts.StockItems;
    using Warehouse.Api.Exceptions;
    using Warehouse.Api.Extensions;
    using Warehouse.Api.IntegrationTests.Lib;
    using Warehouse.Api.Models.StockItems;

    public class StockItemProviderTests
    {
        [Theory]
        [InlineData(-1)]
        [InlineData(10000)]
        public async Task CreateAsyncMinimumQuantityInvalid(int minimumQuantity)
        {
            var (stockItem, provider, transactionHandler) = await StockItemProviderTests.Init();
            stockItem.MinimumQuantity = minimumQuantity;

            await Assert.ThrowsAsync<BadRequestException>(
                () => StockItemProviderTests.Run(
                    session => provider.CreateAsync(
                        stockItem,
                        new CancellationToken(),
                        session),
                    transactionHandler));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(101)]
        public async Task CreateAsyncNameInvalid(int nameLength)
        {
            var (stockItem, provider, transactionHandler) = await StockItemProviderTests.Init();
            stockItem.Name = new string(
                'a',
                nameLength);

            await Assert.ThrowsAsync<BadRequestException>(
                () => StockItemProviderTests.Run(
                    session => provider.CreateAsync(
                        stockItem,
                        new CancellationToken(),
                        session),
                    transactionHandler));
        }

        [Fact]
        public async Task CreateAsyncNameNotUnique()
        {
            var (stockItem, provider, transactionHandler) = await StockItemProviderTests.Init();
            await StockItemProviderTests.Run(
                session => provider.CreateAsync(
                    stockItem,
                    new CancellationToken(),
                    session),
                transactionHandler);

            var stockItem2 = StockItemProviderTests.StockItem();
            stockItem2.UserId = stockItem.UserId;
            stockItem2.Name = stockItem.Name;

            await Assert.ThrowsAsync<ConflictException>(
                () => StockItemProviderTests.Run(
                    session => provider.CreateAsync(
                        stockItem,
                        new CancellationToken(),
                        session),
                    transactionHandler));
        }

        [Fact]
        public async Task CreateAsyncOk()
        {
            var (stockItem, provider, transactionHandler) = await StockItemProviderTests.Init();

            await StockItemProviderTests.Run(
                session => provider.CreateAsync(
                    stockItem,
                    new CancellationToken(),
                    session),
                transactionHandler);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(10000)]
        public async Task CreateAsyncQuantityInvalid(int quantity)
        {
            var (stockItem, provider, transactionHandler) = await StockItemProviderTests.Init();
            stockItem.Quantity = quantity;

            await Assert.ThrowsAsync<BadRequestException>(
                () => StockItemProviderTests.Run(
                    session => provider.CreateAsync(
                        stockItem,
                        new CancellationToken(),
                        session),
                    transactionHandler));
        }

        [Theory]
        [InlineData("7c2c71da-72f0-4501-b949-3a97e4ed980")]
        [InlineData("7c2c71da-72f0-4501-b949-3a97e4ed98041")]
        public async Task CreateAsyncStockItemIdInvalid(string stockItemId)
        {
            var (stockItem, provider, transactionHandler) = await StockItemProviderTests.Init();
            stockItem.Id = stockItemId;

            await Assert.ThrowsAsync<BadRequestException>(
                () => StockItemProviderTests.Run(
                    session => provider.CreateAsync(
                        stockItem,
                        new CancellationToken(),
                        session),
                    transactionHandler));
        }

        [Fact]
        public async Task CreateAsyncStockItemIdNotUnique()
        {
            var (stockItem, provider, transactionHandler) = await StockItemProviderTests.Init();

            await StockItemProviderTests.Run(
                session => provider.CreateAsync(
                    stockItem,
                    new CancellationToken(),
                    session),
                transactionHandler);

            var stockItem2 = StockItemProviderTests.StockItem();
            stockItem2.UserId = stockItem.UserId;
            stockItem2.Id = stockItem.Id;

            await Assert.ThrowsAsync<ConflictException>(
                () => StockItemProviderTests.Run(
                    session => provider.CreateAsync(
                        stockItem,
                        new CancellationToken(),
                        session),
                    transactionHandler));
        }

        [Theory]
        [InlineData("7c2c71da-72f0-4501-b949-3a97e4ed980")]
        [InlineData("7c2c71da-72f0-4501-b949-3a97e4ed98041")]
        public async Task CreateAsyncUserIdInvalid(string userId)
        {
            var (stockItem, provider, transactionHandler) = await StockItemProviderTests.Init();
            stockItem.UserId = userId;

            await Assert.ThrowsAsync<BadRequestException>(
                () => StockItemProviderTests.Run(
                    session => provider.CreateAsync(
                        stockItem,
                        new CancellationToken(),
                        session),
                    transactionHandler));
        }

        [Fact]
        public async Task DeleteNotFound()
        {
            var (stockItem, provider, transactionHandler) = await StockItemProviderTests.Init();

            await Assert.ThrowsAsync<NotFoundException>(
                () => StockItemProviderTests.Run(
                    session => provider.DeleteAsync(
                        stockItem.UserId,
                        stockItem.Id,
                        new CancellationToken(),
                        session),
                    transactionHandler));
        }

        [Fact]
        public async Task DeleteOk()
        {
            var (stockItem, provider, transactionHandler) = await StockItemProviderTests.Init(true);

            await StockItemProviderTests.Run(
                session => provider.DeleteAsync(
                    stockItem.UserId,
                    stockItem.Id,
                    new CancellationToken(),
                    session),
                transactionHandler);
        }

        [Fact]
        public async Task ReadByIdNotFound()
        {
            var (stockItem, provider, transactionHandler) = await StockItemProviderTests.Init();

            await Assert.ThrowsAsync<NotFoundException>(
                () => StockItemProviderTests.Run(
                    session => provider.ReadByIdAsync(
                        stockItem.UserId,
                        stockItem.Id,
                        new CancellationToken(),
                        session),
                    transactionHandler));
        }

        [Fact]
        public async Task ReadByIdOk()
        {
            var (stockItem, provider, _) = await StockItemProviderTests.Init(true);

            var actual = await provider.ReadByIdAsync(
                stockItem.UserId,
                stockItem.Id,
                new CancellationToken());

            Assert.Equal(
                stockItem.Id,
                actual.Id);
        }

        [Fact]
        public async Task ReadEmpty()
        {
            var (stockItem, provider, _) = await StockItemProviderTests.Init();

            var result = await provider.ReadAsync(
                stockItem.UserId,
                new CancellationToken());

            Assert.Empty(result);
        }

        [Fact]
        public async Task ReadOk()
        {
            var (stockItem, provider, transactionHandler) = await StockItemProviderTests.Init();
            var items = new[]
            {
                stockItem,
                StockItemProviderTests.StockItem(),
                StockItemProviderTests.StockItem()
            };

            foreach (var item in items)
            {
                item.UserId = stockItem.UserId;
                await StockItemProviderTests.Run(
                    session => provider.CreateAsync(
                        item,
                        new CancellationToken(),
                        session),
                    transactionHandler);
            }

            var actual = (await provider.ReadAsync(
                stockItem.UserId,
                new CancellationToken())).ToArray();

            Assert.Equal(
                items.Length,
                actual.Length);
            foreach (var item in items)
            {
                Assert.Contains(
                    actual,
                    x => x.Id == item.Id);
            }
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(10000)]
        public async Task UpdateAsyncMinimumQuantityInvalid(int minimumQuantity)
        {
            var (stockItem, provider, transactionHandler) = await StockItemProviderTests.Init(true);
            stockItem.MinimumQuantity = minimumQuantity;

            await Assert.ThrowsAsync<BadRequestException>(
                () => StockItemProviderTests.Run(
                    session => provider.UpdateAsync(
                        stockItem,
                        new CancellationToken(),
                        session),
                    transactionHandler));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(101)]
        public async Task UpdateAsyncNameInvalid(int nameLength)
        {
            var (stockItem, provider, transactionHandler) = await StockItemProviderTests.Init(true);
            stockItem.Name = new string(
                'a',
                nameLength);

            await Assert.ThrowsAsync<BadRequestException>(
                () => StockItemProviderTests.Run(
                    session => provider.UpdateAsync(
                        stockItem,
                        new CancellationToken(),
                        session),
                    transactionHandler));
        }

        [Fact]
        public async Task UpdateAsyncNameNotUnique()
        {
            var (stockItem, provider, transactionHandler) = await StockItemProviderTests.Init(true);
            var update = StockItemProviderTests.StockItem();
            update.UserId = stockItem.UserId;

            await StockItemProviderTests.Run(
                session => provider.CreateAsync(
                    update,
                    new CancellationToken(),
                    session),
                transactionHandler);

            update.Name = stockItem.Name;

            await Assert.ThrowsAsync<ConflictException>(
                () => StockItemProviderTests.Run(
                    session => provider.CreateAsync(
                        stockItem,
                        new CancellationToken(),
                        session),
                    transactionHandler));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(10000)]
        public async Task UpdateAsyncQuantityInvalid(int quantity)
        {
            var (stockItem, provider, transactionHandler) = await StockItemProviderTests.Init(true);
            stockItem.Quantity = quantity;

            await Assert.ThrowsAsync<BadRequestException>(
                () => StockItemProviderTests.Run(
                    session => provider.UpdateAsync(
                        stockItem,
                        new CancellationToken(),
                        session),
                    transactionHandler));
        }

        [Fact]
        public async Task UpdateNotFound()
        {
            var (stockItem, provider, transactionHandler) = await StockItemProviderTests.Init();

            var update = new StockItem(stockItem) {Quantity = stockItem.Quantity + 1};

            await Assert.ThrowsAsync<NotFoundException>(
                () => StockItemProviderTests.Run(
                    session => provider.UpdateAsync(
                        update,
                        new CancellationToken(),
                        session),
                    transactionHandler));
        }

        [Fact]
        public async Task UpdateOk()
        {
            var (stockItem, provider, transactionHandler) = await StockItemProviderTests.Init(true);

            var update = new StockItem(stockItem) {Quantity = stockItem.Quantity + 1};

            await StockItemProviderTests.Run(
                session => provider.UpdateAsync(
                    update,
                    new CancellationToken(),
                    session),
                transactionHandler);

            var updated = await provider.ReadByIdAsync(
                stockItem.UserId,
                stockItem.Id,
                new CancellationToken());

            Assert.Equal(
                stockItem.Id,
                updated.Id);
            Assert.Equal(
                stockItem.Quantity + 1,
                updated.Quantity);
        }

        [Theory]
        [InlineData(-10000)]
        [InlineData(10000)]
        public async Task UpdateQuantityAsyncQuantityInvalid(int delta)
        {
            var (stockItem, provider, transactionHandler) = await StockItemProviderTests.Init(true);

            await Assert.ThrowsAsync<BadRequestException>(
                () => StockItemProviderTests.Run(
                    session => provider.UpdateQuantityAsync(
                        stockItem.UserId,
                        stockItem.Id,
                        delta,
                        new CancellationToken(),
                        session),
                    transactionHandler));
        }

        [Theory]
        [InlineData(10)]
        [InlineData(0)]
        [InlineData(-10)]
        public async Task UpdateQuantityOk(int delta)
        {
            var (stockItem, provider, transactionHandler) = await StockItemProviderTests.Init(true);

            await StockItemProviderTests.Run(
                session => provider.UpdateQuantityAsync(
                    stockItem.UserId,
                    stockItem.Id,
                    delta,
                    new CancellationToken(),
                    session),
                transactionHandler);

            var updated = await provider.ReadByIdAsync(
                stockItem.UserId,
                stockItem.Id,
                new CancellationToken());

            Assert.Equal(
                stockItem.Id,
                updated.Id);
            Assert.Equal(
                stockItem.Quantity + delta,
                updated.Quantity);
        }

        private static async
            Task<(StockItem stockItem, IStockItemProvider provider, ITransactionHandler transactionHandler)> Init(
                bool create = false
            )
        {
            var builder = Host.CreateApplicationBuilder();
            TestMongoClient.Connect(builder.Services);

            builder.Services.AddDependencies();
            var app = builder.Build();

            var provider = app.Services.GetService<IStockItemProvider>();
            Assert.NotNull(provider);

            var transactionHandler = app.Services.GetService<ITransactionHandler>();
            Assert.NotNull(transactionHandler);

            var stockItem = StockItemProviderTests.StockItem();

            if (create)
            {
                await StockItemProviderTests.Run(
                    session => provider.CreateAsync(
                        stockItem,
                        new CancellationToken(),
                        session),
                    transactionHandler);
            }

            return (stockItem, provider, transactionHandler);
        }

        private static async Task Run(Func<ITransactionHandle, Task> func, ITransactionHandler transactionHandler)
        {
            using var session = await transactionHandler.StartTransactionAsync(new CancellationToken());
            {
                await func(session);
                await session.CommitTransactionAsync(new CancellationToken());
            }
        }

        private static StockItem StockItem()
        {
            return new StockItem
            {
                MinimumQuantity = 10,
                Name = Guid.NewGuid().ToString(),
                Quantity = 20,
                Id = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString()
            };
        }
    }
}
