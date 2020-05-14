using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webapi.Data.Interfaces;
using webapi.DTO;
using webapi.Models;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongController : ControllerBase
    {
        IPlayAppRepository _playAppRepository;
        IMapper _mapper;
        public SongController(IPlayAppRepository playAppRepository, IMapper mapper)
        {
            _playAppRepository = playAppRepository;
            _mapper = mapper;
        }

        [HttpGet("{songId}")]
        public async Task<IActionResult> GetSong(int songId)
        {
            var song = await _playAppRepository.GetSong(songId);

            if (song == null)
                return NotFound(new { message = "Not Found" });

            return Ok(new { song });
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateSong(Song song)
        {
            _playAppRepository.Add(song);
            if (await _playAppRepository.SaveAll())
                return Ok(new { data = song });

            return BadRequest(new { message="Error when creating the song"});
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateSong(SongForUpdate songForUpdate)
        {
            var currentSong = await _playAppRepository.GetSong(songForUpdate.Id);

            _mapper.Map(songForUpdate, currentSong);

            if (await _playAppRepository.SaveAll())
            {
                return Ok(new {data= currentSong });
            }

            return BadRequest(new { message = "Song was not updated" });
        }

        [HttpDelete("{songId}")]
        [Authorize]
        public async Task<IActionResult> DeleteSong(int songId)
        {
            var song = await _playAppRepository.GetSong(songId);
            _playAppRepository.Delete(song);

            if (await _playAppRepository.SaveAll())
                return NoContent();

            return BadRequest("Failed to delete the song");
        }
    }
}