using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Domain;

namespace UMS.Application.Abstraction
{
    public interface IStudentsHelper
    {
        public ActionResult<ClassEnrollment> StudentEnrollToCourse(ControllerBase controllerBase,[FromQuery] int courseId);
        public ActionResult<List<TeacherPerCourse>> GetAllClassesForStudent(ControllerBase controllerBase);

    }
}
