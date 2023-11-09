namespace Warehouse.Api.Exceptions
{
    /// <summary>
    ///     Exception is thrown if the data validation fails.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class BadRequestException : Exception
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="BadRequestException" /> class.
        /// </summary>
        public BadRequestException()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BadRequestException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public BadRequestException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BadRequestException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The inner exception.</param>
        public BadRequestException(string message, Exception inner)
            : base(
                message,
                inner)
        {
        }
    }
}
