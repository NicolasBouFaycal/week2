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
    public class LoginCommand : IRequest<Task<string>>
    {
        public string _email{ get; set; }
        public string _password { get; set; }
        public LoginCommand( string email,  string password)
        {
            _email=email;
            _password=password;
        }
    }
    public class LoginHandler : IRequestHandler<LoginCommand, Task<string>>
    {
        private readonly IAuthenticationHelper _firebaseHelper;

        public LoginHandler(IAuthenticationHelper firebaseHelper)
        {
            _firebaseHelper = firebaseHelper;
        }
        public async Task<Task<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            return _firebaseHelper.Login(request._email, request._password);
        }

        
    }
}
