using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using webapi.Data.Interfaces;
using webapi.DTO;
using webapi.Models;

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

        [HttpGet("{albumId}")]
        public async Task<IActionResult> GetAlbum(int albumId)
        {
            var album = await _playAppRepository.GetAlbum(albumId);

            if (album == null)
                return NotFound(new { message = "Not Found" });

            var albumToReturn = _mapper.Map<AlbumToReturn>(album);

            return Ok(new { albumToReturn });
        }
        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> CreateAlbum(Album album)
        {
            _playAppRepository.Add(album);
            if (await _playAppRepository.SaveAll())
                return Ok(new { album });

            return BadRequest(new { message = "Error when creating Album" });
        }

        [HttpPut]
        //[Authorize]
        public async Task<IActionResult> UpdateAlbum(AlbumForUpdate albumForUpdate)
        {
            var currentAlbum = await _playAppRepository.GetAlbum(albumForUpdate.Id);

            _mapper.Map(albumForUpdate, currentAlbum);

            if (await _playAppRepository.SaveAll())
            {
                return Ok(new { message = "Album updated" });
            }

            return BadRequest(new { message = "Album was not updated" });
        }

        [HttpDelete("{albumId}")]
        //[Authorize]
        public async Task<IActionResult> DeleteAlbum(int albumId)
        {
            var album = await _playAppRepository.GetAlbum(albumId);
            _playAppRepository.Delete(album);

            if (await _playAppRepository.SaveAll())
                return NoContent();

            return BadRequest("Failed to delete the album");
        }
    }
}