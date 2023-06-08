using System.Security.Claims;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    //private readonly UserManager<User> _userManager;
    //private readonly RoleManager<IdentityRole> _roleManager;
    //private readonly IConfiguration _configuration;
    
    //public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration) 
    //{
    //    _userManager = userManager;
    //    _roleManager = roleManager;
    //    _configuration = configuration;      
    //}

    //[HttpGet("me")]
    //public async Task<IActionResult> GetCurrentUser()
    //{
    //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //    var user = await _userManager.FindByIdAsync(userId!);

    //    if (user == null)
    //    {
    //        return NotFound();
    //    }

    //    // Return user data
    //    return Ok(user);
    //}
}
