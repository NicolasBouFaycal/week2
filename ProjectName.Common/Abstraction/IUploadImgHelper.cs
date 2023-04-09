using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Domain;

namespace UMS.Common.Abstraction
{
    public interface IUploadImgHelper
    {
        public  ActionResult<string> UploadProfileAsync( [FromForm] UploadImg obj);

    }
}
