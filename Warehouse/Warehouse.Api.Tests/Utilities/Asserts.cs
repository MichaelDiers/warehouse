namespace Warehouse.Api.Tests.Utilities
{
    using Warehouse.Api.Contracts.ShoppingItems;
    using Warehouse.Api.Contracts.StockItems;

    internal static class Asserts
    {
        public static void Assert(ICreateShoppingItem a, string userId, IShoppingItem b)
        {
            Xunit.Assert.True(
                Guid.TryParse(
                    b.Id,
                    out var guid) &&
                guid != Guid.Empty);
            Xunit.Assert.Equal(
                a.Quantity,
                b.Quantity);
            Xunit.Assert.Equal(
                a.Name,
                b.Name);
            Xunit.Assert.Equal(
                a.StockItemId,
                b.StockItemId);
            Xunit.Assert.Equal(
                userId,
                b.UserId);
        }

        public static bool Assert(ICreateStockItem a, string userId, IStockItem b)
        {
            Xunit.Assert.True(
                Guid.TryParse(
                    b.Id,
                    out var guid) &&
                guid != Guid.Empty);
            Xunit.Assert.Equal(
                a.Quantity,
                b.Quantity);
            Xunit.Assert.Equal(
                a.MinimumQuantity,
                b.MinimumQuantity);
            Xunit.Assert.Equal(
                a.Name,
                b.Name);
            Xunit.Assert.Equal(
                userId,
                b.UserId);

            return true;
        }

        public static bool Assert(IShoppingItem a, IShoppingItem b)
        {
            Xunit.Assert.Equal(
                a.Id,
                b.Id);
            Xunit.Assert.Equal(
                a.Quantity,
                b.Quantity);
            Xunit.Assert.Equal(
                a.Name,
                b.Name);
            Xunit.Assert.Equal(
                a.StockItemId,
                b.StockItemId);
            Xunit.Assert.Equal(
                a.UserId,
                b.UserId);

            return true;
        }

        public static bool Assert(IStockItem a, IStockItem b)
        {
            Xunit.Assert.Equal(
                a.Id,
                b.Id);
            Xunit.Assert.Equal(
                a.Quantity,
                b.Quantity);
            Xunit.Assert.Equal(
                a.MinimumQuantity,
                b.MinimumQuantity);
            Xunit.Assert.Equal(
                a.Name,
                b.Name);
            Xunit.Assert.Equal(
                a.UserId,
                b.UserId);

            return true;
        }

        public static bool Assert(IUpdateStockItem a, IUpdateStockItem b)
        {
            Xunit.Assert.Equal(
                a.Id,
                b.Id);
            Xunit.Assert.Equal(
                a.Quantity,
                b.Quantity);
            Xunit.Assert.Equal(
                a.MinimumQuantity,
                b.MinimumQuantity);
            Xunit.Assert.Equal(
                a.Name,
                b.Name);

            return true;
        }

        public static bool Assert(ICreateStockItem a, ICreateStockItem b)
        {
            Xunit.Assert.Equal(
                a.Quantity,
                b.Quantity);
            Xunit.Assert.Equal(
                a.MinimumQuantity,
                b.MinimumQuantity);
            Xunit.Assert.Equal(
                a.Name,
                b.Name);

            return true;
        }
    }
}
