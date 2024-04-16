using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WushuCompetition.Configurations;
using WushuCompetition.Helper;
using WushuCompetition.Models;
using WushuCompetition.Repository.Interfaces;
using WushuCompetition.Services.Interfaces;

namespace WushuCompetition.Services
{
    public class TokenService:ITokenService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly JwtConfig _jwtConfig;

        public TokenService(UserManager<IdentityUser> userManager,
             IOptions<JwtConfig> jwtConfig, IRefreshTokenRepository refreshTokenRepository, TokenValidationParameters tokenValidationParameters)
        {
            _userManager = userManager;
            _refreshTokenRepository = refreshTokenRepository;
            _tokenValidationParameters = tokenValidationParameters;
            _jwtConfig = jwtConfig.Value;
        }

        public async Task<AuthResult>CreateToken(IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim("Id",user.Id),
                new Claim(JwtRegisteredClaimNames.NameId,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(TimeSpan.Parse(_jwtConfig.ExpirationTime.ToString())),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var jwtToken= tokenHandler.WriteToken(token);

            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                Token = RandomStringGeneration(22), //Generate a refresh token
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6),
                IsRevoked = false,
                IsUsed = false,
                UserId = user.Id
            };

            await _refreshTokenRepository.CreateRefreshToken(refreshToken);

            return new AuthResult()
            {
                Token = jwtToken,
                RefreshToken = refreshToken.Token,
                Result = true
            };
        }

        private string RandomStringGeneration(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890abcdefghijklmnopqrstuvwxyz_";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToUniversalTime();

            return dateTimeVal;
        }

        public async Task<AuthResult> VerifyToken(TokenRequest tokenRequest)
        {
            var jwtTokenHandler=new JwtSecurityTokenHandler();
            
            try
            {
                _tokenValidationParameters.ValidateLifetime = false;
                var tokenInVerification = 
                    jwtTokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParameters, out var validatedToken);

                if(validatedToken is JwtSecurityToken jwtSecurityToken)
                {

                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512,
                        StringComparison.InvariantCulture);

                    if (!result)
                    {
                        return new AuthResult()
                        {
                            Result = false,
                            Errors = new List<string>()
                            {
                                "Invalid Error"
                            }
                        }; 
                    }
                }

                var utcExpiryDate = long.Parse(tokenInVerification.Claims
                     .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);
                
                if(expiryDate>DateTime.Now)
                {
                    return new AuthResult()
                    {
                        Result = false,
                        Errors = new List<string>()
                        {
                            "Expired token"
                        }
                    };
                }

                var storedToken = await _refreshTokenRepository.GetRefreshToken(tokenRequest);

                if(storedToken==null)
                {
                    return new AuthResult()
                    {
                        Result = false,
                        Errors = new List<string>()
                        {
                            "Invalid tokens"
                        }
                    };
                }

                if(storedToken.IsUsed)
                {
                    return new AuthResult()
                    {
                        Result = false,
                        Errors = new List<string>()
                        {
                            "Invalid tokens"
                        }
                    };
                }

                if(storedToken.IsRevoked)
                {
                    return new AuthResult()
                    {
                        Result = false,
                        Errors = new List<string>()
                        {
                            "Invalid tokens"
                        }
                    };
                }

                var jti = tokenInVerification.Claims.FirstOrDefault(elem=>
                    elem.Type== JwtRegisteredClaimNames.Jti).Value;


                if(storedToken.JwtId!=jti)
                {
                    return new AuthResult()
                    {
                        Result = false,
                        Errors = new List<string>()
                        {
                            "Invalid tokens"
                        }
                    };
                }

                if(storedToken.ExpiryDate<DateTime.UtcNow)
                {
                    return new AuthResult()
                    {
                        Result = false,
                        Errors = new List<string>()
                        {
                            "Expired token"
                        }
                    };
                }

                return new AuthResult()
                {
                    Result = true,
                    Token="token will be generated"
                };

            }
            catch (Exception ex)
            {
                return new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Server error"
                    }
                };
            }
        }

        public async Task<AuthResult>GenerateRefreshToken(TokenRequest tokenRequest)
        {
            try
            {
                var storedToken = await _refreshTokenRepository.GetRefreshToken(tokenRequest);
                storedToken.IsUsed = true;
                await _refreshTokenRepository.UpdateRefreshToken(storedToken);

                var dbUser = await _userManager.FindByIdAsync(storedToken.UserId);
                return await CreateToken(dbUser);
            }
            catch(Exception ex)
            {
                return new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Server error"
                    }
                };
            }
            
        }
    }
}
