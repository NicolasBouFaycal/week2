
using Microsoft.AspNetCore.Mvc;
using UMS.Application.Abstraction;
using UMS.Domain;
using UMS.Persistence;
using UMS.Common;

namespace UMS.Application.Service
{
    public class TeachersHelper:ITeachersHelper
    {
        public readonly MyDbContext _context;
        public TeachersHelper(MyDbContext context)
        {
            _context = context;
        }

        public ActionResult<TeacherPerCoursePerSessionTime> AssignTeacherPerCoursePerSessionTime(ControllerBase controllerBase,[FromQuery] int teacherPerCourseId, [FromQuery] int sessionTimeId)
        {
            try
            {
                var userid = controllerBase.Request.Cookies["UserId"];
                TeacherPerCoursePerSessionTime SessionCourse = new TeacherPerCoursePerSessionTime();
                SessionCourse.Id = 2;
                SessionCourse.TeacherPerCourseId = teacherPerCourseId;
                SessionCourse.SessionTimeId = sessionTimeId;

                if (SessionCourse == null)
                    throw new InvalidOperationException("Insert Data");
                _context.Add(SessionCourse);
                _context.SaveChanges();
                return controllerBase.Ok(SessionCourse);
            }
            catch (Exception ex)
            {
                throw new Exception("connection not find");

            }
        }

        public ActionResult<TeacherPerCourse> AssignTeacherToCourse(ControllerBase controllerBase,[FromQuery] int courseId)
        {
            try
            {
                var userid = controllerBase.Request.Cookies["UserId"];
                TeacherPerCourse course = new TeacherPerCourse();
                course.Id = 3;
                course.CourseId = courseId;
                course.TeacherId = (from s in _context.Users where (s.KeycloakId == userid) select s.Id).FirstOrDefault();

                if (course == null)
                    throw new InvalidOperationException("INsert Data");
                _context.Add(course);
                _context.SaveChanges();
                return controllerBase.Ok(course);
            }
            catch (Exception ex)
            {
                throw new Exception("connection not fond");

            }
        }

        public ActionResult<List<Course>> GetAllCourses()
        {
            var courses = _context.Courses
            .Where(c => !_context.TeacherPerCourses
                .Any(tpc => tpc.CourseId == c.Id))
            .ToList();

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            var courses2 = _context.Courses
                .Where(course => !course.TeacherPerCourses.Any())
                .ToList();

            if (courses.Count > 0)
            {
                return courses;
            }
            throw new Exception("NUll");
        }

        public ActionResult<List<SessionTime>> GetAllSessionTime(ControllerBase controllerBase)
        {
            var userid = controllerBase.Request.Cookies["UserId"];
            var getTeacherId = (from t in _context.Users where t.KeycloakId == userid select t.Id).FirstOrDefault();

            var Scourses = _context.SessionTimes
            .Where(st => !_context.TeacherPerCoursePerSessionTimes
                .Any(tpcpst => tpcpst.SessionTimeId == st.Id))
            .ToList();
            if (Scourses.Count > 0)
            {
                return Scourses;
            }
            throw new Exception("NUll");

        }

        public ActionResult<List<TeacherPerCourse>> GetAllTeacherPerCourse(ControllerBase controllerbase)
        {
            var userid = controllerbase.Request.Cookies["UserId"];
            var getTeacherId = (from t in _context.Users where t.KeycloakId == userid select t.Id).FirstOrDefault();

            var thcourses = _context.TeacherPerCourses
            .Where(tpc => !_context.TeacherPerCoursePerSessionTimes
                .Any(tpcpst => tpcpst.TeacherPerCourseId == tpc.Id && tpc.TeacherId != getTeacherId))
            .ToList();
            if (thcourses.Count > 0)
            {
                return thcourses;
            }
            throw new Exception("NUll");
        }

        public ActionResult<SessionTime> InserSessionTime(ControllerBase controllerBase,[FromQuery] DateTime StartTimeYYYY_MM_DD, [FromQuery] DateTime EndTimeYYYY_MM_DD, [FromQuery] int Duration)
        {
            try
            {
                SessionTime tm = new SessionTime();
                tm.StartTime = StartTimeYYYY_MM_DD;
                tm.EndTime = EndTimeYYYY_MM_DD;
                tm.Duration = Duration;
                tm.Id = 5;
                if (tm == null)
                    throw new InvalidOperationException("INsert Data");
                _context.Add(tm);
                _context.SaveChanges();
                return controllerBase.Ok(tm);
            }
            catch (Exception ex)
            {
                throw new Exception("connection not find");

            }
        }
    }
}
