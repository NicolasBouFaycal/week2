using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NpgsqlTypes;
using ProjectName.Domain;
using ProjectName.Infrastructure.EmailServiceAbstraction;
using ProjectName.Persistence;

namespace Project.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Student")]
    public class StudentController : ControllerBase
    {
        public readonly MyDbContext _context;
        private readonly IEmailService _emailService;

        public StudentController(MyDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
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
        [HttpPost(template: "StudentEnrollToCourse")]
        public async Task<ActionResult<ClassEnrollment>> StudentEnrollToCourse([FromQuery] int courseId)
        {
            var courses = (from c in _context.Courses select c).ToList();
            var userid = Request.Cookies["UserId"];
            var getUserEmail = (from u in _context.Users where u.KeycloakId == userid select u.Email).FirstOrDefault();
            if (courses.Count == 0)
            {
                return NotFound();
            }
            foreach (var course in courses)
            {
                if (course.Id == courseId)
                {
                    NpgsqlRange<DateOnly>? range = course.EnrolmentDateRange;
                    DateOnly start = range.Value.LowerBound;
                    DateOnly end = range.Value.UpperBound;
                    DateOnly currentDate = DateOnly.FromDateTime(DateTime.UtcNow.Date);
                    if (start <= currentDate && end >= currentDate)
                    {
                        ClassEnrollment newclass = new ClassEnrollment();
                        newclass.Id = 2;
                        newclass.ClassId = courseId;
                        newclass.StudentId = (from s in _context.Users where (s.KeycloakId == userid) select s.Id).FirstOrDefault();
                        if (newclass == null)
                            throw new InvalidOperationException("INsert Data");
                        _context.Add(newclass);
                        _context.SaveChanges();
                        var className = (from c in _context.Courses where c.Id == courseId select c.Name).FirstOrDefault();
                        _emailService.SendEmail(getUserEmail, getUserEmail, "Enrollment to class ", "You have been Suseesfully enrolled in the class :" + className);

                        return newclass;
                    };
                }
            }
            throw new Exception("connection add Class bcause times up");
        }
    }
}
