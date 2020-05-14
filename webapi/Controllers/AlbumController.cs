using System.Collections.Generic;
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
    public class AlbumController : ControllerBase
    {
        IPlayAppRepository _playAppRepository;
        IMapper _mapper;
        public AlbumController(IPlayAppRepository playAppRepository, IMapper mapper)
        {
            _playAppRepository = playAppRepository;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var album = await _playAppRepository.GetAllAlbums();

            var albumToReturn = _mapper.Map<List<AlbumToReturn>>(album).ToArray();

            return Ok(albumToReturn );
        }
        [Authorize]
        [HttpGet("{albumId}")]
        public async Task<IActionResult> GetAlbum(int albumId)
        {
            
            var album = await _playAppRepository.GetAlbum(albumId);

            if (album == null)
                return NotFound(new { message = "Not Found" });

            var albumToReturn = _mapper.Map<AlbumToReturn>(album);

            return Ok(albumToReturn);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateAlbum(Album album)
        {
            CloudinaryManager cm = new CloudinaryManager();
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            album.UserId = userId;

            if (album.Image != null)
            {
                string urlImage = cm.UploadImage(album.Image);
                if (urlImage != string.Empty)
                {
                    album.Image = urlImage;
                }
                else
                {
                    return BadRequest(new { message = "An error has ocur when uploading the image!" });
                }
            }

            _playAppRepository.Add(album);
            if (await _playAppRepository.SaveAll())
                return Ok(new { message = "Album Created!", data = album });

            return BadRequest(new { message = "Error when creating Album" });
        }


        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateAlbum(AlbumForUpdate albumForUpdate)
        {
            CloudinaryManager cm = new CloudinaryManager();
            var currentAlbum = await _playAppRepository.GetAlbum(albumForUpdate.Id);
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (userId != currentAlbum.UserId)
            {
                return BadRequest(new { message = "Not authorized" });
            }

            if (albumForUpdate.Image != null)
            {
                string urlImage = cm.UploadImage(albumForUpdate.Image);
                if (urlImage != string.Empty)
                {
                    albumForUpdate.Image = urlImage;
                }
                else
                {
                    return BadRequest(new { message = "An error has ocur when uploading the image!" });
                }
            }
            else
            {
                albumForUpdate.Image = currentAlbum.Image;
            }


            _mapper.Map(albumForUpdate, currentAlbum);

            if (await _playAppRepository.SaveAll())
            {
                return Ok(new { message = "Album updated", data= _mapper.Map<AlbumToReturn>(currentAlbum) });
            }

            return BadRequest(new { message = "Album was not updated" });
        }

        [Authorize]
        [HttpDelete("{albumId}")]
        public async Task<IActionResult> DeleteAlbum(int albumId)
        {
            var album = await _playAppRepository.GetAlbum(albumId);
            _playAppRepository.Delete(album);

            if (await _playAppRepository.SaveAll())
                return Ok(new { message = "Album Deleted!"});

            return BadRequest("Failed to delete the album");
        }
    }
}