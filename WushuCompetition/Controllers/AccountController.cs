using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WushuCompetition.Configurations;
using WushuCompetition.Dto.Identity;
using WushuCompetition.Helper;
using WushuCompetition.Services.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WushuCompetition.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(ITokenService tokenService, IMapper mapper,
            UserManager<IdentityUser> userManager)
        {
            _tokenService = tokenService;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Email))
            {
                return BadRequest("Email is already used");
            }

            var user = _mapper.Map<IdentityUser>(registerDto);
            user.UserName = registerDto.Email.ToLower();

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "Referee");

            if (!roleResult.Succeeded) return BadRequest(result.Errors);

            return Ok( "Login to complete the authentication process");
        }

        private async Task<bool> UserExists(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email == email.ToLower());
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResult>> Login(LoginDto loginDto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(elem => elem.Email == loginDto.Email);

            if (user == null)
            {
                return Unauthorized("invalid username or password");
            }

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!result)
            {
                return Unauthorized("invalid username or password");
            }

            var jwtToken = await _tokenService.CreateToken(user);
            return Ok(jwtToken);
        }

        [HttpPost]
        [Route("RefreshToken")]
        public async Task<ActionResult<AuthResult>> RefreshToken([FromBody]TokenRequest tokenRequest)
        {
            if(ModelState.IsValid)
            {
                var result = await _tokenService.VerifyToken(tokenRequest);

                if(result == null || !result.Result)
                {
                    return BadRequest(new AuthResult()
                    {
                        Errors = new List<string>()
                        {
                            "Invalid parameters"
                        },
                        Result = false
                    });
                }

                var token =await _tokenService.GenerateRefreshToken(tokenRequest);
                if (token == null)
                {
                    return BadRequest(new AuthResult()
                    {
                        Errors = new List<string>()
                        {
                            "Server Error"
                        },
                        Result = false
                    });
                }

                return Ok(token);
            }
            return BadRequest(new AuthResult()
            {
                Errors = new List<string>()
                {
                    "Invalid parameters"
                },
                Result = false
            });
        }
    }
}
