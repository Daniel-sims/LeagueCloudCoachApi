using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LccWebAPI.Authentication;
using LccWebAPI.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LccWebAPI.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser { UserName = model.UserName, FirstName = model.FirstName, LastName = model.LastName, Email = model.Email };

            var result = await _userManager.CreateAsync(user, model.Password);

            const string role = "Basic User";

            if (result.Succeeded)
            {
                if (await _roleManager.FindByNameAsync(role) == null)
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }

                await _userManager.AddToRoleAsync(user, role);
                await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("userName", user.UserName));
                await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("firstName", user.FirstName));
                await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("lastName", user.LastName));
                await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("email", user.Email));
                await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("role", role));

                return Ok(new ProfileViewModel(user));
            }

            return BadRequest(result.Errors);
        }

        /// <summary>
        /// Adds a new summoner that belongs to the user, I.E their smurfs
        /// </summary>
        [HttpPost("NewUserSummoner")]
        public async Task<IActionResult> AddNewUserSummonerToUserProfile(string username, string summonerName)
        {
            return BadRequest("Not yet implemented");
        }

        /// <summary>
        /// Gets all summoners for a users profile, I.E their smurfs
        /// </summary>
        [HttpPost("NewUserSummoner")]
        public async Task<IActionResult> GetUsersSummonersForUserProfile(string username)
        {
            return BadRequest("Not yet implemented");
        }

        /// <summary>
        /// Adds a new tracking summoner to a users profile, I.E someone may want to have quick access to view Apdos profile
        /// </summary>
        [HttpPost("newTrackingSummoner")]
        public async Task<IActionResult> AddNewTrackingSummonerToUserProfile(string username, string summonerName)
        {
            return BadRequest("Not yet implemented");
        }

        /// <summary>
        ///Gets all tracking summoners for a users profile
        /// </summary>
        [HttpPost("newTrackingSummoner")]
        public async Task<IActionResult> GetTrackingSummonersForUserProfile(string username)
        {
            return BadRequest("Not yet implemented");
        }
    }
}
