using API.Dtos;
using API.IService;
using API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(IUserService userService, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userService = userService;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] AuthRequestDto authDto)
        {
            AppUser? user = await _userService.FindByUsernameAsync(authDto.Username);

            if (user is null) return Unauthorized("Wrong username or password");

            var result = await _signInManager.CheckPasswordSignInAsync(user, authDto.Password, false);

            if (!result.Succeeded) return Unauthorized("Wrong username or password");

            return new UserDto()
            {
                Username = authDto.Username,
                Token = await _tokenService.CreateToken(user)
            };
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] AuthRequestDto authDto)
        {
            AppUser user = await _userService.CreateAsync(authDto);

            UserDto userDto = new()
            {
                Username = authDto.Username,
                Token = await _tokenService.CreateToken(user),
            };

            return Ok(userDto);
        }
    }
}
