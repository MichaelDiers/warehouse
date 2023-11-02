namespace Warehouse.Api.Tests.Validation
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Routing;
    using Moq;
    using Warehouse.Api.Validation;

    /// <summary>
    ///     Tests for <see cref="GuidValidationAttribute" />.
    /// </summary>
    public class GuidValidationAttributeTests
    {
        [Theory]
        [InlineData(
            "guid",
            "fbde8711-12fa-4895-8ecc-307ecb41b6a4",
            true)]
        [InlineData(
            "other",
            "fbde8711-12fa-4895-8ecc-307ecb41b6a4",
            true)]
        [InlineData(
            "guid",
            null,
            false)]
        [InlineData(
            "guid",
            "",
            false)]
        [InlineData(
            "guid",
            "fbde8711-12fa-4895-8ecc6307ecb41b6a4",
            false)]
        [InlineData(
            "guid",
            "fbde8711-12fa-4895-8ecc6307ecb41b6a433",
            false)]
        public void OnActionExecuting(string parameterName, string? parameterValue, bool success)
        {
            const string name = "guid";
            var validationFilter = new GuidValidationAttribute(name);
            var modelState = new ModelStateDictionary();

            var actionContext = new ActionContext(
                Mock.Of<HttpContext>(),
                Mock.Of<RouteData>(),
                Mock.Of<ActionDescriptor>(),
                modelState);

            var actionExecutingContext = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object> {{parameterName, parameterValue}},
                Mock.Of<Controller>());

            validationFilter.OnActionExecuting(actionExecutingContext);

            if (success)
            {
                Assert.Null(actionExecutingContext.Result);
            }
            else
            {
                Assert.IsType<BadRequestResult>(actionExecutingContext.Result);
            }
        }
    }
}
