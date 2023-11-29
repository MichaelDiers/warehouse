namespace Warehouse.Api.Tests.IntegrationTests
{
    using Generic.Base.Api.AuthServices.UserService;
    using Generic.Base.Api.Test.Lib.CrudTest;
    using Warehouse.Api.StockItems;

    [Trait(
        "TestType",
        "MongoDbIntegrationTest")]
    public class StockItemServicesTest
        : UserBoundCrudTests<Program, TestFactory, CreateStockItem, ResultStockItem, ResultStockItem, UpdateStockItem,
            ResultStockItem>
    {
        public StockItemServicesTest()
            : base(
                nameof(StockItemController)[..^10],
                "api/Options",
                TestFactory.ApiKey,
                new[]
                {
                    Role.User,
                    Role.Accessor
                },
                new[]
                {
                    Role.Accessor,
                    Role.User
                },
                new[]
                {
                    Role.Accessor,
                    Role.User
                },
                new[]
                {
                    Role.Accessor,
                    Role.User
                },
                new[]
                {
                    Role.Accessor,
                    Role.User
                },
                new[]
                {
                    Role.Accessor,
                    Role.User
                })
        {
        }

        protected override CreateStockItem GetValidCreateEntry()
        {
            return new CreateStockItem(
                10,
                Guid.NewGuid().ToString(),
                20);
        }

        protected override UpdateStockItem GetValidUpdateEntry()
        {
            return new UpdateStockItem(
                Guid.NewGuid().ToString(),
                20,
                10);
        }
    }
}
