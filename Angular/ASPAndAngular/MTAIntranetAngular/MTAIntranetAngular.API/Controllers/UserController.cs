using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MTAIntranetAngular.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public string? GetUser()
        {
            var user = User.Identity?.Name;
            return user;
        }

        [Route("HasRole/{role}")]
        [HttpGet]
        public string? UserHasRole(string role)
        {
            var user = User.Identity?.Name;

            return User.IsInRole(role).ToString();

        }
    }
}
