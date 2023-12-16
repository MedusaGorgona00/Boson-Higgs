using BosonHiggsApi.BL.Models;
using BosonHiggsApi.BL.Services;
using Microsoft.AspNetCore.Mvc;

namespace BosonHiggsApi.Controllers
{
    /// <summary>
    /// Chat controller
    /// </summary>
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {

        private readonly ChatService _service;

        public ChatController(ChatService service)
        {
            _service = service;
        }

        [HttpPost("send-message")]
        //[IpRequestThrottling(100, 1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SendMessage([FromBody] MessageModel.In model)
        {
            await _service.SendMessage(model);

            return NoContent();
        }

        [HttpGet]
        [IpRequestThrottling(100, 1)]
        [ProducesResponseType(typeof(IList<MessageModel.Out>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List(string userToken)
        {
            var result = await _service.List(userToken);

            return Ok(result);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Remove(string userToken, int messageId)
        {
            await _service.Remove(userToken, messageId);

            return NoContent();
        }
    }
}