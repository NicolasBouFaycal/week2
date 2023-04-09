using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        [HttpPost(template: "SessionTime")]
        public async Task<ActionResult<SessionTime>> InserSessionTime([FromQuery] DateTime StartTimeYYYY_MM_DD, [FromQuery] DateTime EndTimeYYYY_MM_DD, [FromQuery] int Duration)
        {
            var result = await _mediator.Send(new InserSessionTimeCommand(this, StartTimeYYYY_MM_DD, EndTimeYYYY_MM_DD, Duration));
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
            var result = await _mediator.Send(new AllTeacherPerCourse(this));
            return result;

        }
        [HttpGet("AllSessionTime")]
        public async Task<ActionResult<List<SessionTime>>> AllSessionTime()
        {
            var result = await _mediator.Send(new AllSessionTime(this));
            return result;

        }

        [HttpPost(template: "AssignTeacherToCourse")]
        public async Task<ActionResult<TeacherPerCourse>> AssignTeacherToCourse([FromQuery] int courseId)
        {
            var result = await _mediator.Send(new AssignTeacherToCourseCommand(this, courseId));
            return result;
        }
        [HttpPost(template: "AssignTeacher/CourseToSessionTime")]
        public async Task<ActionResult<TeacherPerCoursePerSessionTime>> AssignTeacherPerCoursePerSessionTime([FromQuery] int teacherPerCourseId, [FromQuery] int sessionTimeId)
        {
            var result = await _mediator.Send(new AssignTeacherPerCoursePerSessionTimeCommand(this, teacherPerCourseId, sessionTimeId));
            return result;
        }
        [HttpPost(template: "UploadImage")]
        public ActionResult<string> UploadImage([FromForm] UploadImg obj)
        {
            return _uploadImgHelper.UploadProfileAsync(obj);
        }
    }
}
