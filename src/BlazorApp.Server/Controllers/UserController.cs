using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(
            IConfiguration configuration,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager)
        {
            _configuration = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Post([FromBody]RegisterModel model)
        {
            var newUser = new IdentityUser
            {
                UserName = model.Username,
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description);
                return BadRequest(RegisterResult.Fail(errors));
            }

            await _userManager.AddToRoleAsync(newUser, "User");

            if (newUser.UserName == "admin")
                await _userManager.AddToRoleAsync(newUser, "Admin");

            return Ok(RegisterResult.Success());
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            var result = await _signInManager.PasswordSignInAsync(login.Username, login.Password, false, false);

            if (!result.Succeeded)
                return BadRequest(LoginResult.Fail("Login credentials are invalid."));

            var user = await _signInManager.UserManager.FindByNameAsync(login.Username);
            var roles = await _signInManager.UserManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, login.Username),
            };

            var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role));
            claims.AddRange(roleClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddHours(double.Parse(_configuration["Jwt:ExpiryInHours"]));

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: expiry,
                signingCredentials: creds
            );

            return Ok(LoginResult.Success(new JwtSecurityTokenHandler().WriteToken(token)));
        }
    }
}