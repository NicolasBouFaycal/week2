using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Application.Abstraction;

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
    public class LoginHandler : IRequestHandler<LoginCommand, Task<ActionResult<string>>>
    {
        private readonly IAuthenticationHelper _firebaseHelper;

        public LoginHandler(IAuthenticationHelper firebaseHelper)
        {
            _firebaseHelper = firebaseHelper;
        }
        public async Task<Task<ActionResult<string>>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            return _firebaseHelper.Login(request._controller, request._email, request._password);
        }
    }
}
