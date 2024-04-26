using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.Net.Mail;
using System.Numerics;
using WushuCompetition.Configurations;
using WushuCompetition.Dto.Identity;
using WushuCompetition.Helper;
using WushuCompetition.Services.Interfaces;

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

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (ModelState.IsValid)
            {

                if (await UserExists(registerDto.Email))
                {
                    return BadRequest("Email is already used");
                }

                var user = new IdentityUser()
                {
                    Email = registerDto.Email.ToLower(),
                    UserName = registerDto.Email.ToLower(),
                    EmailConfirmed = false
                };
                

                var isCreated = await _userManager.CreateAsync(user, registerDto.Password);
                
                if(!isCreated.Succeeded)
                {
                    return BadRequest(isCreated.Errors);
                }
                var roleResult = await _userManager.AddToRoleAsync(user, "Referee");

                if (!roleResult.Succeeded)
                {
                    return BadRequest(isCreated.Errors);
                }

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var callBackUrl = Request.Scheme + "://" + Request.Host + Url.Action("ConfirmEmail",
                    "Account", new { userId = user.Id, code = code });

                var emailBody = "Please confirm your email address. <a href=\"" + callBackUrl + "\">Click here</a>";

                var result = await _tokenService.SendEmail(emailBody, user.Email);

                if (result)
                {
                    return Ok("Please verify your email, through the verification email we have just send");
                }

                return Ok("Please request an email verification link");
               
            }
            return BadRequest(new AuthResult()
            {
                Errors = new List<string>()
                {
                    "Server error"
                },
                Result = false
            });
        }

        private async Task<bool> UserExists(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email == email.ToLower());
        }

        [HttpPost("ForgotPassword")]
        public async Task<ActionResult<AuthResult>> ForgotPassword(string emailAddress)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(elem => elem.Email == emailAddress);

            if (user == null)
            {
                return Unauthorized("invalid username or password");
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            var emailBody = "Reset email <a href=\"#URL#\">Click here</a>";

            var callBackUrl = Request.Scheme + "://" + Request.Host + Url.Action("ResetPasswordEmail",
                "Account", new { userId = user.Id, code = code });

            emailBody = emailBody.Replace("#URL#", callBackUrl);

            var result = await _tokenService.SendEmail(emailBody, user.Email);
            if (result)
            {
                return Ok("Please verify your email, through the password reset email we have just send");
            }

            return Ok("Please request an reset password link");
        }

        [HttpPost]
        [Route("RefreshToken")]
        public async Task<ActionResult<AuthResult>> RefreshToken([FromBody] TokenRequest tokenRequest)
        {
            if (ModelState.IsValid)
            {
                var result = await _tokenService.VerifyToken(tokenRequest);

                if (result == null || !result.Result)
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

                var token = await _tokenService.GenerateRefreshToken(tokenRequest);
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

        [Route("ConfirmEmail")]
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return BadRequest(new AuthResult()
                {
                    Errors = new List<string>()
                    {
                        "Invalid email confirmation url"
                    }
                });
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return BadRequest(new AuthResult()
                {
                   Errors = new List<string>()
                   {
                       "Invalid email confirmation url"
                   }
                });
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            var status=result.Succeeded
                ? "Thank you for confirming your email"
                : "Your email is not confirmed, please try again later";

            return Ok(status);
        }


        [HttpPost("Login")]
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

        [Route("ResetPasswordEmail")]
        [HttpPost]
        public async Task<IActionResult> ResetPasswordEmail(ResetPasswordDto resetPassword)
        {
            if (resetPassword.UserId == null || resetPassword.ResetPasswordToken == null)
            {
                return BadRequest(new AuthResult()
                {
                    Errors = new List<string>()
                    {
                        "Invalid email confirmation url"
                    }
                });
            }

            var user = await _userManager.FindByIdAsync(resetPassword.UserId);

            if (user == null)
            {
                return BadRequest(new AuthResult()
                {
                    Errors = new List<string>()
                    {
                        "Invalid email confirmation url"
                    }
                });
            }

            var result = await _userManager.ResetPasswordAsync(user, resetPassword.ResetPasswordToken,resetPassword.NewPassword);
            var status = result.Succeeded
                ? "You successfully reset password"
                : "Reset password is not able, please try again later";

            return Ok(status);
        }
    }
}
