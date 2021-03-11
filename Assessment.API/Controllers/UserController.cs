using Assessment.API.Models;
using Assessment.Data.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

namespace Assessment.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;

        public UserController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;

            // Statically add an admin account upon run just for demonstration purposes
            StaticallyAddUser(UserRole.Admin).Wait();

            // Statically add a customer account upon run just for demonstration purposes
            StaticallyAddUser(UserRole.Costumer).Wait(); ;
        }

        private async Task StaticallyAddUser(string role)
        {
            var admin = userManager.FindByEmailAsync($"{role}@assessment.no").Result;
            if (admin == null)
            {
                IdentityUser user = new IdentityUser()
                {
                    Email = $"{role}@assessment.no",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = $"{role}@assessment.no",
                    Id = role
                };
                var result = await userManager.CreateAsync(user, $"{role}1234!");

                // add role
                if (!roleManager.RoleExistsAsync(role).Result)
                    await roleManager.CreateAsync(new IdentityRole(role));

                var temp = userManager.AddToRoleAsync(user, role).Result;
            }
        }

        [HttpPost]
        [Route(nameof(Register))]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return BadRequest("User name taken!");

            IdentityUser user = new IdentityUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Email
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(new { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Route(nameof(LogIn))]
        public async Task<IActionResult> LogIn([FromBody] LogInModel logIn)
        {
            var user = await userManager.FindByEmailAsync(logIn.Email);
            if (user != null)
            {
                // check password
                var passCheck = await userManager.CheckPasswordAsync(user, logIn.Password);
                if (passCheck)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    var auth = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

                    var token = new JwtSecurityToken(issuer: configuration["JWT:ValidIssuer"], audience: configuration["JWT:ValidAudience"], expires: DateTime.Now.AddDays(1), claims: claims, signingCredentials: new SigningCredentials(auth, SecurityAlgorithms.HmacSha512));

                    return Ok(new { AccessToken = new JwtSecurityTokenHandler().WriteToken(token) });
                }
            }

            return Unauthorized();
        }
    }
}
