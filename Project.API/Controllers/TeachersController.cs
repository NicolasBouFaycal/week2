using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using UMS.Application.Abstraction;
using UMS.Application.Commands;
using UMS.Application.Queries;
using UMS.Common;
using UMS.Common.Abstraction;
using UMS.Domain;
using UMS.Persistence;
namespace UMS.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Teacher")]
    public class TeachersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public static IWebHostEnvironment _environment;
        public readonly MyDbContext _context;
        private readonly IUploadImgHelper _uploadImgHelper ;

        public TeachersController(IUploadImgHelper uploadImgHelper, ITeachersHelper teachersHelper, IMediator mediator, IWebHostEnvironment environment, MyDbContext context)
        {
            _mediator = mediator;
            _environment = environment;
            _context = context;
            _uploadImgHelper = uploadImgHelper;
        }
        [HttpPost("SessionTime")]
        public async Task<ActionResult<SessionTime>> SessionTime([FromBody] SessionTime sessionTime)
        {
            var result = await _mediator.Send(new SessionTimeCommand(sessionTime.StartTime, sessionTime.EndTime , sessionTime.Duration));
            return result;
        }
        [HttpGet("AllCourses")]
        public async Task<ActionResult<List<Course>>> AllCourses()
        {
            var result = await _mediator.Send(new AllCourses());
            return result;

        }
        [HttpGet("AllTeacherPerCourse")]
        public async Task<ActionResult<List<TeacherPerCourse>>> AllTeacherPerCourse()
        {
            var result = await _mediator.Send(new AllTeacherPerCourse());
            return result;

        }
        [HttpGet("AllSessionTime")]
        public async Task<ActionResult<List<SessionTime>>> AllSessionTime()
        {
            var result = await _mediator.Send(new AllSessionTime());
            return result;

        }

        [HttpPost("TeacherToCourse")]
        public async Task<ActionResult<TeacherPerCourse>> TeacherToCourse([FromBody] int courseId)
        {
            var result = await _mediator.Send(new TeacherToCourseCommand(courseId));
            return result;
        }
        [HttpPost(template: "Teacher/CourseToSessionTime")]
        public async Task<ActionResult<TeacherPerCoursePerSessionTime>> TeacherPerCoursePerSessionTime([FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] TeacherPerCoursePerSessionTime teacherPerCoursePerSessionTime)
        {
            var result = await _mediator.Send(new TeacherPerCoursePerSessionTimeCommand(Convert.ToInt32(teacherPerCoursePerSessionTime.TeacherPerCourseId), Convert.ToInt32(teacherPerCoursePerSessionTime.SessionTimeId)));
            return result;
        }
        [HttpPost(template: "UploadImage")]
        public ActionResult<string> UploadImage([FromForm] UploadImg obj)
        {
            return _uploadImgHelper.UploadProfileAsync(obj);
        }
    }
}
