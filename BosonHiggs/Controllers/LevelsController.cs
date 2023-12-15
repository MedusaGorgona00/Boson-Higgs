using System.ComponentModel.DataAnnotations;
using BosonHiggsApi.BL.Models;
using BosonHiggsApi.BL.Services;
using Microsoft.AspNetCore.Mvc;

namespace BosonHiggsApi.Controllers
{
    /// <summary>
    /// Levels controller
    /// </summary>
    [ApiController]
    [Route("api/levels")]
    public class LevelsController : ControllerBase
    {

        private readonly LevelService _levelService;

        public LevelsController(LevelService levelService)
        {
            _levelService = levelService;
        }

        [HttpGet("by-token")]
        [IpRequestThrottling(100, 1)]
        [ProducesResponseType(typeof(LevelModel.GetByUser), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByToken([Required] string token, [Required] string userToken)
        {
            var result = await _levelService.GetByToken(token, userToken);

            return Ok(result);
        }

        [HttpGet("get-hint")]
        [IpRequestThrottling(10, 1)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetHint([Required] string id, [Required] string userToken)
        {
            var result = await _levelService.GetHint(id, userToken);

            return Ok(result);
        }

        [HttpGet("get-next-level-token")]
        [IpRequestThrottling(10, 1)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetNextLevelToken([Required] string id, [Required] string userToken)
        {
            var result = await _levelService.GetNextLevelToken(id, userToken);

            return Ok(result);
        }
    }
}