namespace Warehouse.Api
{
    using Microsoft.Extensions.Primitives;

    public class AccessControlAllowOriginMiddleware
    {
        /// <summary>
        ///     The request delegate.
        /// </summary>
        private readonly RequestDelegate next;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AccessControlAllowOriginMiddleware" /> class.
        /// </summary>
        /// <param name="next">The next delegate.</param>
        public AccessControlAllowOriginMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public Task Invoke(HttpContext context)
        {
            context.Response.Headers.AccessControlAllowOrigin =
                new StringValues(context.Request.Headers.Referer.ToString());
            context.Response.Headers.Vary = new StringValues("Origin");
            return this.next(context);
        }
    }
}
