using BlazorApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BlazorApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = Policies.IsAdmin)]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _userManager.Users
                .Select(x => new UserRecord { Id = x.Id, Username = x.UserName })
                .ToArray();

            return Ok(users);
        }
    }
}