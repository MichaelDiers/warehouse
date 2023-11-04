namespace Warehouse.Api.Contracts.StockItems
{
    /// <summary>
    ///     Specifies the type of an update operation.
    /// </summary>
    public enum UpdateOperation
    {
        /// <summary>
        ///     The undefined value.
        /// </summary>
        None = 0,

        /// <summary>
        ///     The increase operation.
        /// </summary>
        Increase,

        /// <summary>
        ///     The decrease operation.
        /// </summary>
        Decrease
    }
}
