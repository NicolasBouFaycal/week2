using Microsoft.AspNetCore.Mvc;
using MediatR;
using UMS.Application.Commands;
using UMS.Domain.Models;

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
        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login([FromBody] Authentication auth)
        {
            var result = await _mediator.Send(new LoginCommand( auth.Email, auth.Password));
            return await result;
        }
    }
}
