using BosonHiggsApi.BL.Models;
using BosonHiggsApi.BL.Services;
using Microsoft.AspNetCore.Mvc;

namespace BosonHiggsApi.Controllers
{
    /// <summary>
    /// Leader-boards controller
    /// </summary>
    [ApiController]
    [Route("api/leader-boards")]
    public class LeaderBoardsController : ControllerBase
    {

        private readonly LeaderBoardService _LeaderBoardService;

        public LeaderBoardsController(LeaderBoardService LeaderBoardService)
        {
            _LeaderBoardService = LeaderBoardService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IList<LeaderModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List()
        {
            var result = await _LeaderBoardService.List();

            return Ok(result);
        }
    }
}