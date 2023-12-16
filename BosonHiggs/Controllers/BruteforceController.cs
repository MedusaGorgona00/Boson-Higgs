using BosonHiggsApi.BL.Exceptions;
using BosonHiggsApi.BL.Helpers;
using BosonHiggsApi.BL.Models;
using BosonHiggsApi.BL.Services;
using Microsoft.AspNetCore.Mvc;

namespace BosonHiggsApi.Controllers
{
    /// <summary>
    /// Bruteforce controller
    /// </summary>
    [ApiController]
    [Route("api/accounts")]
    public class BruteforceController : ControllerBase
    {

        private readonly BruteforceService _service;

        public BruteforceController(BruteforceService service)
        {
            _service = service;
        }

        [HttpPost("login")] //TODO: check ddos
        //[IpRequestThrottling(10, 1)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] BruteforceModel model)
        {
            var result = await _service.Login(model);

            return Ok(result);
        }
    }
}