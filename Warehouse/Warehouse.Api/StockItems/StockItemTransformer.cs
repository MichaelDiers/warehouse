namespace Warehouse.Api.StockItems
{
    using Generic.Base.Api.Models;
    using Generic.Base.Api.Transformer;

    public class StockItemTransformer
        : IControllerTransformer<StockItem, ResultStockItem>,
            IUserBoundAtomicTransformer<CreateStockItem, StockItem, UpdateStockItem>,
            IProviderEntryTransformer<StockItem, StockItem>
    {
        /// <summary>
        ///     Transforms the specified entry of <typeparamref name="TEntry" /> to <typeparamref name="TResultEntry" />.
        /// </summary>
        /// <param name="entry">The data for creating an instance of <typeparamref name="TResultEntry" />.</param>
        /// <param name="links">The urn that describe the supported operations on the result entry.</param>
        /// <returns>The transformed entry of type <typeparamref name="TResultEntry" />.</returns>
        public ResultStockItem Transform(StockItem entry, IEnumerable<Link> links)
        {
            return new ResultStockItem(
                entry.Name,
                entry.Quantity,
                entry.MinimumQuantity,
                links);
        }

        /// <summary>
        ///     Transforms the specified entry of type <typeparamref name="TEntry" /> to <typeparamref name="TDatabaseEntry" />.
        /// </summary>
        /// <param name="entry">The data for creating an instance of type <typeparamref name="TDatabaseEntry" />.</param>
        /// <returns>The transformed entry of type <typeparamref name="TDatabaseEntry" />.</returns>
        public StockItem Transform(StockItem entry)
        {
            return entry;
        }

        /// <summary>
        ///     Transforms the specified <paramref name="createEntry" /> of type <typeparamref name="TCreateEntry" /> to
        ///     <typeparamref name="TEntry" />.
        /// </summary>
        /// <param name="createEntry">The data for creating an entry of type <typeparamref name="TEntry" />.</param>
        /// <param name="userId">The id of the owner.</param>
        /// <returns>The transformed entry of type <typeparamref name="TEntry" />.</returns>
        public StockItem Transform(CreateStockItem createEntry, string userId)
        {
            return new StockItem(
                Guid.NewGuid().ToString(),
                createEntry.Name,
                createEntry.Quantity,
                createEntry.MinimumQuantity,
                userId);
        }

        /// <summary>
        ///     Transforms the specified <paramref name="updateEntry" /> of type <typeparamref name="TUpdateEntry" /> to
        ///     <typeparamref name="TEntry" />.
        /// </summary>
        /// <param name="updateEntry">The data for creating an entry of type <typeparamref name="TEntry" />.</param>
        /// <param name="userId">The id of the owner.</param>
        /// <param name="entryId">The id of the entry to be updated.</param>
        /// <returns>The transformed entry of type <typeparamref name="TEntry" />.</returns>
        public StockItem Transform(UpdateStockItem updateEntry, string userId, string entryId)
        {
            return new StockItem(
                entryId,
                updateEntry.Name,
                updateEntry.Quantity,
                updateEntry.MinimumQuantity,
                userId);
        }
    }
}
