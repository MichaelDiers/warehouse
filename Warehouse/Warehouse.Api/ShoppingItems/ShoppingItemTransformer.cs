namespace Warehouse.Api.ShoppingItems
{
    using Generic.Base.Api.Models;
    using Generic.Base.Api.Transformer;

    /// <summary>
    ///     Transformer for shopping items.
    /// </summary>
    public class ShoppingItemTransformer
        : IControllerTransformer<ShoppingItem, ResultShoppingItem>,
            IUserBoundAtomicTransformer<CreateShoppingItem, ShoppingItem, UpdateShoppingItem>,
            IProviderEntryTransformer<ShoppingItem, ShoppingItem>
    {
        /// <summary>
        ///     Transforms the specified entry of <see cref="ShoppingItem" /> to <see cref="ResultShoppingItem" />.
        /// </summary>
        /// <param name="entry">The data for creating an instance of <see cref="ShoppingItem" />.</param>
        /// <param name="links">The urn that describe the supported operations on the result entry.</param>
        /// <returns>The transformed entry of type <see cref="ResultShoppingItem" />.</returns>
        public ResultShoppingItem Transform(ShoppingItem entry, IEnumerable<Link> links)
        {
            return new ResultShoppingItem(
                entry.Id,
                entry.Name,
                entry.Quantity,
                entry.UserId,
                links);
        }

        /// <summary>
        ///     Transforms the specified entry of type <see cref="ShoppingItem" /> to <see cref="ShoppingItem" />.
        /// </summary>
        /// <param name="entry">The data for creating an instance of type <see cref="ShoppingItem" />.</param>
        /// <returns>The transformed entry of type <see cref="ShoppingItem" />.</returns>
        public ShoppingItem Transform(ShoppingItem entry)
        {
            return entry;
        }

        /// <summary>
        ///     Transforms the specified <paramref name="createEntry" /> of type <see cref="CreateShoppingItem" /> to
        ///     <see cref="ShoppingItem" />.
        /// </summary>
        /// <param name="createEntry">The data for creating an entry of type <see cref="ShoppingItem" />.</param>
        /// <param name="userId">The id of the owner.</param>
        /// <returns>The transformed entry of type <see cref="ShoppingItem" />.</returns>
        public ShoppingItem Transform(CreateShoppingItem createEntry, string userId)
        {
            return new ShoppingItem(
                Guid.NewGuid().ToString(),
                createEntry.Name,
                createEntry.Quantity,
                userId);
        }

        /// <summary>
        ///     Transforms the specified <paramref name="updateEntry" /> of type <see cref="UpdateShoppingItem" /> to
        ///     <see cref="ShoppingItem" />.
        /// </summary>
        /// <param name="updateEntry">The data for creating an entry of type <see cref="ShoppingItem" />.</param>
        /// <param name="userId">The id of the owner.</param>
        /// <param name="entryId">The id of the entry to be updated.</param>
        /// <returns>The transformed entry of type <see cref="ShoppingItem" />.</returns>
        public ShoppingItem Transform(UpdateShoppingItem updateEntry, string userId, string entryId)
        {
            return new ShoppingItem(
                entryId,
                updateEntry.Name,
                updateEntry.Quantity,
                userId);
        }
    }
}
