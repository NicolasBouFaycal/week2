using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMS.Application.Commands
{
    public class LoginCommand : IRequest<Task<ActionResult<string>>>
    {
        public ControllerBase _controller { get; set; }
        public string _email{ get; set; }
        public string _password { get; set; }
        public LoginCommand(ControllerBase controller, [FromQuery] string email, [FromQuery] string password)
        {
            _controller = controller;
            _email=email;
            _password=password;
        }
    }
}
