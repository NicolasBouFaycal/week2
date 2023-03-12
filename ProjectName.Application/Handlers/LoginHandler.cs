using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Application.Abstraction;
using UMS.Application.Commands;

namespace UMS.Application.Handlers
{
    public class LoginHandler : IRequestHandler<LoginCommand, Task<ActionResult<string>>>
    {
        private readonly IFirebaseHelper _firebaseHelper;

        public LoginHandler(IFirebaseHelper firebaseHelper)
        {
            _firebaseHelper = firebaseHelper;
        }
        public async Task<Task<ActionResult<string>>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            return _firebaseHelper.Login(request._controller, request._email, request._password);
        }
    }
}
