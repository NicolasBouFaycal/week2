using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMS.Application.Abstraction
{
    public interface IFirebaseHelper
    {
        public Task<ActionResult<string>> Login(ControllerBase controllerBase, [FromQuery] string email, [FromQuery] string password);

    }
}
