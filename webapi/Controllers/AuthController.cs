using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using webapi.Data.Interfaces;
using webapi.DTO;
using webapi.Models;
using webapi.Utils;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        IAuthRepository _authRepository;
        IPlayAppRepository _playAppRepository;
        IConfiguration _configuration;
        IMapper _mapper;
        public AuthController(IAuthRepository authRepository, IConfiguration configuration, IPlayAppRepository playAppRepository, IMapper mapper)
        {
            _authRepository = authRepository;
            _configuration = configuration;
            _playAppRepository = playAppRepository;
            _mapper = mapper;
        }

        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            return Ok(new { message = "Working!" });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegister userForRegister)
        {
            byte[] passwordHash;
            byte[] passwordSalt;



            var userFound = await _authRepository.GetUserByUserName(userForRegister.Username);
            if (userFound == null)
            {
                userFound = await _authRepository.GetUserByEmail(userForRegister.Email);

                if (userFound != null)
                {
                    return BadRequest(new { message = "Email already exists!" });
                }
            }
            else
            {
                return BadRequest(new { message = "Username Already exists!" });
            }


            Utilities.CreatePasswordHash(userForRegister.Password, out passwordHash, out passwordSalt);
            User user = _mapper.Map<User>(userForRegister);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _playAppRepository.Add(user);
            await _playAppRepository.SaveAll();


            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.Username));

            var accessToken = GenerateToken(claims);
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            await _authRepository.SaveRefreshToken(user);

            return Ok(new
            {
                accessToken = accessToken.Token,
                expirationDate = accessToken.ExpirationDate.ToString("s"),
                refreshToken = refreshToken.Replace("+", "%2B")
            });
        }


        [HttpPost("recover")]
        public async Task<IActionResult> Recover(string email)
        {
            return Ok(new { message = "An email with a new password was sent to " + email });
        }

        [HttpPost("login")]
        public async Task<IActionResult> LogIn(UserForLogin user)
        {
            try
            {
                User userFound = await _authRepository.GetUserByUserName(user.Username);

                if (userFound == null)
                    userFound = await _authRepository.GetUserByEmail(user.Username);

                if (userFound != null)
                {

                    if (Utilities.VerifyPasswordHash(user.Password, userFound.PasswordHash, userFound.PasswordSalt))
                    {
                        List<Claim> claims = new List<Claim>();
                        claims.Add(new Claim("UserId", userFound.Id.ToString()));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, userFound.Id.ToString()));
                        claims.Add(new Claim(ClaimTypes.Name, userFound.Username));

                        var accessToken = GenerateToken(claims);
                        var refreshToken = GenerateRefreshToken();
                        userFound.RefreshToken = refreshToken;
                        await _authRepository.SaveRefreshToken(userFound);

                        

                        return Ok(new
                        {
                            accessToken = accessToken.Token,
                            expirationDate = accessToken.ExpirationDate.ToString("s"),
                            refreshToken = refreshToken.Replace("+", "%2B")
                        });
                    }
                    else
                    {
                        return Unauthorized(new
                        {
                            message = "Invalid credentials!"
                        });
                    }

                }
                else
                {
                    return Unauthorized(new { message = "Invalid credentials!" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error when login" });
            }
        }

        

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(string refreshToken)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var claims = User.Claims;

            User userFound = await _authRepository.GetUserById(userId);
            
            if (userFound.RefreshToken != refreshToken)
                return BadRequest(new { message = "Invalid refresh token" });

            var accessToken = GenerateToken(claims);
            var newRefreshToken = GenerateRefreshToken();

            userFound.RefreshToken = newRefreshToken;

            await _authRepository.SaveRefreshToken(userFound);

            return Ok(new
            {
                accessToken = accessToken.Token,
                expirationDate = accessToken.ExpirationDate.ToString("s"),
                refreshToken = newRefreshToken.Replace("+", "%2B")
            });
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public AccessToken GenerateToken(IEnumerable<Claim> claims)
        {
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("jwt").GetValue<string>("SecretKey"));
            var expirationInDays = int.Parse( _configuration.GetSection("jwt").GetValue<string>("expirationInDays"));

            var tokenHandler = new JwtSecurityTokenHandler();

            var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(expirationInDays),
                SigningCredentials = creds
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AccessToken { Token = tokenHandler.WriteToken(token), ExpirationDate = DateTime.Now.AddMinutes(1) };
        }
    }
}
