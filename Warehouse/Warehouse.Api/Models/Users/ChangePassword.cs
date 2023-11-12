namespace Warehouse.Api.Models.Users
{
    using Warehouse.Api.Contracts.Users;

    /// <inheritdoc cref="IChangePassword" />
    public class ChangePassword : IChangePassword
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ChangePassword" /> class.
        /// </summary>
        /// <param name="oldPassword">The old password.</param>
        /// <param name="newPassword">The new password.</param>
        public ChangePassword(string oldPassword, string newPassword)
        {
            this.OldPassword = oldPassword;
            this.NewPassword = newPassword;
        }

        /// <summary>
        ///     Gets or sets the current password.
        /// </summary>
        public string NewPassword { get; set; }

        /// <summary>
        ///     Gets or sets the old password.
        /// </summary>
        public string OldPassword { get; set; }
    }
}
