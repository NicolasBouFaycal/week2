using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UMS.Domain.LinqModels
{
    [Keyless]

    public partial class UploadImg
    {
        [NotMapped]
        public IFormFile file { get; set; }

    }
}
