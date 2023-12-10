using BosonHiggsApi.BL.Models;
using BosonHiggsApi.BL.Services;
using Microsoft.AspNetCore.Mvc;

namespace BosonHiggsApi.Controllers
{
    /// <summary>
    /// Accounts controller
    /// </summary>
    [ApiController]
    [Route("api/accounts")]
    public class AccountsController : ControllerBase
    {

        private readonly AccountService _accountService;

        public AccountsController(AccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        [IpRequestThrottling(10, 1)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            var result = await _accountService.RegisterUserAsync(model, ip);

            return Ok(result);
        }
    }
}