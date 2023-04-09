using UMS.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace UMS.Application.Abstraction
{
    public interface ITeachersHelper
    {
        public  ActionResult<List<Course>> AllCourses();
        public ActionResult<List<TeacherPerCourse>> AllTeacherPerCourse(ControllerBase controllerBase);
        public ActionResult<List<SessionTime>> AllSessionTime(ControllerBase controllerBase);
        public ActionResult<TeacherPerCourse> AssignTeacherToCourse(ControllerBase controllerBase, [FromQuery] int courseId);
        public ActionResult<TeacherPerCoursePerSessionTime> AssignTeacherPerCoursePerSessionTime(ControllerBase controllerBase,[FromQuery] int TeacherPerCourseId, [FromQuery] int SessionTimeId);
        public ActionResult<SessionTime> InserSessionTime(ControllerBase controllerBase,[FromQuery] DateTime StartTimeYYYY_MM_DD, [FromQuery] DateTime EndTimeYYYY_MM_DD, [FromQuery] int Duration);


    }
}
