using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NpgsqlTypes;
using ProjectName.Domain;
using ProjectName.Persistence;

namespace Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagementController : ControllerBase
    {
        public readonly MyDbContext _context;
        public ManagementController(MyDbContext context)
        {
            _context = context;
        }
        [HttpPost(template: "SessionTime")]
        public async Task<ActionResult<SessionTime>> InsertTime([FromQuery] DateTime StartTimeYYYY_MM_DD, [FromQuery] DateTime EndTimeYYYY_MM_DD, [FromQuery] int Duration)
        {
            try
            {
                SessionTime tm = new SessionTime();
                tm.StartTime = StartTimeYYYY_MM_DD;
                tm.EndTime = EndTimeYYYY_MM_DD;
                tm.Duration = Duration;
                tm.Id = 4;
                if (tm == null)
                    throw new InvalidOperationException("INsert Data");
                _context.Add(tm);
                _context.SaveChanges();
                return Ok(tm);
            }
            catch (Exception ex)
            {
                throw new Exception("connection not find");

            }
        }
        [HttpPost(template: "CreateCourse")]
        public async Task<ActionResult<Course>> CreateCourse([FromQuery] string Name, [FromQuery] int MaxStudentsNumber, [FromQuery] int startyear, [FromQuery] int startMonth, [FromQuery] int startDay, [FromQuery] int endyear, [FromQuery] int endMonth, [FromQuery] int endDay)
        {
            try
            {
                var startDate = new DateOnly(startyear, startMonth, startDay);
                var endDate = new DateOnly(endyear, endMonth, endDay - 1);
                var range = new NpgsqlRange<DateOnly>(startDate, endDate);

                Course tm = new Course();
                tm.EnrolmentDateRange = range;
                tm.Name = Name;
                tm.MaxStudentsNumber = MaxStudentsNumber;
                tm.Id = 6;
                if (tm == null)
                    throw new InvalidOperationException("INsert Data");
                _context.Add(tm);
                _context.SaveChanges();
                return Ok(tm);
            }
            catch (Exception ex)
            {
                throw new Exception("change course id");

            }
        }
            [HttpPost(template: "CreateRole")]
            public async Task<ActionResult<Role>> CreateRole([FromQuery] string Name)
            {
                try
                {
                    

                    Role r=new Role();
                    r.Name = Name;
                    r.Id = 3;
                    if (r == null)
                        throw new InvalidOperationException("INsert Data");
                    _context.Add(r);
                    _context.SaveChanges();
                    return Ok(r);
                }
                catch (Exception ex)
                {
                    throw new Exception("connection not find");

                }
            }
        [HttpPost(template: "CreateUser")]
        public async Task<ActionResult<Role>> CreateUser([FromQuery] string Name, [FromQuery] string Email, [FromQuery] string FireBaseId)
        {
            try
            {


                User r = new User();
                r.Name = Name;
                r.Email=Email;
                r.KeycloakId=FireBaseId;
                r.Id = 4;
                r.RoleId = 3;
                if (r == null)
                    throw new InvalidOperationException("INsert Data");
                _context.Add(r);
                _context.SaveChanges();
                return Ok(r);
            }
            catch (Exception ex)
            {
                throw new Exception("connection not find");

            }
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
                .Any(tpcpst => tpcpst.SessionTimeId == st.Id ))
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
                course.CourseId= RoleId;
                course.TeacherId = (from s in _context.Users where (s.KeycloakId == userid)select s.Id).FirstOrDefault();
               
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
        [HttpGet("GetAllCoursesForStudent")]
        public async Task<ActionResult<List<Course>>> GetAllCoursesForStudent()
        {
            var userid = Request.Cookies["UserId"];
            var getStudentId = (from t in _context.Users where t.KeycloakId == userid select t.Id).FirstOrDefault();
            var courses = _context.Courses
            .Where(c => !_context.ClassEnrollments
                .Any(ce => ce.ClassId == c.Id && ce.StudentId == getStudentId))
            .ToList(); 
     
            if (courses.Count > 0)
            {
                return courses;
            }
            throw new NullReferenceException("NUll");

        }
        [HttpPost(template:"StudentEnrollToCourse")]
        public async Task<ActionResult<ClassEnrollment>> StudentEnrollToCourse([FromQuery] int courseId )
        {
            var courses = (from c in _context.Courses select c).ToList();
            var userid = Request.Cookies["UserId"];
            if (courses.Count == 0)
            {
                return NotFound();
            }
            foreach (var course in courses)
            {
                if(course.Id == courseId)
                {
                    NpgsqlRange<DateOnly>? range=course.EnrolmentDateRange;
                    DateOnly start = range.Value.LowerBound ;
                    DateOnly end= range.Value.UpperBound ;
                    DateOnly currentDate = DateOnly.FromDateTime(DateTime.UtcNow.Date);
                    if(start<=currentDate && end >= currentDate)
                    {

                        ClassEnrollment newclass = new ClassEnrollment();

                        newclass.Id = 1;
                        newclass.ClassId = courseId;
                        newclass.StudentId= (from s in _context.Users where (s.KeycloakId == userid) select s.Id).FirstOrDefault();

                        if (newclass == null)
                            throw new InvalidOperationException("INsert Data");
                        _context.Add(newclass);
                        _context.SaveChanges();
                        return newclass;

                    };
                }
            }
            throw new Exception("connection add Class bcause times up");
        }
    }
}
