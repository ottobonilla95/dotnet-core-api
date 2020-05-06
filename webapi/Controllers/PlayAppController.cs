using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using webapi.Data.Interfaces;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayAppController : ControllerBase
    {
        IPlayAppRepository _playAppRepository;
        public PlayAppController(IPlayAppRepository playAppRepository)
        {
            _playAppRepository = playAppRepository;
        }

        [HttpGet("GetMenu")]
        public async Task<IActionResult> GetMenu()
        {
            var items = await _playAppRepository.GetMenu();
            return Ok(items);
        }
    }
}