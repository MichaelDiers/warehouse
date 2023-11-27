namespace Warehouse.Api.StockItems
{
    using Generic.Base.Api.Models;
    using Generic.Base.Api.Transformer;

    /// <summary>
    ///     Transformer for stock items.
    /// </summary>
    public class StockItemTransformer
        : IControllerTransformer<StockItem, ResultStockItem>,
            IUserBoundAtomicTransformer<CreateStockItem, StockItem, UpdateStockItem>,
            IProviderEntryTransformer<StockItem, StockItem>
    {
        /// <summary>
        ///     Transforms the specified entry of <see cref="StockItem" /> to <see cref="ResultStockItem" />.
        /// </summary>
        /// <param name="entry">The data for creating an instance of <see cref="ResultStockItem" />.</param>
        /// <param name="links">The urn that describe the supported operations on the result entry.</param>
        /// <returns>The transformed entry of type <see cref="ResultStockItem" />.</returns>
        public ResultStockItem Transform(StockItem entry, IEnumerable<Link> links)
        {
            return new ResultStockItem(
                entry.Name,
                entry.Quantity,
                entry.MinimumQuantity,
                links);
        }

        /// <summary>
        ///     Transforms the specified entry of type <see cref="StockItem" /> to <see cref="StockItem" />.
        /// </summary>
        /// <param name="entry">The data for creating an instance of type <see cref="StockItem" />.</param>
        /// <returns>The transformed entry of type <see cref="StockItem" />.</returns>
        public StockItem Transform(StockItem entry)
        {
            return entry;
        }

        /// <summary>
        ///     Transforms the specified <paramref name="createEntry" /> of type <see cref="CreateStockItem" /> to
        ///     <see cref="StockItem" />.
        /// </summary>
        /// <param name="createEntry">The data for creating an entry of type <see cref="StockItem" />.</param>
        /// <param name="userId">The id of the owner.</param>
        /// <returns>The transformed entry of type <see cref="StockItem" />.</returns>
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
        ///     Transforms the specified <paramref name="updateEntry" /> of type <see cref="UpdateStockItem" /> to
        ///     <see cref="StockItem" />.
        /// </summary>
        /// <param name="updateEntry">The data for creating an entry of type <see cref="StockItem" />.</param>
        /// <param name="userId">The id of the owner.</param>
        /// <param name="entryId">The id of the entry to be updated.</param>
        /// <returns>The transformed entry of type <see cref="StockItem" />.</returns>
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
