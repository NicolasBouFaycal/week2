using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectName.Domain;
using ProjectName.Persistence;

namespace Project.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Teacher")]
    public class TeacherController : ControllerBase
    {
        public readonly MyDbContext _context;
        public TeacherController(MyDbContext context)
        {
            _context = context; 
        }
        [HttpGet("GetAllCourses")]
        public async Task<ActionResult<List<Course>>> GetAllCourses()
        {
            var courses = _context.Courses
            .Where(c => !_context.TeacherPerCourses
                .Any(tpc => tpc.CourseId == c.Id))
            .ToList();
            if (courses.Count > 0)
            {
                return courses;
            }
            throw new NullReferenceException("NUll");

        }
        [HttpGet("GetAllTeacherPerCourse")]
        public async Task<ActionResult<List<TeacherPerCourse>>> GetAllTeacherPerCourse()
        {
            var userid = Request.Cookies["UserId"];
            var getTeacherId = (from t in _context.Users where t.KeycloakId == userid select t.Id).FirstOrDefault();

            var thcourses = _context.TeacherPerCourses
            .Where(tpc => !_context.TeacherPerCoursePerSessionTimes
                .Any(tpcpst => tpcpst.TeacherPerCourseId == tpc.Id && tpc.TeacherId != getTeacherId))
            .ToList();
            if (thcourses.Count > 0)
            {
                return thcourses;
            }
            throw new NullReferenceException("NUll");

        }
        [HttpGet("GetAllSessionTime")]
        public async Task<ActionResult<List<SessionTime>>> GetAllSessionTime()
        {
            var userid = Request.Cookies["UserId"];
            var getTeacherId = (from t in _context.Users where t.KeycloakId == userid select t.Id).FirstOrDefault();

            var Scourses = _context.SessionTimes
            .Where(st => !_context.TeacherPerCoursePerSessionTimes
                .Any(tpcpst => tpcpst.SessionTimeId == st.Id))
            .ToList();
            if (Scourses.Count > 0)
            {
                return Scourses;
            }
            throw new NullReferenceException("NUll");

        }

        [HttpPost(template: "AssignTeacherToCourse")]
        public async Task<ActionResult<TeacherPerCourse>> AssignTeacherToCourse([FromQuery] int RoleId)
        {
            try
            {
                var userid = Request.Cookies["UserId"];
                TeacherPerCourse course = new TeacherPerCourse();
                course.Id = 1;
                course.CourseId = RoleId;
                course.TeacherId = (from s in _context.Users where (s.KeycloakId == userid) select s.Id).FirstOrDefault();

                if (course == null)
                    throw new InvalidOperationException("INsert Data");
                _context.Add(course);
                _context.SaveChanges();
                return Ok(course);
            }
            catch (Exception ex)
            {
                throw new Exception("connection not find");

            }
        }
        [HttpPost(template: "AssignTeacher/CourseToSessionTime")]
        public async Task<ActionResult<TeacherPerCoursePerSessionTime>> AssignToSessionTime([FromQuery] int TeacherPerCourseId, [FromQuery] int SessionTimeId)
        {
            try
            {
                var userid = Request.Cookies["UserId"];
                TeacherPerCoursePerSessionTime SessionCourse = new TeacherPerCoursePerSessionTime();
                SessionCourse.Id = 1;
                SessionCourse.TeacherPerCourseId = TeacherPerCourseId;
                SessionCourse.SessionTimeId = SessionTimeId;

                if (SessionCourse == null)
                    throw new InvalidOperationException("INsert Data");
                _context.Add(SessionCourse);
                _context.SaveChanges();
                return Ok(SessionCourse);
            }
            catch (Exception ex)
            {
                throw new Exception("connection not find");

            }
        }
    }
}
