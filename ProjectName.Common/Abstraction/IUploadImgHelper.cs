using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Domain;
using UMS.Domain.LinqModels;

namespace UMS.Common.Abstraction
{
    public interface IUploadImgHelper
    {
        public  string UploadProfileAsync(UploadImg obj);

    }
}
