using Microsoft.AspNetCore.Mvc;
using MediatR;
using UMS.Application.Commands;

namespace UMS.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    
    public class AuthenticationController : Controller
    {
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
            
        }
        [HttpPost(template: "Login")]
        public async Task<ActionResult<string>> Login([FromQuery] string email, [FromQuery] string password)
        {
            var result = await _mediator.Send(new LoginCommand(this, email, password));
            return await result;
        }
    }
}
