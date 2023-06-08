using Application.DTOs;
using Application.Services;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IAuthenticationService _authenticationService;
    private readonly IMapper _mapper;

    public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IAuthenticationService authenticationService, IMapper mapper)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _authenticationService = authenticationService;
        _mapper = mapper;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var username = User.Identity!.Name;
        var user = await _userManager.FindByNameAsync(username!);

        if (user == null)
        {
            return NotFound();
        }

        // Return user data
        return Ok(user);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult> Login([FromBody] UserLoginDto userLoginDto)
    {
        var user = await _userManager.FindByNameAsync(userLoginDto.UserName);
        if (user != null && await _userManager.CheckPasswordAsync(user, userLoginDto.Password))
        {
            return Ok(new
            {
                token = await _authenticationService.GenerateJwtToken(user),
                expiration = _authenticationService.GetTokenExpirationDays()
            });
        }
        return Unauthorized();
    }

    [HttpPost("signup")]
    [AllowAnonymous]
    public async Task<ActionResult> SignUp([FromBody] UserSignupDto userSignupDto)
    {
        try
        {
            var userExists = await _userManager.FindByNameAsync(userSignupDto.UserName);
            if (userExists != null)
                return Conflict("User already exists!");

            var user = _mapper.Map<User>(userSignupDto);

            var result = await _userManager.CreateAsync(user, userSignupDto.Password);

            // if (!await _roleManager.RoleExistsAsync(UserRole.Customer))
            //     await _roleManager.CreateAsync(new IdentityRole(UserRole.Customer));

            // await _userManager.AddToRoleAsync(user, UserRole.Customer);

            if (!result.Succeeded)
                return UnprocessableEntity("User creation failed! Please check user details and try again.");

            return Ok("User created successfully!");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
