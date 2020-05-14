using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.Data.Interfaces;
using webapi.DTO;
using webapi.Models;
using webapi.Utils;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayAppController : ControllerBase
    {
        IPlayAppRepository _playAppRepository;
        IAuthRepository _authRepository;
        IMapper _mapper;
        public PlayAppController(IPlayAppRepository playAppRepository, IAuthRepository authRepository, IMapper mapper)
        {
            _playAppRepository = playAppRepository;
            _authRepository = authRepository;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet("GetMenu")]
        public async Task<IActionResult> GetMenu()
        {
            var items = await _playAppRepository.GetMenu();
            return Ok(items);
        }

        [Authorize]
        [HttpGet("GetUserProfile")]
        public async Task<IActionResult> GetUserProfile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var fetchedUser = await _playAppRepository.GetUser(userId);
            return Ok(_mapper.Map<UserProfile>(fetchedUser));
        }

        [Authorize]
        [HttpPost("SaveUserProfile")]
        public async Task<IActionResult> GetUserProfile(UserProfile user)
        {
            CloudinaryManager cm = new CloudinaryManager();
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            //Get the to use to udpate
            User UserToUpdate = await _playAppRepository.GetUser(userId);

            //Check  username
            if (UserToUpdate.Username != user.Username)
            {
                var userFound = await _authRepository.GetUserByUserName(user.Username);
                if (userFound != null)
                {
                    return BadRequest(new { message = "Username Already exists!" });
                }
            }
            //Check email
            if (UserToUpdate.Email != user.Email)
            {
                var userFound = await _authRepository.GetUserByEmail(user.Email);
                if (userFound != null)
                {
                    return BadRequest(new { message = "Email Already exists!" });
                }
            }


            if (UserToUpdate == null)
            {
                return BadRequest(new { message = "User not found!" });
            }

            UserToUpdate.Username = user.Username;
            UserToUpdate.Email = user.Email;

            //Update image
            if (user.ProfileImage != null)
            {
                string urlImage = cm.UploadImage(user.ProfileImage);
                if (urlImage != string.Empty)
                {
                    UserToUpdate.ProfileImage = urlImage;
                }
                else
                {
                    return BadRequest(new { message = "An error has ocur when uploading the image!" });
                }
            }

            await _playAppRepository.SaveAll();

            return Ok(new { message = "User updated", user = _mapper.Map<UserProfile>(UserToUpdate) });

        }
    }
}