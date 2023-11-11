namespace Warehouse.Api.Tests.Exceptions
{
    using Warehouse.Api.Exceptions;

    public class NotFoundExceptionTests
    {
        [Fact]
        public void DefaultCtor()
        {
            var exception = new NotFoundException();

            Assert.IsAssignableFrom<Exception>(exception);
        }

        [Fact]
        public void InnerExceptionCtor()
        {
            const string message = "message";
            var innerException = new ArgumentOutOfRangeException();

            var exception = new NotFoundException(
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

            var exception = new NotFoundException(message);

            Assert.IsAssignableFrom<Exception>(exception);
            Assert.Equal(
                message,
                exception.Message);
        }
    }
}
