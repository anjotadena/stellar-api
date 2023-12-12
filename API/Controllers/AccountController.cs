using System.Security.Claims;
using API.DTO;
using API.Errors;
using Core.Entities;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ApiBaseController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _tokenService = tokenService;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        var user = await _userManager.FindByEmailAsync(email);

        return new UserDto
        {
            Email = user.Email,
            Token = _tokenService.CreateToken(user),
            DisplayName = user.DisplayName
        };
    }

    [HttpGet("emailexists")]
    public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
    {
        return await _userManager.FindByEmailAsync(email) is not null;
    }

    [Authorize]
    [HttpGet("address")]
    public async Task<ActionResult<Address>> GetUserAddress()
    {
        var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        var user = await _userManager.FindByEmailAsync(email);

        return user.Address;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);

        if (user is null)
        {
            return Unauthorized(new ApiResponse(StatusCodes.Status401Unauthorized));
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded)
        {
            return Unauthorized(new ApiResponse(StatusCodes.Status401Unauthorized));
        }

        return new UserDto
        {
            Email = user.Email,
            Token = _tokenService.CreateToken(user),
            DisplayName = user.DisplayName
        };
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        var user = new AppUser
        {
            DisplayName = registerDto.DisplayName,
            Email = registerDto.Email,
            UserName = registerDto.Email
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest));
        }

        return new UserDto
        {
            Email = user.Email,
            Token = _tokenService.CreateToken(user),
            DisplayName = user.DisplayName
        };
    }
}