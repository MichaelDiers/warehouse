namespace Warehouse.Api.Contracts.Users
{
    /// <summary>
    ///     Describes change password request.
    /// </summary>
    public interface IChangePassword
    {
        /// <summary>
        ///     Gets the current password.
        /// </summary>
        string NewPassword { get; }

        /// <summary>
        ///     Gets the old password.
        /// </summary>
        string OldPassword { get; }
    }
}
