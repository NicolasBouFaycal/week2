using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Domain;

namespace UMS.Application.Abstraction
{
    public interface IAdminsHelper
    {
        public ActionResult<Course> Courses(ControllerBase controllerBase,[FromQuery] string Name, [FromQuery] int MaxStudentsNumber, [FromQuery] int startyear, [FromQuery] int startMonth, [FromQuery] int startDay, [FromQuery] int endyear, [FromQuery] int endMonth, [FromQuery] int endDay);

    }
}
