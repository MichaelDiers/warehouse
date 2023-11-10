namespace Warehouse.Api.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    ///     The auth controller.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        /// <summary>
        ///     Deletes the current user.
        /// </summary>
        /// <returns>An <see cref="OkResult" /> if the user is deleted and <see cref="NotFoundResult" /> otherwise.</returns>
        [HttpDelete]
        public IActionResult Delete()
        {
            return this.Ok();
        }

        [HttpGet("{userId}")]
        public IActionResult Read(string userId)
        {
            return this.Ok();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult ReadAll()
        {
            return this.Ok();
        }

        [AllowAnonymous]
        [HttpPost("sign-in")]
        public IActionResult SignIn()
        {
            return this.Ok();
        }

        [AllowAnonymous]
        [HttpPost("sign-up")]
        public IActionResult SignUp()
        {
            return this.Ok();
        }

        [HttpPost("verify")]
        public IActionResult Verify()
        {
            return this.Ok();
        }
    }
}
