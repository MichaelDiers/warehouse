namespace Warehouse.Api.Validation
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    /// <summary>
    ///     Validation of guid in route parameters.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute" />
    public class GuidValidationAttribute : ActionFilterAttribute
    {
        /// <summary>
        ///     The name of the route parameter.
        /// </summary>
        private readonly string name;

        /// <summary>
        ///     Initializes a new instance of the <see cref="GuidValidationAttribute" /> class.
        /// </summary>
        /// <param name="name">The name of the route parameter.</param>
        public GuidValidationAttribute(string name)
        {
            this.name = name;
        }

        /// <summary>
        ///     Validate the guid if present in route parameters.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionArguments.TryGetValue(
                    this.name,
                    out var guidValue) &&
                (!Guid.TryParse(
                     guidValue as string,
                     out var guid) ||
                 guid == Guid.Empty))
            {
                filterContext.Result = new BadRequestResult();
            }
        }
    }
}
