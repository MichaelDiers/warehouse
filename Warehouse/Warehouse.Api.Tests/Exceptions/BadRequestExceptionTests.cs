namespace Warehouse.Api.Tests.Exceptions
{
    using Warehouse.Api.Exceptions;

    public class BadRequestExceptionTests
    {
        [Fact]
        public void DefaultCtor()
        {
            var exception = new BadRequestException();

            Assert.IsAssignableFrom<Exception>(exception);
        }

        [Fact]
        public void InnerExceptionCtor()
        {
            const string message = "message";
            var innerException = new ArgumentOutOfRangeException();

            var exception = new BadRequestException(
                message,
                innerException);

            Assert.IsAssignableFrom<Exception>(exception);
            Assert.Equal(
                message,
                exception.Message);
            Assert.IsAssignableFrom<ArgumentOutOfRangeException>(exception.InnerException);
        }

        [Fact]
        public void MessageCtor()
        {
            const string message = "message";

            var exception = new BadRequestException(message);

            Assert.IsAssignableFrom<Exception>(exception);
            Assert.Equal(
                message,
                exception.Message);
        }
    }
}
