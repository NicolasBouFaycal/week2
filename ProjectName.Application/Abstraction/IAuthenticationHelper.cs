using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMS.Application.Abstraction
{
    public interface IAuthenticationHelper
    {
        public Task<string> Login( string email,  string password);

    }
}
