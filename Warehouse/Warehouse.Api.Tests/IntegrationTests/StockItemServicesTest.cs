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
        private readonly Role[] defaultRoles =
        {
            Role.User,
            Role.Accessor
        };

        public StockItemServicesTest()
            : base(TestFactory.ApiKey)
        {
        }

        /// <summary>
        ///     Gets test data for that the data validation fails in the create context.
        /// </summary>
        protected override IEnumerable<(CreateStockItem createData, string testInfo)>
            CreateDataValidationFailsTestData =>
            new[]
            {
                StockItemServicesTest.BadRequestCreateData(
                    $"MinimumQuantity lower than {CreateStockItem.MinQuantity}.",
                    CreateStockItem.MinQuantity - 1),
                StockItemServicesTest.BadRequestCreateData(
                    $"MinimumQuantity larger than {CreateStockItem.MaxQuantity}.",
                    CreateStockItem.MaxQuantity + 1),
                StockItemServicesTest.BadRequestCreateData(
                    $"Length of name too small {CreateStockItem.NameMinLength}.",
                    name: new string(
                        'a',
                        CreateStockItem.NameMinLength - 1)),
                StockItemServicesTest.BadRequestCreateData(
                    $"Length of name too large {CreateStockItem.NameMaxLength}.",
                    name: new string(
                        'a',
                        CreateStockItem.NameMaxLength + 1)),
                StockItemServicesTest.BadRequestCreateData(
                    $"Quantity lower than {CreateStockItem.MinQuantity}.",
                    quantity: CreateStockItem.MinQuantity - 1),
                StockItemServicesTest.BadRequestCreateData(
                    $"Quantity larger than {CreateStockItem.MaxQuantity}.",
                    quantity: CreateStockItem.MaxQuantity + 1)
            };

        /// <summary>
        ///     Gets the entry point URL that is supposed to be an options operation.
        /// </summary>
        protected override string EntryPointUrl => "api/Options";

        /// <summary>
        ///     Gets the roles that are required for options requests.
        /// </summary>
        protected override IEnumerable<Role> OptionsRoles => this.defaultRoles;

        protected override IEnumerable<Role> RequiredCreateRoles => this.defaultRoles;

        protected override IEnumerable<Role> RequiredDeleteRoles => this.defaultRoles;

        protected override IEnumerable<Role> RequiredReadAllRoles => this.defaultRoles;

        protected override IEnumerable<Role> RequiredReadByIdRoles => this.defaultRoles;

        protected override IEnumerable<Role> RequiredUpdateRoles => this.defaultRoles;

        protected override string UrnNamespace => nameof(StockItemController)[..^10];

        /// <summary>
        ///     Asserts that the created entry matches the read result.
        /// </summary>
        /// <param name="createResult">The expected created result.</param>
        /// <param name="readResult">The actual read result that should match the created result.</param>
        protected override void AssertEntry(ResultStockItem createResult, ResultStockItem readResult)
        {
            Assert.Equal(
                createResult.MinimumQuantity,
                readResult.MinimumQuantity);
            Assert.Equal(
                createResult.Quantity,
                readResult.Quantity);
            Assert.Equal(
                createResult.Name,
                readResult.Name);
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

        private static (CreateStockItem createData, string testInfo) BadRequestCreateData(
            string testInfo,
            int minimumQuantity = 0,
            string name = "name",
            int quantity = 0
        )
        {
            return (new CreateStockItem(
                minimumQuantity,
                name,
                quantity), testInfo);
        }
    }
}
